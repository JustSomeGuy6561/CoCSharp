using CoC.Backend.Engine;

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
	}
}
