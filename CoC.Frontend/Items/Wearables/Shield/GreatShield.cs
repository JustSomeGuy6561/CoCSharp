namespace CoC.Frontend.Items.Wearables.Shield
{
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Shield;
	using CoC.Backend.Tools;

	public class GreatShield : ReforgableShield
	{
		public GreatShield(ForgeTier tier) : base(ShieldType.MEDIUM, tier)
		{
		}

		public override ReforgableShield ForgesInto(ForgeTier tier)
		{
			return new GreatShield(tier);
		}

		public override double BlockRate(Creature wearer) => DefaultBlock(10);

		public override bool Equals(ShieldBase other)
		{
			return other is GreatShield greatShield && tier == greatShield.tier;
		}

		public override string AbbreviatedName()
		{
			return "GreatShdLv" + ((int)tier + 1);
		}

		public override string ItemName()
		{
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					return "Masterwork Greatshield";
				case ForgeTier.REINFORCED:
					return "Fine Greatshield";
				default:
					return "Greatshield";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string shieldText = count == 1 ? "greatshield" : "greatshields";
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
			return "A large, rectangular metal shield. It's a bit heavy." + AboutItemTier();
		}

		protected override int monetaryValue => DefaultPrice(300);
	}

}