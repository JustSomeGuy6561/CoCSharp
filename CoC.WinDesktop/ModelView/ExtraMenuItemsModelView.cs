using CoC.WinDesktop.ContentWrappers.ButtonWrappers;
using CoC.WinDesktop.CustomControls;
using CoC.WinDesktop.CustomControls.ExtraItemModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoC.WinDesktop.ModelView
{
	public sealed class ExtraMenuItemsModelView : ModelViewBase
	{
		public AutomaticButtonWrapper OkButton { get; }

		//public Extra
		public ExtraItemModelViewBase subModel
		{
			get => _subModel;
			private set
			{
				CheckPropertyChanged(ref _subModel, value);
			}
		}
		private ExtraItemModelViewBase _subModel;

		private readonly MenuAchievementsModelView achievements;
		private readonly MenuCreditsModelView credits;
		private readonly MenuInstructionsModelView instructions;

		public ExtraMenuItemsModelView(ModelViewRunner modelViewRunner) : base(modelViewRunner)
		{
			credits = new MenuCreditsModelView(modelViewRunner, this);
			instructions = new MenuInstructionsModelView(modelViewRunner, this);
			achievements = new MenuAchievementsModelView(modelViewRunner, this);

			OkButton = new AutomaticButtonWrapper(CoC.Backend.Strings.GlobalStrings.OK, handleReturn, unlockedLockedTipCallback:null, null, defaultButton:true);

			_subModel = achievements;
		}


		private void handleReturn()
		{
			runner.SwitchToMainMenu();
		}

		public void SetState_Credits()
		{
			subModel = credits;
		}

		public void SetState_Instructions()
		{
			subModel = instructions;
		}

		public void SetState_Achievements()
		{
			subModel = achievements;
		}

		protected override void ParseDataForDisplay()
		{
			subModel.ParseDataForDisplay();
		}


	}
}
