﻿using CoC.WinDesktop.Helpers;
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
	/// Interaction logic for OptionRow.xaml
	/// </summary>


	public partial class OptionRowSlider : UserControl
	{
		//public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(OptionRowDisplayMode), typeof(OptionRowSlider),
		//	new FrameworkPropertyMetadata(OptionRowDisplayMode.GAME, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));




		public OptionRowSlider()
		{
			InitializeComponent();
		}
	}
}
