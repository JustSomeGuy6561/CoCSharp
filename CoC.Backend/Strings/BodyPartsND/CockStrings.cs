//CockNBallzStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 4:08 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Linq;
using System.Text;

namespace CoC.Backend.BodyParts
{
#warning Handle GOO TFs so they suck less

	public partial class CockPiercingLocation
	{
		private static string AlbertButton()
		{
			return "PrinceAlbert";
		}

		private static string AlbertLocation()
		{
			return "cock-head";
		}

		private static string UpperFrenum1Button()
		{
			return UpperFrenumButton(1);
		}
		private static string UpperFrenum1Location()
		{
			return UpperFrenumLocation(1);
		}

		private static string UpperFrenum2Button()
		{
			return UpperFrenumButton(2);
		}
		private static string UpperFrenum2Location()
		{
			return UpperFrenumLocation(2);
		}

		private static string MiddleFrenum1Button()
		{
			return MiddleFrenumButton(1);
		}
		private static string MiddleFrenum1Location()
		{
			return MiddleFrenumLocation(1);
		}

		private static string MiddleFrenum2Button()
		{
			return MiddleFrenumButton(2);
		}
		private static string MiddleFrenum2Location()
		{
			return MiddleFrenumLocation(2);
		}

		private static string MiddleFrenum3Button()
		{
			return MiddleFrenumButton(3);
		}
		private static string MiddleFrenum3Location()
		{
			return MiddleFrenumLocation(3);
		}

		private static string MiddleFrenum4Button()
		{
			return MiddleFrenumButton(4);
		}
		private static string MiddleFrenum4Location()
		{
			return MiddleFrenumLocation(4);
		}

		private static string LowerFrenum1Button()
		{
			return LowerFrenumButton(1);
		}
		private static string LowerFrenum1Location()
		{
			return LowerFrenumLocation(1);
		}

		private static string LowerFrenum2Button()
		{
			return LowerFrenumButton(2);
		}
		private static string LowerFrenum2Location()
		{
			return LowerFrenumLocation(2);
		}

		private static string LowerFrenumButton(byte index)
		{
			return $"L.Frenum({index})";
		}

		private static string MiddleFrenumButton(byte index)
		{
			return $"M.Frenum({index})";
		}

		private static string UpperFrenumButton(byte index)
		{
			return "upper frenum";
		}

		private static string LowerFrenumLocation(byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return $"L.Frenum({index})";
		}

		private static string MiddleFrenumLocation(byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return $"M.Frenum({index})";
		}

		private static string UpperFrenumLocation(byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return $"U.Frenum({index})";
		}
	}

	internal interface ICock
	{
		string ShortDescription();

		string HeadDescription();

		string AdjectiveText(bool multipleAdjectives);

		string ShortDescriptionNoAdjective();
		string ShortDescriptionNoAdjective(bool plural);

		string ShortDescription(bool noAdjective, bool plural);

		string LongDescription(bool alternateFormat = false);

		string LongDescriptionPrimary();

		string LongDescriptionAlternate();

		string FullDescription(bool alternateFormat);

		string FullDescription();

		string FullDescriptionPrimary();

		string FullDescriptionAlternate();

		CockType type { get; }
	}

	public partial class Cock : ICock
	{
		public static string Name()
		{
			return "Cock";
		}

		private string AllCockPiercingsShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllCockPiercingsLong(Creature creature)
		{
			StringBuilder sb = new StringBuilder();
			//full piercings, or full piercings without prince albert.
			//if (cockPiercings.jewelryCount == cockPiercings.MaxPiercings)
			//{
			//	sb.Append("Piercings run down the entire length of your cock, crowned by a " + cockPiercings[CockPiercingLocations.PRINCE_ALBERT].LongDescription() + "in its " +
			//		HeadDescription() + ". You have little doubt it'll make an impression on your partners. ");
			//}
			//else if (cockPiercings.jewelryCount == cockPiercings.MaxPiercings - 1 && !cockPiercings.WearingJewelryAt(CockPiercingLocations.PRINCE_ALBERT))
			//{
			//
			//}
			int frenumJewelryCount = piercings.jewelryCount;
			bool hasPA = piercings.WearingJewelryAt(CockPiercingLocation.PRINCE_ALBERT);
			if (hasPA)
			{
				frenumJewelryCount--;
				//sb.Append("Looking positively pervese, " + cockPiercings[CockPiercingLocations.PRINCE_ALBERT].LongDescription(true) + "adorns your " + HeadDescription() + ".");
			}

			if (frenumJewelryCount == piercings.MaxFrenumPiercings)
			{
				string intro, outro;
				if (hasPA)
				{
					intro = " Additional piercings ";
					outro = ", completing the look";
				}
				else
				{
					intro = "Piercings, ";
					outro = ", giving you a full ladder";
				}
				sb.Append(intro + "run down the entire length of your cock" + outro);

				//if (hasPA, giving you a full ladder. You have little doubt they'll make ")
			}


			if (piercings.jewelryCount > 1 || (piercings.wearingJewelry && !piercings.WearingJewelryAt(CockPiercingLocation.PRINCE_ALBERT)))
			{

			}

			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public static string GenericCockNoun(bool plural = false) => CockType.GenericCockNoun(plural);
	}

	public partial class CockType
	{
		public static string GenericCockNoun(bool plural = false)
		{
			if (!plural) return Utils.RandomChoice("cock", "dick", "dong", "endowment", "mast", "member", "pecker", "penis", "prick", "shaft", "tool");
			else return Utils.RandomChoice("cocks", "dicks", "dongs", "endowments", "masts", "members", "peckers", "penises", "pricks", "shafts", "tools");
		}

		//New cock adjectives. The old one sucked dicks
		//This function handles all cockAdjectives. Previously there were separate functions for the player, monsters and NPCs.
		//C# note: this still works for everything, but all the random data is wrapped in the CockData class. It's more flexible this way- adding or removing
		//extra variables wont break existing code.
		internal static string CockAdjectiveText(CockData cock, bool multipleAdjectives)
		{
			//modified to allow ultra verbose descriptions - the original order of precedence is kept, but the order it's displayed is new.
			//note that this only applies to the verbose version with multiple objectives - for single adjective, this is irrelevant.
			//verbose : size + horny + pierced + sock

			//if not using multiple objectives, only one of these will ever be true, and it's possible none of these are true. we handle this case with girth.
			//otherwise, they will be true if the corresponding conditions are met.
			bool pierced = false, sock = false, size = false, lustiness = false;

			bool anyHit() => pierced || sock || size || lustiness;

			//determine what, if anything are going to use to desribe it.
			//First, the possible special cases
			//pierced. 20% chance if cock is pierced, unless we're forcing all adjectives, then 100%.
			if (cock.cockPiercings.isPierced && (multipleAdjectives || Utils.Rand(5) == 0))
			{
				pierced = true;
			}

			//if (isGooey && Utils.Rand(4) == 0) return Utils.RandomChoice("goopey", "gooey", "slimy");

			//cocksock check. 20% chance if none of the previous are hit, or 100% if forcing all adjectives.
			if (cock.cockSock != null && (multipleAdjectives || (!pierced && Utils.Rand(5) == 0)))
			{
				sock = true;
			}
			//length check. no prerequisites. if we aren't doing multiple adjectives, occurs 33% of the time, but only if none of the above options have been hit.
			if (multipleAdjectives || (!anyHit() && Utils.Rand(3) == 0))
			{
				size = true;
			}
			//lust check. must be at or above 75% lust. 50% chance if none of the above have been hit, or 100% chance if we are doing multiple objectives.
			if (cock.currentRelativeLust >= 75 && (multipleAdjectives || (!anyHit() && Utils.RandBool())))
			{
				lustiness = true;
			}

			//handle the edge case where none of the above are hit. only occurs if multiple adjectives is false.
			//Girth - fallback
			if (!anyHit())
			{
				if (cock.girth <= 0.75) return Utils.RandomChoice("thin", "slender", "narrow");
				else if (cock.girth <= 1.2) return "ample";
				else if (cock.girth <= 1.4) return Utils.RandomChoice("ample", "big");
				else if (cock.girth <= 2) return Utils.RandomChoice("broad", "meaty", "girthy");
				else if (cock.girth <= 3.5) return Utils.RandomChoice("fat", "distended", "wide");
				else return Utils.RandomChoice("inhumanly distended", "monstrously thick", "bloated");
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				if (size)
				{
					if (cock.length < 3) sb.Append(Utils.RandomChoice("little", "toy-sized", "mini", "budding", "tiny"));
					else if (cock.length < 5) sb.Append(Utils.RandomChoice("short", "small"));
					else if (cock.length < 7) sb.Append(Utils.RandomChoice("fair-sized", "nice"));
					else if (cock.length < 9)
					{
						if (cock.type == CockType.HORSE) sb.Append(Utils.RandomChoice("sizable", "pony-sized", "colt-like"));
						else sb.Append(Utils.RandomChoice("sizable", "long", "lengthy"));
					}
					else if (cock.length < 13)
					{
						if (cock.type == CockType.DOG) sb.Append(Utils.RandomChoice("huge", "foot-long", "mastiff-like"));
						else sb.Append(Utils.RandomChoice("huge", "foot-long", "cucumber-length"));
					}
					else if (cock.length < 18) sb.Append(Utils.RandomChoice("massive", "knee-length", "forearm-length"));
					else if (cock.length < 30) sb.Append(Utils.RandomChoice("enormous", "giant", "arm-like"));
					else if (cock.type == CockType.TENTACLE && Utils.RandBool()) sb.Append("coiled");
					else sb.Append(Utils.RandomChoice("towering", "freakish", "monstrous", "massive"));
				}

				if (lustiness)
				{
					if (sb.Length != 0)
					{
						sb.Append(", ");
					}

					//Uber horny like a baws!
					if (cock.currentRelativeLust > 90)
					{
						if (cock.cumAmount < 50) sb.Append(Utils.RandomChoice("throbbing", "pulsating")); //Weak as shit cum
						else if (cock.cumAmount < 200) sb.Append(Utils.RandomChoice("dribbling", "leaking", "drooling")); //lots of cum? drippy.
						else sb.Append(Utils.RandomChoice("very drippy", "pre-gushing", "cum-bubbling", "pre-slicked", "pre-drooling")); //Tons of cum
					}
					//A little less lusty, but still lusty.
					else
					{
						if (cock.cumAmount < 50) sb.Append(Utils.RandomChoice("turgid", "blood-engorged", "rock-hard", "stiff", "eager")); //Weak as shit cum
						else if (cock.cumAmount < 200) sb.Append(Utils.RandomChoice("turgid", "blood-engorged", "rock-hard", "stiff", "eager", "fluid-beading", "slowly-oozing")); //A little drippy
						else sb.Append(Utils.RandomChoice("dribbling", "drooling", "fluid-leaking", "leaking")); //uber drippy
					}
				}

				if (pierced)
				{
					if (sb.Length != 0)
					{
						sb.Append(", ");
					}

					sb.Append("pierced");
				}

				if (sock)
				{
					if (sb.Length != 0)
					{
						sb.Append(", ");
					}

					sb.Append(Utils.RandomChoice("sock-sheathed", "garment-wrapped", "smartly dressed", "cloth-shrouded", "fabric swaddled", "covered"));
				}
				sb.Append(" ");

				return sb.ToString();
			}
		}

