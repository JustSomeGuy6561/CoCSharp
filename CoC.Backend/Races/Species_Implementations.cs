//Species_Implementations.cs
//Description: Implementation of the specific races. It may be better practice for each of these to be their own class
//but for faster development this is easier for now.
//Author: JustSomeGuy
//2/20/2019, 5:46 PM

using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Backend.Races
{

	public class Anemone : Species
	{
		public HairFurColors defaultHair => HairFurColors.CERULEAN;
		public Tones defaultTone => Tones.CERULEAN;
		internal Anemone() : base(AnemoneStr) { }
	}

	public class Basilisk : Species
	{
		public HairFurColors defaultSpines = HairFurColors.GREEN;
		public HairFurColors defaultPlume = HairFurColors.RED;
		public Tones defaultTone => Tones.GREEN;
		public EyeColor defaultEyeColor => EyeColor.GRAY;

		public HairFurColors ToNearestSpineColor(HairFurColors currentColor)
		{
#warning Fix Me!
			return currentColor;
		}

		internal Basilisk() : base(BasiliskStr) { }
	}

	public class Bee : Species
	{
		public HairFurColors defaultHair => HairFurColors.BLACK;
		public FurColor defaultFur => new FurColor(HairFurColors.BLACK, HairFurColors.YELLOW, FurMulticolorPattern.STRIPED);
		public Tones defaultTone => Tones.BLACK;
		public Tones defaultAbdomenTone => defaultTone;
		internal Bee() : base(BeeStr) { }
	}
	public class Behemoth : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.DARK_RED);
		public FurColor defaultTailFur => defaultFur;
		internal Behemoth() : base(BehemothStr) { }
	}

	public class Boar : Species
	{
		internal Boar() : base(BoarStr) { }
	}

	public class Bunny : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Bunny() : base(BunnyStr) { }
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
	}

	public class Centaur : Species
	{
		internal Centaur() : base(CentaurStr) { }
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

		public FurColor[] availablePrimaryFurColors => new FurColor[]
		{
			new FurColor(HairFurColors.BLUE),
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.GREEN),
			new FurColor(HairFurColors.PURPLE),
			new FurColor(HairFurColors.BLACK),
			new FurColor(HairFurColors.BLONDE),
			new FurColor(HairFurColors.WHITE),
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

		public FurColor[] availableSecondaryFurColors => new FurColor[]
		{
			new FurColor(HairFurColors.TURQUOISE),
			new FurColor(HairFurColors.RED),
			new FurColor(HairFurColors.YELLOW),
			new FurColor(HairFurColors.PINK),
			new FurColor(HairFurColors.WHITE),
			new FurColor(HairFurColors.BROWN),
			new FurColor(HairFurColors.GRAY),
		};

		public void GetRandomCockatriceColors(out FurColor primaryFeathers, out Tones secondaryScales)
		{
			FurColor[] primaryColors = availablePrimaryFurColors;
			Tones[] secondaryTones = availableTones;
			int rand = Utils.Rand(primaryColors.Length);
			primaryFeathers = primaryColors[rand];
			if (rand >= secondaryTones.Length)
			{
				secondaryScales = secondaryTones[secondaryTones.Length - 1];
			}
			else secondaryScales = secondaryTones[rand];
		}

		public FurColor GetSecondaryCockatriceFeathers(FurColor primaryColor)
		{
			FurColor[] primaryColors = availablePrimaryFurColors;
			FurColor[] secondaryColors = availableSecondaryFurColors;
			bool predicate(FurColor x) => x == primaryColor;
			int y = Array.FindIndex(primaryColors, predicate);
			if (y != -1)
			{
				if (y >= secondaryColors.Length)
				{
					return secondaryColors[secondaryColors.Length - 1];
				}
				else return secondaryColors[y];
			}
			else return new FurColor(HairFurColors.YELLOW);
		}

		public FurColor defaultPrimaryFeathers => new FurColor(HairFurColors.BLUE);
		public Tones defaultScaleTone => Tones.BLUE;
		public FurColor defaultSecondaryFeathers => new FurColor(HairFurColors.TURQUOISE);
		public FurColor defaultTailFeaithers => defaultSecondaryFeathers;
		public EyeColor defaultEyeColor => EyeColor.BLUE;
		internal Cockatrice() : base(CockatriceStr) { }
	}

	public class Cow : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.WHITE, HairFurColors.BLACK, FurMulticolorPattern.SPOTTED);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Cow() : base(CowStr) { }
	}

	public class Deer : Species
	{
		public FurColor defaultFurColor => new FurColor(HairFurColors.LIGHT_BROWN);
		public FurColor defaultYouthFurColor => new FurColor(HairFurColors.LIGHT_BROWN, HairFurColors.WHITE, FurMulticolorPattern.SPOTTED); //unused atm.
		public FurColor defaultTailFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultFurColor;

		internal Deer() : base(DeerStr) { }
	}

	public class Demon : Species
	{
		public Tones defaultTone => Tones.DARK_RED;
		public Tones defaultTailTone => defaultTone;
		internal Demon() : base(DemonStr) { }
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
	}

	public class Dryad : Species
	{
		public Tones defaultBarkColor => Tones.WOODLY_BROWN;
		public HairFurColors defaultVineColor => HairFurColors.GREEN;
		internal Dryad() : base(DryadStr) { }
	}

	public class Echidna : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public Tones defaultSpineColor => Tones.IVORY; //unused atm.

		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);

		internal Echidna() : base(EchidnaStr) { }
	}

	public class Ferret : Species
	{

		public HairFurColors[] availableHair => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.BROWN, HairFurColors.CARAMEL };

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
		public FurColor defaultTailFur =>defaultUnderFur;

		internal Ferret() : base(FerretStr) { }
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
		internal Fox() : base(FoxStr) { }
	}

	public class Gazelle : Species
	{
		internal Gazelle() : base(GazelleStr) { }
	}

	public class Ghost : Species
	{
		internal Ghost() : base(GhostStr) { }
	}

	public class Goat : Species
	{
		public FurColor defaultWool => new FurColor(HairFurColors.WHITE);
		public FurColor defaultTailFur => defaultWool;
		internal Goat() : base(GoatStr) { }
	}

	public class Goblin : Species
	{
		public Tones[] availableTones = new Tones[] { Tones.PALE_YELLOW, Tones.GRAYISH_BLUE, Tones.GREEN, Tones.DARK_GREEN };

		public Tones defaultTone => Tones.GREEN;

		internal Goblin() : base(GoblinStr) { }
	}

	public class Goo : Species
	{
		public Tones defaultTone => Tones.CERULEAN;

		public Tones[] availableTones => new Tones[] { Tones.GREEN, Tones.PURPLE, Tones.BLUE, Tones.CERULEAN, Tones.EMERALD };

		internal Goo() : base(GooStr) { }
	}

	public class Harpy : Species
	{
		public FurColor defaultFeathers => new FurColor(HairFurColors.WHITE);
		public HairFurColors defaultFeatherHair => HairFurColors.WHITE;
		public FurColor defaultTailFeathers => defaultFeathers;
		internal Harpy() : base(HarpyStr) { }
	}

	public class Horse : Species
	{
		public FurColor defaultFacialFur => new FurColor(HairFurColors.BROWN);
		public FurColor defaultFur => new FurColor(HairFurColors.BROWN);
		public HairFurColors defaultHairColor => HairFurColors.BLACK;
		public FurColor defaultTailFur => new FurColor(defaultHairColor);

		internal Horse() : base(HorseStr) { }
	}
	public class Human : Species
	{
		public Tones[] availableTones => new Tones[] { Tones.LIGHT, Tones.FAIR, Tones.OLIVE, Tones.DARK, Tones.EBONY, Tones.MAHOGANY, Tones.RUSSET };
		public Tones defaultTone => Tones.LIGHT;
		public EyeColor defaultEyeColor => EyeColor.GRAY;
		internal Human() : base(HumanStr) { }
	}

	public class Imp : Species
	{
		public Tones[] availableTones => new Tones[] { Tones.RED, Tones.ORANGE };
		public Tones defaultTone => Tones.ORANGE;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);
		internal Imp() : base(ImpStr) { }
	}

	public class Kangaroo : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.LIGHT_BROWN);
		public FurColor defaultUnderbodyFur => new FurColor(HairFurColors.WHITE);
		public FurColor defaultFacialFur => defaultUnderbodyFur;

		public FurColor defaultTailFur => defaultFur;
		internal Kangaroo() : base(KangarooStr) { }
	}

	public class Kitsune : Species
	{
		public HairFurColors[] kitsuneHairColors => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.BLACK, HairFurColors.RED };

		public FurColor[] kitsuneFurColors => new FurColor[]
		{
			new FurColor(HairFurColors.ORANGE),
			new FurColor(HairFurColors.BLACK),
			new FurColor(HairFurColors.RED),
			new FurColor(HairFurColors.WHITE)
		};

		public void GetRandomKitsuneFurColors(out FurColor primary, out FurColor underbody)
		{
			FurColor[] colors = kitsuneFurColors;



			primary = Utils.RandomChoice(kitsuneFurColors);
			if (Utils.RandBool())
			{
				underbody = new FurColor(HairFurColors.WHITE);
				if (primary.primaryColor == HairFurColors.WHITE)
				{
					underbody.UpdateFurColor(HairFurColors.GRAY);
				}
			}
			else underbody = new FurColor();
		}


		public FurColor[] elderKitsuneFurColors => new FurColor[] {
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

		public FurColor defaultFur => kitsuneFurColors[0];
		public Tones defaultSkin => KitsuneTones[0];

		internal Kitsune() : base(KitsuneStr) { }
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

		private static Lottery<Tones> theLottery = new Lottery<Tones>();

		public Tones GetRandomSkinTone()
		{
			return theLottery.Select();
		}

		static Lizard()
		{
			theLottery.addItem(Tones.PURPLE, 5);
			theLottery.addItem(Tones.SILVER, 5);
			theLottery.addItem(Tones.RED, 18);
			theLottery.addItem(Tones.GREEN, 18);
			theLottery.addItem(Tones.WHITE, 18);
			theLottery.addItem(Tones.BLUE, 18);
			theLottery.addItem(Tones.BLACK, 18);
		}


		public Tones defaultTone => Tones.DARK_RED;
		public EyeColor defaultEyeColor => EyeColor.YELLOW;
		public Tones defaultTailTone => defaultTone;
		internal Lizard() : base(LizardStr) { }
	}

	public class Mantis : Species
	{
		internal Mantis() : base(MantisStr) { }
	}

	public class Mouse : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.CHOCOLATE);
		public FurColor defaultFacialFur => defaultFur;

		public FurColor defaultTailFur =>defaultFur;
		internal Mouse() : base(MouseStr) { }
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
	}

	public class Pig : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.PINK);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Pig() : base(PigStr) { }
	}

	public class Pony : Species
	{
		public FurColor MLP_Fur => new FurColor(HairFurColors.PINK);
		internal Pony() : base(PonyStr) { }
	}

	public class Raccoon : Species
	{
		public FurColor defaultFur => new FurColor(HairFurColors.DARK_GRAY, HairFurColors.LIGHT_GRAY, FurMulticolorPattern.STRIPED);
		public FurColor defaultFacialFur => defaultFur;
		public FurColor defaultTailFur => defaultFur;
		internal Raccoon() : base(RaccoonStr) { }
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

		public HairFurColors[] availableHairColors => new HairFurColors[] { HairFurColors.WHITE, HairFurColors.AUBURN, HairFurColors.RED, HairFurColors.RUSSET };

		public HairFurColors defaultHairColor => HairFurColors.WHITE;

		public FurColor defaultFaceEarTailFur => new FurColor(HairFurColors.WHITE);

		public FurColor defaultUnderFur => new FurColor(HairFurColors.BLACK);
		public FurColor defaultFur => new FurColor(HairFurColors.AUBURN);
		
		internal RedPanda() : base(RedPandaStr) { }
	}

	public class Reptile : Species
	{
		internal Reptile() : base(ReptileStr) { }
	}

	public class Rhino : Species
	{
		public Tones defaultTone = Tones.DARK_GRAY;
		public FurColor defaultTailFur => new FurColor(HairFurColors.BLACK);
		internal Rhino() : base(RhinoStr) { }
	}

	public class Salamander : Species
	{
		public Tones defaultTone => Tones.DARK_RED;

		public Tones[] availableTones => new Tones[] { Tones.LIGHT, Tones.FAIR, Tones.TAN, Tones.DARK, Tones.DARK_RED };

		public Tones defaultTailTone => defaultTone;
		internal Salamander() : base(SalamanderStr) { }
	}

	public class SandTrap : Species
	{
		public EyeColor defaultEyeColor => EyeColor.BLACK;
		internal SandTrap() : base(SandTrapStr) { }
	}

	public class Scorpion : Species
	{
		public Tones defaultTailTone => Tones.TAN;
		internal Scorpion() : base(ScorpionStr) { }
	}

	public class Shark : Species
	{
		public Tones defaultTone => Tones.GRAY;
		public Tones defaultTailTone => defaultTone;
		internal Shark() : base(SharkStr) { }
	}

	public class Sheep : Species
	{
		public FurColor defaultColor => new FurColor(HairFurColors.WHITE);
		public FurColor defaultTailFur => defaultColor;
		internal Sheep() : base(SheepStr) { }
	}

	public class Snake : Species
	{
		internal Snake() : base(SnakeStr) { }
	}

	public class Spider : Species
	{
		public EyeColor defaultEyeColor => EyeColor.BLACK;

		public Tones defaultTone => Tones.BLACK;
		public Tones defaultAbdomenTone =>Tones.BLACK;
		internal Spider() : base(SpiderStr) { }
	}

	public class Unicorn : Species
	{
		internal Unicorn() : base(UnicornStr) { }
	}

	public class Wolf : Species
	{
		public EyeColor defaultEyeColor => EyeColor.AMBER;

		public FurColor defaultFurColor => new FurColor(HairFurColors.BLACK, HairFurColors.GRAY, FurMulticolorPattern.NO_PATTERN);
		public FurColor defaultFacialFur => new FurColor(HairFurColors.GRAY);
		public FurColor defaultTailFur => new FurColor(HairFurColors.GRAY);

		internal Wolf() : base(WolfStr) { }
	}
}
