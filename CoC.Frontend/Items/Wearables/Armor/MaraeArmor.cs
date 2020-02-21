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
	public class MaraeArmor : ArmorBase, IBulgeArmor
	{
		private bool bulged;

		public override string AbbreviatedName() => "T.B.Armor";

		public override string ItemName() => (bulged ? "crotch-hugging " : "") + "tentacled bark armor";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string setText = count != 1 ? "suits" : "suit";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string crotchText = bulged ? "crotch-hugging, " : "";

			return $"{count}{setText} of {crotchText}tentacled bark armor";
		}

		public override string AboutItem()
		{
			return "This suit of armor is finely made from the white bark from corrupted Marae you've defeated. It comes with tentacles though." +
				(bulged ? " It also has been magically modified to prominently display every facet of your manly bulge" : "");
		}

		protected override int monetaryValue => 1000;

		public MaraeArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		public bool supportsBulgeArmor => true;

		public override double PhysicalDefensiveRating(Creature wearer)
		{
			return 20 + (int)Math.Floor(wearer.corruptionTrue / 5);
		}

		protected override void OnEquip(Creature wearer)
		{
			base.OnEquip(wearer);
			//min lust is 20.
			if (wearer.relativeLust < 20)
			{
				wearer.IncreaseLust(30);
			}
		}

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("You " + (wearer.wearingAnything ? "strip yourself naked before you " : "") + "proceed to put on the armor. ");
			if (wearer.corruption < 33)
			{
				sb.Append("You shudder at the idea of wearing armor that is infested with tentacles but you proceed anyway. ");
			}

			if (wearer.corruption >= 33 && wearer.corruption < 66)
			{
				sb.Append("You are not sure about the idea of armor that is infested with tentacles. ");
			}

			if (wearer.corruption >= 66)
			{
				sb.Append("You are eager with the idea of wearing tentacle-infested armor. ");
			}

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
					if (biggest.area >= 40 && biggest.area < 100)
					{
						sb.Append("Large bulge forms against your silken loincloth. ");
					}
					else if (biggest.area >= 100)
					{
						sb.Append("Your manhood is too big to be concealed by your silken loincloth. Part of your " + biggest.ShortDescription() + " is visible. ");
						if (wearer.corruption >= 66 || wearer.IsExhibitionist())
						{
							sb.Append("You admire how your manhood is visible. ");
						}

						else if (wearer.corruption >= 33)
						{
							sb.Append("You blush a bit, not sure how you feel. ");
						}
						else //if (wearer.corruption < 33)
						{
							sb.Append("You let out a sigh. ");
						}

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
				else if (wearer.corruption >= 33)
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
			if (wearer.relativeLust < 20)
			{
				sb.Append(GlobalStrings.NewParagraph() + "You can feel the tentacles inside your breastplate slither their way and tease your " + wearer.build.ButtLongDescription() + ". You " + (wearer.corruption < 60 ? "gasp in surprise" : "moan in pleasure") + ". ");
			}

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is MaraeArmor;
		}

		protected override string RemoveText(Creature wearer)
		{
			return this.GenericBulgeAwareRemoveText(bulged, base.RemoveText(wearer));
		}

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
