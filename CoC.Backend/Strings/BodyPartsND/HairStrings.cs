//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019private static string 12:21 PM

using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.ComponentModel;
using System.Text;

namespace CoC.Backend.BodyParts
{
	//Note that harpy hair uses [claws]. replace it with short description, not full description.

	public static class HairHelpers
	{
		public static string AsString(this HairStyle style)
		{
			switch (style)
			{
				case HairStyle.MESSY:
					return "Messy";
				case HairStyle.BRAIDED:
					return "Braided";
				case HairStyle.STRAIGHT:
					return "Straight";
				case HairStyle.WAVY:
					return "Wavy";
				case HairStyle.CURLY:
					return "Curly";
				case HairStyle.COILED:
					return "Coiled";
				case HairStyle.PONYTAIL:
					return "Pony-Tail";
				case HairStyle.NO_STYLE:
					return "No Style";
				default:
					throw new InvalidEnumArgumentException("A new hair style was added but someone didn't implement its AsString");
			}
		}
	}

	public partial class Hair
	{
		public static string Name()
		{
			return "Hair";
		}

		private string HighlightStr()
		{
			return "Highlights";
		}

		private string YourHairStr()
		{
			return " your hair";
		}

		private string YourHighlightsStr()
		{
			return " the hair that forms your highlights";
		}

		private string NoMoreAcceleratedGrowthFrownyFace()
		{
			return Environment.NewLine + SafelyFormattedString.FormattedText("The tingling on your scalp slowly fades away as the hair extension serum wears off. " +
				"Maybe it's time to go get some more?", StringFormats.BOLD);
		}

		private string NoLongerBaldStr()
		{
			return Environment.NewLine + Tools.SafelyFormattedString.FormattedText("You are no longer bald. You now have " + LongDescription() + " coating your head.", StringFormats.BOLD) + Environment.NewLine;
		}

		private string HairLongerStr()
		{
			return Environment.NewLine + SafelyFormattedString.FormattedText("Your hair's growth has reached a new threshold, giving you " + LongDescription() + ".", StringFormats.BOLD) + Environment.NewLine;
		}
	}

	public partial class HairType
	{

		private static string NoHairDesc()
		{
			return "bald head";
		}
		private static string NoHairLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairToGrow()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string NoHairToCut()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string NoHairTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalDesc()
		{
			return "hair";
		}
		private static string NormalLongDesc(HairData hair)
		{
			return DefaultLongDesc(hair);
		}

		private static string NormalPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string NormalCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string NormalTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherDesc()
		{
			return "hair-feathers";
		}
		private static string FeatherLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string FeatherCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}


		private static string FeatherTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooDesc()
		{
			return "gooey hair";
		}

		private static string GooLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneDesc()
		{
			return "hair-like tendrils";
		}
		private static string AnemoneLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemonePlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneNoGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string AnemoneNoCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string AnemoneTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillDesc()
		{
			return "quill-hair";
		}

		private static string QuillLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string QuillCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string QuillTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineDesc()
		{
			return "basilisk spines";
		}

		private static string SpineLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpinePlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineNoGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string SpineNoCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}


		private static string SpineTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeDesc()
		{
			return "basilisk plume";
		}

		private static string PlumeLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumePlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string PlumeCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string PlumeTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolDesc()
		{
			return "woolen hair";
		}
		private static string WoolLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolPlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string WoolCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string WoolTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineDesc()
		{
			return "leafy vines";
		}

		private static string VineLongDesc(HairData hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VinePlayerStr(Hair hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineNoGrowStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string VineNoCutStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string VineTransformStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineRestoreStr(HairData hair, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public string ShortDescriptionWithTransparency(bool isSemiTransparent)
		{
			//functionally identical to string builder. i'm just gonna leave it.
			return SemiTransparentString(isSemiTransparent) + shortDescription();
		}

		public string DescriptionWithColor(HairData hair)
		{
			return SemiTransparentString(hair.isSemiTransparent) + hair.hairColor.AsString() + " " + hair.ShortDescription();
		}

		public string DescriptionWithColorAndStyle(HairData hair)
		{
			StringBuilder retVal = new StringBuilder();
			return StyleStr(hair.style) + SemiTransparentString(hair.isSemiTransparent) + hair.hairColor.AsString() + " " + hair.ShortDescription();
		}

		public string DescriptionWithColorLengthAndStyle(HairData hair)
		{
			byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

			StringBuilder sb = new StringBuilder(LengthText(hair.length, creatureHeight));
			if (hair.length == 0)
			{
				return sb.ToString() + " head";
			}
			sb.Append(hair.hairColor.AsString());
			sb.Append(hair.ShortDescription());
			return sb.ToString();
		}

		//long has length, color, and highlights.
		private static string DefaultLongDesc(HairData hair)
		{
			byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

			StringBuilder sb = new StringBuilder(LengthText(hair.length, creatureHeight));
			if (hair.length == 0)
			{
				return sb.ToString() + " head";
			}
			sb.Append(hair.hairColor.AsString());
			sb.Append(hair.ShortDescription());
			sb.Append(HighlightStr(hair.highlightColor));
			return sb.ToString();
		}

		public string FullDescription(HairData hair)
		{
			byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

			StringBuilder sb = new StringBuilder(LengthText(hair.length, creatureHeight));
			if (hair.length == 0)
			{
				return sb.ToString() + " head";
			}
			sb.Append(hair.hairColor.AsString());
			if (hair.style != HairStyle.NO_STYLE)
			{
				sb.Append(", ");
			}
			sb.Append(StyleStr(hair.style));
			sb.Append(shortDescription());
			sb.Append(HighlightStr(hair.highlightColor));
			return sb.ToString();
		}


		public static string SemiTransparentString(bool isSemiTransparent) { return isSemiTransparent ? " semi-transparent " : ""; }
		private static string StyleStr(HairStyle style) { return style != HairStyle.NO_STYLE ? style.AsString() + " " : ""; }
		private static string HighlightStr(HairFurColors highlight)
		{
			if (HairFurColors.IsNullOrEmpty(highlight))
			{
				return "";
			}
			else
			{
				return " with " + highlight.AsString() + " highlights";
			}
		}

		public static string LengthText(float length, float creatureHeight)
		{
			if (length == 0) return Utils.RandomChoice("shaved", "bald", "smooth", "hairless", "glabrous");

			else if (length < 1) return Utils.RandomChoice("close-cropped, ", "trim, ", "very short, ");
			else if (length < 3) return "short, ";
			else if (length < 6) return "shaggy, ";
			else if (length < 10) return "moderately long, ";
			else if (length < 16) return Utils.RandomChoice("long, ", "shoulder-length, ");
			else if (length < 26) return Utils.RandomChoice("very long, ", "flowing locks of ");

			else if (length < 40) return "ass-length, ";
			else if (length < creatureHeight) return "obscenely long, ";
			else // if (hair.length >= tallness)
				return Utils.RandomChoice("floor-length, ", "floor-dragging, ");
		}
	}
}
