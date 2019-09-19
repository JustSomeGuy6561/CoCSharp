//PlayerHelper.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:07 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend
{
	internal static class PlayerHelper
	{
		public static bool UpdateAntennaeAndDisplayMessage(this Player player, AntennaeType newType)
		{
			if (player.antennae.type != newType)
			{
				OutputText(player.antennae.TransformIntoText(newType));
				return player.UpdateAntennae(newType);
			}
			return false;
		}

		public static bool RestoreAntennaeAndDisplayMessage(this Player player)
		{
			if (!player.antennae.isDefault)
			{
				OutputText(player.antennae.RestoreText());
				return player.RestoreAntennae();
			}
			return false;
		}

		public static bool UpdateArmsAndDisplayMessage(this Player player, ArmType newType)
		{
			if (player.arms.type != newType)
			{
				OutputText(player.arms.TransformIntoText(newType));
			}
			return player.UpdateArms(newType);
		}

		public static bool RestoreArmsAndDisplayMessage(this Player player)
		{
			if (!player.arms.isDefault)
			{
				OutputText(player.arms.RestoreText());
				return player.RestoreArms();
			}
			return false;
		}

		public static bool UpdateEyesAndDisplayMessage(this Player player, EyeType newType)
		{
			if (player.eyes.type != newType)
			{
				OutputText(player.eyes.TransformIntoText(newType));
				return player.UpdateEyes(newType);
			}
			return false;
		}

		public static bool RestoreEyesAndDisplayMessage(this Player player)
		{
			if (!player.eyes.isDefault)
			{
				OutputText(player.eyes.RestoreText());
				return player.RestoreEyes();
			}
			return false;
		}

		public static bool UpdateTongueAndDisplayMessage(this Player player, TongueType newType)
		{
			if (player.tongue.type != newType)
			{
				OutputText(player.tongue.TransformIntoText(newType));
				return player.UpdateTongue(newType);
			}
			return false;
		}

		public static bool RestoreTongue(this Player player)
		{
			if (!player.tongue.isDefault)
			{
				OutputText(player.tongue.RestoreText());
				return player.RestoreTongue();
			}
			return false;
		}
	}
}
