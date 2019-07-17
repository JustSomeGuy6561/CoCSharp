using CoC.Backend.Areas;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.Engine.Events
{
	//Reactions are a helper, i guess - they're like one-time events, but you don't need to bother with adding them, waiting for them to proc, then removing them - it's automatically handled.
	//2 Types of Reactions: Time-Based, and Area-Based. A Time Reaction acts like a one-off Active Time Event. An Area Reaction (Place or Location) acts as a one-time Triggered Encounter. Both
	//can be set to trigger as soon as possible, or after a delay. In the case of a Time Reaction, you also have the option of fiddling with stats and such - normally, you're not supposed to do that
	//in a Special Event - it's just supposed to be text with a menu or something based on whatever happens - get an item, etc. Time Reactions are also given the highest priority, so if other Time Events 
	//rely on stats being changed, they will see these changes.

	//Note that it is possible for something to trigger a Reaction multiple times before the reaction actually occurs - for example, a poorly coded segment could change the player's gender or body part type
	//multiple times before signalling that time passed, which could lead things that create a reaction on those changes to proc twice. it's therefore recommended to store a reference to any reaction, then
	//check if it's already in the reaction store before attempting to add it again. if it is already there, simply update it to use the new information. It may even be possible to get away with simply
	//checking to see if it's already in the reaction store, and adding it if it isn't if you capture the values from the source creature, though that requires knowledge of capturing values. 

	//a reaction that occurs after a specific amount of time.
	public sealed class TimeReaction : IComparable<TimeReaction>
	{
		public readonly GameDateTime procTime;
		public Action onProc { get; private set; }
		public EventWrapper eventWrapper { get; private set; }

		//fires immediately.
		public TimeReaction(Action procAction, EventWrapper wrapper)
		{
			onProc = procAction;
			eventWrapper = wrapper;
			procTime = GameDateTime.Now;
		}

		//fires after a delay. if randomized is set to true, it'll occur at some point between now and the delay. Note that you should make this delay relatively small. 
		public TimeReaction(Action procAction, EventWrapper wrapper, byte delay, bool randomized = false)
		{
			onProc = procAction;
			eventWrapper = wrapper;
			procTime = GameDateTime.HoursFromNow(randomized ? (byte)Utils.Rand(delay) : delay);
		}

		public int CompareTo(TimeReaction other)
		{
			return procTime.CompareTo(other.procTime);
		}

		public void UpdateReaction(Action newActionOnProc, EventWrapper newWrapper)
		{
			onProc = newActionOnProc;
			eventWrapper = newWrapper;
		}
	}

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
		public Action onTrigger { get; private set; }

		private LocationReaction(Action reaction, Type locationType, byte delay = 0)
		{
			targetLocation = locationType;
			onTrigger = reaction;
			timesToVisitUntilProccing = delay;
		}

		public static LocationReaction CreateLocationReaction<T>(Action reaction) where T : LocationBase
		{
			if (typeof(T) == typeof(LocationBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new LocationReaction(reaction, typeof(T));
		}

		public static LocationReaction CreateLocationReaction<T>(Action reaction, byte delay) where T : LocationBase
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
		public Action onTrigger { get; private set; }

		private PlaceReaction(Action reaction, Type place, byte delay = 0)
		{
			targetPlace = place;
			onTrigger = reaction;
			timesToVisitUntilProccing = delay;
		}

		//yes, i hid the constructor. C# hates generics with a passion with regards to derived. Class<X> != Class<Y> where Y derives X, and cannot be coerced to do so any way i could find.
		//this is the only way i could find around it that's still useful - forcing an Average Joe to use the Type class is a recipe for disaster. using <T> is not. 
		public static PlaceReaction CreatePlaceReaction<T>(Action reaction) where T : PlaceBase
		{
			if (typeof(T) == typeof(PlaceBase))
			{
				throw new ArgumentException("Cannot create a reaction to an abstract class");
			}
			return new PlaceReaction(reaction, typeof(T));
		}

		public static PlaceReaction CreatePlaceReaction<T>(Action reaction, byte delay) where T : PlaceBase
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


}
