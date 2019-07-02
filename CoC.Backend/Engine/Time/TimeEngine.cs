//TimeEngine.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using CoC.Backend.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Backend.Engine.Time
{
	public sealed class TimeEngine
	{
		private readonly Action<string> OutputText;
		private readonly Action<Action> DoNext;


		//Event variables
		private Queue<ITimeActiveListener> activeQueue = new Queue<ITimeActiveListener>();
		private Queue<ITimeLazyListener> lazyQueue = new Queue<ITimeLazyListener>();
		private Queue<ITimeDailyListener> dailyQueue = new Queue<ITimeDailyListener>();

		private Queue<SpecialEvent> specialEvents = new Queue<SpecialEvent>();


		private bool running = false;
		private readonly StringBuilder outputMagic = new StringBuilder();


		private bool ranLaziesYet = false;

		private byte hoursPassed = 0;
		private byte hoursPassedForLazies = 0;

		private bool hasAnyOutput = false;
		private AreaBase nextLocation;
		//end Event Variables.

		internal byte CurrentHour { get; private set; }
		internal int CurrentDay { get; private set; }

		public TimeEngine(Action<string> textWriter, Action<Action> buttonMaker)
		{
			OutputText = textWriter;
			DoNext = buttonMaker;
		}

		public void GoToLocationAndUseHours(AreaBase location, byte hours)
		{
			nextLocation = location;
			UseHours(hours);
		}

		public void UseHours(byte hours)
		{
			hoursPassed += hours;
			hoursPassedForLazies = hoursPassed;
			if (nextLocation == null)
			{
				nextLocation = GameEngine.currentLocation;
			}
			Run();

		}

		private void incrementHour()
		{
			CurrentHour++;
			if (CurrentHour == 24)
			{
				CurrentHour = 0;
				CurrentDay++;
			}
		}

		public void ForceRun()
		{
			Run();
		}

		private void Run()
		{
			if (!running && hoursPassed > 0)
			{
				hasAnyOutput = false;
				running = true;
				InitializeNewHour();
			}
		}

		private void InitializeNewHour()
		{
			hoursPassed--;
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
			IEnumerable<ITimeDailyListener> converts = GameEngine.dayMultiListeners.Where((x) => x.triggerHours.Contains(CurrentHour)).Select((x) => x.ToSingleDay(CurrentHour));
			//then we union that together with the current OrderedHashSet
			//since both are ordered hashsets, they will preserve order from IEnumerable, though all entries with a single hour proc will go before the converted multi proc ones.
			dailyQueue = new Queue<ITimeDailyListener>(GameEngine.dailyListeners.Where((x) => x.hourToTrigger == CurrentHour).Union(converts));

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

			activeQueue = new Queue<ITimeActiveListener>(GameEngine.activeListeners);

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

		//Currently, the first special event will be on the same page as all the short, non-special stuff.
		//If i read the code correctly, that's what we currently do, but if that's not the desired result, we can fix it. 
		//To do so, uncomment the first iteration code in NextEvent, then toggle the NextEvent functions in CatchUpLazies and RunHour to their 
		//boolean variants. (comment out current, uncomment one with 'true')


		private void NextEvent(/*bool firstIteration = false*/)
		{
			void next() => DoNext(() => NextEvent());

			if (specialEvents.Count > 0)
			{
				hasAnyOutput = true;
				SpecialEvent item = specialEvents.Dequeue();
				//if (firstIteration)
				//{
				//	DoNext(() => item.BuildInitialScene(next));
				//}
				//else
				//{
				item.BuildInitialScene(next);
				//}
			}
			else if (hoursPassed > 0)
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
			lazyQueue = new Queue<ITimeLazyListener>(GameEngine.lazyListeners);

			while (lazyQueue.Count > 0)
			{
				ITimeLazyListener item = lazyQueue.Dequeue();
				EventWrapper wrapper = item.reactToTimePassing(hoursPassedForLazies);
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

			hoursPassedForLazies = 0;

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
			var loc = nextLocation;
			nextLocation = null;
			if (hasAnyOutput)
			{
				DoNext(loc.RunArea);
			}
			else
			{
				loc.RunArea();
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
