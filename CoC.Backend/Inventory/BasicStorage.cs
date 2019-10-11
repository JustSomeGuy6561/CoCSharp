using CoC.Backend.Items;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoC.Backend.Inventory
{
	public class BasicStorage<T> where T: CapacityItem
	{
		public readonly byte maxSlots;

		public BasicStorage(byte maxItems)
		{
			maxSlots = maxItems;
		}

		public BasicStorage(byte maxItems, byte initialUnlockedSlots)
		{
			maxSlots = maxItems;
			currentlyUnlockedSlots = Math.Min(initialUnlockedSlots, maxItems);
		}

		public byte currentlyUnlockedSlots
		{
			get => _currentlyUnlockedSlots;
			private set
			{
				Utils.Clamp(ref value, (byte)0, maxSlots);

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

		private readonly List<ItemSlot> slotStorage = new List<ItemSlot>();

		public ReadOnlyCollection<ReadOnlyItemSlot> itemSlots => new ReadOnlyCollection<ReadOnlyItemSlot>(slotStorage.Select(x => x?.AsReadOnly()).ToList());

		public byte UnlockAdditionalSlots(byte amount = 1)
		{
			byte oldCount = currentlyUnlockedSlots;
			currentlyUnlockedSlots += amount;
			return currentlyUnlockedSlots.subtract(oldCount);
		}

		public bool AddItem(T item)
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
		public int AddItemReturnSlot(T item)
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

		public bool AddItemBack(byte originalIndex, CapacityItem originalItem)
		{
			if (originalIndex >= currentlyUnlockedSlots)
			{
				return false;
			}
			else if (slotStorage[originalIndex].item != null && slotStorage[originalIndex].item != originalItem)
			{
				return false;
			}
			else if (slotStorage[originalIndex].item == originalItem && slotStorage[originalIndex].itemCount >= slotStorage[originalIndex].item.maxCapacityPerSlot)
			{
				return false;
			}
			else
			{
				slotStorage[originalIndex].AddOrReplaceItem(originalItem);
				return true;
			}
		}

		public void ClearSlot(byte index)
		{
			if (index > slotStorage.Count)
			{
				return;
			}
			slotStorage[index].ClearItem();
		}

		public void ReplaceItemInSlot(byte index, T replacement, bool addIfSameItem = true)
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

		public T RemoveItem(byte slotIndex)
		{
			if (slotIndex > slotStorage.Count)
			{
				return null;
			}
			else
			{
				return (T)slotStorage[slotIndex].RemoveItem();
			}
		}
	}
}
