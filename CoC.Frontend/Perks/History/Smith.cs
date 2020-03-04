//Smith.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM
using CoC.Backend;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Smith : HistoryPerkBase
	{
		public Smith() : base(SmithStr, SmithBtn, SmithHint, SmithDesc)
		{
		}

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.armorEffectivenessMultiplier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD,  0.1));
		}

		protected override void OnRemoval()
		{ }
	}
}
