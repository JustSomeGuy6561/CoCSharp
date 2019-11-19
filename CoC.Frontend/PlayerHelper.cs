//PlayerHelper.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:07 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Frontend.UI;

namespace CoC.Frontend
{
	internal static class PlayerHelper
	{
		public static bool UpdateAntennaeDisplayMessageOnActivePage(this PlayerBase player, AntennaeType newType, StandardDisplay display)
		{
			var oldData = player.antennae.AsReadOnlyData();
			if (player.UpdateAntennae(newType))
			{
				display.OutputText(player.antennae.TransformFromText(oldData));
				return true;
			}
			return false;
		}

		public static bool RestoreAntennaeDisplayMessageOnActivePage(this PlayerBase player, StandardDisplay display)
		{
			var oldData = player.antennae.AsReadOnlyData();
			if (player.RestoreAntennae())
			{
				display.OutputText(player.antennae.RestoredText(oldData));
				return true;
			}
			return false;
		}
	}
}
