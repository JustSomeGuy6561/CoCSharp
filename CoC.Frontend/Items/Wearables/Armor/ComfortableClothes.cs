/**
 * Created by aimozg on 18.01.14.
 */
namespace CoC.Frontend.Items.Wearables.Armor
{
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Tools;

	public class ComfortableClothes : ArmorBase, IBulgeArmor
	{
		//consider giving this a boolean that lets it get upgraded in place when Exgartuan is active. it'd alter the text for this, but otherwise not affect how the item
		//works.

		private bool bulgified;

		public override string AbbreviatedName() => "C.Cloth";

		public override string ItemName() => bulgified ? "crotch-hugging clothes" : "comfortable clothes";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			string bulgifiedText = bulgified ? "crotch-hugging" : "comfortable";

			return $"{count}{setText} of {bulgifiedText} clothes";
		}

		public override float DefensiveRating(Creature wearer) => 0;

		protected override int monetaryValue => 0;

		public override string AboutItem()
		{
			return "These loose fitting and comfortable clothes allow you to move freely while protecting you from the elements." + (bulgified ? "They've been modified to hug " +
				"your crotch and proudly display your manly bulge" : "");
		}

		public ComfortableClothes() : base(ArmorType.CLOTHING)
		{
		}


		public override bool Equals(ArmorBase other)
		{
			return other is ComfortableClothes;
		}

		public bool supportsBulgeArmor => true;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (this.bulgified == bulgified)
			{
				return "";
			}

			this.bulgified = bulgified;
			if (bulgified)
			{
				return "Your clothing shifts, tightening up about your crotch until every curve and nodule of your " + wearer.genitals.AllCocksShortDescription(out bool isPlural) +
					(isPlural ? " are" : " is") + " visible through the fabric.";
			}
			else //if (bulgified)
			{
				return "Your clothing shifts, relaxing around your groin until it's no longer displaying your "+ wearer.genitals.AllCocksShortDescription(out bool isPlural) +
					"quite so prominently.";
			}
		}

		bool IBulgeArmor.isBulged => bulgified;

		protected override string OnRemoveText()
		{
			if (bulgified)
			{
				return "As you remove your clothes, you notice the modifications that made them hug your crotch and prominently display your bulge gradually disappear, " +
					"as if by magic.";
			}
			else
			{
				return base.OnRemoveText();
			}
		}

	}
}
