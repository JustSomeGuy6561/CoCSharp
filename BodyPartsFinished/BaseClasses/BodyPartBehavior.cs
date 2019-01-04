//BodyPartBehavior.cs
//Description: The base class for implementing how a body part behaves.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM
using CoC.Tools;
namespace CoC.BodyParts
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
		public readonly GenericDescription shortDescription;
		//a description of this part. Any creature who has this part could call this and
		//it could be used in a sentence. No "you have/are/etc"
		public readonly CreatureDescription<ContainerClass> creatureDescription;
		//a description of this part, unique to the player. It should be completely self-contained
		//
		public readonly PlayerDescription<ContainerClass> playerDescription;

		public readonly ChangeType<ContainerClass> transformFrom;
		public readonly ChangeType<ContainerClass> restoreString;

		protected BodyPartBehavior(GenericDescription shortDesc, CreatureDescription<ContainerClass> creatureDesc,
			PlayerDescription<ContainerClass> playerDesc, ChangeType<ContainerClass> transform, ChangeType<ContainerClass> restore)
		{
			shortDescription = shortDesc;
			creatureDescription = creatureDesc;
			playerDescription = playerDesc;
			transformFrom = transform;
			restoreString = restore;
		}

	}
}
