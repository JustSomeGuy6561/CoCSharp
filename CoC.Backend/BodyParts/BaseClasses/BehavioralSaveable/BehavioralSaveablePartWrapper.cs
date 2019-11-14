using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public abstract class BehavioralSaveablePartWrapper<ThisClass, SourceClass, BehaviorClass> : PartWithBehavioralEventsWrapper<ThisClass, SourceClass, BehaviorClass>
		where ThisClass : BehavioralSaveablePartWrapper<ThisClass, SourceClass, BehaviorClass>
		where SourceClass : BehavioralSaveablePart<SourceClass, BehaviorClass, ThisClass>
		where BehaviorClass : SaveableBehavior<BehaviorClass, SourceClass, ThisClass>

	{
		private protected BehavioralSaveablePartWrapper(SourceClass source) : base(source)
		{
		}

		public string LongDescription() => sourceData.LongDescription();

		public string PlayerDescription() => sourceData.PlayerDescription();

		public string TransformIntoText(BehaviorClass newBehavior) => sourceData.TransformIntoText(newBehavior);

		public string RestoreText() => sourceData.RestoreText();


		
		//a full description of this part, with flavor text. it will be called whenever the player asks for their description.
	}
}
