using CoCWinDesktop.CustomControls;
using CoCWinDesktop.CustomControls.OptionsModelViews;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView
{
	public class MainMenuModelView : ModelViewBase
	{

		private SafeAction resumeCallback
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
		private SafeAction _resumeCallback = null;

		public ICommand CanContinue { get; }

		public ICommand OnNewGame { get; }
		public ICommand OptionsCommand { get; }

		public MainMenuModelView(ModelViewRunner runner) : base(runner)
		{
			CanContinue = new RelayCommand(resumeCallbackWrapper, () => resumeCallback != null);
			OnNewGame = new RelayCommand(newGameCallbackWrapper, returnTrue);

			OptionsCommand = new RelayCommand(optionsCallbackWrapper, returnTrue);

		}

		private void optionsCallbackWrapper()
		{
			runner.SwitchToOptions();
		}

		private void newGameCallbackWrapper()
		{
			runner.SwitchToStandard();
		}

		private static bool returnTrue()
		{
			return true;
		}

		private void resumeCallbackWrapper()
		{
			runner.SwitchToStandard();
		}

		protected override void ParseDataForDisplay()
		{
		}
	}
}
