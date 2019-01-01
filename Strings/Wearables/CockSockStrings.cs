//CockSockStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:53 PM
using CoC.BodyParts;
using System;

namespace CoC.Strings.Wearables
{
	static class CockSockStrings
	{
		public static string alabasterPlayer(Cock cock)
		{
			return "It's covered by a white, lacey cock-sock, snugly wrapping around it like a bridal dress around a bride.";
		}
		public static string cockringPlayer(Cock cock, Balls balls)
		{
			return "It's covered by a black latex cock-sock with two attached metal rings, keeping your cock just a little harder and " + balls.shortDescription() + " aching for release.";
		}
		public static string viridianPlayer(Cock cock)
		{
			return "It's covered by a lacey dark green cock-sock accented with red rose-like patterns.  Just wearing it makes your body, especially your cock, tingle.";
		}
		public static string scarletPlayer(Cock cock)
		{
			return "It's covered by a lacey red cock-sock that clings tightly to your member.  Just wearing it makes your cock throb, as if it yearns to be larger...";
		}
		public static string cobaltPlayer(Cock cock)
		{
			return "It's covered by a lacey blue cock-sock that clings tightly to your member... really tightly.  It's so tight it's almost uncomfortable, and you wonder if any growth might be inhibited.";
		}
		public static string gildedPlayer(Cock cock)
		{
			return "It's covered by a metallic gold cock-sock that clings tightly to you, its surface covered in glittering gems.  Despite the warmth of your body, the cock-sock remains cool.";
		}
		public static string amaranthinePlayer(Cock cock)
		{
			String retVal = "It's covered by a lacey purple cock-sock";
			if (cock != CockType.DISPLACER)
				retVal += " that fits somewhat awkwardly on your member";
			else
				retVal += " that fits your coeurl cock perfectly";
			retVal += ".  Just wearing it makes you feel stronger and more powerful.";
			return retVal;
		}
		public static string redPlayer(Cock cock)
		{
			return "It's covered by a red cock-sock that seems to glow.  Just wearing it makes you feel a bit powerful.";
		}
		public static string greenPlayer(Cock cock)
		{
			return "It's covered by a green cock-sock that seems to glow.  Just wearing it makes you feel a bit healthier.";
		}
		public static string bluePlayer(Cock cock)
		{
			return "It's covered by a blue cock-sock that seems to glow.  Just wearing it makes you feel like you can cast spells more effectively.";
		}
		public static string WoolStringPlayer(CockType cock)
		{
			return "It's covered by a wooly white cock-sock, keeping it snug and warm despite how cold it might get.";
		}
	}
}
