﻿//SpecialCharacters.cs
//Description:
//Author: JustSomeGuy
//3/25/2019, 12:08 AM
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using CoC.Frontend.UI;
using CoC.Frontend.Items.Materials.Jewelry;
using CoC.Frontend.Perks.Endowment;
using CoC.Frontend.Perks.History;
using CoC.Frontend.Strings.Creatures;
using CoC.Frontend.Items.Wearables.Piercings;
using System;
using System.Collections.Generic;
using CoC.Frontend.Creatures.PlayerData;

namespace CoC.Frontend.Creatures
{
	//Not Gonna Lie, i really, really like local functions now. so convenient.

	internal static class SpecialCharacters
	{
		//NOTE: Most of these characters were created during different versions of the code, and therefore have different levels of support for customization options.
		//As of this writing (June 2019), new features have been implemented which none of these characters take advantage of. Normally, a player would have the
		//option to change these new features, as the custom character does not have values for these. However, in the spirit of the old style, any characters that
		//were otherwise locked from player customization will have these new features locked as well, using the default values.

		//In the event you are the creative mind behind these characters, feel free to implement the new features (or free them for player customization) as you see fit.

		public static readonly Dictionary<string, Func<PlayerCreator>> specialCharacterLookup;
		public static readonly Dictionary<string, SimpleDescriptor> nameAndFlavorText;

		//technically, hashsets are ordered until something is removed, and since we'll never remove anything, we can abuse this.
		private static readonly HashSet<string> charactersWithPartialHistory;
		private static readonly HashSet<string> charactersWithFullHistory;

		//private static StandardDisplay currentDisplay => DisplayManager.GetCurrentDisplay();

		public static DropDownEntry[] SpecialCharacterDropDownList(StandardDisplay currentDisplay)
		{
			DropDownEntry[] retVal = new DropDownEntry[nameAndFlavorText.Count];//+2 later for helpers.

			int x = 0;
			Action OnSelect(string name, SimpleDescriptor text)
			{
				return () =>
				{
					currentDisplay.UpdateInputText(name);
					currentDisplay.SetPostDropDownMenuText(text);
				};
			}
			foreach (var entry in nameAndFlavorText)
			{
				retVal[x] = new DropDownEntry(entry.Key, OnSelect(entry.Key, entry.Value));
				x++;
			}
			return retVal;
		}


		static SpecialCharacters()
		{
			specialCharacterLookup = new Dictionary<string, Func<PlayerCreator>>();
			nameAndFlavorText = new Dictionary<string, SimpleDescriptor>();
			charactersWithPartialHistory = new HashSet<string>();
			charactersWithFullHistory = new HashSet<string>();
			//
			AddCustomCharacter("Annetta", AnnettaFlavorText, customAnnetta); //locked
			AddCustomCharacter("Aria", AriaFlavorText, customAria);//unlocked
			AddCustomCharacter("Bertram", BertramFlavorText, customBertram);//locked
			AddCustomCharacter("Ceveo", CeveoFlavorText, customCeveo);//
			AddCustomCharacter("Charaun", CharaunFlavorText, customCharaun);//unlocked
			AddCustomCharacter("Charlie", CharlieFlavorText, customCharlie);
			AddCustomCharacter("Cody", CodyFlavorText, customCody);//unlocked
			AddCustomCharacter("Etis", EtisFlavorText, customEtis);
			AddCustomCharacter("Galatea", GalateaFlavorText, customGalatea);//unlocked

			AddCustomCharacter("Gundam", GundamFlavorText, customGundam);//unlocked
			AddCustomCharacter("Hikari", HikariFlavorText, customHikari);//unlocked
			AddCustomCharacter("Isaac", IsaacFlavorText, customIsaac);
			AddCustomCharacter("Katti", KattiFlavorText, customKatti);//unlocked
			AddCustomCharacter("Leah", LeahFlavorText, customLeah);
			AddCustomCharacter("Lucina", LucinaFlavorText, customLucina);//unlocked
			AddCustomCharacter("Lukaz", LukazFlavorText, customLukaz);
			AddCustomCharacter("Mara", MaraFlavorText, customMara);
			AddCustomCharacter("Mihari", MihariFlavorText, customMihari);
			AddCustomCharacter("Mirvanna", MirvannaFlavorText, customMirvanna);
			AddCustomCharacter("Nami", NamiFlavorText, customNami);
			AddCustomCharacter("Navorn", NavornFlavorText, customNavorn);//unlocked
			AddCustomCharacter("Nixi", NixiFlavorText, customNixi);
			AddCustomCharacter("Peone", PeoneFlavorText, customPeone);
			AddCustomCharacter("Prismere", PrismereFlavorText, customPrismere);
			AddCustomCharacter("Rann Rayla", RannRaylaFlavorText, customRannRayla);
			AddCustomCharacter("Rope", RopeFlavorText, customRope);//unlocked
			AddCustomCharacter("Sera", SeraFlavorText, customSera);
			AddCustomCharacter("Siveen", SiveenFlavorText, customSiveen);
			AddCustomCharacter("Sora", SoraFlavorText, customSora);
			AddCustomCharacter("Tyriana", TyrianaFlavorText, customTyriana);
			AddCustomCharacter("Vahdunbrii", VahdunbriiFlavorText, customVahdunbrii);
		}

		private static void AddCustomCharacter(string name, SimpleDescriptor selectFlavorText, Func<PlayerCreator> createCharacterFunction/*, bool isFullyDefined*/)
		{
			if (selectFlavorText is null) throw new ArgumentNullException(nameof(selectFlavorText));
			specialCharacterLookup.Add(name, createCharacterFunction);
			nameAndFlavorText.Add(name, selectFlavorText);
			//if (isFullyDefined)
			//{
			//	charactersWithFullHistory.Add(name);
			//}
			//else
			//{
			//	charactersWithPartialHistory.Add(name);
			//}
		}
		/*
				public PlayerCreator CharSpecial() { }

			// 2d array name | PlayerCreator | skip creation | description
			public const customs:Array = [
					//
			[ "Without pre-defined history:", null, false, "" ],

				]
				*/
		private static SimpleDescriptor AnnettaFlavorText => SpecialCharacterStrings.AnnettaText;
		private static PlayerCreator customAnnetta()
		{
			//sb.Append("You're a rather well-endowed hermaphrodite that sports a thick, dog-knotted cock, an unused pussy, and a nice, stretchy butt-hole. You've also got horns and demonic high-heels on your feet. It makes you wonder why you would ever get chosen to be champion!");
			//Specific Character	"Gender: Herm
			return new PlayerCreator("Annetta")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,
				femininity = 90,
				//Balls: Four 5 inch wide
				numBalls = 4,
				ballSize = 5,
				heightInInches = 67,
				//Butt: Loose"
				analLooseness = AnalLooseness.ROOMY,
				assVirgin = false,
				//Skin: Purple
				complexion = Tones.PURPLE,
				//Vagina: Tight, virgin, 0.5 inch clitoris
				vaginas = new VaginaCreator[] { new VaginaCreator(0.5) },
				//Penis: 13 inch long 3 inch wide penis, dog shaped, 6.5 inch knot
				cocks = new CockCreator[] { new CockCreator(CockType.DOG, 13, 3, 2.2) },
				cockVirgin = false,
				cumMultiplier = 20,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.J, 5) },
				nippleStatus = NippleStatus.FUCKABLE,
				lactationMultiplier = 20,
				//Hair: Back length orange
				hairLength = 30,
				hairColor = HairFurColors.ORANGE,
				//Face: Elf ears, 4x demonic horns
				earType = EarType.ELFIN,
				hornCount = 4,
				hornType = HornType.DEMON,
				//Body: Plump, no muscle tone, wide thighs, badonkulous ass, demon tail, demonic high heels
				thickness = 75,
				tone = 0,
				hipSize = 17,
				buttSize = 17,
				tailType = TailType.DEMONIC,
				lowerBodyType = LowerBodyType.DEMONIC_HIGH_HEELS,

