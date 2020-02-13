using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items.Wearables
{
	//Note: it's possible, though unlikely, for a wearable item to use a menu (to adjust settings or characteristics of that wearable item)
	//if you do this, it will require overriding a lot of stuff manually.

	public abstract class WearableItemBase<T> : CapacityItem<T> where T:WearableItemBase<T>
	{
		//the actual defense this grants, when worn by the given creature. also takes into account any i
		public abstract float DefensiveRating(Creature wearer);

		protected WearableItemBase() : base()
		{
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

		/// <summary>
		/// Equip the item. At this point, CanUse has been called. It's assumed that this will always succeed at this point.
		/// Additionally, apply any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature equipping this wearable item.</param>
		/// <returns>The wearable item this object replaces, or null if none exists.</returns>
		protected abstract T EquipItem(Creature wearer, out string equipOutput);

		protected override T UseItem(Creature target, out string resultsOfUseText)
		{
			return EquipItem(target, out resultsOfUseText);
		}

		/// <summary>
		/// Remove this item. this cannot fail. update any perks, reactions, and/or status effects as necessary.
		/// </summary>
		/// <param name="wearer">The creature that is removing this item</param>
		protected internal virtual void OnRemove(Creature wearer) { }

		protected internal virtual string OnRemoveText()
		{
			return "You remove your " + this.ItemName();
		}

		public override byte maxCapacityPerSlot => 1;

		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			return CanWearWithBodyData(target, out whyNot);
		}

		//a variation of the about item text, this time with any applicable stats the wearable item has (like armor rating, sexiness, etc).
		public abstract string AboutItemWithStats(Creature wearer);

		protected string DefenseDifference(T other)
		{
			throw new NotImplementedException();
		}


		protected string GenericEquipText(Creature wearer)
		{
			return "You equip your " + ItemDescription(1, false);
		}

		//if your item gets destroyed on removal, set this to true. afaik this is only used in one spot.
		protected internal virtual bool destroyOnRemoval => false;
	}
}
