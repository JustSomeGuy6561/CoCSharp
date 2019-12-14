//FaceStrings.cs
//Description:
//Author: JustSomeGuy
//1/11/2019, 6:58 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Face
	{
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
		private static string HumanShortDesc()
		{
			return "face";
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
		private static string HumanTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Horse
		private static string HorseShortDesc()
		{
			return "horse-like face";
		}
		private static string HorseLongDesc(FaceData face, bool alternateForm)
		{
			return $"{(alternateForm ? "a " : "")}longe, equine muzzle covered in {face.primaryEpidermis.LongDescription()}";
		}
		private static string HorsePlayerStr(Face face, PlayerBase player)
		{
			return "Your face is almost entirely equine in appearance, even having a" + face.primary.LongDescription() +
				". Underneath the fur, you believe you have " + face.facialSkin.LongDescription();
		}
		private static string HorseTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Dog
		private static string DogShortDesc()
		{
			return "canine muzzle";
		}
		private static string DogLongDesc(FaceData face, bool alternateForm)
		{
			return $"{face.primaryEpidermis.LongAdjectiveDescription(alternateForm)} canine face";
		}
		private static string DogPlayerStr(Face face, PlayerBase player)
		{
			return "You have a dog's face, complete with wet nose and panting tongue.  You've got a" + face.primary.LongDescription() + ", hiding your " + face.facialSkin.LongDescription()
				+ " underneath your furry visage.";
		}
		private static string DogTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cow/Minotaur
		private static string CowShortDesc()
		{
			return "cow-like face";
		}
		private static string MinotaurShortDesc()
		{
			return "bovine snout";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Cow_MinotaurPlayerStr(Face face, PlayerBase player)
		{
			if (face.isFullMorph)
			{
				string noseRingStr = face.wearingCowNoseRing ? "particularly your nose, which is squared off and has a ring running through it" : "particularly a squared off wet nose";
				return "You have a face resembling that of a minotaur, with cow-like features, " + noseRingStr + ". Your " + face.facialSkin.DescriptionWithColor() +
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
		private static string Cow_MinotaurTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Cow_MinotaurRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Shark
		private static string SharkShortDesc()
		{
			return "shark teeth";
		}
		private static string SharkLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is human in shape and structure, covered in " + face.facialSkin.LongDescription() + ". A set of razor-sharp, retractable shark-teeth fill your mouth " +
				"and give your visage a slightly angular appearance.";
		}
		private static string SharkTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Snake
		//Snake? SNAKE?!? SNAAAAAAKE!?!?!?!?!
		private static string SnakeShortDesc()
		{
			return "snake fangs";
		}
		private static string SnakeLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakePlayerStr(Face face, PlayerBase player)
		{
			return "Your face is fairly human in shape, but is covered in " + face.primary.LongDescription() + ". In addition, a pair of fangs hang over your lower lip, " +
				"dripping with venom.";
		}
		private static string SnakeTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakeRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cat
		private static string CatGirlShortDesc()
		{
			return "cat-girl face";

		}
		private static string CatMorphShortDesc()
		{
			return "feline face";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string CatTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Lizard
		private static string LizardShortDesc()
		{
			return "reptilian face";
		}
		private static string LizardLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string LizardTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Bunny
		private static string BunnyFirstLevelShortDesc()
		{
			return "bunny-like teeth";
		}
		private static string BunnySecondLevelShortDesc()
		{
			return "bunny face";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyPlayerStr(Face face, PlayerBase player)
		{
			if (!face.isFullMorph)
			{
				return "Your face is generally human in shape and structure, though " + face.primary.LongDescription() + " covers your " + face.facialSkin.LongDescription() +
					". Your two front teeth have grown into a pair of incisors, giving you a bunny-like appearance.";
			}
			else
			{
				return "Your face is somewhat human in appearance, though more oblong and covered in " + face.primary.LongDescription() + " The constant twitches of your nose " +
				"and the length of your incisors gives your visage a hint of bunny - like cuteness.";
			}
		}
		private static string BunnyTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Kangaroo
		private static string KangarooShortDesc()
		{
			return "kangaroo face";
		}
		private static string KangarooLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is covered with " + face.primary.LongDescription() + " and shaped like that of a kangaroo - somewhat rabbit-like except " +
				"for the extreme length of your odd visage.";
		}
		private static string KangarooTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Spider
		private static string SpiderShortDesc()
		{
			return "spider fangs";
		}
		private static string SpiderLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is mostly human in appearance, though it is covered in " + face.primary.LongDescription() + ". A set of retractable, needle-like fangs " +
				"sit in place of your canines and are ready to dispense their venom.";
		}
		private static string SpiderTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Fox/Kitsune
		private static string KitsuneShortDesc()
		{
			return "kitsune face";
		}
		private static string FoxShortDesc()
		{
			return "fox face";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string FoxTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Dragon
		private static string DragonShortDesc()
		{
			return "reptilian face";
		}
		private static string DragonLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(Face face, PlayerBase player)
		{
			string skinStr = face.secondary.isEmpty
				? "decorated by " + face.primary.LongDescription()
				: "mostly decorated by " + face.primary.LongDescription() + ", but with " + face.secondary.LongDescription() + " along the lower jaw.";

			return "Your face is a narrow, reptilian muzzle.  It looks like a predatory lizard's, at first glance, but with an unusual array of spikes along the under-jaw. " +
				"It gives you a regal but fierce visage. Opening your mouth reveals several rows of dagger-like sharp teeth. The fearsome visage is" + skinStr;
		}
		private static string DragonTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Raccoon
		private static string RaccoonMaskShortDesc()
		{
			return "raccoon mask";
		}
		private static string RaccoonFaceShortDesc()
		{
			return "raccoon face";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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
				return "  You have a triangular raccoon face, replete with sensitive whiskers and a little black nose; a mask of " + face.secondary.JustColor() +
					" shades the space around your eyes, set apart from the " + face.primary.DescriptionWithColor() + "covering the rest of your face by a band of white.";
			}
		}
		private static string RaccoonTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Mouse
		private static string MouseTeethShortDesc()
		{
			return "mouse-like teeth";
		}
		private static string MouseFaceShortDesc()
		{
			return "mouse face";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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
		private static string MouseTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Ferret
		private static string FerretMaskShortDesc()
		{
			return "ferret mask";

		}
		private static string FerretFaceShortDesc()
		{
			return "ferret face";
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
				return "Your face is an adorable cross between human and ferret features. A layer of  short, " + face.primary.LongDescription() + " covers the " +
					face.facialSkin.DescriptionWithTexture() + " underneath, and the " + face.secondary.ShortDescription() + " around your eyes " +
					"contrasts with it, giving you a cute little ferret mask.";
			}
			else
			{
				return "Your face has mustelid muzzle, with a ferret-like visage and a cute pink nose. It's covered by a layer of " + face.primary.LongDescription() +
					", with patches of white on your muzzle and cheeks. A noticeable mask of " + face.secondary.DescriptionWithColor() + " is shaped around your eyes.";
			}
		}
		private static string FerretTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Pig
		private static string PigShortDesc()
		{
			return "pig-like face";
		}
		private static string BoarShortDesc()
		{
			return "boar snout";
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
		private static string PigTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Rhino
		private static string RhinoShortDesc()
		{
			return "rhino face";
		}
		private static string RhinoLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoPlayerStr(Face face, PlayerBase player)
		{
			return "Your face is like that of a rhino: " + face.primary.JustColor() + ", with a long muzzle and a horn on your nose.";
		}
		private static string RhinoTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Echidna
		private static string EchidnaShortDesc()
		{
			return "echidna face";
		}
		private static string EchidnaLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(Face face, PlayerBase player)
		{
			return "Your odd visage consists of a long, thin echidna snout, covered in " + face.primary.LongDescription();
		}
		private static string EchidnaTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Deer
		private static string DeerShortDesc()
		{
			return "deer face";
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
		private static string DeerTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Wolf
		private static string WolfShortDesc()
		{
			return "wolf face";
		}
		private static string WolfLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(Face face, PlayerBase player)
		{
			return "You have an angular wolf's face complete with a muzzle and black nose and covered in " + face.primary.LongDescription();
		}
		private static string WolfTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cockatrice
		private static string CockatriceShortDesc()
		{
			return "cockatrice face";
		}
		private static string CockatriceLongDesc(FaceData face, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(Face face, PlayerBase player)
		{
			return "You have a cockatrice's face, complete with " + face.primary.DescriptionWithColor() + " and " + face.secondary.DescriptionWithColor();
		}
		private static string CockatriceTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		//private static string BeakShortDesc()
		//{
		//	return "placeholder beak face"; // This is a placeholder for the next beaked face type, so feel free to refactor (rename)
		//}
		//private static string BeakLongDesc(FaceData face)
		//{
		//	throw new InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//private static string BeakPlayerStr(Face face, PlayerBase player)
		//{
		//	return ""
		//}
		//private static string BeakTransformStr(FaceData face, PlayerBase player)
		//{
		//	throw new InDevelopmentExceptionThatBreaksOnRelease();
		//}
		//private static string BeakRestoreStr(FaceData face, PlayerBase player)
		//{
		//	throw new InDevelopmentExceptionThatBreaksOnRelease();
		//}
		#region Red Panda
		private static string PandaShortDesc()
		{
			return "red panda face";
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
		private static string PandaTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Goo
		private static string GooShortDesc()
		{
			return "gooey, humanoid face";
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

		private static string GooTransformStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooRestoreStr(FaceData face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		//not part of face: the text that merges between face and neck or face and body.
		//if neck is not humanoid - the bridge text says: "The lowest parts of your face blend with the {epidermisType string} of your neck. Speaking of, "
		//else if body epidermis type does not match primary face epidermis type. "Your neck blends the {face.epidermisType} on your face with the {epidermisType string} of the rest of your body"
	}
}
