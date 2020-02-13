//using System;
//using System.Collections.Generic;
//using System.Text;
//using CoC.Backend.Creatures;
//using CoC.Backend.Items.Wearables.Armor;
//using CoC.Frontend.Perks.ArmorPerks;

//namespace CoC.Frontend.Items.Wearables.Armor
//{
//	public static class CommonGenericArmor
//	{
//		//Clothing
//		public static Func<ArmorBase> ADVETURERS_CLOTHES => () => new GenericArmorWithBulge(Guid.Parse("77d1bcf0-746c-4e3b-ad74-a7ccf5ae1c0b"), ArmorType.LIGHT_ARMOR, AdveturersClothesAbbr, AdveturersClothesName, AdveturersClothesDesc, AdveturersClothesAbout, AdveturersClothesBulgeChanged, 2, 200);
//		public static Func<ArmorBase> BALLROOM_DRESS => () => new GenericArmor(Guid.Parse("ea91e237-085d-4de9-a578-b1c2eba0c4f5"), ArmorType.MEDIUM_ARMOR, BallroomDressAbbr, BallroomDressName, BallroomDressDesc, BallroomDressAbout, 0, 1200);
//		public static Func<ArmorBase> BONDAGE_STRAPS => () => new GenericArmor(Guid.Parse("d6ebc038-2f97-4a0f-a0de-965261c1efa9"), ArmorType.LIGHT_ARMOR, BondageStrapsAbbr, BondageStrapsName, BondageStrapsDesc, BondageStrapsAbout, 0, 600, 10, 0, false);
//		public static Func<ArmorBase> CLASSY_SUITCLOTHES => () => new GenericArmor(Guid.Parse("ea918703-49a2-4e4d-bc97-ba84e322b7e4"), ArmorType.LIGHT_ARMOR, ClassySuitclothesAbbr, ClassySuitclothesName, ClassySuitclothesDesc, ClassySuitclothesAbout, 1, 400);
//		public static Func<ArmorBase> KIMONO => () => new GenericArmor(Guid.Parse("ff03d684-5af4-4854-8c43-c7b4e024809c"), ArmorType.LIGHT_ARMOR, KimonoAbbr, KimonoName, KimonoDesc, KimonoAbout, 2, 500);
//		public static Func<ArmorBase> SHIRT_LEATHER_PANTS => () => new GenericArmor(Guid.Parse("008e6e4f-9d1a-4ce5-b0cd-d86aa608b40d"), ArmorType.LIGHT_ARMOR, ShirtLeatherPantsAbbr, ShirtLeatherPantsName, ShirtLeatherPantsDesc, ShirtLeatherPantsAbout, 0, 450);
//		public static Func<ArmorBase> MODEST_ROBES => () => new GenericArmor(Guid.Parse("6cfecc45-846a-4870-9ce5-58723d1f215f"), ArmorType.LIGHT_ARMOR, ModestRobesAbbr, ModestRobesName, ModestRobesDesc, ModestRobesAbout, 0, 120);
//		public static Func<ArmorBase> NURSE_OUTFIT => () => new GenericArmor(Guid.Parse("1ca389c9-5071-41e9-b411-38db5a8368aa"), ArmorType.LIGHT_ARMOR, NurseOutfitAbbr, NurseOutfitName, NurseOutfitDesc, NurseOutfitAbout, 0, 800, 8);
//		public static Func<ArmorBase> OVERALLS => () => new GenericArmorWithBulge(Guid.Parse("90e97375-a3e4-4a2c-85ea-916c5195114a"), ArmorType.LIGHT_ARMOR, OverallsAbbr, OverallsName, OverallsDesc, OverallsAbout, OverallsBulgeChanged, 0, 60, 0, 0);
//		public static Func<ArmorBase> RIGID_BODYSUIT => () => new GenericArmorWithBulge(Guid.Parse("b6583e5f-3ecb-4016-9d1c-92c49f35bd55"), ArmorType.LIGHT_ARMOR, RigidBodysuitAbbr, RigidBodysuitName, RigidBodysuitDesc, RigidBodysuitAbout, RigidBodysuitBulgeChanged, 1, 1200, 0, 1, false); //bulge
//		public static Func<ArmorBase> RUBBER_FETISH_SUIT => () => new GenericArmorWithBulge(Guid.Parse("1e490712-9cd1-4f32-a18a-6738802d0c4c"), ArmorType.LIGHT_ARMOR, RubberFetishSuitAbbr, RubberFetishSuitName, RubberFetishSuitDesc, RubberFetishSuitAbout, RubberFetishSuitBulgeChanged, 3, 1000, 8, 0, false); //bulge
//		public static Func<ArmorBase> SEMI_TRANSPARENT_BODYSUIT => () => new GenericArmor(Guid.Parse("b4272783-729b-406e-a8e6-3f513c4efc71"), ArmorType.LIGHT_ARMOR, SemiTransparentBodysuitAbbr, SemiTransparentBodysuitName, SemiTransparentBodysuitDesc, SemiTransparentBodysuitAbout, 0, 1300, 7);
//		public static Func<ArmorBase> TUBETOP => () => new GenericArmor(Guid.Parse("8252063a-bade-47f2-a7d5-8540d9629005"), ArmorType.LIGHT_ARMOR, TubetopAbbr, TubetopName, TubetopDesc, TubetopAbout, 0, 80);

