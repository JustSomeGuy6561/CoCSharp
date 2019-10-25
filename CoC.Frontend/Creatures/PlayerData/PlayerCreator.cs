using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.PlayerData
{
	public sealed class PlayerCreator : PlayerCreatorBase
	{
		public PlayerCreator(string name) : base(name)
		{
		}

		public override Womb GetWomb(Guid creatureID)
		{
			return new PlayerWomb(creatureID);
		}
	}
}
