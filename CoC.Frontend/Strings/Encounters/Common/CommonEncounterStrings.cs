using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Encounters.Common
{
	internal partial class BigJunkEncounter
	{
		private static void OutputBigJunkText(bool isForest, bool isLake)
		{
			/*
			Player player = GameEngine.currentPlayer;
			Cock largestCock = player.genitals.LargestCock();
			string helperText1, helperText2;
			if (isForest)
			{
				helperText1 = "various paths of the forest";
				helperText2 = "earth behind you.";
			}
			else if (isLake)
			{
				helperText1 = "grassy and muddy shores of the lake";
				helperText2 = "wet ground behind you.";
			}
			else
			{
				helperText1 = "sandy dunes of the desert";
				helperText2 = "sandscape behind you. The incredibly hot surface of the desert causes your loins to sweat heavily and fills them with relentless heat.";
			}
			OutputText("Walking along the " + helperText1 + ", you find yourself increasingly impeded by the bulk of your " + largestCock.fullDescription() + " dragging along the " + helperText2);
			if (player.cocks.Count == 1)
			{
				if (isForest)
				{
					helperText1 = " across the grass, twigs, and exposed tree roots";
					helperText2 = " the fingers of a giant hand sliding along the head of your " + largestCock.shortDescription() + ", gently jerking it off.";
				}
				else if (isLake)
				{
					helperText1 = " through the lakeside mud";
					helperText2 = " the velvety folds of a monstrous pussy sliding along the head of your " + largestCock.shortDescription() + ", gently attempting to suck it off.";
				}
				
				else
				{
					helperText1 = " along the dunes";
					helperText2 = " the rough textured tongue of a monstrous animal sliding along the head of your " + largestCock.shortDescription() + ".";
				}
				OutputText(" As it drags" + helperText1 + ", the sensation forces you to imagine" + helperText2);
			}
			else //(player.cocks.length >= 2) 
			{
				if (isForest)
				{
					helperText1 = " across the grass, twigs, and exposed tree roots";
					helperText2 = " rough fingers of " + Utils.Count(player.genitals.numCocks) + " different monstrous hands were sliding over each shaft, gently jerking them off.";
				}
				else if (isLake)
				{
					helperText1 = " through the mud";
					helperText2 = " lips of " + Utils.Count(player.genitals.numCocks) + " different cunts were slobbering over each one.";
				}
				else
				{
					helperText1 = " through the sands";
					helperText2 = " rough textured tongues of " + Utils.Count(player.genitals.numCocks) + " different monstrous animals were slobbering over each one.";
				}
				OutputText(" With all of your " + player.genitals.AllCocksShortDesc() + " dragging" + helperText1 + ", they begin feeling as if the" + helperText2);
			}
			OutputText(Environment.NewLine + Environment.NewLine);
			
			//PARAGRAPH 2
			if (player.lowerBody.isQuadruped)
			{
				helperText1 = "the barrel of your bestial torso to the ground. Normally your erection would merely hover above the ground in between your legs";
				helperText2 = "being forcibly pulled down at your hind legs until your bestial body is resting on top of your " + player.genitals.AllCocksShortDesc() + ".";
			}
			else
			{
				helperText1 = "your torso to the ground. Normally your erection would merely raise itself skyward";
				helperText2 = "forcibly pivoting at the hips until your torso is compelled to rest face down atop your " + player.genitals.AllCocksShortDesc() + ".";
			}
			OutputText(" The impending erection can't seem to be stopped. Your sexual frustration forces stiffness into your " + player.genitals.AllCocksShortDesc() + ", which forces" +
				helperText1 + ", but your genitals have grown too large and heavy for your " + player.hips.ShortDescription() + " to hold them aloft. Instead, you feel your body" + helperText2);

			if (player.biggestTitSize() >= 35)
			{
				if (player.lowerBody.isQuadruped)
				{

				}
				else
				{

					OutputText(" Your " + player.chestDesc() + "hang lewdly off your torso to rest" + helperText1 + ".Their immense weight anchors your body, further preventing your torso from lifting itself up." + helperText2;
				}
			}

			if (player.hasBalls)
			{
				if (isForest) helperText1 = " Your " + player.balls.ShortDesc() + " pulse with the need to release their sperm through your " + player.genitals.AllCocksShortDesc() + " and onto the fertile soil of the forest.";
				else if (isLake) helperText1 = " Your " + player.balls.ShortDesc() + " pulse with the need to release their sperm through your " + player.genitals.AllCocksShortDesc() + " and into the waters of the nearby lake.";
				else helperText1 = " The fiery warmth of the desert caresses it, causing your " + player.balls.ShortDesc() + " to pulse with the need to release their sperm through your " + player.genitals.AllCocksShortDesc() + ".";
				
				OutputText(" Your " + player.skin.tone + " " + player.sackDescript() + " rests beneath your raised " + player.buttDescript() + "." + helperText1);
			}

			if (player.hasVagina)
			{
				OutputText("  Your " + player.vaginaDescript() + " and " + player.clitDescript() + " are thoroughly squashed between the bulky flesh where your male genitals protrude from between your hips and the " + player.buttDescript() + " above.");
				if (player.lowerBody.isQuadruped)
				{
					//IF CHARACTER HAS A DROOLING PUSSY ADD SENTENCE
					if (player.vaginas[0].wetness >= VaginalWetness.DROOLING)
					{
						if (isLake || isForest) helperText1 = "  A leaf falls from a tree and lands on the wet lips of your cunt, its light touch teasing your sensitive skin.  Like a mare or cow in heat, your juices stream from your womanhood and pool in";
						else helperText1 = " The desert sun beats down on your body, its fiery heat inflaming the senses of your vaginal lips.";

						if (isForest) helperText2 = " the dirt and twigs beneath you.";
						else if (isLake) helperText2 = "the mud beneath you. The sloppy fem-spunk only makes the ground more muddy.";
						else helperText2 = "Juices stream from your womanhood and begin pooling on the hot sand beneath you.";

						OutputText(helperText1 + helperText2);
					}
				}
				else
				{
					//IF CHARACTER HAS A DROOLING PUSSY ADD SENTENCE
					if (player.vaginas[0].wetness >= VaginalWetness.DROOLING)
					{
						if (isForest) helperText1 = "the dirt and twigs beneath you. The sticky fem-spunk immediately soaks down into the rich soil.";
						else if (isLake) helperText1 = "the wet ground beneath you. The drooling fem-spunk only makes the ground more muddy.";
						else helperText1 = " the hot sand beneath you. Wisps of steam rise up into the air only to tease your genitals further.";

						OutputText("Juices stream from your womanhood and begin pooling on " + helperText1);
					}
				}
			}

			//PARAGRAPH 3

			OutputText(Environment.NewLine + Environment.NewLine + "You realize you are effectively trapped here by your own body.");
			//CORRUPTION BASED CHARACTER'S VIEW OF SITUATION
			if (player.corruption < 33) OutputText(" Panic slips into your heart as you realize that if any dangerous predator were to find you in this state, you'd be completely defenseless.  You must find a way to regain your mobility immediately!");
			else if (player.corruption < 66) OutputText(" You realize that if any dangerous predator were to find you in this state, you'd be completely defenseless!  You must find a way to regain your mobility... yet there is a certain appeal to imagining how pleasurable it would be for a sexual predator to take advantage of your obscene body.");
			else OutputText(" Your endowments have rendered you completely helpless should any predators find you.  Somewhere in your heart, you find this prospect almost exhilarating.  The idea of being a helpless fucktoy for a wandering beast is unusually inviting to you.  Were it not for the thought that you might starve to death, you'd be incredibly tempted to remain right where you are.");

			//SCENE END = IF CHARACTER HAS FULL WINGS
			if (player.wings.canFly)
			{
				if (isForest) helperText1 = " out of the forest";
				else if (isLake) helperText1 = " out of the mud";
				else helperText1 = " across the hot sands";
				OutputText(" You extend your wings and flap as hard as you can until at last, you manage to lighten the bulk of your body. It helps just enough to let you drag your genitals" + helperText1 + " and back to camp.The ordeal takes nearly an hour for you to return and deal with.");
			}
			//SCENE END IF CHARACTER HAS CENTAUR BODY
			else if (player.lowerBody.isQuadruped)
			{
				if (isForest)
				{
					helperText1 = " soft dirt";
					helperText2 = " ground fails to provide enough leverage to lift your bulk. You breath in deeply and lean side to side, until eventually, your feet brace against the various roots of the trees around you. " +
						"With a crude crawl, your legs manage to shuffle your body and genitals out of the forest and back to camp.";
				}
				else if (isLake)
				{
					helperText1 = " wet ground";
					helperText2 = " mud fails to provide enough leverage to lift your bulk. You breath in deeply and lean side to side, trying to find some easier vertical leverage beneath your feet." +
						" Eventually, with a crude crawl, your legs manages to push the bulk of your body onto more solid ground. With great difficulty, you spend the next hour shuffling your genitals back to camp.";
				}
				else
				{
					helperText1 = " surface of the dune you are trapped on";
					helperText2 = " soft sand fails to provide enough leverage to lift your bulk. You breath in deeply and lean from side to side, trying to find some easier vertical leverage." +
						" Eventually, with a crude crawl, your legs manage to push the bulk of your body onto more solid ground. With great difficulty, you spend the next hour shuffling your genitals across the sandscape and back to camp.";
				}

				OutputText("  You struggle and work your multiple legs against the" + helperText1 + ".  Your " + player.feet() + " have consistent trouble finding footing as the" + helperText2);
			}
			//SCENE END = FOR ALL OTHER CHARACTERS
			else
			{
				if (isForest) helperText1 = " across the forest floor.";
				else if (isLake) helperText1 = " through the mud.";
				else helperText1 = " across the warm sand.";
				OutputText(" You struggle and push with your " + player.legs() + " as hard as you can, but it's no use. You do the only thing you can and begin stroking your " +
					player.genitals.AllCocksShortDesc() + " with as much vigor as you can muster. Eventually your body tenses and a light load of jizz erupts from your body, " +
					"but the orgasm is truly mild compared to what you need. You're simply too weary from struggling to give yourself the masturbation you truly need, but you continue to try." +
					" Nearly an hour later " + player.genitals.AllCocksFullDesc() + " has softened enough to allow you to stand again, and you make your way back to camp, still dragging your genitals" + helperText1); 
			}*/
		}
	}
}
