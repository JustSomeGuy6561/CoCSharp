//CharacterCreation.cs
//Description:
//Author: JustSomeGuy
//6/7/2019, 1:02 AM
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Areas;
using CoC.Frontend.Creatures.PlayerData;
using CoC.Frontend.UI;
using CoC.Frontend.Engine;
using CoC.Frontend.Perks;
using CoC.Frontend.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using static CoC.Frontend.UI.ViewOptions;
using CoC.Backend.UI;
//minor rework of creator. I'm just going to make this dynamic (meaning you have to instantiate it, even though there's only one at a time)
//because more often than not this won't be in use and therefore has no reason to be stored in memory. this also allows it to function like a true event-based program.

//Note: From what i can tell (nothing is documented), NG+ prevents you from using a special character. Therefore, we can get away with a single creator in the constructor.
//if it's a NG+, we simply have the NG+ stuff defined and nothing else. if it's a special character, it has no NG+ stuff defined. if it's neither, it'll be empty.
//it'd be possible to allow NG+ by first loading the special char, then adding in all perks, stats, and carryover items to it. But for now i haven't done that.

//Also, it would in theory be able to adapt this to allow a user to set all attributes about their character (horns, body part types, etc), but in practice it's a great deal of work.
//But you could!

namespace CoC.Frontend.Creatures
{
	//partially defined characters are getting special treatment:
	internal sealed partial class CharacterCreation
	{
		private const byte MIN_HEIGHT_IN = 48;
		private const byte MIN_HEIGHT_CM = 120;
		private const byte MAX_HEIGHT_IN = 96;
		private const byte MAX_HEIGHT_CM = 250;

		//used to parse the character's build. less than min average is the slender option, above is the thicc option.
		private const byte MIN_BUILD_AVERAGE = 40;
		private const byte MAX_BUILD_AVERAGE = 60;
		//same as above, but these help parse if we should use the more androgynous options. Only checked if build is "Average"
		private const byte MIN_MASCULINE = Femininity.SLIGHTLY_MASCULINE;
		private const byte MIN_FEMININE = Femininity.SLIGHTLY_FEMININE;
		private const byte ANDROGYNOUS = Femininity.ANDROGYNOUS;

		private const EyeColor DEFAULT_EYE_COLOR = EyeColor.BROWN;

		//PlayerCreator playerCreator;
		//bool genderLocked, occupationLocked, buildLocked, complexionLocked, hairLocked,

		private readonly PlayerCreator creator;
		private readonly bool genderLocked, buildLocked, complexionLocked, furLocked, hairLocked, historyLocked, endowmentLocked;
		private readonly bool heightLocked, eyesLocked, beardLocked, cockLocked, clitLocked, breastsLocked;
		private Gender chosenGender;

		//a character is considered locked if its perks and gender, build, skin tone, and hair are all known.
		//a character is considered semi-locked if its gender, build, skin tone, and hair are all known.
		private bool isSemiLocked => genderLocked && buildLocked && complexionLocked && hairLocked;
		private bool isLocked => isSemiLocked && historyLocked && endowmentLocked;

		private readonly bool newGamePlus;

		private bool hermUnlocked => FrontendGlobalSave.data.UnlockedNewGameHerm;

		private readonly StandardDisplay currentDisplay;

		#region Constructor
		internal CharacterCreation(StandardDisplay display, PlayerCreator specialCreator, bool isNewGamePlus = false)
		{
			currentDisplay = display ?? throw new ArgumentNullException(nameof(display));
			newGamePlus = isNewGamePlus;

			creator = specialCreator ?? throw new ArgumentNullException();
			//primarily for predefined characters, but also works for NG+ if future NG+ lets you carry over appearance or gender.
			if (!Tones.IsNullOrEmpty(creator.complexion))
			{
				complexionLocked = true;
			}
			else if (creator.complexion == Tones.NOT_APPLICABLE)
			{
				creator.complexion = null;
			}
			if (!HairFurColors.IsNullOrEmpty(creator.hairColor))
			{
				hairLocked = true;
			}
			else if (creator.hairColor == HairFurColors.NO_HAIR_FUR)
			{
				creator.hairColor = null;
			}
			//minor data cleanup. for the most part, we don't care if the creator is incomplete, but in this case, it's just easier if we do this.
			if (FurColor.IsNullOrEmpty(creator.furColor) && !FurColor.IsNullOrEmpty(creator.underFurColor))
			{
				creator.furColor = creator.underFurColor;
			}

			furLocked = (creator.bodyType?.epidermisType.usesFurColor != true && creator.bodyType?.secondaryEpidermisType.usesFurColor != true) || !FurColor.IsNullOrEmpty(creator.furColor);

			if (creator.defaultGender != null && creator.forceDefaultGender == true)
			{
				genderLocked = true;
			}
			else
			{
				creator.forceDefaultGender = false;
			}

			if (creator.thickness != null)
			{
				buildLocked = true;
			}

			heightLocked = creator.heightInInches != 0;
			beardLocked = genderLocked; //let's just leave it be, yeah.
			if (creator.rightEyeColor != null && creator.leftEyeColor == null)
			{
				creator.leftEyeColor = creator.rightEyeColor;
			}
			eyesLocked = creator.leftEyeColor != null;

			if (creator.leftEyeColor is null)
			{
				creator.leftEyeColor = DEFAULT_EYE_COLOR;
			}

			cockLocked = genderLocked || (creator.cocks != null && creator.cocks.Length != 0);
			clitLocked = genderLocked || (creator.vaginas != null && creator.vaginas.Length != 0);
			breastsLocked = genderLocked || (creator.breasts != null && creator.breasts.Length != 0);

			//set default stats.
			if (creator.strength == null) creator.strength = 15;
			if (creator.toughness == null) creator.toughness = 15;
			if (creator.speed == null) creator.speed = 15;
			if (creator.intelligence == null) creator.intelligence = 15;
			if (creator.sensitivity == null) creator.sensitivity = 15;
			if (creator.libido == null) creator.libido = 15;
			if (creator.corruption == null) creator.corruption = 15;

			endowmentLocked = creator.perks?.Find(x => x is EndowmentPerkBase) != null;
			historyLocked = creator.perks?.Find(x => x is HistoryPerkBase) != null;

			//clean up any invalid data set in the player creator.
			creator.artificiallyInfertile = false;

			//move these do player constructor. afaik theres not reason not to have these as default.
			//if (creator.lust == null) creator.lust = 15;
			//if (creator.hunger == null) creator.hunger = 80;
			//if (creator.obey == null) creator. = 10;
			//if (creator.esteem == null) creator. = 50;
			//if (creator.will == null) creator. = 80;
		}
		#endregion

		#region What's in a Name?


		internal void SetGenderGeneric()
		{
			currentDisplay.ClearOutput();
			currentDisplay.ClearButtons();
			if (genderLocked)
			{
				defaultGenderHelper(creator.defaultGender)();
			}
			else if (creator.defaultGender != null)
			{
				currentDisplay.OutputText(GenderQuestionWithDefault((Gender)creator.defaultGender));
				GenderOptions(defaultGenderHelper(creator.defaultGender));
			}
			else
			{
				currentDisplay.OutputText(GenderQuestion());
				GenderOptions();
			}
		}


		internal void SetGenderSpecial(bool isSpecial)
		{

			currentDisplay.ClearOutput();
			if (isSpecial)
			{
				//also prints out text for locked or semi-locked characters.
				currentDisplay.OutputText(SpecialText());
				//if (isLocked)
				//{
				//	DoNext(CreatePlayer);
				//}
				//else if (isSemiLocked)
				//{
				//	DoNext(ChooseEndowment);
				//}
				/*else*/ if (genderLocked)
				{
					defaultGenderHelper(creator.defaultGender)();
				}
				else if (creator.defaultGender != null)
				{
					currentDisplay.OutputText(GenderQuestionWithDefault((Gender)creator.defaultGender));
					GenderOptions(defaultGenderHelper(creator.defaultGender));
				}
				else
				{
					currentDisplay.OutputText(GenderQuestion2());
					GenderOptions();
				}
			}
			else
			{
				currentDisplay.OutputText(NotSpecialText());
				currentDisplay.OutputText(GenderQuestion2());
				GenderOptions();
			}
		}
		#endregion
		#region Gender
		private void GenderOptions(Action defaultAction = null)
		{
			currentDisplay.AddButton(0, GlobalStrings.MAN(), GenderMale);
			currentDisplay.AddButton(1, GlobalStrings.WOMAN(), GenderFemale);
			if (hermUnlocked)
			{
				currentDisplay.AddButton(2, GlobalStrings.HERM(), GenderHerm);
			}
			if (defaultAction != null)
			{
				currentDisplay.AddButton(4, GlobalStrings.DEFAULT(), defaultAction);
			}
		}

		private Action defaultGenderHelper(Gender? defaultGender)
		{
			switch (defaultGender)
			{
				case null:
					return null;
				case Gender.GENDERLESS:
					return GenderNoGender;
				case Gender.MALE:
					return GenderMale;
				case Gender.FEMALE:
					return GenderFemale;
				case Gender.HERM:
				default:
					return GenderHerm;
			}
		}

