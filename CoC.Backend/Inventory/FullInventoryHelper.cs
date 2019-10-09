using CoC.Backend.Engine;
using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Inventory
{
	public sealed class FullInventoryHelper
	{
		private Inventory inventory;
		private AddItemHelper itemHelper;

		public FullInventoryHelper(Inventory inventoryStore)
		{
			inventory = inventoryStore;
		}

		public void InitItemsFull(CapacityItem item, Action resumeCallback, Action putBackOverride, Action abandonItemOverride)
		{
			itemHelper = new AddItemHelper(item, resumeCallback, putBackOverride, abandonItemOverride);
			FullItemsChooseAction();
		}

		private void FullItemsChooseAction()
		{ 
			//print items full disclaimer.
			for (byte x = 0; x < inventory.currentlyUnlockedSlots; x++) 
			{
				var slot = inventory.itemSlots[x];
				addButton(x, slot.item.shortName() + 'x' + slot.itemCount, () => ReplaceItem(x, itemHelper.item));
			}
			if (itemHelper.returnCallback != null)
			{
				addButton(12, putBackText(), itemHelper.returnCallback);
			}
			if (itemHelper.item.CanUse(CreatureStore.currentControlledCharacter))
			{
				addButton(13, useText(), AttemptToUseItem);
			}

			Action abandon = DefaultAbandonAction;
			if (itemHelper.abandonCallback != null)
			{
				abandon = itemHelper.abandonCallback;
			}
			addButton(14, abandonText(), abandon);
		}

		private string putBackText()
		{
			return "Put Back";
		}

		private string useText()
		{
			return "Use Now";
		}

		private string abandonText()
		{
			return "Abandon";
		}

		private void DefaultAbandonAction()
		{
			//print abandonText
			DoReturn();
		}

		private void ReplaceItem(byte index, CapacityItem item)
		{
			inventory.ReplaceItemInSlot(index, item, true);
		}

		private void AttemptToUseItem()
		{
			itemHelper.item.AttemptToUse(CreatureStore.currentControlledCharacter, PostItemUseAttempt);
		}

		private void PostItemUseAttempt(bool succeeded, CapacityItem newItem)
		{
			if (succeeded && newItem is null)
			{
				DoReturn();
			}
			else
			{
				if (succeeded)
				{
					itemHelper.item = newItem;
				}
				FullItemsChooseAction();
			}
		}

		private void addButton(byte index, string title, Action callback)
		{

		}

		private void DoReturn()
		{
			Action resumeAction = itemHelper.resumeCallback;
			itemHelper = null;
			resumeAction();
		}
	}

	public sealed class AddItemHelper
	{
		public CapacityItem item;
		public readonly Action resumeCallback;
		public readonly Action returnCallback;
		public readonly Action abandonCallback;

		public AddItemHelper(CapacityItem capacityItem, Action resumeCallback, Action returnCallback, Action abandonCallback)
		{
			item = capacityItem;
			this.resumeCallback = resumeCallback ?? throw new ArgumentNullException(nameof(resumeCallback));
			this.returnCallback = returnCallback ?? throw new ArgumentNullException(nameof(returnCallback));
			this.abandonCallback = abandonCallback ?? throw new ArgumentNullException(nameof(abandonCallback));
		}
	}
}
