using CoC.Backend.Engine;
using CoC.WinDesktop.ContentWrappers.ButtonWrappers;
using CoC.WinDesktop.Helpers;
using System;
using System.Windows.Input;

namespace CoC.WinDesktop.ModelView
{
	public sealed partial class MainMenuModelView : ModelViewBase
	{
		private const string MOD_THREAD_URL = "https://forum.fenoxo.com/threads/coc-revamp-mod.3/";

		private int LastLanguageIndex;

		private SafeAction resumeCallback => runner.resumeGameAction;

		public AutomaticButtonWrapper ContinueButton { get; }
		public AutomaticButtonWrapper NewGameButton { get; }
		public AutomaticButtonWrapper DataButton { get; }
		public AutomaticButtonWrapper OptionsButton { get; }
		public AutomaticButtonWrapper AchievementsButton { get; }
		public AutomaticButtonWrapper InstructionsButton { get; }
		public AutomaticButtonWrapper CreditsButton { get; }
		public AutomaticButtonWrapper ModThreadButton { get; }


		public MainMenuModelView(ModelViewRunner runner) : base(runner)
		{
			runner.PropertyChanged += Runner_PropertyChanged;

			ContinueButton = new AutomaticButtonWrapper(ContinueBtnText, resumeCallbackWrapper, ContinueButtonTip, null, resumeCallback != null, false);

			NewGameButton = new AutomaticButtonWrapper(newGameBtnText, newGameCallbackWrapper, NewGameBtnTip, null);
			DataButton = new AutomaticButtonWrapper(DataButtonText, dataCallbackWrapper, DataButtonTip, null);
			OptionsButton = new AutomaticButtonWrapper(OptionsButtonText, optionsCallbackWrapper, OptionsButtonTip, null);
			AchievementsButton = new AutomaticButtonWrapper(AchievementsButtonText, handleAchievements, AchievementsButtonTip, null);
			InstructionsButton = new AutomaticButtonWrapper(InstructionsButtonText, handleInstructions, InstructionsButtonTip, null);
			CreditsButton = new AutomaticButtonWrapper(CreditsButtonText, handleCredits, CreditsButtonTip, null);
			ModThreadButton = new AutomaticButtonWrapper(ModThreadButtonText, handleModUrl, ModThreadButtonTip, null);

			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
		}

		private void Runner_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(runner.resumeGameAction))
			{
				ContinueButton.SetEnabled(resumeCallback != null);
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

		private void handleModUrl()
		{
			UrlHelper.OpenURL(MOD_THREAD_URL);
		}

		private static bool returnTrue()
		{
			return true;
		}

		private void resumeCallbackWrapper()
		{
			runner.SwitchToStandard(false);
		}

		protected override void ParseDataForDisplay()
		{
			if (LanguageEngine.currentLanguageIndex != LastLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;
				//handle content that needs it.
			}
		}
	}
}
