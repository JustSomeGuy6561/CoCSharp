using CoC.Backend.Engine;

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
			return 1.5;
		}
	}
}
