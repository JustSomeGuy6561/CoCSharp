using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.UI
{
	/// <summary>
	/// Wraps a return value of either a string or a full page. Different from a special event in that this is created as a result of something, whereas a special event
	/// generates its values based on external factors, on demand. Note that this could be used as the backing for a dynamic special event, if doing so would not invalidate the result.
	/// </summary>
	public sealed class DisplayWrapper
	{
		public readonly string simpleReaction;

		public readonly DisplayBase fullPageReaction;

		public readonly bool isSimpleReaction;

		public DisplayWrapper(string simpleAction)
		{
			simpleReaction = simpleAction;
			isSimpleReaction = true;
			fullPageReaction = null;
		}

		public DisplayWrapper(DisplayBase fullPage)
		{
			fullPageReaction = fullPage ?? throw new ArgumentNullException(nameof(fullPage));
			isSimpleReaction = false;
			simpleReaction = null;
		}

		public static DisplayWrapper GenerateFullOrEmpty(DisplayBase fullPage)
		{
			if (fullPage is null)
			{
				return Empty;
			}
			else
			{
				return new DisplayWrapper(fullPage);
			}
		}

		public bool isEmpty => isSimpleReaction && string.IsNullOrEmpty(simpleReaction);

		public static bool IsNullOrEmpty(DisplayWrapper dataWrapper)
		{
			return dataWrapper is null || dataWrapper.isEmpty;
		}

		public static DisplayWrapper Empty => new DisplayWrapper(simpleAction: null);
	}
}
