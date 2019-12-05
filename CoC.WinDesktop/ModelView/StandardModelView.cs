using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.UI;
using CoC.WinDesktop.ContentWrappers;
using CoC.WinDesktop.ContentWrappers.ButtonWrappers;
using CoC.WinDesktop.CustomControls;
using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoC.WinDesktop.ModelView
{
	public sealed partial class StandardModelView : ModelViewBase
	{
		//Input field uses the same font size as the rest of the game, plus or minus a set amount. To handle variable font size, we need to know how big to make our
		//textbox that the user can input things into, so we need to know roughly how many characters to allow. The longest name i could think of ("Christopher") is only
		//13 Characters. The longest special character as of now is 10 characters ("Vahdunbrii"/"Rann Rayla"). It also need to be able to fit in the sidebar, so...
		//For Sizing, we're using this number of the widest character, in the given font and size -AFAIK W is the widest character in non kerned fonts, for English.
		//obviously, if this ever gets translated, which we support, this will need to be updated to work for the current language.

		public const byte INPUT_FIELD_MAX_CHARS = 16;
		public const char INPUT_FIELD_WIDEST_CHAR = 'W';

		Controller controller => Controller.instance;

		#region StatBar
		private readonly StatDisplayParser statDisplayParser;

		public SideBarBase sideBar
		{
			get => _sideBar;
			private set => CheckPropertyChanged(ref _sideBar, value);
		}
		private SideBarBase _sideBar;

		public bool ShowSidebar
		{
			get => _ShowSidebar;
			private set => CheckPrimitivePropertyChanged(ref _ShowSidebar, value);
		}
		private bool _ShowSidebar = true;

		public SideBarBase enemySideBar
		{
			get => _enemySideBar;
			private set => CheckPropertyChanged(ref _enemySideBar, value);
		}
		private SideBarBase _enemySideBar;

		public bool ShowEnemySidebar
		{
			get => _ShowEnemySidebar;
			private set => CheckPrimitivePropertyChanged(ref _ShowEnemySidebar, value);
		}
		private bool _ShowEnemySidebar = false;
		#endregion

		#region Credits And Sprite

		public string SceneByText => SceneByStr();

		public string authorText
		{
			get => _authorText;
			private set => CheckPropertyChanged(ref _authorText, value);
		}
		private string _authorText = null; //expects null for hidden.

		public BitmapImage sprite
		{
			get => _sprite;
			private set => CheckPropertyChanged(ref _sprite, value);
		}
		private BitmapImage _sprite = null;

		private string spriteUri
		{
			get => _spriteUri;
			set
			{
				if (_spriteUri != value)
				{
					_spriteUri = value;
					sprite = ImageHelper.GetImage(_spriteUri);
				}
			}
		}
		private string _spriteUri;

		#endregion

		#region Output Field
		public string output
		{
			get => _output;
			private set => CheckPropertyChanged(ref _output, value);
		}
		private string _output = "";

		public BitmapImage bitmap
		{
			get => _bitmap;
			set => CheckPropertyChanged(ref _bitmap, value);
		}
		private BitmapImage _bitmap = null;

		public string postControlText
		{
			get => _postControlText;
			private set => CheckPropertyChanged(ref _postControlText, value);
		}
		private string _postControlText = "";
		#endregion

		#region Top Row Buttons

		public bool showTopRow
		{
			get => _showTopRow;
			private set => CheckPrimitivePropertyChanged(ref _showTopRow, value);
		}
		private bool _showTopRow = true;

		public AutomaticButtonWrapper MainMenuButton { get; }

		public AutomaticButtonWrapper DataButton { get; }

		public AutomaticButtonWrapper StatsButton { get; }

		public ManualButtonWrapper LevelingButton { get; } = new ManualButtonWrapper();

		public AutomaticButtonWrapper PerksButton { get; }

		public AutomaticButtonWrapper AppearanceButton { get; }

		//public string DataText
		//{
		//	get => _DataText;
		//	private set => CheckPropertyChanged(ref _DataText, value);
		//}
		//private string _DataText;
		//public ICommand GoToDataScreen => new RelayCommand(HandleData, () => true);

		//public string StatText
		//{
		//	get => _StatText;
		//	private set => CheckPropertyChanged(ref _StatText, value);
		//}
		//private string _StatText;
		//public ICommand DoStats => new RelayCommand(HandleStatsScreen, () => true);

		//public string LevelUpText
		//{
		//	get => _LevelUpText;
		//	private set => CheckPropertyChanged(ref _LevelUpText, value);
		//}
		//private string _LevelUpText = "Level Up";
		//public ICommand DoLeveling => new RelayCommand(HandleLeveling, () => CanDoLeveling);

		//public string PerkText
		//{
		//	get => _PerkText;
		//	private set => CheckPropertyChanged(ref _PerkText, value);
		//}
		//private string _PerkText;
		//public ICommand DoPerks => new RelayCommand(HandlePerksScreen, () => true);

		//public string AppearanceText
		//{
		//	get => _AppearanceText;
		//	private set => CheckPropertyChanged(ref _AppearanceText, value);
		//}
		//private string _AppearanceText;
		//public ICommand DoAppearance => new RelayCommand(HandleAppearanceScreen, () => true);

		//private bool CanDoLeveling
		//{
		//	get => _CanDoLeveling;
		//	set
		//	{
		//		if (_CanDoLeveling != value)
		//		{
		//			_CanDoLeveling = value;
		//			((RelayCommand)DoLeveling).RaiseExecuteChanged();
		//		}
		//	}
		//}
		//private bool _CanDoLeveling = false;
		#endregion

		#region Bottom Buttons

		private readonly ManualButtonWrapper[] bottomButtonHolder = new ManualButtonWrapper[15];

		public ReadOnlyCollection<ManualButtonWrapper> BottomButtons { get; }
		#endregion

		#region ExtraControl Properties
		public string InputText
		{
			get => _inputText;
			set
			{
				CheckPropertyChanged(ref _inputText, value);
				if (controller.inputField != null)
				{
					controller.setOutputFromUI(_inputText);
				}
			}
		}
		private string _inputText;


		public bool InputInUse
		{
			get => _inputInUse;
			private set => CheckPrimitivePropertyChanged(ref _inputInUse, value);
		}
		private bool _inputInUse;
		public double InputWidth
		{
			get => _inputWidth;
			private set => CheckPrimitivePropertyChanged(ref _inputWidth, value);
		}
		private double _inputWidth;
		public int InputMaxLen
		{
			get => _inputMaxLen;
			private set => CheckPrimitivePropertyChanged(ref _inputMaxLen, value);
		}
		private int _inputMaxLen;
		public Regex InputCharRegex
		{
			get => _inputCharRegex;
			private set => CheckPropertyChanged(ref _inputCharRegex, value);
		}
		private Regex _inputCharRegex;

		public Regex StringValidRegex
		{
			get => _StringValidRegex;
			private set => CheckPropertyChanged(ref _StringValidRegex, value);
		}
		private Regex _StringValidRegex;

		public bool DropdownInUse
		{
			get => _dropdownInUse;
			private set => CheckPrimitivePropertyChanged(ref _dropdownInUse, value);
		}
		private bool _dropdownInUse = false;

		public ComboBoxWrapper DropdownWrapper { get; }
		#endregion

		#region Private

		//private bool isLoadingStatus;
#pragma warning disable IDE0044 // Add readonly modifier
		private int LastLanguageIndex;
#pragma warning restore IDE0044 // Add readonly modifier

		private Action lastAction;

		//private readonly StringParserUtil parser = StringUtils.GetParser;
		#endregion

		public StandardModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			Controller controller = modelViewRunner.controller;

			MainMenuButton = new AutomaticButtonWrapper(MainMenuStr, HandleMainMenu, MainMenuTip, null);
			DataButton = new AutomaticButtonWrapper(DataStr, HandleData);
			StatsButton = new AutomaticButtonWrapper(StatsStr, HandleStatsScreen);
			LevelingButton.UpdateButtonHidden();
			PerksButton = new AutomaticButtonWrapper(PerksStr, HandlePerksScreen);
			AppearanceButton = new AutomaticButtonWrapper(AppearanceStr, HandleAppearanceScreen);

			statDisplayParser = new StatDisplayParser(controller.statDataCollection);

			LastLanguageIndex = LanguageEngine.currentLanguageIndex;

			sideBar = statDisplayParser.GetSideBarBase(true, CoC.Frontend.UI.PlayerStatus.IDLE); //set it to the default to start with.

			for (int x = 0; x < 15; x++)
			{
				bottomButtonHolder[x] = new ManualButtonWrapper();
			}


			DropdownWrapper = new ComboBoxWrapper(new List<ComboBoxItemWrapper>());

			BottomButtons = new ReadOnlyCollection<ManualButtonWrapper>(bottomButtonHolder);

		}



		protected override void OnSwitchFrom()
		{
			runner.SetSafeAction(lastAction);
		}

		protected override void OnSwitchTo()
		{
			if (runner.resumeGameAction is null)
			{
				DoNewGame();
			}
			else
			{
				lastAction = OnSwitchTo;
				Controller.ForceReloadFromGUI();
				runner.resumeGameAction();
			}
		}

		private void DoNewGame()
		{
			Controller.DoNewGame();
			ParseData();
		}

		//standard view run.
		protected override void ParseDataForDisplay()
		{
			//Application.Current.

			//the only way we hit need to update display is if we're at a location that allows us to do so. by definition, this location MUST have a OnReload method,
			//so we're going to force it to call.

			bool needToUpdateDisplay = lastAction != ParseData;

			lastAction = ParseData;

			//update the data in the controller.
			controller.QueryData();

			//handle main menu and stat visibility
			showTopRow = controller.displayTopMenu;

			#warning Handle Level Up Button - idk how yet.
			//LevelingButton.UpdateEnabled(...

			ShowSidebar = controller.displayStats;
			if (ShowSidebar)
			{
				sideBar.ClearArrows();
				//get the current sidebar;
				sideBar = statDisplayParser.GetSideBarBase(true, controller.playerStatus);
				//and update it.
				sideBar.UpdateSidebar(controller.statDataCollection);
			}

			//handle data that appears in the text view.
			if (controller.outputChanged || needToUpdateDisplay)
			{
				//deep copy string builder because we fuck it up good while parsing it. in the rare case the game doesn't clear the text before adding more text (generally a mistake,
				//but may not be the case) it'll break really badly when we go to parse it again (for example: \par becomes \\par which prints out "\par", not a newline).
				StringBuilder sb = new StringBuilder(controller.outputField.ToString());
				output = ParseOutput(sb);
			}
			//handle extra elements

			//we may later need to put these in a canvas or grid or something to make them play nice with each other.
			//if (controller.inputField != null)
			//{


			if (controller.inputField.active)
			{
				if (controller.inputFieldChanged || needToUpdateDisplay)
				{
					InputMaxLen = controller.inputField.maxChars == null || controller.inputField.maxChars >= INPUT_FIELD_MAX_CHARS ? INPUT_FIELD_MAX_CHARS : (int)controller.inputField.maxChars;
					char[] maxLength = new char[InputMaxLen];
					for (int x = 0; x < InputMaxLen; x++)
					{
						maxLength[x] = INPUT_FIELD_WIDEST_CHAR;
					}
					CultureInfo cultureInfo = CultureInfo.CurrentCulture;
					FlowDirection flowDirection = cultureInfo.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
					Typeface typeface = new Typeface(runner.TextFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, new FontFamily("Times New Roman"));
					FormattedText formattedText = new FormattedText(maxLength.ToString(), CultureInfo.CurrentCulture, flowDirection, typeface, runner.FontSizeEms, runner.FontColor,
						VisualTreeHelper.GetDpi(new TextBlock()).PixelsPerDip);

					InputInUse = true;
					InputWidth = formattedText.Width + 6; //the offset for empty is 6. No Longer need to multiply by 4/3 b/c we're actually using ems now.
					InputText = controller.inputField.defaultInput;
					InputCharRegex = controller.inputField.limitValidInputCharacters;
					StringValidRegex = controller.inputField.checkTextForValidity;
				}
			}
			else
			{
				InputInUse = false;
			}

			if (controller.dropDownMenu.active)
			{
				if (controller.dropDownMenuItemsChanged || needToUpdateDisplay)
				{
					List<ComboBoxItemWrapper> wrapper = new List<ComboBoxItemWrapper>();
					Action WrapCallback(Action entry)
					{
						return () => { entry?.Invoke(); ParseData(); };
					}
					foreach (var entry in controller.dropDownMenu.entries)
					{
						wrapper.Add(new ComboBoxItemWrapper(WrapCallback(entry.onSelect), entry.title));
					}
					DropdownWrapper.UpdateCollection(wrapper, null); //in hindsight, a default value wouldn't really make sense, would it. still, clearing the default is probably good.

					DropdownInUse = true;
				}

				if (controller.dropDownPostTextChanged || needToUpdateDisplay)
				{
					postControlText = ParseOutput(new StringBuilder(controller.postControlText));
				}
			}
			else
			{
				DropdownInUse = false;
				postControlText = "";
			}

			if (controller.spriteChanged || needToUpdateDisplay)
			{
				if (!string.IsNullOrWhiteSpace(controller.SpriteName))
				{
					spriteUri = runner.GetSpriteUriString(controller.SpriteName);
				}
				else
				{
					spriteUri = null;
				}
			}
			if (!string.IsNullOrWhiteSpace(controller.CreatorName))
			{
				authorText = controller.CreatorName;
			}
			else
			{
				authorText = null;
			}

			if (controller.buttonsChanged || needToUpdateDisplay)
			{
				bool onlyOneButton = controller.ValidButtons == 1;
				if (controller.buttons.Count <= 15)
				{
					for (int x = 0; x < controller.buttons.Count; x++)
					{
						ParseButton(controller.buttons.ElementAtOrDefault(x), x, onlyOneButton);
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
						ParseButton(tooManyButtonsYouAsshat[0][x], x, onlyOneButton);
					}
					ParseButton(tooManyButtonsYouAsshat[0][11], 11, onlyOneButton, false);
				}
				IInputElement keyboardElement = Keyboard.FocusedElement;
				Keyboard.ClearFocus();
				Keyboard.Focus(keyboardElement);
			}
		}

		private void ChangeTooManyButtonsPage(int newIndex)
		{
			for (int x = 0; x < 10; x++)
			{
				ParseButton(tooManyButtonsYouAsshat[newIndex][x], x, false);
			}
			ParseButton(tooManyButtonsYouAsshat[newIndex].ElementAtOrDefault(10), 10, false);
			ParseButton(tooManyButtonsYouAsshat[newIndex].ElementAtOrDefault(11), 11, false);
		}

		private ButtonData[][] tooManyButtonsYouAsshat;

		private void ParseButton(ButtonData button, int index, bool onlyOneButton, bool wrapCallback = true)
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
					BottomButtons[index].UpdateButtonEnabled(callback, onlyOneButton, button.title, button.tooltip, button.tooltipTitle);
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

		//handles output for primary and secondary, based on what is passed in.
		//we convert our html-like code to RTF, so RichTextBox can just load it as a "document" and keep all our formatting. In reality, we just dump a string into a memory stream
		//but the converter doesn't know the difference between a memory stream and an inputstream, so we're fine.
		private string ParseOutput(StringBuilder outputBuilder)
		{
			return RTFParser.FromHTML(outputBuilder, runner);
		}

		private void HandleMainMenu()
		{
			runner.SwitchToMainMenu();
		}

		private void HandleData()
		{
			runner.SwitchToData();
		}

		private void HandleStatsScreen()
		{
			lastAction = HandleStatsScreen;

			DropdownInUse = true;
		}
		private void HandleLeveling()
		{
			lastAction = HandleLeveling;
		}

		private void HandlePerksScreen()
		{
			lastAction = HandlePerksScreen;
			//get the perk data. parse it, continue.
		}
		private void HandleAppearanceScreen()
		{
			lastAction = HandleAppearanceScreen;
			//get the appearance data, parse it, continue.
		}

		private void ReturnToStandardContent()
		{
			ParseData();
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
			sideBar.ClearArrows();
		}
	}

}
