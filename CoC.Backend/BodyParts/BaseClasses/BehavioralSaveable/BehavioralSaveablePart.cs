//BodyPartBase.cs
//Description: base class for all body parts. it must have a ruleset attached.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.BodyParts.EventHelpers;
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


	public abstract class BehavioralSaveablePart<ThisClass, BehaviorClass, DataClass>
		: BehavioralPartBase<BehaviorClass, DataClass>, IBehavioralBodyPart, IBodyPart<DataClass>
		where ThisClass : BehavioralSaveablePart<ThisClass, BehaviorClass, DataClass>
		where BehaviorClass : BehaviorBase
		where DataClass : BehavioralSaveableData<DataClass, ThisClass, BehaviorClass>
	{
		public readonly Guid creatureID;

		internal virtual bool UpdateType(BehaviorClass newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			type = newType;

			NotifyTypeChanged(oldValue);
			return true;
		}

		protected readonly WeakEventSource<BodyPartChangedEventArg<DataClass, BehaviorClass>> typeChangeSource =
			new WeakEventSource<BodyPartChangedEventArg<DataClass, BehaviorClass>>();

		public event EventHandler<BodyPartChangedEventArg<DataClass, BehaviorClass>> typeChanged
		{
			add => typeChangeSource.Subscribe(value);
			remove => typeChangeSource.Unsubscribe(value);
		}

		protected void NotifyTypeChanged(BehaviorClass previousType)
		{
			typeChangeSource.Raise(this, new BodyPartChangedEventArg<DataClass, BehaviorClass>(AsReadOnlyData(), previousType, type));
		}

		protected readonly WeakEventSource<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>> dataChangeSource =
			new WeakEventSource<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>>();

		public event EventHandler<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		protected void NotifyDataChanged(DataClass oldData)
		{
			dataChangeSource.Raise(this, new BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>(oldData, AsReadOnlyData()));
		}

		Type IBehavioralBodyPart.BehaviorType()
		{
			return typeof(BehaviorClass);
		}

		public abstract string BodyPartName();

		Type IBodyPart.BaseType()
		{
			return typeof(ThisClass);
		}

		Type IBodyPart.DataType()
		{
			return typeof(DataClass);
		}

		Guid IBodyPart.creatureID => creatureID;



		private protected BehavioralSaveablePart(Guid creatureID)
		{
			this.creatureID = creatureID;
		}


		//standard implementations.
		//public static BehaviorType default[Name] => <value>;


		internal abstract bool Validate(bool correctInvalidData);

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
