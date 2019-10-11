//DynamicNPC.cs
//Description:
//Author: JustSomeGuy
//6/28/2019, 8:30 PM
using CoC.Backend.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures
{
	//NOTE: This is untested, but should work. This would allow you to create NPCs that can update or change, just like the player. It'd be useful to simulate
	//other, random inhabitants of Mareth or possibly other adventurers. As of now, NPCs don't need this, as they can just be hard-coded or "hacked" in. 
	//For example: We always know what Urta looks like, so we don't need the whole creature class for her - maybe just the Genitals class if we want to use it for fertility and such.
	//as such, we can get away with describing Urta manually, as we aren't going to use the generic text anyway. 
	//Even the more complicated cases, like Katherine or Rubi, which do change appearance based on what the PC gives them, can be stored in a few variables and parsed manually.
	//Of course, if doing so is too complicated (katherine, for example, is a complicated mess to the uninformed), you can always use this - the cost really isn't that bad, all things considered. 
	//Alternatively, you may add a character that is genuinely dynamic - they may completely change their appearance over time, possibly w/o the player causing it - that would likely require this.
	//Note, you can derive this class and add stuff beyond the base creature class, like a custom womb, for example. 

	public class DynamicNPC : Creature
	{
		public DynamicNPC(DynamicNPC_Creator creator) : base(creator)
		{
			//now set up all the listeners.
			//if any listeners are DynamicNPC specific, add them here.

			//then activate them. 
			//occurs AFTER the creature constructor, so we're fine.
			UnFreezeCreature();
		}

		protected override string PlaceItemInCreatureStorageText(CapacityItem item, byte slot)
		{
			return "The NPC places the " + item.shortName() + " in its " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReturnItemToCreatureStorageText(CapacityItem item, byte slot)
		{
			return "The NPC returns the " + item.shortName() + " to its " + Tools.Utils.NumberAsPlace(slot) + " pouch. ";
		}

		protected override string ReplaceItemInCreatureStorageWithNewItemText(CapacityItem newItem, byte slot)
		{
			return "The NPC replaces the " + inventory[slot].item.shortName() + " in its " + Tools.Utils.NumberAsPlace(slot) + " pouch with " + newItem.shortName() + ". ";
		}
	}
}
