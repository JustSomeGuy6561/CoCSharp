//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Creatures;
using System;
using System.Runtime.Serialization;
using WeakEvent;

namespace CoC.Backend.BodyParts
{	
	public abstract class PartWithBehaviorAndEventBase<ThisClass, BehaviorClass, DataClass> : BehavioralPartBase<BehaviorClass, DataClass> 
		where ThisClass: PartWithBehaviorAndEventBase<ThisClass, BehaviorClass, DataClass> where BehaviorClass : BehaviorBase
		where DataClass : BehavioralPartDataBase<BehaviorClass>
	{
		private protected readonly Creature source;

		protected void NotifyTypeChanged(BehaviorClass previousType)
		{
			typeChangeSource.Raise(source, new BodyPartChangedEventArg<DataClass, BehaviorClass>(AsReadOnlyData(), previousType, type));
		}

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

		private protected PartWithBehaviorAndEventBase(Creature parent)
		{
			source = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		public event EventHandler<BodyPartChangedEventArg<DataClass, BehaviorClass>> typeChanged
		{
			add => typeChangeSource.Subscribe(value);
			remove => typeChangeSource.Unsubscribe(value);
		}

	}
}