//		//Armor (spelling? AMERICAn! - FUCK YEAH!)

//		public static Func<ArmorBase> CHAINMAIL_BIKINI => () => new GenericArmorWithBulge(Guid.Parse("536f1041-74f6-485e-853d-d49363bbd325"), ArmorType.LIGHT_ARMOR, ChainmailBikiniAbbr, ChainmailBikiniName, ChainmailBikiniDesc, ChainmailBikiniAbout, ChainmailBikiniBulgeChanged, 2, 700, 5, 0, false);
//		public static Func<ArmorBase> DRAGONSCALE_ARMOR => () => new GenericArmor(Guid.Parse("2a57faa5-aeaa-42a4-89c1-bc20732b2951"), ArmorType.MEDIUM_ARMOR, DragonscaleArmorAbbr, DragonscaleArmorName, DragonscaleArmorDesc, DragonscaleArmorAbout, 18, 900);
//		public static Func<ArmorBase> DRAGONSCALE_ROBES => () => new GenericArmor(Guid.Parse("375b5df0-ae85-4df8-83cd-e8814ddd8c00"), ArmorType.LIGHT_ARMOR, DragonscaleRobesAbbr, DragonscaleRobesName, DragonscaleRobesDesc, DragonscaleRobesAbout, 9, 900, 0, 20);
//		public static Func<ArmorBase> EBONWEAVE_PLATEMAIL => () => new GenericArmor(Guid.Parse("08098f29-0635-40dd-ad5f-34bb8b12a0ca"), ArmorType.HEAVY_ARMOR, EbonweavePlatemailAbbr, EbonweavePlatemailName, EbonweavePlatemailDesc, EbonweavePlatemailAbout, 27, 3000, 0, 15);
//		public static Func<ArmorBase> EBONWEAVE_JACKET => () => new GenericArmor(Guid.Parse("aa2689a0-5527-4862-a848-299e3ea5eb4c"), ArmorType.MEDIUM_ARMOR, EbonweaveJacketAbbr, EbonweaveJacketName, EbonweaveJacketDesc, EbonweaveJacketAbout, 18, 3000, 0, 15);
//		public static Func<ArmorBase> EBONWEAVE_ROBE => () => new GenericArmor(Guid.Parse("0d1d93cd-a63a-4b64-a473-c90ba26177a9"), ArmorType.MEDIUM_ARMOR, EbonweaveRobeAbbr, EbonweaveRobeName, EbonweaveRobeDesc, EbonweaveRobeAbout, 9, 3000, 0, 30);
//		public static Func<ArmorBase> INDECENT_EBONWEAVE_ROBE => () => new GenericArmorWithBulge(Guid.Parse("4f0b2bb7-cfc8-4fed-b5f1-f5954ac8ecab"), ArmorType.LIGHT_ARMOR, IndecentEbonweaveRobeAbbr, IndecentEbonweaveRobeName, IndecentEbonweaveRobeDesc, IndecentEbonweaveRobeAbout, IndecentEbonweaveRobeBulgeChanged, 6, 3000, 5, 30); //bulge
//		public static Func<ArmorBase> FULL_CHAINMAIL => () => new GenericArmorWithBulge(Guid.Parse("42f87653-be8f-4681-95ec-6da1743fb856"), ArmorType.MEDIUM_ARMOR, FullChainmailAbbr, FullChainmailName, FullChainmailDesc, FullChainmailAbout, 8, 150);
//		public static Func<ArmorBase> FULL_PLATEMAIL => () => new GenericArmorWithBulge(Guid.Parse("4f19efd6-b294-4c90-a259-45a6fdab331c"), ArmorType.HEAVY_ARMOR, FullPlatemailAbbr, FullPlatemailName, FullPlatemailDesc, FullPlatemailAbout, 21, 250);
//		public static Func<ArmorBase> GEL_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("7993bf72-8688-4cb4-87f6-8a5604502ba5"), ArmorType.HEAVY_ARMOR, GelArmorAbbr, GelArmorName, GelArmorDesc, GelArmorAbout, 10, 150);
//		public static Func<ArmorBase> INDECENT_STEEL_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("805ba9be-2e1a-4f5f-aafa-979601b1e1f5"), ArmorType.MEDIUM_ARMOR, IndecentSteelArmorAbbr, IndecentSteelArmorName, IndecentSteelArmorDesc, IndecentSteelArmorAbout, 5, 800, 6);
//		public static Func<ArmorBase> LEATHER_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("34993416-ae47-4cd0-bd07-6d52f4f88601"), ArmorType.LIGHT_ARMOR, LeatherArmorAbbr, LeatherArmorName, LeatherArmorDesc, LeatherArmorAbout, 5, 76);
//		public static Func<ArmorBase> LEATHER_ROBES => () => new GenericArmorWithBulge(Guid.Parse("64f301ba-3079-4721-bbb7-f2c170d0644b"), ArmorType.LIGHT_ARMOR, LeatherRobesAbbr, LeatherRobesName, LeatherRobesDesc, LeatherRobesAbout, 6, 100);
//		public static Func<ArmorBase> SAMURAI_ARMOR => () => new GenericArmor(Guid.Parse("b87290a5-a5b1-432d-a7a7-bb9bdafa8fc0"), ArmorType.HEAVY_ARMOR, SamuraiArmorAbbr, SamuraiArmorName, SamuraiArmorDesc, SamuraiArmorAbout, 18, 300);
//		public static Func<ArmorBase> SCALE_MAIL => () => new GenericArmorWithBulge(Guid.Parse("249cdb51-96cc-4058-929d-2abcad41f1ed"), ArmorType.HEAVY_ARMOR, ScaleMailAbbr, ScaleMailName, ScaleMailDesc, ScaleMailAbout, 12, 170);
//		public static Func<ArmorBase> SPIDER_SILK_ROBES => () => new GenericArmor(Guid.Parse("773e6b61-379f-4188-8396-777b6d776925"), ArmorType.LIGHT_ARMOR, SpiderSilkRobesAbbr, SpiderSilkRobesName, SpiderSilkRobesDesc, SpiderSilkRobesAbout, 6, 950, 0, 30);
//		public static Func<ArmorBase> SPIDER_SILK_ARMOR => () => new GenericArmorWithBulge(Guid.Parse("3b068ecc-2cea-479f-a84f-fd2162eb4fad"), ArmorType.HEAVY_ARMOR, SpiderSilkArmorAbbr, SpiderSilkArmorName, SpiderSilkArmorDesc, SpiderSilkArmorAbout, 25, 950);
//		public static Func<ArmorBase> WIZARDS_ROBES => () => new GenericArmor(Guid.Parse("8820325c-08e6-4f36-9d3f-fac3e68d5d90"), ArmorType.LIGHT_ARMOR, WizardsRobesAbbr, WizardsRobesName, WizardsRobesDesc, WizardsRobesAbout, 1, 50, 0, 25);

