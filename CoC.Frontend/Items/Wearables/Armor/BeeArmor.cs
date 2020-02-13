namespace CoC.Frontend.Items.Wearables.Armor
{
	using System.Text;
	using CoC.Backend.BodyParts;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;
	using CoC.Frontend.Creatures;
	using CoC.Frontend.Perks;

	public sealed class BeeArmor : ArmorBase, IBulgeArmor
	{
		private bool bulged;

		public BeeArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		public override string AbbreviatedName() => "Bee Armor";

		public override string ItemName() => (bulged ? "crotch-hugging, " : "") + "sexy black chitin armor-plating";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "sets" : "set";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string crotchText = bulged ? "crotch-hugging " : "";
			return $"{count}{setText} of {crotchText}chitinous armor";
		}

		public override float DefensiveRating(Creature wearer) => 18;

		protected override int monetaryValue => 200;

		public override string AboutItem()
		{
			string modestyText = bulged
				? "The silk loincloth originally designed to help protect your modesty has been modified to tightly hug your man bulge, prominently displaying it instead"
				: "It comes with a silken loincloth to protect your modesty.";

			return "A suit of armor cleverly fashioned from giant bee chitin. " + modestyText;
		}

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("You" + (wearer.wearingAnything ? " first strip yourself naked and " : "") + " equip your armor, one piece at a time. " + GlobalStrings.NewParagraph() +
				"First, you clamber into the breastplate. ");
			if (wearer.isBiped) //Some variants.
			{
				if (wearer.lowerBody.type == LowerBodyType.HUMAN)
				{
					sb.Append("Then you put your feet into your boots. With the boots fully equipped, you move on to the next piece. ");
				}
				else
				{
					sb.Append("Then you attempt to put your feet into your boots. You realize that the boots are designed for someone with normal feet. " +
						"You have to modify the boots to fit and when you do put on your boots, your feet are exposed. ");
				}
			}
			sb.Append("Next, you put on your chitinous bracers to protect your arms." + GlobalStrings.NewParagraph());
			if (!wearer.isQuadruped)
			{
				sb.Append("Last but not least, you put your silken loincloth on to cover your groin. You thank Rathazul for that and you know that you easily have access to your ");

				if (wearer.gender != Gender.GENDERLESS)
				{
					if (wearer.hasCock)
					{
						sb.Append(wearer.genitals.AllCocksShortDescription());
					}

					if (wearer.hasCock && wearer.hasVagina)
					{
						sb.Append(" and ");
					}

					if (wearer.hasVagina)
					{
						sb.Append(wearer.genitals.AllVaginasShortDescription());
					}
				}
				//Genderless
				else
				{
					sb.Append("groin");
				}

				sb.Append(" should you need to. ");
				if (wearer.hasCock)
				{
					var biggest = wearer.genitals.BiggestCockByArea();

					if (biggest.area >= 100)
					{
						sb.Append("Your manhood is too big to be concealed by your silken loincloth. Part of your " + biggest.ShortDescription() + " is visible. ");
						if (wearer.corruption >= 66 || wearer.IsExhibitionist())
						{
							sb.Append("You admire how your manhood is visible. ");
						}
						else if (wearer.corruption >= 33 && wearer.corruption < 66)
						{
							sb.Append("You blush a bit, not sure how you feel. ");
						}
						else //if (wearer.corruption < 33)
						{
							sb.Append("You let out a sigh. ");
						}
					}

					else if (biggest.area >= 40)
					{
						sb.Append("Large bulge forms against your silken loincloth. ");
					}

				}
				if (wearer.corruption >= 66 || wearer.IsExhibitionist())
				{
					sb.Append("You'd love to lift your loincloth and show off whenever you want to. ");
				}
			}
			else
			{
				sb.Append("Last but not least, you take a silken loincloth in your hand but stop short as you examine your tauric body. There is no way you could properly conceal your genitals! ");
				if (wearer.corruption >= 66 || wearer.IsExhibitionist())
				{
					sb.Append("Regardless, you are happy with what you are right now. ");
				}

				else if (wearer.corruption >= 33 && wearer.corruption < 66)
				{
					sb.Append("You blush a bit, not sure how you feel. ");
				}
				else //(wearer.corruption < 33)
				{
					sb.Append("You let out a sigh. Being a centaur surely is inconvenient! ");
				}

				sb.Append("You leave the silken loincloth in your possessions for the time being.");
			}
			sb.Append("" + GlobalStrings.NewParagraph() + "You admire the design of your armor. No wonder it looks so sexy! ");

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is BeeArmor;
		}

		public bool supportsBulgeArmor => true;

		string IBulgeArmor.SetBulgeState(Creature wearer, bool bulgified)
		{
			if (bulgified == bulged)
			{
				return "";
			}
			bulged = bulgified;
			if (bulged)
			{
				return "The silken loin-cloth of your chitin armor cinches up, tightening against your groin until it displays the prominent bulge of your demon-possessed dick clearly.";
			}
			else
			{
				return "The silken loin-cloth of your chitin armor relaxes, no longer cursed to prominently display your dick-bulge";
			}

		}

		bool IBulgeArmor.isBulged => bulged;

		protected override string OnRemoveText()
		{
			return this.GenericBulgeAwareRemoveText(bulged);
		}

	}
}