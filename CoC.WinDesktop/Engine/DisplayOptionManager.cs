using CoCWinDesktop.DisplaySettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Engine
{
	public static class DisplayOptionManager
	{
		private static readonly List<DisplayOptions> _displays;
		private static readonly HashSet<DisplayOptions> _displayLookup;
		public static readonly ReadOnlyCollection<DisplayOptions> displayOptions;
		static DisplayOptionManager()
		{
			_displays = new List<DisplayOptions>();
			_displayLookup = new HashSet<DisplayOptions>();
			displayOptions = new ReadOnlyCollection<DisplayOptions>(_displays);
		}

		public static void IncludeOption(DisplayOptions option)
		{
			if (!_displayLookup.Contains(option))
			{
				_displayLookup.Add(option);
				_displays.Add(option);
			}
		}

		public static T GetOptionOfType<T>() where T : DisplayOptions
		{
			return _displays.Find(x => x.GetType() == typeof(T)) as T;
		}
	}
}
