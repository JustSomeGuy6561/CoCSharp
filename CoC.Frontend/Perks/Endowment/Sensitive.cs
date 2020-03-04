//Sensitive.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Sensitive : EndowmentPerkBase
	{
		public Sensitive() : base(SensitiveStr, SensitiveBtn, SensitiveHint, SensitiveDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minSensitivityDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
			AddModifierToPerk(baseModifiers.sensitivityGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{ }
	}
}
