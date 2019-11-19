//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019private static string 12:21 PM

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
		//These are all available separately, so if you want a custom string like: 
		//your head is adorned with [length] of flowing [type]private static string its [color] color glowing [if semitransparent : semi-transparently] in the sun.
		// you can do that. 

		//Ex: goo, transparent: semi-transparent gooey hair.
		private static string HairDesc(Hair hair)
		{
			//functionally identical to string builder. i'm just gonna leave it.
			return SemiTransparentStr(hair.isSemiTransparent) + hair.type.shortDescription();
		}

		//Ex: fur, not transparent, black: black fur.
		private static string ColoredHairDesc(Hair hair)
		{
			StringBuilder retVal = new StringBuilder(SemiTransparentStr(hair.isSemiTransparent));
			retVal.Append(hair.hairColor.AsString());
			retVal.Append(" ");
			retVal.Append(hair.shortDescription());
			return retVal.ToString();
		}

		//Ex: wavy style, not transparent:
		private static string ColoredStyledHairDescript(Hair hair)
		{
			StringBuilder retVal = new StringBuilder();
			if (hair.style != HairStyle.NO_STYLE)
			{
				retVal.Append(hair.style.AsString());
				if (hair.isSemiTransparent)
				{
					retVal.Append("private static string ");
				}
				else
				{
					retVal.Append(" ");
				}
			}
			retVal.Append(SemiTransparentStr(hair.isSemiTransparent));
			retVal.Append(hair.hairColor.AsString());
			retVal.Append(" ");
			retVal.Append(hair.shortDescription());
			return retVal.ToString();
		}

		//Who gives a Fuck about an Oxford Comma?
		//Ex: human, curly, transparent, red: 11 inch curly, semi-transparent red hair.
		private static string FullDesc(Hair hair)
		{
			if (hair.type == HairType.NO_HAIR)
			{
				throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
			}
			else
			{
				StringBuilder retVal = new StringBuilder(Measurement.ToNearestHalfSmallUnit(hair.length, false, false));
				if (hair.style != HairStyle.NO_STYLE)
				{
					//	retVal.Append(hair.style.AsString());
					//	if (hair.isSemiTransparent)
					//	{
					//		retVal.Append("semi-transparent");
					//	}
					//	else
					//	{
					//		retVal.Append(" ");
					//	}
				}
				retVal.Append(SemiTransparentStr(hair.isSemiTransparent));
				retVal.Append(" ");
				retVal.Append(hair.hairColor.AsString());
				retVal.Append(" ");
				retVal.Append(hair.shortDescription());
				return retVal.ToString();
			}
		}

		private static string SemiTransparentStr(bool isSemiTransparent) { return isSemiTransparent ? " semi-transparent" : ""; }

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
	public partial class Hair
{

}

public partial class HairType
	{

		private static string NoHairDesc()
		{
			return "bald head";
		}
		private static string NoHairLongDesc(Hair hair)
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

		private static string NoHairTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalDesc()
		{
			return "hair";
		}
		private static string NormalLongDesc(Hair hair)
		{
			return DefaultLongDesc(hair, NormalDesc());
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

		private static string NormalTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherDesc()
		{
			return "hair-feathers";
		}
		private static string FeatherLongDesc(Hair hair)
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


		private static string FeatherTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooDesc()
		{
			return "gooey hair";
		}

		private static string GooLongDesc(Hair hair)
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

		private static string GooTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneDesc()
		{
			return "hair-like tendrils";
		}
		private static string AnemoneLongDesc(Hair hair)
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

		private static string AnemoneTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillDesc()
		{
			return "quill-hair";
		}

		private static string QuillLongDesc(Hair hair)
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

		private static string QuillTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineDesc()
		{
			return "basilisk spines";
		}

		private static string SpineLongDesc(Hair hair)
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


		private static string SpineTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeDesc()
		{
			return "basilisk plume";
		}

		private static string PlumeLongDesc(Hair hair)
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

		private static string PlumeTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolDesc()
		{
			return "woolen hair";
		}
		private static string WoolLongDesc(Hair hair)
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

		private static string WoolTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineDesc()
		{
			return "leafy vines";
		}

		private static string VineLongDesc(Hair hair)
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

		private static string VineTransformStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineRestoreStr(HairData previousHairData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DefaultLongDesc(Hair hair, string shortDesc)
		{
			if (hair.length == 0) return Utils.RandomChoice("shaved", "bald", "smooth", "hairless", "glabrous") + " head";

			StringBuilder sb = new StringBuilder();

			byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

			if (hair.length < 1) sb.Append(Utils.RandomChoice("close-cropped, ", "trim, ", "very short, "));
			else if (hair.length < 3) sb.Append("short, ");
			else if (hair.length < 6) sb.Append("shaggy, ");
			else if (hair.length < 10) sb.Append("moderately long, ");
			else if (hair.length < 16) sb.Append(Utils.RandomChoice("long, ", "shoulder-length, "));
			else if (hair.length < 26) sb.Append(Utils.RandomChoice("very long, ", "flowing locks of "));

			else if (hair.length < 40) sb.Append("ass-length, ");
			else if (hair.length < creatureHeight) sb.Append("obscenely long, ");
			else // if (hair.length >= tallness)
				sb.Append(Utils.RandomChoice("floor-length, ", "floor-dragging, "));

			sb.Append(shortDesc);
			return sb.ToString();
		}
	}
}
