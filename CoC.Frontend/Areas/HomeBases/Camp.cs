using CoC.Backend.Areas;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Reaction;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using CoC.Backend.UI;
using CoC.Frontend.SaveData;
using CoC.Frontend.UI;
using System;
using System.Collections;
using System.Text;

namespace CoC.Frontend.Areas.HomeBases
{
	internal sealed partial class Camp : HomeBaseBase
	{
#warning implement me!
		private bool beatTheGame => false;

		public static byte CabinProgress
		{
			get => FrontendSessionSave.data.cabinProgress;
			private set => FrontendSessionSave.data.cabinProgress = value;
		}

		public static byte WallProgress
		{
			get => FrontendSessionSave.data.wallProgress;
			private set => FrontendSessionSave.data.wallProgress = value;
		}

		public static ushort WallSkulls
		{
			get => FrontendSessionSave.data.wallSkullCount;
			private set => FrontendSessionSave.data.wallSkullCount = value;
		}

		public static ushort WallStatues
		{
			get => FrontendSessionSave.data.wallStatueCount;
			private set => FrontendSessionSave.data.wallStatueCount = value;
		}

		public static bool CabinBuilt => CabinProgress >= 10;

		public static bool WallGatesBuilt => WallProgress >= 120;

		private static BitArray furnitureFlags = new BitArray(6);

		public const int BED_FLAG = 2;

		public bool CabinHasFurniture(int index)
		{
			return CabinBuilt && (furnitureFlags.Length < index ? furnitureFlags[index] : false);
		}

		public static bool HasDefenseCanopy
		{
			get; private set;
		} = false;

		public Camp() : base(CampName)
		{
		}

		protected override void OnReload()
		{
			throw new NotImplementedException();
		}

		protected override void LoadUniqueCampActionsMenu(DisplayBase currentDisplay)
		{
			currentDisplay.OutputText("Not implemented :(");
			AddReturnButtonToDisplay();
		}

