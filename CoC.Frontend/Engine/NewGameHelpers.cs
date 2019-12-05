//NewGameHelpers.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:05 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Strings;
using CoC.Frontend.Creatures;
using CoC.Frontend.UI;
using CoC.Frontend.SaveData;
using CoC.Frontend.Strings.Engine;
using static CoC.Frontend.UI.ViewOptions;
using CoC.Backend.UI;
using System;
using CoC.Frontend.Creatures.PlayerData;

namespace CoC.Frontend.Engine
{
	public static class NewGameHelpers
	{
		private static StandardDisplay currentDisplay;

		public static void NewGame()
		{
			//clear all the extraneous data stored in the various engines in the backend.
			GameEngine.StartNewGame();

			GameEngine.UnlockAchievement<Achievements.StartTheGameINeedAnAchievementForDebugging>();

			SetPlayerStatus(PlayerStatus.IDLE);
			HideMenu();
			HideStats();

			if (currentDisplay is null)
			{
				currentDisplay = new StandardDisplay();
			}
			DisplayManager.LoadDisplay(currentDisplay);

			currentDisplay.ClearOutput();
			currentDisplay.OutputText(NewGameHelperText.IntroText());
			currentDisplay.ActivateInputField();
			currentDisplay.ActivateDropDownMenu(SpecialCharacters.SpecialCharacterDropDownList(currentDisplay));
			currentDisplay.AddButton(0, GlobalStrings.OK(), ChooseName);
		}

		private static void ChooseName()
		{
			if (string.IsNullOrWhiteSpace(currentDisplay.GetOutput()))
			{
				return;
			}
			else
			{
				currentDisplay.ClearOutput();
				string playerName = currentDisplay.GetOutput().Trim();
				string check = playerName.CapitalizeFirstLetter();
				PlayerCreator pc;
				if (!SpecialCharacters.specialCharacterLookup.ContainsKey(check))
				{
					pc = new PlayerCreator(playerName);
					CharacterCreation creator = new CharacterCreation(currentDisplay, pc);
					creator.SetGenderGeneric();
				}
				else
				{
					PromptSpecial(check);
				}
				currentDisplay.ClearInputData();
			}
		}

		private static void PromptSpecial(string specialName)
		{
			currentDisplay.OutputText(NewGameHelperText.PromptSpecial());
			currentDisplay.AddButton(0, NewGameHelperText.SpecialName(), () => ParseSpecial(specialName, true));
			currentDisplay.AddButton(1, NewGameHelperText.ContinueOn(), () => ParseSpecial(specialName, false));
		}

		private static void ParseSpecial(string name, bool useSpecial)
		{
			PlayerCreator pc = useSpecial ? SpecialCharacters.specialCharacterLookup[name]() : new PlayerCreator(name);
			CharacterCreation creator = new CharacterCreation(currentDisplay, pc);
			creator.SetGenderSpecial(useSpecial);
		}

		public static void NewGamePlus(PlayerBase currentPlayer)
		{
			if (currentDisplay == null)
			{
				currentDisplay = new StandardDisplay();
			}
			currentDisplay.ClearOutput();

			//parse the current player to New Game Plus related PlayerCreator;
			PlayerCreator pc = new PlayerCreator(currentPlayer.name);
			CharacterCreation creator = new CharacterCreation(currentDisplay, pc, true);
			creator.SetGenderGeneric();
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public static void ChooseSettings(PlayerBase player)
		{
			//ClearOutput();
			//OutputText("Not Yet Implemented :)");
			FrontendSessionSave.data.SillyModeLocal = false;
			BackendSessionSave.data.difficulty = 1;
			//hunger enabled is false. NYI.
			BackendSessionSave.data.HungerEnabled = false;
			BackendSessionSave.data.RealismEnabled = false;
			BackendSessionSave.data.hardcoreMode = false;

			GameEngine.SetDifficulty(1);

			StartTheGame(player);
		}

		public static void StartTheGame(PlayerBase player)
		{
			GameEngine.InitializeGame(player, FirstExplorationPage);
			GameEngine.InitializeOrJumpTime(0, 11);
			currentDisplay.ClearOutput();
			ShowStats();
			//if (flags[kFLAGS.GRIMDARK_MODE] > 0)
			//{
			//currentDisplay.OutputText("You are prepared for what is to come. Most of the last year has been spent honing your body and mind to prepare for the challenges ahead. You are the Champion of Ingnam. The one who will journey to the demon realm and guarantee the safety of your friends and family, even though you'll never see them again. You wipe away a tear as you enter the courtyard and see Elder... Wait a minute...\n\n");
			//currentDisplay.OutputText("Something is not right. Elder Nomur is already dead. Ingnam has been mysteriously pulled into the demon realm and the surroundings look much worse than you've expected. A ruined portal frame stands in the courtyard, obviously no longer functional and instead serves as a grim reminder on the now-ceased tradition of annual sacrifice of Champions. Wooden palisades surround the town of Ingnam and outside the walls, spears are set out and angled as a mean to make the defenses more intimidating. As if that wasn't enough, some of the spears have demonic skulls impaled on them.");
			//	flags[kFLAGS.IN_INGNAM] = 1;
			//	doNext(creatorMenu);
			//	return;
			//}
			currentDisplay.OutputImage("camp-arrival");
			currentDisplay.OutputText(NewGameHelperText.ArrivalPartOne());
			GameEngine.currentlyControlledCharacter.IncreaseLust(15);
			currentDisplay.DoNext(ArrivalPartTwo);

		}

		private static void ArrivalPartTwo()
		{
			currentDisplay.ClearOutput();
			GameEngine.currentlyControlledCharacter.IncreaseCreatureStats(lus: 40, corr: 2);
			GameEngine.InitializeOrJumpTime(0, 18);

			currentDisplay.OutputImage("encounter-zetaz");
			currentDisplay.SetSprite("zetaz_imp.png");
			currentDisplay.OutputText(NewGameHelperText.ArrivalPartThree());

			currentDisplay.DoNext(ArrivalPartThree);
		}
		private static void ArrivalPartThree()
		{
			currentDisplay.ClearOutput();
			GameEngine.currentlyControlledCharacter.DecreaseLust(30);
			currentDisplay.OutputImage("item-draft-lust");
			currentDisplay.OutputText(NewGameHelperText.ArrivalPartThree());
			currentDisplay.DoNext(ArrivalPartFour);
		}
		private static void ArrivalPartFour()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputImage("zetaz-runaway");
			currentDisplay.OutputText(NewGameHelperText.ArrivalPartFour());

			currentDisplay.DoNext(ArrivalPartFive);
		}
		private static void ArrivalPartFive()
		{
			currentDisplay.ClearOutput();
			currentDisplay.OutputImage("camp-portal");
			currentDisplay.ClearSprite();
			currentDisplay.OutputText(NewGameHelperText.ArrivalPartFive());
			currentDisplay.DoNext(() => GameEngine.UseHoursGoToBase(0));
			//awardAchievement("Newcomer", kACHIEVEMENTS.STORY_NEWCOMER, true, true);
			//currentDisplay.DoNext(() => GameEngine.ReturnToBaseAfter(0));
			//currentDisplay.DoNext(NewGame);//debug helping.
		}

		private static void FirstExplorationPage()
		{
			var display = DisplayManager.GetCurrentDisplay();
			display.ClearOutput();
			display.OutputText(NewGameHelperText.FirstExploration());
			display.DoNext(() => GameEngine.UseHoursGoToBase(1));
		}
	}
}
