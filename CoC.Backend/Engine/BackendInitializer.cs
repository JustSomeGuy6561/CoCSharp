using CoC.Backend.Creatures;
using CoC.Backend.SaveData;

namespace CoC.Backend.Engine
{
	public static class BackendInitializer
	{
		public static void Init()
		{
			SaveSystem.AddGlobalSave(new BackendGlobalData());
			SaveSystem.AddSessionSave(new BackendSessionData());
			Player initialPlayer = new Player(creator: null);
			BackendSessionData.data.player = initialPlayer;
		}
	}
}
