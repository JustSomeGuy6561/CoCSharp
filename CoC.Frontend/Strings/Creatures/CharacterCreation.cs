//CharacterCreation.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 8:41 PM
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Frontend.Creatures
{
	internal sealed partial class CharacterCreation
	{
		private string GenderQuestion()
		{
			if (hermUnlocked)
			{
				return "Congratulations, you've unlocked the hermaphrodite option! Are you a man, woman, or herm?";
			}
			else
			{
				return "Are you a man or a woman?";
			}
		}

		private string GenderQuestionWithDefault(Gender defaultGender)
		{
			StringBuilder sb = new StringBuilder(" By default, your character is a ");
			sb.Append(defaultGender.AsText());
			sb.Append(". If you want, you could change this now, to male or female. ");
			if (hermUnlocked)
			{
				sb.Append("You've also unlocked the hermaphrodite option.");
			}
			return sb.ToString();
		}

		private string GenderQuestion2()
		{
			return " What is your gender?";
		}

		private string SpecialText()
		{
			StringBuilder sb = new StringBuilder("While it's true everyone is unique in their own way, your unique traits are a bit more apparent. ");
			if (isLocked)
			{
				sb.Append("You find yourself unable to change these; they define you, for better or for worse, though you suppose that may change once you're through the portal. ");
			}
			else
			{
				sb.Append("While this means you don't have the same level of flexibility as others, there are still some things you can change about yourself. ");
			}
			return sb.ToString();
		}

		private string NotSpecialText()
		{
			return "Your name carries little significance beyond it being your name.";
		}

		private string BuildText(Gender gender, byte? build = null)
		{
			StringBuilder sb = new StringBuilder();
			switch (gender)
			{
				case Gender.GENDERLESS:
					sb.Append("You are a neuter. Your upbringing hasn't provided you any advantage in stats."); break;
				case Gender.FEMALE:
					sb.Append("You are a woman. Your upbringing has provided you an advantage in speed and intellect."); break;
				case Gender.MALE:
					sb.Append("You are a man. Your upbringing has provided you an advantage in strength and toughness."); break;
				case Gender.HERM:
				default:
					sb.Append("You are a hermaphrodite. Your upbringing has provided you an average in stats."); break;
			}
			sb.Append(Environment.NewLine + Environment.NewLine + "What type of build do you have?");
			return sb.ToString();
		}

		private readonly string[] MALE_BUILDS = { "Lean", "Average", "Thick", "Girly" };

		private readonly string[] FEMALE_BUILDS = { "Slender", "Average", "Curvy", "Tomboyish" };
		private string MaleBuild(int index)
		{
			return MALE_BUILDS[index];
		}

		private string FemaleBuild(int index)
		{
			return FEMALE_BUILDS[index];
		}

		private readonly Triple<string>[] HERM_BUILDS =
		{
			new Triple<string>("Fem. Slender", "Feminine build." + Environment.NewLine + Environment.NewLine + "Will make you a futanari.", "Feminine, Slender"), //0
			new Triple<string>("Fem. Average", "Feminine build." + Environment.NewLine + Environment.NewLine + "Will make you a futanari.", "Feminine, Average"), //1
			new Triple<string>("Fem. Curvy", "Feminine build." + Environment.NewLine + Environment.NewLine + "Will make you a futanari.", "Feminine, Curvy"), //2
			new Triple<string>("Fem. Tomboy", "Androgynous build." + Environment.NewLine + Environment.NewLine + "A bit feminine, but fit and slender.", "Feminine, Tomboyish"), //3
			new Triple<string>(GlobalStrings.DEFAULT(), DefaultBuildHint(), GlobalStrings.DEFAULT()), //4
			new Triple<string>("Mas. Lean", "Masculine build." + Environment.NewLine + Environment.NewLine + "Will make you a maleherm.", "Masculine, Lean"), //5
			new Triple<string>("Mas. Average", "Masculine build." + Environment.NewLine + Environment.NewLine + "Will make you a maleherm.", "Masculine, Average"), //6
			new Triple<string>("Mas. Thick", "Masculine build." + Environment.NewLine + Environment.NewLine + "Will make you a maleherm.", "Masculine, Thick"), //7
			new Triple<string>("Mas. Girly", "Androgynous build." + Environment.NewLine + Environment.NewLine + "A bit masculine, but soft and slender.", "Masculine, Girly") //8
		};

		private Triple<string> HermButtonData(int index)
		{
			return HERM_BUILDS[index];
		}

		private readonly Triple<string>[] GENDERLESS_BUILDS =
		{
			new Triple<string>("S. Feminine", "Will make you appear slightly feminine.", "Slightly Feminine"),
			new Triple<string>("S. Masculine", "Will make you appear slightly masculine.", "Slightly Masculine"),
			new Triple<string>("Androgynous", "Will make you appear truly androgynous.", "")
		};

		private Triple<string> GenderlessButtonData(int index)
		{
			return GENDERLESS_BUILDS[index];
		}

		private static string DefaultBuildHint()
		{
			return "Default build." + Environment.NewLine + Environment.NewLine + "Uses the default build for this character.";
		}

		private string ComplexionText()
		{
			return "What is your complexion?";
		}

		private string ComplexionStr()
		{
			return "Complexion";
		}

		private string ConfirmComplexionText(Tones choice)
		{
			return "You selected a " + choice.AsString() + " complexion." + Environment.NewLine + Environment.NewLine;
		}

		private string ChooseFurStr(bool isPrimary)
		{
			bool multiFur = creator.bodyType?.secondaryEpidermisType.usesFur == true && creator.bodyType?.epidermisType.usesFur == true;

			string whereFur;
			if (isPrimary && multiFur)
			{
				whereFur = "fur covering most of their body";
			}
			else if (isPrimary)
			{
				whereFur = "fur covering their body";
			}
			else //if (multiFur)
			{
				whereFur = "a secondary type of fur on parts of their body";
			}
			string backStr = "", returnStr = "";
			if (setPrimaryFur)
			{
				backStr = "Or, you can cancel these changes and go Back to change other fur options";
				if (hitCustomizationMenu)
				{
					returnStr = ", or Return to change the other customization options";
				}
			}

			return "Your character is special and has " + whereFur + ". You may either select one solid color for this fur, or a combination of two colors in a pattern." + backStr + returnStr;
		}

		private string SolidColorStr()
		{
			return "Single Color";
		}

		private string MultiColorStr()
		{
			return "MultiColored";
		}

		private string FurColorStr()
		{
			return "Fur Color(s)";
		}

		private string ChooseFurColorStr()
		{
			return "Choose a color from the drop down list. ";
		}

		private string SelectedColor(HairFurColors color, bool isPrimaryFur, bool isPrimaryColor, bool isMulticolored)
		{
			string currentColor, doNextAction, backStr = "", returnStr = "";
			currentColor = isMulticolored ? isPrimaryColor ? "first" : "second" : "";

			doNextAction = isMulticolored ? (isPrimaryColor ? "select your second color" : "select the pattern your fur colors will appear in") : (setPrimaryFur ? "return to the fur options" : "continue customizing your character");
			if (setPrimaryFur)
			{
				backStr = "Remember, at any time you can cancel these changes and go Back to customize other fur options";
				if (hitCustomizationMenu)
				{
					returnStr = ", or you can Return to set other customization options";
				}
			}

			return "You've selected " + color.AsString() + " as your " + currentColor + " fur color. Hit confirm to lock this color in and " + doNextAction + backStr + returnStr;
		}

		private string ChooseFurPatternStr()
		{
			return "what pattern would you like your " + furBuilderColorOne.AsString() + " and " + furBuilderColorTwo.AsString() + " fur to be in?";
		}

		private string FurColorFirstRunStr(FurColor created)
		{
			return "Your fur color is now " + created.AsString() + ". You may now choose to set additional hair options, such as your fur's texture, or continue on with the next customization";
		}

		private string FurOptionsStr()
		{
			return "Fur Options";
		}

		private string FurOptionsText()
		{
			StringBuilder sb = new StringBuilder("You may now choose to change your ");
			if (multiFurred)
			{
				sb.Append("main fur's color or texture, or your other fur's color or texture. ");
			}
			else
			{
				sb.Append("body's fur color or texture. ");
			}
			sb.Append(Environment.NewLine + Environment.NewLine + "You may also choose to ");
			if (hitCustomizationMenu)
			{
				sb.Append("return to set other customization options.");
			}
			else
			{
				sb.Append("finish changing your fur, and move on to your hair. You may return to change your fur settings later, if you'd like.");
			}
			return sb.ToString();
		}

		private string FurStr(bool primary)
		{
			if (primary)
			{
				return multiFurred ? "Main Color" : "Fur Color";
			}
			return "Other Color";
		}

		private string FurTextureStr(bool primary)
		{
			if (primary)
			{
				return multiFurred ? "Main Texture" : "Fur Texture";
			}
			return "OtherTexture";
		}

		private string ChooseTextureText(bool isPrimary)
		{
			return "How would you describe the look and feel of your " + (isPrimary ? (multiFurred ? "main fur?" : "fur?") : "other fur?");
		}

		private string HairColorStr()
		{
			return "Hair Color";
		}

		private string HighlightColorStr()
		{
			return "Highlights";
		}

		private string HairText()
		{
			return "What color is your hair?";
		}

		private string HighlightText()
		{
			return "What color highlights do you have in your hair?";
		}

		private string ConfirmHairFirstRun(HairFurColors choice)
		{
			return "You have " + choice.AsString() + " hair. " + Environment.NewLine + Environment.NewLine +
				"You may now choose to set additional hair options, or continue on with other customization options";
		}

		private string HairOptionStr()
		{
			return "Hair Options";
		}

		private string HairOptionsText(bool isFromCustomization)
		{
			StringBuilder sb = new StringBuilder("You may now choose to change the hair's primary color, highlight color, length, or style.");
			if (creator.hairType != null && creator.hairType != HairType.NORMAL && creator.hairType != HairType.NO_HAIR)
			{
				sb.Append(" Your character has non-human hair. You cannot change this now, but you may be able to do so during gameplay, along with the other hair attributes listed.");
			}
			else
			{
				sb.Append(" Your hair will grow over time, and certain items or interactions can change your hair during gameplay.");
			}
			sb.Append(Environment.NewLine + Environment.NewLine + "You may also choose to ");
			sb.Append(isFromCustomization ? "return to" : "continue on to");
			sb.Append(" other customization options");
			return sb.ToString();
		}

		private string HairLengthStr()
		{
			return "Hair Length";
		}

		private string ChooseHairLengthStr()
		{
			return "Enter the length of your hair in " + (Measurement.UsesMetric ? "centimeters" : "inches") + " and press Confirm when done.";
		}

		private string HairTooLongStr(float inputValue)
		{
			return "The value you entered, " + inputValue + ",  is taller than you! Try again, using a smaller value." + Environment.NewLine;
		}

		private string NegativeNumberHairStr(float inputValue)
		{
			return "The value you entered, " + inputValue + ", is negative, and last we checked, that's not possible. Try again, with a valid length." + Environment.NewLine;
		}

		private string NotANumberInput(string input)
		{
			//make sure to parse null string. should never happen, but I don't actually control the GUI code, do I?
			return "The text you entered, " + (input ?? "") + ", could not be parsed as a number. Try again, with a number." + Environment.NewLine;
		}

		private string HairStyleStr()
		{
			return "Hair Style";
		}

		private string ChooseHairStyleStr()
		{
			return "What style is your hair?";
		}
		private string GenericCustomizationText()
		{
			StringBuilder sb = new StringBuilder("You can finalize your appearance customization before you proceed to perk selection." +
				" You will be able to alter your appearance through the usage of certain items." + Environment.NewLine + Environment.NewLine +
				"Height:" + Measurement.ToNearestLargeAndSmallUnit(creator.heightInInches, true) + Environment.NewLine +
				"Skin tone: " + creator.complexion.AsString() + Environment.NewLine);

			//Hair is complicated. basically, we build the string in parts, and we can tell if we've added to the string at any point based on if
			//the length of the builder has changed.
			//It'll read Hair: <Length> <Style> <Color> <HairType> <with Highlights>, with each <Data> filled if available.
			sb.Append("Hair: ");

			if (creator.hairLength != null && creator.hairLength != 0)
			{
				sb.Append(Measurement.ToNearestSmallUnit((float)creator.hairLength, true, false) + " ");
			}

			if (creator.hairStyle != HairStyle.NO_STYLE)
			{
				sb.Append(creator.hairStyle.AsString());
			}

			sb.Append(creator.hairColor.AsString());
			HairType hairType = creator.hairType ?? HairType.NORMAL; //not changeable in customization menu.
			sb.Append(hairType.ShortDescription());
			if (!HairFurColors.IsNullOrEmpty(creator.hairHighlightColor))
			{
				sb.Append(" with " + creator.hairHighlightColor.AsString() + " highlights");
			}
			sb.Append(Environment.NewLine);

#warning Add Beard style above when implemented.
			//if (canGrowBeard /* && creator.beardStyle != null*/)
			//{
			//	//	sb.Append("Beard Style: " + creator.beardStyle.AsString() + Environment.NewLine);
			//}

			sb.Append("Eyes: ");
			if (creator.leftEyeColor != creator.rightEyeColor && creator.rightEyeColor != null)
			{
				sb.Append("Heterochromatic: The left is" + ((EyeColor)creator.leftEyeColor).AsString() + ", the right is " + ((EyeColor)creator.rightEyeColor).AsString());
			}
			else
			{
				sb.Append("Monochromatic - Both are " + ((EyeColor)creator.leftEyeColor).AsString());
			}

			if (chosenGender.HasFlag(Gender.MALE))
			{
				sb.Append("Cock size: " + Measurement.ToNearestLargeAndSmallUnit(creator.cocks[0].validLength, true) + " long, "
					+ Measurement.ToNearestQuarterInchOrMillimeter(creator.cocks[0].validGirth, true) + "thick");
				if (creator.cocks[0].validKnot != 0)
				{
					sb.Append(", with a " + Measurement.ToNearestQuarterInchOrMillimeter(creator.cocks[0].validKnot * creator.cocks[0].validGirth, true) + "knot");
				}
				sb.Append(Environment.NewLine);
			}
			if (chosenGender.HasFlag(Gender.FEMALE))
			{
				sb.Append("Clit Size: " + Measurement.ToNearestQuarterInchOrMillimeter(creator.vaginas[0].validClitLength, true) + Environment.NewLine);
			}
			sb.Append("Breast size: " + creator.breasts[0].cupSize.AsText() + " with " + Measurement.ToNearestQuarterInchOrHalfCentimeter(creator.breasts[0].validNippleLength, true) + " nipples" + Environment.NewLine);
			return sb.ToString();
		}

		private string ComplexionLockedStr()
		{
			return LockedStr("complexion");
		}
		private string HairLockedStr()
		{
			return LockedStr("hair");
		}

		private string BeardOptionStr()
		{
			return "Beard Style";
		}

		private string BeardLockedStr()
		{
			return LockedStr("beard");
		}

		private string EyeColorStr()
		{
			return "Eye Color";
		}

		private string EyeLockedStr()
		{
			return LockedStr("eye color");
		}
		private string ChooseEyeStr()
		{
			return "What color are your eyes? Are they both the same color, or do you have a heterochromia?";
		}

		private string MonoChromaticStr()
		{
			return "Monochrome";
		}

		private string HeteroChromaticStr()
		{
			return "Heterochrome";
		}


		private string UseCurrentEyeColor(bool isLeftEye)
		{
			EyeColor color = (EyeColor)(!isLeftEye && creator.rightEyeColor != null ? creator.rightEyeColor : creator.leftEyeColor);
			return "Use the current settings? Your " + (isLeftEye ? "left" : "right") + " eye is currently " + color.AsString();
		}

		private string IChangedMyMindIllTakeMonochromaticEyesFor200Alex()
		{
			return "You can always change your mind and choose to have standard, monochromatic eyes, if you want.";
		}

		private string CockSizeStr()
		{
			return "Cock Size";
		}

		private string CockLockedStr()
		{
			return LockedStr("cock size and girth");
		}

		private string CockLengthStr()
		{
			byte min, max;
			string unit;
			if (Measurement.UsesMetric)
			{
				min = MIN_COCK_CM;
				max = MAX_COCK_CM;
				unit = " centimeters";
			}
			else
			{
				min = MIN_COCK_IN;
				max = MAX_COCK_IN;
				unit = " inches";
			}
			return "You can choose a cock length between " + min + " and " + max + unit + ". Your starting cock length will also affect starting cock thickness." +
				Environment.NewLine + Environment.NewLine + "Cock type and size can be altered later in the game through certain items or interactions.";
		}

		private string ClitSizeStr()
		{
			return "Clit Size";
		}
		private string ClitLockedStr()
		{
			return LockedStr("clit size");
		}

		private string ClitLengthStr()
		{
			float min, max;
			string unit;
			if (Measurement.UsesMetric)
			{
				min = MIN_CLIT_CM;
				max = MAX_CLIT_CM;
				unit = " centimeters";
			}
			else
			{
				min = MIN_CLIT_IN;
				max = MAX_CLIT_IN;
				unit = " inches";
			}
			return "You can choose a clit length between " + min + " and " + max + unit + Environment.NewLine + Environment.NewLine +
				"Clit size and other vaginal traits can be altered later in the game through certain items or interactions.";
		}
		private string BreastSizeStr()
		{
			return "Breast Size";
		}

		private string BreastsLockedStr()
		{
			return LockedStr("cup size and nipple length");
		}

		private string ChooseBreastStr()
		{
			return "You can choose a cup size for your breats. These cup sizes may have a slight effect on your nipple length. Both breast and nipple size may be altered through gameplay. ";
		}

		private string HeightStr()
		{
			return "Height";
		}

		private string HeightLockedStr()
		{
			return LockedStr("height");
		}

		private string SetHeightStr()
		{
			string min, max, unit;
			if (Measurement.UsesMetric)
			{
				unit = "in centimeters.";
				min = MIN_HEIGHT_CM + " centimeters"; //121.92, but we'll allow it for nicer numbers.
				max = MAX_HEIGHT_CM + " centimeters"; //243.84. see above.
			}
			else
			{
				unit = "in inches.";
				min = Measurement.ToNearestLargeAndSmallUnit(MIN_HEIGHT_IN, false) + "(" + Measurement.ToNearestSmallUnit(MIN_HEIGHT_IN, false, false) + ")";
				max = Measurement.ToNearestLargeAndSmallUnit(MAX_HEIGHT_IN, false) + "(" + Measurement.ToNearestSmallUnit(MAX_HEIGHT_IN, false, false) + ")";
			}

			return "Set your height " + unit + Environment.NewLine + Environment.NewLine + "You can choose any height between " + min + " and " + max;
		}

		private string InvalidHeightStr(string inputText)
		{
			return "The value you entered, " + inputText + "cannot be parsed as a number. Try again with a valid number";
		}

		private string InvalidHeightStr(int parsedInt)
		{
			byte max = Measurement.UsesMetric ? MAX_HEIGHT_CM : MAX_HEIGHT_IN;
			byte min = Measurement.UsesMetric ? MIN_HEIGHT_CM : MIN_HEIGHT_IN;
			return parsedInt > max
				? ("Your chosen value, " + parsedInt.ToString() + " is above the maximum height (" + max + ") you can choose! Try again with a larger value")
				: ("Your chosen value, " + parsedInt.ToString() + " is below the minimum height (" + min + ") you can choose! Try again with a larger value");
		}

		private string ConfirmHeightStr()
		{
			return "You'll be " + Measurement.ToNearestLargeAndSmallUnit(creator.heightInInches, false) + " tall. Is this okay with you?";
		}

		private string FurLockedStr()
		{
			return LockedStr(multiFurred ? "fur colors" : "fur color");
		}
		private string LockedStr(string src)
		{
			return "The " + src + " for this character cannot be changed";
		}

		private string EndowmentQuestionStr()
		{
			return "Every person is born with a gift. What's yours?";
		}

		private string HistoryQuestionStr()
		{
			return "Before you became a champion, you had other plans for your life. What were you doing before?";
		}
	}
}