				////Equipment: Starts with spiked fist
				//creator.setWeapon(weapons.S_GAUN0);
				////Perks: Fighter and Lotsa Jizz"	Annetta
				perks = new List<StandardPerk>()
				{
					new Fighter(),
					new MessyOrgasms(),
				},
			};

		}



		private static SimpleDescriptor AriaFlavorText => SpecialCharacterStrings.AriaText;
		private static PlayerCreator customAria()
		{
			//2/26/2013 8:18:21	rdolave@gmail.com	Character Creation	"female DD breasts feminity 100 butt size 5 hip size 5 body thickness 10 clit I would like her nipples pierced with Ceraphs piercing
			//(on a side note how much do you think it would cost to add bell nipple,labia and clit piercings as well as an option for belly button piercings would like to see belly button piecings with a few different options as well.
			//Also would love to have handcuff ear piercings.)"	Would like the bimbo brain and bimbo body perks as well as the nine tail PerkLib. demonic high heels, pink skin, obscenely long pink hair would like her to be a kitsune with the nine tails.
			//pink fur. starting equipment would like to be the succubus whip and nurse's outfit. Also would like the xmas perk and all three Vday perks. Aria
			PiercingJewelry labiaPiercing() => new PiercingJewelry(JewelryType.RING, new Ruby(), true);
			return new PlayerCreator("Aria")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,
				femininity = 100,
				//interpreted the valentines perks as not being a virgin, considering their whole attitude on sex. also, 4 labia piercings on each side (of a possible 6), and a horizontal clit hood piercing
				vaginas = new VaginaCreator[]
				{
					new VaginaCreator(vaginalLooseness: VaginalLooseness.NORMAL, isVirgin: false,
						clitJewelry: new PiercingData<ClitPiercingLocation>(){ [ClitPiercingLocation.HOOD_HORIZONTAL] = new PiercingJewelry(JewelryType.RING, new Emerald(), true)},
						labiaJewelry: new PiercingData<LabiaPiercingLocation>()
						{
							[LabiaPiercingLocation.LEFT_1] = labiaPiercing(), [LabiaPiercingLocation.LEFT_2] = labiaPiercing(), [LabiaPiercingLocation.LEFT_5] = labiaPiercing(), [LabiaPiercingLocation.LEFT_6] = labiaPiercing(),
							[LabiaPiercingLocation.RIGHT_1] = labiaPiercing(), [LabiaPiercingLocation.RIGHT_2] = labiaPiercing(), [LabiaPiercingLocation.RIGHT_5] = labiaPiercing(), [LabiaPiercingLocation.RIGHT_6] = labiaPiercing()
						})
				},
				analLooseness = AnalLooseness.LOOSE,
				assVirgin = false,
				tailType = TailType.FOX,
				tailCount = 9,
				breasts = new BreastCreator[]
				{
					new BreastCreator(CupSize.DD, null,
						new PiercingData<NipplePiercingLocation>()
						{
						[NipplePiercingLocation.LEFT_HORIZONTAL] = new CeraphNipplePiercings(),
						[NipplePiercingLocation.RIGHT_HORIZONTAL] = new CeraphNipplePiercings()
						})
				},
				lowerBodyType = LowerBodyType.DEMONIC_HIGH_HEELS,
				complexion = Tones.PINK,
				underTone = Tones.PINK,
				bodyType = BodyType.KITSUNE,
				furColor = new FurColor(HairFurColors.PINK),
				hairColor = HairFurColors.PINK,
				hairLength = 50,
				hipSize = 5,
				buttSize = 5,
				thickness = 10,
				earPiercings = new PiercingData<EarPiercingLocation>()
				{
					[EarPiercingLocation.LEFT_LOBE_1] = new Handcuffs(),
					[EarPiercingLocation.RIGHT_LOBE_1] = new Handcuffs()
				},

			};
			//flags[kFLAGS.PC_FETISH] = 2;
			//creator.createPerk(PerkLib.EnlightenedNinetails, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.BimboBody, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.BimboBrains, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.ElvenBounty, 0, 15, 0, 0);
			//creator.createPerk(PerkLib.PureAndLoving, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.SensualLover, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.OneTrackMind, 0, 0, 0, 0);
			//creator.setWeapon(weapons.SUCWHIP);
			//creator.setArmor(armors.NURSECL);
		}

		private static SimpleDescriptor BertramFlavorText => SpecialCharacterStrings.BertramText;
		private static PlayerCreator customBertram()
		{
			//Character Creation
			//herm, canine cock - 8", virgin, tight, wet
			//fox ears, tails, A cup breasts with normal nipples	Betram
			return new PlayerCreator("Bertram")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,
				earType = EarType.FOX,
				tailType = TailType.FOX,
				tailCount = 1,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.A) },
				cocks = new CockCreator[] { new CockCreator(CockType.DOG, 8, 1, 1.4) },
				cockVirgin = true,
				vaginas = new VaginaCreator[] { new VaginaCreator(0.25, VaginalWetness.WET, isVirgin: true) }
			};
			//sb.Append("You're quite the foxy herm, and as different as you were compared to the rest of Ingnam, it's no surprise you were sent through first.");
		}

		private static SimpleDescriptor CeveoFlavorText => SpecialCharacterStrings.CeveoText;
		private static PlayerCreator customCeveo()
		{
			//Pair<CockPiercingLocation, PiercingJewelry> silverAlbert = [CockPiercingLocation.PRINCE_ALBERT] );
			CockCreator createCock() => new CockCreator(CockType.HUMAN, 12, 5.5, cockJewelry: new PiercingData<CockPiercingLocation> { [CockPiercingLocation.PRINCE_ALBERT] = new PiercingJewelry(JewelryType.RING, new Silver(), true) });
			PiercingJewelry silverStuds() => new PiercingJewelry(JewelryType.BARBELL_STUD, new Silver(), true);

			//finally, a character with a description that can reasonably allow the player to choose their gender.
			return new PlayerCreator("Ceveo")
			{
				//Male. 2 cock. 5.5 average thickness and 12 in with excessive thickness both pierced with silver rings. Balls, large, about the size of a billiard ball, four of them. All humanish, more details on the character.
				defaultGender = Gender.MALE,
				forceDefaultGender = false,

				numBalls = 4,
				ballSize = 3,
				cocks = new CockCreator[] { createCock(), createCock() },
				cockVirgin = false,
				//vagina defined to allow players to override gender and still keep the whole piercing theme. will be ignored if male.
				vaginas = new VaginaCreator[]
				{
					new VaginaCreator(2.0, isVirgin:true, clitJewelry:new Dictionary<ClitPiercingLocation, PiercingJewelry>(){[ClitPiercingLocation.HOOD_VERTICAL] = new PiercingJewelry(JewelryType.HORSESHOE, new Silver(), true) })
				},
				//"Androgynous face, large brown eyes, long black hair down to about ass level, full lips, pirced with one silver ring ass itself is round and thick, chest is flat, only two nipples, about nickel sized pierced with silver studs, skin of a pale ghostly transparent complexion, rest of the body is not notably muscular or chubby in any definite way, feet seem to taper off into full transparency. Full body housed in the lewd Inquisitor Armor, wielding a Wizard Staff. Starting at level 5 with tank, regeneration, healing, smarts, channeling, mage and incorperability perks, a full knowledge of
				heightInInches = 72,
				femininity = 50,
				hairLength = 35,
				leftEyeColor = EyeColor.BROWN,
				hairColor = HairFurColors.BLACK,
				lipPiercings = new PiercingData<LipPiercingLocation>() { [LipPiercingLocation.LABRET] = new PiercingJewelry(JewelryType.RING, new Silver(), true) },
				buttSize = 8,
				hipSize = 8,
				breasts = new BreastCreator[]
				{
					new BreastCreator(CupSize.FLAT, 1.0,new PiercingData<NipplePiercingLocation>()
					{
						[NipplePiercingLocation.LEFT_HORIZONTAL] = silverStuds(),
						[NipplePiercingLocation.RIGHT_HORIZONTAL] = silverStuds()
					})
				},

				complexion = Tones.PALE,
				initialLevel = 5,
				//magic, 50 Int, 50 tough, Speed 15, Str 10, 30 corruption, 30 libido, 10 sensitivity.
				intelligence = 50,
				toughness = 50,
				speed = 15,
				strength = 10,
				corruption = 30,
				libido = 30,
				sensitivity = 10,
				perks = new List<StandardPerk>()
				{
					new Smart(),
					new Healer(),
				},
			};

			//creator.createPerk(PerkLib.Incorporeality, 0, 0, 0, 0);
			//creator.setArmor(armors.I_CORST);
			//creator.setWeapon(weapons.W_STAFF);

			//creator.createPerk(PerkLib.Regeneration, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Channeling, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Mage, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Tank, 0, 0, 0, 0);
			//creator.createStatusEffect(StatusEffects.KnowsArouse,0,0,0,0);
			//creator.createStatusEffect(StatusEffects.KnowsHeal,0,0,0,0);
			//creator.createStatusEffect(StatusEffects.KnowsMight,0,0,0,0);
			//creator.createStatusEffect(StatusEffects.KnowsCharge,0,0,0,0);
			//creator.createStatusEffect(StatusEffects.KnowsBlind,0,0,0,0);
			//creator.createStatusEffect(StatusEffects.KnowsWhitefire,0,0,0,0);

			//sb.Append("As a wandering mage you had found your way into no small amount of trouble in the search for knowledge. A strange tome here, a ritual there, most people found your pale form unsettling. They would be further troubled if they could see your feet! Lets not even begin on the blood magic. Yes, your interest in examining every aspect of magic has run you down a strange path, so when you wandered into Ingnam and began to hear of the exile of the Champion, and the superstitions that surrounded it you were intrigued, as every little rumor and ritual often had a grain of truth. You snuck into the cave prior to the ritual, where the old man supposedly led every Champion, and there you found a strange portal that emanated a certain degree of spacial transparency - more than the portal's own. Within it must have been a whole new world! Throwing caution to the wind, your curiosities engulfing you, you dove in with nary a thought for the consequences.");
		}

		private static SimpleDescriptor CharaunFlavorText => SpecialCharacterStrings.CharaunText;
		private static PlayerCreator customCharaun()
		{
			//Herm, Fox Cock: (27"l x 1.4"w, knot multiplier 3.6), No Balls, Cum Multiplier: 7,500, Vaginal Wetness: 5, Clit length: 0.5, Virgin, Fertility: 15	9-tailed "enlightened" kitsune( a pure-blooded kitsune with the "Enlightened Nine-tails" perk and magic specials)
			return new PlayerCreator("Charaun")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,

				cocks = new CockCreator[] { new CockCreator(CockType.DOG, 27, 1.4, 3.6) },
				numBalls = 0,
				ballSize = 0,
				cumMultiplier = 7500,
				vaginas = new VaginaCreator[] { new VaginaCreator(0.5, VaginalWetness.SLAVERING) },
				fertility = 15,
				tailType = TailType.FOX,
				tailCount = 1,
				//creator.createPerk(PerkLib.EnlightenedNinetails,0,0,0,0);
				//if possible with fur, Hair color: "midnight black", Skin/Fur color: "ashen grayish-blue", Height: 65", Tone: 100, Thickness: 0, Hip rating: 6, Butt rating: 3,Feminimity: 50, ( 4 rows of breasts (Descending from the top ones: D,C,B,A), nipple length: 0.1", Fuckable, 1 nipple per breast, Tongue type: demon
				hairColor = HairFurColors.MIDNIGHT_BLACK,
				furColor = new FurColor(HairFurColors.GRAYISH_BLUE),
				bodyType = BodyType.KITSUNE,
				heightInInches = 65,
				tone = 100,
				thickness = 0,
				hipSize = 6,
				buttSize = 3,
				femininity = 50,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.D, 3.0), new BreastCreator(CupSize.C, 3.0), new BreastCreator(CupSize.B, 3.0), new BreastCreator(CupSize.A, 3.0) },
				nippleStatus = NippleStatus.FUCKABLE,
				tongueType = TongueType.DEMONIC
			};
			//Starting with an Inscribed Spellblade and Bondage Straps.	Charaun
			//creator.setArmor(armors.BONSTRP);
			//creator.setWeapon(weapons.S_BLADE);
		}

		private static SimpleDescriptor CharlieFlavorText => SpecialCharacterStrings.CharlieText;
		private static PlayerCreator customCharlie()
		{
			//sb.Append("You're strong, smart, fast, and tough. It also helps that you've got four dongs well beyond what others have lurking in their trousers. With your wings, bow, weapon, and tough armor, you're a natural for protecting the town.");
			return new PlayerCreator("Charlie")
			{
				defaultGender = Gender.MALE,
				forceDefaultGender = true,

				toughness = 25,
				strength = 25,
				speed = 25,
				fertility = 5,
				hairLength = 26,
				hairColor = HairFurColors.BLONDE,
				complexion = Tones.LIGHT,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT) },

				hipSize = 5,
				buttSize = 5,
				//Large feathered wings (Any chance in heck I could get 'angel' as the race descriptor? Just asking. I'm fine if the answer is 'no')
				wingType = WingType.FEATHERED,
				largeWings = true,

				//While we're on the subject, would glowing eyes be possible? I'll take normal eyes if not.
				//Not quite, but i can make them silver. that's pretty close, i guess.
				leftEyeColor = EyeColor.SILVER,
				//Tallness 84 (8 feet 0 inches)
				heightInInches = 84,
				//Femininity 10
				femininity = 10,
				//Thickness 50
				thickness = 50,
				//Tone 90
				tone = 90,
				//Int 50 (if possible)
				intelligence = 50,
				//Freckled skinAdj
				skinTexture = SkinTexture.FRECKLED,
				facialSkinTexture = SkinTexture.FRECKLED,
				//Male
				//Would it be possible to code a cock type that morphs into different cock types? (i.e. it loads a different cock type description each sex scene) If so, I'd like him to have a pair of them, one 24 inches long and 3 inches wide and the second 12-inches long and 2 inches wide. If not, I'll take a dragon and horse cock at 24/3 each as well as a dog and cat cock at 12/2 each.
				cocks = new CockCreator[] { new CockCreator(CockType.DRAGON, 24, 3), new CockCreator(CockType.HORSE, 24, 3), new CockCreator(CockType.DOG, 12, 2), new CockCreator(CockType.CAT, 12, 2) },
				cockVirgin = false,
				//A pair of 8-inch balls
				numBalls = 2,
				ballSize = 8,
				//removed these and replaced them with messy orgasm. same effect, but now has an endowment perk.
				////A virility boost would be nice too if possible.
				//cumMultiplier = 50

				//Is it possible to get extra starting perks added? If so, I'd like History: Religious added to whatever is selected on creation. If not, please ignore this line.
				//i mean, i guess it's possible. For reasons unknown, has fighter and whore. could remove them and do some magic to make religious added on.
				perks = new List<StandardPerk>
				{
					new Fighter(),
					new Whore(),
					new MessyOrgasms(),
				},
			};

			//creator.teaseLevel = 1;
			//Beautiful Sword
			//creator.setWeapon(weapons.B_SWORD);
			//creator.setArmor(armors.SSARMOR);
			//Beautiful Armor (Or just Spider Silk Armor)
			//Pure Pearl
			//10 Perk Points (if possible, feel free to make it less if you feel it necessary)
			//creator.perkPoints = 10;
			//Bow
			//creator.createKeyItem("Bow",0,0,0,0);
			//Bow skill 100 (Sorry Kelt, I can't hear your insults over my mad Robin Hood skillz)
			//creator.createStatusEffect(StatusEffects.Kelt,100,0,0,0);
			//creator.createPerk(PerkLib.HistoryFighter, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.HistoryWhore, 0, 0, 0, 0);
		}

		private static SimpleDescriptor CodyFlavorText => SpecialCharacterStrings.CodyText;
		private static PlayerCreator customCody()
		{
			//sb.Append("Your orange and black tiger stripes make you cut a more imposing visage than normal, and with your great strength, armor, and claymore, you're a natural pick for champion.");
			//well to start off the name would be Cody
			//-Cat with (black and orange tiger fur if possible) if not just Orange fur
			return new PlayerCreator("Cody")
			{
				defaultGender = Gender.MALE,
				forceDefaultGender = false,

				hairColor = HairFurColors.BLACK,
				furColor = new FurColor(HairFurColors.BLACK, HairFurColors.ORANGE, FurMulticolorPattern.STRIPED),
				bodyType = BodyType.SIMPLE_FUR,
				complexion = Tones.LIGHT,

				strength = 41
			};
			//-Chainmail armor
			//creator.setArmor(armors.FULLCHN);
			//-Large Claymore (i understand 40 Strength is need so if he could start with that would be great if not hit the gyms)"
			//creator.setWeapon(weapons.CLAYMR0);
		}

		private static SimpleDescriptor GalateaFlavorText => SpecialCharacterStrings.GalateaText;
		private static PlayerCreator customGalatea()
		{
			//"(Dangit Fenoxo! Stop adding sexy must-have things to the game! If it's not too late to update it I've added in that sexy new armor. Thanks!)
			//sb.Append("You've got large breasts prone to lactation. You aren't sure WHY you got chosen as a champion, but with your considerable strength, you're sure you'll do a good job protecting Ingnam.");
			return new PlayerCreator("Galatea")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,
				//Other:
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				//Hair length: Very long
				hairLength = 22,
				//Breast size: HH
				breasts = new BreastCreator[] { new BreastCreator(CupSize.HH) },
				//Femininity/Beauty: Very high
				femininity = 90,
				// Height: 5'4
				heightInInches = 64,

				//Perks: Feeder, Strong Back, Strong Back 2
				//creator.createStatusEffect(StatusEffects.Feeder,0,0,0,0);
				//creator.createPerk(PerkLib.Feeder, 0, 0, 0, 0);

				//creator.createPerk(PerkLib.StrongBack, 0, 0, 0, 0);
				//creator.createPerk(PerkLib.StrongBack2, 0, 0, 0, 0);

				//Equipment:
				//Weapon: Warhammer
				//creator.setWeapon(weapons.WARHAM0);
				//Armor: Lusty shit
				//creator.setArmor(armors.LMARMOR);
				//creator.createPerk(PerkLib.SluttySeduction, 10 + flags[kFLAGS.BIKINI_ARMOR_BONUS], 0, 0, 0);

				//Stats: (if possible)
				//Strength: 90
				strength = 90,
				//Fertility: 100
				fertility = 100,
				corruption = 25
			};
			//Inventory: Lactaid, GroPlus, BimboLq
			//creator.itemSlot1.setItemAndQty(consumables.LACTAID,5);
			//creator.itemSlot2.setItemAndQty(consumables.GROPLUS,5);
			//creator.itemSlot3.setItemAndQty(consumables.BIMBOLQ,1);
			//creator.itemSlot4.unlocked = true;
			//creator.itemSlot4.setItemAndQty(armors.BIMBOSK,1);
			//creator.itemSlot5.unlocked = true;
		}

		//the easiest player creator ever.
		private static SimpleDescriptor GundamFlavorText => SpecialCharacterStrings.GundamText;
		private static PlayerCreator customGundam()
		{
			//sb.Append("You're fabulously rich, thanks to a rather well-placed bet on who would be the champion. Hopefully you can buy yourself out of any trouble you might get in.");
			return new PlayerCreator("Gundam")
			{
				gems = 1000 + Utils.Rand(1501)
			};
			//for my custom character profile i want the name to be gundam all i want is to start out with around 1000-2500 gems like as a gift from the elder or something to help me out.
		}

		private static SimpleDescriptor HikariFlavorText => SpecialCharacterStrings.HikariText;
		private static PlayerCreator customHikari()
		{
			//Character Creation	If possible I would like a herm with a cat cock that is 10 inches by 4 inches. Anything else is up to you.	I would like a herm catmorph with two large d breasts and shoulder length hair. Also if possible I would like to start with some gel armor. Everything else is fair game.	Hikari
			//sb.Append("As a herm with a super-thick cat-cock, D-cup breasts, and out-of-this-world armor, you're a natural pick for champion.");

			return new PlayerCreator("Hikari")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,

				cocks = new CockCreator[] { new CockCreator(CockType.CAT, 10, 4) },
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				breasts = new BreastCreator[] { new BreastCreator(CupSize.D) },
				hairLength = 10
			};
			//creator.setArmor(armors.GELARMR);
		}

		private static SimpleDescriptor IsaacFlavorText => SpecialCharacterStrings.IsaacText;
		private static PlayerCreator customIsaac()
		{
			Dictionary<CockPiercingLocation, PiercingJewelry> ladderPiercings(JewelryMaterial material)
			{
				Dictionary<CockPiercingLocation, PiercingJewelry> retVal = new Dictionary<CockPiercingLocation, PiercingJewelry>();
				foreach (CockPiercingLocation location in CockPiercingLocation.allLocations)
				{
					if (location == CockPiercingLocation.PRINCE_ALBERT)
					{
						continue;
					}
					retVal.Add(location, new PiercingJewelry(JewelryType.BARBELL_STUD, material, true));
				}
				return retVal;
			};

			//sb.Append("You were born of a disgraced priestess, who raised you alone until she was taken by illness. Since then, you've worked a number of odd jobs to get by, which ultimately resulted in you being chosen as champion.");
			return new PlayerCreator("Isaac")
			{
				defaultGender = Gender.MALE,
				forceDefaultGender = true,
				speed = 20,
				//in my ar, Issac was born to a disgraced priestess (she was raped by marauders) and raised by her alone until she died from an illness and was pretty much left to fend for and earn a living for himself (hence the fighter and smith background's too) until, years later he was chosen as 'champion'~
				//~~~~~~~~~~~~~~~~~~~~~~~~~~~~
				//sex - male
				numBalls = 2,
				//- a pair of apple sized balls each measuring three inches across
				ballSize = 3,
				//anatomy - twin dicks
				//the first, a vulpine dick (12 in. long, 2.8 in. thick with a knot roughly 4.5 in. at full size) with a Fertite jacob's ladder piercing
				//and the second, a barbed feline dick (10 in. long and 2.5 in thick) with an Emerald jacob's ladder
				//heh, ribbed for their pleasure ;d lol
				cocks = new CockCreator[]
				{
					new CockCreator(CockType.DOG, 12, 2.8, 1.8, cockJewelry:ladderPiercings(new Fertite())),
					new CockCreator(CockType.CAT, 10, 2.5, null, cockJewelry:ladderPiercings(new Emerald()))
				},
				//- and one tight asshole
				analLooseness = AnalLooseness.NORMAL,
				assVirgin = true,
				//- kitsune
				//- moderately long white hair (9 inches)
				hairLength = 9,
				hairColor = HairFurColors.SILVER_WHITE,
				//- human face
				//- fox ears
				earType = EarType.FOX,
				//- olive complexion
				complexion = Tones.OLIVE,
				//- demon tongue (oral fetish ;d)
				tongueType = TongueType.DEMONIC,
				//- 5 foot 9 inch tall
				heightInInches = 69,
				//- average build
				//- body thickness of around 50
				thickness = 50,
				//- 'tone of about 70
				tone = 70,
				//- two flat breasts each supporting one 0.2-inch nipple
				breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT, 0.2) },
				//- three fox tails
				tailType = TailType.FOX,
				tailCount = 3,
				//- gift: fast
				//creator.createPerk(PerkLib.Fast, 0.25, 0, 0, 0);
				////- history: religion
				//creator.createPerk(PerkLib.HistoryReligious,0,0,0,0);
				////(and if possible)
				////- history: fighter
				//creator.createPerk(PerkLib.HistoryFighter,0,0,0,0);
				////- history: smith
				//creator.createPerk(PerkLib.HistorySmith,0,0,0,0);

				perks = new List<StandardPerk>()
				{
					new Fast(),
					new Religious(),
					new Fighter(),
					new Smith(),
				},
			};
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//equipment;
			//- katana (don't suppose you could rename the katana 'Zon'ith' could you? ~.^)
			//Items: Katana, Leather Armor
			//creator.setWeapon(weapons.KATANA0);
			//- robes
			//creator.setArmor(armors.M_ROBES);
		}

		private static SimpleDescriptor KattiFlavorText => SpecialCharacterStrings.KattiText;
		private static PlayerCreator customKatti()
		{
			//sb.Append("You have big breasts with big, fuckable nipples on them, and no matter what, your vagina always seems to be there to keep you company.");
			return new PlayerCreator("Katti")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,

				//Gender: Female
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				//"Ears: Bunny
				earType = EarType.BUNNY,
				//Tail: Bunny
				tailType = TailType.RABBIT,
				//Face: Human
				//Breasts: H-cup with 4.5 inch fuckable nipples"
				breasts = new BreastCreator[] { new BreastCreator(CupSize.H, 4.5) },

				nippleStatus = NippleStatus.FUCKABLE
			};
		}

		private static SimpleDescriptor LeahFlavorText => SpecialCharacterStrings.LeahText;
		private static PlayerCreator customLeah()
		{
			return new PlayerCreator("Leah")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,

				//creator.setArmor(armors.LEATHRA);
				//creator.createPerk(PerkLib.WizardsEndurance, 30, 0, 0, 0);
				//creator.setWeapon(weapons.W_STAFF);
				//creator.itemSlot1.setItemAndQty(consumables.B__BOOK, 1);
				//creator.itemSlot2.setItemAndQty(consumables.W__BOOK, 2);


				breasts = new BreastCreator[] { new BreastCreator(CupSize.D) },
				vaginas = new VaginaCreator[] { new VaginaCreator(0.5) },
				fertility = 10,
				hipSize = 8,
				buttSize = 8,
				strength = 15,
				toughness = 15,
				speed = 18,
				intelligence = 17,
				sensitivity = 15,
				libido = 15,
				corruption = 0,
				hairLength = 13,
				femininity = 85,
				tone = 50,
				thickness = 65,
				complexion = Tones.OLIVE,
				hairColor = HairFurColors.BLACK,
				analLooseness = AnalLooseness.NORMAL,
				assVirgin = true,
				heightInInches = 67,

				perks = new List<StandardPerk>()
				{
					new Scholar(),
					new Smart(),
				},
			};
		}

		private static SimpleDescriptor LucinaFlavorText => SpecialCharacterStrings.LucinaText;
		private static PlayerCreator customLucina()
		{
			//sb.Append("You're a blond, fair-skinned lass with a well-made bow and the skills to use it. You have D-cup breasts and a very moist cunt that's seen a little action. You're fit and trim, but not too thin, nor too well-muscled. All in all, you're a good fit for championing your village's cause.");
			return new PlayerCreator("Lucina")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,

				//428347355782040	Character Creation	Female,wetness=wet, Looseness=normal,not a virgin, Fertility high i guess i dont really care can be up to you.	for her face normal human, ears i want Elvin, no tails, just normal skin, body thickness i want to be slender, body tone kinda athletic but not too much, hair i want really long i think like a 30 on the codex number i think and her hair color light blonde, i want her to have normal D size breast with you can choose how you want them really though i dont think i really care, nipple size i dont care, her skin color a fair light light color but not too pale, for her starting equipment i want im not sure what i want her to wear but basically i want a Elvin archer with a bow. so maybe you can do something about the clothing. i just want a Elvin character in the game since theres goblins plus another archer besides kelt a female one add to that.	Lucina
				vaginas = new VaginaCreator[] { new VaginaCreator(null, VaginalWetness.SLICK, VaginalLooseness.LOOSE, false) },
				femininity = 80,
				fertility = 40,
				earType = EarType.ELFIN,
				thickness = 25,
				tone = 60,
				hairLength = 30,
				hairColor = HairFurColors.LIGHT_BLONDE,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.D) },
				complexion = Tones.LIGHT
			};
			//Bow skill 100 (Sorry Kelt, I can't hear your insults over my mad Robin Hood skillz)
			//creator.createStatusEffect(StatusEffects.Kelt, 100, 0, 0, 0);
			//creator.createKeyItem("Bow", 0, 0, 0, 0);
		}

		private static SimpleDescriptor LukazFlavorText => SpecialCharacterStrings.LukazText;
		private static PlayerCreator customLukaz()
		{
			return new PlayerCreator("Lukaz")
			{
				defaultGender = Gender.MALE,
				forceDefaultGender = false,
				//Specific Character
				//Male. 11.5 inch dog dick, 4 balls, 2 inches in diameter.
				cocks = new CockCreator[] { new CockCreator(CockType.DOG, 11.5, 2, 1.5) },
				cockVirgin = false,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT) },
				heightInInches = 71,
				hipSize = 4,
				buttSize = 4,
				femininity = 30,
				numBalls = 4,
				cumMultiplier = 4,
				ballSize = 2,
				strength = 18,
				toughness = 17,
				speed = 15,
				intelligence = 15,
				sensitivity = 15,
				libido = 15,
				corruption = 0,
				hairLength = 1,
				complexion = Tones.LIGHT,
				hairColor = HairFurColors.BROWN,
				thickness = 50,
				fertility = 5,
				//"dog face, dog ears, draconic tail, blue fur.
				faceType = FaceType.DOG,
				earType = EarType.DOG,
				tailType = TailType.DRACONIC,
				bodyType = BodyType.SIMPLE_FUR,
				furColor = new FurColor(HairFurColors.BLUE),
				tone = 88,
				tongueType = TongueType.DRACONIC,

				perks = new List<StandardPerk>()
				{
					new Fighter(),
					new MessyOrgasms(),
				},
			};
			//gel plate armor, warhammer, 88 body tone, 1 breast row, flat manly breasts, 0.2 inch nipples, 1 on each breast, draconic tongue, short hair-blue, light skin."	Lukaz
			//creator.createPerk(PerkLib.HistoryFighter, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.MessyOrgasms, 1.25, 0, 0, 0);
		}

		private static SimpleDescriptor MaraFlavorText => SpecialCharacterStrings.MaraText;
		private static PlayerCreator customMara()
		{
			return new PlayerCreator("Mara")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,

				//#226096893686530
				//For the custom PC Profile can you make a Bimbo Bunny girl (no bunny feet) (named Mara) dont really care about clothes i can get what i want pretty quickly and I change from time to time.
				//sb.Append("You're a bunny-girl with bimbo-tier curves, jiggly and soft, a curvy, wet girl with a bit of a flirty past.");
				speed = 18,
				intelligence = 17,
				tone = 20,
				fertility = 10,
				hairLength = 15,
				vaginas = new VaginaCreator[] { new VaginaCreator(0.5, VaginalWetness.SLICK, VaginalLooseness.LOOSE, false) },
				heightInInches = 67,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.E) },
				hipSize = 12,
				buttSize = 12,
				femininity = 100,
				thickness = 33,
				assVirgin = false,
				earType = EarType.BUNNY,
				tailType = TailType.RABBIT,
				complexion = Tones.TAN,
				hairColor = HairFurColors.PLATINUM_BLONDE,

				perks = new List<StandardPerk>()
				{
					new Slut(),
					new BigTits(),
				},
			};

			//creator.createPerk(PerkLib.BimboBody, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.BimboBrains, 0, 0, 0, 0);
			//creator.teaseLevel = 3;
		}

		private static SimpleDescriptor MihariFlavorText => SpecialCharacterStrings.MihariText;
		private static PlayerCreator customMihari()
		{
			//sb.Append("The portal is not something you fear, not with your imposing armor and inscribed spellblade. You're much faster and stronger than every champion that came before you, but will it be enough?");
			return new PlayerCreator("Mihari")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,

				//[Values will be listed as if taken from Minerva]
				//I'm kinda going under the assumption you are letting us go hog wild if not, take what's allowed and do what you wish out of what's below
				//Core Stats:
				strength = 40,
				toughness = 20,
				speed = 100,
				intelligence = 80,
				libido = 25,
				sensitivity = 15,

				//Body Values:
				//breastRows
				breasts = new BreastCreator[] { new BreastCreator(CupSize.DD, 0.5) },
				//-breastRating: 5
				//-breasts: 2
				//-nipplesPerBreast: 1
				buttSize = 2,
				vaginas = new VaginaCreator[] { new VaginaCreator(0.2, VaginalWetness.SLAVERING, VaginalLooseness.TIGHT, true) },
				earType = EarType.CAT,
				faceType = FaceType.CAT,
				femininity = 100,
				fertility = 85,
				hairColor = HairFurColors.BLONDE,
				hairLength = 24,
				hipSize = 6,
				lowerBodyType = LowerBodyType.CAT,

				complexion = Tones.ASHEN,
				bodyType = BodyType.SIMPLE_FUR,
				tailType = TailType.CAT,
				heightInInches = 55,
				thickness = 10,
				tone = 75,

				perks = new List<StandardPerk>()
				{
					new Fast(),
					new Fertile(),
					new Scholar(),
				},
			};

			//creator.teaseLevel = 4;
			//perks:
			//creator.createPerk(PerkLib.Agility, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Evade, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Runner, 0, 0, 0, 0);

			//creator.createPerk(PerkLib.Flexibility, 0, 0, 0, 0);
			//Posted everything above sorry if it wasn't supposed to go there.
			//starting equipment: black leather armor surrounded by voluminous robes
			//starting weapon: Spellblade if not gamebreaking otherwise spear is fine.
			//creator.setArmor(armors.LTHRROB);
			//creator.setWeapon(weapons.S_BLADE);
		}

		private static SimpleDescriptor MirvannaFlavorText => SpecialCharacterStrings.MirvannaText;
		private static PlayerCreator customMirvanna()
		{
			//sb.Append("You're an equine dragon-morph with a rather well-proportioned body. Ingnam is certainly going to miss having you whoring yourself out around town. You don't think they'll miss cleaning up all the messy sex, though.");
			return new PlayerCreator("Mirvanna")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = false,

				//Any equine or dragonny attributes accompanying it a big plus! As I'm a dragon-unicorn furry (Qilin~). Bonus points if you add a horn type for unicorn horn.
				speed = 18,
				intelligence = 17,
				strength = 18,
				fertility = 20,
				hairLength = 15,
				vaginas = new VaginaCreator[] { new VaginaCreator(VaginaType.EQUINE, 0.5, VaginalWetness.SLICK, VaginalLooseness.LOOSE, false) },
				assVirgin = false,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.DD) },
				cocks = new CockCreator[] { new CockCreator(CockType.HORSE, 14, 2.5) },
				cockVirgin = false,
				heightInInches = 73,
				tone = 20,
				hipSize = 8,
				buttSize = 8,
				femininity = 73,
				thickness = 33,
				hairColor = HairFurColors.PLATINUM_BLONDE,
				//Mirvanna;
				//Gender = Herm
				//Ears = Horse
				earType = EarType.HORSE,
				//we'll give you a unicorn horn over dragon, you're welcome i guess.
				//Horns = Dragon
				hornType = HornType.UNICORN,
				additionalHornTransformStrength = 0,

				//hornType = HornType.DRACONIC,
				//hornCount = 4,
				//hornSize = 12,
				//Face = Horse
				faceType = FaceType.HORSE,
				//Skin type = Black Fur
				complexion = Tones.BROWN,
				bodyType = BodyType.SIMPLE_FUR,
				furColor = new FurColor(HairFurColors.BLACK),
				//Legs/Feet = Digigrade hooved
				lowerBodyType = LowerBodyType.HOOVED,
				//Wing type = Dragon
				wingType = WingType.DRACONIC,
				largeWings = true,
				//Tail type = Dragon
				tailType = TailType.DRACONIC,

				cumMultiplier = 5.5,

				//Herm, lots of jizz.
				perks = new List<StandardPerk>()
				{
					new MessyOrgasms(),
					new Whore(),
				},
			};
			//creator.teaseLevel = 1;
			//Beautiful Sword & Wizard Robe
			//creator.setArmor(armors.W_ROBES);
		}

		private static SimpleDescriptor NamiFlavorText => SpecialCharacterStrings.NamiText;
		private static PlayerCreator customNami()
		{
			return new PlayerCreator("Nami")
			{
				defaultGender = Gender.FEMALE, //tempted to make this genderless, as it'd fit.
				forceDefaultGender = false,
				//Female with the sand-trap black pussy
				//Non-Virgin
				//Fertility- Normal Starting Value
				//Wetness- Above Average
				//Looseness- Normal Starting Value
				//Clit-size- Normal Value"
				vaginas = new VaginaCreator[] { new VaginaCreator(VaginaType.SAND_TRAP, 0.25, VaginalWetness.SLICK, VaginalLooseness.NORMAL, false) },
				analLooseness = AnalLooseness.NORMAL,
				assVirgin = false,
				//Face- Canine
				faceType = FaceType.DOG,
				//Ears- Canine
				earType = EarType.DOG,
				//Tail- Canine
				tailType = TailType.DOG,
				//Lower body- Canine
				lowerBodyType = LowerBodyType.DOG,
				//White Fur (if possible)
				bodyType = BodyType.SIMPLE_FUR,
				furColor = new FurColor(HairFurColors.WHITE),
				//Body Thickness/breastsize/- As if I had selected the ""Average"" body type from the start.
				breasts = new BreastCreator[] { new BreastCreator(CupSize.B) },
				//Muscle Tone- A bit above average enough to trigger a mention of it in the desc.
				tone = 55,
				//Nipples- As above on size but the black sand trap nipples.
				blackNipples = true,
				//Hair Length- Long
				hairLength = 16,
				//Hair Color- Black
				hairColor = HairFurColors.BLACK,
				//Skin Color- Light
				complexion = Tones.LIGHT,

				heightInInches = 64,
				femininity = 75,
				buttSize = 7,
				hipSize = 7,
				intelligence = 40,
				strength = 20,
				speed = 25,
				toughness = 15,

				perks = new List<StandardPerk>()
				{
					////Gift Perk- Smarts
					////History- Schooling
					new Smart(),
					new Scholar(),
				}
			};

			////Starting Equipment: Wizard's Robe, Wizards Staff, and one White and one Black book in inventory.
			////equipArmor("inquisitor's corset",false);
			//creator.setArmor(armors.W_ROBES);

			//creator.setWeapon(weapons.W_STAFF);

			//creator.itemSlot1.setItemAndQty(consumables.W__BOOK, 1);
			//creator.itemSlot2.setItemAndQty(consumables.B__BOOK, 1);
			//StringBuilder sb = new StringBuilder();
			//sb.Append("Your exotic appearance caused you some trouble growing up, but you buried your nose in books until it came time to go through the portal.");
		}

		private static SimpleDescriptor NavornFlavorText => SpecialCharacterStrings.NavornText;
		private static PlayerCreator customNavorn()
		{
			BreastCreator newBreastRow() => new BreastCreator(CupSize.D, 4.0);
			CockCreator newCock(CockType type) => new CockCreator(type, 15, 3, 2); //knot is ignored if invalid, so i can be lazy and use this for all of them.
			return new PlayerCreator("Navorn")
			{
				//sb.Append("There's been something special about you since day one, whether it's your numerous sexual endowments or your supernatural abilities. You're a natural pick for champion.");
				//Character Creation	"Herm same number and types of cocks from email sent earlier.
				defaultGender = Gender.HERM,
				forceDefaultGender = false,

				strength = 41,
				//femininity: 95
				femininity = 95,
				//(0 lust cum production: 10000)
				cumMultiplier = 500,
				//(base fertility 20 if possible?)
				fertility = 20,
				//Appearence: 7t 9in tall covered in thick shining silver fur, has a vulpine head and ears, eight breast all the same size at DD, dragon like wings, tail, and legs. With a large mare like pussy, 6 dicks, two equine, two dragon, two vulpine, all 15in long and 3 in wide, and four nuts 5 in across
				heightInInches = 93,
				complexion = Tones.BLACK,
				bodyType = BodyType.SIMPLE_FUR,
				hairColor = HairFurColors.SILVER,
				furColor = new FurColor(HairFurColors.SILVER),
				faceType = FaceType.FOX,
				earType = EarType.FOX,

				breasts = new BreastCreator[] { newBreastRow(), newBreastRow(), newBreastRow(), newBreastRow() },
				nippleStatus = NippleStatus.FUCKABLE,

				cocks = new CockCreator[] { newCock(CockType.HORSE), newCock(CockType.HORSE), newCock(CockType.DOG), newCock(CockType.DOG), newCock(CockType.DRAGON), newCock(CockType.DRAGON) },

				numBalls = 4,
				ballSize = 5,
				//hair length: 45 in (waist length for 7'9")
				hairLength = 45,
				//hip size: 15/20
				hipSize = 15,
				//butt size: 15/20
				buttSize = 15,
				//body thickness: 50/100
				thickness = 50,
				//Muscle: 75/100"
				tone = 75,
				//for wetness a squirter, looseness a 2 and capacity at 140.
				vaginas = new VaginaCreator[] { new VaginaCreator(vaginalWetness: VaginalWetness.SLAVERING, vaginalLooseness: VaginalLooseness.LOOSE, isVirgin: true) },
				//Virgin, high fertility like in the email I sent before. dragon wings, nine fox tails, dragon legs, eight DD breasts with four fuckable nipples each, dragon tongue, waist length hair, large dragon wings.
				wingType = WingType.DRACONIC,
				largeWings = true,
				tailType = TailType.FOX,
				tailCount = 9,
				lowerBodyType = LowerBodyType.DRAGON,
				tongueType = TongueType.DRACONIC,
			};

			//creator.createStatusEffect(StatusEffects.BonusVCapacity, 132, 0, 0, 0);
			//creator.createPerk(PerkLib.EnlightenedNinetails, 0, 0, 0, 0);
			////Special abilities: Fire breath, fox fire?
			//creator.createPerk(PerkLib.Dragonfire, 0, 0, 0, 0);
			////equipment: Large claymore, and platemail
			////-Chainmail armor
			//creator.setArmor(armors.FULLPLT);
			////-Large Claymore (i understand 40 Strength is need so if he could start with that would be great if not hit the gyms)"
			//creator.setWeapon(weapons.CLAYMR0);
		}

		private static SimpleDescriptor NixiFlavorText => SpecialCharacterStrings.NixiText;
		private static PlayerCreator customNixi()
		{
			return new PlayerCreator("Nixi")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,

				//some starting gems (just go ahead and surprise me on the amount)
				gems = Utils.Rand(800),
				//Specific Character
				//-Female... with a dog cock
				//11"" long, 2"" wide, 2.4"" knot
				//no balls
				//virgin pussy, 0.2"" clit
				//wetness 2
				vaginas = new VaginaCreator[] { new VaginaCreator(0.2, VaginalWetness.WET) },
				//fertility 30
				//virgin bum
				analLooseness = AnalLooseness.NORMAL,
				assVirgin = true,
				//anal wetness 1
				analWetness = AnalWetness.DAMP,
				cocks = new CockCreator[] { new CockCreator(CockType.DOG, 11, 2, 1.2) },
				numBalls = 0,
				ballSize = 0,

				//1 pair DD's, 0.5"" nipples"
				breasts = new BreastCreator[] { new BreastCreator(CupSize.DD, 0.5) },

				fertility = 30,
				hipSize = 6,
				buttSize = 6,
				strength = 15,
				toughness = 15,
				speed = 18,
				intelligence = 17,
				sensitivity = 15,
				libido = 15,
				corruption = 0,

				femininity = 85,
				//75 muscle tone
				tone = 75,
				//25 thickness
				thickness = 25,
				bodyType = BodyType.SIMPLE_FUR,
				complexion = Tones.LIGHT,
				furColor = new FurColor(HairFurColors.WHITE),
				hairColor = HairFurColors.SILVER_WHITE,
				hairLength = 10,
				//shoulder length silver hair

				cumMultiplier = 1,
				heightInInches = 82,
				//6' 10"" german-shepherd morph, face ears hands feet tail, the whole nine yards
				faceType = FaceType.DOG,
				lowerBodyType = LowerBodyType.DOG,
				tailType = TailType.DOG,
				earType = EarType.DOG,

				//fertility AND messy orgasm (hope that's not pushing it)
				//fighting history
				perks = new List<StandardPerk>()
				{
					new Fertile(),
					new MessyOrgasms(),
					new Fighter(),
				},

			};
			////"	"I'm picturing a tall, feminine German-Shepherd morph, solid white and gorgeous. She has both sets of genitals, with no balls, and a large set of breasts. She wields a large claymore and is dressed in a full chain vest and pants.
			////large claymore (and the strength to use it)
			//creator.setWeapon(weapons.CLAYMR0);
			//creator.strength = 40;
			////full chain
			//creator.setArmor(armors.FULLCHN);

			////3 starting perk points
			//creator.perkPoints = 3;
			//sb.Append("As a German-Shepherd morph, the rest of the village never really knew what to do with you... until they sent you through the portal to face whatever's on the other side...");
		}

		private static SimpleDescriptor PrismereFlavorText => SpecialCharacterStrings.PrismereText;
		private static PlayerCreator customPrismere()
		{
			//sb.Append("You're more of a scout than a fighter, but you still feel confident you can handle your responsibilities as champion. After all, what's to worry about when you can outrun everything you encounter? You have olive skin, deep red hair, and a demonic tail and wings to blend in with the locals.");
			return new PlayerCreator("Prismere")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,

				//Specific Character	Female, virgin, high fertility, tight with standard wetness and clit.
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				fertility = 14,
				speed = 35,
				//In the human world, Prismere began as a scout, helping patrol areas with portals to make sure demonspawn and corruption didn't reach the human homeland. She's gotten herself into a few tight spots because of it, but she's hard to keep pinned down. She has a fiance back in her village whom she fully intends to get back to, so her libido isn't especially high.
				//As of the time the PC takes her on, she has some signs of demonic taint, so Corruption might start at 5 to 10 points."	"Breasts at E, height at 5'0, a curvy build with a more narrow waist and substantial hips and butt. Skin is olive, like a mocha, hair is long and wildly wavy, a deep red, and eyes are a stormy blue. Muscles are barely visible; what muscle she has is the lean build of a runner, not a fighter. Nipples aren't especially long, but more soft.
				heightInInches = 60,
				corruption = 10,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.E) },
				leftEyeColor = EyeColor.BLUE,
				hipSize = 8,
				buttSize = 8,
				thickness = 25,
				tone = 40,
				complexion = Tones.OLIVE,
				hairLength = 30,
				hairColor = HairFurColors.DEEP_RED,
				femininity = 90,
				//She has a demonic tail and small demonic wings thanks to some encounters early on with succubus milk (that stuff is delicious!) but is otherwise still human.
				wingType = WingType.BAT_LIKE,
				largeWings = false,
				tailType = TailType.DEMONIC,

				perks = new List<StandardPerk>()
				{
					new Fast(),
					//i interpreted the initially corrupt as this. sorry if that's not what you want.
					new Pervert(),
				}
				//creator.createPerk(PerkLib.Fast, 0.25, 0, 0, 0);
			};
			////I feel really weird talking about all this, so if there's anything you need to change or can't do, or if I totally misinterpreted this, just shoot me an email! jordie.wierenga@gmail.com . Thanks in advance... I'm a big fan. "	Prismere
			////Perk is speed, she was a scout, and it'd be neat (if possible) to give her something akin to the Runner perk. She might not start out very strong or tough, but at least she's fast.
			//creator.createPerk(PerkLib.Runner, 0, 0, 0, 0);
		}

		private static SimpleDescriptor RannRaylaFlavorText => SpecialCharacterStrings.RannRaylaText;
		private static PlayerCreator customRannRayla()
		{
			//sb.Append("You're a young, fiery redhead who\'s utterly feminine. You've got C-cup breasts and long red hair. Being a champion can\'t be that bad, right?");
			//Specific Character	Virgin female.	Max femininity. Thin with a little muscle. Size C breasts. Long red hair. Light colored skin. 5'5" tall. 	Rann Rayla
			return new PlayerCreator("Rann Rayla")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,

				vaginas = new VaginaCreator[] { new VaginaCreator(0.25) },
				breasts = new BreastCreator[] { new BreastCreator(CupSize.C, 0.5) },
				hairLength = 22,
				hairColor = HairFurColors.DEEP_RED,
				complexion = Tones.LIGHT,
				femininity = 100,
				thickness = 25,
				tone = 65,
				heightInInches = 65,

				perks = new List<StandardPerk>()
				{
					new Slacker(),
				},
			};
		}

		private static SimpleDescriptor RopeFlavorText => SpecialCharacterStrings.RopeText;
		private static PlayerCreator customRope()
		{
			//sb.Append("Despite outward appearances, you're actually something of a neuter, with shark-like teeth, an androgynous face, and a complete lack of genitalia.");
			return new PlayerCreator("Rope")
			{
				//529315025394020	Character Creation	Neuter (no genitals) "50-50 masculine-feminine ratio. Shark teeth."	Rope
				defaultGender = Gender.GENDERLESS,
				forceDefaultGender = true,
				femininity = 50,
				faceType = FaceType.SHARK
			};

		}

		private static SimpleDescriptor SeraFlavorText => SpecialCharacterStrings.SeraText;
		private static PlayerCreator customSera()
		{

			//sb.Append("You're something of a shemale - three rows of C-cup breasts matched with three, plump, juicy cocks. Some decent sized balls, bat wings, and cat-like ears round out the package.");

			return new PlayerCreator("Sera")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,

				toughness = 17,
				strength = 18,
				fertility = 5,
				//Hair: very long white
				hairLength = 43,
				hairColor = HairFurColors.WHITE,
				//Complexion: light
				complexion = Tones.LIGHT,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.C, 0.2), new BreastCreator(CupSize.C, 0.2), new BreastCreator(CupSize.C, 0.2) },
				cocks = new CockCreator[] { new CockCreator(8, 1.6), new CockCreator(8, 1.6), new CockCreator(8, 1.6) },
				numBalls = 2,
				ballSize = 3,
				//9 foot 5 inches tall
				heightInInches = 113,
				//Build: average
				tone = 50,
				thickness = 50,
				femininity = 50,
				hipSize = 5,
				buttSize = 5,
				//Apperance: Cat Ears, Large Bat Like Wings, 3 Rows of breasts (C cub, 0,2 nipples)
				earType = EarType.CAT,
				wingType = WingType.BAT_LIKE,
				largeWings = true,
				cumMultiplier = 5.5,
				//Gift: Lotz of Jizz
				//History: Schooling
				perks = new List<StandardPerk>()
				{
					new Scholar(),
					new MessyOrgasms(),
				},
			};
			//creator.teaseLevel = 1
			////Items: Katana, Leather Armor
			//creator.setWeapon(weapons.KATANA0);
			//creator.setArmor(armors.URTALTA);
			////Key Item: Deluxe Dildo
			//creator.createKeyItem("Deluxe Dildo", 0, 0, 0, 0);
		}

		private static SimpleDescriptor SiveenFlavorText => SpecialCharacterStrings.SiveenText;
		private static PlayerCreator customSiveen()
		{
			//sb.Append("You are a literal angel from beyond, and you take the place of a vilage's champion for your own reasons...");
			return new PlayerCreator("Siveen")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,

				//Female
				//Virgin
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				//has a self-repairing hymen in her cunt"	"Angel
				//(means feathered wings on her back)
				wingType = WingType.FEATHERED,
				wingFeatherColor = HairFurColors.CHARCOAL,
				largeWings = true,
				//Halo (Flaming)
				//D-cups
				//7"" nips
				breasts = new BreastCreator[] { new BreastCreator(CupSize.D, 7) },
				nippleStatus = NippleStatus.FUCKABLE,
				//human skin
				//heart-shaped ass
				buttSize = 9,
				hipSize = 6,
				//Ass-length white and black hair
				hairLength = 40,
				hairColor = HairFurColors.WHITE,
				hairHighlightColor = HairFurColors.BLACK,
				//heterochromia (one blue eye one red eye)
				leftEyeColor = EyeColor.BLUE,
				rightEyeColor = EyeColor.RED,
				//waif thin body
				thickness = 0,
				//Fallen Angel gear (complete with flaming sword and light arrows)
				//dark skin tone
				complexion = Tones.DARK,

				//Elfin ears
				earType = EarType.ELFIN,
				//tight asshole
				//human tongue
				//human face
				strength = 25,
				toughness = 25,
				intelligence = 25,
				speed = 25
			};
			//no tail, fur, or scales"
			//creator.setWeapon(weapons.S_BLADE);
			//flags[kFLAGS.HISTORY_PERK_SELECTED] = 0;
		}

		private static SimpleDescriptor SoraFlavorText => SpecialCharacterStrings.SoraText;
		private static PlayerCreator customSora()
		{
			//sb.Append("As a Kitsune, you always got weird looks, but none could doubt your affinity for magic...");
			return new PlayerCreator("Sora")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,

				//Character Creation	Female,virgin	A kitsune with a snake-like tongue	Sora
				vaginas = new VaginaCreator[] { new VaginaCreator() },
				cocks = new CockCreator[] { new CockCreator(CockType.DOG) },
				tongueType = TongueType.SNAKE,
				earType = EarType.FOX,
				tailType = TailType.FOX,
				tailCount = 2,
				intelligence = 30,
				nippleStatus = NippleStatus.FULLY_INVERTED
			};
		}
