using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Polly wants a cracker?
	 */
	public class HardBiscuits : StandardConsumable
	{
		private const int ITEM_VALUE = 5;

		public override string AbbreviatedName() => "H.Bisct";
		public override string ItemName() => "H.Biscuits";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "packs" : "pack";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{itemText} of hard biscuits";
		}
		public override string AboutItem() => "These biscuits are tasteless, but they can stay edible for an exceedingly long time.";
		protected override int monetaryValue => ITEM_VALUE;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;

		public HardBiscuits() : base()
		{ }

		public override bool Equals(CapacityItem other)
		{
			return other is HardBiscuits;
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			isBadEnd = false;
			resultsOfUse = "You eat the flavorless biscuits. It satisfies your hunger a bit, but not much else.";
			return true;
		}
		public override byte sateHungerAmount => 15;
	}
}
