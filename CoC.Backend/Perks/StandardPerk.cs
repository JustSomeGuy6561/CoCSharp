using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//This is a 'standard' perk, or a perk that is obtained and removed based on the actions the creature chooses (or in the case of leveling, what the PC picks).
	//These perks are not lost over time, and generally are not lost automatically. these perks must be obtained manually (aka they must be granted via code, not automatically)
	public abstract class StandardPerk : PerkBase
	{
		//as long as a standard perk is attached to a creature, it's enabled
		private protected override bool enabled => !(sourceCreature is null);

		private protected override void OnCreate()
		{
			OnActivation();
		}

		private protected override void OnDestroy()
		{
			OnRemoval();
		}

		private protected override bool retainOnAscension => keepOnAscension;

		protected internal abstract bool keepOnAscension { get; }


		//called when the perk is added to the perk collection on the character. sourceCreature is guarenteed to be NOT NULL by this point.
		protected abstract void OnActivation();

		//called when the perk is removed from the perk collection. sourceCreature is guarenteed to be NOT NULL.
		//after this is called, sourceCreature WILL BE NULL.
		protected internal abstract void OnRemoval();
	}
}
