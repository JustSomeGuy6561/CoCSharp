using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine
{
	public interface ITimeListener
	{
		//if there's any instance that would require more than 10 days, 15 hours, lmk. we'll make this a ushort.
		void ReactToTimePassing(byte hoursPassed);
	}

	public interface ITimeListenerWithOutput : ITimeListener
	{
		//This will be checked immediately after ReactToTimePassing. It is recommended to have this getter refer to a local variable that you can set in your class,
		//unless your time aware will always require output. I can't think of a reason for this to occur. If it never outputs, use the base ITimeAware.
		//It is YOUR RESPONSIBILITY to make sure these are not outputting anything when you dont want them to, and that they are outputting correctly when they should.
		//Therefore, it is highly recommended that you always set whatever local variable this getter refers to to false as your first step in ReactToTimePassing. set it to true when the need arises.

		//For Example: Womb has a local (private) bool, needsOutput, and RequiresOutput refers to it. Womb's ReactToTimePassing sets needsOutput to false as it's first step. It then updates itself according to
		//the time passed, if applicable. If a condition is met, such as the water breaking, it determines it needs to output text, so it sets needsOutput to true. It then sets other local variables so Output()
		//returns "Your water broke" or whatever the flavor text actually is.
		bool RequiresOutput { get; } 

		//only is checked if RequiresOutput returns true. see above comments.
		string Output();
	}
}
