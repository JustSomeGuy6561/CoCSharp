//DisplayManager.cs
//Description:
//Author: JustSomeGuy
//Note: date follows MMDDYYYY format.
//10/14/2019 4:28:15 PM

using CoC.Backend.UI;
using System;

namespace CoC.Frontend.UI
{
	public static class DisplayManager
	{
		private static StandardDisplay currentDisplay = new StandardDisplay(); //initialize it or it fails, lol.
		private static bool displayChanged = true;

		internal static void LoadDisplay(DisplayBase display)
		{
			if (display is null) throw new ArgumentNullException(nameof(display));
			if (!ReferenceEquals(display, currentDisplay))
			{
				currentDisplay = (StandardDisplay)display;
				displayChanged = true;
			}
		}

		internal static void LoadDisplay(StandardDisplay display)
		{
			if (!ReferenceEquals(display, currentDisplay))
			{
				currentDisplay = display;
				displayChanged = true;
			}
		}

		internal static StandardDisplay GetCurrentDisplay()
		{
			return currentDisplay;
		}

		internal static bool QueryDisplay(out StandardDisplay display)
		{
			var changed = displayChanged;
			display = currentDisplay;
			displayChanged = false;
			return changed;
		}

	}
}
