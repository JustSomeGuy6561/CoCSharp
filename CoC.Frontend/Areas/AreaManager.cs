using CoC.Backend.Areas;
using CoC.Frontend.Areas.HomeBases;
using CoC.Frontend.Areas.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Frontend.Areas
{
	internal static class AreaManager
	{
		private static readonly Dictionary<Type, Func<PlaceBase>> places = new Dictionary<Type, Func<PlaceBase>>();
		internal static readonly ReadOnlyDictionary<Type, Func<PlaceBase>> placeCollection = new ReadOnlyDictionary<Type, Func<PlaceBase>>(places);
		private static readonly Dictionary<Type, Func<LocationBase>> locations = new Dictionary<Type, Func<LocationBase>>();
		internal static readonly ReadOnlyDictionary<Type, Func<LocationBase>> locationCollection = new ReadOnlyDictionary<Type, Func<LocationBase>>(locations);

		private static readonly Dictionary<Type, Func<DungeonBase>> dungeons = new Dictionary<Type, Func<DungeonBase>>();
		internal static readonly ReadOnlyDictionary<Type, Func<DungeonBase>> dungeonCollection = new ReadOnlyDictionary<Type, Func<DungeonBase>>(dungeons);

		private static readonly Dictionary<Type, Func<HomeBaseBase>> homeBases = new Dictionary<Type, Func<HomeBaseBase>>();
		internal static readonly ReadOnlyDictionary<Type, Func<HomeBaseBase>> homeBaseCollection = new ReadOnlyDictionary<Type, Func<HomeBaseBase>>(homeBases);

		//add your areas here. 
		static AreaManager()
		{
			AddLocationHelper(() => new Forest());
			AddLocationHelper(() => new Deepwoods());



			AddHomeBaseHelper(() => new IngnamBase());
			AddHomeBaseHelper(() => new Camp());
		}

		private static void AddPlaceHelper<T>(Func<T> constructorCallback) where T : PlaceBase
		{
			if (constructorCallback is null) throw new ArgumentNullException(nameof(constructorCallback));
			//else if (constructorCallback() is null) throw new ArgumentException("constructor callback cannot return null");
			if (typeof(T).IsAbstract) throw new ArgumentException("Cannot add an abstract type to the list of possible places");
			places.Add(typeof(T), constructorCallback);
		}

		private static void AddLocationHelper<T>(Func<T> constructorCallback) where T : LocationBase
		{
			if (constructorCallback is null) throw new ArgumentNullException(nameof(constructorCallback));
			//else if (constructorCallback() is null) throw new ArgumentException("constructor callback cannot return null");
			if (typeof(T).IsAbstract) throw new ArgumentException("Cannot add an abstract type to the list of possible locations");
			locations.Add(typeof(T), constructorCallback);
		}

		private static void AddDungeonHelper<T>(Func<T> constructorCallback) where T : DungeonBase
		{
			if (constructorCallback is null) throw new ArgumentNullException(nameof(constructorCallback));
			//else if (constructorCallback() is null) throw new ArgumentException("constructor callback cannot return null");
			if (typeof(T).IsAbstract) throw new ArgumentException("Cannot add an abstract type to the list of possible dungeons");
			dungeons.Add(typeof(T), constructorCallback);
		}

		private static void AddHomeBaseHelper<T>(Func<T> constructorCallback) where T : HomeBaseBase
		{
			if (constructorCallback is null) throw new ArgumentNullException(nameof(constructorCallback));
			//else if (constructorCallback() is null) throw new ArgumentException("constructor callback cannot return null");
			if (typeof(T).IsAbstract) throw new ArgumentException("Cannot add an abstract type to the list of possible home bases");
			homeBases.Add(typeof(T), constructorCallback);
		}
	}
}
