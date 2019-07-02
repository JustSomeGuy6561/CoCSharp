//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019private static string 12:21 PM

using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
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
					return "messy";
				case HairStyle.BRAIDED:
					return "braided";
				case HairStyle.STRAIGHT:
					return "straight";
				case HairStyle.WAVY:
					return "wavy";
				case HairStyle.CURLY:
					return "curly";
				case HairStyle.COILED:
					return "coiled";
				case HairStyle.NO_STYLE:
					return "no style";
				default:
					throw new NotImplementedException("A new hair style was added but someone didn't implement it's AsString");
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
				throw new System.NotImplementedException();
			}
			else
			{
				StringBuilder retVal = new StringBuilder(Measurement.ToNearestHalfSmallUnit(hair.length, false, false));
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
				retVal.Append(" ");
				retVal.Append(hair.hairColor.AsString());
				retVal.Append(" ");
				retVal.Append(hair.shortDescription());
				return retVal.ToString();
			}
		}

		private static string SemiTransparentStr(bool isSemiTransparent) { return isSemiTransparent ? " semi-transparent" : ""; }

		private string HairStr()
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
			return Environment.NewLine + Tools.SafelyFormattedString.FormattedText("You are no longer bald. You now have " + fullDescription() + " coating your head.", StringFormats.BOLD) + Environment.NewLine;
		}

		private string HairLongerStr()
		{
			return Environment.NewLine + SafelyFormattedString.FormattedText("Your hair's growth has reached a new threshold, giving you " + fullDescription() + ".", StringFormats.BOLD) + Environment.NewLine;
		}
	}
	public partial class HairType
	{

		private static string NoHairDesc()
		{
			return "bald head";
		}
		private static string NoHairFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairPlayerStr(Hair hair, Player player)
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

		private static string NoHairTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NoHairRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalDesc()
		{
			return "hair";
		}
		private static string NormalFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalPlayerStr(Hair hair, Player player)
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

		private static string NormalTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NormalRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherDesc()
		{
			return "hair-feathers";
		}
		private static string FeatherFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherPlayerStr(Hair hair, Player player)
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


		private static string FeatherTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooDesc()
		{
			return "gooey hair";
		}

		private static string GooFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(Hair hair, Player player)
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

		private static string GooTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneDesc()
		{
			return "hair-like tendrils";
		}
		private static string AnemoneFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemonePlayerStr(Hair hair, Player player)
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

		private static string AnemoneTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string AnemoneRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillDesc()
		{
			return "quill-hair";
		}

		private static string QuillFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillPlayerStr(Hair hair, Player player)
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

		private static string QuillTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string QuillRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineDesc()
		{
			return "basilisk spines";
		}

		private static string SpineFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpinePlayerStr(Hair hair, Player player)
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


		private static string SpineTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpineRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeDesc()
		{
			return "basilisk plume";
		}

		private static string PlumeFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumePlayerStr(Hair hair, Player player)
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

		private static string PlumeTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PlumeRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolDesc()
		{
			return "woolen hair";
		}
		private static string WoolFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolPlayerStr(Hair hair, Player player)
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

		private static string WoolTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineDesc()
		{
			return "leafy vines";
		}

		private static string VineFullDesc(Hair hair)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VinePlayerStr(Hair hair, Player player)
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

		private static string VineTransformStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VineRestoreStr(Hair hair, Player player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
