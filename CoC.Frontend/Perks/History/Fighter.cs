//Fighter.cs
//Description:
//Author: JustSomeGuy
//7/9/2019, 4:55 PM
using CoC.Backend.Creatures;

namespace CoC.Frontend.Perks.History
{
	public sealed partial class Fighter : HistoryPerkBase
	{
		public Fighter() : base(FighterStr, FighterBtn, ChooseFighterStr, HasFighterStr) { }

		protected override void OnActivation()
		{
			baseModifiers.combatDamageModifier += 0.1f;
			if (sourceCreature is CombatCreature combatCreature)
			{
				combatCreature.addGems(50);
			}
		}

		protected override void OnRemoval()
		{
			baseModifiers.combatDamageModifier -= 0.1f;
		}
	}
}
