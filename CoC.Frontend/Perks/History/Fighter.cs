//Fighter.cs
//Description:
//Author: JustSomeGuy
//7/9/2019, 4:55 PM
using CoC.Backend.Creatures;
using CoC.Backend.Perks;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Fighter : HistoryPerkBase
	{
		public Fighter() : base(FighterStr, FighterBtn, ChooseFighterStr, HasFighterStr) { }

		protected override void OnActivation()
		{
			AddModifierToPerk(baseModifiers.combatDamageModifier, new ValueModifierStore<double>(ValueModifierType.FLAT_ADD, 0.1));

			sourceCreature.AddGems(50);
		}

		protected override void OnRemoval()
		{
		}
	}
}
