using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	internal interface IVaginaCollection<T> where T : IVagina
	{
		ReadOnlyCollection<T> vaginas { get; }

		VaginaData AverageVagina();
	}

	public partial class VaginaCollection : IVaginaCollection<Vagina>
	{
		private static string Name()
		{
			return "All Vaginas";
		}


		ReadOnlyCollection<Vagina> IVaginaCollection<Vagina>.vaginas => vaginas;

		public string RestoredAllVaginasGeneric(VaginaCollectionData oldVaginas)
		{
			//if changed has elements, we know they still exist in the current collection.
			IEnumerable<ValueDifference<VaginaData>> changed = ChangedVaginas(oldVaginas, true);
			int changeCount = changed.Count(x => x.newValue.isDefault && !x.oldValue.isDefault);
			if (changeCount == 0)
			{
				return "";
			}

			int humanVags = CountVaginasOfType(VaginaType.HUMAN);

			string changeText;

			if (vaginas.Count == 1)
			{
				//old should never be null.
				VaginaData old = oldVaginas.vaginas.FirstOrDefault(x => x.collectionID == vaginas[0].collectionID);
				if (old is null)
				{
					return "";
				}
				else
				{
					return vaginas[0].RestoredText(old);
				}
			}

			if (humanVags < vaginas.Count)
			{
				changeText = "vaginas and find that one has returned back to a more natural, human size and color.";
			}
			else if (humanVags > changeCount)
			{
				changeText = "vaginas and find that one has returned back to something more fit on a human, matching the other one.";
			}
			else
			{
				changeText = "vaginas and find that they have turned back to their more natural, human size and color.";

			}

			string armorText = creature?.wearingArmor == true || creature?.wearingUpperGarment == true ? "Undoing your clothes" : "Looking down";

			return GlobalStrings.NewParagraph() + "nSomething invisible brushes against your sex, making you twinge. " + armorText + ", you take a look at your "
				+ changeText;

		}

		private string AllVaginasPlayerText(PlayerBase player)
		{
			if (_vaginas.Count == 0)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			if (creature?.lowerBody.type == LowerBodyType.CENTAUR && player.cocks.Count == 0)
			{
				sb.Append(Environment.NewLine + "Your womanly parts have shifted to lie between your hind legs, in a rather feral fashion.");
			}
			if (vaginas.Count == 1)
			{
				Vagina vagina = vaginas[0];
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
					Vagina vagina = vaginas[x];
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
				Vagina vagina = player.vaginas[0];
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

	internal static class VaginaCollectionStrings
	{
		private static int NumVaginas<T>(this IVaginaCollection<T> collection) where T : IVagina
		{
			return collection.vaginas.Count;
		}

		#region Vagina Text
		internal static string AllVaginasShortDescription<T>(IVaginaCollection<T> collection) where T : IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}
			else if (collection.vaginas.Count == 1)
			{
				return collection.vaginas[0].ShortDescription();
			}
			bool mismatched = collection.vaginas.Any(x => x.type != collection.vaginas[0].type);

			return mismatched ? VaginaType.VaginaNoun(true) : collection.vaginas[0].ShortDescription(false);
		}

		internal static string AllVaginasLongDescription<T>(IVaginaCollection<T> collection) where T : IVagina
		{
			return AllVaginasDesc(collection, false);
		}

		internal static string AllVaginasFullDescription<T>(IVaginaCollection<T> collection) where T : IVagina
		{
			return AllVaginasDesc(collection, true);
		}

		internal static string OneVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection) where T : IVagina => OneVaginaOrVaginasNoun(collection, Conjugate.YOU);
		internal static string OneVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, Conjugate conjugate) where T : IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.vaginas.Count > 1, conjugate, VaginaType.VaginaNoun(collection.vaginas.Count > 1));
		}

		internal static string OneVaginaOrVaginasShort<T>(IVaginaCollection<T> collection) where T : IVagina => OneVaginaOrVaginasShort(collection, Conjugate.YOU);
		internal static string OneVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, Conjugate conjugate) where T : IVagina
		{
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(collection.vaginas.Count > 1, conjugate, AllVaginasShortDescription(collection));
		}

		internal static string EachVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection) where T : IVagina => EachVaginaOrVaginasNoun(collection, Conjugate.YOU);
		internal static string EachVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, Conjugate conjugate) where T : IVagina
		{
			return EachVaginaOrVaginasNoun(collection, conjugate, out bool _);
		}

		internal static string EachVaginaOrVaginasShort<T>(IVaginaCollection<T> collection) where T : IVagina => EachVaginaOrVaginasShort(collection, Conjugate.YOU);
		internal static string EachVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, Conjugate conjugate) where T : IVagina
		{
			return EachVaginaOrVaginasShort(collection, conjugate, out bool _);
		}

		internal static string EachVaginaOrVaginasNoun<T>(IVaginaCollection<T> collection, Conjugate conjugate, out bool isPlural) where T : IVagina
		{
			isPlural = collection.vaginas.Count != 1;
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.vaginas.Count > 1, conjugate, VaginaType.VaginaNoun(collection.vaginas.Count > 1));
		}

		internal static string EachVaginaOrVaginasShort<T>(IVaginaCollection<T> collection, Conjugate conjugate, out bool isPlural) where T : IVagina
		{
			isPlural = collection.vaginas.Count != 1;
			if (collection.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(collection.vaginas.Count > 1, conjugate, AllVaginasShortDescription(collection));
		}
		#endregion
		private static string RandomMixedVaginasText()
		{
			return Utils.RandomChoice("mixed ", "mixed ", "mismatched ") + VaginaType.VaginaNoun(true);
		}

		private static string AllVaginasDesc<T>(IVaginaCollection<T> collection, bool full) where T : IVagina
		{

			if (collection.vaginas.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (collection.vaginas.Count == 1)
			{
				return collection.vaginas[0].ShortDescription();
			}
			else
			{
				bool mismatched = collection.vaginas.Any(x => x.type != collection.vaginas[0].type);

				string[] countOptions = mismatched ? CommonGenitalStrings.mismatchedPairOptions : CommonGenitalStrings.matchedPairOptions;
				string description = mismatched ? RandomMixedVaginasText() : collection.vaginas[0].ShortDescription(true);

				return Utils.RandomChoice(countOptions) + collection.AverageVagina().AdjectiveText(full) + description;
			}
		}
	}
}
