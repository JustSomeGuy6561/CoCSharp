//SwampStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class Swamp
	{
		private static string SwampName()
		{
			return "Swamp";
		}
		private static string SwampUnlock()
		{
			return "All things considered, you decide you wouldn't mind a change of scenery. Gathering up your belongings, you begin a journey into the wasteland. " +
				"The journey begins in high spirits, and you whistle a little traveling tune to pass the time. After an hour of wandering, however, " +
				"your wanderlust begins to whittle away. Another half-hour ticks by. Fed up with the fruitless exploration, you're nearly about to head back to camp " +
				"when a faint light flits across your vision. Startled, you whirl about to take in three luminous will-o'-the-wisps, swirling around each other whimsically." +
				" As you watch, the three ghostly lights begin to move off, and though the thought of a trap crosses your mind, you decide to follow." +
				Environment.NewLine + Environment.NewLine + "Before long, you start to detect traces of change in the environment. " +
				"The most immediate difference is the increasingly sweltering heat. A few minutes pass, then the will-o'-the-wisps plunge into the boundaries of a dark, " +
				"murky, stagnant swamp; after a steadying breath you follow them into the bog. Once within, however, the gaseous balls double off in different directions, " +
				"causing you to lose track of them. You sigh resignedly and retrace your steps, satisfied with your discovery. " +
				"Further exploration can wait. For now, your camp is waiting." + Environment.NewLine + Environment.NewLine +
				SafelyFormattedString.FormattedText("You've discovered the Swamp!", StringFormats.BOLD);
		}

	}
}
