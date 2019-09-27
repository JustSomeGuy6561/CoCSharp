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
		private protected BehavioralSaveablePartData(Guid creatureID, BehaviorClass currentBehavior) : base(creatureID, currentBehavior)
		{
		}

		protected static Guid GetID(SourceClass source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.creatureID;
		}

		protected static BehaviorClass GetBehavior(SourceClass source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.type;
		}
	}
}
