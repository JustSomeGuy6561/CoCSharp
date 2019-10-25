using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.Monsters
{
	public class Monster : MonsterBase, IExtendedCreature
	{
		public ExtraPerkCollection extendedPerks => perks as ExtraPerkCollection;

		public Monster(MonsterCreator creator) : base(creator, (x) => new ExtraPerkCollection(x))
		{
		}
	}
}
