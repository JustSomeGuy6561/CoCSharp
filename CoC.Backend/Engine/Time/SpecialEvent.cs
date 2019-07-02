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
		protected internal abstract void BuildInitialScene(Action callMeWhenYourSceneIsDone);
	}
}
