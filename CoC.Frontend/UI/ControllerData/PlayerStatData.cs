using CoC.Backend.Engine;
using CoC.Backend.Strings.Creatures;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Frontend.UI.ControllerData
{
	public sealed partial class PlayerStatData
	{
		//The expected behavior for this class is that all data objects will be hard-coded in here; nothing is dynamic.
		//Data WILL CHANGE during gameplay, but NO NEW data is added into a list or whatever. if you need a new stat for events like
		//being in prison or whatever, hard-code it in, but leave it disabled when not in use. enable it when needed. The GUI isn't magic - 
		//it can't support your shit just because you add it here and hope the G
		public const string STRENGTH = "strength";
		public const string TOUGHNESS = "toughness";
		public const string SPEED = "speed";
		public const string INTELLIGENCE = "intelligence";
		public const string LIBIDO = "libido";
		public const string SENSITIVITY = "sensitivity";
		public const string CORRUPTION = "corruption";
		public const string HIT_POINTS = "HP";
		public const string LUST = "lust";
		public const string FATIGUE = "fatigue";
		public const string SATIETY = "satiety";
		public const string SELF_ESTEEM = "self esteem";
		public const string WILLPOWER = "willpower";
		public const string OBEDIENCE = "obedience";
		public const string EXPERIENCE = "XP";
		public const string LEVEL = "level";
		public const string GEMS = "gems";

		public readonly ReadOnlyCollection<CreatureStat> coreStats;
		public readonly ReadOnlyCollection<CreatureStat> combatStats;

		public string nameString => GameEngine.currentPlayer?.name ?? "";

		public readonly ReadOnlyCollection<CreatureStat> advancementStats;

		public readonly CreatureStatWithMinMax Strength = new CreatureStatWithMinMax(STRENGTH, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Toughness = new CreatureStatWithMinMax(TOUGHNESS, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Speed = new CreatureStatWithMinMax(SPEED, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Intelligence = new CreatureStatWithMinMax(INTELLIGENCE, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Libido = new CreatureStatWithMinMax(LIBIDO, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Sensitivity = new CreatureStatWithMinMax(SENSITIVITY, CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Corruption = new CreatureStatWithMinMax(CORRUPTION, CreatureStatCategory.CORE);

		public CreatureStatWithMinMax HP = new CreatureStatWithMinMax(HIT_POINTS, CreatureStatCategory.COMBAT) { isRatio = true};
		public CreatureStatWithMinMax Lust = new CreatureStatWithMinMax(LUST, CreatureStatCategory.COMBAT) { isRatio = true };

		public readonly CreatureStatWithMinMax Fatigue = new CreatureStatWithMinMax(FATIGUE, CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax Satiety = new CreatureStatWithMinMax(SATIETY, CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax SelfEsteem = new CreatureStatWithMinMax(SELF_ESTEEM, CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };
		public readonly CreatureStatWithMinMax Willpower = new CreatureStatWithMinMax(WILLPOWER, CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };
		public readonly CreatureStatWithMinMax Obedience = new CreatureStatWithMinMax(OBEDIENCE, CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };

		public readonly CreatureStat Level = new CreatureStat(LEVEL, CreatureStatCategory.ADVANCEMENT);
		public readonly CreatureStatWithMinMax XP = new CreatureStatWithMinMax(EXPERIENCE, CreatureStatCategory.ADVANCEMENT) { isRatio = true };
		public readonly CreatureStat Gems = new CreatureStat(GEMS, CreatureStatCategory.ADVANCEMENT) { notifyPlayerOfChange = false };//idk why. 

		internal PlayerStatData()
		{
			coreStats = new ReadOnlyCollection<CreatureStat>(new List<CreatureStat>() { Strength, Toughness, Speed, Intelligence, Libido, Sensitivity, Corruption, });
			combatStats = new ReadOnlyCollection<CreatureStat>(new List<CreatureStat>() { HP, Lust, Fatigue, Satiety, SelfEsteem, Willpower, Obedience, });
			advancementStats = new ReadOnlyCollection<CreatureStat>(new List<CreatureStat>() { Level, XP, Gems });
		}

	}




}
