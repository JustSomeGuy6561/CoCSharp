//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using System;

namespace CoC.Backend.BodyParts
{
	public abstract class BehavioralPartDataBase<BehaviorClass> where BehaviorClass : BehaviorBase
	{
		public readonly BehaviorClass type;
		public readonly Guid creatureID;

		public virtual string ShortDescription() => type.shortDescription();

		private protected BehavioralPartDataBase(Guid creatureID, BehaviorClass currentBehavior)
		{
			type = currentBehavior ?? throw new ArgumentNullException(nameof(currentBehavior));
			this.creatureID = creatureID;
		}
	}
}
