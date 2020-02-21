using CoC.Backend;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Backend.Reaction;
using CoC.Backend.UI;
using CoC.Frontend.Creatures;
using CoC.Frontend.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Frontend.Inventory
{
	public static class GainItemHelper
	{
		//used for events that require items. Resumes execution once the item is obtained, no way to return an item or have any special callback for throw out.
		public static DynamicTimeReaction GainItemEvent(this Creature source, CapacityItem item, SimpleDescriptor gainItemContext)
		{
			return new GainItemSpecialEvent(source, item, gainItemContext);
		}

		//used for standard gameplay, which requires a callback to handle once the item has been added successfully. The content is guarenteed to be written to the display before the callback
		//is called.
		public static void GainItemWithCallback(this Creature source, CapacityItem item, string originalOutput, bool isInCombat, Action resumeCallback)
		{
			if (source.CanAddItem(item))
			{
				int slot = source.TryAddItem(item);
				DisplayManager.GetCurrentDisplay().OutputText(((IInteractiveStorage<CapacityItem>)source).PlaceItemInSlot(item, (byte)slot));
				resumeCallback();
			}
			else
			{
				new ItemFullHelper(source, item, originalOutput, isInCombat, resumeCallback).Init();
			}
		}
	}

	public class GainItemSpecialEvent : DynamicTimeReaction
	{
		private readonly Creature source;
		private readonly CapacityItem target;
		private readonly SimpleDescriptor howItemWasObtainedText;

		public GainItemSpecialEvent(Creature creature, CapacityItem item, SimpleDescriptor gainItemText)
		{
			source = creature ?? throw new ArgumentNullException(nameof(creature));
			target = item ?? throw new ArgumentNullException(nameof(item));
			howItemWasObtainedText = gainItemText ?? throw new ArgumentNullException(nameof(gainItemText));
		}

		protected override DisplayBase AsFullPageScene(bool currentlyIdling, bool hasIdleHours)
		{
			return new ItemFullHelper(source, target, howItemWasObtainedText(), false, () => GameEngine.ResumeExection()).Init();
		}

		protected override string AsTextScene(bool currentlyIdling, bool hasIdleHours)
		{
			source.TryAddItem(target, out string result);
			return howItemWasObtainedText() + result;
		}

		protected override bool IsJustTextScene(bool currentlyIdling, bool hasIdleHours)
		{
			return source.CanAddItem(target);
		}
	}

	/// <summary>
	/// Class that stores the various item gain related information while in use. Note that this is not meant to be kept in scope by anything other than callbacks and lambdas.
	/// Basically, all the data needs to be stored temporarily, so this acts like an anonymous class to do so, though it's not anonymous for clarity.
	/// </summary>
	/// <typeparam name="T">Item type, if applicable. if generic, store as capacity item.</typeparam>

	public class ItemFullHelper<T> where T:CapacityItem<T>
	{
		private readonly IInteractiveStorage<T> inventory;
		private T item;
		private readonly string context;
		private readonly Action resumeCallback;
		private readonly Action returnCallback;
		private readonly Action abandonCallback;
		private readonly bool isInCombat;
		private StandardDisplay display;

		//
		public ItemFullHelper(IInteractiveStorage<T> source, T item, string context, bool inCombat, Action resumeCallback,
			Action returnItemFunction = null, Action cancelItemOverride = null)
		{
			this.inventory = source ?? throw new ArgumentNullException(nameof(source));
			this.item = item ?? throw new ArgumentNullException(nameof(item));
			this.context = context;
			isInCombat = inCombat;
			this.resumeCallback = resumeCallback ?? throw new ArgumentNullException(nameof(resumeCallback));
			this.returnCallback = returnItemFunction;
			this.abandonCallback = cancelItemOverride ?? DefaultAbandonAction;
		}

		public DisplayBase Init()
		{
			var slot = inventory.TryAddItem(item);
			if (slot != -1)
			{
				return new StandardDisplay(context + inventory.PlaceItemInSlot(item, (byte)slot)); //should never occur.
			}
			else
			{
				display = new StandardDisplay();
				FullItemsChooseAction();
				return display;
			}

		}

		private void FullItemsChooseAction()
		{
			//print items full disclaimer.
			ReadOnlyCollection<ReadOnlyItemSlot> slots = inventory.ItemSlots();

			for (byte x = 0; x < slots.Count; x++)
			{
				var slot = slots[x];
				DoButton(x, slot.item.ItemName() + 'x' + slot.itemCount, () => ReplaceItem(x, item));
			}
			if (returnCallback != null)
			{
				DoButton(12, putBackText(), returnCallback);
			}
			if (item.CanUse(CreatureStore.currentControlledCharacter, isInCombat, out string _))
			{
				DoButton(13, useText(), AttemptToUseItem);
			}

			DoButton(14, abandonText(), abandonCallback);
		}

		private void DoButton(byte index, string content, Action callback)
		{
			display.AddButton(index, content, () => { display.ClearOutput(); callback(); });
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
			DoReturn();
		}

		private void ReplaceItem(byte index, T item)
		{
			inventory.ReplaceItem(item, index);
			display.OutputText(inventory.ReplaceItemInSlotWith(item, index));
			DisplayManager.LoadDisplay(display); //if not already, probably redundant. oh well.
			resumeCallback();
		}

		private void AttemptToUseItem()
		{
			item.UseItemSafe(CreatureStore.currentControlledCharacter, (w, x, y, z) => PostItemUseAttempt(w, x, y, z));
		}

		private void PostItemUseAttempt(bool succeeded, string whatHappened, string author, T newItem)
		{
			display.ClearOutput();

			display.OutputText(whatHappened);

			if (succeeded && newItem is null)
			{
				DoReturn();
			}
			else
			{
				if (succeeded)
				{
					item = newItem;
				}
				FullItemsChooseAction();
			}
		}

		private void DoReturn()
		{
			resumeCallback();
		}
		//		public static PageDataWrapper GainItem<T>(IInteractiveStorage<T> source, T item, string context, Action resumeCallback) where T:CapacityItem
		//		{
		//			var slot = source.TryAddItem(item);
		//			if (slot != -1)
		//			{
		//				return new PageDataWrapper(context + source.PlaceItemInSlot(item, (byte)slot));
		//			}
		//			else
		//			{
		//				var display = new StandardDisplay(context);
		//				var fullItemMaker = new ItemFullClass(source, item, context);
		//				//do item shit.
		////#error implement me.
		//				throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();

		//				return new PageDataWrapper(display);
		//			}
		//		}
	}
	public class ItemFullHelper
	{
		private readonly IInteractiveStorage<CapacityItem> inventory;
		private CapacityItem item;
		private readonly string context;
		private readonly Action resumeCallback;
		private readonly Action returnCallback;
		private readonly Action abandonCallback;
		private readonly bool isInCombat;
		private readonly StandardDisplay display;

		//
		public ItemFullHelper(IInteractiveStorage<CapacityItem> source, CapacityItem item, string context, bool inCombat, Action resumeCallback,
			Action returnItemFunction = null, Action cancelItemOverride = null)
		{
			this.inventory = source ?? throw new ArgumentNullException(nameof(source));
			this.item = item ?? throw new ArgumentNullException(nameof(item));
			this.context = context;
			this.resumeCallback = resumeCallback ?? throw new ArgumentNullException(nameof(resumeCallback));
			this.returnCallback = returnItemFunction;
			this.abandonCallback = cancelItemOverride ?? DefaultAbandonAction;

			isInCombat = inCombat;

			display = DisplayManager.GetCurrentDisplay();
			display.ClearOutput();
			display.OutputText(context);
		}

		public StandardDisplay Init()
		{
			var slot = inventory.TryAddItem(item);
			if (slot != -1)
			{
				throw new NotSupportedException("Full items helper will have serious side-effects if called when the inventory is not full, and is thus unsupported");
			}
			else
			{
				FullItemsChooseAction();
				return display;
			}

		}

		private void FullItemsChooseAction()
		{
			//print items full disclaimer.
			ReadOnlyCollection<ReadOnlyItemSlot> slots = inventory.ItemSlots();

			for (byte x = 0; x < slots.Count; x++)
			{
				var slot = slots[x];
				DoButton(x, slot.item.ItemName() + 'x' + slot.itemCount, () => ReplaceItem(x, item));
			}
			if (returnCallback != null)
			{
				DoButton(12, putBackText(), returnCallback);
			}
			if (item.CanUse(CreatureStore.currentControlledCharacter, isInCombat, out string _))
			{
				DoButton(13, useText(), AttemptToUseItem);
			}

			DoButton(14, abandonText(), abandonCallback);
		}

		private void DoButton(byte index, string content, Action callback)
		{
			display.AddButton(index, content, () => { display.ClearOutput(); callback(); });
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
			DoReturn();
		}

		private void ReplaceItem(byte index, CapacityItem item)
		{
			inventory.ReplaceItem(item, index);
			display.OutputText(inventory.ReplaceItemInSlotWith(item, index));
			DisplayManager.LoadDisplay(display); //if not already, probably redundant. oh well.
			resumeCallback();
		}

		private void AttemptToUseItem()
		{
			item.UseItem(CreatureStore.currentControlledCharacter, (w, x, y, z) => PostItemUseAttempt(w, x, y, z));
		}

		private void PostItemUseAttempt(bool succeeded, string whatHappened, string author, CapacityItem newItem)
		{
			display.ClearOutput();

			display.OutputText(whatHappened);

			if (succeeded && newItem is null)
			{
				DoReturn();
			}
			else
			{
				if (succeeded)
				{
					item = newItem;
				}
				FullItemsChooseAction();
			}
		}

		private void DoReturn()
		{
			resumeCallback();
		}
		//		public static PageDataWrapper GainItem<T>(IInteractiveStorage<T> source, T item, string context, Action resumeCallback) where T:CapacityItem
		//		{
		//			var slot = source.TryAddItem(item);
		//			if (slot != -1)
		//			{
		//				return new PageDataWrapper(context + source.PlaceItemInSlot(item, (byte)slot));
		//			}
		//			else
		//			{
		//				var display = new StandardDisplay(context);
		//				var fullItemMaker = new ItemFullClass(source, item, context);
		//				//do item shit.
		////#error implement me.
		//				throw new Backend.Tools.InDevelopmentExceptionThatBreaksOnRelease();

		//				return new PageDataWrapper(display);
		//			}
		//		}
	}
}
