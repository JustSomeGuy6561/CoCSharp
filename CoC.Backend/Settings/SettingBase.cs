using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.Settings
{
	public abstract class SettingBase
	{
		private protected SettingBase()
		{

		}

		//if simply warning the player is not enough, you can request the gui confirm a change. Note that this may be ignored, or the player may choose to disable this behavior. 
		public virtual bool confirmChange => false;
	}
	public abstract class SimpleSetting : SettingBase
	{
		protected SimpleSetting() : base()
		{

		}

		public abstract bool setting { get; set; }
		public bool Get() => setting;
		public void Set(bool arg) => setting = arg;


		public virtual SimpleDescriptor enabledText { get; } = EngineStrings.EnableText;
		public abstract string EnabledHint();

		public virtual SimpleDescriptor disabledText { get; } = EngineStrings.DisableText;
		public abstract string DisabledHint();

		//there are some setting that prevent the user from switching back or disable settings based on what other settings are. 
		//this provides you with the ability to do that. The 'whyNot' string will only be used if this function returns false, and allows you to explain why that setting is disabled.
		public abstract bool SettingEnabled(bool possibleSetting, out string whyNot);

		//in the same vein as previous function, changing some settings may have unintended consequences for the uninformed. This provides a means to provide a warning that changing this
		//setting will have side effects. How the gui handles this is dependant on implementation, of course. 
		public abstract string WarnPlayersAboutChanging();
	}

	public abstract class SimpleNullableSetting : SettingBase
	{
		protected SimpleNullableSetting() : base()
		{

		}

		public abstract bool? setting { get; set; }
		public bool? Get() => setting;
		public void Set(bool? arg) => setting = arg;

		public virtual SimpleDescriptor enabledText { get; } = EngineStrings.EnableText;
		public abstract string EnabledHint();

		public virtual SimpleDescriptor disabledText { get; } = EngineStrings.DisableText;
		public abstract string DisabledHint();

		public virtual SimpleDescriptor unsetText { get; } = EngineStrings.UnsetText;
		public virtual string UnsetHint() => EngineStrings.UnsetHint();

		//there are some setting that prevent the user from switching back or disable settings based on what other settings are. 
		//this provides you with the ability to do that. The 'whyNot' string will only be used if this function returns false, and allows you to explain why that setting is disabled.
		public abstract bool SettingEnabled(bool? possibleSetting, out string whyNot);

		//in the same vein as previous function, changing some settings may have unintended consequences for the uninformed. This provides a means to provide a warning that changing this
		//setting will have side effects. How the gui handles this is dependant on implementation, of course. 
		public abstract string WarnPlayersAboutChanging();
	}

	public abstract class AdvancedSetting : SettingBase
	{
		protected AdvancedSetting() : base()
		{
		}

		public abstract int setting { get; set; }
		public int Get() => setting;
		public void Set(int arg) => setting = arg;

		//in an ideal world, the settings would be a range, 0 to whatever, and you'd want them to display in that order. However, it's possible for newer versions of code to make 
		//certain values deprecated, or you may simply wish to display them out of order. This ordered hashset is the solution to both problems. simply add the possibilities in order
		//you wish for them to display, and obviously do not add any items that are not accepted. 
		public abstract OrderedHashSet<int> availableOptions { get; }


		public abstract string SelectedSettingText(int selectedSetting);
		public abstract string SelectedSettingHint(int selectedSetting);

		//there are some setting that prevent the user from switching back or disable settings based on what other settings are. 
		//this provides you with the ability to do that. The 'whyNot' string will only be used if this function returns false, and allows you to explain why that setting is disabled.
		public abstract bool SettingEnabled(int possibleSetting, out string whyNot);

		//in the same vein as previous function, changing some settings may have unintended consequences for the uninformed. This provides a means to provide a warning that changing this
		//setting will have side effects. How the gui handles this is dependant on implementation, of course. 
		public abstract string WarnPlayersAboutChanging();
	}

	public abstract class AdvancedNullableSetting : SettingBase
	{
		protected AdvancedNullableSetting() : base()
		{
		}

		public abstract int? setting { get; set; }
		public int? Get() => setting;
		public void Set(int? arg) => setting = arg;
		//in an ideal world, the settings would be a range, 0 to whatever, and you'd want them to display in that order. However, it's possible for newer versions of code to make 
		//certain values deprecated, or you may simply wish to display them out of order. This ordered hashset is the solution to both problems. simply add the possibilities in order
		//you wish for them to display, and obviously do not add any items that are not accepted. 
		public abstract OrderedHashSet<int?> availableOptions { get; }


		public abstract string SelectedSettingText(int? selectedSetting);
		public abstract string SelectedSettingHint(int? selectedSetting);

		//there are some setting that prevent the user from switching back or disable settings based on what other settings are. 
		//this provides you with the ability to do that. The 'whyNot' string will only be used if this function returns false, and allows you to explain why that setting is disabled.
		public abstract bool SettingEnabled(int? possibleSetting, out string whyNot);

		//in the same vein as previous function, changing some settings may have unintended consequences for the uninformed. This provides a means to provide a warning that changing this
		//setting will have side effects. How the gui handles this is dependant on implementation, of course. 
		public abstract string WarnPlayersAboutChanging();
	}

}
