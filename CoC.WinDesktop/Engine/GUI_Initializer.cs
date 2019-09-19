using CoC.Backend.Engine;
using CoC.Frontend.Engine;
using CoC.WinDesktop.DisplaySettings;
using CoC.WinDesktop.GameCredits;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.InterfaceSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoC.WinDesktop.Engine
{
    public static class GUI_Initializer
    {
		public const string GLOBAL_SETTINGS_PATH = @"settings.sav";

		/// <summary>
		/// Initialize any managers specific to the GUI layer that GUI save data may depend on in order to run correctly. For example, the language engine must exist, otherwise
		/// the game cannot properly deserialize the previously selected language. 
		/// </summary>
		public static void PreSaveInit()
		{
			FrontendInitalizer.PreSaveInit();

			CreditManager.AddCreditCategory(new GUICredits());
		}

		/// <summary>
		/// Some members may rely on other members to exist. Instead of worrying about initialization order, which can quickly become tedious, you may place all systems that are
		/// independant of one another in the presave, then initialize the dependant ones in the late pre save. 
		/// </summary>
		public static void LatePreSaveInit()
		{
			FrontendInitalizer.LatePreSaveInit();
		}

		/// <summary>
		/// Handle the initial loading of global save information, if such a file exists. 
		/// </summary>
		public static void InitializeSaveData()
		{
			FileInfo globalDataFile;

			//We're in windows, don't overcomplicate shit. 
			//if (File.Exists(GLOBAL_SETTINGS_PATH))
			//{
			//	globalDataFile = new FileInfo(GLOBAL_SETTINGS_PATH);
			//}
			//else
			//{
				globalDataFile = null;
				//File.Create(GLOBAL_SETTINGS_PATH);
			//}

			FrontendInitalizer.InitializeSaveData(globalDataFile);

			if (globalDataFile is null)
			{
				CoC.Backend.SaveData.SaveSystem.AddGlobalSave(new GuiGlobalSave());
				CoC.Backend.SaveData.SaveSystem.AddSessionSave<GuiSessionSave>();
			}
			else throw new CoC.Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public static void PostSaveInit()
		{
			FrontendInitalizer.PostSaveInit();

			InterfaceOptionManager.IncludeOption(new SidebarFontOption());
			InterfaceOptionManager.IncludeOption(new SpriteStatusOption());
			InterfaceOptionManager.IncludeOption(new ImagePackOption());
			InterfaceOptionManager.IncludeOption(new SidebarAnimationOption());
			InterfaceOptionManager.IncludeOption(new EnemySidebarOption());

			DisplayOptionManager.IncludeOption(new FontSizeOption());
			DisplayOptionManager.IncludeOption(new TextBackgroundOption());
			DisplayOptionManager.IncludeOption(new BackgroundOption());

			foreach (var data in InterfaceOptionManager.interfaceOptions)
			{
				data.PostLocalSessionInit();
			}

			foreach (var data in DisplayOptionManager.displayOptions)
			{
				data.PostLocalSessionInit();
			}

			ModelViewRunner runner = Application.Current.Resources["Runner"] as ModelViewRunner;
		}

		/// <summary>
		/// The final step, a sort of catch-all for anything missed previously. Things such as data validation can be run here to finalize the initialization and correct anything
		/// that does not make sense, like bad save data (likely via save editing, for example). 
		/// </summary>
		public static void FinalizeInitialization()
		{
			FrontendInitalizer.FinalizeInitialization();
		}
    }
}
