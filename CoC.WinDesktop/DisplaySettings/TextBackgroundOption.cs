using CoC.Backend.Settings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCWinDesktop.DisplaySettings
{
	public sealed partial class TextBackgroundOption : DisplayOptions
	{
		public TextBackgroundOption() : base(TextBackgroundText, new TextBackgroundSetting())
		{
			((TextBackgroundSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class TextBackgroundSetting : AdvancedSetting
		{
			public Action handleSets { get; set; }


			public TextBackgroundSetting()
			{
				
			}

			public override int setting
			{
				get => GuiGlobalSave.data.textBackgroundIndex;
				set => GuiGlobalSave.data.textBackgroundIndex = value;
			}

			public override void OnSet()
			{
				handleSets();
			}

			public override OrderedHashSet<int> availableOptions { get; } = new OrderedHashSet<int>(Enumerable.Range(0, ModelViewRunner.numTextBackgrounds));

			public override string SelectedSettingHint(int selectedSetting)
			{
				return null;
			}

			public override string SelectedSettingText(int selectedSetting)
			{
				return ModelViewRunner.textBackgrounds[selectedSetting].title();
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
