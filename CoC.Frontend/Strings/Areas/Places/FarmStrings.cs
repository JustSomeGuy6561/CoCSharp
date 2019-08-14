//FarmStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Places
{
	internal sealed partial class Farm
	{
		private static string FarmName()
		{
			return "Farm";
		}
		private static string FarmUnlockText()
		{
			return "You stumble across Whitney's farm again. The anthropomorphic canine woman gives you a friendly wave and tosses you another Canine pepper." + 
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You've been to the farm enough to easily find it." +
					" You can return by selecting it from the places menu (and will no longer encounter it during random lake exploration).", StringFormats.BOLD);
		}
	}
}
