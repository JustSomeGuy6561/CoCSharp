//Slacker.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Slacker : HistoryPerkBase
	{
		public Slacker() : base(SlackerStr, SlackerBtn, SlackerHint, SlackerDesc) { }

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.fatigueRecoveryMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD,  0.2));
		}

		protected override void OnRemoval()
		{
		}
	}
}
