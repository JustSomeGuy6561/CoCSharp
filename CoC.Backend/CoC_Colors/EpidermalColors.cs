//EpidermalColors.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 2:35 AM
using System;
using System.Drawing;

namespace CoC.Backend.CoC_Colors
{

	public abstract class CoCColors
	{
		//RGB is used for comparing colors. it factors in when trying to convert one color to something else
		protected CoCColors(Color rgb, string colorName) : base()
		{
			rgbValue = rgb;
			if (!string.IsNullOrWhiteSpace(colorName))
			{
				value = colorName;
			}
			else
			{
				value = rgb.Name;
			}
		}

		protected CoCColors(Color rgb) : base()
		{
			rgbValue = rgb;
			value = rgb.Name;
		}

		public string AsString(bool lowerCase = true)
		{
			if (lowerCase)
			{
				return value.ToLower();
			}
			else
			{
				return value;
			}
		}

		protected readonly string value;
		public readonly Color rgbValue;


		public static double WeightedColorCompare(Color first, Color second)
		{
			return Math.Pow((second.R - first.R) * 0.30, 2)
				+ Math.Pow((second.G - first.G) * 0.59, 2)
				+ Math.Pow((second.B - first.B) * 0.11, 2);
		}

		public static double WeightedColorComparePercent(Color first, Color second)
		{
			return Math.Pow((second.R - first.R) * 0.30, 2)
				+ Math.Pow((second.G - first.G) * 0.59, 2)
				+ Math.Pow((second.B - first.B) * 0.11, 2);
		}
	}
}
