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
		private static string HumanLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HoovedRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DemonHiHeelsDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			return "Your " + lowerBody.legCount.ToString() + " perfect lissome legs end in mostly human feet, apart from the " +
				"horn protruding straight down from the heel that forces you to walk with a sexy, swaying gait.";

		}
		private static string DemonHiHeelsTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonHiHeelsRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonClawRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeePlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeeRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PonyRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DriderRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ClovenHoofRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaLongDesc(LowerBody lowerBody)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaPlayerStr(LowerBody lowerBody, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaTransformStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(LowerBodyData previousLowerBodyData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

	}
}
