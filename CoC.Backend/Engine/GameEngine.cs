//GameEngine.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.IO;

namespace CoC.Backend.Engine
{
	public static class GameEngine
	{
		//NYI
		public static AreaBase currentLocation;

		//NYI
		public static Player currentPlayer
		{
			get => _currentPlayer;
			private set
			{
				//only necessary b/c garbage collection may not be instant and if events are firing due to new player loaded and playing. I'd bet my 
				//house (or apartment, whatever) that this would never actually happen. But now it won't, guarenteed.
				if (_currentPlayer != value)
				{
					_currentPlayer.CleanupCreatureForDeletion();
				}
				_currentPlayer = value;
			}
		}
		private static Player _currentPlayer;


		//Time
		private static TimeEngine timeEngine;
		public static byte CurrentHour => timeEngine.CurrentHour;
		public static int CurrentDay => timeEngine.CurrentDay;

		public static void RemainInLocationUseHours(byte hours)
		{
			timeEngine.UseHours(hours);
		}

		public static void GoToLocationThenUseHours(AreaBase location, byte hours)
		{
			currentLocation = location;
			timeEngine.GoToLocationAndUseHours(location, hours);
		}

		public static void UseHoursThenGoToLocation(AreaBase location, byte hours)
		{
			timeEngine.UseHours(hours);
			currentLocation = location;
		}


		public static void InitializeBackend(Action<Action> DoNext, Action<string> OutputText)
		{
			timeEngine = new TimeEngine(OutputText, DoNext);
			currentLocation = null;
			_currentPlayer = null;
		}

		public static void LoadFileBackend(FileInfo file)
		{
			//open file, do magic parsing shit. 
			//timeEngine.LoadInSavedTime(file.whatever.hours, file.whatever.days);
			//currPlater = Player.LoadFromFile(file);
			//currLocation = file.whatever.location;
			//load all the save datas. 
			//

			//currLocation.Initialize();
		}

		//what i would do for a linked hashset in C#. Update: Nevermind, That's what friends (and beer, apparently) are for. -JSG

		internal static readonly OrderedHashSet<ITimeLazyListener> lazyListeners = new OrderedHashSet<ITimeLazyListener>();
		internal static readonly OrderedHashSet<ITimeActiveListener> activeListeners = new OrderedHashSet<ITimeActiveListener>();
		internal static readonly OrderedHashSet<ITimeDailyListener> dailyListeners = new OrderedHashSet<ITimeDailyListener>();
		internal static readonly OrderedHashSet<ITimeDayMultiListener> dayMultiListeners = new OrderedHashSet<ITimeDayMultiListener>();

		public static bool RegisterLazyListener(ITimeLazyListener listener)
		{
			return lazyListeners.Add(listener);
		}

		public static bool DeregisterLazyListener(ITimeLazyListener listener)
		{
			return lazyListeners.Remove(listener);
		}

		public static bool RegisterActiveListener(ITimeActiveListener listener)
		{
			return activeListeners.Add(listener);
		}

		public static bool DeregisterActiveListener(ITimeActiveListener listener)
		{
			return activeListeners.Remove(listener);
		}

		public static bool RegisterDailyListener(ITimeDailyListener listener)
		{
			return dailyListeners.Add(listener);
		}

		public static bool DeregisterDailyListener(ITimeDailyListener listener)
		{
			return dailyListeners.Remove(listener);
		}

		public static bool RegisterDayMultiListener(ITimeDayMultiListener listener)
		{
			return dayMultiListeners.Add(listener);
		}

		public static bool DeregisterDayMultiListener(ITimeDayMultiListener listener)
		{
			return dayMultiListeners.Remove(listener);
		}

	}
}
