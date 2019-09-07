using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CoC.Backend.Engine;
using CoC.UI;
using CoCWinDesktop.Helpers;
using CoCWinDesktop.ModelView;

namespace CoCWinDesktop.CustomControls.ExtraItemModelViews
{
	public sealed partial class MenuCreditsModelView : ExtraItemModelViewBase
	{
		private int LastLanguageIndex;
		private int FontSizeInEms;

		private Controller controller => runner.controller;

		private string rtfContentWithSubstitutes;

		public MenuCreditsModelView(ModelViewRunner modelViewRunner, ExtraMenuItemsModelView parentModelView) : base(modelViewRunner, parentModelView)
		{
			LastLanguageIndex = LanguageEngine.currentLanguageIndex;
			FontSizeInEms = MeasurementHelpers.FontSizeInEms;

			ContentTitle = CreditStr();
			ContentHelper = CreditHelperStr();
			ParseCredits();

			PostContent = DisclaimerStr();
		}

		internal override void ParseDataForDisplay()
		{
			if (LastLanguageIndex != LanguageEngine.currentLanguageIndex)
			{
				LastLanguageIndex = LanguageEngine.currentLanguageIndex;

				ContentTitle = CreditStr();
				ContentHelper = CreditHelperStr();
				PostContent = DisclaimerStr();
				ParseCredits();
			}
			else if (FontSizeInEms != MeasurementHelpers.FontSizeInEms)
			{
				FontSizeInEms = MeasurementHelpers.FontSizeInEms;
				UpdateDisplay();
			}
		}

		private void ParseCredits()
		{
			StringBuilder contentString = new StringBuilder();
			string safeString;
			foreach (var category in controller.GetCredits())
			{
				if (contentString.Length != 0)
				{
					contentString.Append(@"\par");
				}

				safeString = RTFParser.ToRTFSafeString(category.CreditCategoryText());
				contentString.Append(@"\fs{0}\par\b " + safeString + @"\b0");
				foreach (var subCategory in category.subCategories)
				{
					safeString = RTFParser.ToRTFSafeString(subCategory.CreditCategoryText());
					contentString.Append(@"\par\fs{1}\par\b " + safeString + @"\b0\fs{2}");

					foreach (var credit in subCategory.names)
					{
						safeString = RTFParser.ToRTFSafeString(credit);
						contentString.Append(@"\line \bullet  " + safeString);
					}
				}
			}
			rtfContentWithSubstitutes = contentString.ToString();

			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			Content = RTFParser.FromRTFText(string.Format(rtfContentWithSubstitutes, FontSizeInEms + 12, FontSizeInEms + 6, FontSizeInEms), new List<Color>() { runner.FontColor.Color }, runner);
		}
	}
}
