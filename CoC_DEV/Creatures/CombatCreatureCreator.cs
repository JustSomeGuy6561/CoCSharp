using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Creatures
{
	internal abstract class CombatCreatureCreator : GenericCreatureCreator
	{
		public int? level;
		public float? experience;
		public float? strength;
		public float? toughness;
		public float? speed;
		public float? intelligence;
		public float? corruption;
		public float? hp;
		public float? lust;
		public float? fatigue;
		public float? satiety;
		public int? gems; //generally i'd say this is just for PC, but considering monsters take your money when you lose, maybe they have money too.

		public LowerGarment lowerGarment;
		public UpperGarment upperGarment;
		public Armor armor;
		public Weapon weapon;
		public Shield shield;

		public CombatCreatureCreator(Species race) : base(race)	{}
	}
}
