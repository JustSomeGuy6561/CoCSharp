﻿//NewGameHelpers.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:05 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Frontend.Creatures;
using CoC.Frontend.Strings.Engine;
using CoC.Frontend.UI;
using CoC.UI;
using static CoC.Frontend.UI.ButtonManager;
using static CoC.Frontend.UI.TextOutput;

namespace CoC.Frontend.Engine
{
	public static class NewGameHelpers
	{
		public static void NewGame()
		{
			Controller.SetPlayerStatus(PlayerStatus.IDLE);
			ViewOptions.HideMenu();
			ViewOptions.HideStats();

			ClearOutput();
			ClearButtons();
			AddOutput(NewGameHelperText.IntroText);
			InputField.ActivateInputField(null, "", "");
			//enable drop-list. auto-fill name on selection (left to GUI layer).
			AddButton(0, GlobalStrings.OK, ChooseName);
		}

		private static void ChooseName()
		{
			if (string.IsNullOrWhiteSpace(Controller.instance.inputText))
			{
				return;
			}
			else
			{
				ClearOutput();
				//disable input field.
				string playerName = Controller.instance.inputText.Trim();
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
			}
		}

		private static void PromptSpecial(string specialName)
		{
			AddOutput(NewGameHelperText.PromptSpecial);
			AddButton(0, NewGameHelperText.SpecialName, () => ParseSpecial(specialName, true));
			AddButton(1, NewGameHelperText.ContinueOn, () => ParseSpecial(specialName, false));
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

		public static void StartTheGame(Player player)
		{

		}
	}
}
