using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.SaveData
{
	internal sealed class FrontendGlobalSave : Backend.SaveData.SaveData
	{
		public static FrontendGlobalSave data => SaveSystem.getGlobalSave<FrontendGlobalSave>();

		internal bool UnlockedNewGameHerm = false;
	}
}
