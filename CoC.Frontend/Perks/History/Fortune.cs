//Fortune.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Creatures;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Fortune : HistoryPerkBase
	{
		public Fortune() : base(FortuneStr, FortuneBtn, FortuneHint, FortuneDesc) { }

		protected override void OnActivation()
		{
			sourceCreature.AddGems(250);
			AddModifierToPerk(baseModifiers.gemsGainRate, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.15));
		}

		protected override void OnRemoval()
		{
		}
	}
}
