using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	partial class Genitals
	{
		public static string Name()
		{
			return "Genitals";
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

		/*
		public static function multiCockDescript(creature:Creature):String
		{
			//Get cock counts
			var descript = "";
		var currCock = 0;
		var totCock = creature.cocks.length;
		var dogCocks = 0;
		var wolfCocks = 0;
		var horseCocks = 0;
		var normalCocks = 0;
		var normalCockKey = 0;
		var dogCockKey = 0;
		var wolfCockKey = 0;
		var horseCockKey = 0;
		var averageLength = 0;
		var averageThickness = 0;
		var same = true;
		//For temp14 random values
		var rando = 0;
		var descripted = false;
			//Count cocks & Prep average totals
			while (currCock <= totCock - 1) {
				if (creature.cocks[currCock].cockType == CockTypesEnum.HUMAN) {
					normalCocks++;
					normalCockKey = currCock;
				}
				if (creature.cocks[currCock].cockType == CockTypesEnum.HORSE) {
					horseCocks++;
					horseCockKey = currCock;
				}
				if (creature.cocks[currCock].cockType == CockTypesEnum.DOG) {
					dogCocks++;
					dogCockKey = currCock;
				}
				if (creature.cocks[currCock].cockType == CockTypesEnum.WOLF) {
					wolfCocks++;
					wolfCockKey = currCock;
				}
				averageLength += creature.cocks[currCock].cockLength;
				averageThickness += creature.cocks[currCock].cockThickness;
				//If cocks are matched make sure they still are
				if (same && currCock > 0 && creature.cocks[currCock].cockType != creature.cocks[currCock - 1].cockType) same = false;
				currCock++;
			}
			//Crunch averages
			averageLength /= currCock;
			averageThickness /= currCock;
			//Quantity descriptors
			if (currCock == 1) {
				if (dogCocks == 1) return cockNoun(CockTypesEnum.DOG);
				if (wolfCocks == 1) return cockNoun(CockTypesEnum.WOLF);
				if (horseCocks == 1) return cockNoun(CockTypesEnum.HORSE);
				if (normalCocks == 1) return cockDescript(creature,0);
				//Catch-all for when I add more cocks.  Let cock descript do the sorting.
				if (creature.cocks.length == 1) return cockDescript(creature,0);
			}
			if (currCock == 2) {
				//For cocks that are the same
				if (same) {
					descript += randomChoice("a pair of ", "two ", "a brace of ", "matching ", "twin ");
descript += cockAdjectives(averageLength, averageThickness, creature.cocks[0].cockType, creature);
					if (normalCocks == 2) descript += " " + cockNoun(CockTypesEnum.HUMAN) + "s";
					if (horseCocks == 2) descript += ", " + cockNoun(CockTypesEnum.HORSE) + "s";
					if (dogCocks == 2) descript += ", " + cockNoun(CockTypesEnum.DOG) + "s";
					if (wolfCocks == 2) descript += ", " + cockNoun(CockTypesEnum.WOLF) + "s";
					//Tentacles
					if (creature.cocks[0].cockType.Index > 2)
						descript += ", " + cockNoun(creature.cocks[0].cockType) + "s";
				}
				//Nonidentical
				else {
					descript += randomChoice("a pair of ", "two ", "a brace of ");
descript += cockAdjectives(averageLength, averageThickness, creature.cocks[0].cockType, creature) + ", ";
					descript += randomChoice("mutated cocks", "mutated dicks", "mixed cocks", "mismatched dicks");
				}
			}
			if (currCock == 3) {
				//For samecocks
				if (same) {
					descript += randomChoice("three ", "a group of ", "a <i>ménage à trois</i> of ", "a triad of ", "a triumvirate of ");
descript += cockAdjectives(averageLength, averageThickness, creature.cocks[currCock - 1].cockType, creature);
					if (normalCocks == 3)
						descript += " " + cockNoun(CockTypesEnum.HUMAN) + "s";
					if (horseCocks == 3)
						descript += ", " + cockNoun(CockTypesEnum.HORSE) + "s";
					if (dogCocks == 3)
						descript += ", " + cockNoun(CockTypesEnum.DOG) + "s";
					if (wolfCocks == 3)
						descript += ", " + cockNoun(CockTypesEnum.WOLF) + "s";
					//Tentacles
					if (creature.cocks[0].cockType.Index > 2) descript += ", " + cockNoun(creature.cocks[0].cockType) + "s";   // Not sure what's going on here, referencing index *may* be a bug.

				}
				else {
					descript += randomChoice("three ", "a group of ");
descript += cockAdjectives(averageLength, averageThickness, creature.cocks[0].cockType, creature);
descript += randomChoice(", mutated cocks", ", mutated dicks", ", mixed cocks", ", mismatched dicks");
				}
			}
			//Large numbers of cocks!
			if (currCock > 3) {
				descript += randomChoice("a bundle of ", "an obscene group of ", "a cluster of ", "a wriggling group of ");
//Cock adjectives and nouns
descripted = false;
				//If same types...
				if (same) {
					if (creature.cocks[0].cockType == CockTypesEnum.HUMAN) {
						descript += cockAdjectives(averageLength, averageThickness, CockTypesEnum.HUMAN, creature) + " ";
						descript += cockNoun(CockTypesEnum.HUMAN) + "s";
						descripted = true;
					}
					if (creature.cocks[0].cockType == CockTypesEnum.DOG) {
						descript += cockAdjectives(averageLength, averageThickness, CockTypesEnum.DOG, creature) + ", ";
						descript += cockNoun(CockTypesEnum.DOG) + "s";
						descripted = true;
					}
					if (creature.cocks[0].cockType == CockTypesEnum.WOLF) {
						descript += cockAdjectives(averageLength, averageThickness, CockTypesEnum.WOLF, creature) + ", ";
						descript += cockNoun(CockTypesEnum.WOLF) + "s";
						descripted = true;
					}
					if (creature.cocks[0].cockType == CockTypesEnum.HORSE) {
						descript += cockAdjectives(averageLength, averageThickness, CockTypesEnum.HORSE, creature) + ", ";
						descript += cockNoun(CockTypesEnum.HORSE) + "s";
						descripted = true;
					}
					//TODO More group cock type descriptions!
					if (creature.cocks[0].cockType.Index > 2) {
						descript += cockAdjectives(averageLength, averageThickness, CockTypesEnum.HUMAN, creature) + ", ";
						descript += cockNoun(creature.cocks[0].cockType) + "s";
						descripted = true;
					}
				}
				//If mixed
				if (!descripted) {
					descript += cockAdjectives(averageLength, averageThickness, creature.cocks[0].cockType, creature) + ", ";
					rando = rand(4);
descript += randomChoice("mutated cocks", "mutated dicks", "mixed cocks", "mismatched dicks");
				}
			}
			return descript;
		}
	}
}
*/

		public string AllCocksShortDescription()
		{
			if (cocks.Count == 0)
			{
				return "";
			}
			else if (cocks.Count == 1)
			{
				return cocks[0].ShortDescription();
			}
			bool mismatched = _cocks.Exists(x => x.type != _cocks[0].type);

			return mismatched ? CockType.GenericCockNoun() + "s" : cocks[0].ShortDescription() + "s";
		}

		public string OneCockOrCocksText(bool capital)
		{
			if (cocks.Count == 0)
			{
				return "";
			}
			else if (cocks.Count == 1)
			{
				return (capital ? "Your " : "your ") + AllCocksShortDescription();
			}
			else return (capital ? "One " : "one ") + "of your " + AllCocksShortDescription();
		}

		public string EachCockOrCocksText(bool capital)
		{
			if (cocks.Count == 0)
			{
				return "";
			}
			else if (cocks.Count == 1)
			{
				return (capital ? "Your " : "your ") + AllCocksShortDescription();
			}
			else return (capital ? "Each " : "each ") + "of your " + AllCocksShortDescription();
		}

		public string AllCocksLongDescription()
		{
			return AllCocksDesc(false);
		}

		public string AllCocksFullDescription()
		{
			return AllCocksDesc(true);
		}

		private string AllCocksDesc(bool full)
		{
			if (cocks.Count == 0)
			{
				return "";
			}
			//If one, return normal cock descript
			else if (cocks.Count == 1)
			{
				return cocks[0].ShortDescription();
			}
			else
			{
				bool mismatched = _cocks.Exists(x => x.type != _cocks[0].type);

				string[] countOptions;
				string description;

				if (cocks.Count == 2)
				{
					countOptions = mismatched ? mismatchedPairOptions : matchedPairOptions;
				}
				else if (cocks.Count == 3)
				{
					countOptions = mismatched ? mismatchedTripleOptions : mismatchedTripleOptions;
				}
				else
				{
					countOptions = lottaDicksOptions;
				}

				description = mismatched ? Utils.RandomChoice(mismatchedDicksTextOptions) : cocks[0].ShortDescription() + "s";

				return Utils.RandomChoice(countOptions) + AverageCock().AdjectiveText(full) + description;
			}
		}

		public string AllCocksPlayerDescription()
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

				sb.Append(cock.LongDescription() + (plural ? "s" : "") + " is " + Measurement.ToNearestLargeAndSmallUnit(cock.length, false) + " long and " +
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

		public string SheathOrBaseText()
		{
			return hasSheath ? "sheath" : "base";
		}

		public string AllVaginasShortDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public string AllVaginasLongDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}



		public string AllVaginasPlayerText()
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
				sb.Append(vaginas[0].PlayerDescription());
			}
			else
			{
				sb.Append("Unlike most " + gender.AsText() + "s, you have a pair of vaginas, situated alongside one another. ");
				foreach (var vag in vaginas)
				{
					sb.Append(vag.PlayerDescription());
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
						return "Both of your " + VaginaType.VaginaNoun() + "s are " + loosenessText(first.looseness) + ", but otherwise normal.";
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
						return "Both of your " + VaginaType.VaginaNoun() + "s are " + loosenessText(first.looseness) + ", and " + WetnessText(first).ToLower() + "from both of them";
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

					return "Both of your " + VaginaType.VaginaNoun() + "s are " + loosenessText(first.looseness) + ", but your " + wetter + " is wetter - " + describe + ".";
				}
				//same wetness, differnet looseness.
				else if (first.wetness == second.wetness)
				{

					if (first.looseness >= VaginalLooseness.GAPING && second.looseness >= VaginalLooseness.GAPING)
					{
						string looser = first.looseness > second.looseness ? "first" : "second";

						return "Both of your " + VaginaType.VaginaNoun() + "s are " + first.wetness.AsAdjective() + " and are so loose they gape; but your " + looser +
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

						return "Both of your " + VaginaType.VaginaNoun() + "s are " + first.wetness.AsAdjective() + ", but your " + looser + " is looser - " + describe + ".";
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

		private string BreastRowCountText(bool withEven)
		{
			string evenStr = withEven ? (_breasts.Exists(x => x.cupSize != _breasts[0].cupSize) ? "uneven " : "even ") : "";
			if (_breasts.Count <= 1)
			{
				return "";
			}
			else if (_breasts.Count == 2)
			{
				return "two " + evenStr + "rows of ";
			}
			else if (_breasts.Count == 3)
			{
				return Utils.RandBool() ? "three " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "multi-layered ";
			}
			else //if (creature.breasts.Count == 4)
			{
				return Utils.RandBool() ? "four " + evenStr + "rows of " : (!string.IsNullOrEmpty(evenStr) ? evenStr + ", " : "") + "four-tiered ";
			}
		}

		public string AllBreastsShortDescription()
		{
			return BreastRowCountText(true) + this.AverageBreasts().ShortDescription();
		}

		public string AllBreastsLongDescription()
		{
			return BreastRowCountText(true) + this.AverageBreasts().LongDescription(false);
		}

		public string AllBreastsFullDescription()
		{
			return BreastRowCountText(true) + this.AverageBreasts().FullDescription(false);
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
}
