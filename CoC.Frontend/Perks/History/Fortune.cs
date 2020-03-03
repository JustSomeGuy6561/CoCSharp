//Fortune.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Creatures;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Fortune : HistoryPerkBase
	{
		public Fortune() : base(FortuneStr, FortuneBtn, FortuneHint, FortuneDesc) { }

		protected override void OnActivation()
		{
			sourceCreature.AddGems(250);
			if (hasExtraModifiers)
			{
				extraModifiers.gemGainMultiplier += 0.15f;
			}
		}

		protected override void OnRemoval()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.gemGainMultiplier += 0.15f;
			}
		}
	}
}
