namespace CoC.Frontend.Items.Wearables.Shield
{
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Shield;
	using CoC.Backend.Tools;

	public class Buckler : ReforgableShield
	{
		public Buckler(ForgeTier tier) : base(ShieldType.LIGHT, tier)
		{ }

		public override ReforgableShield ForgesInto(ForgeTier tier)
		{
			return new Buckler(tier);
		}

		public override double BlockRate(Creature wearer)
		{
			return DefaultBlock(6);
		}

		public override bool Equals(ShieldBase other)
		{
			return other is Buckler buckler && tier == buckler.tier;
		}

		public override string AbbreviatedName()
		{
			return "BucklerLv" + ((int)tier + 1);
		}

		public override string ItemName()
		{
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					return "masterwork buckler";
				case ForgeTier.REINFORCED:
					return "fine buckler";
				default:
					return "wooden buckler";
			}
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
			string bucklerText = count == 1 ? "buckler" : "bucklers";
			string tierText;
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					tierText= "masterwork, metal ";
					break;
				case ForgeTier.REINFORCED:
					tierText = "fine, reinforced ";
					break;
				default:
					tierText = "wooden ";
					break;
			}

			return countText + tierText + bucklerText;
		}

		public override string AboutItem()
		{
			string tierText;
			switch (tier)
			{
				case ForgeTier.MASTERWORK:
					tierText = "metal";
					break;
				case ForgeTier.REINFORCED:
					tierText = "reinforced, wooden";
					break;
				default:
					tierText = "wooden";
					break;
			}


			return "A small, " + tierText + " rounded shield that's light and easy to hold. Doesn't offer much protection but it's better than nothing." +
				AboutItemTier();
		}

		protected override int monetaryValue => DefaultPrice(50);
	}

}