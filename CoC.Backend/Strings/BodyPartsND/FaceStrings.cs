﻿//FaceStrings.cs
//Description:
//Author: JustSomeGuy
//1/11/2019, 6:58 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Face
	{
		public static string Name()
		{
			return "Face";
		}
	}

	public partial class FaceType
	{
		private string FaceStr()
		{
			return "Face";
		}

		private string YourFaceStr()
		{
			return " your face";
		}


		private static string HumanShortDesc()
		{
			return " face";
		}
		private static string HumanLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanPlayerStr(Face face, PlayerBase player)
		{
			return " Your face is human in shape and structure, covered in  .";
		}
		private static string HumanTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HumanRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseShortDesc()
		{
			return "longe, equine muzzle";
		}
		private static string HorseLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorsePlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HorseRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogShortDesc()
		{
			return "canine muzzle";
		}
		private static string DogLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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

		private static string Cow_MinotaurLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Cow_MinotaurPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Cow_MinotaurTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string Cow_MinotaurRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkShortDesc()
		{
			return "shark teeth";
		}
		private static string SharkLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakeShortDesc()
		{
			return "snake fangs";
		}
		private static string SnakeLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakePlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakeTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SnakeRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string CatLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardShortDesc()
		{
			return "reptilian face";
		}
		private static string LizardLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

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
		private static string BunnyLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BunnyRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooShortDesc()
		{
			return "kangaroo face";
		}
		private static string KangarooLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderShortDesc()
		{
			return "spider fangs";
		}
		private static string SpiderLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SpiderRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string FoxLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonShortDesc()
		{
			return "reptilian face";
		}
		private static string DragonLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string RaccoonLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string MouseLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MousePlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string FerretLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

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
		private static string PigLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoShortDesc()
		{
			return "rhino face";
		}
		private static string RhinoLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaShortDesc()
		{
			return "echidna face";
		}
		private static string EchidnaLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerShortDesc()
		{
			return "deer face";
		}
		private static string DeerLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfShortDesc()
		{
			return "wolf face";
		}
		private static string WolfLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceShortDesc()
		{
			return "cockatrice face";
		}
		private static string CockatriceLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatricePlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeakShortDesc()
		{
			return "placeholder beak face"; // This is a placeholder for the next beaked face type, so feel free to refactor (rename)
		}
		private static string BeakLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeakPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeakTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BeakRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaShortDesc()
		{
			return "red panda face";
		}
		private static string PandaLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaPlayerStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PandaRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooShortDesc()
		{
			return "gooey, humanoid face";
		}

		private static string GooLongDesc(Face face)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooPlayerStr(Face face, PlayerBase player)
		{
			return " Your face is human in shape and structure, but with one critical difference - it's made entirely out of goo. It's fully transparent, seemingly lacking any sort of " +
				"brain or skeleton, yet it retains its shape and you can still think and talk, though your voice warbles slightly, as if underwater.";
		}

		private static string GooTransformStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooRestoreStr(Face face, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
