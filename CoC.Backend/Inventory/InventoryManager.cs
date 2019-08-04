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
		private readonly HashSet<KeyItem> keyItems;
		//public readonly ReadOnlyCollection<KeyItem> keyItemCollection = new ReadOnlyCollection<KeyItem>(keyItems);
	}
}
