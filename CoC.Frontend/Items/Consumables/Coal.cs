//Coal.cs
//Description:
//Author: JustSomeGuy
//1/31/2020 7:09:47 AM

using System;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class Coal : StandardConsumable
	{
		public Coal() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "Coal";
		}

		public override string ItemName()
		{
			return "Coal";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			if (count == 1)
			{
				return "two pieces of coal";
			}
			else if (count == 0)
			{
				return "nonexistent coal";
			}
			else
			{
				return (displayCount ? Utils.NumberAsText(count) : "") + "sets of coal pieces";
			}
		}

		public override string AboutItem()
		{
			return "These two pieces of coal may look ordinary but it makes you wonder what happens when you rub them together.";
		}

		private string InitialItemText()
		{
			return "You handle the coal rocks experimentally and they crumble to dust in your hands! You cough as you breathe in the cloud, sputtering and wheezing. " +
				"After a minute of terrible coughing, you recover and realize there's no remaining trace of the rocks, not even a sooty stain on your hands!";
		}

		private string IncreasedAnalCapacityText(Creature consumer)
		{
			return GlobalStrings.NewParagraph() + "You feel... more accommodating somehow. Your " + consumer.ass.LongDescription() + " is tingling a bit, " +
				"and though it doesn't seem to have loosened, it has grown more elastic.";
		}

		private string NothingHappened()
		{
			return GlobalStrings.NewParagraph() + "Your whole body tingles for a moment but it passes. It doesn't look like the coal can do anything to you at this point.";
		}

		public override bool Equals(CapacityItem other)
		{
			return other is Coal;
		}


		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 0;
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;

			StringBuilder sb = new StringBuilder();
			//if this item calls a TF, remove these
			sb.Append(InitialItemText());

			if (consumer.GoIntoHeat(2, out string text))
			{
				sb.Append(text);
			}
			else if (consumer.GoIntoRut(1, out text))
			{
				sb.Append(text);
			}
			else if (consumer.genitals.standardBonusAnalCapacity < 80)
			{
				consumer.genitals.IncreaseBonusAnalCapacity(5);
				sb.Append(IncreasedAnalCapacityText(consumer));
			}
			else
			{
				sb.Append(NothingHappened());
			}

			consumeItem = true;
			return sb.ToString();
		}

		//combat consume is identical, so no need to override.


	}
}
