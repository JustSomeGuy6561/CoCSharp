//ClitVaginaStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 10:11 PM
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.BodyParts
{
	public partial class Vagina
	{
		private string VaginaTightenedUpDueToInactivity(VaginalLooseness currentLooseness)
		{
			string recoverText;

			if (currentLooseness <= VaginalLooseness.ROOMY)
			{
				recoverText = " recovers from your ordeals, tightening up a bit.";
			}
			else if (currentLooseness == VaginalLooseness.GAPING)
			{
				recoverText = " recovers from your ordeals and becomes tighter.";
			}
			else //if (currentLooseness >= VaginalLooseness.CLOWN_CAR_WIDE)
			{
				recoverText = " recovers from the brutal stretching it has received and tightens up a little bit, but not much.";
			}
			return Environment.NewLine + "Your " + shortDescription() + recoverText + Environment.NewLine;

		}
	}
	public partial class VaginaType
	{
		private static string VagHumanDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagHumanFullDesc(Vagina vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagHumanPlayerStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineFullDesc(Vagina vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquinePlayerStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineTransformStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineRestoreStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapFullDesc(Vagina vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapPlayerStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapTransformStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapRestoreStr(Vagina vagina, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
