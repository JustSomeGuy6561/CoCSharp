//Tough.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Tough : EndowmentPerkBase
	{
		public Tough() : base(ToughStr, ToughBtn, ToughHint, ToughDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.minToughness += 5;
			baseModifiers.ToughnessGainMultiplier += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minToughness -= 5;
			baseModifiers.ToughnessGainMultiplier -= 0.25f;
		}
	}
}
