
using CreditCategoryBase = CoC.Backend.GameCredits.CreditCategoryBase;
using SubCategory = CoC.Backend.GameCredits.SubCategory;

namespace CoC.Frontend.GameCredits
{
	public sealed partial class FrontendCredits : CreditCategoryBase
	{
		private static SubCategory[] frontendCategories = new SubCategory[]
		{
			new GeneralCode(),
			new ExtraContent(),
		};

		internal FrontendCredits() : base(FrontendCreditStr, frontendCategories)
		{
		}
	}

	public sealed partial class GeneralCode : SubCategory
	{
		private static string[] credits = new string[]
		{
			"Fenoxo",
			"Stradler76",
			"JustSomeGuy",
		};

		public GeneralCode() : base(GeneralCodeStr, credits)
		{
		}
	}

	//public sealed partial class MajorEventsContent : SubCategory
	//{
	//	private static string[] credits = new string[]
	//	{

	//	};

	//	public MajorEventsContent() : base(MajorEventsStr, credits)
	//	{
	//	}
	//}

	public sealed partial class ExtraContent : SubCategory
	{
		private static string[] credits = new string[]
		{
			"Dxasmodeus (Tentacles, Worms, Giacomo)",
			"Kirbster (Christmas Bunny Trap)",
			"nRage (Kami the Christmas Roo)",
			"Abraxas (Alternate Naga Scenes w/Various Monsters, Tamani Anal, Female Shouldra Tongue Licking, Chameleon Girl, Christmas Harpy)",
			"Astronomy (Fetish Cultist Centaur Footjob Scene)",
			"Adjatha (Scylla the Cum Addicted Nun, Vala, Goo-girls, Bimbo Sophie Eggs, Ceraph Urta Roleplay, Gnoll with Balls Scene, Kiha futa scene, Goblin Web Fuck Scene, and 69 Bunny Scene)",
			"ComfyCushion (Muff Wrangler)",
			"B (Brooke)",
			"Quiet Browser (Half of Niamh, Ember, Amily The Mouse-girl Breeder, Katherine, Part of Katherine Employment Expansion, Urta’s in-bar Dialogue Trees, some of Izma, Loppe)",
			"Indirect (Alternate Non-Scylla Katherine Recruitment, Part of Katherine Employment Expansion, Phouka, Coding of Bee Girl Expansion)",
			"Schpadoinkle (Victoria Sex)",
			"Donto (Ro’gar the Orc, Polar Pete)",
			"Angel (Additional Amily Scenes)",
			"Firedragon (Additional Amily Scenes)",
			"Danaume (Jojo masturbation texts)",
			"LimitLax (Sand-Witch Bad-End)",
			"KLN (Equinum Bad-End)",
			"TheDarkTemplar11111 (Canine Pepper Bad End)",
			"Silmarion (Canine Pepper Bad End)",
			"Soretu (Original Minotaur Rape)",
			"NinjArt (Small Male on Goblin Rape Variant)",
			"DoubleRedd (\"Too Big\" Corrupt Goblin Fuck)",
			"Nightshade (Additional Minotaur Rape)",
			"JCM (Imp Night Gangbang, Addition Minotaur Loss Rape - Oral)",
			"Xodin (Nipplefucking paragraph of Imp GangBang, Encumbered by Big Genitals Exploration Scene, Big Bits Run Encumbrance, Player Getting Beer Tits, Sand Witch Dungeon Misc Scenes)",
			"Blusox6 (Original Queen Bee Rape)",
			"Thrext (Additional Masturbation Code, Faerie, Ivory Succubus)",
			"XDumort (Genderless Anal Masturbation)",
			"Uldego (Slime Monster)",
			"Noogai, Reaper, and Numbers (Nipple-Fucking Victory vs Imp Rape)",
			"Verse and IAMurow (Bee-Girl MultiCock Rapes)",
			"Sombrero (Additional Imp Lust Loss Scene (Dick insertion ahoy!)",
			"The Dark Master (Marble, Fetish Cultist, Fetish Zealot, Hellhound, Lumi, Some Cat Transformations, LaBova, Ceraph’s Cat-Slaves, a Cum Witch Scene, Mouse Dreams, Forced Nursing:Imps&Goblins, Bee Girl Expansion)",
			"Mr. Fleshcage (Cat Transformation/Masturbation)",
			"Spy (Cat Masturbation, Forced Nursing: Minotaur, Bee, & Cultist)",
			"PostNuclearMan (Some Cat TF)",
			"MiscChaos (Forced Nursing: Slime Monster)",
			"Ourakun (Kelt the Centaur)",
			"Rika_star25 (Desert Tribe Bad End)",
			"Versesai (Additional Bee Rape)",
			"Mallowman (Additional Bee Rape)",
			"HypnoKitten (Additional Centaur x Imp Rape)",
			"Ari (Minotaur Gloryhole Scene)",
			"SpectralTime (Aunt Nancy)",
			"Foxxling (Akbal)",
			"Elfensyne (Phylla)",
			"Radar (Dominating Sand Witches, Some Phylla)",
			"Jokester (Sharkgirls, Izma, & Additional Amily Scenes)",
			"Lukadoc (Additional Izma, Ceraph Followers Corrupting Gangbang, Satyrs, Ember)",
			"IxFa (Dildo Scene, Virgin Scene for Deluxe Dildo, Naga Tail Masturbation)",
			"Bob (Additional Izma)",
			"lh84 (Various Typos and Code-Suggestions)",
			"Dextersinister (Gnoll girl in the plains)",
			"ElAcechador, Bandichar, TheParanoidOne, Xoeleox (All Things Naga)",
			"Symphonie (Dominika the Fellatrix, Ceraph RPing as Dominika, Tel’Adre Library)",
			"Soulsemmer (Ifris)",
			"WedgeSkyrocket (Zetsuko, Pure Amily Anal, Kitsunes)",
			"Zeikfried (Anemone, Male Milker Bad End, Kanga TF, Raccoon TF, Minotaur Chef Dialogues, Sheila, and More)",
			"User21 (Additional Centaur/Naga Scenes)",
			"~M~ (Bimbo + Imp loss scene)",
			"Grype (Raping Hellhounds)",
			"B-Side (Fentendo Entertainment Center Silly-Mode Scene)",
			"Not Important (Face-fucking a defeated minotaur)",
			"Third (Cotton, Rubi, Nieve, Urta Pet-play)",
			"Gurumash (Parts of Nieve)",
			"Kinathis (A Nieve Scene, Sophie Daughter Incest, Minerva)",
			"Jibajabroar (Jasun)",
			"Merauder (Raphael)",
			"EdgeofReality (Gym fucking machine)",
			"Bronycray (Heckel the Hyena)",
			"Sablegryphon (Gnoll spear-thrower)",
			"Nonesuch (Basilisk, Sandtraps, assisted with Owca/Vapula, Whitney Farm Corruption)",
			"Anonymous Individual (Lilium, PC Birthing Driders)",
			"PKD (Owca, Vapula, Fap Arena, Isabella Tentacle Sex, Lottie Tentacle Sex)",
			"Shamblesworth (Half of Niamh, Shouldra the Ghost-Girl, Ceraph Roleplaying As Marble, Yara Sex, Shouldra Follow Expansion)",
			"Kirbu (Exgartuan Expansion, Yara Sex, Shambles’s Handler, Shouldra Follow Expansion)",
			"05095 (Shouldra Expansion, Tons of Editing)",
			"Smidgeums (Shouldra + Vala threesome)",
			"FC (Generic Shouldra talk scene)",
			"Oak (Bro + Bimbo TF, Isabella’s ProBova Burps)",
			"Space (Victory Anal Sex vs Kiha)",
			"Venithil (LippleLock w/Scylla & Additional Urta Scenes)",
			"Butts McGee (Minotaur Hot-dogging PC loss, Tamani Lesbo Face-ride, Bimbo Sophie Mean/Nice Fucks)",
			"Savin (Hel the Salamander, Valeria, Spanking Drunk Urta, Tower of the Phoenix, Drider Anal Victory, Hel x Isabella 3Some, Centaur Sextoys, Thanksgiving Turkey, Uncorrupt Latexy Recruitment, Assert Path for Direct Feeding Latexy, Sanura the Sphinx)",
			"Gats (Lottie, Spirit & Soldier Xmas Event, Kiha forced masturbation, Goblin Doggystyle, Chicken Harpy Egg Vendor)",
			"Aeron the Demoness (Generic Goblin Anal, Disciplining the Eldest Minotaur)",
			"Gats, Shamblesworth, Symphonie, and Fenoxo (Corrupted Drider)",
			"Bagpuss (Female Thanksgiving Event, Harpy Scissoring, Drider Bondage Fuck)",
			"Frogapus (The Wild Hunt)",
			"Fenoxo (Everything Else)",
		};

		public ExtraContent() : base(ExtraEventsStr, credits)
		{
		}
	}

	//public sealed partial class BugSquashing : SubCategory
	//{
	//	private static string[] credits = new string[]
	//	{

	//	};

	//	public BugSquashing() : base(BugSquashingStr, credits)
	//	{
	//	}
	//}
}
