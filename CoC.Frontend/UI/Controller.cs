//ModelView.cs
//Description:
//Author: JustSomeGuy
//2/27/2019, 10:15 PM

//Controller.cs
//Description:
//Author: JustSomeGuy
//1/19/2019, 8:01 PM
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.GameCredits;
using CoC.Backend.Settings;
using CoC.Frontend.UI;
using CoC.Frontend.Engine;
using CoC.Frontend.UI.ControllerData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoC.Backend.UI;
using CoC.Backend.Areas;

namespace CoC.UI
{

	//consider firing custom events instead of INotifyPropertyChanged. 
	//or, implement inotifypropertychanged, but instead of firing each time, add each property name to a hashset each time they change. then, when the code is ready
	//for the game to return to the GUI layer, fire them off. This will prevent multiple fires and make sure the data is only updated when it needs to be. 
	public sealed partial class Controller
	{
#warning Consider moving all static functions for buttons, output, input, selector here. makes it easier to link everything in.

		public static Controller instance { get; } = new Controller();

		public void QueryData()
		{
			bool displayChanged = DisplayManager.QueryDisplay(out _display);

			buttonsChanged = displayChanged | _display.QueryButtons(out _buttons);
			outputChanged = displayChanged | _display.QueryData(out _outputField, out _outputImagePath);
			inputFieldChanged = displayChanged | _display.QueryInputField(out _inputField);
			spriteChanged = displayChanged | _display.QuerySpriteCreditData(out _spriteName, out _creatorText);

			dropDownMenuItemsChanged = displayChanged | _display.QueryDropDownStatus(out _dropDownMenu);
			dropDownPostTextChanged = displayChanged | _display.QueryPostText(out _postControlText);

			displayStats = ViewOptions.showStats;
			displayTopMenu = ViewOptions.showStandardMenu;
			playerStatus = ViewOptions.playerStatus;
		}

		private StandardDisplay _display;

		public bool displayStats { get; private set; }
		public bool displayTopMenu { get; private set; }
		public PlayerStatus playerStatus { get; private set; }

		//public Image outputImage; 
		public string outputImagePath => _outputImagePath;
		private string _outputImagePath;
		public StringBuilder outputField => _outputField;
		private StringBuilder _outputField;
		public bool outputChanged { get; private set; }

		/// <summary>
		/// Called from the GUI when the game needs to reload the current scene. Should only occur if the pc is currently at a home base. Forces the text for the current location
		/// to be regenerated, but with no other interactions running. This way, the text can respond to language changes and such, or handle the case where it's switching from
		/// appearance to skills and back to standard text or whatever. 
		/// </summary>
		public static void ForceReloadFromGUI()
		{
			GameEngine.ForceReload();
		}


		public ReadOnlyCollection<ButtonData> buttons => _buttons;
		private ReadOnlyCollection<ButtonData> _buttons;
		public bool buttonsChanged { get; private set; }
		public int ValidButtons => buttons.Count(x => x != null);
		//do this later. for now fuck it.


		public InputField inputField { get => _inputField; }
		private InputField _inputField;
		public bool inputFieldChanged { get; private set; }


		public string SpriteName => _spriteName;
		private string _spriteName = null;

		public string CreatorName => _creatorText;
		private string _creatorText = null;

		public bool spriteChanged { get; private set; }


		public void setOutputFromUI(string output)
		{
			inputField.UpdateInputFromOutput(output);
		}

		public DropDownMenu dropDownMenu { get => _dropDownMenu; }
		private DropDownMenu _dropDownMenu;
		public bool dropDownMenuItemsChanged { get; private set; }

		public string postControlText { get => _postControlText; }
		private string _postControlText;

		public bool dropDownPostTextChanged { get; private set; }
		public bool hasPlayer => GameEngine.currentlyControlledCharacter != null;

		private static GameDateTime currTime()
		{
			return GameDateTime.Now;
		}

		public StatDataCollection statDataCollection
		{
			get
			{
				_statDataCollection.ParseData();
				return _statDataCollection;
			}
		}

		private readonly StatDataCollection _statDataCollection = new StatDataCollection();

		public StatisticsData GetStatistics()
		{
			return new StatisticsData();
		}

		public bool canLevelUp()
		{
			return false;
		}

		public LevelingData GetLevelingData()
		{
			return new LevelingData();
		}

		public PerkData GetPerkData()
		{
			return new PerkData();
		}

		public AppearanceData GetAppearanceData()
		{
			return new AppearanceData();
		}

		public ReadOnlyCollection<InstructionItem> GetInstructions() => InstructionsEtc.instructions;

		public bool AchievementsUnlockedChanged => AchievementData.QueryData(out _achievementData);

		public AchievementData achievementData => _achievementData;
		private AchievementData _achievementData;

		public ReadOnlyCollection<CreditCategoryBase> GetCredits() => CreditManager.categories;

		public ReadOnlyCollection<GameplaySetting> GetGameplaySettings()
		{
			return GameplaySettingsManager.gameSettings;
		}

		public ReadOnlyCollection<FetishSetting> GetFetishSettings()
		{
			return FetishSettingsManager.fetishes;
		}

		private Controller()
		{
			QueryData();

			AchievementData.QueryData(out _achievementData); //not linked to any display.
		}

		public static void DoNewGame()
		{
			NewGameHelpers.NewGame();
		}
	}
}
