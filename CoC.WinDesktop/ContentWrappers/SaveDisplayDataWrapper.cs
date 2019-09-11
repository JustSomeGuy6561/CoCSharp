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

#pragma warning disable 0649
		private readonly DateTime LastPlayedDateTime;
		private readonly GameDifficulty gameDifficulty;
		private readonly Gender gender;
#pragma warning restore 0649
		public string LastPlayed => LastPlayedDateTime.ToString();
		public string GenderString => gender.AsText();
		public string GameDifficultyString => gameDifficulty.difficultyStr();


#pragma warning disable 0649
		internal readonly StatDataCollectionBase saveStatData;
#pragma warning restore 0649
		public SaveDisplayDataWrapper(string fileName)
		{

		}
	}
}
