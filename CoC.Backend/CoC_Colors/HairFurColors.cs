﻿//HairFurColors.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 7:56 PM
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

//using CoC.Internal.

namespace CoC.Backend.CoC_Colors
{
	//"Why do this - we had strings. strings were fine! why overdesign the wheel?"
	//well, because we had several switch statements manually parse a string to find a close color.
	//which you can do with RGB values automatically. and adding a color here is (relatively) painless.

	public class HairFurColors : CoCColors
	{
		private const int RAINBOW_HACK = unchecked((int)0xaa18855e);
		private static readonly List<HairFurColors> supportedColors = new List<HairFurColors>();

		public static readonly HairFurColors[] DYE_COLORS = { BLACK, AUBURN, BLONDE, BLUE, BROWN, GRAY, GREEN, ORANGE, PINK, PURPLE, RAINBOW, RED, RUSSET, YELLOW, WHITE };

		public static readonly HairFurColors NO_HAIR_FUR = new HairFurColors();

		public static readonly HairFurColors AUBURN = new HairFurColors(Color.FromArgb(153, 51, 51), "Auburn");
		public static readonly HairFurColors BLACK = new HairFurColors(Color.FromArgb(10, 10, 10), "Black");
		public static readonly HairFurColors BLONDE = new HairFurColors(Color.Goldenrod, "Blonde");
		public static readonly HairFurColors BLUE = new HairFurColors(Color.Blue);
		public static readonly HairFurColors BROWN = new HairFurColors(Color.Brown);
		public static readonly HairFurColors CARAMEL = new HairFurColors(Color.PeachPuff, "Caramel");
		public static readonly HairFurColors CERULEAN = new HairFurColors(Color.Azure, "Cerulean");
		public static readonly HairFurColors CHARCOAL = new HairFurColors(Color.SlateGray, "Charcoal");
		public static readonly HairFurColors CHARTREUSE = new HairFurColors(Color.YellowGreen, "Chartreuse");
		public static readonly HairFurColors CHOCOLATE = new HairFurColors(Color.Chocolate);
		public static readonly HairFurColors DARK_BLUE = new HairFurColors(Color.DarkBlue);
		public static readonly HairFurColors DARK_BROWN = new HairFurColors(Color.FromArgb(107, 67, 33), "DarkBrown");
		public static readonly HairFurColors DARK_GRAY = new HairFurColors(Color.DarkGray);
		public static readonly HairFurColors DARK_GREEN = new HairFurColors(Color.DarkGreen);
		public static readonly HairFurColors DARK_RED = new HairFurColors(Color.DarkRed);
		public static readonly HairFurColors DEEP_RED = new HairFurColors(Color.Magenta, "DeepRed");
		public static readonly HairFurColors EMERALD = new HairFurColors(Color.FromArgb(80, 200, 120), "Emerald");
		public static readonly HairFurColors GOLDEN = new HairFurColors(Color.Gold);
		public static readonly HairFurColors GOLDEN_BLONDE = new HairFurColors(Color.FromArgb(243, 219, 95), "Golden-Blonde");
		public static readonly HairFurColors GRAY = new HairFurColors(Color.Gray);
		public static readonly HairFurColors GRAYISH_BLUE = new HairFurColors(Color.DarkSlateBlue, "Grayish-Blue");
		public static readonly HairFurColors GREEN = new HairFurColors(Color.Green);
		public static readonly HairFurColors GREY_GREEN = new HairFurColors(Color.FromArgb(108, 147, 108), "Grey-Green");
		public static readonly HairFurColors INDIGO = new HairFurColors(Color.Indigo);
		public static readonly HairFurColors LIGHT_BLONDE = new HairFurColors(Color.LightGoldenrodYellow, "LightBlonde");
		public static readonly HairFurColors LIGHT_BROWN = new HairFurColors(Color.Peru, "LightBrown");
		public static readonly HairFurColors LIGHT_BLUE = new HairFurColors(Color.LightBlue);
		public static readonly HairFurColors LIGHT_GRAY = new HairFurColors(Color.LightGray);
		public static readonly HairFurColors MIDNIGHT_BLACK = new HairFurColors(Color.Black, "MidnightBlack");
		public static readonly HairFurColors ORANGE = new HairFurColors(Color.Orange);
		public static readonly HairFurColors PEACH = new HairFurColors(Color.LightCoral, "Peach");
		public static readonly HairFurColors PINK = new HairFurColors(Color.Pink);
		public static readonly HairFurColors PLATINUM_BLONDE = new HairFurColors(Color.FromArgb(244, 255, 155), "Platinum-Blonde");
		public static readonly HairFurColors PURPLE = new HairFurColors(Color.Purple);
		public static readonly HairFurColors PURPLISH_BLACK = new HairFurColors(Color.FromArgb(52, 0, 52), "DarkPurple");
		public static readonly HairFurColors RED = new HairFurColors(Color.Red, "Red"); //well, red seems too strong.
		public static readonly HairFurColors RED_ORANGE = new HairFurColors(Color.OrangeRed, "Red-Orange");
		public static readonly HairFurColors RUSSET = new HairFurColors(Color.FromArgb(128, 70, 27), "Russet");
		public static readonly HairFurColors SANDY_BLONDE = new HairFurColors(Color.PaleGoldenrod, "Sandy-Blonde");
		public static readonly HairFurColors SANDY_BROWN = new HairFurColors(Color.Tan, "Sandy-Brown");
		public static readonly HairFurColors SILVER = new HairFurColors(Color.Silver); //metalic silver == "Shiny" Silver. or "Metallic" silver
		public static readonly HairFurColors SILVER_BLONDE = new HairFurColors(Color.FromArgb(238, 229, 128), "Silvery-Blonde");
		public static readonly HairFurColors SILVER_WHITE = new HairFurColors(Color.Gainsboro, "Silver-White");
		public static readonly HairFurColors SNOW_WHITE = new HairFurColors(Color.White, "Snow-White");
		public static readonly HairFurColors TAN = new HairFurColors(Color.Tan);
		public static readonly HairFurColors TURQUOISE = new HairFurColors(Color.Turquoise);
		public static readonly HairFurColors WHITE = new HairFurColors(Color.Linen, "White"); //= #ffffff
		public static readonly HairFurColors YELLOW = new HairFurColors(Color.Yellow);
		//thanks, rainbow. Hacking it in with a random RGB and an alpha. all other colors (except transparent) use full alpha, meaning this will be unique
		public static readonly HairFurColors RAINBOW = new HairFurColors(Color.FromArgb(RAINBOW_HACK), "Rainbow-Colored");


