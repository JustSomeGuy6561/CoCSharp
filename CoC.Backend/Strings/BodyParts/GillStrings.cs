//GillStrings.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 9:29 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Gills
	{
		public static string Name()
		{
			return "Gills";
		}
	}

	public partial class GillType
	{

		private static string AnemoneDescStr()
		{
			return "anemone gills";
		}
		private static string AnemoneLongDesc(GillData gills)
		{
			return "feathery anemone gills";
		}
		private static string AnemonePlayerStr(Gills gills, PlayerBase player)
		{
			return "A pair of feathery gills are growing out just below your neck, spreading out horizontally and draping down your chest. They allow you to stay in the water for quite a long time.";
		}
		private static string AnemoneTransformStr(GillData previousGillData, PlayerBase player)
		{
			if (previousGillData.type != GillType.NONE)
			{
				return Utils.NewParagraph() + "You feel your gills tighten, the slits seeming to close all at once. As you let out a choked gasp your gills shrink into nothingness, " +
					"leaving only smooth skin behind. When you think it's over you feel something emerge from under your neck, flowing down over your chest and brushing your nipples. " +
					"You look in surprise as your new feathery gills finish growing out, a film of mucus forming over them shortly after." + Utils.NewParagraph() +
					SafelyFormattedString.FormattedText("You now have feathery gills!", StringFormats.BOLD);
			}
			else
			{ // if no gills
				return Utils.NewParagraph() + "You feel a pressure in your lower esophageal region and pull your garments down to check the area. Before your eyes " +
					"a pair of feathery gills start to push out of the center of your chest, just below your neckline, parting sideways and draping over your " +
					player.nipples[0].CreatureDescription() + ". They feel a bit uncomfortable in the open air at first, but soon a thin film of mucus covers them and you" +
					" hardly notice anything at all. You redress carefully." + Utils.NewParagraph() + SafelyFormattedString.FormattedText("You now have feathery gills!", StringFormats.BOLD);
			}
		}
		private static string AnemoneRestoreStr(GillData previousGillData, PlayerBase player)
		{
			return Utils.NewParagraph() + "Your chest itches, and as you reach up to scratch it, you realize your gills have withdrawn into your skin. " +
				SafelyFormattedString.FormattedText("You no longer have gills!", StringFormats.BOLD);
		}

		private static string FishDescStr()
		{
			return "gills";
		}
		private static string FishLongDesc(GillData gills)
		{
			return "large, fish-like gills";
		}
		private static string FishPlayerStr(Gills gills, PlayerBase player)
		{
			return "A set of fish like gills reside on your neck, several relatively-small slits that can close flat against your skin. " +
				"They allow you to stay in the water for quite a long time.";
		}
		private static string FishTransformStr(GillData previousGillData, PlayerBase player)
		{
			if (previousGillData.type != GillType.NONE)
			{
				return Utils.NewParagraph() + "You feel your gills tingle, a vague numbness registering across their feathery exterior. You watch in awe as your gill's " +
					"feathery folds dry out and fall off like crisp autumn leaves. The slits of your gills then rearrange themselves, becoming thinner and shorter, as they " +
					"shift to the sides of your neck. They now close in a way that makes them almost invisible. As you run a finger over your neck you feel more than a few small, " +
					"raised lines where they meet your skin." + Utils.NewParagraph() + SafelyFormattedString.FormattedText("You now have fish like gills!", StringFormats.BOLD);
			}
			else
			{ // if no gills
				return Utils.NewParagraph() + "You feel a sudden tingle on your neck. You reach up to it to feel, what's the source of it. When you touch your neck, " +
					"you feel that it begins to grow several narrow slits which slowly grow longer. After the changes have stopped you quickly head to a nearby puddle" +
					" to take a closer look at your neck. You realize that your neck has grown gills allowing you to breathe under water as if you were standing on land." +
					Utils.NewParagraph() + SafelyFormattedString.FormattedText("You now have fish like gills!", StringFormats.BOLD);
			}
		}
		private static string FishRestoreStr(GillData previousGillData, PlayerBase player)
		{
			return Utils.NewParagraph() + "You feel your gills tighten, the slits seeming to close all at once. As you let out a choked gasp your" +
				" gills shrink into nothingness, leaving only smooth skin behind. Seems you won't be able to stay in the water quite so long anymore. " +
				SafelyFormattedString.FormattedText("You no longer have gills!", StringFormats.BOLD);
		}
	}
}
