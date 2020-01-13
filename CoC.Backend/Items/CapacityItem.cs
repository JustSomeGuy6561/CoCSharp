using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{


	public abstract class CapacityItem
	{
		//abbreviated name, for buttons and such.
		private readonly SimpleDescriptor abbreviatedName;
		//Non-abbreviated name. useful when the player has an item and we don't want to describe it again, but can't use the abbreviated name because that doesn't work in a sentence.
		private readonly SimpleDescriptor name;
		//concise description of the item. this is used basically everywhere.
		private readonly SimpleDescriptor description;
		//a full sentence description of the item, with appearance and smells or whatever. used for tooltips, primarily.
		private readonly SimpleDescriptor apperanceSentence;


		protected CapacityItem(SimpleDescriptor abbreviate, SimpleDescriptor itemName, SimpleDescriptor shortDesc, SimpleDescriptor appearance)
		{
			this.abbreviatedName = abbreviate ?? throw new ArgumentNullException(nameof(abbreviate));
			this.name = itemName ?? throw new ArgumentNullException(nameof(itemName));

			description = shortDesc ?? throw new ArgumentNullException(nameof(shortDesc));
			apperanceSentence = appearance ?? throw new ArgumentNullException(nameof(appearance));
		}

		//name used for buttons and such.
		public string AbbreviatedName() => abbreviatedName();

		//name used when we don't have any character limit.
		public string ItemName() => name();

		public string ItemDescription() => description();

		public string Appearance() => apperanceSentence();

		public abstract byte maxCapacityPerSlot { get; }

		/// <summary>
		/// Checks to see if the given creature can use this item. Note that just because an item can be used doesn't mean it will ultimately succeed, i.e. if the player cancels it.
		/// </summary>
		/// <param name="target">The creature attempting to use this item.</param>
		/// <param name="whyNot">A string explaining why the current item cannot be used. only checked if this returns false.</param>
		/// <returns>true if the creature can use this item, false otherwise.</returns>
		public abstract bool CanUse(Creature target, out string whyNot);

		/// <summary>
		/// Attempts to use the item, returning either its own page if it needs several pages, or the results of postItemUseCallback.
		/// </summary>
		/// <param name="target">the creature attempting to use this item.</param>
		/// <param name="postItemUseCallback">Callback to call when the item is actually used. </param>
		/// <returns>Either a page, or a string, explaining what it did, or why it failed.</returns>
		/// <remarks>Items that can result in a bad end will not call the post item use callback. If the base class they inherit does not have a built-in means of dealing with
		/// a bad end, it will need to override this.  </remarks>
		public abstract void AttemptToUse(Creature target, UseItemCallback postItemUseCallback);

		//how much it costs to buy this item. Generally, the sell price is 1/2 it's value because video game capitalism. Items that cannot be bought should be given a price of 0;
		protected abstract int monetaryValue { get; }

		public int buyPrice => Math.Max(monetaryValue, 0);

		//by default, items that can be bought are given a positive value. you may override this if you want an item that can be sold and thus has a price,
		public virtual bool canBuy => buyPrice > 0;
		public virtual bool canSell => true;

	}
}
