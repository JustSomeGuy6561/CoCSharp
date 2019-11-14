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
	public abstract class PartWithBehavioralEventsBase<ThisClass, BehaviorClass, WrapperClass> : BehavioralPartBase<ThisClass, BehaviorClass, WrapperClass>, IBehavioralBodyPart
		where ThisClass: PartWithBehavioralEventsBase<ThisClass, BehaviorClass, WrapperClass> 
		where BehaviorClass : BehaviorBase
		where WrapperClass : PartWithBehavioralEventsWrapper<WrapperClass, ThisClass, BehaviorClass>
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

		
		private protected PartWithBehavioralEventsBase(Guid CreatureID)
		{
			creatureID = CreatureID;
		}

		protected readonly WeakEventSource<BodyPartChangedEventArg<ThisClass, BehaviorClass, WrapperClass>> typeChangeSource =
			new WeakEventSource<BodyPartChangedEventArg<ThisClass, BehaviorClass, WrapperClass>>();

		public event EventHandler<BodyPartChangedEventArg<ThisClass, BehaviorClass, WrapperClass>> typeChanged
		{
			add => typeChangeSource.Subscribe(value);
			remove => typeChangeSource.Unsubscribe(value);
		}

		protected void NotifyTypeChanged(BehaviorClass previousType)
		{
			typeChangeSource.Raise(this, new BodyPartChangedEventArg<ThisClass, BehaviorClass, WrapperClass>(AsReadOnlyReference(), previousType, type));
		}

		//protected readonly WeakEventSource<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>> dataChangeSource =
		//	new WeakEventSource<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>>();

		//public event EventHandler<BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>> dataChanged
		//{
		//	add => dataChangeSource.Subscribe(value);
		//	remove => dataChangeSource.Unsubscribe(value);
		//}

		//protected void NotifyDataChanged(DataClass oldData)
		//{
		//	dataChangeSource.Raise(this, new BehavioralDataChangeEvent<ThisClass, BehaviorClass, DataClass>(oldData, AsReadOnlyReference()));
		//}

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
			return typeof(WrapperClass);
		}
	}
}
