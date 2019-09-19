using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts.EventHelpers
{
	public class BehavioralDataChangeEvent<Source, Behavior, Data> : EventArgs where Source : PartWithBehaviorAndEventBase<Source, Behavior, Data> 
		where Behavior : BehaviorBase where Data : BehavioralPartDataBase<Behavior>
	{
		public readonly Data oldValues;
		public readonly Data newValues;

		public BehavioralDataChangeEvent(Data oldData, Data newData)
		{
			oldValues = oldData ?? throw new ArgumentNullException(nameof(oldData));
			newValues = newData ?? throw new ArgumentNullException(nameof(newData));
		}
	}
}
