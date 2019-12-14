using System;
using System.Collections.Generic;
using System.Drawing;
using CoC.Backend.BodyParts;
namespace CoC.Backend.CoC_Colors
{
	public static class DefaultValueHelpers
	{
		public static Tones defaultHumanTone => Tones.LIGHT;

		public static HairFurColors defaultAnemoneHair => HairFurColors.CERULEAN;

		public static HairFurColors defaultBasiliskSpines => HairFurColors.GREEN;
		public static HairFurColors defaultBasiliskPlume => HairFurColors.RED;
		public static EyeColor defaultBasiliskEyeColor => EyeColor.GRAY;

		public static Tones defaultBeeTone => Tones.BLACK;

		public static FurColor defaultBehemothFur => new FurColor(HairFurColors.DARK_RED);

		public static FurColor defaultBunnyFur => new FurColor(HairFurColors.WHITE);

		public static FurColor defaultCatFur => new FurColor(HairFurColors.BROWN);
		public static EyeColor defaultCatEyeColor => EyeColor.GREEN;

		public static FurColor defaultCockatricePrimaryFeathers => new FurColor(HairFurColors.BLUE);
		public static Tones defaultCockatriceScaleTone => Tones.BLUE;
		public static FurColor defaultCockatriceTailFeaithers => new FurColor(HairFurColors.TURQUOISE);
		public static EyeColor defaultCockatriceEyeColor => EyeColor.BLUE;

		public static FurColor defaultCowFur => new FurColor(HairFurColors.WHITE, HairFurColors.BLACK, FurMulticolorPattern.SPOTTED);

		public static FurColor defaultDeerFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public static FurColor defaultDeerTail => new FurColor(HairFurColors.WHITE);


		public static Tones defaultDemonTone => Tones.DARK_RED;

		public static FurColor defaultDogFur => new FurColor(HairFurColors.DARK_RED);

		public static Tones defaultDragonTone => Tones.SILVER;
		public static Tones defaultDragonWingTone => Tones.DARK_RED;
		public static EyeColor defaultDragonEyeColor => EyeColor.ORANGE;
		public static HairFurColors defaultDragonManeColor => HairFurColors.GREEN;



		public static Tones defaultBarkColor => Tones.WOODLY_BROWN;
		public static HairFurColors defaultVineColor => HairFurColors.GREEN;



		public static FurColor defaultEchidnaFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public static Tones defaultEchidnaSpineColor => Tones.IVORY;
		public static FurColor defaultEchidnaTailFur => new FurColor(HairFurColors.BLACK);



		public static FurColor defaultFerretFur => new FurColor(HairFurColors.BLACK);
		public static FurColor defaultFerretUnderFur => new FurColor(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.MIXED);
		public static FurColor defaultFerretSecondaryFacialFur => new FurColor(HairFurColors.WHITE);


