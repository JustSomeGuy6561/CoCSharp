//HorseTransformations.cs
//Description:
//Author: JustSomeGuy
//1/18/2020 7:00:51 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class Equinium : StandardConsumable
	{
		public Equinium() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			return "Equinum";
		}

		public override string ItemName()
		{
			return "Equinum";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string vialText = count != 1 ? "vials" : "vial";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of Equinum";
		}

		public override string AboutItem()
		{
			return "This is a long flared vial with a small label that reads, \"<i>Equinum</i>\". It is likely this potion is tied to horses in some way.";
		}


		public override bool countsAsLiquid => true;

		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 15;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is Equinium;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			HorseTFs tf = new HorseTFs();
			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			HorseTFs tf = new HorseTFs();
			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class HorseTFs : HorseTransformations
		{
			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
