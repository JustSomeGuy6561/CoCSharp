//MenuHelpers.cs
//Description:
//Author: JustSomeGuy
//6/7/2019, 12:52 AM
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;
using static CoC.Frontend.UI.ButtonData;
namespace CoC.Frontend.Engine
{
	//buttons should be automatically cleared whenever a scene changes. the dev must provide these. 
	//before some places had weird edge case cleanup to deal with this, and it was spaghetti.


	public static class MenuHelpers
	{
		public static bool ButtonPreviousPage(Action callback, byte location = 10)
		{
			return AddButton(location, PREV_PAGE(), callback);
		}
		public static bool ButtonNextPage(Action callback, byte location = 11)
		{
			return AddButton(location, NEXT_PAGE(), callback);
		}
		public static void DoNext(Action callback)
		{
			ClearButtons();
			AddButton(0, GlobalStrings.NEXT(), callback);
		}

		public static void DoYesNo(Action yesCallback, Action noCallback)
		{
			ClearButtons();
			AddButton(0, GlobalStrings.YES(), yesCallback);
			AddButton(1, GlobalStrings.NO(), noCallback);
		}


		private static string NEXT_PAGE()
		{
			return "Next Page";
		}

		private static string PREV_PAGE()
		{
			return "Prev Page";
		}
	}
}
