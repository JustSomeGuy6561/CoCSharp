//Smart.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Smart : EndowmentPerkBase
	{
		public Smart() : base(SmartStr, SmartBtn, SmartHint, SmartDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minIntelligenceDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
			AddModifierToPerk(baseModifiers.intelligenceGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{ }
	}
}
