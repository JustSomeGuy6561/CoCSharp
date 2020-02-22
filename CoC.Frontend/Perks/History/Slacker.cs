//Slacker.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Slacker : HistoryPerkBase
	{
		private double delta;

		public Slacker() : base(SlackerStr, SlackerBtn, SlackerHint, SlackerDesc) { }

		protected override void OnActivation()
		{
			double oldVal = baseModifiers.fatigueRegenMultiplier;
			baseModifiers.fatigueRegenMultiplier += 0.2f;
			delta = baseModifiers.fatigueRegenMultiplier - oldVal;
		}

		protected override void OnRemoval()
		{
			baseModifiers.fatigueRegenMultiplier -= delta;
		}
	}
}
