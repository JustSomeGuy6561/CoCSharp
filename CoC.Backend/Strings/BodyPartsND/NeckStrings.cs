//NeckStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 11:45 PM
using CoC.Backend.Creatures;

namespace CoC.Backend.BodyParts
{
	public partial class Neck
{
public static string Name()
{
return "Neck";
}
}

public partial class NeckType
	{
		private string GenericButtonDesc()
		{
			return "Neck";
		}

		private string GenericLocationText()
		{
			return " your neck";
		}

		private static string HumanDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanLongDesc(Neck neck)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanPlayerStr(Neck neck, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonLongDesc(Neck neck)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(Neck neck, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonTransformStr(NeckData previousNeckData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(NeckData previousNeckData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceLongDesc(Neck neck)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(Neck neck, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceTransformStr(NeckData previousNeckData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(NeckData previousNeckData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

	}
}
