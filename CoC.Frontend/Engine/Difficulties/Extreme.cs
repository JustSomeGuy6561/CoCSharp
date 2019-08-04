using CoC.Backend.Engine;

namespace CoC.Frontend.Engine.Difficulties
{
	public sealed partial class Extreme : GameDifficulty
	{
		public Extreme() : base(ExtremeStr, ExtremeHint, ExtremeDesc) { }

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
			return 2.0;
		}
	}
}
