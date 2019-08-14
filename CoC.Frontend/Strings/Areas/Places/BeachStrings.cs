//BazaarStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Beach
	{
		private static string BeachName()
		{
			return "Beach";
		}
		private static string BeachUnlockText()
		{
			string corruptText = GameEngine.currentPlayer.corruption > 60 ? " or fuck" : "";
			return "You journey around the lake, seeking demons to fight" + corruptText + ". The air is fresh, and the grass is cool and soft under your feet." +
				" Soft waves lap against the muddy sand of the lake-shore, as if radiating outward from the lake. You pass around a few bushes carefully, " +
				"being wary of hidden 'surprises', and come upon any area of pristine sand, unlike anything you've previously seen in Mareth. You can't help but run towards it, " +
				"your previous caution momentarily forgotten. Once closer, you realize your initial assumptions were right; you ARE at some sort of beach. " +
				"The lake's waves seem stronger here, forming a wake near the shore, and you're sure you could surf it with the right materials. " +
				"Further along, you spot the lifeguard tower, though it seems to be empty at the moment. There's even a series of docks for fishing and boating, " +
				"with a few rowboats tied along them. A sign hanging above one of the docks denotes they are available for public use. " +
				"It's at this point you notice the lack of people, and your wariness returns. You quickly flee, but make note of this location so you could investigate it more later. " +
				Environment.NewLine +  SafelyFormattedString.FormattedText("You have discovered the Beach!", StringFormats.BOLD) + Environment.NewLine +
				"(You may return here to investigate this area and its surroundings by using the 'places' menu.)";
		}

	}
}
