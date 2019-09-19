using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoC.WinDesktop.InterfaceSettings
{
	public partial class SpriteStatusOption : InterfaceOptions
	{
		public SpriteStatusOption() : base(SpriteStatus, new SpriteStatusSetting())
		{
			((SpriteStatusSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class SpriteStatusSetting : SimpleNullableSetting
		{
			GuiGlobalSave glob => GuiGlobalSave.data;
			public Action handleSets { get; set; }

			public override void OnSet()
			{
				handleSets();
			}

			public SpriteStatusSetting()
			{
			}

			public override bool? setting
			{
				get => glob.usesOldSprites;
				set => glob.usesOldSprites = value;
			}

			public override string DisabledHint() => NewSpritesSelected();

			public override string EnabledHint() => OldSpritesSelected();

			public override string UnsetHint() => OffSpritesSelected();

			public override bool SettingEnabled(bool? possibleSetting, out string whyNot)
			{
				whyNot = null;
				return true;
			}

			public override string WarnPlayersAboutChanging()
			{
				return null;
			}

			public override SimpleDescriptor enabledText => InterfaceStrings.OldText;
			public override SimpleDescriptor disabledText => InterfaceStrings.NewText;
			public override SimpleDescriptor unsetText => InterfaceStrings.OffText;
		}
	}
}
