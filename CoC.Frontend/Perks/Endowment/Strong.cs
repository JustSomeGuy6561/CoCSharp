//Strong.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:31 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Strong : EndowmentPerkBase
	{
		public Strong() : base(StrongStr, StrongBtn, StrongHint, StrongDesc)
		{}

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minStrengthDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, 5));
			AddModifierToPerk(baseModifiers.strengthGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
		}

		protected override void OnRemoval()
		{ }
	}
}

//];
