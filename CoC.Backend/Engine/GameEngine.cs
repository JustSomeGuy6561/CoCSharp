//GameEngine.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Events;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CoC.Backend.Engine
{
	//inventory engine (does not exist) or inventory manager (does)
	//will need the ability to display their items, which means they'll need access to button manager as well. 
	//alternatively, it'd be possible to simply pass it to the GUI and let the GUI decide how it wants to deal with it. 
	public static class GameEngine
	{
		public static AreaBase currentArea => areaEngine.currentArea;

		//NYI
		public static Player currentPlayer
		{
			get => _currentPlayer;
			private set
			{
				//unfortunately, this is probably necessary. No weak 
				if (_currentPlayer != value)
				{
					_currentPlayer.CleanupCreatureForDeletion();
				}
				_currentPlayer = value;
			}
		}
		private static Player _currentPlayer;

		public static ReadOnlyCollection<GameDifficulty> difficulties;
		public static int defaultDifficultyIndex { get; private set; }

		//Time
		private static TimeEngine timeEngine;
		//areas.
		private static AreaEngine areaEngine;
		//only for testing.
		//internal static AreaEngine areaEngine;

		internal static Func<BasePerkModifiers> constructPerkModifier;

		//internal static ReadOnlyDictionary<Type, Func<PerkBase>> perkList;

#warning TODO: Fix this engine - use should never be interrupted, but cancelIdle breaks that. in the event someone says goToLocation during a use, it should be ignored - perhaps delayed?
#warning The best way of doing this, i guess, is to pass any time event a bool that indicates whether current actions can be interrupted. if not, some events may be handled silently. 
		public static byte CurrentHour => timeEngine.CurrentHour;
		public static int CurrentDay => timeEngine.CurrentDay;

		//Time flow follows video game logic - when you're doing some scene, time is stopped. However, whatever you're doing takes time - it's just that that time passes after
		//you're done, instead. This means you don't have bebes during combat or while visiting town, etc. even if the time that passed says your should have. 
		//Is that how reality works? no. but that's honestly how we expect the game to work. 

		//There is one exception to this rule: idling/waiting. When you idle, time flows normally, so you have bebes or whatever exactly when you're supposed to, then resume idling
		//afterward. Because of this, you need to provide a callback to resume idling after one of these actions occur. Generally, this will just output some text saying akin to
		//"you go back to idling for the remaining X hours", but i suppose you could do anything. 

		//Unlike normal timeflow, an idle can be canceled - if you're passed out in a ditch and would normally wake up after 8 hours, but someone wakes your ass up and 
		//drags you to camp or wherever after only 3 hours, then you've only used 3 hours. Note that these interrupts are rare, but can happen.

		//For the most part, you can get away with just using the "ReturnToBaseAfter" function, or, in the aforementioned passed out examples (via combat loss or alcohol, for example)
		//"ReturnToBaseAfterIdling". However, if you need to do custom things, you have the ability to do so. 
		
		//use a certain amount of hours up. The next time PassTime is called, the game will 
		//Use Hours cannot be interrupted. 
		public static void ReturnToBaseAfter(byte hours)
		{
			areaEngine.ReturnToBase();
			UseHoursPassTime(hours);
		}

		//99% of the time, the resume callback will probably be Outputting some text about returning to camp, and cancelling the remaining idle hours. 
		public static void ReturnToBaseAfterIdling(byte hours, ResumeTimeCallback resumeCallback)
		{
			IdleHoursPassTime(hours, resumeCallback);
			areaEngine.ReturnToBaseIdle();
		}

		//use a certain number of hours, but continue the current scene as if nothing happened. This is useful for scenes that could consume a variable number of hours depending on
		//what the user does, and you don't want to manually keep track of that. For example, certain scenes can be extended if you stay, instead of leaving early. it may be easier
		//to implement and/or modify if you can just say 'this particular sub-scene is x hours, keep track of that for me.'
		public static void UseHoursWithoutPassingTime(byte hours)
		{
			timeEngine.SetAsideHours(hours);
		}

		//use a certain number of hours, and immediately consume them at the current location. 
		public static void UseHoursPassTime(byte hours)
		{
			timeEngine.UseHours(hours);
		}

		//set aside a certain number of hours, and immediately attempt to consume all of them at the current location
		//If something happens, resumeCallback will be called with the total number of hours remaining to idle. 
		public static void IdleHoursPassTime(byte hours, ResumeTimeCallback resumeCallback)
		{
			timeEngine.IdleHours(hours, resumeCallback);
		}

		//set aside a certain number of hours, and immediately attempt to consume them. After the idling is completed, will change
		//the current location to the provided area.
		public static void IdleHoursThenGotoArea<T>(byte hours, ResumeTimeCallback resumeCallback) where T : AreaBase
		{
			timeEngine.IdleHours(hours, resumeCallback);
			areaEngine.SetAreaIdle<T>();
		}

		//cancels any remaining idle hours. If a new destination was set via idle, it will change the current location to that are.
		public static void CancelRemainingIdleHours()
		{
			timeEngine.CancelIdle();
		}

		//cancels any remaining idle hours. Changes the current location to the provided area, overriding value set by idle, if any.
		public static void CancelRemainingIdleHoursOverrideArea<T>() where T: AreaBase
		{
			timeEngine.CancelIdle();
			areaEngine.SetAreaIdle<T>();
		}

		public static void CancelRemainingIdleHoursReturnToBase()
		{
			timeEngine.CancelIdle();
			areaEngine.ReturnToBaseIdle();
		}

		//Functions for dealing with time related actions. Should otherwise be useless. 

		public static byte IdleHoursRemaining()
		{
			return timeEngine.idleHoursPassed;
		}

		public static byte UseHoursRemaining()
		{
			return timeEngine.usedHoursPassed;
		}

		public static ushort TotalHoursRemaining()
		{
			return timeEngine.totalHoursPassed;
		}

		public static bool IdleDestinationIs<T>() where T : AreaBase
		{
			return areaEngine.delayedArea.GetType() == typeof(T);
		}

		public static bool IdleDestinationIsHomeBase()
		{
			return areaEngine.delayedArea == areaEngine.currentHomeBase;
		}
		//End Note. 


		//changes the current area to the provided one, but do not run the scene. Useful for cases where the game is doing time related shenanigans, or
		//you want to manually parse the scene. Note that it will occur immediately, even if the game is doing time logic, so be aware of any side effects that may cause.
		public static bool ChangeAreaSilent<T>() where T : AreaBase
		{
			return areaEngine.SetArea<T>();
		}

		public static bool ReturnToBaseSilent()
		{
			return areaEngine.ReturnToBase();
		}

		//changes the current area to the provided one, and tells the area engine to run it immediately. Useful for changing areas and doing the expected results. 
		public static void ChangeAreaRunScene<T>() where T: AreaBase
		{
			areaEngine.SetArea<T>();
			areaEngine.RunArea();
		}

		public static void ReturnToBaseRunScene()
		{
			ReturnToBaseAfter(0);
		}

		//attempts to unlock the provided area. If the area is already unlocked, it will return false, otherwise it'll return true. Regardless, it'll run
		//the area 
		public static bool UnlockArea<T>() where T : VisitableAreaBase
		{
			return areaEngine.UnlockArea<T>();
		}


		public static void InitializeEngine(
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations,
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> gameHomeBases, //Area Engine
			Func<BasePerkModifiers> perkVariables, //perk data for creatures to use. 
			ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficulty) //Game Difficulty Collections.
		{
			areaEngine = new AreaEngine(gamePlaces, gameLocations, gameDungeons, gameHomeBases);
			timeEngine = new TimeEngine(areaEngine);
			difficulties = gameDifficulties ?? throw new ArgumentNullException(nameof(gameDifficulties));
			defaultDifficultyIndex = defaultDifficulty;
			constructPerkModifier = perkVariables ?? throw new ArgumentNullException(nameof(perkVariables));
			if (perkVariables() is null)
			{
				throw new ArgumentException("perk variables cannot be null.");
			}
			_currentPlayer = null;
		}

		//local save data.
		public static void LoadFileBackend(FileInfo file)
		{
			//open file, do magic parsing shit. 
			//timeEngine.LoadInSavedTime(file.whatever.hours, file.whatever.days);
			//currPlater = Player.LoadFromFile(file);
			//currHomeBase = file.whatever.homeBase;
			//currLocation = file.whatever.location;
			//load all the save datas. 
			//

			//currLocation.Initialize();

		}




		public static bool RegisterLazyListener(ITimeLazyListener listener)
		{
			return timeEngine.lazyListeners.Add(listener);
		}

		public static bool DeregisterLazyListener(ITimeLazyListener listener)
		{
			return timeEngine.lazyListeners.Remove(listener);
		}

		public static bool RegisterActiveListener(ITimeActiveListener listener)
		{
			return timeEngine.activeListeners.Add(listener);
		}

		public static bool DeregisterActiveListener(ITimeActiveListener listener)
		{
			return timeEngine.activeListeners.Remove(listener);
		}

		public static bool RegisterDailyListener(ITimeDailyListener listener)
		{
			return timeEngine.dailyListeners.Add(listener);
		}

		public static bool DeregisterDailyListener(ITimeDailyListener listener)
		{
			return timeEngine.dailyListeners.Remove(listener);
		}

		public static bool RegisterDayMultiListener(ITimeDayMultiListener listener)
		{
			return timeEngine.dayMultiListeners.Add(listener);
		}

		public static bool DeregisterDayMultiListener(ITimeDayMultiListener listener)
		{
			return timeEngine.dayMultiListeners.Remove(listener);
		}

		public static void AddTimeReaction(TimeReaction reaction)
		{
			timeEngine.reactions.Push(reaction);
		}

		public static bool RemoveTimeReaction(TimeReaction reaction)
		{
			return timeEngine.reactions.Remove(reaction);
		}

		public static bool HasTimeReaction(TimeReaction reaction)
		{
			return timeEngine.reactions.Contains(reaction);
		}

		public static void AddPlaceReaction(PlaceReaction reaction)
		{
			areaEngine.AddReaction(reaction);
		}

		public static void AddLocationReaction(LocationReaction reaction)
		{
			areaEngine.AddReaction(reaction);
		}

		public static bool RemovePlaceReaction(PlaceReaction reaction)
		{
			return areaEngine.RemoveReaction(reaction);
		}

		public static bool RemoveLocationReaction(LocationReaction reaction)
		{
			return areaEngine.RemoveReaction(reaction);
		}

		public static bool HasPlaceReaction(PlaceReaction reaction)
		{
			return areaEngine.HasReaction(reaction);
		}

		public static bool HasLocationReaction(LocationReaction reaction)
		{
			return areaEngine.HasReaction(reaction);
		}

		public static void SetHomeBase<T>() where T: HomeBaseBase
		{
			areaEngine.ChangeHomeBase<T>();
		}
	}
}
