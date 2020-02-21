using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Shield;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.Shield
{
	public class KiteShield : ReforgableShield
	{
		public KiteShield(ForgeTier tier) : base (ShieldType.MEDIUM, tier)
		{
		}

		public override ReforgableShield ForgesInto(ForgeTier tier)
		{
			return new KiteShield(tier);
		}

		public override double BlockRate(Creature wearer) => DefaultBlock(10);

		public override bool Equals(ShieldBase other)
		{
			return other is KiteShield kiteShield && tier == kiteShield.tier;
		}

		public override string AbbreviatedName()
		{
			return "KiteShldLv" + ((int)tier + 1);
		}

		public override string ItemName()
		{
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					return "Masterwork Kiteshield";
				case ForgeTier.REINFORCED:
					return "Fine Kiteshield";
				default:
					return "Kiteshield";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string shieldText = count == 1 ? "kiteshield" : "kiteshields";
			string tierText;
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					tierText = "masterwork ";
					break;
				case ForgeTier.REINFORCED:
					tierText = "fine ";
					break;
				default:
					tierText = " ";
					break;
			}

			return countText + tierText + shieldText;
		}

		public override string AboutItem()
		{
			return "A teardrop-shaped kiteshield made of durable wood." + AboutItemTier();
		}

		protected override int monetaryValue => DefaultPrice(300);
	}

}