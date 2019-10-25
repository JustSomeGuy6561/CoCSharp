//MadScientist.cs
//Description:
//Author: JustSomeGuy
//7/9/2019, 6:58 PM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class AlchemicalMastery : HistoryPerkBase
	{
		public AlchemicalMastery() : base(ScientistStr, ScientistBtn, ScientistHint, ScientistDesc)
		{
		}

		protected override void OnActivation()
		{
			ExtendedPerkModifiers data = extraModifiers;
			data.itemForgeCostReduction.addIn(1);
		}

		protected override void OnRemoval()
		{
			ExtendedPerkModifiers data = extraModifiers;
			data.itemForgeCostReduction.subtractOff(1);
		}
	}
}
