using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoCWinDesktop.InterfaceSettings
{
	public sealed partial class SidebarAnimationOption : InterfaceOptions
	{
		public SidebarAnimationOption() : base(SidebarAnimationText, new SidebarAnimationSetting())
		{
			((SidebarAnimationSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class SidebarAnimationSetting : SimpleSetting
		{
			public Action handleSets { get; set; }

			GuiGlobalSave glob => GuiGlobalSave.data;

			public SidebarAnimationSetting()
			{

			}

			public override bool setting
			{
				get => glob.isAnimated;
				set => glob.isAnimated = value;
			}

			public override string DisabledHint() => AnimationsOff();

			public override string EnabledHint() => AnimationsOn();

			public override bool SettingEnabled(bool possibleSetting, out string whyNot)
			{
				whyNot = null;
				return true;
			}

			public override string WarnPlayersAboutChanging()
			{
				return null;
			}

			public override SimpleDescriptor enabledText => InterfaceStrings.OnText;
			public override SimpleDescriptor disabledText => InterfaceStrings.OffText;
		}
	}
}
