﻿//ClitVaginaStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 10:11 PM
using CoC.Backend.Creatures;
using System;

namespace CoC.Backend.BodyParts
{
	public partial class Clit
	{
		public static string Name()
		{
			return "Clit";
		}

		public string ShortDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public string LongDescription()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Vagina
	{
		public static string Name()
		{
			return "Vagina";
		}

		public string FullDescription()
		{
			return LongDescription() + " with a " + clit.LongDescription() + (virgin ? " and an intact hymen" : "") + ".";
		}


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
			return Environment.NewLine + "Your " + ShortDescription() + recoverText + Environment.NewLine;
		}
	}

	public partial class VaginaType
	{
		private static string VagHumanDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagHumanLongDesc(VaginaData vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagHumanPlayerStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineLongDesc(VaginaData vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquinePlayerStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineTransformStr(VaginaData previousVaginaData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagEquineRestoreStr(VaginaData previousVaginaData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapLongDesc(VaginaData vagina)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapPlayerStr(Vagina vagina, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapTransformStr(VaginaData previousVaginaData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string VagSandTrapRestoreStr(VaginaData previousVaginaData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
