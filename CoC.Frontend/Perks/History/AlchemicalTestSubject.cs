//AlchemicalGuineaPig.cs
//Description:
//Author: JustSomeGuy
//7/9/2019, 5:38 PM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class TestSubject : HistoryPerkBase
	{
		public TestSubject() : base(GuineaPigStr, GuineaPigBtn, GuineaPigHint, GuineaPigDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.numTransformsDelta += 1;
			}
		}

		protected override void OnRemoval()
		{
			if (hasExtraModifiers)
			{
				extraModifiers.numTransformsDelta -= 1;
			}
		}
	}
}
