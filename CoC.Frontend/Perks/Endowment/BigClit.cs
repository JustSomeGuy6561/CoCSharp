//BigClit.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigClit : EndowmentPerkBase
	{
		private const double DELTA = 1.0f - Clit.DEFAULT_CLIT_SIZE;
		public BigClit() : base(BigClitStr, BigClitBtn, BigClitHint, BigClitDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.DefaultNewClitSize += DELTA;
			baseModifiers.NewClitSizeDelta += 0.25f;
			baseModifiers.ClitGrowthMultiplier += 0.25f;
			baseModifiers.ClitShrinkMultiplier -= 0.25f;
			baseModifiers.MinClitSize += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.DefaultNewClitSize -= DELTA;
			baseModifiers.NewClitSizeDelta -= 0.25f;
			baseModifiers.ClitGrowthMultiplier -= 0.25f;
			baseModifiers.ClitShrinkMultiplier += 0.25f;
			baseModifiers.MinClitSize -= 0.25f;
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.FEMALE);
		}
	}
}
