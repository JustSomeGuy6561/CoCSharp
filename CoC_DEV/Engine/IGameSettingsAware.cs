//IGameSettingsAware.cs
//Description:
//Author: JustSomeGuy
//1/28/2019, 2:20 AM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Engine
{
	interface IGameSettingsAware<T> where T : SessionSettings
	{
		void reactToChangeInSetting(T setting);
	}
}
