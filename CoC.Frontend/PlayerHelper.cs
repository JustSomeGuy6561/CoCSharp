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
				OutputText(player.antennae.transformInto(newType, player));
				return player.UpdateAntennae(newType);
			}
			return false;
		}

		public static bool RestoreAntennaeAndDisplayMessage(this Player player)
		{
			if (!player.antennae.isDefault)
			{
				OutputText(player.antennae.restoreString(player));
				return player.RestoreAntennae();
			}
			return false;
		}

		public static bool UpdateArmsAndDisplayMessage(this Player player, ArmType newType)
		{
			if (player.arms.type != newType)
			{
				OutputText(player.arms.transformInto(newType, player));
			}
			return player.UpdateArms(newType);
		}

		public static bool RestoreArmsAndDisplayMessage(this Player player)
		{
			if (!player.arms.isDefault)
			{
				OutputText(player.arms.restoreString(player));
				return player.RestoreArms();
			}
			return false;
		}
	}
}
