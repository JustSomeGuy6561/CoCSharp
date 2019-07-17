//Player.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:15 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using System;

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

		public override uint maxHealth
		{
			get
			{
				double max = 50;
				max += toughness * 2;
				max += perks.baseModifiers.bonusMaxHP;
				max += GameEngine.difficulties[BackendSessionData.data.difficulty].basePlayerHP(level);
				if (max > 9999)
				{
					return 9999;
				}
				else if (max < 50)
				{
					return 50;
				}
				return (uint)max;
			}
		}
	}
}