using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Settings
{
	#warning Add validation techniques to allow these to be disabled based on other settings. 
	public abstract class GameplaySettingBase
	{
		public readonly SimpleDescriptor name;
		private protected GameplaySettingBase(SimpleDescriptor settingName)
		{
			name = settingName ?? throw new ArgumentNullException();
		}

		private protected abstract bool isOneWay { get; }

		public virtual bool globalCannotBeNull => false;

		public bool cannotEnableOnceDisabled => isOneWay;
	}

	public abstract class SimpleGameplaySettingBase : GameplaySettingBase
	{
		protected SimpleGameplaySettingBase(SimpleDescriptor name) : base(name)
		{

		}

		public abstract string enabledHint(bool isGlobal);
		public abstract string disabledHint(bool isGlobal);

		public virtual SimpleDescriptor enabledText { get; } = EngineStrings.EnableText;
		public virtual SimpleDescriptor disabledText { get; } = EngineStrings.DisableText;
		//enabledText = "enabled" or "on"
		//enabledText = "disabled" or "off"

		private protected override bool isOneWay => false;

		public abstract bool enabled { get; protected set; }
		public abstract bool? enabledGlobal { get; protected set; }

		public void SetEnabled(bool? isEnabled, bool isGlobal)
		{
			if (isGlobal)
			{
				enabledGlobal = isEnabled;
			}
			else
			{
				enabled = isEnabled ?? throw new ArgumentException();
			}
		}

		public bool? GetStatus(bool isGlobal)
		{
			return isGlobal ? enabledGlobal : enabled;
		}
	}

	public abstract class AdvancedGameplaySettingBase : GameplaySettingBase
	{
		protected AdvancedGameplaySettingBase(SimpleDescriptor settingName) : base(settingName)
		{
		}

		//int is the value used to descibe the current setting. This gets the possible values. Ordered so GUI displays it consistently. 
		protected internal abstract OrderedHashSet<int> availableSettings { get; }
		public OrderedHashSet<int> availableStatuses => new OrderedHashSet<int>(availableSettings);
		public abstract string settingHint(int status, bool isGlobal);
		public abstract string settingText(int status, bool isGlobal);

		private protected override bool isOneWay => false;


		public abstract int status { get; protected set; }
		public abstract int? statusGlobal { get; protected set; }

		public void SetStatus(int? index, bool isGlobal)
		{
			if (isGlobal)
			{
				statusGlobal = index;
			}
			else
			{
				status = index ?? throw new ArgumentException();
			}
		}

		public int? GetStatus(bool isGlobal)
		{
			return isGlobal ? statusGlobal : status;
		}
	}

	public abstract class LimitedGameplaySettingBase : GameplaySettingBase
	{
		protected LimitedGameplaySettingBase(SimpleDescriptor settingName) : base(settingName)
		{

		}

		public abstract bool enabled { get; protected set; }
		public abstract bool? enabledGlobal { get; protected set; }

		public abstract string enabledHint(bool isGlobal);
		public abstract string disabledHint(bool isGlobal);

		public virtual SimpleDescriptor enabledText { get; } = EngineStrings.EnableText;
		public virtual SimpleDescriptor disabledText { get; } = EngineStrings.DisableText;

		public void SetEnabled(bool? isEnabled, bool isGlobal)
		{
			if (isGlobal)
			{
				enabledGlobal = isEnabled;
			}
			else
			{
				enabled = isEnabled ?? throw new ArgumentException();
			}
		}

		public bool? GetStatus(bool isGlobal)
		{
			return isGlobal ? enabledGlobal : enabled;
		}

		private protected override bool isOneWay => true;
	}
}

