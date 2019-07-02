//ForestStrings.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:48 PM
using CoC.Backend;
using CoC.Backend.Areas;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Forest : LocationBase
	{
		private static string ForestName()
		{
			return "Forest";
		}

		private static string ForestMimic()
		{
			return "The peaceful sounds of nature are everywhere ";
		}

		private static string ForestUnlock()
		{
			throw new CoC.Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BigJunkForestText()
		{
			throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
