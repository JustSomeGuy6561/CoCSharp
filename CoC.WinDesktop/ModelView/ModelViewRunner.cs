using CoC.UI;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace CoCWinDesktop.ModelView
{
	public sealed class ModelViewRunner : INotifyPropertyChanged
	{
		//Aside from being the runner, this class also stores "global" settings - backgrounds, font colors, etc. Note that some stuff is set from code-behind, so this provides
		//a universal means to get data in both contexts. For example, a Flow Document loaded from RTF will ignore all of it's contained classes flags (except for align, for some reason)
		//so we have to set these through code-behind, notably font color. However, for the stuff that DOES respect font color (like sidebar), we also need to be able to bind it.
		//so i'm just storing them here. Note that it's entirely possible to do these in the class that defines them, but that always seems to work sporadically, so i'm just putting it here.


		private static readonly string[] backgrounds = { Path.Combine("resources", "background1.png"), Path.Combine("resources", "background2.png"), Path.Combine("resources", "background3.png"), Path.Combine("resources", "background4.png"), null, Path.Combine("resources", "backgroundKaizo.png") };

		private static readonly string[] sidebars = { Path.Combine("resources", "sidebar1.png"), Path.Combine("resources", "sidebar2.png"), Path.Combine("resources", "sidebar3.png"), Path.Combine("resources", "sidebar4.png"), null, Path.Combine("resources", "sidebarKaizo.png") };

		private static readonly SolidColorBrush[] textBgs = { newSolidColorWithTransparency(Colors.White, 0.4), new SolidColorBrush(Colors.White), new SolidColorBrush(Color.FromRgb(0xEB, 0xD5, 0xA6)), new SolidColorBrush(Colors.Transparent) };

		private static readonly FontFamily sidebarLegacy = new FontFamily("Lucida Sans Typewriter");
		private static readonly FontFamily sidebarModern = new FontFamily("Palatino Linotype");

		private static SolidColorBrush newSolidColorWithTransparency(Color color, double opacity)
		{
			return new SolidColorBrush(color)
			{
				Opacity = opacity
			};
		}

		public Controller controller => Controller.instance;

		private readonly MainMenuModelView mainMenu;
		private readonly StandardModelView standard;
		private readonly CombatModelView combat;
		private readonly DataModelView data;


		public ModelViewBase ModelView
		{
			get => _modelView;
			private set
			{
				if (_modelView != value) //the only time this will proc is if the 
				{
					_modelView = value;
					NotifyPropertyChanged();
				}
			}
		}
		private ModelViewBase _modelView;

		public string BackgroundImage
		{
			get => _BackgroundImage;
			private set
			{
				if (_BackgroundImage != value)
				{
					_BackgroundImage = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string _BackgroundImage = backgrounds[0];

		public string SidebarBackgroundImage
		{
			get => _SidebarBackgroundImage;
			private set
			{
				if (_SidebarBackgroundImage != value)
				{
					_SidebarBackgroundImage = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string _SidebarBackgroundImage = sidebars[0];

		public FontFamily SidebarFontFamily
		{
			get => _SidebarFontFamily;
			private set
			{
				if (_SidebarFontFamily != value)
				{
					_SidebarFontFamily = value;
					NotifyPropertyChanged();
				}
			}
		}
		private FontFamily _SidebarFontFamily = sidebarModern;

		public SolidColorBrush TextBackground
		{
			get => _TextBackground;
			private set
			{
				if (_TextBackground != value)
				{
					_TextBackground = value;
					NotifyPropertyChanged();
				}
			}
		}
		private SolidColorBrush _TextBackground = textBgs[0];

		public FontFamily TextFontFamily
		{
			get => _textFontFamily;
			private set
			{
				if (_textFontFamily != value)
				{
					_textFontFamily = value;
					NotifyPropertyChanged();
				}
			}
		}
		private FontFamily _textFontFamily = new FontFamily("Times New Roman");

		public SolidColorBrush FontColor
		{
			get => _FontColor;
			private set
			{
				if (_FontColor != value) //the only time this will proc is if the 
				{
					_FontColor = value;
					NotifyPropertyChanged();
				}
			}
		}
		private SolidColorBrush _FontColor = new SolidColorBrush(Colors.Black); //start out with black.

		public SolidColorBrush ButtonDisableHoverTextColor
		{
			get => _ButtonDisableHoverTextColor;
			private set
			{
				if (_ButtonDisableHoverTextColor != value) //the only time this will proc is if the 
				{
					_ButtonDisableHoverTextColor = value;
					NotifyPropertyChanged();
				}
			}
		}
		private SolidColorBrush _ButtonDisableHoverTextColor = new SolidColorBrush(Colors.Transparent);

		public double FontSize
		{
			get => _fontSize;
			private set
			{
				if (_fontSize != value)
				{
					_fontSize = value;
					NotifyPropertyChanged();
				}
			}
		}
		private double _fontSize = 15;

		public int FontEmSize => (int)Math.Round(FontSize * 2);


		public bool IsDarkMode => BackgroundImage == backgrounds[4] || BackgroundImage == backgrounds[3];

		public event PropertyChangedEventHandler PropertyChanged;

		public ModelViewRunner()
		{
			mainMenu = new MainMenuModelView(this);
			standard = new StandardModelView(this);
			combat = new CombatModelView(this);
			data = new DataModelView(this);

			_modelView = mainMenu;
		}

		internal void SwitchToMainMenu(Action lastAction)
		{
			if (lastAction is null) throw new ArgumentNullException(nameof(lastAction));

			if (ModelView == mainMenu)
			{
				return;
			}

			ModelViewBase currentBase = ModelView; //captured so it doesn't automatically update and mess things up.
			void action()
			{
				ModelView = currentBase;
				lastAction();
			}

			ModelView = mainMenu;
			ModelView.OnSwitch(action);
		}

		//note, resumeCallback can be null. If null, it's a New Game. else its a continue
		internal void SwitchToStandard(Action resumeCallback)
		{
			if (ModelView == standard)
			{
				return;
			}
			else if (resumeCallback != null)
			{
				ModelView = standard;
				resumeCallback();
			}
			else
			{
				ModelViewBase current = ModelView;

				ModelView = standard;
				ModelView.OnSwitch(resumeCallback);
			}
		}

		internal void SwitchToDataPage(Action onNonLoadCallback)
		{
			if (onNonLoadCallback is null) throw new ArgumentNullException(nameof(onNonLoadCallback));

			ModelViewBase current = ModelView;
			void onFail()
			{
				ModelView = current;
				onNonLoadCallback();
			}

			ModelView = data;
			ModelView.OnSwitch(onFail);
		}

		//note: controller never sees us hit this. so we can just jump to standard view, clear its data, and tell it to load data options, ignore other stuff.
		//on a method reload, we just rerun the parse controller. does not work with other menu items though. 
		//internal void SwitchToDataPage(Action resumeActionIfNotLoading)
		//{
		//	ModelViewBase currentBase = ModelView; //captured so it doesn't automatically update and mess things up.

		//	void action()
		//	{
		//		ModelView = currentBase;
		//		resumeActionIfNotLoading();
		//	}

		//	ModelView = standard;
		//	Action resumeAction = resumeActionIfNotLoading != null ? action : resumeActionIfNotLoading;
		//	standard.ExecuteLoadDataDisplay();
		//}

		internal void TestViewSwitch()
		{
			ModelView = standard;
			ModelView.ParseData();
		}


		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public void SetSidebarText(bool legacy)
		{
			SidebarFontFamily = legacy ? sidebarLegacy : sidebarModern;
		}

		public void SetBackground(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= backgrounds.Length)
			{
				index = backgrounds.Length - 1;
			}
			bool wasDarkMode = IsDarkMode;

			BackgroundImage = backgrounds[index];
			SidebarBackgroundImage = sidebars[index];
			if (wasDarkMode != IsDarkMode)
			{
				NotifyPropertyChanged(nameof(IsDarkMode));
			}
			SetFontColor();
		}

		public void SetTextBackground(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= textBgs.Length)
			{
				index = textBgs.Length - 1;
			}

			TextBackground = textBgs[index];
			SetFontColor();
		}

		private void SetFontColor()
		{
			if (IsDarkMode)
			{
				if (TextBackground == textBgs[2])
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
