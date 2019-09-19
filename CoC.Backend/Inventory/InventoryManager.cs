using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Inventory
{
	public sealed class InventoryManager
	{
		//ten inventory slots, each with a bool for unlocked, and an item slot for what they're holding.
#pragma warning disable CS0169 // #never used
		private readonly HashSet<KeyItem> keyItems;
#pragma warning restore CS0169 // #never used
		//public readonly ReadOnlyCollection<KeyItem> keyItemCollection = new ReadOnlyCollection<KeyItem>(keyItems);
	}
}
