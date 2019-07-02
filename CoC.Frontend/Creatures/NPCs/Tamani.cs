//Tamani.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 1:48 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Frontend.SaveData;
namespace CoC.Frontend.Creatures.NPCs
{
	internal sealed class Tamani
	{
		private static FrontendSessionSave data => FrontendSessionSave.data;
		internal static bool isDisabled => data.TamaniIsDisabled;
	}
}