		public static FurColor defaultFoxFur => new FurColor(HairFurColors.ORANGE);
		public static FurColor defaultFoxUnderbody => new FurColor(HairFurColors.WHITE);
		public static FurColor defaultFoxFacialFur => new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.NO_PATTERN);
		public static FurColor defaultFoxSecondaryFacialFur => new FurColor(HairFurColors.WHITE);
		public static FurColor defaultFoxTailFur => new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.STRIPED);

		public static FurColor defaultSatyrTailColor => new FurColor(HairFurColors.WHITE);


		public static Tones defaultGooTone => Tones.CERULEAN;

		public static FurColor defaultHarpyFeathers => new FurColor(HairFurColors.WHITE);
		public static HairFurColors defaultHarpyFeatherHair => HairFurColors.WHITE;


		public static FurColor defaultHorseFur => new FurColor(HairFurColors.BROWN);
		public static HairFurColors defaultHorseHairColor => HairFurColors.BLACK;
		public static FurColor defaultHorseTailFur => new FurColor(defaultHorseHairColor);


		public static EyeColor defaultHumanEyeColor => EyeColor.GRAY;


		public static Tones defaultImpTone => Tones.ORANGE;
		public static FurColor defaultImpTailFur => new FurColor(HairFurColors.BLACK);


		public static FurColor defaultKangarooFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public static FurColor defaultKangarooUnderbodyFur => new FurColor(HairFurColors.WHITE);
		public static FurColor defaultKangarooFacialFur => defaultKangarooUnderbodyFur;
		public static FurColor defaultKangarooTailFur => defaultKangarooFur;


		public static FurColor defaultKitsuneFacialFur => new FurColor(HairFurColors.WHITE);
		public static FurColor defaultKitsuneFur => new FurColor(HairFurColors.ORANGE);
		public static Tones defaultKitsuneSkin => Tones.TAN;

		public static Tones defaultLizardTone => Tones.DARK_RED;
		public static EyeColor defaultLizardEyeColor => EyeColor.YELLOW;

		public static FurColor defaultMouseFur => new FurColor(HairFurColors.CHOCOLATE);

		public static Tones defaultNagaTone => Tones.DARK_GREEN;
		public static Tones defaultNagaUnderTone => Tones.TAN;

		public static FurColor defaultPigFur => new FurColor(HairFurColors.PINK);

		public static FurColor defaultRaccoonFur => new FurColor(HairFurColors.DARK_GRAY, HairFurColors.LIGHT_GRAY, FurMulticolorPattern.STRIPED);


		public static HairFurColors defaultRedPandaHairColor => HairFurColors.WHITE;
		public static FurColor defaultRedPandaFaceEarTailFur => new FurColor(HairFurColors.WHITE);
		public static FurColor defaultRedPandaUnderFur => new FurColor(HairFurColors.BLACK);
		public static FurColor defaultRedPandaFur => new FurColor(HairFurColors.AUBURN);


		public static FurColor defaultRhinoTailFur => new FurColor(HairFurColors.BLACK);



		public static Tones defaultSalamanderTone => Tones.DARK_RED;



		public static EyeColor defaultSandTrapEyeColor => EyeColor.BLACK;



		public static Tones defaultScorpionTailTone => Tones.TAN;



		public static Tones defaultSharkTone => Tones.GRAY;



		public static FurColor defaultSheepWoolFur => new FurColor(HairFurColors.WHITE);


		public static EyeColor defaultSpiderEyeColor => EyeColor.BLACK;
		public static Tones defaultSpiderTone => Tones.BLACK;

		public static Tones defaultTentacleTone => Tones.CERULEAN;

		public static EyeColor defaultWolfEyeColor => EyeColor.AMBER;
		public static FurColor defaultWolfFurColor => new FurColor(HairFurColors.BLACK, HairFurColors.GRAY, FurMulticolorPattern.NO_PATTERN);
		public static FurColor defaultWolfFacialFur => new FurColor(HairFurColors.GRAY);
		public static FurColor defaultWolfTailFur => new FurColor(HairFurColors.GRAY);

		public static FurColor defaultBronyFur => new FurColor(HairFurColors.PINK);

		internal static HairFurColors ToNearestSpineColor(HairFurColors currentColor, Tones currentSkinTone)
		{
			Color color = Color.FromArgb((currentColor.rgbValue.R + currentSkinTone.rgbValue.R) / 2, (currentColor.rgbValue.G + currentSkinTone.rgbValue.G) / 2,
				(currentColor.rgbValue.B + currentSkinTone.rgbValue.B) / 2);
			return HairFurColors.NearestHairFurColor(color);
		}

		private static readonly Dictionary<Tones, Tones> nagaLowerBodyMapper = new Dictionary<Tones, Tones>()
		{
			{Tones.RED, Tones.ORANGE},
			{Tones.ORANGE, Tones.YELLOW},
			{Tones.YELLOW, Tones.YELLOWISH_GREEN},
			{Tones.YELLOWISH_GREEN, Tones.YELLOW},
			{Tones.GREEN, Tones.LIGHT_GREEN},
			{Tones.SPRING_GREEN, Tones.CYAN},
			{Tones.CYAN, Tones.TURQUOISE},
			{Tones.TURQUOISE, Tones.LIGHT_BLUE},
			{Tones.BLUE, Tones.LIGHT_BLUE},
			{Tones.PURPLE, Tones.LIGHT_PURPLE},
			{Tones.MAGENTA, Tones.BLUE},
			{Tones.DEEP_PINK, Tones.PINK},
			{Tones.BLACK, Tones.DARK_GRAY},
			{Tones.WHITE, Tones.LIGHT_GRAY},
			{Tones.GRAY, Tones.LIGHT_GRAY},
			{Tones.LIGHT_GRAY, Tones.WHITE},
			{Tones.DARK_GRAY, Tones.GRAY},
			{Tones.PINK, Tones.PALE_PINK}
		};
		internal static Tones GetNageUnderToneFrom(Tones primary)
		{
			if (nagaLowerBodyMapper.ContainsKey(primary))
			{
				return nagaLowerBodyMapper[primary];
			}
			else
			{
				return defaultNagaUnderTone;
			}
		}


	}
}
