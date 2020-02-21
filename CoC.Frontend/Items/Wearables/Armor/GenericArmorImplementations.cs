//using System;
//using System.Collections.Generic;
//using System.Text;
//using CoC.Backend.Creatures;
//using CoC.Backend.Items.Wearables.Armor;
//using CoC.Backend.Tools;
//using CoC.Frontend.Perks.ArmorPerks;

//namespace CoC.Frontend.Items.Wearables.Armor
//{
//	public static class CommonGenericArmor
//	{
//		//Clothing
//		public static Func<ArmorBase> ADVETURERS_CLOTHES => () => new GenericArmorWithBulge(Guid.Parse("77d1bcf0-746c-4e3b-ad74-a7ccf5ae1c0b"), ArmorType.LIGHT_ARMOR, AdveturersClothesAbbr, AdveturersClothesName, AdveturersClothesDesc, AdveturersClothesAbout, AdveturersClothesBulgeChanged, 2, 200);
//		public static Func<ArmorBase> BALLROOM_DRESS => () => new GenericArmor(Guid.Parse("ea91e237-085d-4de9-a578-b1c2eba0c4f5"), ArmorType.MEDIUM_ARMOR, BallroomDressAbbr, BallroomDressName, BallroomDressDesc, BallroomDressAbout, 0, 1200);
//		public static Func<ArmorBase> CLASSY_SUITCLOTHES => () => new GenericArmor(Guid.Parse("ea918703-49a2-4e4d-bc97-ba84e322b7e4"), ArmorType.LIGHT_ARMOR, ClassySuitclothesAbbr, ClassySuitclothesName, ClassySuitclothesDesc, ClassySuitclothesAbout, 1, 400);
//		public static Func<ArmorBase> KIMONO => () => new GenericArmor(Guid.Parse("ff03d684-5af4-4854-8c43-c7b4e024809c"), ArmorType.LIGHT_ARMOR, KimonoAbbr, KimonoName, KimonoDesc, KimonoAbout, 2, 500);
//		public static Func<ArmorBase> SHIRT_LEATHER_PANTS => () => new GenericArmor(Guid.Parse("008e6e4f-9d1a-4ce5-b0cd-d86aa608b40d"), ArmorType.LIGHT_ARMOR, ShirtLeatherPantsAbbr, ShirtLeatherPantsName, ShirtLeatherPantsDesc, ShirtLeatherPantsAbout, 0, 450);
//		public static Func<ArmorBase> MODEST_ROBES => () => new GenericArmor(Guid.Parse("6cfecc45-846a-4870-9ce5-58723d1f215f"), ArmorType.LIGHT_ARMOR, ModestRobesAbbr, ModestRobesName, ModestRobesDesc, ModestRobesAbout, 0, 120);
//		public static Func<ArmorBase> OVERALLS => () => new GenericArmorWithBulge(Guid.Parse("90e97375-a3e4-4a2c-85ea-916c5195114a"), ArmorType.LIGHT_ARMOR, OverallsAbbr, OverallsName, OverallsDesc, OverallsAbout, OverallsBulgeChanged, 0, 60, 0, 0);
//		public static Func<ArmorBase> RUBBER_FETISH_SUIT => () => new GenericArmorWithBulge(Guid.Parse("1e490712-9cd1-4f32-a18a-6738802d0c4c"), ArmorType.LIGHT_ARMOR, RubberFetishSuitAbbr, RubberFetishSuitName, RubberFetishSuitDesc, RubberFetishSuitAbout, RubberFetishSuitBulgeChanged, 3, 1000, 8, 0, false); //bulge
//		public static Func<ArmorBase> SEMI_TRANSPARENT_BODYSUIT => () => new GenericArmor(Guid.Parse("b4272783-729b-406e-a8e6-3f513c4efc71"), ArmorType.LIGHT_ARMOR, SemiTransparentBodysuitAbbr, SemiTransparentBodysuitName, SemiTransparentBodysuitDesc, SemiTransparentBodysuitAbout, 0, 1300, 7);
//		public static Func<ArmorBase> TUBETOP => () => new GenericArmor(Guid.Parse("8252063a-bade-47f2-a7d5-8540d9629005"), ArmorType.LIGHT_ARMOR, TubetopAbbr, TubetopName, TubetopDesc, TubetopAbout, 0, 80);

//		//Armor (spelling? AMERICAn! - FUCK YEAH!)

