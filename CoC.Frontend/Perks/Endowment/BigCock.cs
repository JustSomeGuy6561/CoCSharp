//BigCock.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigCock : EndowmentPerkBase
	{
		public BigCock() : base(BigCockStr, BigCockBtn, BigCockHint, BigCockDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.NewCockDefaultSize += 2.0f;
			baseModifiers.NewCockSizeDelta += 2.0f;
			baseModifiers.CockGrowthMultiplier += 0.25f;
			baseModifiers.CockShrinkMultiplier -= 0.25f;
			//no limit on min cock size, as size is used to remove cocks. i suppose it'd be possible to set 
			//a min value and once the size goes below the min, remove it. 
		}

		protected override void OnRemoval()
		{
			baseModifiers.NewCockDefaultSize -= 2.0f;
			baseModifiers.NewCockSizeDelta -= 2.0f;
			baseModifiers.CockGrowthMultiplier -= 0.25f;
			baseModifiers.CockShrinkMultiplier += 0.25f;
			//no limit on min cock size, as size is used to remove cocks. i suppose it'd be possible to set 
			//a min value and once the size goes below the min, remove it. 
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.MALE);
		}
	}
}
