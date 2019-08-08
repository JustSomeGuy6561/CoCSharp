using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Frontend.UI.ControllerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ModelView.Helpers
{
	public sealed class SaveDisplayData : INotifyPropertyChanged
	{
		public string Name { get; }
		public string Notes { get; }
		public DateTime LastPlayed { get; }
		public string GenderString => gender.AsText();
		public string GameDifficultyString => gameDifficulty.difficultyStr();

		private readonly GameDifficulty gameDifficulty;
		private readonly Gender gender;

		internal readonly StatDataCollection saveStatData;

		public event PropertyChangedEventHandler PropertyChanged;

		public SaveDisplayData(string fileName)
		{

		}
	}
}
