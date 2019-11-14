//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using System;

namespace CoC.Backend.BodyParts
{
	public abstract class BehavioralWrapperBase<ThisClass, SourceClass, BehaviorClass>
		where ThisClass : BehavioralWrapperBase<ThisClass, SourceClass, BehaviorClass>
		where BehaviorClass : BehaviorBase
		where SourceClass : BehavioralPartBase<SourceClass, BehaviorClass, ThisClass>
	{

		protected readonly SourceClass sourceData;

		public BehaviorClass type => sourceData.type;

		private protected BehavioralWrapperBase(SourceClass source)
		{
			sourceData = source;
		}

		public string ShortDescription() => sourceData.ShortDescription();
	}
}