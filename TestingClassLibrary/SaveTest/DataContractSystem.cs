using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace TestingClassLibrary.SaveTest
{
	public static class DataContractSystem
	{
		internal static readonly HashSet<Type> knownTypes = new HashSet<Type>();
		internal static readonly HashSet<Type> allItemsWithSurrogates = new HashSet<Type>();
		internal static readonly HashSet<Type> allSurrogates = new HashSet<Type>(); //must be of the type ISurrogateClass<T>
		internal static readonly Dictionary<Type, Type> itemToCurrentSurrogateType = new Dictionary<Type, Type>();



		private static readonly SaveSurrogate surrogate = new SaveSurrogate();
		internal static DataContractSerializer getSerializer()
		{
			DataContractSerializer dcs = new DataContractSerializer(typeof(SaveableData[]), knownTypes);
			dcs.SetSerializationSurrogateProvider(surrogate);
			return dcs;
		}

		public static bool AddType(Type typeThatNeedsToBeKnown)
		{
			return knownTypes.Add(typeThatNeedsToBeKnown);
		}

		public static bool AddSurrogateData(ISaveableBase classWithSurrogates)
		{
			Type item = classWithSurrogates.GetType();
			if (allItemsWithSurrogates.Contains(item))
			{
				return false;
			}
			allItemsWithSurrogates.Add(item);

			knownTypes.Add(item);
			knownTypes.UnionWith(classWithSurrogates.saveVersionTypes);

			allSurrogates.UnionWith(classWithSurrogates.saveVersionTypes);

			itemToCurrentSurrogateType.Add(item, classWithSurrogates.currentSaveType);
			return true;
		}
	}
	internal class SaveSurrogate : ISerializationSurrogateProvider
	{
		//tells it what it's searching for when attempting to replace our save data with it's current iteration
		public Type GetSurrogateType(Type type)
		{
			//could one-line linq this probably, but meh.
			foreach (var save in DataContractSystem.itemToCurrentSurrogateType)
			{
				if (save.Key.IsAssignableFrom(type))
				{
					return save.Value;
				}
			}
			return type;
		}
		//tells our save data how to save itself. note that this will always use the most recent save version available.
		public object GetObjectToSerialize(object obj, Type targetType)
		{
			foreach (Type save in DataContractSystem.allItemsWithSurrogates)
			{
				if (obj.GetType() == save || obj.GetType().IsSubclassOf(save))
				{
					return ((ISaveableBase)obj).ToCurrentSaveVersion();
				}
			}
			return obj;
		}
		//tells us how to parse saved data, even if it was an old save. this will work for all save versions.
		public object GetDeserializedObject(object obj, Type targetType)
		{
			foreach (Type data in DataContractSystem.allSurrogates)
			{
				if (obj.GetType() == data || obj.GetType().IsSubclassOf(data))
				{
					return ((ISurrogateBase)obj).ToObject();
				}
			}
			return obj;
		}
	}
}
