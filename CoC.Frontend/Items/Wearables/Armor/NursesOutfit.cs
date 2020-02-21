using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.Armor
{
	class NursesOutfit : ArmorBase
	{
		public NursesOutfit() : base(ArmorType.LIGHT_ARMOR)
		{
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 0;

		public override double BonusTeaseDamage(Creature wearer) => 8;

		//default is 0. this provides an additional 10% health gain.
		public override double BonusHealingMultiplier(Creature wearer) => 0.1;

		public override string AbbreviatedName() => "NurseCl";
		public override string ItemName() => "skimpy nurse's outfit";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			if (count == 1)
			{
				string countText = displayCount ? "a " : "";
				return $"{countText}nurse's outfit";
			}
			else
			{
				string countText = displayCount ? Utils.NumberAsText(count) + " " : "";
				return $"{countText}sets of nurse's outfits";
			}
		}

		public override string AboutItem()
		{
			return "This borderline obscene nurse's outfit would barely cover your hips and crotch. The midriff is totally exposed, and the white top " +
				"leaves plenty of room for cleavage. A tiny white hat tops off the whole ensemble. It would grant a small regeneration to your HP.";
		}

		public override bool Equals(ArmorBase other)
		{
			return other is NursesOutfit;
		}

		protected override int monetaryValue => 800;

	}
}
