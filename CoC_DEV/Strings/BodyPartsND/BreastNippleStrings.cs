//BreastNippleStrings.cs
//Description:
//Author: JustSomeGuy
//1/10/2019, 9:07 PM
using  CoC.BodyParts;
using CoC.Tools;

namespace   CoC.BodyParts
{

	internal partial class Nipples
	{
		private static string NippleShortDescription(Nipples nipples)
		{
			return "nipples";
		}
		private static string NippleDescription(Nipples nipples)
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

		private static string NippleDescriptionWithLength(Nipples nipples)
		{
			return Helpers.ToNearestQuarter(nipples.length).ToString() + " inch " + NippleDescription(nipples);
		}
	}
}
