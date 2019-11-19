using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Ovipositor
	{
		public static string Name()
		{
			return "Ovipositor";
		}
	}

	public partial class OvipositorType
	{
		private static string NoneShortDesc()
		{
			return "";
		}
		private static string NoneFullDesc(Ovipositor ovipositor)
		{
			return "";
		}
		private static string NonePlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			return "";
		}
		private static string NoneTransformStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			return previousOvipositorData.type.restoredString(previousOvipositorData, player);
		}
		private static string NoneRestoreStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousOvipositorData, player);
		}

		private static string SpiderShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderFullDesc(Ovipositor ovipositor)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderTransformStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderRestoreStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BeeShortDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeFullDesc(Ovipositor ovipositor)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeePlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeTransformStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeRestoreStr(OvipositorData previousOvipositorData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
