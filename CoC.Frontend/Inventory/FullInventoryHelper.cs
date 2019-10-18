using CoC.Backend.Engine;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace CoC.Frontend.Inventory
{
	//public sealed class FullStorageHelper
	//{
	//	private IInteractiveStorage<CapacityItem> inventory;
	//	private AddItemHelper itemHelper;

	//	public FullStorageHelper(IInteractiveStorage<CapacityItem> inventoryStore)
	//	{
	//		inventory = inventoryStore;
	//	}

	//	public void InitItemsFull(CapacityItem item, Action resumeCallback, Action putBackOverride = null, Action abandonItemOverride = null)
	//	{
	//		itemHelper = new AddItemHelper(item, resumeCallback, putBackOverride, abandonItemOverride);
	//		FullItemsChooseAction();
	//	}

	//	private void FullItemsChooseAction()
	//	{
	//		//print items full disclaimer.
	//		ReadOnlyCollection<ReadOnlyItemSlot> slots = inventory.ItemSlots();

	//		for (byte x = 0; x < slots.Count; x++)
	//		{
	//			var slot = slots[x];
	//			DoButton(x, slot.item.shortName() + 'x' + slot.itemCount, () => ReplaceItem(x, itemHelper.item));
	//		}
	//		if (itemHelper.returnCallback != null)
	//		{
	//			DoButton(12, putBackText(), itemHelper.returnCallback);
	//		}
	//		if (itemHelper.item.CanUse(CreatureStore.currentControlledCharacter, out string _))
	//		{
	//			DoButton(13, useText(), AttemptToUseItem);
	//		}

	//		Action abandon = DefaultAbandonAction;
	//		if (itemHelper.abandonCallback != null)
	//		{
	//			abandon = itemHelper.abandonCallback;
	//		}
	//		DoButton(14, abandonText(), abandon);
	//	}

	//	private void DoButton(byte index, string content, Action callback)
	//	{
	//		ButtonManager.AddButton(index, content, () => { display.ClearText(); callback(); });
	//	}

	//	private string putBackText()
	//	{
	//		return "Put Back";
	//	}

	//	private string useText()
	//	{
	//		return "Use Now";
	//	}

	//	private string abandonText()
	//	{
	//		return "Abandon";
	//	}

	//	private void DefaultAbandonAction()
	//	{
	//		//print abandonText
	//		DoReturn();
	//	}

	//	private void ReplaceItem(byte index, CapacityItem item)
	//	{
	//		inventory.ReplaceItem(item, index);
	//		display.OutputText(inventory.ReplaceItemInSlotWith(item, index));
	//	}

	//	private void AttemptToUseItem()
	//	{
	//		itemHelper.item.AttemptToUse(CreatureStore.currentControlledCharacter, PostItemUseAttempt);
	//	}

	//	private void PostItemUseAttempt(bool succeeded, string content, CapacityItem newItem)
	//	{
	//		display.OutputText(content);
	//		if (succeeded && newItem is null)
	//		{
	//			DoReturn();
	//		}
	//		else
	//		{
	//			if (succeeded)
	//			{
	//				itemHelper.item = newItem;
	//			}
	//			FullItemsChooseAction();
	//		}
	//	}

	//	private void DoReturn()
	//	{
	//		Action resumeAction = itemHelper.resumeCallback;
	//		itemHelper = null;
	//		resumeAction();
	//	}
	//}
	//public sealed class FullStorageHelper<T> where T: CapacityItem<T>
	//{
	//	private IInteractiveStorage<T> inventory;
	//	private AddItemHelper<T> itemHelper;

	//	public FullStorageHelper(IInteractiveStorage<T> inventoryStore)
	//	{
	//		inventory = inventoryStore;
	//	}

	//	public void InitItemsFull(T item, Action resumeCallback, Action putBackOverride = null, Action abandonItemOverride = null)
	//	{
	//		itemHelper = new AddItemHelper<T>(item, resumeCallback, putBackOverride, abandonItemOverride);
	//		FullItemsChooseAction();
	//	}

	//	private void FullItemsChooseAction()
	//	{
	//		//print items full disclaimer.
	//		ReadOnlyCollection<ReadOnlyItemSlot> slots = inventory.ItemSlots();

	//		for (byte x = 0; x < slots.Count; x++)
	//		{
	//			var slot = slots[x];
	//			DoButton(x, slot.item.shortName() + 'x' + slot.itemCount, () => ReplaceItem(x, itemHelper.item));
	//		}
	//		if (itemHelper.returnCallback != null)
	//		{
	//			DoButton(12, putBackText(), itemHelper.returnCallback);
	//		}
	//		if (itemHelper.item.CanUse(CreatureStore.currentControlledCharacter, out string _))
	//		{
	//			DoButton(13, useText(), AttemptToUseItem);
	//		}

	//		Action abandon = DefaultAbandonAction;
	//		if (itemHelper.abandonCallback != null)
	//		{
	//			abandon = itemHelper.abandonCallback;
	//		}
	//		DoButton(14, abandonText(), abandon);
	//	}

	//	private void DoButton(byte index, string content, Action callback)
	//	{
	//		ButtonManager.AddButton(index, content, () => { display.ClearText(); callback(); });
	//	}

	//	private string putBackText()
	//	{
	//		return "Put Back";
	//	}

	//	private string useText()
	//	{
	//		return "Use Now";
	//	}

	//	private string abandonText()
	//	{
	//		return "Abandon";
	//	}

	//	private void DefaultAbandonAction()
	//	{
	//		//print abandonText
	//		DoReturn();
	//	}

	//	private void ReplaceItem(byte index, T item)
	//	{
	//		inventory.ReplaceItem(item, index);
	//		display.OutputText(inventory.ReplaceItemInSlotWith(item, index));
	//	}

	//	private void AttemptToUseItem()
	//	{
	//		itemHelper.item.AttemptToUseSafe(CreatureStore.currentControlledCharacter, PostItemUseAttempt);
	//	}

	//	private void PostItemUseAttempt(bool succeeded, string content, T newItem)
	//	{
	//		display.OutputText(content);
	//		if (succeeded && newItem is null)
	//		{
	//			DoReturn();
	//		}
	//		else
	//		{
	//			if (succeeded)
	//			{
	//				itemHelper.item = newItem;
	//			}
	//			FullItemsChooseAction();
	//		}
	//	}

	//	private void DoReturn()
	//	{
	//		Action resumeAction = itemHelper.resumeCallback;
	//		itemHelper = null;
	//		resumeAction();
	//	}
	//}

	//public sealed class AddItemHelper
	//{

	//	public CapacityItem item;
	//	public readonly Action resumeCallback;
	//	public readonly Action returnCallback;
	//	public readonly Action abandonCallback;

	//	public AddItemHelper(CapacityItem capacityItem, Action resumeCallback, Action returnCallback, Action abandonCallback)
	//	{
	//		item = capacityItem;
	//		this.resumeCallback = resumeCallback ?? throw new ArgumentNullException(nameof(resumeCallback));
	//		this.returnCallback = returnCallback ?? throw new ArgumentNullException(nameof(returnCallback));
	//		this.abandonCallback = abandonCallback ?? throw new ArgumentNullException(nameof(abandonCallback));
	//	}
	//}

	//public sealed class AddItemHelper<T> where T : CapacityItem
	//{
	//	public T item;
	//	public readonly Action resumeCallback;
	//	public readonly Action returnCallback;
	//	public readonly Action abandonCallback;

	//	public AddItemHelper(T tItem, Action resumeCallback, Action returnCallback, Action abandonCallback)
	//	{
	//		item = tItem;
	//		this.resumeCallback = resumeCallback ?? throw new ArgumentNullException(nameof(resumeCallback));
	//		this.returnCallback = returnCallback ?? throw new ArgumentNullException(nameof(returnCallback));
	//		this.abandonCallback = abandonCallback ?? throw new ArgumentNullException(nameof(abandonCallback));
	//	}
	//}
}
