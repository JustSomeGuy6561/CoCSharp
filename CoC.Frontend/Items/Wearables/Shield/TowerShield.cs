namespace CoC.Frontend.Items.Wearables.Shield
{
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Shield;
	using CoC.Backend.Tools;

	public class TowerShield : ReforgableShield
	{
		public TowerShield(ForgeTier tier) : base(ShieldType.HEAVY, tier)
		{}

		public override string AbbreviatedName()
		{
			return "TwrShieldLv" + ((int)tier + 1);
		}

		public override string ItemName()
		{
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					return "Masterwork Tower Shield";
				case ForgeTier.REINFORCED:
					return "Reinforced Tower Shield";
				default:
					return "Tower Shield";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string shieldText = count == 1 ? "shield" : "shields";
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

			return countText + tierText + "towering " + shieldText;
		}


		public override string AboutItem()
		{
			return "A towering metal shield. It looks heavy! The weight of this shield might incite some penalties to accuracy and evasion." + AboutItemTier();
		}

		public override ReforgableShield ForgesInto(ForgeTier tier)
		{
			return new TowerShield(tier);
		}

		public override bool Equals(ShieldBase other)
		{
			return other is TowerShield towerShield && tier == towerShield.tier;
		}

		protected override int monetaryValue => DefaultPrice(500);

		public override double BlockRate(Creature wearer) => DefaultBlock(16);


		protected override bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			if (creature.strength < 40)
			{
				whyNot = TooHeavy();
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		private string TooHeavy()
		{
			return "This shield is too heavy for you to hold effectively. Perhaps you should try again when you have at least 40 strength?";
		}
	}

}