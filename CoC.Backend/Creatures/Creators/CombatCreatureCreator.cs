//CombatCreatureCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:12 PM

namespace CoC.Backend.Creatures
{
	public abstract class CombatCreatureCreator : CreatureCreator
	{
		//main stats
		public byte? strength;
		public byte? speed;
		public byte? intelligence;
		public byte? toughness;
		public byte? sensitivity;

		//secondary stats. 
		public byte? libido;
		public byte? corruption;

		//i see no reason for these to be in the creator. 
		//lust is immediately altered the moment the game starts, and fatigue is reset to 0.
		//public ushort? lust;
		//public byte? fatigue;
		//hidden stat.
		public byte? fertility;


		public ushort initialXP = 0;
		public ushort initialLevel = 1; //probably could be a byte.
		protected CombatCreatureCreator(string name) : base(name)
		{

		}
	}
}
