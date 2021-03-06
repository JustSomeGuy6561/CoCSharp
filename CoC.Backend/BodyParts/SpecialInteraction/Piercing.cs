﻿//Piercing.cs
//Description:
//Author: JustSomeGuy
//3/27/2019, 11:41 AM
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEvent;

namespace CoC.Backend.BodyParts.SpecialInteraction
{

	public abstract class PiercingLocation
	{
		public delegate bool CompatibleWith(JewelryType jewelryType);

		protected readonly SimpleDescriptor buttonText;
		protected readonly SimpleDescriptor description;

		protected readonly CompatibleWith compatibleWith;


		public string Description() => description();

		public string Button() => buttonText();

		public bool AllowsJewelryOfType(JewelryType jewelryType) => compatibleWith(jewelryType);

		private protected PiercingLocation(CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
		{
			compatibleWith = allowsJewelryOfType ?? throw new ArgumentNullException(nameof(allowsJewelryOfType));
			buttonText = btnText ?? throw new ArgumentNullException(nameof(btnText));
			description = locationDesc ?? throw new ArgumentNullException(nameof(locationDesc));
		}
	}

	public abstract class Piercing<Location> where Location : PiercingLocation
	{
		public delegate bool PiercingUnlocked(Location location, out string whyNot);
		public delegate JewelryType JewelryTypeAllowed(Location locations);

		//tattoos and piercings use creature agnostic descriptions - that is, they will work for the player and all npcs, too.
		//make sure to use the correct conjugate nouns and such that are built in to the creatures.
		protected readonly CreatureStr allPiercingsShortDescription;
		protected readonly CreatureStr allPiercingsLongDescription;
		public string ShortCreatureDescription(Creature creature)
		{
			return allPiercingsShortDescription(creature);
		}

		public string VerboseCreatureDesription(Creature creature)
		{
			return allPiercingsLongDescription(creature);
		}

		protected readonly PiercingUnlocked piercingUnlocked;

		public readonly IBodyPart parent;

		//tbh, not the cleanest tool, but idgaf. it works. As far as anyone implementing this shit will know or care, it's basically identical to the standard event.
		private readonly WeakEventSource<PiercingDataChangedEventArgs<Location>> piercingChangeSource = new WeakEventSource<PiercingDataChangedEventArgs<Location>>();
		public event EventHandler<PiercingDataChangedEventArgs<Location>> OnPiercingChange
		{
			add { piercingChangeSource.Subscribe(value); }
			remove { piercingChangeSource.Unsubscribe(value); }
		}


		public bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;

		protected readonly Dictionary<Location, bool> piercedAt = new Dictionary<Location, bool>();
		protected readonly Dictionary<Location, PiercingJewelry> jewelryEquipped = new Dictionary<Location, PiercingJewelry>();

		internal Piercing(IBodyPart source, PiercingUnlocked LocationUnlocked, CreatureStr shortDesc, CreatureStr longDesc)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));

