using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	public abstract class StackablePerk : PerkBase
	{
		public StackablePerk(SimpleDescriptor perkName, SimpleDescriptor havePerkText) : base(perkName, havePerkText)
		{
		}

		internal bool attemptStackIncrease()
		{
			return OnStackIncreaseAttempt();
		}

		internal bool attemptStackDecrease()
		{
			return OnStackDecreaseAttempt();
		}

		protected abstract bool OnStackIncreaseAttempt();

		protected abstract bool OnStackDecreaseAttempt();
	}
}
