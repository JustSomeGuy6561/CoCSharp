using CoC.Backend.Areas;
using CoC.Backend.Items;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using System;

namespace CoC.Backend.Reaction
{
	//these are designed for one-off Encounters. if you need something that consistently procs every x times or always procs if conditions are met, use
	//the encounter classes. this is designed for certain things occuring that cause a follow-up Encounter potentially somewhere else. 
	//Basically, when a Location is visited, the Area Engine checks to see if that location has any reactions that should proc now. if it does, it fires the first one off
	//instead of the normal roll scene for that location. when completed, the procced reaction is removed from the list for this location. A location reaction is a complete
	//scene - it must either return to camp or continue from that location. Note that this can be as simple as reading a sign that person X moved to a new location, then
	//continuing on from there, or something as complicated as an one-off, special enemy encounter due to the PC's body reaching a series of transformations and a certain corruption level. 
	//Note that these technically can be repeating - but rarely enough that it isn't worth adding it to the list of Encounters and having the area check all the time if it's available. 
	public sealed class LocationReaction : IComparable<LocationReaction>
	{
		public byte timesToVisitUntilProccing { get; private set; }

		internal readonly Type targetLocation;
		public Func<DisplayBase> onTrigger { get; private set; }

		private LocationReaction(Func<DisplayBase> reaction, Type locationType, byte delay = 0)
		{
			targetLocation = locationType;
			onTrigger = reaction;
			timesToVisitUntilProccing = delay;
		}

		public static LocationReaction CreateLocationReaction<T>(Func<DisplayBase> reaction) where T : LocationBase
		{
			if (typeof(T) == typeof(LocationBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new LocationReaction(reaction, typeof(T));
		}

		public static LocationReaction CreateLocationReaction<T>(Func<DisplayBase> reaction, byte delay) where T : LocationBase
		{
			if (typeof(T) == typeof(LocationBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new LocationReaction(reaction, typeof(T), delay);
		}

		public int CompareTo(LocationReaction other)
		{
			return timesToVisitUntilProccing.CompareTo(other.timesToVisitUntilProccing);
		}

		internal void VisitLocation()
		{
			timesToVisitUntilProccing--;
		}
	}

	//same as above, but with places instead of locations. Remember, places are much more flexible than Locations -
	//you can define any sub-area within a Place as another Place (so, the Wet Bitch can be a Place, and also part of Tel Adre)
	//which means you can also add special reaction encounters to sub-areas, too. 

	public sealed class PlaceReaction : IComparable<PlaceReaction>
	{
		public byte timesToVisitUntilProccing { get; private set; }

		internal readonly Type targetPlace;
		public Func<DisplayBase> onTrigger { get; private set; }

		private PlaceReaction(Func<DisplayBase> reaction, Type place, byte delay = 0)
		{
			targetPlace = place;
			onTrigger = reaction;
			timesToVisitUntilProccing = delay;
		}

		//yes, i hid the constructor. C# hates generics with a passion with regards to derived. Class<X> != Class<Y> where Y derives X, and cannot be coerced to do so any way i could find.
		//this is the only way i could find around it that's still useful - forcing an Average Joe to use the Type class is a recipe for disaster. using <T> is not. 
		public static PlaceReaction CreatePlaceReaction<T>(Func<DisplayBase> reaction) where T : PlaceBase
		{
			if (typeof(T) == typeof(PlaceBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new PlaceReaction(reaction, typeof(T));
		}

		public static PlaceReaction CreatePlaceReaction<T>(Func<DisplayBase> reaction, byte delay) where T : PlaceBase
		{
			if (typeof(T) == typeof(PlaceBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new PlaceReaction(reaction, typeof(T), delay);
		}

		public int CompareTo(PlaceReaction other)
		{
			return timesToVisitUntilProccing.CompareTo(other.timesToVisitUntilProccing);
		}

		internal void VisitLocation()
		{
			timesToVisitUntilProccing--;
		}
	}


	//Identical To Location reaction, but specifically for the current home base. 
	public sealed class HomeBaseReaction : IComparable<HomeBaseReaction>
	{
		public byte timesToVisitUntilProccing { get; private set; }

		public Func<DisplayBase> onTrigger { get; private set; }

		private HomeBaseReaction(Func<DisplayBase> reaction, byte delay = 0)
		{
			onTrigger = reaction;
			timesToVisitUntilProccing = delay;
		}

		public static HomeBaseReaction CreateHomeBaseReaction(Func<DisplayBase> reaction)
		{
			return new HomeBaseReaction(reaction);
		}

		public static HomeBaseReaction CreateHomeBaseReaction(Func<DisplayBase> reaction, byte delay)
		{
			return new HomeBaseReaction(reaction, delay);
		}

		public int CompareTo(HomeBaseReaction other)
		{
			return timesToVisitUntilProccing.CompareTo(other.timesToVisitUntilProccing);
		}

		internal void VisitLocation()
		{
			timesToVisitUntilProccing--;
		}
	}

}
