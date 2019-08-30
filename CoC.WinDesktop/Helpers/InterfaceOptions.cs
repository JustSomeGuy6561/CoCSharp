using CoC.Backend;
using CoC.Backend.Settings;
using System;

namespace CoCWinDesktop.Helpers
{
	//public static class InterfaceOptions
	//{
	//public static
	//private static GuiGlobalSave saveData => GuiGlobalSave.data;

	//public static readonly OptionsRowButtonWrapper SidebarFont;

	//private static void SetFontState(bool isNew)
	//{
	//	saveData.sidebarUsesModernFont = isNew;
	//}

	//private static bool GetFontState()
	//{
	//	return saveData.sidebarUsesModernFont;
	//}

	//public static readonly OptionsRowButtonWrapper SpriteStatus;

	////off, old, new. 
	//private static void SetSpriteState(bool? status)
	//{
	//	saveData.usesOldSprites = status;
	//}

	//private static bool? GetSpriteState()
	//{
	//	return saveData.usesOldSprites;
	//}

	//public static readonly OptionsRowButtonWrapper ImagePackStatus;
	//public static readonly OptionsRowButtonWrapper SideBarAnimations;
	//public static readonly OptionsRowButtonWrapper EnemySideBars;

	//static InterfaceOptions()
	//{
	//	SidebarFont = new OptionsRowButtonWrapper(InterfaceStrings.SidebarFont, SetFontState, GetFontState,
	//		InterfaceStrings.NewText, InterfaceStrings.OldText, InterfaceStrings.NewFontSelected, InterfaceStrings.OldFontSelected);
	//	SpriteStatus = new OptionsRowButtonWrapper(InterfaceStrings.SpriteStatus, SetSpriteState, GetSpriteState,
	//		InterfaceStrings.OldText, InterfaceStrings.NewText, InterfaceStrings.OldSpritesSelected, InterfaceStrings.NewSpritesSelected);
	//	ImagePackStatus = new OptionsRowButtonWrapper(InterfaceStrings.ImagePackStatus, SetImagePack, GetImagePack,
	//		InterfaceStrings.NewText, InterfaceStrings.OldText, InterfaceStrings.NewFontSelected, InterfaceStrings.OldFontSelected);
	//	SideBarAnimations = new OptionsRowButtonWrapper(InterfaceStrings.SidebarFont, SetFontState, GetFontState,
	//		InterfaceStrings.NewText, InterfaceStrings.OldText, InterfaceStrings.NewFontSelected, InterfaceStrings.OldFontSelected);
	//	EnemySideBars = new OptionsRowButtonWrapper(InterfaceStrings.SidebarFont, SetFontState, GetFontState,
	//		InterfaceStrings.NewText, InterfaceStrings.OldText, InterfaceStrings.NewFontSelected, InterfaceStrings.OldFontSelected);
	//}

	public class InterfaceOptions : OptionBase
	{
		public InterfaceOptions(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
		}

		public override void PostLocalSessionInit()
		{
			//do nothing - no local setting.
		}
	}

	//#error Fix Me!
	public sealed partial class SidebarFontOption : InterfaceOptions
	{
		public SidebarFontOption(ModelViewRunner runner) : base(SidebarFont, new SidebarFontSetting(runner))
		{
		}

		private class SidebarFontSetting : SimpleSetting
		{
			private readonly ModelViewRunner runner;
			public SidebarFontSetting(ModelViewRunner runner) : base()
			{
				this.runner = runner;
			}

			public override bool setting
			{
				get => runner.SidebarUsesModernFont;
				set => runner.SidebarUsesModernFont = value;
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

	public partial class SpriteStatusOption : InterfaceOptions
	{
		public SpriteStatusOption(ModelViewRunner runner) : base(SpriteStatus, new SpriteStatusSetting(runner))
		{
		}

		private class SpriteStatusSetting : SimpleNullableSetting
		{
			private readonly ModelViewRunner runner;

			public SpriteStatusSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			public override bool? setting
			{
				get => runner.UsesOldSprites;
				set => runner.UsesOldSprites = value;
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

	public sealed partial class ImagePackOption : InterfaceOptions
	{
		public ImagePackOption(ModelViewRunner runner) : base(ImagePackText, new ImagePackSetting(runner))
		{
		}

		private class ImagePackSetting : SimpleSetting
		{
			private readonly ModelViewRunner runner;

			public ImagePackSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			public override bool setting
			{
				get => runner.ImagePackEnabled;
				set => runner.ImagePackEnabled = value;
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

	public sealed partial class SidebarAnimationOption : InterfaceOptions
	{
		public SidebarAnimationOption(ModelViewRunner runner) : base(SidebarAnimationText, new SidebarAnimationSetting(runner))
		{
		}

		private class SidebarAnimationSetting : SimpleSetting
		{
			private readonly ModelViewRunner runner;

			public SidebarAnimationSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			public override bool setting
			{
				get => runner.IsAnimated;
				set => runner.IsAnimated = value;
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
	public sealed partial class EnemySidebarOption : InterfaceOptions
	{
		public EnemySidebarOption(ModelViewRunner runner) : base(EnemyStatBar, new EnemySidebarSetting(runner))
		{
		}

		private class EnemySidebarSetting : SimpleSetting
		{
			private readonly ModelViewRunner runner;

			public EnemySidebarSetting(ModelViewRunner runner)
			{
				this.runner = runner;
			}

			public override bool setting
			{
				get => runner.ShowEnemyStatBars;
				set => runner.ShowEnemyStatBars = value;
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
