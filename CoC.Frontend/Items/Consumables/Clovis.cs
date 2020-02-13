//Clovis.cs
//Description:
//Author: JustSomeGuy
//1/15/2020 9:00:04 AM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class Clovis : StandardConsumable
	{
		public Clovis() : base()
		{
		}

		public override string AbbreviatedName()
		{
			return "Clovis";
		}

		public override string ItemName()
		{
			return "Clovis";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string vialText = count != 1 ? "bottles" : "bottle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of Clovis";
		}

		public override string AboutItem()
		{
			return "This bottle is in the shape of a 4-leaf-clover and contains a soft pink potion. An image of a sheep is on the label along with text, \"" +
				SafelyFormattedString.FormattedText("Clovis - to help you to live in clover", StringFormats.ITALIC) + "\".";
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => true;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 1;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is Clovis;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new SheepTFs();
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			var tf = new SheepTFs();
			resultsOfUse = tf.DoTransformationFromCombat(consumer, out isCombatLoss, out isBadEnd);
			return true;
		}

		private class SheepTFs : SheepTransformations
		{

		}


	}
}