//		public static Func<ArmorBase> DRAGONSCALE_ARMOR => () => new GenericArmor(Guid.Parse("2a57faa5-aeaa-42a4-89c1-bc20732b2951"), ArmorType.MEDIUM_ARMOR, DragonscaleArmorAbbr, DragonscaleArmorName, DragonscaleArmorDesc, DragonscaleArmorAbout, 18, 900);
//		public static Func<ArmorBase> DRAGONSCALE_ROBES => () => new GenericArmor(Guid.Parse("375b5df0-ae85-4df8-83cd-e8814ddd8c00"), ArmorType.LIGHT_ARMOR, DragonscaleRobesAbbr, DragonscaleRobesName, DragonscaleRobesDesc, DragonscaleRobesAbout, 9, 900, 0, 20);
//		public static Func<ArmorBase> EBONWEAVE_PLATEMAIL => () => new GenericArmor(Guid.Parse("08098f29-0635-40dd-ad5f-34bb8b12a0ca"), ArmorType.HEAVY_ARMOR, EbonweavePlatemailAbbr, EbonweavePlatemailName, EbonweavePlatemailDesc, EbonweavePlatemailAbout, 27, 3000, 0, 15);
//		public static Func<ArmorBase> EBONWEAVE_JACKET => () => new GenericArmor(Guid.Parse("aa2689a0-5527-4862-a848-299e3ea5eb4c"), ArmorType.MEDIUM_ARMOR, EbonweaveJacketAbbr, EbonweaveJacketName, EbonweaveJacketDesc, EbonweaveJacketAbout, 18, 3000, 0, 15);
//		public static Func<ArmorBase> EBONWEAVE_ROBE => () => new GenericArmor(Guid.Parse("0d1d93cd-a63a-4b64-a473-c90ba26177a9"), ArmorType.MEDIUM_ARMOR, EbonweaveRobeAbbr, EbonweaveRobeName, EbonweaveRobeDesc, EbonweaveRobeAbout, 9, 3000, 0, 30);
//		public static Func<ArmorBase> INDECENT_EBONWEAVE_ROBE => () => new GenericArmorWithBulge(Guid.Parse("4f0b2bb7-cfc8-4fed-b5f1-f5954ac8ecab"), ArmorType.LIGHT_ARMOR, IndecentEbonweaveRobeAbbr, IndecentEbonweaveRobeName, IndecentEbonweaveRobeDesc, IndecentEbonweaveRobeAbout, IndecentEbonweaveRobeBulgeChanged, 6, 3000, 5, 30); //bulge
//		public static Func<ArmorBase> FULL_CHAINMAIL => () => new GenericArmorWithBulge(Guid.Parse("42f87653-be8f-4681-95ec-6da1743fb856"), ArmorType.MEDIUM_ARMOR, FullChainmailAbbr, FullChainmailName, FullChainmailDesc, FullChainmailAbout, FullChainmailBulgeChanged, 8, 150);
//		public static Func<ArmorBase> FULL_PLATEMAIL => () => new GenericArmorWithBulge(Guid.Parse("4f19efd6-b294-4c90-a259-45a6fdab331c"), ArmorType.HEAVY_ARMOR, FullPlatemailAbbr, FullPlatemailName, FullPlatemailDesc, FullPlatemailAbout, FullPlatemailBulgeChanged, 21, 250);
//		public static Func<ArmorBase> GEL_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("7993bf72-8688-4cb4-87f6-8a5604502ba5"), ArmorType.HEAVY_ARMOR, GelArmorAbbr, GelArmorName, GelArmorDesc, GelArmorAbout, GelArmorBulgeChanged, 10, 150);
//		public static Func<ArmorBase> INDECENT_STEEL_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("805ba9be-2e1a-4f5f-aafa-979601b1e1f5"), ArmorType.MEDIUM_ARMOR, IndecentSteelArmorAbbr, IndecentSteelArmorName, IndecentSteelArmorDesc, IndecentSteelArmorAbout, IndecentSteelArmorBulgeChanged, 5, 800, 6);
//		public static Func<ArmorBase> LEATHER_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("34993416-ae47-4cd0-bd07-6d52f4f88601"), ArmorType.LIGHT_ARMOR, LeatherArmorAbbr, LeatherArmorName, LeatherArmorDesc, LeatherArmorAbout, LeatherArmorBulgeChanged, 5, 76);
//		public static Func<ArmorBase> LEATHER_ROBES => () => new GenericArmorWithBulge(Guid.Parse("64f301ba-3079-4721-bbb7-f2c170d0644b"), ArmorType.LIGHT_ARMOR, LeatherRobesAbbr, LeatherRobesName, LeatherRobesDesc, LeatherRobesAbout, LeatherRobesBulgeChanged, 6, 100);
//		public static Func<ArmorBase> SAMURAI_ARMOR => () => new GenericArmor(Guid.Parse("b87290a5-a5b1-432d-a7a7-bb9bdafa8fc0"), ArmorType.HEAVY_ARMOR, SamuraiArmorAbbr, SamuraiArmorName, SamuraiArmorDesc, SamuraiArmorAbout, 18, 300);
//		public static Func<ArmorBase> SCALE_MAIL => () => new GenericArmorWithBulge(Guid.Parse("249cdb51-96cc-4058-929d-2abcad41f1ed"), ArmorType.HEAVY_ARMOR, ScaleMailAbbr, ScaleMailName, ScaleMailDesc, ScaleMailAbout, ScaleMailBulgeChanged, 12, 170);
//		public static Func<ArmorBase> SPIDER_SILK_ROBES => () => new GenericArmor(Guid.Parse("773e6b61-379f-4188-8396-777b6d776925"), ArmorType.LIGHT_ARMOR, SpiderSilkRobesAbbr, SpiderSilkRobesName, SpiderSilkRobesDesc, SpiderSilkRobesAbout, 6, 950, 0, 30);
//		public static Func<ArmorBase> SPIDER_SILK_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("3b068ecc-2cea-479f-a84f-fd2162eb4fad"), ArmorType.HEAVY_ARMOR, SpiderSilkArmorAbbr, SpiderSilkArmorName, SpiderSilkArmorDesc, SpiderSilkArmorAbout, SpiderSilkArmorBulgeChanged, 25, 950);
//		public static Func<ArmorBase> WIZARDS_ROBES => () => new GenericArmor(Guid.Parse("8820325c-08e6-4f36-9d3f-fac3e68d5d90"), ArmorType.LIGHT_ARMOR, WizardsRobesAbbr, WizardsRobesName, WizardsRobesDesc, WizardsRobesAbout, 1, 50, 0, 25);