//		//ADVETURERS_CLOTHES
//		private static string AdveturersClothesAbbr() => "G. Clothes";
//		private static string AdveturersClothesName(bool bulged) => (bulged ? "crotch-hugging " : "") + "green adventurer's clothes";
//		private static string AdveturersClothesDesc(bool bulged, byte count, bool display)
//		{
//			string setText = count != 1 ? "sets" : "set";

//			string countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";
//			string crotchText = bulged ? "crotch-hugging " : "";
//			return $"{count} {setText} of {crotchText}chitinous armor";
//			return "a " + (bulged ? "crotch-hugging " : "") + "green adventurer's outfit, complete with pointed cap";
//		}

//		private static string AdveturersClothesAbout(bool bulged)
//		{
//			return "A set of comfortable green adventurer's clothes. It even comes complete with a pointy hat!" + (bulged ? " It's been modified to prominently display your " +
//				"manly package to the world." : "");
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
//		private static string BallroomDressAbbr() => "Long Dress";
//		private static string BallroomDressName() => "long ballroom dress patterned with sequins";

//		private static string BallroomDressDesc(byte count, bool display) => "a ballroom dress patterned with sequins";

//		private static string BallroomDressAbout() => "A long ballroom dress patterned with sequins. Perfect for important occasions.";

