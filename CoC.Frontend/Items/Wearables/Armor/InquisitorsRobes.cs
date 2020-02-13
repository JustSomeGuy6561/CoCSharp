/**
 * Created by aimozg on 15.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using System;
	using System.Text;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;
	using CoC.Frontend.Perks.ArmorPerks;

	public sealed class InquisitorsRobes : ArmorBase
	{

		public override string AbbreviatedName() => "I.Robes";

		public override string ItemName() => "inquisitor's robes";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{setText} of inquisitor's robes";
		}
		public override float DefensiveRating(Creature wearer) => 8;

		protected override int monetaryValue => 2000;

		public override string AboutItem() => "These foreboding red and gold robes are embroidered with the symbols of a lost kingdom. Wearing them will cause spells to tax your health instead of exhausting you.";

		public InquisitorsRobes() : base(ArmorType.LIGHT_ARMOR)
		{
		}

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You unfold the robes you received from the secret chamber in the swamp and inspect them. They have not changed since the last time you saw them - perhaps the transformative magic promised in the letter has been exhausted. Looking at the two separate parts to the outfit, it becomes clear that the mantle is constructed of a thicker fabric and is intended to be the primary protection of the outfit - what protection a robe can provide, at least. The undershirt is made of a much lighter material, and you dare say that it could prove quite a classy number on its own. You strip naked and then slip into the robe." + GlobalStrings.NewParagraph() + "");

			sb.Append("The degree to which it fits you is moderately surprising. For lack of a better word, it seems to be perfect. The fabric does not cling to you, but gives you a full range of movement. There is a clasp over the high collar, displaying a golden sword. Though your arms are bare the holes through which your arms extend are comfortable, and have the same golden trim as the collar. Along the middle of the robe the trim gathers around the waist, descending down the skirt in two lines. As it reaches the bottom it explodes into elaborate embroidery circling around the back, patterning based on holy symbols and iconography that may have meant something long ago before the advent of demons. Between the two lines of gold a sword is displayed, similar to the one on the collar's clasp. You take a few dramatic movements to see how it responds, and find that you continue to enjoy free movement." + GlobalStrings.NewParagraph() + "");

			sb.Append("Taking the heavier coat, you slide your hands into the sleeves, and secure the belt firmly around your waist. Your initial concern was that the sleeves would be too open, but in making a few quick motions with your hands you don't feel that the cloth gets in the way. The weight of the gold-trimmed hood surprises you somewhat, but you quickly grow accustomed. After attempting to move the hood down you realize that doing so is remarkably difficult; it's designed by clever stitching and wires to stay up, and straight. You suppose that unless you're overheating there's no real need to adjust it. The coat covers the undershirt's waist decorations, hiding them completely behind its belt. Now-familiar sword imagery runs over your back, along your spine. The loops of the belt meet twice - once behind your back, and once beneath the clasp." + GlobalStrings.NewParagraph() + "");

			sb.Append("To finish the look, you take the two fingerless alchemical gloves and slide them over your hands. What seems to be a prayer is embroidered in gold on their back." + GlobalStrings.NewParagraph() + "");

			sb.Append("You feel pious." + GlobalStrings.NewParagraph() + "(<b>Perk Gained - Blood Mage</b>: Spells consume HP (minimum 5) instead of fatigue!)" + GlobalStrings.NewParagraph() + "");

			return sb.ToString();
		}

		public override string AboutItemWithStats(Creature target)
		{
			return base.AboutItemWithStats(target) + Environment.NewLine + "Special: Blood Mage";
		}

		protected override void OnEquip(Creature wearer)
		{
			if (!wearer.HasPerk<BloodMage>())
			{
				wearer.AddPerk<BloodMage>();
			}
		}

		protected override void OnRemove(Creature wearer)
		{
			if (wearer.HasPerk<BloodMage>())
			{
				wearer.RemovePerk<BloodMage>();
			}
		}

		public override bool Equals(ArmorBase other)
		{
			return other is InquisitorsRobes;
		}
	}
}
