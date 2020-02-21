//Healer.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Healer : HistoryPerkBase
	{
		private double delta;
		public Healer() : base(HealerStr, HealerBtn, HealerHint, HealerDesc)
		{
		}

		protected override void OnActivation()
		{
			var oldVal = baseModifiers.healingMultiplier;
			baseModifiers.healingMultiplier += 0.2;
			delta = baseModifiers.healingMultiplier - oldVal;
		}

		protected override void OnRemoval()
		{
			baseModifiers.healingMultiplier -= delta;
		}
	}
}
