//WingStrings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019,A 10:27 PM
using CoC.Backend.CoC_Colors;
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
		private static string YourBoneDesc(out bool isPlural)
		{
			isPlural = true;
			return " your wings' bones";
		}
		private static string WingText()
		{
			return "Wings";
		}
		private static string WingDesc(bool isLotion, out bool isPlural)
		{
			if (isLotion)
			{
				isPlural = true;
				return " your wings' membranes";
			}
			else
			{
				isPlural = true;
				return " your wings";
			}
		}

		private string WingsDyeText(HairFurColors color)
		{
			return "";
		}

		private string WingsToneText(Wings wings, byte index)
		{
			return "";
		}

		protected string TonablePostToneText(Wings wings, byte index)
		{
			return $"{wings.wingTone.AsString()} membranes and {wings.wingBoneTone.AsString()} bones";
		}

		private static string NoneDesc(bool plural)
		{
			return "";
		}
		private static string NoneLongDesc(WingData wings, bool alternateFormat, bool plural)
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
		private static string BeeLikeDesc(bool? isLarge, bool plural = true)
		{
			string size = "";
			if (isLarge is bool large)
			{
				size = large ? "large, " : "tiny, ";
			}
			return size + "bee-like wing" + (plural ? "s" : "");
		}

		private static string BeeLikeLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + BeeLikeDesc(wings.isLarge);
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

		protected string FeatheredWingsPostDyeText(HairFurColors color)
		{
			return $"{color.AsString()}-tinted feathers on your wings";
		}

		private static string FeatheredDesc(bool? isLarge, bool plural)
		{
			if (isLarge is bool large)
			{
				return large ? "large feathered wing" + (plural ? "s" : "") : "feathered harpy wing" + (plural ? "s" : "");
			}
			else
			{
				return "feathered wing" + (plural ? "s" : "");
			}
		}
		private static string FeatheredLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro;
			if (wings.isLarge)
			{
				if (alternateFormat && plural)
				{
					intro = "a pair of ";
				}
				else if (alternateFormat)
				{
					intro = "a ";
				}
				else if (plural)
				{
					intro = "pair of ";
				}
				else
				{
					intro = "";
				}
				return intro + Utils.Pluralize("large wing", plural) + " covered in " + wings.featherColor.AsString() + " feathers";

			}
			else
			{
				if (alternateFormat && plural)
				{
					intro = "a pair of " + wings.featherColor.AsString();
				}
				else if (alternateFormat)
				{
					intro = Utils.AddArticle(wings.featherColor.AsString());
				}
				else if (plural)
				{
					intro = "pair of " + wings.featherColor.AsString();
				}
				else
				{
					intro = wings.featherColor.AsString();
				}
				return intro + Utils.Pluralize("harpy wing", plural);
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
		private static string BatLikeDesc(bool? isLarge, bool plural)
		{
			string sizeStr = "";
			if (isLarge is bool large)
			{
				sizeStr = large ? "large, " : "tiny, ";
			}

			return sizeStr + "demonic, bat-like wing" + (plural ? "s" : "");
		}

		private static string BatLikeLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			//large and cute both use "a ", so i can cheat.
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}

			if (wings.isLarge)
			{
				return intro + "large demonic wings, which appear similar to those of a bat.";
			}
			return intro + "cute, tiny, demonic bat-like wings.";
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
		private static string DraconicDesc(bool? isLarge, bool plural = true)
		{
			string wingStr = plural ? "wings" : "wing";
			if (isLarge is null)
			{
				return "draconic " + wingStr;
			}
			else if (isLarge == true)
			{
				return "large draconic " + wingStr;
			}
			else
			{
				return "small, vestigial wings";
			}
		}

		private static string DraconicLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of " + wings.wingTone.AsString();
			}
			else if (alternateFormat)
			{
				intro = Utils.AddArticle(wings.wingTone.AsString());
			}
			else if (plural)
			{
				intro = "pair of " + wings.wingTone.AsString();
			}
			else
			{
				intro = wings.wingTone.AsString();
			}
			return intro + " " + DraconicDesc(wings.isLarge, plural);
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
		private static string FaerieDesc(bool? isLarge, bool plural = true)
		{
			string size = isLarge is bool large ? (large ? "large " : "small ") : "";
			return size + "faerie-like wing" + (plural ? "s" : "");
		}

		private static string FaerieLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + FaerieDesc(wings.isLarge, plural);
		}
		private static string FaeriePlayerStr(Wings wings, PlayerBase player)
		{
			return "You have somehow obtained " + wings.LongDescription(true) + ". Congratulations! You should never see this, because this is just for NPCs and monsters! "
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
		//always large.
		private static string DragonflyDesc(bool plural = true)
		{
			return "giant dragonfly wing" + (plural ? "s" : "");
		}
		private static string DragonflyLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + DragonflyDesc();
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
		private static string ImpDesc(bool? isLarge, bool plural = true)
		{
			string size = isLarge is bool large ? (large ? "large " : "small ") : "";
			return size + "imp-like wing" + (plural ? "s" : "");
		}
		private static string ImpLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + ImpDesc(wings.isLarge);
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
