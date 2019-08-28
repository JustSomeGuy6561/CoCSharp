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

	public abstract class InterfaceOptions : OptionBase
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

	public class SidebarFontOption : SimpleSetting
	{
		public SidebarFontOption() : base()
		{
		}

		public override bool setting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string DisabledHint()
		{
			throw new NotImplementedException();
		}

		public override string EnabledHint()
		{
			throw new NotImplementedException();
		}

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			throw new NotImplementedException();
		}

		public override string WarnPlayersAboutChanging()
		{
			throw new NotImplementedException();
		}
	}

	public class SpriteStatusSetting : SimpleNullableSetting
	{
		public override bool? setting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string DisabledHint()
		{
			throw new NotImplementedException();
		}

		public override string EnabledHint()
		{
			throw new NotImplementedException();
		}

		public override bool SettingEnabled(bool? possibleSetting, out string whyNot)
		{
			throw new NotImplementedException();
		}

		public override string WarnPlayersAboutChanging()
		{
			throw new NotImplementedException();
		}
	}
	public sealed partial class ImagePackStatus : SimpleSetting
	{
		public override bool setting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string DisabledHint()
		{
			throw new NotImplementedException();
		}

		public override string EnabledHint()
		{
			throw new NotImplementedException();
		}

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			throw new NotImplementedException();
		}

		public override string WarnPlayersAboutChanging()
		{
			throw new NotImplementedException();
		}
	}
	public sealed partial class SideBarAnimations : SimpleSetting
	{
		public override bool setting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string DisabledHint()
		{
			throw new NotImplementedException();
		}

		public override string EnabledHint()
		{
			throw new NotImplementedException();
		}

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			throw new NotImplementedException();
		}

		public override string WarnPlayersAboutChanging()
		{
			throw new NotImplementedException();
		}
	}
	public sealed partial class EnemySideBars : SimpleSetting
	{
		public override bool setting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string DisabledHint()
		{
			throw new NotImplementedException();
		}

		public override string EnabledHint()
		{
			throw new NotImplementedException();
		}

		public override bool SettingEnabled(bool possibleSetting, out string whyNot)
		{
			throw new NotImplementedException();
		}

		public override string WarnPlayersAboutChanging()
		{
			throw new NotImplementedException();
		}
	}
}
