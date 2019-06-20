using System;
using System.Collections.Generic;
using System.Text;

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
