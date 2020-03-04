//FerretFruit.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 9:05:07 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class FerretFruit : StandardConsumable
	{
		public FerretFruit() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "FerretFrt";
		}

		public override string ItemName()
		{
			return "ferret fruit";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			string fruitText = count != 1 ? "fruits" : "fruit";
			return $"{count}ferret {fruitText}";
		}

		public override string AboutItem()
		{
			return "This fruit is curved oddly, just like the tree it came from. The skin is fuzzy and brown, like the skin of a peach.";
		}

		private string RottenFruitOrSomething(Creature consumer)
		{
			return GlobalStrings.NewParagraph() + "Seems like nothing else happened. Was the fruit spoiled?";
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is FerretFruit;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			//fun fact: Ferret Fruit has a 1/100 chance of doing nothing.
			if (Utils.Rand(100) == 0)
			{
				isBadEnd = false;
				resultsOfUse = RottenFruitOrSomething(consumer);
			}
			else
			{
				FerretTFs tf = new FerretTFs();
				resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);
			}
			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isBadEnd)
		{
			//fun fact: Ferret Fruit has a 1/100 chance of doing nothing.
			if (Utils.Rand(100) == 0)
			{
				isBadEnd = false;
				resultsOfUse = RottenFruitOrSomething(consumer);
			}
			else
			{
				FerretTFs tf = new FerretTFs();
				resultsOfUse = tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
			}
			return true;
		}

		public override string Author()
		{
			return "Coalsack (revision)";
		}

		private class FerretTFs : FerretTransformations
		{
			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
