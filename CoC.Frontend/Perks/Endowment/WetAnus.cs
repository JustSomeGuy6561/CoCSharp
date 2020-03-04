//WetAnus.cs
//Description:
//Author: JustSomeGuy
//7/14/2019, 8:32 PM
using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	//new perk that allows Genderless PCs to have some unique perk.
	public sealed partial class WetAnus : EndowmentPerkBase
	{
		public WetAnus() : base(WetAnusStr, WetAnusBtn, WetAnusHint, WetAnusDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minAnalWetness, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 1));
		}

		protected override void OnRemoval()
		{
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender == Gender.GENDERLESS;
		}

	}

}
