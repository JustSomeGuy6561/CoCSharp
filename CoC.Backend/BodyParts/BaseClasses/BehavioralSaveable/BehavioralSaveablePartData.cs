using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public abstract class BehavioralSaveablePartData<ThisClass, SourceClass, BehaviorClass> : BehavioralPartDataBase<BehaviorClass>
		where ThisClass : BehavioralSaveablePartData<ThisClass, SourceClass, BehaviorClass>
		where SourceClass : BehavioralSaveablePart<SourceClass, BehaviorClass, ThisClass>
		where BehaviorClass : SaveableBehavior<BehaviorClass, SourceClass, ThisClass>

	{
		private protected BehavioralSaveablePartData(BehaviorClass currentBehavior) : base(currentBehavior)
		{
		}

		protected static BehaviorClass GetBehavior(SourceClass source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.type;
		}
	}
}
