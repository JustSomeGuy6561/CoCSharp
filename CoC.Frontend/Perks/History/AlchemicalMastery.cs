//MadScientist.cs
//Description:
//Author: JustSomeGuy
//7/9/2019, 6:58 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class AlchemicalMastery : HistoryPerkBase
	{
		public AlchemicalMastery() : base(ScientistStr, ScientistBtn, ScientistHint, ScientistDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{
				AddModifierToPerk(extraModifiers.itemForgeCostReduction, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 1));
			}
		}

		protected override void OnRemoval()
		{
		}
	}
}
