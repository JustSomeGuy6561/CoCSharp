using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Inventory
{
	public static class InventoryManager
	{
		public static void AddItem<T>(IInteractiveStorage<T> storage, T item, DisplayBase displayPage) where T : CapacityItem
		{
			int storeLocation = storage.TryAddItem(item);
			if (storeLocation != -1)
			{
				displayPage.OutputText(storage.PlaceItemInSlot(item, (byte)storeLocation));
			}
			else
			{
				displayPage.OutputText("Generic item full text");
			}
		}

	}
}
