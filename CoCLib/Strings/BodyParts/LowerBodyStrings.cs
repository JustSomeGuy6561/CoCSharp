//LowerBodyStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 8:21 PM
using CoC.BodyParts;
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



		public static string HumanDesc()
		{

		}
		public static string HumanFullDesc(LowerBody lowerBody)
		{

		}
		public static string HumanPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HumanTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HumanRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HoovedDesc()
		{

		}
		public static string HoovedFullDesc(LowerBody lowerBody)
		{

		}
		public static string HoovedPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HoovedTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HoovedRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DogDesc()
		{

		}
		public static string DogFullDesc(LowerBody lowerBody)
		{

		}
		public static string DogPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DogTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DogRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string NagaDesc()
		{

		}
		public static string NagaFullDesc(LowerBody lowerBody)
		{

		}
		public static string NagaPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string NagaTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string NagaRestoreStr(LowerBody lowerBody, Player player)
		{

		}

		public static string DemonHiHeelsDesc()
		{

		}
		public static string DemonHiHeelsFullDesc(LowerBody lowerBody)
		{

		}
		public static string DemonHiHeelsPlayerStr(LowerBody lowerBody, Player player)
		{
			return "Your " + lowerBody.legCount.ToString() + " perfect lissome legs end in mostly human feet, apart from the " +
				"horn protruding straight down from the heel that forces you to walk with a sexy, swaying gait.";

		}
		public static string DemonHiHeelsTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DemonHiHeelsRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DemonClawDesc()
		{

		}
		public static string DemonClawFullDesc(LowerBody lowerBody)
		{

		}
		public static string DemonClawPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DemonClawTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DemonClawRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BeeDesc()
		{

		}
		public static string BeeFullDesc(LowerBody lowerBody)
		{

		}
		public static string BeePlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BeeTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BeeRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string GooDesc()
		{

		}
		public static string GooFullDesc(LowerBody lowerBody)
		{

		}
		public static string GooPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string GooTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string GooRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CatDesc()
		{

		}
		public static string CatFullDesc(LowerBody lowerBody)
		{

		}
		public static string CatPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CatTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CatRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string LizardDesc()
		{

		}
		public static string LizardFullDesc(LowerBody lowerBody)
		{

		}
		public static string LizardPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string LizardTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string LizardRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string PonyDesc()
		{

		}
		public static string PonyFullDesc(LowerBody lowerBody)
		{

		}
		public static string PonyPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string PonyTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string PonyRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BunnyDesc()
		{

		}
		public static string BunnyFullDesc(LowerBody lowerBody)
		{

		}
		public static string BunnyPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BunnyTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string BunnyRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HarpyDesc()
		{

		}
		public static string HarpyFullDesc(LowerBody lowerBody)
		{

		}
		public static string HarpyPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HarpyTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string HarpyRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string KangarooDesc()
		{

		}
		public static string KangarooFullDesc(LowerBody lowerBody)
		{

		}
		public static string KangarooPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string KangarooTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string KangarooRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SpiderDesc()
		{

		}
		public static string SpiderFullDesc(LowerBody lowerBody)
		{

		}
		public static string SpiderPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SpiderTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SpiderRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DriderDesc()
		{

		}
		public static string DriderFullDesc(LowerBody lowerBody)
		{

		}
		public static string DriderPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DriderTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DriderRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FoxDesc()
		{

		}
		public static string FoxFullDesc(LowerBody lowerBody)
		{

		}
		public static string FoxPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FoxTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FoxRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DragonDesc()
		{

		}
		public static string DragonFullDesc(LowerBody lowerBody)
		{

		}
		public static string DragonPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DragonTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string DragonRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RaccoonDesc()
		{

		}
		public static string RaccoonFullDesc(LowerBody lowerBody)
		{

		}
		public static string RaccoonPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RaccoonTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RaccoonRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FerretDesc()
		{

		}
		public static string FerretFullDesc(LowerBody lowerBody)
		{

		}
		public static string FerretPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FerretTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string FerretRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ClovenHoofDesc()
		{

		}
		public static string ClovenHoofFullDesc(LowerBody lowerBody)
		{

		}
		public static string ClovenHoofPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ClovenHoofTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ClovenHoofRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string EchidnaDesc()
		{

		}
		public static string EchidnaFullDesc(LowerBody lowerBody)
		{

		}
		public static string EchidnaPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string EchidnaTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string EchidnaRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SalamanderDesc()
		{

		}
		public static string SalamanderFullDesc(LowerBody lowerBody)
		{

		}
		public static string SalamanderPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SalamanderTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string SalamanderRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string WolfDesc()
		{

		}
		public static string WolfFullDesc(LowerBody lowerBody)
		{

		}
		public static string WolfPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string WolfTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string WolfRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ImpDesc()
		{

		}
		public static string ImpFullDesc(LowerBody lowerBody)
		{

		}
		public static string ImpPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ImpTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string ImpRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CockatriceDesc()
		{

		}
		public static string CockatriceFullDesc(LowerBody lowerBody)
		{

		}
		public static string CockatricePlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CockatriceTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string CockatriceRestoreStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RedPandaDesc()
		{

		}
		public static string RedPandaFullDesc(LowerBody lowerBody)
		{

		}
		public static string RedPandaPlayerStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RedPandaTransformStr(LowerBody lowerBody, Player player)
		{

		}
		public static string RedPandaRestoreStr(LowerBody lowerBody, Player player)
		{

		}

	}
}
