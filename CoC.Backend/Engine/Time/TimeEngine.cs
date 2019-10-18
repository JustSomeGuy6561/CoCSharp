//TimeEngine.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using CoC.Backend.Areas;
using CoC.Backend.Reaction;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;

namespace CoC.Backend.Engine.Time
{
#warning CONSIDER REWORKING ALL TIME AND AREA ENGINE BULLSHIT FAKE EVENTS TO USE WeakEvent. 

	public sealed class TimeEngine
	{
		//Time engine is a monstrosity. That's basically it. 90% of the time you only use one hour and this is all irrelevant. But in the case it's not, here's how it works:

		//Time follows video game logic - We're embracing it, and not trying to overcomplicate things by making it actually make sense. Basically, we're lampshading it by saying your body 
		//and mind process time differently, and your body tends not to reflect any time changes until you have time to notice it or are actively thinking about it (like with TF items). 
		//Any special events that occur go first, in the order they are processed. if they cause more time to be used, it is used up. Then, you notice all other changes, more or less at once.

		//This explanation covers up for the fact that we break causality for the sake of convenience - you probably wouldn't be able to go off and fight goblins or whatever when about to give
		//birth, but we simply ignore this, and you give birth after fighting the goblins. 
		//We can pass time in two ways: used up time, and idled time. used up time is time that has already passed, and your body needs to catch up. this is basically everything in the game - 
		//any interactions with NPCs or the environment/areas/whatever counts as time used up. idled time is synchronous - any event occurs in "real-time". used up time cannot be canceled 
		//or interrupted, and as far as any event is concerned, the current time is after all those hours have passed. idle time can be interrupted; the current time is the same as the 
		//current hour being processed. Note that these can stack, but use time overwrites any idle time before it. so if you are idleing for 4 hours and then use up two hours fighting, 
		//the current time is 2 hours later, and we need to process 2 used up hours, and (4-2, or 2) idle hours. if you idle more in an idle, those are added, same with adding idle hours 
		//after the current use hours, or using additional hours on previous used hours. 99.9% of the time, this will never happen, but in case it does, the logic is here. 

		//as of this writing, we use idle time when the player is KOed, asleep, or resting. all other time is used up time. Do whichever makes the most sense in context. 

		//when any time is added (and not set aside, see below), the engine will immediately start running, and handle anything that "occured" during that time. It does this by running every
		//hour until there is no more time left to handle.

		//when an hour is run, it checks for any reactions that should occur for that hour, followed by any listeners for that hour, and then any listeners that happen every hour. each of these
		//can either return a full scene or simply some text. if it requires a full scene, it is placed in a queue. if just text, it is added to a final display page. the queue is then processed
		//and the resume callback always displays the next item in the queue. 

		//when all hours are ran, then any lazy listeners are checked, and their results are processed just like their daily and active counterparts. Additionally, once completed, it will add
		//the final display page to the queue if it has any output at all. regardless, when the queue is finished, the last resume callback will change the player's location to whatever was
		//initially provided, if applicable, or simply run the current area if not. 

		//note that if the final page has no content, it will not be displayed, meaning the player wont have to click through an empty page - it'll simply skip to the run area. 

		//it's also possible to set aside used up hours, without running the engine. In some cases, scenes may take variable amounts of time, and it may be convenient to simply let this handle
		//any time changes as needed, instead of storing how much should be added, and passing the result around everywhere. Setting aside time will not cause the engine to run and will return
		//execution immediately. however, since this does not start the time engine, make sure to do that later or the game will never load the target area as the time will never pass to do so. 

		//any event scene or reaction should function just like a normal scene, complete with calling the use hour/use hour change location, but with one key difference: it calls the
		//resume callback that it initially received when it was called from here. These use hours and change location will simply stack until all hours are used up.

		//finally, any calls that change location after a period of time can allow the location to be changed by any events. for example, it may make sense to allow the player to initially
		//start to head toward the base, but on the way remember they need to meet up with an NPC about to give birth. Of course, if a scene says that it "reaches <location>" in the text,
		//that really wouldn't make any sense, so you can also prevent that behavior. by default, any used up time will not allow its destination to be changed, and any idled time will. 
		//if an event tries to change the location, but the current destination cannot be changed, the game will simply delay that location change by one hour, and increment the used up time
		//to accommodate that. Essentially, it just stacks a use one hour change location function on top of the current data. for more on stacking, see below. 