//		//ADVETURERS_CLOTHES
//		private static string AdveturersClothesAbbr() => "G. Clothes";
//		private static string AdveturersClothesName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "green adventurer's clothes";
//		}

//		private static string AdveturersClothesDesc(bool bulged, byte count, bool displayCount)
//		{
//			string outfitText = count != 1 ? "outfits" : "outfit";

//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
//			string crotchText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{crotchText}green adventurer's {outfitText}, complete with pointed cap";
//		}

//		private static string AdveturersClothesAbout(bool bulged)
//		{
//			return "A set of comfortable green adventurer's clothes. It even comes complete with a pointy hat!"
//				+ (bulged ? " It's been modified so that even the slightest bulge of a manly package would be on prominent display" : "");
//		}

//		private static string AdveturersClothesBulgeChanged(Creature wearer, bool bulged)
//		{
//			if (bulged)
//			{
//				return "The layer beneath your tunic begins to compress against your " + wearer.genitals.AllCocksLongDescription() + ", highlighting every curve " +
//					"and nodule while lifting your package to be clearly visible beneath your outer layers.";
//			}
//			else
//			{
//				return "The layers beneath your tunic shift, reverting toward a more normal fit and granting you a bit of modesty in the process.";
//			}
//		}

//		//BALLROOM_DRESS
//		private static string BallroomDressAbbr() => "LongDress";
//		private static string BallroomDressName() => "long ballroom dress";

//		private static string BallroomDressDesc(byte count, bool displayCount)
//		{
//			string dressText = count != 1 ? "dresses" : "dress";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}sequin-patterned ballroom {dressText}";
//		}

//		private static string BallroomDressAbout() => "A long ballroom dress patterned with sequins. Perfect for important occasions.";

//		//BONDAGE_STRAPS

//


//		//"Your fetishy bondage outfit allows you access to an improved form of 'Tease'."

//		//CLASSY_SUITCLOTHES

//		private static string ClassySuitclothesAbbr() => "Suitclothes";
//		private static string ClassySuitclothesName() => "classy suitclothes";

//		private static string ClassySuitclothesDesc(byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{setText} of classy suit-clothes";
//		}

