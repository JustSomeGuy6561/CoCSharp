﻿using CoC.Backend.Engine;
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

		internal static readonly ReadOnlyCollection<GameDifficulty> difficultyCollection = new ReadOnlyCollection<GameDifficulty>(difficulties);

		static DifficultyManager()
		{
			difficulties.Add(new Easy());
			difficulties.Add(new Normal());
			difficulties.Add(new Hard());
			difficulties.Add(new Extreme());
			difficulties.Add(new Grimdark());
		}
	}
}