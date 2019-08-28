using CoC.Backend;
using CoC.Backend.Settings;
using CoC.Backend.Tools;
using CoCWinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	public class DisplayOptions : OptionBase
	{
		protected DisplayOptions(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}

		public override void PostLocalSessionInit()
		{
			//do nothing - the session save in GUI doesn't do anything.
		}
	}

	public sealed partial class BackgroundOption : DisplayOptions
	{
		public BackgroundOption() : base(InterfaceStrings.BackgroundText, new BackgroundSetting())
		{
		}

		private class BackgroundSetting : AdvancedSetting
		{
			GuiGlobalSave glob => GuiGlobalSave.data;
			public override int setting
			{
				get => glob.backgroundIndex;
				set => glob.backgroundIndex = value;
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

	public sealed partial class TextBackgroundOption : DisplayOptions
	{
		public TextBackgroundOption(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}
	}

	public sealed partial class FontSizeOption : DisplayOptions
	{
		public FontSizeOption(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}
	}
}
