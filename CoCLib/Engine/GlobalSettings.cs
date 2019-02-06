//GlobalSettings.cs
//Description:
//Author: JustSomeGuy
//2/1/2019, 7:20 PM
using CoC.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Engine
{
	public static class GlobalSettings
	{
		[StaticSave]
		static bool waterSports;
	}
}
