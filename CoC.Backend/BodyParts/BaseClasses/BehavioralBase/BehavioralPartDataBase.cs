//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using System;

namespace CoC.Backend.BodyParts
{
	public abstract class BehavioralPartDataBase<BehaviorClass> where BehaviorClass : BehaviorBase
	{
		public readonly BehaviorClass currentType;
		private protected BehavioralPartDataBase(BehaviorClass currentBehavior)
		{
			currentType = currentBehavior ?? throw new ArgumentNullException(nameof(currentBehavior));
		}
	}
}