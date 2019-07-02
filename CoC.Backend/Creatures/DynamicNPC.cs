//DynamicNPC.cs
//Description:
//Author: JustSomeGuy
//6/28/2019, 8:30 PM
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Creatures.Creators
{
	//NOTE: This is 100% not implemented or tested in game. but if you want dynamic NPCs that exist in game (generally Randomly or Proceduraly generated)
	//that have their own adventures (or simulated ones, again random or procedural), and therefore react to certain things (like being pregnant or eating a TF item or whatever)
	//feel free to use this. Note that if you do you're essentially creating a whole new experience, and that's frankly a lot of work. Not gonna lie, that'd be cool as hell, but 
	//it's well out of my skillset, so you'd well and truly be on your own. 

	public class DynamicNPC : Creature
	{
		public DynamicNPC(CreatureCreator creator) : base(creator)
		{
			//now set up all the listeners.
			//if any listeners are DynamicNPC specific, add them here.

			//then activate them. 
			//occurs AFTER the creature constructor, so we're fine.
			ActivateTimeListeners();
		}
	}
}
