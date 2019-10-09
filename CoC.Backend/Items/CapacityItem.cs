using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{
	public delegate void UseItemCallback(bool successfullyUsedItem, CapacityItem replacementItem);


	public abstract class CapacityItem
	{
		public readonly SimpleDescriptor shortName;
		public readonly SimpleDescriptor fullName;

		protected CapacityItem(SimpleDescriptor shortName, SimpleDescriptor fullName)
		{
			this.shortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
			this.fullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
		}

		public abstract byte maxCapacityPerSlot { get; }

		public abstract bool CanUse(Creature target);

		public abstract void AttemptToUse(Creature target, UseItemCallback postItemUseCallback);

		//how much it costs to buy this item. Generally, the sell price is 1/2 it's value because video game capitalism. Items that cannot be bought should be given a price of 0;
		protected abstract int monetaryValue { get; }

		public int buyPrice => Math.Max(monetaryValue, 0);

		//by default, items that can be bought are given a positive value. you may override this if you want an item that can be sold and thus has a price, 
		public virtual bool canBuy => buyPrice > 0;
		public virtual bool canSell => true;


	}
}