			piercingUnlocked = LocationUnlocked ?? throw new ArgumentNullException(nameof(LocationUnlocked));
			allPiercingsShortDescription = shortDesc ?? throw new ArgumentNullException(nameof(shortDesc));
			allPiercingsLongDescription = longDesc ?? throw new ArgumentNullException(nameof(longDesc));
		}

		public virtual ReadOnlyPiercing<Location> AsReadOnlyData()
		{
			return new ReadOnlyPiercing<Location>(piercedAt, jewelryEquipped);
		}

		public PiercingJewelry this[Location location]
		{
			get => jewelryEquipped[location];
		}

		public int piercingCount => piercedAt.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });
		public bool isPierced => piercedAt.Values.Any((x) => x);
		public bool isPiercedAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool isPierced);
			return isPierced;
		}

		public int jewelryCount => jewelryEquipped.Values.Aggregate(0, (x, y) => { if (y != null) x++; return x; });
		public bool wearingJewelry => jewelryEquipped.Values.Any((x) => x != null);
		public bool WearingJewelryAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			return jewelryEquipped.TryGetValue(location, out PiercingJewelry jewelry) && jewelry != null;
		}

		public bool EquipPiercingJewelry(Location piercingLocation, PiercingJewelry jewelry, bool forceEquip = false)
		{
			if (jewelry == null) throw new ArgumentNullException(nameof(jewelry));
			if (piercingLocation is null) throw new ArgumentNullException(nameof(piercingLocation));

			//if we can't pierce this location: fail
			else if (!CanPierce(piercingLocation))
			{
				return false;
			}
			//if we aren't already pierced at this location: fail
			else if (!isPiercedAt(piercingLocation))
			{
				return false;
			}
			//or we can't equip this type of jewelry at this location: fail.
			else if (!CanWearThisJewelry(piercingLocation, jewelry))
			{
				return false;
			}
			//if we are already wearing jewelry at that location and not force, fail
			else if (WearingJewelryAt(piercingLocation) && !forceEquip)
			{
				return false;
			}
			//if we are already wearing jewelry at that location and force
			else if (!WearingJewelryAt(piercingLocation))
			{
				//swap out the jewelry
				jewelryEquipped.Add(piercingLocation, jewelry);
				ProcChange(piercingLocation, jewelry);
				return jewelryEquipped.ContainsKey(piercingLocation); //should always return true, but i'd like to proc it on unit tests if we somehow broke Dictionaries.
			}
			//final case - we are pierced at this location and don't have any jewelry here and our jewelry type is valid for this location.
			else
			{
				//put in the jewelry.
				jewelryEquipped[piercingLocation] = jewelry;
				ProcChange(piercingLocation, jewelryEquipped[piercingLocation], jewelry);
				return jewelryEquipped[piercingLocation] == jewelry;
			}
		}

		public PiercingJewelry RemovePiercingJewelry(Location piercingLocation, bool forceRemove = false)
		{
			if (!jewelryEquipped.ContainsKey(piercingLocation))
			{
				return null;
			}
			else if (!jewelryEquipped[piercingLocation].removable && !forceRemove)
			{
				return null;
			}
			else
			{
				PiercingJewelry jewelry = jewelryEquipped[piercingLocation];
				jewelryEquipped.Remove(piercingLocation);
				ProcChange(piercingLocation, jewelry, null);
				return jewelry;
			}

		}

		public bool Pierce(Location piercingLocation)
		{
			if (!CanPierce(piercingLocation))
			{
				return false;
			}
			if (isPiercedAt(piercingLocation))
			{
				return false;
			}

			if (piercedAt.ContainsKey(piercingLocation))
			{
				piercedAt[piercingLocation] = true;
			}
			else
			{
				piercedAt.Add(piercingLocation, true);
			}


			ProcChange(piercingLocation, true, null);

			return piercedAt[piercingLocation];

		}

		public bool EquipOrPierceAndEquip(Location piercingLocation, PiercingJewelry jewelry, bool forceEquip = false)
		{
			bool isPierced = true;
			if (!isPiercedAt(piercingLocation))
			{
				isPierced = Pierce(piercingLocation);
			}
			return isPierced && EquipPiercingJewelry(piercingLocation, jewelry, forceEquip);
		}

		//does not take into account whether or not this is already pierced, simply say if this location can be pierced.
		public bool CanPierce(Location piercingLocation)
		{
			return piercingUnlocked(piercingLocation, out string _);
		}

		public bool CanPierceWithHint(Location piercingLocation, out string whyNot)
		{
			return piercingUnlocked(piercingLocation, out whyNot);
		}

		//does not check if it can currently be equipped, just if it is possible.
		public bool CanWearThisJewelry(Location piercingLocation, PiercingJewelry jewelry)
		{
			return piercingLocation.AllowsJewelryOfType(jewelry.jewelryType) && jewelry.CanEquipAt(this, piercingLocation);
		}

		public bool CanWearGenericJewelryOfType(Location piercingLocation, JewelryType jewelryType)
		{
			return piercingLocation.AllowsJewelryOfType(jewelryType);
		}

		internal bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			foreach (Location entry in Enum.GetValues(typeof(Location)).Cast<Location>())
			{

				//if not pierced at current location and there is jewelry there.
				if ((!piercedAt.TryGetValue(entry, out bool x) || x == false) && jewelryEquipped.TryGetValue(entry, out PiercingJewelry y) && y != null)
				{
					if (correctInvalidData)
					{
						jewelryEquipped.Remove(entry);
					}
					valid = false;
				}
				//if you can't pierce there.
				if (piercedAt.ContainsKey(entry) && !CanPierce(entry))
				{
					if (correctInvalidData)
					{
						piercedAt.Remove(entry);
						jewelryEquipped.Remove(entry);
					}
					valid = false;
				}
			}
			return valid;
		}

		internal void InitializePiercings(IEnumerable<KeyValuePair<Location, PiercingJewelry>> piercings)
		{
			if (piercings is null)
			{
				return;
			}

			foreach (var pair in piercings)
			{
				Location loc = pair.Key;
				piercedAt[loc] = true;

				var jewelry = pair.Value;

				if (CanWearThisJewelry(loc, jewelry))
				{
					jewelryEquipped[loc] = jewelry;
				}
			}
		}

		public void Reset()
		{
			ProcChange(piercedAt, jewelryEquipped);
			piercedAt.Clear();
			jewelryEquipped.Clear();
		}

		public abstract int MaxPiercings { get; }

		public abstract IEnumerable<Location> availableLocations { get; }
		//do hint for attempting to pierce. could mark it virtual for override opportunity.


		private void ProcChange(Dictionary<Location, bool> piercedAt, Dictionary<Location, PiercingJewelry> jewelryEquipped)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercedAt, jewelryEquipped));
		}

		//if theres some magic in the future that lets you un-pierce a location and reject any jewelry inside, this will handle it. but it's mostly for the other way - piercing something with new jewelry.
		private void ProcChange(Location piercingLocation, bool isNowPierced, PiercingJewelry deltaJewelry)
		{
			if (isNowPierced)
			{
				if (deltaJewelry != null)
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, isNowPierced, null, deltaJewelry, piercingCount, jewelryCount));
				}
				else
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, isNowPierced, piercingCount, jewelryCount));
				}
			}
			else
			{
				if (deltaJewelry != null)
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, isNowPierced, deltaJewelry, null, piercingCount, jewelryCount));
				}
				else
				{
					piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, isNowPierced, piercingCount, jewelryCount));
				}
			}
		}

		//oldJewelry assumed to be null.
		private void ProcChange(Location piercingLocation, PiercingJewelry newJewelry)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, null, newJewelry, piercingCount, jewelryCount));
		}

		private void ProcChange(Location piercingLocation, PiercingJewelry oldJewelry, PiercingJewelry newJewelry)
		{
			piercingChangeSource.Raise(this, new PiercingDataChangedEventArgs<Location>(parent, piercingLocation, oldJewelry, newJewelry, piercingCount, jewelryCount));
		}

		public virtual bool IsIdenticalTo(ReadOnlyPiercing<Location> original)
		{
			return piercedAt.Keys.Count == original.piercedAt.Keys.Count && piercedAt.Keys.All(k => original.piercedAt.ContainsKey(k) &&
			original.piercedAt[k] == piercedAt[k]) &&
			jewelryEquipped.Keys.Count == original.jewelryEquipped.Keys.Count && jewelryEquipped.Keys.All(k => original.jewelryEquipped.ContainsKey(k) &&
			Equals(original.jewelryEquipped[k], jewelryEquipped[k]));
		}
	}

	public class ReadOnlyPiercing<Location> where Location : PiercingLocation
	{
		internal readonly Dictionary<Location, bool> piercedAt = new Dictionary<Location, bool>();
		internal readonly Dictionary<Location, PiercingJewelry> jewelryEquipped = new Dictionary<Location, PiercingJewelry>();

		public readonly int piercingCount;
		public bool isPierced => piercingCount != 0;

		public bool IsPiercedAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool isPierced);
			return isPierced;
		}

		public readonly int jewelryCount;
		public bool wearingJewelry => jewelryCount > 0;
		public bool WearingJewelryAt(Location location)
		{
			if (location == null)
			{
				return false;
			}
			piercedAt.TryGetValue(location, out bool jewelryAt);
			return jewelryAt;
		}

		public PiercingJewelry this[Location location]
		{
			get => jewelryEquipped[location];
		}

		public ReadOnlyPiercing(Dictionary<Location, bool> piercedAt, Dictionary<Location, PiercingJewelry> jewelryEquipped)
		{
			this.piercedAt = new Dictionary<Location, bool>(piercedAt);
			this.jewelryEquipped = new Dictionary<Location, PiercingJewelry>(jewelryEquipped);

			piercingCount = piercedAt.Values.Aggregate(0, (x, y) => { if (y) x++; return x; });

			jewelryCount = jewelryEquipped.Values.Aggregate(0, (x, y) => { if (y != null) x++; return x; });
		}

		public ReadOnlyPiercing()
		{
			piercedAt = new Dictionary<Location, bool>();
			jewelryEquipped = new Dictionary<Location, PiercingJewelry>();

			piercingCount = 0;
			jewelryCount = 0;
		}
	}

	public delegate void PiercingDataChangedEventHandler<T>(object sender, PiercingDataChangedEventArgs<T> args) where T : PiercingLocation;

	//Generally, if you need the source that raised the event, you typecheck the sender object from the event. however, if you're dealing with a generic piercing and don't know
	//the type used for the piercing location, it becomes difficult to get the associated body part that the sender is associated with. therefore, it is stored here as well, so
	//you can obtain it without resorting to reflection. further, if you need to, you can get the creature associated with this body part (if applicable) by retrieving it
	//from the CreatureStore using the body part's creatureID.
	public class PiercingDataChangedEventArgs<T> : EventArgs where T: PiercingLocation
	{
		public readonly ReadOnlyDictionary<T, ValueDifference<bool>> piercingDiffs;
		public readonly ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>> jewelryDiffs;

		public readonly IBodyPart parent;

		public readonly int oldPiercingCount;
		public readonly int newPiercingCount;
		public readonly int oldJewelryCount;
		public readonly int newJewelryCount;

		internal PiercingDataChangedEventArgs(IBodyPart source, T location, bool piercingChange, int newPiercingCount, int newJewelryCount)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (location is null) throw new ArgumentNullException(nameof(location));

			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = piercingChange ? newPiercingCount - 1 : newPiercingCount + 1;
			this.newJewelryCount = newJewelryCount;
			oldJewelryCount = newJewelryCount;

			Dictionary<T, ValueDifference<bool>> pDiff = new Dictionary<T, ValueDifference<bool>>
			{
				{ location, new ValueDifference<bool>(!piercingChange, piercingChange) }
			};
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiff);
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(new Dictionary<T, ValueDifference<PiercingJewelry>>());
		}

		internal PiercingDataChangedEventArgs(IBodyPart source, T location, PiercingJewelry oldJewelry, PiercingJewelry newJewelry, int newPiercingCount, int newJewelryCount)
		{
			parent = source;
			if (location is null) throw new ArgumentNullException(nameof(location));

			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = newPiercingCount;
			this.newJewelryCount = newJewelryCount;
			if (oldJewelry == null != (newJewelry == null))
			{
				if (oldJewelry == null)
				{
					oldJewelryCount = newJewelryCount - 1;
				}
				else
				{
					oldJewelryCount = newJewelryCount + 1;
				}
			}
			else
			{
				oldJewelryCount = newJewelryCount;
			}

			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(new Dictionary<T, ValueDifference<bool>>());

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiff = new Dictionary<T, ValueDifference<PiercingJewelry>>
			{
				{ location, new ValueDifference<PiercingJewelry>(oldJewelry, newJewelry) }
			};
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiff);
		}

		internal PiercingDataChangedEventArgs(IBodyPart source, T location, bool piercingChange, PiercingJewelry oldJewelry, PiercingJewelry newJewelry, int newPiercingCount, int newJewelryCount)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (location is null) throw new ArgumentNullException(nameof(location));

			this.newPiercingCount = newPiercingCount;
			oldPiercingCount = piercingChange ? newPiercingCount - 1 : newPiercingCount + 1;

			Dictionary<T, ValueDifference<bool>> pDiff = new Dictionary<T, ValueDifference<bool>>
			{
				{ location, new ValueDifference<bool>(!piercingChange, piercingChange) }
			};
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiff);


			this.newJewelryCount = newJewelryCount;
			if (oldJewelry == null != (newJewelry == null))
			{
				if (oldJewelry == null)
				{
					oldJewelryCount = newJewelryCount - 1;
				}
				else
				{
					oldJewelryCount = newJewelryCount + 1;
				}
			}
			else
			{
				oldJewelryCount = newJewelryCount;
			}

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiff = new Dictionary<T, ValueDifference<PiercingJewelry>>
			{
				{ location, new ValueDifference<PiercingJewelry>(oldJewelry, newJewelry) }
			};
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiff);
		}

		internal PiercingDataChangedEventArgs(IBodyPart source, Dictionary<T, bool> unclearedPiercingDict, Dictionary<T, PiercingJewelry> unclearedJewelryDict)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			if (unclearedPiercingDict is null) throw new ArgumentNullException(nameof(unclearedPiercingDict));
			if (unclearedJewelryDict is null) throw new ArgumentNullException(nameof(unclearedJewelryDict));



			Dictionary<T, ValueDifference<bool>> pDiffs = unclearedPiercingDict.Where(x => x.Value == true).ToDictionary(x => x.Key, x => new ValueDifference<bool>(x.Value, false));
			piercingDiffs = new ReadOnlyDictionary<T, ValueDifference<bool>>(pDiffs);
			oldPiercingCount = unclearedPiercingDict.Count;
			newPiercingCount = 0;

			Dictionary<T, ValueDifference<PiercingJewelry>> jDiffs = unclearedJewelryDict.Where(x => x.Value != null).ToDictionary(x => x.Key, x => new ValueDifference<PiercingJewelry>(x.Value, null));
			jewelryDiffs = new ReadOnlyDictionary<T, ValueDifference<PiercingJewelry>>(jDiffs);
			oldJewelryCount = unclearedJewelryDict.Count;
			newJewelryCount = 0;
		}
	}
}
