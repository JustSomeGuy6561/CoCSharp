//WetAnus.cs
//Description:
//Author: JustSomeGuy
//7/14/2019, 8:32 PM
using CoC.Backend.BodyParts;

namespace CoC.Frontend.Perks.Endowment
{
	//new perk that allows Genderless PCs to have some unique perk. 
	public sealed partial class WetAnus : EndowmentPerkBase
	{
		public WetAnus() : base(WetAnusStr, WetAnusBtn, WetAnusHint, WetAnusDesc)
		{ }
		protected override void OnActivation()
		{
			baseModifiers.minAnalWetness++;
		}

		protected override void OnRemoval()
		{
			baseModifiers.minAnalWetness--;
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender == Gender.GENDERLESS;
		}

	}

}
