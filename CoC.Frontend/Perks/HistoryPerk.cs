//ForestEncounterStrings.cs
//Description:
//Author: JustSomeGuy
//4/5/2019, 9:36 PM
using CoC.Backend;
using CoC.Backend.Perks;
using CoC.Frontend.Perks.Endowment;
using System.Collections.Generic;

namespace CoC.Frontend.Perks
{
	public abstract class HistoryPerkBase : PerkBase
	{
		public readonly SimpleDescriptor buttonText;
		public readonly SimpleDescriptor unlockHistoryText;

		public HistoryPerkBase(SimpleDescriptor perkName, SimpleDescriptor buttonStr, SimpleDescriptor perkHintStr, SimpleDescriptor hasPerkText) : base(perkName, hasPerkText)
		{
			unlockHistoryText = perkHintStr;
			buttonText = buttonStr;
		}

		protected override bool KeepOnAscension => false;

		protected ExtraPerkModifiers extraModifiers => this.ExtraModifiers();
	}
}
