using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables
{
	//Note: it's possible, though unlikely, for a wearable item to use a menu (to adjust settings or characteristics of that wearable item)
	//if you do this, it will require overriding a lot of stuff manually.

	public abstract class WearableItemBase<T> : CapacityItem<T> where T : WearableItemBase<T>
	{
		protected WearableItemBase() : base()
		{
		}

		//Backend Exclusive.
		private protected abstract T UpdateCreatureEquipmentInternal(Creature target);

		//this is exposed to the end user via change equipment. as a result, wearable sub groups (armor, etc) are able to expose the full menu system manually
		//by calling change equipment in place of use item safe. this also lets members override it and not need to be in this project.
		private protected override T UseItemSafe(Creature target, out string resultsOfUseText)
		{
			return ChangeEquipment(target, out resultsOfUseText);
		}

		//values that negate gains of damage. these are flat values.
		public abstract double PhysicalDefensiveRating(Creature wearer);
		public virtual double MagicalDefensiveRating(Creature wearer) => 0;
		public virtual double LustDefensiveRating(Creature wearer) => 0;


		//values that affect the wearer's changes in stats over time. these add to multiplier values. 0.1 represents a 10% increase, whereas -0.1 a 10% decrease. defaults to 0.
		public virtual double BonusHealingMultiplier(Creature wearer) => 0; //health gain from all sources.
		public virtual double LustGainOffset(Creature wearer) => 0; //lust gain over time.


		//Also a new feature, but more or less to clean up old code. range from 1 to 100.
		//This is before any perks or such the creature has that may enhance or lessen the effectiveness. The total evasion rate is determined by adding the values granted
		//from all the equipment, along with a modified value from the creature's speed. heavier weapons, armor, and shields will lower this, though the effect can be lessened
		//if the player is sufficiently strong enough. This value is also capped based on the level of the creature and their opponent and game difficulty.

		//Note that some perks may choose to grant the user additional opportunities to evade an attack that do not affect this rate. if so, they must provide a default 'miss'
		//text. some may grant the user a second evade roll or something of the sort as well.
		//some attacks may bypass evasion entirely.
		public virtual double EvasionRate(Creature wearer) => 0;

		//some items also get a block rate. note that blocks are not the same as evades; a block may still stun the player, for example, and generally increases fatigue.

		//Capped at 5. semi-new. now can be applied by more than just undergarments.
		public virtual double BonusTeaseRate(Creature wearer) => 0;
		//capped at 10. semi-new. now can be applied by more than just armor.
		public virtual double BonusTeaseDamage(Creature wearer) => 0;

		//a variation of the about item text, this time with any applicable stats the wearable item has (like armor rating, sexiness, etc).
		public abstract string AboutItemWithStats(Creature wearer);

		public override byte maxCapacityPerSlot => 1;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			return CanWearWithBodyData(target, out whyNot);
		}

		protected T UpdateCreatureEquipment(Creature target)
		{
			return UpdateCreatureEquipmentInternal(target);
		}

		protected T ChangeEquipment(Creature wearer, out string equipOutput)
		{
			T oldEquipment = UpdateCreatureEquipment(wearer);
			T retVal = DoRemove(wearer, out string removeText);

			DoEquip(wearer, out string equipText);

			equipOutput = removeText + equipText;

			return retVal;
		}

		protected internal void DoEquip(Creature wearer, out string equipText)
		{
			OnEquip(wearer);
			equipText = EquipText(wearer);
		}
		//removing cannot fail. this allows armor to be removed without requiring another item be used to replace it.
		//this is marked protected internal because it is needed internally to remove items without the full item system hullabaloo.
		protected internal T DoRemove(Creature wearer, out string removeText)
		{
			T retVal = OnRemove(wearer);
			removeText = RemoveText(wearer);

			return retVal;
		}

#warning Figure out best way to handle body changes that cause this wearable to no longer be equipable. the easiest way is to simply fire off a reaction
		//when the player's body data changes.

		//by default, we assume the wearable will work regardless of current creature's form. if your item will not work with certain creature body parts, override this.
		//(notable examples: some items won't work with monopeds or quadrupeds (read: nagas and centaurs). it's also possible for other things like tails and wings to interfere)
		protected virtual bool CanWearWithBodyData(Creature creature, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//if you need to do complex things
		protected virtual void OnEquip(Creature wearer) { }
		protected virtual string EquipText(Creature wearer)
		{
			return GenericEquipText(wearer);
		}


		protected virtual T OnRemove(Creature wearer)
		{
			return (T)this;
		}

		protected virtual string RemoveText(Creature wearer)
		{
			return GenericRemoveText(wearer);
		}

		//Generic Texts

		protected string DefenseDifference(T other)
		{
			throw new NotImplementedException();
		}

		protected string GenericEquipText(Creature wearer)
		{
			return "You equip your " + ItemDescription(1, false);
		}

		protected string GenericRemoveText(Creature wearer)
		{
			return "You remove your " + ItemDescription(1, false);
		}
	}
}
