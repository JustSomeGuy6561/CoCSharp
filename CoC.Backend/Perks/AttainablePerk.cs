using System;
using System.Collections.Generic;
using System.Text;
using CoC.Backend.Creatures;

namespace CoC.Backend.Perks
{
	//name could be changed. an Attainable perk is a perk that can be obtained via leveling. Note that it may be possible to gain this type of perk through gameplay,
	//but because it can be obtained via leveling, it needs text for what it does. It's also possible for this type of perk to be stackable (i.e. strong back)
	//hence the implementation as an interface.

	//anything that implements this will need to add itself to the leveling engine so it can handle itself, however, that is not implemented as of this writing.
	//it's technically possible to do via reflection, but i really really hate using reflection for that.

	public interface IAttainablePerk<T> where T: PerkBase
	{
		string AbbreviatedName();

		bool CanUnlock(Creature target);

		string UnlockPerkText();


	}
}
