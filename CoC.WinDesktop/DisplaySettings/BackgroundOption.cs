using CoC.Backend.Settings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.WinDesktop.DisplaySettings
{
	public sealed partial class BackgroundOption : DisplayOptions
	{
		public BackgroundOption() : base(BackgroundText, new BackgroundSetting())
		{
			((BackgroundSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class BackgroundSetting : AdvancedSetting
		{

			public BackgroundSetting()
			{
			}

			GuiGlobalSave glob => GuiGlobalSave.data;
			public override int setting
			{
				get => glob.backgroundIndex;
				set => glob.backgroundIndex = value;
			}
			public Action handleSets { get; set; }

			public override void OnSet()
			{
				handleSets();
			}

			public override OrderedHashSet<int> availableOptions => new OrderedHashSet<int>(Enumerable.Range(0, ModelViewRunner.numBackgrounds));

			public override string SelectedSettingHint(int selectedSetting)
			{
				return "";
			}

			public override string SelectedSettingText(int selectedSetting)
			{
				return ModelViewRunner.backgrounds[selectedSetting].title();
			}

			public override bool SettingEnabled(int possibleSetting, out string whyNot)
			{
				return ModelViewRunner.backgrounds[possibleSetting].disabledTooltip(out whyNot);
			}

			public override string WarnPlayersAboutChanging()
			{
				return null;
			}
		}
	}
}
