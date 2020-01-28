//BehemothCum.cs
//Description:
//Author: JustSomeGuy
//1/27/2020 11:07:24 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class BehemothCum : ConsumableBase
	{
		public BehemothCum() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "BhmtCum";
		}

		public override string ItemName()
		{
			return "Behemoth Cum";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";
			string bottleText = count == 1 ? "sealed bottle" : "sealed bottles";

			return $"{count} {bottleText} of behemoth cum";
		}

		public override string Appearance()
		{
			return "This bottle of behemoth cum looks thick and viscous. You suspect that it might boost your strength and toughness. It also tastes delicious.";
		}



		//this can be removed safely if the item does a transformation. transformations handle bad end and results text for you.
		private string UseItemText()
		{
			return "You uncork the bottle and drink the behemoth cum; it tastes great and by the time you've finished drinking, you feel a bit stronger. ";
		}

		public override bool Equals(CapacityItem other)
		{
			return other is BehemothCum;
		}



		public override bool countsAsLiquid => true;
		public override bool countsAsCum => true;
		public override byte sateHungerAmount => 25;

		protected override int monetaryValue => 15;

		public override bool CanUse(Creature target, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			consumer.IncreaseLust(5 + (consumer.corruptionTrue / 5));
			(consumer as CombatCreature)?.AddHPPercent(25);
			(consumer as CombatCreature)?.DeltaCombatCreatureStats(str: 0.5f, tou: 0.5f);

			resultsOfUse = UseItemText();
			isBadEnd = false;

			return true;
		}
	}
}
