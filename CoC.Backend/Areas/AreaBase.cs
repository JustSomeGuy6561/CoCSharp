//AreaBase.cs
//Description:
//Author: JustSomeGuy
//4/7/2019, 7:37 PM
using System;
using System.Collections.Generic;

namespace CoC.Backend.Areas
{
	public abstract class AreaBase
	{
		public readonly SimpleDescriptor name;

		private protected AreaBase(SimpleDescriptor areaName)
		{
			name = areaName ?? throw new ArgumentNullException();
		}

		//When the area engine runs, it checks to see if you were last at this location, and updates internal values accordingly. if you need to 
		//manually update values for your area based on whether or not the player is just reaching this area or returning to it after doing something,
		//you may override OnEnter and OnStay

		//Runs the current area normally. Internally, the current area is checked for any special reactions that may be associated with it, and if none exist,
		//calls this function. OnEnter or OnStay will be called before RunArea is called. 
		internal abstract void RunArea();

		//Triggers when an area is entered from another area. Allows you to implement any custom logic your area needs when this occurs. Internally, 
		//the game will automatically increment timesVisited, if applicable. 
		protected internal virtual void OnEnter() { }

		//triggers when an area is to run, but it came from the same area. (Think of Tel Adre, for example). Allows you to implement any custom logic your area needs when this occurs. 
		protected internal virtual void OnStay() { }
	}
}