		//these functions can be stacked - if some event that occurs that would cause more time to be used (for example, if player the player is pregnanct with urta's kid, certain pregnancy
		//progress checkpoints can result in sex, which uses up an hour.), the engine will handle this. One caveat worth knowing is there is no way to know if something will use more time
		//until it's running, so anything parsed before this will get one hour as the current time and the ones after will get another. This really shouldn't be an issue, and odds of it 
		//actually happening are pretty small.

		//Tl;Dr: 99% of the time, you will be using the use hour group of functions, and it will not have any strange side effects. It will simply update the time and change location, 
		//if necessary. in the rare case where something happens over this used up time, it will display the results before returning, with any scenes displaying first. 
		//in the rarer case where it adds more time, it will stack on without issue. in the ultra-rare case where an event tries to change a location, it will do so immediately if possible, 
		//or delay one hour and then do so if not. Events are processed in order, so if something before the current event uses additional time, it will be treated as if it ran at the new time
		//instead. You don't need to worry about this to use everything here. 

		//also, just 

		//dev notes:
		//implement accordingly. use up used hours first, not idle hours. 


		private readonly Func<DisplayBase> pageMaker;
		private readonly Action<DisplayBase> displayPage;
		//private readonly Action<string> OutputText;
		//private readonly Action ClearOutput;

		//this is a queue of all the active pages currently in the time engine. the call when done function for each page in the time queue is a function that checks this and 
		//does the next one as available. It's 10x simpler to read, understand, and write than any other more "correct" version. Believe me, i tried. 
		private readonly Queue<DisplayBase> activePages = new Queue<DisplayBase>();
		private DisplayBase currentContentPage;

		private bool changedLocationThisHourAlready;

		private IdleDestinationStorage finalDestination; //no items, fox only.

		private void QueryPageStatus()
		{
			//pop off any empty items. i highly doubt this will ever occur, but better safe than sorry.
			while (activePages.Count > 0 && activePages.Peek() is null)
			{
				activePages.Dequeue();
			}

			if (activePages.Count > 0)
			{
				DisplayBase nextPage = activePages.Dequeue();
				displayPage(nextPage);
			}
			else
			{
				CheckHour();
			}
		}

		private readonly AreaEngine areaEngine;

		//the actual important info for the time engine - the day and hour. 
		private byte currentHour;
		private int currentDay;

		internal GameDateTime currentTime => new GameDateTime(currentDay, currentHour).delta(totalHours);

		private GameDateTime engineTime => new GameDateTime(currentDay, currentHour);

		private byte useHours;
		internal byte idleHours;
		internal ushort totalHours => (ushort)(useHours + idleHours);

		#region Persistent Data
		//what i would do for a linked hashset in C#. Update: Nevermind, That's what friends (and beer, apparently) are for. -JSG

		//lazies will only update when they need to. This generally means they will only update once, at the end of the given timeframe, before the final destination is locked in (if applicable)
		//however, if there are multiple location changes in one timeframe, they will update just before each location change. 
		internal readonly OrderedHashSet<ITimeLazyListener> lazyListeners = new OrderedHashSet<ITimeLazyListener>();
		//these run every hour.
		internal readonly OrderedHashSet<ITimeActiveListenerSimple> simpleActiveListeners = new OrderedHashSet<ITimeActiveListenerSimple>();
		internal readonly OrderedHashSet<ITimeActiveListenerFull> fullActiveListeners = new OrderedHashSet<ITimeActiveListenerFull>();
		//these run once a day, at a given time.
		internal readonly OrderedHashSet<ITimeDailyListenerSimple> simpleDailyListeners = new OrderedHashSet<ITimeDailyListenerSimple>();
		internal readonly OrderedHashSet<ITimeDailyListenerFull> fullDailyListeners = new OrderedHashSet<ITimeDailyListenerFull>();
		//these run multiple times a day, at a collection of given times.
		internal readonly OrderedHashSet<ITimeDayMultiListenerSimple> simpleMultiTimeListeners = new OrderedHashSet<ITimeDayMultiListenerSimple>();
		internal readonly OrderedHashSet<ITimeDayMultiListenerFull> fullMultiTimeListeners = new OrderedHashSet<ITimeDayMultiListenerFull>();

