//AntennaeStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:02 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;

namespace  CoC.Backend.BodyParts
{
	public partial class Antennae
	{
		public static string Name()
		{
			return "Antennae";
		}
	}

	public partial class AntennaeType
	{
		private static string RemoveAntennaeStr(AntennaeData oldData, PlayerBase p)
		{
			return oldData.type.RestoredString(oldData, p);
		}

		private static string BeeDesc(bool plural)
		{
			return plural ? "bee antennae" : "bee antenna";
		}

		private static string BeeLongDesc(AntennaeData antennae, bool plural, bool articleFormat)
		{
			string article;
			if (plural && articleFormat)
			{
				article = "a pair of ";
			}
			else if (articleFormat)
			{
				article = "a ";
			}
			//no article.
			else
			{
				article = "";
			}

			return article + "cute " + BeeDesc(plural);
		}

		private static string BeePlayerStr(PlayerBase player)
		{
			return "Floppy antennae also appear on your skull, bouncing and swaying in the breeze.";
		}

		private static string BeeTransformStr(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "Your head itches momentarily as two floppy antennae sprout from your " + player.hair.ShortDescription() + ". " +
				SafelyFormattedString.FormattedText("You now have bee antennae!", StringFormats.BOLD);
		}

		private static string BeeRestoreStr(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "Your " + player.hair.LongDescription() + " itches so you give it a scratch, only to have your antennae fall to the ground. What a relief."
				+ Environment.NewLine + SafelyFormattedString.FormattedText("You've lost your antennae", StringFormats.BOLD) + "!";
		}

		private static string CockatriceDesc(bool plural)
		{
			return plural ? "cockatrice antennae" : "cockatrice antenna";
		}

		private static string CockatriceLongDesc(AntennaeData antennae, bool plural, bool articleFormat)
		{
			string article;
			if (articleFormat && plural)
			{
				article = "a pair of ";
			}
			else if (articleFormat)
			{
				article = "an ";
			}
			else
			{
				article = "";
			}
			string noun = plural ? "quill-feathers" : "quill-feather";

			return article + "antenna-like " + noun;
		}

		private static string CockatricePlayerStr(PlayerBase player)
		{
			return "Two long antenna-like feathers sit on your hairline, curling over the shape of your head. " +
				   "They move with every expression, making even the most mundane action seem dramatic.";
		}

		private static string CockatriceTransformStr(AntennaeData oldAntennaeData, PlayerBase player)
		{
			string retVal = "Your forehead suddenly itches, your fingers instantly there to relieve the stress.";
			if (oldAntennaeData.type != AntennaeType.NONE)
			{
				retVal += " Your antennae feel weird, shifting uncomfortably, until they receed into your hairline. But the feeling doesn't fade "
					   + "- in fact, it seems to be getting stronger.";
			}
			else
				retVal += " You feel your pores stretch as the shaft of one of your feathers gets thicker and sturdier. A sudden"
					   + " pressure builds and then fades, making you groan as you hold your head tight. You tentatively run your fingers over the"
					   + " two spots where the feeling originated, only to feel the body of a long, soft and extravagant quill like feather on each"
					   + " side. While sturdy enough to support themselves these " + player.hair.hairColor.AsString() + " feathers flop daintily as you move."
					   + " They seem to move with your eyebrows, helping convey your expressions.";
			return retVal;
		}

		private static string CockatriceRestoreStr(AntennaeData oldData, PlayerBase player)
		{
			return "You feel your antennae like feathers shrivel at the root, the pair of soft quills falling softly to the"
				+ " ground as your pores close." + Environment.NewLine + SafelyFormattedString.FormattedText("You’ve lost your antennae like feathers!", StringFormats.BOLD);
		}

		//unused right now. was defined in vanilla as a fallback, which was never used.
		private static string GenericRestore(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "The muscles in your brow clench tightly, and you feel a tremendous pressure on your upper forehead."
				+ " When it passes, you touch yourself and discover " + SafelyFormattedString.FormattedText("your antennae have vanished", StringFormats.BOLD) +"!";
		}
	}
}
