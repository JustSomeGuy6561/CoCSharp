//Whore.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Whore : HistoryPerkBase
	{
		public Whore() : base(WhoreStr, WhoreBtn, WhoreHint, WhoreDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{

				AddModifierToPerk(extraModifiers.teaseStrengthMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.15));
				AddModifierToPerk(extraModifiers.isASlut, true);
			}
		}

		protected override void OnRemoval()
		{
		}
	}
}
