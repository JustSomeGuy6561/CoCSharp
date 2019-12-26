using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

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
		private static string NoneShortDesc(bool singleMemberFormat)
		{
			return "";
		}
		private static string NoneLongDesc(OvipositorData ovipositor, bool alternateFormat)
		{
			return "";
		}
		private static string NonePlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			return "";
		}
		private static string NoneTransformStr(TailData previousTailData, PlayerBase player)
		{
			return previousTailData.ovipositor.type.restoreStr(previousTailData, player);
		}
		private static string NoneRestoreStr(TailData previousTailData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousTailData, player);
		}

		private static string SpiderShortDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("spider ovipositor", singleMemberFormat);
		}
		private static string SpiderLongDesc(OvipositorData ovipositor, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			return "You have a large, retractible spider ovipositor reaching down from your abdomen, just below your " + player.tail.ShortDescription()
				 + ". It's ready to deposit spider eggs into a host at any time. ";
		}
		private static string SpiderTransformStr(TailData previousTailData, PlayerBase player)
		{
			OvipositorData previousOvipositorData = previousTailData.ovipositor;

			if (previousOvipositorData.type != OvipositorType.NONE)
			{
				return "Your " + previousOvipositorData.ShortDescription() + " changes along with your tail. It expels all of its eggs, which then disintegrate for some reason. " +
					"It them morphs slightly, shifting until " + SafelyFormattedString.FormattedText("you now have a bee ovipositor!", StringFormats.BOLD);
			}
			else
			{
				string corruptionText;
				if (player.corruption > 80)
				{
					corruptionText = "Looks like you have something " + (player.hasCockOrClitCock ? "else " : "") + "to use to punish your enemies with.";
				}
				else if (player.corruption > 50)
				{
					corruptionText = "You can think of more than a few kinky things you can do with your new appendage.";
				}
				else
				{
					corruptionText = "You idly wonder what laying eggs with this thing will feel like...";
				}

				return "An odd swelling sensation floods your " + (previousTailData.type != player.tail.type ? "now " : "") + "spider half, just below where you store your webbing. " +
					"Curling your abdomen underneath you for a better look, you gasp in recognition at your new 'equipment'! Your semi-violent run-ins with the swamp's population " +
					"have left you " + SafelyFormattedString.FormattedText("intimately", StringFormats.ITALIC) + " familiar with the new appendage. " +
					SafelyFormattedString.FormattedText("It's a drider ovipositor!", StringFormats.BOLD) + " A few light prods confirm that it's just as sensitive " +
					"as any of your other sexual organs. " + corruptionText;
			}
		}
		private static string SpiderRestoreStr(TailData previousTailData, PlayerBase player)
		{
			return GenericLoseOvipositorText(previousTailData, player, "spider-like");
		}

		private static string BeeShortDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("bee ovipositor", singleMemberFormat);
		}
		private static string BeeLongDesc(OvipositorData ovipositor, bool alternateFormat)
		{
			return Utils.AddArticleIf("short, retractible bee ovipositor", alternateFormat);
		}
		private static string BeePlayerStr(Ovipositor ovipositor, PlayerBase player)
		{
			return "You have a short, retractible bee ovipositor reaching down from your abdomen, just below your " + player.tail.ShortDescription()
				+ ". It's ready to deposit bee eggs into a host at any time. ";
		}
		private static string BeeTransformStr(TailData previousTailData, PlayerBase player)
		{
			OvipositorData previousOvipositorData = previousTailData.ovipositor;

			if (previousOvipositorData.type != OvipositorType.NONE)
			{
				return "Your " + previousOvipositorData.ShortDescription() + " feels strange. Suddenly, it expels all of its eggs, which then disintegrate for some reason. " +
					"It them morphs slightly, shifting until " + SafelyFormattedString.FormattedText("you now have a bee ovipositor!", StringFormats.BOLD);
			}
			else
			{
				string corruptionText;
				if (player.corruption > 80)
				{
					corruptionText = "Looks like you have something " + (player.hasCockOrClitCock ? "else " : "") + "to use to punish your enemies with.";
				}
				else if (player.corruption > 50)
				{
					corruptionText = "You can think of more than a few kinky things you can do with your new sex-organ.";
				}
				else
				{
					corruptionText = "You idly wonder what laying them with your new bee ovipositor will feel like...";
				}

				return "An odd swelling starts in your " + (previousTailData.type != player.tail.type ? "now bee-like " : "") + "abdomen, just below your stinger. Curling around, " +
					"you reach back to your extended, bulbous bee stinger and run your fingers along the underside. You gasp when you feel a tender, yielding slit near the stinger. " +
					"As you probe this new orifice, a shock of pleasure runs through you, and a tubular, black, semi-hard appendage drops out, pulsating as heavily as any sexual organ. " +
					SafelyFormattedString.FormattedText("The new organ is clearly an ovipositor!", StringFormats.BOLD) + " A few gentle prods confirm that it's just as sensitive; " +
					"you can already feel your internals changing, adjusting to begin the production of unfertilized eggs. " + corruptionText;
			}
		}
		private static string BeeRestoreStr(TailData previousTailData, PlayerBase player)
		{
			return GenericLoseOvipositorText(previousTailData, player, "bee-like");
		}

		private static string GenericLoseOvipositorText(TailData previousTailData, PlayerBase player, string typeLike)
		{
			OvipositorData previousOvipositor = previousTailData.ovipositor;

			//check if the creature changed tail types. now, it may actually be possible the tail type changes and the ovipositor could still be available, but that's currently
			//not possible, and certainly not covered by generic text regardless.
			if (previousTailData.type != player.tail.type)
			{
				return "Now that you no longer have " + previousTailData.LongDescription(true) + ", your body can't support your " + previousOvipositor.ShortDescription() +
					", and it gradually collapses upon itself until it's no longer there." + SafelyFormattedString.FormattedText("Your no longer have an ovipositor!", StringFormats.BOLD);
			}
			else
			{
				string eggText = previousOvipositor.eggCount > 0 ? "(and eggs) vanish" : "vanishes";
				return SafelyFormattedString.FormattedText("Your ovipositor " + eggText + "as your body becomes less " + typeLike + ".", StringFormats.BOLD);
			}
		}
	}
}
