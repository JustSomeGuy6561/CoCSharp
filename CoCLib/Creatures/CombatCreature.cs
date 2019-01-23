using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Creatures
{
	//class defining combat traits for creatures that do combat. 
	public abstract class CombatCreature : Creature
	{
		protected CombatCreature(string creatureName) : base(creatureName)
		{

		}

		public abstract void InitCombat(out int level, out int experience, out Weapon weapon, out Armor armor, out Shield shield, out Jewelry jewelry, out UpperGarment upperGarment, out LowerGarment lowerGarment);
		public abstract void InitStats(out float strength, out float toughness, out float speed, out float intelligence, out float lust, out float sensitivity, out float libido, out float corruption, out float money);

		public virtual float level { get; protected set; }
		public virtual float experience { get; protected set; }
		public virtual float strength { get; protected set; }
		public virtual float toughness { get; protected set; }
		public virtual float speed { get; protected set; }
		public virtual float intelligence { get; protected set; }
		public virtual float corruption { get; protected set; }
		public virtual float hp { get; protected set; }
		public virtual float lust { get; protected set; }
		public virtual float fatigue { get; protected set; }
		public virtual float satiety { get; protected set; }
	}
}
