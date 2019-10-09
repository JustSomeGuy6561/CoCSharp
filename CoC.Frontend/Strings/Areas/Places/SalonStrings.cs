//SalonStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Salon
	{
		private static string SalonName()
		{
			return "Salon";
		}
		private static string SalonUnlockText()
		{
			var player = GameEngine.currentlyControlledCharacter;
			//OutputImage("location-salon"));
			StringBuilder sb = new StringBuilder("You step inside the cave, and are greeted by a sight you did not expect. The cave's floor is covered with smooth wood panelling " +
				"and the walls are nearly entirely covered with hanging mirrors. The few stalactites have hooks drilled into them, " +
				"from which hang hundreds of scissors, shears, razors, combs, and other hairstyling implements. It reminds you of the hair-cutter's shop in your hometown." +
				Environment.NewLine + Environment.NewLine + "There are a few chairs along the wall and goblins with latex dresses and gloves looking bored." +
				" At the sight of you they perk up and clamor around you excitedly, until one with a gravity-defying chest pushes them apart and greets you. \"" +
				SafelyFormattedString.FormattedText("I apologize for my daughters,", StringFormats.ITALIC) + "\" she says as she presses herself against you. \" +" +
				SafelyFormattedString.FormattedText("They're a bunch of brainless hussies for the most part. My name is Lynnette, and welcome to my salon! " +
					"You want your hair cut or lengthened? We've got you covered, and we don't ask for much - just a shot of cum.", StringFormats.ITALIC));

			if (player.genitals.numCocks == 0)
			{
				sb.Append(SafelyFormattedString.FormattedText(" You look like you don't got any of your own, but we've got glory holes in the back if you need to get some. " +
					"Just don't swallow too much, ok?", StringFormats.ITALIC) + "\"" + Environment.NewLine + Environment.NewLine +
					"She shows you to the back of the cave, which is boarded-up. There are about 20 holes in boards, and most are empty. While you watch, " +
					"a few new dicks slide in, and just as quickly the goblin's daughters commence sucking and fucking them. There are only a few you could take a crack at -" +
					" do you blow one (and if so which one)?");
			}
			else
			{
				string genderStr = player.genitals.trappyGenderToMaleFemaleIsMale() ? "kinky boi" : "naughty girl";
				sb.Append(SafelyFormattedString.FormattedText(" I'll even do you the favor of letting you blow it in my mouth, I've already got a bun in the oven. " +
					"So what do you say? Want a spooge and a haircut? Or would you rather go get your payment from one of the gloryholes in the back, you " + genderStr,
					StringFormats.ITALIC) + Environment.NewLine + Environment.NewLine + 
					"In the very back of the salon you can see a boarded-up wall with holes cut in it, some of which are currently plugged by various monstrous penises. " +
					"Do you let the goblin blow you, or do you go suck your payment from one of them?");
			}

			sb.Append(Environment.NewLine + Environment.NewLine + "(" + SafelyFormattedString.FormattedText("Salon unlocked in 'places' menu from camp", StringFormats.BOLD) + ")");

			return sb.ToString();
		}
	}
}
