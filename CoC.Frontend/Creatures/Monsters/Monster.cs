﻿using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.Monsters
{
	public class Monster : MonsterBase, IExtendedCreature
	{
		public Monster(MonsterCreator creator) : base(creator)
		{
			extendedPerkModifiers = new ExtendedPerkModifiers(this);
			extendedData = new ExtendedCreatureData(this, extendedPerkModifiers);
		}

		public ExtendedCreatureData extendedData { get; }

		public ExtendedPerkModifiers extendedPerkModifiers { get; }
	}
}
