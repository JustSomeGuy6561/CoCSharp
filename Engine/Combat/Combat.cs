//Combat.cs
//Description:
//Author: JustSomeGuy
//1/8/2019, 4:45 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine.Combat
{
	//pseudo code for now.

	public class Combat
	{
		private static double damageModifier, lustResistanceModifier, lustAttackModifier, foo;

		public static void engageInCombat(Player player, Creature enemy)
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

		private static bool combatContinues(Player player, Creature enemy)
		{
			return player.hp > 0 && player.lust100 < 100 && enemy.hp > 0 && enemy.lust100 < 100;
		}

		private static void CombatRoundStart(Player player, Creature enemy)
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

		private static void PlayerPhaseStart(Player player, Creature enemy)
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

		private static void EnemyPhaseStart(Player player, Creature enemy)
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
