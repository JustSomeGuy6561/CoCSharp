//LowerBodyStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 8:21 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Strings.BodyParts
{
	static class LowerBodyStrings
	{
		public static string HumanPlayer(Player player)
		{
			return "[legCountTextUC] normal human legs grow down from your waist, ending in normal human feet.";
		}
		public static string FerretPlayer(Player player)
		{
			return "Your [legCountText] legs are equally covered in [hairOrFurColor] fur, the lower half having a darker shade."
					 + " They end on digitigrade ferret paws with short claws.";
		}
		public static string HoofedPlayer(Player player)
		{
			return "Your [legCountText] legs are muscled and jointed oddly, covered in fur, and end in a bestial hooves.";
		}
		public static string WolfPlayer(Player player)
		{
			return "You have [legCountText] digitigrade legs that end in wolf paws.";
		}
		public static string DogPlayer()
		{
			return "[legCountTextUC] digitigrade legs grow downwards from your waist, ending in dog-like hind-paws.";
		}
		public static string NagaPlayer()
		{
			if (player.hasReptileUnderBody(true))
			{
				if (player.underBody.type == UnderBody.NAGA)
					nagaColors = [player.underBody.skin.tone, player.nagaLowerBodyColor2()];
				else
					nagaColors = [player.skin.tone, player.underBody.skin.tone];
				return "Below your waist, in place of where your legs would be, your body transitions into a long snake like tail."
						  + " Your snake-like lower body is covered by " + nagaColors[0] + " color scales,"
						  + " with " + nagaColors[1] + " color ventral scales along your underside.";
			}
			else
				return "Below your waist your flesh is fused together into a very long snake-like tail.";
		}
		public static string DemonicPlayer()
		{
			if (true)
			{
				return "Your [legCountText] perfect lissome legs end in mostly human feet, apart from the horn protruding straight down from the heel that forces you to walk with a sexy, swaying gait.";
			}
			else
			{
				return "Your [legCountText] lithe legs are capped with flexible clawed feet.  Sharp black nails grow where once you had toe-nails, giving you fantastic grip.";
			}
		}
		public static string BeePlayer()
		{
			return "Your [legCountText] legs are covered in a shimmering insectile carapace up to mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.  A bit of downy yellow and black fur fuzzes your upper thighs, just like a bee.";
		}
		public static string GooPlayer()
		{
			return "In place of legs you have a shifting amorphous blob.  Thankfully it's quite easy to propel yourself around on.  The lowest portions of your " + player.armorName + " float around inside you, bringing you no discomfort.";
		}
		public static string CatPlayer()
		{
			return "[legCountTextUC] digitigrade legs grow downwards from your waist, ending in soft, padded cat-paws.";
		}
		public static string LizardPlayer()
		{
			return "[legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		}
		public static string SalamanderPlayer()
		{
			return "[legCountTextUC] digitigrade legs covered in thick, leathery red scales up to the mid-thigh grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		}
		public static string BunnyPlayer()
		{
			return "Your [legCountText] legs thicken below the waist as they turn into soft-furred rabbit-like legs.  You even have large bunny feet that make hopping around a little easier than walking.";
		}
		public static string HarpyPlayer()
		{
			return "Your [legCountText] legs are covered with [furColor] plumage.  Thankfully the thick, powerful thighs are perfect for launching you into the air, and your feet remain mostly human, even if they are two-toed and tipped with talons.";
		}
		public static string KangarooPlayer()
		{
			return "Your [legCountText] furry legs have short thighs and long calves, with even longer feet ending in prominently-nailed toes.";
		}
		public static string ChitinousSpiderLegsPlayer()
		{
			return "Your [legCountText] legs are covered in a reflective black, insectile carapace up to your mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.";
		}
		public static string FoxPlayer()
		{
			return "Your [legCountText] legs are crooked into high knees with hocks and long feet, like those of a fox; cute bulbous toes decorate the ends.";
		}
		public static string DragonPlayer()
		{
			return "[legCountTextUC] human-like legs grow down from your [hips], sheathed in scales and ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		}
		public static string RaccoonPlayer()
		{
			return "Your [legCountText] legs, though covered in fur, are humanlike.  Long feet on the ends bear equally long toes, and the pads on the bottoms are quite sensitive to the touch.";
		}
		public static string ClovenHoofedPlayer()
		{
			return "[legCountTextUC] digitigrade legs form below your [hips], ending in cloven hooves.";
		}
		public static string ImpPlayer()
		{
			return "[legCountTextUC] digitigrade legs form below your [hips], ending in clawed feet. Three extend out the front, and one smaller one is in the back to keep your balance.";
		}
		public static string CockatricePlayer()
		{
			return "[legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet."
					 + " There are three long toes on the front, and a small hind-claw on the back."
					 + " A layer of " + (player.hasCockatriceSkin() ? player.skin.furColor : player.hair.color) + " feathers covers your legs from the"
					 + " hip to the knee, ending in a puffy cuff.";
		}
		public static string RedPandaPlayer()
		{
			return "Your [legCountText] legs are equally covered in [if (hasFurryUnderBody)[underBody.furColor]|black-brown] fur,"
					 + " ending on red-panda paws with short claws. They have a nimble and strong build,"
					 + " in case you need to escape from something.";
		}
	}
}
