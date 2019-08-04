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
using CoC.Backend.Strings.Creatures;
using CoC.Frontend.UI;
using CoC.Frontend.UI.ControllerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace CoC.UI
{
	public enum PlayerStatus { IDLE, EXPLORING, TALKING, COMBAT, PRISON, INGRAM }


	//consider firing custom events instead of INotifyPropertyChanged. 
	//or, implement inotifypropertychanged, but instead of firing each time, add each property name to a hashset each time they change. then, when the code is ready
	//for the game to return to the GUI layer, fire them off. This will prevent multiple fires and make sure the data is only updated when it needs to be. 
	public sealed partial class Controller
	{
#warning Consider moving all static functions for buttons, output, input, selector here. makes it easier to link everything in.

		public static Controller instance
		{
			get
			{
				if (_controller is null)
					_controller = new Controller();
				return _controller;
			}
		}
		private static Controller _controller;

		public bool displayStats => ViewOptions.showStats;
		public bool displayTopMenu => ViewOptions.showStandardMenu;

		//public Image outputImage; 
		public string outputImagePath => TextOutput.image;
		public StringBuilder outputField => TextOutput.data;

		public ReadOnlyCollection<ButtonData> buttons => ButtonManager.buttons;

		public InputField inputField => InputField.instance;
		//WILL THROW NULL REFERENCE EXCEPTION IF INPUT FIELD IS NULL. it'd do so if you accessed inputField and asked for it there, so this seems the most consistent.
		//may return null, though this is GUI dependant and hopefully wont happen. empty string is preferable.
		internal string inputText => inputField.input;

		public DropDownMenu dropDownMenu => DropDownMenu.instance;

		public bool hasPlayer => GameEngine.currentPlayer != null;

		//i'll need to update this whenever stats are changed in player. should be simple, so long as i subscribe to player change and data change events. 
		public readonly PlayerStatData playerStats = new PlayerStatData();

		public StatisticsData GetStatsData()
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


		public GameDateTime currentTime => GameDateTime.Now;

		private Controller()
		{
		}

		public PlayerStatus playerStatus { get; private set; } = PlayerStatus.IDLE;

		internal static void SetPlayerStatus(PlayerStatus status)
		{
			instance.playerStatus = status;
		}

		public void TestShit()
		{
			playerStats.Strength.current = 46;
			playerStats.Strength.maximum = 85;
			playerStats.Lust.maximum = 100;
			playerStats.Lust.current = 23;
			playerStats.Lust.minimum = 10;
			playerStats.HP.maximum = 216;
			playerStats.HP.current = 12;
			TextOutput.AddOutput(() => "Testing <b>Bold</b>, <i> Italic</i>, and <u>Underline</u>\r\nTesting new line.\nTesting scumbag newline.\\ " +
				"Testing \\ Slashes\\par\r\n Testing bad tag </u> Testing,,,, Quotes: \" '\r\n" +
				"Testing list: <ul><li>batman</li><li>robin</li><li>nightwing</li><li>oracle</li></ul>Cool! ");
		}

		public void TestShit2()
		{
			playerStats.HP.current = 35;
			playerStats.Strength.current = 40;
		}

		public void DoNewGame()
		{
			CoC.Frontend.Engine.NewGameHelpers.NewGame();
		}
	}
}
