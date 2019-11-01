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
		bool? oldStatus = null;
		public Whore() : base(WhoreStr, WhoreBtn, WhoreHint, WhoreDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{
				oldStatus = extraModifiers.IsASlut;

				extraModifiers.teaseStrengthMultiplier += 0.15f;
				extraModifiers.IsASlut = true;
			}
		}

		protected override void OnRemoval()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.teaseStrengthMultiplier -= 0.15f;

				if (oldStatus is bool revert)
				{
					extraModifiers.IsASlut = revert;
				}
				oldStatus = null;
			}
		}
	}
}
