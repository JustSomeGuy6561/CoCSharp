//BigTits.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class BigTits : EndowmentPerkBase
	{
		public BigTits() : base(BigTitsStr, BigTitsBtn, BigTitsHint, BigTitsDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.femaleNewBreastCupSizeDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 1));
			AddModifierToPerk(baseModifiers.titsGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			AddModifierToPerk(baseModifiers.titsShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.25));
			AddModifierToPerk(baseModifiers.femaleNewBreastDefaultCupSize, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 1));
			AddModifierToPerk(baseModifiers.femaleMinCupSize, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 1));
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
