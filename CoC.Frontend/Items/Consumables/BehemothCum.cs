//BehemothCum.cs
//Description:
//Author: JustSomeGuy
//11/2/2019 10:59:58 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class BehemothCum : ConsumableBase
	{
		public BehemothCum() : base(Short, Full, Desc)
		{
		}

		private static string Short()
		{
			return "BhmtCum";
		}

		private static string Full()
		{
			return "a sealed bottle of behemoth cum";
		}

		private static string Desc()
		{
			return "This bottle of behemoth cum looks thick and viscous. You suspect that it might boost your strength and toughness. It also has delicious taste.";
		}

		private string UseItemText()
		{
			return "You uncork the bottle and drink the behemoth cum; it tastes great and by the time you've finished drinking, you feel a bit stronger. ";
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => true;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => true;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 25;

		protected override int monetaryValue => 15;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			resultsOfUse = UseItemText();

			if (consumer is CombatCreature combatStats)
			{
				combatStats.IncreaseStrength(0.5f);
				combatStats.IncreaseToughness(0.5f);
				combatStats.AddHPPercent(0.25f);
			}

			consumer.IncreaseLust(5 + consumer.corruptionTrue / 5);

			consumer.HaveGenericOralOrgasm(false, true);

			isBadEnd = false;
			return true;
		}

		public override byte maxCapacityPerSlot => 5;
	}
}
