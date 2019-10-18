//Monster.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:11 PM
using CoC.Backend.Engine;
using CoC.Backend.UI;
using CoC.Backend.Items;
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
				double hp = GameEngine.difficulties[BackendSessionSave.data.difficulty].baseMonsterHP(level) + bonusHP;
				//Apply NG+, NG++, NG+++, etc.
				hp = GetAscensionHP(hp);
				
				hp += toughness * 2;
				hp += perks.baseModifiers.bonusMaxHP;

				hp *= GameEngine.difficulties[BackendSessionSave.data.difficulty].monsterHPMultiplier();
				hp = Math.Round(hp);
				if (hp >= ushort.MaxValue)
				{
					return ushort.MaxValue;
				}
				else
				{
					return (ushort)hp;
				}
			}
		}

		public virtual double GetAscensionHP(double hp)
		{
			return hp *  BackendSessionSave.data.NewGamePlusLevel * 1.50;
		}

		protected override string PlaceItemInCreatureStorageText(CapacityItem item, byte slot)
		{
			return "The monster places the " + item.shortName() + " in its " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReturnItemToCreatureStorageText(CapacityItem item, byte slot)
		{
			return "The monster returns the " + item.shortName() + " to its " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReplaceItemInCreatureStorageWithNewItemText(CapacityItem newItem, byte slot)
		{
			return "The monster replaces the " + inventory[slot].item.shortName() + " in its " + Tools.Utils.NumberAsPlace(slot) + " pouch with " + newItem.shortName() + ". ";
		}
	}
}