		//i wrote the priority queue though - that wasn't too hard. though it also isn't even remotely bulletproof. 

		//this is a list of special reactions that only run once. They are then removed from the queue.
		//it is possible to add special reactions at any time, usually as a result of certain conditions being met. 
		internal readonly PriorityQueue<OneOffTimeReactionBase> reactions = new PriorityQueue<OneOffTimeReactionBase>();
		#endregion

		#region Data For Time Engine Current Run

		private GameDateTime startTime = null;
		private bool running => startTime != null; //is the time engine currently processing any time related stuff?

		private bool hitLazies = false;

		private string newHourHeader = null;
		#endregion

		//is the current hour we're running an idle hour?
		private bool isIdleTime => useHours == 0;
		private bool hasAnyIdleTime => idleHours > 0;

		internal TimeEngine(Func<DisplayBase> pageDataMaker, Action<DisplayBase> displayThePage, AreaEngine areaEngineReference)
		{
			pageMaker = pageDataMaker ?? throw new ArgumentNullException(nameof(pageDataMaker));
			displayPage = displayThePage ?? throw new ArgumentNullException(nameof(displayThePage));
			areaEngine = areaEngineReference ?? throw new ArgumentNullException(nameof(areaEngineReference));
		}

		internal void InitializeTime(int currDay, byte currHour)
		{
			currentDay = Utils.Clamp2(currDay, -5, int.MaxValue);
			currentHour = Utils.Clamp2<byte>(currHour, 0, 23);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hours"></param>
		/// <param name="resumeCallback"></param>
		/// <param name="mustEndUpHere"></param>
		internal void IdleHours(byte hours, ResumeTimeCallback resumeCallback)
		{
			IdleHoursPrivate(hours);
			RunEngine();
		}

		internal void UseHours(byte hours)
		{
			UseHoursPrivate(hours);
			RunEngine();
		}


		/// <summary>
		/// Reserve a certain number of hours to be consumed, but allow any interrupts or special events to use these hours as they see fit. If any hour is interrupted, the resumeCallback
		/// will be called after all interrupts for that hour are handled. If no hours are interrupted, the default text will be called and displayed, if possible. during the final hour,
		/// the location will be changed to the given value. Note that it's possible for an idle to be cancelled, and if this happens the player will not reach the given destination. 
		/// </summary>
		/// <typeparam name="T">the area to travel to after idleing</typeparam>
		/// <param name="hours">the number of hours to idle away</param>
		/// <param name="onInterruptCallback">the function called after all interrupts for a given hour have been processed.</param>
		/// <param name="defaultText">Any text to display if no interrupts are processed (optional)</param>
		internal void IdleHoursChangeLocation<T>(byte hours, ResumeTimeCallback onInterruptCallback, SimpleDescriptor defaultText = null) where T : AreaBase
		{
			IdleHoursPrivate(hours);
			DoIdleChange(new IdleDestinationStorage(typeof(T), onInterruptCallback, defaultText, currentTime.delta(idleHours)));
			RunEngine();

		}

		/// <summary>
		/// Consume a certain number of hours, and change the location to the given value. any interrupts or special events will be treated as if they occur after this time.
		/// </summary>
		/// <typeparam name="T">Type of the area to change to.</typeparam>
		/// <param name="hours">The number of hours used up.</param>
		internal void UseHoursChangeLocation<T>(byte hours) where T : AreaBase
		{
			UseHoursPrivate(hours);
			DoLocationChange(typeof(T), hours);
			RunEngine();

		}

		internal void IdleHoursChangeLocationToBase(byte hours, ResumeTimeCallback onInterruptCallback, SimpleDescriptor defaultText = null)
		{
			IdleHoursPrivate(hours);
			DoIdleChange(new IdleDestinationStorage(areaEngine.currentHomeBase.GetType(), onInterruptCallback, defaultText, currentTime.delta(idleHours)));
			RunEngine();
		}

		internal void UseHoursChangeLocationToBase(byte hours)
		{
			UseHoursPrivate(hours);
			DoLocationChange(areaEngine.currentHomeBase.GetType(), hours);
			RunEngine();
		}

		internal void SetAsideUsedHours(byte hours)
		{
			UseHoursPrivate(hours);
		}

		//alias that may be useful if something happens but for some reason does not take any time. 
		internal void ResumeTimePassing()
		{
			RunEngine();
		}

		/// <summary>
		/// Cancels any remaining idle time, if applicable. if not applicable, this has no effect.
		internal void CancelRemainingIdleTime()
		{
			if (idleHours == 0)
			{
				return;
			}
			idleHours = 0;
			//any other stuff to do. 
		}

		private void DoLocationChange(Type newLocationType, byte hoursOffset)
		{
			//find the first available time that is either empty, or can be overwritten.
			if (hoursOffset == 0 && changedLocationThisHourAlready)
			{
				UseHoursPrivate(1);
			}

			changedLocationThisHourAlready = true;
			areaEngine.SetArea(newLocationType);
		}

		private void DoIdleChange(IdleDestinationStorage idler)
		{
			finalDestination = idler;
		}

		private void RunEngine()
		{
			if (!running && totalHours > 0)
			{
				startTime = engineTime;
				hitLazies = false;
				currentContentPage = pageMaker();
				InitializeNewHour();
			}
			else
			{
				QueryPageStatus();
			}
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

			currentHour++;
			if (currentHour == 24)
			{
				currentHour = 0;
				currentDay++;
			}

			changedLocationThisHourAlready = false;
		}

		private void UseHoursPrivate(byte hours)
		{
			idleHours.subtractOff(hours);
			useHours.addIn(hours);
		}

		private void IdleHoursPrivate(byte hours)
		{
			idleHours.addIn(hours);
		}

		//assumes we have hours to pass. always precede this with a totalHoursPassed > 0 check.
		private void InitializeNewHour()
		{
			incrementHour();
			RunHour();
		}

		private void RunHour()
		{
			//Linq for the win! Simple version: take all reactions, full daily, multi-daily, and active listeners, and convert them into a function that returns an eventwrapper.
			//similarly, for all creatures currently participating in time events, take all their daily, multi-daily, and active items (collection of collections), and flatten them into one
			//collection for each type. we union all of these results together into one giant collection of items for this engine to parse. this giant collection is passed into the constructor
			//for a queue, and then we're good to go. 

			


			while (!reactions.isEmpty && reactions.Peek().procTime.CompareTo(GameDateTime.Now) <= 0)
			{
				TimeReactionBase scene = reactions.Pop().onProc;
				ParsePage(scene);
			}

			IEnumerable<Func<TimeReactionBase>> eventsToParse =
				fullDailyListeners.Where(x => x.hourToTrigger == currentHour).Select<ITimeDailyListenerFull, Func<TimeReactionBase>>(x => x.reactToDailyTrigger)
				.Union(CreatureStore.activeCreatureList.SelectMany(x => x.QueryFullDailyListeners(currentHour).Select<ITimeDailyListenerFull, Func<TimeReactionBase>>(y => y.reactToDailyTrigger)))
				.Union(fullMultiTimeListeners.Where(x => x.triggerHours.Contains(currentHour)).Select<ITimeDayMultiListenerFull, Func<TimeReactionBase>>(x => () => x.reactToTrigger(currentHour)))
				.Union(CreatureStore.activeCreatureList.SelectMany(x => x.QueryFullDayMultiListeners(currentHour).Select<ITimeDayMultiListenerFull, Func<TimeReactionBase>>(y => () => y.reactToTrigger(currentHour))))
				.Union(fullActiveListeners.Select<ITimeActiveListenerFull, Func<TimeReactionBase>>(x => x.reactToHourPassing))
				.Union(CreatureStore.activeCreatureList.SelectMany(x => x.QueryFullActiveListeners().Select<ITimeActiveListenerFull, Func<TimeReactionBase>>(y => y.reactToHourPassing)));

			foreach (var element in eventsToParse)
			{
				TimeReactionBase result = element?.Invoke();
				ParsePage(result);
			}

			if (this.activePages.Count > 0 && finalDestination != null)
			{
				this.newHourHeader = finalDestination.DoResume(idleHours, areaEngine.currentArea);
			}

			QueryPageStatus();
		}

		private void ParsePage(TimeReactionBase scene)
		{
			if (scene is null)
			{
				return;
			}
			DisplayWrapper eventResult = scene.RunEvent(isIdleTime, idleHours > 0);
			if (DisplayWrapper.IsNullOrEmpty(eventResult))
			{
				return;
			}
			else if (eventResult.isSimpleReaction)
			{
				currentContentPage.OutputText(eventResult.simpleReaction);
			}
			else
			{
				DisplayBase display = eventResult.fullPageReaction;
				if (!string.IsNullOrEmpty(newHourHeader))
				{
					display.CombineWith(newHourHeader, false);
					newHourHeader = null;
				}
				activePages.Enqueue(display);
			}
		}

		private void CheckHour()
		{
			if (totalHours > 0)
			{
				InitializeNewHour();
			}
			else if (!hitLazies)
			{
				DoLazies();
			}
			else
			{
				ReturnExectution();
			}
		}

		private void DoLazies()
		{
			int hoursPassed = startTime.hoursToNow();
			byte lazyHoursPassed = hoursPassed > byte.MaxValue ? byte.MaxValue : (byte)hoursPassed;

			IEnumerable<ITimeLazyListener> lazyItems = lazyListeners.Union(CreatureStore.activeCreatureList.SelectMany(x => x.QueryLazyListeners()));

			foreach (var item in lazyItems)
			{
				currentContentPage.OutputText(item.reactToTimePassing(lazyHoursPassed));
			}

			//it may be possible for some of the previously processed events to create a new reaction, like an asshole. take care of that here.
			while (!reactions.isEmpty && reactions.Peek().procTime.CompareTo(GameDateTime.Now) <= 0)
			{
				TimeReactionBase scene = reactions.Pop().onProc;
				ParsePage(scene);
			}

			QueryPageStatus();
		}

		private void ReturnExectution()
		{
			bool newPage = !currentContentPage.hasNoText;

			//i'm placing the final destination text on the current content page. However, if the only thing on the current context page is just this text,
			//it shouldn't get its own page, so that will appear on the same page as the destination context. 

			string text;
			if (!string.IsNullOrEmpty(newHourHeader))
			{
				text = newHourHeader;
			}
			else
			{
				text = finalDestination?.GetFinalText(); //trudge toward your final destination. 
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				currentContentPage.OutputText(text);
				//hasAnyOutput = true; imo this would be annoying if this was the only thing that forced it to a new page.
			}

			startTime = null;
			newHourHeader = null;


			if (!newPage)
			{
				areaEngine.RunArea(currentContentPage);
			}
			else
			{
				currentContentPage.DoNext(() => displayPage(areaEngine.RunArea(null)));
				displayPage(currentContentPage);
			}

			currentContentPage = null;
		}
		
		private class IdleDestinationStorage
		{
			public readonly Type locationType;
			private readonly SimpleDescriptor locationCallback;
			public readonly GameDateTime travelTime;
			private readonly ResumeTimeCallback onInterruptCallback;

			private bool didResume = false;

			public IdleDestinationStorage(Type locationType, ResumeTimeCallback onInterruptCallback, SimpleDescriptor locationCallback, GameDateTime timeToBoogie)
			{
				this.locationType = locationType ?? throw new ArgumentNullException(nameof(locationType));
				this.locationCallback = locationCallback;
				this.onInterruptCallback = onInterruptCallback ?? throw new ArgumentNullException(nameof(onInterruptCallback));
				travelTime = timeToBoogie ?? throw new ArgumentNullException(nameof(timeToBoogie));
			}

			public string DoResume(ushort hoursRemaining, AreaBase currentArea)
			{
				didResume = true;
				return onInterruptCallback?.Invoke(hoursRemaining, currentArea);
			}

			public string GetFinalText()
			{
				if (didResume)
				{
					return null;
				}
				else
				{
					return locationCallback?.Invoke();
				}
			}
		}
	}
}
