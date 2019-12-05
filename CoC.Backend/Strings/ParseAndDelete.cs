//./classes/Appearance.as
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParseAndDelete
{
	class DeleteMe
	{
		//./classes/Appearance.as
		//NOT IMPLEMENTED
		/*
		public static string beardDescription(Creature creature)
		{
			string description = "";
			string[] options;
			// LENGTH ADJECTIVE!
			if (creature.beard.length == 0)
			{
				options = new string[]
				{
					"shaved",
					"bald",
					"smooth",
					"hairless",
					"glabrous"
				};
				description = Utils.RandomChoice(options) + " chin and cheeks";
				return description;
			}
			if (creature.beard.length < 0.2)
			{
				options = new string[]
				{
					"close-cropped, ",
					"trim, ",
					"very short, "
				};
				description += Utils.RandomChoice(options);
			}
			if (creature.beard.length >= 0.2 && creature.beard.length < 0.5) description += "short, ";
			if (creature.beard.length >= 0.5 && creature.beard.length < 1.5) description += "medium, ";
			if (creature.beard.length >= 1.5 && creature.beard.length < 3) description += "moderately long, ";
			if (creature.beard.length >= 3 && creature.beard.length < 6)
			{
				if (Utils.Rand(2) == 0) description += "long, ";
				else description += "neck-length, ";
			}
			if (creature.beard.length >= 6)
			{
				if (Utils.Rand(2) == 0) description += "very long, ";
				description += "chest-length, ";
			}

			// COLORS
			//
			description += creature.hair.color + " ";
			//
			// BEARD WORDS
			// Follows hair type.
			if (creature.hair.type == 1) description += "";
			else if (creature.hair.type == 2) description += "transparent ";
			else if (creature.hair.type == 3) description += "gooey ";
			else if (creature.hair.type == 4) description += "tentacley ";

			if (creature.beard.style == 0) description += "beard"
			else if (creature.beard.style == 1) description += "goatee"
			else if (creature.beard.style == 2) description += "clean-cut beard"
			else if (creature.beard.style == 3) description += "mountain-man beard"
			return description;
		}
		*/

		//./classes/Appearance.as
		public static string cockDescript(Creature creature, int cockIndex = 0)
		{
			if (creature.cocks.Count == 0) return "<b>ERROR: cockDescript Called But No Cock Present</b>";
			CockType cockType = CockType.HUMAN;
			//if (cockIndex != 99)
			//{ //CockIndex 99 forces a human cock description
			//	if (creature.cocks.Count <= cockIndex) return "<b>ERROR: cockDescript called with index of " + cockIndex + " - out of BOUNDS</b>";
			//	cockType = creature.cocks[cockIndex].cockType;
			//}
			bool isPierced = (creature.cocks.Count == 1) && (creature.cocks[cockIndex].isPierced); //Only describe as pierced or sock covered if the creature has just one cock

			//bool hasSock = (creature.cocks.Count == 1) && (creature.cocks[cockIndex].sock != "");
			bool hasSock = false;
			bool isGooey = creature.body.type == BodyType.GOO;
			return cockDescription(cockType, creature.cocks[cockIndex].length, creature.cocks[cockIndex].girth, creature.lust, creature.genitals.totalCum, isPierced, hasSock, isGooey);
		}

		//./classes/Appearance.as
		public static string cockDescription(CockType cockType, float length, float girth, int lust = 50, int cumQ = 10, bool isPierced = false, bool hasSock = false, bool isGooey = false)
		{
			if (Utils.Rand(2) == 0)
			{
				if (cockType == CockType.HUMAN) return cockAdjective(cockType, length, girth, lust, cumQ, isPierced, hasSock, isGooey) + " " + cockNoun(cockType);
				else return cockAdjective(cockType, length, girth, lust, cumQ, isPierced, hasSock, isGooey) + ", " + cockNoun(cockType);
			}
			return cockNoun(cockType);
		}
		public static string cockNoun(CockType cockType)
		{
			string cockWord = "";

			if (cockType == CockType.ANEMONE)
			{
				cockWord += Utils.RandomChoice("anemone ", "tentacle-ringed ", "blue ", "stinger-laden ", "pulsating ", "anemone ", "stinger-coated ", "blue ", "tentacle-ringed ", "near-transparent ", "squirming ");
			}
			else if (cockType == CockType.AVIAN)
			{
				cockWord += Utils.RandomChoice("bird ", "avian ", "tapered ");
			}
			else if (cockType == CockType.BEE)
			{
				cockWord += Utils.RandomChoice("bee ", "insectoid ", "furred ");
			}
			else if (cockType == CockType.CAT)
			{
				if (Utils.Rand(3) >= 1) cockWord += Utils.RandomChoice("pink ", "animalistic ", "spiny ", "spined ", "oddly-textured ", "barbed ", "nubby ");
				cockWord += Utils.RandomChoice("feline ", "cat-", "kitty-", " kitten-");
			}
			else if (cockType == CockType.DEMON)
			{
				cockWord += Utils.RandomChoice("corrupted ", "nub-covered ", "nubby ", "perverse ", "bumpy ", "cursed ", "infernal ", "unholy ", "blighted ");
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("demon-", "demonic ");
			}
			else if (cockType == CockType.DISPLACER)
			{
				cockWord += Utils.RandomChoice("tentacle-tipped ", "starfish-tipped ", "bizarre ", "beastly ", "cthulhu-tier ", "star-capped ", "knotted ");
				if (Utils.Rand(3) >= 1) cockWord += Utils.RandomChoice("coerl ", "alien ", "almost-canine ", "animal ", "displacer ");
			}
			else if (cockType == CockType.DOG)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("pointed ", "knotty ", "knotted ", "bestial ", "animalistic ");
				cockWord += Utils.RandomChoice("dog-", "dog-shaped ", "canine ", "bestial ", " puppy-", "canine ");
			}
			else if (cockType == CockType.DRAGON)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("segmented ", "pointed ", "knotted ", "mythical ", "tapered", "unusual ", "scaly ");
				cockWord += Utils.RandomChoice("dragon-like ", "draconic ", "dragon-");
			}
			else if (cockType == CockType.ECHIDNA)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("strange ", "four-headed ", "exotic ", "unusual ");
				if (Utils.Rand(4) >= 1) cockWord += "echidna ";
			}
			else if (cockType == CockType.FOX)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("pointed ", "knotty ", "knotted", "bestial ", "animalistic ");
				cockWord += Utils.RandomChoice("fox-", "fox-shaped ", "vulpine ", "vixen-");
			}
			else if (cockType == CockType.HORSE)
			{
				if (Utils.Rand(3) >= 1) cockWord += Utils.RandomChoice("flared ", "bestial ", "flat-tipped ", "mushroom-headed ", "");
				cockWord += Utils.RandomChoice("horse-", "equine ", "stallion-", "beast ");
			}
			else if (cockType == CockType.HUMAN)
			{
				if (Utils.Rand(2) == 0) cockWord += Utils.RandomChoice("human ", "humanoid ", "ordinary-looking ");
			}
			else if (cockType == CockType.KANGAROO)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("pointed ", "tapered ", "curved ", "squirming ");
				if (Utils.Rand(4) >= 1) cockWord += Utils.RandomChoice("kangaroo-like ", "marsupial ");
			}
			else if (cockType == CockType.LIZARD)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("purple ", "bulbous ", "bulging ");
				cockWord += Utils.RandomChoice("reptilian ", "inhuman ", "serpentine ", " snake-", " snake-");
			}
			else if (cockType == CockType.PIG)
			{
				cockWord += Utils.RandomChoice("pig ", "swine ", "pig-like ", "corkscrew-tipped ", "hoggish ", "pink pig-", "pink ");
			}
			else if (cockType == CockType.RHINO)
			{
				cockWord += Utils.RandomChoice("oblong ", "rhino ", "bulged rhino ");
			}
			else if (cockType == CockType.TENTACLE)
			{
				if (Utils.Rand(2) >= 1) cockWord += Utils.RandomChoice("twisting ", "wriggling ", "writhing", "sinuous ", "squirming ", "undulating ", "slithering ");
				cockWord += Utils.RandomChoice("tentacle-", "plant-", "tentacle-", "plant-", "flora ", "smooth ", "vine-", "vine-shaped ", "", "");
			}
			else if (cockType == CockType.WOLF)
			{
				if (Utils.Rand(3) >= 1) cockWord += Utils.RandomChoice("knotted ", "knotty ", "animalistic ", "pointed ", "bestial ");
				cockWord += Utils.RandomChoice("wolf-shaped ", "wolf-", "wolf-", "wolf-", "canine ", "", "");
			}
			else
			{
				cockWord += "";
			}
			cockWord += Utils.RandomChoice("cock", "dick", "dong", "endowment", "mast", "member", "pecker", "penis", "prick", "shaft", "tool");
			return cockWord;
		}

		//New cock adjectives.  The old one sucked dicks
		//This function handles all cockAdjectives. Previously there were separate functions for the player, monsters and NPCs.
		public static string cockAdjective(CockType cockType, float length, float girth, int lust = 50, int cumQ = 10, bool isPierced = false, bool hasSock = false, bool isGooey = false)
		{
			//First, the three possible special cases
			if (isPierced && Utils.Rand(5) == 0) return "pierced";
			if (hasSock && Utils.Rand(5) == 0) return Utils.RandomChoice("sock-sheathed", "garment-wrapped", "smartly dressed", "cloth-shrouded", "fabric swaddled", "covered");
			if (isGooey && Utils.Rand(4) == 0) return Utils.RandomChoice("goopey", "gooey", "slimy");
			//Length 1/3 chance
			if (Utils.Rand(3) == 0)
			{
				if (length < 3) return Utils.RandomChoice("little", "toy-sized", "mini", "budding", "tiny");
				if (length < 5) return Utils.RandomChoice("short", "small");
				if (length < 7) return Utils.RandomChoice("fair-sized", "nice");
				if (length < 9)
				{
					if (cockType == CockType.HORSE) return Utils.RandomChoice("sizable", "pony-sized", "colt-like");
					return Utils.RandomChoice("sizable", "long", "lengthy");
				}
				if (length < 13)
				{
					if (cockType == CockType.DOG) return Utils.RandomChoice("huge", "foot-long", "mastiff-like");
					return Utils.RandomChoice("huge", "foot-long", "cucumber-length");
				}
				if (length < 18) return Utils.RandomChoice("massive", "knee-length", "forearm-length");
				if (length < 30) return Utils.RandomChoice("enormous", "giant", "arm-like");
				if (cockType == CockType.TENTACLE && Utils.Rand(2) == 0) return "coiled";
				return Utils.RandomChoice("towering", "freakish", "monstrous", "massive");
			}
			//Hornyness 1/2
			else if (lust > 75 && Utils.Rand(2) == 0)
			{
				if (lust > 90)
				{ //Uber horny like a baws!
					if (cumQ < 50) return Utils.RandomChoice("throbbing", "pulsating"); //Weak as shit cum
					if (cumQ < 200) return Utils.RandomChoice("dribbling", "leaking", "drooling"); //lots of cum? drippy.
					return Utils.RandomChoice("very drippy", "pre-gushing", "cum-bubbling", "pre-slicked", "pre-drooling"); //Tons of cum
				}
				else
				{//A little less lusty, but still lusty.
					if (cumQ < 50) return Utils.RandomChoice("turgid", "blood-engorged", "rock-hard", "stiff", "eager"); //Weak as shit cum
					if (cumQ < 200) return Utils.RandomChoice("turgid", "blood-engorged", "rock-hard", "stiff", "eager", "fluid-beading", "slowly-oozing"); //A little drippy
					return Utils.RandomChoice("dribbling", "drooling", "fluid-leaking", "leaking"); //uber drippy
				}
			}
			//Girth - fallback
			if (girth <= 0.75) return Utils.RandomChoice("thin", "slender", "narrow");
			if (girth <= 1.2) return "ample";
			if (girth <= 1.4) return Utils.RandomChoice("ample", "big");
			if (girth <= 2) return Utils.RandomChoice("broad", "meaty", "girthy");
			if (girth <= 3.5) return Utils.RandomChoice("fat", "distended", "wide");
			return Utils.RandomChoice("inhumanly distended", "monstrously thick", "bloated");
		}


		//./classes/Appearance.as
		public static string describeByScale(float value, float[] scale, string lessThan = "less than", string moreThan = "more than")
		{
			if (scale.Length == 0) return "indescribable";
			if (scale.Length == 1) return "about " + scale[0];
			if (value < scale[0]) return lessThan + " " + scale[0];
			if (value == scale[0]) return scale[0].ToString();
			for (int i = 1; i < scale.Length; i++)
			{
				if (value < scale[i]) return "between " + scale[i - 1] + " and " + scale[i];
				if (value == scale[i]) return scale[i].ToString();
			}
			return moreThan + " " + scale[scale.Length - 1];
		}

		public static string eyesDescript(Creature creature)
		{
			return DEFAULT_EYES_NAMES[creature.eyes.type] + " eyes";
		}

		//./classes/Appearance.as
		public static string hipDescription(Character character)
		{
			string description = "";
			string[] options;
			if (character.hips.rating <= 1)
			{
				options = new string[]
				{
				"tiny ",
					"narrow ",
					"boyish "
					};
				description = Utils.RandomChoice(options);
			}
			else if (character.hips.rating > 1 && character.hips.rating < 4)
			{
				options = new string[]
				{
				"slender ",
					"narrow ",
					"thin "
					};
				description = Utils.RandomChoice(options);
				if (character.thickness < 30)
				{
					if (Utils.Rand(2) == 0) description = "slightly-flared ";
					else description = "curved ";
				}
			}
			else if (character.hips.rating >= 4 && character.hips.rating < 6)
			{
				options = new string[]
				{
				"well-formed ",
					"pleasant "
					};
				description = Utils.RandomChoice(options);
				if (character.thickness < 30)
				{
					if (Utils.Rand(2) == 0) description = "flared ";
					else description = "curvy ";
				}
			}
			else if (character.hips.rating >= 6 && character.hips.rating < 10)
			{
				options = new string[]
				{
				"ample ",
					"noticeable ",
					"girly "
					};
				description = Utils.RandomChoice(options);
				if (character.thickness < 30)
				{
					if (Utils.Rand(2) == 0) description = "flared ";
					else description = "waspish ";
				}
			}
			else if (character.hips.rating >= 10 && character.hips.rating < 15)
			{
				options = new string[]
				{
				"flared ",
					"curvy ",
					"wide "
					};
				description = Utils.RandomChoice(options);
				if (character.thickness < 30)
				{
					if (Utils.Rand(2) == 0) description = "flared ";
					else description = "waspish ";
				}
			}
			else if (character.hips.rating >= 15 && character.hips.rating < 20)
			{
				if (character.thickness < 40)
				{
					if (Utils.Rand(2) == 0) description = "flared, ";
					else description = "waspish, ";
				}
				options = new string[]
				{
				"fertile ",
					"child-bearing ",
					"voluptuous "
					};
				description += Utils.RandomChoice(options);
			}
			else if (character.hips.rating >= 20)
			{
				if (character.thickness < 40)
				{
					if (Utils.Rand(2) == 0) description = "flaring, ";
					else description = "incredibly waspish, ";
				}
				options = new string[]
				{
				"broodmother-sized ",
					"cow-like ",
					"inhumanly-wide "
					};
				description += Utils.RandomChoice(options);
			}
			//Taurs
			if (character.isTaur() && Utils.Rand(3) == 0) description += "flanks";
			//Nagas have sides, right?
			else if (character.isNaga() && Utils.Rand(3) == 0) description += "sides";
			//Non taurs or taurs who didn't roll flanks
			else
			{
				options = new string[]
				{
				"hips",
					"thighs"
					};
				description += Utils.RandomChoice(options);
			}
			return description;
		}
		//./classes/Appearance.as
		public static string neckDescript(Creature creature)
		{
			return DEFAULT_NECK_NAMES[creature.neck.type] + " neck";
		}

		//./classes/Appearance.as
		public static string oneTailDescript(Creature creature)
		{
			if (creature.tail.type == Tail.NONE)
			{
				return "<b>!Creature has no tails to describe!</b>";
			}

			string descript = "";

			if (creature.tail.type == Tail.FOX && creature.tail.venom >= 1)
			{
				if (creature.tail.venom == 1)
				{
					descript += "your kitsune tail";
				}
				else
				{
					descript += "one of your kitsune tails";
				}
			}
			else
			{
				descript += "your " + DEFAULT_TAIL_NAMES[creature.tail.type] + " tail";
			}

			return descript;
		}
		//./classes/Appearance.as
		public static string rearBodyDescript(Creature creature)
		{
			return DEFAULT_REAR_BODY_NAMES[creature.rearBody.type];
		}
		//./classes/Appearance.as
		public static string sackDescript(Creature creature)
		{
			if (creature.balls.count == 0) return "prostate";

			string[] options;
			string description = "";

			options = new string[]
			{
			"scrotum",
							"sack",
							"nutsack",
							"ballsack",
							"beanbag",
							"pouch"
							};

			description += Utils.RandomChoice(options);

			return description;
		}
		//./classes/Appearance.as
		public static string tailDescript(Creature creature)
		{
			if (creature.tail.type == Tail.NONE)
			{
				return "<b>!Creature has no tails to describe!</b>";
			}

			string descript = "";

			if (creature.tail.type == Tail.FOX && creature.tail.venom >= 1)
			{
				// Kitsune tails, we're using tailVenom to track tail count
				if (creature.tail.venom > 1)
				{
					if (creature.tail.venom == 2) descript += "pair ";
					else if (creature.tail.venom == 3) descript += "trio ";
					else if (creature.tail.venom == 4) descript += "quartet ";
					else if (creature.tail.venom == 5) descript += "quintet ";
					else if (creature.tail.venom > 5) descript += "bundle ";

					descript += "of kitsune tails";
				}
				else descript += "kitsune tail";
			}
			else
			{
				descript += DEFAULT_TAIL_NAMES[creature.tail.type];
				descript += " tail";
			}

			return descript;
		}
		//./classes/Appearance.as
		public static string tongueDescription(Character character)
		{
			// fallback for tongueTypes not fully implemented yet
			if (character.tongue.type == Tongue.HUMAN || !DEFAULT_TONGUE_NAMES.hasOwnProperty('' + character.tongue.type))
				return "tongue";

			return DEFAULT_TONGUE_NAMES[character.tongue.type] + " tongue";
		}
		//./classes/Appearance.as
		public static string wingsDescript(Creature creature)
		{
			return DEFAULT_WING_NAMES[creature.wings.type] + " wings";
		}

		//./classes/Character.as
		public string beardDesc()
		{
			if (hasBeard())
				return "beard";
			else
			{
				//CoC_Settings.error("");
				return "ERROR: NO BEARD! <b>YOU ARE NOT A VIKING AND SHOULD TELL KITTEH IMMEDIATELY.</b>";
			}
		}

		//./classes/Character.as
		public string hornDescript()
		{
			return Appearance.DEFAULT_HORNS_NAMES[horns.type] + " horns";
		}
		//./classes/Character.as
		public string allChestDesc()
		{
			if (biggestTitSize() < 1) return "chest";
			return allBreastsDescript();
		}

		//./classes/Creature.as
		public string chestDesc()
		{
			if (biggestTitSize() < 1) return "chest";
			return Appearance.biggestBreastSizeDescript(this);
			//			return Appearance.chestDesc(this);
		}
		

		public string cockDescriptShort(int cockIndex = 0)
		{
			// catch calls where we're outside of combat, and eCockDescript could be called.
			if (cocks.Count == 0)
				return "<B>ERROR. INVALID CREATURE SPECIFIED to cockDescriptShort</B>";

			string description = "";
			bool descripted = false;
			//Discuss length one in 3 times
			if (Utils.Rand(3) == 0)
			{
				if (cocks[cockIndex].cockLength >= 30)
					description = "towering ";
				else if (cocks[cockIndex].cockLength >= 18)
					description = "enormous ";
				else if (cocks[cockIndex].cockLength >= 13)
					description = "massive ";
				else if (cocks[cockIndex].cockLength >= 10)
					description = "huge ";
				else if (cocks[cockIndex].cockLength >= 7)
					description = "long ";
				else if (cocks[cockIndex].cockLength >= 5)
					description = "average ";
				else
					description = "short ";
				descripted = true;
			}
			else if (Utils.Rand(2) == 0)
			{ //Discuss girth one in 2 times if not already talked about length.
			  //narrow, thin, ample, broad, distended, voluminous
				if (cocks[cockIndex].cockThickness <= .75) description = "narrow ";
				if (cocks[cockIndex].cockThickness > 1 && cocks[cockIndex].cockThickness <= 1.4) description = "ample ";
				if (cocks[cockIndex].cockThickness > 1.4 && cocks[cockIndex].cockThickness <= 2) description = "broad ";
				if (cocks[cockIndex].cockThickness > 2 && cocks[cockIndex].cockThickness <= 3.5) description = "fat ";
				if (cocks[cockIndex].cockThickness > 3.5) description = "distended ";
				descripted = true;
			}
			//Seems to work better without this comma:			if (descripted && cocks[cockIndex].cockType != CockType.HUMAN) description += ", ";
			description += Appearance.cockNoun(cocks[cockIndex].cockType);

			return description;
		}

		//./classes/PlayerAppearance.as
		public void sockDescript(int index)
		{
			outputText(" ");
			if (player.cocks[index].sock == "wool")
				outputText("It's covered by a wooly white cock-sock, keeping it snug and warm despite how cold it might get.");
			else if (player.cocks[index].sock == "alabaster")
				outputText("It's covered by a white, lacey cock-sock, snugly wrapping around it like a bridal dress around a bride.");
			else if (player.cocks[index].sock == "cockring")
				outputText("It's covered by a black latex cock-sock with two attached metal rings, keeping your cock just a little harder and [balls] aching for release.");
			else if (player.cocks[index].sock == "viridian")
				outputText("It's covered by a lacey dark green cock-sock accented with red rose-like patterns. Just wearing it makes your body, especially your cock, tingle.");
			else if (player.cocks[index].sock == "scarlet")
				outputText("It's covered by a lacey red cock-sock that clings tightly to your member. Just wearing it makes your cock throb, as if it yearns to be larger...");
			else if (player.cocks[index].sock == "cobalt")
				outputText("It's covered by a lacey blue cock-sock that clings tightly to your member... really tightly. It's so tight it's almost uncomfortable, and you wonder if any growth might be inhibited.");
			else if (player.cocks[index].sock == "gilded")
				outputText("It's covered by a metallic gold cock-sock that clings tightly to you, its surface covered in glittering gems. Despite the warmth of your body, the cock-sock remains cool.");
			else if (player.cocks[index].sock == "amaranthine")

			{
				outputText("It's covered by a lacey purple cock-sock");
				if (player.cocks[index].cockType != CockType.DISPLACER)
					outputText(" that fits somewhat awkwardly on your member");
				else
					outputText(" that fits your coeurl cock perfectly");
				outputText(". Just wearing it makes you feel stronger and more powerful.");
			}
			else if (player.cocks[index].sock == "red")
				outputText("It's covered by a red cock-sock that seems to glow. Just wearing it makes you feel a bit powerful.");
			else if (player.cocks[index].sock == "green")
				outputText("It's covered by a green cock-sock that seems to glow. Just wearing it makes you feel a bit healthier.");
			else if (player.cocks[index].sock == "blue"
							outputText("It's covered by a blue cock-sock that seems to glow. Just wearing it makes you feel like you can cast spells more effectively.");

			else outputText("<b>Yo, this is an error.</b>");
		}

	}
}