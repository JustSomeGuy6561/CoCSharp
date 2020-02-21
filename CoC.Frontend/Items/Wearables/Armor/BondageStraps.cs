using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.Armor
{
	class BondageStraps : ArmorBase
	{
		public BondageStraps() : base(ArmorType.LIGHT_ARMOR)
		{
		}


		public override double PhysicalDefensiveRating(Creature wearer) => 0;

		public override double BonusTeaseDamage(Creature wearer) => 10;

		public override bool CanWearWithLowerGarment(Creature wearer, LowerGarmentBase lowerGarment, out string whyNot)
		{
			whyNot = GenericArmorIncompatText(lowerGarment);
			return false;
		}

		public override bool CanWearWithUpperGarment(Creature wearer, UpperGarmentBase upperGarment, out string whyNot)
		{
			whyNot = GenericArmorIncompatText(upperGarment);
			return false;
		}

		public override string AbbreviatedName() => "BonStrp";


		public override string ItemName() => "barely-decent bondage straps";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			return $"{countText}{setText} of bondage straps";
		}

		public override string AboutItem() => "These leather straps and well-placed hooks are actually designed in such a way as to be worn as clothing. " +
					"While they technically would cover your naughty bits, virtually every other inch of your body would be exposed.";
		protected override int monetaryValue => 700;

		public override bool isNearlyNaked => true;

		public override bool Equals(ArmorBase other)
		{
			return other is BondageStraps;
		}





	}
}
