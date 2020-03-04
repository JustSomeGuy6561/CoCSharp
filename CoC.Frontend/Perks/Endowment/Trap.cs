//Trap.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Trap : EndowmentPerkBase
	{
		public Trap() : base(TrapStr, TrapBtn, TrapHint, TrapDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.cockGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
			AddModifierToPerk(baseModifiers.cockShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));

			AddModifierToPerk(baseModifiers.clitGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
			AddModifierToPerk(baseModifiers.clitShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));

			AddModifierToPerk(baseModifiers.ballsGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
			AddModifierToPerk(baseModifiers.ballsShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));

			AddModifierToPerk(baseModifiers.nippleGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
			AddModifierToPerk(baseModifiers.nippleShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));

			AddModifierToPerk(baseModifiers.titsGrowthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
			AddModifierToPerk(baseModifiers.titsShrinkMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));
		}

		protected override void OnRemoval()
		{
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender != Gender.HERM;
		}
	}
}
