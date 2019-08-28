using CoC.Backend;
using CoC.UI;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoCWinDesktop
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

		public static readonly ReadOnlyCollection<BackgroundWrapper> backgrounds;
		public static int numBackgrounds => backgrounds.Count;

		public static readonly ReadOnlyCollection<TextBackgroundWrapper> textBackgrounds;
		public static int numTextBackgrounds => textBackgrounds.Count;

		private static readonly FontFamily sidebarLegacy = new FontFamily("Lucida Sans Typewriter");
		private static readonly FontFamily sidebarModern = new FontFamily("Palatino Linotype");
		#endregion
		public Controller controller => Controller.instance;

		#region ModelViews and Navigation
		private readonly MainMenuModelView mainMenu;
		private readonly OptionsModelView options;
		private readonly StandardModelView standard;
		private readonly CombatModelView combat;
		private readonly DataModelView data;

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

		public SafeAction resumeGameAction { get; private set; } = null;
		#endregion

		//Font size is super annoying and stupid - Everything in WPF defaults to using Pixels, except RTF content, which uses EMs. Further, any typeface magic requires EMs. Unfortunately,
		//RTF only respects EMs, so that gets the highest priority. Thus Ems. It also lets us use integers. Note that any UI that does not require common logic, constants can be used
		//but i'd recommend using strings literals with "em" as a prefix, for consistency. Note, this only applies to font size. everything else uses pixels. 
		public void SetFontSize(double amount, SizeUnit sizeUnit)
		{
			double oldValue = saveData.FontSizeInEms;
			saveData.FontSizeInEms = MeasurementHelpers.ConvertToEms(amount, sizeUnit);
			if (oldValue != saveData.FontSizeInEms)
			{
				RaisePropertyChanged(nameof(FontSizePixels));
				RaisePropertyChanged(nameof(FontSizePoints));
				RaisePropertyChanged(nameof(FontSizeEms));
			}
		}

		#region SaveData
		//Everything but the RTF uses size in pixels. However, points, and thus EMs, are greatly preferred because they work nicely with RTF and Typeface. 
		public double FontSizePixels => MeasurementHelpers.ConvertFromEms(saveData.FontSizeInEms, SizeUnit.PIXELS);

		public int FontSizeEms => (int)MeasurementHelpers.ConvertFromEms(saveData.FontSizeInEms, SizeUnit.EMS);
		public double FontSizePoints => MeasurementHelpers.ConvertFromEms(saveData.FontSizeInEms, SizeUnit.POINTS);

		public int BackgroundIndex
		{
			get => saveData.backgroundIndex;
			set
			{
				saveData.backgroundIndex = value;
				SetBackground();
			}

		}
		public int TextBackgroundIndex
		{
			get => saveData.textBackgroundIndex;
			set
			{
				saveData.textBackgroundIndex = value;
				SetTextBackground();
			}

		}
		public bool? UsesOldSprites
		{
			get => saveData.usesOldSprites;
			set => saveData.usesOldSprites = value;
		}

		public bool ImagePackEnabled
		{
			get => saveData.imagePackEnabled;
			set => saveData.imagePackEnabled = value;
		}
		public bool IsAnimated
		{
			get => saveData.isAnimated;
			set
			{
				bool oldValue = saveData.isAnimated;
				saveData.isAnimated = value;
				if (oldValue != saveData.isAnimated)
				{
					RaisePropertyChanged(nameof(IsAnimated));
				}
			}

		}
		public bool ShowEnemyStatBars
		{
			get => saveData.showEnemyStatBars;
			set
			{
				bool oldValue = saveData.showEnemyStatBars;
				saveData.showEnemyStatBars = value;
				if (oldValue != saveData.showEnemyStatBars)
				{
					RaisePropertyChanged(nameof(ShowEnemyStatBars));
				}
			}
		}

		public bool SidebarUsesModernFont
		{
			get => saveData.sidebarUsesModernFont;
			set
			{
				saveData.sidebarUsesModernFont = value;
				SidebarFontFamily = SidebarUsesModernFont ? sidebarModern : sidebarLegacy;
			}

		}
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

		public BitmapImage GetSprite(string spriteName)
		{
			if (UsesOldSprites == null)
			{
				return null;
			}
			else
			{
				string sourcePath = UsesOldSprites == true ? @"pack://application:,,,/resources/sprites8bit/" : @"pack://application:,,,/resources/sprites/";

				BitmapImage retVal = null;
				if (Uri.TryCreate(sourcePath + spriteName, UriKind.RelativeOrAbsolute, out Uri file))
				{
					try
					{
						retVal = new BitmapImage(file);
					}
					catch (Exception)
					{
						retVal = null;
					}
				}
				return retVal;
			}
		}

		public BitmapImage GetImage(string imageName)
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

		public readonly HotKeyWrapper MainMenuHotkey = new HotKeyWrapper(Key.M, ModifierKeys.None);

		public readonly HotKeyWrapper DataHotkey = new HotKeyWrapper(Key.H, ModifierKeys.None);

		public readonly HotKeyWrapper StatHotkey = new HotKeyWrapper(Key.Y, ModifierKeys.None);

		public readonly HotKeyWrapper LevelHotkey = new HotKeyWrapper(Key.L, ModifierKeys.None);

		public readonly HotKeyWrapper AppearanceHotkey = new HotKeyWrapper(Key.U, ModifierKeys.None);

		public readonly HotKeyWrapper Button1Hotkey = new HotKeyWrapper(Key.D1, ModifierKeys.None);

		public readonly HotKeyWrapper Button2Hotkey = new HotKeyWrapper(Key.D2, ModifierKeys.None);

		public readonly HotKeyWrapper Button3Hotkey = new HotKeyWrapper(Key.D3, ModifierKeys.None);

		public readonly HotKeyWrapper Button4Hotkey = new HotKeyWrapper(Key.D4, ModifierKeys.None);

		public readonly HotKeyWrapper Button5Hotkey = new HotKeyWrapper(Key.D5, ModifierKeys.None);

		public readonly HotKeyWrapper Button6Hotkey = new HotKeyWrapper(Key.D6, ModifierKeys.None, Key.Q, ModifierKeys.None);

		public readonly HotKeyWrapper Button7Hotkey = new HotKeyWrapper(Key.D7, ModifierKeys.None, Key.W, ModifierKeys.None);

		public readonly HotKeyWrapper Button8Hotkey = new HotKeyWrapper(Key.D8, ModifierKeys.None, Key.E, ModifierKeys.None);

		public readonly HotKeyWrapper Button9Hotkey = new HotKeyWrapper(Key.D9, ModifierKeys.None, Key.R, ModifierKeys.None);

		public readonly HotKeyWrapper Button10Hotkey = new HotKeyWrapper(Key.D0, ModifierKeys.None, Key.T, ModifierKeys.None);

		public readonly HotKeyWrapper Button11Hotkey = new HotKeyWrapper(Key.A, ModifierKeys.None);

		public readonly HotKeyWrapper Button12Hotkey = new HotKeyWrapper(Key.S, ModifierKeys.None);

		public readonly HotKeyWrapper Button13Hotkey = new HotKeyWrapper(Key.D, ModifierKeys.None);

		public readonly HotKeyWrapper Button14Hotkey = new HotKeyWrapper(Key.F, ModifierKeys.None);

		public readonly HotKeyWrapper Button15Hotkey = new HotKeyWrapper(Key.G, ModifierKeys.None);

		public readonly HotKeyWrapper QuickSaveHotkey = new HotKeyWrapper(Key.F5, ModifierKeys.None);

		public readonly HotKeyWrapper QuickLoadHotkey = new HotKeyWrapper(Key.F9, ModifierKeys.None);

		public readonly HotKeyWrapper CycleBackgroundForwardHotkey = new HotKeyWrapper(Key.F2, ModifierKeys.None);
		#endregion


		public bool IsDarkMode => backgrounds[BackgroundIndex].isDarkMode;

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
					whyNot = becauseReasons();
					return true;
				}
				whyNot = null;
				return false;
			}

			List<BackgroundWrapper> backgroundList = new List<BackgroundWrapper>()
			{
				new BackgroundWrapper(Path.Combine("resources", "background1.png"), Path.Combine("resources", "sidebar1.png"), InterfaceStrings.MapBGText, emptyTooltip, false),
				new BackgroundWrapper(Path.Combine("resources", "background2.png"), Path.Combine("resources", "sidebar2.png"), InterfaceStrings.ParchmentBGText, emptyTooltip, false),
				new BackgroundWrapper(Path.Combine("resources", "background3.png"), Path.Combine("resources", "sidebar3.png"), InterfaceStrings.MarbleBGText, emptyTooltip, false),
				new BackgroundWrapper(Path.Combine("resources", "background4.png"), Path.Combine("resources", "sidebar4.png"), InterfaceStrings.ObsidianBGText, emptyTooltip, true),
				new BackgroundWrapper(null, null, InterfaceStrings.NightModeBGText, emptyTooltip, true),
				new BackgroundWrapper(Path.Combine("resources", "backgroundKaizo.png"), Path.Combine("resources", "sidebarKaizo.png"), InterfaceStrings.GrimdarkBGText, kaizoTooltip, false),
			};
			backgrounds = new ReadOnlyCollection<BackgroundWrapper>(backgroundList);

			List<TextBackgroundWrapper> textBackgroundList = new List<TextBackgroundWrapper>()
			{
				new TextBackgroundWrapper(GenerateSolidColorWithTransparency(Colors.White, 0.4), InterfaceStrings.NormalTextBgDesc, emptyTooltip, false),
				new TextBackgroundWrapper(new SolidColorBrush(Colors.White), InterfaceStrings.WhiteTextBgDesc, emptyTooltip, false),
				new TextBackgroundWrapper(new SolidColorBrush(Color.FromRgb(0xEB, 0xD5, 0xA6)), InterfaceStrings.TanTextBgDesc, emptyTooltip, true),
				new TextBackgroundWrapper(new SolidColorBrush(Colors.Transparent), InterfaceStrings.ClearTextBgDesc, emptyTooltip, false),
			};
			textBackgrounds = new ReadOnlyCollection<TextBackgroundWrapper>(textBackgroundList);
		}

		private static bool condition()
		{
			throw new NotImplementedException();
		}

		private static string becauseReasons()
		{
			throw new NotImplementedException();
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
			mainMenu = new MainMenuModelView(this);
			options = new OptionsModelView(this);
			standard = new StandardModelView(this);
			combat = new CombatModelView(this);
			data = new DataModelView(this);

			_modelView = mainMenu;

			_BackgroundImage = backgrounds[BackgroundIndex].path;
			_SidebarBackgroundImage = backgrounds[BackgroundIndex].sidebarPath;
			_SidebarFontFamily = SidebarUsesModernFont ? sidebarModern : sidebarLegacy;
			_TextBackground = textBackgrounds[TextBackgroundIndex].color;
			SetFontColor();
		}

		#region View Switching

		internal void SwitchToMainMenu()
		{
			SwitchViews(mainMenu);
		}

		internal void SwitchToStandard()
		{
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
			if (ModelView is StandardModelView standardModel)
			{
				this.resumeGameAction = GenerateSafeAction(standardModel.GetLastAction());
			}

			SafeAction returnHere = GenerateSafeAction();
			ModelView = target;
			ModelView.SwitchToThisView();
		}
		#endregion
		#region View Switching Helpers
		public SafeAction GenerateSafeAction(Action callback)
		{
			if (callback is null) throw new ArgumentNullException(nameof(callback));
			ModelViewBase currModel = ModelView;
			return () => actionCreator(currModel, callback);
		}

		public SafeAction GenerateSafeAction()
		{
			ModelViewBase currModel = ModelView;
			return () => actionCreator(currModel, currModel.ParseData);
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
			bool wasDarkMode = IsDarkMode;

			BackgroundImage = backgrounds[index].path;
			SidebarBackgroundImage = backgrounds[index].sidebarPath;
			if (wasDarkMode != IsDarkMode)
			{
				RaisePropertyChanged(nameof(IsDarkMode));
			}
			SetFontColor();
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
	}
}
