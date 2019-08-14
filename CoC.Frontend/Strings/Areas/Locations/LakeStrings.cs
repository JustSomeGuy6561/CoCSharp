//ForestStrings.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:48 PM
using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Lake : LocationBase
	{
		private static string LakeName()
		{
			return "Lake";
		}

		//private static string LakeMimic()
		//{
		//	return "The peaceful sounds of nature are everywhere ";
		//}

		private static string LakeUnlock()
		{
			return "Your wanderings take you far and wide across the barren wasteland that surrounds the portal, " +
				"until the smell of humidity and fresh water alerts you to the nearby lake. With a few quick strides you find a lake so massive the distant shore cannot be seen. " +
				"Grass and a few sparse trees grow all around it." + Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've discovered the Lake!", StringFormats.BOLD);
		}
	}
}