#if DEBUG
		//Debug character. not in final game. feel free to populate this with whatever you need tested.
		private static SimpleDescriptor TestCharFlavorText => () => "Testing Character. You should never see this!";
		private static PlayerCreator customTestChar()
		{
			return new PlayerCreator("TestChar")
			{
			};
		}
#endif

		private static SimpleDescriptor TyrianaFlavorText => SpecialCharacterStrings.TyrianaText;
		private static PlayerCreator customTyriana()
		{
			return new PlayerCreator("Tyriana")
			{
				//"Gender: Female
				defaultGender = Gender.FEMALE,
				forceDefaultGender = true,
				//Vagina: Ridiculously loose, 3 inch clitoris, dripping constantly, fertile like a bunny on steroids and non-virgin
				vaginas = new VaginaCreator[] { new VaginaCreator(3, VaginalWetness.DROOLING, VaginalLooseness.CLOWN_CAR_WIDE, false) },
				fertility = 50,
				//Butt: Just as loose
				analLooseness = AnalLooseness.GAPING,
				assVirgin = false,
				//"Skin: Tanned
				complexion = Tones.TAN,
				//Hair: Ridiculously long red
				hairLength = 80,
				hairColor = HairFurColors.RED,
				//Face: Gorgeous Feminine, long demonic tongue, cat ears
				femininity = 100,
				tongueType = TongueType.DEMONIC,
				//added a tongue piercing because seemed relevant to demon tongue and whorish nature, idk.
				tonguePiercings = new PiercingData<TonguePiercingLocation>()
				{
					[TonguePiercingLocation.MIDDLE] = new PiercingJewelry(JewelryType.BARBELL_STUD, new Steel(), true)
				},
				earType = EarType.CAT,
				//Body: Very muscular, average weight, plump ass, above average thighs, cat tail and cat paws
				tone = 80,
				thickness = 50,
				buttSize = 12,
				hipSize = 10,
				tailType = TailType.CAT,
				lowerBodyType = LowerBodyType.CAT,
				//Breasts: 2 E-cups on top, 2 DD-cups mid, 2 D-cups bottom, 3.5 inch nipples
				breasts = new BreastCreator[] { new BreastCreator(CupSize.E, 3.5), new BreastCreator(CupSize.DD_BIG, 3.5), new BreastCreator(CupSize.D, 3.5) },
				nippleStatus = NippleStatus.FUCKABLE,
				heightInInches = 67,
				//Perks: Slut and Fertile"

				speed = 18,
				intelligence = 17,

				perks = new List<StandardPerk>()
				{
					new Slut(),
					new Fertile(),
				},
			};

			//creator.teaseLevel = 3;
		}

		//this champ is so friggin overpowered holy shit.

		private static SimpleDescriptor VahdunbriiFlavorText => SpecialCharacterStrings.VahdunbriiText;
		private static PlayerCreator customVahdunbrii()
		{
			//sb.Append("You're something of a powerhouse, and you wager that between your odd mutations, power strong enough to threaten the village order, and talents, you're the natural choice to send through the portal.");
			return new PlayerCreator("Vahdunbrii")
			{
				defaultGender = Gender.FEMALE,
				forceDefaultGender = false,
				breasts = new BreastCreator[] { new BreastCreator(CupSize.C) },
				vaginas = new VaginaCreator[] { new VaginaCreator(0.5, VaginalWetness.WET) },
				cocks = new CockCreator[] { new CockCreator(8.5, 1.5) },
				fertility = 10,
				hipSize = 7,
				buttSize = 6,
				strength = 19,
				toughness = 19,
				speed = 18,
				intelligence = 17,
				sensitivity = 15,
				libido = 15,
				corruption = 0,
				femininity = 85,
				tone = 98,
				thickness = 4,
				numBalls = 0,
				ballSize = 0,
				cumMultiplier = 1,
				heightInInches = 68,
				earType = EarType.CAT,
				lowerBodyType = LowerBodyType.CAT,
				tailType = TailType.CAT,
				wingType = WingType.FEATHERED,
				largeWings = true,
				armType = ArmType.HARPY,
				hornType = HornType.DRACONIC,
				additionalHornTransformStrength = 1,
				faceType = FaceType.SPIDER,
				isFaceFullMorph = false,
				hairLength = 69,
				hairColor = HairFurColors.DARK_BLUE,
				hairType = HairType.NORMAL,
				hairTransparent = true,
				skinTexture = SkinTexture.SMOOTH,
				complexion = Tones.SANGUINE,
				gems = 300,

				perks = new List<StandardPerk>()
				{
					new Scholar(),
					new Religious(),

					new WetPussy(),
					new Tough(),
					new Strong(),
					new Fast(),
					new Smart(),

				}
			};

			//creator.createPerk(PerkLib.Incorporeality, 0, 0, 0, 0);
			////Beautiful Sword
			//creator.setWeapon(weapons.B_SWORD);
			//creator.setArmor(armors.SSARMOR);
			////Bow skill 100 (Sorry Kelt, I can't hear your insults over my mad Robin Hood skillz)
			//creator.createStatusEffect(StatusEffects.Kelt, 100, 0, 0, 0);
			//creator.createKeyItem("Bow", 0, 0, 0, 0);
			//inventory.createStorage();
			//inventory.createStorage();
			//inventory.createStorage();
			//inventory.createStorage();
			//inventory.createStorage();
			//inventory.createStorage();
			//creator.createKeyItem("Camp - Chest", 0, 0, 0, 0);
			//creator.createKeyItem("Equipment Rack - Weapons", 0, 0, 0, 0);
			//creator.createKeyItem("Equipment Rack - Armor", 0, 0, 0, 0);
			////(Flexibility), (Incorporeality), History: Religious, Dragonfire, Brood Mother, Magical Fertility, Wet Pussy, Tough, Strong, Fast, Smart, History: Scholar, History: Slacker, Strong Back, Strong Back 2: Stronger Harder
			//creator.createPerk(PerkLib.Flexibility, 0, 0, 0, 0);

			//creator.createPerk(PerkLib.Dragonfire, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.BroodMother, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.Fertile, 1.5, 0, 0, 0);
			//creator.vaginas[0].vaginalWetness = Vagina.WETNESS_WET;

			//creator.createPerk(PerkLib.StrongBack, 0, 0, 0, 0);
			//creator.itemSlot4.unlocked = true;
			//creator.itemSlot5.unlocked = true;
			//creator.createPerk(PerkLib.StrongBack2, 0, 0, 0, 0);
			//creator.createPerk(PerkLib.HistorySlacker, 0, 0, 0, 0);
		}

		//private static SimpleDescriptor Kitteh6660FlavorText => SpecialCharacterStrings.Kitteh6660Text;
		//private static PlayerCreator customKitteh6660()
		//{
		//	PlayerCreator creator = new PlayerCreator("Kitteh6660"); //Not yet implemented.
		//}

		private static SimpleDescriptor EtisFlavorText => SpecialCharacterStrings.EtisText;
		private static PlayerCreator customEtis()
		{
			Dictionary<EarPiercingLocation, PiercingJewelry> makeEarPiercings(params EarPiercingLocation[] locations)
			{
				Dictionary<EarPiercingLocation, PiercingJewelry> retVal = new Dictionary<EarPiercingLocation, PiercingJewelry>();
				foreach (var location in locations)
				{
					retVal.Add(location, new PiercingJewelry(JewelryType.RING, new Crimstone(), true));
				}
				return retVal;
			}
			//sb.Append("You are young (by kitsune measure) but very talented.");
			//sb.Append("\n\nFormer priestess, you abandoned religion and resorted to your hobby - alchemy.");
			//sb.Append("\n\nYou tried to improve your body with controlled transformations, and mostly successful. Now you are half-dragon, and while most changes are hidden inside your body, you still possess magnificent wings, imposing horns and incredibly long, prehensile tongue. Otherwise, your body is mostly what you would expect from kitsune - cute, graceful and having a capacity, straightforward impossible for your thin and small 4-foot frame.");
			//sb.Append("\n\nWith both religious and alchemical training, you are a skilled healer, able to treat wounds and poisonings alike. Your kitsune's trickster nature and pervert inclinations are making you susceptible to corruption, but at the same time, your enlightenment keeps you from actually turning into a demon, so corruption barely affects your mind. Even with your willpower and religious training you sometimes struggle to restrain your impulse, and you always are ready for something very lewd. With your knowledge of healing and modified body, it is easy for you to enjoy things which would be painful for others, and you are always ready to return a favor. Still, you tend to care about mutual enjoyment - there is a difference between extreme entertainment and torture, and you are mischievous, not evil. Natural gift at magic, extraordinary even by kitsune's measures and complimented by nine tails, makes you able to cast spells without exhausting yourself even without an enchanted robe. Sadly, twisted energies of the new world would make you knowledge almost useless, so you only can use fox fire as an offensive spell.");
			//sb.Append("\n\nYour experiments left some strange effects within your body. Some are nice (you have unusually fast regeneration), some are strange (you can shift to incorporeality for a few seconds and even try to possess someone while in this state).");
			//sb.Append("\n\nAlso, you are an almost compulsive hoarder, so you developed an ability to carry and store a vast amount of things.");
			//sb.Append("\n\nNow you want to give your body thorough test run, and portal to demon infested world looks appealing. No one said that common sense is one of your strong sides...");
			//sb.Append("\n\nDespite your comfidence in your transformations, the one you still haven't landed on is your gender, though you currently have just enough materials to make whatever you decide stick.);
			return new PlayerCreator("Etis")
			{
				// Herm kitsune-dragon.
				defaultGender = Gender.HERM,
				forceDefaultGender = false,

				fertility = 5,
				vaginas = new VaginaCreator[] { new VaginaCreator(0.3, VaginalWetness.WET, VaginalLooseness.NORMAL, false) },
				breasts = new BreastCreator[] { new BreastCreator(CupSize.A, 0.5) },
				nippleStatus = NippleStatus.FULLY_INVERTED,

				cumMultiplier = 500, // don't ask how it works, copyright for this potion was sold for Joey

				analWetness = AnalWetness.MOIST, // moist
				analLooseness = AnalLooseness.NORMAL, // not virgin
				assVirgin = false,

				cocks = new CockCreator[] { new CockCreator(CockType.TENTACLE, 12, 1.3, 1.5), new CockCreator(CockType.TENTACLE, 12, 1.3, 1.5), new CockCreator(CockType.TENTACLE, 12, 1.3, 1.5) },

				heightInInches = 48, // 120 cm
				hipSize = Hips.BOYISH,
				buttSize = Butt.TIGHT,
				thickness = 20, // thin
				tone = 20, // soft
				bodyType = BodyType.KITSUNE,
				furColor = new FurColor(HairFurColors.SNOW_WHITE),
				furTexture = FurTexture.FLUFFY,
				complexion = Tones.LIGHT,
				underTone = Tones.LIGHT,
				hairColor = HairFurColors.SNOW_WHITE,
				hairType = HairType.ANEMONE,
				hairLength = 42, // obscenely long, you still can use ext serum to get more, but they would drag the floor, and this wouldn't be pleasant
				femininity = 55, // androgynous

				faceType = FaceType.FOX,
				earType = EarType.FOX,
				earPiercings = makeEarPiercings
				(
					EarPiercingLocation.LEFT_LOBE_1, EarPiercingLocation.LEFT_LOBE_2, EarPiercingLocation.LEFT_UPPER_LOBE,
					EarPiercingLocation.RIGHT_LOBE_1, EarPiercingLocation.RIGHT_LOBE_3, EarPiercingLocation.RIGHT_HELIX_1
				),
				armType = ArmType.FOX,
				eyeType = EyeType.DRAGON,
				lowerBodyType = LowerBodyType.FOX,
				tailType = TailType.FOX, // soft fur feels so lovely...
				tailCount = 9,
				tongueType = TongueType.DRACONIC, // tongue as long as your whole body height! almost tentackle! and so much fun to use!
				hornType = HornType.DRACONIC, // draconic horns adds to your exotic look, counts towards dragon score and keeps your tentacle hair out of your face! and your partners can use them as handles on occasions, letting your delicate ears uncrumpled!
				hornCount = 8,
				wingType = WingType.DRACONIC, // wings! to fly!
				largeWings = true,
				primaryWingTone = Tones.WHITE,
				secondaryWingTone = Tones.WHITE,

				strength = 5, // strength? not a kitsune way, besides, you are small and really neglected physical training
				toughness = 15, // still, your dragon blood makes you surprisingly tough for your size and condition
				speed = 20, // can take some advantage from small frame
				intelligence = 70, // your mind is your power!
				libido = 100, // yes, you have problems
				corruption = 41, // have high initial corruption, but also have religious history to meditate

				gems = 20,

				perks = new List<StandardPerk>()
				{
					new Religious(), // abandoned religion after obtaining nine tails
					new AlchemicalMastery(), // and resorted to your hobby - alchemy
					new Fast(),
					new Smart(),
					new Lusty(),
					new Pervert(),
					new Healer(), // with religious and alchemical knowledge you are skilled healer
				}
			};

			//creator.createStatusEffect(StatusEffects.BonusACapacity, 100, 0, 0, 0);
			//// almost compulsive hoarder, start with backpack, chests and racks... never enough storage space
			//if (creator.createPerkIfNotHasPerk(PerkLib.StrongBack)) creator.itemSlot4.unlocked = true;
			//if (creator.createPerkIfNotHasPerk(PerkLib.StrongBack2)) creator.itemSlot5.unlocked = true;
			//if (creator.hasKeyItem("Backpack") < 0) creator.createKeyItem("Backpack", 5, 0, 0, 0);

			//// have lots of different traits


			//creator.createPerkIfNotHasPerk(PerkLib.EnlightenedNinetails);
			//creator.createPerkIfNotHasPerk(PerkLib.Medicine); // able to treat wounds and poisoning alike
			//creator.createPerkIfNotHasPerk(PerkLib.AscensionWisdom, 5); // learns quickly
			//creator.createPerkIfNotHasPerk(PerkLib.AscensionTolerance, 10); // but in the same time your enlightment keeps you from really turning to demon, so corruption level does not really affect you much
			//creator.createPerkIfNotHasPerk(PerkLib.HotBlooded, 20); // even with your willpower and religious training you sometimes struggling to restrain your impulse
			//creator.createPerkIfNotHasPerk(PerkLib.Masochist); // with your knowledge of healing and innatural body it is easy for you to enjoy things which would be really painful for others
			//creator.createPerkIfNotHasPerk(PerkLib.Sadist); // and you are always ready to return favor
			//creator.createPerkIfNotHasPerk(PerkLib.SensualLover); // still you tend to care about mutual enjoyment - there are difference between extreme entertainment and torture, and you are mischievous, not evil
			//creator.createPerkIfNotHasPerk(PerkLib.SpellcastingAffinity, 80); // very strong magic affinity, can even be effective as mage without robe
			//creator.createPerkIfNotHasPerk(PerkLib.Channeling); // despite strong magic affinity wasn't actually much interested in combat magic before, so only can use fox fire as offensive spell
			//creator.createPerkIfNotHasPerk(PerkLib.Spellpower);
			//// have some racial traits
			//creator.createPerkIfNotHasPerk(PerkLib.Dragonfire);
			//// some experiments with your body gave unusual results
			//creator.createPerkIfNotHasPerk(PerkLib.LustyRegeneration); // some of your experiments had nice returns
			//creator.createPerkIfNotHasPerk(PerkLib.Incorporeality); // some was... strange
			//														// Metamorph, Magic Metabolism and Puppeteer suggested perks would be also nice addition to character, but they are not implemented yet

			//creator.setArmor(armors.KIMONO);
			//creator.setWeapon(weapons.KATANA0);

			//creator.createPerk(PerkLib.PiercedCrimstone, 5, 0, 0, 0); // with Omnibus' Gift it would give 50 min lust... perfect!

			//creator.createStatusEffect(StatusEffects.KnowsHeal, 0, 0, 0, 0); // to compliment history
		}

		//	private static SimpleDescriptor ChimeraFlavorText => SpecialCharacterStrings.ChimeraText;
		//private static PlayerCreator customChimera()
		//	{

		//		PlayerCreator creator = new PlayerCreator("Chimera");
		//		creator.defaultGender = (Gender)Utils.Rand((byte)(Gender.HERM + 1));
		//		creator.forceDefaultGender = true;
		//		int randVal;
		//		creator.breasts = new BreastCreator[Utils.Rand(4) + 1];
		//		for (int x = 0; x < creator.breasts.Length; x++)
		//		{
		//			randVal = Utils.Rand(30);
		//			if (randVal < 21)
		//			{
		//				creator.breasts[x] = new VaginaCreator(randVal)
		//			}
		//		}

		//		CupSize cupSize = (CupSize)Utils.Rand((byte)(CupSize.DD + 1));

		//		creator.vaginas = new VaginaCreator(aginaType = )
		//		// This one is sort of challenge character. It is supposed to be mage, at least at start.
		//		// always one vagina with random stats
		//		creator.createVagina();
		//		creator.vaginas[0].vaginalLooseness = randValue<VaginalLooseness>(); // from tight to gaping
		//		creator.vaginas[0].vaginalWetness = rand(4) + 1; // from normal to slavering
		//		creator.vaginas[0].virgin = false;

		//		creator.setClitLength(rand(3) == 0 ? (rand(10) + 1) * 0.25 : 0.25); // from 0.25 to 2.5
		//		creator.fertility = (rand(5) + 1) * 5; // from 5 to 25 with 5 step

		//		// 1-4 breast rows, from flats to dd sized
		//		var row:Number = 0;
		//		do
		//		{
		//			creator.createBreastRow();
		//			var size:Number = 0;
		//			if (row == 0)
		//				creator.breastRows[row].breastRating = rand(6);
		//			else
		//				creator.breastRows[row].breastRating = creator.breastRows[row - 1].breastRating - rand(2); // lower rows are same size or one size smaller than upper
		//			row++;
		//		} while (row < 4 && rand(2) == 0 && creator.breastRows[0].breastRating > 1); // if last row was flat do not add more

		//		var tent:Boolean = rand(5) == 0; // 20% chance to have tentacle cluster
		//		var cocks:Number = rand(5) + (tent ? 4 : 2); // 2-6 mixed cocks
		//		var i:int;
		//		for (i = 0; i < cocks; i++)
		//		{
		//			creator.createCock();
		//			creator.cocks[i].length = Math.round((rand(220) / 10 + 3) * 10) / 10; // 3-25 inches
		//			creator.cocks[i].girth = Math.round((rand(creator.cocks[i].length) / 10 + 1) * 10) / 10;
		//			var type:Number = rand(90);
		//			if (tent)
		//				creator.cocks[i].cockType = CockTypesEnum.TENTACLE;
		//			else if (type < 25) // high chance
		//			else if (type < 30)
		//				creator.cocks[i].cockType = CockTypesEnum.HORSE;
		//			else if (type < 40)
		//			{
		//				creator.cocks[i].cockType = CockTypesEnum.DOG; // double chance, since it is fox one as well
		//				creator.cocks[i].knotMultiplier = 1.4;
		//			}
		//			else if (type < 45)
		//				creator.cocks[i].cockType = CockTypesEnum.DEMON;
		//			else if (type < 50)
		//				creator.cocks[i].cockType = CockTypesEnum.CAT;
		//			else if (type < 60)
		//				creator.cocks[i].cockType = CockTypesEnum.TENTACLE; // double chance, for no reason
		//			else if (type < 65)
		//				creator.cocks[i].cockType = CockTypesEnum.LIZARD;
		//			else if (type < 70)
		//				creator.cocks[i].cockType = CockTypesEnum.ANEMONE;
		//			else if (type < 75)
		//				creator.cocks[i].cockType = CockTypesEnum.KANGAROO;
		//			else if (type < 80)
		//			{
		//				creator.cocks[i].cockType = CockTypesEnum.DRAGON;
		//				creator.cocks[i].knotMultiplier = 1.3;
		//			}
		//			else if (type < 85)
		//			{
		//				creator.cocks[i].cockType = CockTypesEnum.DISPLACER;
		//				creator.cocks[i].knotMultiplier = 1.5;
		//			}
		//			else
		//				creator.cocks[i].cockType = CockTypesEnum.PIG;

		//			if (creator.cocks[i].knotMultiplier == 1 && rand(5) == 0)
		//				creator.cocks[i].knotMultiplier = 1.2 + rand(9) / 10.;
		//		}


		//		// 40% to have no balls, 40% to one pair, 20% to 2 pairs
		//		var balls:Number = rand(5);
		//		if (balls <= 1) { creator.numBalls = 0; }
		//		else if (balls <= 3) { creator.numBalls = 2; }
		//		else if (balls == 4) { creator.numBalls = 4; }
		//		if (creator.numBalls > 0) creator.ballSize = rand(4) + 1;
		//		creator.cumMultiplier = 5 + creator.ballSize * creator.numBalls * 2.5 + rand(25);

		//		creator.analLooseness = rand(3);
		//		creator.analWetness = rand(3) + 1;

		//		// lean build
		//		creator.heightInInches = 47 + rand(43); // 118-230 cm
		//		creator.hipSize = Hips.RATING_BOYISH;
		//		creator.buttSize = Butt.RATING_TIGHT;
		//		creator.thickness = rand(10) + 10; // lithe
		//		creator.tone = rand(10) + 10; // not in a good shape...
		//		creator.complexion = "light";
		//		creator.hairColor = "black";
		//		creator.hairLength = rand(50) + 5;
		//		creator.femininity = rand(30) + 35;

		//		// wrecked body and obsessed mind...
		//		creator.strength -= 15;
		//		creator.toughness -= 15;
		//		creator.speed -= 15;
		//		creator.intelligence += 60;
		//		//creator.sensitivity = 15;
		//		//creator.libido = 15;
		//		creator.corruption += 2;

		//		var skin:Number = rand(100);
		//		if (skin < 50)
		//		{
		//		else if (skin < 80)
		//			{
		//				creator.bodyType = Skin.FUR; // fur, 30%
		//		else if (skin < 95)
		//				{
		//					creator.bodyType = Skin.LIZARD_SCALES; // scales, 15%
		//		else{
		//						creator.bodyType = Skin.GOO; // goo, 5%
		//						creator.skin.adj = "slimy";
		//					}

		//					if (rand(3) != 0) // 2/3 to have human face
		//					else // totally random one
		//						creator.faceType = rand(20) + 1; // since it is not a enum, it is impossible to make it auto-ajust...

		//					if (creator.faceType == FaceType.SPIDER_FANGS && rand(2) == 0)
		//						creator.eyesType = Eyes.SPIDER; // 50% to have spider eyes with spider fangs
		//					else if (rand(20) == 0) // 5% for inhuman eyes otherwise
		//						if (rand(2) == 0)
		//							creator.eyesType = Eyes.SPIDER;
		//						else
		//							creator.eyesType = Eyes.BLACK_EYES_SAND_TRAP;

		//					if (creator.faceType == FaceType.HUMAN && rand(2) != 0) // if human face, 50% to have human ears
		//					else // totally random one
		//						creator.earType = rand(14) + 1; // since it is not a enum, it is impossible to make it auto-ajust...


		//					if (rand(2) != 0) // 50% to have human lower body
		//					else // totally random one
		//					{
		//						creator.lowerBodyType = rand(21) + 1; // since it is not a enum, it is impossible to make it auto-ajust...
		//						if (creator.lowerBodyType == 4)
		//						{
		//							creator.lowerBodyType = LowerBodyType.HOOVED;
		//							creator.lowerBody.legCount = 4;
		//						}
		//						else if (creator.lowerBodyType == LowerBodyType.DRIDER)
		//							creator.lowerBody.legCount = 8;
		//						else if (creator.lowerBodyType == LowerBodyType.NAGA || creator.lowerBodyType == LowerBodyType.GOO)
		//							creator.lowerBody.legCount = 1;
		//						else if (rand(15) == 0)
		//							creator.lowerBody.legCount = 4;
		//					}

		//					creator.tailType = rand(21); // always have totally random tail
		//					if (creator.tailType == TailType.SPIDER_SPINNERET || creator.tailType == TailType.BEE_STINGER)
		//					{ // insect abdomens comes with poison
		//						creator.tailCount = 5;
		//						if (creator.tailType == TailType.SPIDER_SPINNERET && rand(2) == 0)
		//							creator.createPerk(PerkLib.SpiderOvipositor, 0, 0, 0, 0); // spider abdomen have chance 50/50 to have ovipositor
		//					}

		//					// 70% normal tongue, 30% to non-human with even chances of every one
		//					if (rand(100) < 70)
		//					else
		//						creator.tongueType = randomChoice(TongueType.DEMONIC, TongueType.DRACONIC, TongueType.SNAKE);


		//					var horns:Number = rand(100); // 70% no horns, 30% to random
		//					if (horns < 70)
		//					else if (horns < 80)
		//					{
		//						creator.hornType = HornType.DEMON;
		//						creator.hornCount = (rand(4) + 1) * 2; // 1-4 pairs
		//					}
		//					else if (horns < 90)
		//					{
		//						creator.hornType = HornType.BULL_LIKE;
		//						creator.hornCount = 2;
		//					}
		//					else
		//					{
		//						creator.hornType = HornType.DRACONIC_X2;
		//						creator.hornCount = 2;
		//					}

		//					var wings:Number = rand(4); // always have wings to fly... small boon to make up for lack of fighting power
		//					if (wings == 0)
		//						creator.wingType = WingType.BAT_LIKE_LARGE;
		//					else if (wings == 1)
		//						creator.wingType = WingType.FEATHERED_LARGE;
		//					else if (wings == 2)
		//						creator.wingType = WingType.DRACONIC_LARGE;
		//					else
		//						creator.wingType = WingType.GIANT_DRAGONFLY;


		//					var arms:Number = rand(100); // if have harpy wings 33% chance to have harpy hands, otherwise 5% to have spider hands
		//					if (creator.wingType == WingType.FEATHERED_LARGE && rand(4) == 0)
		//						creator.armsType = ArmType.HARPY;
		//					else if (rand(20) == 0)
		//						creator.armsType = ArmType.SPIDER;
		//					else


		//					// 90% to have normal hair, even chances to have feathers, anemone or goo otherwise
		//					if (rand(100) < 90) creator.hairType = Hair.NORMAL;
		//					else creator.hairType = randomChoice(Hair.FEATHER, Hair.GOO, Hair.ANEMONE);

		//					// wizard staff and modest robes
		//					creator.setWeapon(weapons.W_STAFF);
		//					creator.setArmor(armors.M_ROBES);

		//					// have some perks from past
		//					creator.createPerk(PerkLib.Smart, 0.25, 0, 0, 0);
		//					creator.createPerk(PerkLib.HistoryScholar, 0, 0, 0, 0);
		//					creator.createPerk(PerkLib.HistoryAlchemist, 0, 0, 0, 0);
		//					creator.createPerk(PerkLib.Channeling, 0, 0, 0, 0);
		//					creator.createPerk(PerkLib.Mage, 0, 0, 0, 0);
		//					creator.createPerk(PerkLib.SpellcastingAffinity, 50, 0, 0, 0);

		//					// and knows white magic
		//					creator.createStatusEffect(StatusEffects.KnowsCharge, 0, 0, 0, 0);
		//					creator.createStatusEffect(StatusEffects.KnowsBlind, 0, 0, 0, 0);
		//					creator.createStatusEffect(StatusEffects.KnowsWhitefire, 0, 0, 0, 0);


		//					creator.gems += 15 + rand(55);

		//					sb.Append("Your body is wrecked by your own experiments with otherworldly transformation items, and now you have no more money to buy any more from smugglers... But you would make your body as strong as your will. Or die trying.");
		//				}

		//			}

		//		}

		//		*/
		private static SimpleDescriptor PeoneFlavorText => SpecialCharacterStrings.PeoneText;
		private static PlayerCreator customPeone()
		{
			return new PlayerCreator("Peone")
			{
				defaultGender = Gender.HERM,
				forceDefaultGender = true,

				//Herm. born female, was accidently given concentrated incubus draft instead of some potion to test, which shrunk tits, grew cock (no balls), and also grew clit slightly.

				//7 inch cock, relatively normal thickness, well used, with a steel frenum barbell at the top as a result of a drunken dare
				cocks = new CockCreator[]
				{
					new CockCreator(7, 1.2, null, cockJewelry:new PiercingData<CockPiercingLocation>(){
					[CockPiercingLocation.FRENUM_UPPER_1] = new PiercingJewelry(JewelryType.BARBELL_STUD, new Steel(), true) })
				},
				cockVirgin = false,

				//stretched butt with toys, but has never had anal sex.
				analLooseness = AnalLooseness.LOOSE,
				assVirgin = true,

				//no balls
				numBalls = 0,
				ballSize = 0,

				//strangely, clit grew slightly with achemical accident. more blood to the area, i guess.
				//again, stretched via toys, but never had vaginal sex.
				vaginas = new VaginaCreator[] { new VaginaCreator(1, VaginalWetness.WET, VaginalLooseness.LOOSE, true) },

				//formerly C Cup, shrunk by the accident. Let's make use of that new inverted option - slightly.
				breasts = new BreastCreator[] { new BreastCreator(CupSize.A, 1) },
				nippleStatus = NippleStatus.SLIGHTLY_INVERTED,

				//amber eyes.
				leftEyeColor = EyeColor.AMBER,
				//freckles? yes please!
				facialSkinTexture = SkinTexture.FRECKLED,

				//three studs in the upper part of left ear. rings in both lobes. an industrial bar in the right ear.
				earPiercings = new PiercingData<EarPiercingLocation>()
				{
					[EarPiercingLocation.LEFT_HELIX_1] = new PiercingJewelry(JewelryType.BARBELL_STUD, new Ruby(), true),
					[EarPiercingLocation.LEFT_HELIX_2] = new PiercingJewelry(JewelryType.BARBELL_STUD, new Sapphire(), true),
					[EarPiercingLocation.LEFT_HELIX_3] = new PiercingJewelry(JewelryType.BARBELL_STUD, new Emerald(), true),
					[EarPiercingLocation.LEFT_LOBE_1] = new PiercingJewelry(JewelryType.RING, new Obsidian(), true),
					[EarPiercingLocation.RIGHT_LOBE_1] = new PiercingJewelry(JewelryType.RING, new Obsidian(), true),
					[EarPiercingLocation.RIGHT_AURICAL_3] = new Industrial(new Steel()),
					[EarPiercingLocation.RIGHT_ANTI_HELIX] = new Industrial(new Steel())
				},

				//hair: neck length, dirty blonde or brown with red streaks. But as it's kinda curly/wavy, it poofs up a bit
				hairColor = HairFurColors.SANDY_BROWN,
				hairHighlightColor = HairFurColors.RED,
				hairLength = 8,
				hairStyle = HairStyle.WAVY,

				//golden dangly belly button piercing
				navelPiercings = new PiercingData<NavelPiercingLocation>()
				{
					[NavelPiercingLocation.BOTTOM] = new PiercingJewelry(JewelryType.DANGLER, new Gold(), true),
				},
				complexion = Tones.FAIR,
				skinTexture = SkinTexture.SOFT,


				//concentrated incubus draft but no TF changes? that makes no sense. Have some demonic high-heels
				lowerBodyType = LowerBodyType.DEMONIC_HIGH_HEELS,

				//perk : Alchemist - Guniea Pig (makes you more sucsceptible to TFs) also slacker, as it seems to fit her backstory.
				//endowment: sensitivity
				perks = new List<StandardPerk>()
				{
					new Slacker(),
					new TestSubject(),
					new Sensitive(),
				},
			};

			//wearing: torn thong, comfortable bra. torn thong is like a normal thong, but with a hole for easy access to pussy or to let a herm's dick hang free

			//extra trait: lightweight (bad with liquor)
			//fetish (NYI): submissive. also piercings. receiving anal sex, or giving anal sex to males.
			//dislikes (NYI): big clits. other subs. over-large cocks.
		}
	}
}
