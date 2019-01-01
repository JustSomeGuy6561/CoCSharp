//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019, 12:21 PM
using CoC.BodyParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Strings.BodyParts
{
	public static class HairStrings
	{

		public static string asString(this HAIR_STYLE style)
		{
			switch (style)
			{
				case HAIR_STYLE.MESSY:
					return "messy";
				case HAIR_STYLE.MANGEY:
					return "mangey";
				case HAIR_STYLE.STRAIGHT:
					return "straight";
				case HAIR_STYLE.WAVY:
					return "wavy";
				case HAIR_STYLE.CURLY:
					return "curly";
				case HAIR_STYLE.COILED:
					return "coiled";
				case HAIR_STYLE.NOT_APPLICABLE:
				default:
					return "";

			}
		}

		//These are all available separately, so if you want a custom string like: 
		//your head is adorned with [length] of flowing [type], its [color] color glowing [if semitransparent : semi-transparently] in the sun.
		// you can do that. 

		//Ex: fur, not transparent, black: black fur.
		public static string ColoredHairDesc(Hair hair)
		{
			return SemiTransparentStr(hair.isSemiTransparent) +  hair.color.AsString() + " " +  hair.hairType.shortDescription();
		}

		//Ex: goo, transparent: semi-transparent gooey hair.
		public static string HairDesc(Hair hair)
		{
			return SemiTransparentStr(hair.isSemiTransparent) + hair.hairType.shortDescription();
		}

		//Ex: normal, transparent, red: 11 inch [semi-transparent] red hair.
		public static string FullDesc(Hair hair)
		{
			string retVal = GlobalStrings.LengthInHalfInch(hair.lengthInInches);
			if (hair.style != HAIR_STYLE.NOT_APPLICABLE)
			{
				retVal += hair.style.asString();
				if (hair.isSemiTransparent)
				{
					retVal += ",";
				}
				retVal += " ";
			}
			return retVal + SemiTransparentStr(hair.isSemiTransparent) + hair.color.AsString() + " " + hair.hairType.shortDescription();
		}

		public static string SemiTransparentStrNoSpace(bool isSemiTransparent) { return isSemiTransparent ? "semi-transparent" : ""; }
		public static string SemiTransparentStr(bool isSemiTransparent) { return isSemiTransparent ? "semi-transparent " : ""; }

		public static string NoHairStr() { return "bald head"; }
		public static string NormalStr() { return "hair"; }
		public static string FeatherStr() { return "hair-feathers"; }
		public static string GooStr() { return "gooey hair"; }
		public static string AnemoneStr() { return "hair-like tendrils"; }
		public static string QuillStr() { return "quill-hair"; }
		public static string BasiliskSpinesStr() { return "basilisk spines"; }
		public static string BasiliskPlumesStr() { return "basilisk plume"; }
		public static string WoolStr() { return "woolen hair"; }
		public static string LeafStr() { return "leafy hair"; }
	}
}
