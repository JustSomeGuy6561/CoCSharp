
//i don't want to accidently get backend game credit instances in my intellisense, hence this weird using directive. this is just personal preference, feel free to change it.
using CoC.Backend.GameCredits;
using CreditCategoryBase = CoC.Backend.GameCredits.CreditCategoryBase;
using SubCategory = CoC.Backend.GameCredits.SubCategory;
namespace CoC.Frontend.GameCredits
{
	public sealed partial class FrontendModCredits : CreditCategoryBase
	{
		private static SubCategory[] modCategories = new SubCategory[]
		{
			new MajorEventsModContent(),
			new OvipositorReworkContent(),
			new ExtraModContent(),
			new ModBugSquashing(),
			new TypoCheckers(),
		};
		internal FrontendModCredits() : base(FrontendModStr, modCategories)
		{

		}
	}

	public sealed partial class MajorEventsModContent : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"Parth37955 (Pure Jojo anal pitch scene, Behemoth’s vaginal catch scene)",
			"Liadri (Manticore and Dragonne suggestions)",
			"Warbird Zero (Replacement Ingnam descriptions)",
			"Matraia (Replacement Cabin construction texts)",
			"Stadler76 (New arm types, Code refactoring)",
		};

		public MajorEventsModContent() : base(MajorEventsStr, credits)
		{
		}
	}

	public sealed partial class OvipositorReworkContent : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"DCR (Idea, Drider Transformation, and Drider Impreg of: Goblins, Beegirls, Nagas, Harpies, and Basilisks)",
			"Fenoxo (Bee Ovipositor Transformation, Bee Oviposition of Nagas and Jojo, Drider Oviposition of Tamani)",
			"Smokescreen (Bee Oviposition of Basilisks)",
			"Radar (Oviposition of Sand Witches)",
			"OutlawVee (Bee Oviposition of Goo-Girls)",
			"Zeikfried (Editing this mess, Oviposition of Anemones)",
			"Woodrobin (Oviposition of Minotaurs)",
			"Posthuman (Oviposition of Ceraph Follower)",
			"Slywyn (Bee Oviposition of Gigantic PC Dick)",
			"Shaxarok (Drider Oviposition of Large Breasted Nipplecunts)",
			"Quiet Browser (Bee Oviposition of Urta)",
			"Bagpuss (Laying Eggs In Pure Amily)",
			"Eliria (Bee Laying Eggs in Bunny-Girls)",
			"Gardeford (Helia x Bimbo Sophie Threesomes)",
		};

		public OvipositorReworkContent() : base(OvipositorUpdateStr, credits)
		{ }
	}

	public sealed partial class ExtraModContent : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"worldofdrakan (Pablo the Pseudo-Imp, Pigtail Truffles & Pig/Boar TFs)",
			"FeiFongWong (Prisoner Mod)",
			"Foxxling (Lizan Rogue, Skin Oils & Body Lotions, Black Cock)",
			"LukaDoc (Bimbo Jojo)",
			"Kitteh6660 (Behemoth, Cabin, Ingnam, Pure Jojo sex scenes. Feel free to help me with quality control.)",
			"Ormael (Salamander TFs)",
			"Coalsack (Anzu the Avian Deity)",
			"Nonesuch (Izmael)",
			"IxFa (Naga Tail Masturbation)",
		};

		public ExtraModContent() : base(ExtraEventsStr, credits)
		{ }
	}

	public sealed partial class ModBugSquashing : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"Wastarce",
			"Sorenant",
			"tadams857",
			"SirWolfie",
			"Atlas1965",
			"Elitist",
			"Bsword",
			"stationpass",
			"JDoraime",
			"Ramses",
			"OPenaz",
			"EternalDragon (github)",
			"PowerOfVoid (github)",
			"kalleangka (github)",
			"sworve (github)",
			"Netys (github)",
			"Drake713 (github)",
			"NineRed (github)",
			"Fergusson951 (github)",
			"aimozg (github)",
			"Stadler76 (github + bug fix coding)",
			"Just Some Guy (C# rewrite. Also guilty of creating most of the C# bugs :( )",
		};

		public ModBugSquashing() : base(BugSquashingStr, credits)
		{ }
	}

	public sealed partial class TypoCheckers : SubCategory
	{
		private static Creditor[] credits = new Creditor[]
		{
			"SoS",
			"Prisoner416",
			"Chibodee",
			"Just Some Guy",
		};

		public TypoCheckers() : base(TypoCheckingStr, credits)
		{ }
	}
}
