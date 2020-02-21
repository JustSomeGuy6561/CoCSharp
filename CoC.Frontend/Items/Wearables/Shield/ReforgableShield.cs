using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Shield;

namespace CoC.Frontend.Items.Wearables.Shield
{
	public abstract class ReforgableShield : ShieldBase
	{
		protected readonly ForgeTier tier;

		public bool canReforge => tier < ForgeTier.MASTERWORK;

		protected ReforgableShield(ShieldType armor, ForgeTier forgeTier) : base(armor)
		{
			tier = forgeTier;
		}

		public abstract ReforgableShield ForgesInto(ForgeTier tier);

		public override string AboutItemWithStats(Creature target)
		{
			string tierStrength;

			switch (tier)
			{
				case ForgeTier.REINFORCED:
					tierStrength = " This shield has been upgraded to be of fine quality.";
					break;
				case ForgeTier.MASTERWORK:
					tierStrength = " This shield has been upgraded to be of masterwork quality.";
						break;

				case ForgeTier.STANDARD:
				default:
					tierStrength = "";
					break;
			}


			return base.AboutItemWithStats(target)
				+ tierStrength;
		}


		protected float DefaultBlock(float baseValue)
		{
			return baseValue + ((int)tier - 1) * 2;
		}

		protected int DefaultPrice(int basePrice)
		{
			return basePrice * (1 + (int)tier) / 2;
		}

		protected string AboutItemTier()
		{
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					return " This shield has been upgraded to be of masterwork quality.";
				case ForgeTier.REINFORCED:
					return " This shield has been upgraded to be of fine quality.";
				default:
					return "";
			}
		}
	}
}