//		private static string ClassySuitclothesAbout() => "A set of classy suitclothes.";

//		//KIMONO

//		private static string KimonoAbbr() => "Kimono ";
//		private static string KimonoName() => "kimono";

//		private static string KimonoDesc(byte count, bool displayCount)
//		{
//			string kimonoText = count != 1 ? "kimonos" : "kimono";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}traditional {kimonoText}";
//		}

//		private static string KimonoAbout() => "This is a type of robes also known as kimono traditionally worn by the people of the far East. It's pretty elegant.";

//		//SHIRT_LEATHER_PANTS

//		private static string ShirtLeatherPantsAbbr() => "T.Lthr Pants";
//		private static string ShirtLeatherPantsName() => "white silk shirt and tight leather pants";

//		private static string ShirtLeatherPantsDesc(byte count, bool displayCount)
//		{
//			string pairText = count != 1 ? "pairs" : "pair";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{pairText} of leather pants and a white silk shirt";
//		}

//		private static string ShirtLeatherPantsAbout() => "A flowing silk shirt and tight black leather pants. Suave!";

//		//MODEST_ROBES

//		private static string ModestRobesAbbr() => "Robes";
//		private static string ModestRobesName() => "modest robes";

//		private static string ModestRobesDesc(byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{setText} of modest robes";
//		}

//		private static string ModestRobesAbout() => "A set of modest robes, not dissimilar from what the monks back home would wear.";

//		//NURSE_OUTFIT

//

//		//"Your fetishy nurse outfit allows you access to an improved form of 'Tease'.");
//		//OVERALLS

//		private static string OverallsAbbr() => "Overalls";
//		private static string OverallsName(bool bulged)
//		{
//			return "white shirt and " + (bulged ? "crotch-hugging " : "") + "overalls";
//		}

//		private static string OverallsDesc(bool bulged, byte count, bool displayCount)
//		{
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			if (count == 1)
//			{
//				string countText = displayCount ? "a " : "";
//				return $"{countText} white shirt and {bulgeText}overalls";
//			}
//			else
//			{
//				string countText = displayCount ? Utils.NumberAsText(count) + " " : "";
//				return $"{countText}sets of white shirts and {bulgeText}overalls";
//			}
//		}
//		private static string OverallsAbout(bool bulged)
//		{
//			return "A simple white shirt and overalls." + (bulged ? " They've been magically altered to prominently display the wearer's dick-bulge" : "");
//		}

//		private static string OverallsBulgeChanged(Creature wearer, bool bulged)
//		{
//			if (bulged)
//			{
//				return "The denim of your overalls begins to press tightly against your " + wearer.genitals.AllCocksLongDescription(out bool isPlural) +
//					", molding itself around your " + (isPlural ? "member and its every facet." : "members and their respective facets");
//			}
//			else
//			{
//				return "The denim of your overalls changes, reverting to a more relaxed fit, and no longer display your manly bulge quite so prominently";
//			}
//		}



//		//RUBBER_FETISH_SUIT

//		private static string RubberFetishSuitAbbr() => "Rbbr Fetish";
//		private static string RubberFetishSuitName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "rubber fetish clothes";
//		}

//		private static string RubberFetishSuitDesc(bool bulged, byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging" : "revealing";

//			return $"{countText}{setText} of {bulgeText} rubber fetish clothes";
//		}

//		private static string RubberFetishSuitAbout(bool bulged)
//		{
//			return "A revealing set of fetish-wear. Upgrades your tease attack with the \"Slutty Seduction\" perk." +
//				(bulged ? " It has also been magically altered to more prominently display your manly bulge." : "");
//		}

//		private static string RubberFetishSuitBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "You begin to feel your rubber outfit compressing itself against your upper " + wearer.lowerBody.LongDescription() + " and "
//					+ wearer.genitals.AllCocksShortDescription() + ", eliminating any pockets of air or wrinkles that may have existed before.";
//			}
//			else
//			{
//				string cockText = wearer.hasCock ? "but beyond relieving the constant pressure on your " + wearer.genitals.AllCocksShortDescription() + ", " : "though ";
//				return "The alterations to your rubber fetish-suit that made it further tighten against your manly bulge relax, " + cockText +
//					" it doesn't really make a huge difference.";
//			}
//		}

//		//"Your fetishy rubberwear allows you access to 'Seduce', an improved form of 'Tease'."
//		//SEMI_TRANSPARENT_BODYSUIT

