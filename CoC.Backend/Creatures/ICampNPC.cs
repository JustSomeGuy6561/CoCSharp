using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	//Farm is frontend, so creatures that can be sent to farm must implement an interface defined in the frontend. Not enforced here. 
	public interface ICampNPC
	{
		//the "default" description for the npc when they're idleing about in camp. each home base has the option to override this default text with text more suited
		//to that specific home base. for example, if you're in ingnam because grimdark, you might not have a river nearby so creatures that require it may have different
		//text. Of course, how that's actually implemented is up to the devs - they could implement it in the NPC, and in the home base attempt to cast the creature to
		//that specific NPC and then simply return its Ingnam idle text, or they could implement it directly in the Ingnam home base class. 
		SimpleDescriptor idleBaseDescription(byte currentHour);
	}
	interface ICampSlaveCreature : ICampNPC
	{
		Action OnSelect { get; }
	}

	interface ICampFollowerCreature : ICampNPC
	{
		Action OnSelect { get; }
	}

	interface ICampLoverCreature : ICampNPC
	{
		Action OnSelect { get; }
	}
}
