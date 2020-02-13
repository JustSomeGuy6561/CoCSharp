//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoC.Backend.CoC_Colors
{
	public enum FurMulticolorPattern { NO_PATTERN, STRIPED, SPOTTED, MIXED }

	public class FurColor : IEquatable<FurColor>, IEquatable<HairFurColors>, IEquatable<ReadOnlyFurColor>
	{
		public FurMulticolorPattern multiColorPattern { get; protected set; }
		public HairFurColors primaryColor { get; protected set; }
		public HairFurColors alternateColor { get; protected set; }
		public bool isMultiColored => !alternateColor.isEmpty && primaryColor != alternateColor && primaryColor != alternateColor;

		public HairFurColors GetSecondaryColor() => alternateColor.isEmpty ? primaryColor : alternateColor;

		public FurColor() : base()
		{
			Reset();
		}

		public static FurColor Empty => new FurColor();

		public FurColor(HairFurColors primary)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			alternateColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
		}

		public FurColor(FurColor other)
		{
			if (other == null) throw new ArgumentNullException();
			primaryColor = other.primaryColor;
			alternateColor = other.alternateColor;
			multiColorPattern = other.multiColorPattern;
			validateData();
		}

		public FurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			alternateColor = secondary ?? throw new ArgumentNullException();
			multiColorPattern = pattern;
			validateData();
		}
		public void UpdateFurColor(FurColor other)
		{
			if (other == null) throw new ArgumentNullException();
			primaryColor = other.primaryColor;
			alternateColor = other.alternateColor;
			multiColorPattern = other.multiColorPattern;
			validateData();
		}

		public void UpdateFurColor(HairFurColors primary)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			alternateColor = HairFurColors.NO_HAIR_FUR;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			validateData();
		}

		public void UpdateFurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary ?? throw new ArgumentNullException();
			alternateColor = secondary ?? throw new ArgumentNullException();
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
			alternateColor = HairFurColors.NO_HAIR_FUR;
		}

		private void validateData()
		{
			if (HairFurColors.IsNullOrEmpty(primaryColor) && HairFurColors.IsNullOrEmpty(alternateColor))
			{
				primaryColor = HairFurColors.NO_HAIR_FUR;
				alternateColor = HairFurColors.NO_HAIR_FUR;
			}
			if (HairFurColors.IsNullOrEmpty(primaryColor) && !HairFurColors.IsNullOrEmpty(alternateColor))
			{
				primaryColor = alternateColor;
				alternateColor = HairFurColors.NO_HAIR_FUR;
			}

			if (primaryColor == alternateColor && primaryColor != HairFurColors.NO_HAIR_FUR) //prevent weird "brown fur with brown spots" - that's just "brown fur".
			{
				alternateColor = HairFurColors.NO_HAIR_FUR;
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

		public bool IsIdenticalTo(ReadOnlyFurColor furColor)
		{
			return !(furColor is null) && furColor.isMultiColored == isMultiColored && (!isMultiColored || furColor.pattern == multiColorPattern)
				&& furColor.primary == primaryColor && furColor.secondary == this.alternateColor;
		}

		public string AsString(bool withArticle = false)
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
						return primaryColor.AsString(withArticle) + " with " + alternateColor.AsString() + " spots";
					case FurMulticolorPattern.STRIPED:
						return primaryColor.AsString(withArticle) + " with " + alternateColor.AsString() + " stripes";
					case FurMulticolorPattern.MIXED:
						return (withArticle ? "a " : "") + "mixed " + primaryColor.AsString() + " and " + alternateColor.AsString();
					case FurMulticolorPattern.NO_PATTERN:
					default:
						return primaryColor.AsString(withArticle) + " and " + alternateColor.AsString();
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
				   EqualityComparer<HairFurColors>.Default.Equals(alternateColor, color.alternateColor);
		}

		public bool Equals(HairFurColors other)
		{
			return IsIdenticalTo(other);
		}

		public bool Equals(ReadOnlyFurColor other)
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
				hashCode = hashCode * -1521134295 + EqualityComparer<HairFurColors>.Default.GetHashCode(alternateColor);
				return hashCode;
			}
		}

		public ReadOnlyFurColor AsReadOnly()
		{
			return new ReadOnlyFurColor(this);
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
			throw new InvalidEnumArgumentException("Either a non-valid FurMulticolorPattern Enum value was used, or a new fur pattern was added, but the AsString function was not updated.");
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
			secondary = source?.alternateColor ?? HairFurColors.NO_HAIR_FUR;
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

		public string AsString()
		{
			if (!isMultiColored)
			{
				return primary.AsString();
			}
			else
			{
				switch (pattern)
				{

					case FurMulticolorPattern.SPOTTED:
						return primary.AsString() + " with " + secondary.AsString() + " spots";
					case FurMulticolorPattern.STRIPED:
						return primary.AsString() + " with " + secondary.AsString() + " stripes";
					case FurMulticolorPattern.MIXED:
						return "mixed " + primary.AsString() + " and " + secondary.AsString();
					case FurMulticolorPattern.NO_PATTERN:
					default:
						return primary.AsString() + " and " + secondary.AsString();
				}
			}
		}

		public bool Equals(ReadOnlyFurColor other)
		{
			return other != null && primary == other.primary && secondary == other.secondary && pattern == other.pattern;
		}
		public bool Equals(FurColor other)
		{
			return other != null && primary == other.primaryColor && secondary == other.alternateColor && pattern == other.multiColorPattern;
		}

		public bool Equals(HairFurColors other)
		{
			return other != null && primary == other && secondary.isEmpty;
		}

	}
}
