using CoC.Backend.Save.Internals;
using System.Runtime.Serialization;

namespace CoC.Backend.Creatures
{
	[DataContract]
	public abstract class CombatCreature : Creature
	{
		public CombatCreature(CreatureCreator creator) : base(creator)
		{
		}

		internal CombatCreature(SurrogateCombatCreator surrogateCreator) : base(surrogateCreator)
		{

		}

		internal override void AddSurrogateData()
		{
			base.AddSurrogateData();
			//additional surrogate based classes.
		}
	}
}
