//NewGameHelpers.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:05 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Frontend.Creatures;
using CoC.Frontend.Strings.Engine;
using CoC.Frontend.UI;
using static CoC.Frontend.UI.ButtonManager;
using static CoC.Frontend.UI.TextOutput;
using static CoC.Frontend.UI.ViewOptions;
namespace CoC.Frontend.Engine
{
	public static class NewGameHelpers
	{
		public static void NewGame()
		{
			SetPlayerStatus(PlayerStatus.IDLE);
			HideMenu();
			HideStats();

			ClearOutput();
			AddOutput(NewGameHelperText.IntroText);
			InputField.ActivateInputField(null, "", "");
			DropDownMenu.ActivateDropDownMenu(SpecialCharacters.SpecialCharacterDropDownList());
			AddButton(0, GlobalStrings.OK, ChooseName);
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
