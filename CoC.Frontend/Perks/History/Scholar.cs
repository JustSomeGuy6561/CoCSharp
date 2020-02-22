//Scholar.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Scholar : HistoryPerkBase
	{
		private double delta;
		public Scholar() : base(ScholarStr, ScholarBtn, ScholarHint, ScholarDesc)
		{
		}

		protected override void OnActivation()
		{
			double oldVal = baseModifiers.magicalSpellCost;
			baseModifiers.magicalSpellCost -= 0.2f;
			delta = oldVal - baseModifiers.magicalSpellCost;
		}

		protected override void OnRemoval()
		{
			baseModifiers.magicalSpellCost += delta;
		}
	}
}