//		//BONDAGE_STRAPS

//		private static string BondageStrapsAbbr() => "BonStrp";
//		private static string BondageStrapsName() => "barely-decent bondage straps";

//		private static string BondageStrapsDesc(byte count, bool display) => "a set of bondage straps";

//		private static string BondageStrapsAbout() => "These leather straps and well-placed hooks are actually designed in such a way as to be worn as clothing. While they technically would cover your naughty bits, virtually every other inch of your body would be exposed.";


//		//"Your fetishy bondage outfit allows you access to an improved form of 'Tease'."

//		//CLASSY_SUITCLOTHES

//		private static string ClassySuitclothesAbbr() => "Suitclothes";
//		private static string ClassySuitclothesName() => "classy suitclothes";

//		private static string ClassySuitclothesDesc(byte count, bool display) => "a set of classy suit-clothes";

//		private static string ClassySuitclothesAbout() => "A set of classy suitclothes.";

//		//KIMONO

//		private static string KimonoAbbr() => "Kimono ";
//		private static string KimonoName() => "kimono";

//		private static string KimonoDesc(byte count, bool display) => "a traditional kimono";

//		private static string KimonoAbout() => "This is a type of robes also known as kimono traditionally worn by the people of the far East. It's pretty elegant.";

//		//SHIRT_LEATHER_PANTS

//		private static string ShirtLeatherPantsAbbr() => "T.Lthr Pants";
//		private static string ShirtLeatherPantsName() => "white silk shirt and tight leather pants";

//		private static string ShirtLeatherPantsDesc(byte count, bool display) => "a pair of leather pants and a white silk shirt";

//		private static string ShirtLeatherPantsAbout() => "A flowing silk shirt and tight black leather pants. Suave!";

//		//MODEST_ROBES

//		private static string ModestRobesAbbr() => "Robes";
//		private static string ModestRobesName() => "modest robes";

//		private static string ModestRobesDesc(byte count, bool display) => "a set of modest robes";

