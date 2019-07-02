//MonsterCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:13 PM

namespace CoC.Backend.Creatures
{
	public sealed class MonsterCreator : CombatCreatureCreator
	{
		public ushort maxLust = 100;
		public byte maxLibido = 100;
		public byte maxCorruption = 100;

		public MonsterCreator(string name) : base(name)
		{

		}
	}
}
