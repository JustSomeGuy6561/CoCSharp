//NewGameHelpers.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:05 PM
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using CoC.Backend.Strings;
using CoC.Frontend.Creatures;
using CoC.Frontend.SaveData;
using CoC.Frontend.Strings.Engine;
using CoC.Frontend.UI;
using static CoC.Backend.Engine.ButtonManager;
using static CoC.Frontend.UI.TextOutput;
using static CoC.Frontend.UI.ViewOptions;
using static CoC.Frontend.UI.ControllerData.SpriteAndCreditsOutput;
namespace CoC.Frontend.Engine
{
	public static class NewGameHelpers
	{
		public static void NewGame()
		{
			//clear all the extraneous data stored in the various engines in the backend. 
			GameEngine.StartNewGame();

			GameEngine.UnlockAchievement<Achievements.StartTheGameINeedAnAchievementForDebugging>();

			SetPlayerStatus(PlayerStatus.IDLE);
			HideMenu();
			HideStats();

			ClearOutput();
			OutputText(NewGameHelperText.IntroText());
			InputField.ActivateInputField();
			DropDownMenu.ActivateDropDownMenu(SpecialCharacters.SpecialCharacterDropDownList());
			AddButton(0, GlobalStrings.OK(), ChooseName);
		}

		private static void ChooseName()
		{
			if (string.IsNullOrWhiteSpace(InputField.output))
			{
				return;
			}
			else
			{
				ClearOutput();
				string playerName = InputField.output.Trim();
				string check = playerName.CapitalizeFirstLetter();
				PlayerCreator pc;
				if (!SpecialCharacters.specialCharacterLookup.ContainsKey(check))
				{
					pc = new PlayerCreator(playerName);
					CharacterCreation creator = new CharacterCreation(pc);
					creator.SetGenderGeneric();
				}
				else
				{
					PromptSpecial(check);
				}
				InputField.ClearData();
			}
		}

		private static void PromptSpecial(string specialName)
		{
			OutputText(NewGameHelperText.PromptSpecial());
			AddButton(0, NewGameHelperText.SpecialName(), () => ParseSpecial(specialName, true));
			AddButton(1, NewGameHelperText.ContinueOn(), () => ParseSpecial(specialName, false));
		}

		private static void ParseSpecial(string name, bool useSpecial)
		{
			PlayerCreator pc = useSpecial ? SpecialCharacters.specialCharacterLookup[name]() : new PlayerCreator(name);
			CharacterCreation creator = new CharacterCreation(pc);
			creator.SetGenderSpecial(useSpecial);
		}

		public static void NewGamePlus(Player currentPlayer)
		{
			//parse the current player to New Game Plus related PlayerCreator;
			PlayerCreator pc = new PlayerCreator(currentPlayer.name);
			CharacterCreation creator = new CharacterCreation(pc, true);
			creator.SetGenderGeneric();
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public static void ChooseSettings(Player player)
		{
			//ClearOutput();
			//OutputText("Not Yet Implemented :)");
			FrontendSessionSave.data.SillyModeLocal = false;
			BackendSessionSave.data.difficulty = 1;
			//hunger enabled is false. NYI.
			BackendSessionSave.data.HungerEnabled = false;
			BackendSessionSave.data.RealismEnabled = false;
			BackendSessionSave.data.hardcoreMode = false;

			StartTheGame(player);
		}

		public static void StartTheGame(Player player)
		{
			GameEngine.InitializeGame(player);
			GameEngine.InitializeTime(0, 11);
			ClearOutput();
			ShowStats();
			//if (flags[kFLAGS.GRIMDARK_MODE] > 0)
			//{
			//	OutputText("You are prepared for what is to come. Most of the last year has been spent honing your body and mind to prepare for the challenges ahead. You are the Champion of Ingnam. The one who will journey to the demon realm and guarantee the safety of your friends and family, even though you'll never see them again. You wipe away a tear as you enter the courtyard and see Elder... Wait a minute...\n\n");
			//	OutputText("Something is not right. Elder Nomur is already dead. Ingnam has been mysteriously pulled into the demon realm and the surroundings look much worse than you've expected. A ruined portal frame stands in the courtyard, obviously no longer functional and instead serves as a grim reminder on the now-ceased tradition of annual sacrifice of Champions. Wooden palisades surround the town of Ingnam and outside the walls, spears are set out and angled as a mean to make the defenses more intimidating. As if that wasn't enough, some of the spears have demonic skulls impaled on them.");
			//	flags[kFLAGS.IN_INGNAM] = 1;
			//	doNext(creatorMenu);
			//	return;
			//}
			OutputImage("camp-arrival");
			OutputText(NewGameHelperText.ArrivalPartOne());
			//dynStats("lus", 15);
			MenuHelpers.DoNext(ArrivalPartTwo);

		}

		private static void ArrivalPartTwo()
		{
			ClearOutput();
			//dynStats("lus", 40, "cor", 2);
			GameEngine.InitializeTime(0, 18);

			OutputImage("encounter-zetaz");
			SetSprite("zetaz_imp.png");
			OutputText(NewGameHelperText.ArrivalPartThree());
			
			MenuHelpers.DoNext(ArrivalPartThree);
		}
		private static void ArrivalPartThree()
		{
			ClearOutput();
			//dynStats("lus", -30);
			OutputImage("item-draft-lust");
			OutputText(NewGameHelperText.ArrivalPartThree());
			MenuHelpers.DoNext(ArrivalPartFour);
		}
		private static void ArrivalPartFour()
		{
			ClearOutput();
			OutputImage("zetaz-runaway");
			OutputText(NewGameHelperText.ArrivalPartFour());

			MenuHelpers.DoNext(ArrivalPartFive);
		}
		private static void ArrivalPartFive()
		{
			ClearOutput();
			OutputImage("camp-portal");
			ClearSprite();
			OutputText(NewGameHelperText.ArrivalPartFive());
			//awardAchievement("Newcomer", kACHIEVEMENTS.STORY_NEWCOMER, true, true);
			//MenuHelpers.DoNext(() => GameEngine.ReturnToBaseAfter(0));
			MenuHelpers.DoNext(NewGame);//debug helping.
		}
	}
}
