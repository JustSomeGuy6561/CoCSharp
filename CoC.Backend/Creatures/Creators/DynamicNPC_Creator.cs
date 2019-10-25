//PlayerCreator.cs
//Description:
//Author: JustSomeGuy
//3/22/2019, 6:13 PM

using System;
using CoC.Backend.BodyParts;

namespace CoC.Backend.Creatures
{
	public abstract class PlayerCreatorBase : CombatCreatureCreator
	{
		public int gems = 0;

		//public WombType wombType;

		public PlayerCreatorBase(string name) : base(name) { }
	}
}