//		private static string SemiTransparentBodysuitAbbr() => "Bodysuit";
//		private static string SemiTransparentBodysuitName() => "semi-transparent bodysuit";

//		private static string SemiTransparentBodysuitDesc(byte count, bool displayCount)
//		{
//			string bodysuitText = count != 1 ? "bsodysuit" : "bodysuit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}semi-transparent, curve-hugging {bodysuitText}";
//		}

//		private static string SemiTransparentBodysuitAbout() => "A semi-transparent bodysuit. It looks like it will cling to all the curves of your body.";

//		//"Your clingy transparent bodysuit allows you access to 'Seduce', an improved form of 'Tease'.");
//		//TUBETOP

//		private static string TubetopAbbr() => "Tube Top";
//		private static string TubetopName() => "tube top and short shorts";

//		private static string TubetopDesc(byte count, bool displayCount)
//		{
//			string topText = count != 1 ? "tops" : "top";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}snug tube {topText} with VERY short shorts";
//		}

//		private static string TubetopAbout() => "A clingy tube top and VERY short shorts.";

//		//Armour

//		//CHAINMAIL_BIKINI;
//


//		//"Your revealing chain bikini allows you access to 'Seduce', an improved form of 'Tease'."
//		//DRAGONSCALE_ARMOR

//		private static string DragonscaleArmorAbbr() => "D.Scale Armor";
//		private static string DragonscaleArmorName() => "dragonscale armor";

//		private static string DragonscaleArmorDesc(byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{suitText} of dragonscale armor";
//		}

//		private static string DragonscaleArmorAbout() => "This armor is cleverly fashioned from dragon scales. It offers high protection while at the same time, quite flexible.";

//		//DRAGONSCALE_ROBES

//		private static string DragonscaleRobesAbbr() => "D.Scale Robes";
//		private static string DragonscaleRobesName() => "dragonscale robes";

//		private static string DragonscaleRobesDesc(byte count, bool displayCount)
//		{
//			if (count != 1)
//			{
//				string countText = displayCount ? Utils.NumberAsText(count) + " " : "";
//				return $"{countText}sets of dragonscale robes";
//			}
//			else
//			{
//				return "dragonscale robes";

//			}
//		}

//		private static string DragonscaleRobesAbout() => "This robe is expertly made from dragon scales. It offers high protection while being lightweight and should be comfortable to wear all day.";

//		//EBONWEAVE_PLATEMAIL

//		private static string EbonweavePlatemailAbbr() => "EW Plate";
//		private static string EbonweavePlatemailName() => "Ebonweave Platemail";

//		private static string EbonweavePlatemailDesc(byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{setText} of Ebonweave Platemail";
//		}

//		private static string EbonweavePlatemailAbout() => "The armor is made of ebonweave, created using refined Ebonbloom petals. " +
//			"The armor consists of two layers: an outer of ebonweave plating and an inner of arrow-proof ebonweave cloth.";

//		//EBONWEAVE_JACKET

//		private static string EbonweaveJacketAbbr() => "EW Jacket";
//		private static string EbonweaveJacketName() => "Ebonweave Jacket";

//		private static string EbonweaveJacketDesc(byte count, bool displayCount)
//		{
//			string jacketText = count != 1 ? "jackets" : "jacket";
//			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}Ebonweave {jacketText}";
//		}

//		private static string EbonweaveJacketAbout() => "This outfit is made of ebonweave, created using refined Ebonbloom petals. The outfit consists of a leather-like jacket, a mesh breastplate, and a set of arrow-proof clothing.";

//		//EBONWEAVE_ROBE

//		private static string EbonweaveRobeAbbr() => "EW Robe";
//		private static string EbonweaveRobeName() => "Ebonweave Robe";

//		private static string EbonweaveRobeDesc(byte count, bool displayCount)
//		{
//			string robeText = count != 1 ? "robes" : "robe";
//			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}Ebonweave {robeText}";
//		}

//		private static string EbonweaveRobeAbout() => "This robe is made of ebonweave, a material created using refined Ebonbloom petals. This robe is comfortable, yet more protective than chainmail. It has a mystical aura to it.";

//		//INDECENT_EBONWEAVE_ROBE

//		private static string IndecentEbonweaveRobeAbbr() => "EW I.Robe";
//		private static string IndecentEbonweaveRobeName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "indecent Ebonweave Robe";
//		}

