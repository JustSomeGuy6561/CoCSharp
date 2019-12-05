//AreaBase.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:37 PM
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;

namespace CoC.Backend.Areas
{
	public abstract class AreaBase
	{
		private protected static Func<DisplayBase> GetCurrentDisplay;

		internal static void SetPageMaker(Func<DisplayBase> currentDisplayCallback)
		{
			GetCurrentDisplay = currentDisplayCallback ?? throw new ArgumentNullException(nameof(currentDisplayCallback));
		}

		public readonly SimpleDescriptor name;



		private protected AreaBase(SimpleDescriptor areaName)
		{
			name = areaName ?? throw new ArgumentNullException();
		}

		//When the area engine runs, it checks to see if you were last at this location, and updates internal values accordingly. if you need to
		//manually update values for your area based on whether or not the player is just reaching this area or returning to it after doing something,
		//you may override OnEnter and OnStay

		/// <summary>
		/// Returns a new page with the required content and buttons for running this area.
		/// </summary>
		/// <returns></returns>
		internal abstract void RunArea();

		//Triggers when an area is entered from another area. Allows you to implement any custom logic your area needs when this occurs. Internally,
		//the game will automatically increment timesVisited, if applicable.
		protected internal virtual void OnEnter() { }

		//triggers when an area is to run, but it came from the same area. (Think of Tel Adre, for example). Allows you to implement any custom logic your area needs when this occurs.
		protected internal virtual void OnStay() { }
	}
}
