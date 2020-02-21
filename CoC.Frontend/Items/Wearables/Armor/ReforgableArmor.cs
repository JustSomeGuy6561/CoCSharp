using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;

namespace CoC.Frontend.Items.Wearables.Armor
{
	public abstract class ReforgableArmor : ArmorBase
	{
		protected ForgeTier tier;

		public bool canReforge => tier < ForgeTier.MASTERWORK;


		protected ReforgableArmor(ArmorType armor, ForgeTier forgeTier) : base(armor)
		{
			tier = forgeTier;
		}

		public override string AboutItemWithStats(Creature target)
		{
			string tierStrength;

			switch (tier)
			{
				case ForgeTier.REINFORCED:
					tierStrength = " This armor has been upgraded to be of fine quality.";
					break;
				case ForgeTier.MASTERWORK:
					tierStrength = " This armor has been upgraded to be of masterwork quality.";
						break;

				case ForgeTier.STANDARD:
				default:
					tierStrength = "";
					break;
			}


			return base.AboutItemWithStats(target)
				+ tierStrength;
		}
	}
}
