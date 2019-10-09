//GameEngine.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using CoC.Backend.Achievements;
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
		public static Player currentlyControlledCharacter => CreatureStore.currentControlledCharacter;

		public static Player playerCharacter
		{
			get => CreatureStore.activePlayer;
			private set => CreatureStore.SetActivePlayerCharacter(value);
		}

		public static ReadOnlyCollection<GameDifficulty> difficulties;
		public static int defaultDifficultyIndex { get; private set; }

		//Time
		private static TimeEngine timeEngine;
		//areas.
		private static AreaEngine areaEngine;
		//only for testing.
		//internal static AreaEngine areaEngine;

		private static AchievementCollection globalAchievements;

		internal static Func<Creature, BasePerkModifiers> constructPerkModifier;

		//internal static ReadOnlyDictionary<Type, Func<PerkBase>> perkList;

#warning TODO: Fix this engine - see below:
		//Needs to be fixed to make occur/needs implementing:
		//use hours cannot be interrupted, and idle hours should be consumed if a current active/daily trigger uses hours. 
		//cancel idle only interrupts idle - if there are still hours to be used, they must be used. 
		//special interrupt scenes need to be given their own code land. when it occurs, the remaining active items for the current hour are ran to completion. any other special interrupts for 
		//the current hour are queued. 
		//need a means of internally handling the special interrupt scenes to do the above 
		//special interrupt scenes cannot occur from lazy land - that doesn't really make sense, as lazies can occur at any time. 
		//need a means of handling how go to location is handled, especially in terms of use/idle - if you're idling then go to camp, but it's interrupted and you go to tel-adre instead, you need
		//to stay in Tel-Adre or whatever if it's canceled. 
		//location cannot be changed during "used" hours, but it can be queued. Upon completion of the current used hours, the location will be updated. 
		//special interrupt scenes need special logic for location change and time usage - if they need those, they break shit. 
		
		//new idea: location change now functions like a one-off reaction - it's added to the time events after the current used time. any additional time used up caused by various reactions and
		//whatnot will occur AFTER this change. further, location changes are now split into two categories: head toward and go to. if the current location change is a "head toward" variant, it's
		//possible to change that location without requiring any additional time. For example, if you are heading back to camp when some NPC finds you and drags you to another location, then
		//you never get to camp. However, if the location change is a go to, it cannot be interrupted. if another location change occurs, it takes an additional hour, and is queued as the last event
		//in that new hour. this effect stacks as needed. ideally, this kind of collision will never occur, but this is a nice, clean, reasonable means of addressing it. 

		//sub-places need to be handled better. Current idea is to integrate them directly into places. places will need a special "onreturnfromsubplace" for what happens when they say "go back"
		//in that subplace. note that some sub-places may not allow a "go back". on stay exists, but it's designed for when something happens, time passes, but you stay in the same location.
		//the on return from sub-place would be for when no time has changed.

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
		
		//use a certain amount of hours up. After all hours are consumed, returns to base.
		//Use Hours cannot be interrupted. 
		public static void ReturnToBaseAfter(byte hours)
		{
			timeEngine.UseHoursGoToBase(hours, true);
		}

		public static void ReturnToBaseAfter(byte hours, bool headBackToCurrentLocationIfMoreHoursRemain)
		{
			timeEngine.UseHoursGoToBase(hours, headBackToCurrentLocationIfMoreHoursRemain);
		}

		//99% of the time, the resume callback will probably be Outputting some text about returning to camp, and cancelling the remaining idle hours. 
		public static void ReturnToBaseAfterIdling(byte hours, ResumeTimeCallback resumeCallback)
		{
			timeEngine.IdleHoursGoToBase(hours, resumeCallback);
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

		//use a certain number of hours. After all hours are consumed, goes to target Area
		public static void UseHoursThenGotoArea<T>(byte hours) where T : AreaBase
		{
			timeEngine.UseHoursGoToArea<T>(hours, true);
		}

		public static void UseHoursThenGotoArea<T>(byte hours, bool headBackToCurrentLocationIfMoreHoursRemain) where T : AreaBase
		{
			timeEngine.UseHoursGoToArea<T>(hours, headBackToCurrentLocationIfMoreHoursRemain);
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
			timeEngine.IdleHoursGoToArea<T>(hours, resumeCallback);
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
			timeEngine.ChangeLocation<T>(true);
		}

		public static void CancelRemainingIdleHoursReturnToBase()
		{
			timeEngine.CancelIdle();
			timeEngine.GoToBase(true);
		}

		//Functions for dealing with time related actions. Should otherwise be useless. 

		public static byte IdleHoursRemaining()
		{
			return timeEngine.idleHours;
		}

		public static byte UseHoursRemaining()
		{
			return timeEngine.useHours;
		}

		public static ushort TotalHoursRemaining()
		{
			return timeEngine.totalHours;
		}

		public static bool IdleDestinationIs<T>() where T : AreaBase
		{
			return timeEngine.QueryFinalDestinationType() == typeof(T);
		}

		public static bool IdleDestinationIsHomeBase()
		{
			return timeEngine.QueryFinalDestinationType() == areaEngine.currentHomeBase.GetType();
		}

		public static void UpdateIdleFinalDestination<T>() where T : AreaBase
		{
			timeEngine.UpdateIdleFinalDestination<T>();
		}

		public static void UpdateIdleFinalDestinationHomeBase()
		{
			timeEngine.UpdateIdleFinalDestinationHomeBase();
		}
		//End Note. 

		public static void InitializeTime(int currDay, byte currHour)
		{
			timeEngine.InitializeTime(currDay, currHour);
		}

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

		//attempts to unlock the provided area. If the area is already unlocked, it will return false, otherwise it'll return true. Regardless, it'll run
		//the area 
		public static bool UnlockArea<T>() where T : VisitableAreaBase
		{
			return areaEngine.UnlockArea<T>();
		}


		public static void InitializeEngine(Action<string> output,
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations,
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> gameHomeBases, //Area Engine
			Func<Creature, BasePerkModifiers> perkVariables, //perk data for creatures to use. 
			ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficulty) //Game Difficulty Collections.
		{
			areaEngine = new AreaEngine(output, gamePlaces, gameLocations, gameDungeons, gameHomeBases);
			timeEngine = new TimeEngine(output, areaEngine);


			difficulties = gameDifficulties ?? throw new ArgumentNullException(nameof(gameDifficulties));
			defaultDifficultyIndex = defaultDifficulty;
			constructPerkModifier = perkVariables ?? throw new ArgumentNullException(nameof(perkVariables));
		}

		internal static void PostSaveInit()
		{
			globalAchievements = new AchievementCollection();
		}



		public static void StartNewGame()
		{
			playerCharacter = null;
			SaveData.SaveSystem.ResetSessionDataForNewGame();
		}

		public static void InitializeGame(Player player)
		{
			playerCharacter = player;
			SaveData.SaveSystem.MarkGameLoaded();
		}

		public static void OnGameCompletion()
		{
			int? highestBeaten = SaveData.BackendGlobalSave.data.highestDifficultyBeaten;
			int difficulty = SaveData.BackendSessionSave.data.lowestDifficultyForThisCampaign;

			if (highestBeaten == null || highestBeaten < difficulty)
			{
				SaveData.BackendGlobalSave.data.highestDifficultyBeaten = difficulty;
			}
			Console.WriteLine("Womp, Womp. Roll Credits!"); //my easter egg for anyone who attaches this to console out. 
		}

		//local save data.
		public static void LoadFileBackend(FileInfo file)
		{
			//if ()
			//open file, do magic parsing shit. 
			//timeEngine.LoadInSavedTime(file.whatever.hours, file.whatever.days);
			//currPlater = Player.LoadFromFile(file);
			//currHomeBase = file.whatever.homeBase;
			//currLocation = file.whatever.location;
			//load all the save datas. 
			//

			//currLocation.Initialize();

		}

		public static bool UnlockAchievement<T>() where T: AchievementBase
		{
			if (!AchievementManager.HasRegisteredAchievement<T>())
			{
				throw new ArgumentException($"A member of Type {typeof(T)} was never added to the achievement manager.");
			}
			else
			{
				var adder = AchievementManager.GetAchievement<T>();
				if (adder != null)
				{
					return globalAchievements.AddAchievement(adder);
				}
				return false;
			}
		}

		public static bool QueryUnlockedAchievements(out ReadOnlyCollection<AchievementBase> unlockedAchievements)
		{
			unlockedAchievements = globalAchievements.unlockedAchievements;
			return globalAchievements.QueryNewAchievementsUnlocked();
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
