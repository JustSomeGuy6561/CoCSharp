//SaveSystem.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 11:14 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.SaveData
{
	public static class SaveSystem
	{
		private static readonly Dictionary<Type, SaveData> sessionSaves = new Dictionary<Type, SaveData>();
		private static readonly Dictionary<Type, SaveData> globalSaves = new Dictionary<Type, SaveData>();

		public static bool AddSessionSave(SaveData item)
		{
			if (sessionSaves.ContainsKey(item.GetType()))
			{
				return false;
			}
			else
			{
				sessionSaves.Add(item.GetType(), item);
				return true;
			}
		}

		public static T getSessionSave<T>() where T : SaveData
		{
			if (sessionSaves.ContainsKey(typeof(T)))
			{
				return (T)sessionSaves[typeof(T)];
			}
			throw new ArgumentException("Argument does not exist in current save system. make sure you added it during the initialization phase.");
		}

		public static bool AddGlobalSave(SaveData item)
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

		public static T getGlobalSave<T>() where T : SaveData
		{
			if (globalSaves.ContainsKey(typeof(T)))
			{
				return (T)globalSaves[typeof(T)];
			}
			throw new ArgumentException("Argument does not exist in current save system. make sure you added it during the initialization phase.");
		}

	}
}
