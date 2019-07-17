//Fast.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fast : EndowmentPerkBase
	{
		public Fast() : base(FastStr, FastBtn, FastHint, FastDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.minSpeed += 5;
			baseModifiers.SpeedGainMultiplier += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minSpeed -= 5;
			baseModifiers.SpeedGainMultiplier -= 0.25f;
		}
	}
}