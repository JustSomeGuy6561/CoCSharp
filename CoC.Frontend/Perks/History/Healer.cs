//Healer.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Healer : HistoryPerkBase
	{
		public Healer() : base(HealerStr, HealerBtn, HealerHint, HealerDesc)
		{
		}

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.healthGainMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.2));
		}

		protected override void OnRemoval()
		{
		}
	}
}
