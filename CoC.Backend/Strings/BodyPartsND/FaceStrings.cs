//FaceStrings.cs
//Description:
//Author: JustSomeGuy
//1/11/2019, 6:58 PM
using CoC.Backend.Creatures;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	public partial class LipPiercingLocation
	{
		private static string LabretButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LabretLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MedusaButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MedusaLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MonroeLeftButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MonroeLeftLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MonroeRightButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MonroeRightLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerLeft1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerLeft1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerLeft2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerLeft2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerRight1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerRight1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerRight2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerRight2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class EyebrowPiercingLocation
	{
		private static string Left1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Left2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right1Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right1Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right2Button()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Right2Location()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class NosePiercingLocation
	{
		private static string LeftButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SeptimusButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SeptimusLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BridgeButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BridgeLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class FaceTattooLocation
	{
		private static string LeftCheekButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftCheekLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftTysonButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftTysonLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCheekButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCheekLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightTysonButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightTysonLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerJawButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LowerJawLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ForeheadButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ForeheadLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullFaceButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullFaceLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Face
	{
		//NOTE To future implementers: GOO is kindof a bitch, because it's not solid like literally everything else. this means literally anywhere it gets a unique type it requires
		//unique text. I've created a generic formula to deal with this, but you can write your own flavor text as you see fit. Copypaste this as the first check in the transform text
		//function and adapt as needed.


		//if (previousFaceData.type == GOO)
		//{
		//	return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
		//		SafelyFormattedString.FormattedText("it has solidified into a more natural state, and is now <NewTypeAdjective> in nature!", StringFormats.BOLD);
		//}


		public static string Name()
		{
			return "Face";
		}

		public static string PlayerFacialStructure(PlayerBase player)
		{
			return "It has " + FacialStructureText(player.genitals.femininity.AsReadOnlyData());
		}

		public static string FacialStructureText(FemininityData femininity)
		{
			//0-10
			if (femininity.value < 10)
			{
				return "a square chin and chiseled jawline";
			}
			//10+ -20
			else if (femininity.value < 20)
			{
				return "a rugged, handsome look to it";
			}
			//21-28
			else if (femininity.value < 28)
			{
				return "a well-defined jawline and a fairly masculine profile";
			}
			//28+-35
			else if (femininity.value < 35)
			{
				return "a somewhat masculine, angular jawline";
			}
			//35-45
			else if (femininity.value < 45)
			{
				return "the barest hint of masculinity on its features";
			}
			//45-55
			else if (femininity.value <= 55)
			{
				return "an androgynous set of features that would look normal on a male or female";
			}
			//55+-65
			else if (femininity.value <= 65)
			{
				return "a tiny touch of femininity.value to it, with gentle curves";
			}
			//65+-72
			else if (femininity.value <= 72)
			{
				return "a nice set of cheekbones and lips that have the barest hint of pout";
			}
			//72+-80
			else if (femininity.value <= 80)
			{
				return "a beautiful, feminine shapeliness that's sure to draw the attention of males";
			}
			//81-90
			else if (femininity.value <= 90)
			{
				return "a gorgeous profile with full lips, a button nose, and noticeable eyelashes";
			}
			//91-100
			else
			{
				return "a jaw-droppingly feminine shape with full, pouting lips, an adorable nose, and long, beautiful eyelashes";
			}
		}

		private string AllLipPiercingsShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllNosePiercingsShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllEyebrowPiercingsShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllLipPiercingsLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllNosePiercingsLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllEyebrowPiercingsLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTattoosShort(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllTattoosLong(PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class FaceType
	{
		private string FaceStr()
		{
			return "Face";
		}

		private string YourFaceStr(out bool isPlural)
		{
			if (epidermisType == EpidermisType.FEATHERS)
			{
				isPlural = false;
				return "the skin beneath the feathers covering your face";
			}
			else if (epidermisType == EpidermisType.FUR)
			{
				isPlural = false;
				return "the skin beneath the fur covering your face";
			}
			else if (epidermisType == EpidermisType.BARK)
			{
				isPlural = false;
				return "the bark covering your face";
			}
			else if (epidermisType == EpidermisType.CARAPACE)
			{
				isPlural = false;
				return "the carapace making up your face";
			}
			else if (epidermisType == EpidermisType.GOO)
			{
				isPlural = false;
				return "the gooey surface of your face";
			}
			else if (epidermisType == EpidermisType.WOOL)
			{
				isPlural = false;
				return "the skin beneath the wool covering your face";
			}
			else if (epidermisType == EpidermisType.SKIN)
			{
				isPlural = false;
				return "the skin covering your face";
			}
			else if (epidermisType == EpidermisType.SCALES)
			{
				isPlural = true;
				return "the scales covering your face";
			}

			else //(epidermisType == EpidermisType.EMPTY)
			{
				isPlural = false;
				return "your face";
			}
		}

		#region Human
		private static string HumanShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("face", singleItemFormat);
		}
		private static string HumanLongDesc(FaceData face, bool alternateFormat)
		{
			return face.skinTexture.AsString(alternateFormat) + " human face";
		}
		private static string HumanPlayerStr(Face face, PlayerBase player)
		{
			string noseStr = face.nosePiercings.isPierced ? "pierced " : "normal ";
			return " Your face is human in shape and structure, with " + face.primary.DescriptionWithTexture() + " and a " + noseStr + "nose. ";
		}
		private static string HumanTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			return previousFaceData.type.RestoredString(previousFaceData, player);
		}
		private static string HumanRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(originalFaceData, player);
		}
		#endregion
		#region Horse
		private static string HorseShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("horse-like face", singleItemFormat);
		}
		private static string HorseLongDesc(FaceData face, bool alternateFormat)
		{
			return $"{(alternateFormat ? "a " : "")}longe, equine muzzle covered in {face.primaryEpidermis.LongDescription()}";
		}
		private static string HorsePlayerStr(Face face, PlayerBase player)
		{
			return "Your face is almost entirely equine in appearance, even having a" + face.primary.LongDescription() +
				". Underneath the fur, you believe you have " + face.facialSkin.LongDescription();
		}
		private static string HorseTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified into a more natural state, and is now equine in nature!", StringFormats.BOLD);
			}

			string typeChange;
			if (previousFaceData.type == FaceType.DOG)
			{
				typeChange = "your skin crawls and shifts, your visage reshaping to replace your dog-like characteristics with those of a horse. ";
			}
			else
			{
				typeChange = "you feel your skin crawl and elongate under your fingers. Eventually the pain subsides, leaving you with a face that seamlessly blends human " +
					"and equine features. ";
			}

			return "Mind-numbing pain shatters through you as you feel your facial bones rearranging. You clutch at your face in agony as " + typeChange +
				SafelyFormattedString.FormattedText("You now have a very equine-looking face!", StringFormats.BOLD);
		}
		private static string HorseRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Dog
		private static string DogShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("canine muzzle", singleItemFormat);
		}
		private static string DogLongDesc(FaceData face, bool alternateFormat)
		{
			return $"{face.primaryEpidermis.LongAdjectiveDescription(alternateFormat)} canine face";
		}
		private static string DogPlayerStr(Face face, PlayerBase player)
		{
			return "You have a dog's face, complete with wet nose and panting tongue. You've got a" + face.primary.LongDescription() + ", hiding your " + face.facialSkin.LongDescription()
				+ " underneath your furry visage.";
		}
		private static string DogTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified into a more natural state, and is now canine-shaped!", StringFormats.BOLD);
			}

			string intro = "Your face is wracked with pain. You throw back your head and scream in agony as you feel your cheekbones breaking and shifting, reforming into something";
			if (previousFaceData.type == HORSE)
			{
				return intro + " else. " + SafelyFormattedString.FormattedText("Your horse-like features rearrange to take on many canine aspects.", StringFormats.BOLD);
			}
			else
			{
				return intro + "... different. You find a puddle to view your reflection, and realize " + SafelyFormattedString.FormattedText("your face is now a cross " +
					"between human and canine features.", StringFormats.BOLD);
			}
		}
		private static string DogRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Cow/Minotaur
		private static string CowShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("cow-like face", singleItemFormat);
		}
		private static string MinotaurShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("bovine snout", singleItemFormat);
		}

		private static string CowMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "Your cow-like face is becoming rougher and more bull-like, losing what remained of your human features.";
			}
			else
			{
				return "Your face starts to soften and starts resembling a more human face, though you aren't completely devoid of cow-like features. ";
			}
		}

		private static string Cow_MinotaurLongDesc(FaceData face, bool alternateFormat)
		{
			if (face.isFullMorph)
			{
				return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " somewhat cow-like face";
			}
			else
			{
				return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " bovine face";
			}
		}
		private static string Cow_MinotaurPlayerStr(Face face, PlayerBase player)
		{
			if (face.isFullMorph)
			{
				string noseRingStr = face.wearingCowNoseRing ? "particularly your nose, which is squared off and has a ring running through it" : "particularly a squared off wet nose";
				return "You have a face resembling that of a minotaur, with cow-like features, " + noseRingStr + ". Your " + face.primary.DescriptionWithColor() +
					" thickens noticeably on your head, looking shaggy and more than a little monstrous once laid over your visage.";
			}
			else
			{
				string noseStr = face.wearingCowNoseRing
					? "The most obvious is your nose, which is wider than a human's and squared off - not to mention the ring running through it."
					: "Your nose, for example, is squared - off, like one you'd normally see on a cow.";
				return "Your face appears human, though some features show some cow-like traits. " + noseStr;
			}
		}
		private static string Cow_MinotaurTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				if (!player.face.isHumanoid)
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural, human head!", StringFormats.BOLD) +
						" That's not all, however: patches of fur grow in, and your features start to take on some cow-like traits. " +
						SafelyFormattedString.FormattedText("You now have a cow-like face!", StringFormats.BOLD);
				}
				else
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural state, and has taken on a minotaur-like appearance!", StringFormats.BOLD);
				}
			}
			else if (!player.face.isFullMorph)
			{

				return "Your visage twists painfully, warping and crackling as your bones are molded into a new shape. Once it finishes, you reach up to touch it, and you discover that " +
					SafelyFormattedString.FormattedText("Your face is like that of a cow!", StringFormats.BOLD);

			}
			else
			{
				return "Bones shift and twist painfully as your visage twists and morphs, soon resembling that of a minotaur. " +
					SafelyFormattedString.FormattedText("You now have a minotaur-like face!", StringFormats.BOLD);
			}
		}
		private static string Cow_MinotaurRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Shark
		private static string SharkShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("shark teeth", singleItemFormat);
		}
		private static string SharkLongDesc(FaceData face, bool alternateFormat)
		{
			return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " face with retractible shark-teeth";
		}
		private static string SharkPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is human in shape and structure, covered in " + face.facialSkin.LongDescription() + ". A set of razor-sharp, retractable shark-teeth fill your mouth " +
				"and give your visage a slightly angular appearance.";
		}
		private static string SharkTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified, into a more natural, human head!", StringFormats.BOLD) +
					" Strangely, though, it's not completely human - a second, sharper set of teeth appear in front of your normal ones. As you ponder this, they retract inward. " +
					"It seems " + SafelyFormattedString.FormattedText("You now have retractible shark-like teeth, too!", StringFormats.BOLD);
			}

			string intro;
			if (player.face.type == HUMAN)
			{
				intro = "";
			}
			else if (player.face.isHumanoid)
			{
				intro = "Your face reshapes slightly, until it appears fully human. ";
			}
			else
			{
				intro = "Your " + previousFaceData.LongDescription() + " explodes with agony, reshaping into a more human-like visage. ";
			}

			return intro + "You firmly grasp your mouth, an intense pain racking your oral cavity. Your gums shift around and the bones in your jaw reset. " +
				"You blink a few times, wondering what just happened. You move over to a puddle to catch sight of your reflection, and you are thoroughly surprised by what you see - " +
				SafelyFormattedString.FormattedText("a set of retractable shark fangs have grown in front of your normal teeth, and your face has elongated slightly to accommodate them!",
				StringFormats.BOLD) + " They even scare you a little.";
		}
		private static string SharkRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Snake
		//Snake? SNAKE?!? SNAAAAAAKE!?!?!?!?!
		private static string SnakeShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("snake fangs", singleItemFormat);
		}
		private static string SnakeLongDesc(FaceData face, bool alternateFormat)
		{
			return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " face with venomous fangs";
		}
		private static string SnakePlayerStr(Face face, PlayerBase player)
		{
			return "Your face is fairly human in shape, but is covered in " + face.primary.LongDescription() + ". In addition, a pair of fangs hang over your lower lip, " +
				"dripping with venom.";
		}
		private static string SnakeTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			string typeChange;

			//added in because snake and spider fangs are described almost identically; it'd be weird to say you gain fangs when you already have them.
			if (previousFaceData.type == SPIDER)
			{
				return "Without warning, your venomous spider fangs suddenly snap out, forcing your jaw open in the process. You have the faint taste of blood on your " +
					player.tongue.ShortDescription() + ", but thankfully it doesn't seem like you've gashed them that badly. You're definitely glad you're immune to your own" +
					"venom, too, or you'd really be in trouble now. Before you can comprehend why they snapped out in the first place, they quickly retract inward again. They begin " +
					"to ache, as if they were loose or something. The feeling dies down, and they once again snap out, but they seem different this time. It takes a while, " +
					"but you eventually notice they emit a different type of venom. " + SafelyFormattedString.FormattedText("Your spider fangs have shifted into snake-like ones!",
					StringFormats.BOLD);
			}
			else if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified, into a more natural, human head!", StringFormats.BOLD) +
					" Strangely, though, it's not completely human - where you'd normally have a pair of canines, you instead have sharp fangs. " +
					"It seems " + SafelyFormattedString.FormattedText("You now have retractible, snake-like fangs, too!", StringFormats.BOLD);
			}
			//canines = the teeth, not an adjective describing the face type. slightly confusing out of context, lol.
			else if (previousFaceData.type == HUMAN)
			{
				typeChange = "";
			}
			else if (previousFaceData.isHumanoid)
			{
				typeChange = " Meanwhile, the rest of your face shifts slightly, and now looks completely human, minus your new venomous fangs, of course. ";
			}
			else
			{
				typeChange = " As the change progresses, your " + previousFaceData.LongDescription() + " reshapes. The sensation is far more pleasant than teeth cutting into gums, " +
					"and as the tingling transformation completes, " + SafelyFormattedString.FormattedText("you've gained with a normal-looking, human visage.", StringFormats.BOLD);
			}

			return "Without warning, you feel your canines jump " + (Measurement.UsesMetric ? "several centimeters" : "almost an inch") + " in size, clashing on your gums, " +
				"cutting yourself quite badly. As you attempt to find a new way to close your mouth without dislocating your jaw, you notice that they are dripping with a bitter, " +
				"khaki liquid. Instinctively, you know that " + SafelyFormattedString.FormattedText("you now have venomous fangs!", StringFormats.BOLD) + "Fortunately, you seem to " +
				"be immune to it, but you'd still prefer not to bite your tongue anytime soon. " + typeChange;
		}
		private static string SnakeRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Cat
		private static string CatGirlShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("cat-girl face", singleItemFormat);

		}
		private static string CatMorphShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("feline face", singleItemFormat);
		}

		private static string CatMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "Your face changes as your feline features become even more pronounced, and soon any humanoid traces are gone. ";
			}
			else
			{
				return "Your face changes as some humam traits start to poke through your feline features, until you resemble something like a cat-girl. ";
			}

		}
		private static string CatLongDesc(FaceData face, bool alternateFormat)
		{
			if (face.isFullMorph)
			{
				return Utils.AddArticleIf("partially-furred, cat-girl face", alternateFormat);
			}
			else
			{
				return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " feline face";

			}

		}
		private static string CatPlayerStr(Face face, PlayerBase player)
		{
			//deal with cat-morph first.
			if (face.isFullMorph)
			{
				if (EpidermalData.MixedFurColors(face.primary, face.secondary))
				{
					return "You have a cat-like face, complete with moist nose and whiskers. Most of your face is covered in " + face.primary.LongDescription() +
						", though there is also some " + face.secondary.DescriptionWithColor() + " mixed in. You have " + face.facialSkin.LongDescription() + " undeneath.";
				}
				else
				{
					return "You have a cat-like face, complete with moist nose and whiskers. Your face is covered in " + face.primary.LongDescription() +
						", and you have " + face.facialSkin.LongDescription() + " undeneath.";
				}
			}
			//cat-girl/guy.
			else
			{
				return "Your face is mostly human in appearance, though you do show some feline features, most notably your teeth.";
			}

		}
		//isn't a cat-girl tf, so i'm making one.
		private static string CatTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				if (!player.face.isHumanoid)
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural, human head!", StringFormats.BOLD) +
						" That's not all, however: patches of fur grow in, and your features start to take on some feline traits. " +
						SafelyFormattedString.FormattedText("Your facial features are now a hybrid between human and feline, like those of a cat-girl!", StringFormats.BOLD);
				}
				else
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural state, and has taken on a feline appearance!", StringFormats.BOLD);
				}
			}
			//other, humanoid to cat-girl (also humanoid) gets unique text.
			else if (previousFaceData.isHumanoid && !player.face.isFullMorph)
			{
				return "A slightly unpleasant sensation moves through your face, and you can feel it shifting slightly. Finding something reflective to look at what has changed, " +
					"you notice you have some short fur around your features, and your nose has flattened slightly, and short whiskers have come in just below it. " +
					SafelyFormattedString.FormattedText("You now have a hybrid between human and feline features, like a cat-girl!", StringFormats.BOLD);
			}
			//cat-girl
			else if (!player.face.isFullMorph)
			{
				return "Mind-numbing pain courses through you as you feel your facial bones rearranging. You clutch at your face in agony as your skin crawls and shifts, " +
					"and you have the vague impression it is roughly humanoid now. Bringing your " + player.hands.HandText() + " to your face, however, reveals that not to be the case." +
					" While it's still roughly human in shape, the flatter nose, fur, and whiskers tell you " +
					SafelyFormattedString.FormattedText("Your facial features are now a hybrid between human and feline, like those of a cat-girl!", StringFormats.BOLD);
			}
			//cat morph
			else
			{
				return "Mind-numbing pain courses through you as you feel your facial bones rearranging. You clutch at your face in agony as your skin crawls and shifts, " +
					"your visage reshaping to replace your facial characteristics with those of a feline along with a muzzle, a cute cat-nose and whiskers." + Environment.NewLine +
					SafelyFormattedString.FormattedText("You now have a cat-face!", StringFormats.BOLD);
			}
		}
		private static string CatRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Lizard
		private static string LizardShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("reptilian face", singleItemFormat);
		}
		private static string LizardLongDesc(FaceData face, bool alternateFormat)
		{
			return face.primaryEpidermis.LongAdjectiveDescription() + " reptilian face";
		}
		private static string LizardPlayerStr(Face face, PlayerBase player)
		{
			string intro = "Your face is that of a lizard, complete with a toothy maw and pointed snout. Reflective " + face.primary.DescriptionWithColor();

			if (!face.secondary.isEmpty)
			{
				return intro + "everything but your lower jaw, which has " + face.secondary.LongDescription() + ". Together, they give your quite the fearsome look.";
			}
			else
			{
				return intro + "complete the look, making you look quite fearsome.";
			}
		}
		private static string LizardTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					"it has solidified into a more natural, humanoid state. Scales begin growing across the surface, doubtlessly giving you a fearsome look. " +
					SafelyFormattedString.FormattedText("Your face is now distinctly reptilian!", StringFormats.BOLD);
			}
			else
			{
				return "Terrible agony wracks your " + previousFaceData.LongDescription() + " as bones crack and shift. Your jawbone rearranges while your cranium shortens. " +
					"The changes seem to last forever; once they've finished, no time seems to have passed. Your fingers brush against your toothy snout as you get used to your new face. " +
					"It seems " + SafelyFormattedString.FormattedText("you have a toothy, reptilian visage now!", StringFormats.BOLD);
			}
		}
		private static string LizardRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Bunny
		private static string BunnyFirstLevelShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("bunny-like teeth", singleItemFormat);
		}
		private static string BunnySecondLevelShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("bunny face", singleItemFormat);
		}
		private static string BunnyMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "Your face feels weird, undoubtedly the result of some change. While you suspect it's your bunny-like teeth, a quick check shows they remained the same. Further investigation reveals"
					+ "everything BUT your teeth changed, as your face is now even more bunny-like, and covered in short, fuzzy hairs, and you can't help but rub the new, soft surface. ";
			}
			else
			{
				return "Your face feels weird, and knowing what that likely means, you find a shiny surface to check your reflection. The fuzzy hair covering your face recedes until your face is roughly human again." +
					" Unfortunately, you still have your bunny-like teeth. Bummer. ";
			}
		}
		private static string BunnyLongDesc(FaceData face, bool alternateFormat)
		{
			if (!face.isFullMorph)
			{
				return (alternateFormat ? "a " : "") + "bunny-like, humanoid face with buckteeth";
			}
			else
			{
				return (alternateFormat ? "a " : "") + "bunny face with buckteeth and a twitching nose";
			}
		}
		private static string BunnyPlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face is generally human in shape and structure, though part of your " + face.facialSkin.LongDescription() + " is covered in " + face.primary.LongDescription() +
					". Your two front teeth have grown into a pair of incisors, giving you a bunny-like appearance.";
			}
			else
			{
				return "Your face is somewhat human in appearance, though more oblong and covered in " + face.primary.LongDescription() + " The constant twitches of your nose " +
				"and the length of your incisors gives your visage a hint of bunny-like cuteness.";
			}
		}
		private static string BunnyTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				if (!player.face.isHumanoid)
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural, human head!", StringFormats.BOLD) +
						" That's not all, however: patches of fur grow in around your facial features, and your two front teeth lengthen into a pair of incisors." +
						SafelyFormattedString.FormattedText("You now have a hybrid between a human and a bunny's features!", StringFormats.BOLD);
				}
				else
				{
					return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural state, and has taken a bunny-like appearance!", StringFormats.BOLD) +
						" Specifically, your nose twitches nearly constantly, and your two front teeth have grown longer, into a pair of incisors.";
				}
			}

			else if (!player.face.isFullMorph)
			{
				if (previousFaceData.isHumanoid)
				{
					//humanoid face.
					return "You catch your nose twitching on its own at the bottom of your vision, but as soon as you focus on it, it stops. A moment later, some of your teeth tingle " +
						"and brush past your lips, exposing a white pair of buckteeth! "
						+ SafelyFormattedString.FormattedText("Your face has taken on some rabbit-like characteristics!", StringFormats.BOLD);
				}
				//full tf
				else
				{
					//Crazy furry TF shit
					return "You grunt as your " + previousFaceData.LongDescription() + " twists and reforms. Even your teeth ache as their positions are rearranged to match some new, " +
						"undetermined order. When the process finishes, " + SafelyFormattedString.FormattedText("you're left with a perfectly human looking face, " +
						"save for your constantly twitching nose and prominent buck-teeth!", StringFormats.BOLD);
				}
			}
			else
			{
				if (previousFaceData.isHumanoid)
				{
					//humanoid face.
					return "You catch your nose twitching on its own at the bottom of your vision, but as soon as you focus on it, it stops, at least for the moment. Just as soon as " +
						"you look away, however, it starts up again, and this time it doesn't seem to be slowing down. A pair of whiskers grow underneath it, and your teeth tingle. " +
						"Your two front teeth brush past your lips, lengthening into a pair of buckteeth!" +
						SafelyFormattedString.FormattedText("Your face now appears distinctly bunny-like!", StringFormats.BOLD);
				}
				//full tf
				else
				{
					//Crazy furry TF shit
					return "You grunt as your " + previousFaceData.LongDescription() + " twists and reforms. Even your teeth ache as their positions are rearranged to match some new, " +
						"undetermined order. When the process finishes, " + SafelyFormattedString.FormattedText("you're left with a furry, oblong, distinctly bunny-like face, " +
						"with a constantly twitching nose and prominent buck-teeth!", StringFormats.BOLD);
				}
			}
		}
		private static string BunnyRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Kangaroo
		private static string KangarooShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("kangaroo face", singleItemFormat);
		}
		private static string KangarooLongDesc(FaceData face, bool alternateFormat)
		{
			return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + " bunny face";
		}
		private static string KangarooPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is covered with " + face.primary.LongDescription() + " and shaped like that of a kangaroo - somewhat rabbit-like except " +
				"for the extreme length of your odd visage.";
		}
		private static string KangarooTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			//Crikey is now silly mode exclusive. if you want it to not be, feel free to alter this.
			string tfText = SafelyFormattedString.FormattedText("You now have a kangaroo face!" + (SillyModeSettings.isEnabled ? " Crikey!" : ""), StringFormats.BOLD);
			if (previousFaceData.type == GOO)
			{
				return "Your gelatenous face begins to tingle, and begins solidifying. As it does, your nose extends outward, forming into a muzzle similar to a stretched-out rabbit's. "
					+ "The process finishes as " + player.face.primary.LongDescription() + " grows along your face, complete whiskers. just above your mout" + tfText;
			}
			//gain roo face from humanoid.
			else if (previousFaceData.isHumanoid)
			{
				return "The base of your nose suddenly hurts, as though someone were pinching and pulling at it. As you shut your eyes against the pain and bring your hands to your face, " +
					"you can feel your nose and palate shifting and elongating. This continues for about twenty seconds as you stand there, quaking. When the pain subsides, " +
					"you run your hands all over your face; what you feel is a long muzzle sticking out, whiskered at the end and with a cleft lip under a pair of flat nostrils. " +
					"You open your eyes and receive confirmation. " + tfText;
			}
			//gain roo face from other snout:
			else
			{
				return "Your nose tingles. As you focus your eyes toward the end of it, it twitches and shifts into a muzzle similar to a stretched-out rabbit's, " +
					"complete with harelip and whiskers. " + tfText;
			}
		}
		private static string KangarooRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Spider
		private static string SpiderShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("spider fangs", singleItemFormat);
		}
		private static string SpiderLongDesc(FaceData face, bool alternateFormat)
		{
			return "humanoid face with retractible, spider-like fangs";
		}
		private static string SpiderPlayerStr(Face face, PlayerBase player)
		{
			return "Your face appears human, though your mouth shows hints that that is not exactly the case. In place of what would normally be canine teeth, " +
				"you have a set of retractable, needle-like fangs, ready to dispense their venom.";
		}
		//spider tf only occured when human face and normal body.
		//but it's virtually identical to snake fangs, so i'm just tweaking that to work here. feel free to change it.
		private static string SpiderTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == HUMAN)
			{
				return "Tension builds within your upper gum, just above your canines. You open your mouth and prod at the affected area, pricking your finger on the sharpening tooth. " +
					"It slides down while you're touching it, lengthening into a needle-like fang. You check the other side and confirm your suspicions. " +
					SafelyFormattedString.FormattedText("You now have a pair of pointy spider-fangs, complete with their own venom!", StringFormats.BOLD);
			}
			else if (previousFaceData.type == GOO)
			{
				return "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified, into a more natural, human head!", StringFormats.BOLD) +
					" Strangely, though, it's not completely human - where you'd normally have a pair of canines, you instead have sharp fangs, both dripping venom. " +
					"It seems " + SafelyFormattedString.FormattedText("You now have retractible spider fangs, too!", StringFormats.BOLD);
			}
			else if (previousFaceData.type == SNAKE)
			{
				return "Without warning, your venomous, snake-like fangs suddenly snap out, forcing your jaw open in the process. You have the faint taste of blood on your " +
					player.tongue.ShortDescription() + ", but thankfully it doesn't seem like you've gashed them that badly. You're definitely glad you're immune to your own" +
					"venom, too, or you'd really be in trouble now. Before you can comprehend why they snapped out in the first place, they quickly retract inward again. They begin " +
					"to ache, as if they were loose or something. The feeling dies down, and they once again snap out, but they seem different this time. It takes a while, " +
					"but you eventually notice they emit a different type of venom. " + SafelyFormattedString.FormattedText("Your snake-like fangs have shifted into spider ones!",
					StringFormats.BOLD);
			}
			else if (previousFaceData.isHumanoid)
			{
				return "An unpleasant sensation runs across your face, shifting to a dull pain that centers in your mouth, like you have a bad toothache. Your face seems to reshape " +
					"slightly, until you have what appears to be a normal face and mouth again. But your teeth and gums still ache faintly, particularly around your canines. " +
					"You open your mouth and prod at the affected area, pricking your finger on the sharpening tooth. It slides down while you're touching it, " +
					"lengthening into a needle-like fang. You check the other side and confirm your suspicions. " +
					SafelyFormattedString.FormattedText("You now have a pair of pointy spider-fangs, complete with their own venom!", StringFormats.BOLD);
			}
			else
			{
				return "Without warning, you feel your canines jump " + (Measurement.UsesMetric ? "several centimeters" : "almost an inch") + " in size, clashing on your gums, " +
				"cutting yourself quite badly. You attempt to close your jaw, paying carefuly attention to your newly elongated canines, but find yourself unable to. " +
				"Bringing a hand to your mouth, you notice your canines are long and curved now, and are dripping some sort of liquid. It seems to be the source of your sudden oral" +
				" paralysis. With a start, you realize " + SafelyFormattedString.FormattedText("you now have venomous spider fangs!", StringFormats.BOLD) +
				" Panic runs through you as you realize the venom is now in your blood, but it's quickly dispelled when your fangs retract inward and you can close your mouth freely " +
				"again. It seems you're immune to your own venom, thankfully. As you regain control of the remaining facial muscles, they begin to move erratically. it seems the rest " +
				"of your face is also shifting to match your newly adjusted mouth. Checking yourself out in a nearby pool of water, you notice " +
				SafelyFormattedString.FormattedText("you've gained with a normal-looking, human visage, as well!", StringFormats.BOLD);
			}
		}
		private static string SpiderRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Fox/Kitsune
		private static string KitsuneShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("kitsune face", singleItemFormat);
		}
		private static string FoxShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("fox face", singleItemFormat);
		}
		private static string FoxMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "Your kitsune face starts becoming, well, foxy. As if to spite you for the joke, your face shifts past what could pass for humanoid, becoming that of a full fox-morph. ";
			}
			else
			{
				return "Your face starts to soften, and soon the telltale facial features of a kitsune poke free from your fox-like visage. ";
			}
		}
		private static string FoxLongDesc(FaceData face, bool alternateFormat)
		{
			if (!face.isFullMorph)
			{
				return (alternateFormat ? "an " : "") + "alluring, partially-furred, kitsune-hybrid face";
			}
			else
			{
				return face.primaryEpidermis.LongAdjectiveDescription(alternateFormat) + "vulpine face";
			}

		}
		private static string FoxPlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face appears human, though your features are sharper and more playful - you'd almost say it makes you look mischievous. Short "
					+ face.primary.DescriptionWithColor() + " pokes through your skin in various places, completing the kitsune look.";
			}
			else
			{
				string furStr;
				if (EpidermalData.MixedFurColors(face.primary, face.secondary))
				{
					furStr = "Most of your face is covered in " + face.primary.LongDescription() + ", though some is covered in " + face.secondary.LongDescription() +
						", particularly along your lower jaw.";
				}
				else
				{
					furStr = "Your coat of " + face.primary.LongDescription() + " decorates your face and muzzle";
				}
				return "You have a tapered, shrewd-looking vulpine face with a speckling of downward-curved whiskers just behind the nose." + furStr;
			}
		}
		//ngl the silly mode phrase is lost on me, but i'll keep it because why not.
		private static string FoxTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			string silly = SillyModeSettings.isEnabled ? " And they called you crazy..." : "";

			if (!player.face.isFullMorph)
			{
				string intro;
				if (previousFaceData.type == GOO)
				{
					intro = "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
					SafelyFormattedString.FormattedText("it has solidified into a more natural, human head! ", StringFormats.BOLD);
				}
				else if (previousFaceData.type != HUMAN)
				{

					intro = "Your face aches, though you're not entirely sure of the cause. Suddenly, it shifts, rearranging slightly until all of its non-human features " +
						"are gone, leaving you with a normal, human face. ";
				}

				string mischiefText = player.corruption < 30 ? "Where did that come from? " : "";

				string furText = previousFaceData.type != HUMAN ? "in around your now fully human features" : "around your facial features";


				return "Small patches of fur start to grow " + furText + ", and you look for a pool of water to see exactly what kind of visage you now have. " +
					"Checking your reflection, you note just how alluring your new look is. A mischevious smile crosses your face; you're fairly confident you could " +
					"prank someone and get away with it with little more than a wink of an eye. " + mischiefText + "As you push the thought from your mind, it dawns on you - " +
					SafelyFormattedString.FormattedText("You now have the face of a kitsune!", StringFormats.BOLD);
			}
			else
			{
				if (previousFaceData.isHumanoid || previousFaceData.type == GOO)
				{
					string intro = "";
					if (previousFaceData.type == GOO)
					{
						intro = "Sensation floods your gelatenous head - it seems to be reshaping! Placing your " + player.hands.HandText() + " to your head, you notice " +
						SafelyFormattedString.FormattedText("it has solidified into a more natural, human head! ", StringFormats.BOLD);
					}

					return intro + "Your nose feels strange, as if someone is pinching it. You clap your " + player.hands.HandText() + " to it, and watch, dumbfounded, as your nose " +
						"enlongates, pushes them to the side slightly. You feel your bones shifting beneath to match your new nose, which finally stops reforming when it has taken " +
						"a clearly defined, tapered shape, now visible even without the aid of a mirror. Fur grows in around your face, completeing the look. " +
						SafelyFormattedString.FormattedText("You now have a distinctly vulpine face!", StringFormats.BOLD) + silly;
				}
				else if (previousFaceData.hasMuzzle)
				{

					return "A series of violent sneezes overcomes you, as if some unpleasant smell or senation has found its way into your muzzle. You clasp your " +
						player.hands.HandText() + " to it, hoping to stifle the sneezing fit you find yourself under. As you do, you feel your muzzle shifting, and try to open your " +
						"eyes between sneezes to see exactly what's happening. As they die down, you notice " +
						SafelyFormattedString.FormattedText("you now have a vulpine muzzle, and no doubt the rest of your face has shifted as well!", StringFormats.BOLD) +
						"You confirm your suspicions in a nearby puddle; your reflection now has " + player.face.primary.LongDescription() + " and has sharper features." + silly;
				}
				else
				{
					return "Your face pinches and you clap your " + player.hands.HandText() + "  to it. Within seconds, your nose is poking through those " + player.hands.HandText() +
						", pushing them slightly to the side as new flesh and bone build and shift behind it, until it stops in a clearly defined, tapered, and familiar point " +
						"you can see even without the aid of a mirror. " +
						SafelyFormattedString.FormattedText("Looks like you now have a vulpine muzzle, and no doubt the rest of your face has shifted as well!", StringFormats.BOLD) +
						"You confirm your suspicions in a nearby puddle; your reflection now has " + player.face.primary.LongDescription() + " and has sharp, fox-like features." +
						silly;
				}
			}
		}
		private static string FoxRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Dragon
		private static string DragonShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("reptilian face", singleItemFormat);
		}
		private static string DragonLongDesc(FaceData face, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "narrow" + face.primaryEpidermis.AdjectiveDescription() + ", draconic face";
		}
		private static string DragonPlayerStr(Face face, PlayerBase player)
		{
			string skinStr = face.secondary.isEmpty
				? "decorated by " + face.primary.LongDescription()
				: "mostly decorated by " + face.primary.LongDescription() + ", but with " + face.secondary.LongDescription() + " along the lower jaw.";

			return "Your face is a narrow, reptilian muzzle. It looks like a predatory lizard's, at first glance, but with an unusual array of spikes along the under-jaw. " +
				"It gives you a regal but fierce visage. Opening your mouth reveals several rows of dagger-like sharp teeth. The fearsome visage is" + skinStr;
		}
		private static string DragonTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			if (previousFaceData.type == GOO)
			{
				return "An unpleasant, almost burning sensation rips through your gooey head. You can feel your face solidifying from the inside out, first forming bones, " +
					"then muscle and tissue, and finally a few layers of dermal skin to cover it. The remaining unaltered goo runs down your face, pooling in small points " +
					"along your rapidly fornming jawline. They begin to solidify, forming bony spurs, then finalizing into a series of spike. Your mouth and nose form next, " +
					"extending outward into a distinct muzzle shape. Your teeth form shortly afterward, and your front row elongate until yout have a row of narrow, sharp fangs. " +
					"As the process finishes, you look for a convenient puddle to examine your changed appearance. Gone is your gooey visage; " +
					SafelyFormattedString.FormattedText("You now have a distinctly draconic face, complete with scales, a reptilian muzzle, and spikes!", StringFormats.BOLD);
			}
			//identical, but without the fangs and the spurs on the lower jaw.
			else if (previousFaceData.type == LIZARD)
			{
				return "Your jaws ache, and you can't help but rub them to try and lessen the pain. You soon feel a series of small growths pushing against your " +
					player.hands.HandText() + ", along your lower jawline. As they finalize into something resembling a row of spikes, you notice your jaws are being pushed apart " +
					"slightly; it quickly becomes evident why when you notice your front row of teeth are now sharp, narrow fangs. Wondering if anything else has changed, you quickly" +
					"search for something reflective, and find a convenient puddle nearby. "
					+ SafelyFormattedString.FormattedText("Your reptilian face has gained some draconic traits!", StringFormats.BOLD);
			}
			else if (previousFaceData.hasMuzzle)
			{
				return "Your muzzle is suddenly wrought with an unexpected pain, and you bring your " + player.hands.HandText() + "to it. You quickly notice " +
					previousFaceData.primaryEpidermis.ShortDescription(out bool isPlural) + "in them, no doubt forcibly shed by the scales you can feel forming beneath " +
					(isPlural ? "them" : "it") + ". The pain spreads to your jaws, and you have little doubt of the cause when your teeth are forced from your gums, becoming a row " +
					"of sharp, narrow fangs. Soon after, a set of small growths begin forcing themselves from your lower jawline, settling into a row of spikes. " +
					"With this final change, the pain finally dies down, and you look for a convenient puddle to examine your changed appearance. " +
					"Your now have a scaly reptilian muzzle, with small barbs on the underside of the jaw. " +
					SafelyFormattedString.FormattedText("You now have a dragon's face!", StringFormats.BOLD);
			}
			else
			{
				return "You scream as your face is suddenly twisted; your facial bones begin rearranging themselves under your skin, restructuring into a long, narrow muzzle. " +
					"Spikes of agony rip through your jaws as your teeth are brutally forced from your gums, giving you new rows of fangs - long, narrow and sharp. " +
					"Your jawline begins to sprout strange growths; small spikes grow along the underside of your muzzle, giving you an increasingly inhuman visage." +
					"Finally, the pain dies down, and you look for a convenient puddle to examine your changed appearance. Your now have a scaly reptilian muzzle, " +
					"with small barbs on the underside of the jaw. " + SafelyFormattedString.FormattedText("You now have a dragon's face!", StringFormats.BOLD);
			}
		}
		private static string DragonRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Raccoon
		private static string RaccoonMaskShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("raccoon mask", singleItemFormat);
		}
		private static string RaccoonFaceShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("raccoon face", singleItemFormat);
		}
		private static string RaccoonMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "The mask-like raccoon features around your eyes expand until they cover the rest of your face, which now resembles a full raccoon-morph. ";
			}
			else
			{
				return "The raccoon-like features covering your face start to fall away, leaving a mostly-human face, however, a layer around your eyes remains. ";
			}
		}
		private static string RaccoonLongDesc(FaceData face, bool alternateFormat)
		{
			if (!face.isFullMorph)
			{
				return (alternateFormat ? "a " : "") + "humanoid face with " + face.primaryEpidermis.LongAdjectiveDescription(true) + "raccoon-mask";
			}
			else
			{
				return "raccoon-patterned, " + face.primaryEpidermis.JustColor(alternateFormat) + "and " + face.secondaryEpidermis.JustColor() + " furry face";
			}
		}
		private static string RaccoonPlayerStr(Face face, PlayerBase player)
		{
			bool colorsClose = CoC_Colors.CoCColors.WeightedColorComparePercent(face.primary.fur.primaryColor.rgbValue, face.secondary.fur.primaryColor.rgbValue) < 0.2f;
			if (!face.isFullMorph)
			{
				string maskStr = colorsClose
					? "barely differs from the rest of your face, giving you a definitive, if slight, raccoon mask."
					: "differs from the rest of your face, giving you a distinctly sly-looking raccoon mask.";//you have no idea how badly i wanted to write Sly-(cooper)-looking.

				return "Your face is human in shape and structure, but is covered in short " + face.primary.LongDescription() + ". The fur around your eyes" + maskStr;
			}
			else
			{
				return " You have a triangular raccoon face, replete with sensitive whiskers and a little black nose; a mask of " + face.secondary.JustColor() +
					" shades the space around your eyes, set apart from the " + face.primary.DescriptionWithColor() + "covering the rest of your face by a band of white.";
			}
		}
		private static string RaccoonTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Mouse
		private static string MouseTeethShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("mouse-like teeth", singleItemFormat);
		}
		private static string MouseFaceShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("mouse face", singleItemFormat);
		}
		//shamelessly pulled from bunny.
		private static string MouseMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "Your face feels weird, undoubtedly the result of some change. You hope it's your bunny-like teeth. "
					+ "Unfortunately, they remain unchanged, and they're about the only part of your face that remains untouched. "
					+ "The rest of your face has grown short, fuzzy hairs, shifting your face to full mouse-morph. Still, it's not all bad, as you find yourself rubbing the new, fuzzy surface. ";
			}
			else
			{
				return "Your face feels weird, and knowing what that likely means, you find a shiny surface to check your reflection. The fuzzy hair covering your face recedes until your face is roughly human again." +
					" Unfortunately, you still have your mouse-like teeth. Bummer. ";
			}
		}
		private static string MouseLongDesc(FaceData face, bool alternateFormat)
		{
			if (!face.isFullMorph)
			{
				return (alternateFormat ? "a " : "") + "mouse-like, humanoid face with buckteeth";
			}
			else
			{
				return (alternateFormat ? "a " : "") + "mouse face with buckteeth, whiskers, and a small pink nose";
			}
		}
		private static string MousePlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face is generally human in shape and structure, though " + face.primary.LongDescription() + " covers your " + face.facialSkin.LongDescription() +
					". You also have a pair of buckteeth that would be more at home on a mouse.";
			}
			else
			{
				return "You have a snubby, tapered mouse's face, with whiskers, a little pink nose, and " + face.primary.LongDescription() + " covering your "
					+ face.facialSkin.LongDescription() + ". Two large incisors complete it.";
			}
		}
		private static string MouseTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Ferret
		private static string FerretMaskShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("ferret mask", singleItemFormat);

		}
		private static string FerretFaceShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("ferret face", singleItemFormat);
		}

		//shamelessly copied from raccoon.
		private static string FerretMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "The mask-like ferret features around your eyes expand until they cover the rest of your face, which now resembles a full ferret-morph. ";
			}
			else
			{
				return "The raccoon-like features covering your face start to fall away, leaving a mostly-human face. "
					+ "A thin layer remains here and there, notably around your eyes, giving the appearance of you wearing a ferret mask. ";
			}
		}
		private static string FerretLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face is an adorable cross between human and ferret features. A layer of short, " + face.primary.LongDescription() + " covers the " +
					face.facialSkin.DescriptionWithTexture() + " underneath, and the " + face.secondary.ShortDescription() + " around your eyes " +
					"contrasts with it, giving you a cute little ferret mask.";
			}
			else
			{
				return "Your face has mustelid muzzle, with a ferret-like visage and a cute pink nose. It's covered by a layer of " + face.primary.LongDescription() +
					", with patches of white on your muzzle and cheeks. A noticeable mask of " + face.secondary.DescriptionWithColor() + " is shaped around your eyes.";
			}
		}
		private static string FerretTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Pig
		private static string PigShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("pig-like face", singleItemFormat);
		}
		private static string BoarShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("boar snout", singleItemFormat);
		}
		private static string PigMorphText(bool isStrengthening)
		{
			if (isStrengthening)
			{
				return "A strange, painful feeling hits your nose. Without warning, it elongates, pulling part of your mouth with it. "
					+ "Once your bones settle again, you notice you now have a boar-like snout, complete with a pair of tusk-like teeth out of your bottom jaw. "
					+ "Well, that's new. ";
			}
			else
			{
				return "Pain lances across your snout, and before long, it's shrinking into your face. While you don't black out, it feels like you got punched square in the nose. "
					+ "After it ends, you note that while your snout isn't completely gone, it's far more humanoid, now resembling that of a pig-morph. You've also lost your boar-like tusks. ";
			}
		}
		private static string PigLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigPlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face is like that of a pig, with " + face.facialSkin.JustColor() + " skin, complete with a snout that is always wiggling.";
			}
			else
			{
				return "Your face is like that of a boar: Elongated, with " + face.primary.LongDescription() + " covering your " + face.facialSkin.DescriptionWithColor() + " underneath. " +
					"Tusks sprouting from your lower jaw complete the look, along a snout that is always wiggling.";
			}
		}
		private static string PigTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Rhino
		private static string RhinoShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("rhino face", singleItemFormat);
		}
		private static string RhinoLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is like that of a rhino: " + face.primary.JustColor() + ", with a long muzzle and a horn on your nose.";
		}
		private static string RhinoTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Echidna
		private static string EchidnaShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("echidna face", singleItemFormat);
		}
		private static string EchidnaLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(Face face, PlayerBase player)
		{
			return "Your odd visage consists of a long, thin echidna snout, covered in " + face.primary.LongDescription();
		}
		private static string EchidnaTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Deer
		private static string DeerShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("deer face", singleItemFormat);
		}
		private static string DeerLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerPlayerStr(Face face, PlayerBase player)
		{
			string intro = "Your face is like that of a deer, with a nose at the end of your muzzle.";
			if (EpidermalData.MixedFurColors(face.primary, face.secondary))
			{
				return intro + " It's covered in " + face.primary.LongDescription() + " on your upper jaw and head and " + face.secondary.LongDescription() + "on your lower jaw, " +
					"both hiding the " + face.facialSkin.LongDescription() + " underneath.";
			}
			else
			{

				return " " + GlobalStrings.CapitalizeFirstLetter(face.primary.LongDescription()) + "covers your face, hiding the " + face.facialSkin.LongDescription() + " underneath.";
			}
		}
		private static string DeerTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Wolf
		private static string WolfShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("wolf face", singleItemFormat);
		}
		private static string WolfLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(Face face, PlayerBase player)
		{
			return "You have an angular wolf's face complete with a muzzle and black nose and covered in " + face.primary.LongDescription();
		}
		private static string WolfTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Cockatrice
		private static string CockatriceShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("cockatrice face", singleItemFormat);
		}
		private static string CockatriceLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(Face face, PlayerBase player)
		{
			return "You have a cockatrice's face, complete with " + face.primary.DescriptionWithColor() + " and " + face.secondary.DescriptionWithColor();
		}
		private static string CockatriceTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		//private static string BeakShortDesc()
		//{
		//	return "placeholder beak face"; // This is a placeholder for the next beaked face type, so feel free to refactor (rename)
		//}
		//private static string BeakLongDesc(FaceData face)
		//{
		//
		//}
		//private static string BeakPlayerStr(Face face, PlayerBase player)
		//{
		//	return ""
		//}
		//private static string BeakTransformStr(FaceData previousFaceData, PlayerBase player)
		//{
		//
		//}
		//private static string BeakRestoreStr(FaceData originalFaceData, PlayerBase player)
		//{
		//
		//}
		#region Red Panda
		private static string PandaShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("red panda face", singleItemFormat);
		}
		private static string PandaLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaPlayerStr(Face face, PlayerBase player)
		{
			return "Your face has a distinctive animalistic muzzle, proper from a red-panda, complete with a cute pink nose. A coat of soft, " + face.primary.DescriptionWithColor() +
				" covers your head, with patches of " + face.secondary.JustColor() + " on your muzzle, cheeks and eyebrows.";
		}
		private static string PandaTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			return GenericRestoreText(originalFaceData, player);
		}
		#endregion
		#region Goo
		private static string GooShortDesc(bool singleItemFormat)
		{
			return Utils.AddArticleIf("gooey, humanoid face", singleItemFormat);
		}
		private static string GooLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooPlayerStr(Face face, PlayerBase player)
		{
			return " Your face is human in shape and structure, but with one critical difference - it's made entirely out of goo. It's fully transparent, seemingly lacking any sort of " +
				"brain or skeleton, yet it retains its shape and you can still think and talk, though your voice warbles slightly, as if underwater.";
		}

		private static string GooTransformStr(FaceData previousFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooRestoreStr(FaceData originalFaceData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		//not part of face: the text that merges between face and neck or face and body.
		//if neck is not humanoid - the bridge text says: "The lowest parts of your face blend with the {epidermisType string} of your neck. Speaking of, "
		//else if body epidermis type does not match primary face epidermis type. "Your neck blends the {face.epidermisType} on your face with the {epidermisType string} of the rest of your body"
		private static string GenericRestoreText(FaceData originalFaceData, PlayerBase player)
		{
			return "A tingling sensation in your nose forces you to sneeze. The feeling soon overwhelms you, and a series of violent, painful sneezes force you to shut your eyes " +
				"and hold your neck to prevent whiplash. When the sensation finally passes, your entire face feels sore, particularly your nose. You rub your nose tenderly and discover" +
				" it has changed back into a more normal, human nose. Running your " + player.hands.ShortDescription() + "along your face, you notice it has similarly changed, leaving you " +
				"with " + SafelyFormattedString.FormattedText("a human looking face again!", StringFormats.BOLD);
		}
	}
}
