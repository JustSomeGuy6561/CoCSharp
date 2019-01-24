using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoCWinDesktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.ToString());
		}

		private void ContinueBtn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DataBtn_Click(object sender, RoutedEventArgs e)
		{
			Settings.SetBackgroundImage((Settings.currBackground+1) % 6);
		}

		private void ModThreadBtn_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://forum.fenoxo.com/threads/coc-revamp-mod.3/");
		}
	}
}
