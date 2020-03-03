using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * Moderate boost to HP.
	 *
	 * Retro UTG stuff!
	 */
	public class HealPill : StandardConsumable
	{
		public HealPill() : base()
		{ }

		public override string AbbreviatedName() => "H. Pill";
		public override string ItemName() => "Heal Pill";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			throw;
			//"a small healing pill";
		}
		public override string AboutItem() => "A small healing pill that's guaranteed to heal you by a bit.";
		protected override int monetaryValue => DEFAULT_VALUE;

		public override bool countsAsLiquid => false;
		public override bool countsAsCum => false;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (target is CombatCreature)
			{
				whyNot = null;
				return true;
			}
			else
			{
				whyNot = "Requires you to be a combat creature. will probably never see this";
				return false;
			}
		}

		protected override bool OnConsumeAttempt(Creature creature, out string resultsOfUse, out bool isBadEnd)
		{
			var consumer = creature as CombatCreature;

			var rando = Utils.Rand(10);

			StringBuilder sb = new StringBuilder();
			sb.Append("You pop the small pill into your mouth and swallow. ");

			if (consumer.AddHP(50 + consumer.toughness, true))
			{
				sb.Append("Some of your wounds are healed. ");
			}
			else
			{
				sb.Append("You feel an odd sensation. ");
			}
			if (rando == 9)
			{
				sb.Append("You shudder as a small orgasm passes through you. When you recover you actually feel more aroused.");
				consumer.DeltaCreatureStats(lus: 5);
			}
			else if (rando >= 6)
			{
				sb.Append("Your body tingles and feels more sensitive.");
				consumer.DeltaCreatureStats(sens: 4);
			}
			else if (consumer.libido < 40)
			{
				sb.Append("You feel a sense of warmth spread through your erogenous areas.");
				consumer.DeltaCreatureStats(lib: 1);
			}

			isBadEnd = false;
			resultsOfUse = sb.ToString();
			return true;
		}

		public override byte maxCapacityPerSlot => 5;
	}
}
