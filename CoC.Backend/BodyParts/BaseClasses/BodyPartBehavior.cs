﻿//BodyPartBehavior.cs
//Description: The base class for implementing how a body part behaves.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Tools;

namespace  CoC.Backend.BodyParts
{
	/* Behavior or ruleset base class.
	 * All body parts are made up of two things: a "ruleset", and then variables
	 * such as length or style that are unique to each body part.
	 * the "ruleset" defines how the body part behaves - if/how it grows, if/how it reacts
	 * to changes in hair color or skin tone, etc. The "ruleset" is defined as an 
	 * BodyPartBehavior. the bodypart itself is a BodyPart
	 * "rulesets" are just that - they don't change, their internal variables are constant
	 * a "ruleset" accepts a variable and some conditions, then changes that variable based
	 * on what rules it has and what the conditions are. 
	 * For Example: 
	 * you have a dragon neck, and want to make it grow. 
	 * 1) You call your neck's growth function, passing in your current length by reference
	 * 2) the the dragon neck behavior takes your value, and determines how to change it.
	 * 3) it then sets your neck length, and returns.
	 */

	public abstract class BodyPartBehavior<ThisClass, ContainerClass> where ThisClass : BodyPartBehavior<ThisClass, ContainerClass> where ContainerClass : BodyPartBase<ContainerClass, ThisClass>
	{
		
		public abstract int index { get; }

		//Function pointers. Wooo!
		//but they make the code significantly shorter and at the same time way more
		//functional. with these, it's possible (and easier) to support language packs.

		//a short description saying the race and type. ex: Hands.CAT: "cat paws"
		public readonly SimpleDescriptor shortDescription;
		//The full description of this part. 
		public readonly DescriptorWithArg<ContainerClass> fullDescription;
		//a full description of this part, with flavor text. it will be called whenever the player asks for their description.
		public readonly TypeAndPlayerDelegate<ContainerClass> playerDescription;

		public readonly ChangeType<ContainerClass> transformFrom;
		public readonly RestoreType<ContainerClass> restoreString;

		private protected BodyPartBehavior(SimpleDescriptor shortDesc, DescriptorWithArg<ContainerClass> fullDesc,
			TypeAndPlayerDelegate<ContainerClass> playerDesc, ChangeType<ContainerClass> transform, RestoreType<ContainerClass> restore)
		{
			shortDescription = shortDesc;
			fullDescription = fullDesc;
			playerDescription = playerDesc;
			transformFrom = transform;
			restoreString = restore;
		}

	}
}
