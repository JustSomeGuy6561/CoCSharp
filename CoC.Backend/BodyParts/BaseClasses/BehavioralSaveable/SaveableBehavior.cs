//BodyPartBehavior.cs
//Description: The base class for implementing how a body part behaves.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using System;

namespace CoC.Backend.BodyParts
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

	public abstract class SaveableBehavior<ThisClass, ContainerClass, DataClass> : BehaviorBase
		where ThisClass : SaveableBehavior<ThisClass, ContainerClass, DataClass> 
		where ContainerClass : BehavioralSaveablePart<ContainerClass, ThisClass, DataClass>
		where DataClass: BehavioralSaveablePartData<DataClass, ContainerClass, ThisClass>
	{


		//Function pointers. Wooo!
		//but they make the code significantly shorter and at the same time way more
		//functional. with these, it's possible (and easier) to support language packs.

		//a short description saying the race and type. ex: Hands.CAT: "cat paws"
		//The full description of this part. 
		public readonly DescriptorWithArg<ContainerClass> longDescription;
		//a full description of this part, with flavor text. it will be called whenever the player asks for their description.
		public readonly PlayerBodyPartDelegate<ContainerClass> playerDescription;

		//transform from: take old data, which was the previous settings. new data can be obtained from player if needed.
		//called on the behavior we transformed into.
		public readonly ChangeType<DataClass> transformFrom;

		//this is called on the behavior we transformed from. 
		//requires any old data. it should know how it restores the old data based on what it does internally, but if needed it can just get 
		//any new data from hte player passed in. 
		public readonly RestoreType<DataClass> restoredString;

		private protected SaveableBehavior(SimpleDescriptor shortDesc, DescriptorWithArg<ContainerClass> fullDesc,
			PlayerBodyPartDelegate<ContainerClass> playerDesc, ChangeType<DataClass> transform, RestoreType<DataClass> restore) : base(shortDesc)
		{
			longDescription = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));
			playerDescription = playerDesc ?? throw new ArgumentNullException(nameof(playerDesc));
			transformFrom = transform ?? throw new ArgumentNullException(nameof(transform));
			restoredString = restore ?? throw new ArgumentNullException(nameof(restore));
		}

	}
}
