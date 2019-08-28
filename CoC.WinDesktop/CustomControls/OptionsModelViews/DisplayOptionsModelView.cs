using CoC.Backend;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using CoCWinDesktop.CustomControls.SideBarModelViews;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoCWinDesktop.CustomControls.OptionsModelViews
{
	public sealed class DisplayOptionsModelView : OptionModelViewDataBase
	{
		public StandardSideBarModelView sidebarView { get; }

		public OptionsRowSliderWrapper FontSizeSlider { get; }
		public OptionsRowSliderWrapper BackgroundSlider { get; }
		public OptionsRowSliderWrapper TextBackgroundSlider { get; }

		public string ContentString
		{
			get => _contentString;
			private set => CheckPropertyChanged(ref _contentString, value);
		}
		private string _contentString;

		private string rtfSource;
		private int lastLanguageIndex;
		private List<Color> colors;


		public string DisplayOptionsText
		{
			get => _DisplayOptionsText;
			private set => CheckPropertyChanged(ref _DisplayOptionsText, value);
		}
		private string _DisplayOptionsText;

		public string DisplayOptionsHelper
		{
			get => _DisplayOptionsHelper;
			private set => CheckPropertyChanged(ref _DisplayOptionsHelper, value);
		}
		private string _DisplayOptionsHelper;

		public int[] availableFontSizes { get; }

		private int GetFontIndex()
		{
			int index = Array.IndexOf(availableFontSizes, (int)Math.Round(runner.FontSizePoints));
			if (index < 0)
			{
				index = Array.IndexOf(availableFontSizes, 15);
				if (index < 0) index = 0;
			}
			return index;
		}

		private void SetFontSizeFromIndex(int index)
		{
			runner.SetFontSize(index, SizeUnit.POINTS);
			UpdateDisplay();
		}

		private int GetBackgroundIndex()
		{
			return runner.BackgroundIndex;
		}

		private void SetBackgroundIndex(int index)
		{
			runner.BackgroundIndex = index;
			CheckFontColor();
		}

		private int GetTextBackgroundIndex()
		{
			return runner.TextBackgroundIndex;
		}

		private void SetTextBackgroundIndex(int index)
		{
			runner.TextBackgroundIndex = index;
			CheckFontColor();
		}

		private void CheckFontColor()
		{
			if (colors.Count > 0)
			{
				colors[0] = runner.FontColor.Color;
			}
			UpdateDisplay();
		}

		public DisplayOptionsModelView(ModelViewRunner modelViewRunner, OptionsModelView optionsModelView) : base(modelViewRunner, optionsModelView)
		{
			sidebarView = new StandardSideBarModelView(new SaveDataCollection(null), true);

			OrderedHashSet<int> hashSetLookup = lookupMaker(ModelViewRunner.backgrounds);
			Func<int, string> arrayToLookup = (x) => ModelViewRunner.backgrounds[x].title();

			string emptyText(int _) => "";
			string emptyDescriptor() => "";

			EnabledOrDisabledWithToolTipSldr tooltip = (int x, out string y) => ModelViewRunner.backgrounds[x].disabledTooltip(out y);

			BackgroundSlider = new OptionsRowSliderWrapper(InterfaceStrings.BackgroundText, hashSetLookup, arrayToLookup, emptyText, GetBackgroundIndex, SetBackgroundIndex,
				emptyDescriptor, tooltip);

			int count = (int)(MeasurementHelpers.MaxPointFontSize - MeasurementHelpers.MinPointFontSize + 1);

			availableFontSizes = Enumerable.Range((int)MeasurementHelpers.MinPointFontSize, count).ToArray();

			hashSetLookup = new OrderedHashSet<int>(availableFontSizes);
			arrayToLookup = x => availableFontSizes[x].ToString();

			tooltip = (int x, out string y) => { y = null; return true; };

			FontSizeSlider = new OptionsRowSliderWrapper(InterfaceStrings.FontSizeText, hashSetLookup, arrayToLookup, emptyText, GetFontIndex, SetFontSizeFromIndex,
				emptyDescriptor, tooltip);

			hashSetLookup = lookupMaker(ModelViewRunner.textBackgrounds);
			arrayToLookup = x => ModelViewRunner.textBackgrounds[x].title();

			tooltip = (int x, out string y) => { y = null; return true; };

			TextBackgroundSlider = new OptionsRowSliderWrapper(InterfaceStrings.TextBackgroundText, hashSetLookup, arrayToLookup, emptyText, GetTextBackgroundIndex, 
				SetTextBackgroundIndex, emptyDescriptor, tooltip);

			lastLanguageIndex = LanguageEngine.currentLanguageIndex;
			//GetRTFText();
			rtfSource = null;

		}

		private OrderedHashSet<int> lookupMaker<T>(ReadOnlyCollection<T> collection) where T: class
		{
			List<int> validIndices = new List<int>();
			for (int x = 0; x < collection.Count; x++)
			{
				if (collection[x] != null)
				{
					validIndices.Add(x);
				}
			}
			return new OrderedHashSet<int>(validIndices);
		}

		//Top: header, helper
		//ContentRTB in the middle. Full of lorem ipsum or language check text. 

		//bottom: 3 sliders - for font size, text bg color and bg image. 

		//confirm on bottom icons. 
		public override void ParseDataForDisplay()
		{
			if (lastLanguageIndex != LanguageEngine.currentLanguageIndex || string.IsNullOrEmpty(rtfSource))
			{
				lastLanguageIndex = LanguageEngine.currentLanguageIndex;
				GetRTFText();
			}
		}

		private void GetRTFText()
		{
			rtfSource = RTFParser.FromHTMLNoHeader(new StringBuilder(LanguageEngine.currentLanguage.GenericFlavorTextExample()), runner.FontColor.Color, out colors);
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			ContentString = RTFParser.FromRTFText(rtfSource, colors, runner);
		}

		public override Action OnConfirmation => ConfirmControls;


		private void ConfirmControls()
		{
			//do nothing. 
		}
	}
}
