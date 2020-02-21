
using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Shield;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures;
using CoC.Frontend.Creatures.NPCs;
/**
* Created by aimozg on 10.01.14.
*/
namespace CoC.Frontend.Items.Wearables.Shield
{
	public class DragonShellShield : ReforgableShield
	{
#warning this is supposed to have a combat perk negating certain attacks. as far as i can tell, this was never implemented.


		public DragonShellShield(bool upgraded) : base(ShieldType.MEDIUM, upgraded ? ForgeTier.MASTERWORK : ForgeTier.STANDARD)
		{
		}

		public override ReforgableShield ForgesInto(ForgeTier tier)
		{
			return new DragonShellShield(tier == ForgeTier.MASTERWORK);
		}

		public override string AbbreviatedName()
		{
			return tier != ForgeTier.MASTERWORK ? "DrgnShell" : "R.DrgnShl";
		}

		public override string ItemName()
		{
			return (tier == ForgeTier.MASTERWORK ? "runic " : "") + "dragon-shell shield";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string shieldText = count == 1 ? "shield" : "shields";
			string tierText = tier == ForgeTier.MASTERWORK ? "runed " : "";

			return countText + tierText + "dragon-shell " + shieldText;
		}



		public override string AboutItem()
		{
			return "a durable shield forged from a dragon eggshell. Absorbs any fluid attacks that hit it, rendering them useless.";
		}

		public override string AboutItemWithStats(Creature target)
		{
				string desc = Ember.hatched
				? "A durable shield that has been forged from the dragon eggshell Ember gave you for maxing out " + (Ember.gender.HasFlag(Gender.FEMALE) ? "her" : "his") + " affection."
				: "A durable shield that has been forged from the remains of the dragon egg you found in the swamp.";

				desc += " Absorbs any fluid attacks you can catch, rendering them useless.";
				if (tier > 0) desc += " This shield has since been enhanced and now intricate glowing runes surround the edges in addition to more imposing spiky appearance.";
				//Type
				desc += GlobalStrings.NewParagraph() + "Type: Shield";
				//Block Rating
				desc += Environment.NewLine + "Block: " + BlockRate(target);
				//Value
				desc += Environment.NewLine + "Base value: " + monetaryValue;
				return desc;
		}

		public override bool Equals(ShieldBase other)
		{
			return other is DragonShellShield dragonShell && dragonShell.tier == tier;
		}

		protected override int monetaryValue => tier == ForgeTier.MASTERWORK ? 2250 : 1500;

		public override double BlockRate(Creature wearer) => DefaultBlock(14);

		protected override string EquipText(Creature wearer)
		{
			if (wearer is IExtendedCreature extended && !extended.extendedData.everUsedDragonshellShield)
			{
				extended.extendedData.everUsedDragonshellShield = true;

				StringBuilder sb = new StringBuilder();
				sb.Append("Turning the sturdy shield over in inspection, you satisfy yourself as to its craftsmanship and adjust the straps to fit your arm snugly. " +
					"You try a few practice swings, but find yourself overbalancing at each one due to the deceptive lightness of the material. " +
					"Eventually, though, you pick up the knack of putting enough weight behind it to speed it through the air while thrusting a leg forward " +
					"to stabilize yourself, and try bashing a nearby rock with it. You smile with glee as ");

				if (wearer.strength < 80) sb.Append("bits and pieces from the surface of the");
				else sb.Append("huge shards of the shattered");

				sb.Append(" rock are sent flying in all directions.");

				sb.Append(GlobalStrings.NewParagraph() + "After a few more practice bashes and shifts to acquaint yourself with its weight, " +
					"you think you're ready to try facing an enemy with your new protection. One last thing... taking off the shield and turning it straps-down, " +
					"you spit onto the surface. Satisfyingly, the liquid disappears into the shell as soon as it touches.");

				return sb.ToString();
			}
			else
			{
				return base.EquipText(wearer);
			}
		}
	}
}
