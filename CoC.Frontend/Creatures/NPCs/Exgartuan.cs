using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Frontend.Creatures.NPCs
{
	class Exgartuan
	{
		public static bool possessingPlayer;
		public static bool malePossession;
		public static bool femalePossession => possessingPlayer && !malePossession;

		internal static string BeeHoneyBlock(Creature target)
		{
			return "You uncork the bottle only to hear Exgartuan suddenly speak up. <i>\"Hey kid, this beautiful cock here doesn't need any of that special bee shit. " +
				"Cork that bottle up right now or I'm going to make it so that you can't drink anything but me.\"</i> You give an exasperated sigh " +
				"and put the cork back in the bottle.";
		}
	}
}
