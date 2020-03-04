using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Items.Materials;

namespace CoC.Frontend.Items.Consumables
{


	public sealed class KitsuneGift : StandardConsumable
	{
#warning fix me when items are implemented
		public KitsuneGift() : base()
		{ }

		public override string AbbreviatedName() => "KitGift";
		public override string ItemName() => "KitGift";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "gifts" : "gift";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}kitsune's {itemText}";
			//"a kitsune's gift";
		}
		public override string AboutItem() => "A small square namespace given to you by a forest kitsune. It is wrapped up in plain white paper and tied with a string. Who knows what's inside?";
		protected override int monetaryValue => 0;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override byte sateHungerAmount => 0;

		public override bool Equals(CapacityItem other)
		{
			return other is KitsuneGift;
		}

		CapacityItem replacement;
		//kitsune gift afaik is the only thing to do this. what a pain. luckily fixing my mistake from code cleanup was as easy as marking this virtual - jsg.
		protected override CapacityItem ConsumeItem(Creature target, out string resultsOfUseText, out bool isBadEnd)
		{
			resultsOfUseText = OnConsumeAttempt(target, out bool usedItem, out isBadEnd);
			CapacityItem item = this;

			if (usedItem)
			{
				if (target is PlayerBase player)
				{
					player.RefillHunger(sateHungerAmount);
				}
				item = replacement;
			}

			replacement = null;
			return item;
		}
		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{

			StringBuilder sb = new StringBuilder();
			sb.Append("Curiosity gets the best of you, and you decide to open the namespace. After all, what's the worst that could happen?" + GlobalStrings.NewParagraph());
			//Opening the gift randomly results in one of the following:
			switch (Utils.Rand(12))
			{
				//[Fox Jewel]
				case 0:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and to your delight, sitting in the center is a small teardrop-shaped jewel!");
					sb.Append(GlobalStrings.NewParagraph() + "<b>You've received a shining Fox Jewel from the kitsune's gift! How generous!</b> ");
					//replacement = new FoxJewel();
					break;
				//[Fox Berries]
				case 1:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and to your delight, there is a small cluster of orange-colored berries sitting in the center!");
					sb.Append(GlobalStrings.NewParagraph() + "<b>You've received a fox berry from the kitsune's gift! How generous!</b> ");
					//add Fox Berries to inventory
					replacement = new FoxBerry();
					break;

				//[Gems]
				case 2:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and to your delight, it is filled to the brim with shining gems!");
					int gems = 2 + Utils.Rand(20);
					sb.Append(GlobalStrings.NewParagraph() + "<b>You've received " + Utils.NumberAsText(gems) + " shining gems from the kitsune's gift! How generous!</b>");
					consumer.AddGems((uint)gems);
					//add X gems to inventory
					break;

				//[Kitsune Tea/Scholar's Tea] //Just use Scholar's Tea and drop the "trick" effect if you don't want to throw in another new item.
				case 3:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and to your delight, it contains a small bag of dried tea leaves!");
					sb.Append(GlobalStrings.NewParagraph() + "<b>You've received a bag of tea from the kitsune's gift! How thoughtful!</b> ");
					//add Kitsune Tea/Scholar's Tea to inventory
					//replacement = new ScholarsTea();
					break;
				//[Hair Dye]
				case 4:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and to your delight, it contains a small vial filled with hair dye!");

					HairFurColors color = Utils.RandomChoice(HairFurColors.RED, HairFurColors.BLONDE, HairFurColors.BLACK, HairFurColors.WHITE);

					replacement = new HairDye(color);

					sb.Append(GlobalStrings.NewParagraph() + "<b>You've received " + replacement.ItemName() + " from the kitsune's gift! How generous!</b> ");
					//add <color> Dye to inventory
					break;
				//[Knowledge Spell]
				case 5:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, but it seems like there's nothing else inside. As you peer into the box, a glowing circle filled with strange symbols suddenly flashes to life! Light washes over you, and your mind is suddenly assaulted with new knowledge... and the urge to use that knowledge for mischief!");

					sb.Append(GlobalStrings.NewParagraph() + "<b>The kitsune has shared some of its knowledge with you!</b> But in the process, you've gained some of the kitsune's promiscuous trickster nature...");
					//Increase INT and Libido, +10 LUST
					consumer.DeltaCreatureStats(inte: 4, sens: 2, lus: 10);
					break;

				//[Thief!]
				case 6:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. " +
						"The inside of the box is lined with purple velvet, and sitting in the center is an artfully crafted paper doll. " +
						"Before your eyes, the doll springs to life, dancing about fancifully. Without warning, it leaps into your item pouch, " +
						"then hops away and gallavants into the woods, carting off a small fortune in gems.");

					sb.Append(GlobalStrings.NewParagraph() + "<b>The kitsune's familiar has stolen your gems!</b>");
					// Lose X gems as though losing in battle to a kitsune
					consumer.RemoveGems(2 + (uint)Utils.Rand(15));
					break;

				//[Prank]
				case 7:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and sitting in the center is an artfully crafted paper doll. Before your eyes, the doll springs to life, dancing about fancifully. Without warning, it pulls a large calligraphy brush from thin air and leaps up into your face, then hops away and gallavants off into the woods. Touching your face experimentally, you come away with a fresh coat of black ink on your fingertips.");

					sb.Append(GlobalStrings.NewParagraph() + "<b>The kitsune's familiar has drawn all over your face!</b> The resilient marks take about an hour to completely scrub off in the nearby stream. You could swear you heard some mirthful snickering among the trees while you were cleaning yourself off.");
					//Advance time 1 hour, -20 LUST
					consumer.DeltaCreatureStats(lus: -20);
					break;

				//[Aphrodisiac]
				case 8:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and sitting in the center is an artfully crafted paper doll. Before your eyes, the doll springs to life, dancing about fancifully. Without warning, it tosses a handful of sweet-smelling pink dust into your face, then hops over the rim of the box and gallavants off into the woods. Before you know what has happened, you feel yourself growing hot and flushed, unable to keep your hands away from your groin.");
					sb.Append(GlobalStrings.NewParagraph() + "<b>Oh no! The kitsune's familiar has hit you with a powerful aphrodisiac! You are debilitatingly aroused and can think of nothing other than masturbating.</b>");
					//+100 LUST
					consumer.DeltaCreatureStats(lus: 100);
					break;

				//[Wither]
				case 9:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, and sitting in the center is an artfully crafted paper doll. Before your eyes, the doll springs to life, dancing about fancifully. Without warning, it tosses a handful of sour-smelling orange powder into your face, then hops over the rim of the box and gallavants off into the woods. Before you know what has happened, you feel the strength draining from your muscles, withering away before your eyes.");
					sb.Append(GlobalStrings.NewParagraph() + "<b>Oh no! The kitsune's familiar has hit you with a strength draining spell! Hopefully it's only temporary...</b>");
					consumer.DeltaCreatureStats(str: -5, tou: -5);
					break;

				//[Dud]
				case 10:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, but to your disappointment, the only other contents appear to be nothing more than twigs, leaves, and other forest refuse.");
					sb.Append(GlobalStrings.NewParagraph() + "<b>It seems the kitsune's gift was just a pile of useless junk! What a ripoff!</b>");
					break;

				//[Dud... Or is it?]
				case 11:
				default:
					sb.Append("As the paper falls away, you carefully lift the cover of the box, your hands trembling nervously. The inside of the box is lined with purple velvet, but to your disappointment, the only other contents appear to be nothing more than twigs, leaves, and other forest refuse. Upon further investigation, though, you find a shard of shiny black chitinous plating mixed in with the other useless junk.");
					sb.Append(GlobalStrings.NewParagraph() + "<b>At least you managed to salvage a shard of black chitin from it...</b> ");
					//replacement = new BlackChitin();
					break;
			}

			isBadEnd = false;
			consumeItem = true;
			return sb.ToString();

		}
	}
}
