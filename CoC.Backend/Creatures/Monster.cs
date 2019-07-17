//Monster.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:11 PM
using CoC.Backend.Engine;
using CoC.Backend.SaveData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	public class Monster : CombatCreature
	{
		public readonly ushort bonusHP;
		public Monster(MonsterCreator creator) : base(creator)
		{
			bonusHP = creator.baseHealth;
		}

		public override uint maxHealth
		{
			get
			{
				double hp = GameEngine.difficulties[BackendSessionData.data.difficulty].baseMonsterHP(level) + bonusHP;
				//Apply NG+, NG++, NG+++, etc.
				hp = GetAscensionHP(hp);
				
				hp += toughness * 2;
				hp += perks.baseModifiers.bonusMaxHP;

				hp *= GameEngine.difficulties[BackendSessionData.data.difficulty].monsterHPMultiplier();
				if (hp > ushort.MaxValue)
				{
					return ushort.MaxValue;
				}
				else
				{
					return (ushort) Math.Round(hp);
				}
			}
		}

		public virtual double GetAscensionHP(double hp)
		{
			return hp *  BackendSessionData.data.NewGamePlusLevel * 1.50;
		}
	}
}
