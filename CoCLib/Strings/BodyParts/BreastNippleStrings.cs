//BreastNippleStrings.cs
//Description:
//Author: JustSomeGuy
//1/10/2019, 9:07 PM
using CoC.BodyParts;
using CoC.Tools;

namespace CoC.Strings.BodyParts
{
	public static class BreastNippleStrings
	{
		public static string NippleShortDescription(Nipples nipples)
		{
			return "nipples";
		}
		public static string NippleDescription(Nipples nipples)
		{
			string retVal = "";
			retVal += nipples.blackNipples ? "black" : "";
			if (nipples.nippleStatus.IsInverted())
			{
				if (string.IsNullOrWhiteSpace(retVal))
				{
					retVal += ", ";
				}
				retVal += (nipples.nippleStatus == NippleStatus.SLIGHTLY_INVERTED ? "slightly-" : "") + "inverted";
			}
			retVal += nipples.quadNipples ? "quad " : "";
			if (nipples.nippleStatus == NippleStatus.DICK_NIPPLE)
			{
				retVal += "dick-nipples";
			}
			else if (nipples.nippleStatus == NippleStatus.FUCKABLE)
			{
				retVal += "nipple-cunts";
			}
			else
			{
				retVal += "nipples";
			}
			return retVal;
		}

		public static string NippleDescriptionWithLength(Nipples nipples)
		{
			return Helpers.ToNearestQuarter(nipples.length).ToString() + " inch " + NippleDescription(nipples);
		}
	}
}
