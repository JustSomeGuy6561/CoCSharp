//BreastNippleStrings.cs
//Description:
//Author: JustSomeGuy
//1/10/2019, 9:07 PM
using CoC.Backend.Tools;

namespace   CoC.Backend.BodyParts
{

	public sealed partial class Nipples
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
			return Measurement.ToNearestQuarterInchOrMillimeter(nipples.length, false) + NippleDescription(nipples);
		}
	}

	public static class NippleHelpers
	{
		public static bool IsInverted(this NippleStatus nippleStatus)
		{
			return nippleStatus == NippleStatus.FULLY_INVERTED || nippleStatus == NippleStatus.SLIGHTLY_INVERTED;
		}

		public static string AsText(this NippleStatus nippleStatus)
		{
			switch (nippleStatus)
			{
				case NippleStatus.FULLY_INVERTED: return "fully inverted";
				case NippleStatus.SLIGHTLY_INVERTED: return "slightly inverted";
				case NippleStatus.FUCKABLE: return "fuckable";
				case NippleStatus.DICK_NIPPLE: return "dick-nipple";

				case NippleStatus.NORMAL:
				default:
					return "normal";

			}
		}
	}
}
