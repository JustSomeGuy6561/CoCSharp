using CoC.Backend.Settings.Gameplay;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Backend.UI.DisplayBase;


namespace CoC.Frontend.Encounters.Common
{
	class MotorboatScene
	{
		bool silly => SillyModeSettings.isEnabled;

		//motorboat scene. on a herm character, variants for balls or no.
		void DoMotorboat(Player player)
		{
			StandardDisplay currentDisplay = DisplayManager.GetCurrentDisplay();
			currentDisplay.OutputText(" Your request for sex is met with obvious enthusiasm; she's completely stripped off all her clothing at an unbelievable pace. " +
				"She's definitely well endowed but your attention is immediately drawn to her volumous breasts. A giggle escapes her lips, and you realize you've already taken one " +
				"in hand. Embarrassed, you remove your hand, a sheepish look on your face. <i>Drawn to my knockers, ehh? Can't say i blame you - they are irresistable.</i> " +
				"You can't get over just how plush they are, and you have one thought in mind: \"I wanna motorboat these puppies.\" A moment passes before you realize " +
				"you said that aloud. A bemused look crosses <Name>'s face, and she responds: <i>Not sure what you mean by that, hun - care to explain?</i>" +
				"You quickly explain what that entails, ");
			if (silly)
			{
				currentDisplay.OutputText("careful to omit the fact that its namesake doesn't exist yet. ");
			}
			else
			{
				currentDisplay.OutputText("though you're not entirely sure where the name comes from. ");
			}

			currentDisplay.OutputText("<i>Sounds like fun, though i hope that's not all you have in mind.</i> You're sure you'll come up with something, but for now you're focused " +
				"on burying your head between the twin prizes. With your face now firmly in place, you begin to make the telltale 'brrrr' sound, grabbing both breasts so they remain in" +
				"place as you move your head from side to side. Her short fur caresses you with each motion, feeling absolutely divine. Suddenly, they seem to vibrate on their own, " +
				"and it takes you a moment to realize the source is <Name>, laughing. Now Reminded you're not the only one participating in your little fantasy, You pause, " +
				"curious to see exactly what she's laughing at. ");


			//if (player.hasBeard)
			//{
			//	currentDisplay.OutputText("<i>Yo-you're <beard></i> she manages between giggles <i>it-it tickles!</i>");
			//}
			//else
			//{
			currentDisplay.OutputText("<i>Yo-you look</i> she manages unable to finish the thought before bursting out into another round of giggles. <i>You look so absurd!</i>");
			//}
			currentDisplay.OutputText("<i> D-Don't stop, love</i>, she manages before bursting into laughter once again. You oblige, but it's not exactly the reaction you were hoping " +
				"for. A thought crosses your mind, and the laughter is soon cut short, replaced with an audible gasp as your fingers find her nipples and tug, ever so gently. " +
				"Seizing the opportunity, you lift your head, sticking out your tongue just enough");

			if (player.tongue.piercings.wearingJewelry)
			{
				currentDisplay.OutputText("so that both it and the bottom half of your tongue stud make contact with her sensitive fur between her bust. You gradually move " +
					"down her chest, occasionally adjusting your tongue to vary when and where your piercing makes contact. You can tell this is having a marked impact, as " +
					"she twitches each time the contrasting sensation of warm and cold shifts.");
			}
			else
			{
				currentDisplay.OutputText("so that you're just barely dragging your tongue along the sensitive fur between her bust. You move down withward with agonizing slowness, " +
					"trying to gauge how she'll react to the new stimulation. You can tell you're starting to get to her as a shudder runs through her body, and you continue on with " +
					"renewed vigor.");
			}

			currentDisplay.OutputText("You continue your quest downward, lingering ever so slightly whenever she draws a breath. By the time you reach her navel, her breathing has " +
				"become erratic and you know you've got her. As you probe ever closer to her dual sexes, you notice her breath has caught, as if desperately trying to speed you up." +
				"That simply won't do, you think to yourself, your fingers resuming their assault on her sensitive nipples. The resulting moan is music to your ears. " +
				"You consider continuing to tease her, but decide she's had enough. You run you tongue down her cock, which rapidly elongates under the sudden stimulation. " +
				"You pause for a moment, allow it to rise to a full erection before continuing, this time down its generous length");

			//if (hasBalls)
			//{

			//	currentDisplay.OutputText(", towards her now throbbing balls. They too feel your tongue, but only briefly; you're true target glistening just below them.");
			//}
			//else
			//{
			currentDisplay.OutputText(". As you reach its base, you briefly pause, but soon find your way to your true target, the glistening entrance situated just below.");
			//}

			currentDisplay.OutputText("At this point, you abandon any remsining hints of foreplay, attacking her buzzer with your tongue. ");

			if (player.tongue.piercings.wearingJewelry)
			{
				currentDisplay.OutputText("With each pass, you ");
			}
		}
	}
}
