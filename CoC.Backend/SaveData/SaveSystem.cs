//SaveSystem.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 11:14 PM
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Backend.SaveData
{
	public static class SaveSystem
	{
		private static readonly Dictionary<Type, SessionSaveData> sessionSaves = new Dictionary<Type, SessionSaveData>();
		private static readonly Dictionary<Type, GlobalSaveData> globalSaves = new Dictionary<Type, GlobalSaveData>();

		public static bool AddSessionSave<T>() where T : SessionSaveData, new()
		{
			if (sessionSaves.ContainsKey(typeof(T)))
			{
				return false;
			}
			else
			{
				sessionSaves.Add(typeof(T), new T());
				return true;
			}
		}

		public static T GetSessionSave<T>() where T : SessionSaveData
		{
			if (sessionSaves.ContainsKey(typeof(T)))
			{
				return (T)sessionSaves[typeof(T)];
			}
			throw new ArgumentException("Argument does not exist in current save system. make sure you added it during the initialization phase.");
		}

		public static bool HasSessionSave<T>() where T : SessionSaveData
		{
			return sessionSaves.ContainsKey(typeof(T));
		}

		public static bool TryGetSessionSave<T>(out T data) where T : SessionSaveData
		{
			if (sessionSaves.ContainsKey(typeof(T)))
			{
				data = (T)sessionSaves[typeof(T)];
				return true;
			}
			else
			{
				data = null;
				return false;
			}
		}

		public static bool AddGlobalSave(GlobalSaveData item)
		{
			if (globalSaves.ContainsKey(item.GetType()))
			{
				return false;
			}
			else
			{
				globalSaves.Add(item.GetType(), item);
				return true;
			}
		}

		public static T GetGlobalSave<T>() where T : GlobalSaveData
		{
			if (globalSaves.ContainsKey(typeof(T)))
			{
				return (T)globalSaves[typeof(T)];
			}
			throw new ArgumentException("Argument does not exist in current save system. make sure you added it during the initialization phase.");
		}

		public static bool HasGlobalSave<T>() where T : GlobalSaveData
		{
			return sessionSaves.ContainsKey(typeof(T));
		}

		public static bool TryGetGlobalSave<T>(out T data) where T : GlobalSaveData
		{
			if (globalSaves.ContainsKey(typeof(T)))
			{
				data = (T)globalSaves[typeof(T)];
				return true;
			}
			else
			{
				data = null;
				return false;
			}
		}

		//determines if the current session data is part of an existing game, or is being preset for a new game. 
		//we need to know this during creation, and some settings may depend on if a game is loaded or not. 
		public static bool isSessionActive { get; private set; } = false;

		internal static void ResetSessionDataForNewGame()
		{
			if (!isSessionActive)
			{
				return;
			}
			else
			{
				List<Type> keys = sessionSaves.Keys.ToList();
				foreach (var key in keys)
				{
					sessionSaves[key] = (SessionSaveData)Activator.CreateInstance(key); //we can do this because the add functions require new(). 
				}
				isSessionActive = false;
			}
		}

		internal static void MarkGameLoaded()
		{
			isSessionActive = true;
		}
	}
}
