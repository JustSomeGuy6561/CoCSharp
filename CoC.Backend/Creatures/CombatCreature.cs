namespace CoC.Backend.Creatures
{

	public abstract class CombatCreature : Creature
	{
		public int level { get; private protected set; } = 1;

		public int corruption { get; private protected set; } = 0;
		public CombatCreature(CreatureCreator creator) : base(creator)
		{
		}

		//internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		//{

		//}
	}
}
