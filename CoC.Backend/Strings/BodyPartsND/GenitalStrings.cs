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

			return mismatched ? CockType.HUMAN.shortDescription() + "s" : cocks[0].ShortDescription() + "s";
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

				return Utils.RandomChoice(countOptions) + AverageCock().AdjectiveText(false) + description;
			}
		}

		public string AllCocksFullDescription()
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

				return Utils.RandomChoice(countOptions) + AverageCock().AdjectiveText(true) + description;
			}
		}

		public string AllCocksPlayerDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
				sb.Append("Unlike most " + gender.AsText() + "s, you have a pair of vaginas, situated alongside one another. The first is " +
					vaginas[0].FullDescription() + "; the second is " + vaginas[1].FullDescription());
			}

			return sb.ToString();
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
