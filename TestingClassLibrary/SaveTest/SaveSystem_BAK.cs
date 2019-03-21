//SaveSystem.cs
//Description: The save system for the entire game. data can be added to this from any
//layer of this game engine. 
//Author: JustSomeGuy
//1/30/2019, 10:11 PM
using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace TestingClassLibrary.SaveTest
{
	//NOTE: This MUST have all the data created before first deserialization or it MAY
	//cause null reference errors, and will DEFINITELY fail to serialize everything

	//HOWEVER, if the game is run properly, this should never occur. 
	//NOTE: This requires creating the default serialization base children during the init functions
	//for backend/frontend/ui/whatever. if you do that, and then it attempts to load user settings, all will work fine.


	public static class SaveSystem
	{
		internal static readonly Dictionary<Type, Type> reverseLookup = new Dictionary<Type, Type>();
		//useful for data lookup, serialization. needs to updated or data is lost. 
		internal static readonly Dictionary<Type, SerializationBase> sessionData = new Dictionary<Type, SerializationBase>();
		internal static readonly Dictionary<Type, SerializationBase> globalData = new Dictionary<Type, SerializationBase>();
		internal static readonly Dictionary<Type, SerializationBase> saveObjects = new Dictionary<Type, SerializationBase>();

		private static readonly SaveSurrogate surrogate = new SaveSurrogate();
		private static Type[] knownTypes => reverseLookup.Keys.ToArray();

		public static string saveDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public static string SETTINGS_LOCATION => Path.Combine(saveDirectory, "GlobalSettings.CoCSav");

		public static bool AddSessionSaveInstance(SerializationBase sessionSave, Type sessionType)
		{
			if (sessionData.ContainsKey(sessionType))
			{
				return false;
			}
			sessionData[sessionType] = sessionSave;
			AddInstance(sessionSave, sessionType);
			return true;
		}

		public static bool AddGlobalSaveInstance(SerializationBase globalSave, Type type)
		{
			if (globalData.ContainsKey(type))
			{
				return false;
			}
			globalData[type] = globalSave;
			AddInstance(globalSave, type);
			return true;
		}

		private static void AddInstance(SerializationBase saveData, Type type)
		{
			saveObjects[type] = saveData;
			Type[] surrogates = saveData.saveVersionTypes;
			Array.ForEach(surrogates, x => reverseLookup.Add(x, type));
		}
		/// <summary>
		/// Gets the serializebase derived class associated with that type
		/// </summary>
		/// <param name="serializableType"></param>
		/// <returns></returns>
		public static T GetSessionData<T>() where T : SerializationBase
		{
			if (sessionData.ContainsKey(typeof(T)))
			{
				return (T)sessionData[typeof(T)];
			}
			return null;
		}

		public static T GetGlobalData<T>() where T : SerializationBase
		{
			if (globalData.ContainsKey(typeof(T)))
			{
				return (T)globalData[typeof(T)];
			}
			return null;
		}

		//Should add File exists checks for loads to make them not throw and also allow for early early exit. 

		public static void LoadSession(string fileName)
		{
			//surrogate.workaround = true;
			string file = Path.Combine(saveDirectory, fileName);
			Type[] types = knownTypes.Union(saveObjects.Keys.ToArray()).ToArray();
			SerializationBase[] members = sessionData.Values.ToArray();
			SerializationBase[] newMembers;
			DataContractSerializer dcs = new DataContractSerializer(typeof(SerializationBase[]), types);
			dcs.SetSerializationSurrogateProvider(surrogate);
			using (XmlReader reader = XmlReader.Create(file))
			{
				newMembers = (SerializationBase[])dcs.ReadObject(reader);
			}
#if DEBUG
			Type[] missingTypes = sessionData.Keys.Except(Array.ConvertAll(newMembers, x => x.GetType())).ToArray();
			//surrogate.workaround = false;
			if (missingTypes.Length > 0)
			{
				Debug.WriteLine("Warning: there are session setting types that were either not found or not parsed correctly. they are as follows:");
				Array.ForEach(missingTypes, x => Debug.WriteLine(x.ToString()));
			}
#endif
			//override the default saves with the new, loaded data. 
			foreach (SerializationBase member in newMembers)
			{
				if (sessionData.ContainsKey(member.GetType()))
				{
					sessionData[member.GetType()] = member;
					saveObjects[member.GetType()] = member;
				}
#if DEBUG
				else
				{
					Debug.WriteLine("Warning: Type " + member.GetType() + " found, but was not expected. it will be ignored.This is probably an unintentional error somewhere, though may be due to obsolete"
						+ "code. Please make sure this is either part of the init functions or this legacy code in an old save file.");
				}
#endif
			}
		}
		public static bool SaveSession(string fileName)
		{
			bool successful = false;
			string file = Path.Combine(saveDirectory, fileName);
			SerializationBase[] members = sessionData.Values.ToArray();
			DataContractSerializer dcs = new DataContractSerializer(typeof(SerializationBase[]), knownTypes);
			dcs.SetSerializationSurrogateProvider(surrogate);
			using (XmlWriter writer = XmlWriter.Create(file))
			{
				dcs.WriteObject(writer, members);
			}
			successful = true;
			return successful;
		}

		public static bool LoadGlobalSettings()
		{
			bool successful = false;
			string file = SETTINGS_LOCATION;
			try
			{
				Type[] knownTypes = globalData.Keys.ToArray();
				SerializationBase[] members = globalData.Values.ToArray();
				SerializationBase[] newMembers;
				DataContractSerializer dcs = new DataContractSerializer(typeof(SerializationBase[]), knownTypes);
				dcs.SetSerializationSurrogateProvider(surrogate);
				using (XmlReader reader = XmlReader.Create(file))
				{
					newMembers = (SerializationBase[])dcs.ReadObject(reader);
				}
#if DEBUG
				SerializationBase[] missingMembers = members.Except(newMembers).ToArray();
				if (missingMembers.Length > 0)
				{
					Debug.WriteLine("Warning: there are global setting types that were either not found or not parsed correctly. they are as follows:");
					Array.ForEach(missingMembers, x => Console.WriteLine(x.GetType().ToString()));
				}
#endif
				//override the default saves with the new, loaded data. 
				foreach (SerializationBase member in newMembers)
				{
					if (globalData.ContainsKey(member.GetType()))
					{
						globalData[member.GetType()] = member;
						saveObjects[member.GetType()] = member;
					}
#if DEBUG
					else
					{
						Debug.WriteLine("Warning: Type " + member.GetType() + " found, but was not expected. it will be ignored.This is probably an unintentional error somewhere, though may be due to obsolete"
							+ "code. Please make sure this is either part of the init functions or this legacy code in an old save file.");
					}
#endif

				}
				successful = true;
			}
			catch (Exception)
			{
				successful = false;
			}
			return successful;
		}
		public static bool SaveGlobalSettings()
		{
			bool successful = false;
			string file = SETTINGS_LOCATION;
			try
			{
				Type[] knownTypes = globalData.Keys.ToArray();
				SerializationBase[] members = globalData.Values.ToArray();

				DataContractSerializer dcs = new DataContractSerializer(typeof(SerializationBase[]), knownTypes);
				dcs.SetSerializationSurrogateProvider(surrogate);
				using (XmlWriter writer = XmlWriter.Create(file))
				{
					dcs.WriteObject(writer, members);
				}
				successful = true;
			}
			catch (Exception)
			{
				successful = false;
			}
			return successful;
		}
	}

	internal class SaveSurrogate : ISerializationSurrogateProvider
	{
		//tells it what it's searching for when attempting to replace our save data with it's current iteration
		public Type GetSurrogateType(Type type)
		{
			//could one-line linq this probably, but meh.
			foreach (KeyValuePair<Type, SerializationBase> save in SaveSystem.saveObjects)
			{
				if (save.Key.IsAssignableFrom(type))
				{
					return save.Value.currentSaveType;
				}
			}
			return type;
		}
		//tells our save data how to save itself. note that this will always use the most recent save version available.
		public object GetObjectToSerialize(object obj, Type targetType)
		{
			foreach (KeyValuePair<Type, SerializationBase> save in SaveSystem.saveObjects)
			{
				if (obj.GetType() == save.Key || obj.GetType().IsSubclassOf(save.Key))
				{
					object retVal = save.Value.ToCurrentSaveVersion();
					targetType = retVal.GetType();
					return retVal;
				}
			}
			return obj;
		}
		//tells us how to parse saved data, even if it was an old save. this will work for all save versions.
		public object GetDeserializedObject(object obj, Type targetType)
		{
			foreach (Type surrogate in SaveSystem.reverseLookup.Keys)
			{
				if (obj.GetType() == surrogate || obj.GetType().IsSubclassOf(surrogate))
				{
					object retVal = SaveSystem.saveObjects[SaveSystem.reverseLookup[surrogate]].FromSave(obj, obj.GetType());
					targetType = retVal.GetType();
					return retVal;
				}
			}
			return obj;
		}
	}
}