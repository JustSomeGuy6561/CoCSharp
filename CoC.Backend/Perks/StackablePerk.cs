using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//a variation of the standard perk that has a built-in stacking mechanic. this i
	public abstract class StackablePerk : StandardPerk
	{
		public StackablePerk() : base()
		{
		}

		public bool AttemptStackIncrease()
		{
			return OnStackIncreaseAttempt();
		}

		public bool AttemptStackDecrease()
		{
			return OnStackDecreaseAttempt();
		}

		protected abstract bool OnStackIncreaseAttempt();

		protected abstract bool OnStackDecreaseAttempt();
	}
}
