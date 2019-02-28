//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM
using System.Drawing;

namespace CoC.Backend.CoC_Colors
{
	public enum FurMulticolorPattern { STRIPED, SPOTTED, MIXED, NO_PATTERN }
	public class FurColor
	{
		public FurMulticolorPattern multiColorPattern { get; protected set; }
		public HairFurColors primaryColor { get; protected set; }
		public HairFurColors secondaryColor { get; protected set; }
		public bool isMultiColored { get; private set; }

		public FurColor()
		{
			Reset();
		}

		public FurColor(HairFurColors primary)
		{
			primaryColor = primary;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			isMultiColored = false;
		}

		public FurColor(FurColor other)
		{
			primaryColor = other.primaryColor;
			secondaryColor = other.secondaryColor;
			isMultiColored = other.isMultiColored;
			multiColorPattern = other.multiColorPattern;
		}

		public FurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary;
			secondaryColor = secondary;
			multiColorPattern = pattern;
			isMultiColored = true;
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
			secondaryColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
		}

		public void UpdateFurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			isMultiColored = true;
			primaryColor = primary;
			secondaryColor = secondary;
			multiColorPattern = pattern;
		}

		public void Reset()
		{
			isMultiColored = false;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			primaryColor = HairFurColors.NO_HAIR_FUR;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
		}

		public bool isNoFur()
		{
			return primaryColor.rgbValue == Color.Transparent;
		}

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