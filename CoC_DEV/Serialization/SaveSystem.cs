//Serializer.cs
//Description:
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

namespace CoC.Serialization
{

	internal class SaveSurrogate : ISerializationSurrogateProvider
	{
		private readonly HashSet<SerializationBase> saveBases = new HashSet<SerializationBase>();
		//tells it what it's searching for when attempting to replace our save data with it's current iteration
		public Type GetSurrogateType(Type type)
		{
			foreach (SerializationBase save in saveBases)
			{
				Type t = save.GetType();
				if (type == save.GetType())
				{
					return save.currentSaveType;
				}
			}
			return type;
		}
		//tells our save data how to save itself. note that this will always use the most recent save version available.
		public object GetObjectToSerialize(object obj, Type targetType)
		{
			foreach (SerializationBase save in saveBases)
			{
				if (obj.GetType() == save.GetType())
				{
					return save.ToCurrentSaveVersion();
				}
			}
			return obj;
		}
		//tells us how to parse saved data, even if it was an old save. this will work for all save versions.
		public object GetDeserializedObject(object obj, Type targetType)
		{
			foreach (SerializationBase save in saveBases)
			{
				if (save.saveVersionTypes.Contains(obj.GetType()))
				{
					return save.FromSave(obj, obj.GetType());
				}
			}
			return obj;
		}



		public bool AddItem(SerializationBase saveInstance)
		{
			return saveBases.Add(saveInstance);
		}
	}

	//NOTE: This MUST have all the data created before first deserialization or it MAY
	//cause null reference errors, and will DEFINITELY fail to serialize everything

	//HOWEVER, if the game is run properly, this should never occur. 
	//NOTE: This requires creating the default serialization base children during the init functions
	//for backend/frontend/ui/whatever. if you do that, and then it attempts to load user settings, all will work fine.


	public static class SaveSystem
	{
		private static readonly Dictionary<Type, SerializationBase> sessionSaveData = new Dictionary<Type, SerializationBase>();
		private static readonly Dictionary<Type, SerializationBase> globalSaveData = new Dictionary<Type, SerializationBase>();
		private static readonly SaveSurrogate surrogate = new SaveSurrogate();
		public static string saveDirectory;
		public static string SETTINGS_LOCATION => Path.Combine(saveDirectory, "GlobalSettings.CoCSav");

		public static bool AddSessionSaveInstance(in SerializationBase sessionSave, Type sessionSaveType)
		{
			bool retVal = !sessionSaveData.ContainsKey(sessionSaveType);
			if (retVal)
			{
				sessionSaveData.Add(sessionSaveType, sessionSave);
				surrogate.AddItem(sessionSave);
			}
			return retVal;

		}

		public static bool AddGlobalSaveInstance(in SerializationBase globalSave, Type globalSaveType)
		{
			bool retVal = !globalSaveData.ContainsKey(globalSaveType);
			if (retVal)
			{
				globalSaveData.Add(globalSaveType, globalSave);
				surrogate.AddItem(globalSave);
			}
			return retVal;

		}
		/// <summary>
		/// Gets the serializebase derived class associated with that type
		/// </summary>
		/// <param name="serializableType"></param>
		/// <returns></returns>
		public static SerializationBase GetSessionDataFromType(Type serializableType)
		{
			if (sessionSaveData.ContainsKey(serializableType))
			{
				return sessionSaveData[serializableType];
			}
			return null;
		}

		public static SerializationBase GetGlobalDataFromType(Type serializableType)
		{
			if (globalSaveData.ContainsKey(serializableType))
			{
				return globalSaveData[serializableType];
			}
			return null;
		}

		//Should add File exists checks for loads to make them not throw and also allow for early early exit. 

		public static bool LoadSession(string fileName)
		{
			bool successful = false;
			string file = Path.Combine(saveDirectory, fileName);
			try
			{
				Type[] knownTypes = sessionSaveData.Keys.ToArray();
				SerializationBase[] members = sessionSaveData.Values.ToArray();
				SerializationBase[] newMembers;
				DataContractSerializer dcs = new DataContractSerializer(typeof(List<SerializationBase>), knownTypes);
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
					if (sessionSaveData.ContainsKey(member.GetType()))
					{
						sessionSaveData[member.GetType()] = member;
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
		public static bool SaveSession(string fileName)
		{
			bool successful = false;
			string file = Path.Combine(saveDirectory, fileName);
			try
			{
				Type[] knownTypes = sessionSaveData.Keys.ToArray();
				SerializationBase[] members = sessionSaveData.Values.ToArray();

				DataContractSerializer dcs = new DataContractSerializer(typeof(List<SerializationBase>), knownTypes);
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

		public static bool LoadGlobalSettings()
		{
			bool successful = false;
			string file = SETTINGS_LOCATION;
			try
			{
				Type[] knownTypes = globalSaveData.Keys.ToArray();
				SerializationBase[] members = globalSaveData.Values.ToArray();
				SerializationBase[] newMembers;
				DataContractSerializer dcs = new DataContractSerializer(typeof(List<SerializationBase>), knownTypes);
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
					if (globalSaveData.ContainsKey(member.GetType()))
					{
						globalSaveData[member.GetType()] = member;
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
				Type[] knownTypes = globalSaveData.Keys.ToArray();
				SerializationBase[] members = globalSaveData.Values.ToArray();

				DataContractSerializer dcs = new DataContractSerializer(typeof(List<SerializationBase>), knownTypes);
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

}