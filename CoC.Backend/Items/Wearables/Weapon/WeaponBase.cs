using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Shield;

namespace CoC.Backend.Items.Wearables.Weapon
{
	public abstract class WeaponBase : WearableItemBase<WeaponBase>
	{
		//damage type: physical, magical, mixed, true.
		//attack type: projectile, melee

		public abstract double BlockRate(Creature wearer);

		public abstract double BaseDamage(Creature wearer);

		//replaces the old 'verb' text. this is the text for performing the attack; it does not actually describe the attack hitting (or missing) the target.
		//this is a little more flexible because you can say "You fire your flintlock pistol at <the enemy>" or "<you> slash at <the enemy> with your sword"
		//this also lets us use the weapon for enemies - "She slashes" for example, instead of "she slash", which makes no sense. this does require more work for implementers though :(
		protected abstract string PerformAttackText(CombatCreature attacker, CombatCreature defender);



		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			if (!base.CanUse(target, currentlyInCombat, out whyNot))
			{
				return false;
			}
			else if (!CanUseWithShield(target, target.shield, out whyNot))
			{
				return false;
			}
			else
			{
				whyNot = null;
				return true;
			}
		}

		public virtual bool CanUseWithShield(Creature wearer, ShieldBase shield, out string whyNot)
		{
			whyNot = null;
			return true;
		}

	}
}
