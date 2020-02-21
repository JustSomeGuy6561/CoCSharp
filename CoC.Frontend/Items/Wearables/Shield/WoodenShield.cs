
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Shield;
using CoC.Backend.Tools;
/**
* ...
* @author Melchi ...
*/
namespace CoC.Frontend.Items.Wearables.Shield
{

	public class WoodenShield : ShieldBase
	{

		public WoodenShield() : base(ShieldType.LIGHT)
		{
		}

		public override double BlockRate(Creature wearer) => 6;


		public override bool Equals(ShieldBase other)
		{
			return other is WoodenShield;
		}

		public override string AbbreviatedName() => "WoodShld";

		public override string ItemName()
		{
			return "wood shield";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string shieldText = count == 1 ? "shield" : "shields";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			return $"{countText}wooden {shieldText}";
		}

		public override string AboutItem()
		{
			return "A crude wooden shield. It doesn't look very sturdy";
		}

		protected override int monetaryValue => 10;
	}

}