//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM
using System.Drawing;

namespace CoC.EpidermalColors
{
	public enum FurMulticolorPattern { STRIPED, SPOTTED, MIXED, NO_PATTERN }
	internal class FurColor
	{
		protected FurMulticolorPattern multiColorPattern;
		public HairFurColors primaryColor { get; protected set; }
		public HairFurColors secondaryColor { get; protected set; }
		public bool isMultiColored { get; private set; }

		protected FurColor()
		{
			Reset();
		}
		public static FurColor GenerateEmpty()
		{
			return new FurColor();
		}
		public static FurColor Generate(HairFurColors primary)
		{
			return new FurColor
			{
				primaryColor = primary
			};
		}

		public static FurColor GenerateFromOther(FurColor other)
		{
			return new FurColor
			{
				primaryColor = other.primaryColor,
				secondaryColor = other.secondaryColor,
				isMultiColored = other.isMultiColored,
				multiColorPattern = other.multiColorPattern
			};
		}

		public static FurColor Generate(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			return new FurColor
			{
				primaryColor = primary,
				secondaryColor = secondary,
				multiColorPattern = pattern,
				isMultiColored = true
			};
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

		public void Reset()
		{
			isMultiColored = false;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			primaryColor = HairFurColors.NO_HAIR_FUR;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
		}

		public bool isNoFur()
		{
			return primaryColor.rgbValue == Color.Transparent && secondaryColor.rgbValue == Color.Transparent;
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


		public static FurColor HARPY_DEFAULT => throw new System.NotImplementedException();
		public static FurColor DOG_DEFAULT => throw new System.NotImplementedException();
		public static FurColor RED_PANDA_DEFAULT => throw new System.NotImplementedException();
		public static FurColor FERRET_DEFAULT => throw new System.NotImplementedException();
		public static FurColor CAT_DEFAULT => throw new System.NotImplementedException();



	}


}
