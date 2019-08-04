using CoC.Backend;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public enum CreatureStatCategory { CORE, COMBAT, ADVANCEMENT, PRISON, OTHER }

	//Note: The Creature stat has a "stat Name". The GUI uses this ONLY AS A LOOKUP/FALLBACK VALUE.
	//English text is hard-coded in every function so if something is not translated, it will fallback to displaying it in English - it's 
	//much easier to debug and find what wasn't translated when you can search for the text that did display and see why it wasn't translated.
	//but this is all it's used for. 

	public class CreatureStat : INotifyPropertyChanged//,ILanguageAware
	{
		public readonly string statName;
		public readonly CreatureStatCategory category;

		public string text => statName;

		public bool enabled
		{
			get => _enabled;
			internal set
			{
				if (_enabled != value)
				{
					_enabled = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool _enabled = true;
		/// <summary>
		/// Should the UI tell the player that this stat changed? note that this is only a hint; the UI can ignore it. 
		/// </summary>
		public bool notifyPlayerOfChange
		{
			get => _notifyPlayerOfChange;
			internal set
			{
				if (_notifyPlayerOfChange != value)
				{
					_notifyPlayerOfChange = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool _notifyPlayerOfChange = true;

		public uint current
		{
			get => _current;
			internal set
			{
				if (_current != value)
				{
					_current = value;
					NotifyPropertyChanged();
				}
			}
		}
		private uint _current = 0;

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		internal CreatureStat(string name, CreatureStatCategory statCategory)
		{
			statName = name ?? throw new ArgumentNullException(nameof(name));
			category = statCategory;

			//game engine add reference 
		}

		private void GameEngine_LanguageChanged(object sender, EventArgs e)
		{
			NotifyPropertyChanged(nameof(text));
		}

		~CreatureStat()
		{
		}
	}
}
