//CreatureHelpers.cs
//Description:
//Author: JustSomeGuy
//10/11/2019 1:24:57 AM

using CoC.Backend.Creatures;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Tools;
using CoC.Frontend.Inventory;
using CoC.Frontend.UI;
using System;

namespace CoC.Frontend.Creatures
{
	//public static class CreatureHelper
	//{
	//	public static void AddItem(this Creature creature, CapacityItem item, Action resumeCallback)
	//	{
	//		if (creature.TryAddItem(item, out string result))
	//		{
	//			TextOutput.OutputText(result);
	//			resumeCallback();
	//		}
	//		else
	//		{
	//			new FullStorageHelper(creature).InitItemsFull(item, resumeCallback);
	//		}
	//	}

	//	public static void UseItem(this Creature creature, CapacityItem item, Action resumeCallback)
	//	{
	//		creature.UseItemManual(item, (x, y, z) => ReturnFromItemAttempt(creature, x, y, z, resumeCallback));
	//	}

	//	public static void UseItemFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.UseItemInInventoryManual(index, (x, y, z) => ReturnFromItemInInventoryAttempt(creature, index, x, y, z, resumeCallback));
	//	}

	//	public static void EquipArmor(this Creature creature, ArmorBase item, Action resumeCallback)
	//	{
	//		creature.EquipArmorManual(item, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void EquipArmorFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.EquipArmorFromInventoryManual(index, (x, y, z) => ReturnFromItemInInventoryAttemptSafe(creature, index, x, y, z, resumeCallback));
	//	}

	//	public static void EquipUpperGarment(this Creature creature, UpperGarmentBase item, Action resumeCallback)
	//	{
	//		creature.EquipUpperGarmentManual(item, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void EquipUpperGarmentFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.EquipUpperGarmentFromInventoryManual(index, (x, y, z) => ReturnFromItemInInventoryAttemptSafe(creature, index, x, y, z, resumeCallback));
	//	}

	//	public static void EquipLowerGarment(this Creature creature, LowerGarmentBase item, Action resumeCallback)
	//	{
	//		creature.EquipLowerGarmentManual(item, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void EquipLowerGarmentFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.EquipLowerGarmentFromInventoryManual(index, (x, y, z) => ReturnFromItemInInventoryAttemptSafe(creature, index, x, y, z, resumeCallback));
	//	}

	//	public static void RemoveArmor(this Creature creature, Action resumeCallback)
	//	{
	//		var armor = creature.RemoveArmorManual();
	//		if (armor != null)
	//		{
	//			creature.AddItem(armor, resumeCallback);
	//		}
	//		else
	//		{
	//			resumeCallback();
	//		}
	//	}

	//	public static void ReplaceArmor(this Creature creature, ArmorBase armor, Action resumeCallback)
	//	{
	//		creature.ReplaceArmorManual(armor, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void ReplaceArmorFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.ReplaceArmorFromInventoryManual(index, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void RemoveUpperGarment(this Creature creature, Action resumeCallback)
	//	{
	//		var upperGarment = creature.RemoveUpperGarmentManual();
	//		if (upperGarment != null)
	//		{
	//			creature.AddItem(upperGarment, resumeCallback);
	//		}
	//		else
	//		{
	//			resumeCallback();
	//		}
	//	}

	//	public static void ReplaceUpperGarment(this Creature creature, UpperGarmentBase upperGarment, Action resumeCallback)
	//	{
	//		creature.ReplaceUpperGarmentManual(upperGarment, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void ReplaceUpperGarmentFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.ReplaceUpperGarmentFromInventoryManual(index, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void RemoveLowerGarment(this Creature creature, Action resumeCallback)
	//	{
	//		var lowerGarment = creature.RemoveLowerGarmentManual();
	//		if (lowerGarment != null)
	//		{
	//			creature.AddItem(lowerGarment, resumeCallback);
	//		}
	//		else
	//		{
	//			resumeCallback();
	//		}
	//	}

