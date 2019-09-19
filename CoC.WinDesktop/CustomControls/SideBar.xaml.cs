using CoC.Frontend.UI;
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

namespace CoC.WinDesktop.CustomControls
{
	/// <summary>
	/// Interaction logic for SideBar.xaml
	/// </summary>
	public partial class SideBar : UserControl
	{
		public static readonly DependencyProperty SideBarObjectProperty = DependencyProperty.Register("SideBarObject", typeof(SideBarBase), typeof(SideBar),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		public SideBarBase SideBarObject
		{
			get => (SideBarBase)GetValue(SideBarObjectProperty);
			set => SetCurrentValue(SideBarObjectProperty, value);
		}

		public SideBar()
		{
			InitializeComponent();
		}
	}
}
