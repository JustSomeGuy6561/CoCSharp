using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Reaction;

namespace CoC.Backend.Items.Wearables
{
	public interface IWearableDailyFull
	{
		byte hourToTrigger { get; }

		TimeReactionBase ReactToDailyTrigger(Creature wearer);
	}

	public interface IWearableDailySimple
	{
		byte hourToTrigger { get; }

		string reactToDailyTrigger(Creature wearer);
	}
}
