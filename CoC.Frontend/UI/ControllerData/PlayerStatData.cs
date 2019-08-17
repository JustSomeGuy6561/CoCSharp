using CoC.Backend.Creatures;
using CoC.Backend.Engine;

namespace CoC.Frontend.UI.ControllerData
{

	public sealed partial class PlayerStatData
	{
		public string nameString => GameEngine.currentPlayer?.name ?? "";

		internal void Update(Player currentPlayer)
		{
			if (currentPlayer != null)
			{
				Strength.current = currentPlayer.strength;
				Strength.maximum = currentPlayer.maxStrength;
				Strength.minimum = currentPlayer.minStrength;
				Toughness.current = currentPlayer.toughness;
				Toughness.maximum = currentPlayer.maxToughness;
				Toughness.minimum = currentPlayer.minToughness;
				Speed.current = currentPlayer.speed;
				Speed.maximum = currentPlayer.maxSpeed;
				Speed.minimum = currentPlayer.minSpeed;
				Intelligence.current = currentPlayer.intelligence;
				Intelligence.maximum = currentPlayer.maxIntelligence;
				Intelligence.minimum = currentPlayer.minIntelligence;
				Libido.current = currentPlayer.libido;
				Libido.maximum = currentPlayer.maxLibido;
				Libido.minimum = currentPlayer.minLibido;
				Sensitivity.current = currentPlayer.sensitivity;
				Sensitivity.maximum = currentPlayer.maxSensitivity;
				Sensitivity.minimum = currentPlayer.minSensitivity;
				Corruption.current = currentPlayer.corruption;
				Corruption.maximum = currentPlayer.maxCorruption;
				Corruption.minimum = currentPlayer.minCorruption;

				HP.current = currentPlayer.currentHealth;
				HP.maximum = currentPlayer.maxHealth;
				Lust.current = currentPlayer.lust;
				Lust.maximum = currentPlayer.maxLust;
				Lust.minimum = currentPlayer.minLust;

				Fatigue.current = currentPlayer.fatigue;
				Fatigue.maximum = currentPlayer.maxFatigue;
				Fatigue.minimum = currentPlayer.minFatigue;
				//Satiety.current = currentPlayer.;
				//Satiety.maximum = currentPlayer.maxSatiety;
				//Satiety.minimum = currentPlayer.minSatiety;
				//SelfEsteem.current = currentPlayer.SelfEsteem;
				//SelfEsteem.maximum = currentPlayer.maxSelfEsteem;
				//SelfEsteem.minimum = currentPlayer.minSelfEsteem;
				//Willpower.current = currentPlayer.Willpower;
				//Willpower.maximum = currentPlayer.maxWillpower;
				//Willpower.minimum = currentPlayer.minWillpower;
				//Obedience.current = currentPlayer.Obedience;
				//Obedience.maximum = currentPlayer.maxObedience;
				//Obedience.minimum = currentPlayer.minObedience;

				Level.current = currentPlayer.level;
				XP.current = currentPlayer.experience;
				XP.maximum = 100;
				XP.minimum = 0;
				Gems.current = unchecked((uint)currentPlayer.gems); //remember to fix this later lol.
			}
		}

		public readonly CreatureStatWithMinMax Strength = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Toughness = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Speed = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Intelligence = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Libido = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Sensitivity = new CreatureStatWithMinMax(CreatureStatCategory.CORE);
		public readonly CreatureStatWithMinMax Corruption = new CreatureStatWithMinMax(CreatureStatCategory.CORE);

		public CreatureStatWithMinMax HP = new CreatureStatWithMinMax(CreatureStatCategory.COMBAT) { isRatio = true };
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
