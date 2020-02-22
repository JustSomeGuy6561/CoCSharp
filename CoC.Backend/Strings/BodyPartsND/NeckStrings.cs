//NeckStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 11:45 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class NeckTattooLocation
	{
		private static string FrontButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FrontLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BackButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BackLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Neck
	{
		public static string Name()
		{
			return "Neck";
		}

		private string AllTattoosShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTattoosLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class NeckType
	{
		private string GenericButtonDesc()
		{
			return "Neck";
		}

		private string GenericLocationText(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		private string GenericPostDyeText(HairFurColors color)
		{
			return "";
		}


		private static string HumanDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("neck", singleMemberFormat);
		}
		private static string HumanLongDesc(NeckData neck, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "neck";
		}
		private static string HumanPlayerStr(Neck neck, PlayerBase player)
		{
			return "";
		}
		private static string DragonDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("draconic neck", singleMemberFormat);
		}
		private static string DragonLongDesc(NeckData neck, bool alternateFormat)
		{
			string lengthText;
			string article = "a ";
			if (neck.length < 8)
			{
				lengthText = "long-ish ";
			}
			else if (neck.length < 13)
			{
				lengthText = "long ";
			}
			else if (neck.length < 18)
			{
				lengthText = "very long ";
			}
			else
			{
				lengthText = "extremely long ";
				article = "an ";
			}
			if (alternateFormat)
			{
				return article + lengthText + "draconic neck";
			}
			else
			{
				return lengthText + "draconic neck";
			}
		}
		private static string DragonPlayerStr(Neck neck, PlayerBase player)
		{
			string lengthText;
			if (neck.length < 8) lengthText = (Measurement.UsesMetric ? "several centimeters " : "a few inches ") + "longer";
			else if (neck.length < 13) lengthText = "somewhat longer";
			else if (neck.length < 18) lengthText = "a great deal longer";
			else lengthText = "significantly longer";

			string visionStr = neck.length >= 8
				? "in virtually any direction more than makes up for it - you can even see your own back!"
				: "with a wider degree of motion means you have the same total range of vision, if not a little more.";

			return "Your neck is draconic, and " + lengthText + " than that of a human. The base of your neck is shifted more toward the back of your head, causing you "
				+ "to roll your head instead of move it side-to-side, but the flexibility granted by being able to move your neck " + visionStr;
		}
		private static string DragonTransformStr(NeckData previousNeckData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("You start to feel a sudden pain in your neck.");
			if (previousNeckData.length > player.neck.length)
			{
				sb.Append(" Its skin tightens as your spine gets shorter, until it's " + Measurement.ToNearestSmallUnit(player.neck.length, false, true) + " in length.");
			}
			else if (previousNeckData.length < player.neck.length)
			{
				sb.Append(" Your skin stretches and your spine grows a bit. Your neck has grown a few inches longer, and is now " +
					Measurement.ToNearestSmallUnit(player.neck.length, false, true) + ".");
			}
			else
			{
				sb.Append(" Your spine reshapes itself slightly, though you're pretty sure your neck is still the same size.");
			}
			if (previousNeckData.neckAtBaseOfSkull)
			{
				sb.Append(" An intense discomfort forces you to jutt your neck forward, where it is strangely much more comfortable. Your neck appears to have shifted away from " +
					"the base of your neck, and now rests angled away slightly. You realize you can't move your head laterally anymore; your head just rotates instead. After a few " +
					"disorienting moments, you realize you can make up for it simply by moving your neck, which is incredibly flexible.");
			}
			sb.Append(SafelyFormattedString.FormattedText("You now have a draconic neck!", StringFormats.BOLD));
			return sb.ToString();
		}
		private static string DragonRestoreStr(NeckData previousNeckData, PlayerBase player)
		{
			return GlobalStrings.NewParagraph() + "Your draconic neck reverts down toward a more humanoid length, finally settling when it's back to normal. With the change, " +
				"your neck also shifts back to the base or your skull, once again giving you lateral motion, though you don't have nearly the same flexibility. " +
				Environment.NewLine + SafelyFormattedString.FormattedText("You have a human neck again!", StringFormats.BOLD);
		}

		protected static string CockatriceDyeText(out bool isPlural)
		{
			isPlural = true;
			return "the feathers on your neck";
		}

		protected static string CockatricePostDyeText(HairFurColors color)
		{
			return color.AsString() + " feathers on your neck";
		}

		protected static string CockatriceDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("feathered, cockatrice-like neck", singleMemberFormat);
		}
		protected static string CockatriceLongDesc(NeckData neck, bool alternateFormat)
		{
			return (alternateFormat ? "an " : "") + "elongated, feathered cockatrice-like neck.";
		}
		protected static string CockatricePlayerStr(Neck neck, PlayerBase player)
		{
			return " Your neck is longer and thinner than a humans, as if it were somehow stretched. Around your neck is a ruff of " + neck.color.AsString() +
				" feathers which tends to puff out with your emotions.";
		}
		protected static string CockatriceTransformStr(NeckData previousNeckData, PlayerBase player)
		{
			string deltaLengthString;
			if (previousNeckData.length > player.neck.length)
			{
				deltaLengthString = " and soon shifts, retracting down toward a length and shape of a human, but thinner and just a little longer. " +
					GlobalStrings.CapitalizeFirstLetter(player.neck.color.AsString());
			}
			else if (previousNeckData.length < player.neck.length)
			{
				deltaLengthString = " and soon shifts, growing larger and thinner, until it's slightly longer than that of a human's. " +
					GlobalStrings.CapitalizeFirstLetter(player.neck.color.AsString());
			}
			else
			{
				deltaLengthString = ", though nothing seems to happen, at least right away. Soon, however, " + player.neck.color.AsString();
			}
			return GlobalStrings.NewParagraph() + "Your neck starts to tingle" + deltaLengthString + " feathers begin to grow out of it, one after another, " +
				"until a ruff of soft fluffy feathers has formed like that of an exotic bird." + Environment.NewLine +
				SafelyFormattedString.FormattedText("You now have a cockatrice neck!", StringFormats.BOLD);
		}
		protected static string CockatriceRestoreStr(NeckData previousNeckData, PlayerBase player)
		{
			return GlobalStrings.NewParagraph() + "Your neck starts to tingle uncomfortably, as if compressed like an accordion. As it does, the feathers that decorate your neck begin to " +
				"fall out until " + SafelyFormattedString.FormattedText("you're left with a normal neck!", StringFormats.BOLD);
		}

	}
}
