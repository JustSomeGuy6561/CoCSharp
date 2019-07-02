//Monster.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:11 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	public class Monster : CombatCreature
	{
		public Monster(MonsterCreator creator) : base(creator)
		{
		}
	}
}
