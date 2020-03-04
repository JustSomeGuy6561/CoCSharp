//Ectoplasm.cs
//Description:
//Author: JustSomeGuy
//1/16/2020 10:42:10 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;
using CoC.Frontend.UI;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class Ectoplasm : StandardConsumable
	{
		public Ectoplasm() : base()
		{
		}

		public override string AbbreviatedName()
		{
			return "EctoPls";
		}

		public override string ItemName()
		{
			return "Ectoplasm";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string bottleText = count != 1 ? "bottles" : "bottle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{bottleText} of ectoplasm";
		}

		public override string AboutItem()
		{
			return "The green-tinted, hardly corporeal substance flows like a liquid inside its container. It makes you feel... uncomfortable, as you observe it.";
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;
		public override byte sateHungerAmount => 20;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is Ectoplasm;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			GhostTF tf = new GhostTF();

			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			GhostTF tf = new GhostTF();

			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class GhostTF : GhostTransformations
		{
			protected override string InitialTransformationText(Creature target)
			{
				throw new NotImplementedException();
			}
		}
	}
}
