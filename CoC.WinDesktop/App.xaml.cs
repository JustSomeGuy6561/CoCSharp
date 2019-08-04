using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using CoC.Frontend.Engine;
using CoC.UI;
using CoCWinDesktop.ModelView;

namespace CoCWinDesktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public const string GLOBAL_SETTINGS_PATH = @"settings.sav";

		public 

		void App_Startup(object sender, StartupEventArgs e)
		{
			// Application is running
			// Process command line args

			//Initialize Classes and related info.
			FileInfo globalDataFile;

			//We're in windows, don't overcomplicate shit. 
			if (File.Exists(GLOBAL_SETTINGS_PATH))
			{
				globalDataFile = new FileInfo(GLOBAL_SETTINGS_PATH);
			}
			else
			{
				globalDataFile = null;
				//File.Create(GLOBAL_SETTINGS_PATH);
			}

			CoC.Backend.SaveData.SaveSystem.AddGlobalSave(new GuiGlobalSave());
			CoC.Backend.SaveData.SaveSystem.AddSessionSave(new GuiSessionSave());

			FrontendInitalizer.Init(globalDataFile);

			MainWindow window = new MainWindow() { /*DataContext = new ModelViewRunner()*/ };
			window.Show();
			//Controller.instance.TestShit();
			//ModelViewRunner runner = Current.Resources["Runner"] as ModelViewRunner;
			//runner.ParseData();
		}
	}
}
