/**
 * Created by aimozg on 11.01.14.
 */
using System;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Consumables
{


	public sealed class GroPlus : ConsumableWithMenuBase
	{

		public GroPlus() : base()
		{ }

		public override string AbbreviatedName() => "GroPlus";
		public override string ItemName() => "GroPlus";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			throw;
			//"a needle filled with Gro+";
		}
		public override string AboutItem() => "This is a small needle with a reservoir full of blue liquid. A faded label marks it as 'GroPlus'. Its purpose seems obvious.";
		protected override int monetaryValue => 50;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			return true;
		}

		//		public override bool hasSubMenu() { return true; } //Only GroPlus and Reducto use this.

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		protected override DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback)
		{

			var display = new StandardDisplay();
			display.OutputText("You ponder the needle in your hand knowing it will enlarge the injection site. What part of your body will you use it on? ");

			var listMaker = new ButtonListMaker(display);
			foreach (var bodyPart in consumer.bodyParts)
			{
				if (bodyPart is IGrowable growable)
				{
					listMaker.AddButtonToList(bodyPart.BodyPartName(), growable.CanGroPlus(), () => ApplyGrowPlus(consumer, growable, postItemUseCallback);
				}
			}

			return display;
		}

		private void ApplyGrowPlus(Creature consumer, IGrowable growable, UseItemCallback postItemUseCallback)
		{
			consumer.IncreaseLust(10);

			postItemUseCallback(true, growable.UseGroPlus(), null, null);
		}

#warning convert these and use them in the backend where needed.

		//private void growPlusBreasts()
		//{
		//	StringBuilder sb = new StringBuilder();
		//	sb.Append("You sink the needle into the flesh of your " + consumer.allBreastsDescript() + " injecting each with a portion of the needle." + GlobalStrings.NewParagraph());
		//	if (consumer.breasts.Count == 1)
		//	{
		//		consumer.growTits(Utils.Rand(5) + 1, 1, true, 1);
		//	}
		//	else
		//	{
		//		consumer.growTits(Utils.Rand(2) + 1, consumer.breasts.Count, true, 1);
		//	}

		//	dynStats("lus", 10);
		//	inventory.itemGoNext();
		//}

		//private void growPlusClit()
		//{
		//	StringBuilder sb = new StringBuilder();
		//	sb.Append("You sink the needle into your clit, nearly crying with how much it hurts. You push down the plunger and the pain vanishes as your clit starts to grow." + GlobalStrings.NewParagraph());
		//	consumer.changeClitLength(1);
		//	sb.Append("Your " + consumer.clitDescript() + " stops growing after an inch of new flesh surges free of your netherlips. It twitches, feeling incredibly sensitive.");
		//	dynStats("sen", 2, "lus", 10);
		//	inventory.itemGoNext();
		//}

		//private void growPlusCock()
		//{
		//	StringBuilder sb = new StringBuilder();
		//	sb.Append("You sink the needle into the base of your " + consumer.genitals.AllCocksShortDescription() + ". It hurts like hell, but as you depress the plunger, the pain vanishes, replaced by a tingling pleasure as the chemicals take effect." + GlobalStrings.NewParagraph());
		//	if (consumer.cocks.Count == 1)
		//	{
		//		sb.Append("Your " + consumer.cocks[0].LongDescription() + " twitches and thickens, pouring more than an inch of thick new length from your ");
		//		consumer.increaseCock(0, 4);
		//		consumer.cocks[0].length += 1; // This was forcing "what was said" to match "what actually happened" no matter what increase/growCock /actually/ did.
		//		consumer.cocks[0].girth += 0.5; // And growCock never actually touched thickness. Nor does the new version. Thickness mod was stripped out entirely.
		//	}
		//	//MULTI
		//	else
		//	{
		//		sb.Append("Your " + consumer.genitals.AllCocksShortDescription() + " twitch and thicken, each member pouring out more than an inch of new length from your ");
		//		for (int i = 0; i < consumer.cocks.Count; i++)
		//		{
		//			consumer.increaseCock(i, 2);
		//			consumer.cocks[i].length += 1;
		//			consumer.cocks[i].girth += 0.5;
		//		}
		//	}
		//	if (consumer.hasSheath())
		//	{
		//		sb.Append("sheath.");
		//	}
		//	else
		//	{
		//		sb.Append("crotch.");
		//	}

		//	dynStats("sen", 2, "lus", 10);
		//	inventory.itemGoNext();
		//}

		//private void growPlusNipples()
		//{
		//	StringBuilder sb = new StringBuilder();
		//	sb.Append("You sink the needle into each of your " + consumer.nippleDescript(0) + "s in turn, dividing the fluid evenly between them. Though each injection hurts, the pain is quickly washed away by the potent chemical cocktail." + GlobalStrings.NewParagraph());
		//	//Grow nipples
		//	sb.Append("Your nipples engorge, prodding hard against the inside of your " + consumer.armorName + ". Abruptly you realize they've grown more than an additional quarter-inch." + GlobalStrings.NewParagraph());
		//	consumer.nippleLength += (Utils.Rand(2) + 3) / 10;
		//	dynStats("lus", 15);
		//	//NIPPLECUNTZZZ
		//	if (!consumer.genitals.nippleType == NippleStatus.FUCKABLE && Utils.Rand(4) == 0)
		//	{
		//		bool nowFuckable = false;
		//		for (int x = 0; x < consumer.breasts.Count; x++)
		//		{
		//			if (!consumer.breasts[x].fuckable && consumer.nippleLength >= 2)
		//			{
		//				consumer.breasts[x].fuckable = true;
		//				nowFuckable = true;
		//			}
		//		}
		//		//Talk about if anything was changed.
		//		if (nowFuckable)
		//		{
		//			sb.Append("Your " + consumer.allBreastsDescript() + " tingle with warmth that slowly migrates to your nipples, filling them with warmth. You pant and moan, rubbing them with your fingers. A trickle of wetness suddenly coats your finger as it slips inside the nipple. Shocked, you pull the finger free. <b>You now have fuckable nipples!</b>" + GlobalStrings.NewParagraph());
		//		}
		//	}
		//	inventory.itemGoNext();
		//}

		private void GrowPlusCancel(Creature consumer, UseItemCallback postItemUseCallback)
		{
			string results = "You put the vial away." + GlobalStrings.NewParagraph();
			postItemUseCallback(false, results, null, this);
		}
	}
}
