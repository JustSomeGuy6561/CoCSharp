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
