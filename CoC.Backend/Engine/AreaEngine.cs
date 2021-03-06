﻿using CoC.Backend.Areas;
using CoC.Backend.Reaction;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Engine
{
	//will need OutputText, Button Manager for HomeBase, DungeonBase.
	//dungeon needs to add east, west, north, and south buttons, with masturbate, inventory.
	//home base needs to basically set up everything for a home base - buttons for inventory, places, explore, stash, slaves/followers/lovers, masturbate, rest.
	//the only thing that is unique to each location is its camp actions, which would probably still need a button at engine level, just with an abstract onClick and Enabled
	//flag. if we go this route, it'd also be smart to simply make the unlock area/place/dungeon functions done internally, with output text of their unlock text (which
	//would now be abstract) there would still be a virtual function for onUnlock if they need to do anything else.


	public sealed class AreaEngine
	{
		//these dictionaries pair a type and a function that returns a new instance of place or location.
		private readonly ReadOnlyDictionary<Type, Func<PlaceBase>> placeLookup;
		private readonly ReadOnlyDictionary<Type, Func<LocationBase>> locationLookup;
		private readonly ReadOnlyDictionary<Type, Func<DungeonBase>> dungeonLookup;

		private readonly ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBaseLookup;

		//these dicionaries pair a type with a priority queue of reactions for that particular place or location
		//allows us to have these special reactions at a location, even if that location does not exist in memory right now.
		private readonly Dictionary<Type, PriorityQueue<PlaceReaction>> placeReactionStorage = new Dictionary<Type, PriorityQueue<PlaceReaction>>();
		private readonly Dictionary<Type, PriorityQueue<LocationReaction>> locationReactionStorage = new Dictionary<Type, PriorityQueue<LocationReaction>>();
		private readonly PriorityQueue<HomeBaseReaction> homeBaseReactions = new PriorityQueue<HomeBaseReaction>();

		private readonly Func<DisplayBase> pageMaker;
		private readonly Action<DisplayBase> SetCurrentPage;
		private readonly Func<DisplayBase> GetCurrentPage;

		private int currentLanguage;

		private readonly Dictionary<Type, string> unlockedPlaces = new Dictionary<Type, string>();
		private readonly Dictionary<Type, string> unlockedLocations = new Dictionary<Type, string>();
		private readonly Dictionary<Type, string> unlockedDungeons = new Dictionary<Type, string>();

#warning if treating areas within places as places, this areaChanged logic will fail. figure out way to fix.

		internal AreaBase currentArea
		{
			get => _currentArea;
			private set
			{
				if (_currentArea != value)
				{
					areaChanged = true;
					_currentArea = value;
				}
			}
		}
		private AreaBase _currentArea;

		private bool areaChanged = false;

		//defined during a load of file or new game. obtained by the current difficulty.
		internal HomeBaseBase currentHomeBase { get; private set; }

		internal AreaEngine(Func<DisplayBase> pageDataConstructor, Func<DisplayBase> currentPageGetter, Action<DisplayBase> currentPageSetter,
			ReadOnlyDictionary<Type, Func<PlaceBase>> places, ReadOnlyDictionary<Type, Func<LocationBase>> locations,
			ReadOnlyDictionary<Type, Func<DungeonBase>> dungeons, ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBases)
		{
			pageMaker = pageDataConstructor ?? throw new ArgumentNullException(nameof(pageDataConstructor));
			GetCurrentPage = currentPageGetter ?? throw new ArgumentNullException(nameof(currentPageGetter));
			SetCurrentPage = currentPageSetter ?? throw new ArgumentNullException(nameof(currentPageSetter));
			placeLookup = places ?? throw new ArgumentNullException(nameof(places));
			locationLookup = locations ?? throw new ArgumentNullException(nameof(locations));
			dungeonLookup = dungeons ?? throw new ArgumentNullException(nameof(dungeons));
			homeBaseLookup = homeBases ?? throw new ArgumentNullException(nameof(homeBases));

			foreach (var key in places.Keys)
			{
				placeReactionStorage.Add(key, new PriorityQueue<PlaceReaction>());
			}
			foreach (var key in locations.Keys)
			{
				locationReactionStorage.Add(key, new PriorityQueue<LocationReaction>());
			}
			//dungeons don't have interrupts.
		}

		internal void OnNewGame()
		{
			currentLanguage = LanguageEngine.currentLanguageIndex;
			Exploration.InitializeAreas(GetCurrentPage, locationLookup.Values);
		}

		internal void ForceReload()
		{
			if (!(currentArea is HomeBaseBase))
			{
				currentArea = currentHomeBase;
				((HomeBaseBase)currentArea).OnReload();
			}
		}

		#region Change Location

		internal bool SetArea(Type type)
		{
			AreaBase prevArea = currentArea;
			if (type.IsSubclassOf(typeof(LocationBase)))
			{
				currentArea = locationLookup[type]();
			}
			else if (type.IsSubclassOf(typeof(PlaceBase)))
			{
				currentArea = placeLookup[type]();
			}
			else if (type.IsSubclassOf(typeof(DungeonBase)))
			{
				currentArea = dungeonLookup[type]();
			}
			else if (type == currentHomeBase.GetType())
			{
				currentArea = currentHomeBase;
			}
			else
			{
				throw new ArgumentException("type must derive PlaceBase, DungeonBase, or LocationBase, or must be the current HomeBase. Additionally, it must be be properly initialized so the Game Engine knows of it.");
			}
			return prevArea != currentArea;
		}

		internal bool SetArea<T>() where T : AreaBase
		{
			return SetArea(typeof(T));
		}

		internal bool ReturnToBase()
		{
			bool areaChanged = currentArea != currentHomeBase;
			currentArea = currentHomeBase;
			return areaChanged;
		}

		//only should be called during creation or loading a save. I suppose this could also happen during prologue => normal gameplay.
		internal void ChangeHomeBase<T>() where T : HomeBaseBase
		{
			if (!homeBaseLookup.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("HomeBase was not initialized into the list of possible home bases. Please do so.");
			}
			else
			{
				currentHomeBase = homeBaseLookup[typeof(T)]();
			}
		}
		#endregion

		#region Check And Get Data
		internal T GetPlace<T>() where T : PlaceBase
		{
			if (!placeLookup.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("Place was not initialized into the list of possible places. Please do so.");
			}
			else
			{
				return (T)placeLookup[typeof(T)]();
			}
		}

		internal bool HasPlace<T>() where T : PlaceBase
		{
			return placeLookup.ContainsKey(typeof(T));
		}

		internal T GetLocation<T>() where T : LocationBase
		{
			if (!locationLookup.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("Location was not initialized into the list of possible location. Please do so.");
			}
			else
			{
				return (T)locationLookup[typeof(T)]();
			}
		}

		internal bool HasLocation<T>() where T : LocationBase
		{
			return locationLookup.ContainsKey(typeof(T));
		}
		#endregion

		#region Reactions
		internal void AddReaction(LocationReaction locationReaction)
		{
			locationReactionStorage[locationReaction.targetLocation].Push(locationReaction);
		}

		internal void AddReaction(PlaceReaction placeReaction)
		{
			placeReactionStorage[placeReaction.targetPlace].Push(placeReaction);
		}

		internal void AddReaction(HomeBaseReaction homeReaction)
		{
			homeBaseReactions.Push(homeReaction);
		}

		internal bool RemoveReaction(LocationReaction locationReaction)
		{
			Type type = locationReaction.targetLocation;
			if (!locationReactionStorage.ContainsKey(type))
			{
				return false;
			}
			return locationReactionStorage[type].Remove(locationReaction);
		}

		internal bool RemoveReaction(PlaceReaction placeReaction)
		{
			Type type = placeReaction.targetPlace;
			if (!placeReactionStorage.ContainsKey(type))
			{
				return false;
			}
			return placeReactionStorage[type].Remove(placeReaction);
		}

		internal bool RemoveReaction(HomeBaseReaction homeReaction)
		{
			return homeBaseReactions.Remove(homeReaction);
		}

		internal bool HasReaction(LocationReaction locationReaction)
		{
			Type type = locationReaction.targetLocation;
			if (!locationReactionStorage.ContainsKey(type))
			{
				return false;
			}
			return locationReactionStorage[type].Contains(locationReaction);
		}

		internal bool HasReaction(PlaceReaction placeReaction)
		{
			Type type = placeReaction.targetPlace;
			if (!placeReactionStorage.ContainsKey(type))
			{
				return false;
			}
			return placeReactionStorage[type].Contains(placeReaction);
		}

		internal bool HasReaction(HomeBaseReaction homeReaction)
		{
			return homeBaseReactions.Contains(homeReaction);
		}
		#endregion

		internal void RunArea()
		{
			Action ToDo = null;
			if (currentArea is VisitableAreaBase visitable && areaChanged)
			{
				visitable.timesVisited++;
			}

			if (areaChanged)
			{
				currentArea.OnEnter();
			}
			else
			{
				currentArea.OnStay();
			}


			//get the current area type. if it exists in the current lookups, check if any special reactions occured.
			Type type = currentArea.GetType();
			if (currentArea is PlaceBase && !placeReactionStorage[type].isEmpty)
			{
				var queue = placeReactionStorage[type];
				if (queue.Peek().timesToVisitUntilProccing <= 0)
				{
					ToDo = queue.Pop().onTrigger;
				}
			}
			else if (currentArea is LocationBase && !locationReactionStorage[type].isEmpty)
			{
				var queue = locationReactionStorage[type];
				if (queue.Peek().timesToVisitUntilProccing <= 0)
				{
					ToDo = queue.Pop().onTrigger;
				}
			}
			else if (currentArea is HomeBaseBase && !homeBaseReactions.isEmpty)
			{
				if (homeBaseReactions.Peek().timesToVisitUntilProccing <= 0)
				{
					ToDo = () => homeBaseReactions.Pop().onTrigger(false);
				}
			}

			//if no special reactions occured,
			if (ToDo == null)
			{
				ToDo = currentArea.RunArea;
			}
			//decrement the reaction counters for applicable area.
			if (areaChanged)
			{
				if (currentArea is PlaceBase)
				{
					foreach (PlaceReaction data in placeReactionStorage[type])
					{
						data.VisitLocation();
					}
				}
				else if (currentArea is LocationBase)
				{
					foreach (LocationReaction data in locationReactionStorage[type])
					{
						data.VisitLocation();
					}
				}
				else if (currentArea is HomeBaseBase)
				{
					foreach (HomeBaseReaction data in homeBaseReactions)
					{
						data.OnVisit();
					}
				}
				areaChanged = false;
			}
			ToDo();
		}

		//cannot unlock a base camp - hence this distinction.
		internal bool UnlockArea<T>(out string unlockText) where T : VisitableAreaBase
		{
			return UnlockArea(typeof(T), out unlockText);
		}

		internal bool UnlockArea(Type type, out string unlockText)
		{
			VisitableAreaBase area = null;
			bool location = false, place = false, dungeon = false;
			if (type.IsSubclassOf(typeof(LocationBase)))
			{
				area = locationLookup[type]();
				location = true;
			}
			else if (type.IsSubclassOf(typeof(PlaceBase)))
			{
				area = placeLookup[type]();
				place = true;
			}
			else if (type.IsSubclassOf(typeof(DungeonBase)))
			{
				area = dungeonLookup[type]();
				dungeon = true;
			}
			//find it via place, location, dungeon lookup.
			if (area is null || area.isUnlocked)
			{
				unlockText = null;
				return false;
			}

			if (location)
			{
				unlockedLocations.Add(type, area.name());
			}
			else if (place)
			{
				unlockedPlaces.Add(type, area.name());
			}
			else if (dungeon)
			{
				unlockedDungeons.Add(type, area.name());
			}
			unlockText = area.Unlock();
			return true;
		}
		public bool anyUnlockedPlaces => unlockedPlaces.Count > 0;

		public ReadOnlyDictionary<Type, string> GetUnlockedPlaces()
		{
			CheckLanguage();
			return new ReadOnlyDictionary<Type, string>(unlockedPlaces);
		}

		public bool anyUnlockedLocations => unlockedLocations.Count > 0;

		public ReadOnlyDictionary<Type, string> GetUnlockedLocations()
		{
			CheckLanguage();
			return new ReadOnlyDictionary<Type, string>(unlockedLocations);
		}

		public bool anyUnlockedDungeons => unlockedDungeons.Count > 0;

		public ReadOnlyDictionary<Type, string> GetUnlockedDungeons()
		{
			CheckLanguage();
			return new ReadOnlyDictionary<Type, string>(unlockedDungeons);
		}



		private void CheckLanguage()
		{
			if (LanguageEngine.currentLanguageIndex != currentLanguage)
			{
				UpdateSource(unlockedLocations, x => locationLookup[x]().name());
				UpdateSource(unlockedPlaces, x => placeLookup[x]().name());
				UpdateSource(unlockedDungeons, x => dungeonLookup[x]().name());
				currentLanguage = LanguageEngine.currentLanguageIndex;
			}
		}

		private void UpdateSource(Dictionary<Type, string> lookup, Func<Type, string> getText)
		{
			foreach (var key in lookup.Keys)
			{
				lookup[key] = getText(key);
			}
		}
	}
}
