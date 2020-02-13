
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Tools;

/**
* Created by aimozg on 11.01.14.
*/
namespace CoC.Frontend.Items.Wearables.Armor
{
	public sealed class FurLoincloth : ArmorBase
	{
		public override string AbbreviatedName() => "FurLoin";

		public override string ItemName() => "Fur Loincloth";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			if (count == 1)
			{
				return (displayCount ? "a " : "") + "front and back set of loincloths";
			}
			else
			{
				return Utils.NumberAsText(count) + "sets of matching front and back loincloths";
			}
		}
		public override float DefensiveRating(Creature wearer) => 0;

		protected override int monetaryValue => 100;

		public override string AboutItem() => "A pair of loincloths to cover your crotch and butt. Typically worn by people named 'Conan'.";

		public FurLoincloth() : base(ArmorType.CLOTHING)
		{
		}


		public override bool Equals(ArmorBase other)
		{
			return other is FurLoincloth;
		}
	}
}
