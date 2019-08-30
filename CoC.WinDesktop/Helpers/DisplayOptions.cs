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
		public BackgroundOption(ModelViewRunner runner) : base(BackgroundText, new BackgroundSetting(runner))
		{
		}

		private class BackgroundSetting : AdvancedSetting
		{
			private readonly ModelViewRunner runner;

			public BackgroundSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			GuiGlobalSave glob => GuiGlobalSave.data;
			public override int setting
			{
				get => runner.BackgroundIndex;
				set => runner.BackgroundIndex = value;
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
		public TextBackgroundOption(ModelViewRunner runner) : base(TextBackgroundText, new TextBackgroundSetting(runner))
		{
		}

		private class TextBackgroundSetting : AdvancedSetting
		{
			private readonly ModelViewRunner runner;

			public TextBackgroundSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			public override int setting
			{
				get => runner.TextBackgroundIndex;
				set => runner.TextBackgroundIndex = value;
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

	public sealed partial class FontSizeOption : DisplayOptions
	{
		public FontSizeOption(ModelViewRunner runner) : base(FontSizeText, new FontSizeSetting(runner))
		{
			
		}

		private class FontSizeSetting : AdvancedSetting
		{
			private GuiGlobalSave glob => GuiGlobalSave.data;
			private readonly ModelViewRunner runner;
			public FontSizeSetting(ModelViewRunner runner)
			{
				int count = (int)(MeasurementHelpers.MaxPointFontSize - MeasurementHelpers.MinPointFontSize + 1);
				this.runner = runner;
				availableOptions = new OrderedHashSet<int>(Enumerable.Range((int)MeasurementHelpers.MinPointFontSize, count));
			}

			//we're wiring it through the runner so it procs the correct font size changes for the GUI. 
			public override int setting
			{
				get => (int)runner.FontSizePoints;
				set => runner.SetFontSize(value, SizeUnit.POINTS);
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
