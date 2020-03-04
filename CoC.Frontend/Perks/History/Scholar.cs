//Scholar.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Scholar : HistoryPerkBase
	{
		public Scholar() : base(ScholarStr, ScholarBtn, ScholarHint, ScholarDesc)
		{
		}

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.magicalSpellCost, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, -0.2));
		}

		protected override void OnRemoval()
		{
		}
	}
}
