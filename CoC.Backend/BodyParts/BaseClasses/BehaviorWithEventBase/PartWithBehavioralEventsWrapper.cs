using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public abstract class PartWithBehavioralEventsWrapper<ThisClass, SourceClass, BehaviorClass> : BehavioralWrapperBase<ThisClass, SourceClass, BehaviorClass>
		where ThisClass : PartWithBehavioralEventsWrapper<ThisClass, SourceClass, BehaviorClass>
		where SourceClass : PartWithBehavioralEventsBase<SourceClass, BehaviorClass, ThisClass>
		where BehaviorClass : BehaviorBase
	{
		public Guid creatureID => sourceData.creatureID;

		private protected PartWithBehavioralEventsWrapper(SourceClass source) : base(source)
		{
		}
	}
}
