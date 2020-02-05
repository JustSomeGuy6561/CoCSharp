using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;

namespace CoC.Backend.Items
{
	public abstract class CapacityItem<T> : CapacityItem, IEquatable<T> where T : CapacityItem<T>
	{
		protected CapacityItem() : base() { }

		public override DisplayBase AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			return AttemptToUseSafe(target, (w, x, y, z) => postItemUseCallback(w, x, y, z));
		}

		//safe variant that ensures T->T when item is used. this allows us to enforce inventories that require a certain Base Item Type.
		//for example: the weapon rack will always have weapons; you can't pull a fast one and swap in a non-weapon when equipping a weapon off the rack.
		public virtual DisplayBase AttemptToUseSafe(Creature target, UseItemCallbackSafe<T> postItemUseCallbackSafe)
		{
			if (!CanUse(target, out string whyNot))
			{
				postItemUseCallbackSafe(false, whyNot, Author(), (T)this);
				return null;
			}
			else
			{
				T retVal = UseItem(target, out string resultsOfUse);
				postItemUseCallbackSafe(true, resultsOfUse, Author(), retVal);
				return null;
			}
		}


		/// <summary>
		/// Do any internal logic for using the item. if this item overrides AttemptToUseSafe, this can be ignored.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="resultsOfUseText">Text explaining the results of the current use. </param>
		/// <returns></returns>
		protected abstract T UseItem(Creature target, out string resultsOfUseText);

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
