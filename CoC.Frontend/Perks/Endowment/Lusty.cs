//Lusty.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Lusty : EndowmentPerkBase
	{
		public Lusty() : base(LustyStr, LustyBtn, LustyHint, LustyDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.lustGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			AddModifierToPerk(baseModifiers.minLibidoDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
		}

		protected override void OnRemoval()
		{
		}
	}
}
