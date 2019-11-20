//Fertile.cs
//Description:
//Author: JustSomeGuy
//7/12/2019, 11:38 PM

namespace CoC.Frontend.Perks.Endowment
{
	public sealed partial class Fertile : EndowmentPerkBase
	{
		public Fertile() : base(FertileStr, FertileBtn, FertileHint, FertileDesc)
		{ }

		protected override void OnActivation()
		{
			baseModifiers.bonusFertility += 15;
		}

		protected override void OnRemoval()
		{
			baseModifiers.bonusFertility -= 15;
		}
	}
}
