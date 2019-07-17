//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

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


	public abstract class BehavioralSaveablePart<ThisClass, BehaviorClass> : BehavioralPartBase<BehaviorClass> where ThisClass : BehavioralSaveablePart<ThisClass, BehaviorClass> where BehaviorClass : SaveableBehavior<BehaviorClass, ThisClass>
	{

		protected BehavioralSaveablePart()
		{
			//DataContractSystem.AddSurrogateData(this);
		}

		//standard implementations.
		//public static BehaviorType defaultType;
		public abstract bool isDefault { get; }

		//statics cannot be created here, but they'll appear as follows

		//internal static ThisClass GenerateDefault();
		//internal static ThisClass GenerateDefaultOfType(BehaviorClass type);

		//internal static ThisClass Generate[Special Name](BehaviorClass type, [additional parameters]);
		//internal static bool Update[Special Name](BehaviorClass type, [additional parameters]);
		//(optional) internal static bool Change[Special Name]([additional parameters]);

		//statics will work as follows: they will immediately return false if the type is identical
		//otherwise, they will do all the update stuff and return true.
		//if there are addition parameters that can be set via update, you MUST implement the change functions
		//so that these can be set if the type is identical and update will immediately return false.

		internal abstract bool UpdateType(BehaviorClass newType);

		internal abstract bool Restore();

		internal abstract bool Validate(bool correctInvalidData);

		//Text output.
		public virtual SimpleDescriptor fullDescription => () => type.fullDescription((ThisClass)this);

		public virtual PlayerStr playerDescription => (player) => type.playerDescription((ThisClass)this, player);
		public virtual ChangeStr<BehaviorClass> transformInto => (newBehavior, player) => newBehavior.transformFrom((ThisClass)this, player);
		public virtual RestoreStr restoreString => (player) => type.restoreString((ThisClass)this, player);

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
