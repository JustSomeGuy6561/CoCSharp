using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;

namespace CoC.Backend.Items.Consumables
{
	public abstract class ConsumableWithMenuBase : ConsumableBase
	{
		protected ConsumableWithMenuBase() : base() { }

		//Note to implementers: OnConsumeAttempt is not used in this class. if you do not need it in your implementation because you did it manually, simply throw a NotSupported or NotImplemented
		//as it will never be called (and if it is somehow, we'd probably want to know).

		public override DisplayBase AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			return BuildMenu(target, postItemUseCallback);
		}

		public override DisplayBase AttemptToUseInCombat(CombatCreature target, UseItemCombatCallback postItemUseCallback)
		{
			return BuildCombatAwareMenu(target, postItemUseCallback);
		}

		/// <summary>
		/// Build the first menu page that displays after the target tries to use the item. Eventually, post item use callback must be called, but where and how is up to your implementation.
		/// Generally, this will be after a button is pressed, with the function callback the button executing it, though it may occur on a sub menu or something.
		/// </summary>
		/// <param name="consumer"></param>
		/// <returns>The initial menu display and any text required for it.</returns>
		protected abstract DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback);

		protected virtual DisplayBase BuildCombatAwareMenu(Creature consumer, UseItemCombatCallback postItemUseCallback)
		{
			return BuildMenu(consumer, (success, results, author, replacement) => postItemUseCallback(success, false, results, author, replacement));
		}

		public override byte maxCapacityPerSlot => 10;

	}
}
