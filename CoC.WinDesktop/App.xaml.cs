using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoC.Frontend.Engine;
using CoC.UI;
using CoC.WinDesktop.Engine;
using CoC.WinDesktop.ModelView;

namespace CoC.WinDesktop
{
	//select text for textbox taken from https://www.intertech.com/Blog/how-to-select-all-text-in-a-wpf-textbox-on-focus/
	//and modified so it doesn't select text in readonly or non-focusable items (that'd be weird) Afaik, it's not necessary for
	//non-focusable items (they never should get the focus). But what do i know?

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		void App_Startup(object sender, StartupEventArgs e)
		{
			// Application is running
			// Process command line args

			//Initialize Classes and related info.
			GUI_Initializer.PreSaveInit();
			GUI_Initializer.LatePreSaveInit();
			GUI_Initializer.InitializeSaveData();
			GUI_Initializer.PostSaveInit();
			GUI_Initializer.FinalizeInitialization();
			//handle special gui related events and things.
			EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
				new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
			EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
			  new RoutedEventHandler(SelectAllText), true);


			MainWindow window = new MainWindow();
			window.Show();
		}

		private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
		{
			if (sender is TextBox textbox && !textbox.IsKeyboardFocusWithin && !textbox.IsReadOnly && textbox.Focusable)
			{
				if (e.OriginalSource.GetType().Name == "TextBoxView")
				{
					e.Handled = true;
					textbox.Focus();
				}
			}
		}

		private static void SelectAllText(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is TextBox textBox && !textBox.IsReadOnly && textBox.Focusable)
			{
				textBox.SelectAll();
			}
		}
	}

	/*    protected override void OnStartup(StartupEventArgs e)
    {


        base.OnStartup(e);
    }


}*/
}
