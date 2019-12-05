﻿//SimpleBodyPart.cs
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

		public virtual string ShortDescription() => type.ShortDescription();

		protected internal virtual void PostPerkInit()
		{ }

		protected internal virtual void LateInit()
		{ }

		private protected BehavioralPartBase()
		{ }

	}
}