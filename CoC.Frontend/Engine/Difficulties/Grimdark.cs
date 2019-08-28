using System;
using CoC.Backend.Engine;
using CoC.Frontend.Areas.HomeBases;

namespace CoC.Frontend.Engine.Difficulties
{
	public sealed partial class Grimdark : GameDifficulty
	{
		public Grimdark() : base(GrimdarkStr, GrimdarkHint, GrimdarkDesc) { }

		public override ushort baseMonsterHP(byte level)
		{
			return level.mult(15);
		}

		public override ushort basePlayerHP(byte level)
		{
			return level.mult(5);
		}

		public override double monsterHPMultiplier()
		{
			return 2.0;
		}

		public override void OnGameStart()
		{
			GameEngine.SetHomeBase<IngnamBase>();
		}

		public override bool IsEnabled(bool isGlobal, out string whyNot)
		{
			if (Backend.SaveData.BackendGlobalSave.data.highestDifficultyBeaten >= DifficultyManager.difficultyCollection.IndexOf(this) - 1)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = YouStillNeedToUnlockGrimdark();
				return false;
			}
		}
	}
}
