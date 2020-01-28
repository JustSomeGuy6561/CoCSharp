using CoC.Backend;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Perks
{
	public sealed class CeraphFetishesPerk : StackablePerk
	{
		private byte stacks;

		public bool hasExhibitionistFetish => stacks >= 1;
		public bool hasBondageFetish => stacks >= 2;

		public bool hasPassiveFetish => stacks >= 3;

		public CeraphFetishesPerk(byte initialStack = 1) : base()
		{
			stacks = Utils.Clamp2<byte>(initialStack, 1, 3);
		}

		public override string Name()
		{
			throw new NotImplementedException();
		}

		public override string HasPerkText()
		{
			throw new NotImplementedException();
		}

		protected override bool KeepOnAscension => false;

		//passive perk - does nothing when activating or deactivating.
		protected override void OnActivation() { }

		protected override void OnRemoval() { }

		protected override bool OnStackDecreaseAttempt()
		{
			if (stacks > 1)
			{
				stacks--;
				return true;
			}
			return false;
		}

		protected override bool OnStackIncreaseAttempt()
		{
			if (stacks < 3)
			{
				stacks++;
				return true;
			}
			return false;
		}

		internal void SetStacks(byte stackCount)
		{
			stacks = Utils.Clamp2<byte>(stackCount, 1, 3);
		}
	}
}
