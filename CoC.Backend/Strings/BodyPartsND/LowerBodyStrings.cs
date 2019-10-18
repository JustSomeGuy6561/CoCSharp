//LowerBodyStrings.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 8:21 PM

using CoC.Backend.Creatures;

namespace CoC.Backend.BodyParts
{
	public partial class LowerBody
{
public static string Name()
{
return "LowerBody";
}
}

public partial class LowerBodyType
	{
		//private static string HumanPlayer(Player player)
		//{
		//	return "[legCountTextUC] normal human legs grow down from your waist, ending in normal human feet.";
		//}
		//private static string FerretPlayer(Player player)
		//{
		//	return "Your [legCountText] legs are equally covered in [hairOrFurColor] fur, the lower half having a darker shade."
		//			 + " They end on digitigrade ferret paws with short claws.";
		//}
		//private static string HoofedPlayer(Player player)
		//{
		//	return "Your [legCountText] legs are muscled and jointed oddly, covered in fur, and end in a bestial hooves.";
		//}
		//private static string WolfPlayer(Player player)
		//{
		//	return "You have [legCountText] digitigrade legs that end in wolf paws.";
		//}
		//private static string DogPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs grow downwards from your waist, ending in dog-like hind-paws.";
		//}
		//private static string NagaPlayer()
		//{
		//	if (player.hasReptileUnderBody(true))
		//	{
		//		if (player.underBody.type == UnderBody.NAGA)
		//			nagaColors = [player.underBody.skin.tone, player.nagaLowerBodyColor2()];
		//		else
		//			nagaColors = [player.skin.tone, player.underBody.skin.tone];
		//		return "Below your waist, in place of where your legs would be, your body transitions into a long snake like tail."
		//				  + " Your snake-like lower body is covered by " + nagaColors[0] + " color scales,"
		//				  + " with " + nagaColors[1] + " color ventral scales along your underside.";
		//	}
		//	else
		//		return "Below your waist your flesh is fused together into a very long snake-like tail.";
		//}
		//private static string DemonicPlayer()
		//{
		//	if (true)
		//	{
		//	}
		//	else
		//	{
		//		return "Your [legCountText] lithe legs are capped with flexible clawed feet.  Sharp black nails grow where once you had toe-nails, giving you fantastic grip.";
		//	}
		//}
		//private static string BeePlayer()
		//{
		//	return "Your [legCountText] legs are covered in a shimmering insectile carapace up to mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.  A bit of downy yellow and black fur fuzzes your upper thighs, just like a bee.";
		//}
		//private static string GooPlayer()
		//{
		//	return "In place of legs you have a shifting amorphous blob.  Thankfully it's quite easy to propel yourself around on.  The lowest portions of your " + player.armorName + " float around inside you, bringing you no discomfort.";
		//}
		//private static string CatPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs grow downwards from your waist, ending in soft, padded cat-paws.";
		//}
		//private static string LizardPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		//}
		//private static string SalamanderPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs covered in thick, leathery red scales up to the mid-thigh grow down from your [hips], ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		//}
		//private static string BunnyPlayer()
		//{
		//	return "Your [legCountText] legs thicken below the waist as they turn into soft-furred rabbit-like legs.  You even have large bunny feet that make hopping around a little easier than walking.";
		//}
		//private static string HarpyPlayer()
		//{
		//	return "Your [legCountText] legs are covered with [furColor] plumage.  Thankfully the thick, powerful thighs are perfect for launching you into the air, and your feet remain mostly human, even if they are two-toed and tipped with talons.";
		//}
		//private static string KangarooPlayer()
		//{
		//	return "Your [legCountText] furry legs have short thighs and long calves, with even longer feet ending in prominently-nailed toes.";
		//}
		//private static string ChitinousSpiderLegsPlayer()
		//{
		//	return "Your [legCountText] legs are covered in a reflective black, insectile carapace up to your mid-thigh, looking more like a set of 'fuck-me-boots' than exoskeleton.";
		//}
		//private static string FoxPlayer()
		//{
		//	return "Your [legCountText] legs are crooked into high knees with hocks and long feet, like those of a fox; cute bulbous toes decorate the ends.";
		//}
		//private static string DragonPlayer()
		//{
		//	return "[legCountTextUC] human-like legs grow down from your [hips], sheathed in scales and ending in clawed feet.  There are three long toes on the front, and a small hind-claw on the back.";
		//}
		//private static string RaccoonPlayer()
		//{
		//	return "Your [legCountText] legs, though covered in fur, are humanlike.  Long feet on the ends bear equally long toes, and the pads on the bottoms are quite sensitive to the touch.";
		//}
		//private static string ClovenHoofedPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs form below your [hips], ending in cloven hooves.";
		//}
		//private static string ImpPlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs form below your [hips], ending in clawed feet. Three extend out the front, and one smaller one is in the back to keep your balance.";
		//}
		//private static string CockatricePlayer()
		//{
		//	return "[legCountTextUC] digitigrade legs grow down from your [hips], ending in clawed feet."
		//			 + " There are three long toes on the front, and a small hind-claw on the back."
		//			 + " A layer of " + (player.hasCockatriceSkin() ? player.skin.furColor : player.hair.color) + " feathers covers your legs from the"
		//			 + " hip to the knee, ending in a puffy cuff.";
		//}
		//private static string RedPandaPlayer()
		//{
		//	return "Your [legCountText] legs are equally covered in [if (hasFurryUnderBody)[underBody.furColor]|black-brown] fur,"
		//			 + " ending on red-panda paws with short claws. They have a nimble and strong build,"
		//			 + " in case you need to escape from something.";
		//}

		//private static string GenericButtHipsPlayerDesc(LowerBody lowerBody, Player player)
		//{

		//}


		private static string HumanDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DemonHiHeelsDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsPlayerStr(LowerBody lowerBody, Player player)
		{
			return "Your " + lowerBody.legCount.ToString() + " perfect lissome legs end in mostly human feet, apart from the " +
				"horn protruding straight down from the heel that forces you to walk with a sexy, swaying gait.";

		}
		private static string DemonHiHeelsTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeePlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaFullDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaPlayerStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaTransformStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(LowerBody lowerBody, Player player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

	}
}
