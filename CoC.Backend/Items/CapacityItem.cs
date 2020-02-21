using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Items
{

	//Note: this class cannot be derived outside of the backend.
	//By default, this assumes any capacity item will not allow a menu. you can override this to allow menus.
	public abstract class CapacityItem : IEquatable<CapacityItem>
	{
		protected const int DEFAULT_VALUE = 6;

		//unlike the original, which put all of the data in the constructor, C# doesn't let us pass along function pointers to the base that use data from the current object
		//because the current object doesn't technically exist yet - the functions need to be static. personally, i'm not a fan of this approach, but there's nothing i can
		//do about it except some crazy workarounds that are just plain ugly. so, these are all abstract function calls. this lets us have variables unique to each object
		//factor in to their descriptions (so, if an armor has some unique perk or trait like durability or something, we can actually describe that), or lets us create 'enhanced'
		//items that use the same source but have different values and text. Its possible for people to misuse this, i guess, because it's easier to make the description mutable
		//(i.e. can change between function calls), but whatever.

		protected CapacityItem() { }

		public abstract string AbbreviatedName();

		public abstract string ItemName();

		public abstract string ItemDescription(byte count = 1, bool displayCount = false);

		public abstract string AboutItem();


		public abstract byte maxCapacityPerSlot { get; }

		protected abstract int monetaryValue { get; }

		public int buyPrice => Math.Max(monetaryValue, 0);

		public virtual bool canBuy => buyPrice > 0;
		public virtual bool canSell => true;

		public virtual string Author() => "";

		public abstract bool Equals(CapacityItem other);

		public abstract bool CanUse(Creature target, bool currentlyInCombat, out string whyNot);

		public DisplayBase UseItem(Creature target, UseItemCallback postItemUseCallback)
		{
			return AttemptToUseItem(target, postItemUseCallback);
		}

		public DisplayBase UseItemInCombat(CombatCreature target, UseItemCombatCallback postItemUseCallback)
		{
			return AttemptToUseItemInCombat(target, postItemUseCallback);
		}

		private protected virtual DisplayBase AttemptToUseItem(Creature target, UseItemCallback postItemUseCallback)
		{
			if (!CanUse(target, false, out string whyNot))
			{
				postItemUseCallback(false, whyNot, Author(), this);
				return null;
			}
			else
			{
				CapacityItem retVal = UseItem(target, out string resultsOfUse);
				postItemUseCallback(true, resultsOfUse, Author(), retVal);
				return null;
			}
		}
		private protected virtual DisplayBase AttemptToUseItemInCombat(CombatCreature target, UseItemCombatCallback postItemUseCallback)
		{
			if (!CanUse(target, true, out string whyNot))
			{
				postItemUseCallback(false, false, whyNot, Author(), this);
				return null;
			}
			else
			{
				CapacityItem retVal = UseItemInCombat(target, out bool causesLoss, out string resultsOfUse);
				postItemUseCallback(true, causesLoss, resultsOfUse, Author(), retVal);
				return null;
			}
		}

		//Expose these to the end user where useful. I've made whatever i can private protected to hide it from the end user. the less they see
		//(while still being able to do everything) the better
		private protected abstract CapacityItem UseItem(Creature target, out string resultsOfUseText);
		private protected virtual CapacityItem UseItemInCombat(CombatCreature target, out bool resultsInLoss, out string resultsOfUseText)
		{
			resultsInLoss = false;
			return UseItem(target, out resultsOfUseText);
		}

	}
}