//		private static string IndecentEbonweaveRobeDesc(bool bulged, byte count, bool displayCount)
//		{
//			string robeText = count != 1 ? "robes" : "robe";
//			string countText = displayCount ? (count == 1 ? "an " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? ", crotch-hugging" : "";

//			return $"{countText}indecent{bulgeText} Ebonweave {robeText}";
//		}

//		private static string IndecentEbonweaveRobeAbout(bool bulged)
//		{
//			return "More of a longcoat than a robe, this outfit is crafted from refined Ebonbloom petals. " +
//				"Discrete straps centered around the belt keep the front open." +
//				(bulged ? " The magical enhancements on it remove any semblence of modesty it might have had; any manly endowments would almost certainly " +
//				"catch the eye of any passerby" : "");
//		}

//		private static string IndecentEbonweaveRobeBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "Your revealing robes shift, compressing until they almost wrap around your " + wearer.genitals.AllCocksLongDescription() +
//					". It certainly aides to your already indecent appearance, and makes it appear like you're constantly sporting an erection.";
//			}
//			else
//			{
//				var groinText = wearer.hasCock ? wearer.genitals.AllCocksShortDescription() : "groin";
//				return "Your revealing robes shift, relaxing their noticable grip on your " + groinText + ". They still will undoubted draw the eyes of everyone you face, " +
//					"but now the focus will be on the rest of your body instead.";
//			}
//		}


//		//"Your revealing robes allow you access to 'Seduce', an improved form of 'Tease'."

//		//FULL_CHAINMAIL

//		private static string FullChainmailAbbr() => "Full Chain";
//		private static string FullChainmailName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "full-body chainmail";
//		}

//		private static string FullChainmailDesc(bool bulged, byte count, bool displayCount)
//		{
//			string fullSuitText = count != 1 ? "full suits" : "full suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{fullSuitText} of {bulgeText}chainmail armor";
//		}

//		private static string FullChainmailAbout(bool bulged)
//		{
//			return "This full suit of chainmail armor covers its wearer from head to toe in protective steel rings." +
//				(bulged ? " Strangely, the lower half has taken a form that would certainly highlight any dicks its user has lurking beneath." : "");
//		}

//		private static string FullChainmailBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "You're taken by surprise as the binds of your chainmail begin to flatten and rearrange themselves, doing their best to match the curves of your "
//				+ wearer.genitals.AllCocksShortDescription(out bool isPlural) + " and make " + (isPlural ? "their" : "its") + " presence known.";
//			}
//			else
//			{
//				string cockText = wearer.hasCock ? wearer.genitals.AllCocksShortDescription() : "nonexistent cocks";

//				return "The flattened links of chainmail around your groin rearrange themselves, regaining their former shape, until they no longer promote your " + cockText + ".";
//			}
//		}


//		//FULL_PLATEMAIL

//		private static string FullPlatemailAbbr() => "Full Plate";
//		private static string FullPlatemailName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "full platemail";
//		}

//		private static string FullPlatemailDesc(bool bulged, byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{suitText} of {bulgeText}full-plate armor";
//		}

//		private static string FullPlatemailAbout(bool bulged)
//		{
//			return "A highly protective suit of steel platemail. It would be hard to find better physical protection than this." +
//				(bulged ? " It appears to be magically altered to fully highlight any manly bulges, but thankfully without losing any protection." : "");
//		}

//		private static string FullPlatemailBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "You begin to clench your fists as your steel platemail heats up around your " + wearer.lowerBody.LongDescription() + " and crotch. " +
//					"Slowly it begins to press itself against your " + wearer.genitals.AllCocksShortDescription(out bool isPlural) + " and match " +
//					(isPlural ? "their respective features" : "its every feature.");
//			}
//			else
//			{
//				return "You wince as the solid platemail heats up once more, leveling out around your groin until it's even once more";
//			}

//		}


//		//GEL_ARMOR

//		private static string GelArmorAbbr() => "GelArmr";
//		private static string GelArmorName(bool bulged)
//		{
//			return "glistening" + (bulged ? ", crotch-hugging" : "") + " gel-armor plates";
//		}

//		private static string GelArmorDesc(bool bulged, byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{suitText} of {bulgeText}gel armor";
//		}

//		private static string GelArmorAbout(bool bulged)
//		{
//			return "This suit of interlocking plates is made from a strange green material. The material appears flexible enough to mold perfectly around its user, " +
//				"but its resiliancy cannot be overlooked, either." + (bulged ? " Extra material around the groin suggests this one has been magically altered to show off " +
//				"any dicks the user has lurking beneath" : "");
//		}

