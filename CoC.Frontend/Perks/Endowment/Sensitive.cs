//Sensitive.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Sensitive : EndowmentPerkBase
	{
		public Sensitive() : base(SensitiveStr, SensitiveBtn, SensitiveHint, SensitiveDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.minSensitivity += 5;
			baseModifiers.SensitivityGainMultiplier += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minSensitivity -= 5;
			baseModifiers.SensitivityGainMultiplier -= 0.25f;
		}
	}
}