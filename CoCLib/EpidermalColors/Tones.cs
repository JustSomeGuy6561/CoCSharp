//Tones.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 1:02 AM
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CoC.EpidermalColors
{
	//"Why do this - we had strings. strings were fine! why overdesign the wheel?"
	//well, because we had several switch statements manually parse a string to find a close color.
	//which you can do with RGB values automatically. and adding a color here is (relatively) painless.
	internal class Tones : EpidermalColors
	{
		//I probably missed one somewhere. sue me.
		protected static List<Tones> availableTones = new List<Tones>();

		public static readonly Tones NOT_APPLICABLE = new Tones();

		public static readonly Tones ALBINO = new Tones(Color.Beige, "albino");
		public static readonly Tones AUBURN = new Tones(Color.FromArgb(153, 51, 51), "auburn");
		public static readonly Tones BIRCH_WHITE = new Tones(Color.FromArgb(253, 253, 218), "birch-white");
		public static readonly Tones BLACK = new Tones(Color.Black);
		public static readonly Tones BLUE = new Tones(Color.Blue);
		public static readonly Tones BLUE_BLACK = new Tones(Color.FromArgb(0, 0, 55), "blue-black");
		public static readonly Tones BRONZED = new Tones(Color.FromArgb(205, 127, 50), "bronzen");
		public static readonly Tones BROWN = new Tones(Color.Brown);
		public static readonly Tones CERULEAN = new Tones(Color.Azure, "cerulean");
		public static readonly Tones CREAMY_WHITE = new Tones(Color.MintCream, "creamy-white");
		public static readonly Tones CRIMSON = new Tones(Color.Crimson);
		public static readonly Tones DARK_BROWN = new Tones(Color.FromArgb(107, 67, 33), "dark brown");
		public static readonly Tones DARK_GREEN = new Tones(Color.DarkGreen); //Use For emerald as well. fuck it.
		public static readonly Tones DARK_RED = new Tones(Color.DarkRed);
		public static readonly Tones DEEP_BLUE = new Tones(Color.DarkBlue, "deep blue");
		public static readonly Tones DUSKY = new Tones(Color.DarkSlateGray, "dusky");
		public static readonly Tones DARK = new Tones(Color.FromArgb(65, 22, 0), "dark");
		public static readonly Tones EBONY = new Tones(Color.SaddleBrown, "ebony");
		public static readonly Tones FAIR = new Tones(Color.SandyBrown, "fair");
		public static readonly Tones GRAY = new Tones(Color.Gray);
		public static readonly Tones GRAYISH_BLUE = new Tones(Color.DarkSlateBlue, "grayish-blue");
		public static readonly Tones GREEN = new Tones(Color.Green);
		public static readonly Tones GREENISH_GRAY = new Tones(Color.FromArgb(105, 160, 105), "greenish-gray");
		public static readonly Tones GREY_GREEN = new Tones(Color.FromArgb(108, 147, 108), "grey-green");
		public static readonly Tones INDIGO = new Tones(Color.Indigo);
		public static readonly Tones IVORY = new Tones(Color.Ivory);
		public static readonly Tones LIGHT_GREEN = new Tones(Color.LightGreen);
		public static readonly Tones LIGHT_PURPLE = new Tones(Color.Plum, "light purple");
		public static readonly Tones LIGHT = new Tones(Color.Bisque, "light");
		public static readonly Tones MAHOGANY = new Tones(Color.FromArgb(192, 64, 0), "mahogany");
		public static readonly Tones MEDITERRANEAN = new Tones(Color.Peru, "mediterranean");
		public static readonly Tones METALLIC = new Tones(Color.Silver, "metallic");
		public static readonly Tones MIDNIGHT_BLUE = new Tones(Color.MidnightBlue);
		public static readonly Tones OLIVE = new Tones(Color.Olive);
		public static readonly Tones ORCHID = new Tones(Color.MediumOrchid, "orchid");
		public static readonly Tones ORANGE = new Tones(Color.Orange);
		public static readonly Tones PALE_YELLOW = new Tones(Color.FromArgb(255, 255, 153), "pale-yellow");
		public static readonly Tones PEARL = new Tones(Color.FromArgb(248, 245, 237), "pearl-white"); //Milky white
		public static readonly Tones PALE = new Tones(Color.BlanchedAlmond, "pale");
		public static readonly Tones PINK = new Tones(Color.Pink);
		public static readonly Tones PURPLE = new Tones(Color.Purple);
		public static readonly Tones RED = new Tones(Color.DarkRed, "red"); //well, red seems too strong.
		public static readonly Tones RUSSET = new Tones(Color.FromArgb(128, 70, 27), "russet");
		public static readonly Tones SABLE = new Tones(Color.FromArgb(134, 92, 67), "sable");
		public static readonly Tones SANGUINE = new Tones(Color.FromArgb(108, 55, 54), "sanguine");
		public static readonly Tones TAN = new Tones(Color.Tan);
		public static readonly Tones TAWNY = new Tones(Color.FromArgb(205, 87, 0), "tawny"); //= #cd5700
		public static readonly Tones WHITE = new Tones(Color.White); //= #ffffff
		public static readonly Tones WOODLY_BROWN = new Tones(Color.FromArgb(85, 67, 55), "wooden brown");
		public static readonly Tones YELLOW = new Tones(Color.Yellow);
		public static readonly Tones YELLOWISH_GREEN = new Tones(Color.YellowGreen);

		public static readonly Tones HUMAN_DEFAULT = LIGHT;

		public static readonly Tones SPIDER_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones BEE_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones DRAGON_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones IMP_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones LIZARD_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones SALAMANDER_DEFAULT = throw new System.NotImplementedException();
		public static readonly Tones COCKATRICE_DEFAULT = throw new System.NotImplementedException();


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

		public static Tones NearestTone(HairFurColors currentHairFur)
		{
			//edge cases. they'll already do weird shit, so just randomize it.
			//you really should check for these first.
			if (currentHairFur == HairFurColors.RAINBOW || currentHairFur == HairFurColors.NO_HAIR_FUR)
			{
				return Tools.Utils.RandomChoice(availableTones.ToArray());
			}
			return NearestTone(currentHairFur);
		}

		public static Tones NearestTone(Color color)
		{
			return availableTones.Aggregate((c, d) => c.compare(color) < d.compare(color) ? c : d);
		}

		public static List<Tones> allAvailableTones()
		{
			return new List<Tones>(availableTones);
		}

		private double compare(Color color)
		{
			return Tools.Utils.weightedColorCompare(color, rgbValue);
		}

	}
}
