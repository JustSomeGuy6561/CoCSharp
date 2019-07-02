//PlayerCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:13 PM

namespace CoC.Backend.Creatures
{
	public class PlayerCreator : CombatCreatureCreator
	{
		public int gems = 0;

		//public WombType wombType;

		public PlayerCreator(string name) : base(name) { }
	}
}
