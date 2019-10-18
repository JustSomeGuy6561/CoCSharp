using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Frontend.Areas.HomeBases;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Engine.Difficulties
{
	public sealed partial class Nightmare : GameDifficulty
	{
		public Nightmare() : base(NightmareStr,NightmareHint, NightmareDesc)
		{
		}

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
			return 1.5;
		}
		public override void OnGameStart()
		{
			GameEngine.SetHomeBase<Camp>();
		}

		public override bool IsEnabled(bool isGlobal, out string whyNot)
		{
			whyNot = null;
			return true;
		}
	}
}
