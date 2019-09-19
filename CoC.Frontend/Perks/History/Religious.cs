//Religious.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Religious : HistoryPerkBase
	{
		private byte delta;
		public Religious() : base(ReligiousStr, ReligiousBtn, ReligiousHint, ReligiousDesc)
		{
		}

		protected override void OnActivation()
		{
			extraModifiers.replaceMasturbateWithMeditate = true;

			sbyte oldMin = baseModifiers.minLibido;
			baseModifiers.minLibido = baseModifiers.minLibido.subtract(2);
			delta = oldMin.delta(baseModifiers.minLibido);
		}

		protected override void OnRemoval()
		{
			extraModifiers.replaceMasturbateWithMeditate = false;

			baseModifiers.minLibido = (sbyte)(baseModifiers.minLibido + delta);
		}
	}
}
