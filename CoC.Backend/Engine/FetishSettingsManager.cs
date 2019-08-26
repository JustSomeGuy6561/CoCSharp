using CoC.Backend.Fetishes;
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
		private static readonly List<FetishBase> _fetishes;
		private static readonly HashSet<FetishBase> _fetishLookup;
		public static readonly ReadOnlyCollection<FetishBase> fetishes;
		static FetishSettingsManager()
		{
			_fetishes = new List<FetishBase>();
			_fetishLookup = new HashSet<FetishBase>();
			fetishes = new ReadOnlyCollection<FetishBase>(_fetishes);
		}

		public static void IncludeFetish(FetishBase fetish)
		{
			if (!_fetishLookup.Contains(fetish))
			{
				_fetishLookup.Add(fetish);
				_fetishes.Add(fetish);
			}
		}
	}
}
