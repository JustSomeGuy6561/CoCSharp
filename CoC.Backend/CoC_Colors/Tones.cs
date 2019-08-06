//Tones.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:02 AM
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CoC.Backend.CoC_Colors
{
	//"Why do this - we had strings. strings were fine! why overdesign the wheel?"
	//well, because we had several switch statements manually parse a string to find a close color.
	//which you can do with RGB values automatically. and adding a color here is (relatively) painless.

	public class Tones : CoCColors
	{
		//I probably missed one somewhere. sue me.
		protected static HashSet<Tones> availableTones = new HashSet<Tones>();

		public static readonly Tones NOT_APPLICABLE = new Tones();

		public static readonly Tones ALBINO = new Tones(Color.Beige, "Albino");
		public static readonly Tones ASHEN = new Tones(Color.FromArgb(0xca, 0xbf, 0xb2), "Ashen");
		public static readonly Tones AUBURN = new Tones(Color.FromArgb(153, 51, 51), "Auburn");
		public static readonly Tones BIRCH_WHITE = new Tones(Color.FromArgb(253, 253, 218), "Birch-White");
		public static readonly Tones BLACK = new Tones(Color.Black);
		public static readonly Tones BLUE = new Tones(Color.Blue);
		public static readonly Tones BLUE_BLACK = new Tones(Color.FromArgb(0, 0, 55), "Blue-Black");
		public static readonly Tones BRONZED = new Tones(Color.FromArgb(205, 127, 50), "Bronzen");
		public static readonly Tones BROWN = new Tones(Color.Brown);
		public static readonly Tones CERULEAN = new Tones(Color.Azure, "Cerulean");
		public static readonly Tones CREAMY_WHITE = new Tones(Color.MintCream, "Creamy-White");
		public static readonly Tones CRIMSON = new Tones(Color.Crimson);
		public static readonly Tones CYAN = new Tones(Color.Cyan);
		public static readonly Tones DARK_BROWN = new Tones(Color.FromArgb(107, 67, 33), "DarkBrown");
		public static readonly Tones DARK_GRAY = new Tones(Color.DarkGray);
		public static readonly Tones DARK_GREEN = new Tones(Color.DarkGreen);
		public static readonly Tones DARK_RED = new Tones(Color.DarkRed);
		public static readonly Tones DEEP_BLUE = new Tones(Color.DarkBlue, "DeepBlue");
		public static readonly Tones DEEP_PINK = new Tones(Color.DeepPink);
		public static readonly Tones DUSKY = new Tones(Color.DarkSlateGray, "Dusky");
		public static readonly Tones DARK = new Tones(Color.FromArgb(65, 22, 0), "Dark");
		public static readonly Tones EBONY = new Tones(Color.SaddleBrown, "Ebony");
		public static readonly Tones EMERALD = new Tones(Color.FromArgb(0x50, 0xc8, 0x78), "Emerald");
		public static readonly Tones FAIR = new Tones(Color.SandyBrown, "Fair");
		public static readonly Tones GOLD = new Tones(Color.Gold);
		public static readonly Tones GRAY = new Tones(Color.Gray);
		public static readonly Tones GRAYISH_BLUE = new Tones(Color.DarkSlateBlue, "Grayish-Blue");
		public static readonly Tones GREEN = new Tones(Color.Green);
		public static readonly Tones GREENISH_GRAY = new Tones(Color.FromArgb(105, 160, 105), "Greenish-Gray");
		public static readonly Tones GREY_GREEN = new Tones(Color.FromArgb(108, 147, 108), "Grey-Green");
		public static readonly Tones INDIGO = new Tones(Color.Indigo);
		public static readonly Tones IVORY = new Tones(Color.Ivory);
		public static readonly Tones LIGHT_BLUE = new Tones(Color.LightBlue);
		public static readonly Tones LIGHT_GRAY = new Tones(Color.LightGray);
		public static readonly Tones LIGHT_GREEN = new Tones(Color.LightGreen);
		public static readonly Tones LIGHT_PURPLE = new Tones(Color.Plum, "LightPurple");
		public static readonly Tones LIGHT = new Tones(Color.Bisque, "Light");
		public static readonly Tones MAHOGANY = new Tones(Color.FromArgb(192, 64, 0), "Mahogany");
		public static readonly Tones MAGENTA = new Tones(Color.Magenta);
		public static readonly Tones MEDITERRANEAN = new Tones(Color.Peru, "Mediterranean");
		public static readonly Tones METALLIC = new Tones(Color.Silver, "Metallic");
		public static readonly Tones MIDNIGHT_BLUE = new Tones(Color.MidnightBlue);
		public static readonly Tones MILKY_WHITE = new Tones(Color.FromArgb(248, 245, 237), "Milky-White"); //Milky white
		public static readonly Tones OLIVE = new Tones(Color.Olive);
		public static readonly Tones ORCHID = new Tones(Color.MediumOrchid, "Orchid");
		public static readonly Tones ORANGE = new Tones(Color.Orange);
		public static readonly Tones PALE_YELLOW = new Tones(Color.FromArgb(255, 255, 153), "Pale-Yellow");
		public static readonly Tones PALE_PINK = new Tones(Color.FromArgb(0xfa, 0xda, 0xdd), "Pale-Pink");
		public static readonly Tones PEARL = new Tones(Color.FloralWhite, "Pearl-White");
		public static readonly Tones PALE = new Tones(Color.BlanchedAlmond, "Pale");
		public static readonly Tones PINK = new Tones(Color.Pink);
		public static readonly Tones PURPLE = new Tones(Color.Purple);
		public static readonly Tones RED = new Tones(Color.DarkRed, "Red"); //well, red seems too strong.
		public static readonly Tones RUSSET = new Tones(Color.FromArgb(128, 70, 27), "Russet");
		public static readonly Tones SABLE = new Tones(Color.FromArgb(134, 92, 67), "Sable");
		public static readonly Tones SANGUINE = new Tones(Color.FromArgb(108, 55, 54), "Sanguine");
		public static readonly Tones SILVER = new Tones(Color.Silver);
		public static readonly Tones SPRING_GREEN = new Tones(Color.SpringGreen);
		public static readonly Tones TAN = new Tones(Color.Tan);
		public static readonly Tones TAWNY = new Tones(Color.FromArgb(205, 87, 0), "Tawny"); //= #cd5700
		public static readonly Tones TURQUOISE = new Tones(Color.Turquoise);
		public static readonly Tones WHITE = new Tones(Color.White); //= #ffffff
		public static readonly Tones WOODLY_BROWN = new Tones(Color.FromArgb(85, 67, 55), "WoodenBrown");
		public static readonly Tones YELLOW = new Tones(Color.Yellow);
		public static readonly Tones YELLOWISH_GREEN = new Tones(Color.YellowGreen);



		//Don't add transparent to the available colors.
		//DO NOT USE THIS. IT'S ONLY HERE FOR DEFAULT!
		private Tones() : base(Color.Transparent, "") { }


		private Tones(Color rgb, string colorName) : base(rgb, colorName)
		{
			availableTones.Add(this);
		}

		private Tones(Color rgb) : base(rgb)
		{
			availableTones.Add(this);
		}

		public bool isEmpty => this == NOT_APPLICABLE;

		public static bool IsNullOrEmpty(Tones tone)
		{
			return tone == null || tone == NOT_APPLICABLE;
		}

		public static Tones NearestTone(HairFurColors currentHairFur)
		{
			//edge cases. they'll already do weird shit, so just randomize it.
			//you really should check for these first.
			if (currentHairFur == HairFurColors.RAINBOW || HairFurColors.IsNullOrEmpty(currentHairFur))
			{
				return NOT_APPLICABLE;
			}
			return NearestTone(currentHairFur);
		}

		public static Tones NearestTone(Color color)
		{
			return availableTones.Aggregate((c, d) => c.compare(color) < d.compare(color) ? c : d);
		}

		public static HashSet<Tones> allAvailableTones()
		{
			return new HashSet<Tones>(availableTones);
		}

		private double compare(Color color)
		{
			return WeightedColorCompare(color, rgbValue);
		}

		internal static Tones Deserialize(Color color)
		{
			if (color == Color.Transparent)
			{
				return Tones.NOT_APPLICABLE;
			}
			else return NearestTone(color);
		}
	}
}
