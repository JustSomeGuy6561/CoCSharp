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
			if (player.antennae.type != newType)
			{
				display.OutputText(player.antennae.TransformIntoText(newType));
				return player.UpdateAntennae(newType);
			}
			return false;
		}

		public static bool RestoreAntennaeDisplayMessageOnActivePage(this PlayerBase player, StandardDisplay display)
		{
			if (!player.antennae.isDefault)
			{
				display.OutputText(player.antennae.RestoreText());
				return player.RestoreAntennae();
			}
			return false;
		}
		/*
		public static bool UpdateArmsDisplayMessageOnActivePage(this Player player, ArmType newType)
		{
			if (player.arms.type != newType)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.arms.TransformIntoText(newType));
			}
			return player.UpdateArms(newType);
		}

		public static bool RestoreArmsDisplayMessageOnActivePage(this Player player)
		{
			if (!player.arms.isDefault)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.arms.RestoreText());
				return player.RestoreArms();
			}
			return false;
		}

		public static bool UpdateEyesDisplayMessageOnActivePage(this Player player, EyeType newType)
		{
			if (player.eyes.type != newType)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.eyes.TransformIntoText(newType));
				return player.UpdateEyes(newType);
			}
			return false;
		}

		public static bool RestoreEyesDisplayMessageOnActivePage(this Player player)
		{
			if (!player.eyes.isDefault)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.eyes.RestoreText());
				return player.RestoreEyes();
			}
			return false;
		}

		public static bool UpdateTongueDisplayMessageOnActivePage(this Player player, TongueType newType)
		{
			if (player.tongue.type != newType)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.tongue.TransformIntoText(newType));
				return player.UpdateTongue(newType);
			}
			return false;
		}

		public static bool RestoreTongue(this Player player)
		{
			if (!player.tongue.isDefault)
			{
				DisplayManager.GetCurrentDisplay().OutputText(player.tongue.RestoreText());
				return player.RestoreTongue();
			}
			return false;
		}
	*/
	}
}
