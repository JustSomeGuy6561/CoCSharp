using CoC.UI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView
{
	public abstract class ModelViewBase : INotifyPropertyChanged
	{
		public abstract event PropertyChangedEventHandler PropertyChanged;


		//gather all the data related to this display, from the controller and/or ui as needed. 
		protected abstract void ParseDataForDisplay();

		internal void ParseData()
		{
			//Initialize Classes and related info.
			Keyboard.ClearFocus();
			Application.Current.MainWindow.Focus();
			var element = Keyboard.Focus(Application.Current.MainWindow);

			//Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

			ParseDataForDisplay();
		}

		protected readonly ModelViewRunner runner;

		/// <summary>
		/// Switches to this model view. If it fails to do so, it executes last action instead. Note that you may have to set values in the
		/// modelview before this is called in order for it to execute properly. It's not reasonably possible to do that here, though. 
		/// </summary>
		/// <param name="lastAction"></param>
		internal void OnSwitch(Action lastAction)
		{
			if (!SwitchToThisModelView(lastAction))
			{
				lastAction();
			}
		}

		protected abstract bool SwitchToThisModelView(Action lastAction);

		public ModelViewBase(ModelViewRunner modelViewRunner)
		{
			runner = modelViewRunner ?? throw new ArgumentNullException(nameof(modelViewRunner));
		}
	}
}
