//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using CoC.Backend.BodyParts.BaseClasses;
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Creatures;
using System;
using System.Runtime.Serialization;
using WeakEvent;

namespace CoC.Backend.BodyParts
{	
	public abstract class PartWithBehaviorAndEventBase<ThisClass, BehaviorClass, DataClass> : BehavioralPartBase<BehaviorClass, DataClass>, IBehavioralBodyPart
		where ThisClass: PartWithBehaviorAndEventBase<ThisClass, BehaviorClass, DataClass> 
		where BehaviorClass : BehaviorBase
		where DataClass : BehavioralPartDataBase<BehaviorClass>
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

		
		private protected PartWithBehaviorAndEventBase(Guid CreatureID)
		{
			creatureID = CreatureID;
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
	}
}
