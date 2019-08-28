using CoC.Backend.Engine;
using CoC.Backend.Engine.Language;
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
	public sealed class LanguageOptionsModelView : OptionModelViewDataBase
	{
		//'Language' At top, H1 format. Translated accordingly.
		//Drop Down Box with Language choices. 
		//RichTextBox, scrollable, filled with flavor text. This will be formatted like the game, so there will be text of different colors, with bold, italic, and underline as well.
		//By default, English is selected From Combo Box. We'll wire up two-way with selected index, when it changes, update the text. we'll need some sort of global "Language Changed"
		//event, which will update all the text in the backend. Note that the only GUI that cares about a language change event is either on this page, or part of the options view.
		//all other text can simply load the correct values on switch, as it wont change there. We'll also need to notify the controller that the language changed, so it'll need a 
		//method for that. 

		//

		public StandardSideBarModelView sidebarView { get; }

		private bool UpdateContent = true;

		public int SelectedIndex
		{
			get => LanguageEngine.currentLanguageIndex;
			set
			{
				var oldValue = SelectedIndex;
				LanguageEngine.currentLanguageIndex = value;
				if (oldValue != SelectedIndex)
				{
					RaisePropertyChanged(nameof(SelectedIndex));
					UpdateContent = true;
				}
			}
		}

		public string MainContent
		{
			get => _rTFContent;
			internal set => CheckPropertyChanged(ref _rTFContent, value);
		}
		private string _rTFContent;

		public string PostContent
		{
			get => _postContent;
			internal set => CheckPropertyChanged(ref _postContent, value);
		}
		private string _postContent;


		//public static IEnumerable<string> AvailableLanguages => LanguageEngine.availableLanguages.Select(x => x.LanguageName());

		public ComboBoxWrapper AvailableLanguages { get; }

		private string LanguageString => LanguageEngine.currentLanguage.Language();
		private string ContentString => LanguageEngine.currentLanguage.GenericFlavorTextExample();
		private string LanguageHelper => LanguageEngine.currentLanguage.LanguageInstructionText();

		private void GetRTFText()
		{

			double fontEmSize = runner.FontSizeEms;

			string formattedText = RTFParser.FromHTMLNoHeader(new StringBuilder(LanguageHelper), runner.FontColor.Color, out List<Color> colors);

			string text = $@"\b\fs{ModelViewRunner.HeaderSizeEm} {LanguageString}\b0\par\fs{ModelViewRunner.SmallHeaderEm}\par {formattedText}";
			MainContent = RTFParser.FromRTFText(text, colors, runner);

			PostContent = RTFParser.FromHTML(ContentString, runner);
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
			AvailableLanguages = new ComboBoxWrapper(items, 0);
		}

		private void HandleSelected(int languageIndex)
		{
			SelectedIndex = languageIndex;
			GetRTFText();
		}

		public override void ParseDataForDisplay()
		{
			if (string.IsNullOrEmpty(MainContent))
			{
				UpdateContent = true;
			}


			if (UpdateContent)
			{
				UpdateContent = false;
				GetRTFText();
			}
		}
	}
}
