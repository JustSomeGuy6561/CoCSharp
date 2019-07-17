//Smith.cs
//Description:
//Author: JustSomeGuy
//7/10/2019, 6:22 AM
using CoC.Backend;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Smith : HistoryPerkBase
	{
		public Smith() : base(SmithStr, SmithBtn, SmithHint, SmithDesc)
		{
		}

		protected override void OnActivation()
		{
			baseModifiers.armorEffectivenessMultiplier += 0.1f;
		}

		protected override void OnRemoval()
		{
			baseModifiers.armorEffectivenessMultiplier -= 0.1f;
		}
	}
}
