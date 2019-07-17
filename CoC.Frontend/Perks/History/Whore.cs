//Whore.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM
using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Whore : HistoryPerkBase
	{
		public Whore() : base(WhoreStr, WhoreBtn, WhoreHint, WhoreDesc)
		{
		}

		protected override void OnActivation()
		{
			extraModifiers.teaseStrengthMultiplier += 0.15f;
			extraModifiers.IsASlut = true;
		}

		protected override void OnRemoval()
		{
			extraModifiers.teaseStrengthMultiplier -= 0.15f;
		}
	}
}