//		private static string GelArmorBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "The green gel-plate protecting your groin thins and presses tightly against you, molding around your " +
//					wearer.genitals.AllCocksShortDescription() + " in an incredibly lewd way.";
//			}
//			else
//			{
//				string junkText = wearer.hasCock ? "junk" : "groin";
//				string pronoun = wearer.hasCock && wearer.cocks.Count > 1 ? "them" : "it";
//				return "The gel plating remains molded to your form, but the gel plating displaying your " + junkText + "shifts to a more natural shape, obscuring " + pronoun + ".";
//			}
//		}


//		//INDECENT_STEEL_ARMOR

//		private static string IndecentSteelArmorAbbr() => "Indec StAr";
//		private static string IndecentSteelArmorName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging, " : "") + "practically indecent steel armor";
//		}

//		private static string IndecentSteelArmorDesc(bool bulged, byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging, " : "";

//			return $"{countText}{suitText} of {bulgeText}practically indecent steel armor";
//		}

//		private static string IndecentSteelArmorAbout(bool bulged)
//		{
//			return "This suit of steel 'armor' has two round disks that barely cover the nipples, a tight chainmail bikini, and circular butt-plates." +
//				(bulged ? " Further alterations to this already skimpy outfit draw the eye towards any bulges lurking beneath the tight bikini, " +
//				"trading what little defense it did provide for pure sex appeal." : "");
//		}

//		private static string IndecentSteelArmorBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "The chainmail bikini of your indecent steel armor rearranges and bends its interlocking rings to best shape itself around your "
//					+ wearer.genitals.AllCocksShortDescription(out bool isPlural) + ", leaving very little else to the imagination.";
//			}
//			else
//			{
//				string groinText = wearer.hasCock ? "man-canceling" : (wearer.hasVagina ? "female-friendly" : "gender-agnostic");
//				return "The interlocking rings that adjusted to highlight your manly bulge revert back to their normal, " + groinText + " form.";
//			}
//		}


//		//"Your incredibly revealing steel armor allows you access to 'Seduce', an improved form of 'Tease'.");
//		//LEATHER_ARMOR

//		private static string LeatherArmorAbbr() => "LeathrA";
//		private static string LeatherArmorName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "leather armor segments";
//		}

//		private static string LeatherArmorDesc(bool bulged, byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{setText} of {bulgeText}leather armor";
//		}

//		private static string LeatherArmorAbout(bool bulged)
//		{
//			return "This is a suit of well-made leather armor. It looks fairly rugged. It's been \"adjusted\" to fit tightly around the groin, " +
//				"and would undoubtedly make a hard-on stand out";
//		}

//		private static string LeatherArmorBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "Your leather armor shifts, pressing tightly against your upper " + wearer.lowerBody.LongDescription() + " and molding itself around your "
//					+ wearer.genitals.AllCocksShortDescription(out bool isPlural) + " to prominently display it.";
//			}
//			else
//			{
//				return "The leather of your armor "
//			}

//		}


//		//LEATHER_ROBES

//		private static string LeatherRobesAbbr() => "Lthr Robes";
//		private static string LeatherRobesName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "black leather armor surrounded by voluminous robes";
//		}

//		private static string LeatherRobesDesc(bool bulged, byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? ", crotch-hugging" : "";

//			return $"{countText}{suitText} of black{bulgeText} leather armor with voluminous robes";
//		}

//		private static string LeatherRobesAbout(bool bulged)
//		{
//			return "This is a suit of flexible leather armor with a voluminous set of concealing black robes." +
//				(bulged ? " Well, mostly concealing, anyway - the leather around the crotch almost seems tailor-made to show off any dick-bulge," +
//					"and the robes just so happen to leave it visible." : "");
//		}

//		private static string LeatherRobesBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "Your leather armor shifts, pressing tightly against your upper " + wearer.lowerBody.LongDescription() + " and molding itself around your "
//					+ wearer.genitals.AllCocksShortDescription(out bool isPlural) + " to prominently display " + (isPlural ? "them" : "it") + " through your robes.";
//			}

//		}


//		//SAMURAI_ARMOR

//		private static string SamuraiArmorAbbr() => "Samu.Armor";
//		private static string SamuraiArmorName() => "samurai armor";

//		private static string SamuraiArmorDesc(byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";

