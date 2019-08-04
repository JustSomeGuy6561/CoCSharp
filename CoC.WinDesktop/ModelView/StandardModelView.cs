using CoC.Frontend.UI.ControllerData;
using CoC.UI;
using CoCWinDesktop.ModelView.Helpers;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop.ModelView
{
	public sealed class StandardModelView : ModelViewBase
	{

		public override event PropertyChangedEventHandler PropertyChanged;

		#region StatBar
		#region Stats
		public string nameText
		{
			get => _nameText;
			private set => IHateYouBoat(ref _nameText, value);
		}
		private string _nameText = "Name: ";

		public string coreStatText
		{
			get => _coreStatsText;
			private set => IHateYouBoat(ref _coreStatsText, value);
		}
		private string _coreStatsText = "Core Stats:";

		public ReadOnlyCollection<StatDisplay> coreStats { get; }

		public string combatStatText
		{
			get => _combatStatsText;
			private set => IHateYouBoat(ref _combatStatsText, value);
		}
		private string _combatStatsText = "Combat Stats:";

		public ReadOnlyCollection<StatDisplay> combatStats { get; }

		public string advancementStatText
		{
			get => _advancementStatsText;
			private set => IHateYouBoat(ref _advancementStatsText, value);
		}
		private string _advancementStatsText = "Advancement:";

		public ReadOnlyCollection<StatDisplay> advancementStats { get; }
		#endregion
		public string dayStr
		{
			get => _dayStr;
			private set => IHateYouBoat(ref _dayStr, value);
		}
		private string _dayStr = "";

		public string hourStr
		{
			get => _hourStr;
			private set => IHateYouBoat(ref _hourStr, value);
		}
		private string _hourStr = "";
		#endregion

		#region Credits And Sprite
		public string authorText
		{
			get => _authorText;
			private set => IHateYouBoat(ref _authorText, value);
		}
		private string _authorText = null; //expects null for hidden.

		public BitmapImage sprite
		{
			get => _sprite;
			private set
			{
				if (_sprite != value)
				{
					_sprite = value;
					NotifyPropertyChanged();
				}
			}
		}
		private BitmapImage _sprite = null;

		#endregion

		#region Output Field
		public string output
		{
			get => _output;
			private set => IHateYouBoat(ref _output, value);
		}
		private string _output = "";

		public BitmapImage bitmap
		{
			get => _bitmap;
			set
			{
				if (_bitmap != value)
				{
					_bitmap = value;
					NotifyPropertyChanged();
				}
			}
		}
		private BitmapImage _bitmap = null;

		public ObservableCollection<Control> extraControls
		{
			get => _extraControls;
			private set
			{
				if (_extraControls != value)
				{
					_extraControls = value;
					NotifyPropertyChanged();
				}
			}
		}
		private ObservableCollection<Control> _extraControls = new ObservableCollection<Control>();

		public string postControlText
		{
			get => _postControlText;
			private set => IHateYouBoat(ref _postControlText, value);
		}
		private string _postControlText = "";
		#endregion

		#region Top Row Buttons

		public bool showTopRow
		{
			get => _showTopRow;
			private set => IHateYouBoat(ref _showTopRow, value);
		}
		private bool _showTopRow = true;

		public ICommand GoToMainMenu => new RelayCommand(HandleMainMenu, () => true);

		public ICommand GoToDataScreen => new RelayCommand(HandleData, () => true);

		public ICommand DoStats => new RelayCommand(HandleStatsScreen, () => true);

		public string LevelUpText
		{
			get => _LevelUpText;
			private set => IHateYouBoat(ref _LevelUpText, value);
		}
		private string _LevelUpText = "Level Up";

		public ICommand DoLeveling => new RelayCommand(HandleLeveling, () => CanDoLeveling);

		public ICommand DoPerks => new RelayCommand(HandlePerksScreen, () => true);

		public ICommand DoAppearance => new RelayCommand(HandleAppearanceScreen, () => true);


		private bool CanDoLeveling
		{
			get => _CanDoLeveling;
			set
			{
				if (_CanDoLeveling != value)
				{
					_CanDoLeveling = value;
					((RelayCommand)DoLeveling).RaiseExecuteChanged();
				}
			}
		}
		private bool _CanDoLeveling = false;
		#endregion

		#region Bottom Buttons

		private readonly BottomButtonWrapper[] bottomButtonHolder = new BottomButtonWrapper[15];

		public ReadOnlyCollection<BottomButtonWrapper> BottomButtons { get; }
		#endregion

		#region Private 

		private bool isLoadingStatus = false;
		private Controller controller => Controller.instance;

		private Action lastAction;

		private readonly StringParserUtil parser = StringUtils.GetParser;
		#endregion


		public StandardModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			PlayerStatData playerStats = controller.playerStats;

#warning ToDo: think of some cleaner way to do this - idk what though.
			var coreStatList = playerStats.coreStats.Select(x => new StatDisplay(x)).ToList();
			var combatStatList = playerStats.combatStats.Select(x => new StatDisplay(x)).ToList();

			var HP = combatStatList.Find(x => x.Name == "HP");
			HP.regColorDefaultOrMax = Color.FromArgb(0xFF, 0xA0, 0xFF, 0x50);
			HP.regColorMin = Color.FromArgb(0xFF, 0xFF, 0x66, 0x50);

			var lust = combatStatList.Find(x => x.Name == "lust");
			lust.regColorDefaultOrMax = Color.FromArgb(0xFF, 0xFF, 0x85, 0x69);//0xFFFF8569

			coreStats = new ReadOnlyCollection<StatDisplay>(coreStatList);
			combatStats = new ReadOnlyCollection<StatDisplay>(combatStatList);

			advancementStats = new ReadOnlyCollection<StatDisplay>(playerStats.advancementStats.Select(x => new StatDisplay(x)).ToList());

			foreach (var stat in coreStats) stat.CheckText();
			foreach (var stat in combatStats) stat.CheckText();
			foreach (var stat in advancementStats) stat.CheckText();

			for (int x = 0; x < 15; x++)
			{
				bottomButtonHolder[x] = new BottomButtonWrapper();
			}

			BottomButtons = new ReadOnlyCollection<BottomButtonWrapper>(bottomButtonHolder);
		}

		protected override bool SwitchToThisModelView(Action lastAction)
		{
			if (isLoadingStatus)
			{
				return ExecuteLoadDataDisplay();
			}
			else
			{
				DoNewGame();
				return true;
			}
		}

		private void DoNewGame()
		{
			controller.DoNewGame();
			ParseData();
		}

		internal void SetStandardStatus(bool loadingData)
		{
			isLoadingStatus = loadingData;
		}

		//standard view run.
		protected override void ParseDataForDisplay()
		{
			lastAction = ParseDataForDisplay;
			PlayerStatData stats = controller.playerStats;

			nameText = "Name: " + stats.nameString;
			coreStatText = "Core Stats:";
			combatStatText = "Combat Stats:";
			advancementStatText = "Advancement:";

			dayStr = "Date: " + controller.currentTime.day.ToString();
			hourStr = "Hour: " + controller.currentTime.GetFormattedHourString();

			//CanDoLeveling = controller.canPlayerLevelUp;

			ParseOutput();


			if (controller.buttons.Count <= 15)
			{
				for (int x = 0; x < controller.buttons.Count; x++)
				{
					ParseButton(controller.buttons.ElementAtOrDefault(x), x);
				}
			}
			//i hate you
			//NOTE: NOT TESTED!
			else //(controller.buttons.Count > 15)
			{
				int numMenus = (int)Math.Ceiling(controller.buttons.Count / 10.0);
				tooManyButtonsYouAsshat = new ButtonData[numMenus][];

				for (int x = 0; x < numMenus; x++)
				{
					bool addPrev = x > 0;
					bool addNext = x < numMenus - 1;

					int toCheck = (10 * (x + 1) <= controller.buttons.Count) ? 10 : controller.buttons.Count % 10;
					int items = addNext ? 12 : 11;

					tooManyButtonsYouAsshat[x] = new ButtonData[items];

					for (int y = 0; y < toCheck; y++)
					{
						int count = x * 10 + y;
						tooManyButtonsYouAsshat[x][y] = controller.buttons[count];
					}

					if (addNext)
					{
						tooManyButtonsYouAsshat[x][11] = ButtonData.NextButtonBecauseWereAssholes(() => ChangeTooManyButtonsPage(x + 1));
					}
					if (addPrev)
					{
						tooManyButtonsYouAsshat[x][10] = ButtonData.PrevButtonBecauseWereAssholes(() => ChangeTooManyButtonsPage(x - 1));
					}

				}
				for (int x = 0; x < 10; x++)
				{
					ParseButton(tooManyButtonsYouAsshat[0][x], x);
				}
				ParseButton(tooManyButtonsYouAsshat[0][11], 11, false);
			}
		}

		private void ChangeTooManyButtonsPage(int newIndex)
		{
			for (int x = 0; x < 10; x++)
			{
				ParseButton(tooManyButtonsYouAsshat[newIndex][x], x);
			}
			ParseButton(tooManyButtonsYouAsshat[newIndex].ElementAtOrDefault(10), 10, false);
			ParseButton(tooManyButtonsYouAsshat[newIndex].ElementAtOrDefault(11), 11, false);
		}

		private ButtonData[][] tooManyButtonsYouAsshat;

		private void ParseButton(ButtonData button, int index, bool wrapCallback = true)
		{
			if (button != null)
			{
				Action callback = button.onClick;
				if (wrapCallback)
				{
					callback = () => { button.onClick(); ParseData(); };
				}

				if (button.enabled)
				{
					BottomButtons[index].UpdateButtonEnabled(callback, button.title, button.tooltip, button.tooltipTitle);
				}
				else
				{
					BottomButtons[index].UpdateButtonDisabled(button.title, button.tooltip, button.tooltipTitle);
				}
			}
			else
			{
				BottomButtons[index].UpdateButtonHidden();
			}
		}

		//special view run for data loading. 
		private bool ExecuteLoadDataDisplay()
		{
			//if load: attempt to load the data. if successful, call runner.ParseData
			//if save: attempt to save. display results. call resume action.
			//if cancel: call resume action.
			return false;
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string FuckOffAndDieRTFIHateYou(string text, string FontFamily, List<Color> colors, int doubleFontSize)
		{
			StringBuilder sb = new StringBuilder(@"{\rtf1\ansi\deff0" + Environment.NewLine +
				@"{\fonttbl{\f0\ " + FontFamily + @";}" + Environment.NewLine +
				@"{\colortbl");

			colors.ForEach(x => sb.Append(RTFColor(x)));

			sb.Append("}}" + Environment.NewLine +
				@"{\fs" + doubleFontSize + @"\fn0\fc0 " + text + @"}");
			return sb.ToString();
			//@"{\expnd-32\expndtw-32\fs" + doubleFontSize + @"\fn0 " + text + @"}";
		}

		private string RTFColor(Color color)
		{
			return @"\red" + color.R + @"\green" + color.G + @"\blue" + color.B + @";";
		}
		private string RTFColor(SolidColorBrush colorBrush)
		{
			Color color = colorBrush.Color;
			return @"\red" + color.R + @"\green" + color.G + @"\blue" + color.B + @";";
		}

		private void ParseOutput()
		{
			StringBuilder outputBuilder = controller.outputField;

			////quick note: @ means treat string as literal, so it's looking for '\', not an escape character.
			//// @"\" is identical to "\\" So it won't replace "\r" or "\n"
			//outputBuilder.Replace(@"\", @"\\");
			//outputBuilder.Replace(@"{", @"\{"); //see above

			//outputBuilder.Replace(@"<b>", @"\b ");
			//outputBuilder.Replace(@"</b>", @"\b0 ");
			//outputBuilder.Replace(@"<i>", @"\i ");
			//outputBuilder.Replace(@"</i>", @"\i0 ");
			//outputBuilder.Replace(@"<em>", @"\i "); //who even uses this?
			//outputBuilder.Replace(@"</em>", @"\i0 ");

			//outputBuilder.Replace(@"<pre>", @"");
			//outputBuilder.Replace(@"</pre>", @"");

			////note the lack of @ on these strings - these strings we actually want to escape and look for the special character. 
			////more fun facts: IIRC Environment.NewLine returns "\n" for all things not Windows, though idk if it has funny rules for Xamarin on Mac. 
			//outputBuilder.Replace("\r\n", @"\par "); //You're on Windows and used the Environment.NewLine. Fun fact, the older parser stripped out the \r in \r\n
			//outputBuilder.Replace("\n", @"\par "); //The 'standard'. IMO the safe route of throwing both (windows) is better, though then there are parsers that treat that as \n\n and that's bad.
			//outputBuilder.Replace("\r", @"\par "); //It was written on a Mac. I hate you. JK, but damn - I'm surprised the old parser didn't blow up with \r
			//outputBuilder.Replace("\t", @"\tab ");

			////This is a simplistic way of simulating an unordered list. Hacky as all hell, and it'll break with nested lists, but we never have nested lists so this should work fine. 
			////also this will break with a random <li> or if there are ever ordered lists (<ol>). It's possible to do it correctly with about 10 different bullshit RTF tags, but fuck that.
			////I mean, if you have a hard-on for perfectly formed RTF, i think you're playing the wrong game here ¯\_(ツ)_/¯. But by all means, fix it.   
			//outputBuilder.Replace(@"<ul>", @"");
			//outputBuilder.Replace(@"</ul>", @"\line ");
			//outputBuilder.Replace(@"<li>", @"\line  \bullet  ");
			//outputBuilder.Replace(@"</li>", @"");

			//outputBuilder.Replace(@"<u>", @"\ul ");
			//outputBuilder.Replace(@"</u>", @"\ul0 ");
			//outputBuilder.Replace(@"<br/>", @"\line ");
			//outputBuilder.Replace(@"<br />", @"\line ");
			//outputBuilder.Replace(@"<br>", @"\line ");
			//outputBuilder.Replace(@"</br>", @""); //i actually hate you.

			//string result = parser.Parse(outputBuilder);

			//add the first color manually b/c it's actually a solid color brush. This is our default color for this render. 
			List<Color> colorList = new List<Color>
			{
				runner.FontColor.Color
			};

			//TODO: Find All instances of <font color=#000000>. Convert color value to Color. it'll be replaced with \fcN where N is the color index. to get the index:
			//int index = colorList.FindIndex(x => x.Equals(color)); //Find out if it's in the list
			//if (index == -1) //if not
			//{
			//	colorList.Add(color);
			//	index = colorList.Count - 1;
			//}
			//any </font> then has a \fc0 to reset to default.
			//note that this will probably have to be done manually, unless there's a regex function that does this manually. 

			//outputBuilder.Append(@"\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par\par Batman!");


			//ToDo: as of now we'll attempt to rerender as each of these fires. we should only fire once after they're all done.
			//consider binding a boolean value that disables rendering and such when off, but then when on causes all stuff to rerender.
			//string partiallyFormatted = outputBuilder.ToString();
			Console.WriteLine("--------------------Possibilities-----------------");
			string parsed =parser.Possibilities();
			Console.WriteLine(parsed);
			string partiallyFormatted = parser.Parse(outputBuilder);
			output = FuckOffAndDieRTFIHateYou(partiallyFormatted, "Times New Roman", colorList, (int)Math.Ceiling(runner.FontSize * 2));
			bitmap = null;
			extraControls.Clear();
			postControlText = "";
			//bitmap = new BitmapImage(new Uri(@"pack://application:,,,/resources/CoCLogo.png"));
			//extraControls.Add(new ComboBox());
			//postControlText = "Bana...Bananana... fuck!";
			//authorText = "JSG";
		}

		private void HandleMainMenu()
		{
			runner.SwitchToMainMenu(lastAction);
		}

		private void HandleData()
		{
			if (!ExecuteLoadDataDisplay())
			{
				lastAction();
			}
		}

		private void HandleStatsScreen()
		{
			lastAction = HandleStatsScreen;
			//StatsObject stats = controller.GetStats();
			//parse the statsObject to data the view will like. 
			//output = however we display it. afaik it's <h2>Stat Category</h2> \r\n list stats: \t\t\t\t\t value also 
			//output = "";
			bitmap = null;
			extraControls.Clear();
			postControlText = "";
			controller.TestShit2();

			foreach (var data in coreStats)
			{
				Console.WriteLine(data.ArrowVisibility);
			}
		}
		private void HandleLeveling()
		{
			lastAction = HandleLeveling;

		}

		private void HandlePerksScreen()
		{
			lastAction = HandlePerksScreen;

			controller.TestShit();
			ParseData();
		}
		private void HandleAppearanceScreen()
		{
			lastAction = HandleAppearanceScreen;

			ClearArrows();
		}


		//Buttons require commands to work. This will automatically wrap your button callback into a command that
		//runs it, then updates the GUI. Tooltip and Text and such are handled seperately. 
		private ICommand SetupCallback(Action callback, bool canUse)
		{

			void wrap()
			{
				callback();
				ParseData();
			};

			return new RelayCommand(wrap, () => canUse);

		}

		private void ClearArrows()
		{
			foreach (var s in coreStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}

			foreach (var s in combatStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}
			foreach (var s in advancementStats)
			{
				s.ArrowVisibility = Visibility.Hidden;
			}
		}

		private void IHateYouBoat<T>(ref T data, T newValue, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
		{
			if (data == null != (newValue == null) || (data != null && !data.Equals(newValue)))
			{
				data = newValue;
				NotifyPropertyChanged(propertyName);
			}
		}
	}

}
