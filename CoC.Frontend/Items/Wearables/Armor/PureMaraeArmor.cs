namespace CoC.Frontend.Items.Wearables.Armor
{
	using System;
	using System.Text;
	using CoC.Backend.BodyParts;
	using CoC.Backend.Creatures;
	using CoC.Backend.Items.Wearables.Armor;
	using CoC.Backend.Strings;
	using CoC.Backend.Tools;
	using CoC.Frontend.Creatures;

	/**
	 * ...
	 * @author Kitteh6660
	 */
	public sealed class PureMaraeArmor : ArmorBase, IBulgeArmor
	{
		private bool bulged;

		public PureMaraeArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		public override string AbbreviatedName() => "D.B.Armor";

		public override string ItemName() => (bulged? "crotch-hugging " : "") + "divine bark armor";
		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "suits" : "suit";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			string crotchText = bulged ? "crotch-hugging " : "";
			return $"{count}{setText} of {crotchText}divine bark armor";
		}

		public override string AboutItem()
		{
			return "This suit of armor is finely made from the white bark you've received from Marae as a reward." + (bulged ? " It has been magically modified to prominently " +
				"display every facet of your manly bulge" : "");
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 40 - (int)Math.Floor(wearer.corruptionTrue / 5);

		//provides a 10% reduction in lust gain over time.
		public override double LustGainOffset(Creature wearer) => -0.1;

		protected override int monetaryValue => 1100;






		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("You strip yourself naked before you proceed to put on the armor. ");

			sb.Append(GlobalStrings.NewParagraph() + "First, you clamber into the breastplate. ");
			if (wearer.isBiped) //Some variants.
			{
				if (wearer.lowerBody.type == LowerBodyType.HUMAN)
				{
					sb.Append("Then you put your feet into your boots. With the boots fully equipped, you move on to the next piece. ");
				}
				else
				{
					sb.Append("Then you attempt to put your feet into your boots. You realize that the boots are designed for someone with normal feet. You have to modify the boots to fit and when you do put on your boots, your feet are exposed. ");
				}
			}
			sb.Append("Next, you put on your reinforced bark bracers to protect your arms." + GlobalStrings.NewParagraph());
			if (!wearer.isQuadruped)
			{
				sb.Append("Last but not least, you put your silken loincloth on to cover your groin. You thank Rathazul for that and you know that you easily have access to your ");
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
				//Genderless
				if (!wearer.hasCock && !wearer.hasVagina)
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
				else //if (wearer.corruption < 33)
				{
					sb.Append("You let out a sigh. Being a centaur surely is inconvenient! ");
				}

				sb.Append("You leave the silken loincloth in your possessions for the time being.");
			}
			sb.Append("You are suited up and all good to go. ");

			return sb.ToString();
		}
		protected override string RemoveText(Creature wearer)
		{
			return this.GenericBulgeAwareRemoveText(bulged, base.RemoveText(wearer));
		}

		public override bool Equals(ArmorBase other)
		{
			return other is PureMaraeArmor;
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
				return "The silken loin-cloth of your magical bark armor cinches up, tightening against your groin until it displays the prominent " +
					"bulge of your demon-possessed dick" + (wearer.cocks.Count > 1 ? "s" : "") + " clearly.";
			}
			else
			{
				return "The silken loin-cloth of your magical bark armor relaxes until it doesn't feature your dick-bulge quite so prominently";
			}
		}

		bool IBulgeArmor.isBulged => bulged;

	}
}
