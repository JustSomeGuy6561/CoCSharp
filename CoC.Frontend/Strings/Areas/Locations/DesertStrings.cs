//ForestStrings.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:48 PM
using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Desert : LocationBase
	{
		private static string DesertName()
		{
			return "Desert";
		}

		private static string DesertMimicText()
		{
			return "The wind howls through the tall pillars of rock that thrust out of the sand on both sides ";
		}

		private static string DesertUnlock()
		{
			Player player = GameEngine.currentPlayer;

			string footString;
			if (player.feet.type.isFeet) footString = "inside your footwear, between your toes.";
			else if (player.feet.type.isClaws) footString = "between your claws.";
			else if (player.feet.type.isPaws) footString = "in your paws.";
			else if (player.feet.type.isInsectoid) footString = "underneath your insectoid feet.";
			else if (player.lowerBody.type == LowerBodyType.NAGA) footString = "in your scales.";
			else if (player.lowerBody.type == LowerBodyType.GOO) footString = "under your goo";
			else footString = "under your \"feet\"";

			return "You stumble as the ground shifts a bit underneath you. Groaning in frustration, you straighten up and discover the rough feeling of sand " +
				footString + Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the Desert!", StringFormats.BOLD);
		}
	}
}
