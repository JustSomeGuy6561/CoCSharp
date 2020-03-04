//EchidnaCake.cs
//Description:
//Author: JustSomeGuy
//1/16/2020 5:19:26 PM

using System;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations;

namespace CoC.Frontend.Items.Consumables
{
	public sealed class EchidnaCake : StandardConsumable
	{
		public EchidnaCake() : base()
		{
		}

		public override string AbbreviatedName()
		{
			return "EchidCk";
		}

		public override string ItemName()
		{
			return "Echidna Cake";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string cakeText = count != 1 ? "cakes" : "cake";

			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}echidna {cakeText}";
		}

		public override string AboutItem()
		{
			return "The label reads: \"Try our special cake, a favorite among the echidna-morphs!" + GlobalStrings.NewParagraph() + "DISCLAIMER: We are not responsible " +
				"if you find yourself altered.\"";
		}

		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => false;
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => false;
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => 40;

		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool Equals(CapacityItem other)
		{
			return other is EchidnaCake;
		}

		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			EchidnaTfs tf = new EchidnaTfs();
			consumeItem = true;
			return tf.DoTransformation(consumer, out isBadEnd);
		}

		protected override string OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out bool consumeItem, out bool isBadEnd)
		{
			EchidnaTfs tf = new EchidnaTfs();
			consumeItem = true;
			return tf.DoTransformationFromCombat(consumer, opponent, out isBadEnd);
		}

		private class EchidnaTfs : EchidnaTransformations
		{

		}
	}
}