	//	public static void ReplaceLowerGarment(this Creature creature, LowerGarmentBase lowerGarment, Action resumeCallback)
	//	{
	//		creature.ReplaceLowerGarmentManual(lowerGarment, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	public static void ReplaceLowerGarmentFromInventory(this Creature creature, byte index, Action resumeCallback)
	//	{
	//		creature.ReplaceLowerGarmentFromInventoryManual(index, (x, y, z) => ReturnFromItemAttemptSafe(creature, x, y, z, resumeCallback));
	//	}

	//	private static void ReturnFromItemAttempt(Creature creature, bool successfullyUsed, string context, CapacityItem originalOrReplacement, Action resumeCallback)
	//	{

	//		TextOutput.OutputText(context);

	//		if (!successfullyUsed)
	//		{
	//			TextOutput.OutputText(AttemptToAddFailedItemToInventoryForLaterUse());
	//		}

	//		if (originalOrReplacement != null)
	//		{
	//			AddItem(creature, originalOrReplacement, resumeCallback);
	//		}
	//		else
	//		{
	//			resumeCallback();
	//		}
	//	}

	//	private static void ReturnFromItemInInventoryAttempt(Creature creature, byte originalIndex, bool successfullyUsed, string context, CapacityItem originalOrReplacement, Action resumeCallback)
	//	{
	//		TextOutput.OutputText(context);

	//		if (successfullyUsed)
	//		{
	//			if (originalOrReplacement != null)
	//			{
	//				AddItem(creature, originalOrReplacement, resumeCallback);
	//			}
	//			else
	//			{
	//				resumeCallback();
	//			}
	//		}
	//		else
	//		{
	//			if (((IInteractiveStorage<CapacityItem>)creature).PlaceItem(originalOrReplacement, originalIndex))
	//			{
	//				TextOutput.OutputText(((IInteractiveStorage<CapacityItem>)creature).ReturnItemToSlot(originalOrReplacement, originalIndex));
	//				resumeCallback();
	//			}
	//			else
	//			{
	//				AddItem(creature, originalOrReplacement, resumeCallback);
	//			}
	//		}
	//	}

	//	private static void ReturnFromItemAttemptSafe<T>(Creature creature, bool successfullyUsed, string context, T originalOrReplacement, Action resumeCallback) where T:CapacityItem<T>
	//	{
	//		TextOutput.OutputText(context);

	//		if (!successfullyUsed)
	//		{
	//			TextOutput.OutputText(AttemptToAddFailedItemToInventoryForLaterUse());
	//		}

	//		if (originalOrReplacement != null)
	//		{
	//			AddItem(creature, originalOrReplacement, resumeCallback);
	//		}
	//		else
	//		{
	//			resumeCallback();
	//		}
	//	}

	//	private static void ReturnFromItemInInventoryAttemptSafe<T>(Creature creature, byte originalIndex, bool successfullyUsed, string context, T originalOrReplacement, Action resumeCallback) where T:CapacityItem<T>
	//	{
	//		TextOutput.OutputText(context);

	//		if (successfullyUsed)
	//		{
	//			if (originalOrReplacement != null)
	//			{
	//				AddItem(creature, originalOrReplacement, resumeCallback);
	//			}
	//			else
	//			{
	//				resumeCallback();
	//			}
	//		}
	//		else
	//		{
	//			if (((IInteractiveStorage<CapacityItem>)creature).PlaceItem(originalOrReplacement, originalIndex))
	//			{
	//				TextOutput.OutputText(((IInteractiveStorage<CapacityItem>)creature).ReturnItemToSlot(originalOrReplacement, originalIndex));
	//				resumeCallback();
	//			}
	//			else
	//			{
	//				AddItem(creature, originalOrReplacement, resumeCallback);
	//			}
	//		}
	//	}

	//	private static string AttemptToAddFailedItemToInventoryForLaterUse()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	//private static void DoCallback(string text, Action resumeAction)
	//	//{
	//	//	TextOutput.OutputText(text);
	//	//	resumeAction?.Invoke();
	//	//}
	//	////private static void ReturnFromItemUseHelper()


