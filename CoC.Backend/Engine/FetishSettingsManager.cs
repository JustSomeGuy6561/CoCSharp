using CoC.Backend.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Engine
{
	/// <summary>
	/// Class used to allow user to update their preferences for strange/extreme fetishes. Members stored in here are given to the GUI 
	/// to handle when it queries for fetishes to display so the user can set them accordingly. 
	/// </summary>
	public static class FetishSettingsManager
	{
		private static readonly List<FetishSetting> _fetishes;
		private static readonly HashSet<FetishSetting> _fetishLookup;
		public static readonly ReadOnlyCollection<FetishSetting> fetishes;
		static FetishSettingsManager()
		{
			_fetishes = new List<FetishSetting>();
			_fetishLookup = new HashSet<FetishSetting>();
			fetishes = new ReadOnlyCollection<FetishSetting>(_fetishes);
		}

		public static void IncludeFetish(FetishSetting fetish)
		{
			if (!_fetishLookup.Contains(fetish))
			{
				_fetishLookup.Add(fetish);
				_fetishes.Add(fetish);
			}
		}
	}
}
