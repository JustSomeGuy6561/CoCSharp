using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.Engine.Time
{
	//Time Listeners: 3 Types - Lazy, Active, and Daily. These listeners can either store a short text, or an entire scene. 
	//Basically, the game pretends that the PC is the center of the universe - time only flows when the PC does something, and we don't care about anything unless it interacts with the PC in some way.
	//As such, there are two major types of Time Listeners - Active and Lazy (and Daily, which is a helper). If an event is important enough that it interrupts what the player is doing, or the player
	//has to know NOW that something happened, it's active. if it occurs regarless of what the player is doing, or it can wait until the next time the player is free, it's lazy.
	//Daily is a helper. It's given highest priority, but in exchange, it only runs once per day, at a specified hour. It'd be possible to implement it in either Lazy or Active, but it's more convenient this way.
	//Note that all of these return an "Event Wrapper". EventWrapper essentially combines the short and large timeChange previously used - if you need to build a scene, use the MajorEvent constructor. if you just 
	//want simple text, use the string constructor. if you have no output, just use EventWrapper.Empty (or null).

	//Usage: TL;DR: Use lazy whenever possible, Active if it's critically important it occurs ASAP. Use Daily when that's what you need. 

	//Implementation (pseudo-code, because callback driven source is much more complicated. God, what i'd do for an await.

	//TimePassed(X:byte hours)
	//create a queue for events that require their own page.
	//loop x times:
	//	increment hour by 1. if hour is 24, reset to 0 and increment day by 1.
	//	for each daily listener that procs on the current hour
	//		run its reactToTrigger
	//		parse its result - if it's just text, append it. if it's a scene, add it to the scene queue.
	//  
	//	for each active listener
	//		run its reactToHourPassing
	//		parse its result - if it's just text, append it. if it's a scene, add it to the scene queue.
	//
	//	display the scenes in the scene queue, one at a time, popping each one off as its used.
	//end Loop;
	//
	//clear page if not empty
	//for each lazy listener
	//	run its reactToTimePassing
	//	parse its result - if it's just text, append it. if it's a scene, add it to the scene queue.
	//
	//display the scenes in the scene queue, one at a time, popping each one off as its used.
	//return

	//NOTE: actual implementation is more complex than this, but this is the basics. 

	//Quick Aside: Order of events is important. If you skip time, THEN go to location, use the GoToLocationAfterXHours. If you go to location, then skip, use GoToLocationThenUseXHours. Some events may only proc
	//if you're at a location. also allows special events (like NPC pregnancies) to cause different actions based on your current location. If you're passed out in a ditch after losing to an Imp (git gud)
	//Your follower(s) may come find you, wake you, and drag your ass to camp if you _need_ to be there for a birthing or whatever. By default, you won't gain back any time this way, you'll simply move to a new
	//location and wait out the rest of the time there. I may (and probably will) provide a means to cancel remaining lost time, which will cause the game to run the remaining active and dailys for this hour,
	//then immediately do the lazy ones, with the current time passed. 

	//return value is if it has any output. We will check if output is null or empty before doing anything. Scenes or text are mutually exclusive; only one will be used. 

	public interface ITimeLazyListener
	{
		EventWrapper reactToTimePassing(byte hoursPassed);
	}

	public interface ITimeActiveListener
	{
		EventWrapper reactToHourPassing();
	}

	public interface ITimeDailyListener
	{
		byte hourToTrigger { get; }

		EventWrapper reactToDailyTrigger();
	}

	public interface ITimeDayMultiListener
	{
		byte[] triggerHours { get; }
		EventWrapper reactToTrigger(byte currHour);
	}

	internal static class TimeListenerHelpers
	{
		public static SingleDayWrapper[] ToSingleDayCollection(this ITimeDayMultiListener listener)
		{
			return Array.ConvertAll(listener.triggerHours, (x) => new SingleDayWrapper(x, listener));
		}

		public static SingleDayWrapper ToSingleDay(this ITimeDayMultiListener listener, byte hour)
		{
			return listener.triggerHours.Contains(hour) ? new SingleDayWrapper(hour, listener) : null;
		}
	}

	internal sealed class SingleDayWrapper : ITimeDailyListener
	{
		private readonly Func<EventWrapper> callback;

		public byte hourToTrigger { get; }

		public EventWrapper reactToDailyTrigger() => callback();

		public SingleDayWrapper(byte hour, ITimeDayMultiListener listener)
		{
			hourToTrigger = hour;
			callback = () => listener.reactToTrigger(hour);
		}
	}
}
