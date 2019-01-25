//ITimeAware.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 1:59 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine
{
#warning add the time system and subscription service, then fix this comment.
	//remember, even if a class implements this interface, unless it subsribes to the time system (NYI, TBD) up, it wont be called.
	//this is useful when dealing with body parts on enemies, who frankly won't exist in memory long enough to matter
	//so why bother even adding them to the list of things to update every hour?
	//but it also means NPCs who have pregnancies or whatever need to subsribe. 
	internal interface ITimeAware
	{
		//generally will be called every hour, but certain actions can cause more than one hour
		//to progress before the player can be made aware of it (generally sex or combat loss)
		void ReactToTimePassing(uint hoursPassed);
	}
}
