using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed class CombatEnemyStatData
	{
		public bool active { get; private set; }

		//public const string HIT_POINTS = "HP";
		//public const string LUST = "lust";
		//public const string FATIGUE = "fatigue";
		//public const string LEVEL = "level";
		//public const string RACE = "race";
		//public const string GENDER = "gender";

		public string nameString { get; private set;}

		public readonly SimpleCreatureStat Level = new SimpleCreatureStat(StatStrings.StrengthStr, "-1", CreatureStatCategory.GENERAL);
		public readonly SimpleCreatureStat Race = new SimpleCreatureStat(StatStrings.RaceStr, "Not Applicable", CreatureStatCategory.GENERAL);
		public readonly SimpleCreatureStat Gender = new SimpleCreatureStat(StatStrings.GenderStr, "Not Applicable", CreatureStatCategory.GENERAL);

		public readonly CreatureStatWithMinMax HP = new CreatureStatWithMinMax(StatStrings.HPStr, CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax Lust = new CreatureStatWithMinMax(StatStrings.LustStr, CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax Fatigue = new CreatureStatWithMinMax(StatStrings.FatigueStr, CreatureStatCategory.COMBAT) { isRatio = true };

		public CombatEnemyStatData()
		{
			ClearEnemyData();
		}

		//if a creature has some missing hp or higher lust due to plot reasons, call update enemy healthlustfatigue afterward. 
		internal void SetEnemyInitialData(MonsterBase creature)
		{
			Level.simpleValue = creature.level.ToString();
			//Race = creature.
			Gender.simpleValue = creature.genitals.gender.AsText();
			HP.maximum = creature.maxHealth;
			HP.current = creature.currentHealth;
			Lust.current = creature.lust;
			Lust.maximum = creature.maxLust;
			Lust.minimum = creature.minLust;
			Fatigue.minimum = creature.fatigue;
			Fatigue.current = creature.fatigue;
			Fatigue.maximum = creature.maxFatigue;

			active = true;
		}

		internal void UpdateEnemyHealthLustFatigue(uint newHP, ushort newLust, byte newFatigue)
		{
			HP.current = newHP;
			Lust.current = newLust;
			Fatigue.current = newFatigue;
		}

		internal void ClearEnemyData()
		{
			active = false;
			nameString = "None";
			Level.simpleValue = "-1";
			Race.simpleValue = "Not Applicable";
			Gender.simpleValue = "Not Applicable";
			HP.current = 0;
			HP.maximum = 0;
			Lust.maximum = 0;
			Lust.minimum = 0;
			Lust.current = 0;
			Fatigue.current = 0;
			Fatigue.maximum = 0;
			Fatigue.minimum = 0;
		}

	}
}
