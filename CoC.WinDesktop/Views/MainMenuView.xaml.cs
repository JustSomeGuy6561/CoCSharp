using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoCWinDesktop.Views
{
	/// <summary>
	/// Interaction logic for MainMenuView.xaml
	/// </summary>
	public partial class MainMenuView : UserControl
	{
		ModelViewRunner runner;
		public MainMenuView()
		{
			InitializeComponent();
			runner = Application.Current.Resources["Runner"] as ModelViewRunner;
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.ToString());
		}

		private int indexMaker
		{
			get => _indexMaker;
			set => _indexMaker = value % 6;
		}
		private int _indexMaker = 0;
		private void DataBtn_Click(object sender, RoutedEventArgs e)
		{
			indexMaker++;
			runner.SetBackground(indexMaker);
		}

		private void ModThreadBtn_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://forum.fenoxo.com/threads/coc-revamp-mod.3/");
		}

		private void NewGameBtn_Click(object sender, RoutedEventArgs e)
		{
			//CoC.Frontend.Engine.NewGameHelpers.NewGame();

			//ParseModelView();
		}

		private void CreditsBtn_Click(object sender, RoutedEventArgs e)
		{
			//runner.FontColor = new SolidColorBrush(Colors.White);
		}

		private void InstructionsBtn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AchievementsBtn_Click(object sender, RoutedEventArgs e)
		{
			//stored in the global data for frontend
		}
	}
}
