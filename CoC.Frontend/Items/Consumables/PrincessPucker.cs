using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables
{


	public class PrincessPucker : StandardConsumable
	{
		public PrincessPucker() : base()
		{ }

		public override string AbbreviatedName() => "PrnsPkr";
		public override string ItemName() => "P.Pucker";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "vials" : "vial";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of pinkish fluid";
		}
		public override string AboutItem() => "A vial filled with a viscous pink liquid. A label reads \"Princess Pucker\".";
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 15;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumedItem, out bool isBadEnd)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("You uncork the bottle, and sniff it experimentally. The fluid is slightly pink, full of flecks of gold, and smelling vaguely of raspberries. Princess Gwynn said it was drinkable." + GlobalStrings.NewParagraph());

			sb.Append("You down the bottle, hiccuping a bit at the syrupy-sweet raspberry flavor. Immediately following the sweet is a bite of sour, like sharp lime. You pucker your lips, and feel your head clear a bit from the intensity of flavor. You wonder what Gwynn makes this out of." + GlobalStrings.NewParagraph());

			sb.Append("Echoing the sensation in your head is an answering tingle in your body. The sudden shock of citrusy sour has left you slightly less inclined to fuck, a little more focused on your priorities." + GlobalStrings.NewParagraph());

			if (Utils.Rand(2) == 0)
			{
				consumer.DeltaCreatureStats(lus: -20, lib: -2);
			}
			else
			{
				consumer.DeltaCreatureStats(lus: -20, sens: -2);
			}

			if (consumer.hair.hairColor != HairFurColors.PINK && Utils.Rand(5) == 0)
			{
				sb.Append("A slight tingle across your scalp draws your attention to your hair. It seems your " + consumer.hair.ShortDescription() +
					" is rapidly gaining a distinctly pink hue, growing in from the roots!" + GlobalStrings.NewParagraph());
				consumer.hair.SetHairColor(HairFurColors.PINK);
			}

			isBadEnd = false;
			consumedItem = true;
			return sb.ToString();
		}

		public override bool Equals(CapacityItem other)
		{
			return other is PrincessPucker;
		}
	}
}
