using System.Windows;
using System.Windows.Input;

namespace CoCWinDesktop
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
