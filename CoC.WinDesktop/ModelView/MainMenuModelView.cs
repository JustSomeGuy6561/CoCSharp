using CoC.Backend;
using CoC.Backend.Engine;
using CoCWinDesktop.CustomControls;
using CoCWinDesktop.CustomControls.OptionsModelViews;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CoCWinDesktop.ModelView
{
	public class MainMenuModelView : ModelViewBase
	{
		private int LastLanguageIndex;

		private SafeAction resumeCallback => runner.resumeGameAction;

		public ICommand CanContinue { get; }

		public ICommand OnNewGame { get; }
		public ICommand OptionsCommand { get; }

		public ICommand OnData { get; }

		public ICommand OnInstructions { get; }
		public ICommand OnCredits { get; }
		public ICommand OnAchievements { get; }

		public MainMenuModelView(ModelViewRunner runner) : base(runner)
		{
			runner.PropertyChanged += Runner_PropertyChanged;
			CanContinue = new RelayCommand(resumeCallbackWrapper, () => resumeCallback != null);
			OnNewGame = new RelayCommand(newGameCallbackWrapper, returnTrue);

			OnData = new RelayCommand(dataCallbackWrapper, returnTrue);
			OnInstructions = new RelayCommand(handleInstructions, returnTrue);
			OnCredits = new RelayCommand(handleCredits, returnTrue);
			OnAchievements = new RelayCommand(handleAchievements, returnTrue);
			OptionsCommand = new RelayCommand(optionsCallbackWrapper, returnTrue);

			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
		}

		private void Runner_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(runner.resumeGameAction))
			{
				((RelayCommand)CanContinue).RaiseExecuteChanged();
			}
		}

		private void optionsCallbackWrapper()
		{
			runner.SwitchToOptions();
		}

		private void dataCallbackWrapper()
		{
			runner.SwitchToData();
		}

		private void newGameCallbackWrapper()
		{
			runner.SwitchToStandard(true);
		}

		private void handleAchievements()
		{
			runner.SwitchToAchievements();
		}

		private void handleInstructions()
		{
			runner.SwitchToInstructions();
		}

		private void handleCredits()
		{
			runner.SwitchToCredits();
		}

		private static bool returnTrue()
		{
			return true;
		}

		private void resumeCallbackWrapper()
		{
			runner.SwitchToStandard(false);
		}

		private void OnViewSwitch()
		{

			UpdateContent();
		}

		protected override void ParseDataForDisplay()
		{
			if (LanguageEngine.currentLanguageIndex != LastLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;
				UpdateContent();
			}
		}

		private void UpdateContent()
		{

		}
	}
}
