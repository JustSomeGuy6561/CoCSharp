using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoC.UI;
using CoCWinDesktop.ModelView.Helpers;

namespace CoCWinDesktop.ModelView
{
	public class MainMenuModelView : ModelViewBase
	{

		public override event PropertyChangedEventHandler PropertyChanged;

		private Action resumeCallback
		{
			get => _resumeCallback;
			set
			{
				if (_resumeCallback != value)
				{
					if (value is null != _resumeCallback is null)
					{
						((RelayCommand)CanContinue).RaiseExecuteChanged();
					}
					_resumeCallback = value;
				}
			}
		}
		private Action _resumeCallback = null;

		public ICommand CanContinue { get; }

		public ICommand OnNewGame { get; }
		public ICommand OptionsCommand { get; }

		public MainMenuModelView(ModelViewRunner runner) : base(runner)
		{
			CanContinue = new RelayCommand(resumeCallbackWrapper, () => resumeCallback != null);
			OnNewGame = new RelayCommand(newGameCallbackWrapper, returnTrue);

			OptionsCommand = new RelayCommand(runner.TestViewSwitch, returnTrue);
		}

		private void newGameCallbackWrapper()
		{
			runner.SwitchToStandard(null);
		}

		private static bool returnTrue()
		{
			return true;
		}

		private void resumeCallbackWrapper()
		{
			runner.SwitchToStandard(resumeCallback);
		}

		//note: lastAction will never be null, 
		protected override bool SwitchToThisModelView(Action lastAction)
		{
			resumeCallback = lastAction;
			if (resumeCallback is null)
			{
				Debug.WriteLine("Switch to main model view has a null callback. This should never occur. The game will still run, but continue will not work.");
			}
			return true;
		}



		//honestly, we don't do anything. 
		protected override void ParseDataForDisplay()
		{
		}


		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
