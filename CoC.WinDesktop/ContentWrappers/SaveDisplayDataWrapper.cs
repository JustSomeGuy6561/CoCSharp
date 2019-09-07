using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Frontend.UI.ControllerData;
using CoCWinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.ContentWrappers
{
	public sealed class SaveDisplayDataWrapper : NotifierBase
	{
		public string Name { get; }
		public string Notes { get; }

		private readonly DateTime LastPlayedDateTime;
		private readonly GameDifficulty gameDifficulty;
		private readonly Gender gender;

		public string LastPlayed => LastPlayedDateTime.ToString();
		public string GenderString => gender.AsText();
		public string GameDifficultyString => gameDifficulty.difficultyStr();



		internal readonly StatDataCollectionBase saveStatData;

		public SaveDisplayDataWrapper(string fileName)
		{

		}
	}
}
