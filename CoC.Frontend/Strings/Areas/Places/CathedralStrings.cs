//CathedralStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Cathedral
	{
		private static string CathedralName()
		{
			return "Cathedral";
		}
		private static string CathedralUnlockText()
		{
			string lustyText = GameEngine.currentlyControlledCharacter.libido >= 40 ? " Maybe you could take this with you as a life-sized sex toy?" : "";
			//OutputImage("gargoyle-cathedral"));
			return "You finally close the distance between yourself and the strange structure, which begins to take shape ahead. Though it's half-buried under " +
				"what must be years of built-up sand and debris, you can clearly make out high stone walls supported by vaulted arches, broken every so often " +
				"by the shattered remains of stained-glass windows and a pair of utterly destroyed oaken doors nearly hidden behind a row of tall marble pillars, " +
				"many of which have long since crumbled. High above the ground, you can see a pair of tall, slender towers reaching up to the heavens, " +
				"one of which has been nearly obliterated by some unimaginably powerful impact, leaving it a stump compared to its twin. From the rooftops, " +
				"strange shapes look down upon you – stone statues made in the image of demons, dragons, and other monsters." + Environment.NewLine + Environment.NewLine +
				"You arrive at the grounds around the ruins, cordoned off by a waist-high wrought-iron fence that surrounds the building and what once might have been " +
				"a beautiful, pastoral garden, now rotting and wilted, its trees chopped down or burned, twig-like bushes a mere gale's difference from being tumbleweeds. " +
				"A few dozen tombstones outline the path to a gaping maw that was once the great wooden doors. Seeing no obvious signs of danger, you make your way inside, " +
				"stepping cautiously over the rubble and rotting debris that litters the courtyard." + Environment.NewLine + Environment.NewLine +
				"It's quite dark inside, illuminated only by thin shafts of light streaming in from the shattered windows and sundered doors. " +
				"You can make out a few dozen wooden pews, all either thrown aside and rotting or long-since crushed, leading up to a stone altar and an effigy of a great green tree, " +
				"now covered in graffiti and filth. Stairs beside the altar lead up to the towers, and down to what must be catacombs or dungeons deep underground." + Environment.NewLine +
				Environment.NewLine + "However, what most catches your eye upon entering the sanctuary are the statues that line the walls. Beautifully carved " +
				"gray stone idols of creatures, chimeras, and nearer to the altar, god-like beings, are each set into their own little alcove. Unfortunately, " +
				"most have been destroyed along with the cathedral, each lying in a pile of its own shattered debris; some having whole limbs or other extremities broken off " +
				"and carried away by looters, leaving them mere shadows of their former glory." + Environment.NewLine + Environment.NewLine +
				"All of them but one. In the farthest, darkest alcove you see a single statue that still seems intact. It is of a woman – well, more like a succubus than a human woman. " +
				"Though posed in a low, predatory crouch, she would normally stand nearly six feet tall, hair sculpted to fall playfully about her shoulders. " +
				"A pair of bat-like wings protruding from her back curl back to expose the lush, smooth orbs of her breasts, easily DD's on a human. A spiked, " +
				"mace-like tail curls about her legs that are attached to the pedestal upon which she's placed. As you stand marveling at the statue's beauty, " +
				"you cannot help but notice the slit of her pussy nearly hidden beneath her. Oddly, it seems to have been carved hollow so that you could easily stick " +
				"a few fingers inside if you so choose." + lustyText + Environment.NewLine + Environment.NewLine +
				"However, your attention is soon drawn from her body to the pedestal upon which she stands. A pair of solid gold chains extend from the pedestal to her wrists, " +
				"binding the statue. You notice a plaque has been bolted to the pedestal, a feature not present on any of the other statues here. Leaning down, " +
				"you blow a sizable amount of dust from the plaque, revealing the following short inscription:" + Environment.NewLine + Environment.NewLine +
				"\"" + SafelyFormattedString.FormattedText("Break my bonds to make me tame.", StringFormats.ITALIC) + "\"" + Environment.NewLine + Environment.NewLine + 
				"You suppose you could break the chains on the statue. But who knows what will happen if you do?" + Environment.NewLine + Environment.NewLine +
				SafelyFormattedString.FormattedText("You have discovered the cathedral. You can return here in the future by selecting it from the 'Places' menu in your camp.", StringFormats.BOLD);
		}

	}
}
