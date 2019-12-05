using CoC.Backend.Areas;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	//Farm is frontend, so creatures that can be sent to farm must implement an interface defined in the frontend. Not enforced here.
	public interface ICampNPC
	{
		string Name();

		//describes what this npc is doing at the current base and current hour.
		string HomeBaseIdleDescription(HomeBaseBase currentBase, byte currentHour);
		//describes where this npc sleeps/eats/whatever at the current base.
		string HomeBaseDomainDescription(HomeBaseBase currentBase, byte currentHour);
	}
	public interface ICampSlaveCreature : ICampNPC
	{
		Action OnSelect { get; }
	}

	public interface ICampFollowerCreature : ICampNPC
	{
		Action OnSelect { get; }
	}

	public interface ICampLoverCreature : ICampNPC
	{
		Action OnSelect { get; }
	}
}
