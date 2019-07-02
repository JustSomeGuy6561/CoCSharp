using CoC.Backend.Perks;

namespace CoC.Backend.Creatures
{

	public abstract class CombatCreature : Creature
	{
		public readonly PerkCollection perks;

		public int level { get; private protected set; } = 1;

		public int corruption { get; private protected set; } = 0;
		public CombatCreature(CreatureCreator creator) : base(creator)
		{
		}

		public float spellCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public float physicalCost(double baseCost)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		public float availableStamina => throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();

		public bool hasEnoughStamina(double baseCost, bool isPhysical)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		//internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		//{

		//}
	}
}
