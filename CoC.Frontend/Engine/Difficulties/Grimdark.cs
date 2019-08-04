using CoC.Backend.Engine;

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
	}
}
