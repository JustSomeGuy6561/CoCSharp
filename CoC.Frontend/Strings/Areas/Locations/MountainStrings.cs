using CoC.Backend;
using CoC.Backend.Areas;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Areas.Locations
{
	internal partial class Mountain : LocationBase
	{
		private static string MountainName()
		{
			return "Mountain";
		}

		private static string MountainMimic()
		{
			return "Thunder rumbles overhead ";
		}

		private static string MountainUnlock()
		{
			return "Thunder booms overhead, shaking you out of your thoughts. High above, dark clouds encircle a distant mountain peak. " +
				"You get an ominous feeling in your gut as you gaze up at it." + Environment.NewLine + Environment.NewLine +
				SafelyFormattedString.FormattedText("You've discovered the Mountain!", StringFormats.BOLD);
		}
	}
}
