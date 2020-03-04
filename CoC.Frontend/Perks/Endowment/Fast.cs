//Fast.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fast : EndowmentPerkBase
	{
		public Fast() : base(FastStr, FastBtn, FastHint, FastDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minSpeedDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
			AddModifierToPerk(baseModifiers.speedGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{
		}
	}
}
