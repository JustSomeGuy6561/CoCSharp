//PerkBase.cs
//Description:
//Author: JustSomeGuy
//6/30/2019, 6:57 PM
using CoC.Backend.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//each perk is to be declared as its own class. we use class type to determine if you have a perk or not, not instances. we do this because Perks are NOT static, and we need an easy
	//way to ask if the character has a perk without needing to allocate a new instance. This allows perks to do anything, and have any variables and helper functions they need.
	//get perk will return the Type you give it, so you can then use these helper functions or update these variables without needing to cast or do typechecking. 
	//one downside is we don't need a "PerkLib" anymore, which means perks can come from anywhere. This makes it far more flexible, but also means they can be anywhere (more difficult to debug)
	//Also, while the game is open source and anyone can build their own version with a god mode or whatever they like, it tends to be complicated and thus dissuade this type of behavior.
	//A downside to this approach is it's really easy to add perks now, because there's no validation layer that PerkLib provided. 

	public abstract class PerkBase
	{
		//the creature that has this perk. Note that this is NULL until the perk is activated by the creature.
		protected CombatCreature sourceCreature { get; private set; } = null;
		//Giving you these means you have access to all the perks the creature currently has, and the base stat multipliers. They should only be used in this context; don't misuse this.
		protected PerkCollection basicData => sourceCreature.perks; //gives you access to hasPerk. allows you to hard-code other checks, to prevent mutual exclusives.
		protected PassiveBaseStatModifiers baseModifiers => basicData.baseModifiers; //allows you to update base stats. 

		internal void Activate(CombatCreature source)
		{
			sourceCreature = source;
			OnActivation();
		}

		internal void Deactivate()
		{
			OnRemoval();
			sourceCreature = null;
		}

		//called when the perk is added to the perk collection on the character. sourceCreature is guarenteed to be NOT NULL by this point.
		protected abstract void OnActivation();

		//called when the perk is removed from the perk collection. sourceCreature is guarenteed to be NOT NULL.
		//after this is called, sourceCreature WILL BE NULL. 
		protected internal abstract void OnRemoval();
	}
}
