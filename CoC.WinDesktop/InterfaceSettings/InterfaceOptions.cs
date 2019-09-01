using CoC.Backend;
using CoC.Backend.Settings;
using System;
using System.Collections.Generic;

namespace CoCWinDesktop.InterfaceSettings
{
	public class InterfaceOptions : OptionBase
	{
		protected List<Action> onSetActions;

		public void AddGlobalSetListener(Action onSet)
		{
			onSetActions.Add(onSet);
		}

		protected void HandleSetActions()
		{
			foreach (var set in onSetActions)
			{
				set?.Invoke();
			}
		}

		public InterfaceOptions(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
			onSetActions = new List<Action>();
		}

		public override void PostLocalSessionInit()
		{
			//do nothing - no local setting.
		}
	}
}