//		private static string ModestRobesAbout() => "A set of modest robes, not dissimilar from what the monks back home would wear.";

//		//NURSE_OUTFIT

//		private static string NurseOutfitAbbr() => "NurseCl";
//		private static string NurseOutfitName() => "skimpy nurse's outfit";

//		private static string NurseOutfitDesc(byte count, bool display) => "a nurse's outfit";

//		private static string NurseOutfitAbout() => "This borderline obscene nurse's outfit would barely cover your hips and crotch. The midriff is totally exposed, and the white top leaves plenty of room for cleavage. A tiny white hat tops off the whole ensemble. It would grant a small regeneration to your HP.";

//		//"Your fetishy nurse outfit allows you access to an improved form of 'Tease'.");
//		//OVERALLS

//		private static string OverallsAbbr() => "Overalls";
//		private static string OverallsName(bool bulged)
//		{
//			return "white shirt and " + (bulged ? "crotch-hugging " : "") + "overalls";
//		}

//		private static string OverallsDesc(bool bulged, byte count, bool display) => "a " + OverallsName(bulged);

//		private static string OverallsAbout(bool bulged)
//		{
//			return "A simple white shirt and overalls." + (bulged ? " They've been magically altered to prominently display your dick-bulge" : "");
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
//		//RIGID_BODYSUIT

//		private static string RigidBodysuitAbbr() => "R.BdySt";
//		private static string RigidBodysuitName() => "red, high-society bodysuit";

//		private static string RigidBodysuitDesc(byte count, bool display) => "a red bodysuit for high society";

//		private static string RigidBodysuitAbout() => "A high society bodysuit. It is as easy to mistake it for ballroom apparel as it is for boudoir lingerie. The thin transparent fabric is so light and airy that it makes avoiding blows a second nature.";
//		//RUBBER_FETISH_SUIT

//		private static string RubberFetishSuitAbbr() => "Rbbr Fetish";
//		private static string RubberFetishSuitName() => "rubber fetish clothes";

//		private static string RubberFetishSuitDesc(byte count, bool display) => "a set of revealing rubber fetish clothes";

//		private static string RubberFetishSuitAbout() => "A revealing set of fetish-wear. Upgrades your tease attack with the \"Slutty Seduction\" perk.";

//		//"Your fetishy rubberwear allows you access to 'Seduce', an improved form of 'Tease'."
//		//SEMI_TRANSPARENT_BODYSUIT

//		private static string SemiTransparentBodysuitAbbr() => "Bodysuit";
//		private static string SemiTransparentBodysuitName() => "semi-transparent bodysuit";

//		private static string SemiTransparentBodysuitDesc(byte count, bool display) => "a semi-transparent, curve-hugging bodysuit";

//		private static string SemiTransparentBodysuitAbout() => "A semi-transparent bodysuit. It looks like it will cling to all the curves of your body.";

//		//"Your clingy transparent bodysuit allows you access to 'Seduce', an improved form of 'Tease'.");
//		//TUBETOP

//		private static string TubetopAbbr() => "Tube Top";
//		private static string TubetopName() => "tube top and short shorts";

//		private static string TubetopDesc(byte count, bool display) => "a snug tube top and VERY short shorts";

//		private static string TubetopAbout() => "A clingy tube top and VERY short shorts.";

//		//Armour

//		//CHAINMAIL_BIKINI;
//		private static string ChainmailBikiniAbbr() => "ChBikni";
//		private static string ChainmailBikiniName() => "revealing chainmail bikini";

//		private static string ChainmailBikiniDesc(byte count, bool display) => "a chainmail bikini";

//		private static string ChainmailBikiniAbout() => "A revealing chainmail bikini that barely covers anything. The bottom half is little more than a triangle of metal and a leather thong.";

//		//"Your revealing chain bikini allows you access to 'Seduce', an improved form of 'Tease'."
//		//DRAGONSCALE_ARMOR

