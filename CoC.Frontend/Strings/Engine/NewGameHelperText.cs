//NewGameHelperText.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:28 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Strings.Engine
{
	internal static class NewGameHelperText
	{
		internal static string IntroText()
		{
			return "You grew up in the small village of Ingnam, a remote village with rich traditions, buried deep in the wilds. " +
				"Every year for as long as you can remember, your village has chosen a champion to send to the cursed Demon Realm. " +
				"Legend has it that in years Ingnam has failed to produce a champion, chaos has reigned over the countryside. " +
				"Children disappear, crops wilt, and disease spreads like wildfire. This year, <b>you</b> have been selected to be the champion." +
				Environment.NewLine + Environment.NewLine + "What is your name?";
		}

		internal static string PromptSpecial()
		{
			return "This name, like you, is special. Do you live up to your name or continue on, assuming it to be coincidence?" +
				" Note that some special characters may limit your initial customization options, though these can be changed during gameplay.";
		}

		internal static string ContinueOn()
		{
			return "Continue On";
		}

		internal static string SpecialName()
		{

			return "SpecialName";
		}

		internal static string ArrivalPartOne()
		{
			Gender gender = GameEngine.currentlyControlledCharacter.genitals.gender;
			string arousalStr;
			switch (gender)
			{
				case Gender.MALE:
					arousalStr = "and your body reacts with a sense of growing warmth focusing in your groin, your manhood hardening for no apparent reason. "; break;
				case Gender.FEMALE:
					arousalStr = "and your body seems to feel the same way, flushing as you feel a warmth and dampness between your thighs. "; break;
				case Gender.GENDERLESS:
					arousalStr = "and your body reacts with a warmth of its own, and you find yourself aroused despite your lack of endowments. "; break;
				case Gender.HERM:
				default:
					arousalStr = "mirrored, strangely, by your own body - your manhood hardens and your lady-lips become damp, despite any clear indication why. "; break;
			}
			return "You are prepared for what is to come. Most of the last year has been spent honing your body and mind to prepare for the challenges ahead. You are the Champion of Ingnam. " +
				"The one who will journey to the demon realm and guarantee the safety of your friends and family, even though you'll probably never see them again. " +
				"You wipe away a tear as you enter the courtyard and see Elder Nomur waiting for you. You are ready." + Environment.NewLine + Environment.NewLine +
				"The walk to the tainted cave is long and silent. Elder Nomur does not speak. There is nothing left to say. The two of you journey in companionable silence. " +
				"Slowly the black rock of Mount Ilgast looms closer and closer, and the temperature of the air drops. You shiver and glance at the Elder, " +
				"noticing he doesn't betray any sign of the cold. Despite his age of nearly 80, he maintains the vigor of a man half his age. You're glad for his strength, " +
				"as assisting him across this distance would be draining, and you must save your energy for the trials ahead." + Environment.NewLine +
				"The entrance of the cave gapes open, sharp stalactites hanging over the entrance, giving it the appearance of a monstrous mouth. " +
				"Elder Nomur stops and nods to you, gesturing for you to proceed alone." + Environment.NewLine + Environment.NewLine +
				"The cave is unusually warm and damp, " + arousalStr + "You were warned of this and press forward, ignoring your body's growing needs. " +
				"A glowing purple-pink portal swirls and flares with demonic light along the back wall. Cringing, you press forward, keenly aware that " +
				"your body seems to be anticipating coming in contact with the tainted magical construct. Closing your eyes, you gather your resolve and leap forwards. " +
				"Vertigo overwhelms you and you black out...";
		}

		internal static string ArrivalPartTwo()
		{
			Gender gender = GameEngine.currentlyControlledCharacter.genitals.gender;
			string arousalStr;
			switch (gender)
			{
				case Gender.MALE:
					arousalStr = "the urge to ram your cock down his throat. The strangeness of the thought surprises you."; break;
				case Gender.FEMALE:
					arousalStr = "the urge to chase down his rod and impale yourself on it."; break;
				case Gender.GENDERLESS:
					arousalStr = "your body's need, unaware of how to completely sate it."; break;
				case Gender.HERM:
				default:
					arousalStr = "your body's urges, which alternate between taking his cock in your folds and shoving your cock down his throat."; break;
			}
			return "You wake with a splitting headache and a body full of burning desire. A shadow darkens your view momentarily and your training kicks in. " +
				"You roll to the side across the bare ground and leap to your feet. A surprised looking imp stands a few feet away, holding an empty vial. " +
				"He's completely naked, an improbably sized pulsing red cock hanging between his spindly legs. You flush with desire as a wave of lust washes over you, " +
				"your mind reeling as you fight " + arousalStr + Environment.NewLine + Environment.NewLine + "The imp says, \"" +
				SafelyFormattedString.FormattedText("I'm amazed you aren't already chasing down my cock, human. The last Champion was an eager whore for me " +
					"by the time she woke up. This lust draft made sure of it.", StringFormats.ITALIC) + "\"";
		}

		internal static string ArrivalPartThree()
		{
			return "The imp shakes the empty vial to emphasize his point. You reel in shock at this revelation - you've just entered the demon realm and you've already been drugged! " +
				"You tremble with the aching need in your groin, but resist, righteous anger lending you strength." + Environment.NewLine + Environment.NewLine +
				"In desperation you leap towards the imp, watching with glee as his cocky smile changes to an expression of sheer terror. " +
				"The smaller creature is no match for your brute strength as you pummel him mercilessly. You pick up the diminutive demon and punt him into the air, " +
				"frowning grimly as he spreads his wings and begins speeding into the distance." + Environment.NewLine + Environment.NewLine;
		}

		internal static string ArrivalPartFour()
		{
			return "With what semblance of dignity he can muster, the imp bellows, \"" + SafelyFormattedString.FormattedText("FOOL! You could have had pleasure unending... " +
				"but should we ever cross paths again you will regret humiliating me! Remember the name Zetaz, as you'll soon face the wrath of my master!", StringFormats.ITALIC) +
				"\"" + Environment.NewLine + Environment.NewLine + "Your pleasure at defeating the demon ebbs as you consider how you've already been defiled. " +
				"You swear to yourself you will find the demon responsible for doing this to you and the other Champions, and destroy him AND his pet imp.";
		}

		internal static string ArrivalPartFive()
		{
			return "You look around, surveying the hellish landscape as you plot your next move. The portal is a few yards away, nestled between a formation of rocks. " +
				"It does not seem to exude the arousing influence it had on the other side. The ground and sky are both tinted different shades of red, " +
				"though the earth beneath your feet feels as normal as any other lifeless patch of dirt. You settle on the idea of making a camp here " +
				"and fortifying this side of the portal. No demons will ravage your beloved hometown on your watch." + Environment.NewLine + Environment.NewLine + 
				"It does not take long to set up your tent and a few simple traps. You'll need to explore and gather more supplies to fortify it any further. " +
				"Perhaps you will even manage to track down the demons who have been abducting the other champions!";
		}

		internal static string FirstExploration()
		{
			return "You tentatively step away from your campsite, alert and scanning the ground and sky for danger. You walk for the better part of an hour, " +
				"marking the rocks you pass for a return trip to your camp. It worries you that the portal has an opening on this side, and it was totally unguarded..." +
				Environment.NewLine + Environment.NewLine + "...Wait a second, why is your campsite in front of you? The portal's glow is clearly visible " +
				"from inside the tall rock formation. Looking carefully you see your footprints leaving the opposite side of your camp, then disappearing. " +
				"You look back the way you came and see your markings vanish before your eyes. The implications boggle your mind as you do your best to mull over them. " +
				"Distance, direction, and geography seem to have little meaning here, yet your campsite remains exactly as you left it. A few things click into place " +
				"as you realize you found your way back just as you were mentally picturing the portal! Perhaps memory influences travel here, just like time, distance, " +
				"and speed would in the real world!" + Environment.NewLine + Environment.NewLine + "This won't help at all with finding new places, " +
				"but at least you can get back to camp quickly. You are determined to stay focused the next time you explore and learn how to traverse this gods-forsaken realm.";
		}
	}
}
