using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * What exactly, is not clear?
	 */
	public class GiantChocolateCupcake : StandardConsumable
	{
		private const int ITEM_VALUE = 250;

		public override string AbbreviatedName() => "CCupcak";
		public override string ItemName() => "Chocolate Cupcake";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "cupcakes" : "cupcake";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}gigantic chocoloate {itemText}";
		}
		public override string AboutItem() => "A gigantic chocolate cupcake. You could easily get full from eating this!";
		protected override int monetaryValue => ITEM_VALUE;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;

		public GiantChocolateCupcake() : base()
		{ }

		public override bool Equals(CapacityItem other)
		{
			return other is GiantChocolateCupcake;
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			isBadEnd = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("You look down at the massive chocolate cupcake and wonder just how you can possibly eat it all. It fills the over-sized wrapper and bulges out over the top, somehow looking obscene even though it's merely a baked treat. There is a single candle positioned atop its summit, and it bursts into flame as if by magic. Eight red gumdrops ring the outer edge of the cupcake, illuminated by the flame." + GlobalStrings.NewParagraph());
			sb.Append("You hesitantly take a bite. It's sweet, as you'd expect, but there's also a slightly salty, chocolaty undercurrent of flavor. Even knowing what the minotaur put in Maddie's mix, you find yourself grateful that this new creation doesn't seem to have any of his 'special seasonings'. It wouldn't do to be getting drugged up while you're slowly devouring the massive, muffin-molded masterpiece. Before you know it, most of the cupcake is gone and you polish off the last chocolaty bites before licking your fingers clean." + GlobalStrings.NewParagraph());
			sb.Append("Gods, you feel heavy! You waddle slightly as your body begins thickening, swelling until you feel as wide as a house. Lethargy spreads through your limbs, and you're forced to sit still a little while until you let out a lazy burp." + GlobalStrings.NewParagraph());
			sb.Append("As you relax in your sugar-coma, you realize your muscle definition is fading away, disappearing until your " + consumer.body.LongEpidermisDescription() + " looks nearly as soft and spongy as Maddie's own. You caress the soft, pudgy mass and shiver in delight, dimly wondering if this is how the cupcake-girl must feel all the time.");
			sb.Append(consumer.ModifyTone(0, 100));
			sb.Append(consumer.ModifyThickness(100, 100));

			resultsOfUse = sb.ToString();
			return true;
		}
		public override byte sateHungerAmount => 100;
	}
}