using CoC.Backend;
using CoC.Backend.Settings;
using CoC.WinDesktop.Strings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoC.WinDesktop.DisplaySettings
{
	public class DisplayOptions : OptionBase
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

		protected DisplayOptions(SimpleDescriptor settingName, SettingBase globalSettingData) : base(settingName, globalSettingData)
		{
			onSetActions = new List<Action>();
		}

		public override void PostLocalSessionInit()
		{
			//do nothing - the session save in GUI doesn't do anything.
		}


	}
}
