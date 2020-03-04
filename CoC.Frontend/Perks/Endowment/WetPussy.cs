//WetPussy.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.BodyParts;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class WetPussy : EndowmentPerkBase
	{
		public WetPussy() : base(WetPussyStr, WetPussyBtn, WetPussyHint, WetPussyDesc)
		{ }
		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.minVaginalWetness, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 1));
			//this.sourceCreature.genitals.incre
		}

		protected override void OnRemoval()
		{
		}

		protected override bool Unlocked(Gender gender)
		{
			return gender.HasFlag(Gender.FEMALE);
		}
	}
}
