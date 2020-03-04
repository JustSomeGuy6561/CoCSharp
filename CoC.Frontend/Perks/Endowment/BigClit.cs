//BigClit.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigClit : EndowmentPerkBase
	{
		public BigClit() : base(BigClitStr, BigClitBtn, BigClitHint, BigClitDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.defaultNewClitSize, new ValueModifierStore<double>(ValueModifierType.MINIMUM, 1));
			AddModifierToPerk(baseModifiers.newClitSizeDelta, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			AddModifierToPerk(baseModifiers.clitGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			AddModifierToPerk(baseModifiers.clitShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.25));
			AddModifierToPerk(baseModifiers.minClitSize, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.FEMALE);
		}
	}
}
