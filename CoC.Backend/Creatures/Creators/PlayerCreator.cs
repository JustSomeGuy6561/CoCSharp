using CoC.Backend.BodyParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	public class PlayerCreator : CombatCreatureCreator
	{
		public int gems = 0;

		public WombType wombType;

		public PlayerCreator(string name) : base(name) {}
	}
}
