/**
 * Created by aimozg on 11.01.14.
 */
using System;
using System.Text;
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
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
			string itemText = count != 1 ? "needles" : "needle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{itemText} filled with Gro+";
			//"a needle filled with Gro+";
		}
		public override string AboutItem() => "This is a small needle with a reservoir full of blue liquid. A faded label marks it as \"Gro+\". Its purpose seems obvious.";
		protected override int monetaryValue => 50;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//		public override bool hasSubMenu() { return true; } //Only GroPlus and Reducto use this.

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 0;
		protected override DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback)
		{

			var display = new StandardDisplay();
			display.OutputText("You ponder the needle in your hand knowing it will enlarge the injection site. What part of your body will you use it on? ");

			var listMaker = new ButtonListMaker(display);
			foreach (var bodyPart in consumer.bodyParts)
			{
				if (bodyPart is IGrowable growable)
				{
					listMaker.AddButtonToList(bodyPart.BodyPartName(), growable.CanGroPlus(), () => ApplyGrowPlus(consumer, growable, postItemUseCallback));
				}
			}

			return display;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is GroPlus;
		}

		private void ApplyGrowPlus(Creature consumer, IGrowable growable, UseItemCallback postItemUseCallback)
		{
			consumer.IncreaseLust(10);

			postItemUseCallback(true, growable.UseGroPlus(), null, null);
		}

		private void GrowPlusCancel(Creature consumer, UseItemCallback postItemUseCallback)
		{
			string results = "You put the vial away." + GlobalStrings.NewParagraph();
			postItemUseCallback(false, results, null, this);
		}
	}
}
