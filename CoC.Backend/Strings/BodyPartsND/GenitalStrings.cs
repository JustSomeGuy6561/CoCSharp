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

	internal interface IGenitals
	{
		ReadOnlyCollection<CockData> cocks { get; }
		ReadOnlyCollection<VaginaData> vaginas { get; }
		ReadOnlyCollection<BreastData> breasts { get; }

		CockData AverageCock();
		VaginaData AverageVagina();
		BreastData AverageBreasts();

		bool hasSheath { get; }
	}



	internal static class GenitalStrings
	{
		private static int NumCocks(this IGenitals genitals)
		{
			return genitals.cocks.Count;
		}

		private static int NumVaginas(this IGenitals genitals)
		{
			return genitals.vaginas.Count;
		}

		private static int NumBreasts(this IGenitals genitals)
		{
			return genitals.breasts.Count;
		}

		private static readonly string[] matchedPairOptions = new string[] { "pair of ", "two ", "brace of ", "matching ", "twin " };
		private static readonly string[] mismatchedPairOptions = new string[] { "pair of ", "two ", "brace of " };
		private static readonly string[] matchedTripleOptions = new string[]
		{
			"three ",
			"group of ",
			SafelyFormattedString.FormattedText("ménage à trois", StringFormats.ITALIC) + " of ",
			"triad of ",
			"triumvirate of "
		};
		private static readonly string[] mismatchedTripleOptions = new string[] { "three ", "group of " };
		private static readonly string[] lottaDicksOptions = new string[] { "bundle of ", "obscene group of ", "cluster of ", "wriggling bunch of " };

		private static readonly string[] mismatchedDicksTextOptions = new string[] { "mutated cocks", "mutated dicks", "mixed cocks", "mismatched dicks" };


		#region Breast Text

		internal static string AllBreastsShortDescription(IGenitals genitals, bool alternateFormat = false)
		{
			return BreastRowCountText(genitals, alternateFormat, true) + genitals.AverageBreasts().ShortDescription();
		}

		internal static string AllBreastsLongDescription(IGenitals genitals, bool alternateFormat = false)
		{
			return BreastRowCountText(genitals, alternateFormat, true) + genitals.AverageBreasts().LongDescription(false, true);
		}

		internal static string AllBreastsFullDescription(IGenitals genitals, bool alternateFormat = false)
		{
			return BreastRowCountText(genitals, alternateFormat, true) + genitals.AverageBreasts().FullDescription(false, false, true);
		}

		internal static string ChestOrAllBreastsShort(IGenitals genitals, bool alternateFormat = false)
		{
			if (genitals.NumBreasts() == 1 && genitals.breasts[0].isMaleBreasts)
			{
				return ChestShortDesc(genitals, alternateFormat);
			}
			else return AllBreastsShortDescription(genitals, alternateFormat);
		}

		internal static string ChestOrAllBreastsLong(IGenitals genitals, bool alternateFormat = false)
		{
			if (genitals.NumBreasts() == 1 && genitals.breasts[0].isMaleBreasts)
			{
				return ChestDesc(genitals, alternateFormat, false);
			}
			else return AllBreastsLongDescription(genitals, alternateFormat);
		}

		internal static string ChestOrAllBreastsFull(IGenitals genitals, bool alternateFormat = false)
		{
			if (genitals.NumBreasts() == 1 && genitals.breasts[0].isMaleBreasts)
			{
				return ChestDesc(genitals, alternateFormat, true);
			}
			else return AllBreastsFullDescription(genitals, alternateFormat);
		}

		#endregion

		#region Cock Text
		internal static string SheathOrBaseStr(IGenitals genitals)
		{
			return genitals.hasSheath ? "sheath" : "base";
		}

		internal static string AllCocksShortDescription(IGenitals genitals)
		{
			if (genitals.cocks.Count == 0)
			{
				return "";
			}
			else if (genitals.cocks.Count == 1)
			{
				return genitals.cocks[0].ShortDescription();
			}
			bool mismatched = genitals.cocks.Any(x => x.type != genitals.cocks[0].type);

			return mismatched ? CockType.GenericCockNoun(true) : genitals.cocks[0].ShortDescription(false, true);
		}

		internal static string AllCocksLongDescription(IGenitals genitals)
		{
			return AllCocksDesc(genitals, false);
		}

		internal static string AllCocksFullDescription(IGenitals genitals)
		{
			return AllCocksDesc(genitals, true);
		}

		internal static string OneCockOrCocksNoun(IGenitals genitals, string pronoun = "your")
		{
			if (genitals.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(genitals.cocks.Count > 1, pronoun, CockType.GenericCockNoun(genitals.cocks.Count > 1));
		}

		internal static string OneCockOrCocksShort(IGenitals genitals, string pronoun = "your")
		{
			if (genitals.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(genitals.cocks.Count > 1, pronoun, AllCocksShortDescription(genitals));
		}

		internal static string EachCockOrCocksNoun(IGenitals genitals, string pronoun = "your")
		{
			return EachCockOrCocksNoun(genitals, pronoun, out bool _);
		}

		internal static string EachCockOrCocksShort(IGenitals genitals, string pronoun = "your")
		{
			return EachCockOrCocksShort(genitals, pronoun, out bool _);
		}

		internal static string EachCockOrCocksNoun(IGenitals genitals, string pronoun, out bool isPlural)
		{
			isPlural = genitals.cocks.Count != 1;
			if (genitals.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(genitals.cocks.Count > 1, pronoun, CockType.GenericCockNoun(genitals.cocks.Count > 1));
		}

		internal static string EachCockOrCocksShort(IGenitals genitals, string pronoun, out bool isPlural)
		{
			isPlural = genitals.cocks.Count != 1;
			if (genitals.cocks.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(genitals.cocks.Count > 1, pronoun, AllCocksShortDescription(genitals));
		}
		#endregion

		#region Vagina Text
		internal static string AllVaginasShortDescription(IGenitals genitals)
		{
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}
			else if (genitals.vaginas.Count == 1)
			{
				return genitals.vaginas[0].ShortDescription();
			}
			bool mismatched = genitals.vaginas.Any(x => x.type != genitals.vaginas[0].type);

			return mismatched ? VaginaType.VaginaNoun(true) : genitals.vaginas[0].ShortDescription(false);
		}

		internal static string AllVaginasLongDescription(IGenitals genitals)
		{
			return AllVaginasDesc(genitals, false);
		}

		internal static string AllVaginasFullDescription(IGenitals genitals)
		{
			return AllVaginasDesc(genitals, true);
		}

		internal static string OneVaginaOrVaginasNoun(IGenitals genitals, string pronoun = "your")
		{
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(genitals.vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(genitals.vaginas.Count > 1));
		}

		internal static string OneVaginaOrVaginasShort(IGenitals genitals, string pronoun = "your")
		{
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.OneOfDescription(genitals.vaginas.Count > 1, pronoun, AllVaginasShortDescription(genitals));
		}

		internal static string EachVaginaOrVaginasNoun(IGenitals genitals, string pronoun = "your")
		{
			return EachVaginaOrVaginasNoun(genitals, pronoun, out bool _);
		}

		internal static string EachVaginaOrVaginasShort(IGenitals genitals, string pronoun = "your")
		{
			return EachVaginaOrVaginasShort(genitals, pronoun, out bool _);
		}

		internal static string EachVaginaOrVaginasNoun(IGenitals genitals, string pronoun, out bool isPlural)
		{
			isPlural = genitals.vaginas.Count != 1;
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(genitals.vaginas.Count > 1, pronoun, VaginaType.VaginaNoun(genitals.vaginas.Count > 1));
		}

		internal static string EachVaginaOrVaginasShort(IGenitals genitals, string pronoun, out bool isPlural)
		{
			isPlural = genitals.vaginas.Count != 1;
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}

			return CommonBodyPartStrings.EachOfDescription(genitals.vaginas.Count > 1, pronoun, AllVaginasShortDescription(genitals));
		}
		#endregion

		#region Private Helpers
		private static string AllCocksDesc(IGenitals genitals, bool full)
		{
			if (genitals.cocks.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (genitals.cocks.Count == 1)
			{
				return genitals.cocks[0].ShortDescription();
			}
			else
			{
				bool mismatched = genitals.cocks.Any(x => x.type != genitals.cocks[0].type);

				string[] countOptions;
				string description;

				if (genitals.cocks.Count == 2)
				{
					countOptions = mismatched ? mismatchedPairOptions : matchedPairOptions;
				}
				else if (genitals.cocks.Count == 3)
				{
					countOptions = mismatched ? mismatchedTripleOptions : mismatchedTripleOptions;
				}
				else
				{
					countOptions = lottaDicksOptions;
				}

				description = mismatched ? Utils.RandomChoice(mismatchedDicksTextOptions) : genitals.cocks[0].ShortDescription(false, true);

				return Utils.RandomChoice(countOptions) + genitals.AverageCock().AdjectiveText(full) + description;
			}
		}

		private static string AllVaginasDesc(IGenitals genitals, bool full)
		{
			if (genitals.vaginas.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (genitals.vaginas.Count == 1)
			{
				return genitals.vaginas[0].ShortDescription();
			}
			else
			{
				bool mismatched = genitals.vaginas.Any(x => x.type != genitals.vaginas[0].type);

				string[] countOptions = mismatched ? mismatchedPairOptions : matchedPairOptions;
				string description = mismatched ? Utils.RandomChoice(mismatchedDicksTextOptions) : genitals.vaginas[0].ShortDescription(true);

				return Utils.RandomChoice(countOptions) + genitals.AverageVagina().AdjectiveText(full) + description;
			}
		}

		private static string BreastRowCountText(IGenitals genitals, bool alternateFormat, bool withEven)
		{
			string evenStr = withEven ? (genitals.breasts.Any(x => x.cupSize != genitals.breasts[0].cupSize) ? "uneven " : "even ") : "";
			if (genitals.breasts.Count <= 1)
			{
				if (alternateFormat) return "a pair of ";
				else return "";
			}
			else if (genitals.breasts.Count == 2)
			{
				return "two " + evenStr + "rows of ";
			}
			else if (genitals.breasts.Count == 3)
			{
				return Utils.RandBool() ? "three " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "multi-layered ";
			}
			else //if (creature.genitals.breasts.Count == 4)
			{
				return Utils.RandBool() ? "four " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "four-tiered ";
			}
		}

		private static string ChestShortDesc(IGenitals genitals, bool withArticle)
		{
			return (withArticle ? "a " : "") + "chest";
		}

		private static string ChestDesc(IGenitals genitals, bool withArticle, bool full)
		{
			string chest = withArticle ? "a flat chest" : "flat chest";
			if (full)
			{
				return chest + " with " + NippleStrings.NippleSizeAdjective(genitals.breasts[0].nipples.length) + " nipples";
			}
			else
			{
				return chest;
			}
		}
		#endregion
	}

	partial class Genitals : IGenitals
	{
		ReadOnlyCollection<CockData> IGenitals.cocks => this._cocks.Select(x => x.AsReadOnlyData()).ToList().AsReadOnly();

		ReadOnlyCollection<VaginaData> IGenitals.vaginas => this._vaginas.Select(x => x.AsReadOnlyData()).ToList().AsReadOnly();

		ReadOnlyCollection<BreastData> IGenitals.breasts => this._breasts.Select(x => x.AsReadOnlyData()).ToList().AsReadOnly();

		bool IGenitals.hasSheath => this.hasSheath;

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

		private string AllCocksPlayerDesc()
		{
			if (!CreatureStore.TryGetCreature(creatureID, out Creature creature) || !(creature is PlayerBase player))
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();

			int rando = Utils.Rand(100);

			for (int x = 0; x < _cocks.Count; x++)
			{
				Cock cock = _cocks[x];
				bool plural = false;

				if (player.cocks.Count == 1)
				{
					sb.Append("Your ");
				}
				else if (cock.cockIndex == 0)
				{
					sb.Append("--Your first ");
				}
				else if (rando % 5 == 0)
				{
					sb.Append("--The next ");

				}
				else if (rando % 5 == 1)
				{
					sb.Append("--The " + Utils.NumberAsPlace(x + 1) + " of your ");
					plural = true;
				}
				else if (rando % 5 == 2)
				{
					sb.Append("--One of your ");
					plural = true;
				}
				else if (rando % 5 == 3)
				{
					sb.Append("--The " + Utils.NumberAsPlace(x + 1) + " ");
				}
				else /*if (rando % 5 == 4)*/
				{
					sb.Append("--Another of your ");
					plural = true;
				}

				sb.Append(cock.AdjectiveText(false) + " " + Cock.GenericCockNoun(plural) + " is " + Measurement.ToNearestLargeAndSmallUnit(cock.length, false) + " long and " +
					Measurement.ToNearestSmallUnit(cock.girth, false, false));
				if (rando % 3 == 0)
				{
					sb.Append("wide. ");
				}
				else if (rando % 3 == 1)
				{
					sb.Append("thick. ");
				}
				else
				{
					sb.Append(" in diameter. ");
				}

				sb.Append(cock.PlayerDescription() + Environment.NewLine);

			}

			return sb.ToString();
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
			if (hasClitCock && gender == Gender.FEMALE)
			{
				sb.Append("During your travels, you've gained the ability to transform your " + (vaginas.Count == 2 ? "first " : "") + "clit into a cock, allowing you to indulge "
					+ "in the sexual pleasures of a man while still remaining a woman. ");
				if (player.relativeLust > 80)
				{
					sb.Append("Unfortunately, it sometimes has a mind of its own and you're having trouble keeping it under control. ");
				}
			}

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

		private string LactationSlowedDownDueToInactivity(bool becameOverFullThisPass, LactationStatus oldLevel)
		{
			int delta = oldLevel - lactationStatus;
			if (lactationStatus > LactationStatus.STRONG)
			{
				string hitLongDescing = becameOverFullThisPass ? ", though they're still very much in need of a milking" : "";
				return "\n<b>Your breasts feel somewhat lighter, though not much. It seems you aren't producing milk at such an ungodly rate anymore" + hitLongDescing + ".</b>\n";
			}
			else if (lactationStatus > LactationStatus.MODERATE)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production starts to wind down." + hitLongDescing + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.LIGHT)
			{
				string helperStr = delta > 1 ? "significantly " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still very tender and probably should be milked soon." : "";

				return "\n<b>Your breasts feel " + helperStr + "lighter as your body's milk production winds down." + hitLongDescing + "</b>\n";
			}
			else if (lactationStatus > LactationStatus.NOT_LACTATING)
			{
				string helperStr = delta > 1 ? "significantly, " : "";
				string hitLongDescing = becameOverFullThisPass ? "Despite this, they're still tender and probably should be milked soon." : "";
				return "\n<b>Your body's milk output drops " + helperStr + "down to what would be considered 'normal' for a pregnant woman." + hitLongDescing + "</b>\n";
			}
			else
			{
				return "\n<b>Your body no longer produces any milk.</b>\n";
			}
		}
		private string LactationFullWarning()
		{
			return Environment.NewLine + "<b>Your " + breastRows[0].nipples.ShortDescription() + "s feel swollen and bloated, needing to be milked.</b>" + Environment.NewLine;
		}
	}

	partial class GenitalsData : IGenitals
	{
		ReadOnlyCollection<CockData> IGenitals.cocks => this.cocks;

		ReadOnlyCollection<VaginaData> IGenitals.vaginas => this.vaginas;

		ReadOnlyCollection<BreastData> IGenitals.breasts => breasts;

		bool IGenitals.hasSheath => this.hasSheath;
	}

}
