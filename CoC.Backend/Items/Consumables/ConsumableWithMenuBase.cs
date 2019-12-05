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
		protected ConsumableWithMenuBase(SimpleDescriptor shortName, SimpleDescriptor fullName, SimpleDescriptor description) : base(shortName, fullName, description)
		{
		}

		//Note to implementers: OnConsumeAttempt is not used in this class. if you do not need it in your implementation because you did it manually, simply throw a NotSupported or NotImplemented
		//as it will never be called (and if it is somehow, we'd probably want to know).

		public override void AttemptToUse(Creature target, UseItemCallback postItemUseCallback)
		{
			BuildMenu(target, postItemUseCallback);
		}

		/// <summary>
		/// Build the first menu page that displays after the target tries to use the item. Eventually, post item use callback must be called, but where and how is up to your implementation.
		/// Generally, this will be after a button is pressed, with the function callback the button executing it, though it may occur on a sub menu or something.
		/// </summary>
		/// <param name="consumer"></param>
		protected abstract void BuildMenu(Creature consumer, UseItemCallback postItemUseCallback);
	}
}
