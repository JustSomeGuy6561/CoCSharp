using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoC.WinDesktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
#if DEBUG
			System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Warning;
			ToolTipService.ShowDurationProperty.OverrideMetadata(
				typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
			System.Windows.Controls.ToolTip.FocusableProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(true));
#endif
			InitializeComponent();

			this.Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			//Initialize Classes and related info.
			Keyboard.ClearFocus();
			Application.Current.MainWindow.Focus();
			var element = Keyboard.Focus(Application.Current.MainWindow);

			Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
		}
	}
}
