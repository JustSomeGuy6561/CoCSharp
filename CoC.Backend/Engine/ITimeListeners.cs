using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine
{
	//Time Listeners: 3 Types - Lazy, Active, and Daily. each has 2 Variants: a short one, and a long one. A single class could (in theory) implement all 6 of these if it was really necessary.
	//Basically, the game pretends that the PC is the center of the universe - time only flows when the PC does something, and we don't care about anything unless it interacts with the PC in some way.
	//As such, there are two major types of Time Listeners - Active and Lazy (and Daily, which is a helper). If an event is important enough that it interrupts what the player is doing, or the player
	//has to know NOW that something happened, it's active. if it occurs regarless of what the player is doing, or it can wait until the next time the player is free, it's lazy.
	//Daily is a helper. It's given highest priority, but in exchange, it only runs once per day, at a specified hour. It'd be possible to implement it in either Lazy or Active, but it's more convenient this way.

	//Usage: TL;DR: Use lazy whenever possible, Active if it's critically important it occurs ASAP. Use Daily when that's what you need. 

	//Implementation (pseudo-code, for those who don't want to read source)
	//TimePassed(X:byte hours)
	//create a queue for events that require their own page.
	//loop x times:
	//	increment hour by 1. if hour is 24, reset to 0 and increment day by 1.
	//	for each daily listener that procs on the current hour
	//		run its reactToTrigger
	//		if it has output:
	//			if output is on its own page, add it to a queue of pages to display
	//			otherwise, append it to the current output.
	//  
	//	for each active listener
	//		run its reactToHourPassing
	//		if it has output:
	//			if output is on its own page, add it to a queue of pages to display
	//			otherwise, append it to the current output.
	//		run its reactToTrigger
	//
	//	remove all pages from the queue, one at a time, and display their output.
	//end Loop;
	//
	//clear page if not empty
	//for each lazy listener
	//	run its reactToTimePassing
	//	if it has output:
	//		if output is on its own page, add it to a queue of pages to display
	//		otherwise, append it to the current output.
	//return

	//NOTE: actual implementation is more complex than this, but this is the basics. 

	//Quick Aside: Order of events is important. If you skip time, THEN go to location, use the GoToLocationAfterXHours. If you go to location, then skip, use GoToLocationThenUseXHours. Some events may only proc
	//if you're at a location. also allows special events (like NPC pregnancies) to cause different actions based on your current location. If you're passed out in a ditch after losing to an Imp (git gud)
	//Your follower(s) may come find you, wake you, and drag your ass to camp if you _need_ to be there for a birthing or whatever. Note that this kind of thing won't interrupt your time lost due to losing;
	//you'll just spend a few hours recuperating at the new location instead. Also lets events handle edge cases (like you being stuck in Prison). 

	//return value is if it has output. note that we will still check if output is null or empty. 

	public interface ITimeLazyListener
	{
		bool reactToTimePassing(byte hoursPassed, out OutputWrapper output, out bool outputOnOwnPage);
	}

	public interface ITimeActiveListener
	{
		bool reactToHourPassing(out OutputWrapper output, out bool outputOnOwnPage);
	}

	public interface ITimeDailyListener
	{
		byte hourToTrigger { get; }

		bool reactToDailyTrigger(out OutputWrapper output, out bool outputOnOwnPage);
	}

	public interface ITimeDayMultiListener
	{
		byte[] triggerHours { get; }
		bool reactToTrigger(byte currHour, out OutputWrapper output, out bool outputOnOwnPage);
	}
}
