using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Save;
using CoC.Backend.Save.Internals;
namespace CoC.Backend.Engine
{
	public static class BackendInitializer
	{
		public static void Init()
		{
			SaveSystem.AddGlobalSaveInstance(new BackendGlobalData());
			SaveSystem.AddSessionSaveInstance(new BackendSessionData());
			Player initialPlayer = new Player(creator:null);
			BackendSessionData.data.player = initialPlayer;
		}
	}
}
