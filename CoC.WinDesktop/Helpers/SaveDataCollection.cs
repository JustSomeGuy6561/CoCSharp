using CoC.Backend.Engine.Time;
using CoC.Frontend.UI.ControllerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCWinDesktop.Helpers
{
	class SaveDataCollection : StatDataCollectionBase
	{
		private readonly GameDateTime gameTime;
		public SaveDataCollection(string fileName)
		{
			//if (string.isNullOrEmpty(fileName) || !Path.Exists(fileName))
			//{
			gameTime = new GameDateTime(0, 0);
			//}
		}


		public override GameDateTime GetTime()
		{
			return gameTime;
		}

		public override void ParseData()
		{
			//don't do anything. the data is constant. 
		}
	}
}