//		private static string DragonscaleArmorAbbr() => "D.Scale Armor";
//		private static string DragonscaleArmorName() => "dragonscale armor";

//		private static string DragonscaleArmorDesc(byte count, bool display) => "a suit of dragonscale armor";

//		private static string DragonscaleArmorAbout() => "This armor is cleverly fashioned from dragon scales. It offers high protection while at the same time, quite flexible.";

//		//DRAGONSCALE_ROBES

//		private static string DragonscaleRobesAbbr() => "D.Scale Robes";
//		private static string DragonscaleRobesName() => "dragonscale robes";

//		private static string DragonscaleRobesDesc(byte count, bool display) => "a dragonscale robes";

//		private static string DragonscaleRobesAbout() => "This robe is expertly made from dragon scales. It offers high protection while being lightweight and should be comfortable to wear all day.";

//		//EBONWEAVE_PLATEMAIL

//		private static string EbonweavePlatemailAbbr() => "EW Plate";
//		private static string EbonweavePlatemailName() => "Ebonweave Platemail";

//		private static string EbonweavePlatemailDesc(byte count, bool display) => "a set of Ebonweave Platemail";

//		private static string EbonweavePlatemailAbout() => "The armor is made of ebonweave, created using refined Ebonbloom petals. The armor consists of two layers: an outer of ebonweave plating and an inner of arrow-proof ebonweave cloth.";

//		//EBONWEAVE_JACKET

//		private static string EbonweaveJacketAbbr() => "EW Jacket";
//		private static string EbonweaveJacketName() => "Ebonweave Jacket";

//		private static string EbonweaveJacketDesc(byte count, bool display) => "an Ebonweave Jacket";

//		private static string EbonweaveJacketAbout() => "This outfit is made of ebonweave, created using refined Ebonbloom petals. The outfit consists of a leather-like jacket, a mesh breastplate, and a set of arrow-proof clothing.";

//		//EBONWEAVE_ROBE

//		private static string EbonweaveRobeAbbr() => "EW Robe";
//		private static string EbonweaveRobeName() => "Ebonweave Robe";

//		private static string EbonweaveRobeDesc(byte count, bool display) => "an Ebonweave Robe";

//		private static string EbonweaveRobeAbout() => "This robe is made of ebonweave, a material created using refined Ebonbloom petals. This robe is comfortable, yet more protective than chainmail. It has a mystical aura to it.";

//		//INDECENT_EBONWEAVE_ROBE

//		private static string IndecentEbonweaveRobeAbbr() => "EW I.Robe";
//		private static string IndecentEbonweaveRobeName() => "indecent Ebonweave Robe";

//		private static string IndecentEbonweaveRobeDesc(byte count, bool display) => "an indecent Ebonweave Robe";

//		private static string IndecentEbonweaveRobeAbout() => "More of a longcoat than a robe, this outfit is crafted from refined Ebonbloom petals. Discrete straps centered around the belt keep the front open.";

//		//"Your revealing robes allow you access to 'Seduce', an improved form of 'Tease'."

//		//FULL_CHAINMAIL

//		private static string FullChainmailAbbr() => "Full Chain";
//		private static string FullChainmailName() => "full-body chainmail";

//		private static string FullChainmailDesc(byte count, bool display) => "a full suit of chainmail armor";

//		private static string FullChainmailAbout() => "This full suit of chainmail armor covers its wearer from head to toe in protective steel rings.";

//		//FULL_PLATEMAIL

//		private static string FullPlatemailAbbr() => "Full Plate";
//		private static string FullPlatemailName() => "full platemail";

//		private static string FullPlatemailDesc(byte count, bool display) => "a suit of full-plate armor";

//		private static string FullPlatemailAbout() => "A highly protective suit of steel platemail. It would be hard to find better physical protection than this.";

//		//GEL_ARMOR

//		private static string GelArmorAbbr() => "GelArmr";
//		private static string GelArmorName() => "glistening gel-armor plates";

