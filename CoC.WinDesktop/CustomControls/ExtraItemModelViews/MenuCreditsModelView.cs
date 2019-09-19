using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CoC.Backend.Engine;
using CoC.UI;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;

namespace CoC.WinDesktop.CustomControls.ExtraItemModelViews
{
	public sealed partial class MenuCreditsModelView : ExtraItemModelViewBase
	{
		private int LastLanguageIndex;
		private int FontSizeInEms;

		private Controller controller => runner.controller;

		private string rtfContentWithSubstitutes;

		public MenuCreditsModelView(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView) : base(modelViewRunner, parentModelView)
		{
			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
			FontSizeInEms = MeasurementHelpers.FontSizeInEms;

			ContentTitle = CreditStr();
			ContentHelper = CreditHelperStr();
			ParseCredits();

			PostContent = DisclaimerStr();
		}

		internal override void ParseDataForDisplay()
		{
			if (LastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;

				ContentTitle = CreditStr();
				ContentHelper = CreditHelperStr();
				PostContent = DisclaimerStr();
				ParseCredits();
			}
			else if (FontSizeInEms != MeasurementHelpers.FontSizeInEms)
			{
				FontSizeInEms = MeasurementHelpers.FontSizeInEms;
				UpdateDisplay();
			}
		}

		private bool needsUrlColor = false;

		private void ParseCredits()
		{
			needsUrlColor = false;
			StringBuilder contentString = new StringBuilder();
			string safeString;
			foreach (var category in controller.GetCredits())
			{
				if (contentString.Length != 0)
				{
					contentString.Append(@"\par");
				}

				safeString = ParseToFormatSafeRTFString(category.CreditCategoryText());
				contentString.Append(@"\fs{0}\par\b " + safeString + @"\b0");
				foreach (var subCategory in category.subCategories)
				{
					safeString = ParseToFormatSafeRTFString(subCategory.CreditCategoryText());


					contentString.Append(@"\par\fs{1}\par\b " + safeString + @"\b0\fs{2}");

					foreach (var credit in subCategory.creditList)
					{
						string name = credit.name;
						safeString = ParseToFormatSafeRTFString(name);
						contentString.Append(@"\line \bullet  " + safeString);
						if (credit.url != null)
						{
							contentString.Append(" " + ParseToFormatSafeRTFUrl(credit.url));
							needsUrlColor = true;

						}
					}
				}
			}
			string temp = contentString.ToString();
			//because we use string.Format to cheat and place 

			rtfContentWithSubstitutes = temp;

			UpdateDisplay();
		}

		//we need 4 '{' chars per actual '{' in the end result - format consumes 2 to return 1, and converting to RTF at the FlowDocument level consumes 2 to return 1. 
		//related: https://xkcd.com/1638/
		private string ParseToFormatSafeRTFString(string unsafeString)
		{
			string temp = RTFParser.ToRTFSafeString(unsafeString);
			temp = temp.Replace(@"{", @"{{");
			temp = temp.Replace(@"}", @"}}");
			return temp;
		}

		private string ParseToFormatSafeRTFUrl(Uri url)
		{
			string temp = RTFParser.ToRTFUrl(url, 1);
			temp = temp.Replace(@"{", @"{{");
			temp = temp.Replace(@"}", @"}}");
			return temp;
		}

