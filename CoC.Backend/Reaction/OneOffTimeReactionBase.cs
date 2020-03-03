using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Reaction
{
	//Reactions are a helper, i guess - they're like one-time events, but you don't need to bother with adding them, waiting for them to proc, then removing them - it's automatically handled.
	//2 Types of Reactions: Time-Based, and Area-Based. A Time Reaction acts like a one-off Active Time Event. An Area Reaction (Place or Location) acts as a one-time Triggered Encounter. Both
	//can be set to trigger as soon as possible, or after a delay. In the case of a Time Reaction, you also have the option of fiddling with stats and such - normally, you're not supposed to do that
	//in a Special Event - it's just supposed to be text with a menu or something based on whatever happens - get an item, etc. Time Reactions are also given the highest priority, so if other Time Events
	//rely on stats being changed, they will see these changes.

	//Note that it is possible for something to trigger a Reaction multiple times before the reaction actually occurs - for example, a poorly coded segment could change the player's gender or body part type
	//multiple times before signalling that time passed, which could lead things that create a reaction on those changes to proc twice. it's therefore recommended to store a reference to any reaction, then
	//check if it's already in the reaction store before attempting to add it again. if it is already there, simply update it to use the new information. It may even be possible to get away with simply
	//checking to see if it's already in the reaction store, and adding it if it isn't if you capture the values from the source creature, though that requires knowledge of capturing values.

	//a reaction that occurs after a specific amount of time.
	public sealed class OneOffTimeReaction : IComparable<OneOffTimeReaction>
	{
		public readonly GameDateTime procTime;
		public readonly TimeReactionBase onProc;

		//fires immediately.
		public OneOffTimeReaction(TimeReactionBase doProc)
		{
			onProc = doProc ?? throw new ArgumentNullException(nameof(doProc));

			procTime = GameDateTime.Now;
		}

		//fires after a delay. if randomized is set to true, it'll occur at some point between now and the delay. Note that you should make this delay relatively small.
		public OneOffTimeReaction(TimeReactionBase doProc, byte delay, bool randomized = false)
		{
			onProc = doProc ?? throw new ArgumentNullException(nameof(doProc));

			procTime = GameDateTime.HoursFromNow(randomized ? (byte)Utils.Rand(delay) : delay);
		}

		public int CompareTo(OneOffTimeReaction other)
		{
			return procTime.CompareTo(other.procTime);
		}

		//public void UpdateReaction(Func<EventWrapper> doProc)
		//{
		//	onProc = doProc;
		//}
	}
}
