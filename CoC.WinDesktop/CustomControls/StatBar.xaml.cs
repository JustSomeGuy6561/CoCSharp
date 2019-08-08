using CoC.Backend.Tools;
using System.Windows;
using System.Windows.Controls;
using CoC;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

	public partial class StatBar : UserControl
	{
		//these let you bind instead of just doing code-behind magic. Bind these and the whole code will just work (TM). Of course, the source you bind to needs to
		//implement notify property changed or it won't do shit. 
		public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(StatBar),
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

		public string Text { get; set; }

		public static DependencyProperty IsNumericProperty = DependencyProperty.Register("IsNumeric", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

		public bool IsNumeric { get; set; } = true;

		public static DependencyProperty HasGaugeProperty = DependencyProperty.Register("HasGauge", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

		public bool HasGauge { get; set; } = true;

		public static DependencyProperty RegularColorProperty = DependencyProperty.Register("RegularColor", typeof(SolidColorBrush), typeof(StatBar),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0xA0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

		public SolidColorBrush RegularColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0xA0, 0, 0));

		public static DependencyProperty MinColorProperty = DependencyProperty.Register("MinColor", typeof(SolidColorBrush), typeof(StatBar),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0xD0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

		public SolidColorBrush MinColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0xD0, 0, 0));

		public static DependencyProperty ShowValueOverMaxProperty = DependencyProperty.Register("ShowValueOverMax", typeof(bool), typeof(StatBar),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

		public bool ShowValueOverMax { get; set; } = false;

		//#66FF8080 lust
		//#66600000 rest

		public static DependencyProperty MinimumProperty = DependencyProperty.Register("MinimumValue", typeof(uint), typeof(StatBar),
			new FrameworkPropertyMetadata(uint.MinValue, FrameworkPropertyMetadataOptions.AffectsRender, OnMinumumValueChanged));
		public static DependencyProperty MaximumProperty = DependencyProperty.Register("MaximumValue", typeof(uint), typeof(StatBar),
			new FrameworkPropertyMetadata((uint)100, FrameworkPropertyMetadataOptions.AffectsRender, OnMaximumValueChanged));
		public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(uint), typeof(StatBar),
			new FrameworkPropertyMetadata(uint.MinValue, FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged));
		//update the current bar. called if value changes or max changes. 
		
		private double barWidth => BarHolder.Width - 2;
		private static void OnMinumumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;
			statusBar.MinimumValue = (uint)e.NewValue;

			double width = statusBar.barWidth * statusBar.MinimumValue / statusBar.MaximumValue;
			if (width < 0) width = 0;
			statusBar.MinBar.Width = width;
		}
		public uint MinimumValue { get; set; } = 0;

		private static void OnMaximumValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;
			statusBar.MaximumValue = (uint)e.NewValue;

			double width;
			if (statusBar.IsNumeric)
			{
				if (statusBar.numericValue <= statusBar.MaximumValue)
				{
					width = statusBar.barWidth * statusBar.numericValue / statusBar.MaximumValue;
					if (width < 0) width = 0;
					statusBar.FillBar.Width = width;
				}
				else
				{
					OnValueChanged(d, new DependencyPropertyChangedEventArgs(ValueProperty, e.OldValue, e.NewValue));
				}
			}
			width = statusBar.barWidth * statusBar.MinimumValue / statusBar.MaximumValue;
			if (width < 0) width = 0;
			statusBar.MinBar.Width = width;
		}
		public uint MaximumValue { get; set; } = 100;

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StatBar statusBar = d as StatBar;

			if ((string)e.NewValue != statusBar.Value)
			{
				statusBar.Value = (string)e.NewValue;

				if ((uint)e.OldValue > (uint)e.NewValue)
				{
					statusBar.Arrow.Source = ArrowDown;
				}
				else if ((uint)e.OldValue < (uint)e.NewValue)
				{
					statusBar.Arrow.Source = ArrowUp;
				}

				d.SetCurrentValue(ArrowVisibleProperty, Visibility.Visible);
			}
			if (statusBar.IsNumeric)
			{
				double width = statusBar.barWidth * statusBar.numericValue / statusBar.MaximumValue;
				if (width < 0) width = 0;
				statusBar.FillBar.Width = width;
			}
		}
		private static BitmapImage ArrowUp = new BitmapImage(new Uri(@"pack://application:,,,/resources/arrow-up.png"));
		private static BitmapImage ArrowDown = new BitmapImage(new Uri(@"pack://application:,,,/resources/arrow-down.png"));


		public static DependencyProperty ArrowVisibleProperty = DependencyProperty.Register("ArrowVisible", typeof(Visibility), typeof(StatBar),
			new FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Visibility ArrowVisible
		{
			get => (Visibility)GetValue(ArrowVisibleProperty);
			set => PropertyHelper(ArrowVisibleProperty, value);
		}

		public string Value { get; set; }

		private uint numericValue => IsNumeric ? uint.Parse(Value) : 0;

		public StatBar()
		{
			InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void PropertyHelper(DependencyProperty d, object value, [CallerMemberName]string propertyName = "")
		{
			SetValue(d, value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
