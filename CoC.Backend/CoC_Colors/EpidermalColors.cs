//EpidermalColors.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 2:35 AM
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace CoC.Backend.CoC_Colors
{
	[DataContract]
	public abstract class CoCColors
	{
		//RGB is used for comparing colors. it factors in when trying to convert one color to something else 
		protected CoCColors(Color rgb, string colorName)
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

		protected CoCColors(Color rgb)
		{
			rgbValue = rgb;
			value = rgb.Name;
		}

		public string AsString()
		{
			return value;
		}

		protected readonly string value;
		public readonly Color rgbValue;


		public static double WeightedColorCompare(Color first, Color second)
		{
			return Math.Pow((second.R - first.R) * 0.30, 2)
				+ Math.Pow((second.G - first.G) * 0.59, 2)
				+ Math.Pow((second.B - first.B) * 0.11, 2);
		}
	}
}
