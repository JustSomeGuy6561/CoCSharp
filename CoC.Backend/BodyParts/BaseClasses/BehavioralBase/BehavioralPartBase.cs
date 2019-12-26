//SimpleBodyPart.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using System;
using System.Runtime.Serialization;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	/*
	 * Simple body parts are different from standard ones in that they do not exist on their own -
	 * They must be part of another body part. hands are a part of arms, for example.
	 * Furthermore, they cannot change type. whatever they are attached to knows for a fact they will
	 * always be what they are. Dragon arms will always have dragon claws, for example.
	 * As such, they don't have transform or restore strings, as they are part of something else which
	 * will do that. they dont have appearance strings for the exact same reason.
	 *
	 * They DO, however, have short descriptions, which can be used by whatever they are attached to.
	 * continuing the arms example, claws will return "claws", which can be used when the arm changes, for example.
	 * Ex: dragon arms to human arms: "your " + oldarms.hands.shortDescription() + " have become newarms.hands.shortDescription()"
	 * prints out "your claws have become hands"
	 *
	 * Further, you can add new callbacks or string functions if that body part has some unique feature: anything that uses the epidermis,
	 * for example, may want to print out the skin tone or fur color before returning. again, using hands: "ivory claws" instead of "claws"
	 * I cannot create a template for that, however, as it is unique to that class.
	 */

	//Stores a reference to the simple body part, and any unique variables to that type, like skin tone or fur color for epidermis, etc.
	//so you can attach epidermis to any body part that needs it and not need to deal with anything past keeping it updated.
	//learned this the hard way after having to deal with it in arms. woo!

	public abstract class BehavioralPartBase<BehaviorClass, DataClass> where BehaviorClass : BehaviorBase
		where DataClass : BehavioralPartDataBase<BehaviorClass>
	{

		public abstract BehaviorClass type { get; protected set; }

		public abstract DataClass AsReadOnlyData();


		public virtual int index => type.index;

		//describes the entire body part, concisely. if the body part is made up of multiple members (i.e. the wing part is made of 2 wings), this is plural.
		//This is virtual because there may be some types where the behavior class cannot determine whether or not it has multiple members unless additional information is
		//given to it. Notable examples are horns and tails, as some types can have varying amounts based on player choice or NPC design.

		//additionally, in the cases where this body part is made of multiple members, an overload of will be provided that allows you to specify whether or not you want to describe
		//just one member. this version will not have any special formatting. if you need it with special formatting, use the single item descriptor.
		public virtual string ShortDescription() => type.ShortDescription();

		//variant of the short description that will always be singular, even if the body part has multiple members. additionally, it will have a unique format specifically
		//for this singular description.
		//in english, ths includes the article necessary for the text. (i.e. "a dragon wing" or "an imp wing"). This means you can just call this and not have to worry about
		//edge cases that would make the grammar sound bad, or need to manually parse it (even worse imo).

		//if you do not want the special formatting, use the short description, and specify it as singular if applicable.
		public virtual string ShortSingleItemDescription() => type.ShortSingleItemDescription();

		protected internal virtual void PostPerkInit()
		{ }

		protected internal virtual void LateInit()
		{ }

		private protected BehavioralPartBase()
		{ }

	}
}