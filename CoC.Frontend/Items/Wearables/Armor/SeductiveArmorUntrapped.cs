namespace CoC.Frontend.Items.Wearables.Armor
{
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;

	public class SeductiveArmorUntrapped : ArmorBase
	{

		protected override string EquipText(Creature wearer)
		{
			return GlobalStrings.NewParagraph() + "You are relieved to find out that there is no curse on this armor, and you can equip it without " +
				"it turning into a sudden and unwelcome pair of nipple piercings.";
		}

		public override string AbbreviatedName() => "SeductU";

		public override string ItemName() => "Seductive Armor (Inert)";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{setText} of (inert) scandalously seductive armor";
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 10;

		protected override int monetaryValue => 1;

		public override string AboutItem()
		{
			return "A complete suit of scalemail shaped to hug tightly against every curve, it has a solid steel chest-plate with obscenely large nipples molded into it. " +
				"The armor does nothing to cover the backside, exposing the wearer's cheeks to the world. Fortunately, it seems this version isn't cursed, so you won't " +
				"have to worry about it collapsing on itself or giving you a set of nipple-piercings.";
		}

		public SeductiveArmorUntrapped() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		public override double BonusTeaseDamage(Creature wearer) => 5;

		public override bool Equals(ArmorBase other)
		{
			return other is SeductiveArmorUntrapped;
		}

	}

}