using CoC.Backend.Areas;
using CoC.Backend.Engine.Events;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Engine
{
	public sealed class AreaEngine
	{
		//these dictionaries pair a type and a function that returns a new instance of place or location.
		private readonly ReadOnlyDictionary<Type, Func<PlaceBase>> placeLookup;
		private readonly ReadOnlyDictionary<Type, Func<LocationBase>> locationLookup;
		//these dicionaries pair a type with a priority queue of reactions for that particular place or location
		//allows us to have these special reactions at a location, even if that location does not exist in memory right now.
		private readonly Dictionary<Type, PriorityQueue<PlaceReaction>> placeReactionStorage = new Dictionary<Type, PriorityQueue<PlaceReaction>>();
		private readonly Dictionary<Type, PriorityQueue<LocationReaction>> locationReactionStorage = new Dictionary<Type, PriorityQueue<LocationReaction>>();

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

		private AreaBase delayedArea;

		private bool areaChanged = false;

		internal AreaEngine(ReadOnlyDictionary<Type, Func<PlaceBase>> places, ReadOnlyDictionary<Type, Func<LocationBase>> locations)
		{
			placeLookup = places ?? throw new ArgumentNullException(nameof(places));
			locationLookup = locations ?? throw new ArgumentNullException(nameof(locations));
			foreach (var key in places.Keys)
			{
				placeReactionStorage.Add(key, new PriorityQueue<PlaceReaction>());
			}
			foreach (var key in locations.Keys)
			{
				locationReactionStorage.Add(key, new PriorityQueue<LocationReaction>());
			}
		}

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

		internal void AddReaction(LocationReaction locationReaction)
		{
			locationReactionStorage[locationReaction.targetLocation].Push(locationReaction);
		}

		internal void AddReaction(PlaceReaction placeReaction)
		{
			placeReactionStorage[placeReaction.targetPlace].Push(placeReaction);
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

		internal void RunArea()
		{
			Action ToDo = null;
			if (delayedArea != null)
			{
				//will automatically check if different and set areaChanged accordingly
				currentArea = delayedArea;
				delayedArea = null;
			}

			if (areaChanged)
			{
				currentArea.timesExplored++;
				currentArea.OnEnter();
			}
			else
			{
				currentArea.OnStay();
			}

			Type type = currentArea.GetType();
			if (currentArea is PlaceBase)
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

			if (ToDo == null)
			{
				ToDo = currentArea.RunArea;
			}

			if (areaChanged)
			{
				foreach (PlaceReaction data in placeReactionStorage[type])
				{
					data.VisitLocation();
				}

				foreach (LocationReaction data in locationReactionStorage[type])
				{
					data.VisitLocation();
				}

				areaChanged = false;
			}

			ToDo();
		}

		internal void SetArea<T>() where T : AreaBase
		{
			if (typeof(T).IsSubclassOf(typeof(LocationBase)))
			{
				currentArea = locationLookup[typeof(T)]();
			}
			else if (typeof(T).IsSubclassOf(typeof(PlaceBase)))
			{
				currentArea = placeLookup[typeof(T)]();
			}
			throw new ArgumentException("Type T must derive PlaceBase or LocationBase, and must be added to the Area Engine in its static constructor");
		}

		internal void SetAreaDelayed<T>() where T : AreaBase
		{
			if (typeof(T).IsSubclassOf(typeof(LocationBase)))
			{
				delayedArea = locationLookup[typeof(T)]();
			}
			else if (typeof(T).IsSubclassOf(typeof(PlaceBase)))
			{
				delayedArea = placeLookup[typeof(T)]();
			}
			throw new ArgumentException("Type T must derive PlaceBase or LocationBase, and must be added to the Area Engine in its static constructor");
		}
	}
}
