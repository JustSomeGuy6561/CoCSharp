using CoC.Backend.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace CoCWinDesktop.CustomControls
{
	/// <summary>
	/// Interaction logic for StatBar.xaml. Stat Bar is a WPF recreation of the original StatBar, but with slight variations to make the code less spaghetti (also i'm lazy)
	/// You can bind the min, max, and value properties to their corresponding values and it'll automatically update, and display the correct arrow. You will need to call
	/// the clear arrow function to clear them, which should probably be done on a clear output or something of the like.
	/// </summary>

	//Animations do not exist, and we need to support it for feature parity. it's possible with animations, but idk how to fill them - is it just x number of milliseconds 
	//per cent? so if hp goes up 30%, its duration is 30*time per cent? Because that's doable. 

	//we're doing a two-way bind on the visibility property for arrow. this will allow us to force it invis

#warning I think this property/dependency property bullshit is bullshit, but should be able to fix properties to not need all the OnChanged events if i just use GetValue and SetValue for dependancy properties.
	//would also allow the shit to be one time, like it's supposed to be. Would probably also allow us to fix our code so we can just run the bullshit in constructors instead of commenting it out like i have.


	public partial class StatBar : UserControl
	{
		//these let you bind instead of just doing code-behind magic. Bind these and the whole code will just work (TM). Of course, the source you bind to needs to
		//implement notify property changed or it won't do shit. 
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(StatBar),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetCurrentValue(TextProperty, value);
		}

		public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register("IsNumeric", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));



		public bool IsNumeric
		{
			get => (bool)GetValue(IsNumericProperty);
			set => SetCurrentValue(IsNumericProperty, value);
		}

		public static readonly DependencyProperty HasMinMaxProperty = DependencyProperty.Register("HasMinMax", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
		public bool HasMinMax
		{
			get => (bool)GetValue(HasMinMaxProperty);
			set => SetCurrentValue(HasMinMaxProperty, value);
		}

		public static readonly DependencyProperty RegularColorProperty = DependencyProperty.Register("RegularColor", typeof(SolidColorBrush), typeof(StatBar),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0xA0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

		public SolidColorBrush RegularColor
		{
			get => (SolidColorBrush)GetValue(RegularColorProperty);
			set => SetCurrentValue(RegularColorProperty, value);
		}

		public static readonly DependencyProperty MinColorProperty = DependencyProperty.Register("MinColor", typeof(SolidColorBrush), typeof(StatBar),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0xD0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

		public SolidColorBrush MinColor
		{
			get => (SolidColorBrush)GetValue(MinColorProperty);
			set => SetCurrentValue(MinColorProperty, value);
		}

		public static readonly DependencyProperty ShowValueOverMaxProperty = DependencyProperty.Register("ShowValueOverMax", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

		public bool ShowValueOverMax
		{
			get => (bool)GetValue(ShowValueOverMaxProperty);
			set => SetCurrentValue(ShowValueOverMaxProperty, value);
		}

		//#66FF8080 lust
		//#66600000 rest

		public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register("MinimumValue", typeof(uint), typeof(StatBar),
			new FrameworkPropertyMetadata(uint.MinValue, FrameworkPropertyMetadataOptions.AffectsRender, OnMinumumValueChanged));
		public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register("MaximumValue", typeof(uint), typeof(StatBar),
			new FrameworkPropertyMetadata((uint)100, FrameworkPropertyMetadataOptions.AffectsRender, OnMaximumValueChanged));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(StatBar),
			new FrameworkPropertyMetadata(uint.MinValue.ToString(), FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged));
		//update the current bar. called if value changes or max changes. 

		private double barWidth => BarHolder.Width - 2;
		private static void OnMinumumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;

			if (statusBar.HasMinMax)
			{
				statusBar.MinimumValue = (uint)e.NewValue;

				double width = statusBar.barWidth * statusBar.MinimumValue / statusBar.MaximumValue;
				if (width < 0) width = 0;
				statusBar.MinBar.Width = width;
			}
		}
		public uint MinimumValue
		{
			get => (uint)GetValue(MinimumValueProperty);
			set => SetCurrentValue(MinimumValueProperty, value);
		}

		private static void OnMaximumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;
			
			if (statusBar.HasMinMax)
			{
				double width;
				//if (statusBar.numericValue <= statusBar.MaximumValue)
				//{
				//	width = statusBar.barWidth * statusBar.numericValue / statusBar.MaximumValue;
				//	if (width < 0) width = 0;
				//	statusBar.FillBar.Width = width;
				//}
				//else
				//{
				//	statusBar.Value = e.NewValue.ToString();
				//}

				width = statusBar.barWidth * statusBar.MinimumValue / statusBar.MaximumValue;
				width = Utils.Clamp2(width, 0, statusBar.barWidth);
				statusBar.MinBar.Width = width;
			}
		}
		public uint MaximumValue
		{
			get => (uint)GetValue(MaximumValueProperty);
			set => SetCurrentValue(MaximumValueProperty, value);
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;

			if ((string)e.OldValue != (string)e.NewValue && statusBar.IsNumeric)
			{
				uint old = uint.Parse((string)e.OldValue);
				uint @new = uint.Parse((string)e.NewValue);

				if (old > @new)
				{
					statusBar.Arrow.Source = ArrowDown;
				}
				else if (old < @new)
				{
					statusBar.Arrow.Source = ArrowUp;
				}
				statusBar.ArrowVisible = Visibility.Visible;
			}


			if (statusBar.HasMinMax)
			{
				double width = statusBar.barWidth * statusBar.numericValue / statusBar.MaximumValue;
				if (width < 0) width = 0;
				statusBar.FillBar.Width = width;
				statusBar.FillBarShadowTop.Width = width;
				statusBar.FillBarShadowFull.Width = width + 4;
			}
		}
		public string Value
		{
			get => (string)GetValue(ValueProperty);
			set => SetCurrentValue(ValueProperty, value);
		}
		private uint numericValue => HasMinMax ? uint.Parse(Value) : 0;


		private static BitmapImage ArrowUp = new BitmapImage(new Uri(@"pack://application:,,,/resources/arrow-up.png"));
		private static BitmapImage ArrowDown = new BitmapImage(new Uri(@"pack://application:,,,/resources/arrow-down.png"));


		public static readonly DependencyProperty ArrowVisibleProperty = DependencyProperty.Register("ArrowVisible", typeof(Visibility), typeof(StatBar),
			new FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Visibility ArrowVisible
		{
			get => (Visibility)GetValue(ArrowVisibleProperty);
			set => SetCurrentValue(ArrowVisibleProperty, value);
		}

		public StatBar()
		{
			InitializeComponent();
		}

	}
}
