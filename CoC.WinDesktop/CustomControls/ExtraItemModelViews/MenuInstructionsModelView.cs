using CoC.Backend.Engine;
using CoC.Frontend.Engine;
using CoC.UI;
using CoC.WinDesktop.Helpers;
using CoC.WinDesktop.ModelView;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CoC.WinDesktop.CustomControls.ExtraItemModelViews
{
	public sealed partial class MenuInstructionsModelView : ExtraItemModelViewBase
	{
		private int LastLanguageIndex;
		private int FontSizeInEms;
		private List<Color> colors;
		private Color lastFontColor;
		private Controller controller => runner.controller;

		private string rtfContentWithSubstitutes;

		private InstructionItem controls = new InstructionItem(ControlsTitle, ControlsDesc);

		public MenuInstructionsModelView(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView) : base(modelViewRunner, parentModelView)
		{
			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
			FontSizeInEms = MeasurementHelpers.FontSizeInEms;

			lastFontColor = runner.FontColor.Color;

			ContentTitle = InstructionsHeader();
			ContentHelper = null;
			ParseInstructions();

			PostContent = SaveWarning();
		}

		internal override void ParseDataForDisplay()
		{
			if (LastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;

				ContentTitle = InstructionsHeader();
				PostContent = SaveWarning();
				ParseInstructions();
			}
			else if (FontSizeInEms != MeasurementHelpers.FontSizeInEms)
			{
				FontSizeInEms = MeasurementHelpers.FontSizeInEms;
				UpdateDisplay();
			}
			else if (runner.FontColor.Color != lastFontColor)
			{
				UpdateDisplay();
			}
		}

		private void ParseInstructions()
		{
			StringBuilder contentString = new StringBuilder();
			string safeString;
			foreach (var category in controller.GetInstructions().Union(new InstructionItem[1] { controls }))
			{
				if (contentString.Length != 0)
				{
					contentString.Append(@"\par");
				}

				safeString = RTFParser.ToRTFSafeString(category.header());
				contentString.Append(@"\fs{0}\par\b " + safeString + @"\b0");
				safeString = RTFParser.FromHTMLNoHeader(new StringBuilder(category.description()), runner.FontColor.Color, out colors);
				contentString.Append(@"\fs{1}\par " + safeString);
			}

			rtfContentWithSubstitutes = contentString.ToString();

			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			if (colors is null || colors.Count == 0)
			{
				colors = new List<Color>()
				{
					runner.FontColor.Color
				};
			}
			if (lastFontColor != runner.FontColor.Color )
			{
				lastFontColor = runner.FontColor.Color;
				colors[0] = runner.FontColor.Color;
			}

			Content = RTFParser.FromRTFText(string.Format(rtfContentWithSubstitutes, FontSizeInEms + 6, FontSizeInEms), colors, runner);
		}
	}
}