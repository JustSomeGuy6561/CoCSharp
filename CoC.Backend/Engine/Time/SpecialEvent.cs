//SpecialEvent.cs
//Description:
//Author: JustSomeGuy
//6/29/2019, 11:55 PM
using System;

namespace CoC.Backend.Engine.Time
{
	//base class for special events. a special event is a time-based event that gets its own scene. 
	public abstract class SpecialEvent
	{

		//this will be called when the scene is called. You MUST (eventually) call callMeWhenYourSceneIsDone
		//In some cases, you may have several pages of text, or may use other menus like inventory.
		//in these cases, i'd recommend storing the callMe variable internally, then making it your final action
		//basically, it's just a wrapper for DoNext(nextEvent). 

		//there may be cases where you want to do things based on what is going on, so booleans for if the current hour can be skipped 
		//and if there are any future hours that can be skipped (currentlyIdling and hasIdleHours, respectively), have been provided. 
		//for example, you may wish to cancel any additional hours set aside for idling, but only do it if you are currently idling. 
		//Note that the standard behavior is to never have idle hours and used hours mix, so both values would the same, but it is 
		//possible (though frowned upon? idk) for some unique scene to mix idle time and used time.  
		protected internal abstract void BuildInitialScene(bool currentlyIdling, bool hasIdleHours, Action callMeWhenYourSceneIsDone);
	}
}
