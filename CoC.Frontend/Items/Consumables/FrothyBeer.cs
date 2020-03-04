using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Alcoholic beverage.
	 */
	public class FrothyBeer : StandardConsumable
	{
		public FrothyBeer() : base()
		{ }

		public override string AbbreviatedName() => "Fr Beer";
		public override string ItemName() => "Fr Beer";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "mugs" : "mug";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of frothy beer";
		}
		public override string AboutItem() => "A bottle of beer from The Black Cock.";
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override byte sateHungerAmount => 10;

		public override bool Equals(CapacityItem other)
		{
			return other is FrothyBeer;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("Feeling parched, you twist the metal cap from the clear green bottle and chug it down. ");
			consumer.DeltaCreatureStats(lus: 15);
			if (!consumer.HasTimedEffect<Drunk>())
			{
				consumer.AddTimedEffect<Drunk>();

				consumer.DeltaCreatureStats(str: 0.1);
				consumer.DeltaCreatureStats(inte: -0.5);
				consumer.DeltaCreatureStats(lib: 0.25);
			}
			else
			{
				sb.Append(consumer.GetTimedEffectData<Drunk>().StackEffect());
			}

			if (consumer.build.muscleTone < 70)
			{
				sb.Append(consumer.ModifyTone(70, (byte)Utils.Rand(3)));
			}

			if (consumer.femininity > 30)
			{
				sb.Append(consumer.ModifyFemininity(30, (byte)Utils.Rand(3)));
			}

			consumeItem = true;
			return sb.ToString();
		}
	}
}
