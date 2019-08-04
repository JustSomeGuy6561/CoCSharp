//ButtonData.cs
//Description:
//Author: JustSomeGuy
//6/10/2019, 7:52 PM
using CoC.Backend;
using CoC.Backend.Strings;
using System;
using System.Collections.ObjectModel;

namespace CoC.Frontend.UI.ControllerData
{
	//Button Data, Like Output, is generated every time. We don't need to know when it updates - we assume it never updates unless language changes.
	//In the event of a language change, we're already redoing the GUI, so this isn't a problem. 

	/// <summary>
	/// 
	/// </summary>
	public sealed class ButtonData
	{
		public const byte MAX_BUTTON_TEXT_LEN = 12;

		private readonly SimpleDescriptor titleStr;
		private readonly SimpleDescriptor tooltipStr;
		private readonly SimpleDescriptor tooltipTitleStr;

		public string title => titleStr().Trim().Truncate(MAX_BUTTON_TEXT_LEN);
		public string tooltip => tooltipStr().Trim();
		public string tooltipTitle => string.IsNullOrWhiteSpace(tooltip) ? "" : string.IsNullOrWhiteSpace(tooltipTitleStr()) ? title : tooltipTitleStr();

		public readonly bool enabled;
		public readonly Action onClick;

		internal ButtonData(SimpleDescriptor desc, bool active, Action callback)
		{
			titleStr = desc ?? throw new ArgumentNullException();
			tooltipStr = GlobalStrings.None;
			tooltipTitleStr = GlobalStrings.None;
			enabled = active;
			onClick = callback;
		}

		internal ButtonData(SimpleDescriptor desc, bool active, Action callback, SimpleDescriptor tip, SimpleDescriptor replacementTitle)
		{
			titleStr = desc ?? throw new ArgumentNullException();
			tooltipStr = tip ?? GlobalStrings.None;
			tooltipTitleStr = replacementTitle ?? GlobalStrings.None;
			enabled = active;
			onClick = callback;
		}

		public static ButtonData NextButtonBecauseWereAssholes(Action callback)
		{
			return new ButtonData(GlobalStrings.NEXT_PAGE, true, callback);
		}

		public static ButtonData PrevButtonBecauseWereAssholes(Action callback)
		{
			return new ButtonData(GlobalStrings.PREVIOUS_PAGE, true, callback);
		}
	}
}
