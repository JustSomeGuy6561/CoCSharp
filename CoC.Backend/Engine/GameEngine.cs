using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.Engine
{
	public static class GameEngine
	{
		//NYI
		public static LocationBase currentLocation;

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

		//NYI
		public static byte CurrentHour;
		public static int CurrentDay;

		public static void Run()
		{

		}

		//what i would do for a linked hashset in C#. Update: Nevermind, That's what friends (and beer, apparently) are for. -JSG

		private static readonly OrderedHashSet<ITimeLazyListener> lazyListeners = new OrderedHashSet<ITimeLazyListener>();
		private static readonly OrderedHashSet<ITimeActiveListener> activeListeners = new OrderedHashSet<ITimeActiveListener>();
		private static readonly OrderedHashSet<ITimeDailyListener> dailyListeners = new OrderedHashSet<ITimeDailyListener>();
		private static readonly OrderedHashSet<ITimeDayMultiListener> dayMultiListeners = new OrderedHashSet<ITimeDayMultiListener>();

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
