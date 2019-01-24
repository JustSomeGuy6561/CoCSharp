//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/31/2018, 12:35 AM

//BodyPartBase.cs
//Description:
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Tools;
using CoC.Creatures;

namespace  CoC.BodyParts
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
		public abstract bool Restore();
		public abstract bool RestoreAndDisplayMessage(Player player);

		public abstract BehaviorClass type { get; protected set; }

		//there may be cases where you need to know the length of the hair or something - something not stored 
		//not stored in the immutable part. you can override these to do so.
		public virtual int index => type.index;
		public virtual SimpleDescriptor shortDescription => type.shortDescription;
		public virtual SimpleDescriptor fullDescription => () => type.fullDescription((ThisClass)this);

		public virtual PlayerStr playerDescription => (player) => type.playerDescription((ThisClass)this, player);
		public virtual ChangeStr<BehaviorClass> transformInto => (newBehavior, player) => newBehavior.transformFrom((ThisClass)this, player);
		public virtual RestoreStr restoreString => (player) => type.restoreString((ThisClass)this, player);

	}
}