	//	//private static void ReturnFromItemAttempt(Creature source, CapacityItem originalItem, bool successfullyUsedItem, string context, CapacityItem replacementItem, ReturnFromUsingItemCallback onUseItemReturn)
	//	//{
	//	//	if (successfullyUsedItem)
	//	//	{
	//	//		if (replacementItem != null)
	//	//		{
	//	//			AddItemManual(replacementItem, context, onUseItemReturn);
	//	//		}
	//	//		else
	//	//		{
	//	//			onUseItemReturn(context);
	//	//		}
	//	//	}
	//	//	else
	//	//	{
	//	//		context += 
	//	//		AddItemManual(originalItem, context, onUseItemReturn);
	//	//	}
	//	//}

	//	//private static void ReturnFromItemInInventoryAttempt(Creature source, CapacityItem originalItem, byte originalIndex, bool successfullyUsedItem, string context, CapacityItem replacementItem,
	//	//	ReturnFromUsingItemCallback onUseItemReturn)
	//	//{
	//	//	if (successfullyUsedItem)
	//	//	{
	//	//		if (replacementItem != null)
	//	//		{
	//	//			AddItemManual(replacementItem, context, onUseItemReturn);
	//	//		}
	//	//		else
	//	//		{
	//	//			onUseItemReturn(context);
	//	//		}
	//	//	}
	//	//	else
	//	//	{
	//	//		if (inventoryStore.AddItemBack(originalIndex, originalItem)) //try to add it back to its original location. this should always succeed, unless something broke or was already broken.
	//	//		{
	//	//			context += ReturnItemToPreviousSlotText(originalIndex, originalItem);
	//	//			onUseItemReturn(context);
	//	//		}
	//	//		else
	//	//		{
	//	//			context += ReturnItemToPreviousSlotFailedForSomeReasonText(originalIndex, originalItem);
	//	//			AddItemManual(originalItem, context, onUseItemReturn);
	//	//		}
	//	//	}
	//	//}

	//	//private static void ReturnFromItemAttemptSafe<T>(Creature source, T originalItem, bool successfullyUsedItem, string context, T replacementItem, ReturnFromUsingItemCallback onUseItemReturn)
	//	//	where T : CapacityItem
	//	//{
	//	//	if (successfullyUsedItem)
	//	//	{
	//	//		if (replacementItem != null)
	//	//		{
	//	//			AddItemManual(replacementItem, context, onUseItemReturn);
	//	//		}
	//	//		else
	//	//		{
	//	//			onUseItemReturn(context);
	//	//		}
	//	//	}
	//	//	else
	//	//	{
	//	//		context += AttemptToAddFailedItemToInventoryForLaterUse();
	//	//		AddItemManual(originalItem, context, onUseItemReturn);
	//	//	}
	//	//}

	//	//private static void ReturnFromItemInInventoryAttemptSafe<T>(Creature source, T originalItem, byte originalIndex, bool successfullyUsedItem, string context, T replacementItem,
	//	//	ReturnFromUsingItemCallback onUseItemReturn) where T : CapacityItem
	//	//{
	//	//	if (successfullyUsedItem)
	//	//	{
	//	//		if (replacementItem != null)
	//	//		{
	//	//			AddItemManual(replacementItem, context, onUseItemReturn);
	//	//		}
	//	//		else
	//	//		{
	//	//			onUseItemReturn(context);
	//	//		}
	//	//	}
	//	//	else
	//	//	{
	//	//		if (inventoryStore.AddItemBack(originalIndex, originalItem)) //try to add it back to its original location. this should always succeed, unless something broke or was already broken.
	//	//		{
	//	//			context += ReturnItemToPreviousSlotText(originalIndex, originalItem);
	//	//			onUseItemReturn(context);
	//	//		}
	//	//		else
	//	//		{
	//	//			context += ReturnItemToPreviousSlotFailedForSomeReasonText(originalIndex, originalItem);
	//	//			AddItemManual(originalItem, context, onUseItemReturn);
	//	//		}
	//	//	}
	//	//}
	//}
}
