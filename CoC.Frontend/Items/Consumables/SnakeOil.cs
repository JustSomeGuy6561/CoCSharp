﻿//SnakeOil.cs
//Description:
//Author: JustSomeGuy
//2/10/2020 12:02:34 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class SnakeOil : StandardConsumable
	{
		public SnakeOil() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "SnakeOil";
		}

		public override string ItemName()
		{
			return "snake oil";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string vialText = count != 1 ? "vials" : "vial";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of snake oil";
		}

		public override string AboutItem()
		{
			return "A vial the size of your fist made of dark brown glass. It contains what appears to be an oily, yellowish liquid. The odor is abominable.";
		}


		public override bool Equals(CapacityItem other)
		{
			return other is SnakeOil;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 5;
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			var tf = new NagaTFs();
			resultsOfUse = tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			var tf = new NagaTFs();
			resultsOfUse = tf.DoTransformationFromCombat(consumer, out isCombatLoss, out isBadEnd);

			return true;
		}

		private class NagaTFs : NagaTransformations
		{
			protected override string InitialTransformationText(Creature target, bool currentlyInCombat)
			{
				throw new NotImplementedException();
			}
		}
	}
}
