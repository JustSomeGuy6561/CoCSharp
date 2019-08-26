using CoC.Backend.SaveData;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.Fetishes
{
	//Base class for fetishes that can be disabled or enabled via settings. 
	public abstract class FetishBase
	{
		public readonly SimpleDescriptor name;

		private protected FetishBase(SimpleDescriptor fetishName)
		{
			name = fetishName ?? throw new ArgumentNullException(nameof(fetishName));
		}

		public virtual bool globalCannotBeNull => false;
	}

	public abstract class SimpleFetish : FetishBase
	{
		protected SimpleFetish(SimpleDescriptor fetishName) : base(fetishName)
		{

		}

		public abstract bool enabled { get; protected set; }
		public abstract bool? enabledGlobal { get; protected set; }

		public void SetEnabled(bool? value, bool global)
		{
			if (!global)
			{
				enabled = value ?? throw new ArgumentException();
			}
			else
			{
				enabledGlobal = value;
			}
		}

		public abstract SimpleDescriptor enabledHint { get; }
		public abstract SimpleDescriptor disabledHint { get; }

		public virtual SimpleDescriptor enabledText { get; } = EngineStrings.EnableText;
		public virtual SimpleDescriptor disabledText { get; } = EngineStrings.DisableText;
		//enabledText = "enabled" or "on"
		//enabledText = "disabled" or "off"
	}

	/// <summary>
	/// Advanced version of a fetish that allows more than two options. It is implemented via an integer, instead of a boolean.
	/// Unlike the simple version, the text for enabled and disabled cannot be inferred, so it needs a dictionary lookup for these values. 
	/// </summary>
	public abstract class AdvancedFetish : FetishBase
	{
		protected AdvancedFetish(SimpleDescriptor fetishName) : base(fetishName)
		{
		}

		//int is the value used to descibe the current setting. This gets the possible values. Ordered so GUI displays it consistently. 
		protected internal abstract OrderedHashSet<int> availableSettings { get; }
		public OrderedHashSet<int> availableStatuses => new OrderedHashSet<int>(availableSettings);
		public abstract string settingHint(int status, bool isGlobal);
		public abstract string settingText(int status, bool isGlobal);

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
		public abstract bool isEnabled();
	}
}
