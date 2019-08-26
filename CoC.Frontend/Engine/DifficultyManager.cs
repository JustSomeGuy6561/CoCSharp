using CoC.Backend.Engine;
using CoC.Frontend.Engine.Difficulties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Frontend.Engine
{
	internal static class DifficultyManager
	{
		private static readonly List<GameDifficulty> difficulties = new List<GameDifficulty>();
		internal static readonly int defaultDifficultyIndex;
		internal static readonly ReadOnlyCollection<GameDifficulty> difficultyCollection = new ReadOnlyCollection<GameDifficulty>(difficulties);

		static DifficultyManager()
		{
			difficulties.Add(new Easy());
			difficulties.Add(new Normal());

			defaultDifficultyIndex = difficulties.Count - 1; //must appear right after normal. 

			difficulties.Add(new Hard());
			difficulties.Add(new Nightmare());
			difficulties.Add(new Extreme());
			difficulties.Add(new Grimdark());
		}
	}
}
