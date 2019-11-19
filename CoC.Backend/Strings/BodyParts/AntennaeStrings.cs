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
			return oldData.type.restoredString(oldData, p);
		}

		private static string BeeDesc()
		{
			return "bee antennae";
		}
		private static string BeeLongDesc(Antennae antennae)
		{
			return "a pair of cute bee antennae";
		}

		private static string BeePlayer(PlayerBase player)
		{
			return "Floppy antennae also appear on your skull, bouncing and swaying in the breeze.";
		}

		private static string BeeTransform(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "Your head itches momentarily as two floppy antennae sprout from your " + player.hair.shortDescription() + ". " +
				SafelyFormattedString.FormattedText("You now have bee antennae!", StringFormats.BOLD);
		}

		private static string CockatriceDesc()
		{
			return "cockatrice antennae";
		}
		private static string CockatriceLongDesc(Antennae antennae)
		{
			return "a pair of quill-like feathers atop the eyes";
		}

		private static string CockatricePlayer(PlayerBase player)
		{
			return "Two long antennae like feathers sit on your hairline, curling over the shape of your head. " +
				   "They move with every expression, making even the most mundane action seem dramatic.";
		}

		private static string CockatriceTransform(AntennaeData oldAntennaeData, PlayerBase player)
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

		private static string CockatriceRestore(AntennaeData oldData, PlayerBase player)
		{
			return "You feel your antennae like feathers shrivel at the root, the pair of soft quills falling softly to the"
				+ " ground as your pores close." + Environment.NewLine + SafelyFormattedString.FormattedText("You’ve lost your antennae like feathers!", StringFormats.BOLD);
		}

		private static string BeeRestore(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "Your " + player.hair.LongDescription() + " itches so you give it a scratch, only to have your antennae fall to the ground. What a relief."
				+ Environment.NewLine + SafelyFormattedString.FormattedText("You've lost your antennae", StringFormats.BOLD) + "!";
		}

		//unused right now. was defined in vanilla as a fallback, which was never used.
		private static string GenericRestore(AntennaeData oldAntennaeData, PlayerBase player)
		{
			return "The muscles in your brow clench tightly, and you feel a tremendous pressure on your upper forehead."
				+ " When it passes, you touch yourself and discover " + SafelyFormattedString.FormattedText("your antennae have vanished", StringFormats.BOLD) +"!";
		}
	}
}
