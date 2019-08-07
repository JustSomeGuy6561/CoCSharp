using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ModelView.Helpers
{
	public sealed class SaveDisplayData : INotifyPropertyChanged
	{
		public string Name { get; private set; }
		public string Notes { get; private set; }
		public DateTime LastPlayed { get; private set; }
		public string GenderString => gender.AsText();
		public string GameDifficultyString => gameDifficulty.difficultyStr();

		private GameDifficulty gameDifficulty;
		private Gender gender;




		public event PropertyChangedEventHandler PropertyChanged;
	}
}
