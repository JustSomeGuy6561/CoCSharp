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

		private void ModThreadBtn_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://forum.fenoxo.com/threads/coc-revamp-mod.3/");
		}
	}
}
