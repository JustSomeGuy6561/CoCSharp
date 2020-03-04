//BigCock.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigCock : EndowmentPerkBase
	{
		public BigCock() : base(BigCockStr, BigCockBtn, BigCockHint, BigCockDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.newCockDefaultSize, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 2.0));
			AddModifierToPerk(baseModifiers.newCockSizeDelta, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 2.0));
			AddModifierToPerk(baseModifiers.cockGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			AddModifierToPerk(baseModifiers.cockShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.25));
			//no limit on min cock size, as size is used to remove cocks. i suppose it'd be possible to set
			//a min value and once the size goes below the min, remove it.
		}

		protected override void OnRemoval()
		{
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.MALE);
		}
	}
}
