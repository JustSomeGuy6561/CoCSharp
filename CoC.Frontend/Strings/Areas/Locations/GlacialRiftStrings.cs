//GlacialRiftStrings.cs
//Description:
//Author: JustSomeGuy
//4/6/2019, 12:24 AM
using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class GlacialRift
	{
		private static string GlacialRiftName()
		{
			return "GlacialRift";
		}
		private static string GlacialRiftUnlock()
		{
			return "You walk for some time, roaming the hard-packed and pink-tinged earth of the demon-realm of Mareth. As you progress, " +
				"a cool breeze suddenly brushes your cheek, steadily increasing in intensity and power until your clothes are whipping around your body in a frenzy. " +
				"Every gust of wind seems to steal away part of your strength, the cool breeze having transformed into a veritable arctic gale. " +
				"You wrap your arms around yourself tightly, shivering fiercely despite yourself as the hard pink dirt slowly turns to white; " +
				"soon you’re crunching through actual snow, thick enough to make you stumble with every other step. You come to a stop suddenly as the ground before you " +
				"gives way to a grand ocean, many parts of it frozen in great crystal islands larger than any city." + Environment.NewLine + Environment.NewLine + 
				SafelyFormattedString.FormattedText("You've discovered the Glacial Rift!", StringFormats.BOLD);
		}

	}
}