		//Don't add transparent to the available colors.
		//DO NOT USE THIS. IT'S ONLY HERE FOR DEFAULT!
		private HairFurColors() : base(Color.Transparent, "") { }

		protected HairFurColors(Color rgb, string colorName) : base(rgb, colorName)
		{
			supportedColors.Add(this);
		}

		protected HairFurColors(Color rgb) : base(rgb)
		{
			supportedColors.Add(this);
		}

		public bool isEmpty => this == NO_HAIR_FUR;

		public static bool IsNullOrEmpty(HairFurColors color)
		{
			return color == null || color == NO_HAIR_FUR;
		}

		public static List<HairFurColors> AvailableHairFurColors()
		{
			return new List<HairFurColors>(supportedColors);
		}

		//useful for things like basilisk spines which forces hair to act like scales
		public static HairFurColors NearestHairFurColor(Tones currentTone)
		{
			//you really should check for this first.
			if (Tones.IsNullOrEmpty(currentTone))
			{
				return NO_HAIR_FUR;
			}
			return NearestHairFurColor(currentTone.rgbValue);
		}

		public static HairFurColors NearestHairFurColor(Color currentTone)
		{
			return supportedColors.Aggregate((c, d) => c.compare(currentTone) < d.compare(currentTone) ? c : d);
		}

		//perhaps we don't want to use that one, idk.
		private double compare(Color color)
		{
			return WeightedColorCompare(color, rgbValue);
		}
		internal static HairFurColors Deserialize(Color color)
		{
			if (color == Color.Transparent)
			{
				return HairFurColors.NO_HAIR_FUR;
			}
			else if (color.ToArgb() == RAINBOW_HACK)
			{
				return HairFurColors.RAINBOW;
			}
			else return NearestHairFurColor(color);
		}
	}
}
