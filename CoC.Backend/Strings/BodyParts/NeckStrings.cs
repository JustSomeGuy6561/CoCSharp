//NeckStrings.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 11:45 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

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
			return "neck";
		}
		private static string HumanLongDesc(NeckData neck)
		{
			return "neck";
		}
		private static string HumanPlayerStr(Neck neck, PlayerBase player)
		{
			return "";
		}
		private static string DragonDesc()
		{
			return "draconic neck";
		}
		private static string DragonLongDesc(NeckData neck)
		{
			string lengthText;
			if (neck.length < 8) lengthText = "a long-ish ";
			else if (neck.length < 13) lengthText = "a long ";
			else if (neck.length < 18) lengthText = "a very long ";
			else lengthText = "an extremely long ";
			return lengthText + "draconic neck";
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
			return Utils.NewParagraph() + "Your draconic neck reverts down toward a more humanoid length, finally settling when it's back to normal. With the change, " +
				"your neck also shifts back to the base or your skull, once again giving you lateral motion, though you don't have nearly the same flexibility. " +
				Environment.NewLine + SafelyFormattedString.FormattedText("You have a human neck again!", StringFormats.BOLD);
		}
		private static string CockatriceDesc()
		{
			return "cockatrice-like neck";
		}
		private static string CockatriceLongDesc(NeckData neck)
		{
			return "an elongated, feathered cockatrice-like neck.";
		}
		private static string CockatricePlayerStr(Neck neck, PlayerBase player)
		{
			return " Your neck is longer and thinner than a humans, as if it were somehow stretched. Around your neck is a ruff of " + neck.color.AsString() +
				" feathers which tends to puff out with your emotions.";
		}
		private static string CockatriceTransformStr(NeckData previousNeckData, PlayerBase player)
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
			return Utils.NewParagraph() + "Your neck starts to tingle" + deltaLengthString + " feathers begin to grow out of it, one after another, " +
				"until a ruff of soft fluffy feathers has formed like that of an exotic bird." + Environment.NewLine +
				SafelyFormattedString.FormattedText("You now have a cockatrice neck!", StringFormats.BOLD);
		}
		private static string CockatriceRestoreStr(NeckData previousNeckData, PlayerBase player)
		{
			return Utils.NewParagraph() + "Your neck starts to tingle uncomfortably, as if compressed like an accordion. As it does, the feathers that decorate your neck begin to " +
				"fall out until " + SafelyFormattedString.FormattedText("you're left with a normal neck!", StringFormats.BOLD);
		}

	}
}