		internal void GenderMale()
		{
			//set the base attributes, but only if they haven't been set.
			//we aren't overriding custom defined values.

			//make our life easier
			chosenGender = Gender.MALE;

			if (!newGamePlus)
			{ //attributes
				creator.strength += 3;
				creator.toughness += 2;
			}
			//Body attributes
			if (creator.fertility == null) creator.fertility = 5;
			if (creator.hairLength == null) creator.hairLength = 1;
			if (creator.heightInInches == 0) creator.heightInInches = 71;
			if (creator.tone == null) creator.tone = 60;
			//Genitalia
			if (creator.numBalls == null || creator.numBalls == 0) creator.numBalls = 2;
			if (creator.ballSize == null || creator.ballSize == 0) creator.ballSize = 1;
			if (creator.cocks == null || creator.cocks.Length == 0)
			{
				creator.cocks = new CockCreator[] { new CockCreator(CockType.HUMAN, 5.5f, 1f) };
			}
			creator.vaginas = null;

			if (!buildLocked)
			{
				currentDisplay.ClearOutput();
				//currentDisplay.OutputText(images.showImage("event-question"));
				currentDisplay.OutputText(BuildText(Gender.MALE));

				currentDisplay.AddButton(0, MaleBuild(0), BuildLeanMale);
				currentDisplay.AddButton(1, MaleBuild(1), BuildAverageMale);
				currentDisplay.AddButton(2, MaleBuild(2), BuildThickMale);
				currentDisplay.AddButton(3, MaleBuild(3), BuildGirlyMale);
				//if default enabled and can be overridden
				//currentScene.AddButtonWithToolTip(4, GlobalStrings.DEFAULT(), defaultBuildHelper(Gender.MALE, creator.thickness, creator.femininity), DefaultBuildHint());
			}
			else
			{
				defaultBuildHelper(chosenGender, creator.thickness, creator.femininity)();
			}
		}

		internal void GenderFemale()
		{
			//make our life easier later.
			chosenGender = Gender.FEMALE;
			if (!newGamePlus)
			{ //attributes
				creator.speed += 3;
				creator.intelligence += 2;
			}

			//Body attributes
			creator.fertility = 10;
			creator.hairLength = 10;
			creator.heightInInches = 67;
			creator.tone = 30;
			//Genetalia
			creator.numBalls = 0;
			creator.ballSize = 0;
			creator.cocks = null;

			if (creator.vaginas == null || creator.vaginas.Length == 0)
			{
				creator.vaginas = new VaginaCreator[] { new VaginaCreator() };
			}

			if (!buildLocked)
			{
				currentDisplay.ClearOutput();
				//currentDisplay.OutputText(images.showImage("event-question"));
				currentDisplay.OutputText(BuildText(Gender.FEMALE));
				currentDisplay.AddButton(0, FemaleBuild(0), BuildSlenderFemale);
				currentDisplay.AddButton(1, FemaleBuild(1), BuildAverageFemale);
				currentDisplay.AddButton(2, FemaleBuild(2), BuildCurvyFemale);
				currentDisplay.AddButton(3, FemaleBuild(3), BuildTomboyishFemale);
				//if default enabled and can be overridden
				//currentScene.AddButtonWithToolTip(4, GlobalStrings.DEFAULT(), defaultBuildHelper(Gender.FEMALE, creator.thickness, creator.femininity), DefaultBuildHint());
			}
			else
			{
				defaultBuildHelper(chosenGender, creator.thickness, creator.femininity)();
			}
		}

		internal void GenderHerm()
		{
			//make our life easier
			chosenGender = Gender.HERM;

			if (!newGamePlus)
			{ //attributes
				creator.strength += 1;
				creator.toughness += 1;
				creator.speed += 1;
				creator.intelligence += 1;
			}
			//Body attributes
			creator.fertility = 10;
			creator.hairLength = 10;
			creator.heightInInches = 69;
			creator.tone = 45;
			//Genetalia
			if (creator.numBalls == null) creator.numBalls = 2;
			if (creator.ballSize == null) creator.ballSize = 1;

			if (creator.cocks == null || creator.cocks.Length == 0)
			{
				creator.cocks = new CockCreator[] { new CockCreator() };
			}

			if (creator.vaginas == null || creator.vaginas.Length == 0)
			{
				creator.vaginas = new VaginaCreator[] { new VaginaCreator() };
			}

			if (!buildLocked)
			{
				currentDisplay.ClearOutput();
				//currentDisplay.OutputText(images.showImage("event-question"));
				currentDisplay.OutputText(BuildText(Gender.HERM));

				HermButtons(0, BuildSlenderFemale);
				HermButtons(1, BuildAverageFemale);
				HermButtons(2, BuildCurvyFemale);
				HermButtons(3, BuildTomboyishFemale);
				//if default enabled and can be overridden
				//HermButtons(4, defaultBuildHelper(Gender.HERM, creator.thickness, creator.femininity));
				HermButtons(5, BuildLeanMale);
				HermButtons(6, BuildAverageMale);
				HermButtons(7, BuildThickMale);
				HermButtons(8, BuildGirlyMale);
			}
			else
			{
				defaultBuildHelper(chosenGender, creator.thickness, creator.femininity)();
			}
		}

		private bool HermButtons(byte index, Action callback)
		{
			Triple<string> data = HermButtonData(index);
			return currentDisplay.AddButtonWithToolTip(index, data.first, callback, data.second, data.third);
		}

		internal void GenderNoGender()
		{
			//make our life easier
			chosenGender = Gender.GENDERLESS;
			//attributes
			//Genderless don't get any bonus stats as of current version.
			//if (!newGamePlus)
			//{
			//creator.strength += 1;
			//creator.toughness += 1;
			//creator.speed += 1;
			//creator.intelligence += 1;
			//}
			if (creator.fertility == null) creator.fertility = 0;
			if (creator.hairLength == null) creator.hairLength = 5;
			if (creator.heightInInches == 0) creator.heightInInches = 60;
			if (creator.tone == null) creator.tone = 50;

			creator.cocks = null;
			creator.vaginas = null;

			if (!buildLocked)
			{
				currentDisplay.ClearOutput();
				currentDisplay.OutputText(BuildText(Gender.GENDERLESS));

				GenderlessButtons(0, BuildGirlyMale);
				GenderlessButtons(1, BuildTomboyishFemale);
				GenderlessButtons(2, BuildAndrogynous);
				//if default enabled and can be overridden
				//currentScene.AddButtonWithToolTip(4, GlobalStrings.DEFAULT(), defaultBuildHelper(Gender.GENDERLESS, creator.thickness, creator.femininity), DefaultBuildHint());
			}
			else
			{
				defaultBuildHelper(chosenGender, creator.thickness, creator.femininity)();
			}
		}
		private bool GenderlessButtons(byte index, Action callback)
		{
			Triple<string> data = GenderlessButtonData(index);
			return currentDisplay.AddButtonWithToolTip(index, data.first, callback, data.second, data.third);
		}
		#endregion
		#region Build
		private Action defaultBuildHelper(Gender gender, byte? thickness, byte? femininity)
		{
			if (thickness == null)
			{
				return null;
			}
			//if possible, treat herms as if they're female or male for purpose of build.
			//otherwise, genderless and herms have 3 options: fully androgynous, slightly feminine, or slightly masculine.
			if (gender == Gender.GENDERLESS || gender == Gender.HERM)
			{
				if (gender == Gender.HERM && femininity >= MIN_FEMININE)
				{
					gender = Gender.FEMALE;
				}
				else if (gender == Gender.HERM && femininity <= MIN_MASCULINE)
				{
					gender = Gender.MALE;
				}
				if (femininity < ANDROGYNOUS) //masculine
				{
					return BuildGirlyMale;
				}
				else if (femininity > ANDROGYNOUS) //feminine
				{
					return BuildTomboyishFemale;
				}
				else //null or androgynous (null defaults to androgynous)
				{
					return BuildAndrogynous;
				}
			}

			if (thickness <= MIN_BUILD_AVERAGE)
			{
				return gender.HasFlag(Gender.FEMALE) ? (Action)BuildSlenderFemale : (Action)BuildLeanMale; //only one of these casts is necessary, but both makes it more readible imo.
			}
			else if (thickness >= MAX_BUILD_AVERAGE)
			{
				return gender.HasFlag(Gender.FEMALE) ? (Action)BuildCurvyFemale : (Action)BuildThickMale;
			}
			else
			{
				return gender.HasFlag(Gender.FEMALE) ? (Action)BuildAverageFemale : (Action)BuildAverageMale;
			}
		}

