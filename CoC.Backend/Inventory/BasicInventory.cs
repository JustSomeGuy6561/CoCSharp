using CoC.Backend.Items;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.Inventory
{
	public sealed class BasicInventory
	{
		//ten inventory slots, each with a bool for unlocked, and an item slot for what they're holding.
		private readonly HashSet<KeyItem> keyItems = new HashSet<KeyItem>();
		public IReadOnlyCollection<KeyItem> keyItemCollection => keyItems;

		private readonly BasicStorage<CapacityItem> storage;
		public ReadOnlyCollection<ReadOnlyItemSlot> itemSlots => storage.itemSlots;

		public byte currentlyUnlockedSlots => storage.currentlyUnlockedSlots;
		public const byte MAX_SLOTS = 10;

		//automatically handles allocating the right space for the slot storage.

		public BasicInventory()
		{
			storage = new BasicStorage<CapacityItem>(MAX_SLOTS, 3); //start with 3 unlocked slots.
		}

		public byte UnlockAdditionalSlots(byte amount = 1)
		{
			return storage.UnlockAdditionalSlots(amount);
		}

		public bool AddItem(KeyItem key)
		{
			return keyItems.Add(key);
		}

		public bool HasItem(KeyItem key)
		{
			return keyItems.Contains(key);
		}

		public bool HasItem(Predicate<KeyItem> condition)
		{
			return keyItems.FirstOrDefault(new Func<KeyItem, bool>(condition)) != null;
		}

		public KeyItem GetItem(Predicate<KeyItem> condition)
		{
			return keyItems.FirstOrDefault(new Func<KeyItem, bool>(condition));
		}

		public IEnumerable<KeyItem> GetAllItems(Predicate<KeyItem> condition)
		{
			return keyItems.Where(new Func<KeyItem, bool>(condition));
		}

		public bool RemoveKeyItem(KeyItem key)
		{
			return keyItems.Remove(key);
		}

		public int RemoveWhere(Predicate<KeyItem> condition)
		{
			return keyItems.RemoveWhere(condition);
		}

		public bool RemoveFirst(Predicate<KeyItem> condition)
		{
			return keyItems.Remove(GetItem(condition));
		}

		public bool AddItem(CapacityItem item)
		{
			return storage.AddItem(item);
		}

		/// <summary>
		/// Attempts to add the item. returns the index at which the item is stored, or -1 if it has not been added.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int AddItemReturnSlot(CapacityItem item)
		{
			return storage.AddItemReturnSlot(item);
		}

		public void ClearSlot(byte index)
		{
			storage.ClearSlot(index);
		}

		public void ReplaceItemInSlot(byte index, CapacityItem replacement, bool addIfSameItem = true)
		{
			storage.ReplaceItemInSlot(index, replacement, addIfSameItem);
		}

		public CapacityItem RemoveItem(byte slotIndex)
		{
			return storage.RemoveItem(slotIndex);
		}

		public bool AddItemBack(byte originalIndex, CapacityItem originalItem)
		{
			return storage.AddItemBack(originalIndex, originalItem);
		}

		public bool CanAddItem(CapacityItem item)
		{
			return storage.CanAddItem(item);
		}
	}

	public sealed class ItemSlot
	{
		public CapacityItem item { get; private set; }
		public byte itemCount { get; private set; }

		public ReadOnlyItemSlot AsReadOnly()
		{
			return new ReadOnlyItemSlot(this);
		}

		public bool AddItem(CapacityItem newItem)
		{

			if (newItem is null)
			{
				throw new ArgumentNullException(nameof(newItem));
			}
			else if (isEmpty || item.Equals(newItem) && itemCount < item.maxCapacityPerSlot)
			{
				//shouldn't be necessary, but idk.
				if (isEmpty)
				{
					itemCount = 0;
				}

				itemCount++;
				return true;
			}
			return false;
		}

		public bool ReplaceItem(CapacityItem newItem, bool addIfSame = true)
		{
			if (newItem is null)
			{
				throw new ArgumentNullException(nameof(newItem));
			}
			else if (!item.Equals(newItem) || !addIfSame)
			{
				item = newItem;
				itemCount = 1;
				return true;
			}
			else if (itemCount >= item.maxCapacityPerSlot)
			{
				return false;
			}
			else
			{
				itemCount++;
				return true;
			}
		}

		public bool isEmpty => item is null || itemCount == 0;

		public bool AddOrReplaceItem(CapacityItem newItem)
		{
			if (newItem is null)
			{
				return false;
			}
			else if (!item.Equals(newItem))
			{
				item = newItem;
				itemCount = 1;
				return true;
			}
			else if (itemCount >= item.maxCapacityPerSlot)
			{
				return false;
			}
			else
			{
				itemCount++;
				return true;
			}
		}

		public CapacityItem RemoveItem()
		{
			if (item is null)
			{
				return null;
			}
			else if (itemCount == 0)
			{
				item = null;
				return null;
			}
			else if (itemCount == 1)
			{
				CapacityItem retVal = item;
				item = null;
				itemCount--;
				return retVal;
			}
			else
			{
				itemCount--;
				return item;
			}
		}

		public void ClearItem()
		{
			itemCount = 0;
			item = null;
		}
	}

	public sealed class ReadOnlyItemSlot
	{
		public readonly CapacityItem item;
		public readonly byte itemCount;

		public bool isEmpty => item is null || itemCount == 0;

		internal ReadOnlyItemSlot(ItemSlot source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			item = source.item;
			itemCount = source.itemCount;
		}
	}
}
