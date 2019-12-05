using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;


namespace CoC.Frontend.Creatures
{
	//not gonna lie, multiple inheritance (or defaults for interfaces, like in C# 8.0) would be fantastic right about now.

	//provides a common interface for the new features we're baking into the player/monster/npc that is unique to the frontend. since multiple inheritance is out, this is the only way,
	//and it requires some serious code copypasta. right now, the c# 8.0 default for interfaces seems really useful, but i'm cautious to force another c# version upgrade.

	public interface IExtendedCreature
	{
		ExtendedCreatureData extendedData { get; }

		ExtendedPerkModifiers extendedPerkModifiers { get; }
	}
}
