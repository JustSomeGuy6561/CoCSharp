using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.Inventory
{
	public interface IInteractiveStorage<T> where T : CapacityItem
	{
		ReadOnlyCollection<ReadOnlyItemSlot> ItemSlots();

		string ReturnItemToSlot(T item, byte slot);

		string PlaceItemInSlot(T item, byte slot);

		string ReplaceItemInSlotWith(T item, byte slot);

		T RetrieveItemFromSlot(byte slot);

		bool ReplaceItem(T replacement, byte slot);

		bool PlaceItem(T item, byte slot);

		//note: can and try add should return the same - if can add returns true, try add should return a valid index.
		//if can add returns false, try add should return -1.

		int TryAddItem(T item);
		bool CanAddItem(T item);
	}
}
