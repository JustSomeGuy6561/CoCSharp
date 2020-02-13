/**
 * Created by aimozg on 15.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using System;
	using System.Text;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Items.Wearables.LowerGarment;
	using CoC.Backend.Items.Wearables.UpperGarment;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;
	using CoC.Frontend.Perks.ArmorPerks;

	public sealed class InquisitorsCorset : ArmorBase, ISluttySeductionItem
	{

		public override string AbbreviatedName() => "I.Corst";

		public override string ItemName() => "inquisitor's corset";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string corsetText = count != 1 ? "corsets" : "corset";

			string countText = displayCount ? (count == 1 ? "an" : Utils.NumberAsText(count)) : "";

			return $"{count} inquisitor's {corsetText}";
		}

		public override float DefensiveRating(Creature wearer) => 4;

		protected override int monetaryValue => 2000;

		public override string AboutItem()
		{
			return "This sexualized and feminine red and gold ensemble carries with it the hopes of a shamed sorcerer. Wearing it will cause spells to tax your " +
				"health instead of exhausting you.";
		}


		public override bool CanWearWithUpperGarment(UpperGarmentBase upperGarment, out string whyNot)
		{
			if (!(upperGarment is null))
			{
				whyNot = GenericArmorIncompatText(upperGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public override bool CanWearWithLowerGarment(LowerGarmentBase lowerGarment, out string whyNot)
		{
			if (!(lowerGarment is null))
			{
				whyNot = GenericArmorIncompatText(lowerGarment);
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}


		public InquisitorsCorset() : base(ArmorType.LIGHT_ARMOR)
		{
		}


		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("You remove the clothing you received from the underground chamber in the swamp. The sexual nature of the clothing excites you a bit, " +
				"as does the knowledge that it will be functional in combat. You quickly disrobe, eager to try it on." + GlobalStrings.NewParagraph());

			sb.Append("Checking to see whether the posture collar can be detached from the sleeves, you find a complete lack of any way to do so. " +
				"Shrugging, you slide your arms into the sleeves, sticking your middle fingers through the extended ends. Golden embroidery down their length " +
				"contains some of the most ornate and stylized pictures of dicks you have ever seen, culminating in an excessive amount of gold thread splayed in your palm, " +
				"symbolizing the ejaculate. You lift your chin to fit the posture collar in place, completely hiding your neck from view as you lace it from behind. " +
				"The dark red works nicely with your " + wearer.body.ShortEpidermisDescription() + ". A small amount of delight fills you as you realize the collar " +
				"is not as unforgiving as a true posture collar, allowing you a comfortable amount of movement - though you do briefly wonder what a fight would be like if it didn't.");

			sb.Append(GlobalStrings.NewParagraph() + "You fit the corset on next, wrapping it around your waist and chest, hiding your " + wearer.breasts[0].LongNippleDescription()
				+ " beneath more red and gold trim. As you lace the corset up you realize that what you had taken for a stylized sword rising up between the cups " +
					"bears a much greater resemblance to a prick. Fair enough. The belt practically attaches to the corset as though it were made to be a single article of clothing, " +
					"clasping beneath a set of heavily stylized golden lines that you suspect are meant to be a face in the middle of either prayer or oral sex, or possibly both.");

			sb.Append(GlobalStrings.NewParagraph() + "Earlier suspicions about the skirt prove correct - it does not conceal in the slightest. It covers some skin to the right side, " +
				"mostly in the back, but your front is on full display to the world. The skirt feels as though it should go on a dancing showgirl rather than an inquisitor, " +
				"but you made the choice you did and the results " + SafelyFormattedString.FormattedText("are", StringFormats.ITALIC) +" arousing. Golden trim takes on the form " +
				"of waving and twisting tentacles as the bottom of the skirt gets closer, looking as though it wants to burst from the fabric and molest its wearer."
				+ GlobalStrings.NewParagraph());

			//[(if PC has human, demon, or bee feet)
			if (wearer.isBiped)
			{
				sb.Append("Finally you slide your legs into the boots. The heel itself is hollow, to accommodate any natural spurs on the wearer's foot, " +
					"and the rest of the boot seems surprisingly fitted to your dimensions. To a certain extent it feels as though it is reshaping itself as you enter - " +
					"parts that initially feel tight loosen up in minutes, as if the boots were breaking themselves in. If what the note said is true, " +
					"and the armor adapts to he who finds it, perhaps even hooves and paws could have fit, given enough magical power used in the creation. " +
					"Laces run up the side of each boot all the way to your thighs, resembling the corset they were paired with. The front of them bear golden symbology, " +
					"akin to the symbols that sat on the front of the secret chamber. Based on your previous decoding, they're halfway between a prayer and an exaltation of cock. " +
					"Standing up in them, you smile as your " + wearer.build.ButtLongDescription() + " rises with their influence.");
			}

			///(else if PC has funny feet)
			else
			{
				sb.Append("The final bit of the outfit is a pair of long-heeled lace-up boots, which, though decorated with gorgeous golden symbols spelling out what appears " +
					"to be a hymn to sex, seem... relatively unusable in your current state. You tuck them away someplace safe in case you ever become a biped again, " +
					"then stand to your full height and assess yourself.");
			}

			sb.Append(GlobalStrings.NewParagraph());

			sb.Append("You feel sexy... and pious." + GlobalStrings.NewParagraph() + "(<b>Perk Gained - Blood Mage</b>: Spells consume HP (minimum 5) instead of fatigue!)"
				+ GlobalStrings.NewParagraph());

			return sb.ToString();
		}

		protected override void OnEquip(Creature wearer)
		{
			wearer.IncreaseLust(5);

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

		public byte SluttySeductionModifier(Creature wearer) => 10;

		public override string AboutItemWithStats(Creature target)
		{
			return base.AboutItemWithStats(target) + Environment.NewLine + "Special: Blood Mage and Slutty Seduction (10 Magnitude)";
		}

		public override bool Equals(ArmorBase other)
		{
			return other is InquisitorsCorset;
		}
	}
}
