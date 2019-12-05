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

	//by default, we actually don't use the hair's player description; it's part of ears. For the sake of completion, it's just the long text, but made into a sentence. 
	//idk if this may change in the future. 

	//Grammar format: (English. adapt to target language if necessary)
	//Hair breaks the rules for most other cases because it cannot work as a noun and an object in the same way.
	//Hair is a "indifferent" noun: it can be quantified (i.e all the hair on my neck), or treated as a single group (i.e. a full head of hair)
	//however, head is not indifferent; it must be quantified. ('my head', or 'i have _A_ head');
	//evidently, this breaks these strings. My solution is to create an "object" and "noun" format for this class; use whichever makes the most sense for the current context. 

	//by default, this is virtual; only no-hair (or any hair that is currently bald) have unique text for each one. 
	public partial class HairType
	{

		private static string NoHairDesc()
		{
			return "";
		}

		private static string NoHairLongDesc(HairData hair)
		{
			return "";
		}

		private static string NoHairToGrow()
		{
			return "can't grow hair that does not exist";
		}

		private static string NoHairToCut()
		{
			return "can't cut hair that does not exist";
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
			return GenericLongDesc(hair, NormalDesc());
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
			return GenericLongDesc(hair, Utils.RandomChoice("feathered hair", "fluffy plumes for hair", "hair-like basilisk plumes", "shock of feathers for hair"));
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
			return GenericLongDesc(hair, Utils.RandomChoice("goo-hair", "gooey hair", "gelatinous hair"));
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
			return GenericLongDesc(hair, Utils.RandomChoice(AnemoneDesc(), "anemone-hair"));
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
			return GenericLongDesc(hair, Utils.RandomChoice(QuillDesc(), "hair-like quills"));
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
			return "basilisk spines for hair";
		}

		private static string SpineLongDesc(HairData hair)
		{
			return GenericLongDesc(hair, Utils.RandomChoice("rubbery spines for hair", "crown of spines for hair", "basilisk spines for hair", "reptilian spines for hair"));
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
			return "basilisk plumes for hair";
		}

		private static string PlumeLongDesc(HairData hair)
		{
			return GenericLongDesc(hair, Utils.RandomChoice("feathered hair", "fluffy plumes for hair", "hair-like basilisk plumes", "shock of feathers for hair"));
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
			return GenericLongDesc(hair, Utils.RandomChoice("woolen hair",
						"poofy hair",
						"soft wool-hair"));
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
			return "leafy vines for hair";
		}

		private static string VineLongDesc(HairData hair)
		{
			return GenericLongDesc(hair, Utils.RandomChoice("leafy hair", "grassy hair", "pine needle hair", "hair-like vines", "leafy vines for hair"));
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

		public string DescriptionWithTransparency(bool isSemiTransparent)
		{
			if (this == NO_HAIR)
			{
				return "";
			}
			return SemiTransparentString(isSemiTransparent) + ShortDescription();
		}

		public string DescriptionWithColor(HairData hair)
		{
			if (this == NO_HAIR || hair.length == 0)
			{
				return "";
			}
			else
			{
				return SemiTransparentString(hair.isSemiTransparent) + hair.hairColor.AsString() + " " + hair.ShortDescription();
			}
		}

		public string DescriptionWithColorAndStyle(HairData hair)
		{
			if (this == NO_HAIR || hair.length == 0)
			{
				return "";
			}
			else
			{
				//style str properly spaces itself if needed. so does semi transparent str.
				return StyleStr(hair.style) + SemiTransparentString(hair.isSemiTransparent) + hair.hairColor.AsString() + " " + hair.ShortDescription();
			}
		}

		public string DescriptionWithColorLengthAndStyle(HairData hair)
		{
			if (this == NO_HAIR || hair.length == 0)
			{
				return "";
			}
			else
			{
				byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

				return LengthText(hair.length, creatureHeight) + hair.hairColor.AsString() + " " + hair.ShortDescription();
			}
		}

		//long has length, color, and highlights.
		protected static string GenericLongDesc(HairData hair, string desc)
		{
			if (hair.type == NO_HAIR || hair.length == 0)
			{
				return "";
			}
			else
			{
				byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;

				return LengthText(hair.length, creatureHeight) + " " + hair.hairColor.AsString() + desc + HighlightStr(hair.highlightColor);
			}
		}

		public string FullDescription(HairData hair)
		{
			if (this == NO_HAIR || hair.length == 0)
			{
				return "";
			}
			else
			{
				byte creatureHeight = CreatureStore.GetCreatureClean(hair.creatureID)?.build.heightInInches ?? Build.DEFAULT_HEIGHT;


				StringBuilder sb = new StringBuilder(LengthText(hair.length, creatureHeight) + hair.hairColor.AsString());
				if (hair.style != HairStyle.NO_STYLE)
				{
					sb.Append(", ");
				}
				else
				{
					sb.Append(" ");
				}
				sb.Append(StyleStr(hair.style));
				sb.Append(ShortDescription());
				sb.Append(HighlightStr(hair.highlightColor));
				return sb.ToString();
			}
		}

		public string BaldOrDescriptionWithTransparency(bool isSemiTransparent, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR) return BaldHeadText(baldFallbackNeedsArticle);
			return DescriptionWithTransparency(isSemiTransparent);
		}

		public string BaldOrDescriptionWithColor(HairData hair, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR || hair.length == 0) return BaldHeadText(baldFallbackNeedsArticle);
			else return DescriptionWithColor(hair);
		}

		public string BaldOrDescriptionWithColorAndStyle(HairData hair, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR || hair.length == 0) return BaldHeadText(baldFallbackNeedsArticle);
			else return DescriptionWithColorAndStyle(hair);
		}

		public string BaldOrDescriptionWithColorLengthAndStyle(HairData hair, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR || hair.length == 0) return BaldHeadText(baldFallbackNeedsArticle);
			else return DescriptionWithColorLengthAndStyle(hair);
		}

		//long has length, color, and highlights.
		public string BaldOrLongDescription(HairData hair, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR || hair.length == 0) return BaldHeadText(baldFallbackNeedsArticle);
			else return LongDescription(hair);
		}

		public string BaldOrFullDescription(HairData hair, bool baldFallbackNeedsArticle)
		{
			if (this == NO_HAIR || hair.length == 0) return BaldHeadText(baldFallbackNeedsArticle);
			else return FullDescription(hair);
		}

		protected static string DefaultPlayerDesc(Hair hair, PlayerBase player)
		{
			return "You have " + hair.type.BaldOrFullDescription(hair.AsReadOnlyData(), false); //you have (hair stuff) or you have no hair if bald or no hair. 
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

		public static string BaldText()
		{
			return Utils.RandomChoice("shaved", "bald", "smooth", "hairless", "glabrous");
		}

		public static string BaldHeadText(bool withArticle)
		{
			if (withArticle)
			{
				return "a " + BaldText() + " head";
			}
			else
			{
				return "no hair";
			}
		}

		public static string LengthText(float length, float creatureHeight)
		{
			if (length == 0) return BaldText();

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
