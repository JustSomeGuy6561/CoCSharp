using CoC.Backend;
using CoC.Backend.Engine;
using CoC.WinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class SaveOptionsModelView : OptionModelViewDataBase
	{
		private int lastLanguageIndex;

		public string SaveOptionsTitle
		{
			get => _SaveOptionsTitle;
			private set => CheckPropertyChanged(ref _SaveOptionsTitle, value);
		}
		private string _SaveOptionsTitle;
		protected override SimpleDescriptor TitleText => SaveOptionText;

		public string SaveOptionsHelper
		{
			get => _SaveOptionsHelper;
			private set => CheckPropertyChanged(ref _SaveOptionsHelper, value);
		}
		private string _SaveOptionsHelper;
		protected override SimpleDescriptor TitleHelperText => SaveHelperText;

		public override SimpleDescriptor ButtonText => SaveButtonText;

		public SaveOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;

			_SaveOptionsTitle = TitleText();
			_SaveOptionsHelper = TitleHelperText();
		}



		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;
				SaveOptionsTitle = TitleText();
				SaveOptionsHelper = TitleHelperText();
			}
		}
	}
}