		private static string HumanCockHeadDesc()
		{
			return Utils.RandomChoice("crown", "crown", "head", "cock-head");
		}
		private static string HumanDesc(bool noAdjective, bool plural = false)
		{
			return (Utils.RandBool() || noAdjective ? Utils.RandomChoice("human ", "humanoid ", "ordinary-looking ") : "") + GenericCockNoun(plural);
		}

		private static string HumanSingleDesc(bool noAdjective)
		{
			return Utils.RandomChoice("a human ", "a humanoid ", "an ordinary-looking ") + GenericCockNoun();
		}

		private static string HumanLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string HumanPlayerStr(Cock cock, PlayerBase player)
		{
			string humanText;
			if (cock.length > 9)
			{
				humanText = "Most men back in Ingnam would be proud to have a cock as long as yours.";
			}
			else if (cock.length < 4)
			{
				humanText = "It's pretty small, even by human standards.";
			}
			else
			{
				humanText = "As human cocks go, it's pretty average.";
			}
			return humanText + GenericCockSockText(cock, player);
		}
		private static string HumanTransformStr(CockData previousCockData, PlayerBase player)
		{
			return previousCockData.type.RestoredString(previousCockData, player);
		}
		private static string HumanGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string HumanRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(originalCockData, player);
		}
		private static string HumanRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}


		private static string HorseGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string HorseCockHeadDesc()
		{
			return Utils.RandomChoice("flare", "flat tip");
		}
		private static string HorseDesc(bool noAdjective, bool plural = false)
		{
			string adj = (Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("flared ", "bestial ", "flat-tipped ", "mushroom-headed ", "") : "");
			return adj + Utils.RandomChoice("horse-", "equine ", "stallion-", "beast ") + GenericCockNoun(plural);
		}
		private static string HorseSingleDesc(bool noAdjective)
		{
			if (!noAdjective && Utils.Rand(3) >= 1)
			{
				return Utils.RandomChoice("a flared ", "a bestial ", "a flat-tipped ", "a mushroom-headed ")
					+ Utils.RandomChoice("horse-", "equine ", "stallion-", "beast ") + GenericCockNoun(false);
			}
			else
			{
				return Utils.RandomChoice("a horse-", "an equine ", "a stallion-", "a beast ") + GenericCockNoun(false);
			}
		}

		private static string HorseLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string HorsePlayerStr(Cock cock, PlayerBase player)
		{
			return " It's mottled black and brown in a very animalistic pattern. The 'head' of its shaft flares proudly, just like a horse's." + GenericPlayerPostText(cock, player);
		}
		private static string HorseTransformStr(CockData previousCockData, PlayerBase player)
		{
			var grownCock = player.cocks[previousCockData.cockIndex];
			string wearingText = player.wearingLowerGarment || player.wearingArmor ? "you pull down your clothing to take" : "you give it ";
			if (previousCockData.type == CockType.HUMAN)
			{

				return "Your " + previousCockData.LongDescription() + " begins to feel strange... " + wearingText + " a look and see it " +
					"darkening as you feel a tightness near the base where your skin seems to be bunching up. A sheath begins forming around your cock's base, " +
					"tightening and pulling your cock inside its depths. A hot feeling envelops your member as it suddenly grows into a horse penis, dwarfing its old size. " +
					"The skin is mottled brown and black and feels more sensitive than normal. Your hands are irresistibly drawn to it, and you jerk yourself off, " +
					"splattering cum with intense force.";
			}
			else //if (previousCockData.type == CockType.DOG) //it was low key the same text, just with knot and sheath text. since we have others that do that too, i just made it generic
			{
				string measurement = Measurement.UsesMetric ? "centimeter" : "inch";
				string sheathText = player.cocks.Any(x => x.cockIndex != previousCockData.cockIndex && x.requiresASheath) ?
					"Your cock pushes out of your sheath, " + measurement + " after " + measurement + " of it as it grows beyond it's traditional size." :
					"Your skin folds and bunches around the base, forming an an animalistic sheath.";
				string knotText = previousCockData.hasKnot ? " You notice your knot vanishing, the extra flesh surging into new length instead." : "";

				return "Your " + previousCockData.LongDescription() + " begins to feel odd... " + wearingText + " a look and see it darkening. " +
					"You feel a growing tightness in the tip of your " + previousCockData.ShortDescription() + " as it flattens, flaring outwards. " +
					 sheathText + knotText + " Your hands are drawn to the strange new " + grownCock.ShortDescription() + ", and you jerk yourself off, " +
					"splattering thick ropes of cum with intense force.";
			}
		}
		private static string HorseRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string HorseRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string DogGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			StringBuilder sb = new StringBuilder("A painful lump forms on your groin, nearly doubling you over");

			if (player.wearingAnything)
			{
				sb.Append("as it presses against your " + player.armor.ItemName() + ".You rip open your gear and watch,");
			}
			else
			{
				sb.Append(". You can't help but watch,");
			}
			sb.Append(" horrified, as the discolored skin epands into a red-tipped point. A feeling of relief and surprising lust grows as it pushes forward, " +
				"glistening red and thickening. ");

			//if we didn't previously have a sheath.
			if (!player.cocks.Any(x => x.cockIndex != grownCockIndex && x.requiresASheath))
			{
				sb.Append("The skin bunches up into an animal-like sheath");
				//had a cock before growing this one.
				if (player.cocks.Count > 1)
				{
					sb.Append(", capturing your other cocks with it. A");
				}
				else
				{
					sb.Append(", and a");
				}
			}
			else
			{
				sb.Append("A");
			}
			sb.Append("fat bulge pops from free, forming into a thick canine cock with a decent-sized knot. It pulses and dribble animal-pre, arousing you in spite of " +
				"your attempts at self-control.");

