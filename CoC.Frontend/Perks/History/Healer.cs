//Healer.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Healer : HistoryPerkBase
	{
		public Healer() : base(HealerStr, HealerBtn, HealerHint, HealerDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.healingMultiplier += 0.2f;
			}
		}

		protected override void OnRemoval()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.healingMultiplier -= 0.2f;
			}
		}
	}
}