		protected override string CampDescription(bool isReload)
		{
			//nothing in here has any side effects (except duplicated adds on reactions), so we're fine to ignore isreload (except the reactions)

			StringBuilder sb = new StringBuilder();
			//if (isabellaFollower()) //Isabella upgrades camp level!
			//	sb.Append("Your campsite got a lot more comfortable once Isabella moved in. Carpets cover up much of the barren ground, simple awnings tied to the rocks provide shade, and hand-made wooden furniture provides comfortable places to sit and sleep. ");
			//else
			//{ //live in-ness
			if (GameDateTime.Now.day < 10) sb.Append("Your campsite is fairly simple at the moment. Your tent and bedroll are set in front of the rocks that lead to the portal. You have a small fire pit as well. ");
			else if (GameDateTime.Now.day < 20) sb.Append("Your campsite is starting to get a very 'lived-in' look. The fire-pit is well defined with some rocks you've arranged around it, and your bedroll and tent have been set up in the area most sheltered by rocks. ");
			else //if (GameDateTime.Now.day >= 20)
			{
				//if (!isabellaFollower()) sb.Append("Your new home is as comfy as a camp site can be. ");
				sb.Append("The fire-pit ");
				if (CabinBuilt && CabinHasFurniture(BED_FLAG)) sb.Append("is ");
				else sb.Append("and tent are both ");
				sb.Append("set up perfectly, and in good repair. ");
			}
			//}
			if (GameDateTime.Now.day >= 20) sb.Append("You've even managed to carve some artwork into the rocks around the camp's perimeter." + Environment.NewLine + Environment.NewLine);
			if (CabinProgress == 7) sb.Append("There's an unfinished wooden structure. As of right now, it's just frames nailed together." + Environment.NewLine + Environment.NewLine);
			else if (CabinProgress == 8) sb.Append("There's an unfinished cabin. It's currently missing windows and door." + Environment.NewLine + Environment.NewLine);
			else if (CabinProgress == 9) sb.Append("There's a nearly-finished cabin. It looks complete from the outside but inside, it's missing flooring." + Environment.NewLine + Environment.NewLine);
			else if (CabinProgress >= 10) sb.Append("Your cabin is situated near the edge of camp." + Environment.NewLine + Environment.NewLine);

			//companion domains and such.
			visitors.ForEach(x => sb.Append(x.HomeBaseDomainDescription(this, GameEngine.CurrentHour)));
			
			//fortifications.

			if (HasDefenseCanopy) sb.Append("A thorny tree has sprouted near the center of the camp, growing a protective canopy of spiky vines around the portal and your camp. ");

			if (WallProgress == 0)
			{
				sb.Append("You have a number of traps surrounding your makeshift home, but they are fairly simple and may not do much to deter a demon. ");
			}
			else if (WallProgress < 20)
			{
				sb.Append("A thick wooden wall has been erected to provide a small amount of defense. ");
			}
			else if (WallProgress < 100)
			{
				sb.Append("Thick wooden walls have been erected to provide some defense. ");
			}
			else //if (WallProgress >= 100)
			{
				sb.Append("Thick wooden walls have been erected; they encloseone half of your camp perimeter and provide good defense, leaving the another half open for access to the stream. ");
				sb.Append("Defenses continue along the opposite riverbank. ");

				if (WallGatesBuilt) sb.Append("A gate has been constructed in the middle of the walls, and both sides of the stream are protected by metal grates. These" +
					"are closed at night to keep any invaders out. ");
				if (WallSkulls > 0)
				{
					if (WallSkulls == 1) sb.Append("A single imp skull has been mounted near the gateway");
					else if (WallSkulls >= 2 && WallSkulls < 5) sb.Append("A few imp skulls have been mounted near the gateway");
					else if (WallSkulls >= 5 && WallSkulls < 15) sb.Append("Several imp skulls have been mounted near the gateway");
					else sb.Append("A significant number of imp skulls decorate the gateway and wall, some even impaled on wooden spikes");
					sb.Append(" to serve as deterrence. ");

					if (WallSkulls > 1)
					{
						sb.Append("There are currently " + Utils.NumberAsText(WallSkulls) + " skulls. ");
					}
				}
				if (WallStatues > 0)
				{
					if (WallStatues == 1) sb.Append("Looking around the perimeter of your camp you spy a single marble imp statue. ");
					else sb.Append("Dotted around and on the wall that surrounds your camp you spy " + Utils.NumberAsText(WallStatues) + " marble imp statues. ");
				}
				sb.Append(Environment.NewLine + Environment.NewLine);
			}


			sb.Append("The portal shimmers in the background as it always does, looking menacing and reminding you of why you came.");
			sb.Append(Environment.NewLine + Environment.NewLine);

			visitors.ForEach(x => x.HomeBaseIdleDescription(this, GameDateTime.Now.hour));

			//companion text. 
			/*
			if (flags[kFLAGS.ANT_KIDS] > 1000) sb.Append(" A series of small mounds exist nearby it, no doubt linked to the underground maze your ant children inhabit. Sometimes you'll catch movement around one of them - " +
				"you have no doubt Phylla is keeping her promise to keep an eye on it. You feel a safer knowing you'll be notified should anything threaten the portal, or if, gods forbid, anyone else has come through after you." +
				Environment.NewLine + Environment.NewLine);
			if (flags[kFLAGS.EMBER_CURRENTLY_FREAKING_ABOUT_MINOCUM] == 1)
			{ //Ember's anti-minotaur crusade!
			 //Modified Camp Description
				sb.Append("Since Ember began " + emberMF("his", "her") + " 'crusade' against the minotaur population, skulls have begun to pile up on either side of the entrance to " + emberScene.emberMF("his", "her") + " den. There're quite a lot of them." + Environment.NewLine + Environment.NewLine);
			}
			if (flags[kFLAGS.FUCK_FLOWER_LEVEL] >= 4 && flags[kFLAGS.FUCK_FLOWER_KILLED] == 0) //dat tree!
				sb.Append("On the outer edges, half-hidden behind a rock, is a large, very healthy tree. It grew fairly fast, but seems to be fully developed now. Holli, Marae's corrupt spawn, lives within." + Environment.NewLine + Environment.NewLine);
			campFollowers(true); //display NPCs
								 //MOUSEBITCH
			if (amilyScene.amilyFollower() && flags[kFLAGS.AMILY_FOLLOWER] == 1)
			{
				if (flags[kFLAGS.FUCK_FLOWER_LEVEL] >= 4 && flags[kFLAGS.FUCK_FLOWER_KILLED] == 0) sb.Append("Amily has relocated her grass bedding to the opposite side of the camp from the strange tree; every now and then, she gives it a suspicious glance, as if deciding whether to move even further." + Environment.NewLine + Environment.NewLine);
				else sb.Append("A surprisingly tidy nest of soft grasses and sweet-smelling herbs has been built close to your " + (flags[kFLAGS.CAMP_BUILT_CABIN] > 0 ? "cabin" : "bedroll") + ". A much-patched blanket draped neatly over the top is further proof that Amily sleeps here. She changes the bedding every few days, to ensure it stays as nice as possible." + Environment.NewLine + Environment.NewLine);
			}
			campLoversMenu(true); //display Lovers
			campSlavesMenu(true); //display Slaves
			*/
			Player player = GameEngine.currentlyControlledCharacter;

			if (BackendSessionSave.data.HungerEnabled && player.hunger < 25)
			{ //hunger check!
				sb.Append("<b>You have to eat something; your stomach is growling " + (player.hunger < 1 ? "painfully" : "loudly") + ". </b>");
				if (player.hunger < 10) sb.Append("<b>You are getting thinner and you're losing muscles. </b>");
				if (player.hunger <= 0) sb.Append("<b>You are getting weaker due to starvation. </b>");
				sb.Append(Environment.NewLine + Environment.NewLine);
			}
			if (player.lust >= player.maxLust)
			{ //the uber horny
			  //if (player.hasStatusEffect(StatusEffects.Dysfunction)) sb.Append("<b>You are debilitatingly aroused, but your sexual organs are so numbed the only way to get off would be to find something tight to fuck or get fucked...</b>" + Environment.NewLine + Environment.NewLine);
			  //else if (flags[kFLAGS.UNABLE_TO_MASTURBATE_BECAUSE_CENTAUR] > 0 && player.isTaur()) sb.Append("<b>You are delibitatingly aroused, but your sex organs are so difficult to reach that masturbation isn't at the forefront of your mind.</b>" + Environment.NewLine + Environment.NewLine);
			  //else
			  //{
				sb.Append("<b>You are debilitatingly aroused, and can think of doing nothing other than masturbating.</b>" + Environment.NewLine + Environment.NewLine);
				//This once disabled the ability to rest, sleep or wait, but ir hasn't done that for many many builds
				//}
			}
			//Set up rest stuff
			if (GameDateTime.Now.hour < 6 || GameDateTime.Now.hour > 20)
			{ //night
				if (!beatTheGame) sb.Append("It is dark out, made worse by the lack of stars in the sky. A blood-red moon hangs in the sky, seeming to watch you, but provides little light. It's far too dark to leave camp." + Environment.NewLine + Environment.NewLine); //Lethice not defeated
				else
				{ //Lethice defeated, proceed with weather
					switch (WeatherEngine.CurrentConditions)
					{
						case Weather.CLOUDY: sb.Append("Despite the darkness of the nighttime sky, you can make out some clouds, faintly illumated red by what must be the moon. It's far too dark to leave camp." + Environment.NewLine + Environment.NewLine); break;
						case Weather.RAINY: sb.Append("Darker clouds cover the already dark sky, accompanied by rain. It's far too dark to leave camp." + Environment.NewLine + Environment.NewLine); break;
						case Weather.THUNDERSTROMS: sb.Append("Dark clouds and the pelting rain fill the night sky. While the occasional flash of lightning provides some light, it's far too treacherous to leave camp." + Environment.NewLine + Environment.NewLine); break;
						case Weather.CLEAR:
						case Weather.PARTLY_CLOUDY:
						default:
							sb.Append("A blood-red moon hangs in the sky, seeming to watch you, but providing little light. Despite the plethora of stars dotting the night sky, they too provide almost no light. " +
					   "It's far too dark to leave camp." + Environment.NewLine + Environment.NewLine); break;
					}
				}
				if (hasAnyVisitors && (GameDateTime.Now.hour < 4 || GameDateTime.Now.hour < 23))
					sb.Append("Your camp is silent as your companions are sleeping right now." + Environment.NewLine);
			}
			else
			{ //day time!
				if (beatTheGame)
				{ //Lethice defeated
					switch (WeatherEngine.CurrentConditions)
					{
						case Weather.CLEAR: sb.Append("The sun shines brightly, illuminating the now-blue sky. "); break;
						case Weather.PARTLY_CLOUDY: sb.Append("The sun shines brightly, illuminating the now-blue sky. Occasional clouds dot the sky, appearing to form different shapes. "); break;
						case Weather.CLOUDY: sb.Append("Clouds cover the sky, blanketing it in a dull gray. "); break;
						case Weather.RAINY: sb.Append("Dark clouds cover the sky, accompanied by rain, though fortunately it's light enough that it's only a minor inconvenience."); break;
						case Weather.THUNDERSTROMS:
							sb.Append("Dark clouds and the pelting rain hinder your visibility, making most travel difficult. Occasional steaks of Lightning provide some visibility," +
								" but are almost always followed by the deafening boom of thunder"); break;
						//NGL, y'all suck at defaults with switch statements - default should always fallback to the default (hence the name) not be some sort of error. 
						//so, if the default is sunny, make the sunny case fallthrough to the default and use it there. BUT, i'm keeping this one because rule of cool. 
						default: sb.Append("The sky is black and flashing green 0's and 1's, seems like the weather is broken! "); break;
					}
				}
				if (GameDateTime.Now.hour == 19)
				{
					if (WeatherEngine.CurrentConditions < Weather.CLOUDY) sb.Append("The sun is close to the horizon, getting ready to set. ");
					else sb.Append("Though you cannot see the sun, the sky near the horizon began to glow orange. ");
				}
				if (GameDateTime.Now.hour == 20)
				{
					if (WeatherEngine.CurrentConditions < Weather.CLOUDY) sb.Append("The sun has already set below the horizon. The sky glows orange. ");
					else sb.Append("Even with the clouds, the sky near the horizon is glowing bright orange. The sun may have already set at this point. ");
				}
				sb.Append("It's light outside, a good time to explore and forage for supplies with which to fortify your camp." + Environment.NewLine);
			}

			//the next time you visit camp, you'll realize it's pretty shitty to be in a tent still. 
			if (CabinProgress <= 0 && GameDateTime.Now.day >= 14 && !isReload) //don't cause a dupe to fire if we are reloading.
			{
				GameEngine.AddHomeBaseReaction(new HomeBaseReaction(MakeACabinYouBitch));
			}

			//testing the reactions.
			//if (!FrontendGlobalSave.data.UnlockedNewGameHerm && player.gender == Gender.HERM)
			//{
			//	GameEngine.AddOneOffReaction(new OneOffTimeReactionBase(new GenericSimpleReaction(UnlockedHermWhoo)));	
			//}
			if (!FrontendGlobalSave.data.UnlockedNewGameHerm)
			{
				FrontendGlobalSave.data.UnlockedNewGameHerm = true;
				sb.Append(Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("Congratulations! " +
					"You have unlocked hermaphrodite option on character creation, available for any New Game!", StringFormats.BOLD));
			}

			return sb.ToString();
		}