			return sb.ToString();
		}
		private static string DogCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string DogDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("pointed ", "knotty ", "knotted ", "bestial ", "animalistic ") : "";
			return adj + Utils.RandomChoice("dog-", "dog-shaped ", "canine ", "bestial ", " puppy-", "canine ") + GenericCockNoun(plural);
		}

		private static string DogSingleDesc(bool noAdjective)
		{
			//we're lucky, the dog nouns are all 'a' articles, so i can be lazy.
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("a pointed ", "a knotty ", "a knotted ", "a bestial ", "an animalistic ") : "a ";
			return adj + Utils.RandomChoice("dog-", "dog-shaped ", "canine ", "bestial ", " puppy-", "canine ") + GenericCockNoun(false);
		}

		private static string DogLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string DogPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is shiny, pointed, and covered in veins, just like a large dog's cock." + GenericPlayerPostText(cock, player);
		}
		private static string DogTransformStr(CockData previousCockData, PlayerBase player)
		{
			string knotText = previousCockData.hasKnot ? "while the know reshapes itself into something more canine. " : " while a gradually widening bulge forms around the base. ";

			string sheathText = previousCockData.currentlyHasSheath ? "" : "Where it meets your crotch, the skin bunches up around it, forming a canine -like sheath. ";

			return "Your " + previousCockData.LongDescription() + " vibrates, the veins clearly visible as it reddens and distorts. The head narrows into a pointed tip " +
				knotText + sheathText + SafelyFormattedString.FormattedText("Your " + previousCockData.ShortDescription() + " is now " +
				player.cocks[previousCockData.cockIndex].LongDescription(true), StringFormats.BOLD);

		}
		private static string DogRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string DogRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string DemonGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			Cock grownCock = player.cocks[grownCockIndex];

			string measure = Measurement.UsesMetric ? "few centimeters" : "inch";
			StringBuilder sb = new StringBuilder("A sudden pressure builds in your groin draws your attention. Your skin ripples and bulges outwards, the sensation turning " +
				"from pressure to feelings of intense warmth. The bump distends, turning purple near the tip as it reaches " + Measurement.ToNearestHalfSmallUnit(3, false, true) +
				" in size.  You touch it " + "and cry out with pleasure, watching it leap forwards another " + measure + " in response. At this point, you are sure " +
				SafelyFormattedString.FormattedText("you are growing a new dick!", StringFormats.BOLD));
			if (grownCock.length <= 4)
			{
				sb.Append("Your tiny dick's crown becomes more and more defined, until it looks like a regular, albeit minute, dick. ");
			}
			else
			{
				double size = Math.Min(6, grownCock.length);
				sb.Append("Your tiny dick's crown becomes more and more defined as it grows larger, until you have what looks like a normal " +
					Measurement.ToNearestHalfSmallUnit(size, false, true, false) + " dick. ");
			}
			sb.Append("You sigh with happiness and desire at your new addition. ");
			if (grownCock.length > 6)
			{
				double size = Math.Min(8, grownCock.length);
				sb.Append("Before you can enjoy it, another wave of heat washes through you, making your new addition respond. It grows painfully hard as it crests at " +
					Measurement.ToNearestHalfSmallUnit(grownCock.length, false, false) + " in length. ");
			}

			if (player.corruption < 80)
			{
				sb.Append("You watch in horror as the skin turns a shiny-dark purple. Tiny wriggling nodules begin to erupt from the purplish skin, making your cock " +
					"look more like a crazed sex-toy than a proper penis. You pant and nearly cum ");
				if (grownCock.length > 8)
				{
					sb.Append("as it lengthens one last time, peaking at " + Measurement.ToNearestHalfSmallUnit(grownCock.length, false, true) + " long.");
				}
				else
				{
					sb.Append("from the increase in sensitivity they provide.");
				}
				sb.Append("One last ring of nodules forms around the edge of your demon-dick's crown, pulsating darkly with each beat of your horrified heart.");
			}
			else
			{
				sb.Append("You watch, curious, as the skin turn a shiny-dark purple. Tiny wriggling nodules begin to erupt from the purplish skin, making your penis " +
					"look more like those amazing cocks you saw on demons! You pant and moan in happiness ");
				if (grownCock.length > 8)
				{
					sb.Append("as it lengthens one last time, peaking at " + Measurement.ToNearestHalfSmallUnit(grownCock.length, false, true) + " long.");
				}
				else
				{
					sb.Append("from the increase in sensitivity they provide.");
				}
				sb.Append("The excitement of possessing such a magnificent pleasure tool makes you cum. As one last ring of nodules forms around the edge " +
					"of your new demon-dick's crown, you notice to your surprise that the liquid you ejaculated is pitch black! But as your new cock pulsates darkly " +
					"with each beat of your heart, the only thing you have on your mind is to try it out as soon as possible...");
			}

			return sb.ToString();
		}
		private static string DemonCockHeadDesc()
		{
			return Utils.RandomChoice("tainted crown", "nub-ringed tip");
		}
		private static string DemonDesc(bool noAdjective, bool plural = false)
		{
			string adj = noAdjective ? "" : Utils.RandomChoice("corrupted ", "nub-covered ", "nubby ", "perverse ", "bumpy ", "cursed ", "infernal ", "unholy ", "blighted ");
			return adj + (Utils.RandBool() || noAdjective ? Utils.RandomChoice("demon-", "demonic ") : "") + GenericCockNoun(plural);
		}

		private static string DemonSingleDesc(bool noAdjective)
		{
			string adj = !noAdjective && Utils.RandBool()
				? Utils.RandomChoice("a corrupted ", "a nub-covered ", "a nubby ", "a perverse ", "a bumpy ", "a cursed ", "an infernal ", "an unholy ", "a blighted ")
				: "a ";
			return adj + Utils.RandomChoice("demon-", "demonic ") + GenericCockNoun(false);
		}

		private static string DemonLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string DemonPlayerStr(Cock cock, PlayerBase player)
		{
			return " The crown is ringed with a circle of rubbery protrusions that grow larger as you get more aroused. The entire thing is shiny and covered with tiny, " +
				"sensitive nodules that leave no doubt about its demonic origins." + GenericPlayerPostText(cock, player);
		}
		//revert to human, then grow nodules and such. not really used in game tbh.
		private static string DemonTransformStr(CockData previousCockData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("A wave of burning desire suddenly passes through you");

			Cock newCock = player.cocks[previousCockData.cockIndex];
			string measure = Measurement.UsesMetric ? "centimeter" : "inch";
			if (player.wearingAnything)
			{
				sb.Append(", and you undress");

				if (player.corruption < 40)
				{
					sb.Append(", powerless to stop yourself");
				}
			}
			sb.Append(".");


			if (player.corruption < 80)
			{
				sb.Append("You watch in horror as the skin of your " + previousCockData.LongDescription() + " turns shiny and purplish-black. ");
			}
			else
			{
				sb.Append("Curious, you watch the skin of your " + previousCockData.LongDescription() + " turn a shiny-dark purple.");
			}

			if (previousCockData.hasKnot)
			{
				sb.Append(" Your knot also discolors, then shrinks into the rest of your length.");
			}

			if (player.corruption < 50)
			{
				sb.Append("Corrupt nodules begin to spring up over its entire length. " + SafelyFormattedString.FormattedText("Your penis is transforming into a " +
					newCock.ShortDescription() + "!", StringFormats.BOLD) + " The new nubs wriggle about as they sprout over every " + measure + " of surface, save for the head. " +
					"Unable to do anything but groan with forced pleasure and horror, you can only watch. One last batch of nodules forms in a ring around the crown of your " +
					newCock.LongDescription() + ", completing its transformation.");
			}
			else
			{
				sb.Append("As you watch expectantly, tiny wriggling nodules begin to erupt from the purplish skin, like those magnificent cocks you saw on demons! " +
					SafelyFormattedString.FormattedText("Your penis is transforming into a " + newCock.ShortDescription() + "!", StringFormats.BOLD) + "You pant and moan " +
					"in happiness at the transformation. As you stroke all of its amazing length with both hands, the excitement of possessing such a beautiful pleasure tool " +
					"makes you cum. As one last ring of nodules forms around the edge of your " + newCock.LongDescription() + "'s crown, you notice that your ejaculate is pitch black! " +
					"You ponder why this is the case, but as you stroke the dark, bumpy shaft, you decide that if this feels as good as this looks, it doesn't really matter. ");
			}
			return sb.ToString();
		}
		private static string DemonRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string DemonRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string TentacleGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string TentacleCockHeadDesc()
		{
			return Utils.RandomChoice("mushroom-like tip", "wide plant-like crown");
		}
		private static string TentacleDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("twisting ", "wriggling ", "writhing", "sinuous ", "squirming ", "undulating ", "slithering ") : "";
			return adj + Utils.RandomChoice("tentacle-", "plant-", "tentacle-", "plant-", "flora ", "smooth ", "vine-", "vine-shaped ", "", "") + GenericCockNoun(plural);
		}

		private static string TentacleSingleDesc(bool noAdjective)
		{
			string adj = Utils.RandBool() && !noAdjective
				? Utils.RandomChoice("a twisting ", "a wriggling ", "a writhing", "a sinuous ", "a squirming ", "a undulating ", "a slithering ")
				: "a ";
			return adj + Utils.RandomChoice("tentacle-", "plant-", "tentacle-", "plant-", "flora ", "smooth ", "vine-", "vine-shaped ") + GenericCockNoun(false);
		}

		private static string TentacleLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string TentaclePlayerStr(Cock cock, PlayerBase player)
		{
			return " With a thought, you can make it even longer, though you don't feel anything along this additional length. The entirety of its green surface is covered in " +
				"perspiring beads of slick moisture. It frequently shifts and moves of its own volition, the slightly oversized and mushroom-like head shifting in coloration " +
				"to purplish-red whenever you become aroused." + GenericPlayerPostText(cock, player);
		}
		private static string TentacleTransformStr(CockData previousCockData, PlayerBase player)
		{
			string armorText = player.wearingAnything ? ", and you quickly undress" + (player.relativeCorruption < 40 ? ", despite yourself" : "") : ".";

			string lustText = player.relativeCorruption > 50 ? "While you're usually down for a bit of self-pleasure, you're not really in the mood right now. "
				: "You try to push this unwanted urge from your mind and ignore the longing in your crotch. ";

			return "A sudden, overwhelming wave of lust overcomes you" + armorText + "The intense feeling makes your" + player.build.HipsLongDescription() + " twitch and squirm, " +
				"throbbing and hard, making your " + player.genitals.AllCocksLongDescription() + " bob in the air. " + lustText + player.genitals.OneCockOrCocksShort() + "squirms in" +
				" protest, desperate for the attention. Wait, squirms? You look down, and see your formerly " + previousCockData.LongDescription() +
				SafelyFormattedString.FormattedText("is now a tentacle-dick, waving around and seeking a nearby orifice to fuck!", StringFormats.BOLD);
		}
		private static string TentacleRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string TentacleRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string CatGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string CatCockHeadDesc()
		{
			return Utils.RandomChoice("point", "narrow tip");
		}
		private static string CatDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("pink ", "animalistic ", "spiny ", "spined ", "oddly-textured ", "barbed ", "nubby ") : "";
			return adj + Utils.RandomChoice("feline ", "cat-", "kitty-", " kitten-") + GenericCockNoun(plural);
		}

		private static string CatSingleDesc(bool noAdjective)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective
				? Utils.RandomChoice("a pink ", "an animalistic ", "a spiny ", "a spined ", "an oddly-textured ", "a barbed ", "a nubby ")
				: "a ";
			return adj + Utils.RandomChoice("feline ", "cat-", "kitty-", " kitten-") + GenericCockNoun(false);
		}

		private static string CatLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string CatPlayerStr(Cock cock, PlayerBase player)
		{
			return " It ends in a single point, much like a spike, and is covered in small, fleshy barbs. The barbs are larger at the base " +
				"and shrink in size as they get closer to the tip. Each of the spines is soft and flexible, and shouldn't be painful for any of your partners." +
				GenericPlayerPostText(cock, player);
		}
		private static string CatTransformStr(CockData previousCockData, PlayerBase player)
		{
			string sheathText = previousCockData.currentlyHasSheath && !player.genitals.hasSheath
				? "Then, it begins to shrink and sucks itself inside your body. Within a few moments, a fleshy sheath is formed."
				: "";
			return "Your " + previousCockData.LongDescription() + " swells up with near-painful arousal and begins to transform. It turns pink and begins to narrow " +
				"until the tip is barely wide enough to accommodate your urethra.  Barbs begin to sprout from its flesh, if you can call the small, fleshy nubs barbs. " +
				"They start out thick around the base of your " + CockType.GenericCockNoun() + " and shrink towards the tip. The smallest are barely visible. " +
				SafelyFormattedString.FormattedText("Your new feline dong throbs powerfully", StringFormats.BOLD) + " and spurts a few droplets of cum. " + sheathText +
				"Once it's finished, it returns back to your sheath";
		}
		private static string CatRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string CatRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string LizardGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			string placementText = player.cocks.Count > 1 ? ", adjacent to your " + player.genitals.AllCocksShortDescription() : //has other cocks, else:
				player.vaginas.Count > 0 ? ", just above your " + player.genitals.AllVaginasShortDescription() : //has vagina(s), else:
				", contrasting with your otherwise featureless crotch"; //genderless

			string alterSkinText;

			if (player.body.mainEpidermis.type != EpidermisType.SKIN && player.body.hasSecondaryEpidermis && player.body.supplementaryEpidermis.type != EpidermisType.SKIN)
			{
				alterSkinText = ", and sheds " + player.body.mainEpidermis.ShortDescription() + " and " + player.body.supplementaryEpidermis.ShortDescription() + ". The";
			}
			else if (player.body.mainEpidermis.type != EpidermisType.SKIN)
			{
				alterSkinText = " and shedding " + player.body.mainEpidermis.ShortDescription() + " as";
			}
			else if (player.body.hasSecondaryEpidermis && player.body.supplementaryEpidermis.type != EpidermisType.SKIN)
			{
				alterSkinText = " and shedding " + player.body.supplementaryEpidermis.ShortDescription() + " as";
			}
			else
			{
				alterSkinText = " as";
			}

			Cock grownCock = player.cocks[grownCockIndex];

			int currentLizardCockCount = player.genitals.CountCocksOfType(grownCock.type);
			string typeCountText = currentLizardCockCount > 1 ? $"a {Utils.NumberAsPlace(currentLizardCockCount)}" : "a ";

			return "A knot of pressure forms in your groin, forcing you off your " + player.feet.ShortDescription() + " as you try to endure it. You examine the affected area " +
				"and see a lump starting to bulge under your " + player.body.mainEpidermis.ShortDescription() + placementText + ".  The flesh darkens, turning purple" +
				alterSkinText + " the bulge lengthens, pushing out from your body. Too surprised to react, you can only pant in pain and watch as the fleshy lump starts to take on " +
				"a penis-like appearance. " + SafelyFormattedString.FormattedText("You're growing " + typeCountText + "lizard-cock!", StringFormats.BOLD) +
				"It doesn't stop growing until it's " + Measurement.ToNearestHalfSmallUnit(grownCock.length, false, true) + "long. A dribble of cum oozes from its tip, " +
				"and you feel relief at last.";
		}
		private static string LizardCockHeadDesc()
		{
			return Utils.RandomChoice("crown", "head");
		}
		private static string LizardDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("purple ", "bulbous ", "bulging ") : "";
			return adj + Utils.RandomChoice("reptilian ", "inhuman ", "serpentine ", " snake-", " snake-") + GenericCockNoun(plural);
		}

		private static string LizardSingleDesc(bool noAdjective)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("a purple ", "a bulbous ", "a bulging ") : "a ";
			return adj + Utils.RandomChoice("reptilian ", "inhuman ", "serpentine ", " snake-", " snake-") + GenericCockNoun(false);
		}

		private static string LizardLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string LizardPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a deep, iridescent purple in color. Unlike a human penis, the shaft is not smooth, and is instead patterned with multiple bulbous bumps." + GenericPlayerPostText(cock, player);
		}
		private static string LizardTransformStr(CockData previousCockData, PlayerBase player)
		{
			string armorText = player.wearingArmor || player.wearingLowerGarment ? "Before it can progress any further, you yank back your " +
				(player.wearingArmor ? player.armor.ItemName() : player.lowerGarment.ItemName()) + " to investigate."
				: " After a quick look around to make sure you're relatively safe, you focus your eyes on the source of the sensation.";

			string corruptionText;
			if (player.corruption < 33)
			{
				corruptionText = "horrifies you.";
			}
			else if (player.corruption < 66)
			{
				corruptionText = "is a little strange for your tastes.";
			}
			else
			{
				corruptionText = "looks like it might be more fun to receive than use on others. " + (player.vaginas.Count > 0
					? "Maybe you could find someone else with one to ride?"
					: "Maybe you should test it out on someone and ask them exactly how it feels?");
			}

			Cock lizardCock = player.cocks[previousCockData.cockIndex];

			string sheathText = player.genitals.hasSheath ? GlobalStrings.NewParagraph() + "Your sheath tightens and starts to smooth out, revealing ever greater amounts of your "
				+ lizardCock.LongDescription() + "'s lower portions.  After a few moments " + SafelyFormattedString.FormattedText("your groin is no longer so animalistic – " +
					"the sheath is gone.", StringFormats.BOLD) : "";

			return "A slow tingle warms your groin. " + armorText + " Your " + previousCockData.LongDescription() + " is changing!  It ripples loosely from " +
				(previousCockData.currentlyHasSheath ? "sheath " : "base ") + "to tip, undulating and convulsing as its color lightens, darkens, " +
				"and finally settles on a purplish hue. Your " + CockType.GenericCockNoun() + " resolves itself into a bulbous form, with a slightly pointed tip. " +
				"The 'bulbs' throughout its shape look like they would provide an interesting ride for your sexual partners, but the perverse, alien pecker " + corruptionText +
				SafelyFormattedString.FormattedText("You now have a bulbous, lizard-like cock.", StringFormats.BOLD) + sheathText;
		}
		private static string LizardRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string LizardRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string AnemoneGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string AnemoneCockHeadDesc()
		{
			return Utils.RandomChoice("tentacle-ringed head", "stinger-laden crown");
		}
		private static string AnemoneDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("anemone ", "tentacle-ringed ", "blue ", "stinger-laden ", "pulsating ", "anemone ", "stinger-coated ", "blue ",
				"tentacle-ringed ", "near-transparent ", "squirming ") + GenericCockNoun(plural);
		}

		private static string AnemoneSingleDesc(bool noAdjective)
		{
			return Utils.RandomChoice("an anemone ", "a tentacle-ringed ", "a blue ", "a stinger-laden ", "a pulsating ", "an anemone ", "a stinger-coated ", "a blue ",
				"a tentacle-ringed ", "a near-transparent ", "a squirming ") + GenericCockNoun(false);
		}

		private static string AnemoneLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string AnemonePlayerStr(Cock cock, PlayerBase player)
		{
			return " The crown is surrounded by tiny tentacles with a venomous, aphrodisiac payload. At its base a number of similar, longer tentacles have formed, " +
				"guaranteeing that pleasure will be forced upon your partners." + GenericPlayerPostText(cock, player);
		}
		//in game this never happens, so idk how to explain it lol.
		//general idea lifted from cat, but obviously altered.
		private static string AnemoneTransformStr(CockData previousCockData, PlayerBase player)
		{
			string humanText = previousCockData.type == CockType.HUMAN ? " but otherwise remains mostly human. " : " then reshapes itself toward something that appears mostly human. ";

			string corruptText;
			//corrupt asshole/rapey response.
			if (player.corruption >= 80)
			{
				corruptText = " meaning your partners will enjoy it, not matter what you put them through. ";
			}
			//generic
			else if (player.corruption >= 50)
			{
				corruptText = " making it all the more pleasurable for your sexual partners. ";
			}
			//be considerate, i guess.
			else
			{
				corruptText = " and you'll have to be careful not to force it upon your sexual partners. ";
			}

			return "Your " + previousCockData.LongDescription() + " swells up with near-painful arousal and begins to transform. It shifts to a dark blue, " + humanText +
				"Its crown begins to swell, and throbs almost painfully, and you instinctively bring a " + player.hands.ShortDescription(false) + " to it. As you do, a pleasant, " +
				"tingling sensation begins to spread through your " + player.hands.ShortDescription(false) + ". It radiates through your body, and you suddenly have the overwheling " +
				"desire for sex. You can't help yourself but stroke your transforming member, quickly reaching climax. After your head clears, you notice the source of your sudden lust:" +
				SafelyFormattedString.FormattedText("You now have tiny tentacles along the bottom of your crown, like an anemone!", StringFormats.BOLD) + " Similar, longer tentacles " +
				"have also sprouted along the base. If your recent reaction is any indication, each one is filled with a natural aphrodesiac, " + corruptText;
		}
		private static string AnemoneRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string AnemoneRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string KangarooGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string KangarooCockHeadDesc()
		{
			return Utils.RandomChoice("tip", "point");
		}
		private static string KangarooDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("pointed ", "tapered ", "curved ", "squirming ") : "";
			return adj + (Utils.Rand(4) >= 1 || noAdjective ? Utils.RandomChoice("kangaroo-like ", "marsupial ") : "") + GenericCockNoun(plural);
		}

		private static string KangarooSingleDesc(bool noAdjective)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("a pointed ", "a tapered ", "a curved ", "a squirming ") : "a ";
			return adj + (Utils.Rand(4) >= 1 || noAdjective ? Utils.RandomChoice("kangaroo-like ", "marsupial ") : "") + GenericCockNoun(false);
		}
		private static string KangarooLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string KangarooPlayerStr(Cock cock, PlayerBase player)
		{
			return " It usually lies coiled inside a sheath, but undulates gently and tapers to a point when erect, somewhat like a taproot." + GenericPlayerPostText(cock, player);
		}
		private static string KangarooTransformStr(CockData previousCockData, PlayerBase player)
		{
			string clothesText = player.wearingArmor || player.wearingLowerGarment ? " and whip down your clothes to check" : " and pause to check";
			string sheathText = previousCockData.currentlyHasSheath ? "your sheath" : "a newly-formed sheath at its base";

			return "You feel a sharp pinch at the end of your penis" + clothesText + ".  Before your eyes, the tip of it collapses into a narrow point " +
				"and the shaft begins to tighten behind it, assuming a conical shape before it retracts into " + sheathText + ". " +
				SafelyFormattedString.FormattedText("You now have a kangaroo-penis!", StringFormats.BOLD);
		}
		private static string KangarooRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string KangarooRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string DragonGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string DragonCockHeadDesc()
		{
			return Utils.RandomChoice("tapered head", "tip");
		}
		private static string DragonDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("segmented ", "pointed ", "knotted ", "mythical ", "tapered", "unusual ", "scaly ") : "";
			return adj + Utils.RandomChoice("dragon-like ", "draconic ", "dragon-") + GenericCockNoun(plural);
		}

		private static string DragonSingleDesc(bool noAdjective)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("a segmented ", "a pointed ", "a knotted ", "a mythical ", "a tapered", "an unusual ", "a scaly ") : "a ";
			return adj + Utils.RandomChoice("dragon-like ", "draconic ", "dragon-") + GenericCockNoun(false);
		}

		private static string DragonLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string DragonPlayerStr(Cock cock, PlayerBase player)
		{
			return " With its tapered tip, there are few holes you wouldn't be able to get into. It has a strange, knot-like bulb at its base, " +
				"but doesn't usually flare during arousal as a dog's knot would. The knot is " + Measurement.ToNearestHalfSmallUnit(cock.knotSize, false, true) +
				" thick when at full size." + GenericCockSockText(cock, player);
		}
		private static string DragonTransformStr(CockData previousCockData, PlayerBase player)
		{
			string armorText;
			if (player.wearingArmor)
			{
				string lowerText = player.wearingLowerGarment ? " and remove your " + player.lowerGarment.ItemName() : "";
				armorText = "You pull open your " + player.armor.ItemName() + lowerText;
			}
			else if (player.wearingLowerGarment)
			{
				armorText = "You remove your " + player.lowerGarment.ItemName();
			}
			else
			{
				armorText = "You pause";
			}

			string cumText;
			if (player.relativeSensitivity >= 50)
			{
				cumText = ", but it's not until you press on your " + (previousCockData.hasKnot ? "now smaller, more sensitive" : "new, sensitive") + " knot that you manage " +
					"to blow your load and enjoy the last few spasms of pleasure as it finally finishes changing.";
			}
			else
			{
				cumText = ", but you aren't really feeling much from it right now. Disappointed, you rein in your hands, tucking them into your armpits as you let " +
					"the arousing changes run their course.";
			}

			return "Your " + previousCockData.LongDescription() + " tingles as pins and needles sweep across it. " + armorText + " to watch as it changes. The tip elongates and tapers, " +
				"like a spear. A series of ridges form along the shaft, giving it an almost segmented look, and a prominent knot swells at its base. You can't resist stroking it, " +
				"and it soon begins dripping pre" + cumText + SafelyFormattedString.FormattedText("You now have a dragon penis.", StringFormats.BOLD);
		}
		private static string DragonRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string DragonRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string DisplacerGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string DisplacerCockHeadDesc()
		{
			return Utils.RandomChoice("star tip", "blooming cock-head", "open crown", "alien tip", "bizarre head");
		}
		private static string DisplacerDesc(bool noAdjective, bool plural = false)
		{
			string adj = noAdjective ? "" : Utils.RandomChoice("tentacle-tipped ", "starfish-tipped ", "bizarre ", "beastly ", "cthulhu-tier ", "star-capped ", "knotted ");
			return adj + (Utils.Rand(3) >= 1 || noAdjective ? Utils.RandomChoice("coerl ", "alien ", "almost-canine ", "animal ", "displacer ") : "") + GenericCockNoun(plural);
		}

		private static string DisplacerSingleDesc(bool noAdjective)
		{
			if (!noAdjective)
			{
				return "a " + Utils.RandomChoice("tentacle-tipped ", "starfish-tipped ", "bizarre ", "beastly ", "cthulhu-tier ", "star-capped ", "knotted ") +
					(Utils.Rand(3) >= 1 ? Utils.RandomChoice("coerl ", "alien ", "almost-canine ", "animal ", "displacer-") : "") + GenericCockNoun(false);
			}
			else
			{
				return Utils.RandomChoice("a coerl ", "an alien ", "an almost-canine ", "an animal ", "a displacer-") + GenericCockNoun(false);
			}
		}

		private static string DisplacerLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string DisplacerPlayerStr(Cock cock, PlayerBase player)
		{
			return "Like a dog's cock, yours is shiny, pointed, and covered in veins, but the tip has 5 grooves along its sides, and is wider than a canine's. " +
				"You can split your cock-head along these grooves into something that resembles a 5-point starfish, granting you full control over how and where you deliver your load.";
		}
		//again, not possible in vanilla. idk.
		private static string DisplacerTransformStr(CockData previousCockData, PlayerBase player)
		{

			if (previousCockData.type == CockType.DOG)
			{
				return "Your " + previousCockData.LongDescription() + " swells up with near-painful arousal, but otherwise seems to remain unchanged, suddenly, your " +
					previousCockData.HeadDescription() + " splits, folding outward into five distinct sections. You freak out and look away, hoping it was just a trick of your eyes. " +
					"It wasn't. Then, just as suddenly as it split, the five prongs fold back inward, reforming into a canine-like head. You give it a tentative poke, and the tip" +
					"unfurls again, each prong now tipped with wiggling tendrils. " + SafelyFormattedString.FormattedText("You now have a strange, coeurl cock!", StringFormats.BOLD);
			}
			else
			{
				string knotText = previousCockData.hasKnot ? "while the know reshapes itself into something more canine. " : " while a gradually widening bulge forms around the base. ";
				string sheathText = previousCockData.currentlyHasSheath ? "" : "Where it meets your crotch, the skin bunches up around it, forming a canine -like sheath. ";

				return "Your " + previousCockData.LongDescription() + " swells up with near-painful arousal and begins to transform. The head narrows into a pointed tip " +
					knotText + sheathText + "All thoughts that it's becoming a canine-cock are immediately put to rest when your crown splits into five prongs, each tipped with a small," +
					" wiggling tendril. Unsure how to react to your new, starfish-shaped cock-head, you look away, hoping that it was just a trick of the mind. It wasn't. As the " +
					"transformation completes, the prongs suddenly fold back inward, reforming into something that appears canine. You give it a tentative poke, and the tip" +
					"unfurls again, each prong now tipped with wiggling tendrils. It appears " + SafelyFormattedString.FormattedText("You now have a coeurl cock!", StringFormats.BOLD) +
					" After a few seconds, you figure out how to control your new cock-head, confident it will provide new, \"interesting\" sexual experiences.";
			}
		}
		private static string DisplacerRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string DisplacerRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		//private static string FoxGrewCockStr(PlayerBase player, byte grownCockIndex)
		//{
		//	return GenericGrewCockText(player, grownCockIndex);
		//}
		//private static string FoxCockHeadDesc()
		//{
		//	return Utils.RandomChoice("pointed tip", "narrow tip");
		//}
		//private static string FoxDesc(bool noAdjective, bool plural = false)
		//{
		//	string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("pointed ", "knotty ", "knotted", "bestial ", "animalistic ") : "";
		//	return adj + Utils.RandomChoice("fox-", "fox-shaped ", "vulpine ", "vixen-") + GenericCockNoun(plural);
		//}
		//private static string FoxLongDesc(CockData cock, bool alternateFormat)
		//{
		//	return GenericLongDesc(cock, alternateFormat);
		//}
		//private static string FoxPlayerStr(Cock cock, PlayerBase player)
		//{
		//	return " It is shiny, pointed, and covered in veins, just like a large fox's cock." + GenericPlayerPostText(cock, player);
		//}
		//private static string FoxTransformStr(CockData previousCockData, PlayerBase player)
		//{

		//}
		//private static string FoxRestoreStr(CockData originalCockData, PlayerBase player)
		//{
		//	throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		//}
		private static string BeeGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string BeeCockHeadDesc()
		{
			return Utils.RandomChoice("narrow tip", "flat point");
		}
		private static string BeeDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("bee ", "insectoid ", "furred ") + GenericCockNoun(plural);
		}

		private static string BeeSingleDesc(bool noAdjective)
		{
			return Utils.RandomChoice("a bee ", "an insectoid ", "a furred ") + GenericCockNoun(false);
		}

		private static string BeeLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string BeePlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a long, smooth black shaft that's rigid to the touch. Its base is ringed with a layer of " + Measurement.ToNearestQuarterInchOrMillimeter(4, true, false) +
				" long soft bee hair. The tip has a much finer layer of short yellow hairs. The tip is very sensitive, and it hurts constantly if you don't have bee honey on it." + GenericPlayerPostText(cock, player);
		}
		private static string BeeTransformStr(CockData previousCockData, PlayerBase player)
		{
			string size = previousCockData.length > 18 ? "huge " : "";

			string armorText;
			if (player.wearingArmor)
			{
				armorText = "You tear off your " + player.armor.ItemName() + (player.wearingLowerGarment ? "and " + player.lowerGarment.ItemName() : "");
			}
			else if (player.wearingLowerGarment)
			{
				armorText = "You tear off your " + player.lowerGarment.ItemName();
			}
			else
			{
				armorText = "You look down";
			}

			return "Your " + size + "member suddenly starts to hurt, especially the tip of the thing. At the same time, you feel your length start to get incredibly sensitive " +
				"and the base of your shaft starts to itch. " + armorText + " and watch in fascination as your " + previousCockData.LongDescription() + " starts to change. " +
				"The shaft turns black, while becoming hard and smooth to the touch, while the base develops a mane of " + Measurement.ToNearestSmallUnit(4, false, true, false) +
				" long yellow bee hair. As the transformation continues, your member grows even larger than before. However, it is the tip that keeps your attention the most, " +
				"as a much finer layer of short yellow hairs grow around it. Its appearance isn't the thing that you care about right now, it is the pain that is filling it - " +
				"it's entirely different from the usual feeling you get when your cock is transformed. " + GlobalStrings.NewParagraph() + "When the changes stop, the tip is shaped " +
				"like a typical human mushroom cap, but is covered in fine bee hair and feels nothing like what you'd expect a human dick to feel like. Your whole length is " +
				"incredibly sensitive, and touching it gives you incredible stimulation, but you're sure that no matter how much you rub it, you aren't going to cum by yourself. " +
				SafelyFormattedString.FormattedText("You now have a bee cock!", StringFormats.BOLD);
		}
		private static string BeeRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string BeeRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string PigGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string PigCockHeadDesc()
		{
			return Utils.RandomChoice("corkscrew tip", "corkscrew head");
		}
		private static string PigDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("pig ", "swine ", "pig-like ", "corkscrew-tipped ", "hoggish ", "pink pig-", "pink ") + GenericCockNoun(plural);
		}

		private static string PigSingleDesc(bool noAdjective)
		{
			return Utils.RandomChoice("a pig-", "a swine-", "a pig-like ", "a corkscrew-tipped ", "a hoggish ", "a pink pig-", "a pink ") + GenericCockNoun(false);
		}

		private static string PigLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string PigPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's bright pinkish red, ending in a prominent corkscrew shape at the tip." + GenericPlayerPostText(cock, player);
		}
		private static string PigTransformStr(CockData previousCockData, PlayerBase player)
		{
			string cockText = player.cocks.Count == 1 ? "your " + previousCockData.LongDescription() : "one of your cocks";
			string armorText = player.wearingArmor && player.wearingLowerGarment
				? "You pull off your clothing"
				: (player.wearingArmor ? "You pull open your armor" : "You look down at your exposed groin");

			return "You feel an uncomfortable pinching sensation in " + cockText + armorText + ", watching as it warps and changes. As the transformation completes, " +
				"you're left with a shiny, pinkish red pecker ending in a prominent corkscrew at the tip. " +
				SafelyFormattedString.FormattedText("You now have a pig penis!", StringFormats.BOLD);
		}
		private static string PigRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string PigRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string AvianGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string AvianCockHeadDesc()
		{
			return Utils.RandomChoice("tapered head", "tip");
		}
		private static string AvianDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("bird ", "avian ", "tapered ") + GenericCockNoun(plural);
		}

		private static string AvianSingleDesc(bool noAdjective)
		{
			return Utils.RandomChoice("a bird ", "an avian ", "a tapered ") + GenericCockNoun(false);
		}

		private static string AvianLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string AvianPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a red, tapered cock that ends in a tip. It rests nicely in a sheath." + GenericPlayerPostText(cock, player);
		}
		private static string AvianTransformStr(CockData previousCockData, PlayerBase player)
		{
			string armorText = player.ClothingOrNakedTextHelper("open up your " + player.LowerBodyArmorShort(), "look down");

			string sheathText = previousCockData.currentlyHasSheath ? "its sheath. " : "a newly-formed sheath. ";

			return "You feel a strange tingling sensation in your " + previousCockData.LongDescription() + " as an erection forms. You " + armorText + " and see " +
				"it shifting! By the time the transformation's complete, you notice it's tapered, red, and ends in a tip. When you're not aroused, " +
				"your cock rests nicely in " + sheathText + SafelyFormattedString.FormattedText("You now have an avian penis!", StringFormats.BOLD);
		}
		private static string AvianRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string AvianRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string RhinoGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string RhinoCockHeadDesc()
		{
			return Utils.RandomChoice("flared head", "rhinoceros dickhead");
		}
		private static string RhinoDesc(bool noAdjective, bool plural = false)
		{
			if (noAdjective)
			{
				return Utils.RandomChoice("oblong ", "rhino ") + GenericCockNoun(plural);
			}
			else
			{
				return Utils.RandomChoice("oblong ", "rhino ", "bulged rhino ") + GenericCockNoun(plural);
			}
		}

		private static string RhinoSingleDesc(bool noAdjective)
		{
			if (noAdjective)
			{
				return Utils.RandomChoice("an oblong ", "a rhino ") + GenericCockNoun(false);
			}
			else
			{
				return Utils.RandomChoice("an oblong ", "a rhino ", "a bulged rhino ") + GenericCockNoun(false);
			}
		}

		private static string RhinoLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string RhinoPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a smooth, tough pink colored and takes on a long and narrow shape with an oval shaped bulge along the center." + GenericPlayerPostText(cock, player);
		}
		private static string RhinoTransformStr(CockData previousCockData, PlayerBase player)
		{
			string cockText = player.genitals.OneCockOrCocksNoun() + Utils.PluralizeIf("grow", player.cocks.Count == 1);
			string sheathText = previousCockData.currentlyHasSheath
				? "You feel a tightness near the base where your skin seems to be bunching up. A sheath begins forming around your flared rhino cock's, tightening around your stiff member as it " +
					"enlongates."
				: "Your sheath tightens around your member as it enlarges. ";

			return "You feel a stirring in your loins as " + cockText + " rock hard. You " +
				player.LowerBodyArmorTextHelper("pull it out from under your " + player.armor.ItemName(), "pull it out of your " + player.lowerGarment.ItemName(), "lean over") +
				" to take a look. You watch as the " + previousCockData.LongDescription() + "shifts, the skin becoming smooth, tough and pink. It takes on a long and narrow shape " +
				"with an oval shaped bulge along the center. " + sheathText + "The transformation finishes, and the thick flared head leaks a steady stream of funky animal-cum. " +
				SafelyFormattedString.FormattedText("You now have a rhino-dick!", StringFormats.BOLD);
		}
		private static string RhinoRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string RhinoRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string EchidnaGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string EchidnaCockHeadDesc()
		{
			return Utils.RandomChoice("quad heads", "echidna quad heads");
		}
		private static string EchidnaDesc(bool noAdjective, bool plural = false)
		{
			string adj = (Utils.RandBool() && !noAdjective ? Utils.RandomChoice("strange ", "four-headed ", "exotic ", "unusual ") : "");
			return adj + (Utils.Rand(4) >= 1 || noAdjective ? "echidna " : "") + GenericCockNoun(plural);
		}

		private static string EchidnaSingleDesc(bool noAdjective)
		{
			string adj = (Utils.RandBool() && !noAdjective ? Utils.RandomChoice("a strange ", "a four-headed ", "an exotic ", "an unusual ") : "an ");
			return adj + (Utils.Rand(4) >= 1 || noAdjective ? "echidna " : "") + GenericCockNoun(false);
		}

		private static string EchidnaLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string EchidnaPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is quite a sight to behold, coming well-equipped with four heads." + GenericPlayerPostText(cock, player);
		}
		private static string EchidnaTransformStr(CockData previousCockData, PlayerBase player)
		{
			return player.genitals.OneCockOrCocksNoun().CapitalizeFirstLetter() + " suddenly becomes rock hard out of nowhere. You " +
				player.LowerBodyArmorTextHelper("pull it out from under your " + player.armor.ItemName() + " and watch", "pull it out of your " + player.lowerGarment.ItemName() +
				" and watch", "watch") + " as the " + previousCockData.LongDescription() + " begins to shift and change. It becomes pink in color, and you feel a pinch at the head " +
				"as it splits to become four heads. " + (previousCockData.currentlyHasSheath ? "" : "The transformation finishes off with a fleshy sheath forming at the base.") +
				" It ejaculates before going limp, retreating into your sheath. " + SafelyFormattedString.FormattedText("You now have an echidna penis!", StringFormats.BOLD);
		}
		private static string EchidnaRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string EchidnaRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}
		private static string WolfGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			string armorText = player.wearingArmor ? ", and you take off your " + player.armor.ItemName() + " just in time to watch" : "and you watch in disbelief as";
			return "You double over as a pain fills your groin " + armorText + " a bulge push out of your body. The skin folds back and bunches up into a sheath, " +
				"revealing a red, knotted wolf cock drooling pre-cum underneath it. You take a shuddering breath as the pain dies down, " +
				"leaving you with only a vague sense of lust and quiet admiration for your new endowment. " +
				SafelyFormattedString.FormattedText("You now have a wolf cock", StringFormats.BOLD) + ".";
		}
		private static string WolfCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string WolfDesc(bool noAdjective, bool plural = false)
		{
			if (noAdjective || Utils.Rand(3) == 0)
			{
				return Utils.RandomChoice("wolf-shaped ", "wolf-", "wolf-", "wolf-", "canine ") + GenericCockNoun(plural);
			}
			else
			{
				return Utils.RandomChoice("knotted ", "knotty ", "animalistic ", "pointed ", "bestial ") +
					Utils.RandomChoice("wolf-shaped ", "wolf-", "wolf-", "wolf-", "canine ", "", "") + GenericCockNoun(plural);
			}

		}

		private static string WolfSingleDesc(bool noAdjective)
		{
			if (noAdjective || Utils.Rand(3) == 0)
			{
				return Utils.RandomChoice("a wolf-shaped ", "a wolf-", "a wolf-", "a wolf-", "a canine ") + GenericCockNoun(false);
			}
			else
			{
				return Utils.RandomChoice("a knotted ", "a knotty ", "an animalistic ", "a pointed ", "a bestial ") +
					Utils.RandomChoice("wolf-shaped ", "wolf-", "wolf-", "wolf-", "canine ", "", "") + GenericCockNoun(false);
			}
		}

		private static string WolfLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string WolfPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is shiny red, pointed, and covered in veins, just like a large wolf's cock." + GenericPlayerPostText(cock, player);
		}
		private static string WolfTransformStr(CockData previousCockData, PlayerBase player)
		{
			return "Your " + previousCockData.LongDescription() + " trembles, resizing and reshaping itself into a shining, red wolf cock with a fat knot at the base. "
				+ SafelyFormattedString.FormattedText("You now have a wolf cock!", StringFormats.BOLD);
		}
		private static string WolfRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string WolfRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}

		//taken from fox/dog/wolf. feel free to correct; idk what it actually looks like.
		private static string RedPandaGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string RedPandaCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string RedPandaDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("animalistic ", "oddly-textured ") : "";
			return adj + Utils.RandomChoice("panda-", "panda-like", "red panda ") + GenericCockNoun(plural);
		}

		private static string RedPandaSingleDesc(bool noAdjective)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("an animalistic ", "an oddly-textured ") : "a ";
			return adj + Utils.RandomChoice("panda-", "panda-like", "red panda ") + GenericCockNoun(false);
		}

		private static string RedPandaLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string RedPandaPlayerStr(Cock cock, PlayerBase player)
		{
			return " It lies protected in a soft, fuzzy sheath." + GenericPlayerPostText(cock, player);
		}
		//not really explained in vanilla, this is just what i could do with it.
		private static string RedPandaTransformStr(CockData previousCockData, PlayerBase player)
		{
			string sheathText;
			if (!previousCockData.currentlyHasSheath)
			{
				sheathText = "The skin surrounding your penis folds, encapsulating it and turning itself into a protective sheath. It continues to shift, ";
			}
			else
			{
				sheathText = "It begins shifting, ";
			}

			return "Your cock feels strange, then suddenly starts to transform." + sheathText + " reforming until " +
				SafelyFormattedString.FormattedText("You now have a ferret cock!", StringFormats.BOLD);
		}
		private static string RedPandaRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string RedPandaRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}

		//taken from fox/dog/wolf. feel free to correct; idk what it actually looks like.
		private static string FerretGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string FerretCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string FerretDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("animalistic ", "oddly-textured ") : "";
			return adj + Utils.RandomChoice("ferret-", "ferret-like") + GenericCockNoun(plural);
		}

		private static string FerretSingleDesc(bool noAdjective)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("an animalistic ", "an oddly-textured ") : "a ";
			return adj + Utils.RandomChoice("ferret-", "ferret-like") + GenericCockNoun(false);
		}

		private static string FerretLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string FerretPlayerStr(Cock cock, PlayerBase player)
		{
			//idk what a ferret cock looks like, and i don't really want to google it. So i just stole red panda's text. feel free to change this.
			return " It lies protected in a soft, fuzzy sheath." + GenericPlayerPostText(cock, player);
		}
		//see comment with red panda.
		private static string FerretTransformStr(CockData previousCockData, PlayerBase player)
		{
			string sheathText;
			if (!previousCockData.currentlyHasSheath)
			{
				sheathText = "The skin surrounding your penis folds, encapsulating it and turning itself into a protective sheath. It continues to shift, ";
			}
			else
			{
				sheathText = "It begins shifting, ";
			}

			return "Your cock feels strange, then suddenly starts to transform." + sheathText + " reforming until " +
				SafelyFormattedString.FormattedText("You now have a ferret cock!", StringFormats.BOLD);
		}
		private static string FerretRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string FerretRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}

		private static string GooGrewCockStr(PlayerBase player, byte grownCockIndex)
		{
			return GenericGrewCockText(player, grownCockIndex);
		}
		private static string GooCockHeadDesc()
		{
			return Utils.RandomChoice("gooey crown", "soft dickhead", "elastic head");
		}
		private static string GooDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? "semi-transparent " : "";
			return adj + Utils.RandomChoice("gooey ", "goo-", "gelatenous") + GenericCockNoun(plural);
		}

		private static string GooSingleDesc(bool noAdjective)
		{
			string adj = Utils.RandBool() && !noAdjective ? "semi-transparent " : "";
			return "a " + adj + Utils.RandomChoice("gooey ", "goo-", "gelatenous") + GenericCockNoun(false);
		}

		private static string GooLongDesc(CockData cock, bool alternateFormat)
		{
			return GenericLongDesc(cock, alternateFormat);
		}
		private static string GooPlayerStr(Cock cock, PlayerBase player)
		{
			return " It appears roughly humanoid, though entirely made of a semi-transparent goo. Surprisingly, it's just as sturdy as any other cock, and the goo is a natural lubricant. " +
				"Additionally, you can morph it to take various forms in order to better suit your needs - it can fit into even the tightest of orifices, and no orifice is truly out of reach."
				+ GenericPlayerPostText(cock, player);
		}
		private static string GooTransformStr(CockData previousCockData, PlayerBase player)
		{
			string multiCockText = player.cocks.Count > 1 ? ", and in stark contrast to the rest of your members" : "";

			string corruptText;

			if (player.corruption > 80)
			{
				corruptText = "You'll have little trouble fitting into any orifice you choose, you note with a smirk.";
			}
			else if (player.corruption > 30)
			{
				corruptText = "You can think of more than a few ways this will come in handy.";
			}
			else
			{
				corruptText = "You suppose this means you won't have to worry about being too big, at least.";
			}


			return "Your " + previousCockData.LongDescription() + " feels strange, and a wave of lust passes through you. " +
				player.ClothingOrNakedTextHelper("Pulling down your " + player.LowerBodyArmorShort(), "Looking down") + ", you notice your " + previousCockData.ShortDescription() +
				"is soft, despite the sudden on rush of lust" + multiCockText + ". Your go to stroke it, curious to see if it will respond to a more direct stimulation. As you grip your " +
				previousCockData.ShortDescription() + ", it suddenly deforms in your " + player.hands.ShortDescription(false) + "! It continues losing its shape until it provides your " +
				player.hands.ShortDescription(false) + "almost no resistance. Stangely, it's not painful - you can still feel your " + player.hands.ShortDescription(false) +
				" rubbing your length. Before you have time to contemplate that, it changes again, shifting to a blue-ish hue and becoming increasingly transparent. It suddenly becomes " +
				" rigid, and you watch, dumbfounded, as cum travels up its length, then out the flexible tip and onto the ground below you. As you go to remove your hand, your cock " +
				"stretches and remains stuck to it. With a start, you realize " + SafelyFormattedString.FormattedText("you now have a goo-cock!", StringFormats.BOLD) + " After a moment," +
				" you gain control of your now gooey member, allowing you to reshape it as you see fit. " + corruptText;
		}
		private static string GooRestoreStr(CockData originalCockData, PlayerBase player)
		{
			return GenericRestoreCockText(originalCockData, player);
		}
		private static string GooRemoveCockStr(CockData removedCock, PlayerBase player)
		{
			return GenericRemovedCockText(removedCock, player);
		}

		//i should handle articles manually, but i really don't want to.
		private static string GenericLongDesc(CockData cock, bool alternateFormat)
		{
			string text = CockAdjectiveText(cock, false) + cock.ShortDescription();
			if (alternateFormat)
			{
				return Utils.AddArticle(text);
			}
			else return text;
		}

		protected string GenericFullDescription(CockData cock, bool alternateFormat = false)
		{
			string text = CockAdjectiveText(cock, true) + cock.ShortDescription();
			if (alternateFormat)
			{
				return Utils.AddArticle(text);
			}
			else return text;
		}

		private static string GenericPlayerPostText(Cock cock, PlayerBase player)
		{
			return GenericKnotText(cock, player) + GenericCockSockText(cock, player);
		}

		private static string GenericKnotText(Cock cock, PlayerBase player)
		{
			if (cock.hasKnot)
			{
				string measure = " The knot is " + Measurement.ToNearestHalfSmallUnit(cock.knotSize, false, true) + " thick when at full size.";

				if (cock.relativeKnotSize >= 1.8)
				{
					return " The obscenely swollen lump of flesh near the base of your " + cock.LongDescription() + " looks almost comically mismatched for your cock." + measure;
				}
				else if (cock.relativeKnotSize >= 1.4)
				{
					return " A large bulge of flesh nestles just above the bottom of your " + cock.LongDescription() + ", to ensure it stays where it belongs during mating." + measure;
				}
				else // knotMultiplier < 1.4
				{
					return " A small knot of thicker flesh is near the base of your " + cock.LongDescription() + ", ready to expand to help you lodge it inside a female." + measure;
				}
			}

			return "";
		}

		private static string GenericCockSockText(Cock cock, PlayerBase player)
		{
			if (cock.cockSock != null)
			{
				return cock.cockSock.PlayerText(player, cock.AsReadOnlyData());
			}

			return "";
		}

		private static string GenericGrewCockText(PlayerBase player, byte grownCockIndex)
		{
			Cock grownCock = player.cocks[grownCockIndex];

			int previousCockCount = player.cocks.Count - 1;
			int previousTypeCockCount = player.genitals.CountCocksOfType(grownCock.type) - 1;

			string multiCockText, hermText, typeCountText;
			typeCountText = previousTypeCockCount > 0 && previousCockCount > 0 ? "another " : "a ";
			hermText = previousCockCount <= 0 ? " situated above your " + player.genitals.AllVaginasShortDescription() + ", making you a herm!" : "!";
			multiCockText = previousCockCount > 0 ? ", next to your " + player.genitals.AllCocksShortDescription() : "";


			return "You shudder as a pressure builds in your crotch, peaking painfully as a large bulge begins to push out from your body" + multiCockText +
				". The skin seems to fold back as a fully formed " + grownCock.type.ShortDescriptionNoAdjective() + " bursts forth from your loins, " +
				"drizzling hot cum everywhere as it orgasms. " +
				SafelyFormattedString.FormattedText("You now have " + typeCountText + grownCock.type.ShortDescriptionNoAdjective() + hermText, StringFormats.BOLD);
		}

		private static string GenericRestoreCockText(CockData previousCock, PlayerBase player)
		{
			string armorText = player.wearingArmor
				? "You resist the urge to undo your " + player.armor.ItemName() + " to check, but by the feel of it, your penis is shifting form."
				: "A quick look down confirms what you suspected: your penis is shifting form.";

			return "A strange tingling begins behind your " + previousCock.LongDescription() + ", slowly crawling up across its entire length. While neither particularly " +
				"arousing nor uncomfortable, you do shift nervously as the feeling intensifies. " + armorText + " Eventually the transformative sensation fades, " +
				SafelyFormattedString.FormattedText("leaving you with a completely human penis.", StringFormats.BOLD);
		}

		//generic version of removing one cock. it has no flavor text or unique interactions based on your current cock type. but then again, no such thing existed before, either.
		//feel free to implement your own flavor text for different types, based on this format.
		private static string GenericRemovedCockText(CockData removedCock, PlayerBase player)
		{
			int numCocks = player.cocks.Count;
			if (numCocks == 0)
			{
				return SafelyFormattedString.FormattedText("Your manhood shrinks into your body, disappearing completely.", StringFormats.BOLD);
			}
			else if (numCocks == 1)
			{
				return SafelyFormattedString.FormattedText("Your " + removedCock.LongDescription() + " disappears, shrinking into your body and leaving you with just one " +
					player.cocks[0].ShortDescription(), StringFormats.BOLD);
			}
			else
			{
				return SafelyFormattedString.FormattedText("Your " + removedCock.LongDescription() + "disappears forever, leaving you with just your " +
					player.genitals.AllCocksShortDescription(), StringFormats.BOLD);
			}
		}
	}
}
