//GameDifficulty.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:20 AM
using System.Collections.ObjectModel;

namespace CoC.Backend.Engine
{
	public abstract class GameDifficulty
	{
		public readonly SimpleDescriptor difficultyStr;
		public GameDifficulty(SimpleDescriptor descriptor)
		{
			difficultyStr = descriptor;
		}

		public abstract ushort baseMonsterHP(int level);

		public abstract double monsterHPMultiplier();

		public abstract ushort basePlayerHP(int level);
	}
}