//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/31/2018, 12:35 AM

//BodyPartBase.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Tools;
using System.ComponentModel;

namespace CoC.BodyParts
{
	/*
	 * base class for all body parts. a new instance of this class will exist for every creature.
	 * it is made of two parts: the unique variables that make up this body type (like length for neck)
	 * and the ruleset or behavior that determines how these unique variables change when something happens
	 * for example: when you tell the neck to grow, the behavior will either allow or not allow it, and
	 * change the neck length value accordingly
	 */

	//super confusing generics ftw! Basically, you have to define what it is, and what it's behavior is.
	//TL;DR: [BodyPart]<[Body Part], [BodyPartType]> 
	//example: 
	//public class Arms : BodyPartBase<Arms, ArmType>
	//public class ArmType : BodyPartBehavior<ArmType, Arms>
	
	public abstract class BodyPartBase<ThisClass,BehaviorClass> where ThisClass : BodyPartBase<ThisClass, BehaviorClass> where BehaviorClass : BodyPartBehavior<BehaviorClass, ThisClass>
	{
		public abstract void Restore();
		public abstract void RestoreAndDisplayMessage(Player player);

		public abstract BehaviorClass type { get; protected set; }

		//there may be cases where you need to know the length of the hair or something - something not stored 
		//not stored in the immutable part. you can override these to do so.
		public virtual int index => type.index;
		public virtual CreatureDescription<ThisClass> creatureDescription => (bodyPart, gender) => type.creatureDescription(bodyPart, gender);

		public virtual PlayerDescription<ThisClass> playerDescription => (bodyPart, player) => type.playerDescription(bodyPart, player);
		public virtual GenericDescription shortDescription => type.shortDescription;
		public virtual ChangeType<ThisClass> transformFrom => (prevPart, player) => type.transformFrom(prevPart.type, player);

		public virtual ChangeType<ThisClass> restoreString => (currentPart, player) => type.restoreString(currentPart.type, player);

	}
}
