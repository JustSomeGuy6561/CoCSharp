//Smart.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Smart : EndowmentPerkBase
	{
		public Smart() : base(SmartStr, SmartBtn, SmartHint, SmartDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.minIntelligence += 5;
			baseModifiers.IntelligenceGainMultiplier += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minIntelligence -= 5;
			baseModifiers.IntelligenceGainMultiplier -= 0.25f;
		}
	}
}