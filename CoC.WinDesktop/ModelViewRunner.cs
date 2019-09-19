using CoC.UI;
using CoC.WinDesktop.ContentWrappers;
using CoC.WinDesktop.DisplaySettings;
using CoC.WinDesktop.Engine;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.InterfaceSettings;
using CoC.WinDesktop.ModelView;
using CoC.WinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoC.WinDesktop
{
	public sealed class ModelViewRunner : NotifierBase
	{
		//Aside from being the runner, this class also stores "global" settings - backgrounds, font colors, etc. Note that some stuff is set from code-behind, so this provides
		//a universal means to get data in both contexts. For example, a Flow Document loaded from RTF will ignore all of it's contained classes flags (except for align, for some reason)
		//so we have to set these through code-behind, notably font color. However, for the stuff that DOES respect font color (like sidebar), we also need to be able to bind it.
		//so i'm just storing them here. Note that it's entirely possible to do these in the class that defines them, but that always seems to work sporadically, so i'm just putting it here.

		public const int HeaderSizeEm = 54;//==36 px in app.xaml.
		public const int SmallHeaderEm = 36;//==24px in app.xaml.
		public const int ItemSizeEm = 27;//==18px in app.xaml.

		#region  Static Data
		private static GuiGlobalSave saveData => GuiGlobalSave.data;

		public static readonly ReadOnlyCollection<BackgroundItem> backgrounds;
		public static int numBackgrounds => backgrounds.Count;

		public static readonly ReadOnlyCollection<TextBackgroundItem> textBackgrounds;
		public static int numTextBackgrounds => textBackgrounds.Count;

		private static readonly FontFamily sidebarLegacy = new FontFamily("Lucida Sans Typewriter");
		private static readonly FontFamily sidebarModern = new FontFamily("Palatino Linotype");
		#endregion
		public Controller controller => Controller.instance;

		#region ModelViews and Navigation
		private readonly MainMenuModelView mainMenu;
		private readonly OptionsModelView options;
		private readonly StandardModelView standard;
		private readonly DataModelView data;
		private readonly ExtraMenuItemsModelView extraItems;

		public ModelViewBase ModelView
		{
			get => _modelView;
			private set
			{
				var oldView = _modelView;
				if (CheckPropertyChanged(ref _modelView, value))
				{
					previousModelView = oldView;
				}
			}
		}
		private ModelViewBase _modelView;

		private ModelViewBase previousModelView;

		public SafeAction resumeGameAction
		{
			get => _resumeGameAction;
			private set => CheckPropertyChanged(ref _resumeGameAction, value);
		}
		private SafeAction _resumeGameAction = null;
		#endregion

		#region SaveData
		//Everything but the RTF uses size in pixels. However, points, and thus EMs, are greatly preferred because they work nicely with RTF and Typeface. 
		
		//BOUND, Handled
		public double FontSizePixels => MeasurementHelpers.FontSizeInPx;
		//Not (yet) bound, but handled anyway.
		public int FontSizeEms => MeasurementHelpers.FontSizeInEms;
		//Not (yet bound, but handled anyway. 
		public double FontSizePoints => MeasurementHelpers.FontSizeInPt;

		//not bound - used to determine another property. that property is handled. 
		public int BackgroundIndex => saveData.backgroundIndex;

		//not bound - used to determine text background, which is handled. 
		public int TextBackgroundIndex => saveData.textBackgroundIndex;

		//not bound - when the Standard model view needs to query a sprite, it calls a function here in the runner that uses this value. 
		public bool? UsesOldSprites => saveData.usesOldSprites;

		//not bound - when the standard model view needs to query an image, it calls a function here in the runner that uses this value.
		public bool ImagePackEnabled => saveData.imagePackEnabled;

		//bound, handled
		public bool IsAnimated => saveData.isAnimated;
		
		//bound, handled
		public bool ShowEnemyStatBars => saveData.showEnemyStatBars;

		//not bound -used to determine another value, which is bound and handled.
		public bool SidebarUsesModernFont => saveData.sidebarUsesModernFont;
		#endregion

		#region GUI Related
		public string BackgroundImage
		{
			get => _BackgroundImage;
			private set => CheckPropertyChanged(ref _BackgroundImage, value);
		}
		private string _BackgroundImage = null;

		public string SidebarBackgroundImage
		{
			get => _SidebarBackgroundImage;
			private set => CheckPropertyChanged(ref _SidebarBackgroundImage, value);
		}
		private string _SidebarBackgroundImage = null;

		public FontFamily SidebarFontFamily
		{
			get => _SidebarFontFamily;
			private set => CheckPropertyChanged(ref _SidebarFontFamily, value);
		}
		private FontFamily _SidebarFontFamily = null;

		public SolidColorBrush TextBackground
		{
			get => _TextBackground;
			private set => CheckPropertyChanged(ref _TextBackground, value);
		}
		private SolidColorBrush _TextBackground = null;
		public FontFamily TextFontFamily
		{
			get => _textFontFamily;
			private set => CheckPropertyChanged(ref _textFontFamily, value);
		}
		private FontFamily _textFontFamily = new FontFamily("Times New Roman");

		public SolidColorBrush FontColor
		{
			get => _FontColor;
			private set => CheckPropertyChanged(ref _FontColor, value);
		}
		private SolidColorBrush _FontColor = null; //start out with black.

		public SolidColorBrush ButtonDisableHoverTextColor
		{
			get => _ButtonDisableHoverTextColor;
			private set => CheckPropertyChanged(ref _ButtonDisableHoverTextColor, value);
		}
		private SolidColorBrush _ButtonDisableHoverTextColor = null;
		#endregion

		public string GetSpriteUriString(string spriteName)
		{
			if (UsesOldSprites == null)
			{
				return null;
			}
			else
			{
				string sourcePath = UsesOldSprites == true ? @"pack://application:,,,/resources/sprites8bit/" : @"pack://application:,,,/resources/sprites/";
				return sourcePath + spriteName;
			}
		}

		public string GetImageUriString(string imageName)
		{
			if (!ImagePackEnabled)
			{
				return null;
			}
			else
			{
				throw new NotImplementedException("I have no idea how we'll do this.");
			}
		}

		#region HotKeys

		public readonly HotKeyWrapper MainMenuHotkey = new HotKeyWrapper(HotKeyStrings.MainMenuHotkeyStr, Key.M, ModifierKeys.None);

		public readonly HotKeyWrapper DataHotkey = new HotKeyWrapper(HotKeyStrings.DataHotkeyStr, Key.H, ModifierKeys.None);

		public readonly HotKeyWrapper StatHotkey = new HotKeyWrapper(HotKeyStrings.StatHotkeyStr, Key.Y, ModifierKeys.None);

		public readonly HotKeyWrapper LevelHotkey = new HotKeyWrapper(HotKeyStrings.LevelHotkeyStr, Key.L, ModifierKeys.None);

		public readonly HotKeyWrapper PerksHotkey = new HotKeyWrapper(HotKeyStrings.PerksHotkeyStr, Key.P, ModifierKeys.None);
		public readonly HotKeyWrapper AppearanceHotkey = new HotKeyWrapper(HotKeyStrings.AppearanceHotkeyStr, Key.U, ModifierKeys.None);

		public readonly HotKeyWrapper Button1Hotkey = new HotKeyWrapper(HotKeyStrings.Button1HotkeyStr, Key.D1, ModifierKeys.None);

		public readonly HotKeyWrapper Button2Hotkey = new HotKeyWrapper(HotKeyStrings.Button2HotkeyStr, Key.D2, ModifierKeys.None);

		public readonly HotKeyWrapper Button3Hotkey = new HotKeyWrapper(HotKeyStrings.Button3HotkeyStr, Key.D3, ModifierKeys.None);

		public readonly HotKeyWrapper Button4Hotkey = new HotKeyWrapper(HotKeyStrings.Button4HotkeyStr, Key.D4, ModifierKeys.None);

		public readonly HotKeyWrapper Button5Hotkey = new HotKeyWrapper(HotKeyStrings.Button5HotkeyStr, Key.D5, ModifierKeys.None);

		public readonly HotKeyWrapper Button6Hotkey = new HotKeyWrapper(HotKeyStrings.Button6HotkeyStr, Key.D6, ModifierKeys.None, Key.Q, ModifierKeys.None);

		public readonly HotKeyWrapper Button7Hotkey = new HotKeyWrapper(HotKeyStrings.Button7HotkeyStr, Key.D7, ModifierKeys.None, Key.W, ModifierKeys.None);

		public readonly HotKeyWrapper Button8Hotkey = new HotKeyWrapper(HotKeyStrings.Button8HotkeyStr, Key.D8, ModifierKeys.None, Key.E, ModifierKeys.None);

		public readonly HotKeyWrapper Button9Hotkey = new HotKeyWrapper(HotKeyStrings.Button9HotkeyStr, Key.D9, ModifierKeys.None, Key.R, ModifierKeys.None);

		public readonly HotKeyWrapper Button10Hotkey = new HotKeyWrapper(HotKeyStrings.Button10HotkeyStr, Key.D0, ModifierKeys.None, Key.T, ModifierKeys.None);

		public readonly HotKeyWrapper Button11Hotkey = new HotKeyWrapper(HotKeyStrings.Button11HotkeyStr, Key.A, ModifierKeys.None);

		public readonly HotKeyWrapper Button12Hotkey = new HotKeyWrapper(HotKeyStrings.Button12HotkeyStr, Key.S, ModifierKeys.None);

		public readonly HotKeyWrapper Button13Hotkey = new HotKeyWrapper(HotKeyStrings.Button13HotkeyStr, Key.D, ModifierKeys.None);

		public readonly HotKeyWrapper Button14Hotkey = new HotKeyWrapper(HotKeyStrings.Button14HotkeyStr, Key.F, ModifierKeys.None);

		public readonly HotKeyWrapper Button15Hotkey = new HotKeyWrapper(HotKeyStrings.Button15HotkeyStr, Key.G, ModifierKeys.None);

		public readonly HotKeyWrapper QuickSaveHotkey = new HotKeyWrapper(HotKeyStrings.QuickSaveHotkeyStr, Key.F5, ModifierKeys.None);

		public readonly HotKeyWrapper QuickLoadHotkey = new HotKeyWrapper(HotKeyStrings.QuickLoadHotkeyStr, Key.F9, ModifierKeys.None);

		#endregion

		public bool IsDarkMode
		{
			get => _isDarkMode;
			private set => CheckPrimitivePropertyChanged(ref _isDarkMode, value);
		}
		private bool _isDarkMode;


		static ModelViewRunner()
		{
			bool emptyTooltip(out string whyNot)
			{
				whyNot = null; return false;
			}

			bool kaizoTooltip(out string whyNot)
			{
				if (condition())
				{
					whyNot = null;
					return false;

				}
				else
				{
					whyNot = becauseReasons();
					return true;
				}
			}

			List<BackgroundItem> backgroundList = new List<BackgroundItem>()
			{
				new BackgroundItem(Path.Combine("resources", "background1.png"), Path.Combine("resources", "sidebar1.png"), BackgroundOption.MapBGText, emptyTooltip, false),
				new BackgroundItem(Path.Combine("resources", "background2.png"), Path.Combine("resources", "sidebar2.png"), BackgroundOption.ParchmentBGText, emptyTooltip, false),
				new BackgroundItem(Path.Combine("resources", "background3.png"), Path.Combine("resources", "sidebar3.png"), BackgroundOption.MarbleBGText, emptyTooltip, false),
				new BackgroundItem(Path.Combine("resources", "background4.png"), Path.Combine("resources", "sidebar4.png"), BackgroundOption.ObsidianBGText, emptyTooltip, true),
				new BackgroundItem(null, null, BackgroundOption.NightModeBGText, emptyTooltip, true),
				new BackgroundItem(Path.Combine("resources", "backgroundKaizo.png"), Path.Combine("resources", "sidebarKaizo.png"), BackgroundOption.GrimdarkBGText, kaizoTooltip, false),
			};
			backgrounds = new ReadOnlyCollection<BackgroundItem>(backgroundList);

			List<TextBackgroundItem> textBackgroundList = new List<TextBackgroundItem>()
			{
				new TextBackgroundItem(GenerateSolidColorWithTransparency(Colors.White, 0.4), TextBackgroundOption.NormalTextBgDesc, emptyTooltip, false),
				new TextBackgroundItem(new SolidColorBrush(Colors.White), TextBackgroundOption.WhiteTextBgDesc, emptyTooltip, false),
				new TextBackgroundItem(new SolidColorBrush(Color.FromRgb(0xEB, 0xD5, 0xA6)), TextBackgroundOption.TanTextBgDesc, emptyTooltip, true),
				new TextBackgroundItem(new SolidColorBrush(Colors.Transparent), TextBackgroundOption.ClearTextBgDesc, emptyTooltip, false),
			};
			textBackgrounds = new ReadOnlyCollection<TextBackgroundItem>(textBackgroundList);
		}

		private static bool condition()
		{
			return false;
		}

		private static string becauseReasons()
		{
			return "Beat the game once on Grimdark to unlock the Grimdark Background. It is used by default when playing in Grimdark Mode.";
		}

		private static SolidColorBrush GenerateSolidColorWithTransparency(Color color, double opacity)
		{
			return new SolidColorBrush(color)
			{
				Opacity = opacity
			};
		}

		public ModelViewRunner()
		{
			DisplayOptions tempDisplay = DisplayOptionManager.GetOptionOfType<TextBackgroundOption>();
			tempDisplay.AddGlobalSetListener(SetTextBackground);

			tempDisplay = DisplayOptionManager.GetOptionOfType<BackgroundOption>();
			tempDisplay.AddGlobalSetListener(SetBackground);

			tempDisplay = DisplayOptionManager.GetOptionOfType<FontSizeOption>();
			tempDisplay.AddGlobalSetListener(SetFontSize);

			InterfaceOptions tempInterface = InterfaceOptionManager.GetOptionOfType<EnemySidebarOption>();
			tempInterface.AddGlobalSetListener(OnEnemySidebarChanged);

			tempInterface = InterfaceOptionManager.GetOptionOfType<SidebarAnimationOption>();
			tempInterface.AddGlobalSetListener(OnAnimationChanged);

			tempInterface = InterfaceOptionManager.GetOptionOfType<SidebarFontOption>();
			tempInterface.AddGlobalSetListener(OnSidebarFontChanged);

			_BackgroundImage = backgrounds[BackgroundIndex].path;
			_SidebarBackgroundImage = backgrounds[BackgroundIndex].sidebarPath;
			_SidebarFontFamily = SidebarUsesModernFont ? sidebarModern : sidebarLegacy;
			_TextBackground = textBackgrounds[TextBackgroundIndex].color;

			SetFontColor();

			mainMenu = new MainMenuModelView(this);
			options = new OptionsModelView(this);
			standard = new StandardModelView(this);
			data = new DataModelView(this);
			extraItems = new ExtraMenuItemsModelView(this);

			_modelView = mainMenu;
		}

		

		#region View Switching

		internal void SwitchToMainMenu()
		{
			SwitchViews(mainMenu);
		}

		internal void SwitchToStandard(bool isNewGame)
		{
			if (isNewGame)
			{
				ClearSafeAction();
			}
			SwitchViews(standard);
		}

		internal void SwitchToOptions()
		{
			SwitchViews(options);
		}

		internal void SwitchToData()
		{
			SwitchViews(data);
		}

		internal void SwitchToCredits()
		{
			extraItems.SetState_Credits();
			SwitchViews(extraItems);
		}

		internal void SwitchToAchievements()
		{
			extraItems.SetState_Achievements();
			SwitchViews(extraItems);
		}

		internal void SwitchToInstructions()
		{
			extraItems.SetState_Instructions();
			SwitchViews(extraItems);
		}

		public void SwitchToPreviousView()
		{
			SwitchViews(previousModelView);
		}

		private void SwitchViews(ModelViewBase target)
		{
			if (target == ModelView)
			{
				return;
			}
			//We're still the old model view - we haven't changed yet. handle it's leaving this model view function.
			ModelView.SwitchFromThisView();
			//switch model views
			ModelView = target;
			//and call the new entering model view function.
			ModelView.SwitchToThisView();
		}
		#endregion
		#region View Switching Helpers

		internal void SetSafeAction(Action lastAction)
		{
			resumeGameAction = GenerateSafeAction(lastAction);
		}
		internal void ClearSafeAction()
		{
			resumeGameAction = null;
		}

		public SafeAction GenerateSafeAction(Action callback)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			ModelViewBase currModel = ModelView;
			return () => actionCreator(currModel, callback);
		}

		private void actionCreator(ModelViewBase model, Action callback)
		{
			ModelView = model;
			callback();
		}
		#endregion

		private void SetBackground()
		{
			int index = BackgroundIndex;
			IsDarkMode = backgrounds[index].isDarkMode;
			BackgroundImage = backgrounds[index].path;
			SidebarBackgroundImage = backgrounds[index].sidebarPath;
			SetFontColor();
		}

		private void SetFontSize()
		{
			RaisePropertyChanged(nameof(FontSizePixels));
			RaisePropertyChanged(nameof(FontSizeEms));
			RaisePropertyChanged(nameof(FontSizePoints));
		}

		private void SetTextBackground()
		{
			TextBackground = textBackgrounds[TextBackgroundIndex].color;
			SetFontColor();
		}

		private void SetFontColor()
		{
			if (IsDarkMode)
			{
				if (textBackgrounds[TextBackgroundIndex].affectedByDarkMode)
				{
					FontColor = new SolidColorBrush(Colors.White);
				}
				else
				{
					FontColor = new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0));
				}
				ButtonDisableHoverTextColor = new SolidColorBrush(Colors.White);
			}
			else
			{
				FontColor = new SolidColorBrush(Colors.Black);
				ButtonDisableHoverTextColor = new SolidColorBrush(Colors.Transparent);
			}
		}

		private void OnEnemySidebarChanged()
		{
			RaisePropertyChanged(nameof(ShowEnemyStatBars));
		}

		private void OnAnimationChanged()
		{
			RaisePropertyChanged(nameof(IsAnimated));
		}

		private void OnSidebarFontChanged()
		{
			SidebarFontFamily = SidebarUsesModernFont ? sidebarModern : sidebarLegacy;
		}
	}
}
