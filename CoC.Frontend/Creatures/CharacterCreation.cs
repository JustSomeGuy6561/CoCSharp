﻿//CharacterCreation.cs
//Description:
//Author: JustSomeGuy
//6/7/2019, 1:02 AM
using CoC.Backend;
using CoC.Backend.BodyParts;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.SaveData;
using CoC.Frontend.UI;
using CoC.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static CoC.Frontend.Engine.MenuHelpers;
using static CoC.Frontend.UI.ButtonManager;
using static CoC.Frontend.UI.TextOutput;
using static CoC.UI.Controller;
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

		//PlayerCreator playerCreator;
		//bool genderLocked, occupationLocked, buildLocked, complexionLocked, hairLocked,

		private readonly PlayerCreator creator;
#warning Make sure to set the locked values for history, endowment when implemented.
		private readonly bool genderLocked, buildLocked, complexionLocked, furLocked, hairLocked, historyLocked, endowmentLocked;
		private readonly bool heightLocked, eyesLocked, beardLocked, cockLocked, clitLocked, breastsLocked;
		private Gender chosenGender;

		//a character is considered locked if its perks and gender, build, skin tone, and hair are all known. 
		//a character is considered semi-locked if its gender, build, skin tone, and hair are all known.
		private bool isSemiLocked => !genderLocked && !buildLocked && !complexionLocked && !hairLocked;
		private bool isLocked => isSemiLocked && !historyLocked && !endowmentLocked;

		private readonly bool newGamePlus;

		private bool hermUnlocked => FrontendGlobalSave.data.UnlockedNewGameHerm;
		#region Constructor
		internal CharacterCreation(PlayerCreator specialCreator, bool isNewGamePlus = false)
		{
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

			furLocked = (creator.bodyType?.epidermisType.usesFur != true && creator.bodyType?.secondaryEpidermisType.usesFur != true) || !FurColor.IsNullOrEmpty(creator.furColor);

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

			//clean up any invalid data set in the player creator.
			creator.wombMaker = () => new PlayerWomb();
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
			ClearOutput();
			ClearButtons();
			if (genderLocked)
			{
				defaultGenderHelper(creator.defaultGender)();
			}
			else if (creator.defaultGender != null)
			{
				AddOutput(() => GenderQuestionWithDefault((Gender)creator.defaultGender));
				GenderOptions(defaultGenderHelper(creator.defaultGender));
			}
			else
			{
				AddOutput(GenderQuestion);
				GenderOptions();
			}
		}


		internal void SetGenderSpecial(bool isSpecial)
		{

			ClearOutput();
			if (isSpecial)
			{
				//also prints out text for locked or semi-locked characters.
				AddOutput(SpecialText);
				if (isLocked)
				{
					DoNext(CreatePlayer);
				}
				else if (isSemiLocked)
				{
					DoNext(ChooseEndowment);
				}
				else if (creator.defaultGender != null)
				{
					AddOutput(() => GenderQuestionWithDefault((Gender)creator.defaultGender));
					GenderOptions(defaultGenderHelper(creator.defaultGender));
				}
				else
				{
					AddOutput(GenderQuestion2);
					GenderOptions();
				}
			}
			else
			{
				AddOutput(NotSpecialText);
				GenderOptions();
			}
		}
		#endregion
		#region Gender
		private void GenderOptions(Action defaultAction = null)
		{
			AddButton(0, GlobalStrings.MAN, GenderMale);
			AddButton(1, GlobalStrings.WOMAN, GenderFemale);
			if (hermUnlocked)
			{
				AddButton(2, GlobalStrings.HERM, GenderHerm);
			}
			if (defaultAction != null)
			{
				AddButton(4, GlobalStrings.DEFAULT, defaultAction);
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
				ClearOutput();
				//OutputText(images.showImage("event-question"));
				AddOutput(() => BuildText(Gender.MALE));

				AddButton(0, () => MaleBuild(0), BuildLeanMale);
				AddButton(1, () => MaleBuild(1), BuildAverageMale);
				AddButton(2, () => MaleBuild(2), BuildThickMale);
				AddButton(3, () => MaleBuild(3), BuildGirlyMale);
				//if default enabled and can be overridden
				//AddButtonWithToolTip(4, GlobalStrings.DEFAULT, defaultBuildHelper(Gender.MALE, creator.thickness, creator.femininity), DefaultBuildHint());
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
				ClearOutput();
				//OutputText(images.showImage("event-question"));
				AddOutput(() => BuildText(Gender.FEMALE));
				AddButton(0, () => FemaleBuild(0), BuildSlenderFemale);
				AddButton(1, () => FemaleBuild(1), BuildAverageFemale);
				AddButton(2, () => FemaleBuild(2), BuildCurvyFemale);
				AddButton(3, () => FemaleBuild(3), BuildTomboyishFemale);
				//if default enabled and can be overridden
				//AddButtonWithToolTip(4, GlobalStrings.DEFAULT, defaultBuildHelper(Gender.FEMALE, creator.thickness, creator.femininity), DefaultBuildHint());
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
				ClearOutput();
				//OutputText(images.showImage("event-question"));
				AddOutput(() => BuildText(Gender.HERM));

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
			Triple<SimpleDescriptor> data = HermButtonData(index);
			return AddButtonWithToolTip(index, data.first, callback, data.second, data.third);
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
				ClearOutput();
				AddOutput(() => BuildText(Gender.GENDERLESS));

				GenderlessButtons(0, BuildGirlyMale);
				GenderlessButtons(1, BuildTomboyishFemale);
				GenderlessButtons(2, BuildAndrogynous);
				//if default enabled and can be overridden
				//AddButtonWithToolTip(4, GlobalStrings.DEFAULT, defaultBuildHelper(Gender.GENDERLESS, creator.thickness, creator.femininity), DefaultBuildHint());
			}
			else
			{
				defaultBuildHelper(chosenGender, creator.thickness, creator.femininity)();
			}
		}
		private bool GenderlessButtons(byte index, Action callback)
		{
			Triple<SimpleDescriptor> data = GenderlessButtonData(index);
			return AddButtonWithToolTip(index, data.first, callback, data.second, data.third);
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
				ClearOutput();
				//OutputText(images.showImage("event-question"));
				AddOutput(ComplexionText);
				AddButton(0, Tones.LIGHT.AsString, () => SetComplexion(Tones.LIGHT));
				AddButton(1, Tones.FAIR.AsString, () => SetComplexion(Tones.FAIR));
				AddButton(2, Tones.OLIVE.AsString, () => SetComplexion(Tones.OLIVE));
				AddButton(3, Tones.DARK.AsString, () => SetComplexion(Tones.DARK));
				if (!hitCustomizationMenu && !complexionLocked && !Tones.IsNullOrEmpty(creator.complexion)) //currently impossible. complexion locks if not null. may change in future, idk.
				{
					AddButton(4, GlobalStrings.DEFAULT, () => SetComplexion(creator.complexion));
				}
				AddButton(5, Tones.EBONY.AsString, () => SetComplexion(Tones.EBONY));
				AddButton(6, Tones.MAHOGANY.AsString, () => SetComplexion(Tones.MAHOGANY));
				AddButton(7, Tones.RUSSET.AsString, () => SetComplexion(Tones.RUSSET));

				if (hitCustomizationMenu)
				{
					AddButton(14, GlobalStrings.BACK, GenericStyleCustomizeMenu);
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
				ClearOutput();
				//OutputText(images.showImage("event-question"));
				AddOutput(() => ConfirmComplexionText(choice));
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
			ClearOutput();
			AddOutput(() => ChooseFurStr(primaryFur));

			AddButton(0, SolidColorStr, () => ChooseFurColor(primaryFur, true, false));
			AddButton(1, MultiColorStr, () => ChooseFurColor(primaryFur, true, true));
			if (!furLocked && !FurColor.IsNullOrEmpty(creator.furColor))
			{
				AddButton(4, GlobalStrings.DEFAULT, () => ChooseHairColor(false));
			}
		}

		private void ChooseFurColor(bool isPrimaryFur, bool isPrimaryColor, bool hasMultipleColors)
		{

			ClearOutput();
			AddOutput(ChooseFurColorStr);

			List<DropDownEntry> vars = HairFurColors.AvailableHairFurColors().ConvertAll(x => new DropDownEntry(x.AsString(), () => ConfirmFurColor(x, isPrimaryFur, isPrimaryColor, hasMultipleColors)));
			DropDownMenu.ActivateDropDownMenu(vars.ToArray());


			if (setPrimaryFur)
			{
				AddButton(9, GlobalStrings.BACK, FurOptions);
			}
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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

			ClearOutput();
			AddOutput(() => SelectedColor(color, isPrimaryFur, isPrimaryColor, isMulticolored));
			AddButton(0, GlobalStrings.CONFIRM, NextAction);
			if (setPrimaryFur)
			{
				AddButton(9, GlobalStrings.BACK, FurOptions);
			}
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
			}
		}

		private void ChooseFurPattern(bool isPrimaryFur)
		{
			void callback(FurMulticolorPattern pattern)
			{
				furBuilderPattern = pattern;
				SetColor(isPrimaryFur, true);
			}

			bool buttonMaker(byte index, FurMulticolorPattern pattern) => AddButton(index, () => pattern.AsString(), () => callback(pattern));

			ClearOutput();
			AddOutput(ChooseFurPatternStr);
			buttonMaker(0, FurMulticolorPattern.NO_PATTERN);
			buttonMaker(1, FurMulticolorPattern.MIXED);
			buttonMaker(2, FurMulticolorPattern.SPOTTED);
			buttonMaker(3, FurMulticolorPattern.STRIPED);

			if (setPrimaryFur)
			{
				AddButton(9, GlobalStrings.BACK, FurOptions);
			}
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
				ClearOutput();
				AddOutput(() => FurColorFirstRunStr(created));
				AddButton(0, FurOptionsStr, FurOptions);
				AddButton(1, GlobalStrings.CONTINUE, () => ChooseHairColor(false));
			}
			else
			{
				FurOptions();
			}
		}

		private bool hasFur => creator.bodyType?.epidermisType.usesFur == true || creator.bodyType?.secondaryEpidermisType.usesFur == true;
		private bool multiFurred => creator.bodyType?.epidermisType.usesFur == true && creator.bodyType?.secondaryEpidermisType.usesFur == true;

		private void FurOptions()
		{
			ClearOutput();
			AddOutput(FurOptionsText);

			AddButton(0, () => FurStr(true), () => ChooseFur(true));
			AddButton(1, () => FurTextureStr(true), () => ChooseFurTexture(true));
			if (multiFurred)
			{
				AddButton(2, () => FurStr(false), () => ChooseFur(false));
				AddButton(3, () => FurTextureStr(false), () => ChooseFurTexture(false));
			}
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
			}
			else
			{
				AddButton(14, GlobalStrings.CONTINUE, () => ChooseHairColor(false));
			}
		}

		private void ChooseFurTexture(bool isPrimary)
		{
			bool buttonMaker(byte index, FurTexture texture) => AddButton(index, () => texture.AsString(), () => SetFurTexture(texture, isPrimary));

			ClearOutput();
			AddOutput(() => ChooseTextureText(isPrimary));

			buttonMaker(0, FurTexture.FLUFFY);
			buttonMaker(1, FurTexture.SMOOTH);
			buttonMaker(2, FurTexture.SHINY);
			buttonMaker(3, FurTexture.SOFT);
			AddButton(4, GlobalStrings.DEFAULT, () => SetFurTexture(FurTexture.NONDESCRIPT, isPrimary));
			buttonMaker(5, FurTexture.MANGEY);

			if (setPrimaryFur)
			{
				AddButton(9, GlobalStrings.BACK, FurOptions);
			}
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
				SimpleDescriptor output = isHighlight ? (SimpleDescriptor)HighlightText : (SimpleDescriptor)HairText;
				AddOutput(output);
				ClearButtons();

				AddButton(0, HairFurColors.BLONDE.AsString, () => SetHair(HairFurColors.BLONDE, isHighlight));
				AddButton(1, HairFurColors.BROWN.AsString, () => SetHair(HairFurColors.BROWN, isHighlight));
				AddButton(2, HairFurColors.BLACK.AsString, () => SetHair(HairFurColors.BLACK, isHighlight));
				AddButton(3, HairFurColors.RED.AsString, () => SetHair(HairFurColors.RED, isHighlight));
				//if has a default hair color and we're in the first run of hair color.
				if (!isHighlight && !hitHairOptions && !HairFurColors.IsNullOrEmpty(creator.hairColor)) //currently can't be hit - the hair is locked if not null.
				{
					AddButton(4, GlobalStrings.DEFAULT, () => SetHair(creator.hairColor, false));
				}
				AddButton(5, HairFurColors.GRAY.AsString, () => SetHair(HairFurColors.GRAY, isHighlight));
				AddButton(6, HairFurColors.WHITE.AsString, () => SetHair(HairFurColors.WHITE, isHighlight));
				AddButton(7, HairFurColors.AUBURN.AsString, () => SetHair(HairFurColors.AUBURN, isHighlight));


				if (hitHairOptions)
				{
					AddButton(9, GlobalStrings.BACK, HairOptions);
				}
				if (hitCustomizationMenu)
				{
					AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
				ClearOutput();
				AddOutput(() => ConfirmHairFirstRun(color));
				AddButton(0, HairOptionStr, HairOptions);
				AddButton(1, GlobalStrings.CONTINUE, GenericStyleCustomizeMenu);
			}
			else
			{
				HairOptions();
			}
		}

		//either all hair options are locked or none are. 

		private void HairOptions()
		{
			AddOutput(() => HairOptionsText(hitCustomizationMenu));
			hitHairOptions = true;

			AddButton(0, HairColorStr, () => ChooseHairColor(false));
			AddButton(1, HighlightColorStr, () => ChooseHairColor(true));
			AddButton(2, HairLengthStr, () => ChooseHairLength());
			AddButton(3, HairStyleStr, ChooseHairStyle);

			//did we come from customization? 
			SimpleDescriptor buttonText = hitCustomizationMenu ? (SimpleDescriptor)GlobalStrings.RETURN : (SimpleDescriptor)GlobalStrings.CONTINUE;
			AddButton(14, buttonText, GenericStyleCustomizeMenu);
		}

		private void ChooseHairLength(bool clearOutput = true)
		{
			if (clearOutput)
			{
				ClearOutput();
			}
			AddOutput(ChooseHairLengthStr);
			InputField.ActivateInputField(InputField.POSITIVE_NUMBERS, creator.hairLength?.ToString() ?? "", HairLengthStr()); //null operators ftw! basically, if hairLength is null, empty str. if not, call its ToString.
																															   //activate input field. 
																															   //set default input value to current hair length.
																															   //go to town.
			AddButton(0, GlobalStrings.CONFIRM, SetHairLength);
			AddButton(9, GlobalStrings.BACK, HairOptions);
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
			}
		}

		private void SetHairLength()
		{
			ClearOutput();
			InputField.DeactivateInputField();
			if (float.TryParse(InputField.output, out float parsedInput))
			{
				if (parsedInput > creator.heightInInches)
				{
					AddOutput(() => HairTooLongStr(parsedInput));
					ChooseHairLength(false);
				}
				else if (parsedInput < 0)
				{
					AddOutput(() => NegativeNumberHairStr(parsedInput));
					ChooseHairLength(false);
				}
				else
				{
					creator.hairLength = parsedInput;
					HairOptions();
				}
			}
		}

		private void ChooseHairStyle()
		{
			bool buttonMaker(byte index, HairStyle style) => AddButton(index, () => style.AsString(), () => SetHairStyle(style));

			ClearOutput();
			AddOutput(ChooseHairStyleStr);

			buttonMaker(0, HairStyle.STRAIGHT);
			buttonMaker(1, HairStyle.CURLY);
			buttonMaker(2, HairStyle.WAVY);
			buttonMaker(3, HairStyle.COILED);
			//we don't need a default, but the common spacing was 4 = default.
			buttonMaker(5, HairStyle.MESSY);
			buttonMaker(6, HairStyle.PONYTAIL);
			buttonMaker(7, HairStyle.BRAIDED);

			AddButton(9, GlobalStrings.BACK, HairOptions);
			if (hitCustomizationMenu)
			{
				AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
			hitCustomizationMenu = true;
			ClearOutput();
			//mainView.nameBox.visible = false;
			//mainView.nameBox.maxChars = 16;
			//mainView.nameBox.restrict = null;
			//OutputText(images.showImage("event-creation"));

			AddOutput(GenericCustomizationText);

			//reuse the old menus - why be redundant? This way there's less code to maintain.
			AddButtonOrAddDisabledWithToolTip(0, ComplexionStr, complexionLocked, ChooseComplexion, ComplexionLockedStr);
			AddButtonOrAddDisabledWithToolTip(1, HairOptionStr, hairLocked, HairOptions, HairLockedStr); //include highlight and hairStyle here, as well as length?

			//if (canGrowBeard)
			//{
			//	AddButtonOrAddDisabledWithToolTip(2, BeardOptionStr(), beardLocked, MenuBeardSettings, BeardLockedStr());
			//}

			AddButtonOrAddDisabledWithToolTip(3, EyeColorStr, eyesLocked, MenuEyeColor, EyeLockedStr); //include heterochromea option here
			if (hasFur) AddButtonOrAddDisabledWithToolTip(4, FurColorStr, furLocked, FurOptions, FurLockedStr);
			AddButtonOrAddDisabledWithToolTip(5, HeightStr, heightLocked, ChooseHeight, HeightLockedStr);
			if (chosenGender.HasFlag(Gender.MALE)) AddButtonOrAddDisabledWithToolTip(6, CockSizeStr, cockLocked, MenuCockLength, CockLockedStr);
			if (chosenGender.HasFlag(Gender.FEMALE)) AddButtonOrAddDisabledWithToolTip(7, ClitSizeStr, clitLocked, MenuClitLength, ClitLockedStr);
			AddButtonOrAddDisabledWithToolTip(8, BreastSizeStr, breastsLocked, menuBreastSize, BreastsLockedStr);
			AddButton(9, GlobalStrings.CONTINUE, ChooseEndowment);
		}
		#endregion
		#region Beards
		//----------------- BEARD STYLE -----------------
		//private bool canGrowBeard => chosenGender == Gender.MALE || (chosenGender != Gender.FEMALE && creator.femininity <= MIN_MASCULINE && creator.breasts[0].cupSize < CupSize.C);

		//private void MenuBeardSettings()
		//{
		//	ClearOutput();
		//	OutputText("You can choose your beard length and style.\n\n");
		//	OutputText("Beard: " + );

		//	AddButton(0, "Style", menuBeardStyle);
		//	AddButton(1, "Length", menuBeardLength);
		//	AddButton(14, "Back", GenericStyleCustomizeMenu);
		//}
		//private void menuBeardStyle()
		//{
		//	ClearOutput();
		//	//OutputText(images.showImage("event-question"));
		//	OutputText("What beard style would you like?");

		//	AddButton(0, "Normal", () => chooseBeardStyle());
		//	AddButton(1, "Goatee", () => chooseBeardStyle());
		//	AddButton(2, "Clean-cut", () => chooseBeardStyle());
		//	AddButton(3, "Mountainman", () => chooseBeardStyle());
		//	AddButton(9, GlobalStrings.BACK, menuBeardSettings);
		//	AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
		//}

		//private void chooseBeardStyle(BeardStyle beardStyle)
		//{
		//	creator.beardStyle = beardStyle;
		//	menuBeardSettings();
		//}
		//private void menuBeardLength()
		//{
		//	ClearOutput();
		//	//OutputText(images.showImage("event-question"));
		//	OutputText("How long would you like your beard be? \n\nNote: Beard will slowly grow over time, just like in the real world. Unless you have no beard. You can change your beard style later in the game.");
		//	AddButton(0, "No Beard", () => chooseBeardLength(0));
		//	AddButton(1, "Trim", () => chooseBeardLength(0.1f));
		//	AddButton(2, "Short", () => chooseBeardLength(0.2f));
		//	AddButton(3, "Medium", () => chooseBeardLength(5f));
		//	AddButton(4, "Mod. Long", () => chooseBeardLength(5f));
		//	AddButton(5, "Long", () => chooseBeardLength(3f));
		//	AddButton(6, "Very Long", () => chooseBeardLength(6f));
		//	AddButton(9, GlobalStrings.BACK, menuBeardSettings);
		//	AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
			AddButton(0, MonoChromaticStr, () => ChooseEyeColor(true, true));
			AddButton(1, HeteroChromaticStr, () => ChooseEyeColor(true, false));
		}

		private EyeColor? left, right;
		private void ChooseEyeColor(bool isLeftEye, bool isBothEyes)
		{
			left = null;
			right = null;

			ClearOutput();
			AddOutput(ChooseEyeStr);
			ChooseEyeColor2(true, isLeftEye, isBothEyes);
		}

		private void ChooseEyeColor2(bool isPage1, bool isLeftEye, bool isBothEyes)
		{
			bool buttonMaker(byte index, EyeColor color) => AddButton(index, () => color.AsString(), () => SetEyeColor(color, isLeftEye, isBothEyes));

			ClearButtons();
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
				ButtonNextPage(() => ChooseEyeColor2(true, isLeftEye, isBothEyes), 10);
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
				ButtonPreviousPage(() => ChooseEyeColor2(true, isLeftEye, isBothEyes));
			}

			if (!isBothEyes && !isLeftEye)
			{
				if (creator.rightEyeColor != null || creator.leftEyeColor != null && creator.leftEyeColor != left)
				{
					EyeColor color;
					color = creator.rightEyeColor != null ? (EyeColor)creator.rightEyeColor : (EyeColor)creator.leftEyeColor;
					AddButtonWithToolTip(4, GlobalStrings.DEFAULT, () => SetEyeColor(color, false, false), () => UseCurrentEyeColor(false));
				}
				AddButtonWithToolTip(9, MonoChromaticStr, () => SetEyeColor((EyeColor)left, true, true), IChangedMyMindIllTakeMonochromaticEyesFor200Alex);
			}
			else if (!isBothEyes && creator.leftEyeColor != null)
			{
				AddButtonWithToolTip(4, GlobalStrings.DEFAULT, () => SetEyeColor((EyeColor)creator.leftEyeColor, true, false), () => UseCurrentEyeColor(true));
			}
			AddButton(14, GlobalStrings.RETURN, GenericStyleCustomizeMenu);
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
			ClearOutput();
			AddOutput(SetHeightStr);

			if (creator.heightInInches < 48)
			{
				creator.heightInInches = 48;
			}

			InputField.ActivateInputField(InputField.POSITIVE_NUMBERS, creator.heightInInches.ToString(), HeightStr());

			AddButton(0, GlobalStrings.OK, ConfirmHeight);
			AddButton(4, GlobalStrings.BACK, GenericStyleCustomizeMenu);
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
			ClearOutput();
			bool successful = int.TryParse(InputField.output, out int parsedInt);

			if (!successful)
			{
				AddOutput(() => InvalidHeightStr(InputField.output));
			}
			else if (parsedInt < min || parsedInt > max)
			{
				AddOutput(() => InvalidHeightStr(parsedInt));
				DoNext(ChooseHeight); //off to the heightInInches selection!
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
			ClearOutput();
			AddOutput(ConfirmHeightStr);
			DoYesNo(GenericStyleCustomizeMenu, ChooseHeight);
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
			bool buttonMaker(byte index, float val, bool metric) => AddButton(index, () => Measurement.ToNearestHalfSmallUnit(val, true, false),
				() => ChooseCockLength(metric ? (float)(val * Measurement.TO_INCHES) : val));
			ClearOutput();
			AddOutput(CockLengthStr);
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

			AddButton(14, GlobalStrings.BACK, GenericStyleCustomizeMenu);
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
			bool buttonMaker(byte index, float val, bool metric) => AddButton(index, () => Measurement.ToNearestQuarterInchOrHalfCentimeter(val, true),
				() => ChooseClitLength(metric ? (float)(val * Measurement.TO_INCHES) : val));
			ClearOutput();
			AddOutput(ClitLengthStr);
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

			AddButton(14, GlobalStrings.BACK, GenericStyleCustomizeMenu);
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
			bool buttonMaker(byte index, CupSize cup, bool condition) => AddButtonIf(index, () => cup.AsText(), condition, () => ChooseBreastSize(cup));
			ClearOutput();
			AddOutput(ChooseBreastStr);

			buttonMaker(0, CupSize.FLAT, creator.femininity < 50);
			buttonMaker(1, CupSize.A, creator.femininity < 60);
			buttonMaker(2, CupSize.B, creator.femininity >= 40);
			buttonMaker(3, CupSize.C, creator.femininity >= 50);
			buttonMaker(4, CupSize.D, creator.femininity >= 60);
			buttonMaker(5, CupSize.DD, creator.femininity >= 70);
			AddButton(14, GlobalStrings.BACK, GenericStyleCustomizeMenu);
		}

		private void ChooseBreastSize(CupSize size)
		{
			creator.breasts[0].cupSize = size;
			//remember null < anything is false, and null > anything is false. 
			//so these are fine. woo!
			if (size > CupSize.C && creator.breasts[0].nippleLength < 0.5f)
			{
				creator.breasts[0].nippleLength = 0.5f;
			}
			else if (size == CupSize.FLAT && creator.breasts[0].nippleLength > 0.25f)
			{
				creator.breasts[0].nippleLength = 0.25f;
			}
			GenericStyleCustomizeMenu();
		}
		#endregion

		private void ChooseEndowment()
		{
			ClearOutput();
			AddOutput(() => "Endowments not yet implemented");
#warning IMPLEMENT THIS WHEN PERKS ARE DONE
			DoNext(ChooseHistory);
		}

		private void ChooseHistory()
		{
			ClearOutput();
			AddOutput(() => "Endowments not yet implemented");
#warning IMPLEMENT THIS WHEN PERKS ARE DONE
			DoNext(CreatePlayer);
		}

		private void CreatePlayer()
		{
#warning TODO: make sure all data is correct and ready in creator before creating the player
			/*
			Player player = new Player(creator);
			NewGameHelpers.StartTheGame(player);
			*/
		}
		/*
				private void chooseEndowment()
				{
					ClearOutput();
					OutputText(images.showImage("event-question"));
					OutputText("Every person is born with a gift.  What's yours?");
					menu();
					var totalStartingPerks:int = 0;
					var button:int = 0;
					//Attribute Perks
					var endowmentPerks:Array = PerkLists.ENDOWMENT_ATTRIBUTE;
					//Endowment Perks
					if (creator.hasCock())
					{
						endowmentPerks = endowmentPerks.concat(PerkLists.ENDOWMENT_COCK);
					}
					if (creator.hasVagina())
					{
						endowmentPerks = endowmentPerks.concat(PerkLists.ENDOWMENT_VAGINA);
					}
					//Add buttons
					for each(var p: Object in endowmentPerks)
					{
						if (!creator.hasPerk(p.perk))
						{
							AddButton(button++, p.text, confirmEndowment, p.perk);
						}
						else
						{
							AddButtonDisabled(button++, p.text, "You already have this starting perk.");
							totalStartingPerks++;
						}
					}
					if (totalStartingPerks >= 4) //option to skip if you have enough starting perks
						AddButton(14, "Skip", chooseHistory);
				}

				private void confirmEndowment(choice:PerkType)
				{
					ClearOutput();
					OutputText(images.showImage("event-question"));
					switch (choice)
					{
						//Attributes
						case PerkLib.Strong: OutputText("Are you stronger than normal? (+5 Strength)\n\nStrength increases your combat damage, and your ability to hold on to an enemy or pull yourself away."); break;
						case PerkLib.Tough: OutputText("Are you unusually tough? (+5 Toughness)\n\nToughness gives you more HP and increases the chances an attack against you will fail to wound you."); break;
						case PerkLib.Fast: OutputText("Are you very quick?  (+5 Speed)\n\nSpeed makes it easier to escape combat and grapples.  It also boosts your chances of evading an enemy attack and successfully catching up to enemies who try to run."); break;
						case PerkLib.Smart: OutputText("Are you a quick learner?  (+5 Intellect)\n\nIntellect can help you avoid dangerous monsters or work with machinery.  It will also boost the power of any spells you may learn in your travels."); break;
						case PerkLib.Lusty: OutputText("Do you have an unusually high sex-drive?  (+5 Libido)\n\nLibido affects how quickly your lust builds over time.  You may find a high libido to be more trouble than it's worth..."); break;
						case PerkLib.Sensitive: OutputText("Is your skin unusually sensitive?  (+5 Sensitivity)\n\nSensitivity affects how easily touches and certain magics will raise your lust.  Very low sensitivity will make it difficult to orgasm."); break;
						case PerkLib.Pervert: OutputText("Are you unusually perverted?  (+5 Corruption)\n\Corruption affects certain scenes and having a higher corruption makes you more prone to Bad Ends.\n"); break;
						//Gender-specific
						case PerkLib.BigCock: OutputText("Do you have a big cock?  (+2\" Cock Length)\n\nA bigger cock will make it easier to get off any sexual partners, but only if they can take your size."); break;
						case PerkLib.MessyOrgasms: OutputText("Are your orgasms particularly messy?  (+50% Cum Multiplier)\n\nA higher cum multiplier will cause your orgasms to be messier."); break;
						case PerkLib.BigTits: OutputText("Are your breasts bigger than average? (+1 Cup Size)\n\nLarger breasts will allow you to lactate greater amounts, tit-fuck larger cocks, and generally be a sexy bitch."); break;
						case PerkLib.BigClit: OutputText("Do you have a big clit?  (1\" Long)\n\nA large enough clit may eventually become as large as a cock.  It also makes you gain lust much faster during oral or manual stimulation."); break;
						case PerkLib.Fertile: OutputText("Is your family particularly fertile?  (+15% Fertility)\n\nA high fertility will cause you to become pregnant much more easily.  Pregnancy may result in: Strange children, larger bust, larger hips, a bigger ass, and other weirdness."); break;
						case PerkLib.WetPussy: OutputText("Does your pussy get particularly wet?  (+1 Vaginal Wetness)\n\nVaginal wetness will make it easier to take larger cocks, in turn helping you bring the well-endowed to orgasm quicker."); break;
						default: OutputText("Something broke!");
					}
					menu();
					AddButton(0, "Yes", setEndowment, choice);
					AddButton(1, "No", chooseEndowment);
				}

				protected void setEndowment(choice:PerkType)
				{
					switch (choice)
					{
						//Attribute-specific
						case PerkLib.Strong:
							creator.strength += 5;
							creator.muscleDefinition += 7;
							creator.thickness += 3;
							creator.createPerk(PerkLib.Strong, 0.25, 0, 0, 0);
							break;
						case PerkLib.Tough:
							creator.toughness += 5;
							creator.muscleDefinition += 5;
							creator.thickness += 5;
							creator.createPerk(PerkLib.Tough, 0.25, 0, 0, 0);
							creator.restoreHP();
							break;
						case PerkLib.Fast:
							creator.speed += 5;
							creator.muscleDefinition += 10;
							creator.createPerk(PerkLib.Fast, 0.25, 0, 0, 0);
							break;
						case PerkLib.Smart:
							creator.inte += 5;
							creator.thickness -= 5;
							creator.createPerk(PerkLib.Smart, 0.25, 0, 0, 0);
							break;
						case PerkLib.Lusty:
							creator.lib += 5;
							creator.createPerk(PerkLib.Lusty, 0.25, 0, 0, 0);
							break;
						case PerkLib.Sensitive:
							creator.sens += 5;
							creator.createPerk(PerkLib.Sensitive, 0.25, 0, 0, 0);
							break;
						case PerkLib.Pervert:
							creator.cor += 5;
							creator.createPerk(PerkLib.Pervert, 0.25, 0, 0, 0);
							break;
						//Genital-specific
						case PerkLib.BigCock:
							creator.femininity -= 5;
							creator.cocks[0].cockLength = 8;
							creator.cocks[0].cockThickness = 1.5;
							creator.createPerk(PerkLib.BigCock, 1.25, 0, 0, 0);
							break;
						case PerkLib.MessyOrgasms:
							creator.femininity -= 2;
							creator.cumMultiplier = 1.5;
							creator.createPerk(PerkLib.MessyOrgasms, 1.25, 0, 0, 0);
							break;
						case PerkLib.BigTits:
							creator.femininity += 5;
							creator.breastRows[0].breastRating += 2;
							creator.createPerk(PerkLib.BigTits, 1.5, 0, 0, 0);
							break;
						case PerkLib.BigClit:
							creator.femininity -= 5;
							creator.setClitLength(1);
							creator.createPerk(PerkLib.BigClit, 1.25, 0, 0, 0);
							break;
						case PerkLib.Fertile:
							creator.femininity += 5;
							creator.fertility += 25;
							creator.hipSize += 2;
							creator.createPerk(PerkLib.Fertile, 1.5, 0, 0, 0);
							break;
						case PerkLib.WetPussy:
							creator.femininity += 7;
							creator.vaginas[0].vaginalWetness = Vagina.WETNESS_WET;
							creator.createPerk(PerkLib.WetPussy, 2, 0, 0, 0);
							break;
						default: //move along, nothing happens in this defaultness
					}
					chooseHistory();
				}
				//----------------- HISTORY PERKS -----------------
				public void chooseHistory()
				{
					ClearOutput();
					OutputText(images.showImage("event-question"));
					if (flags[kFLAGS.HISTORY_PERK_SELECTED] !== 0) //this flag can only be non-zero if chooseHistory is called from camp.as
						OutputText("<b>New history perks are available during creation.  Since this character was created before they were available, you may choose one now!</b>\n\n");
					OutputText("Before you became a champion, you had other plans for your life.  What were you doing before?");
					menu();
					var totalHistoryPerks:int = 0;
					var button:int = 0;
					//Attribute Perks
					for each(var p: Object in PerkLists.HISTORY)
					{
						if (!creator.hasPerk(p.perk))
							AddButton(button++, p.text, confirmHistory, p.perk);
						else
						{
							AddButtonDisabled(button++, p.text, "You already have this history perk.");
							totalHistoryPerks++;
						}
					}
					if (totalHistoryPerks >= 3) AddButton(14, "Skip", completeCharacterCreation);
				}

				private void confirmHistory(choice:PerkType)
				{
					ClearOutput();
					OutputText(images.showImage("event-question"));
					switch (choice)
					{
						case PerkLib.HistoryAlchemist: OutputText("You spent some time as an alchemist's assistant, and alchemical items always seem to be more reactive in your hands.  Is this your history?"); break;
						case PerkLib.HistoryFighter: OutputText("You spent much of your time fighting other children, and you had plans to find work as a guard when you grew up.  You do 10% more damage with physical attacks.  You will also start out with 50 gems.  Is this your history?"); break;
						case PerkLib.HistoryFortune: OutputText("You always feel lucky when it comes to fortune.  Because of that, you have always managed to save up gems until whatever's needed and how to make the most out it (+15% gems on victory).  You will also start out with 250 gems.  Is this your history?"); break;
						case PerkLib.HistoryHealer: OutputText("You often spent your free time with the village healer, learning how to tend to wounds.  Healing items and effects are 20% more effective.  Is this your history?"); break;
						case PerkLib.HistoryReligious: OutputText("You spent a lot of time at the village temple, and learned how to meditate.  The 'masturbation' option is replaced with 'meditate' when corruption is at or below 66.  Is this your history?"); break;
						case PerkLib.HistoryScholar: OutputText("You spent much of your time in school, and even begged the richest man in town, Mr. " + (silly() ? "Savin" : "Sellet") + ", to let you read some of his books.  You are much better at focusing, and spellcasting uses 20% less fatigue.  Is this your history?"); break;
						case PerkLib.HistorySlacker: OutputText("You spent a lot of time slacking, avoiding work, and otherwise making a nuisance of yourself.  Your efforts at slacking have made you quite adept at resting, so your fatigue will lower 20% faster.  Is this your history?"); break;
						case PerkLib.HistorySlut: OutputText("You managed to spend most of your time having sex.  Quite simply, when it came to sex, you were the village bicycle - everyone got a ride.  Because of this, your body is a bit more resistant to penetrative stretching, and has a higher upper limit on what exactly can be inserted.  Is this your history?"); break;
						case PerkLib.HistorySmith: OutputText("You managed to get an apprenticeship with the local blacksmith.  Because of your time spent at the blacksmith's side, you've learned how to fit armor for maximum protection.  Is this your history?"); break;
						default: OutputText("You managed to find work as a whore.  Because of your time spent trading seduction for profit, you're more effective at teasing (+15% tease damage).  Is this your history?");
					}
					menu();
					AddButton(0, "Yes", setHistory, choice);
					AddButton(1, "No", chooseHistory);
				}

				private void setHistory(choice:PerkType)
				{
					creator.createPerk(choice, 0, 0, 0, 0);
					if (choice == PerkLib.HistorySlut || choice == PerkLib.HistoryWhore)
					{
						if (creator.hasVagina())
						{
							creator.vaginas[0].virgin = false;
							creator.vaginas[0].vaginalLooseness = Vagina.LOOSENESS_LOOSE;
						}
						creator.ass.analLooseness = 1;
					}
					if (choice == PerkLib.HistoryFighter || choice == PerkLib.HistoryWhore) creator.gems += 50;
					if (choice == PerkLib.HistoryFortune) creator.gems += 250;
					if (flags[kFLAGS.HISTORY_PERK_SELECTED] == 0)
					{
						flags[kFLAGS.HISTORY_PERK_SELECTED] = 1;
						completeCharacterCreation();
					}
					else
					{
						flags[kFLAGS.HISTORY_PERK_SELECTED] = 1; //Special escape clause for very old saves that do not have a history perk. This is used to allow them the chance to select a perk at camp on load
						creatorMenu();
					}
				}

				private void completeCharacterCreation()
				{
					ClearOutput();
					if (customcreatorProfile !== null)
					{
						customcreatorProfile();
						if (flags[kFLAGS.NEW_GAME_PLUS_LEVEL] == 0) doNext(chooseGameModes);
						else doNext(startTheGame);
						return;
					}
					if (flags[kFLAGS.NEW_GAME_PLUS_LEVEL] == 0) chooseGameModes();
					else startTheGame();
				}
			*/
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
				

				public void arrival()
				{
					showStats();
					statScreenRefresh();
					getGame().time.hours = 11;
					ClearOutput();
					if (flags[kFLAGS.GRIMDARK_MODE] > 0)
					{
						OutputText("You are prepared for what is to come. Most of the last year has been spent honing your body and mind to prepare for the challenges ahead. You are the Champion of Ingnam. The one who will journey to the demon realm and guarantee the safety of your friends and family, even though you'll never see them again. You wipe away a tear as you enter the courtyard and see Elder... Wait a minute...\n\n");
						OutputText("Something is not right. Elder Nomur is already dead. Ingnam has been mysteriously pulled into the demon realm and the surroundings look much worse than you've expected. A ruined portal frame stands in the courtyard, obviously no longer functional and instead serves as a grim reminder on the now-ceased tradition of annual sacrifice of Champions. Wooden palisades surround the town of Ingnam and outside the walls, spears are set out and angled as a mean to make the defenses more intimidating. As if that wasn't enough, some of the spears have demonic skulls impaled on them.");
						flags[kFLAGS.IN_INGNAM] = 1;
						doNext(creatorMenu);
						return;
					}
					OutputText("You are prepared for what is to come.  Most of the last year has been spent honing your body and mind to prepare for the challenges ahead.  You are the Champion of Ingnam.  The one who will journey to the demon realm and guarantee the safety of your friends and family, even though you'll never see them again.  You wipe away a tear as you enter the courtyard and see Elder Nomur waiting for you.  You are ready.\n\n");
					OutputText("The walk to the tainted cave is long and silent.  Elder Nomur does not speak.  There is nothing left to say.  The two of you journey in companionable silence.  Slowly the black rock of Mount Ilgast looms closer and closer, and the temperature of the air drops.  You shiver and glance at the Elder, noticing he doesn't betray any sign of the cold.  Despite his age of nearly 80, he maintains the vigor of a man half his age.  You're glad for his strength, as assisting him across this distance would be draining, and you must save your energy for the trials ahead.\n");
					OutputText(images.showImage("camp-arrival"));
					OutputText("The entrance of the cave gapes open, sharp stalactites hanging over the entrance, giving it the appearance of a monstrous mouth.  Elder Nomur stops and nods to you, gesturing for you to proceed alone.\n\n");
					OutputText("The cave is unusually warm and damp, ");
					if (creator.gender == Gender.FEMALE) OutputText("and your body seems to feel the same way, flushing as you feel a warmth and dampness between your thighs. ");
					else OutputText("and your body reacts with a sense of growing warmth focusing in your groin, your manhood hardening for no apparent reason. ");
					OutputText("You were warned of this and press forward, ignoring your body's growing needs.  A glowing purple-pink portal swirls and flares with demonic light along the back wall.  Cringing, you press forward, keenly aware that your body seems to be anticipating coming in contact with the tainted magical construct.  Closing your eyes, you gather your resolve and leap forwards.  Vertigo overwhelms you and you black out...");
					dynStats("lus", 15);
					doNext(arrivalPartTwo);
				}
				private void arrivalPartTwo()
				{
					ClearOutput();
					hideUpDown();
					dynStats("lus", 40, "cor", 2);
					getGame().time.hours = 18;
					OutputText(images.showImage("encounter-zetaz"));
					spriteSelect(SpriteDb.s_zetaz_imp);
					OutputText("You wake with a splitting headache and a body full of burning desire.  A shadow darkens your view momentarily and your training kicks in.  You roll to the side across the bare ground and leap to your feet.  A surprised looking imp stands a few feet away, holding an empty vial.  He's completely naked, an improbably sized pulsing red cock hanging between his spindly legs.  You flush with desire as a wave of lust washes over you, your mind reeling as you fight ");
					if (creator.gender == Gender.FEMALE) OutputText("the urge to chase down his rod and impale yourself on it.\n\n");
					else OutputText("the urge to ram your cock down his throat.  The strangeness of the thought surprises you.\n\n");
					OutputText("The imp says, \"<i>I'm amazed you aren't already chasing down my cock, human.  The last Champion was an eager whore for me by the time she woke up.  This lust draft made sure of it.</i>\"");
					doNext(arrivalPartThree);
				}
				private void arrivalPartThree()
				{
					ClearOutput();
					hideUpDown();
					dynStats("lus", -30);
					OutputText(images.showImage("item-draft-lust"));
					OutputText("The imp shakes the empty vial to emphasize his point.  You reel in shock at this revelation - you've just entered the demon realm and you've already been drugged!  You tremble with the aching need in your groin, but resist, righteous anger lending you strength.\n\nIn desperation you leap towards the imp, watching with glee as his cocky smile changes to an expression of sheer terror.  The smaller creature is no match for your brute strength as you pummel him mercilessly.  You pick up the diminutive demon and punt him into the air, frowning grimly as he spreads his wings and begins speeding into the distance.\n\n");
					doNext(arrivalPartFour);
				}
				private void arrivalPartFour()
				{
					ClearOutput();
					hideUpDown();
					OutputText(images.showImage("zetaz-runaway"));
					OutputText("The imp says, \"<i>FOOL!  You could have had pleasure unending... but should we ever cross paths again you will regret humiliating me!  Remember the name Zetaz, as you'll soon face the wrath of my master!</i>\"\n\n");
					OutputText("Your pleasure at defeating the demon ebbs as you consider how you've already been defiled.  You swear to yourself you will find the demon responsible for doing this to you and the other Champions, and destroy him AND his pet imp.");
					doNext(arrivalPartFive);
				}
				private void arrivalPartFive()
				{
					ClearOutput();
					hideUpDown();
					OutputText(images.showImage("camp-portal"));
					spriteSelect(null);
					OutputText("You look around, surveying the hellish landscape as you plot your next move.  The portal is a few yards away, nestled between a formation of rocks.  It does not seem to exude the arousing influence it had on the other side.  The ground and sky are both tinted different shades of red, though the earth beneath your feet feels as normal as any other lifeless patch of dirt.   You settle on the idea of making a camp here and fortifying this side of the portal.  No demons will ravage your beloved hometown on your watch.\n\nIt does not take long to set up your tent and a few simple traps.  You'll need to explore and gather more supplies to fortify it any further.  Perhaps you will even manage to track down the demons who have been abducting the other champions!");
					awardAchievement("Newcomer", kACHIEVEMENTS.STORY_NEWCOMER, true, true);
					doNext(creatorMenu);
				}
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
					ClearOutput();
					OutputText("You have chosen Hardcore Mode. In this mode, the game forces autosave and if you encounter a Bad End, your save file is <b>DELETED</b>! \n\nDebug Mode and Easy Mode are disabled in this game mode. \n\nPlease choose a slot to save in. You may not make multiple copies of saves.");
					menu();
					for (var i:int = 0; i < 14; i++)
			{
						AddButton(i, "Slot " + (i + 1), function(slot: int) :*
			{
							flags[kFLAGS.HARDCORE_SLOT] = "CoC_" + slot;
							startTheGame();
						}, i + 1);
					}
					AddButton(14, "Back", chooseGameModes);
				}
				//GRIMDARK!
				private void chooseModeGrimdark()
				{
					ClearOutput();
					OutputText("You have chosen Grimdark Mode. This will drastically alter gameplay and there will be a lot of new obstacles. Enemies are beefed up and the game will be much darker and edgier with plenty of environment changes. Is this what you choose?");
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
					ClearOutput();
					OutputText(images.showImage("event-creation"));
					OutputText("Choose a game mode.\n\n");
					OutputText("<b>Survival:</b> ");
					if (flags[kFLAGS.HUNGER_ENABLED] == 0) OutputText("Normal Mode. You don't have to eat.\n");
					if (flags[kFLAGS.HUNGER_ENABLED] == 0.5) OutputText("Survival Mode. You get hungry from time to time.\n");
					if (flags[kFLAGS.HUNGER_ENABLED] == 1) OutputText("Realistic Mode. You get hungry from time to time and cum production is capped. In addition, it's a bad idea to have oversized parts.\n");
					OutputText("<b>Hardcore:</b> ");
					if (flags[kFLAGS.HARDCORE_MODE] == 0) OutputText("Normal Mode. You choose when you want to save and load.\n");
					if (flags[kFLAGS.HARDCORE_MODE] == 1) OutputText("Hardcore Mode. The game forces save and if you get a Bad End, your save file is deleted. Disables difficulty selection, debug mode, Low Standarts and Hyper Happy mode once the game is started. For the veteran CoC creators only.\n");
					OutputText("<b>Difficulty:</b> ");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 0) OutputText("Normal Mode. No stats changes. Game is nice and simple.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 1) OutputText("Hard Mode. Enemies have would have extra 25% HP and 15% damage.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 2) OutputText("Nightmare Mode. Enemies would have extra 50% HP and 30% damage.\n");
					if (flags[kFLAGS.GAME_DIFFICULTY] == 3) OutputText("Extreme Mode. Enemies would have extra 100% HP and 50% damage.\n");
					if (debug) OutputText("<b>Grimdark mode:</b> (In dev) In the grimdark future, there are only rape and corruptions. Lots of things are changed and Lethice has sent out her minions to wall the borders and put up a lot of puzzles. Can you defeat her in this mode in as few bad ends as possible?\n");
					menu();
					AddButton(0, "Survival", chooseModeSurvival);
					AddButton(1, "Hardcore", chooseModeHardcore);
					AddButton(2, "Difficulty", chooseModeDifficulty);
					if (debug) AddButton(3, "Grimdark", chooseModeGrimdark);
					AddButton(4, "Start!", flags[kFLAGS.HARDCORE_MODE] == 1 ? chooseModeHardcoreSlot : startTheGame);
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
					ClearOutput();
					OutputText(images.showImage("location-ingnam"));
					OutputText("Would you like to play through the 3-day prologue in Ingnam or just skip?");
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
			//			ClearOutput();
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
			//			ClearOutput();
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
			//			ClearOutput();
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
			//			ClearOutput();
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
			//			ClearOutput();
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
			//			ClearOutput();
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
			//			ClearOutput();
			//			outputText("Would you like to reincarnate and start a new life as a Champion?");
			//			doYesNo(reincarnate, ascensionMenu);
			//		}
			//		protected void reincarnate()
			//		{
			//			flags[kFLAGS.NEW_GAME_PLUS_LEVEL]++;
			//			customcreatorProfile = null;
			//			newGameGo();
			//			ClearOutput();
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
			//			ClearOutput();
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
