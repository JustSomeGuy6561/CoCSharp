//AntennaeStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 2:02 PM
using CoC.BodyParts;
using CoC.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Strings.BodyParts
{
	static class AntennaeStrings
	{
		public static string RemoveAntennaeStr(AntennaeType antennae, Player p)
		{
			return "your antennae shrink, and eventually receed into your head completely. <b> Your antennae are gone!</b>";
		}

		public static string BeeAntennae()
		{
			return "bee antennae";
		}
		public static string BeeAntennaCreature(Gender gender)
		{
			return "a pair of cute bee antennae sits atop " + gender.asPronoun() + " head";
		}

		public static string BeeAntennaePlayer(Player player)
		{
			return "Floppy antennae also appear on your skull, bouncing and swaying in the breeze.";
		}

		public static string BeeAntennaeTransform(AntennaeType antennae, Player player)
		{
			return "Your head itches momentarily as two floppy antennae sprout from your " + player.hair.shortDescription() + ". <b>You now have bee antennae!</b>";
		}

		public static string CockatriceAntennae()
		{
			return "cockatrice antennae";
		}
		public static string CockatriceAntennaCreature(Gender gender)
		{
			return "a pair of feathers extend from " + gender.asPronoun() + " forehead, just above the eyes.";
		}

		public static string CockatriceAntennaePlayer(Player player)
		{
			return "Two long antennae like feathers sit on your hairline, curling over the shape of your head. " +
				   "They move with every expression, making even the most mundane action seem dramatic.";
		}

		public static string CockatriceAntennaeTransform(AntennaeType antennae, Player player)
		{
			//doesn't
			string retVal = "Your forehead suddenly itches, your fingers instantly there to relieve the stress.";
			if (antennae != AntennaeType.NONE)
			{
				retVal += " Your antennae feel weird, shifting uncomfortably, until they receed into your hairline. But the feeling doesn't fade "
				       +"- in fact, it seems to be getting stronger.";
			}
			else 
				retVal += " You feel your pores stretch as the shaft of one of your feathers gets thicker and sturdier. A sudden"
				       + " pressure builds and then fades, making you groan as you hold your head tight. You tentatively run your fingers over the"
				       + " two spots where the feeling originated, only to feel the body of a long, soft and extravagant quill like feather on each"
				       + " side. While sturdy enough to support themselves these " + player.hair.color + " feathers flop daintily as you move."
				       + " They seem to move with your eyebrows, helping convey your expressions.";
			return retVal;
		}
	}
}
