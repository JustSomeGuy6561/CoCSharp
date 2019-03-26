//FurColor.cs
//Description:
//Author: JustSomeGuy
//1/3/2019, 1:53 PM
using CoC.Backend.Save;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace CoC.Backend.CoC_Colors
{
	public enum FurMulticolorPattern {NO_PATTERN, STRIPED, SPOTTED, MIXED  }
	[DataContract]
	public class FurColor : ISaveableBase
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
			validateData();
		}

		public FurColor(HairFurColors primary, HairFurColors secondary, FurMulticolorPattern pattern)
		{
			primaryColor = primary;
			secondaryColor = secondary;
			multiColorPattern = pattern;
			isMultiColored = true;
			validateData();
		}
		public void UpdateFurColor(FurColor other)
		{
			this.isMultiColored = other.isMultiColored;
			this.primaryColor = other.primaryColor;
			this.secondaryColor = other.secondaryColor;
			this.multiColorPattern = other.multiColorPattern;
			validateData();
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
			validateData();
		}

		public void Reset()
		{
			isMultiColored = false;
			multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			primaryColor = HairFurColors.NO_HAIR_FUR;
			secondaryColor = HairFurColors.NO_HAIR_FUR;
		}

		private void validateData()
		{
			if (HairFurColors.isNullOrEmpty(primaryColor) && !HairFurColors.isNullOrEmpty(secondaryColor))
			{
				primaryColor = secondaryColor;
				secondaryColor = HairFurColors.NO_HAIR_FUR;
			}
			if (HairFurColors.isNullOrEmpty(secondaryColor))
			{
				multiColorPattern = FurMulticolorPattern.NO_PATTERN;
			}
		}

		public bool isEmpty => primaryColor.isEmpty; //primary color cannot be null.

		public static bool isNullOrEmpty(FurColor furColor)
		{
			return furColor == null || furColor.isEmpty;
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

		Type ISaveableBase.currentSaveType => typeof(FurSurrogateVersion1);
		Type[] ISaveableBase.saveVersionTypes => new Type[] { typeof(FurSurrogateVersion1) };
		object ISaveableBase.ToCurrentSaveVersion()
		{
			return new FurSurrogateVersion1()
			{
				primary = this.primaryColor,
				secondary = this.secondaryColor,
				pattern = (int)this.multiColorPattern
			};
		}

		internal FurColor(FurSurrogateVersion1 surrogate)
		{
			primaryColor = surrogate.primary ?? HairFurColors.NO_HAIR_FUR;
			secondaryColor = surrogate.secondary ?? HairFurColors.NO_HAIR_FUR;
			multiColorPattern = (FurMulticolorPattern)surrogate.pattern;
			validateData();
		}

	}

	[DataContract]
	public sealed class FurSurrogateVersion1 : ISurrogateBase
	{
		[DataMember]
		public HairFurColors primary;
		[DataMember]
		public HairFurColors secondary;
		[DataMember]
		public int pattern;
		
		object ISurrogateBase.ToSaveable()
		{
			return new FurColor(this);
		}
	}
}