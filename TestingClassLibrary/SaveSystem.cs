using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Diagnostics;
namespace TestingClassLibrary
{
	public static class SaveSystem
	{
		private static readonly Dictionary<Type, SaveData2Test> sessionData = new Dictionary<Type, SaveData2Test>();
		private static readonly Dictionary<Type, SaveData2Test> globalData = new Dictionary<Type, SaveData2Test>();

		private static SaveData2Test[] allData => sessionData.Values.Union(globalData.Values).ToArray();
		private static readonly HashSet<Type> knownTypes = new HashSet<Type>();
		private static bool dataReady = true;

		//may in future allow this to be set via command line. probably would still be readonly, but whatever. 
#pragma warning disable IDE0044 // Add readonly modifier
		private static string saveDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#pragma warning restore IDE0044 // Add readonly modifier
		private static string SETTINGS_LOCATION => Path.Combine(saveDirectory, "GlobalSettings.CoCSav");


		public static bool AddSessionSaveInstance(SaveData2Test sessionSave)
		{
			if (sessionData.ContainsKey(sessionSave.GetType()))
			{
				return false;
			}
			sessionData[sessionSave.GetType()] = sessionSave;
			dataReady = false;
			return true;
		}

		public static bool AddGlobalSaveInstance(SaveData2Test globalSave)
		{
			if (globalData.ContainsKey(globalSave.GetType()))
			{
				return false;
			}
			globalData[globalSave.GetType()] = globalSave;
			dataReady = false;
			return true;
		}

		public static T GetSessionData<T>() where T : SaveData2Test
		{
			if (sessionData.ContainsKey(typeof(T)))
			{
				return (T)sessionData[typeof(T)];
			}
			throw new InvalidOperationException("The session data you are looking for was never added to the save system and was thus never initialized.");
		}

		public static T GetGlobalData<T>() where T : SaveData2Test
		{
			if (globalData.ContainsKey(typeof(T)))
			{
				return (T)globalData[typeof(T)];
			}
			throw new InvalidOperationException("The global data you are looking for was never added to the save system and was thus never initialized");
		}
		//ideally only runs once, but is called whenver the data is not correctly memoized and needs to be serialized/deserialized. realistically, if it's called more than once, shit hit the fan.
		//that means you added something after the init phase, which is a huge no-no as some data may not have been loaded (or saved, for that matter) properly.
		private static void Init()
		{
			//set up all the assemblies we can use for known types. start with this assembly.
			HashSet<Assembly> assemblies = new HashSet<Assembly>
			{
				Assembly.GetAssembly(typeof(SaveSystem))
			};
			//add the assemblies where the session and global data are defined.
			Array.ForEach(allData, (x) => assemblies.Add(x.definedAssembly));
			//add all the types in these assemblies.
			foreach (Assembly assembly in assemblies)
			{
				Type[] assemblyTypes = assembly.GetTypes();
				Type[] types = assemblyTypes.Where((x) => x.Attributes.HasFlag(TypeAttributes.Serializable) || x.GetCustomAttribute<DataContractAttribute>() != null).ToArray();
				Array.ForEach(types, Console.WriteLine);
				knownTypes.UnionWith(types);
			}
			//add any additional known types that may have been defined outside these assemblies.
			//these must be defined manually, but i think these are so uncommon as to not be an issue.
			Array.ForEach(allData, (x) => { if (x.externalTypes != null) knownTypes.UnionWith(x.externalTypes); });
			dataReady = true;
		}

		public static void LoadSession(string fileName)
		{
			if (!dataReady)
			{
				Init();
			}
			//surrogate.workaround = true;
			string file = Path.Combine(saveDirectory, fileName);
			SaveData2Test[] members = sessionData.Values.ToArray();
			SaveData2Test[] newMembers;
			DataContractSerializer dcs = GetSerializer();
			using (XmlReader reader = XmlReader.Create(file))
			{
				newMembers = (SaveData2Test[])dcs.ReadObject(reader);
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
			foreach (SaveData2Test member in newMembers)
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
			if (!dataReady)
			{
				Init();
			}
			bool successful = false;
			string file = Path.Combine(saveDirectory, fileName);
			SaveData2Test[] members = sessionData.Values.ToArray();
			DataContractSerializer dcs = GetSerializer();
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
			using (XmlWriter writer = XmlWriter.Create(file, settings))
			{
				dcs.WriteObject(writer, members);
			}
			successful = true;
			return successful;
		}

		public static bool LoadGlobalSettings()
		{
			if (!dataReady)
			{
				Init();
			}
			bool successful = false;
			string file = SETTINGS_LOCATION;
			try
			{
				SaveData2Test[] members = globalData.Values.ToArray();
				SaveData2Test[] newMembers;
				DataContractSerializer dcs = GetSerializer();
				using (XmlReader reader = XmlReader.Create(file))
				{
					newMembers = (SaveData2Test[])dcs.ReadObject(reader);
				}
#if DEBUG
				SaveData2Test[] missingMembers = members.Except(newMembers).ToArray();
				if (missingMembers.Length > 0)
				{
					Debug.WriteLine("Warning: there are global setting types that were either not found or not parsed correctly. they are as follows:");
					Array.ForEach(missingMembers, x => Console.WriteLine(x.GetType().ToString()));
				}
#endif
				//override the default saves with the new, loaded data. 
				foreach (SaveData2Test member in newMembers)
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
			if (!dataReady)
			{
				Init();
			}
			bool successful = false;
			string file = SETTINGS_LOCATION;
			try
			{
				SaveData2Test[] members = globalData.Values.ToArray();
				DataContractSerializer dcs = GetSerializer();
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

		private static DataContractSerializer GetSerializer()
		{
			DataContractSerializerSettings dcss = new DataContractSerializerSettings()
			{
				SerializeReadOnlyTypes = true,
				KnownTypes = knownTypes
			};
			DataContractSerializer dcs = new DataContractSerializer(typeof(SaveData2Test[]), dcss);
			//dcs.SetSerializationSurrogateProvider(surrogate);
			return dcs;
		}
	}
}
