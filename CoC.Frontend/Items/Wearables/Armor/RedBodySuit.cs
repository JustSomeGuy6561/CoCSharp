using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;

namespace CoC.Frontend.Items.Wearables.Armor
{
	//broken out because it provides an evasion bonus.
	class RedBodySuit : ArmorBase, IBulgeArmor
	{
		private bool bulged = false;

		public RedBodySuit() : base(ArmorType.LIGHT_ARMOR)
		{
		}

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

		public override double PhysicalDefensiveRating(Creature wearer) => 1;

		public override double BonusTeaseDamage(Creature wearer) => 1;

		//provides a 5% evasion bonus when worn. This is increased if the wearer has the Misdirection perk.
		public override double EvasionRate(Creature wearer) => wearer.perks.HasPerk<Misdirection>() ? 10 : 5;

		public override bool Equals(ArmorBase other)
		{
			return other is RedBodySuit;
		}

		public override string AbbreviatedName()
		{
			return "R.BdySt";
		}

		public override string ItemName()
		{
			return "red, " + (bulged ? "crotch-hugging " : "") + " high-society bodysuit";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string bodysuitText = count != 1 ? "bodysuits" : "bodysuit";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

			string bulgeText = bulged ? ", crotch-hugging" : "";

			return $"{countText}red{bulgeText} {bodysuitText} for high society";
		}

		public override string AboutItem()
		{
			return "A high society bodysuit. It is as easy to mistake it for ballroom apparel as it is for boudoir lingerie. " +
				"The thin transparent fabric is so light and airy that it makes avoiding blows a second nature." +
				(bulged ? "Magical alterations that prominently display what your packing underneath certainly make this a risque item." : "");
		}

		//RIGID_BODYSUIT

		private string RigidBodysuitBulgeChanged(Creature wearer)
		{
			//gained a bulge.
			if (bulged)
			{
				return "The thin, transparent material of your red bodysuit begins to firmly press against your groin, perfectly shaping to your "
					+ wearer.genitals.AllCocksShortDescription(out bool isPlural) + " and every last one of " + (isPlural ? "their" : "its") + " nubs and nodules.";
			}
			//lost a bulge.
			//but still has a cock.
			else if (wearer.hasCock)
			{
				return "The material hugging your manly package" + (wearer.cocks.Count > 1 ? "s" : "") + "relaxes, returninig to its base form.";
			}
			//has no cocks (which is why it was lost, probably)
			else
			{
				return "Without any cocks to cover, the enchantment forcing the material to hug your groin disappates, reverting back to a normal red bodysuit.";
			}
		}

		protected override int monetaryValue => 1200;

		bool IBulgeArmor.supportsBulgeArmor => true;

		bool IBulgeArmor.isBulged => bulged;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (bulgified == bulged)
			{
				return "";
			}
			else
			{
				bulged = bulgified;
				return RigidBodysuitBulgeChanged(wearer);
			}
		}
	}
}
