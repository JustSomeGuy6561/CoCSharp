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
	class ChainmailBikini : ArmorBase, IBulgeArmor
	{
		private bool bulged;

		public ChainmailBikini() : base(ArmorType.LIGHT_ARMOR)
		{
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 2;

		public override double BonusTeaseDamage(Creature wearer) => 5;

		public override string AbbreviatedName() => "ChBikni";

		public override string ItemName()
		{
			return "revealing" + (bulged ? ", crotch-hugging" : "") + " chainmail bikini";
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string bikiniText = count != 1 ? "bikinis" : "bikini";
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string bulgeText = bulged ? "crotch-hugging " : "";

			return $"{countText}{bulgeText}chainmail {bikiniText}";
		}

		public override string AboutItem()
		{
			return "A revealing chainmail bikini that barely covers anything. The bottom half is little more than a triangle of metal and a leather thong."
				+ (bulged ? " It provides even less protection now that it's been magically altered to further display the wearer's junk, " +
				"though perhaps you could use that to your advantage" : "");
		}

		protected override int monetaryValue => 700;

		public override bool isNearlyNaked => true;

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

		public override bool Equals(ArmorBase other)
		{
			return other is ChainmailBikini;
		}

		protected override string RemoveText(Creature wearer)
		{
			return this.GenericBulgeAwareRemoveText(bulged, base.RemoveText(wearer));
		}

		public bool supportsBulgeArmor => true;

		bool IBulgeArmor.isBulged => bulged;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (bulged == bulgified)
			{
				return "";
			}
			else
			{
				bulged = bulgified;
				return ChainmailBikiniBulgeChanged(wearer);

			}
		}

		private string ChainmailBikiniBulgeChanged(Creature wearer)
		{
			if (bulged)
			{
				return "Your chainmail bikini rearranges and bends its interlocking rings to best shape itself around your " + wearer.genitals.AllCocksShortDescription()
					+ ", leaving very little else to the imagination.";
			}
			else
			{
				return "Your chainmail bikini rearranges and bends its interlocking rings, reforming until its shape is uniform once again.";
			}
		}
	}
}
