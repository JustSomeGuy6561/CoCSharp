using CoC.Backend;
using CoC.Backend.Engine;
using CoCWinDesktop.ModelView;
using System;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class CustomizeControlsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;
		public string CustomControlsText
		{
			get => _CustomControlsText;
			private set => CheckPropertyChanged(ref _CustomControlsText, value);
		}
		private string _CustomControlsText;
		protected override SimpleDescriptor TitleText => CustomControlsTitleText;

		public string CustomControlsHelper
		{
			get => _CustomControlsHelper;
			private set => CheckPropertyChanged(ref _CustomControlsHelper, value);
		}
		private string _CustomControlsHelper;
		protected override SimpleDescriptor TitleHelperText => CustomControlsHelperText;

		public override SimpleDescriptor ButtonText => CustomControlsButtonText;

		public CustomizeControlsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			lastLanguageIndex = LanguageEngine.currentLanguageIndex;

			_CustomControlsText = TitleText();
			_CustomControlsHelper = TitleHelperText();
		}

		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;

				CustomControlsText = TitleText();
				CustomControlsHelper = TitleHelperText();
			}
		}

		public override Action OnConfirmation => ConfirmControls;



		private void ConfirmControls()
		{
			Console.WriteLine("Controls written? NYI, but code works!");
		}
	}
}
