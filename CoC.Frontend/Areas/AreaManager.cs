using CoC.Backend.Areas;
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
		
		//add your areas here. 
		static AreaManager()
		{
			AddLocationHelper(() => new Forest());
			AddLocationHelper(() => new Deepwoods());
		}

		private static KeyValuePair<Type, Func<PlaceBase>> AddPlaceHelper<T>(Func<T> constructorCallback) where T : PlaceBase
		{
			return new KeyValuePair<Type, Func<PlaceBase>>(typeof(T), constructorCallback);
		}

		private static KeyValuePair<Type, Func<LocationBase>> AddLocationHelper<T>(Func<T> constructorCallback) where T : LocationBase
		{
			return new KeyValuePair<Type, Func<LocationBase>>(typeof(T), constructorCallback);
		}

	}
}
