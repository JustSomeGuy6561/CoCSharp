//SaveSystem.cs
//Description: The save system for the entire game. data can be added to this from any
//layer of this game engine, though data must be added to it during runtime, and before anything is saved or loaded.
//Author: JustSomeGuy
//1/30/2019, 10:11 PM
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		//useful for data lookup, serialization. needs to updated or data is lost. 
		internal static readonly Dictionary<Type, SaveableData> sessionData = new Dictionary<Type, SaveableData>();
		internal static readonly Dictionary<Type, SaveableData> globalData = new Dictionary<Type, SaveableData>();

		public static string saveDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public static string SETTINGS_LOCATION => Path.Combine(saveDirectory, "GlobalSettings.CoCSav");

		public static bool AddSessionSaveInstance(SaveableData sessionSave)
		{
			if (sessionData.ContainsKey(sessionSave.GetType()))
			{
				return false;
			}
			sessionData[sessionSave.GetType()] = sessionSave;
			DataContractSystem.AddSurrogateData(sessionSave);
			return true;
		}

		public static bool AddGlobalSaveInstance(SaveableData globalSave)
		{
			if (globalData.ContainsKey(globalSave.GetType()))
			{
				return false;
			}
			globalData[globalSave.GetType()] = globalSave;
			DataContractSystem.AddSurrogateData(globalSave);
			return true;
		}

		/// <summary>
		/// Gets the serializebase derived class associated with that type
		/// </summary>
		/// <param name="serializableType"></param>
		/// <returns></returns>
		public static T GetSessionData<T>() where T : SaveableData
		{
			if (sessionData.ContainsKey(typeof(T)))
			{
				return (T)sessionData[typeof(T)];
			}
			return null;
		}

		public static T GetGlobalData<T>() where T : SaveableData
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
			SaveableData[] members = sessionData.Values.ToArray();
			SaveableData[] newMembers;
			DataContractSerializer dcs = DataContractSystem.getSerializer();
			using (XmlReader reader = XmlReader.Create(file))
			{
				newMembers = (SaveableData[])dcs.ReadObject(reader);
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
			foreach (SaveableData member in newMembers)
			{
				if (sessionData.ContainsKey(member.GetType()))
				{
					sessionData[member.GetType()] = member;
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
			SaveableData[] members = sessionData.Values.ToArray();
			DataContractSerializer dcs = DataContractSystem.getSerializer();
			DataContractSerializerSettings dataContractSerializerSettings = new DataContractSerializerSettings()
			{
				a
			}
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
				SaveableData[] members = globalData.Values.ToArray();
				SaveableData[] newMembers;
				DataContractSerializer dcs = DataContractSystem.getSerializer();
				using (XmlReader reader = XmlReader.Create(file))
				{
					newMembers = (SaveableData[])dcs.ReadObject(reader);
				}
#if DEBUG
				SaveableData[] missingMembers = members.Except(newMembers).ToArray();
				if (missingMembers.Length > 0)
				{
					Debug.WriteLine("Warning: there are global setting types that were either not found or not parsed correctly. they are as follows:");
					Array.ForEach(missingMembers, x => Console.WriteLine(x.GetType().ToString()));
				}
#endif
				//override the default saves with the new, loaded data. 
				foreach (SaveableData member in newMembers)
				{
					if (globalData.ContainsKey(member.GetType()))
					{
						globalData[member.GetType()] = member;
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
				SaveableData[] members = globalData.Values.ToArray();
				DataContractSerializer dcs = DataContractSystem.getSerializer();
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