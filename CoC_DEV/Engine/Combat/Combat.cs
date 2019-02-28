//Combat.cs
//Description:
//Author: JustSomeGuy
//1/8/2019, 4:45 PM
using CoC.Creatures;
using System.Linq;

namespace CoC.Engine.Combat
{
	//pseudo code for now.

	internal class Combat
	{
		private static double damageModifier, lustResistanceModifier, lustAttackModifier, foo;

		public static void engageInCombat(Player player, CombatCreature enemy)
		{
			double[] data = new double[2];
			double playerDamageModifier = player.perks.combatPerks.Aggregate((a, b) => a.damageModifier * b.damageModifier);
			double playerLustAttackModifier = player.perks.combatPerks.Aggregate((a, b) => a.lustAttackModifier * b.lustAttackModifier);
			double playerLustResistanceModifier = player.perks.combatPerks.Aggregate((a, b) => a.lustResistanceModifier * b.lustResistanceModifier);

			while (combatContinues(player, enemy))
			{
				CombatRoundStart(player, enemy);
				if (combatContinues(player, enemy)) PlayerPhaseStart(player, enemy);
				if (combatContinues(player, enemy)) EnemyPhaseStart(player, enemy);

			}
		}

		private static bool combatContinues(Player player, CombatCreature enemy)
		{
			return player.hp > 0 && player.lust100 < 100 && enemy.hp > 0 && enemy.lust100 < 100;
		}

		private static void CombatRoundStart(Player player, CombatCreature enemy)
		{
			foreach (CombatRoundStartPerk perk in player.perks.combatRoundStartPerks)
			{
				perk(player, enemy);
			}
			foreach (CombatRoundStartPerk perk in enemy.perks.combatRoundStartPerks)
			{
				perk(enemy, player);
			}
		}

		private static void PlayerPhaseStart(Player player, CombatCreature enemy)
		{
			CombatAttack combatAttack = player.ChooseAttack();
			combatAttack(player, enemy);
			if (!combatContinues(player, enemy))
			{
				return;
			}
			foreach (PostCombatPerk perk in player.perks.postAttackPerks)
			{
				perk(player, enemy);
			}
		}

		private static void EnemyPhaseStart(Player player, CombatCreature enemy)
		{
			CombatAttack combatAttack = enemy.ChooseAttack();
			combatAttack(enemy, player);
			if (!combatContinues(player, enemy))
			{
				return;
			}
			foreach (PostCombatPerk perk in enemy.perks.postAttackPerks)
			{
				perk(enemy, player);
			}
		}

	}
}
