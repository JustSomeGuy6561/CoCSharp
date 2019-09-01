using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoCWinDesktop.InterfaceSettings
{
	//#error Fix Me!
	public sealed partial class SidebarFontOption : InterfaceOptions
	{
		public SidebarFontOption() : base(SidebarFont, new SidebarFontSetting())
		{
			((SidebarFontSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class SidebarFontSetting : SimpleSetting
		{
			GuiGlobalSave glob => GuiGlobalSave.data;

			public Action handleSets { get; set; }

			public override void OnSet()
			{
				handleSets();
			}

			public SidebarFontSetting() : base()
			{

			}

			public override bool setting
			{
				get => glob.sidebarUsesModernFont;
				set => glob.sidebarUsesModernFont = value;
			}

			public override string DisabledHint() => OldFontSelected();

			public override string EnabledHint() => NewFontSelected();

			public override bool SettingEnabled(bool possibleSetting, out string whyNot)
			{
				whyNot = null;
				return true;
			}

			public override string WarnPlayersAboutChanging()
			{
				return null;
			}

			public override SimpleDescriptor enabledText => InterfaceStrings.NewText;
			public override SimpleDescriptor disabledText => InterfaceStrings.OldText;
		}
	}
}
