//Tough.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Tough : EndowmentPerkBase
	{
		public Tough() : base(ToughStr, ToughBtn, ToughHint, ToughDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minToughnessDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
			AddModifierToPerk(baseModifiers.toughnessGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{ }
	}
}
