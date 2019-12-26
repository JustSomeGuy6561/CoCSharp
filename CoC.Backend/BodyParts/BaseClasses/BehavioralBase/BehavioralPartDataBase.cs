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

		//describes the entire body part, concisely. if the body part is made up of multiple members (i.e. the wing part is made of 2 wings), this is plural.
		//This is virtual because there may be some types where the behavior class cannot determine whether or not it has multiple members unless additional information is
		//given to it. Notable examples are horns and tails, as some types can have varying amounts based on player choice or NPC design.
		public virtual string ShortDescription() => type.ShortDescription();

		//variant of the short description that will always be singular, even if the body part has multiple members. additionally, it will have a unique format specifically
		//for this singular description.
		//in english, ths includes the article necessary for the text. (i.e. "a dragon wing" or "an imp wing"). This means you can just call this and not have to worry about
		//edge cases that would make the grammar sound bad, or need to manually parse it (even worse imo).
		public virtual string ShortSingleItemDescription() => type.ShortSingleItemDescription();

		private protected BehavioralPartDataBase(Guid creatureID, BehaviorClass currentBehavior)
		{
			type = currentBehavior ?? throw new ArgumentNullException(nameof(currentBehavior));
			this.creatureID = creatureID;
		}
	}
}
