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
			return "You walk for quite some time, roaming the hard-packed and pink-tinged earth of the demon-realm. " +
				"Rust-red rocks speckle the wasteland, as barren and lifeless as anywhere else you've been. A cool breeze suddenly brushes against your face, " +
				"as if gracing you with its presence. You turn towards it and are confronted by the lush foliage of a very old looking forest. " +
				"You smile as the plants look fairly familiar and non-threatening. Unbidden, you remember your decision to test the properties of this place, and think of your campsite as you walk forward. " +
				"Reality seems to shift and blur, making you dizzy, but after a few minutes you're back, and sure you'll be able to return to the forest with similar speed." +
				Environment.NewLine + Environment.NewLine + SafelyFormattedString.FormattedText("You have discovered the Forest!", StringFormats.BOLD);
		}
	}
}
