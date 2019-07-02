//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.BodyParts;

namespace CoC.Backend.Creatures
{

	public sealed class Player : CombatCreature
	{

		public Player(PlayerCreator creator) : base(creator)
		{
			//now set up all the listeners.
			//if any listeners are player specifc, and i mean really player specific, add them here.

			//then activate them. 
			//occurs AFTER the creature constructor, so we're fine.
			ActivateTimeListeners();

			//TODO: Add player specific items or whatever.
		}

	}
}