//BeeHoney.cs
//Description:
//Author: JustSomeGuy
//1/27/2020 10:48:50 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class BeeHoney : ConsumableBase
	{
		private readonly BeeModifiers modifier;
		private bool isPure => modifier == BeeModifiers.PURE;
		private bool isSpecial => modifier == BeeModifiers.SPECIAL;

		internal BeeHoney(BeeModifiers beeModifiers) : base()
		{
			modifier = beeModifiers;
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			if (isPure) return "PurHoney";
			else if (isSpecial) return "SpHoney";
			else return "BeeHoney";
		}

		public override string ItemName()
		{
			if (isPure) return "Pure Honey";
			else if (isSpecial) return "Special Honey";
			else return "Bee Honey";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText, vialText, itemText;
			//all use 'a', so we're fine.
			countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";

			if (isSpecial)
			{
				vialText = count == 1 ? "bottle" : "bottles";
				itemText = "of special bee honey";
			}
			else if (isPure)
			{
				vialText = count == 1 ? "crystal vial" : "crystal vials";
				itemText = "filled with glittering honey";
			}
			else
			{
				vialText = count == 1 ? "small vial filled" : "small vials of";
				itemText = "with giant-bee honey";
			}

			return $"{count} {vialText} {itemText}";
		}

		public override string Appearance()
		{
			if (isSpecial)
			{
				return "A clear crystal bottle of a dark brown fluid that you got from the bee handmaiden. It gives off a strong sweet smell even though the bottle is still corked.";
			}
			else
			{
				return "This fine crystal vial is filled with a thick amber liquid that glitters " + (isPure ? "" : "dully ") + "in the light. You can smell a sweet scent, " +
					"even though it is tightly corked.";
			}
		}

		public override bool Equals(CapacityItem other)
		{
			return other is BeeHoney beeHoney && beeHoney.modifier == this.modifier;
		}



		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 15;
		protected override int monetaryValue => isSpecial ? 40 : (isPure ? 20 : DEFAULT_VALUE);

		public override bool CanUse(Creature target, out string whyNot)
		{
#warning Implement Exgartuan honey block if creature has exgartuan possession.
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			if (!CanUse(consumer, out resultsOfUse))
			{
				isBadEnd = false;
				return false;
			}

#warning: A phouka pregnancy will intercept this item. add a virtual pregnancy item intercept function to pregnancy base, and implement it in consumable base class.
//only call on consume attempt if it does not intercept the item.

			var tf = new BeeTFs(modifier);

			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			return true;
		}

		private class BeeTFs : BeeTransformations
		{
			public BeeTFs(BeeModifiers beeModifier) : base(beeModifier)
			{
			}

			protected override bool InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
