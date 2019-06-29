using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.SpecialInteraction
{
	//body part variant of time listeners. these have a unique bool so different actions can be taken if it's not the player. 
	//ideally, we could just assume that non-player characters don't need to care, or won't exist long enough for them to matter,
	//and therefore just never attach time events for NPCs. but in the event RNG-NPCs are a thing and we want them to obey the laws of
	//the game, here we are.

	//Additionally, because body parts are relatively small, they WILL NOT go on their own page. If you have some crazy event that 
	//only applies to one body part, and ends in a game over or some shit, take care of that on your own. This also means we use strings, not OutputWrappers.

	internal interface IBodyPartTimeLazy
	{
		bool reactToTimePassing(bool isPlayer, byte hoursPassed, out string output);
	}

	internal interface IBodyPartTimeActive
	{
		bool reactToHourPassing(bool isPlayer, out string output);
	}

	internal interface IBodyPartTimeDaily
	{
		byte hourToTrigger { get; }

		bool reactToDailyTrigger(bool isPlayer, out string output);
	}

	internal interface IBodyPartTimeDayMulti
	{
		byte[] triggerHours { get; }
		bool reactToTrigger(bool isPlayer, byte currHour, out string output);
	}
}