		private void BuildLeanMale()
		{
			creator.strength -= 1;
			creator.speed += 1;
			if (!buildLocked) creator.tone += 5;

			if (creator.femininity == null) creator.femininity = 34;
			if (creator.thickness == null) creator.thickness = 30;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.TIGHT;
			if (creator.hipSize == null) creator.hipSize = Hips.SLENDER;
			ChooseComplexion();
		}
		private void BuildSlenderFemale()
		{
			creator.strength -= 1;
			creator.speed += 1;
			if (!buildLocked) creator.tone += 5;

			if (creator.femininity == null) creator.femininity = 66;
			if (creator.thickness == null) creator.thickness = 30;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.B) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.TIGHT;
			if (creator.hipSize == null) creator.hipSize = Hips.AMPLE;
			ChooseComplexion();
		}
		private void BuildAverageMale()
		{
			if (creator.femininity == null) creator.femininity = 30;
			if (creator.thickness == null) creator.thickness = 50;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.AVERAGE;
			if (creator.hipSize == null) creator.hipSize = Hips.AVERAGE;
			ChooseComplexion();
		}
		private void BuildAverageFemale()
		{
			if (creator.femininity == null) creator.femininity = 70;
			if (creator.thickness == null) creator.thickness = 50;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.C) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.NOTICEABLE;
			if (creator.hipSize == null) creator.hipSize = Hips.AMPLE;
			ChooseComplexion();
		}
		private void BuildThickMale()
		{
			creator.speed -= 4;
			creator.strength += 2;
			creator.toughness += 2;
			if (!buildLocked) creator.tone -= 5;

			if (creator.femininity == null) creator.femininity = 29;
			if (creator.thickness == null) creator.thickness = 70;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.FLAT) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.NOTICEABLE;
			if (creator.hipSize == null) creator.hipSize = Hips.AVERAGE;
			ChooseComplexion();
		}
		private void BuildCurvyFemale()
		{
			creator.speed -= 2;
			creator.strength += 1;
			creator.toughness += 1;

			if (creator.femininity == null) creator.femininity = 71;
			if (creator.thickness == null) creator.thickness = 70;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.D) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.LARGE;
			if (creator.hipSize == null) creator.hipSize = Hips.CURVY;
			ChooseComplexion();
		}
		private void BuildGirlyMale()
		{
			creator.strength -= 2;
			creator.speed += 2;
			if (creator.femininity == null)
			{
				creator.femininity = chosenGender.HasFlag(Gender.FEMALE) ? (byte)45 : (byte)40;
			}
			if (creator.thickness == null) creator.thickness = 50;
			if (creator.tone == null) creator.tone = 26;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.A) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.NOTICEABLE;
			if (creator.hipSize == null) creator.hipSize = Hips.SLENDER;
			ChooseComplexion();
		}
		private void BuildTomboyishFemale()
		{
			creator.strength += 1;
			creator.speed -= 1;
			if (creator.femininity == null)
			{
				creator.femininity = chosenGender.HasFlag(Gender.MALE) ? (byte)55 : (byte)60;
			}
			if (creator.thickness == null) creator.thickness = 50;
			if (creator.tone == null) creator.tone = 50;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.A) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.TIGHT;
			if (creator.hipSize == null) creator.hipSize = Hips.SLENDER;
			ChooseComplexion();
		}

		private void BuildAndrogynous()
		{
			if (creator.femininity == null) creator.femininity = 50;
			if (creator.thickness == null) creator.thickness = 50;
			if (creator.tone == null) creator.tone = 50;
			if (creator.breasts == null || creator.breasts.Length == 0)
			{
				creator.breasts = new BreastCreator[] { new BreastCreator(CupSize.A) };
			}
			if (creator.buttSize == null) creator.buttSize = Butt.AVERAGE;
			if (creator.hipSize == null) creator.hipSize = Hips.SLENDER;
			ChooseComplexion();
		}
		#endregion
		#region Skin
		//Complicated by the fact it's used both in standard creation and customization menu. the bool takes care of that.
		private void ChooseComplexion()
		{
			if (!complexionLocked)
			{
				currentDisplay.ClearOutput();
				//currentDisplay.OutputText(images.showImage("event-question"));
				currentDisplay.OutputText(ComplexionText());
				currentDisplay.AddButton(0, Tones.LIGHT.AsString(), () => SetComplexion(Tones.LIGHT));
				currentDisplay.AddButton(1, Tones.FAIR.AsString(), () => SetComplexion(Tones.FAIR));
				currentDisplay.AddButton(2, Tones.OLIVE.AsString(), () => SetComplexion(Tones.OLIVE));
				currentDisplay.AddButton(3, Tones.DARK.AsString(), () => SetComplexion(Tones.DARK));
				if (!hitCustomizationMenu && !complexionLocked && !Tones.IsNullOrEmpty(creator.complexion)) //currently impossible. complexion locks if not null. may change in future, idk.
				{
					currentDisplay.AddButton(4, GlobalStrings.DEFAULT(), () => SetComplexion(creator.complexion));
				}
				currentDisplay.AddButton(5, Tones.EBONY.AsString(), () => SetComplexion(Tones.EBONY));
				currentDisplay.AddButton(6, Tones.MAHOGANY.AsString(), () => SetComplexion(Tones.MAHOGANY));
				currentDisplay.AddButton(7, Tones.RUSSET.AsString(), () => SetComplexion(Tones.RUSSET));

				if (hitCustomizationMenu)
				{
					currentDisplay.AddButton(14, GlobalStrings.BACK(), GenericStyleCustomizeMenu);
				}
			}
			else if (!hitCustomizationMenu)
			{
				if (!furLocked)
				{
					ChooseFur();
				}
				else
				{
					ChooseHairColor(false);
				}
			}
			//else we came from the menu, but can't set this. This should be disabled, so this should never happen. in the event it does, simply ignore it.
			//we don't clear the menu, so the menu is still there. We'll run a debug method that won't do anything in RELEASE
			else
			{
				DebugBadButtonHit();
			}
		}

		private void SetComplexion(Tones choice)
		{
			creator.complexion = choice;
			if (hitCustomizationMenu)
			{
				GenericStyleCustomizeMenu();
			}
			else
			{
				currentDisplay.ClearOutput();
				//currentDisplay.OutputText(images.showImage("event-question"));
				currentDisplay.OutputText(ConfirmComplexionText(choice));
				if (!furLocked)
				{
					ChooseFur();
				}
				else
				{
					ChooseHairColor(false);
				}
			}
		}
		#endregion
		#region Fur
		//boolean used to determine if we've hit fur settings at least once. Note that this is a new option for the ultra-rare case a pre-defined character has fur, but there's no color selected.
		//i dunno, it seems like it'd be a cool thing to do - a backstory about an abandoned baby in Ingnam that was mostly human - save for fur covering their body. perhaps they flaunted it, and that's
		//why they're the champion, or perhaps they shaved it to appear normal, but the elders still feared "corrupting influence" or some shit. Or they never wanted this character to begin with, so they
		//trained them to be a champion and are finally getting rid of them. Whatever you want.
		private bool setPrimaryFur = false;
		//easier to store these. this class solely exists to make the creator work, even when you can't pass the data around because callbacks.
		//so why not use it? (Note to Self: DUMMY)
		private HairFurColors furBuilderColorOne, furBuilderColorTwo;
		private FurMulticolorPattern furBuilderPattern;

		//we only go here if the fur color is not locked.
		private void ChooseFur(bool primaryFur = true)
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseFurStr(primaryFur));

			currentDisplay.AddButton(0, SolidColorStr(), () => ChooseFurColor(primaryFur, true, false));
			currentDisplay.AddButton(1, MultiColorStr(), () => ChooseFurColor(primaryFur, true, true));
			if (!furLocked && !FurColor.IsNullOrEmpty(creator.furColor))
			{
				currentDisplay.AddButton(4, GlobalStrings.DEFAULT(), () => ChooseHairColor(false));
			}
		}

		private void ChooseFurColor(bool isPrimaryFur, bool isPrimaryColor, bool hasMultipleColors)
		{

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseFurColorStr());

			List<DropDownEntry> vars = HairFurColors.AvailableHairFurColors().ConvertAll(x => new DropDownEntry(x.AsString(), () => ConfirmFurColor(x, isPrimaryFur, isPrimaryColor, hasMultipleColors)));
			currentDisplay.ActivateDropDownMenu(vars.ToArray());


			if (setPrimaryFur)
			{
				currentDisplay.AddButton(9, GlobalStrings.BACK(), FurOptions);
			}
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void ConfirmFurColor(HairFurColors color, bool isPrimaryFur, bool isPrimaryColor, bool isMulticolored)
		{
			if (isPrimaryColor)
			{
				furBuilderColorOne = color;
			}
			else
			{
				furBuilderColorTwo = color;
			}

			Action NextAction;
			if (!isMulticolored)
			{
				NextAction = () => SetColor(isPrimaryFur, false);
			}
			else if (isPrimaryColor)
			{
				NextAction = () => ChooseFurColor(isPrimaryFur, false, true);
			}
			else
			{
				NextAction = () => ChooseFurPattern(isPrimaryFur);
			}

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(SelectedColor(color, isPrimaryFur, isPrimaryColor, isMulticolored));
			currentDisplay.AddButton(0, GlobalStrings.CONFIRM(), NextAction);
			if (setPrimaryFur)
			{
				currentDisplay.AddButton(9, GlobalStrings.BACK(), FurOptions);
			}
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void ChooseFurPattern(bool isPrimaryFur)
		{
			void callback(FurMulticolorPattern pattern)
			{
				furBuilderPattern = pattern;
				SetColor(isPrimaryFur, true);
			}

			bool buttonMaker(byte index, FurMulticolorPattern pattern) => currentDisplay.AddButton(index, pattern.AsString(), () => callback(pattern));

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseFurPatternStr());
			buttonMaker(0, FurMulticolorPattern.NO_PATTERN);
			buttonMaker(1, FurMulticolorPattern.MIXED);
			buttonMaker(2, FurMulticolorPattern.SPOTTED);
			buttonMaker(3, FurMulticolorPattern.STRIPED);

			if (setPrimaryFur)
			{
				currentDisplay.AddButton(9, GlobalStrings.BACK(), FurOptions);
			}
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void SetColor(bool primary, bool multicolored)
		{
			FurColor created;
			if (multicolored)
			{
				created = new FurColor(furBuilderColorOne, furBuilderColorTwo, furBuilderPattern);
			}
			else
			{
				created = new FurColor(furBuilderColorOne);
			}
			if (primary)
			{
				creator.furColor = created;
			}
			else
			{
				creator.underFurColor = created;
			}
			if (!setPrimaryFur)
			{
				setPrimaryFur = true;
				currentDisplay.ClearOutput();
				currentDisplay.OutputText(FurColorFirstRunStr(created));
				currentDisplay.AddButton(0, FurOptionsStr(), FurOptions);
				currentDisplay.AddButton(1, GlobalStrings.CONTINUE(), () => ChooseHairColor(false));
			}
			else
			{
				FurOptions();
			}
		}

		private bool hasFur => creator.bodyType?.epidermisType.usesFurColor == true || creator.bodyType?.secondaryEpidermisType.usesFurColor == true;
		private bool multiFurred => creator.bodyType?.epidermisType.usesFurColor == true && creator.bodyType?.secondaryEpidermisType.usesFurColor == true;

		private void FurOptions()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(FurOptionsText());

			currentDisplay.AddButton(0, FurStr(true), () => ChooseFur(true));
			currentDisplay.AddButton(1, FurTextureStr(true), () => ChooseFurTexture(true));
			if (multiFurred)
			{
				currentDisplay.AddButton(2, FurStr(false), () => ChooseFur(false));
				currentDisplay.AddButton(3, FurTextureStr(false), () => ChooseFurTexture(false));
			}
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
			else
			{
				currentDisplay.AddButton(14, GlobalStrings.CONTINUE(), () => ChooseHairColor(false));
			}
		}

		private void ChooseFurTexture(bool isPrimary)
		{
			bool buttonMaker(byte index, FurTexture texture) => currentDisplay.AddButton(index, texture.AsString(), () => SetFurTexture(texture, isPrimary));

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseTextureText(isPrimary));

			buttonMaker(0, FurTexture.FLUFFY);
			buttonMaker(1, FurTexture.SMOOTH);
			buttonMaker(2, FurTexture.SHINY);
			buttonMaker(3, FurTexture.SOFT);
			currentDisplay.AddButton(4, GlobalStrings.DEFAULT(), () => SetFurTexture(FurTexture.NONDESCRIPT, isPrimary));
			buttonMaker(5, FurTexture.MANGEY);

			if (setPrimaryFur)
			{
				currentDisplay.AddButton(9, GlobalStrings.BACK(), FurOptions);
			}
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void SetFurTexture(FurTexture choice, bool isPrimary)
		{
			if (isPrimary)
			{
				creator.furTexture = choice;
			}
			else
			{
				creator.underBodyFurTexture = choice;
			}
			FurOptions();
		}
		#endregion
		#region Hair
		//boolean used to determine if we've ran the hair color setting at least once. during character creation, the player must set their hair color, the rest is optional.
		//we come from complexion straight to hair color, then return to the menu. if this isn't set, we can't return to the menu until it's set.
		private bool hitHairOptions = false;

		private void ChooseHairColor(bool isHighlight)
		{
			if (!hairLocked)
			{
				currentDisplay.ClearOutput();
				string output = isHighlight ? HighlightText() : HairText();
				currentDisplay.OutputText(output);

				currentDisplay.AddButton(0, HairFurColors.BLONDE.AsString(), () => SetHair(HairFurColors.BLONDE, isHighlight));
				currentDisplay.AddButton(1, HairFurColors.BROWN.AsString(), () => SetHair(HairFurColors.BROWN, isHighlight));
				currentDisplay.AddButton(2, HairFurColors.BLACK.AsString(), () => SetHair(HairFurColors.BLACK, isHighlight));
				currentDisplay.AddButton(3, HairFurColors.RED.AsString(), () => SetHair(HairFurColors.RED, isHighlight));
				//if has a default hair color and we're in the first run of hair color.
				if (!isHighlight && !hitHairOptions && !HairFurColors.IsNullOrEmpty(creator.hairColor)) //currently can't be hit - the hair is locked if not null.
				{
					currentDisplay.AddButton(4, GlobalStrings.DEFAULT(), () => SetHair(creator.hairColor, false));
				}
				currentDisplay.AddButton(5, HairFurColors.GRAY.AsString(), () => SetHair(HairFurColors.GRAY, isHighlight));
				currentDisplay.AddButton(6, HairFurColors.WHITE.AsString(), () => SetHair(HairFurColors.WHITE, isHighlight));
				currentDisplay.AddButton(7, HairFurColors.AUBURN.AsString(), () => SetHair(HairFurColors.AUBURN, isHighlight));


				if (hitHairOptions)
				{
					currentDisplay.AddButton(9, GlobalStrings.BACK(), HairOptions);
				}
				if (hitCustomizationMenu)
				{
					currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
				}
			}
			else if (!hitCustomizationMenu)
			{
				GenericStyleCustomizeMenu();
			}
			else
			{
				DebugBadButtonHit();
			}
			//else we're not supposed to be able to hit this button (should be disabled) so just pretend they didn't hit it.
		}

		//for the love of god, please use the release version to published projects. this is just here for debugging.
		private void DebugBadButtonHit()
		{
#if DEBUG
			Debug.WriteLine("Never should be hit, as all buttons to this should be disabled. Stack Trace: " + new System.Diagnostics.StackTrace().ToString());
#endif
		}

		//only called if we actually can set the hair, which means hair is not locked. this means we can go to the hair menu w/o fear of it being locked.
		private void SetHair(HairFurColors color, bool isHighlight)
		{

			if (isHighlight)
			{
				creator.hairHighlightColor = color;
				//output highlight text
			}
			else
			{
				creator.hairColor = color;
				//output hair text
			}
			if (!hitHairOptions)
			{
				currentDisplay.ClearOutput();
				currentDisplay.OutputText(ConfirmHairFirstRun(color));
				currentDisplay.AddButton(0, HairOptionStr(), HairOptions);
				currentDisplay.AddButton(1, GlobalStrings.CONTINUE(), GenericStyleCustomizeMenu);
			}
			else
			{
				HairOptions();
			}
		}

		//either all hair options are locked or none are.

		private void HairOptions()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(HairOptionsText(hitCustomizationMenu));
			hitHairOptions = true;

