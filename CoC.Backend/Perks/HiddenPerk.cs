using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//Hidden perks aren't attainable via leveling. They don't require any sort of unlock specific text. 

	public abstract class HiddenPerk : PerkBase
	{
		public HiddenPerk(SimpleDescriptor perkName, SimpleDescriptor perkHint) : base(perkName, perkHint)
		{
		}
	}
}
