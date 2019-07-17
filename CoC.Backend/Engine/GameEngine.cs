//GameEngine.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using CoC.Backend.Areas;
using CoC.Backend.Creatures;
using CoC.Backend.Engine.Events;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace CoC.Backend.Engine
{
	public static class GameEngine
	{
		public static AreaBase currentLocation => areaEngine.currentArea;

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

		public static ReadOnlyCollection<GameDifficulty> difficulties; 

		//Time
		private static TimeEngine timeEngine;
		//areas.
		private static AreaEngine areaEngine;
		//only for testing.
		//internal static AreaEngine areaEngine;

		internal static Func<BasePerkModifiers> constructPerkModifier;

		internal static ReadOnlyDictionary<Type, Func<PerkBase>> perkList;

		public static byte CurrentHour => timeEngine.CurrentHour;
		public static int CurrentDay => timeEngine.CurrentDay;

		//Time flow follows video game logic - when you're doing some scene, time is stopped.
		//when you're done, hours pass almost instantly, until you're at the expected time. This means you don't have bebes during combat or while visiting town, etc.
		//Is that how reality works? no. but that's honestly how we expect the game to work.  

		//There are some instances (mostly "Wait" at camp) where you actually want time to flow naturally. The "Use" Functions are not interruptable - they act like you 
		//used up those hours doing whatever it was you did, and no one could bother you while you were doing it. The "Idle" Functions, however, can be interrupted.
		//Note that an Idle could be interrupted many times, or none at all. Additionally, in some extreme cases, an interrupt could cause the player to change location or 
		//cancel any further idleing. 
		//Due to the fluid nature of idling, we've provided a means for you to "resume" your actions after an interrupt has occured. For the most part, this is just so you can
		//write some flavor text (for example, "you lay back down, intent to get your remaining <x> hours of rest"), but you have the ability to do practically anything i guess.
		//These callbacks are given the number of hours passed and the current location the player is at, so you can handle any weird cases where the player left or whatever. 

		public static void UseHours(byte hours)
		{
			timeEngine.UseHours(hours);
		}

		public static void GoToLocationThenUseHours<T>(byte hours) where T: AreaBase
		{
			areaEngine.SetArea<T>();
			timeEngine.UseHours(hours);
		}

		public static void UseHoursThenGoToLocation<T>(byte hours) where T:AreaBase
		{
			areaEngine.SetAreaDelayed<T>();
			timeEngine.UseHours(hours);
		}

		public static void IdleForHours(byte hours, ResumeTimeCallback resumeCallback)
		{
			timeEngine.IdleHours(hours, resumeCallback);
		}

		public static void IdleForHoursThenGoToLocation<T>(byte hours, ResumeTimeCallback resumeCallback) where T: AreaBase
		{
			areaEngine.SetAreaDelayed<T>();
			timeEngine.IdleHours(hours, resumeCallback);
		}

		public static void GoToLocationThenIdleForHours<T>(byte hours, ResumeTimeCallback resumeCallback) where T:AreaBase
		{
			areaEngine.SetArea<T>();
			timeEngine.IdleHours(hours, resumeCallback);
		}

		public static void InitializeEngine(
			Action<Action> DoNext, Action<string> OutputText, //Time Engine
			ReadOnlyDictionary<Type, Func<PlaceBase>> gamePlaces, ReadOnlyDictionary<Type, Func<LocationBase>> gameLocations, //AreaEngine
			ReadOnlyCollection<GameDifficulty> gameDifficulties) //Game Difficulty Collections.
		{
			areaEngine = new AreaEngine(gamePlaces, gameLocations);
			timeEngine = new TimeEngine(OutputText, DoNext, areaEngine);
			difficulties = gameDifficulties;
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
	}
}
