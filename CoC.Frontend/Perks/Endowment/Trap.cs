//Trap.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Trap : EndowmentPerkBase
	{
		public Trap() : base(TrapStr, TrapBtn, TrapHint, TrapDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.CockGrowthMultiplier -= 0.2f;
			baseModifiers.CockShrinkMultiplier += 0.2f;

			baseModifiers.ClitGrowthMultiplier -= 0.2f;
			baseModifiers.ClitShrinkMultiplier += 0.2f;

			baseModifiers.BallsGrowthMultiplier -= 0.2f;
			baseModifiers.BallsShrinkMultiplier += 0.2f;

			baseModifiers.NippleGrowthMultiplier -= 0.2f;
			baseModifiers.NippleShrinkMultiplier += 0.2f;

			baseModifiers.TitsGrowthMultiplier -= 0.2f;
			baseModifiers.TitsShrinkMultiplier += 0.2f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.CockGrowthMultiplier += 0.2f;
			baseModifiers.CockShrinkMultiplier -= 0.2f;

			baseModifiers.ClitGrowthMultiplier += 0.2f;
			baseModifiers.ClitShrinkMultiplier -= 0.2f;

			baseModifiers.BallsGrowthMultiplier += 0.2f;
			baseModifiers.BallsShrinkMultiplier -= 0.2f;

			baseModifiers.NippleGrowthMultiplier += 0.2f;
			baseModifiers.NippleShrinkMultiplier -= 0.2f;

			baseModifiers.TitsGrowthMultiplier += 0.2f;
			baseModifiers.TitsShrinkMultiplier -= 0.2f;
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender != Gender.HERM;
		}
	}
}