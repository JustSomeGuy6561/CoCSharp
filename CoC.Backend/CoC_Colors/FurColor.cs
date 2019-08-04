//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM

using System;
using System.Collections.Generic;

namespace CoC.Backend.CoC_Colors
{
	public enum FurMulticolorPattern { NO_PATTERN, STRIPED, SPOTTED, MIXED }

	public class FurColor : IEquatable<FurColor>, IEquatable<HairFurColors>
	{
		public FurMulticolorPattern multiColorPattern { get; protected set; }
		public HairFurColors primaryColor { get; protected set; }
		public HairFurColors secondaryColor { get; protected set; }
		public bool isMultiColored => !secondaryColor.isEmpty && primaryColor != secondaryColor && primaryColor != secondaryColor;

		public FurColor() : base()
		{
			Reset();
		}

		public static FurColor Empty => new FurColor();

		public FurColor(HairFurColors primary)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			secondaryColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
		}

		public FurColor(FurColor other)
		{
			if (other == null) throw new ArgumentNullException();
			primaryColor = other.primaryColor;
			secondaryColor = other.secondaryColor;
			multiColorPattern = other.multiColorPattern;
			validateData();
		}

		public FurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			secondaryColor = secondary ?? throw new ArgumentNullException();
			multiColorPattern = pattern;
			validateData();
		}
		public void UpdateFurColor(FurColor other)
		{
			if (other == null) throw new ArgumentNullException();
			primaryColor = other.primaryColor;
			secondaryColor = other.secondaryColor;
			multiColorPattern = other.multiColorPattern;
			validateData();
		}

		public void UpdateFurColor(HairFurColors primary)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			secondaryColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			validateData();
		}

		public void UpdateFurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			secondaryColor = secondary ?? throw new ArgumentNullException();
			multiColorPattern = pattern;
			validateData();
		}

		public void UpdateFurPattern(FurMulticolorPattern newPattern)
		{
			multiColorPattern = newPattern;
			validateData();
		}

		public void Reset()
		{
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			primaryColor = HairFurColors.NO_HAIR_FUR;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
		}

		private void validateData()
		{
			if (HairFurColors.IsNullOrEmpty(primaryColor) && HairFurColors.IsNullOrEmpty(secondaryColor))
			{
				primaryColor = HairFurColors.NO_HAIR_FUR;
				secondaryColor = HairFurColors.NO_HAIR_FUR;
			}
			if (HairFurColors.IsNullOrEmpty(primaryColor) && !HairFurColors.IsNullOrEmpty(secondaryColor))
			{
				primaryColor = secondaryColor;
				secondaryColor = HairFurColors.NO_HAIR_FUR;
			}

			if (primaryColor == secondaryColor && primaryColor != HairFurColors.NO_HAIR_FUR) //prevent weird "brown fur with brown spots" - that's just "brown fur".
			{
				secondaryColor = HairFurColors.NO_HAIR_FUR;
			}

			if (!isMultiColored)
			{
				multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			}
		}

		public bool isEmpty => primaryColor.isEmpty; //primary color cannot be null.





		public static bool IsNullOrEmpty(FurColor furColor)
		{
			return furColor == null || furColor.isEmpty;
		}

		public bool IsIdenticalTo(HairFurColors hairColor)
		{
			return (isEmpty && hairColor?.isEmpty == true) || (!isMultiColored && hairColor == primaryColor);
		}

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

		public override bool Equals(object other)
		{
			if (other is null || !(other is FurColor || other is HairFurColors))
			{
				return false;
			}
			else if (other is FurColor furColor)
			{
				return Equals(furColor);
			}
			else if (other is HairFurColors hairFur)
			{
				return Equals(hairFur);
			}
			return false;
		}

		public bool Equals(FurColor color)
		{
			return color != null &&
				   multiColorPattern == color.multiColorPattern &&
				   EqualityComparer<HairFurColors>.Default.Equals(primaryColor, color.primaryColor) &&
				   EqualityComparer<HairFurColors>.Default.Equals(secondaryColor, color.secondaryColor);
		}

		public bool Equals(HairFurColors other)
		{
			return IsIdenticalTo(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = -1882447767;
				hashCode = hashCode * -1521134295 + multiColorPattern.GetHashCode();
				hashCode = hashCode * -1521134295 + EqualityComparer<HairFurColors>.Default.GetHashCode(primaryColor);
				hashCode = hashCode * -1521134295 + EqualityComparer<HairFurColors>.Default.GetHashCode(secondaryColor);
				return hashCode;
			}
		}
	}

	public static class FurColorHelper
	{
		public static string AsString(this FurMulticolorPattern pattern)
		{
			switch (pattern)
			{
				case FurMulticolorPattern.MIXED:
					return "Mixed";
				case FurMulticolorPattern.NO_PATTERN:
					return "No Pattern";
				case FurMulticolorPattern.SPOTTED:
					return "Spotted";
				case FurMulticolorPattern.STRIPED:
					return "Striped";
			}
			throw new NotImplementedException("A new fur pattern was added, but the AsString function was not updated.");
		}
	}

	public class ReadOnlyFurColor : IEquatable<ReadOnlyFurColor>, IEquatable<FurColor>, IEquatable<HairFurColors>
	{
		public readonly HairFurColors primary;
		public readonly HairFurColors secondary;
		public readonly FurMulticolorPattern pattern;

		public bool isMultiColored => !secondary.isEmpty && primary != secondary;


		public ReadOnlyFurColor(FurColor source)
		{

			primary = source?.primaryColor ?? HairFurColors.NO_HAIR_FUR;
			secondary = source?.secondaryColor ?? HairFurColors.NO_HAIR_FUR;
			pattern = source?.multiColorPattern ?? FurMulticolorPattern.NO_PATTERN;
		}

		public static ReadOnlyFurColor GenerateAllowNulls(FurColor source)
		{
			if (source == null)
			{
				return null;
			}
			return new ReadOnlyFurColor(source);
		}

		public bool Equals(ReadOnlyFurColor other)
		{
			return other != null && primary == other.primary && secondary == other.secondary && pattern == other.pattern;
		}
		public bool Equals(FurColor other)
		{
			return other != null && primary == other.primaryColor && secondary == other.secondaryColor && pattern == other.multiColorPattern;
		}

		public bool Equals(HairFurColors other)
		{
			return other != null && primary == other && secondary.isEmpty;
		}

	}
}