//EpidermalColors.cs
//Description:
//Author: JustSomeGuy
//12/31/2018, 2:35 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CoC.EpidermalColors
{
	public abstract class EpidermalColors
	{
		//RGB is used for comparing colors. it factors in when trying to convert one color to something else 
		protected EpidermalColors(Color rgb, string colorName)
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

		protected EpidermalColors(Color rgb)
		{
			rgbValue = rgb;
			value = rgb.Name;
		}

		public string AsString()
		{
			return value;
		}

		public readonly string value;
		public readonly Color rgbValue;
	}
}
