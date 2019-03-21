//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.Save;
using System;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	/*
	 * base class for all body parts. a new instance of this class will exist for every creature.
	 * it is made of two parts: the unique variables that make up this body type (like length for neck)
	 * and the ruleset or behavior that determines how these unique variables change when something happens
	 * for example: when you tell the neck to grow, the behavior will either allow or not allow it, and
	 * change the neck length value accordingly
	 */

	//super confusing generics ftw! Basically, you have to define what it is, and what it's behavior is.
	//TL;DR: BodyPart<[BodyPart], [BodyPartType]> 
	//example: 
	//internal class Arms : BodyPartBase<Arms, ArmType>
	//internal class ArmType : BodyPartBehavior<ArmType, Arms>

	[DataContract]
	public abstract class BodyPartBase<ThisClass, BehaviorClass> : ISaveableBase where ThisClass : BodyPartBase<ThisClass, BehaviorClass> where BehaviorClass : BodyPartBehavior<BehaviorClass, ThisClass>
	{
		//standard implementations.
		public abstract BehaviorClass type { get; protected set; }
		internal abstract bool Restore();

		public abstract bool isDefault { get; }


		//These probably will never be overridden, but w/e.
		public virtual int index => type.index;

		//Text output.
		public virtual SimpleDescriptor shortDescription => type.shortDescription;
		public virtual SimpleDescriptor fullDescription => () => type.fullDescription((ThisClass)this);

		public virtual PlayerStr playerDescription => (player) => type.playerDescription((ThisClass)this, player);
		public virtual ChangeStr<BehaviorClass> transformInto => (newBehavior, player) => newBehavior.transformFrom((ThisClass)this, player);
		public virtual RestoreStr restoreString => (player) => type.restoreString((ThisClass)this, player);

		//Serialization
		Type ISaveableBase.currentSaveType => currentSaveVersion;
		Type[] ISaveableBase.saveVersionTypes => saveVersions;
		object ISaveableBase.ToCurrentSaveVersion()
		{
			return ToCurrentSave();
		}

		internal abstract Type currentSaveVersion { get; }
		internal abstract Type[] saveVersions { get; }

		internal abstract BodyPartSurrogate<ThisClass, BehaviorClass> ToCurrentSave();
	}
}
