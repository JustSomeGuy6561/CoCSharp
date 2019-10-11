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

		//this will be called when the scene is called. To work properly, 
		protected internal abstract void BuildInitialScene(bool currentlyIdling, bool hasIdleHours, Action callMeWhenYourSceneIsDone);
	}
}
