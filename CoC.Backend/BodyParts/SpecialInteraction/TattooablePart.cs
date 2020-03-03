using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Tattoos;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEvent;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//tattoos are a huge pain to implement.
	//I've broken it down into something i hope is flexible enough to work in a more or less generic way, but there's no guarentee that's the case.
	//each body part that supports tattoos can have multiple tattoos at once (because piercings work that way and i guess if i do it there, i have to do it here too. smh)
	//however, some tattoos may be mutually exclusive - it's simply not possible to have two tattoes in the same spot, after all. Tattoes are implemented in the frontend, so you can
	//create as many unique tattoos as your heart desires.

	//Tattoos are required to define their tattoo type, and any supported color(s), if applicable. we're simply using the tones class for the colors.
	//Tattoo type is an enum, for small, medium, large, or full. This lets us use generic
	//tattoos (like hearts, etc) in multiple locations, and limit exactly what type of tattoo is supported by size- you won't be able to fit a full back tattoo on your arm,
	//for example. Full tattoos are generally specific to one body part - i.e. a 'sleeve' tattoo is your entire arm, and wouldn't make sense anywhere else.

	//Tattoos are 'magic' - they can be removed or added without any real consequence, and may be added automatically if a transformation or whatever calls for it.
	//additionally, they'll work, regardless of skin type. For fur or feathers, they just magically discolor the fur/feathers above the skin to match the tattoo, i guess.
	//it's also possible to just say it's hidden beneath the fur, but that seems to defeat the point.

	//Tattooables are defined as a series of locations, represented by an Enum. these locations are spots a tattoo can go. they need not be specific.
	//a callback defines what locations are compatible with another. that is, if a given location already has a tattoo, can another given location also get a tattoo?
	//another callback determines what type of tattoo can go in this particular location.

	//If you attempt to add a tattoo that conflicts with another, it will fail to do so, unless you force it with a flag. If you force it, all conflicting tattoos will be
	//removed and the new one will be added. This doesn't make sense in reality, where i suppose you'd just repurpose the old tattoo or ink over it if possible, but again, 'magic'

	public abstract class TattooLocation
	{

		public delegate bool TattooSizeLimit(TattooSize tattooSize, bool scaleable);

		protected readonly SimpleDescriptor buttonText;
		protected readonly SimpleDescriptor description;

		public string Description() => description();

		public string Button() => buttonText();

		protected readonly TattooSizeLimit tattooLimit;

		public bool SupportsTattooSize(TattooSize tattooSize, bool scaleable) => tattooLimit(tattooSize, scaleable);

		private protected TattooLocation(TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
		{
			tattooLimit = limitSize ?? throw new ArgumentNullException(nameof(limitSize));

			buttonText = btnText ?? throw new ArgumentNullException(nameof(btnText));
			description = locationDesc ?? throw new ArgumentNullException(nameof(locationDesc));
		}

		//These are the common settings, but it may be possible to allow a type to only be large or medium, or a full to allow generic sizes (not recommended, but w/e).
		//so you can use what you want.
		protected static bool SmallTattoosOnly(TattooSize tattooSize, bool scaleable)
		{
			return tattooSize == TattooSize.SMALL;
		}

		protected static bool MediumTattoosOrSmaller(TattooSize tattooSize, bool scaleable)
		{
			return tattooSize <= TattooSize.MEDIUM;
		}

		protected static bool LargeTattoosOrSmaller(TattooSize tattooSize, bool scaleable)
		{
			return tattooSize <= TattooSize.LARGE;
		}

		protected static bool FullPartTattoo(TattooSize tattooSize, bool scaleable)
		{
			return tattooSize == TattooSize.FULL;
		}
	}

	public abstract class TattooablePart<Location> where Location : TattooLocation
	{
		public delegate bool OverlapChecker(Location first, Location second);

		protected readonly Dictionary<Location, TattooBase> tattoos;

		public readonly IBodyPart parent;

#warning Consider adding hint text delegates (defined below) for getting/replacing/removing tattoo.
		//would need more booleans to define what we're doing - are we trying to add, replace, or remove?
		//protected readonly LocationDescriptor locationHint;

		//tattoos and piercings use creature agnostic descriptions - that is, they will work for the player and all npcs, too.
		//be sure to use the conjugate properties built in to the creature.
		protected readonly CreatureStr allTattoosShortDescription;
		protected readonly CreatureStr allTattoosLongDescription;
		public string ShortCreatureDescription(Creature creature)
		{
			return allTattoosShortDescription(creature);
		}

		public string VerboseCreatureDesription(Creature creature)
		{
			return allTattoosLongDescription(creature);
		}

		private readonly WeakEventSource<TattooDataChangedEventArgs<Location>> tattooChangeSource = new WeakEventSource<TattooDataChangedEventArgs<Location>>();
		public event EventHandler<TattooDataChangedEventArgs<Location>> OnTattooChange
		{
			add { tattooChangeSource.Subscribe(value); }
			remove { tattooChangeSource.Unsubscribe(value); }
		}

		internal TattooablePart(IBodyPart source, CreatureStr allTattoosShort, CreatureStr allTattoosLong)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));

			allTattoosShortDescription = allTattoosShort ?? throw new ArgumentNullException(nameof(allTattoosShort));
			allTattoosLongDescription = allTattoosLong ?? throw new ArgumentNullException(nameof(allTattoosLong));
		}

		public virtual ReadOnlyTattooablePart<Location> AsReadOnlyData()
		{
			return new ReadOnlyTattooablePart<Location>(tattoos);
		}

		//counts all tattoos where the value is not null.
		public int currentTattooCount => tattoos.Values.Aggregate(0, (x, y) => y != null ? ++x : x);

		public TattooBase this[Location location]
		{
			get => tattoos.GetItemClean(location);
		}

		public bool TattooedAt(Location location)
		{
			return tattoos.TryGetValue(location, out TattooBase tattoo) && tattoo != null;
		}

		public Location[] currentTattoos => tattoos.Where(x => x.Value != null).Select(x => x.Key).ToArray();

		public bool HasNoConflictingTattoosWith(Location location)
		{
			//location invalid. fail.
			if (!availableLocations.Contains(location)) return false;

			//tattoo at this location. fail
			if (TattooedAt(location)) return false;

			//incompatible with existing tattoos. fail.
			if (!IncompatibleLocations(location).IsEmpty())
			{
				return false;
			}
			//ok.
			return true;
		}

		public bool CanGetTattooAt(Location location, TattooBase tattoo, bool ignoreExistingTattoos = false)
		{
			//tattoo is valid. location is valid. ignoring existing tattoos or no conflicting tattoos. location allows tattoo of this size. this particular tattoo has no
			//objection to the current tattooable part and location. if all of the previous statements are true, returns true. otherwise returns false.
			return availableLocations.Contains(location) && tattoo != null && (ignoreExistingTattoos || HasNoConflictingTattoosWith(location))
				&& location.SupportsTattooSize(tattoo.tattooSize, tattoo.scaleable) && tattoo.CanTattooOn(this);
		}

		public bool GetTattoo(Location location, TattooBase tattoo, bool force = false)
		{
			//first, see if we can get the tattoo. this checks all of the compatibility necessities. note that unless force is set, any existing tattoos either at the current location
			//or in a conflicting location will cause this to fail. If force is set, this will succeed unless tattoo is null or location is invalid. Note that this assumes you are
			//attempting to add or replace a tattoo, and thus setting tattoo to null will fail instead of potentially removing a tattoo. if you want to remove a tattoo,
			//use the RemoveTattoo function instead.
			if (!CanGetTattooAt(location, tattoo, force))
			{
				return false;
			}
			else if (!force)
			{
				ProcSingleChange(location, tattoo);
				tattoos[location] = tattoo;
				return true;
			}
			else
			{
				Dictionary<Location, ValueDifference<TattooBase>> diffs = new Dictionary<Location, ValueDifference<TattooBase>>();

				TattooBase oldTattoo = null;
				if (tattoos.ContainsKey(location))
				{
					oldTattoo = tattoos[location];
				}
				tattoos[location] = tattoo;

				diffs[location] = new ValueDifference<TattooBase>(oldTattoo, tattoo);

				foreach (var item in IncompatibleLocations(location))
				{
					diffs[item] = new ValueDifference<TattooBase>(tattoos[item], null);
					tattoos[item] = null;
				}

				ProcMultiChange(diffs);
				return true;
			}
		}

		public bool RemoveTattoo(Location location)
		{
			if (tattoos.TryGetValue(location, out var tattoo) && !(tattoo is null))
			{
				ProcSingleChange(location, null);
			}
			return tattoos.Remove(location);
		}

		public abstract bool LocationsCompatible(Location first, Location second);

		public IEnumerable<Location> IncompatibleLocations(Location source)
		{
			return tattoos.Where(x => x.Key != source && x.Value != null && !LocationsCompatible(x.Key, source)).Select(x => x.Key);
		}

		internal bool Validate(bool correctInvalidData)
		{
			throw new NotImplementedException();
		}

		internal void Reset()
		{
			ProcReset();
			tattoos.Clear();
		}

		public abstract int MaxTattoos { get; }

		public abstract IEnumerable<Location> availableLocations { get; }

		//assumes is called before the tattoos are cleared.
		private void ProcReset()
		{
			tattooChangeSource.Raise(this, new TattooDataChangedEventArgs<Location>(parent, tattoos));
		}

		//difficult to do, so we'll just leave this up to implementer.
		private void ProcMultiChange(IDictionary<Location, ValueDifference<TattooBase>> diffs)
		{
			tattooChangeSource.Raise(this, new TattooDataChangedEventArgs<Location>(parent, diffs));
		}

		//assumes is called before the tattoo is changed.
		private void ProcSingleChange(Location tattooLocation, TattooBase newTattoo)
		{
			TattooBase oldTattoo = tattoos.ContainsKey(tattooLocation) ? tattoos[tattooLocation] : null;
			int newCount;

			if ((oldTattoo is null) != (newTattoo is null))
			{
				if (newTattoo is null)
				{
					newCount = currentTattooCount - 1;
				}
				else
				{
					newCount = currentTattooCount + 1;
				}
			}
			else
			{
				newCount = currentTattooCount;
			}

			tattooChangeSource.Raise(this, new TattooDataChangedEventArgs<Location>(parent, tattooLocation, oldTattoo, newTattoo, newCount));
		}

		public virtual bool IsIdenticalTo(ReadOnlyTattooablePart<Location> original)
		{
			return tattoos.Keys.Count == original.tattoos.Keys.Count && tattoos.Keys.All(k => original.tattoos.ContainsKey(k) &&
			Equals(original.tattoos[k], tattoos[k]));
		}

	}

	public delegate void TattooDataChangedEventHandler<T>(object sender, TattooDataChangedEventArgs<T> args) where T : TattooLocation;

	//just like piercing, a tattoo change event now stores the body part associated with it, so you don't need to know exactly what T is to get the associated body part
	//(and by extension, the creature associated with that body part).
	public class TattooDataChangedEventArgs<T> : EventArgs where T : TattooLocation
	{
		public readonly ReadOnlyDictionary<T, ValueDifference<TattooBase>> tattooDiffs;

		public readonly int oldTattooCount;
		public readonly int newTattooCount;

		public readonly IBodyPart parent;

		//add, replace, or remove tattoo.
		internal TattooDataChangedEventArgs(IBodyPart source, T location, TattooBase oldTattoo, TattooBase newTattoo, int newTattooCount)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (location == null) throw new ArgumentNullException(nameof(location));

			this.newTattooCount = newTattooCount;

			if (oldTattoo == null != (newTattoo == null))
			{
				if (oldTattoo == null)
				{
					oldTattooCount = newTattooCount - 1;
				}
				else
				{
					oldTattooCount = newTattooCount + 1;
				}
			}
			else
			{
				oldTattooCount = newTattooCount;
			}


			Dictionary<T, ValueDifference<TattooBase>> tatDiff = new Dictionary<T, ValueDifference<TattooBase>>
			{
				{ location, new ValueDifference<TattooBase>(oldTattoo, newTattoo) }
			};
		}

		//add, replace, or remove multiple tattoos.
		internal TattooDataChangedEventArgs(IBodyPart source, IDictionary<T, ValueDifference<TattooBase>> diffs)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (diffs is null) throw new ArgumentNullException(nameof(diffs));


			oldTattooCount = diffs.Where(x => x.Value.oldValue != null).Count();
			newTattooCount = diffs.Where(x => x.Value.newValue != null).Count();

			tattooDiffs = new ReadOnlyDictionary<T, ValueDifference<TattooBase>>(diffs);
		}

		//clear tattoos.
		internal TattooDataChangedEventArgs(IBodyPart source, IDictionary<T, TattooBase> unclearedTattooDict)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (unclearedTattooDict is null) throw new ArgumentNullException(nameof(unclearedTattooDict));


			Dictionary<T, ValueDifference<TattooBase>> pDiffs = unclearedTattooDict.Where(x => x.Value != null).ToDictionary(x => x.Key, x => new ValueDifference<TattooBase>(x.Value, null));
			tattooDiffs = new ReadOnlyDictionary<T, ValueDifference<TattooBase>>(pDiffs);
			oldTattooCount = unclearedTattooDict.Where(x => x.Value != null).Count();
			newTattooCount = 0;
		}
	}

	public class ReadOnlyTattooablePart<Location> where Location : TattooLocation
	{

		protected internal readonly Dictionary<Location, TattooBase> tattoos;

		public ReadOnlyTattooablePart()
		{
			this.tattoos = new Dictionary<Location, TattooBase>();
		}

		internal ReadOnlyTattooablePart(Dictionary<Location, TattooBase> tattoos)
		{
			this.tattoos = new Dictionary<Location, TattooBase>(tattoos);
		}

		//counts all tattoos where the value is not null.
		public int currentTattooCount => tattoos.Values.Aggregate(0, (x, y) => y != null ? ++x : x);

		public TattooBase this[Location location]
		{
			get => tattoos.GetItemClean(location);
		}

		public bool TattooedAt(Location location)
		{
			return tattoos.TryGetValue(location, out TattooBase tattoo) && tattoo != null;
		}

		public Location[] currentTattoos => tattoos.Where(x => x.Value != null).Select(x => x.Key).ToArray();
	}
}
