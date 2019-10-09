//OwcaStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Owca
	{
		private static string OwcaName()
		{
			return "Owca";
		}
		private static string OwcaUnlockText()
		{
			Player player = GameEngine.currentlyControlledCharacter;
			string corruptText = player.corruption >= 50 ? " You can't help but smile internally. Let her think that, if she wants." : "";
			return "Using the vivid memory of your last encounter, you easily find Owca village; the first houses are in sight when you spot someone rushing to you. " +
				"You recognize Rebecc, the girl you first met in the plains and who begged you for your help. She is smiling, her arms open for a welcoming embrace. " +
				"Before you even try to say something she is already hugging you, pressing her warm body against yours as she takes you in her arms. " +
				"She holds you tightly for a minute before kissing you everywhere affectionately; you haven't said a single word and your cheeks and " +
				"lips are already bearing the mark of her lipstick." + Environment.NewLine + Environment.NewLine + "\"" +
				SafelyFormattedString.FormattedText("Thank you! Thank you so much, " + GameEngine.currentlyControlledCharacter.name + "! You are a wonderful person! " +
					"What you did was noble and selfless. Our people will remember you for years to come!", StringFormats.ITALIC) +
				corruptText + Environment.NewLine + Environment.NewLine + "She grabs your hand and drags you to her village, hopping joyfully as she keeps " +
				"thanking you for your heroic gesture. You finally reach her home; a handful of people join you, attracted by the noise she's been making. " +
				"People are everywhere showing unconditional gratitude, bowing down and muttering thankful words as you go by; it's almost as if they were intimidated -" +
				" or ashamed - by someone worthier than them. Eventually you are left alone with the charming farmer girl." + Environment.NewLine + Environment.NewLine +
				"\"" + SafelyFormattedString.FormattedText("My home is yours, hero; alongside everyone else's. You will always be welcome here. " +
					"Don't hesitate to come to my place; you must feel all dirty and sullied from these corrupted monsters. Don't worry, I'll clean you up.", StringFormats.ITALIC) +
				"\" She winks at you and walks into her house, leaving her door open as an invitation." + Environment.NewLine + Environment.NewLine +
				"(" + SafelyFormattedString.FormattedText("Owca Village added to your 'Places' menu.", StringFormats.BOLD) + ")";
		}
	}
}
