using CoC.Backend.Items.Wearables.Tattoos;
using System;
using System.Collections.Generic;
using System.Linq;

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

#warning TODO: implement tattoo change events.

	public class TattooablePart<TattooLocation> where TattooLocation : Enum
	{
		//short description, button. used for adding, removing tattoos.
		public delegate string LocationDescriptor(TattooLocation location);
		public delegate TattooSize TattooSizeLimit(TattooLocation location);

		protected readonly TattooSizeLimit locationSizeLimit;

		protected readonly Dictionary<TattooLocation, GenericTattooBase> tattoos;

		protected readonly LocationDescriptor locationButton;
		protected readonly LocationDescriptor locationDescription;

#warning Consider adding hint text delegates (defined below) for getting/replacing/removing tattoo.
		//would need more booleans to define what we're doing - are we trying to add, replace, or remove?
		//protected readonly LocationDescriptor locationHint;


		protected readonly PlayerStr allTattoosDescription;

		internal TattooablePart(PlayerStr tattooText, LocationDescriptor locationBtn, LocationDescriptor locationDesc, TattooSizeLimit sizeCompat)
		{
			allTattoosDescription = tattooText ?? throw new ArgumentNullException(nameof(tattooText));

			locationButton = locationBtn ?? throw new ArgumentNullException(nameof(locationBtn));
			locationDescription = locationDesc ?? throw new ArgumentNullException(nameof(locationDesc));

			locationSizeLimit = sizeCompat ?? throw new ArgumentNullException(nameof(sizeCompat));
		}

		//counts all tattoos where the value is not null.
		public int currentTattooCount => tattoos.Values.Aggregate(0, (x, y) => y != null ? ++x : x);

		public GenericTattooBase this[TattooLocation location]
		{
			get => tattoos[location];
		}

		public bool TattooedAt(TattooLocation location)
		{
			return tattoos.TryGetValue(location, out GenericTattooBase tattoo) && tattoo != null;
		}


		public bool CanCurrentlyGetTattooAt(TattooLocation location)
		{
			if (!LocationDefined(location))
			{
				return false;
			}

			if (TattooedAt(location)) return false;

			return true;
		}

		protected static bool LocationDefined(TattooLocation location)
		{
			return Enum.IsDefined(typeof(TattooLocation), location);
		}

		public bool CanGetTattooAt(TattooLocation location, GenericTattooBase tattoo, bool ignoreExistingTattoos = false)
		{
			return LocationDefined(location) && tattoo != null && (ignoreExistingTattoos || CanCurrentlyGetTattooAt(location)) && tattoo.tattooSize <= locationSizeLimit(location) && tattoo.CanTattooOn(this);
		}

		public bool GetTattoo(TattooLocation location, GenericTattooBase tattoo, bool force = false)
		{
			if (!CanGetTattooAt(location, tattoo, force)) return false;
			else
			{
				GenericTattooBase oldTattoo = null;
				if (tattoos.ContainsKey(location))
				{
					oldTattoo = tattoos[location];
				}
				tattoos[location] = tattoo;

				if (oldTattoo != null)
				{
					List<TattooLocation> toRemove = tattoos.Where(x => x.Value == oldTattoo).Select(x => x.Key).ToList();
					foreach (var item in toRemove)
					{
						tattoos.Remove(item);
					}

				}
				return true;
			}
		}

		public bool RemoveTattoo(TattooLocation location)
		{
			return tattoos.Remove(location);
		}


		internal bool Validate(bool correctInvalidData)
		{
			throw new NotImplementedException();
		}

		internal void Reset()
		{
			tattoos.Clear();
		}
	}
}
