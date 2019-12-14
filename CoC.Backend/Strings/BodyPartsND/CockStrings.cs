//CockNBallzStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 4:08 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Cock
	{
		public static string Name()
		{
			return "Cock";
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

		private static string HumanLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
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
		private static string HumanTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string HorseLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string HorsePlayerStr(Cock cock, PlayerBase player)
		{
			return " It's mottled black and brown in a very animalistic pattern. The 'head' of its shaft flares proudly, just like a horse's." + GenericPlayerPostText(cock, player);
		}
		private static string HorseTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string DogLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string DogPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is shiny, pointed, and covered in veins, just like a large dog's cock." + GenericPlayerPostText(cock, player);
		}
		private static string DogTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonCockHeadDesc()
		{
			return Utils.RandomChoice("tainted crown", "nub-ringed tip");
		}
		private static string DemonDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandomChoice("corrupted ", "nub-covered ", "nubby ", "perverse ", "bumpy ", "cursed ", "infernal ", "unholy ", "blighted ");
			return adj + (Utils.RandBool() || noAdjective ? Utils.RandomChoice("demon-", "demonic ") : "") + GenericCockNoun(plural);
		}
		private static string DemonLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string DemonPlayerStr(Cock cock, PlayerBase player)
		{
			return " The crown is ringed with a circle of rubbery protrusions that grow larger as you get more aroused. The entire thing is shiny and covered with tiny, " +
				"sensitive nodules that leave no doubt about its demonic origins." + GenericPlayerPostText(cock, player);
		}
		private static string DemonTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string TentacleLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string TentaclePlayerStr(Cock cock, PlayerBase player)
		{
			return " With a thought, you can make it even longer, though you don't feel anything along this additional length. The entirety of its green surface is covered in " +
				"perspiring beads of slick moisture. It frequently shifts and moves of its own volition, the slightly oversized and mushroom-like head shifting in coloration " +
				"to purplish-red whenever you become aroused." + GenericPlayerPostText(cock, player);
		}
		private static string TentacleTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TentacleRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string CatLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string CatPlayerStr(Cock cock, PlayerBase player)
		{
			return " It ends in a single point, much like a spike, and is covered in small, fleshy barbs. The barbs are larger at the base " +
				"and shrink in size as they get closer to the tip. Each of the spines is soft and flexible, and shouldn't be painful for any of your partners." +
				GenericPlayerPostText(cock, player);
		}
		private static string CatTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string LizardLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string LizardPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a deep, iridescent purple in color. Unlike a human penis, the shaft is not smooth, and is instead patterned with multiple bulbous bumps." + GenericPlayerPostText(cock, player);
		}
		private static string LizardTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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

		private static string AnemoneLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string AnemonePlayerStr(Cock cock, PlayerBase player)
		{
			return " The crown is surrounded by tiny tentacles with a venomous, aphrodisiac payload. At its base a number of similar, longer tentacles have formed, " +
				"guaranteeing that pleasure will be forced upon your partners." + GenericPlayerPostText(cock, player);
		}
		private static string AnemoneTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string KangarooLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string KangarooPlayerStr(Cock cock, PlayerBase player)
		{
			return " It usually lies coiled inside a sheath, but undulates gently and tapers to a point when erect, somewhat like a taproot." + GenericPlayerPostText(cock, player);
		}
		private static string KangarooTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string DragonLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string DragonPlayerStr(Cock cock, PlayerBase player)
		{
			return " With its tapered tip, there are few holes you wouldn't be able to get into. It has a strange, knot-like bulb at its base, " +
				"but doesn't usually flare during arousal as a dog's knot would. The knot is " + Measurement.ToNearestHalfSmallUnit(cock.knotSize, false, true) +
				" thick when at full size." + GenericCockSockText(cock, player);
		}
		private static string DragonTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string DisplacerLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string DisplacerPlayerStr(Cock cock, PlayerBase player)
		{
			return "Like a dog's cock, yours is shiny, pointed, and covered in veins, but the tip has 5 grooves along its sides, and is wider than a canine's. " +
				"You can split your cock-head along these grooves into something that resembles a 5-point starfish, granting you full control over how and where you deliver your load.";
		}
		private static string DisplacerTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DisplacerRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string FoxDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.RandBool() && !noAdjective ? Utils.RandomChoice("pointed ", "knotty ", "knotted", "bestial ", "animalistic ") : "";
			return adj + Utils.RandomChoice("fox-", "fox-shaped ", "vulpine ", "vixen-") + GenericCockNoun(plural);
		}
		private static string FoxLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string FoxPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is shiny, pointed, and covered in veins, just like a large fox's cock." + GenericPlayerPostText(cock, player);
		}
		private static string FoxTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeCockHeadDesc()
		{
			return Utils.RandomChoice("narrow tip", "flat point");
		}
		private static string BeeDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("bee ", "insectoid ", "furred ") + GenericCockNoun(plural);
		}
		private static string BeeLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string BeePlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a long, smooth black shaft that's rigid to the touch. Its base is ringed with a layer of " + Measurement.ToNearestQuarterInchOrMillimeter(4, true, false) +
				" long soft bee hair. The tip has a much finer layer of short yellow hairs. The tip is very sensitive, and it hurts constantly if you don't have bee honey on it." + GenericPlayerPostText(cock, player);
		}
		private static string BeeTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigCockHeadDesc()
		{
			return Utils.RandomChoice("corkscrew tip", "corkscrew head");
		}
		private static string PigDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("pig ", "swine ", "pig-like ", "corkscrew-tipped ", "hoggish ", "pink pig-", "pink ") + GenericCockNoun(plural);
		}
		private static string PigLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string PigPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's bright pinkish red, ending in a prominent corkscrew shape at the tip." + GenericPlayerPostText(cock, player);
		}
		private static string PigTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AvianCockHeadDesc()
		{
			return Utils.RandomChoice("tapered head", "tip");
		}
		private static string AvianDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("bird ", "avian ", "tapered ") + GenericCockNoun(plural);
		}
		private static string AvianLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string AvianPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a red, tapered cock that ends in a tip. It rests nicely in a sheath." + GenericPlayerPostText(cock, player);
		}
		private static string AvianTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AvianRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoCockHeadDesc()
		{
			return Utils.RandomChoice("flared head", "rhinoceros dickhead");
		}
		private static string RhinoDesc(bool noAdjective, bool plural = false)
		{
			return Utils.RandomChoice("oblong ", "rhino ", "bulged rhino ") + GenericCockNoun(plural);
		}
		private static string RhinoLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string RhinoPlayerStr(Cock cock, PlayerBase player)
		{
			return " It's a smooth, tough pink colored and takes on a long and narrow shape with an oval shaped bulge along the center." + GenericPlayerPostText(cock, player);
		}
		private static string RhinoTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string EchidnaLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string EchidnaPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is quite a sight to behold, coming well-equipped with four heads." + GenericPlayerPostText(cock, player);
		}
		private static string EchidnaTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string WolfDesc(bool noAdjective, bool plural = false)
		{
			string adj = (Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("knotted ", "knotty ", "animalistic ", "pointed ", "bestial ") : "");
			return adj + Utils.RandomChoice("wolf-shaped ", "wolf-", "wolf-", "wolf-", "canine ", "", "") + GenericCockNoun(plural);
		}
		private static string WolfLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string WolfPlayerStr(Cock cock, PlayerBase player)
		{
			return " It is shiny red, pointed, and covered in veins, just like a large wolf's cock." + GenericPlayerPostText(cock, player);
		}
		private static string WolfTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//taken from fox/dog/wolf. feel free to correct; idk what it actually looks like.
		private static string RedPandaCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string RedPandaDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("animalistic ", "oddly-textured ") : "";
			return adj + Utils.RandomChoice("panda-", "panda-like", "red panda ") + GenericCockNoun(plural);
		}
		private static string RedPandaLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string RedPandaPlayerStr(Cock cock, PlayerBase player)
		{
			return " It lies protected in a soft, fuzzy sheath." + GenericPlayerPostText(cock, player);
		}
		private static string RedPandaTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//taken from fox/dog/wolf. feel free to correct; idk what it actually looks like.
		private static string FerretCockHeadDesc()
		{
			return Utils.RandomChoice("pointed tip", "narrow tip");
		}
		private static string FerretDesc(bool noAdjective, bool plural = false)
		{
			string adj = Utils.Rand(3) >= 1 && !noAdjective ? Utils.RandomChoice("animalistic ", "oddly-textured ") : "";
			return adj + Utils.RandomChoice("ferret-", "ferret-like") + GenericCockNoun(plural);
		}
		private static string FerretLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string FerretPlayerStr(Cock cock, PlayerBase player)
		{
			//idk what a ferret cock looks like, and i don't really want to google it. So i just stole red panda's text. feel free to change this.
			return " It lies protected in a soft, fuzzy sheath." + GenericPlayerPostText(cock, player);
		}
		private static string FerretTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string GooLongDesc(CockData cock, bool alternateForm)
		{
			return GenericLongDesc(cock, alternateForm);
		}
		private static string GooPlayerStr(Cock cock, PlayerBase player)
		{
			return " It appears roughly humanoid, though entirely made of a semi-transparent goo. Surprisingly, it's just as sturdy as any other cock, and the goo is a natural lubricant. " +
				"Additionally, you can morph it to take various forms in order to better suit your needs - it can fit into even the tightest of orifices, and no orifice is truly out of reach."
				+ GenericPlayerPostText(cock, player);
		}
		private static string GooTransformStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(CockData cock, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//i should handle articles manually, but i really don't want to.
		private static string GenericLongDesc(CockData cock, bool alternateForm)
		{
			string text = CockAdjectiveText(cock, false) + cock.ShortDescription();
			if (alternateForm)
			{
				return Utils.AddArticle(text);
			}
			else return text;
		}

		protected string GenericFullDescription(CockData cock, bool alternateForm = false)
		{
			string text = CockAdjectiveText(cock, true) + cock.ShortDescription();
			if (alternateForm)
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


	}
}
