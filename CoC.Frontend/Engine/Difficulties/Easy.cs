using System;
using CoC.Backend.Engine;
using CoC.Frontend.Areas.HomeBases;

namespace CoC.Frontend.Engine.Difficulties
{
	public sealed partial class Easy : GameDifficulty
	{
		public Easy() : base(EasyStr, EasyHint, EasyDesc) { }

		public override ushort baseMonsterHP(byte level)
		{
			return 50;
		}

		public override ushort basePlayerHP(byte level)
		{
			return level.mult(15);
		}

		public override double monsterHPMultiplier()
		{
			return 0.5;
		}

		public override void OnGameStart()
		{
			GameEngine.SetHomeBase<Camp>();
		}

		public override bool IsEnabled(bool isGlobal, out string whyNot)
		{
			bool hardCoreEnabled = isGlobal ? Backend.SaveData.BackendGlobalSave.data.hardcoreModeGlobal : Backend.SaveData.BackendSessionSave.data.hardcoreMode;

			if (hardCoreEnabled)
			{
				whyNot = NotCompatibleWithHardcoreModeStr();
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool HardcoreModeCompatible => false;
	}
}
