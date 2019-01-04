//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Items
{
	public enum FurMulticolorPattern { STRIPED, SPOTTED, MIXED, NO_PATTERN }
	public class FurColor
	{
		protected FurMulticolorPattern multiColorPattern;
		public HairFurColors primaryColor { get; protected set; }
		public HairFurColors secondaryColor { get; protected set; }
		public bool isMultiColored { get; private set; }

		protected FurColor()
		{
			isMultiColored = false;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			primaryColor = HairFurColors.NO_HAIR_FUR;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
		}

		public static FurColor Generate(HairFurColors primary)
		{
			FurColor retVal = new FurColor
			{
				primaryColor = primary
			};
			return retVal;
		}

		public void UpdateFurColor(FurColor other)
		{
			this.isMultiColored = other.isMultiColored;
			this.primaryColor = other.primaryColor;
			this.secondaryColor = other.secondaryColor;
			this.multiColorPattern = other.multiColorPattern;
		}

		public void UpdateFurColor(HairFurColors primary)
		{
			isMultiColored = false;
			primaryColor = primary;
		}

		public void UpdateFurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			isMultiColored = true;
			primaryColor = primary;
			secondaryColor = secondary;
			multiColorPattern = pattern;
		}

		public static readonly FurColor NO_FUR = new FurColor();

		//ToDo: move these strings to a strings class.
		public string AsString()
		{
			if (!isMultiColored)
			{
				return primaryColor.AsString();
			}
			else
			{
				switch (multiColorPattern)
				{
					
					case FurMulticolorPattern.SPOTTED:
						return primaryColor.AsString() + " with " + secondaryColor.AsString() + " spots";
					case FurMulticolorPattern.STRIPED:
						return primaryColor.AsString() + " with " + secondaryColor.AsString() + " stripes";
					case FurMulticolorPattern.MIXED:
						return "mixed " + primaryColor.AsString() + " and " + secondaryColor.AsString();
					case FurMulticolorPattern.NO_PATTERN:
					default:
						return primaryColor.AsString() + " and " + secondaryColor.AsString();
				}
			}
		}

	}


}
