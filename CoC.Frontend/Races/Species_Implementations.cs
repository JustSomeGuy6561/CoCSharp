//Species_Implementations.cs
//Description: Implementation of the specific races. It may be better practice for each of these to be their own class
//but for faster development this is easier for now.
//Author: JustSomeGuy
//2/20/2019, 5:46 PM

using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Tools;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.Perks.SpeciesPerks;
using CoC.Frontend.StatusEffect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CoC.Frontend.Races
{

	public class Anemone : Species
	{
		public HairFurColors defaultHair => HairFurColors.CERULEAN;
		public Tones defaultTone => Tones.CERULEAN;
		internal Anemone() : base(AnemoneStr) { }

		public override byte Score(Creature source)
		{
			int anemoneCount = 0;

			if (source.gills.type == GillType.ANEMONE)
			{
				anemoneCount++;
			}

			if (source.hair.type == HairType.ANEMONE)
			{
				anemoneCount++;
			}

			if (anemoneCount > 0 && source.gender == Gender.HERM)
			{
				anemoneCount++;
			}
			if (anemoneCount > 1 && source.body.type == BodyType.HUMANOID)
			{
				anemoneCount++;
			}
			if (anemoneCount > 1 && source.body.primarySkin.tone == Tones.BLUE_BLACK || source.body.primarySkin.tone == Tones.CERULEAN)
			{
				anemoneCount++;
			}
			return (byte)Utils.Clamp2(anemoneCount, byte.MinValue, byte.MaxValue);
		}
	}

	public class Basilisk : Species
	{
		public HairFurColors defaultSpines = HairFurColors.GREEN;
		public HairFurColors defaultPlume = HairFurColors.RED;
		public Tones defaultTone => Tones.GREEN;
		public EyeColor defaultEyeColor => EyeColor.GRAY;

		public HairFurColors ToNearestSpineColor(HairFurColors currentColor, Tones currentSkinTone)
		{
			Color color = Color.FromArgb((currentColor.rgbValue.R + currentSkinTone.rgbValue.R) / 2, (currentColor.rgbValue.G + currentSkinTone.rgbValue.G) / 2,
				(currentColor.rgbValue.B + currentSkinTone.rgbValue.B) / 2);
			return HairFurColors.NearestHairFurColor(color);
		}

		internal Basilisk() : base(BasiliskStr) { }


		public override byte Score(Creature source)
		{
			int basiliskCount = 0;
			if (source.eyes.type == EyeType.BASILISK)
			{
				basiliskCount++;
			}
			if (source.womb is PlayerWomb playerWomb && playerWomb.basiliskWomb)
			{
				basiliskCount++;
			}
			if (basiliskCount > 0)
			{
				if (source.face.type == FaceType.LIZARD)
				{
					basiliskCount++;
				}
				if (source.ears.type == EarType.LIZARD)
				{
					basiliskCount++;
				}
				if (source.tail.type == TailType.LIZARD)
				{
					basiliskCount++;
				}
				if (source.lowerBody.type == LowerBodyType.LIZARD)
				{
					basiliskCount++;
				}
				if (source.horns.type == HornType.DRACONIC)
				{
					basiliskCount++;
					if (source.horns.numHorns == 4)
					{
						basiliskCount++;
					}
				}
				if (source.arms.type == ArmType.LIZARD)
				{
					basiliskCount++;
				}
				if (basiliskCount > 2)
				{
					if (source.tongue.type == TongueType.LIZARD || source.tongue.type == TongueType.SNAKE)
					{
						basiliskCount++;
					}
					if (source.genitals.CountCocksOfType(CockType.LIZARD) > 0)
					{
						basiliskCount++;
					}
					if (source.eyes.type == EyeType.LIZARD || source.eyes.type == EyeType.BASILISK)
					{
						basiliskCount++;
					}
					if (source.body.type == BodyType.REPTILE)
					{
						basiliskCount++;
					}
				}
			}
			return (byte)Utils.Clamp2(basiliskCount, byte.MinValue, byte.MaxValue);
		}
	}

	public class Bee : Species
	{
		public HairFurColors defaultHair => HairFurColors.BLACK;
		public FurColor defaultFur => new FurColor(HairFurColors.BLACK, HairFurColors.YELLOW, FurMulticolorPattern.STRIPED);
		public Tones defaultTone => Tones.BLACK;
		public Tones defaultAbdomenTone => defaultTone;
		internal Bee() : base(BeeStr) { }

		public override byte Score(Creature source)
		{
			int beeCounter = 0;
			if (source.hair.hairColor == HairFurColors.BLACK || source.hair.hairColor == HairFurColors.MIDNIGHT_BLACK)
			{
				beeCounter++;
			}
			//if has fur and it's black or midnight black
			if (source.body.mainEpidermis.usesFurColor && (source.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.BLACK) ||
				source.body.mainEpidermis.fur.IsIdenticalTo(HairFurColors.MIDNIGHT_BLACK)))
			{
				beeCounter++;
			}
			//if has fur and it's black and yellow striped.
			else if (source.body.mainEpidermis.usesFurColor && source.body.mainEpidermis.fur.Equals(defaultFur))
			{
				beeCounter += 2;
			}
			if (source.hair.hairColor == HairFurColors.MIDNIGHT_BLACK || source.hair.hairColor == HairFurColors.BLACK)
			{
				beeCounter++;
			}

			if (source.antennae.type == AntennaeType.BEE)
			{
				beeCounter++;
				if (source.face.type == FaceType.HUMAN)
				{
					beeCounter++;
				}
			}
			if (source.arms.type == ArmType.BEE)
			{
				beeCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.BEE)
			{
				beeCounter++;
				if (source.vaginas.Count == 1)
					beeCounter++;
			}
			if (source.ovipositor.type == OvipositorType.BEE)
			{
				beeCounter++;
			}
			if (source.tail.type == TailType.BEE_STINGER)
			{
				beeCounter++;
			}
			if (source.wings.type == WingType.BEE_LIKE)
			{
				beeCounter++;
			}
			return (byte)Utils.Clamp2(beeCounter, byte.MinValue, byte.MaxValue);
		}
	}
	public class Behemoth : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.DARK_RED);
		public FurColor defaultTailFur => defaultFur;
		internal Behemoth() : base(BehemothStr) { }

		public override byte Score(Creature source)
		{
			int behemothCounter = 0;

			//must have one of the following
			if (source.back.type == BackType.BEHEMOTH)
			{
				behemothCounter++;
			}
			if (source.tail.type == TailType.BEHEMOTH)
			{
				behemothCounter++;
			}

			if (behemothCounter > 0)
			{
				if (source.horns.type == HornType.DRACONIC)
				{
					behemothCounter++;
				}
				if (source.ears.type == EarType.ELFIN)
				{
					behemothCounter++;
				}
				if (source.lowerBody.type == LowerBodyType.CAT)
				{
					behemothCounter++;
				}
			}
			//must have at least 3 of the previous options, and one must be behemoth type.
			if (behemothCounter > 2)
			{
				if (source.face.type == FaceType.CAT)
				{
					behemothCounter++;
				}
				if (source.genitals.CountCocksOfType(CockType.CAT) > 0)
				{
					behemothCounter++;
				}

				if (source.eyes.type == EyeType.CAT)
				{
					behemothCounter++;
				}
			}
			return (byte)Utils.Clamp2(behemothCounter, byte.MinValue, byte.MaxValue);
		}
	}
	public class Bunny : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Bunny() : base(BunnyStr) { }

		public override byte Score(Creature source)
		{
			int bunnyCounter = 0;
			if (source.face.type == FaceType.BUNNY)
			{
				bunnyCounter++;
			}
			if (source.tail.type == TailType.RABBIT)
			{
				bunnyCounter++;
			}
			if (source.ears.type == EarType.BUNNY)
			{
				bunnyCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.BUNNY)
				bunnyCounter++;
			//More than 2 balls reduces bunny score
			if (source.balls.count > 2 && bunnyCounter > 0)
			{
				bunnyCounter--;
			}
			//Human skin on bunmorph adds
			if (source.body.mainEpidermis.type == EpidermisType.SKIN && bunnyCounter > 1)
			{
				bunnyCounter++;
			}
			//No wings and antennae a plus
			if (bunnyCounter > 0 && source.antennae.type == AntennaeType.NONE)
			{
				bunnyCounter++;
			}
			if (bunnyCounter > 0 && source.wings.type == WingType.NONE)
			{
				bunnyCounter++;
			}
			return (byte)Utils.Clamp2(bunnyCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Cat : Species
	{
		public FurColor[] availableColors => new FurColor[]
		{
			new FurColor(HairFurColors.BROWN),
			new FurColor(HairFurColors.CHOCOLATE),
			new FurColor(HairFurColors.AUBURN),
			new FurColor(HairFurColors.CARAMEL),
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.SANDY_BROWN),
			new FurColor(HairFurColors.GOLDEN),
			new FurColor(HairFurColors.BLACK),
			new FurColor(HairFurColors.MIDNIGHT_BLACK),
			new FurColor(HairFurColors.DARK_GRAY),
			new FurColor(HairFurColors.GRAY),
			new FurColor(HairFurColors.LIGHT_GRAY),
			new FurColor(HairFurColors.SILVER),
			new FurColor(HairFurColors.WHITE),
		};

		public FurColor defaultFur => new FurColor(HairFurColors.BROWN);
		public FurColor defaultFacialFur => defaultFur;

		//has a 20% chance of an underbody. It will be white, unless primary is white, then black.
		public void getRandomFurColors(out FurColor primary, out FurColor underbody)
		{
			FurColor[] colors = availableColors;
			primary = Utils.RandomChoice(colors);
			FurColor underColor = primary;
			if (Utils.Rand(10) < 2) underbody = primary.primaryColor == HairFurColors.WHITE ? new FurColor(HairFurColors.BLACK) : new FurColor(HairFurColors.WHITE);
			else underbody = new FurColor(primary);
		}

		public EyeColor defaultEyeColor => EyeColor.GREEN;
		public FurColor defaultTailFur => defaultFur;

		internal Cat() : base(CatStr) { }

		public override byte Score(Creature source)
		{
			int catCounter = 0;
			if (source.face.type == FaceType.CAT)
			{
				catCounter++;
			}
			if (source.tongue.type == TongueType.CAT)
			{
				catCounter++;
			}
			if (source.ears.type == EarType.CAT)
			{
				catCounter++;
			}
			if (source.tail.type == TailType.CAT)
			{
				catCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.CAT)
			{
				catCounter++;
			}
			if (source.arms.type == ArmType.CAT)
			{
				catCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.CAT) > 0)
			{
				catCounter++;
			}
			if (source.breasts.Count == 2 && catCounter > 0)
			{
				catCounter++;
			}
			else if (source.breasts.Count == 3 && catCounter > 0)
			{
				catCounter += 2;
			}
			//Fur only counts if some feline features are present
			if (source.hasPrimaryFur && catCounter > 0)
			{
				catCounter++;
			}
			return (byte)Utils.Clamp2(catCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Satyr : Species
	{
		internal Satyr() : base(SatyrStr) { }

		public FurColor defaulTailColor => new FurColor(HairFurColors.WHITE);

		public override byte Score(Creature source)
		{
			int satyrCounter = 0;
			if (source.lowerBody.type == LowerBodyType.CLOVEN_HOOVED)
			{
				satyrCounter++;
			}
			if (source.tail.type == TailType.SATYR)
			{
				satyrCounter++;
			}
			if (satyrCounter >= 2)
			{
				if (source.ears.type == EarType.ELFIN)
				{
					satyrCounter++;
				}
				if (source.face.type == FaceType.HUMAN)
				{
					satyrCounter++;
				}
				if (source.genitals.CountCocksOfType(CockType.HUMAN) > 0)
				{
					satyrCounter++;
				}
				if (source.balls.count > 0 && source.balls.size >= 3)
				{
					satyrCounter++;
				}
			}
			return (byte)Utils.Clamp2(satyrCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Cockatrice : Species
	{
		//flash version behavior is probably not intended, but it works as such:
		//the underbody is set the the secondary fur color (the second value in the array)
		//but it is ignored by player appearance if the body is cockatrice. it can be dyed, sure, but it's not used otherwise.
		//i suppose it'd carry over to other classes on TF, but that seems an unintended side-effect.
		//similarly, unless the check for cockatrice skin is ran, or FurColor is expressly called (which is VERY unsafe, unless the cockatrice check is ran),
		//the primary fur color is ignored

		//if cockatrice, store the fur and tone in the primary, and use them both.

		//i'm changing this - now the body will use the primary fur color and the tone as the underbody (which is scales)
		//the secondary color is currently unused, but it would be possible to use it for the lower body for like fur around groin or something.
		//naga already does this with its own third color, albeit for the tail.

		//public static const COCKATRICE:Array = [

		//	["blue",   "turquoise", "blue"],
		//          ["orange", "red",       "orange"],
		//          ["green",  "yellow",    "green"],
		//          ["purple", "pink",      "purple"],
		//          ["black",  "white",     "black"],
		//          ["blonde", "brown",     "blonde"],
		//          ["white",  "grey",      "white"],
		//      ];

		public HairFurColors[] availablePrimaryFeatherColors => new HairFurColors[]
		{
			HairFurColors.BLUE,
			HairFurColors.ORANGE,
			HairFurColors.GREEN,
			HairFurColors.PURPLE,
			HairFurColors.BLACK,
			HairFurColors.BLONDE,
			HairFurColors.WHITE,
		};
		public Tones[] availableTones => new Tones[]
		{
			Tones.BLUE,
			Tones.ORANGE,
			Tones.GREEN,
			Tones.PURPLE,
			Tones.BLACK,
			Tones.PALE_YELLOW,
			Tones.WHITE
		};

		public HairFurColors[] availableSecondaryFeatherColors => new HairFurColors[]
		{
			HairFurColors.TURQUOISE,
			HairFurColors.RED,
			HairFurColors.YELLOW,
			HairFurColors.PINK,
			HairFurColors.WHITE,
			HairFurColors.BROWN,
			HairFurColors.GRAY,
		};

		public void GetRandomCockatriceColors(out FurColor primaryFeathers, out Tones secondaryScales)
		{
			Tones[] secondaryTones = availableTones;
			int rand = Utils.Rand(availablePrimaryFeatherColors.Length);
			HairFurColors first = availablePrimaryFeatherColors[rand];
			HairFurColors second;
			if (rand >= availableSecondaryFeatherColors.Length)
			{
				second = availableSecondaryFeatherColors[availableSecondaryFeatherColors.Length - 1];
			}
			else
			{
				second = availableSecondaryFeatherColors[rand];
			}
			if (rand >= secondaryTones.Length)
			{
				secondaryScales = secondaryTones[secondaryTones.Length - 1];
			}
			else
			{
				secondaryScales = secondaryTones[rand];
			}

			primaryFeathers = new FurColor(first, second, FurMulticolorPattern.NO_PATTERN);

		}

		public HairFurColors GetSecondaryCockatriceFeathers(FurColor primaryColor)
		{
			HairFurColors[] primaryColors = availablePrimaryFeatherColors;
			HairFurColors[] secondaryColors = availableSecondaryFeatherColors;
			bool predicate(HairFurColors x) => primaryColor.IsIdenticalTo(x);
			int y = Array.FindIndex(primaryColors, predicate);
			if (y != -1)
			{
				{
				}
				if (y >= secondaryColors.Length)
				{
					return secondaryColors[secondaryColors.Length - 1];
				}
				else return secondaryColors[y];
			}

			else return HairFurColors.YELLOW;
		}

		public FurColor defaultPrimaryFeathers => new FurColor(HairFurColors.BLUE);
		public Tones defaultScaleTone => Tones.BLUE;
		public HairFurColors defaultSecondaryFeathers => HairFurColors.TURQUOISE;
		public FurColor defaultTailFeaithers => new FurColor(defaultSecondaryFeathers);
		public EyeColor defaultEyeColor => EyeColor.BLUE;
		internal Cockatrice() : base(CockatriceStr) { }

		public override byte Score(Creature source)
		{
			{
				int cockatriceCounter = 0;
				if (source.ears.type == EarType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.tail.type == TailType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.lowerBody.type == LowerBodyType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.face.type == FaceType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.eyes.type == EyeType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.arms.type == ArmType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.antennae.type == AntennaeType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (source.neck.type == NeckType.COCKATRICE)
				{
					cockatriceCounter++;
				}
				if (cockatriceCounter > 2)
				{
					if (source.tongue.type == TongueType.LIZARD)
					{
						cockatriceCounter++;
					}
					if (source.wings.type == WingType.FEATHERED && source.wings.isLarge)
					{
						cockatriceCounter++;
					}
					if (source.body.type == BodyType.COCKATRICE)
					{
						cockatriceCounter += 3;
					}
					else
					{
						if (source.body.mainEpidermis.type == EpidermisType.SCALES)
						{
							cockatriceCounter++;
						}
						if (source.body.hasSecondaryEpidermis && source.body.supplementaryEpidermis.type == EpidermisType.FEATHERS)
						{
							cockatriceCounter++;
						}
					}
					if (source.genitals.CountCocksOfType(CockType.LIZARD) > 0)
					{
						cockatriceCounter++;
					}
				}
				return (byte)Utils.Clamp2(cockatriceCounter, byte.MinValue, byte.MaxValue);
			}
		}
	}

	public class Cow : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.WHITE, HairFurColors.BLACK, FurMulticolorPattern.SPOTTED);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Cow() : base(CowStr) { }

		public override byte Score(Creature source)
		{
			if (source.gender == Gender.MALE || (source.gender == Gender.HERM && source.genitals.femininity <= Femininity.MASCULINE))
			{
				return MinotaurScore(source);
			}
			else
			{
				return SowScore(source);
			}
		}

		public byte MinotaurScore(Creature source)
		{
			int minoCounter = 0;
			if (source.face.type == FaceType.COW_MINOTAUR)
			{
				minoCounter++;
			}
			if (source.ears.type == EarType.COW)
			{
				minoCounter++;
			}
			if (source.tail.type == TailType.COW)
			{
				minoCounter++;
			}
			if (source.horns.type == HornType.BULL_LIKE)
			{
				minoCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.HOOVED && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.heightInInches > 80 && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.cocks.Count > 0 && minoCounter > 0)
			{
				if (source.genitals.CountCocksOfType(CockType.HORSE) > 0)
				{
					minoCounter++;
				}
			}
			if (source.vaginas.Count > 0)
			{
				minoCounter--;
			}
			return (byte)Utils.Clamp2(minoCounter, byte.MinValue, byte.MaxValue);
		}

		public byte SowScore(Creature source)
		{
			int minoCounter = 0;
			if (source.ears.type == EarType.COW)
			{
				minoCounter++;
			}
			if (source.tail.type == TailType.COW)
			{
				minoCounter++;
			}
			if (source.horns.type == HornType.BULL_LIKE)
			{
				minoCounter++;
			}
			if (source.face.type == FaceType.HUMAN && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.face.type == FaceType.COW_MINOTAUR)
			{
				minoCounter--;
			}
			if (source.lowerBody.type == LowerBodyType.HOOVED && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.heightInInches >= 73 && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.vaginas.Count > 0 && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.genitals.BiggestCupSize() > CupSize.D && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.genitals.BiggestCupSize() > CupSize.EE && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.breasts.Count >= 2 && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.genitals.hasQuadNipples && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.genitals.lactationStatus > LactationStatus.LIGHT && minoCounter > 0)
			{
				minoCounter++;
			}
			if (source.genitals.lactationStatus > LactationStatus.STRONG && minoCounter > 0)
			{
				minoCounter++;
			}

			return (byte)Utils.Clamp2(minoCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Deer : Species
	{
		public FurColor defaultFurColor => new FurColor(HairFurColors.LIGHT_BROWN);
		public FurColor defaultYouthFurColor => new FurColor(HairFurColors.LIGHT_BROWN, HairFurColors.WHITE, FurMulticolorPattern.SPOTTED); //unused atm.
		public FurColor defaultTailFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultFurColor;

		internal Deer() : base(DeerStr) { }

		public override byte Score(Creature source)
		{
			int deerCounter = 0;
			if (source.ears.type == EarType.DEER)
			{
				deerCounter++;
			}
			if (source.tail.type == TailType.DEER)
			{
				deerCounter++;
			}
			if (source.face.type == FaceType.DEER)
			{
				deerCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.CLOVEN_HOOVED)
			{
				deerCounter++;
			}
			if (source.horns.type == HornType.REINDEER_ANTLERS || source.horns.type == HornType.DEER_ANTLERS && source.horns.numHorns >= 4)
			{
				deerCounter++;
			}
			if (deerCounter >= 2 && source.hasPrimaryFur)
			{
				deerCounter++;
			}
			if (deerCounter >= 3 && source.genitals.CountCocksOfType(CockType.HORSE) > 0)
			{
				deerCounter++;
			}
			return (byte)Utils.Clamp2(deerCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Demon : Species
	{
		public Tones defaultTone => Tones.DARK_RED;
		public Tones defaultTailTone => defaultTone;
		internal Demon() : base(DemonStr) { }

		public override byte Score(Creature source)
		{
			int demonCounter = 0;
			if (source.horns.type == HornType.DEMON && source.horns.numHorns > 0)
			{
				demonCounter++;
			}
			if (source.horns.type == HornType.DEMON && source.horns.numHorns > 4)
			{
				demonCounter++;
			}
			if (source.tail.type == TailType.DEMONIC)
			{
				demonCounter++;
			}
			if (source.wings.type == WingType.BAT_LIKE)
			{
				demonCounter++;
			}
			if (source.hasPlainSkin && source.corruption > 50)
			{
				demonCounter++;
			}
			if (source.face.type == FaceType.HUMAN && source.corruption > 50)
			{
				demonCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.DEMONIC_HIGH_HEELS || source.lowerBody.type == LowerBodyType.DEMONIC_CLAWS)
			{
				demonCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.DEMON) > 0)
			{
				demonCounter++;
			}
			return (byte)Utils.Clamp2(demonCounter, byte.MinValue, byte.MaxValue);
		}

	}

	public class Dog : Species
	{
		//correctly written - it uses and.
		public FurColor[] availableColors => new FurColor[]
		{
			new FurColor(HairFurColors.BROWN),
			new FurColor(HairFurColors.CHOCOLATE),
			new FurColor(HairFurColors.CARAMEL),
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.BLACK),
			new FurColor(HairFurColors.DARK_GRAY),
			new FurColor(HairFurColors.GRAY),
			new FurColor(HairFurColors.LIGHT_GRAY),
			new FurColor(HairFurColors.SILVER),
			new FurColor(HairFurColors.WHITE),
			new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.NO_PATTERN),
			new FurColor(HairFurColors.BROWN, HairFurColors.WHITE, FurMulticolorPattern.NO_PATTERN),
			new FurColor(HairFurColors.BLACK, HairFurColors.WHITE, FurMulticolorPattern.NO_PATTERN)
		};
		public FurColor defaultFur => new FurColor(HairFurColors.DARK_RED); //Hellhound
		public FurColor defaultFacialFur => defaultFur; //see above.
		public FurColor defaultTailFur => defaultFacialFur;
		internal Dog() : base(DogStr) { }

		public override byte Score(Creature source)
		{
			int dogCounter = 0;
			if (source.face.type == FaceType.DOG)
			{
				dogCounter++;
			}
			if (source.ears.type == EarType.DOG)
			{
				dogCounter++;
			}
			if (source.tail.type == TailType.DOG)
			{
				dogCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.DOG)
			{
				dogCounter++;
			}
			if (source.arms.type == ArmType.DOG)
			{
				dogCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.DOG) > 0)
			{
				dogCounter++;
			}
			//Fur only counts if some canine features are present
			if (source.hasPrimaryFur && dogCounter > 0)
			{
				dogCounter++;
			}
			if (dogCounter >= 2)
			{
				if (source.breasts.Count == 3)
				{
					dogCounter += 2;
				}
				else if (source.breasts.Count == 2)
				{
					dogCounter++;
				}
			}
			return (byte)Utils.Clamp2(dogCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Dragon : Species
	{
		//eye color: Orange
		public Tones defaultTone => Tones.SILVER; // ember uses to silver/gold. So, that's what i'll use. screw it.

		public Tones defaultWingTone => Tones.DARK_RED;
		public Tones defaultWingBoneTone => defaultWingTone;
		public EyeColor defaultEyeColor => EyeColor.ORANGE;

		public HairFurColors defaultManeColor => HairFurColors.GREEN;
		public Tones defaultTailTone => defaultTone;
		internal Dragon() : base(DragonStr) { }

		public override byte Score(Creature source)
		{
			int dragonCounter = 0;
			if (source.face.type == FaceType.DRAGON)
			{
				dragonCounter++;
			}
			if (source.ears.type == EarType.DRAGON)
			{
				dragonCounter++;
			}
			if (source.tail.type == TailType.DRACONIC)
			{
				dragonCounter++;
			}
			if (source.tongue.type == TongueType.DRACONIC)
			{
				dragonCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.DRAGON) > 0)
			{
				dragonCounter++;
			}
			if (source.wings.type == WingType.DRACONIC)
			{
				dragonCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.DRAGON)
			{
				dragonCounter++;
			}
			if (source.body.type == BodyType.REPTILE && dragonCounter > 0)
			{
				dragonCounter++;
			}
			if (source.horns.type == HornType.DRACONIC)
			{
				dragonCounter++;
				if (source.horns.numHorns == 4)
				{
					dragonCounter++;
				}
			}
#warning NYI
			//if (source.combatAbilities.HasSpecialMove<Dragonfire>())
			//{
			//	dragonCounter++;
			//}
			if (source.arms.type == ArmType.DRAGON)
			{
				dragonCounter++;
			}
			if (source.eyes.type == EyeType.DRAGON)
			{
				dragonCounter++;
			}
			if (source.neck.type == NeckType.DRACONIC)
			{
				dragonCounter++;
			}
			if (source.back.type == BackType.DRACONIC_MANE || source.back.type == BackType.DRACONIC_SPIKES)
			{
				dragonCounter++;
			}
			return (byte)Utils.Clamp2(dragonCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Dryad : Species
	{
		public Tones defaultBarkColor => Tones.WOODLY_BROWN;
		public HairFurColors defaultVineColor => HairFurColors.GREEN;
		internal Dryad() : base(DryadStr) { }

		public override byte Score(Creature source)
		{
			int dryad = 0;
			if (source.body.type == BodyType.WOODEN)
			{
				dryad += 2;
			}
			if (source.cocks.Count > 1 || (source.cocks.Count > 0 && source.cocks[0].type != CockType.TENTACLE))
			{
				dryad--;
			}
			if (source.arms.type != ArmType.HUMAN)
			{
				dryad--;
			}
			if (source.hair.type == HairType.LEAF)
			{
				dryad++;
			}

			if (dryad >= 1 && source.ears.type == EarType.ELFIN)
			{
				dryad++;
			}

			return (byte)Utils.Clamp2(dryad, byte.MinValue, byte.MaxValue);
		}
	}

	public class Echidna : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public Tones defaultSpineColor => Tones.IVORY; //unused atm.

		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);

		internal Echidna() : base(EchidnaStr) { }

		public override byte Score(Creature source)
		{
			int echidnaCounter = 0;
			if (source.ears.type == EarType.ECHIDNA)
			{
				echidnaCounter++;
			}
			if (source.tail.type == TailType.ECHIDNA)
			{
				echidnaCounter++;
			}
			if (source.face.type == FaceType.ECHIDNA)
			{
				echidnaCounter++;
			}
			if (source.tongue.type == TongueType.ECHIDNA)
			{
				echidnaCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.ECHIDNA)
			{
				echidnaCounter++;
			}
			if (echidnaCounter >= 2 && source.hasPrimaryFur)
			{
				echidnaCounter++;
			}
			if (echidnaCounter >= 2 && source.genitals.CountCocksOfType(CockType.ECHIDNA) > 0)
			{
				echidnaCounter++;
			}
			//no one will ever see this easter egg, but i still think it's funnny.
			if (echidnaCounter >= 2 && source.name == "Knuckles" && SillyModeSettings.isEnabled)
			{
				echidnaCounter++;
			}
			return (byte)Utils.Clamp2(echidnaCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Ferret : Species
	{

		public HairFurColors[] availableHairColors => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.BROWN, HairFurColors.CARAMEL };

		private readonly Pair<FurColor, FurColor>[] furData = new Pair<FurColor, FurColor>[]
		{
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.BROWN), new FurColor(HairFurColors.BROWN)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.BROWN), new FurColor(HairFurColors.BLACK)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.LIGHT_BROWN), new FurColor(HairFurColors.CARAMEL)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.CARAMEL), new FurColor(HairFurColors.CARAMEL)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.CARAMEL), new FurColor(HairFurColors.SANDY_BROWN)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.SILVER), new FurColor(HairFurColors.SILVER)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.WHITE)),
			new Pair<FurColor, FurColor>(new FurColor(HairFurColors.SANDY_BROWN), new FurColor(HairFurColors.BROWN)),
		};

		public void GetFurColorsFrom(FurColor currentPrimary, out FurColor primary, out FurColor underbody)
		{
			if (furData.Any(x => x.first.Equals(currentPrimary)))
			{
				primary = currentPrimary;

				underbody = Utils.RandomChoice(furData.Where(x => x.first.Equals(currentPrimary)).ToArray()).second;
			}
			else
			{
				GetRandomFurColor(out primary, out underbody);
			}
		}


		public void GetRandomFurColor(out FurColor primary, out FurColor underbody)
		{
			Pair<FurColor, FurColor> retVal = Utils.RandomChoice(furData);
			primary = retVal.first;
			underbody = retVal.second;
		}

		public FurColor defaultFur => new FurColor(HairFurColors.BLACK);
		public FurColor defaultUnderFur => new FurColor(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.MIXED);

		public FurColor defaultFacialFur => defaultUnderFur;
		public FurColor defaultSecondaryFacialFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultTailFur => defaultUnderFur;

		internal Ferret() : base(FerretStr) { }

		public override byte Score(Creature source)
		{
			int ferretCounter = 0;

			if (source.face.type == FaceType.FERRET)
			{
				ferretCounter++;
				if (source.face.isFullMorph)
				{
					ferretCounter++;
				}
			}
			if (source.ears.type == EarType.FERRET)
			{
				ferretCounter++;
			}
			if (source.tail.type == TailType.FERRET)
			{
				ferretCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.FERRET)
			{
				ferretCounter++;
			}
			if (source.arms.type == ArmType.FERRET)
			{
				ferretCounter++;
			}
			if (ferretCounter >= 2 && source.hasPrimaryFur)
			{
				ferretCounter += 2;
			}
			return (byte)Utils.Clamp2(ferretCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Fox : Species
	{
		private readonly FurColor[] singleColors =
		{
			new FurColor(HairFurColors.WHITE),
			new FurColor(HairFurColors.TAN),
			new FurColor(HairFurColors.BROWN)
		};

		private readonly FurColor[] withWhiteUnderbodyColors =
		{
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.RED),
			new FurColor(HairFurColors.BLACK)
		};

		public void GetRandomFurColors(out FurColor primary, out FurColor underbody)
		{
			int randCount = singleColors.Length + withWhiteUnderbodyColors.Length;
			int rand = Utils.Rand(randCount);
			if (rand >= singleColors.Length)
			{
				primary = new FurColor(withWhiteUnderbodyColors[rand - singleColors.Length]);
				underbody = new FurColor(HairFurColors.WHITE);
			}
			else
			{
				primary = new FurColor(singleColors[rand]);
				underbody = new FurColor();
			}
		}

		public FurColor defaultFur => new FurColor(HairFurColors.ORANGE);
		public FurColor defaultUnderbody => new FurColor(HairFurColors.WHITE);

		public FurColor defaultFacialFur => new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.NO_PATTERN);

		public FurColor defaultTailFur => new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.STRIPED);

		public HairFurColors[] AvailableHairColors => new HairFurColors[]
		{
			HairFurColors.GOLDEN_BLONDE, HairFurColors.RED_ORANGE, HairFurColors.SILVER, HairFurColors.WHITE, HairFurColors.RED, HairFurColors.BLACK
		};

		internal Fox() : base(FoxStr) { }

		public override byte Score(Creature source)
		{
			int foxCounter = 0;
			if (source.face.type == FaceType.FOX)
			{
				foxCounter++;
			}
			if (source.ears.type == EarType.FOX)
			{
				foxCounter++;
			}
			if (source.tail.type == TailType.FOX)
			{
				foxCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.FOX)
			{
				foxCounter++;
			}
			if (source.arms.type == ArmType.FOX)
			{
				foxCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.DOG) > 0 && foxCounter > 0)
			{
				foxCounter++;
			}
			if (source.breasts.Count > 1 && foxCounter > 0)
			{
				foxCounter++;
			}
			if (source.breasts.Count == 3 && foxCounter > 0)
			{
				foxCounter++;
			}
			if (source.breasts.Count == 4 && foxCounter > 0)
				foxCounter++;
			//Fur only counts if some fox features are present
			if (source.hasPrimaryFur && foxCounter > 0)
			{
				foxCounter++;
			}
			return (byte)Utils.Clamp2(foxCounter, byte.MinValue, byte.MaxValue);
		}
	}

	//I'm gonna add this at some point, but for now i'll just leave it commented out.

	//public class Gazelle : Species
	//{
	//	internal Gazelle() : base(GazelleStr) { }
	//}

	public class Ghost : Species
	{
		internal Ghost() : base(GhostStr) { }

		public override byte Score(Creature source)
		{
			int ghoulCount = 0;
#warning NYI
			//if (source.combatAbilities.HasSpecialMove<Possession>())
			//{
			//	ghoulCount++;
			//}
			if (ghoulCount > 0 && source.hair.isSemiTransparent)
			{
				ghoulCount++;
			}
			if (ghoulCount > 1 && source.body.type == BodyType.HUMANOID)
			{
				ghoulCount++;
			}
			if (ghoulCount > 1 && source.face.type == FaceType.HUMAN)
			{
				ghoulCount++;
			}
			if (ghoulCount > 1 && source.body.primarySkin.tone == Tones.PALE || source.body.primarySkin.tone == Tones.ALBINO)
			{
				ghoulCount++;
			}
			return (byte)Utils.Clamp2(ghoulCount, byte.MinValue, byte.MaxValue);
		}
	}

	public class Goblin : Species
	{
		public Tones[] availableTones = new Tones[] { Tones.PALE_YELLOW, Tones.GRAYISH_BLUE, Tones.GREEN, Tones.DARK_GREEN };

		public Tones defaultTone => Tones.GREEN;

		internal Goblin() : base(GoblinStr) { }

		public override byte Score(Creature source)
		{
			int goblinCounter = 0;
			if (source.ears.type == EarType.ELFIN)
			{
				goblinCounter++;
			}
			if (availableTones.Contains(source.body.primarySkin.tone))
			{
				goblinCounter++;
			}
			if (goblinCounter > 0)
			{
				if (source.face.type == FaceType.HUMAN)
				{
					goblinCounter++;
				}
				if (source.heightInInches < 48)
				{
					goblinCounter++;
				}
				if (source.hasVagina)
				{
					goblinCounter++;
				}
				if (source.lowerBody.type == LowerBodyType.HUMAN)
				{
					goblinCounter++;
				}
				//if rather fertile.
				if (source.genitals.fertility.baseFertility >= 30 && !source.genitals.fertility.isInfertile)
				{
					goblinCounter++;
					//if REALLY fertile.
					if (source.genitals.fertility.baseFertility >= 50)
					{
						goblinCounter++;
					}
				}
			}
			return (byte)Utils.Clamp2(goblinCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Goo : Species
	{
		public Tones defaultTone => Tones.CERULEAN;

		public Tones[] availableTones => new Tones[] { Tones.GREEN, Tones.PURPLE, Tones.BLUE, Tones.CERULEAN, Tones.EMERALD };

		private readonly Lottery<HairFurColors> hairColors = new Lottery<HairFurColors>();

		internal Goo() : base(GooStr)
		{
			hairColors.AddItem(HairFurColors.GREEN, 30);  // = ColorLists.GOO_MORPH[0]
			hairColors.AddItem(HairFurColors.PURPLE, 20);  // = ColorLists.GOO_MORPH[1]
			hairColors.AddItem(HairFurColors.BLUE, 20);  // = ColorLists.GOO_MORPH[2]
			hairColors.AddItem(HairFurColors.CERULEAN, 20);  // = ColorLists.GOO_MORPH[3]
			hairColors.AddItem(HairFurColors.EMERALD, 10); // = ColorLists.GOO_MORPH[4]
		}

		public HairFurColors[] availableHairColors => new HairFurColors[]
		{
			HairFurColors.GREEN, HairFurColors.PURPLE, HairFurColors.BLUE, HairFurColors.CERULEAN, HairFurColors.EMERALD,
		};

		public HairFurColors GetRandomHairColor()
		{
			return hairColors.Select();
		}

		public override byte Score(Creature source)
		{
			int gooCounter = 0;
			if (source.hair.type == HairType.GOO)
			{
				gooCounter++;
			}
			if (source.body.type == BodyType.GOO)
			{
				gooCounter++;
				if (availableTones.Contains(source.body.primarySkin.tone))
				{
					gooCounter++;
				}
			}
			if (source.lowerBody.type == LowerBodyType.GOO)
			{
				gooCounter++;
			}
			if (source.perks.HasPerk<ElasticInnards>())
			{
				gooCounter++;
			}
			if (source.statusEffects.HasStatusEffect<SlimeCraving>())//could make this a perk, idk. it's the same as the elastic innards perk in terms or requirements.
			{
				gooCounter++;
			}
			return (byte)Utils.Clamp2(gooCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Harpy : Species
	{
		public FurColor defaultFeathers => new FurColor(HairFurColors.WHITE);
		public HairFurColors defaultFeatherHair => HairFurColors.WHITE;
		public FurColor defaultTailFeathers => defaultFeathers;
		internal Harpy() : base(HarpyStr) { }

		public override byte Score(Creature source)
		{
			int harpy = 0;
			if (source.arms.type == ArmType.HARPY)
			{
				harpy++;
			}
			if (source.hair.type == HairType.FEATHER)
			{
				harpy++;
			}
			if (source.wings.type == WingType.FEATHERED)
			{
				harpy++;
			}
			if (source.tail.type == TailType.HARPY)
			{
				harpy++;
			}
			if (source.lowerBody.type == LowerBodyType.HARPY)
			{
				harpy++;
			}
			if (harpy >= 2 && source.face.type == FaceType.HUMAN)
			{
				harpy++;
			}
			if (harpy >= 2 && (source.ears.type == EarType.HUMAN || source.ears.type == EarType.ELFIN))
			{
				harpy++;
			}
			return (byte)Utils.Clamp2(harpy, byte.MinValue, byte.MaxValue);
		}
	}

	public class Horse : Species
	{
		public FurColor defaultFacialFur => new FurColor(HairFurColors.BROWN);
		public FurColor defaultFur => new FurColor(HairFurColors.BROWN);
		public HairFurColors defaultHairColor => HairFurColors.BLACK;
		public FurColor defaultTailFur => new FurColor(defaultHairColor);

		internal Horse() : base(HorseStr) { }

		protected Horse(SimpleDescriptor name) : base(name) { }

		//april fools day will simply replace horse with pony if you match it. otherwise, you'll be a normal horse.
		//the formula should be the same for both of them, idk how to do that.

		public override byte Score(Creature source)
		{
			if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
			{
				return 0;
			}
			else
			{
				return ScoreInternal(source);
			}
		}

		protected byte ScoreInternal(Creature source)
		{
			int horseCounter = 0;
			if (source.face.type == FaceType.HORSE)
			{
				horseCounter++;
			}
			if (source.ears.type == EarType.HORSE)
			{
				horseCounter++;
			}
			if (source.tail.type == TailType.HORSE)
			{
				horseCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.HORSE) > 0)
			{
				horseCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.HOOVED || source.lowerBody.type == LowerBodyType.CLOVEN_HOOVED)
			{
				horseCounter++;
			}
			//Fur only counts if some equine features are present
			if (source.hasPrimaryFur && horseCounter > 0)
			{
				horseCounter++;
			}
			return (byte)Utils.Clamp2(horseCounter, byte.MinValue, byte.MaxValue);
		}
	}
	public class Human : Species
	{
		public Tones[] availableTones => new Tones[] { Tones.LIGHT, Tones.FAIR, Tones.OLIVE, Tones.DARK, Tones.EBONY, Tones.MAHOGANY, Tones.RUSSET };
		public Tones defaultTone => Tones.LIGHT;
		public EyeColor defaultEyeColor => EyeColor.GRAY;
		internal Human() : base(HumanStr) { }

		public override byte Score(Creature source)
		{
			int humanCounter = 0;
			if (source.face.type == FaceType.HUMAN)
			{
				humanCounter++;
			}
			if (source.body.mainEpidermis.type == EpidermisType.SKIN)
			{
				humanCounter++;
			}
			if (source.horns.type == HornType.NONE)
			{
				humanCounter++;
			}
			if (source.tail.type == TailType.NONE)
			{
				humanCounter++;
			}
			if (source.wings.type == WingType.NONE)
			{
				humanCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.HUMAN)
			{
				humanCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.HUMAN) == 1 && source.cocks.Count == 1)
			{
				humanCounter++;
			}
			if (source.breasts.Count == 1 && source.body.mainEpidermis.type == EpidermisType.SKIN)
			{
				humanCounter++;
			}
			return (byte)Utils.Clamp2(humanCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Imp : Species
	{
		public HairFurColors[] availableHairColors => new HairFurColors[]
		{
			HairFurColors.RED,
			HairFurColors.DARK_RED,
		};

		public Tones[] availableTones => new Tones[] { Tones.RED, Tones.ORANGE };
		public Tones defaultTone => Tones.ORANGE;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);
		internal Imp() : base(ImpStr) { }

		public override byte Score(Creature source)
		{
			int impCounter = 0;
			if (source.ears.type == EarType.IMP)
			{
				impCounter++;
			}
			if (source.tail.type == TailType.IMP)
			{
				impCounter++;
			}
			if (source.wings.type == WingType.IMP && source.wings.isLarge)
			{
				impCounter += 2;
			}
			else if (source.wings.type == WingType.IMP)
			{
				impCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.IMP)
			{
				impCounter++;
			}
			if (source.body.mainEpidermis.type == EpidermisType.SKIN && availableTones.Contains(source.body.primarySkin.tone))
			{
				impCounter++;
			}
			if (source.horns.type == HornType.IMP)
			{
				impCounter++;
			}
			if (source.arms.type == ArmType.IMP)
			{
				impCounter++;
			}
			if (source.heightInInches <= 42)
			{
				impCounter++;
			}
			else //if(source.source.heightInInches > 42)
			{
				impCounter--;
			}
			if (source.genitals.BiggestCupSize() > CupSize.FLAT)
			{
				impCounter--;
			}
			if (source.breasts.Count == 2) //Each extra row takes off a point
			{
				impCounter--;
			}
			else if (source.breasts.Count == 3)
			{
				impCounter -= 2;
			}
			else if (source.breasts.Count == 4) //If you have more than 4 why are trying to be an imp{
			{
				impCounter -= 3;
			}
			return (byte)Utils.Clamp2(impCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Kangaroo : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public FurColor defaultUnderbodyFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultUnderbodyFur;

		public FurColor defaultTailFur => defaultFur;
		internal Kangaroo() : base(KangarooStr) { }

		public override byte Score(Creature source)
		{
			int kanga = 0;
			if (source.genitals.CountCocksOfType(CockType.KANGAROO) > 0)
			{
				kanga++;
			}
			if (source.ears.type == EarType.KANGAROO)
			{
				kanga++;
			}
			if (source.tail.type == TailType.KANGAROO)
			{
				kanga++;
			}
			if (source.lowerBody.type == LowerBodyType.KANGAROO)
			{
				kanga++;
			}
			if (source.face.type == FaceType.KANGAROO)
			{
				kanga++;
			}
			if (kanga >= 2 && source.hasPrimaryFur)
			{
				kanga++;
			}
			return (byte)Utils.Clamp2(kanga, byte.MinValue, byte.MaxValue);
		}
	}

	public class Kitsune : Species
	{
		public HairFurColors[] kitsuneHairColors => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.BLACK, HairFurColors.RED };

		public HairFurColors[] elderKitsuneHairColors => new HairFurColors[]
		{
			HairFurColors.GOLDEN,
			HairFurColors.GOLDEN_BLONDE,
			HairFurColors.SILVER,
			HairFurColors.SILVER_BLONDE,
			HairFurColors.SNOW_WHITE,
			HairFurColors.GRAY
		};

		//also allows dual tones with and white. or and grey if white.
		public FurColor[] kitsuneBaseColors => new FurColor[]
		{
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.BLACK),
			new FurColor(HairFurColors.RED),
			new FurColor(HairFurColors.WHITE)
		};

		public FurColor[] kitsuneMixedColors => new FurColor[]
		{
			new FurColor(HairFurColors.ORANGE, HairFurColors.WHITE, FurMulticolorPattern.MIXED),
			new FurColor(HairFurColors.BLACK, HairFurColors.WHITE, FurMulticolorPattern.MIXED),
			new FurColor(HairFurColors.RED, HairFurColors.WHITE, FurMulticolorPattern.MIXED),
			new FurColor(HairFurColors.WHITE, HairFurColors.GRAY, FurMulticolorPattern.MIXED)
		};

		public FurColor[] allKitsuneColors => kitsuneBaseColors.Union(kitsuneMixedColors).ToArray();

		public FurColor[] elderKitsuneFurColors => new FurColor[]
		{
			new FurColor(HairFurColors.GOLDEN),
			new FurColor(HairFurColors.GOLDEN_BLONDE),
			new FurColor(HairFurColors.SILVER),
			new FurColor(HairFurColors.SILVER_BLONDE),
			new FurColor(HairFurColors.SNOW_WHITE),
			new FurColor(HairFurColors.GRAY)
		};

		public Tones[] KitsuneTones => new Tones[] { Tones.TAN, Tones.OLIVE, Tones.LIGHT };

		public Tones[] ElderKitsuneTones => new Tones[] { Tones.DARK, Tones.EBONY, Tones.ASHEN, Tones.SABLE, Tones.MILKY_WHITE };

		public FurColor defaultFacialFur => new FurColor(HairFurColors.WHITE);

		public FurColor defaultFur => kitsuneBaseColors[0];
		public Tones defaultSkin => KitsuneTones[0];

		internal Kitsune() : base(KitsuneStr) { }

		public override byte Score(Creature source)
		{
			int kitsuneCounter = 0;
			//If the character has fox ears, +1
			if (source.ears.type == EarType.FOX)
				kitsuneCounter++;
			//If the character has a fox tail, +1
			if (source.tail.type == TailType.FOX)
				kitsuneCounter++;
			//If the character has two or more fox tails, +2
			if (source.tail.type == TailType.FOX && source.tail.tailCount >= 2)
			{
				kitsuneCounter += 2;
				if (source.tail.tailCount == TailType.FOX.maxTailCount)
				{
					kitsuneCounter++;
				}
			}
			//If the character has tattooed skin, +1
			//9999
			//If the character has a 'vag of holding', +1
			if (source.perks.HasPerk<VagOfHolding>())
			{
				kitsuneCounter++;
			}
			//If the character's kitsune score is greater than 0 and:
			//If the character has a normal face, +1
			if (kitsuneCounter > 0 && (source.face.type == FaceType.HUMAN || source.face.type == FaceType.FOX))
			{
				kitsuneCounter++;
			}
			//If the character's kitsune score is greater than 1 and:
			//If the character has "blonde","black","red","white", or "silver" hair, +1
			if (kitsuneCounter > 1 && (this.kitsuneHairColors.Contains(source.hair.hairColor) || this.elderKitsuneHairColors.Contains(source.hair.hairColor)))
			{
				kitsuneCounter++;
			}
			//If the character's femininity is 40 or higher, +1
			if (kitsuneCounter > 0 && source.genitals.femininity >= 40)
			{
				kitsuneCounter++;
			}
			//type is kitsune: main epidermis is skin, supplementary is fur
			if (source.body.type == BodyType.KITSUNE)
			{
				kitsuneCounter += 2;
				//if the secondary epidermis fur matches an elder or regular color, and the primary epidermis skin is either elder or regular kitsune skin tone.
				if ((elderKitsuneFurColors.Contains(source.body.supplementaryEpidermis.fur) || allKitsuneColors.Contains(source.body.supplementaryEpidermis.fur)) &&
					(ElderKitsuneTones.Contains(source.body.mainEpidermis.tone) || KitsuneTones.Contains(source.body.mainEpidermis.tone)))
				{
					kitsuneCounter++;
				}

			}
			else if (source.hasPrimaryFur && (this.elderKitsuneFurColors.Contains(source.body.mainEpidermis.fur) || this.kitsuneBaseColors.Contains(source.body.mainEpidermis.fur)))
			{
				kitsuneCounter++;
			}
			else if (source.body.mainEpidermis.type == EpidermisType.SKIN && (this.ElderKitsuneTones.Contains(source.body.mainEpidermis.tone) || KitsuneTones.Contains(source.body.mainEpidermis.tone)))
			{
				kitsuneCounter++;
			}
			else if (source.hasPrimaryFur)
			{
				kitsuneCounter--;
			}
			else if (source.body.mainEpidermis.type == EpidermisType.SCALES)
			{
				kitsuneCounter -= 2;
			}
			else if (source.body.mainEpidermis.type == EpidermisType.GOO)
			{
				kitsuneCounter -= 3;
			}
			else if (source.body.mainEpidermis.type != EpidermisType.SKIN)
			{
				kitsuneCounter--;
			}

			//If the character has abnormal legs, -1
			if (source.lowerBody.type != LowerBodyType.HUMAN && source.lowerBody.type != LowerBodyType.FOX)
			{
				kitsuneCounter--;
			}
			//If the character has a nonhuman face, -1
			if (source.face.type != FaceType.HUMAN && source.face.type != FaceType.FOX)
			{
				kitsuneCounter--;
			}
			//If the character has ears other than fox ears, -1
			if (source.ears.type != EarType.FOX)
			{
				kitsuneCounter--;
			}//If the character has tail(s) other than fox tails, -1
			if (source.tail.type != TailType.FOX)
			{
				kitsuneCounter--;
			}
			return (byte)Utils.Clamp2(kitsuneCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Lizard : Species
	{
		public Tones[] availableTones => new Tones[]
		{
			//second colors are never used. idk.
			Tones.PURPLE,
			Tones.SILVER,
			Tones.RED,
			Tones.GREEN,
			Tones.WHITE,
			Tones.BLUE,
			Tones.BLACK
		};

		private static Lottery<Pair<Tones>> theLottery;

		public void GetRandomSkinTone(out Tones primary, out Tones underbody)
		{
			var result = theLottery.Select();

			primary = result.first;
			underbody = result.second;
		}

		static Lizard()
		{
			theLottery = new Lottery<Pair<Tones>>();
			theLottery.AddItem(new Pair<Tones>(Tones.PURPLE, Tones.DEEP_PINK), 5);
			theLottery.AddItem(new Pair<Tones>(Tones.SILVER, Tones.LIGHT_GRAY), 5);
			theLottery.AddItem(new Pair<Tones>(Tones.RED, Tones.ORANGE), 18);
			theLottery.AddItem(new Pair<Tones>(Tones.GREEN, Tones.YELLOWISH_GREEN), 18);
			theLottery.AddItem(new Pair<Tones>(Tones.WHITE, Tones.LIGHT_GRAY), 18);
			theLottery.AddItem(new Pair<Tones>(Tones.BLUE, Tones.CERULEAN), 18);
			theLottery.AddItem(new Pair<Tones>(Tones.BLACK, Tones.DARK_GRAY), 18);
		}


		public Tones defaultTone => Tones.DARK_RED;
		public EyeColor defaultEyeColor => EyeColor.YELLOW;
		public Tones defaultTailTone => defaultTone;
		internal Lizard() : base(LizardStr) { }

		public override byte Score(Creature source)
		{
			int lizardCounter = 0;
			if (source.face.type == FaceType.LIZARD)
			{
				lizardCounter++;
			}
			if (source.ears.type == EarType.LIZARD)
			{
				lizardCounter++;
			}
			if (source.tail.type == TailType.LIZARD)
			{
				lizardCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.LIZARD)
			{
				lizardCounter++;
			}
			if (source.horns.type == HornType.DRACONIC)
			{
				lizardCounter++;
				if (source.horns.numHorns == 4)
				{
					lizardCounter++;
				}
			}
			if (source.arms.type == ArmType.LIZARD)
			{
				lizardCounter++;
			}
			if (lizardCounter > 2)
			{
				if (source.tongue.type == TongueType.LIZARD || source.tongue.type == TongueType.SNAKE)
				{
					lizardCounter++;
				}
				if (source.genitals.CountCocksOfType(CockType.LIZARD) > 0)
				{
					lizardCounter++;
				}
				if (source.eyes.type == EyeType.LIZARD || source.eyes.type == EyeType.BASILISK)
				{
					lizardCounter++;
				}
				if (source.body.type == BodyType.REPTILE)
				{
					lizardCounter++;
				}
			}
			return (byte)Utils.Clamp2(lizardCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Manticore : Species
	{
		internal Manticore() : base(ManticoreStr) { }

		public override byte Score(Creature source)
		{
			int catCounter = 0;
			if (source.face.type == FaceType.CAT)
			{
				catCounter++;
			}
			if (source.ears.type == EarType.CAT)
			{
				catCounter++;
			}
			if (source.tail.type == TailType.SCORPION)
			{
				catCounter += 2;
			}
			if (source.lowerBody.type == LowerBodyType.CAT)
			{
				catCounter++;
			}
			if (catCounter >= 4)
			{
				if (source.horns.type == HornType.DEMON || source.horns.type == HornType.DRACONIC)
				{
					catCounter++;
				}
				if (source.wings.type == WingType.BAT_LIKE || source.wings.type == WingType.DRACONIC)
				{
					catCounter++;
					if (source.wings.canFly)
					{
						catCounter++;
					}
				}
			}
			//Fur only counts if some feline features are present
			if (source.hasPrimaryFur && catCounter >= 6)
			{
				catCounter++;
			}
			return (byte)Utils.Clamp2(catCounter, byte.MinValue, byte.MaxValue);
		}
	}

	//interMod, not implemented here for reasons, idk.
	//public class Mantis : Species
	//{
	//	internal Mantis() : base(MantisStr) { }
	//}

	public class Mouse : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.CHOCOLATE);
		public FurColor defaultFacialFur => defaultFur;

		public FurColor defaultTailFur => defaultFur;
		internal Mouse() : base(MouseStr) { }

		public override byte Score(Creature source)
		{
			int mouseCounter = 0;
			if (source.ears.type == EarType.MOUSE)
			{
				mouseCounter++;
			}
			if (source.tail.type == TailType.MOUSE)
			{
				mouseCounter++;

			}
			if (source.face.type == FaceType.MOUSE)
			{
				mouseCounter++;
				if (source.face.isFullMorph)
				{
					mouseCounter++;
				}
			}
			//Fur only counts if some mouse features are present
			if (source.hasPrimaryFur && mouseCounter > 0)
			{
				mouseCounter++;
			}
			if (source.heightInInches < 55 && mouseCounter > 0)
			{
				mouseCounter++;
			}
			if (source.heightInInches < 45 && mouseCounter > 0)
			{
				mouseCounter++;
			}
			return (byte)Utils.Clamp2(mouseCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Mutant : Species
	{
		public Mutant() : base(MutantStr) { }

		public override byte Score(Creature source)
		{
			int mutantCounter = 0;
			if (source.face.type != FaceType.HUMAN)
			{
				mutantCounter++;
			}
			if (source.tail.type != TailType.NONE)
			{
				mutantCounter++;
			}
			if (source.cocks.Count > 1)
			{
				mutantCounter++;
			}
			if (source.hasCock && source.hasVagina)
			{
				mutantCounter++;
			}
			if (source.vaginas.Count > 1)
			{
				mutantCounter++;
			}
			if (source.genitals.nippleType == NippleStatus.FUCKABLE || source.genitals.nippleType == NippleStatus.DICK_NIPPLE)
			{
				mutantCounter++;
			}
			if (source.breasts.Count > 1)
			{
				mutantCounter++;
			}
			if (source.back.type == BackType.TENDRILS)
			{
				mutantCounter++;
			}
			if (mutantCounter > 1 && source.hasPlainSkin)
			{
				mutantCounter++;
			}
			if (source.face.type == FaceType.HORSE)
			{
				if (source.hasPrimaryFur)
				{
					mutantCounter--;
				}
				if (source.tail.type == TailType.HORSE)
				{
					mutantCounter--;
				}
			}
			if (source.face.type == FaceType.DOG)
			{
				if (source.hasPrimaryFur)
				{
					mutantCounter--;
				}
				if (source.tail.type == TailType.DOG)
				{
					mutantCounter--;
				}
			}
			return (byte)Utils.Clamp2(mutantCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Naga : Species
	{

		private readonly Dictionary<Tones, Tones> bodyLowerMapper = new Dictionary<Tones, Tones>()
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

		public Tones UnderToneFrom(Tones primary, bool fallbackToDefault = true)
		{
			if (bodyLowerMapper.ContainsKey(primary))
			{
				return bodyLowerMapper[primary];
			}
			else if (fallbackToDefault)
			{
				return defaultUnderTone;
			}
			return Tones.NOT_APPLICABLE;
		}
		public Tones defaultTone => Tones.DARK_GREEN;
		public Tones defaultUnderTone => Tones.TAN;

		internal Naga() : base(NagaStr) { }

		public override byte Score(Creature source)
		{
			int nagaCounter = 0;
			if (source.face.type == FaceType.SNAKE)
			{
				nagaCounter++;
			}
			if (source.tongue.type == TongueType.SNAKE)
			{
				nagaCounter++;
			}
			if (source.body.type == BodyType.REPTILE)
			{
				nagaCounter += 2;
			}
			if (nagaCounter > 0 && source.antennae.type == AntennaeType.NONE)
			{
				nagaCounter++;
			}
			if (nagaCounter > 0 && source.wings.type == WingType.NONE)
			{
				nagaCounter++;
			}

			return (byte)Utils.Clamp2(nagaCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Pig : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.PINK);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Pig() : base(PigStr) { }

		public override byte Score(Creature source)
		{
			int pigCounter = 0;
			if (source.ears.type == EarType.PIG)
			{
				pigCounter++;
			}
			if (source.tail.type == TailType.PIG)
			{
				pigCounter++;
			}
			if (source.face.type == FaceType.PIG)
			{
				pigCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.CLOVEN_HOOVED)
			{
				pigCounter += 2;
			}
			if (source.genitals.CountCocksOfType(CockType.PIG) > 0)
			{
				pigCounter++;
			}
			return (byte)Utils.Clamp2(pigCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Pony : Horse
	{
		public FurColor MLP_Fur => new FurColor(HairFurColors.PINK);
		internal Pony() : base(PonyStr) { }

		public override byte Score(Creature source)
		{
			if (DateTime.Now.Day == 1 && DateTime.Now.Month == 4)
			{
				byte horseScore = base.ScoreInternal(source);
				if (horseScore > 1 && source.hasPrimaryFur && source.body.mainEpidermis.fur.Equals(MLP_Fur))
				{
					horseScore.addIn(1);
				}
				return horseScore;
			}
			else
			{
				return 0;
			}
		}
	}

	public class Raccoon : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.DARK_GRAY, HairFurColors.LIGHT_GRAY, FurMulticolorPattern.STRIPED);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Raccoon() : base(RaccoonStr) { }

		public override byte Score(Creature source)
		{
			int coonCounter = 0;

			if (source.face.type == FaceType.RACCOON)
			{
				coonCounter++;
				if (source.face.isFullMorph)
				{
					coonCounter++;
				}
			}
			if (source.ears.type == EarType.RACCOON)
			{
				coonCounter++;
			}
			if (source.tail.type == TailType.RACCOON)
			{
				coonCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.RACCOON)
			{
				coonCounter++;
			}
			if (coonCounter > 0 && source.hasBalls)
			{
				coonCounter++;
			}
			//Fur only counts if some raccoon features are present
			if (source.hasPrimaryFur && coonCounter > 0)
			{
				coonCounter++;
			}
			return (byte)Utils.Clamp2(coonCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class RedPanda : Species
	{
		private readonly Dictionary<FurColor, FurColor> bodyTailMapper = new Dictionary<FurColor, FurColor>()
		{
			{new FurColor(HairFurColors.AUBURN),  new FurColor(HairFurColors.RUSSET)},
			{new FurColor(HairFurColors.BLACK),   new FurColor(HairFurColors.GRAY)},
			{new FurColor(HairFurColors.BLONDE),   new FurColor(HairFurColors.SANDY_BLONDE)},
			{new FurColor(HairFurColors.BROWN),   new FurColor(HairFurColors.AUBURN)},
			{new FurColor(HairFurColors.RED),     new FurColor(HairFurColors.ORANGE)},
			{new FurColor(HairFurColors.WHITE),   new FurColor(HairFurColors.GRAY)},
			{new FurColor(HairFurColors.GRAY),    new FurColor(HairFurColors.WHITE)},
			{new FurColor(HairFurColors.BLUE),    new FurColor(HairFurColors.LIGHT_BLUE)},
			{new FurColor(HairFurColors.GREEN),   new FurColor(HairFurColors.CHARTREUSE)},
			{new FurColor(HairFurColors.ORANGE),  new FurColor(HairFurColors.YELLOW)},
			{new FurColor(HairFurColors.YELLOW),  new FurColor(HairFurColors.SANDY_BLONDE)},
			{new FurColor(HairFurColors.PURPLE),  new FurColor(HairFurColors.PINK)},
			{new FurColor(HairFurColors.PINK),    new FurColor(HairFurColors.PURPLE)},
			{new FurColor(HairFurColors.RAINBOW), new FurColor(HairFurColors.WHITE)},
			{new FurColor(HairFurColors.RUSSET),  new FurColor(HairFurColors.ORANGE)}
		};
		public FurColor[] definedColors => bodyTailMapper.Keys.ToArray();
		public FurColor[] tailColors => bodyTailMapper.Values.ToArray();

		public FurColor tailColorFrom(FurColor source)
		{
			if (bodyTailMapper.ContainsKey(source))
			{
				return new FurColor(bodyTailMapper[source]);
			}
			//dark grey is fallback in code rn.
			return defaultFaceEarTailFur;
		}

		public override byte Score(Creature source)
		{
			byte redPandaCounter = 0;
			if (source.ears.type == EarType.RED_PANDA)
			{
				redPandaCounter++;
			}
			if (source.tail.type == TailType.RED_PANDA)
			{
				redPandaCounter++;
			}
			if (source.arms.type == ArmType.RED_PANDA)
			{
				redPandaCounter++;
			}
			if (source.face.type == FaceType.RED_PANDA)
			{
				redPandaCounter += 2;
			}
			if (source.lowerBody.type == LowerBodyType.RED_PANDA)
			{
				redPandaCounter++;
			}
			if (redPandaCounter >= 2)
			{
				if (source.hasPrimaryFur)
				{
					redPandaCounter++;
				}
				if (source.hasSupplementaryFur)
				{
					redPandaCounter++;
				}
			}
			return redPandaCounter;
		}


		public HairFurColors[] availableHairColors => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.AUBURN, HairFurColors.RED, HairFurColors.RUSSET };

		public HairFurColors defaultHairColor => HairFurColors.WHITE;

		public FurColor defaultFaceEarTailFur => new FurColor(HairFurColors.WHITE);

		public FurColor defaultUnderFur => new FurColor(HairFurColors.BLACK);
		public FurColor defaultFur => new FurColor(HairFurColors.AUBURN);

		internal RedPanda() : base(RedPandaStr) { }
	}

	public class Rhino : Species
	{
		public Tones defaultTone = Tones.DARK_GRAY;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);
		internal Rhino() : base(RhinoStr) { }

		public override byte Score(Creature source)
		{
			int rhinoCounter = 0;
			if (source.ears.type == EarType.RHINO)
			{
				rhinoCounter++;
			}
			if (source.tail.type == TailType.RHINO)
			{
				rhinoCounter++;
			}
			if (source.face.type == FaceType.RHINO)
			{
				rhinoCounter++;
			}
			if (source.horns.type == HornType.RHINO)
			{
				rhinoCounter++;
			}
			if (rhinoCounter >= 2 && source.body.primarySkin.tone == Tones.GRAY || source.body.primarySkin.tone == Tones.DARK_GRAY || source.body.primarySkin.tone == Tones.LIGHT_GRAY)
			{
				rhinoCounter++;
				if (source.body.primarySkin.skinTexture == SkinTexture.ROUGH || source.body.primarySkin.skinTexture == SkinTexture.THICK)
				{
					rhinoCounter++;
				}
			}
			if (rhinoCounter >= 2 && source.genitals.CountCocksOfType(CockType.RHINO) > 0)
			{
				rhinoCounter++;
			}
			return (byte)Utils.Clamp2(rhinoCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Salamander : Species
	{
		public Tones defaultTone => Tones.DARK_RED;

		public Tones[] availableTones => new Tones[] { Tones.LIGHT, Tones.FAIR, Tones.TAN, Tones.DARK, Tones.DARK_RED };

		public Tones defaultTailTone => defaultTone;
		internal Salamander() : base(SalamanderStr) { }

		public override byte Score(Creature source)
		{
			int salamanderCounter = 0;
			if (source.arms.type == ArmType.SALAMANDER)
			{
				salamanderCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.SALAMANDER)
			{
				salamanderCounter++;
			}
			if (source.tail.type == TailType.SALAMANDER)
			{
				salamanderCounter++;
			}
			if (source.perks.HasPerk<Lustzerker>())
			{
				salamanderCounter++;
			}
			if (salamanderCounter >= 2)
			{
				if (source.genitals.CountCocksOfType(CockType.LIZARD) > 0)
				{
					salamanderCounter++;
				}
				if (source.face.type == FaceType.SNAKE || source.face.type == FaceType.HUMAN)
				{
					salamanderCounter++;
				}
				if (source.ears.type == EarType.HUMAN || source.ears.type == EarType.DRAGON || source.ears.type == EarType.LIZARD)
				{
					salamanderCounter++;
				}
			}
			return (byte)Utils.Clamp2(salamanderCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class SandTrap : Species
	{
		public EyeColor defaultEyeColor => EyeColor.BLACK;
		internal SandTrap() : base(SandTrapStr) { }

		public override byte Score(Creature source)
		{
			int counter = 0;
			if (source.genitals.hasBlackNipples)
			{
				counter++;
			}
			if (source.balls.uniBall)
			{
				counter++;
			}
			if (source.genitals.CountVaginasOfType(VaginaType.SAND_TRAP) > 0)
			{
				counter++;
			}
			if (source.eyes.type == EyeType.SAND_TRAP)
			{
				counter++;
			}
			if (source.wings.type == WingType.DRAGONFLY)
			{
				counter++;
			}
			if (source.genitals.femininity.isAndrogynous && counter > 0)
			{
				counter++;
			}
			return (byte)Utils.Clamp2(counter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Shark : Species
	{
		public Tones defaultTone => Tones.GRAY;
		public Tones defaultTailTone => defaultTone;
		internal Shark() : base(SharkStr) { }

		public override byte Score(Creature source)
		{
			int sharkCounter = 0;
			if (source.face.type == FaceType.SHARK)
			{
				sharkCounter++;
			}
			if (source.gills.type == GillType.FISH)
			{
				sharkCounter++;
			}
			if (source.back.type == BackType.SHARK_FIN)
			{
				sharkCounter++;
			}
			if (source.tail.type == TailType.SHARK)
			{
				sharkCounter++;
			}
			//skin counting only if PC got any other shark traits
			if (source.hasPlainSkin && sharkCounter > 0)
			{
				sharkCounter++;
				if (source.body.primarySkin.skinTexture == SkinTexture.ROUGH || source.body.primarySkin.skinTexture == SkinTexture.THICK)
				{
					sharkCounter++;
				}
			}
			return (byte)Utils.Clamp2(sharkCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Sheep : Species
	{
		public FurColor defaultColor => new FurColor(HairFurColors.WHITE);
		public FurColor defaultTailFur => defaultColor;
		internal Sheep() : base(SheepStr) { }

		public override byte Score(Creature source)
		{
			int sheepCounter = 0;
			if (source.ears.type == EarType.SHEEP)
			{
				sheepCounter++;
			}
			if (source.horns.type == HornType.SHEEP)
			{
				sheepCounter++;
			}
			if (source.horns.type == HornType.GOAT)
			{
				sheepCounter++;
			}
			if (source.tail.type == TailType.SHEEP)
			{
				sheepCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.CLOVEN_HOOVED)
			{
				sheepCounter++;
			}
			if (source.hair.type == HairType.WOOL)
			{
				sheepCounter++;
			}
			if (source.body.type == BodyType.WOOL)
			{
				sheepCounter++;
			}
			return (byte)Utils.Clamp2(sheepCounter, byte.MinValue, byte.MaxValue);
		}
	}

	public class Siren : Species
	{
		public Siren() : base(SirenStr) { }

		public override byte Score(Creature source)
		{
			int sirenCounter = 0;
			if (source.face.type == FaceType.SHARK && source.tail.type == TailType.SHARK && source.wings.type == WingType.FEATHERED && source.arms.type == ArmType.HARPY)
			{
				sirenCounter += 4;
			}
			if (sirenCounter > 0 && source.hasVagina)
			{
				sirenCounter++;
			}
			if (sirenCounter > 0 && source.hasCock && source.genitals.CountCocksOfType(CockType.ANEMONE) > 0)
			{
				sirenCounter++;
			}
			return (byte)Utils.Clamp2(sirenCounter, byte.MinValue, byte.MaxValue);
		}

	}

	public class Spider : Species
	{
		public EyeColor defaultEyeColor => EyeColor.BLACK;

		public Tones defaultTone => Tones.BLACK;
		public Tones defaultAbdomenTone => Tones.BLACK;
		internal Spider() : base(SpiderStr) { }

		public override byte Score(Creature source)
		{
			int score = 0;
			if (source.eyes.type == EyeType.SPIDER)
			{
				score += 2;
			}
			if (source.face.type == FaceType.SPIDER)
			{
				score++;
			}
			if (source.arms.type == ArmType.SPIDER)
			{
				score++;
			}
			if (source.lowerBody.type == LowerBodyType.CHITINOUS_SPIDER || source.lowerBody.type == LowerBodyType.DRIDER)
			{
				score += 2;
			}
			else if (score > 0)
			{
				score--;
			}
			if (source.ovipositor.type == OvipositorType.SPIDER)
			{
				score += 2;
			}
			if (source.tail.type == TailType.SPIDER_SPINNERET)
			{
				score++;
			}
			//carapace =+1. Skin = +/-0. All else = -1
			if (source.body.mainEpidermis.type == EpidermisType.CARAPACE)
			{
				score++;
			}
			else if (source.body.mainEpidermis.type != EpidermisType.SKIN && score > 0)
			{
				score--;
			}
			return (byte)Utils.Clamp2(score, byte.MinValue, byte.MaxValue);
		}
	}

	public class Unicorn : Horse
	{
		internal Unicorn() : base(UnicornStr) { }

		public override byte Score(Creature source)
		{
			if (DateTime.Now.Day == 1 && DateTime.Now.Month == 4)
			{
				return 0;
			}
			else
			{
				byte horseScore = base.Score(source);
				if (source.horns.type == HornType.UNICORN)
				{
					horseScore.addIn(1);
				}
				return horseScore;
			}

		}
	}

	public class Wolf : Species
	{
		public EyeColor defaultEyeColor => EyeColor.AMBER;

		public FurColor defaultFurColor => new FurColor(HairFurColors.BLACK, HairFurColors.GRAY, FurMulticolorPattern.NO_PATTERN);
		public FurColor defaultFacialFur => new FurColor(HairFurColors.GRAY);
		public FurColor defaultTailFur => new FurColor(HairFurColors.GRAY);

		internal Wolf() : base(WolfStr) { }

		public override byte Score(Creature source)
		{
			int wolfCounter = 0;
			if (source.face.type == FaceType.WOLF)
			{
				wolfCounter++;
			}
			if (source.genitals.CountCocksOfType(CockType.WOLF) > 0)
			{
				wolfCounter++;
			}
			if (source.ears.type == EarType.WOLF)
			{
				wolfCounter++;
			}
			if (source.tail.type == TailType.WOLF)
			{
				wolfCounter++;
			}
			if (source.lowerBody.type == LowerBodyType.WOLF)
			{
				wolfCounter++;
			}
			if (source.eyes.type == EyeType.WOLF)
			{
				wolfCounter += 2;
			}
			if (source.hasPrimaryFur && wolfCounter > 0) //Only counts if we got wolf features
			{
				wolfCounter++;
			}
			if (wolfCounter >= 2)
			{
				if (source.breasts.Count > 1)
				{
					wolfCounter++;
				}
				if (source.breasts.Count == 4)
				{
					wolfCounter++;
				}
				if (source.breasts.Count > 4)
					wolfCounter--;
			}
			return (byte)Utils.Clamp2(wolfCounter, byte.MinValue, byte.MaxValue);
		}

		public Pair<FurColor>[] availableFurColors { get; } = new Pair<FurColor>[]
		{
			new Pair<FurColor>(new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.GRAY), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.DARK_GRAY), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.LIGHT_GRAY), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.BLACK), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.LIGHT_BROWN), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.SANDY_BROWN), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.GOLDEN), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.SILVER), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.BROWN), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.AUBURN), new FurColor(HairFurColors.NO_HAIR_FUR)),
			new Pair<FurColor>(new FurColor(HairFurColors.BLACK), new FurColor(HairFurColors.GRAY)),
			new Pair<FurColor>(new FurColor(HairFurColors.BLACK), new FurColor(HairFurColors.BROWN)),
			new Pair<FurColor>(new FurColor(HairFurColors.BLACK), new FurColor(HairFurColors.SILVER)),
			new Pair<FurColor>(new FurColor(HairFurColors.BLACK), new FurColor(HairFurColors.AUBURN)),
			new Pair<FurColor>(new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.GRAY)),
			new Pair<FurColor>(new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.SILVER)),
			new Pair<FurColor>(new FurColor(HairFurColors.WHITE), new FurColor(HairFurColors.GOLDEN)),
		};

		internal void GetRandomFurColors(out FurColor primary, out FurColor underbody)
		{
			var result = Utils.RandomChoice(availableFurColors);

			primary = result.first;
			underbody = result.second;

		}
	}
}
