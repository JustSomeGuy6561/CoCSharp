using CoC.Backend.Engine;
using CoC.Backend.Strings.Creatures;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Frontend.UI.ControllerData
{

	public sealed partial class PlayerStatData
	{
		public string nameString => GameEngine.currentPlayer?.name ?? "";


		public readonly CreatureStatWithMinMax Strength = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Toughness = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Speed = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Intelligence = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Libido = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Sensitivity = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Corruption = new CreatureStatWithMinMax(CreatureStatCategory.CORE);

		public CreatureStatWithMinMax HP = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true};
		public CreatureStatWithMinMax Lust = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true };

		public readonly CreatureStatWithMinMax Fatigue = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax Satiety = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true };
		public readonly CreatureStatWithMinMax SelfEsteem = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };
		public readonly CreatureStatWithMinMax Willpower = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };
		public readonly CreatureStatWithMinMax Obedience = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true, enabled = false };

		public readonly CreatureStatNumeric Level = new CreatureStatNumeric(CreatureStatCategory.ADVANCEMENT);
		public readonly CreatureStatWithMinMax XP = new CreatureStatWithMinMax(CreatureStatCategory.ADVANCEMENT) { isRatio = true };
		public readonly CreatureStatNumeric Gems = new CreatureStatNumeric(CreatureStatCategory.ADVANCEMENT) { notifyPlayerOfChange = false };//idk why. 

		internal PlayerStatData()
		{

		}

	}




}