#warning Perhaps add what the current style data is here?

			currentDisplay.AddButton(0, HairColorStr(), () => ChooseHairColor(false));
			currentDisplay.AddButton(1, HighlightColorStr(), () => ChooseHairColor(true));
			currentDisplay.AddButton(2, HairLengthStr(), () => ChooseHairLength());
			currentDisplay.AddButton(3, HairStyleStr(), ChooseHairStyle);

			//did we come from customization?
			string buttonText = hitCustomizationMenu ? GlobalStrings.RETURN() : GlobalStrings.CONTINUE();
			currentDisplay.AddButton(14, buttonText, GenericStyleCustomizeMenu);
		}

		private void ChooseHairLength(bool clearOutput = true)
		{
			if (clearOutput)
			{
				currentDisplay.ClearOutput();
			}
			currentDisplay.OutputText(ChooseHairLengthStr());
			//null operators ftw! basically, if hairLength is null, empty str. if not, call its ToString.
			currentDisplay.ActivateInputField(InputField.INPUT_POSITIVE_NUMBERS, InputField.VALID_POSITIVE_NUMBERS, creator.hairLength?.ToString() ?? "", HairLengthStr());
			currentDisplay.AddButton(0, GlobalStrings.CONFIRM(), SetHairLength);
			currentDisplay.AddButton(9, GlobalStrings.BACK(), HairOptions);
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void SetHairLength()
		{
			if (string.IsNullOrEmpty(currentDisplay.GetOutput()))
			{
				return;
			}
			currentDisplay.ClearOutput();
			currentDisplay.DeactivateInputField();
			if (float.TryParse(currentDisplay.GetOutput(), out float parsedInput))
			{
				double parsed = parsedInput;
				if (Measurement.UsesMetric)
				{
					parsed *= Measurement.TO_INCHES;
				}

				if (parsed > creator.heightInInches)
				{
					currentDisplay.OutputText(HairTooLongStr(parsedInput));
					ChooseHairLength(false);
				}
				else if (parsed < 0)
				{
					currentDisplay.OutputText(NegativeNumberHairStr(parsedInput));
					ChooseHairLength(false);
				}
				else
				{
					creator.hairLength = (float)parsed;
					HairOptions();
				}
			}
			else
			{
				currentDisplay.OutputText(NotANumberInput(currentDisplay.GetOutput()));
				ChooseHairLength(false);
			}
		}

		private void ChooseHairStyle()
		{
			bool buttonMaker(byte index, HairStyle style) => currentDisplay.AddButton(index, style.AsString(), () => SetHairStyle(style));

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseHairStyleStr());

			buttonMaker(0, HairStyle.STRAIGHT);
			buttonMaker(1, HairStyle.CURLY);
			buttonMaker(2, HairStyle.WAVY);
			buttonMaker(3, HairStyle.COILED);
			//we don't need a default, but the common spacing was 4 = default.
			buttonMaker(5, HairStyle.MESSY);
			buttonMaker(6, HairStyle.PONYTAIL);
			buttonMaker(7, HairStyle.BRAIDED);

			currentDisplay.AddButton(9, GlobalStrings.BACK(), HairOptions);
			if (hitCustomizationMenu)
			{
				currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
			}
		}

		private void SetHairStyle(HairStyle hairStyle)
		{
			creator.hairStyle = hairStyle;
			HairOptions();
		}
		#endregion
		#region Customization Menu
		//i was passing this all over the place, and it was tedious. might as well take advantage of object oriented callbacks, no?
		private bool hitCustomizationMenu = false;

		private void GenericStyleCustomizeMenu()
		{
			if (isLocked)
			{
				CreatePlayer();
			}
			else if (isSemiLocked)
			{
				ChooseEndowment();
			}
			else
			{
				hitCustomizationMenu = true;
				currentDisplay.ClearOutput();
				//mainView.nameBox.visible = false;
				//mainView.nameBox.maxChars = 16;
				//mainView.nameBox.restrict = null;
				//currentDisplay.OutputText(images.showImage("event-creation"));

				currentDisplay.OutputText(GenericCustomizationText());

				//reuse the old menus - why be redundant? This way there's less code to maintain.
				currentDisplay.AddButtonOrAddDisabledWithToolTip(0, ComplexionStr(), !complexionLocked, ChooseComplexion, ComplexionLockedStr());
				currentDisplay.AddButtonOrAddDisabledWithToolTip(1, HairOptionStr(), !hairLocked, HairOptions, HairLockedStr()); //include highlight and hairStyle here, as well as length?

				//if (canGrowBeard)
				//{
				//	currentScene.AddButtonOrAddDisabledWithToolTip(2, BeardOptionStr(), beardLocked, MenuBeardSettings, BeardLockedStr());
				//}

				currentDisplay.AddButtonOrAddDisabledWithToolTip(3, EyeColorStr(), !eyesLocked, MenuEyeColor, EyeLockedStr()); //include heterochromea option here
				if (hasFur) currentDisplay.AddButtonOrAddDisabledWithToolTip(4, FurColorStr(), !furLocked, FurOptions, FurLockedStr());
				currentDisplay.AddButtonOrAddDisabledWithToolTip(5, HeightStr(), !heightLocked, ChooseHeight, HeightLockedStr());
				if (chosenGender.HasFlag(Gender.MALE)) currentDisplay.AddButtonOrAddDisabledWithToolTip(6, CockSizeStr(), !cockLocked, MenuCockLength, CockLockedStr());
				if (chosenGender.HasFlag(Gender.FEMALE)) currentDisplay.AddButtonOrAddDisabledWithToolTip(7, ClitSizeStr(), !clitLocked, MenuClitLength, ClitLockedStr());
				currentDisplay.AddButtonOrAddDisabledWithToolTip(8, BreastSizeStr(), !breastsLocked, menuBreastSize, BreastsLockedStr());
				currentDisplay.AddButton(9, GlobalStrings.CONTINUE(), ChooseEndowment);
			}
		}
		#endregion
		#region Beards
		//----------------- BEARD STYLE -----------------
		//private bool canGrowBeard => chosenGender == Gender.MALE || (chosenGender != Gender.FEMALE && creator.femininity <= MIN_MASCULINE && creator.breasts[0].cupSize < CupSize.C);

		//private void MenuBeardSettings()
		//{
		//	currentDisplay.ClearOutput();
		//	currentDisplay.OutputText("You can choose your beard length and style.\n\n");
		//	currentDisplay.OutputText("Beard: " + );

		//	currentScene.AddButton(0, "Style", menuBeardStyle);
		//	currentScene.AddButton(1, "Length", menuBeardLength);
		//	currentScene.AddButton(14, "Back", GenericStyleCustomizeMenu);
		//}
		//private void menuBeardStyle()
		//{
		//	currentDisplay.ClearOutput();
		//	//currentDisplay.OutputText(images.showImage("event-question"));
		//	currentDisplay.OutputText("What beard style would you like?");

		//	currentScene.AddButton(0, "Normal", () => chooseBeardStyle());
		//	currentScene.AddButton(1, "Goatee", () => chooseBeardStyle());
		//	currentScene.AddButton(2, "Clean-cut", () => chooseBeardStyle());
		//	currentScene.AddButton(3, "Mountainman", () => chooseBeardStyle());
		//	currentScene.AddButton(9, GlobalStrings.BACK(), menuBeardSettings);
		//	currentScene.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
		//}

		//private void chooseBeardStyle(BeardStyle beardStyle)
		//{
		//	creator.beardStyle = beardStyle;
		//	menuBeardSettings();
		//}
		//private void menuBeardLength()
		//{
		//	currentDisplay.ClearOutput();
		//	//currentDisplay.OutputText(images.showImage("event-question"));
		//	currentDisplay.OutputText("How long would you like your beard be? \n\nNote: Beard will slowly grow over time, just like in the real world. Unless you have no beard. You can change your beard style later in the game.");
		//	currentScene.AddButton(0, "No Beard", () => chooseBeardLength(0));
		//	currentScene.AddButton(1, "Trim", () => chooseBeardLength(0.1f));
		//	currentScene.AddButton(2, "Short", () => chooseBeardLength(0.2f));
		//	currentScene.AddButton(3, "Medium", () => chooseBeardLength(5f));
		//	currentScene.AddButton(4, "Mod. Long", () => chooseBeardLength(5f));
		//	currentScene.AddButton(5, "Long", () => chooseBeardLength(3f));
		//	currentScene.AddButton(6, "Very Long", () => chooseBeardLength(6f));
		//	currentScene.AddButton(9, GlobalStrings.BACK(), menuBeardSettings);
		//	currentScene.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
		//}
		//private void chooseBeardLength(float length)
		//{
		//	creator.beardLength = length;
		//	menuBeardSettings();
		//}
		#endregion
		#region EyeColor
		/**/
		// ----------------- EYE COLOR -------------------
		private void MenuEyeColor()
		{
			currentDisplay.AddButton(0, MonoChromaticStr(), () => ChooseEyeColor(true, true));
			currentDisplay.AddButton(1, HeteroChromaticStr(), () => ChooseEyeColor(true, false));
		}

		private EyeColor? left, right;
		private void ChooseEyeColor(bool isLeftEye, bool isBothEyes)
		{
			left = null;
			right = null;

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseEyeStr());
			ChooseEyeColor2(true, isLeftEye, isBothEyes);
		}

		private void ChooseEyeColor2(bool isPage1, bool isLeftEye, bool isBothEyes)
		{
			bool buttonMaker(byte index, EyeColor color) => currentDisplay.AddButton(index, color.AsString(), () => SetEyeColor(color, isLeftEye, isBothEyes));

			currentDisplay.ClearButtons();
			if (isPage1)
			{
				//buttonMaker(0, EyeColor.AMBER);
				buttonMaker(0, EyeColor.AMBER);
				buttonMaker(1, EyeColor.BLUE);
				buttonMaker(2, EyeColor.BROWN);
				buttonMaker(3, EyeColor.GRAY);
				buttonMaker(5, EyeColor.GREEN);
				buttonMaker(6, EyeColor.HAZEL);
				buttonMaker(7, EyeColor.RED);
				buttonMaker(8, EyeColor.VIOLET);
				currentDisplay.ButtonNextPage(() => ChooseEyeColor2(true, isLeftEye, isBothEyes), 10);
			}
			else
			{
				buttonMaker(0, EyeColor.SILVER);
				buttonMaker(1, EyeColor.YELLOW);
				buttonMaker(2, EyeColor.PINK);
				buttonMaker(3, EyeColor.ORANGE);
				buttonMaker(5, EyeColor.INDIGO);
				buttonMaker(6, EyeColor.TAN);
				buttonMaker(7, EyeColor.BLACK);
				currentDisplay.ButtonPreviousPage(() => ChooseEyeColor2(true, isLeftEye, isBothEyes));
			}

			if (!isBothEyes && !isLeftEye)
			{
				if (creator.rightEyeColor != null || creator.leftEyeColor != null && creator.leftEyeColor != left)
				{
					EyeColor color;
					color = creator.rightEyeColor != null ? (EyeColor)creator.rightEyeColor : (EyeColor)creator.leftEyeColor;
					currentDisplay.AddButtonWithToolTip(4, GlobalStrings.DEFAULT(), () => SetEyeColor(color, false, false), UseCurrentEyeColor(false));
				}
				currentDisplay.AddButtonWithToolTip(9, MonoChromaticStr(), () => SetEyeColor((EyeColor)left, true, true), IChangedMyMindIllTakeMonochromaticEyesFor200Alex());
			}
			else if (!isBothEyes && creator.leftEyeColor != null)
			{
				currentDisplay.AddButtonWithToolTip(4, GlobalStrings.DEFAULT(), () => SetEyeColor((EyeColor)creator.leftEyeColor, true, false), UseCurrentEyeColor(true));
			}
			currentDisplay.AddButton(14, GlobalStrings.RETURN(), GenericStyleCustomizeMenu);
		}

		private void SetEyeColor(EyeColor color, bool isLeftEye, bool isBothEyes)
		{
			if (isLeftEye)
			{
				left = color;
			}
			else
			{
				right = color;
			}
			if (isBothEyes)
			{
				creator.leftEyeColor = left;
				creator.rightEyeColor = null;
				GenericStyleCustomizeMenu();
			}
			else if (isLeftEye)
			{
				ChooseEyeColor2(true, false, false);
			}
			else
			{
				creator.leftEyeColor = left;
				creator.rightEyeColor = right;
				GenericStyleCustomizeMenu();
			}
		}
		#endregion
		#region Height
		//----------------- heightInInches -----------------
		private void ChooseHeight()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(SetHeightStr());

			if (creator.heightInInches < 48)
			{
				creator.heightInInches = 48;
			}

			int maxLength = Measurement.UsesMetric ? 3 : 2;



			currentDisplay.ActivateInputField(InputField.INPUT_POSITIVE_INTEGERS_NOSIGN, InputField.VALID_POSITIVE_INTEGERS_NOSIGN,
				creator.heightInInches.ToString(), HeightStr(), maxLength);

			currentDisplay.AddButton(0, GlobalStrings.OK(), ConfirmHeight);
			currentDisplay.AddButton(4, GlobalStrings.BACK(), GenericStyleCustomizeMenu);
		}
		private void ConfirmHeight()
		{
			byte min, max;
			if (Measurement.UsesMetric)
			{
				min = MIN_HEIGHT_CM;
				max = MAX_HEIGHT_CM;
			}
			else
			{
				min = MIN_HEIGHT_IN;
				max = MAX_HEIGHT_CM;
			}
			currentDisplay.ClearOutput();
			bool successful = int.TryParse(currentDisplay.GetOutput(), out int parsedInt);

			if (!successful)
			{
				currentDisplay.OutputText(InvalidHeightStr(currentDisplay.GetOutput()));
			}
			else if (parsedInt < min || parsedInt > max)
			{
				currentDisplay.OutputText(InvalidHeightStr(parsedInt));
				currentDisplay.DoNext(ChooseHeight); //off to the heightInInches selection!
				return;
			}
			else
			{
				if (Measurement.UsesMetric)
				{
					creator.heightInInches = (byte)Math.Round(parsedInt * Measurement.TO_INCHES);
				}
				else
				{
					creator.heightInInches = (byte)parsedInt;
				}

			}
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ConfirmHeightStr());
			currentDisplay.DoYesNo(GenericStyleCustomizeMenu, ChooseHeight);
		}
		#endregion
		#region Cock
		//----------------- COCK LENGTH -----------------

		private const byte MIN_COCK_IN = 4;
		private const byte MIN_COCK_CM = 10;
		private const byte MAX_COCK_IN = 8;
		private const byte MAX_COCK_CM = 20;

		private void MenuCockLength()
		{
			bool buttonMaker(byte index, float val, bool metric) => currentDisplay.AddButton(index, Measurement.ToNearestHalfSmallUnit(val, true, false),
				() => ChooseCockLength(metric ? (float)(val * Measurement.TO_INCHES) : val));
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(CockLengthStr());
			byte count = 0;
			float size, max, delta;
			if (Measurement.UsesMetric)
			{
				size = MIN_COCK_CM;
				max = MAX_COCK_CM;
				delta = 1;
			}
			else
			{
				size = MIN_COCK_IN;
				max = MAX_COCK_IN;
				delta = 0.5f;
			}

			while (count < 14 && size <= max)
			{
				if (count == 4 || count == 9) count++;
				buttonMaker(count++, size, Measurement.UsesMetric);
				size += delta;
			}

			currentDisplay.AddButton(14, GlobalStrings.BACK(), GenericStyleCustomizeMenu);
		}

		private void ChooseCockLength(float length)
		{
			creator.cocks[0].length = length;
			creator.cocks[0].girth = (length / 5) - 0.1f;
			GenericStyleCustomizeMenu();
		}
		#endregion
		#region Clit
		//Shamelessly copied from Cock section, but with slightly different values. Sue Me - JSG
		private const float MIN_CLIT_IN = 0.25f;
		private const float MIN_CLIT_CM = 1.5f;
		private const float MAX_CLIT_IN = 2;
		private const float MAX_CLIT_CM = 5f;

		private void MenuClitLength()
		{
			bool buttonMaker(byte index, float val, bool metric) => currentDisplay.AddButton(index, Measurement.ToNearestQuarterInchOrHalfCentimeter(val, true),
				() => ChooseClitLength(metric ? (float)(val * Measurement.TO_INCHES) : val));
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ClitLengthStr());
			byte count = 0;
			float size, max, delta;
			if (Measurement.UsesMetric)
			{
				size = MIN_CLIT_CM;
				max = MAX_CLIT_CM;
				delta = 0.5f;
			}
			else
			{
				size = MIN_COCK_IN;
				max = MAX_COCK_IN;
				delta = 0.25f;
			}

			while (count < 14 && size <= max)
			{
				if (count == 4 || count == 9) count++;
				buttonMaker(count++, size, Measurement.UsesMetric);
				size += delta;
			}

			currentDisplay.AddButton(14, GlobalStrings.BACK(), GenericStyleCustomizeMenu);
		}

		private void ChooseClitLength(float length)
		{
			creator.vaginas[0].clitLength = length;
			GenericStyleCustomizeMenu();
		}
		#endregion
		#region Breasts
		//----------------- BREAST SIZE -----------------
		private void menuBreastSize()
		{
			bool buttonMaker(byte index, CupSize cup, bool condition) => currentDisplay.AddButtonIf(index, cup.AsText(), condition, () => ChooseBreastSize(cup));
			currentDisplay.ClearOutput();
			currentDisplay.OutputText(ChooseBreastStr());

			buttonMaker(0, CupSize.FLAT, creator.femininity < 50);
			buttonMaker(1, CupSize.A, creator.femininity < 60);
			buttonMaker(2, CupSize.B, creator.femininity >= 40);
			buttonMaker(3, CupSize.C, creator.femininity >= 50);
			buttonMaker(4, CupSize.D, creator.femininity >= 60);
			buttonMaker(5, CupSize.DD, creator.femininity >= 70);
			currentDisplay.AddButton(14, GlobalStrings.BACK(), GenericStyleCustomizeMenu);
		}

		private void ChooseBreastSize(CupSize size)
		{
			creator.breasts[0].cupSize = size;
			//remember null < anything is false, and null > anything is false.
			//so these are fine. woo!
			if (size > CupSize.C && (creator.nippleLength is null || creator.nippleLength < 0.5f))
			{
				creator.nippleLength = 0.5f;
			}
			else if (size == CupSize.FLAT && (creator.nippleLength is null || creator.nippleLength < 0.25f))
			{
				creator.nippleLength = 0.25f;
			}
			GenericStyleCustomizeMenu();
		}
		#endregion

		private void ChooseEndowment()
		{
			if (!endowmentLocked)
			{
				currentDisplay.ClearOutput();
				//OutputImage("event-question"));
				currentDisplay.OutputText(EndowmentQuestionStr());

				List<EndowmentPerkBase> data = EndowmentPerkBase.endowmentPerks.Select(x => x()).Where(x => x.IsUnlocked(chosenGender)).ToList();

				if (data.Count <= 15)
				{
					for (byte x = 0; x < data.Count; x++)
					{
						byte y = x;
						currentDisplay.AddButton(x, data[x].ButtonText(), () => ConfirmEndowment(data[y]));
					}
				}
				else
				{
					throw new InDevelopmentExceptionThatBreaksOnRelease();
				}
			}
			else
			{
				ChooseHistory();
			}
		}



		private void ConfirmEndowment(EndowmentPerkBase endowment)
		{
			currentDisplay.ClearOutput();
			//OutputImage("event-question"));

			currentDisplay.OutputText(endowment.UnlockEndowmentText());

			currentDisplay.AddButton(0, GlobalStrings.YES(), () => ActivateEndowment(endowment));
			currentDisplay.AddButton(1, GlobalStrings.NO(), ChooseEndowment);
		}

		private void ActivateEndowment(EndowmentPerkBase endowment)
		{
			if (creator.perks is null)
			{
				creator.perks = new List<PerkBase>();
			}

			creator.perks.Add(endowment);
			ChooseHistory();
		}

		private void ChooseHistory()
		{

			if (!historyLocked)
			{
				currentDisplay.ClearOutput();
				//OutputImage("event-question"));
				currentDisplay.OutputText(HistoryQuestionStr());

				List<HistoryPerkBase> data = HistoryPerkBase.historyPerks.Select(x => x()).ToList();

				if (data.Count <= 15)
				{
					for (byte x = 0; x < data.Count; x++)
					{
						byte y = x;
						currentDisplay.AddButton(x, data[x].ButtonText(), () => ConfirmHistory(data[y]));
					}
				}
				else
				{
					throw new InDevelopmentExceptionThatBreaksOnRelease();
				}
			}
			else
			{
				CreatePlayer();
			}
		}

		private void ConfirmHistory(HistoryPerkBase history)
		{
			currentDisplay.ClearOutput();
			//OutputImage("event-question"));

			currentDisplay.OutputText(history.UnlockHistoryText());

			currentDisplay.AddButton(0, GlobalStrings.YES(), () => ActivateHistory(history));
			currentDisplay.AddButton(1, GlobalStrings.NO(), ChooseHistory);
		}

		private void ActivateHistory(HistoryPerkBase history)
		{
			creator.perks.Add(history);

			CreatePlayer();
		}

		private void CreatePlayer()
		{
			PlayerBase player = new Player(creator);
			NewGameHelpers.ChooseSettings(player);
		}

		/*
				public const MAX_TOLERANCE_LEVEL:int = 20;
				public const MAX_MORALSHIFTER_LEVEL:int = 10;
				public const MAX_DESIRES_LEVEL:int = 10;
				public const MAX_ENDURANCE_LEVEL:int = 10;
				public const MAX_MYSTICALITY_LEVEL:int = 10;
				public const MAX_WISDOM_LEVEL:int = 5;
				public const MAX_FORTUNE_LEVEL:int = -1; //no max level
				public const MAX_VIRILITY_LEVEL:int = 15;
				public const MAX_FERTILITY_LEVEL:int = 15;
				public const NEW_GAME_PLUS_RESET_CLIT_LENGTH_MAX:Number = 1.5;

				/*



				//----------------- GENERAL STYLE -----------------

				//----------------- SKIN COLOURS -----------------

				//----------------- STARTER PERKS -----------------



				//----------------- GAME MODES -----------------
				private void chooseModeDifficulty()
				{
					if (flags[kFLAGS.GAME_DIFFICULTY] < 3) flags[kFLAGS.GAME_DIFFICULTY]++;
					else flags[kFLAGS.GAME_DIFFICULTY] = 0;
					chooseGameModes();
				}
				private void chooseModeSurvival()
				{
					if (flags[kFLAGS.HUNGER_ENABLED] < 1)
					{
						flags[kFLAGS.HUNGER_ENABLED] += 0.5;
						creator.hunger = 80;
					}
					else
					{
						flags[kFLAGS.HUNGER_ENABLED] = 0;
						creator.hunger = 0;
					}
					chooseGameModes();
				}
				private void chooseModeHardcore()
				{
					if (flags[kFLAGS.HARDCORE_MODE] == 0) flags[kFLAGS.HARDCORE_MODE] = 1;
					else flags[kFLAGS.HARDCORE_MODE] = 0;
					chooseGameModes();
				}
				private void chooseModeHardcoreSlot()
				{
					currentDisplay.ClearOutput();
					currentDisplay.OutputText("You have chosen Hardcore Mode. In this mode, the game forces autosave and if you encounter a Bad End, your save file is <b>DELETED</b>! \n\nDebug Mode and Easy Mode are disabled in this game mode. \n\nPlease choose a slot to save in. You may not make multiple copies of saves.");
					menu();
					for (var i:int = 0; i < 14; i++)
			{
						currentScene.AddButton(i, "Slot " + (i + 1), function(slot: int) :*
			{
							flags[kFLAGS.HARDCORE_SLOT] = "CoC_" + slot;
							startTheGame();
						}, i + 1);
					}
					currentScene.AddButton(14, "Back", chooseGameModes);
				}
				//GRIMDARK!
				private void chooseModeGrimdark()
				{
					currentDisplay.ClearOutput();
					currentDisplay.OutputText("You have chosen Grimdark Mode. This will drastically alter gameplay and there will be a lot of new obstacles. Enemies are beefed up and the game will be much darker and edgier with plenty of environment changes. Is this what you choose?");
					flags[kFLAGS.GRIMDARK_MODE] = 1;
					flags[kFLAGS.HUNGER_ENABLED] = 1;
					flags[kFLAGS.GAME_DIFFICULTY] = 3;
					if (flags[kFLAGS.GRIMDARK_BACKGROUND_UNLOCKED] == 0) flags[kFLAGS.BACKGROUND_STYLE] = 9;
					creator.hunger = 80;
					doNext(startTheGame);
				}
				//Choose the game mode when called!
				private void chooseGameModes()
				{
					currentDisplay.ClearOutput();
					currentDisplay.OutputText(images.showImage("event-creation"));
					currentDisplay.OutputText("Choose a game mode.\n\n");
					currentDisplay.OutputText("<b>Survival:</b> ");
					if (flags[kFLAGS.HUNGER_ENABLED] == 0) currentDisplay.OutputText("Normal Mode. You don't have to eat.\n");
					if (flags[kFLAGS.HUNGER_ENABLED] == 0.5) currentDisplay.OutputText("Survival Mode. You get hungry from time to time.\n");
					if (flags[kFLAGS.HUNGER_ENABLED] == 1) currentDisplay.OutputText("Realistic Mode. You get hungry from time to time and cum production is capped. In addition, it's a bad idea to have oversized parts.\n");
					currentDisplay.OutputText("<b>Hardcore:</b> ");
					if (flags[kFLAGS.HARDCORE_MODE] == 0) currentDisplay.OutputText("Normal Mode. You choose when you want to save and load.\n");
					if (flags[kFLAGS.HARDCORE_MODE] == 1) currentDisplay.OutputText("Hardcore Mode. The game forces save and if you get a Bad End, your save file is deleted. Disables difficulty selection, debug mode, Low Standarts and Hyper Happy mode once the game is started. For the veteran CoC creators only.\n");
					currentDisplay.OutputText("<b>Difficulty:</b> ");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 0) currentDisplay.OutputText("Normal Mode. No stats changes. Game is nice and simple.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 1) currentDisplay.OutputText("Hard Mode. Enemies have would have extra 25% HP and 15% damage.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 2) currentDisplay.OutputText("Nightmare Mode. Enemies would have extra 50% HP and 30% damage.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 3) currentDisplay.OutputText("Extreme Mode. Enemies would have extra 100% HP and 50% damage.\n");
					if (debug) currentDisplay.OutputText("<b>Grimdark mode:</b> (In dev) In the grimdark future, there are only rape and corruptions. Lots of things are changed and Lethice has sent out her minions to wall the borders and put up a lot of puzzles. Can you defeat her in this mode in as few bad ends as possible?\n");
					menu();
					currentScene.AddButton(0, "Survival", chooseModeSurvival);
					currentScene.AddButton(1, "Hardcore", chooseModeHardcore);
					currentScene.AddButton(2, "Difficulty", chooseModeDifficulty);
					if (debug) currentScene.AddButton(3, "Grimdark", chooseModeGrimdark);
					currentScene.AddButton(4, "Start!", flags[kFLAGS.HARDCORE_MODE] == 1 ? chooseModeHardcoreSlot : startTheGame);
				}

				private void startTheGame()
				{
					creator.startingRace = creator.race;
					if (flags[kFLAGS.HARDCORE_MODE] > 0) getGame().saves.saveGame(flags[kFLAGS.HARDCORE_SLOT])
					if (flags[kFLAGS.GRIMDARK_MODE] > 0) flags[kFLAGS.BACKGROUND_STYLE] = 9;
					kGAMECLASS.saves.loadPermObject();
					flags[kFLAGS.MOD_SAVE_VERSION] = kGAMECLASS.modSaveVersion;
					statScreenRefresh();
					chooseToPlay();
					return;
				}
				public void chooseToPlay()
				{
					if (flags[kFLAGS.NEW_GAME_PLUS_LEVEL] == 0)
					{
						if (creator.femininity >= 55) creator.setUndergarment(undergarments.C_PANTY);
						else creator.setUndergarment(undergarments.C_LOIN);
						if (creator.biggestTitSize() >= 2) creator.setUndergarment(undergarments.C_BRA);
					}
					if (flags[kFLAGS.GRIMDARK_MODE] > 0)
					{
						arrival();
						return;
					}
					currentDisplay.ClearOutput();
					currentDisplay.OutputText(images.showImage("location-ingnam"));
					currentDisplay.OutputText("Would you like to play through the 3-day prologue in Ingnam or just skip?");
					doYesNo(goToIngnam, arrival);
				}
				public void goToIngnam()
				{
					getGame().time.days = -3;
					getGame().time.hours = 8;
					flags[kFLAGS.IN_INGNAM] = 1;
					kGAMECLASS.ingnam.menuIngnam();
				}
			//		//------------ ASCENSION ------------
			//		public void ascensionMenu()
			//		{
			//			hideStats();
			//			currentDisplay.ClearOutput();
			//			hideMenus();
			//			mainView.nameBox.visible = false;
			//			if (creator.hasVagina()) outputText(images.showImage("camp-ascension-female"));
			//			else outputText(images.showImage("camp-ascension-male"));
			//			kGAMECLASS.displayHeader("Ascension");
			//			outputText("The world you have departed is irrelevant and you are in an endless black void dotted with tens of thousands of stars. You encompass everything and everything encompasses you.");
			//			outputText("\n\nAscension Perk Points: " + creator.ascensionPerkPoints);
			//			outputText("\n\n(When you're done, select Reincarnate.)");
			//			menu();
			//			addButton(0, "Perk Select", ascensionPerkMenu).hint("Spend Ascension Perk Points on special perks!", "Perk Selection");
			//			addButton(1, "Perm Perks", ascensionPermeryMenu).hint("Spend Ascension Perk Points to make certain perks permanent.", "Perk Selection");
			//			addButton(2, "Respec", respecLevelPerks).hint("Respec all level-up perks for 5 Ascension Perk Points?");
			//			addButton(3, "Rename", renamePrompt).hint("Change your name at no charge?");
			//			addButton(4, "Reincarnate", reincarnatePrompt).hint("Reincarnate and start an entirely new adventure?");
			//		}
			//		//Perk Selection
			//		private void ascensionPerkMenu()
			//		{
			//			currentDisplay.ClearOutput();
			//			outputText("You can spend your Ascension Perk Points on special perks not available at level-up!");
			//			outputText("\n\nAscension Perk Points: " + creator.ascensionPerkPoints);
			//			menu();
			//			addButton(0, "Desires", ascensionPerkSelection, PerkLib.AscensionDesires, MAX_DESIRES_LEVEL, null, PerkLib.AscensionDesires.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionDesires) + " / " + MAX_DESIRES_LEVEL);
			//			addButton(1, "Endurance", ascensionPerkSelection, PerkLib.AscensionEndurance, MAX_ENDURANCE_LEVEL, null, PerkLib.AscensionEndurance.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionEndurance) + " / " + MAX_ENDURANCE_LEVEL);
			//			addButton(2, "Fertility", ascensionPerkSelection, PerkLib.AscensionFertility, MAX_FERTILITY_LEVEL, null, PerkLib.AscensionFertility.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionFertility) + " / " + MAX_FERTILITY_LEVEL);
			//			addButton(3, "Fortune", ascensionPerkSelection, PerkLib.AscensionFortune, MAX_FORTUNE_LEVEL, null, PerkLib.AscensionFortune.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionFortune) + " (No maximum level)");
			//			addButton(4, "Moral Shifter", ascensionPerkSelection, PerkLib.AscensionMoralShifter, MAX_MORALSHIFTER_LEVEL, null, PerkLib.AscensionMoralShifter.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionMoralShifter) + " / " + MAX_MORALSHIFTER_LEVEL);
			//			addButton(5, "Mysticality", ascensionPerkSelection, PerkLib.AscensionMysticality, MAX_MYSTICALITY_LEVEL, null, PerkLib.AscensionMysticality.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionMysticality) + " / " + MAX_MYSTICALITY_LEVEL);
			//			addButton(6, "Tolerance", ascensionPerkSelection, PerkLib.AscensionTolerance, MAX_TOLERANCE_LEVEL, null, PerkLib.AscensionTolerance.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionTolerance) + " / " + MAX_TOLERANCE_LEVEL);
			//			addButton(7, "Virility", ascensionPerkSelection, PerkLib.AscensionVirility, MAX_VIRILITY_LEVEL, null, PerkLib.AscensionVirility.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionVirility) + " / " + MAX_VIRILITY_LEVEL);
			//			addButton(8, "Wisdom", ascensionPerkSelection, PerkLib.AscensionWisdom, MAX_WISDOM_LEVEL, null, PerkLib.AscensionWisdom.longDesc + "\n\nCurrent level: " + creator.perkv1(PerkLib.AscensionWisdom) + " / " + MAX_WISDOM_LEVEL);
			//			addButton(14, "Back", ascensionMenu);
			//		}
			//		private void ascensionPerkSelection(perk:* = null, maxLevel:int = 10)
			//		{
			//			currentDisplay.ClearOutput();
			//			outputText("Perk Effect: " + perk.longDesc);
			//			outputText("\nCurrent level: " + creator.perkv1(perk) + (maxLevel > 0 ? " / " + maxLevel : " (No maximum level)") + "");
			//			if (creator.perkv1(perk) >= maxLevel && maxLevel > 0) outputText(" <b>(Maximum)</b>");
			//			var cost:int = creator.perkv1(perk) + 1;
			//			if (cost > 5) cost = 5;
			//			if (creator.perkv1(perk) < maxLevel || maxLevel < 0) outputText("\nCost for next level: " + cost);
			//			else outputText("\nCost for next level: <b>N/A</b>");
			//			outputText("\n\nAscension Perk Points: " + creator.ascensionPerkPoints);
			//			menu();
			//			if (creator.ascensionPerkPoints >= cost && (creator.perkv1(perk) < maxLevel || maxLevel < 0)) addButton(0, "Add 1 level", addAscensionPerk, perk, maxLevel);
			//			addButton(4, "Back", ascensionPerkMenu);
			//		}
			//		private void addAscensionPerk(perk:* = null, maxLevel:int = 10)
			//		{
			//			var cost:int = creator.perkv1(perk) + 1;
			//			if (cost > 5) cost = 5;
			//			creator.ascensionPerkPoints -= cost;
			//			if (creator.findPerk(perk) >= 0) creator.addPerkValue(perk, 1, 1);
			//			else creator.createPerk(perk, 1, 0, 0, 0);
			//			ascensionPerkSelection(perk, maxLevel);
			//		}
			//		//Perk Permery
			//		private void ascensionPermeryMenu(page:int = 1)
			//		{
			//			currentDisplay.ClearOutput();
			//			outputText("For the price of a few points, you can make certain perks permanent and they will carry over in future ascensions. In addition, if the perks come from transformations, they will stay even if you no longer meet the requirements.");
			//			outputText("\n\nCurrent Cost: " + permanentizeCost() + " Ascension Points");
			//			outputText("\n\nAscension Perk Points: " + creator.ascensionPerkPoints);
			//			var button:int = 0;
			//			var countBeforeAdding:int = (page - 1) * 12;
			//			menu();
			//			for (var i:int = 0; i < creator.perks.length; i++)
			//{
			//				if (isPermable(creator.perks[i].ptype) && button < 14)
			//				{
			//					if (countBeforeAdding > 0)
			//						countBeforeAdding--; //decrement count before adding buttons
			//					else
			//					{ //add buttons when the count reaches zero
			//						if (creator.perks[i].value4 == 0)
			//							addButton(button++, creator.perks[i].ptype.id, permanentizePerk, creator.perks[i].ptype, null, null, creator.perks[i].ptype.desc(creator.perks[i]));
			//						else addButtonDisabled(button++, creator.perks[i].ptype.id, "This perk is already made permanent and will carry over in all subsequent ascensions.");
			//					}
			//				}
			//				//Skip slots reserved for next and previous
			//				if (button == 4) button++;
			//				if (button == 9) button++;
			//			}
			//			//Next and previous page buttons depending on conditions
			//			if (button >= 14) addButton(4, "Next", ascensionPermeryMenu, page + 1);
			//			if (page > 1) addButton(9, "Previous", ascensionPermeryMenu, page - 1);
			//			addButton(14, "Back", ascensionMenu);
			//		}
			//		private void permanentizePerk(perk:PerkType)
			//		{
			//			if (creator.ascensionPerkPoints < permanentizeCost()) return; //Not enough points? Cancel
			//			if (creator.perkv4(perk) > 0) return; //Perk already permed? Cancel
			//			creator.ascensionPerkPoints -= permanentizeCost(); //deduct points
			//			creator.addPerkValue(perk, 4, 1); //permanentize a perk
			//			ascensionPermeryMenu();
			//		}
			//		private int permanentizeCost()
			//		{
			//			var count:int = 1;
			//			for each(var perk: PerkType in PerkLists.PERMEABLE)
			//				if (creator.perkv4(perk) > 0) count++;
			//			return count;
			//		}
			//		private Boolean isPermable(perk:PerkType)
			//		{
			//			return PerkLists.PERMEABLE.indexOf(perk) !== -1;
			//		}
			//		//Respec
			//		private void respecLevelPerks()
			//		{
			//			currentDisplay.ClearOutput();
			//			if (creator.ascensionPerkPoints < 5)
			//			{
			//				outputText("You need at least 5 Ascension Perk Points to respec level-up perks. You have " + creator.ascensionPerkPoints + ".");
			//				doNext(ascensionMenu);
			//				return;
			//			}
			//			if (creator.perkPoints == creator.level - 1)
			//			{
			//				outputText("There is no need to respec as you've already resetted your level-up perks.");
			//				doNext(ascensionMenu);
			//				return;
			//			}
			//			creator.ascensionPerkPoints -= 5;
			//			creator.perkPoints = creator.level - 1;
			//			var ascendPerkTemp:Array = [];
			//			for (var i:int = 0; i < creator.perks.length; i++)
			//				if (isAscensionPerk(creator.perks[i], true)) ascendPerkTemp.push(creator.perks[i]);
			//			creator.removePerks();
			//			if (ascendPerkTemp.length > 0)
			//				for (i = 0; i < ascendPerkTemp.length; i++)
			//					creator.createPerk(ascendPerkTemp[i].ptype, ascendPerkTemp[i].value1, ascendPerkTemp[i].value2, ascendPerkTemp[i].value3, ascendPerkTemp[i].value4);
			//			outputText("Your level-up perks are now reset and you are refunded the perk points.");
			//			doNext(ascensionMenu);
			//		}
			//		//Rename
			//		private void renamePrompt()
			//		{
			//			currentDisplay.ClearOutput();
			//			outputText("You may choose to change your name.");
			//			mainView.promptCharacterName();
			//			mainView.nameBox.text = creator.short;
			//			menu();
			//			addButton(0, "OK", chooseName);
			//			addButton(4, "Back", ascensionMenu);
			//			//Workaround
			//			mainView.nameBox.x = mainView.mainText.x + 5;
			//			mainView.nameBox.y = mainView.mainText.y + 3 + mainView.mainText.textheightInInches;
			//		}
			//		private void reincarnatePrompt()
			//		{
			//			currentDisplay.ClearOutput();
			//			outputText("Would you like to reincarnate and start a new life as a Champion?");
			//			doYesNo(reincarnate, ascensionMenu);
			//		}
			//		protected void reincarnate()
			//		{
			//			flags[kFLAGS.NEW_GAME_PLUS_LEVEL]++;
			//			customcreatorProfile = null;
			//			newGameGo();
			//			currentDisplay.ClearOutput();
			//			mainView.nameBox.visible = false;
			//			boxNames.visible = false;
			//			outputText("Everything fades to white and finally... black. You can feel yourself being whisked back to reality as you slowly awaken in your room. You survey your surroundings and recognize almost immediately; you are in your room inside the inn in Ingnam! You get up and look around. ");
			//			if (creator.hasKeyItem("Camp - Chest") >= 0 || creator.hasKeyItem("Equipment Rack - Weapons") >= 0 || creator.hasKeyItem("Equipment Rack - Armor") >= 0 || creator.hasKeyItem(Inventory.STORAGE_JEWELRY_BOX) >= 0)
			//			{
			//				if (creator.hasKeyItem("Camp - Chest") >= 0)
			//				{
			//					outputText("\n\nYou take a glance at the chest; you don't remember having it inside your room. You open the chest and look inside. ");
			//					if (inventory.hasItemsInStorage()) outputText("Something clicks in your mind; they must be the old stuff you had from your previous incarnation");
			//					else outputText("It's empty and you let out a disappointed sigh.");
			//				}
			//				if (creator.hasKeyItem("Equipment Rack - Weapons") >= 0)
			//				{
			//					outputText("\n\nThere is a weapon rack. You look at it. ");
			//					if (inventory.weaponRackDescription()) outputText(" Something clicks in your mind; they must be the old weapons you had from your previous incarnation!");
			//					else outputText("It's empty and you let out a sigh but you know you can bring it to Mareth.");
			//				}
			//				if (creator.hasKeyItem("Equipment Rack - Armor") >= 0)
			//				{
			//					outputText("\n\nThere is an armor rack. You look at it. ");
			//					if (inventory.armorRackDescription()) outputText(" Something clicks in your mind; they must be the old armors you had from your previous incarnation!");
			//					else outputText("It's empty and you let out a sigh but you know you can bring it to Mareth.");
			//				}
			//				if (creator.hasKeyItem("Equipment Rack - Shields") >= 0)
			//				{
			//					outputText("\n\nThere is a shield rack. You look at it. ");
			//					if (inventory.shieldRackDescription()) outputText(" Something clicks in your mind; they must be the old shields you had from your previous incarnation!");
			//					else outputText("It's empty and you let out a sigh but you know you can bring it to Mareth.");
			//				}
			//				if (creator.hasKeyItem(Inventory.STORAGE_JEWELRY_BOX) >= 0)
			//				{
			//					outputText("\n\nThere is a jewelry box on the dresser. You walk over to the box, open it, and look inside. ");
			//					if (inventory.jewelryBoxDescription()) outputText(" It's making sense! The contents must be from your past adventures.");
			//					else outputText("It's empty and you let out a sigh but you know you can bring it to Mareth.");
			//				}
			//			}
			//			outputText("\n\nAfter looking around the room for a while, you look into the mirror and begin to recollect who you are...");
			//			creator.breastRows = new Vector.< BreastRow > ();
			//			creator.cocks = new Vector.< Cock > ();
			//			creator.vaginas = new Vector.< Vagina > ();
			//			doNext(routeToGenderChoiceReincarnation);
			//		}
			//		private void routeToGenderChoiceReincarnation()
			//		{
			//			currentDisplay.ClearOutput();
			//			genericGenderChoice();
			//		}
			//		private Boolean isAscensionPerk(perk:Perk, respec:Boolean = false)
			//		{ return perk.ptype.keepOnAscension(respec) || perk.value4 > 0; }
			//		private Boolean isSpecialKeyItem(keyName:* = null)
			//		{ return (keyName == "Camp - Chest" || keyName == "Camp - Murky Chest" || keyName == "Camp - Ornate Chest" || keyName == "Equipment Rack - Weapons" || keyName == "Equipment Rack - Armor" || keyName == "Equipment Rack - Shields" || keyName == Inventory.STORAGE_JEWELRY_BOX || keyName == "Backpack" || keyName == "Nieve's Tear"); }
			//		private Boolean isSpell(statusEffect:* = null)
			//		{ return (statusEffect == StatusEffects.KnowsCharge || statusEffect == StatusEffects.KnowsBlind || statusEffect == StatusEffects.KnowsWhitefire || statusEffect == StatusEffects.KnowsArouse || statusEffect == StatusEffects.KnowsHeal || statusEffect == StatusEffects.KnowsMight || statusEffect == StatusEffects.KnowsBlackfire); }
			*/
	}
}
