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
		public abstract int timesExplored { get; protected internal set; }

		internal abstract void RunArea();

		//When the area engine runs, it checks to see if you were last at this location, and updates internal values accordingly. if you need to 
		//manually update values for your area based on whether or not the player is just reaching this area or returning to it after doing something,
		//you may override OnEnter and OnStay

		//Triggers when an area is visited from another area. The game will automatically update timesExplored;
		protected internal virtual void OnEnter() {}

		//triggers when an area is to run, but it came from the same area. (Think of Tel Adre, for example).
		protected internal virtual void OnStay() {}


#warning Should make this virtual later.
		public abstract void Unlock();


		public bool Is<T>()
		{
			return this is T;
		}
	}
}
