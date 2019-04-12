using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;

namespace Testing
{
	internal static class SpecialCharacters
	{
		public static PlayerCreator ANNETTA => new PlayerCreator()
		{
			tone = Tones.PURPLE,
			hornType = HornType.DEMON,
			additionalHornTransformStrength = 1,
			earType = EarType.ELFIN,
			hairColor = HairFurColors.ORANGE,

		};

		public static PlayerCreator ARIA => new PlayerCreator()
		{
			skinTexture = SkinTexture.SMOOTH,
			furColor = new FurColor(HairFurColors.PINK),
			hairColor = HairFurColors.PINK,

			bodyType = BodyType.KITSUNE,
			earPiercings = Piercing<EarPiercings>.CreatePiercingDataForCreator(
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_LOBE_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_LOBE_2, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_AURICAL_3, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_AURICAL_4, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_HELIX_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_HELIX_2, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_LOBE_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_LOBE_2, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_HELIX_2, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_AURICAL_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_AURICAL_2, new JewelryDummyForDebugging()),
				//Right industrial.
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_HELIX_3, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_ANTI_HELIX, new JewelryDummyForDebugging())),

			navelPiercings = Piercing<NavelPiercingLocation>.CreatePiercingDataForCreator(new Pair<NavelPiercingLocation, PiercingJewelry>(NavelPiercingLocation.BOTTOM, new JewelryDummyForDebugging())),
		};

		public static PlayerCreator BERTRAM => new PlayerCreator()
		{
			earType = EarType.FOX,
		};

		public static PlayerCreator CEVEO => new PlayerCreator()
		{
			leftEyeColor = EyeColor.BROWN,
			//lipPiercings = PiercingHelper.CreatePiercingListForCreator<Lip, LipPiercings>(LipPiercings.LABRET),
			hairColor = HairFurColors.BLACK,
			tone = Tones.PALE,
		};

		public static PlayerCreator CHARAUN => new PlayerCreator()
		{
			hairColor = HairFurColors.MIDNIGHT_BLACK,
			bodyType = BodyType.SIMPLE_FUR,
			tone = Tones.GRAYISH_BLUE,
			furColor = new FurColor(HairFurColors.GRAYISH_BLUE),
			tongueType = TongueType.DEMONIC,
		};

		public static PlayerCreator CHARLIE => new PlayerCreator()
		{
			hairColor = HairFurColors.BLONDE,
			tone = Tones.LIGHT,
			wingType = WingType.FEATHERED,
			largeWings = true, //not needed as feathered are always large for PC.

		};

		//the first with true rng, so i can't use shorthand :(.
		//		public static PlayerCreator CHIMERA
		//		{
		//			get
		//			{
		//				PlayerCreator retVal = new PlayerCreator();
		//				int rand = Utils.Rand(100);
		//				if (rand < 50)
		//				{
		//					retVal.bodyType = BodyType.HUMANOID;
		//				}
		//				else if (rand < 80)
		//				{
		//					retVal.bodyType = BodyType.SIMPLE_FUR;
		//				}
		//				else
		//				{
		//					retVal.bodyType = BodyType.GOO;
		//					retVal.skinTexture = SkinTexture.SLIMY;
		//				}
		//#warning: Implement face.
		//				if (/*faceType == Spider &&*/Utils.RandBool())
		//				{
		//					retVal.eyeType = EyeType.SPIDER;
		//				}
		//				else if (Utils.Rand(20) == 0)
		//				{
		//					retVal.eyeType= 
		//				}
		//				return retVal;

		//			}
		//		}

		public static PlayerCreator CODY => new PlayerCreator()
		{
			hairColor = HairFurColors.BLACK,
			furColor = new FurColor(HairFurColors.BLACK, HairFurColors.ORANGE, FurMulticolorPattern.STRIPED),
			tone = Tones.LIGHT,
		};

		public static PlayerCreator ETIS => new PlayerCreator()
		{
			bodyType = BodyType.KITSUNE,
			hairColor = HairFurColors.SNOW_WHITE,
			furColor = new FurColor(HairFurColors.SNOW_WHITE),
			underFurColor = new FurColor(HairFurColors.SNOW_WHITE),
			earType = EarType.FOX,
			armType = ArmType.FOX,
			earPiercings = Piercing<EarPiercings>.CreatePiercingDataForCreator(
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_LOBE_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_LOBE_1, new JewelryDummyForDebugging())
			),
			eyeType = EyeType.DRAGON,
			hornType = HornType.DRACONIC,
			additionalHornTransformStrength = 2,
			forceUniformHornGrowthOnCreate = true, //not needed b/c dragon horns dont have rng, but.
			tongueType = TongueType.DRACONIC,
			wingType = WingType.DRACONIC,
			wingTone = Tones.MILKY_WHITE,
			largeWings = true,
			//anemone hair, not growing.
		};

		public static PlayerCreator GALATEA => new PlayerCreator()
		{

		};

		public static PlayerCreator GUNDAM => new PlayerCreator()
		{

		};

		public static PlayerCreator HIKARI => new PlayerCreator()
		{

		};

		public static PlayerCreator ISAAC => new PlayerCreator()
		{

		};

		public static PlayerCreator KATTI => new PlayerCreator()
		{
			earType = EarType.BUNNY,

		};

		/*
		public static PlayerCreator KITTEH => new PlayerCreator()
		{
			//NOT YET IMPLEMENTED
		};
		*/
		public static PlayerCreator LEAH => new PlayerCreator()
		{
			skinTexture = SkinTexture.SMOOTH,
			tone = Tones.OLIVE,
			hairColor = HairFurColors.BLACK,
		};

		public static PlayerCreator LUCINA => new PlayerCreator()
		{
			tone = Tones.FAIR,
			hairColor = HairFurColors.PLATINUM_BLONDE,

		};

		public static PlayerCreator LUKAZ => new PlayerCreator()
		{
			hairColor = HairFurColors.BROWN, //also blue. idk. NJ other dev.
			earType = EarType.DOG,
			tone = Tones.LIGHT,
			furColor = new FurColor(HairFurColors.BLUE),
			tongueType = TongueType.DRACONIC,
		};

		public static PlayerCreator MARA => new PlayerCreator()
		{
			earType = EarType.BUNNY,
			tone = Tones.TAN,
			hairColor = HairFurColors.PLATINUM_BLONDE,
		};

		public static PlayerCreator MIHARI => new PlayerCreator()
		{
			earType = EarType.CAT,
			bodyType = BodyType.SIMPLE_FUR,
			furColor = new FurColor(HairFurColors.CHARCOAL),

		};

		public static PlayerCreator MIRVANNA => new PlayerCreator()
		{
			hairColor = HairFurColors.PLATINUM_BLONDE,
			earType = EarType.HORSE,
			tone = Tones.BROWN,
			bodyType = BodyType.SIMPLE_FUR,
			furColor = new FurColor(HairFurColors.BLACK),
			wingType = WingType.DRACONIC,
			largeWings = true,
			//horse cunt.
		};

		public static PlayerCreator NAMI => new PlayerCreator()
		{
			earType = EarType.DOG,
			furColor = new FurColor(HairFurColors.WHITE),
		};

		public static PlayerCreator NAVORN => new PlayerCreator()
		{
			tone = Tones.BLACK,
			bodyType = BodyType.SIMPLE_FUR,
			furColor = new FurColor(HairFurColors.SILVER),
			hairColor = HairFurColors.SILVER,
			earType = EarType.FOX,
			wingType = WingType.DRACONIC,
			largeWings = true,
		};

		public static PlayerCreator NIX => new PlayerCreator()
		{
			furColor = new FurColor(HairFurColors.SILVER),
			hairColor = HairFurColors.SILVER,
			earType = EarType.DOG,
		};

		public static PlayerCreator PEONE => new PlayerCreator()
		{
			hairColor = HairFurColors.AUBURN,

		};

		public static PlayerCreator PRISMERE => new PlayerCreator()
		{
			tone = Tones.OLIVE,
			hairColor = HairFurColors.DEEP_RED,
			wingType = WingType.BAT_LIKE,
			largeWings = true,
			leftEyeColor = EyeColor.BLUE,
		};

		//totally ready this as a Quarian from Mass Effect. no regrests -JSG.
		public static PlayerCreator RANN_RAYLA => new PlayerCreator()
		{
			hairColor = HairFurColors.AUBURN,
			tone = Tones.LIGHT,
		};

		public static PlayerCreator ROPE => new PlayerCreator()
		{
			//shark teeth.
		};

		//dragon age, this time. 
		public static PlayerCreator SERA => new PlayerCreator()
		{
			earType = EarType.CAT,
			wingType = WingType.BAT_LIKE,
			largeWings = true,
		};

		public static PlayerCreator SIVEEN => new PlayerCreator()
		{
			hairColor = HairFurColors.WHITE,
			//black highlights in hair? that'd fulfill the two-tone
			tone = Tones.DARK,
			leftEyeColor = EyeColor.RED,
			rightEyeColor = EyeColor.BLUE,
			earType = EarType.ELFIN,
		};

		public static PlayerCreator SORA => new PlayerCreator()
		{
			tongueType = TongueType.SNAKE,
			tonguePiercings = Piercing<TonguePiercingLocation>.CreatePiercingDataForCreator(
				new Pair<TonguePiercingLocation, PiercingJewelry>(TonguePiercingLocation.MIDDLE_CENTER, new JewelryDummyForDebugging())),
			earType = EarType.FOX,
			earPiercings = Piercing<EarPiercings>.CreatePiercingDataForCreator(
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_LOBE_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.RIGHT_LOBE_1, new JewelryDummyForDebugging()),
				new Pair<EarPiercings, PiercingJewelry>(EarPiercings.LEFT_LOBE_2, new JewelryDummyForDebugging())),
			//two fox tails.
			bodyType = BodyType.KITSUNE,

		};

		public static PlayerCreator TYRIANA => new PlayerCreator()
		{
			tone = Tones.TAN,
			hairColor = HairFurColors.AUBURN,
			tongueType = TongueType.DEMONIC,
			earType = EarType.CAT,

		};

		public static PlayerCreator VAHDUNBRII => new PlayerCreator()
		{
			hairColor = HairFurColors.BROWN,
			//tone = Tones.LIGHT,
			tone = Tones.SANGUINE,
			earType = EarType.CAT,
			armType = ArmType.HARPY,
			wingType = WingType.FEATHERED,
			largeWings = true,
			hornType = HornType.DRACONIC,
			additionalHornTransformStrength = 0,
			skinTexture = SkinTexture.SMOOTH,

		};
	}
}
