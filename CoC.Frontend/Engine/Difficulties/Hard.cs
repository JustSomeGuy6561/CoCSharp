using CoC.Backend.Engine;
using CoC.Frontend.Areas.HomeBases;

namespace CoC.Frontend.Engine.Difficulties
{
	public sealed partial class Hard : GameDifficulty
	{
		public Hard() : base(HardStr, HardHint, HardDesc) { }

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
			return 1.25;
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
