using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Language;
using CoCWinDesktop.ContentWrappers;
using CoCWinDesktop.CustomControls.SideBarModelViews;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed partial class LanguageOptionsModelView : OptionModelViewDataBase
	{
		public StandardSideBarModelView sidebarView { get; }

		public string MainContent
		{
			get => _rTFContent;
			internal set => CheckPropertyChanged(ref _rTFContent, value);
		}
		private string _rTFContent;

		private string mainContentStore;
		private List<Color> mainContentColors;

		public string PostContent
		{
			get => _postContent;
			internal set => CheckPropertyChanged(ref _postContent, value);
		}
		private string _postContent;

		private string postContentStore;
		private List<Color> postContentColors;

		//public static IEnumerable<string> AvailableLanguages => LanguageEngine.availableLanguages.Select(x => x.LanguageName());

		public ComboBoxWrapper AvailableLanguages { get; }

		protected override SimpleDescriptor TitleText => LanguageEngine.currentLanguage.Language;
		private string ContentString => LanguageEngine.currentLanguage.GenericFlavorTextExample();
		protected override SimpleDescriptor TitleHelperText => LanguageEngine.currentLanguage.LanguageInstructionText;

		public override SimpleDescriptor ButtonText => LanguageButtonText;

		private void GetRTFText()
		{
			mainContentStore = $@"\b\ul\fs{ModelViewRunner.HeaderSizeEm} {TitleText()}\b0\ul0\par\fs{ModelViewRunner.SmallHeaderEm}\par " + 
				RTFParser.FromHTMLNoHeader(new StringBuilder(TitleHelperText()), runner.FontColor.Color, out mainContentColors);

			postContentStore = RTFParser.FromHTMLNoHeader(new StringBuilder(ContentString), runner.FontColor.Color, out postContentColors);
			
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			if (mainContentColors.Count > 0)
			{
				mainContentColors[0] = runner.FontColor.Color;
			}
			MainContent = RTFParser.FromRTFText(mainContentStore, mainContentColors, runner);

			if (postContentColors.Count > 0)
			{
				mainContentColors[0] = runner.FontColor.Color;
			}
			PostContent = RTFParser.FromRTFText(postContentStore, postContentColors, runner);
		}

		public LanguageOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			sidebarView = new StandardSideBarModelView(new SaveDataCollection(null), true);

			List<ComboBoxItemWrapper> items = new List<ComboBoxItemWrapper>();
			for (int x = 0; x < LanguageEngine.availableLanguages.Count;x++)
			{
				var language = LanguageEngine.availableLanguages[x];
				if (language != null)
				{
					int y = x;
					items.Add(new ComboBoxItemWrapper(() => HandleSelected(y), language.LanguageName()));
				}
			};
			AvailableLanguages = new ComboBoxWrapper(items, LanguageEngine.currentLanguageIndex);
		}

		private void HandleSelected(int languageIndex)
		{
			LanguageEngine.currentLanguageIndex = languageIndex;
			//update sidebar, rtf text.
			sidebarView.UpdateSidebar(new SaveDataCollection(null));
			GetRTFText();
			//let parent update the buttons. 
			parent.OnLanguageChange();
		}



		public override void ParseDataForDisplay()
		{
			if (string.IsNullOrEmpty(MainContent))
			{
				GetRTFText();
			}
			else
			{
				UpdateDisplay();
			}

		}
	}
}
