﻿//Pervert.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Pervert : EndowmentPerkBase
	{
		public Pervert() : base(PervertStr, PervertBtn, PervertHint, PervertDesc)
		{ }
		protected override void OnActivation()
		{
			//we're not setting min corruption as this is a non-removable perk and we don't want to lock players out of
			//low corruption abilities. I suppose we could set it anyway if items or whatever could counteract it.

			//baseModifiers.minCorruption += 5;
			AddModifierToPerk(baseModifiers.corruptionGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.25));
			if (hasExtraModifiers)
			{
				AddModifierToPerk(extraModifiers.corruptionRequiredOffset, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, -5));
			}
		}

		protected override void OnRemoval()
		{
		}
	}
}
