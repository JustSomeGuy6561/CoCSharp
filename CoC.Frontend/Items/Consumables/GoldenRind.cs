//GoldenRind.cs
//Description:
//Author: JustSomeGuy
//1/23/2020 4:10:22 AM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class GoldenRind : StandardConsumable
	{
		public GoldenRind() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "GldnRind";
		}

		public override string ItemName()
		{
			return "Golden Rind";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			//when the text below is corrected, remove this throw.
			string itemText = count == 1 ? "rind" : "rinds";

			return $"{count}golden {itemText}";
		}

		public override string AboutItem()
		{
			return "This shimmering, citrus peel is shaped like a corkscrew and smells sweet and sour at the same time.";
		}

		public override bool countsAsLiquid => false;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 10;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is GoldenRind;
		}

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//what happens when we try to use this item? note that it's unlikely, but possible, for this to be called if CanUse returns false.
		//you need to handle that, yourself, and you'll probably want to return some unique text saying you cant do it, you tried anyway,
		//and looked really dumb or something of the like
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new DeerTFs();
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new DeerTFs();
			resultsOfUse = tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);

			return true;
		}

		private class DeerTFs : DeerTransformations
		{
			public DeerTFs()
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
