using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{


	public abstract class CapacityItem
	{
		public readonly SimpleDescriptor shortName;
		public readonly SimpleDescriptor fullName;
		public readonly SimpleDescriptor description;

		protected CapacityItem(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor desc)
		{
			this.shortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
			this.fullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
			description = desc;
		}

		public abstract byte maxCapacityPerSlot { get; }

		/// <summary>
		/// Checks to see if the given creature can use this item. Note that just because an item can be used doesn't mean it will ultimately succeed, i.e. if the player cancels it.
		/// </summary>
		/// <param name="target">The creature attempting to use this item.</param>
		/// <returns>true if the creature can use this item, false otherwise.</returns>
		public abstract bool CanUse(Creature target);
		/// <summary>
		/// The explanation for why the CanUse function on the given creature fails. If it should succeed, the behavior is not defined. 
		/// </summary>
		/// <param name="creature">The creature attempting to use this item</param>
		/// <returns>a description explaining why it cannot be used.</returns>
		public abstract string CantUseExplanation(Creature creature);

		public abstract void AttemptToUse(Creature target, UseItemCallback postItemUseCallback);

		//how much it costs to buy this item. Generally, the sell price is 1/2 it's value because video game capitalism. Items that cannot be bought should be given a price of 0;
		protected abstract int monetaryValue { get; }

		public int buyPrice => Math.Max(monetaryValue, 0);

		//by default, items that can be bought are given a positive value. you may override this if you want an item that can be sold and thus has a price, 
		public virtual bool canBuy => buyPrice > 0;
		public virtual bool canSell => true;

	}
}
