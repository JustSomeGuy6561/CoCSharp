//Religious.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:21 AM

using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Religious : HistoryPerkBase
	{
		public Religious() : base(ReligiousStr, ReligiousBtn, ReligiousHint, ReligiousDesc)
		{
		}

		protected override void OnActivation()
		{
			if (hasExtraModifiers)
			{
				AddModifierToPerk(extraModifiers.replaceMasturbateWithMeditate, true);
			}
			AddModifierToPerk(baseModifiers.minLibidoDelta, new ValueModifierStore<sbyte>(ValueModifierType.FLAT_ADD, -2));
		}

		protected override void OnRemoval()
		{ }
	}
}
