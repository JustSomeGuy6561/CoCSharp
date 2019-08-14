using CoC.Backend.Tools;
using System;

namespace CoC.Frontend.Areas.Locations
{
	internal sealed partial class VolcanicCrag
	{
		private static string VolcanicCragName()
		{
			return "VolcanicCrag";
		}
		private static string VolcanicCragUnlock()
		{
			return "You walk for some time, roaming the hard-packed and pink-tinged earth of the demon-realm of Mareth. " +
				"As you progress, you can feel the air getting warm. It gets hotter as you progress until you finally stumble across a blackened landscape. " +
				"You reward yourself with a sight of the endless series of a volcanic landscape. Crags dot the landscape." + Environment.NewLine + Environment.NewLine +
				SafelyFormattedString.FormattedText("You've discovered the Volcanic Crag!", StringFormats.BOLD);
		}

	}
}
