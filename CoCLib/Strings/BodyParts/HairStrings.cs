//HairStrings.cs
//Description:
//Author: JustSomeGuy
//1/1/2019, 12:21 PM
using  CoC.BodyParts;

namespace   CoC.BodyParts
{
	public static class HairStrings
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
				case HairStyle.NOT_APPLICABLE:
				default:
					return "";

			}
		}

		//These are all available separately, so if you want a custom string like: 
		//your head is adorned with [length] of flowing [type], its [color] color glowing [if semitransparent : semi-transparently] in the sun.
		// you can do that. 

		//Ex: goo, transparent: semi-transparent gooey hair.
		public static string HairDesc(Hair hair)
		{
			return SemiTransparentStr(hair.isSemiTransparent) + hair.type.shortDescription();
		}

		//Ex: fur, not transparent, black: black fur.
		public static string ColoredHairDesc(Hair hair)
		{
			return SemiTransparentStr(hair.isSemiTransparent) + hair.color.AsString() + " " + hair.type.shortDescription();
		}

		//Ex: wavy style, not transparent:
		public static string ColoredStyledHairDescript(Hair hair)
		{
			string retVal = "";
			if (hair.style != HairStyle.NOT_APPLICABLE)
			{
				retVal += hair.style.asString();
				if (hair.isSemiTransparent)
				{
					retVal += ",";
				}
				retVal += " ";
			}
			return retVal + SemiTransparentStr(hair.isSemiTransparent) + hair.color.AsString() + " " + hair.type.shortDescription();
		}

		//Who gives a Fuck about an Oxford Comma?
		//Ex: normal, curly transparent, red: 11 inch curly, semi-transparent red hair.
		public static string FullDesc(Hair hair)
		{
			string retVal = GlobalStrings.LengthInHalfInch(hair.lengthInInches);
			if (hair.style != HairStyle.NOT_APPLICABLE)
			{
				retVal += hair.style.asString();
				if (hair.isSemiTransparent)
				{
					retVal += ",";
				}
				retVal += " ";
			}
			return retVal + SemiTransparentStr(hair.isSemiTransparent) + hair.color.AsString() + " " + hair.type.shortDescription();
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
