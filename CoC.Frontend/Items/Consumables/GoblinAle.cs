//GoblinAle.cs
//Description:
//Author: JustSomeGuy
//1/21/2020 2:45:33 AM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class GoblinAle : StandardConsumable
	{
		public GoblinAle() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.
		public override string AbbreviatedName()
		{
			return "GoblinAle";
		}

		public override string ItemName()
		{
			return "Goblin Ale";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string vialText = count != 1 ? "flagons" : "flagon";

			return $"{count}{vialText} of potent goblin ale";
		}

		public override string AboutItem()
		{
			return "This sealed flagon of 'Goblin Ale' sloshes noisily with alcoholic brew. Judging by the markings on the flagon, it's a VERY strong drink, " +
				"and not to be trifled with.";
		}

		//it's liquid, sure. but... it's also alcohol. i guess it just has no effect on poor old diapause preggo baby?!?
		public override bool countsAsLiquid => true;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 15;

		//note that this value is half of the value you get selling it to Oswald.
		//A constant, DEFAULT_VALUE, is defined in the base class as 6. if this is fine, simply replace the throw with DEFAULT_VALUE
		protected override int monetaryValue => DEFAULT_VALUE;

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is GoblinAle;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			GoblinTFs tf = new GoblinTFs();
			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			GoblinTFs tf = new GoblinTFs();
			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class GoblinTFs : GoblinTransformations
		{
			public GoblinTFs()
			{
			}

			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
