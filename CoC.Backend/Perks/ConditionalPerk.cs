using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Perks
{
	//A conditional perk is different than a standard perk in that it is always 'active', but the creature may not have it, anyway.
	//A creature has a conditional perk when the requirements for that perk are met, and loses it when those conditions are no longer met.
	//To do this, it always remains active, and listens to see if whatever trigger condition has procced. Its OnCreate and OnDestroy are different than
	//the standard perk variant as it's assumed that these will only be created or destroyed when the creature they are attached to (via perk collection)
	//is also being created or destroyed. This fundamental difference in design is why they are coded differently.
	public abstract class ConditionalPerk : PerkBase
	{

		protected bool currentlyEnabled;

		protected ConditionalPerk()
		{
		}

		private protected override bool enabled => currentlyEnabled;

		private protected override void OnCreate()
		{
			SetupActivationConditions();
		}

		private protected override void OnDestroy()
		{
			RemoveActivationConditions();
		}

		private protected override bool retainOnAscension => false;

		//this realistically could be done in the constructor, but this more or less forces a user to do it.
		protected abstract void SetupActivationConditions();
		protected abstract void RemoveActivationConditions();
	}
}
