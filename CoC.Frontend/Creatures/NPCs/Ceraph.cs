using System;
using System.Collections.Generic;
using System.Text;
using CoC.Frontend.SaveData;

namespace CoC.Frontend.Creatures.NPCs
{
	public class Ceraph
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;


		public static bool isUnlocked => data.CeraphUnlocked;
		public static bool isSlave => data.CeraphIsSlave;
		public static bool isDisabled => data.CeraphIsDisabled;
	}
}
