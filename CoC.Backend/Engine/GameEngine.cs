//GameEngine.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using CoC.Backend.Achievements;
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Reaction;
using CoC.Backend.UI;
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
		//only issue with time as of now is perhaps a return to idle?
		//also may decide we can't just use, run time, then go to a location not the base - that may be weird. if we decide that, it's much simpler to implement. 
		//however, idle DOES need to occur from anywhere, then return to base, whereas, use would occur AT the location (and if this is changed, that would only be the base, AFTER the time passed.

		//one possibility is adding a generic combat area that takes another area as its base, which we could set as the current area so that the game can differentiate between when you're at a
		//location and when you're passed out at that location after losing in combat. 

		//sub-places need to be handled better. Current idea is to integrate them directly into places. places will need a special "onreturnfromsubplace" for what happens when they say "go back"
		//in that subplace. note that some sub-places may not allow a "go back". on stay exists, but it's designed for when something happens, time passes, but you stay in the same location.
		//the on return from sub-place would be for when no time has changed.

		public static byte CurrentHour => timeEngine.currentTime.hour;
		public static int CurrentDay => timeEngine.currentTime.day;

		public static void ForceReload()
		{
			areaEngine.ForceReload();
		}

		public static void SetDifficulty(byte difficultySetting)
		{
			difficulties[difficultySetting].OnGameStart();
		}

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

		//from a display standpoint, these functions will clear the current display, display any special text or events that happen over the period of time, then go to the given location
		//(if applicable). regardless of if the location changed, the game will resume by running the area of the current location. 

		public static void UseHoursGoToBase(byte hours)
		{
			timeEngine.UseHoursChangeLocationToBase(hours);
		}

		public static void ResumeExection()
		{
			timeEngine.ResumeTimePassing();
		}

		//End Note. 

		//some context to below: to allow a time jump and then allow you to call UseHours and such, we'd have to: 
		//	- remove everything from the current time queue that could not handle large jumps
		//	- silently abort all pregnancies everywhere. 
		//	- silently cancel all reactions. 
		//	- handle all perks and related info, even ones we have no idea what would happen - slime feed, cum addicts, succubi, etc. 
		//... so we don't. But we need the initial jump, and it is kinda cool to say - "One Year Later" and have the Day GUI display that too, so as long as you are 
		//confident you can use it without breaking the game. For example, a long/multipage text epilogue X years later would be possible, so long as you only use jump time and
		//never do anything time event related. Or, a bad end over a period of days/months/years, again faking time passing with the jump. Note that unless you store the original date and time
		//which i'd recommend doing, you wouldn't be able to return from this bad end. 

		/// <summary>
		/// Set the day and hour to a new value. The behavior for anything currently in the time engine when this jump occurs is NOT DEFINED, so this should only be called when
		/// the time engine listeners are not initialized, or in such a way that the time engine is not called again (like a game over), or do jumps in such a way that the game
		/// is unaware you did it (like jumping forward a year for a bad end, but if the player chooses to resume from a bad end, jump back to the original time).
		/// Basically, use this at your own risk. 
		/// </summary>
		/// <param name="currDay">day to jump to</param>
		/// <param name="currHour">hour to jump to</param>
		public static void InitializeOrJumpTime(int currDay, byte currHour)
		{
			timeEngine.InitializeTime(currDay, currHour);
		}

		//changes the current area to the provided one, but do not run the scene. Useful for cases where the game is doing time related shenanigans, or
		//you want to manually parse the scene. Note that it will occur immediately, even if the game is doing time logic, so be aware of any side effects that may cause.be 

		public static bool ChangeAreaSilent<T>() where T : AreaBase
		{
			return areaEngine.SetArea<T>();
		}

		public static bool ReturnToBaseSilent()
		{
			return areaEngine.ReturnToBase();
		}

		/// <summary>
		/// Attempts to unlock the current area, and retrieve any flavor text for unlocking if successful. If the location is already unlocked, it will return false, and the unlockText
		/// will be null. 
		/// </summary>
		/// <typeparam name="T">The type of the area to unlock.</typeparam>
		/// <param name="unlockText">the text that resulted from unlocking the area successfully, or null.</param>
		/// <returns>true if the area was unlocked, false if it was already unlocked</returns>
		public static bool UnlockArea<T>(out string unlockText) where T : VisitableAreaBase
		{
			return areaEngine.UnlockArea<T>(out unlockText);
		}


		public static void InitializeEngine(Func<DisplayBase> pageDataConstructor, Action<DisplayBase> displayPage,
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations,
			ReadOnlyDictionary<Type, Func<DungeonBase>> gameDungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> gameHomeBases, //Area Engine
			Func<Creature, BasePerkModifiers> perkVariables, //perk data for creatures to use. 
			ReadOnlyCollection<GameDifficulty> gameDifficulties, int defaultDifficulty) //Game Difficulty Collections.
		{
			areaEngine = new AreaEngine(pageDataConstructor, displayPage, gamePlaces, gameLocations, gameDungeons, gameHomeBases);
			timeEngine = new TimeEngine(pageDataConstructor, displayPage, areaEngine);

			difficulties = gameDifficulties ?? throw new ArgumentNullException(nameof(gameDifficulties));
			defaultDifficultyIndex = defaultDifficulty;
			constructPerkModifier = perkVariables ?? throw new ArgumentNullException(nameof(perkVariables));

			AreaBase.SetPageMaker(pageDataConstructor);
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

		public static bool UnlockAchievement<T>() where T : AchievementBase
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

		public static bool RegisterActiveListener(ITimeActiveListenerFull listener)
		{
			return timeEngine.fullActiveListeners.Add(listener);
		}

		public static bool DeregisterActiveListener(ITimeActiveListenerFull listener)
		{
			return timeEngine.fullActiveListeners.Remove(listener);
		}

		public static bool RegisterDailyListener(ITimeDailyListenerFull listener)
		{
			return timeEngine.fullDailyListeners.Add(listener);
		}

		public static bool DeregisterDailyListener(ITimeDailyListenerFull listener)
		{
			return timeEngine.fullDailyListeners.Remove(listener);
		}

		public static bool RegisterDayMultiListener(ITimeDayMultiListenerFull listener)
		{
			return timeEngine.fullMultiTimeListeners.Add(listener);
		}

		public static bool DeregisterDayMultiListener(ITimeDayMultiListenerFull listener)
		{
			return timeEngine.fullMultiTimeListeners.Remove(listener);
		}

		public static bool RegisterActiveListener(ITimeActiveListenerSimple listener)
		{
			return timeEngine.simpleActiveListeners.Add(listener);
		}

		public static bool DeregisterActiveListener(ITimeActiveListenerSimple listener)
		{
			return timeEngine.simpleActiveListeners.Remove(listener);
		}

		public static bool RegisterDailyListener(ITimeDailyListenerSimple listener)
		{
			return timeEngine.simpleDailyListeners.Add(listener);
		}

		public static bool DeregisterDailyListener(ITimeDailyListenerSimple listener)
		{
			return timeEngine.simpleDailyListeners.Remove(listener);
		}

		public static bool RegisterDayMultiListener(ITimeDayMultiListenerSimple listener)
		{
			return timeEngine.simpleMultiTimeListeners.Add(listener);
		}

		public static bool DeregisterDayMultiListener(ITimeDayMultiListenerSimple listener)
		{
			return timeEngine.simpleMultiTimeListeners.Remove(listener);
		}

		public static void AddOneOffReaction(OneOffTimeReactionBase reaction)
		{
			timeEngine.reactions.Push(reaction);
		}

		public static bool RemoveOneOffReaction(OneOffTimeReactionBase reaction)
		{
			return timeEngine.reactions.Remove(reaction);
		}

		public static bool HasOneOffReaction(OneOffTimeReactionBase reaction)
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

		public static void SetHomeBase<T>() where T : HomeBaseBase
		{
			areaEngine.ChangeHomeBase<T>();
		}
	}
}