//			return $"{countText}{suitText} of samurai armor";
//		}

//		private static string SamuraiArmorAbout() => "This suit of armor is originally worn by the Samurai, the warriors from the far East.";

//		//SCALE_MAIL

//		private static string ScaleMailAbbr() => "Scale Mail";
//		private static string ScaleMailName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "scale-mail armor";
//		}

//		private static string ScaleMailDesc(bool bulged, byte count, bool displayCount)
//		{
//			string setText = count != 1 ? "sets" : "set";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{setText} of {bulgeText}scale-mail armor";
//		}

//		private static string ScaleMailAbout(bool bulged)
//		{
//			return "This suit of scale-mail covers the entire body with layered steel scales, providing flexibility and protection." +
//				(bulged ? "While you'd expect this type of armor to be very concealing, this one appears to have been altered to higlight them instead, " +
//				"with a highly impractical manly bulge situated front and center" : "");
//		}

//		private static string ScaleMailBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "The steel scales that make up your armor begin to flap wildly around your crotch. They bend and shift as they attempt to " +
//					"match the profile of your " + wearer.genitals.AllCocksShortDescription() + ".";
//			}

//		}


//		//SPIDER_SILK_ROBES

//		private static string SpiderSilkRobesAbbr() => "SS.Robes";
//		private static string SpiderSilkRobesName() => "spider-silk robes";

//		private static string SpiderSilkRobesDesc(byte count, bool displayCount)
//		{
//			if (count == 1)
//			{
//				string countText = displayCount ? "a " : "";
//				return $"{countText}robe made of spider silk";
//			}
//			else
//			{
//				string countText = displayCount ? Utils.NumberAsText(count) + " " : "";
//				return $"{countText}sets of spider-silk robes";
//			}
//		}

//		private static string SpiderSilkRobesAbout() => "This robe looks incredibly comfortable. It's made from alchemically enhanced spider-silk, and embroidered with what looks like magical glyphs around the sleeves and hood.";

//		//SPIDER_SILK_ARMOR

//		private static string SpiderSilkArmorAbbr() => "SS.Armor";
//		private static string SpiderSilkArmorName(bool bulged)
//		{
//			return (bulged ? "crotch-hugging " : "") + "spider-silk armor";
//		}

//		private static string SpiderSilkArmorDesc(bool bulged, byte count, bool displayCount)
//		{
//			string suitText = count != 1 ? "suits" : "suit";
//			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count) + " ") : "";
//			string bulgeText = bulged ? "crotch-hugging " : "";

//			return $"{countText}{suitText} of {bulgeText}spider-silk armor";
//		}

//		private static string SpiderSilkArmorAbout(bool bulged)
//		{
//			return "This armor is as white as the driven snow. It's crafted out of thousands of strands of spider-silk into an impenetrable protective suit. " +
//				"The surface is slightly spongy, but so tough you wager most blows would bounce right off." +
//				(bulged ? " That's not to say wearing this would be without consequence - this one has undoubtedly been altered, the parts woven around the groin look " +
//				"more like a heavily-armored cock-sleeve than anything else. " : "");
//		}

//		private static string SpiderSilkArmorBulgeChanged(Creature wearer, bool nowBulged)
//		{
//			if (nowBulged)
//			{
//				return "The fine silk that makes up your armor suddenly undoes itself around your crotch, exposing your " +
//					wearer.genitals.AllCocksShortDescription(out bool isPlural) + " to the open air. The thin strands in the air begin " +
//					"to re-weave themselves around your enormous " + (isPlural ? "members" : "member") + ", forming a prominent new addition to your protection.";
//			}

//		}


//		//WIZARDS_ROBES

//		private static string WizardsRobesAbbr() => "W.Robes";
//		private static string WizardsRobesName() => "wizard's robes";

//		private static string WizardsRobesDesc(byte count, bool displayCount)
//		{
//			if (count == 1)
//			{
//				string countText = displayCount ? "a " : "";
//				return $"{countText}wizard's robes";
//			}
//			else
//			{
//				string countText = displayCount ? Utils.NumberAsText(count) + " " : "";
//				return $"{countText}sets of wizard's robes";
//			}
//		}

//		private static string WizardsRobesAbout() => "These robes appear to have once belonged to a female wizard. They're long with a slit up the side and full billowing sleeves. The top is surprisingly low cut. Somehow you know wearing it would aid your spellcasting.";
//	}
//}


///*
// */
