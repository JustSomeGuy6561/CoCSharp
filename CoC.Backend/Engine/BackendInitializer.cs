using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Save;
using CoC.Backend.SaveData;
namespace CoC.Backend.Engine
{
	class BackendInitializer
	{
		public static void Init()
		{
			SaveSystem.AddGlobalSaveInstance(new BackendGlobalData(), typeof(BackendGlobalData));
			SaveSystem.AddSessionSaveInstance(new BackendSessionData(), typeof(BackendSessionData));
		}
	}
}
