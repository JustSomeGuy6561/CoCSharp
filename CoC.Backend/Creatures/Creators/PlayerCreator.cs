using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	public class PlayerCreator : CombatCreatureCreator
	{
		public int gems = 0;
		public PlayerCreator(string name) : base(name)
		{}
	}
}
