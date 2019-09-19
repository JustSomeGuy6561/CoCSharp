using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoC.WinDesktop.InterfaceSettings
{
	public sealed partial class EnemySidebarOption : InterfaceOptions
	{
		public EnemySidebarOption() : base(EnemyStatBar, new EnemySidebarSetting())
		{
			((EnemySidebarSetting)globalSetting).handleSets = HandleSetActions;
		}

		private class EnemySidebarSetting : SimpleSetting
		{
			public Action handleSets { get; set; }

			GuiGlobalSave glob => GuiGlobalSave.data;


			public EnemySidebarSetting()
			{

			}

			public override bool setting
			{
				get => glob.showEnemyStatBars;
				set => glob.showEnemyStatBars = value;
			}

			public override string DisabledHint() => EnemyStatBarsOff();

			public override string EnabledHint() => EnemyStatBarsOn();

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
