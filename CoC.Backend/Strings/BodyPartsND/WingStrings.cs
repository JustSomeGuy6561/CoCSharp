//WingStrings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019,A 10:27 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Wings
	{
		public static string Name()
		{
			return "Wings";
		}
	}
	public partial class WingType
	{
		private static string Wing2Text()
		{
			return "Wings 2";
		}
		private static string YourBoneDesc()
		{
			return " your wings' bones";
		}
		private static string WingText()
		{
			return "Wings";
		}
		private static string WingDesc(bool isLotion)
		{
			if (isLotion)
			{
				return " your wings' membranes";
			}
			else
			{
				return " your wings";
			}
		}
		private static string NoneDesc()
		{
			return "";
		}
		private static string NoneLongDesc(WingData wings)
		{
			return "";
		}
		private static string NonePlayerStr(Wings wings, PlayerBase player)
		{
			return "";
		}
		private static string NoneTransformStr(WingData previousWingData, PlayerBase player)
		{
			return previousWingData.type.RestoredString(previousWingData, player);
		}
		private static string NoneRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousWingData, player);
		}
		private static string BeeLikeDesc()
		{
			return "bee-like wings";
		}

		private static string BeeLikeSizeDesc(bool isLarge)
		{
			string sizeStr = isLarge ? "large" : "tiny";
			return sizeStr + ", bee-like wings";
		}

		private static string BeeLikeLongDesc(WingData wings)
		{
			return "a pair of " + BeeLikeSizeDesc(wings.isLarge);
		}
		private static string BeeLikePlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large bee-wings sprout from your back, reflecting the light through their clear membranes beautifully. They flap quickly, " +
					"allowing you to easily hover in place or fly.";
			}
			else
			{
				return " A pair of tiny-yet-beautiful bee-wings sprout from your back, too small to allow you to fly.";
			}
		}
		private static string BeeLikeTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeLikeRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatheredDesc()
		{
			return "feathered wings";
		}
		private static string FeatheredSizeDesc(bool isLarge)
		{
			if (isLarge)
			{
				return "large feathered wings";
			}
			else
			{
				return "feathered harpy wings";
			}
		}
		private static string FeatheredLongDesc(WingData wings)
		{
			if (wings.isLarge)
			{
				return "a pair of large wings covered in " + wings.featherColor.AsString() + " feathers";
			}
			else
			{
				return "a pair of " + wings.featherColor.AsString() + "harpy wings";
			}
		}
		private static string FeatheredPlayerStr(Wings wings, PlayerBase player)
		{
			return " A pair of large, feathery wings sprout from your back. Though you usually keep the " + wings.wingTone.AsString() + "-colored wings folded close, " +
				"they can unfurl to allow you to soar as gracefully as a harpy.";
		}
		private static string FeatheredTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatheredRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BatLikeDesc()
		{
			return "demonic, bat-like wings";
		}
		private static string BatLikeSizeDesc(bool isLarge)
		{
			string sizeStr = isLarge ? "large, " : "tiny, ";
			return sizeStr + BatLikeDesc();
		}
		private static string BatLikeLongDesc(WingData wings)
		{
			if (wings.isLarge)
			{
				return "a pair of large demonic wings, which appear similar to those of a bat.";
			}
			return "a pair of cute, tiny, demonic bat-like wings.";
		}
		private static string BatLikePlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large bat-like demon-wings fold behind your shoulders. With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.";
			}
			else
			{
				return " A pair of tiny bat-like demon-wings sprout from your back, flapping cutely, but otherwise being of little use.";
			}
		}
		private static string BatLikeTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BatLikeRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DraconicDesc()
		{
			return "draconic wings";
		}
		private static string DraconicSizeDesc(bool isLarge)
		{
			if (isLarge)
			{
				return "large, draconic wings";
			}
			else
			{
				return "small, vestigial wings";
			}
		}
		private static string DraconicLongDesc(WingData wings)
		{
			return "a pair of " + wings.wingTone.AsString() + " " + DraconicSizeDesc(wings.isLarge);
		}
		private static string DraconicPlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " Magnificent wings sprout from your shoulders. When unfurled they stretch further than your arm span, and a single beat of them is all you need " +
					"to set out toward the sky. They look a bit like bat wings, but the membranes are covered in fine, delicate " + wings.wingTone.AsString() + " scales supported by " +
					wings.wingBoneTone.AsString() + " bones. A wicked talon juts from the end of each bone.";
			}
			else
			{
				return " Small, vestigial wings sprout from your shoulders. They might look like bat wings, but the membranes are covered in fine, delicate " + wings.wingTone.AsString() +
					" scales supported by " + wings.wingBoneTone.AsString() + " bones.";
			}
		}
		private static string DraconicTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DraconicRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		//not (normally) available to the player. Still need the short and long for npcs/enemies, but whatever.
		private static string FaerieDesc()
		{
			return "faerie-like wings";
		}
		private static string FaerieSizeDesc(bool isLarge)
		{
			return (isLarge ? "large " : "small ") + FaerieDesc();
		}
		private static string FaerieLongDesc(WingData wings)
		{
			return "a pair of " + FaerieSizeDesc(wings.isLarge);
		}
		private static string FaeriePlayerStr(Wings wings, PlayerBase player)
		{
			return "You have somehow obtained " + FaerieLongDesc(wings.AsReadOnlyData()) + ". Congratulations! You should never see this, because this is just for NPCs and monsters! "
					+ "Have a cookie. Also, report this to a dev, so they can either fix this if it was caused by a bug, or update this text if added intentionally.";
		}
		private static string FaerieTransformStr(WingData previousWingData, PlayerBase player)
		{
			return "through powers unknown to me, you have managed to transform your wings into faerie wings. You shouldn't be able to do this unless content has changed. Either way, " +
				"please report this to a dev.";
		}
		private static string FaerieRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return "As if to remedy this strange case of you (a player) having faerie wings, they have decided to cease to exist, leaving you without wings. Still, " +
				"this should never happen unless someone changed the content and didn't update this. Regardless, please report this to a dev.";
		}
		private static string DragonflyDesc()
		{
			return "dragonfly wings";
		}
		private static string DragonflySizeDesc(bool isLarge)
		{
			return "giant dragonfly wings";
		}
		private static string DragonflyLongDesc(WingData wings)
		{
			return "A pair of " + DragonflySizeDesc(true);
		}
		private static string DragonflyPlayerStr(Wings wings, PlayerBase player)
		{
			return " Giant dragonfly wings hang from your shoulders. At a whim, you could twist them into a whirring rhythm fast enough to lift you off the ground and allow you to fly.";
		}
		private static string DragonflyTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonflyRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpDesc()
		{
			return "imp-like wings";
		}
		private static string ImpSizeDesc(bool isLarge)
		{
			return (isLarge ? "large " : "small ") + ImpDesc();
		}
		private static string ImpLongDesc(WingData wings)
		{
			return "a pair of " + ImpSizeDesc(wings.isLarge);
		}
		private static string ImpPlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large imp wings fold behind your shoulders. With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.";
			}
			else
			{
				return " A pair of imp wings sprout from your back, flapping cutely but otherwise being of little use.";
			}
		}
		private static string ImpTransformStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(WingData previousWingData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
