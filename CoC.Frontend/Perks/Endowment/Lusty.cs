//Lusty.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Lusty : EndowmentPerkBase
	{
		public Lusty() : base(LustyStr, LustyBtn, LustyHint, LustyDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.LustGainMultiplier += 0.25f;
			baseModifiers.minLibido += 5;
		}

		protected override void OnRemoval()
		{
			baseModifiers.LustGainMultiplier -= 0.25f;
			baseModifiers.minLibido -= 5;
		}
	}
}
