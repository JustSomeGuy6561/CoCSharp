//Strong.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:31 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Strong : EndowmentPerkBase
	{
		public Strong() : base(StrongStr, StrongBtn, StrongHint, StrongDesc)
		{}

		protected override void OnActivation()
		{
			baseModifiers.minStrength += 5;
			baseModifiers.StrengthGainMultiplier += 0.25f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minStrength -= 5;
			baseModifiers.StrengthGainMultiplier -= 0.25f;
		}
	}
}

//];
