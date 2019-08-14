//TimeEngine.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using CoC.Backend.Engine.Events;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.Engine.Time
{
	public sealed class TimeEngine
	{
		private readonly Action<string> OutputText;
		private Action<Action> DoNext => MenuHelpers.DoNext;

		//what i would do for a linked hashset in C#. Update: Nevermind, That's what friends (and beer, apparently) are for. -JSG
		internal readonly OrderedHashSet<ITimeLazyListener> lazyListeners = new OrderedHashSet<ITimeLazyListener>();
		internal readonly OrderedHashSet<ITimeActiveListener> activeListeners = new OrderedHashSet<ITimeActiveListener>();
		internal readonly OrderedHashSet<ITimeDailyListener> dailyListeners = new OrderedHashSet<ITimeDailyListener>();
		internal readonly OrderedHashSet<ITimeDayMultiListener> dayMultiListeners = new OrderedHashSet<ITimeDayMultiListener>();
		//i wrote the priority queue though - that wasn't too hard. though it also isn't even remotely bulletproof. 
		internal readonly PriorityQueue<TimeReaction> reactions = new PriorityQueue<TimeReaction>();

		//Event variables
		private Queue<ITimeActiveListener> activeQueue = new Queue<ITimeActiveListener>();
		private Queue<ITimeLazyListener> lazyQueue = new Queue<ITimeLazyListener>();
		private Queue<ITimeDailyListener> dailyQueue = new Queue<ITimeDailyListener>();

		private Queue<SpecialEvent> specialEvents = new Queue<SpecialEvent>();


		private bool running = false;
		private readonly StringBuilder outputMagic = new StringBuilder();


		private bool ranLaziesYet = false;

		internal ushort totalHoursPassed => ((ushort)usedHoursPassed).add(idleHoursPassed);
		//private ushort hoursPassedForLazies = 0;

		private GameDateTime lastTimeLaziesRan;

		internal byte usedHoursPassed { get; private set; }
		internal byte idleHoursPassed { get; private set; }

		//private bool canCancel = false;

		private bool hasAnyOutput = false;
		private readonly AreaEngine areaEngine;

		private ResumeTimeCallback resumeActionsCallback = null;

		//end Event Variables.

		private bool isIdleHour => usedHoursPassed == 0;

		internal byte CurrentHour { get; private set; }
		internal int CurrentDay { get; private set; }

		public TimeEngine(AreaEngine areaEngineReference)
		{
			areaEngine = areaEngineReference ?? throw new ArgumentNullException(nameof(areaEngineReference));
		}

		public void SetAsideHours(byte hours)
		{
			if (totalHoursPassed == 0)
			{
				lastTimeLaziesRan = GameDateTime.Now;
			}


			byte oldValue = usedHoursPassed;
			usedHoursPassed = usedHoursPassed.add(hours);
			byte delta = usedHoursPassed.subtract(oldValue);

		}

		public void UseHours(byte hours)
		{
			if (totalHoursPassed == 0)
			{
				lastTimeLaziesRan = GameDateTime.Now;
			}

			byte oldValue = usedHoursPassed;
			usedHoursPassed = usedHoursPassed.add(hours);
			byte delta = usedHoursPassed.subtract(oldValue);

			//canCancel = false;
			Run();
		}

		public void IdleHours(byte hours, ResumeTimeCallback resumeCallback)
		{
			if (totalHoursPassed == 0)
			{
				lastTimeLaziesRan = GameDateTime.Now;
			}

			byte oldValue = idleHoursPassed;
			idleHoursPassed = idleHoursPassed.add(hours);
			byte delta = idleHoursPassed.subtract(oldValue);


			resumeActionsCallback = resumeCallback;
			Run();
		}

		private void incrementHour()
		{
			if (usedHoursPassed > 0)
			{
				usedHoursPassed--;
			}
			else
			{
				idleHoursPassed--;
			}

			CurrentHour++;
			if (CurrentHour == 24)
			{
				CurrentHour = 0;
				CurrentDay++;
			}
		}

		//currently, setting aside hours have no means of being ran unless UseHours or IdleHours is called sometime after.
		//The expected behavior is to simply call UseHours or IdleHours afterward at some point, but this would provide
		//an alternate means for running the time engine after setting aside hours. Currently unused. 
		public void ForceRun()
		{
			Run();
		}

		private void Run()
		{
			if (!running && totalHoursPassed > 0)
			{
				if (lastTimeLaziesRan is null)
				{
					lastTimeLaziesRan = GameDateTime.Now;
				}

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
				while (!EventWrapper.IsNullOrEmpty(wrapper))
				{
					if (!wrapper.isScene)
					{
						outputMagic.Append(wrapper.text);
					}
					else
					{
						specialEvents.Enqueue(wrapper.scene);
					}
					wrapper = wrapper.next;
				}
			}

			while (dailyQueue.Count > 0)
			{
				ITimeDailyListener item = dailyQueue.Dequeue();
				EventWrapper wrapper = item.reactToDailyTrigger();
				while (!EventWrapper.IsNullOrEmpty(wrapper))
				{
					if (!wrapper.isScene)
					{
						outputMagic.Append(wrapper.text);
					}
					else
					{
						specialEvents.Enqueue(wrapper.scene);
					}
					wrapper = wrapper.next;
				}
			}

			activeQueue = new Queue<ITimeActiveListener>(activeListeners);

			while (activeQueue.Count > 0)
			{
				ITimeActiveListener item = activeQueue.Dequeue();
				EventWrapper wrapper = item.reactToHourPassing();
				while (!EventWrapper.IsNullOrEmpty(wrapper))
				{
					if (!wrapper.isScene)
					{
						outputMagic.Append(wrapper.text);
					}
					else
					{
						specialEvents.Enqueue(wrapper.scene);
					}
					wrapper = wrapper.next;
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

		internal void CancelIdle()
		{
			var delta = idleHoursPassed;
			idleHoursPassed = 0;

			resumeActionsCallback = null;
		}

		//Currently, the first special event will be on the same page as all the short, non-special stuff.
		//If i read the code correctly, that's what we currently do, but if that's not the desired result, we can fix it. 
		//To do so, uncomment the first iteration code in NextEvent, then toggle the NextEvent functions in CatchUpLazies and RunHour to their 
		//boolean variants. (comment out current, uncomment one with 'true')

		//if it helps, think of this as recursive - while it has any special events, it will tell them to call this when they are done. 
		//it's essentially a while loop, but broken up due to event-based user interraction. 
		private void NextEvent(/*bool firstIteration = false*/)
		{

			if (specialEvents.Count > 0)
			{
				//set up the next iteration
				Action next;

				//if we're the last special event and we have a special resume callback. 
				if (specialEvents.Count == 1 && resumeActionsCallback != null)
				{
					next = () => DoNext(() => { resumeActionsCallback(totalHoursPassed, areaEngine.currentArea); NextEvent(); });
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
				item.BuildInitialScene(isIdleHour, idleHoursPassed > 0, next);
				//}
			}
			else if (totalHoursPassed > 0)
			{
				InitializeNewHour();
			}
			else if (!ranLaziesYet)
			{
				CatchUpLazies();
			}
			else
			{
				ReturnExecution();
			}
		}

		private void CatchUpLazies()
		{
			outputMagic.Clear();

			ranLaziesYet = true;
			lazyQueue = new Queue<ITimeLazyListener>(lazyListeners);

			int hoursPassed = lastTimeLaziesRan.hoursToNow();
			byte lazyHoursPassed = hoursPassed > byte.MaxValue ? byte.MaxValue : (byte)hoursPassed;

			while (lazyQueue.Count > 0)
			{
				ITimeLazyListener item = lazyQueue.Dequeue();

				EventWrapper wrapper = item.reactToTimePassing(lazyHoursPassed);
				while (!EventWrapper.IsNullOrEmpty(wrapper))
				{
					if (!wrapper.isScene)
					{
						outputMagic.Append(wrapper.text);
					}
					else
					{
						specialEvents.Enqueue(wrapper.scene);
					}
					wrapper = wrapper.next;
				}
			}

			lastTimeLaziesRan = GameDateTime.Now;

			if (outputMagic.Length != 0)
			{
				hasAnyOutput = true;
				OutputText(outputMagic.ToString());
				outputMagic.Clear();
			}
			//NextEvent(true);
			NextEvent();
		}


		private void ReturnExecution()
		{
			running = false;
			ranLaziesYet = false;

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

		internal void LoadInSavedTime(byte hours, int days)
		{
			CurrentDay = days;
			CurrentHour = hours;
		}
	}
}
