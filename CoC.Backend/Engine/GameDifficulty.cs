//GameDifficulty.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using System;
using System.Collections.ObjectModel;

namespace CoC.Backend.Engine
{
	public abstract class GameDifficulty
	{
		public readonly SimpleDescriptor difficultyStr;
		public readonly SimpleDescriptor difficultyHint;
		public readonly SimpleDescriptor difficultyDesc;
		public GameDifficulty(SimpleDescriptor name, SimpleDescriptor hint, SimpleDescriptor choseDescription)
		{
			difficultyStr = name ?? throw new ArgumentNullException(nameof(name));
			difficultyHint = hint ?? throw new ArgumentNullException(nameof(hint));
			difficultyDesc = choseDescription ?? throw new ArgumentNullException(nameof(choseDescription));
		}

		public abstract ushort baseMonsterHP(byte level);

		public abstract double monsterHPMultiplier();

		public abstract ushort basePlayerHP(byte level);

		//called after all game related data is parsed and ready to go. This allows you to correct any values that
		//may not be valid for the current difficulty, such as hunger, autosave, etc.
		//you'll also need to change the home Base location here if you do not use the default (aka GrimDark mode)
		public abstract void OnGameStart();

		public abstract bool IsEnabled(bool isGlobal, out string whyNot);

		public virtual bool HardcoreModeCompatible => true;
	}
}
