using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoCWinDesktop.InterfaceSettings
{
	public sealed partial class ImagePackOption : InterfaceOptions
	{
		public ImagePackOption() : base(ImagePackText, new ImagePackSetting())
		{
			((ImagePackSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class ImagePackSetting : SimpleSetting
		{
			public Action handleSets { get; set; }

			GuiGlobalSave glob => GuiGlobalSave.data;


			public ImagePackSetting()
			{

			}

			public override bool setting
			{
				get => glob.imagePackEnabled;
				set => glob.imagePackEnabled = value;
			}

			public override string DisabledHint() => ImagePackDisabled();

			public override string EnabledHint() => ImagePackEnabled();

			public override bool SettingEnabled(bool possibleSetting, out string whyNot)
			{
				//Consider disabling enable option and explaining no image pack installed when no image pack installed.
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
