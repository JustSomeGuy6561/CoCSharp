//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System;
using WeakEvent;

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


	public abstract class BehavioralSaveablePart<ThisClass, BehaviorClass, DataClass> : PartWithBehaviorAndEventBase<ThisClass, BehaviorClass, DataClass>
		where ThisClass : BehavioralSaveablePart<ThisClass, BehaviorClass, DataClass> where BehaviorClass : SaveableBehavior<BehaviorClass, ThisClass, DataClass>
		where DataClass : BehavioralSaveablePartData<DataClass, ThisClass, BehaviorClass>
	{
		private protected BehavioralSaveablePart(Guid creatureID) : base(creatureID)
		{
		}


		//standard implementations.
		//public static BehaviorType default[Name] => <value>;
		public bool isDefault => type == defaultType;
		public abstract BehaviorClass defaultType { get; }

		//statics cannot be created here, but they'll appear as follows

		//internal static ThisClass GenerateDefault();
		//internal static ThisClass GenerateDefaultOfType(BehaviorClass type);

		//internal static ThisClass Generate[Special Name](BehaviorClass type, [additional parameters]);

		//each class may have additional updates for more specific or varied cases, notably cases that use extra variables unique to that class. 
		//if this is the case, functions that can change these variables without updating the type may need to be implemented.

		//internal bool Update[Special Name](BehaviorClass type, [additional parameters]);
		//(optional) internal bool Change[Special Name]([additional parameters]);

		internal virtual bool Restore()
		{
			return UpdateType(defaultType);
		}

		internal abstract bool Validate(bool correctInvalidData);

		public virtual bool CanChangeTo(BehaviorClass newType)
		{
			return newType != type;
		}

		//Text output.
		public virtual SimpleDescriptor fullDescription => () => type.fullDescription((ThisClass)this);

		public virtual string PlayerDescription()
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is Player player)
			{
				return type.playerDescription((ThisClass)this, player);
			}
			else return "";
		}
		public virtual string TransformIntoText(BehaviorClass newBehavior)
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is Player player)
			{
				return newBehavior.transformFrom((ThisClass)this, player);
			}
			else return "";
		}
		public virtual string RestoreText()
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is Player player)
			{
				return type.restoreString((ThisClass)this, player);
			}
			else return "";
		}



		//Serialization
		//Type ISaveableBase.currentSaveType => currentSaveVersion;
		//Type[] ISaveableBase.saveVersionTypes => saveVersions;
		//object ISaveableBase.ToCurrentSaveVersion()
		//{
		//	return ToCurrentSave();
		//}

		//private protected abstract Type currentSaveVersion { get; }
		//private protected abstract Type[] saveVersions { get; }

		//private protected abstract BehavioralSurrogateBase<ThisClass, BehaviorClass> ToCurrentSave();
	}
}
