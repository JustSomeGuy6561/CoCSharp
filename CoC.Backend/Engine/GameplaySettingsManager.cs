using CoC.Backend.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Engine
{
	/// <summary>
	/// Class used to allow user to update their gameplay settings. Members stored in here are given to the GUI
	/// to handle when it queries for gameplay settings to display so the user can set them accordingly.
	/// </summary>
	public static class GameplaySettingsManager
	{
		private static readonly List<GameplaySetting> _settings;
		private static readonly Dictionary<Type, GameplaySetting> _settingLookup;
		public static readonly ReadOnlyCollection<GameplaySetting> gameSettings;
		static GameplaySettingsManager()
		{
			_settings = new List<GameplaySetting>();
			_settingLookup = new Dictionary<Type, GameplaySetting>();
			gameSettings = new ReadOnlyCollection<GameplaySetting>(_settings);
		}

		//do this during the initialization phase, after the game has successfully loaded all save data sessions/globals.
		public static void IncludeGameplaySetting(GameplaySetting setting)
		{
			if (!_settingLookup.ContainsKey(setting.GetType()))
			{
				_settingLookup.Add(setting.GetType(), setting);
				_settings.Add(setting);
			}
		}

		public static GameplaySetting GetSetting(Func<GameplaySetting, bool> findSetting)
		{
			return System.Linq.Enumerable.FirstOrDefault(gameSettings, findSetting);
		}
	}
}
