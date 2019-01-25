I know that both base classes BaseBodyPart and BodyPartBehavior are super confusing to look at.
TL;DR Version: they just work (TM). trust me. A new body part looks like this 
	class NewBodyPart :  BaseBodyPart<NewBodyPart, NewBodyPartType> 
	{
		//Your content here.
		//BaseBodyPart implementation stuff here.

		//any calls to type specific logic here.
	}

	class NewBodyPartType : BodyPartBehavior<NewBodyPartType, NewBodyPart>
	{
		//implement the logic for the behavior/ruleset here.
		//any type specific logic goes here. 
	}
	note that if they are used outside of where you create them, you may need to make them public

for the full version, here's what they do:

Let's look at the header for the BodyPartBase :
internal abstract class BodyPartBase<ThisClass, BehaviorClass> where ThisClass : BodyPartBase<ThisClass, BehaviorClass> where BehaviorClass : BodyPartBehavior<BehaviorClass, ThisClass>

This is a generic class, because it has <ThisClass, BehaviorClass>. Basically, we need to define functions that use variables, but the type they are depends on what this class is. Similarly, we need to know where our behaviors are defined, and that's different for each body part class. 
BUT, generics are tricky if you're not familiar with theim. We could plug anything in and C# wouldn't care. we could tell C# our behavior class is an int (which is okay because Integer is a class), and that's not what we want. Imagine, for example, we told our arms class it's behaviors were defined as an integer. 
that wouldn't work at all! We have to limit the generics to relevant data, and thankfully C# lets us do that, using where clauses.

Thus, the first where clause:
where ThisClass : BodyPartBase<ThisClass, BehaviorClass>

This says that the first type must have the exact same signature as ours does. In other words, This Class. We can now define custom constructors in the abstract class or create callbacks
that use this class in as a parameter. So, a Generate() function in Arms will return Arms, in Legs it will return Legs, and so on.

But, that's not enough. we also need to ensure our ruleset is correct. 

Thus the second where clause:
where BehaviorClass : BodyPartBehavior<BehaviorClass, ThisClass> 

This ensures the second type must be a behavior, and it has to be the behavior for this class. we can now create callback functions in our rulesets that use variables that change, like the length of hair

An added bonus: it makes it nearly impossible to break it if you aren't trying to break it. Generics are tricky if you don't know how they work, but this format is so specific you can only do it one way - the correct one.
If you do know what you're doing, you might be able to work around them, but at that point you don't need this explanation, anyway. 

WHAT ABOUT A SUB BASE TYPE?
AKA another base type that inherits these and adds functionality, but is still abstract.

Good news - you can do that! 
TL;DR Version. Again, it just works (TM) - like this.
internal abstract class SubBodyPart<ThisClass, BehaviorClass> : BodyPartBase<ThisClass, BehaviorClass> [, any additional interfaces] where ThisClass : SubBodyPart<ThisClass, BehaviorClass> where BehaviorClass : SubClassBehavior<BehaviorClass, ThisClass>
{
	//Subclass related variables
	//implement any basebodypart stuff that is handled here. note that you can leave it for the child class to implement

	//any calls to behavior specific code for this subclass
}
abstract class SubClassBehavior<ThisClass, ContainerClass> : BodyPartBehavior<ThisClass, ContainerClass> [, any additional behavior interfaes] where ThisClass : SubClassBehavior<ThisClass, ContainerClass> where ContainerClass : SubBodyPart<ContainerClass, ThisClass>
{
	//subclass related logic
	//implement any bodypartbehavior that is handled in this subclass. note that you can leave this for child(ren) to implement

	//any behavior specific code for this subclass
}

The signature is virtually identical to the base versions, with the obvious difference being they inherit the base class. We've once again limited the generics - they are now limited to these subclasses.
to see a practical example, EpidermalBodyParts does this. it does this so that body parts with hair or fur can work with redundant code and effort by the programmers. 
