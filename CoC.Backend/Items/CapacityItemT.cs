using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;

namespace CoC.Backend.Items
{
	//Note: this class cannot be derived outside of the backend.
	public abstract class CapacityItem<T> : CapacityItem, IEquatable<T> where T : CapacityItem<T>
	{
		protected CapacityItem() : base() { }

		public DisplayBase UseItemSafe(Creature target, UseItemCallbackSafe<T> postItemUseCallback)
		{
			return AttemptToUseSafe(target, postItemUseCallback);
		}

		public DisplayBase UseItemInCombatSafe(CombatCreature target, CombatCreature opponent, UseItemCombatCallbackSafe<T> postItemUseCallback)
		{
			return AttemptToUseInCombatSafe(target, opponent, postItemUseCallback);
		}

		//safe variant that ensures T->T when item is used. this allows us to enforce inventories that require a certain Base Item Type.
		//for example: the weapon rack will always have weapons; you can't pull a fast one and swap in a non-weapon when equipping a weapon off the rack.
		private protected virtual DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<T> postItemUseCallbackSafe)
		{
			if (!CanUse(target, false, out string whyNot))
			{
				postItemUseCallbackSafe(false, whyNot, Author(), (T)this);
				return null;
			}
			else
			{
				T retVal = UseItemSafe(target, out string resultsOfUse);
				postItemUseCallbackSafe(true, resultsOfUse, Author(), retVal);
				return null;
			}
		}

		private protected virtual DisplayBase AttemptToUseInCombatSafe(CombatCreature user, CombatCreature opponent, UseItemCombatCallbackSafe<T> postItemUseCallbackSafe)
		{
			if (!CanUse(user, true, out string whyNot))
			{
				postItemUseCallbackSafe(false, false, whyNot, Author(), (T)this);
				return null;
			}
			else
			{
				T retVal = UseItemInCombatSafe(user, opponent, out bool causesLoss, out string resultsOfUse);
				postItemUseCallbackSafe(true, causesLoss, resultsOfUse, Author(), retVal);
				return null;
			}
		}


		private protected override DisplayBase AttemptToUseItem(Creature target, UseItemCallback postItemUseCallback)
		{
			return AttemptToUseSafe(target, (w, x, y, z) => postItemUseCallback(w, x, y, z));
		}

		private protected override DisplayBase AttemptToUseItemInCombat(CombatCreature user, CombatCreature opponent, UseItemCombatCallback postItemUseCallback)
		{
			return AttemptToUseInCombatSafe(user, opponent, (v, w, x, y, z) => postItemUseCallback(v, w, x, y, z));
		}

		private protected abstract T UseItemSafe(Creature target, out string resultsOfUseText);
		private protected virtual T UseItemInCombatSafe(CombatCreature user, CombatCreature opponent, out bool resultsInLoss, out string resultsOfUseText)
		{
			resultsInLoss = false;
			return UseItemSafe(user, out resultsOfUseText);
		}

		private protected override CapacityItem UseItem(Creature target, out string resultsOfUseText)
		{
			return UseItemSafe(target, out resultsOfUseText);
		}

		private protected override CapacityItem UseItemInCombat(CombatCreature target, CombatCreature opponent, out bool resultsInLoss, out string resultsOfUseText)
		{
			return UseItemInCombatSafe(target, opponent, out resultsInLoss, out resultsOfUseText);
		}

		public override bool Equals(CapacityItem other)
		{
			if (other is T tType)
			{
				return this.Equals(tType);
			}
			else
			{
				return false;
			}
		}

		public abstract bool Equals(T other);

	}
}