		private void UpdateDisplay()
		{
			List<Color> colors = new List<Color> { runner.FontColor.Color };
			if (needsUrlColor)
			{
				colors.Add(Colors.Blue);
			}
			Content = RTFParser.FromRTFText(string.Format(rtfContentWithSubstitutes, FontSizeInEms + 12, FontSizeInEms + 6, FontSizeInEms), colors, runner);
			//			Content = @"{\rtf1\ansi\deff0\nouicompat{\fonttbl{\f0\fnil  Times New Roman;}}
			//{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;}
			//{\*\generator Riched20 10.0.16299}\viewkind4\uc1 
			//\pard\sa200\sl276\slmult1\cf1\f0\fs42\lang9\par
			//\b Backend (Framework, Engine, Etc)\b0\par
			//\fs36\par
			//\b Base Game\b0\fs30\line\bullet  Fenoxo\par
			//\fs36\par
			//\b Framework/Source Code\b0\fs30\line\bullet  Fenoxo (source)\line\bullet  Kitteh6660 (mod source)\line\bullet  Stradler76 (body parts, refactoring)\line\bullet  JustSomeGuy (C# rewrite, refactoring)\par
			//\fs36\par
			//\b C# Engine\b0\fs30\line\bullet  JustSomeGuy\par
			//\fs42\par
			//\b Content\b0\par
			//\fs36\par
			//\b General Code and Helper Functions\b0\fs30\line\bullet  Fenoxo\line\bullet  Stradler76\line\bullet  JustSomeGuy\par
			//\fs36\par
			//\b Additional Events and Content\b0\fs30\line\bullet  Dxasmodeus (Tentacles, Worms, Giacomo)\line\bullet  Kirbster (Christmas Bunny Trap)\line\bullet  nRage (Kami the Christmas Roo)\line\bullet  Abraxas (Alternate Naga Scenes w/Various Monsters, Tamani Anal, Female Shouldra Tongue Licking, Chameleon Girl, Christmas Harpy)\line\bullet  Astronomy (Fetish Cultist Centaur Footjob Scene)\line\bullet  Adjatha (Scylla the Cum Addicted Nun, Vala, Goo-girls, Bimbo Sophie Eggs, Ceraph Urta Roleplay, Gnoll with Balls Scene, Kiha futa scene, Goblin Web Fuck Scene, and 69 Bunny Scene)\line\bullet  ComfyCushion (Muff Wrangler)\line\bullet  B (Brooke)\line\bullet  Quiet Browser (Half of Niamh, Ember, Amily The Mouse-girl Breeder, Katherine, Part of Katherine Employment Expansion, Urta\'92s in-bar Dialogue Trees, some of Izma, Loppe)\line\bullet  Indirect (Alternate Non-Scylla Katherine Recruitment, Part of Katherine Employment Expansion, Phouka, Coding of Bee Girl Expansion)\line\bullet  Schpadoinkle (Victoria Sex)\line\bullet  Donto (Ro\'92gar the Orc, Polar Pete)\line\bullet  Angel (Additional Amily Scenes)\line\bullet  Firedragon (Additional Amily Scenes)\line\bullet  Danaume (Jojo masturbation texts)\line\bullet  LimitLax (Sand-Witch Bad-End)\line\bullet  KLN (Equinum Bad-End)\line\bullet  TheDarkTemplar11111 (Canine Pepper Bad End)\line\bullet  Silmarion (Canine Pepper Bad End)\line\bullet  Soretu (Original Minotaur Rape)\line\bullet  NinjArt (Small Male on Goblin Rape Variant)\line\bullet  DoubleRedd (""Too Big"" Corrupt Goblin Fuck)\line\bullet  Nightshade (Additional Minotaur Rape)\line\bullet  JCM (Imp Night Gangbang, Addition Minotaur Loss Rape - Oral)\line\bullet  Xodin (Nipplefucking paragraph of Imp GangBang, Encumbered by Big Genitals Exploration Scene, Big Bits Run Encumbrance, Player Getting Beer Tits, Sand Witch Dungeon Misc Scenes)\line\bullet  Blusox6 (Original Queen Bee Rape)\line\bullet  Thrext (Additional Masturbation Code, Faerie, Ivory Succubus)\line\bullet  XDumort (Genderless Anal Masturbation)\line\bullet  Uldego (Slime Monster)\line\bullet  Noogai, Reaper, and Numbers (Nipple-Fucking Victory vs Imp Rape)\line\bullet  Verse and IAMurow (Bee-Girl MultiCock Rapes)\line\bullet  Sombrero (Additional Imp Lust Loss Scene (Dick insertion ahoy!)\line\bullet  The Dark Master (Marble, Fetish Cultist, Fetish Zealot, Hellhound, Lumi, Some Cat Transformations, LaBova, Ceraph\'92s Cat-Slaves, a Cum Witch Scene, Mouse Dreams, Forced Nursing:Imps&Goblins, Bee Girl Expansion)\line\bullet  Mr. Fleshcage (Cat Transformation/Masturbation)\line\bullet  Spy (Cat Masturbation, Forced Nursing: Minotaur, Bee, & Cultist)\line\bullet  PostNuclearMan (Some Cat TF)\line\bullet  MiscChaos (Forced Nursing: Slime Monster)\line\bullet  Ourakun (Kelt the Centaur)\line\bullet  Rika_star25 (Desert Tribe Bad End)\line\bullet  Versesai (Additional Bee Rape)\line\bullet  Mallowman (Additional Bee Rape)\line\bullet  HypnoKitten (Additional Centaur x Imp Rape)\line\bullet  Ari (Minotaur Gloryhole Scene)\line\bullet  SpectralTime (Aunt Nancy)\line\bullet  Foxxling (Akbal)\line\bullet  Elfensyne (Phylla)\line\bullet  Radar (Dominating Sand Witches, Some Phylla)\line\bullet  Jokester (Sharkgirls, Izma, & Additional Amily Scenes)\line\bullet  Lukadoc (Additional Izma, Ceraph Followers Corrupting Gangbang, Satyrs, Ember)\line\bullet  IxFa (Dildo Scene, Virgin Scene for Deluxe Dildo, Naga Tail Masturbation)\line\bullet  Bob (Additional Izma)\line\bullet  lh84 (Various Typos and Code-Suggestions)\line\bullet  Dextersinister (Gnoll girl in the plains)\line\bullet  ElAcechador, Bandichar, TheParanoidOne, Xoeleox (All Things Naga)\line\bullet  Symphonie (Dominika the Fellatrix, Ceraph RPing as Dominika, Tel\'92Adre Library)\line\bullet  Soulsemmer (Ifris)\line\bullet  WedgeSkyrocket (Zetsuko, Pure Amily Anal, Kitsunes)\line\bullet  Zeikfried (Anemone, Male Milker Bad End, Kanga TF, Raccoon TF, Minotaur Chef Dialogues, Sheila, and More)\line\bullet  User21 (Additional Centaur/Naga Scenes)\line\bullet  ~M~ (Bimbo + Imp loss scene)\line\bullet  Grype (Raping Hellhounds)\line\bullet  B-Side (Fentendo Entertainment Center Silly-Mode Scene)\line\bullet  Not Important (Face-fucking a defeated minotaur)\line\bullet  Third (Cotton, Rubi, Nieve, Urta Pet-play)\line\bullet  Gurumash (Parts of Nieve)\line\bullet  Kinathis (A Nieve Scene, Sophie Daughter Incest, Minerva)\line\bullet  Jibajabroar (Jasun)\line\bullet  Merauder (Raphael)\line\bullet  EdgeofReality (Gym fucking machine)\line\bullet  Bronycray (Heckel the Hyena)\line\bullet  Sablegryphon (Gnoll spear-thrower)\line\bullet  Nonesuch (Basilisk, Sandtraps, assisted with Owca/Vapula, Whitney Farm Corruption)\line\bullet  Anonymous Individual (Lilium, PC Birthing Driders)\line\bullet  PKD (Owca, Vapula, Fap Arena, Isabella Tentacle Sex, Lottie Tentacle Sex)\line\bullet  Shamblesworth (Half of Niamh, Shouldra the Ghost-Girl, Ceraph Roleplaying As Marble, Yara Sex, Shouldra Follow Expansion)\line\bullet  Kirbu (Exgartuan Expansion, Yara Sex, Shambles\'92s Handler, Shouldra Follow Expansion)\line\bullet  05095 (Shouldra Expansion, Tons of Editing)\line\bullet  Smidgeums (Shouldra + Vala threesome)\line\bullet  FC (Generic Shouldra talk scene)\line\bullet  Oak (Bro + Bimbo TF, Isabella\'92s ProBova Burps)\line\bullet  Space (Victory Anal Sex vs Kiha)\line\bullet  Venithil (LippleLock w/Scylla & Additional Urta Scenes)\line\bullet  Butts McGee (Minotaur Hot-dogging PC loss, Tamani Lesbo Face-ride, Bimbo Sophie Mean/Nice Fucks)\line\bullet  Savin (Hel the Salamander, Valeria, Spanking Drunk Urta, Tower of the Phoenix, Drider Anal Victory, Hel x Isabella 3Some, Centaur Sextoys, Thanksgiving Turkey, Uncorrupt Latexy Recruitment, Assert Path for Direct Feeding Latexy, Sanura the Sphinx)\line\bullet  Gats (Lottie, Spirit & Soldier Xmas Event, Kiha forced masturbation, Goblin Doggystyle, Chicken Harpy Egg Vendor)\line\bullet  Aeron the Demoness (Generic Goblin Anal, Disciplining the Eldest Minotaur)\line\bullet  Gats, Shamblesworth, Symphonie, and Fenoxo (Corrupted Drider)\line\bullet  Bagpuss (Female Thanksgiving Event, Harpy Scissoring, Drider Bondage Fuck)\line\bullet  Frogapus (The Wild Hunt)\line\bullet  Fenoxo (Everything Else)\par
			//\fs42\par
			//\b Content (Revamp Mod)\b0\par
			//\fs36\par
			//\b Major Events and Content\b0\fs30\line\bullet  Parth37955 (Pure Jojo anal pitch scene, Behemoth\'92s vaginal catch scene)\line\bullet  Liadri (Manticore and Dragonne suggestions)\line\bullet  Warbird Zero (Replacement Ingnam descriptions)\line\bullet  Matraia (Replacement Cabin construction texts)\line\bullet  Stadler76 (New arm types, Code refactoring)\par
			//\fs36\par
			//\b Oviposition Update Credits (Ordered by Appearance in Oviposition Document)\b0\fs30\line\bullet  DCR (Idea, Drider Transformation, and Drider Impreg of: Goblins, Beegirls, Nagas, Harpies, and Basilisks)\line\bullet  Fenoxo (Bee Ovipositor Transformation, Bee Oviposition of Nagas and Jojo, Drider Oviposition of Tamani)\line\bullet  Smokescreen (Bee Oviposition of Basilisks)\line\bullet  Radar (Oviposition of Sand Witches)\line\bullet  OutlawVee (Bee Oviposition of Goo-Girls)\line\bullet  Zeikfried (Editing this mess, Oviposition of Anemones)\line\bullet  Woodrobin (Oviposition of Minotaurs)\line\bullet  Posthuman (Oviposition of Ceraph Follower)\line\bullet  Slywyn (Bee Oviposition of Gigantic PC Dick)\line\bullet  Shaxarok (Drider Oviposition of Large Breasted Nipplecunts)\line\bullet  Quiet Browser (Bee Oviposition of Urta)\line\bullet  Bagpuss (Laying Eggs In Pure Amily)\line\bullet  Eliria (Bee Laying Eggs in Bunny-Girls)\line\bullet  Gardeford (Helia x Bimbo Sophie Threesomes)\par
			//\fs36\par
			//\b Additional Events and Content\b0\fs30\line\bullet  worldofdrakan (Pablo the Pseudo-Imp, Pigtail Truffles & Pig/Boar TFs)\line\bullet  FeiFongWong (Prisoner Mod)\line\bullet  Foxxling (Lizan Rogue, Skin Oils & Body Lotions, Black Cock)\line\bullet  LukaDoc (Bimbo Jojo)\line\bullet  Kitteh6660 (Behemoth, Cabin, Ingnam, Pure Jojo sex scenes. Feel free to help me with quality control.)\line\bullet  Ormael (Salamander TFs)\line\bullet  Coalsack (Anzu the Avian Deity)\line\bullet  Nonesuch (Izmael)\line\bullet  IxFa (Naga Tail Masturbation)\par
			//\fs36\par
			//\b Bug Reporting/'Quality' Assurance\b0\fs30\line\bullet  Wastarce\line\bullet  Sorenant\line\bullet  tadams857\line\bullet  SirWolfie\line\bullet  Atlas1965\line\bullet  Elitist\line\bullet  Bsword\line\bullet  stationpass\line\bullet  JDoraime\line\bullet  Ramses\line\bullet  OPenaz\line\bullet  EternalDragon (github)\line\bullet  PowerOfVoid (github)\line\bullet  kalleangka (github)\line\bullet  sworve (github)\line\bullet  Netys (github)\line\bullet  Drake713 (github)\line\bullet  NineRed (github)\line\bullet  Fergusson951 (github)\line\bullet  aimozg (github)\line\bullet  Stadler76 (github + bug fix coding)\line\bullet  Just Some Guy (C# rewrite. Also guilty of creating most of the C# bugs :( )\par
			//\fs36\par
			//\b Typo/String formatting Reporting\b0\fs30\line\bullet  SoS\line\bullet  Prisoner416\line\bullet  Chibodee\line\bullet  Just Some Guy\par
			//\fs42\par
			//\b Graphics/User Interface (GUI)\b0\par
			//\fs36\par
			//\b Original Graphics and UI\b0\fs30\line\bullet  Fenoxo (Base Game)\line\bullet  Dasutin (Background Images)\line\bullet  Invader (Button Graphics, Font, and Other Hawtness)\par
			//\fs36\par
			//\b C# Graphics and UI\b0\fs30\line\bullet  JustSomeGuy\line\bullet  Please for the love of God get someone else to clean this GUI up!\par
			//\fs42\par
			//\b Miscellaneous\b0\par
			//\fs36\par
			//\b Third-Party Tools\b0\fs30\line\bullet  Thomas Levesque - Weak Events.\lang1033  {\cf0\f0\lang9{\field{\*\fldinst{HYPERLINK ""https://github.com/thomaslevesque/WeakEvent"" }}{\fldrslt{https://github.com/thomaslevesque/WeakEvent\ul0\cf0}}}}\f0\fs30\lang9\par
			//\fs36\par
			//\b Special Thanks\par
			//}";
		}
	}
}
