using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class GenitalTattooLocation
	{
		private static string LeftChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftUnderBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftUnderBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftNippleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftNippleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightUnderBreastButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightUnderBreastLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightNippleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightNippleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ChestButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ChestLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GroinButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GroinLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VulvaButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VulvaLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AssButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AssLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	internal static class CommonGenitalStrings
	{
		internal static readonly string[] matchedPairOptions = new string[] { "pair of ", "two ", "brace of ", "matching ", "twin " };
		internal static readonly string[] mismatchedPairOptions = new string[] { "pair of ", "two ", "brace of " };
		internal static readonly string[] matchedTripleOptions = new string[]
		{
			"three ",
			"group of ",
			SafelyFormattedString.FormattedText("ménage à trois", StringFormats.ITALIC) + " of ",
			"triad of ",
			"triumvirate of "
		};
		internal static readonly string[] mismatchedTripleOptions = new string[] { "three ", "group of " };


	}

	partial class Genitals
	{

		public static string Name()
		{
			return "Genitals";
		}

		private string AllTattoosShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTattoosLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}



		private string AllVaginasPlayerText()
		{
			if (!CreatureStore.TryGetCreature(creatureID, out Creature creature) || !(creature is PlayerBase player))
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			if (creature?.lowerBody.type == LowerBodyType.CENTAUR && cocks.Count == 0)
			{
				sb.Append(Environment.NewLine + "Your womanly parts have shifted to lie between your hind legs, in a rather feral fashion.");
			}
			if (vaginas.Count == 1)
			{
				var vagina = vaginas[0];
				sb.Append("You have " + vagina.LongDescription(true) + ", with " + vagina.clit.LongDescription(true));
				if (vagina.isVirgin)
				{
					sb.Append("and an intact hymen");
				}
				sb.Append(". ");
				sb.Append(vaginas[0].PlayerDescription());
			}
			/*StringBuilder sb = new StringBuilder();
		bool onlyOneVag = player.vaginas.Count == 1;
		//start with intro for this vag.
		if (onlyOneVag)
		{
			sb.Append();
		}
		else if (vagina.vaginaIndex == 0)
		{
			sb.Append("Your first is a" + vagina.ShortDescription());
		}
		else
		{
			sb.Append("Your second is a" + vagina.ShortDescription());
		}
		//clit
		sb.Append();
		//virgin.

		return sb.ToString();*/
			else
			{
				sb.Append("Unlike most " + gender.AsText() + "s, you have a pair of vaginas, situated alongside one another. ");
				for (int x = 0; x < 2; x++)
				{
					var vagina = vaginas[x];
					sb.Append("Your " + Utils.NumberAsPlace(x) + " is " + vagina.LongDescription(true) + ", with " + vagina.clit.LongDescription(true));
					if (vagina.isVirgin)
					{
						sb.Append("and an intact hymen");
					}
					sb.Append(". ");
					sb.Append(vagina.PlayerDescription());
					sb.Append(Environment.NewLine);
				}
			}


			//do wetness and looseness.
			sb.Append(WetnessLoosenessText(player));

			//clitcock text. only applies to the first vagina, and only if the player does not have a normal cock.
			//if (hasClitCock && gender == Gender.FEMALE)
			//{
			//	sb.Append("During your travels, you've gained the ability to transform your " + (vaginas.Count == 2 ? "first " : "") + "clit into a cock, allowing you to indulge "
			//		+ "in the sexual pleasures of a man while still remaining a woman. ");
			//	if (player.relativeLust > 80)
			//	{
			//		sb.Append("Unfortunately, it sometimes has a mind of its own and you're having trouble keeping it under control. ");
			//	}
			//}

			return sb.ToString();
		}

		private string WetnessLoosenessText(PlayerBase player)
		{
			string WetnessText(Vagina vagina)
			{
				byte wetnessLevel = 0;

				if (vagina.wetness >= VaginalWetness.DROOLING)
				{
					wetnessLevel = 2;
				}
				else if (vagina.wetness >= VaginalWetness.WET)
				{
					wetnessLevel = 1;
				}


				//stack once if above 50 in at least one - at least horny.
				if (player.relativeLust >= 50 || player.relativeLibido >= 50)
				{
					wetnessLevel++;
				}
				//stack again if both are very high - uber horny.
				if (player.relativeLust >= 80 && player.relativeLibido >= 80)
				{
					wetnessLevel++;
				}
				if (wetnessLevel > 0)
				{
					return "";
				}
				else if (wetnessLevel == 1)
				{
					return "Moisture gleams in "; //horny and below wet or not horny and between wet and max wetness.
				}
				else if (wetnessLevel == 2)
				{
					return "Occasional beads of lubricant drip from "; //uber horny and below wet or horny and above wet and not max or not horny and max.
				}
				else if (wetnessLevel == 3) //uber horny and above wet, but not max. or horny and max.
				{
					return "Thin streams of lubricant occasionally dribble from ";
				}
				else //(wetnessLevel > 3) //uber horny and max wetness.
				{
					return "Thick streams of lubricant drool constantly from ";
				}
			}

			string WetnessPredicate(Vagina vagina)
			{
				byte wetnessLevel = 0;

				if (vagina.wetness >= VaginalWetness.DROOLING)
				{
					wetnessLevel = 2;
				}
				else if (vagina.wetness >= VaginalWetness.WET)
				{
					wetnessLevel = 1;
				}


				//stack once if above 50 in at least one - at least horny.
				if (player.relativeLust >= 50 || player.relativeLibido >= 50)
				{
					wetnessLevel++;
				}
				//stack again if both are very high - uber horny.
				if (player.relativeLust >= 80 && player.relativeLibido >= 80)
				{
					wetnessLevel++;
				}
				if (wetnessLevel > 0)
				{
					return "";
				}
				else if (wetnessLevel == 1)
				{
					return "gleams"; //horny and below wet or not horny and between wet and max wetness.
				}
				else if (wetnessLevel == 2)
				{
					return "drips"; //uber horny and below wet or horny and above wet and not max or not horny and max.
				}
				else if (wetnessLevel == 3) //uber horny and above wet, but not max. or horny and max.
				{
					return "occasionally dribbles";
				}
				else //(wetnessLevel > 3) //uber horny and max wetness.
				{
					return "drools constantly";
				}
			}

			if (player.vaginas.Count == 1)
			{
				var vagina = player.vaginas[0];
				//the original source repeated the same text for various options, though it did so in a specific way. this does the same, but makes it cleaner.


				string wetness = WetnessText(vagina);

				if (!string.IsNullOrEmpty(wetness))
				{
					string tightness;
					if (vagina.looseness == VaginalLooseness.TIGHT)
					{
						tightness = "your tight " + vagina.ShortDescription();
					}
					else if (vagina.looseness == VaginalLooseness.NORMAL)
					{
						tightness = "your " + vagina.ShortDescription();
					}
					else if (vagina.looseness < VaginalLooseness.GAPING)
					{
						tightness = "your " + vagina.ShortDescription() + ", its lips slightly parted";
					}
					else
					{
						tightness = "the massive hole that is your " + vagina.ShortDescription();
					}
					return wetness + tightness + ".";
				}
				//default case: no wetness description. Try to do a looseness description if applicable.
				else if (vagina.looseness != VaginalLooseness.NORMAL)
				{
					if (vagina.looseness == VaginalLooseness.TIGHT)
					{
						return "Your " + vagina.ShortDescription() + " is extremely tight, though otherwise normal.";
					}
					else if (vagina.looseness <= VaginalLooseness.ROOMY)
					{
						return "Your pussy-lips have parted slightly, granting extra room in your otherwise normal " + vagina.ShortDescription() + ".";
					}
					else
					{
						return "Your " + vagina.ShortDescription() + "has been stretched so wide that it gapes.";
					}
				}
				else
				{
					return "";
				}
			}
			else
			{
				Vagina first = player.vaginas[0];
				Vagina second = player.vaginas[1];

				string loosenessText(VaginalLooseness looseness)
				{
					if (looseness <= VaginalLooseness.TIGHT)
					{
						return "a little tight";
					}
					else if (looseness == VaginalLooseness.LOOSE)
					{
						return "a little loose";
					}
					else if (looseness == VaginalLooseness.ROOMY)
					{
						return "rather loose";
					}
					else //if (looseness > VaginalLooseness.GAPING)
					{
						return "so loose that they gape";
					}
				}

				//if both are the same wetness and looseness.
				if (first.looseness == second.looseness && first.wetness == second.wetness)
				{
					//both are normal
					if (first.looseness == VaginalLooseness.NORMAL && first.wetness == VaginalWetness.NORMAL)
					{
						return "Both vaginas are relatively normal";
					}
					//wetness normal, non-normal looseness.
					else if (first.wetness == VaginalWetness.NORMAL)
					{
						return "Both of your " + VaginaType.VaginaNoun(true) + " are " + loosenessText(first.looseness) + ", but otherwise normal.";
					}
					//looseness normal, non-normal wetness.
					else if (first.looseness == VaginalLooseness.NORMAL)
					{
						if (first.wetness == VaginalWetness.DRY)
						{
							return "Both of your vaginas are relatively normal, though a bit dry.";
						}
						else
						{
							return WetnessText(first) + " both of your vaginas.";
						}
					}
					else
					{
						return "Both of your " + VaginaType.VaginaNoun(true) + " are " + loosenessText(first.looseness) + ", and " + WetnessText(first).ToLower() + "from both of them";
					}
				}
				//same looseness, different wetness.
				else if (first.looseness == second.looseness)
				{
					string wetter, describe;

					if (first.wetness > second.wetness)
					{
						wetter = "first";
						describe = "your first is " + first.wetness.AsAdjective() + "; your second: " + second.wetness.AsAdjective();
					}
					else
					{
						wetter = "second";
						describe = "your second is " + second.wetness.AsAdjective() + "; your first: " + first.wetness.AsAdjective();
					}

					return "Both of your " + VaginaType.VaginaNoun(true) + " are " + loosenessText(first.looseness) + ", but your " + wetter + " is wetter - " + describe + ".";
				}
				//same wetness, differnet looseness.
				else if (first.wetness == second.wetness)
				{

					if (first.looseness >= VaginalLooseness.GAPING && second.looseness >= VaginalLooseness.GAPING)
					{
						string looser = first.looseness > second.looseness ? "first" : "second";

						return "Both of your " + VaginaType.VaginaNoun(true) + " are " + first.wetness.AsAdjective() + " and are so loose they gape; but your " + looser +
							" is slightly looser. ";
					}
					else
					{
						string looser, describe;
						if (first.looseness > second.looseness)
						{
							looser = "first";
							describe = "your first is " + loosenessText(first.looseness) + "; your second: " + loosenessText(second.looseness);
						}
						else
						{
							looser = "second";
							describe = "your second is " + loosenessText(second.looseness) + "; your first: " + loosenessText(first.looseness);
						}

						return "Both of your " + VaginaType.VaginaNoun(true) + " are " + first.wetness.AsAdjective() + ", but your " + looser + " is looser - " + describe + ".";
					}

				}
				//different wetness, looseness
				//one looser and wetter than other.
				else if ((first.wetness > second.wetness && first.looseness > second.looseness) || (second.wetness > first.wetness && second.looseness > first.looseness))
				{
					Vagina moreUsed;
					Vagina lessUsed;

					string moreUsedString;
					string lessUsedString;

					if (first.wetness > second.wetness)
					{
						moreUsedString = "first";
						lessUsedString = "second";

						moreUsed = first;
						lessUsed = second;
					}
					else
					{
						moreUsedString = "second";
						lessUsedString = "first";

						moreUsed = second;
						lessUsed = first;
					}

					return "Your " + moreUsedString + " is both wetter and looser than your " + lessUsedString + ". Your " + moreUsedString + " is " + loosenessText(moreUsed.looseness) +
						" and " + WetnessPredicate(moreUsed) + ", while your " + lessUsedString + " is " + loosenessText(lessUsed.looseness) + " and " + WetnessPredicate(lessUsed) + ".";
				}
				//mixed
				else
				{
					string firstString, secondString;

					if (first.wetness > second.wetness)
					{
						firstString = "wetter";
						secondString = "looser";
					}
					else
					{
						firstString = "looser";
						secondString = "wetter";
					}

					return "Your first is " + firstString + "than the second, though the second is " + secondString + ". Your first is " + loosenessText(first.looseness) + " and " +
						WetnessPredicate(first) + ", while your second is " + loosenessText(second.looseness) + " and " + WetnessPredicate(second) + ".";
				}
			}
		}


	}
}
