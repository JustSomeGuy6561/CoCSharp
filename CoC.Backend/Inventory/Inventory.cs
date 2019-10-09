using CoC.Backend.Items;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.Inventory
{
	public sealed class Inventory
	{
		//ten inventory slots, each with a bool for unlocked, and an item slot for what they're holding.
		private readonly HashSet<KeyItem> keyItems = new HashSet<KeyItem>();
		public IReadOnlyCollection<KeyItem> keyItemCollection => keyItems;

		private readonly List<ItemSlot> slotStorage = new List<ItemSlot>();

		public readonly ReadOnlyCollection<ItemSlot> itemSlots;
		public const byte MAX_SLOTS = 10;

		//automatically handles allocating the right space for the slot storage. 
		public byte currentlyUnlockedSlots
		{
			get => _currentlyUnlockedSlots;
			private set
			{
				Utils.Clamp(ref value, (byte)0, MAX_SLOTS);

				if (_currentlyUnlockedSlots != value)
				{
					if (_currentlyUnlockedSlots > value) //should never happen. if it does, silently remove the items. 
					{
						slotStorage.RemoveRange(value - 1, _currentlyUnlockedSlots - value);
					}
					else
					{
						slotStorage.AddRange(Enumerable.Repeat(new ItemSlot(), value - _currentlyUnlockedSlots));
					}
					_currentlyUnlockedSlots = value;
				}
			}
		}
		private byte _currentlyUnlockedSlots = 0;
		internal Inventory()
		{
			itemSlots = new ReadOnlyCollection<ItemSlot>(slotStorage);

			currentlyUnlockedSlots = 3; //start with 3 unlocked slots.
		}

		public byte UnlockAdditionalSlots(byte amount = 1)
		{
			byte oldCount = currentlyUnlockedSlots;
			currentlyUnlockedSlots += amount;
			return currentlyUnlockedSlots.subtract(oldCount);
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
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			var matches = slotStorage.FindAll(x => x.item == item);

			if (matches.Count != 0)
			{
				foreach (var match in matches)
				{
					if (match.itemCount < match.item.maxCapacityPerSlot)
					{
						return match.AddOrReplaceItem(item);
					}
				}
			}
			ItemSlot emptySlot = slotStorage.Find(x => x.item is null);
			if (emptySlot != null)
			{
				return emptySlot.AddOrReplaceItem(item);
			}

			//Notify user they cannot add the item.
			return false;
		}

		/// <summary>
		/// Attempts to add the item. returns the index at which the item is stored, or -1 if it has not been added. 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int AddItemReturnSlot(CapacityItem item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			var matches = slotStorage.FindAll(x => x.item == item);

			if (matches.Count != 0)
			{
				foreach (var match in matches)
				{
					if (match.itemCount < match.item.maxCapacityPerSlot)
					{
						if (match.AddOrReplaceItem(item))
						{
							return slotStorage.IndexOf(match);
						}
					}
				}
			}

			for (int x = 0; x < slotStorage.Count; x++)
			{
				if (slotStorage[x].item is null && slotStorage[x].AddOrReplaceItem(item))
				{
					return x;
				}
			}

			//Notify user they cannot add the item.
			return -1;
		}

		public void ClearSlot(byte index)
		{
			if (index > slotStorage.Count)
			{
				return;
			}
			slotStorage[index].ClearItem();
		}

		public void ReplaceItemInSlot(byte index, CapacityItem replacement, bool addIfSameItem = true)
		{
			if (index > slotStorage.Count)
			{
				return;
			}
			else if (addIfSameItem && replacement == slotStorage[index].item)
			{
				slotStorage[index].AddOrReplaceItem(replacement);
			}
			else if (replacement == slotStorage[index].item)
			{
				slotStorage[index].ClearItem();
				slotStorage[index].AddOrReplaceItem(replacement);
			}
			else
			{
				slotStorage[index].AddOrReplaceItem(replacement);
			}
		}

		public CapacityItem RemoveItem(byte slotIndex)
		{
			if (slotIndex > slotStorage.Count)
			{
				return null;
			}
			else
			{
				return slotStorage[slotIndex].RemoveItem();
			}
		}
	}

	public sealed class ItemSlot
	{
		public CapacityItem item { get; private set; }
		public byte itemCount { get; private set; }

		internal bool AddOrReplaceItem(CapacityItem newItem)
		{
			if (newItem is null)
			{
				return false;
			}
			else if (item != newItem)
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

		internal CapacityItem RemoveItem()
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

		internal void ClearItem()
		{
			itemCount = 0;
			item = null;
		}
	}
}
