using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoC.WinDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.WinDesktop.DisplaySettings
{
	public sealed partial class FontSizeOption : DisplayOptions
	{
		public FontSizeOption() : base(FontSizeText, new FontSizeSetting())
		{
			((FontSizeSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class FontSizeSetting : AdvancedSetting
		{
			private GuiGlobalSave glob => GuiGlobalSave.data;

			public Action handleSets { get; internal set; }


			public FontSizeSetting()
			{
				int count = (int)(MeasurementHelpers.MaxPointFontSize - MeasurementHelpers.MinPointFontSize + 1);
				availableOptions = new OrderedHashSet<int>(Enumerable.Range((int)MeasurementHelpers.MinPointFontSize, count));
			}

			//we're wiring it through the runner so it procs the correct font size changes for the GUI.
			public override int setting
			{
				get => (int)Math.Round(MeasurementHelpers.ConvertFromEms(glob.FontSizeInEms, SizeUnit.POINTS));
				set => glob.FontSizeInEms = MeasurementHelpers.ConvertToEms(value, SizeUnit.POINTS);
			}

			public override void OnSet()
			{
				handleSets();
			}

			public override OrderedHashSet<int> availableOptions { get; }

			public override string SelectedSettingHint(int selectedSetting)
			{
				return FontHint(selectedSetting);
			}

			public override string SelectedSettingText(int selectedSetting)
			{
				return FontText(selectedSetting);
			}

			public override bool SettingEnabled(int possibleSetting, out string whyNot)
			{
				whyNot = null;
				return true;
			}

			public override string WarnPlayersAboutChanging()
			{
				return null;
			}
		}
	}
}
