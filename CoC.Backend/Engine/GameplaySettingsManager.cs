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
		private static readonly List<GameplaySettingBase> _settings;
		private static readonly HashSet<GameplaySettingBase> _settingLookup;
		public static readonly ReadOnlyCollection<GameplaySettingBase> gameSettings;
		static GameplaySettingsManager()
		{
			_settings = new List<GameplaySettingBase>();
			_settingLookup = new HashSet<GameplaySettingBase>();
			gameSettings = new ReadOnlyCollection<GameplaySettingBase>(_settings);
		}

		//do this during the initialization phase, after the game has successfully loaded all save data sessions/globals. 
		public static void IncludeGameplaySetting(GameplaySettingBase setting)
		{
			if (!_settingLookup.Contains(setting))
			{
				_settingLookup.Add(setting);
				_settings.Add(setting);
			}
		}
	}
}
