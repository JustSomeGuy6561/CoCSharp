//BigTits.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigTits : EndowmentPerkBase
	{
		public BigTits() : base(BigTitsStr, BigTitsBtn, BigTitsHint, BigTitsDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.FemaleNewBreastCupSizeDelta += 1;
			baseModifiers.TitsGrowthMultiplier += 0.25f;
			baseModifiers.TitsShrinkMultiplier -= 0.25f;
			baseModifiers.FemaleNewBreastDefaultCupSize += 1;
			baseModifiers.FemaleMinCupSize += 1;
		}

		protected override void OnRemoval()
		{
			baseModifiers.FemaleNewBreastCupSizeDelta -= 1;
			baseModifiers.TitsGrowthMultiplier -= 0.25f;
			baseModifiers.TitsShrinkMultiplier += 0.25f;
			baseModifiers.FemaleNewBreastDefaultCupSize -= 1;
			baseModifiers.FemaleMinCupSize -= 1;
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.FEMALE);
		}
	}
}