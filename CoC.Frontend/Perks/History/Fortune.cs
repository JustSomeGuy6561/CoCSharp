﻿//Fortune.cs
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
			if (sourceCreature is CombatCreature combatCreature)
			{
				combatCreature.addGems(250);
			}
			extraModifiers.gemGainMultiplier += 0.15f;
		}

		protected override void OnRemoval()
		{
			extraModifiers.gemGainMultiplier += 0.15f;
		}
	}
}