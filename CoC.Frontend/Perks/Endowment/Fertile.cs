//Fertile.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fertile : EndowmentPerkBase
	{
		public Fertile() : base(FertileStr, FertileBtn, FertileHint, FertileDesc)
		{ }

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.bonusFertility, new ValueModifierStore<byte>(ValueModifierType.FLAT_ADD, 15));
		}

		protected override void OnRemoval()
		{ }
	}
}