		//testing to see if reactions proc. they do. feel free to move this back to where it came from.
		//private string UnlockedHermWhoo(bool currentlyIdling, bool hasIdleHours)
		//{
		//	//prevents stray duplicates. shouldn't happen.
		//	if (!FrontendGlobalSave.data.UnlockedNewGameHerm)
		//	{
		//		FrontendGlobalSave.data.UnlockedNewGameHerm = true;
		//		return Environment.NewLine + Environment.NewLine + "<b>Congratulations! You have unlocked hermaphrodite option on character creation, available for any New Game!</b>";
		//	}
		//	return null;
		//}

		//alternatively, make this a "dream" or a full one complete with an image, idc.
		private void MakeACabinYouBitch(bool isReload)
		{
			//we actually don't care about it being a reload, as the same logic still applies. if there were side effects that would cause errors
			//on reload, we'd have to manange them.
			if (CabinProgress <= 0)
			{
				CabinProgress = 1;

				var display = DisplayManager.GetCurrentDisplay();
				display.OutputText("You realize that you have spent two weeks sleeping in tent every night, and it's getting a little old. You'd prefer something that will let you sleep " +
					"nicely and comfortably. Perhaps a cabin will suffice?");
				display.DoNext(() => RunAreaWithCurrentDisplay(false));
			}
		}
	}


}
