using CoC.UI;
using CoC.WinDesktop.Helpers;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CoC.WinDesktop.ModelView
{
	public abstract class ModelViewBase : NotifierBase
	{
		//gather all the data related to this display, from the controller and/or ui as needed. 
		protected abstract void ParseDataForDisplay();

		internal void ParseData()
		{
			//Initialize Classes and related info.
			Keyboard.ClearFocus();
			Application.Current.MainWindow.Focus();
			var element = Keyboard.Focus(Application.Current.MainWindow);

			Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

			ParseDataForDisplay();
		}

		protected readonly ModelViewRunner runner;

		internal void SwitchFromThisView()
		{
			OnSwitchFrom();
		}

		/// <summary>
		/// Switches to this model view. If it fails to do so, it executes last action instead. Note that you may have to set values in the
		/// modelview before this is called in order for it to execute properly. It's not reasonably possible to do that here, though. 
		/// </summary>
		/// <param name="lastAction"></param>
		internal void SwitchToThisView()
		{
			OnSwitchTo();
			ParseData();
		}

		protected virtual void OnSwitchTo() { }
		protected virtual void OnSwitchFrom() { }

		public ModelViewBase(ModelViewRunner modelViewRunner)
		{
			runner = modelViewRunner ?? throw new ArgumentNullException(nameof(modelViewRunner));
		}

	}
}
