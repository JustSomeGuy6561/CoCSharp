using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Honey based alcoholic beverage.
	 */
	public class GodMead : StandardConsumable
	{
		private bool silly => SillyModeSettings.isEnabled;

		public GodMead() : base()
		{ }

		public override string AbbreviatedName() => "GodMead";
		public override string ItemName() => "GodMead";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "pints" : "pint";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of god's mead";
		}
		public override string AboutItem() => "A horn of potent, honey-colored mead. A single whiff makes your head swim and your thoughts turn to violence and heroism.";
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override bool Equals(CapacityItem other)
		{
			return other is GodMead;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You take a hearty swig of mead, savoring the honeyed taste on your tongue. Emboldened by the first drink, you chug the remainder of the horn's contents in no time flat. You wipe your lips, satisfied, and let off a small belch as you toss the empty horn aside. ");

			//Libido: No desc., always increases.
			//Corruption: No desc., always decreases.
			consumer.DeltaCreatureStats(lib: 1, corr: -1);
			//Health/HP(Large increase; always occurs):
			sb.Append(GlobalStrings.NewParagraph() + "You feel suddenly invigorated by the potent beverage, like you could take on a whole horde of barbarians or giants and come out victorious! ");
			(consumer as CombatCreature)?.AddHPPercent(.33);
			if (Utils.Rand(3) == 0)
			{
				sb.Append(GlobalStrings.NewParagraph() + "The alcohol fills your limbs with vigor, making you feel like you could take on the world with just your fists!");
				if (silly)
				{
					sb.Append(" Maybe you should run around shirtless, drink, and fight! Saxton Hale would be proud.");
				}

				consumer.DeltaCreatureStats(str: 1);
			}
			//Tough:
			else
			{
				sb.Append(GlobalStrings.NewParagraph() + "You thump your chest and grin - your foes will have a harder time taking you down while you're fortified by liquid courage.");
				consumer.DeltaCreatureStats(tou: 1);
			}
			////Grow Beard [ONLY if PC has a masculine face & a dick.)( -- Why? Bearded ladies are also a fetish [That's just nasty.] (I want a lady beard)): A sudden tingling runs along your chin. You rub it with your hand, and find a thin layer of bristles covering your lower face. You now sport a fine [consumer.HairColor] beard!
			////MOD: i'm with the no lady-beard group, but maybe compromise by using masculinity/androgeny perk. if you have androgeny, any creature can get it. if not, limit to masculine.
			////Regardless, as of this moment beards aren't a thing (yet, working on it)
#warning implement when beards are a thing.
			//if (Utils.Rand(6) == 0 && consumer.beard.Count < 4)
			//{
			//	if (consumer.beard.Count <= 0)
			//	{
			//		sb.Append("A sudden tingling runs along your chin. You rub it with your hand, and find a thin layer of bristles covering your lower face. <b>You now sport a fine " + consumer.hair.color + " beard!</b>");
			//	}
			//	else
			//	{
			//		sb.Append(GlobalStrings.NewParagraph() + "A sudden tingling runs along your chin. You stroke your beard proudly as it slowly grows in length and lustre.");
			//	}

			//	consumer.beard.length += 0.5;
			//}

			//Grow hair: Your scalp is beset by pins and needles as your hair grows out, stopping after it reaches [medium/long] length.}

			isBadEnd = false;
			consumeItem = true;
			return sb.ToString();
		}
		public override byte sateHungerAmount => 20;

		public override byte maxCapacityPerSlot => 5;
	}
}
