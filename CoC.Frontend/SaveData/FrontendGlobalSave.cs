//FrontendGlobalSave.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 9:42 PM
using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.SaveData
{
	internal sealed class FrontendGlobalSave : Backend.SaveData.GlobalSaveData
	{
		public static FrontendGlobalSave data => SaveSystem.GetGlobalSave<FrontendGlobalSave>();

		internal bool UnlockedNewGameHerm = false;
		//internal bool UnlockedNewGameHerm = true; //testing

		internal bool? AllowWaterSports = false;
		internal bool? SillyModeGlobal = null;
	}
}
