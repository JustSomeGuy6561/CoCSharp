using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public sealed class BodyPartChangedEventArg<DataClass, BehaviorClass> : EventArgs 
		where DataClass : BehavioralPartDataBase<BehaviorClass> 
		where BehaviorClass : BehaviorBase
	{
		public readonly DataClass data;
		public readonly BehaviorClass oldValue;
		public readonly BehaviorClass newValue;

		internal BodyPartChangedEventArg(DataClass dataClass, BehaviorClass oldBehavior, BehaviorClass newBehavior)
		{
			data = dataClass;
			oldValue = oldBehavior;
			newValue = newBehavior;
		}
	}
}
