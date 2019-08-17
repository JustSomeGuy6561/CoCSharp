using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class StatDataCollection : StatDataCollectionBase
	{
		public override GameDateTime GetTime()
		{
			return GameDateTime.Now;
		}

		public override void ParseData()
		{
			this.playerStats.Update(GameEngine.currentPlayer);
			//for now just ignore enemy stats.
		}
	}
}
