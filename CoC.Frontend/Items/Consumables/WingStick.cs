/**
 * Created by aimozg on 11.01.14.
 */
using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;


namespace CoC.Frontend.Items.Consumables
{
	//i hate this item. but now combat checks take into consideration the opponent. woo!
	public sealed class WingStick : StandardConsumable
	{

		public WingStick() : base()
		{ }

		public override string AbbreviatedName() => "W.Stick";
		public override string ItemName() => "Wingstick";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string itemText = count != 1 ? "wingsticks" : "wingstick";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{itemText}";
		}

		public override string AboutItem() => "A tri-bladed throwing weapon. Though good for only a single use, it's guaranteed to do high damage if it hits. Inflicts 40 to 100 base damage, affected by strength.";

		protected override int monetaryValue => 16;

		public override bool countsAsCum => false;
		public override bool countsAsLiquid => false;
		public override byte sateHungerAmount => 0;

		public override bool Equals(CapacityItem other)
		{
			return other is WingStick;
		}

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (currentlyInCombat)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = "There's no one to throw it at!";
				return false;
			}
		}

		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			resultsOfUse = "You throw the wingstick at the ground absent-mindedly, though fortunately with enough presence of mind not to shatter it while doing so." +
				"You quickly pick it back up.";
			isBadEnd = false;
			return false;
		}

		protected override bool OnCombatConsumeAttempt(CombatCreature consumer, CombatCreature opponent, out string resultsOfUse, out bool isCombatLoss, out bool isBadEnd)
		{
			isCombatLoss = false;
			isBadEnd = false;

			StringBuilder sb = new StringBuilder();
			sb.Append("You toss a wingstick at your foe! It flies straight and true, almost as if it has a mind of its own as it arcs towards " +
				opponent.Article(true) + opponent.name + "!" + Environment.NewLine);
			if (opponent.speed - 80 > Utils.Rand(100) + 1)
			{ //1% dodge for each point of speed over 80
				sb.Append("Somehow " + opponent.Article(true) + opponent.name + "'");

				sb.Append(" incredible speed allows " + opponent.objectNoun + " to avoid the spinning blades! The deadly device shatters when it impacts " +
					"something in the distance.");
			}
			else
			{ //Not dodged
				uint damage = (uint)(40 + Utils.Rand(61) + (consumer.strength * 2));
				sb.Append(opponent.Article(true).CapitalizeFirstLetter() + opponent.name + " is hit with the wingstick! It breaks apart as it lacerates " +
					opponent.objectNoun + ". <b>(<font color=\"#800000\">" + damage + "</font>)</b>");
				opponent.TakeDamage(damage);
			}

			resultsOfUse = sb.ToString();

			return true;
		}



		public override byte maxCapacityPerSlot => 20;
	}
}
