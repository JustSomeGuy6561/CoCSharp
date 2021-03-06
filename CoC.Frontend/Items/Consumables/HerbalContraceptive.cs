using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Items.Consumables
{


	public class HerbalContraceptive : StandardConsumable
	{
		const byte CONTRACEPTIVE_TIMEOUT = 48;

		public HerbalContraceptive() : base()
		{ }

		public override string AbbreviatedName() => "HrblCnt";
		public override string ItemName() => "HrblCnt";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "bundles" : "bundle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{itemText} of verdant green leaves";
		}
		public override string AboutItem() => "A small bundle of verdant green leaves.";
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 0;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (target.fertility.permanentlyInfertile)
			{
				whyNot = "Considering you're already barren, this feels like a waste. Best find someone else who could use these.";
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;

#warning Vanill code called this a placeholder. consider fixing?
			// Placeholder, sue me

			consumer.fertility.MakeTemporarilyInfertile(CONTRACEPTIVE_TIMEOUT);

			consumeItem = true;
			return "You chew on the frankly awfully bitter leaves as quickly as possible before swallowing them down.";
		}

		public override bool Equals(CapacityItem other)
		{
			return other is HerbalContraceptive;
		}
	}
}
