using CoC.Backend.Creatures;
using CoC.Backend.Perks;
using CoC.Frontend.Perks;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Frontend.Creatures.NPCs
{

	public class DynamicNPC : DynamicNPCBase, IExtendedCreature
	{
		public ExtraPerkCollection extendedPerks => perks as ExtraPerkCollection;

		public DynamicNPC(DynamicNPC_Creator creator) : base(creator, x=> new ExtraPerkCollection(x))
		{
		}
	}
}
