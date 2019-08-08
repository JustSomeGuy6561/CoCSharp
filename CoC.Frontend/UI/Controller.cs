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
using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.UI
{

	//consider firing custom events instead of INotifyPropertyChanged. 
	//or, implement inotifypropertychanged, but instead of firing each time, add each property name to a hashset each time they change. then, when the code is ready
	//for the game to return to the GUI layer, fire them off. This will prevent multiple fires and make sure the data is only updated when it needs to be. 
	public sealed partial class Controller
	{
#warning Consider moving all static functions for buttons, output, input, selector here. makes it easier to link everything in.

		public static Controller instance { get; } = new Controller();

		public bool displayStats => ViewOptions.showStats;
		public bool displayTopMenu => ViewOptions.showStandardMenu;
		public PlayerStatus playerStatus => ViewOptions.playerStatus;

		//public Image outputImage; 
		public string outputImagePath { get => _outputImagePath; }
		private string _outputImagePath;
		public StringBuilder outputField => _outputField;
		private StringBuilder _outputField;
		public bool outputChanged => TextOutput.QueryData(out _outputField, out _outputImagePath);

		public ReadOnlyCollection<ButtonData> buttons { get => _buttons; }
		private ReadOnlyCollection<ButtonData> _buttons;
		public bool buttonsChanged => ButtonManager.QueryButtons(out _buttons);
		public int ValidButtons => buttons.Count(x => x != null);
		//do this later. for now fuck it.


		public InputField inputField { get => _inputField; }
		private InputField _inputField;
		public bool inputFieldChanged => InputField.QueryStatus(out _inputField);
		//WILL THROW NULL REFERENCE EXCEPTION IF INPUT FIELD IS NULL. it'd do so if you accessed inputField and asked for it there, so this seems the most consistent.
		//may return null, though this is GUI dependant and hopefully wont happen. empty string is preferable.
		internal string inputText => inputField.input;

		public void setOutputFromUI(string output)
		{
			inputField.UpdateInputFromOutput(output);
		}

		public DropDownMenu dropDownMenu { get => _dropDownMenu; }
		private DropDownMenu _dropDownMenu;
		public bool dropDownMenuItemsChanged => DropDownMenu.QueryStatus(out _dropDownMenu);

		public string postControlText { get => _postControlText; }
		private string _postControlText;

		public bool dropDownPostTextChanged => DropDownMenu.QueryPostText(out _postControlText);
		public bool hasPlayer => GameEngine.currentPlayer != null;

		private static GameDateTime currTime()
		{
			return GameDateTime.Now;
		}

		public readonly StatDataCollection statDataCollection = new StatDataCollection(currTime);


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

		private Controller()
		{
			InputField.QueryStatus(out _inputField);
			ButtonManager.QueryButtons(out _buttons);
			TextOutput.QueryData(out _outputField, out _outputImagePath);
			DropDownMenu.QueryStatus(out _dropDownMenu);
			DropDownMenu.QueryPostText(out _postControlText);

		}

		public void TestShit()
		{
			statDataCollection.playerStats.Strength.current = 46;
			statDataCollection.playerStats.Strength.maximum = 85;
			statDataCollection.playerStats.Lust.maximum = 100;
			statDataCollection.playerStats.Lust.current = 23;
			statDataCollection.playerStats.Lust.minimum = 10;
			statDataCollection.playerStats.HP.maximum = 216;
			statDataCollection.playerStats.HP.current = 12;
			TextOutput.AddOutput(() => "Testing <b>Bold</b>, <i> Italic</i>, and <u>Underline</u>\r\nTesting new line.\nTesting scumbag newline.\\ " +
				"Testing \\ Slashes\\par\r\n Testing bad tag </u> Testing,,,, Quotes: \" '\r\n" +
				"Testing list: <ul><li>batman</li><li>robin</li><li>nightwing</li><li>oracle</li></ul>Cool! ");
		}

		public void TestShit2()
		{
			statDataCollection.playerStats.HP.current = 35;
			statDataCollection.playerStats.Strength.current = 40;
		}

		public void DoNewGame()
		{
			CoC.Frontend.Engine.NewGameHelpers.NewGame();
		}
	}
}
