using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//name could be changed. an Attainable perk is a perk that can be obtained via leveling. Note that it may be possible to gain this type of perk through gameplay,
	//but because it can be obtained via leveling, it needs text for what it does. 
	public abstract class AttainablePerk : PerkBase
	{
		public AttainablePerk(SimpleDescriptor perkName, SimpleDescriptor perkHint) : base(perkName, perkHint)
		{
		}
	}
}
