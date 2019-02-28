//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019private static string 12:21 PM

using CoC.Creatures;
using CoC.Strings;
using System.Text;

namespace CoC.BodyParts
{
	//Note that harpy hair uses [claws]. replace it with short description, not full description.

	internal static class HairHelpers
	{
		public static string asString(this HairStyle style)
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
				default:
					return "";

			}
		}
	}

	internal partial class Hair
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
			retVal.Append(hair.color.AsString());
			retVal.Append(" ");
			retVal.Append(hair.type.shortDescription());
			return retVal.ToString();
		}

		//Ex: wavy style, not transparent:
		private static string ColoredStyledHairDescript(Hair hair)
		{
			StringBuilder retVal = new StringBuilder();
			if (hair.style != HairStyle.NO_STYLE)
			{
				retVal.Append(hair.style.asString());
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
			retVal.Append(hair.color.AsString());
			retVal.Append(" ");
			retVal.Append(hair.type.shortDescription());
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
				StringBuilder retVal = new StringBuilder(GlobalStrings.LengthInHalfInch(hair.lengthInInches));
				if (hair.style != HairStyle.NO_STYLE)
				{
					retVal.Append(hair.style.asString());
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
				retVal.Append(hair.color.AsString());
				retVal.Append(" ");
				retVal.Append(hair.type.shortDescription());
				return retVal.ToString();
			}
		}

		private static string SemiTransparentStrNoSpace(bool isSemiTransparent) { return isSemiTransparent ? "semi-transparent" : ""; }
		private static string SemiTransparentStr(bool isSemiTransparent) { return isSemiTransparent ? "semi-transparent " : ""; }
	}

	internal partial class HairType
	{

		private static string NoHairDesc()
		{
			return "bald head";
		}
		private static string NoHairFullDesc(Hair hair)
		{
			
		}
		private static string NoHairPlayerStr(Hair hair, Player player)
		{

		}
		private static string NoHairGrowStr(Player player)
		{

		}
		private static string NoHairTransformStr(Hair hair, Player player)
		{

		}
		private static string NoHairRestoreStr(Hair hair, Player player)
		{

		}
		private static string NormalDesc()
		{
			return "hair";
		}
		private static string NormalFullDesc(Hair hair)
		{

		}
		private static string NormalPlayerStr(Hair hair, Player player)
		{

		}
		private static string NormalGrowStr(Player player)
		{

		}
		private static string NormalTransformStr(Hair hair, Player player)
		{

		}
		private static string NormalRestoreStr(Hair hair, Player player)
		{

		}
		private static string FeatherDesc()
		{
			return "hair-feathers";
		}
		private static string FeatherFullDesc(Hair hair)
		{

		}
		private static string FeatherPlayerStr(Hair hair, Player player)
		{

		}
		private static string FeatherGrowStr(Player player)
		{

		}
		private static string FeatherTransformStr(Hair hair, Player player)
		{

		}
		private static string FeatherRestoreStr(Hair hair, Player player)
		{

		}

		private static string GooDesc()
		{
			return "gooey hair";
		}

		private static string GooFullDesc(Hair hair)
		{

		}
		private static string GooPlayerStr(Hair hair, Player player)
		{

		}
		private static string GooGrowStr(Player player)
		{

		}
		private static string GooTransformStr(Hair hair, Player player)
		{

		}
		private static string GooRestoreStr(Hair hair, Player player)
		{

		}
		private static string AnemoneDesc()
		{
			return "hair-like tendrils";
		}
		private static string AnemoneFullDesc(Hair hair)
		{

		}
		private static string AnemonePlayerStr(Hair hair, Player player)
		{

		}
		private static string AnemoneGrowStr(Player player)
		{

		}
		private static string AnemoneTransformStr(Hair hair, Player player)
		{

		}
		private static string AnemoneRestoreStr(Hair hair, Player player)
		{

		}
		private static string QuillDesc()
		{
			return "quill-hair";
		}

		private static string QuillFullDesc(Hair hair)
		{

		}
		private static string QuillPlayerStr(Hair hair, Player player)
		{

		}
		private static string QuillGrowStr(Player player)
		{

		}
		private static string QuillTransformStr(Hair hair, Player player)
		{

		}
		private static string QuillRestoreStr(Hair hair, Player player)
		{

		}
		private static string SpineDesc()
		{
			return "basilisk spines";
		}

		private static string SpineFullDesc(Hair hair)
		{

		}
		private static string SpinePlayerStr(Hair hair, Player player)
		{

		}
		private static string SpineGrowStr(Player player)
		{

		}

		private static string SpineTransformStr(Hair hair, Player player)
		{

		}
		private static string SpineRestoreStr(Hair hair, Player player)
		{

		}
		private static string PlumeDesc()
		{
			return "basilisk plume";
		}

		private static string PlumeFullDesc(Hair hair)
		{

		}
		private static string PlumePlayerStr(Hair hair, Player player)
		{

		}
		private static string PlumeGrowStr(Player player)
		{

		}
		private static string PlumeTransformStr(Hair hair, Player player)
		{

		}
		private static string PlumeRestoreStr(Hair hair, Player player)
		{

		}
		private static string WoolDesc()
		{
			return "woolen hair";
		}
		private static string WoolFullDesc(Hair hair)
		{

		}
		private static string WoolPlayerStr(Hair hair, Player player)
		{

		}
		private static string WoolGrowStr(Player player)
		{

		}
		private static string WoolTransformStr(Hair hair, Player player)
		{

		}
		private static string WoolRestoreStr(Hair hair, Player player)
		{

		}
		private static string VineDesc()
		{
			return "leafy vines";
		}

		private static string VineFullDesc(Hair hair)
		{

		}
		private static string VinePlayerStr(Hair hair, Player player)
		{

		}
		private static string VineGrowStr(Player player)
		{

		}
		private static string VineTransformStr(Hair hair, Player player)
		{

		}
		private static string VineRestoreStr(Hair hair, Player player)
		{

		}
	}
}