//		private static string GelArmorDesc(byte count, bool display) => "a suit of gel armor";

//		private static string GelArmorAbout() => "This suit of interlocking plates is made from a strange green material. It feels spongy to the touch but is amazingly resiliant.";

//		//INDECENT_STEEL_ARMOR

//		private static string IndecentSteelArmorAbbr() => "Indec StAr";
//		private static string IndecentSteelArmorName() => "practically indecent steel armor";

//		private static string IndecentSteelArmorDesc(byte count, bool display) => "a suit of practically indecent steel armor";

//		private static string IndecentSteelArmorAbout() => "This suit of steel 'armor' has two round disks that barely cover the nipples, a tight chainmail bikini, and circular butt-plates.";

//		//"Your incredibly revealing steel armor allows you access to 'Seduce', an improved form of 'Tease'.");
//		//LEATHER_ARMOR

//		private static string LeatherArmorAbbr() => "LeathrA";
//		private static string LeatherArmorName() => "leather armor segments";

//		private static string LeatherArmorDesc(byte count, bool display) => "a set of leather armor";

//		private static string LeatherArmorAbout() => "This is a suit of well-made leather armor. It looks fairly rugged.";

//		//LEATHER_ROBES

//		private static string LeatherRobesAbbr() => "Lthr Robes";
//		private static string LeatherRobesName() => "black leather armor surrounded by voluminous robes";

//		private static string LeatherRobesDesc(byte count, bool display) => "a suit of black leather armor with voluminous robes";

//		private static string LeatherRobesAbout() => "This is a suit of flexible leather armor with a voluminous set of concealing black robes.";

//		//SAMURAI_ARMOR

//		private static string SamuraiArmorAbbr() => "Samu.Armor";
//		private static string SamuraiArmorName() => "samurai armor";

//		private static string SamuraiArmorDesc(byte count, bool display) => "a suit of samurai armor";

//		private static string SamuraiArmorAbout() => "This suit of armor is originally worn by the Samurai, the warriors from the far East.";

//		//SCALE_MAIL

//		private static string ScaleMailAbbr() => "Scale Mail";
//		private static string ScaleMailName() => "scale-mail armor";

//		private static string ScaleMailDesc(byte count, bool display) => "a set of scale-mail armor";

//		private static string ScaleMailAbout() => "This suit of scale-mail covers the entire body with layered steel scales, providing flexibility and protection.";

//		//SPIDER_SILK_ROBES

//		private static string SpiderSilkRobesAbbr() => "SS.Robes";
//		private static string SpiderSilkRobesName() => "spider-silk robes";

//		private static string SpiderSilkRobesDesc(byte count, bool display) => "a spider-silk robes";

//		private static string SpiderSilkRobesAbout() => "This robe looks incredibly comfortable. It's made from alchemically enhanced spider-silk, and embroidered with what looks like magical glyphs around the sleeves and hood.";

//		//SPIDER_SILK_ARMOR

//		private static string SpiderSilkArmorAbbr() => "SS.Armor";
//		private static string SpiderSilkArmorName() => "spider-silk armor";

//		private static string SpiderSilkArmorDesc(byte count, bool display) => "a suit of spider-silk armor";

//		private static string SpiderSilkArmorAbout() => "This armor is as white as the driven snow. It's crafted out of thousands of strands of spider-silk into an impenetrable protective suit. The surface is slightly spongy, but so tough you wager most blows would bounce right off.";

//		//WIZARDS_ROBES

//		private static string WizardsRobesAbbr() => "W.Robes";
//		private static string WizardsRobesName() => "wizard's robes";

//		private static string WizardsRobesDesc(byte count, bool display) => "a wizard's robes";

//		private static string WizardsRobesAbout() => "These robes appear to have once belonged to a female wizard. They're long with a slit up the side and full billowing sleeves. The top is surprisingly low cut. Somehow you know wearing it would aid your spellcasting.";




//	}
//}