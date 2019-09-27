//TimeEngine.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using CoC.Backend.Areas;
using CoC.Backend.Engine.Events;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.Engine.Time
{
#warning CONSIDER REWORKING ALL TIME AND AREA ENGINE BULLSHIT FAKE EVENTS TO USE WeakEvent. 

	public sealed class TimeEngine
	{
		//TimeEngine is a complex monstrosity, but it's logical and consistent, so i guess that's most important.
		//Time engine runs when time is used, or time is idled. Some items need to be checked every hour, while others need to be kept up to date,
		//and otherwise don't care how many hours passed. Still hours need to run once (or multiple times) per day, at certain times. we allow all of these,
		//and no hard-coding is necessary. 

		//Additionally, time engine handles location changes. This way, we can ensure the current location is always correct at the current time, and in the event 
		//theres a weird event that changes the player's location, the behavior is defined and not likely to break everything. 


		//Time flow is broken up into two different types: Idle and Use
		//Use time represents time that was used up during a scene and it cannot be canceled. It's worth noting time passes using video game logic - 
		//no time passes during scenes, but immediately upon completion the time used in that scene passes instantly. Thus, if a scene takes an hour, we need to ensure an hour passes,
		//and nothing prevents us from doing so. Thus, "used up" time.
		//Idle time represents time passing without the player doing anything. This may be caused manually, like sleeping or resting, or be out of the player's control, like when they
		//are passed out after losing a battle or drinking too much. Idle time can be interrupted, or even cancelled, if the situation demands it. 
		//This allows content to adapt and change behavior in unique ways, without requiring crazy levels of hacks. For example, night-time dream scenes where the player is awoken and 
		//forced to go somewhere or fight things can interrupt the player's sleep and do whatever they need to. 

		//additionally, you can set aside used hours for convenience, so you don't have to keep track of multiple scenes' time usage, as that could get tedious. Setting aside used time
		//will not cause the time engine to run. 

		//Time can also be stacked, in the rare event a special time-related interrupt causes the player to spend more time doing things. This behavior will replace any idle hours available
		//with used hours. Idling can be stacked as well, but that seems incredibly unlikely/useless, though YMMV.

		//Time engine works as such: once given time to idle or use, the engine will loop, hour by hour, until the all the idled and used time is gone. 
		//Each hour gives top priority to special, one-off events, then moves on to the daily listeners that proc for the current hour, then finally to the listeners active every hour.
		//At the end of an hour, the game will check if it needs to change locations. If it needs to do so, it will check and run all the lazy members, then change location.
		//this process will repeat until there is no more hours to consume.
		//Note that during the last hour, the lazies will be run regardless of whether or not the area changed. 


		private readonly Action<string> OutputText;
		private Action<Action> DoNext => MenuHelpers.DoNext;
		private readonly AreaEngine areaEngine;

		//the actual important info for the time engine - the day and hour. 
		internal byte CurrentHour { get; private set; }
		internal int CurrentDay { get; private set; }

		//what i would do for a linked hashset in C#. Update: Nevermind, That's what friends (and beer, apparently) are for. -JSG

		//lazies will only update when they need to. This generally means they will only update once, at the end of the given timeframe, before the final destination is locked in (if applicable)
		//however, if there are multiple location changes in one timeframe, they will update just before each location change. 
		internal readonly OrderedHashSet<ITimeLazyListener> lazyListeners = new OrderedHashSet<ITimeLazyListener>();
		//these run every hour.
		internal readonly OrderedHashSet<ITimeActiveListener> activeListeners = new OrderedHashSet<ITimeActiveListener>();
		//these run once a day, at a given time.
		internal readonly OrderedHashSet<ITimeDailyListener> dailyListeners = new OrderedHashSet<ITimeDailyListener>();
		//these run multiple times a day, at a collection of given times.
		internal readonly OrderedHashSet<ITimeDayMultiListener> dayMultiListeners = new OrderedHashSet<ITimeDayMultiListener>();

		//i wrote the priority queue though - that wasn't too hard. though it also isn't even remotely bulletproof. 

		//this is a list of special reactions that only run once. They are then removed from the queue.
		//it is possible to add special reactions at any time, usually as a result of certain conditions being met. 
		internal readonly PriorityQueue<TimeReaction> reactions = new PriorityQueue<TimeReaction>();


		//Event variables - these are used to process the current hour. 
		private Queue<ITimeActiveListener> activeQueue = new Queue<ITimeActiveListener>();
		private Queue<ITimeLazyListener> lazyQueue = new Queue<ITimeLazyListener>();
		private Queue<ITimeDailyListener> dailyQueue = new Queue<ITimeDailyListener>();

		//list of special events we found while processing the current hour - these are given their own page, so we need to store them.
		private Queue<SpecialEvent> specialEvents = new Queue<SpecialEvent>();

		private bool running; //is the time engine currently processing any time related stuff?
		private GameDateTime lastTimeLaziesRan; //the last time we updated the lazy items with their lazy data. 
		private bool hasAnyOutput; //helper used to determine if we need to show a special page for all this data. 

		//used to help display the events. Any events that don't require their own page will have their contents written here. 
		private readonly StringBuilder outputMagic = new StringBuilder();

		//it's possible for some idle events to want to print some sort of text or handle something when resuming from an interrupt.
		//this allows that.
		private ResumeTimeCallback resumeActionsCallback = null;


		private readonly LinkedList<UseHoursLocationHelper> locationChanges = new LinkedList<UseHoursLocationHelper>();
		//the final location to go to after an idle, if applicable. if the final hour has a different location set, this will be ignored. 
		private UseHoursLocationHelper finalDestination; //No items, fox only.

		internal TimeEngine(Action<string> output, AreaEngine areaEngineReference)
		{
			OutputText = output ?? throw new ArgumentNullException(nameof(output));
			areaEngine = areaEngineReference ?? throw new ArgumentNullException(nameof(areaEngineReference));
		}

		internal byte useHours { get; private set; }
		internal byte idleHours { get; private set; }
		internal byte totalHours => useHours.add(idleHours);
		//is the current hour we're running an idle hour?
		private bool isIdleHour => useHours == 0;


		internal void InitializeTime(int currDay, byte currHour)
		{
			CurrentDay = currDay;
			CurrentHour = currHour;
		}

		internal void IdleHours(byte hours, ResumeTimeCallback resumeCallback)
		{
			addHours(hours, true);
			resumeActionsCallback = resumeCallback;
			finalDestination = new UseHoursLocationHelper(areaEngine.currentArea.GetType(), () => { }, GameDateTime.Now.delta(idleHours), false);
			RunEngine();
		}

		internal void IdleHoursGoToArea<T>(byte hours, ResumeTimeCallback resumeCallback) where T : AreaBase
		{
			addHours(hours, true);
			if (locationChanges.Count == 0)
			{
				finalDestination = new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), GameDateTime.Now.delta(idleHours), false);
			}
			resumeActionsCallback = resumeCallback;
			RunEngine();
		}

		internal void IdleHoursGoToBase(byte hours, ResumeTimeCallback resumeCallback)
		{
			addHours(hours, true);
			if (locationChanges.Count == 0)
			{
				finalDestination = new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), GameDateTime.Now.delta(idleHours), false);
			}
			resumeActionsCallback = resumeCallback;
			RunEngine();
		}

		internal void CancelIdle()
		{
			var delta = idleHours;
			idleHours = 0;

			resumeActionsCallback = null;
		}

		internal void UseHours(byte hours)
		{
			addHours(hours, false);
			RunEngine();
		}

		internal void SetAsideHours(byte hours)
		{
			addHours(hours, false);
		}

		internal void UseHoursGoToArea<T>(byte hours, bool mustGoToLocationAfterTimeUp) where T : AreaBase
		{
			addHours(hours, false);

			GameDateTime time = GameDateTime.HoursFromNow(hours + useHours);
			locationChanges.AddLast(new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), time, mustGoToLocationAfterTimeUp));
			RunEngine();
		}

		internal void ChangeLocation<T>(bool mustGoToLocation) where T : AreaBase
		{
			if (locationChanges.Count > 0)
			{
				GameDateTime time = locationChanges.Last.Value.runTime;
				if (locationChanges.Last.Value.immutable)
				{
					time = time.delta(1);
					locationChanges.AddLast(new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), time, mustGoToLocation));
				}
				else
				{
					locationChanges.Last.Value = new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), time, mustGoToLocation);
				}
				RunEngine();
			}
			else if (running)
			{
				locationChanges.AddLast(new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), GameDateTime.Now, mustGoToLocation));
				RunEngine();
			}
			else
			{
				areaEngine.SetArea<T>();
				areaEngine.RunArea();
			}
		}

		internal void UseHoursGoToBase(byte hours, bool mustGoToLocationAfterTimeUp)
		{
			addHours(hours, false);
			GameDateTime time = GameDateTime.HoursFromNow(hours + useHours);
			locationChanges.AddLast(new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), time, mustGoToLocationAfterTimeUp));
			RunEngine();
		}

		internal void GoToBase(bool mustGoToLocation)
		{
			if (locationChanges.Count > 0)
			{
				GameDateTime time = locationChanges.Last.Value.runTime;
				if (locationChanges.Last.Value.immutable)
				{
					time = time.delta(1);
					locationChanges.AddLast(new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), time, mustGoToLocation));
				}
				else
				{
					locationChanges.Last.Value = new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), time, mustGoToLocation);
				}
				RunEngine();
			}
			else if (running)
			{
				locationChanges.AddLast(new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), GameDateTime.Now, mustGoToLocation));
				RunEngine();
			}
			else
			{
				areaEngine.ReturnToBase();
				areaEngine.RunArea();
			}
		}

		internal Type QueryFinalDestinationType()
		{
			if (finalDestination is null)
			{
				return areaEngine.currentArea.GetType();
			}
			else
			{
				return finalDestination.locationType;
			}
		}

		internal void UpdateIdleFinalDestination<T>() where T : AreaBase
		{
			finalDestination = new UseHoursLocationHelper(typeof(T), () => areaEngine.SetArea<T>(), GameDateTime.HoursFromNow(totalHours), false);
		}

		internal void UpdateIdleFinalDestinationHomeBase()
		{
			finalDestination = new UseHoursLocationHelper(areaEngine.currentHomeBase.GetType(), () => areaEngine.ReturnToBase(), GameDateTime.HoursFromNow(totalHours), false);
		}

		private void RunEngine()
		{
			if (!running && totalHours > 0)
			{
				hasAnyOutput = false;
				running = true;
				InitializeNewHour();
			}
		}

		//assumes we have hours to pass. always precede this with a totalHoursPassed > 0 check.
		private void InitializeNewHour()
		{
			incrementHour();

			if (specialEvents != null)
			{
				specialEvents.Clear();
			}
			else
			{
				specialEvents = new Queue<SpecialEvent>();
			}

			RunHour();
		}

		private void incrementHour()
		{
			if (useHours > 0)
			{
				useHours--;
			}
			else
			{
				idleHours--;
			}

			CurrentHour++;
			if (CurrentHour == 24)
			{
				CurrentHour = 0;
				CurrentDay++;
			}
		}

		private void RunHour()
		{
			outputMagic.Clear();

			//Linq magic. Basically, convert all dayMultis currently proccing into a single hour variant, which works with ITimeDailyListener.
			IEnumerable<ITimeDailyListener> converts = dayMultiListeners.Where((x) => x.triggerHours.Contains(CurrentHour)).Select((x) => x.ToSingleDay(CurrentHour));
			//then we union that together with the current OrderedHashSet
			//since both are ordered hashsets, they will preserve order from IEnumerable, though all entries with a single hour proc will go before the converted multi proc ones.
			dailyQueue = new Queue<ITimeDailyListener>(dailyListeners.Where((x) => x.hourToTrigger == CurrentHour).Union(converts));

			while (!reactions.isEmpty && reactions.Peek().procTime.CompareTo(GameDateTime.Now) <= 0)
			{
				EventWrapper wrapper = reactions.Pop().eventWrapper;
				if (!wrapper.isScene)
				{
					outputMagic.Append(wrapper.text);
				}
				else
				{
					specialEvents.Enqueue(wrapper.scene);
				}
			}

			while (dailyQueue.Count > 0)
			{
				ITimeDailyListener item = dailyQueue.Dequeue();
				EventWrapper wrapper = item.reactToDailyTrigger();
				if (!wrapper.isScene)
				{
					outputMagic.Append(wrapper.text);
				}
				else
				{
					specialEvents.Enqueue(wrapper.scene);
				}
			}

			activeQueue = new Queue<ITimeActiveListener>(activeListeners);

			while (activeQueue.Count > 0)
			{
				ITimeActiveListener item = activeQueue.Dequeue();
				EventWrapper wrapper = item.reactToHourPassing();
				if (!wrapper.isScene)
				{
					outputMagic.Append(wrapper.text);
				}
				else
				{
					specialEvents.Enqueue(wrapper.scene);
				}
			}
			if (outputMagic.Length != 0)
			{
				hasAnyOutput = true;
				OutputText(outputMagic.ToString());
				outputMagic.Clear();
			}
			//will we need a new page for the next hour/lazies if caught up?
			//NextEvent(true);
			NextEvent();
		}

		//Currently, the first special event will be on the same page as all the short, non-special stuff.
		//If i read the code correctly, that's what we currently do, but if that's not the desired result, we can fix it. 
		//To do so, uncomment the first iteration code in NextEvent, then toggle the NextEvent functions in CatchUpLazies and RunHour to their 
		//boolean variants. (comment out current, uncomment one with 'true')

		//if it helps, think of this as recursive - while it has any special events, it will tell them to call this when they are done. 
		//it's essentially a while loop, but broken up due to event-based user interaction. 
		private void NextEvent(/*bool firstIteration = false*/)
		{

			if (specialEvents.Count > 0)
			{
				//set up the next iteration
				Action next;

				//if we're the last special event and we have a special resume callback. 
				if (specialEvents.Count == 1 && resumeActionsCallback != null)
				{
					next = () => DoNext(() => { resumeActionsCallback(totalHours, areaEngine.currentArea); NextEvent(); });
				}
				else
				{
					next = () => DoNext(() => NextEvent());
				}
				hasAnyOutput = true;
				SpecialEvent item = specialEvents.Dequeue();

				//if (firstIteration)
				//{
				//	DoNext(() => item.BuildInitialScene(next));
				//}
				//else
				//{
				item.BuildInitialScene(isIdleHour, idleHours > 0, next);
				//}
			}
			else if (locationChanges.First.Value.runTime == GameDateTime.Now)
			{
				RunLazies();
				var firstItem = locationChanges.First.Value;
				locationChanges.RemoveFirst();
				firstItem.locationCallback();
				NextEvent();
			}
			else if (totalHours > 0)
			{
				InitializeNewHour();
			}
			else
			{
				if (lastTimeLaziesRan?.Equals(GameDateTime.Now) != true)
				{
					RunLazies();
					finalDestination?.locationCallback();
				}
				ReturnExecution();
			}
		}

		private void RunLazies()
		{
			outputMagic.Clear();

			lastTimeLaziesRan = GameDateTime.Now;

			lazyQueue = new Queue<ITimeLazyListener>(lazyListeners);

			int hoursPassed = lastTimeLaziesRan.hoursToNow();
			byte lazyHoursPassed = hoursPassed > byte.MaxValue ? byte.MaxValue : (byte)hoursPassed;

			while (lazyQueue.Count > 0)
			{
				ITimeLazyListener item = lazyQueue.Dequeue();
				outputMagic.Append(item.reactToTimePassing(lazyHoursPassed) ?? "");
			}

			if (outputMagic.Length != 0)
			{
				hasAnyOutput = true;
				OutputText(outputMagic.ToString());
				outputMagic.Clear();
			}
		}

		private void ReturnExecution()
		{
			running = false;
			lastTimeLaziesRan = null;

			if (hasAnyOutput)
			{
				DoNext(areaEngine.RunArea);
			}
			else
			{
				areaEngine.RunArea();
			}

			hasAnyOutput = false;
		}

		private void addHours(byte hours, bool isIdle)
		{
			if (hours == 0)
			{
				return;
			}
			else if (isIdle)
			{
				idleHours = idleHours.add(hours);
			}
			else
			{
				useHours = useHours.add(hours);
				idleHours = idleHours.subtract(hours);//does not allow the number to drop below 0. normally, we care about how much would underflow, but in this case it's irrelevant. 
			}
		}

		private class UseHoursLocationHelper
		{
			public readonly Type locationType;
			public readonly Action locationCallback;
			public readonly bool immutable;
			public readonly GameDateTime runTime;

			public UseHoursLocationHelper(Type locationType, Action locationCallback, GameDateTime timeToChangeLocation, bool cannotChange)
			{
				this.locationType = locationType ?? throw new ArgumentNullException(nameof(locationType));
				this.locationCallback = locationCallback ?? throw new ArgumentNullException(nameof(locationCallback));
				this.immutable = cannotChange;
			}
		}
	}
}
