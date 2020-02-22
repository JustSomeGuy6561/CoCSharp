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
		//menu consumables force the creator to build everything from scratch, and use item is only called by the original menu system. further, the use items will be
		//inaccessible when creating these menu consumables (because they are in the frontend). As a result, they remain unused.

		//unused.
		private protected override CapacityItem UseItem(Creature target, out string resultsOfUseText)
		{
			resultsOfUseText = null;
			return null;
		}

		//unused.
		private protected override CapacityItem UseItemInCombat(CombatCreature target, CombatCreature opponent, out bool resultsInLoss, out string resultsOfUseText)
		{
			resultsOfUseText = null;
			resultsInLoss = false;
			return null;
		}


		protected ConsumableWithMenuBase() : base() { }

		//Note to implementers: OnConsumeAttempt is not used in this class. if you do not need it in your implementation because you did it manually, simply throw a NotSupported or NotImplemented
		//as it will never be called (and if it is somehow, we'd probably want to know).

		private protected override DisplayBase AttemptToUseItem(Creature target, UseItemCallback postItemUseCallback)
		{
			return BuildMenu(target, postItemUseCallback);
		}

		private protected override DisplayBase AttemptToUseItemInCombat(CombatCreature user, CombatCreature opponent, UseItemCombatCallback postItemUseCallback)
		{
			return BuildCombatAwareMenu(user, opponent, postItemUseCallback);
		}

		/// <summary>
		/// Build the first menu page that displays after the target tries to use the item. Eventually, post item use callback must be called, but where and how is up to your implementation.
		/// Generally, this will be after a button is pressed, with the function callback the button executing it, though it may occur on a sub menu or something.
		/// </summary>
		/// <param name="consumer"></param>
		/// <returns>The initial menu display and any text required for it.</returns>
		protected abstract DisplayBase BuildMenu(Creature consumer, UseItemCallback postItemUseCallback);

		protected virtual DisplayBase BuildCombatAwareMenu(CombatCreature consumer, CombatCreature opponent, UseItemCombatCallback postItemUseCallback)
		{
			return BuildMenu(consumer, (success, results, author, replacement) => postItemUseCallback(success, false, results, author, replacement));
		}

		public override byte maxCapacityPerSlot => 10;

	}
}
