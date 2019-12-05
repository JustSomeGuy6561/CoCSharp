BODY_PARTS:

Disclaimer: if you know how to program, you may find this over-long and technically incorrect at times, perhaps dangerously so. You're not wrong, but i'm not teaching programming, i'm explaining the basics.
so don't go insane when my definition of signature, for example, omits important details.

---------------------------------------------------
Introduction
---------------------------------------------------

These are all the classes for body parts. The overall design is such that a new type of a specific body part can be created easily by defining a new static readonly member of the type class, right below the other ones.
The actual instantiation of these parts varies by specific body part, as some are more complex than others, but the end result is that it will just work(TM) - you don't need to write code anywhere else for it
to work with player appearance, or to transform to or from this body part type. You will, of course, need to provide a means for the player to get this particular part, like a TF item or other interaction,
but once it's in the game there's nothing else you need to worry about.

A quick aside, you are only required to create text for what happens when other types transform INTO this new type, but you may want to alter the text for other types so they say something special when they
transfrom FROM this new type. ALWAYS assume someone else will add more types after you, and NOT update your transform text, so you'll need a default case. If you can't think of a specific way to do this, I
would recommend first using some generic text about how this particular part seems to shift toward the default, then shifts again into your new type. A few body parts' types do that now, and it's perfectly fine.

---------------------------------------------------
General Programming Basics
---------------------------------------------------
One feature this new rewrite is aiming for is the ability to translate it into other languages. It's a ton of text, to be sure, but if we can separate the code from the text, it'll be much, much easier.
The ideal case is it's as simple as placing a language file in a folder, and then the game natively allows you to use that language from a menu.
The worst case is all the text is hard-coded, so we have to replace all the text and recompile for another language, and there's 30 versions of this game, but with different languages.
We want to do the first one, not the last one. the common strategy for this is to wrap all the text into functions, so that's what we've done. But if we had to create new classes for every member to allow this, we'd go insane.
To prevent this, we use delegates, also known as callbacks. If it helps, you may also think of these as type-safe function pointers (though if you know about function pointers and not delegates that's pretty strange imo).
____________________________________
Delegates 101:
delegates are custom versions of the Func or Action tools available from the System Namespace, and may actually provide more options than the default func or action.
it's also a lot simpler to name a delegate than it is to constantly write Func<CoC.Backend.Tools.Utils.Pair<string>, double> or something equally contrived.
basically, instead of storing a value, like a string or a double, they store a function. this lets us take advantage of the nature of variables, which is we can pass them in to constructors or other functions
and they can be different for multiple instances of the same class, and simultaneously take advantage of functions, which allow the value they return to change based on global or local variables, or their arguments.
This lets us wrap all our text into functions, so the function can return the text for french or whatever if that's the current language, and at the same time not go insane deriving a class for every time of arm, for example.

___________________________________
Context: The "()"
if you've ever forgot to add the "()" to a function, and wondered why it didn't just auto-correct (in fact, it mentions "method groups" in the correct options), delegates are why. you may have meant to set
your variable to Func<bool> instead of executing the function and setting its bool result to your variable - the IDE isn't sure.
___________________________________
Context 2: The Signature, (but really simplified)
You generally don't need to care, but all functions have "signatures." A signature describes a function to the interpreter. or more simply it just tells the interpreter what the function does.
string ToString(int x){ ...}
The interpreter needs to know that when it sees this function, it's giving it an int and getting back a string (among other things, but we'll ignore them for now). In terms of delegates, a signature is considered
"matching" if the parameters and return type are the same (and in the same order).
____________________________________
Assigning Delegates pt1: 'Matching' signatures
delegates work just like any other variable - you can declare one in your class as a field or property, or in the local scope of a function as a local variable.
But delegates are (somewhat) smart - if you provide them a function WITHOUT the "()", and the signature "matches" (see above), they'll automatically treat it as that delegate. you can also be explicit and use "new"
if that helps you. But sometimes, you find yourself in situations where that's not _quite_ the case. you could create a helper function that matches the signature and calls that function, or you could use a lambda.
_____________________________________
Assigning Delegates pt2: Lambdas (or Anonymous Functions)
lambdas are both incredibly useful, and incredibly confusing (at least at first). There are two major reasons to use a lambda. first, what you want to do does not match what it will do, and you don't want to create
a helper function JUST for this one instance. It may be that the function you want doesn't need the extra helper variables, or it returns float instead of bool, and you want anything not zero to be considered true.
Secondly, you may wish to "capture" a variable. sometimes you want to use a local variable in the function callback, but when that function is finally called, the local variable won't exist anymore. you can't reasonably
expect the program to store that variable and pass it everywhere until it's needed again, so what do you do? you capture it. This is the true power of lambdas; the first reason is more for convenience.
Either way, a lambda is a function, but with no name, and it only exists where it's currently being used.
a lambda works as follows:
Action
Func<bool, int> doesItWork = (x) => ToString(x) != "";

//this exists somewhere in the current class.
string ToString(int x){ ...}

the lambda here is (x) => ToString(x) != "";
this is an "anonymous" function that takes an int and returns a bool. it does this by calling ToString(x), which returns a string, and returns true if that string is not "", otherwise it returns false.
you'll note the syntax is weird here, because the (x) part doesn't have any type before the x. This is because the type of x is already known, or more accurately can be inferred. why we don't write the type
anyway, i honestly cannot tell you (i don't know), other than the fact that this is how they have to be written.

It's also worth noting that
(x) => ToString(x) != "";

is identical to
(x)
{
	return ToString(x) != "";
};
which is also equal to the much more beginner-friendly
(x)
{
	string foo = ToString(x);
	if(foo == "")
	{
		return true;
	}
	else
	{
		return false;
	}
};

This is good to know if your lambda requires multiple calculations, and cannot be written in one line. the => operator is what's called "syntactic sugar" which is just a fancy way of saying it looks nicer and saves
space, but otherwise functions exactly the same as the more explicit option.
the => operator in this case just says this is a function with only one line of code, and since this delegate requires a return, it's also the return statement, and this is what it's returning.
Note that it also works with void functions, but there it simply executes the command to the right of => continues along.

you'll also see the => operator used for properties, where it often means a property on has a "get" attribue, and this is the value of it (but not always. if you see set =>, it's doing something else; not important here).


---------------------------------------------------
Implementating a new type to an Existing body part:
---------------------------------------------------
All body parts are different, but at the very least, you will need to provide 5 string functions. All examples flavor text is made up on the spot, to give an indication of how it may be used in game. for
how it'll actually be used in game, you'll have to refer to the examples provided in that class's strings section.

Some body parts are more advanced than others, and have actions that are unique to that particular part. as such, they require additional variables to be defined, like a default skin or fur color, or default length, etc.
We've attempted to keep these relatively simple, but in more extreme cases, these classes become too complicated or unwieldy to use just variables. In this case, we use an abstract base class, and derived members.

If possible, we'd recommend using one of these derived classes to define your new part, but if that's not possible because your type is really unique (like BASILISK_SPINES in Hair, for example), you'll have to derive it yourself.
Fortunately, these are relatively straightforward, especially with Visual Studio. simply override what is abstract, and if you need to, override things labeled virtual (or override, if you are a few levels in).
Remember that this is an Object-Oriented language, so inheritance is a big thing, and we're not worried about the costs of several levels of derived classes. Do whatever is easiest for you.
More specifically, only worry about how optimized it is if you really, REALLY care and it's actually within your skillset. I'm the guy who wrote most of this rework, and i hardly bother (unless it's cool/weird like a bit shift instead of powers of 2)
For the trolls out there, i'm sure it's within your skillset to do the exact opposite, and misuse this to a point it's actually noticable on even modern computers. Don't be that guy(or gal, though from experience computer A-Holes tend to be male)

Here's what every part's type will have. see the specific type to see if the above mention of other stuff applies.
______________________________________
SimpleDescriptor shortDesc: SimpleDescriptor is a delegate. It takes nothing, and returns a string. see the DELEGATES README for more info.
a short description of the body part, generally the name of the type and the part (so "cat-like ears" for CAT instance of EarType). It's meant to be used in a sentence, but only provide the barest of information.
Using the above example of cat-like ears, and some made up text about wearing earrings: "... now has huge hoops dangling from her <ears.shortDesc>" => "now huge hoops dangling from her cat-like ears"
______________________________________
DescriptorWithArg<BodyPart> longDesc: A delegate. Takes the body part data class, and returns a string.
A full description of the body part part. This includes a reference to the body part's data class, so you can provide more information. for example, a human body can have various skin tones or skin textures so a full
	description, might want to say "human body with <skin texture> <skin color> skin" => "human body with smooth, ebony skin" or "human body with freckled, pale skin". This is meant to be used in a sentence.
	For example: The Goblin Assassin has <body.longDesc>" => "the Goblin Assassin has a pudgy human body, with smooth green skin". Exactly how this will be used in context should be described in a comment in that
	body part's strings partial class.
________________________________
PlayerAndTypeDelegate<BodyPart> playerDesc: A delegate: takes the player this body part is part of, and the body part data. returns a string.
The Player String: This is the string that will be printed when the player checks their Appearance. This will be a full sentence or multiple sentences. You have access to the entire player in the event you need
	something related. for example, explaining the tail may require you to get the flavor text for the player's butt, because the tail connects to the rest of body there.
	For example, a BEE Antennae says : "Floppy antennae also appear on your skull, bouncing and swaying in the breeze."
_______________________________________
ChangeType transformMessage: a delegate, syntactic sugar for PlayerAndTypeDelegate<BodyPart>. takes the player and body part data, returns a string
The Transform String. This is the default text that will be used when the player transforms this body part's type from another type to this one. This is to be called BEFORE changing the body part type.
	This allows you to use the old body part data to help describe the transformation. Note that this means you won't have the data for what that body part looks like AFTER the transformation, but
	should know that, or in the absolute worst case, be able to figure that out.

	You may want to address each other type it can transform from, or simply use a generic one. For example, if the player changes to DOG Arms, you could either say "Your <old type> arms shift, becoming dog-like"
	or you could have a different version for each type, like <if old was CAT> "your feline arms gain a little tone, at the cost of some flexibility. they now appear dog-like"
	and <if old was WOLF> "your wolf-like arms shift, the hard sinews and tendons softening slightly. they now resemble a more dog-like version of themselves"... and so on. this could be done with a switch statement or whatever.
_______________________________________
RestoreType restoreMessage: a delegate, syntactic sugar for PlayerAndTypeDelegate<BodyPart>. takes the player and body part data, returns a string
The Restore String. This is the text used by default when the player induces a transformation to revert their body part from this type to the default type. The default type's transform string simply calls
	the old type's restore string, and its restore string is empty. Reverting to normal is such an important part of the game, so it's required for each new type to implement this, and i can't force
	you to add a string to the existing default transform function. i can, however, force you to implement this, so i do.

___________________________________________
Additional Information
___________________________________________
Note that these are the defaults, they are not linked to any items. This is because we want to allow players to transform without forcing them to consume a certain TF item. Sometimes other interactions are introduced,
like an armor that gradually shifts the player toward a certain race (currently, Naga Dress and Forest Gown do this with Naga and Dryad, respectively.), and it sounds really weird when it says you quaffed snake oil
every morning you're wearing the naga dress. If you want text unique to your current transformation item, feel free to not call the default transform text, and instead use your own text.

FURTHERMORE: always assume ANY type can transform into the new type, even if your current logic, says otherwise (For example, the only way to get X arm type is to have Y arm type. Never assume this to be the case)
	First off, if your logic is flawed, there may be some instances in game where this occurs naturally. If it says "this shouldn't happen" that's a problem. even worse, the game crashes
		because you threw an error there for argument exception or something.
	Secondly, some things are designed to throw logic out the window. The game has tricksters in it (hello, kitsunes), and they like to mess with things. Hell, your posessy ghost follower turns your fingers into dicks
	if you meet certain conditions, and other conditions let you turn ceraph into your personal cocksleeve for a scene (found that one naturally by playing the game, and honestly it was pretty cool)
	If that's not enough, we have a Trickster God in game, and new content may give him a bigger role. One famous story involving Pan resulted in a poor human gaining a mule's ears (literally an Ass's Ears iirc).
	A generic transform string is fine, like "your "+ oldtype.shortDesc() +" shift uncomfortably, and when it finally stops you notice you have <new type> now";

---------------------------------------------------
Implementing a new Body Part
---------------------------------------------------
If you're reading this because you need to, not because you're learning how this all works, congrats! you're awesome/weird, and you've decided we need a whole new body part. maybe you want to break out spinnerets or
whatever organ produces venom in spiders/scorpions, or whatever - doesn't matter. Here's how you do it.
___________________________________________________
Background Info:
___________________________________________________
The way body parts are build is in two sections: the behavior class, and the data class. the behavior class explains how each unique type of your new part will work, and your data class stores a reference to this type
(aptly named type), and any other data that is unique to this new class. Both classes have a parent class, which will force you to implement the basic stuff required for the game to work.
Unfortunately, this isn't _quite_ as plug-and-play as an existing type; you will have to add the data class as a member of creature for it to have that body part, and you will have to add a means of creating it from the
character creator. you will also need to add it to the save class, if this isn't done automatically, TODO: correct this when saves are implemented

*************************************************************
IMPORTANT: THE BEHAVIOR CLASS IS IMMUTABLE. DO NOT STORE ANY VARIABLES IN HERE THAT CAN CHANGE OVER THE COURSE OF GAMEPLAY!
	Basically, the behavior class exists to define how a type behaves, but that's it. it doesn't actually store anything; that's the data class's job. if the behavior class affects the data in some way, it must work with the data class
	for example, arms have their own epidermis data, but it's entirely based on what the player currently has for a body type and their fur/skin colors and textures. further, each type of arm behaves differently - reptilian arms
	use the skin tone for their color, and even may change the tone of their claws, where a red-panda arm uses the main fur color the player has, and any secondary fur color the player has, if possible. But, the behavior class doesn't store that
	data. Instead, the data class passes in that data, (usually by reference) and then the behavior class determines how to change it, and returns that value. Think of it like an enum with a ruleset attached.

	If it helps, think of the behavior class like the rules to cards. One instance is the ruleset for Texas Hold'Em, and that there's 100 games going on at once. the rules don't care how much money a player in game 43 or 72 or 91 won or lost.
	The rules don't store that data. The rules just explain that A pair beats a high card, which loses to two-pair, which loses to trips, and so on. the data about the cards on the table and who has what amount of money -
	that's for each unique instance of the data class. you don't need 100 instances of the rules of hold'em if they're all the same, do you? so create one ruleset, and share it will all 100 games.
	pass the data from the current game into the ruleset (let's say, each player's hand), and let the ruleset return the results (in this case, the winner of the round). the data set updates the player's chips accordingly, then shuffles and deals again.
	Now let's say 50 games get bored of poker and switch to blackjack. that's fine, another instance is the ruleset for blackjack.

	Now cards might be overly complicated for this paradigm, but they'd do this
		Initial Phase:
		data -> behavior: here's my players, here's my deck, initial bets: go?
		behavior -> data: get initial bets/ante. if there are rules for blinds, these players must bet more.
		data -> behavior: here's my players, here's my deck, how do i deal the cards out.
		behavior -> data: i've updated your deck and players' hands to be ready.
		data -> behavior: ok, bet time - what are the bet rules? (callback)
		behavior -> data (For hold'em): go around getting their bets. The ante for every player is (this amount), which is more for little and big blinds. Players must match other players' bet to stay in.
			or they can fold, and forfeit any money they have already bet.
		behavior -> data (For blackjack: if the dealer is showing an ace, let them buy "insurance" on blackjack, if they'd like.

		Dealing Phase: (these are probably callbacks)
			data -> behavior: i need a bet/deal loop. can you provide me with it?
			behavior -> data (For hold'em): do the flop. get bets. do the turn. get bets. the river. get bets. exit loop.
			behavior -> data (For blackjack): push all players to stack. iterate over stack.
				current player: reveals cards. if identical, can split, and puts in additional bet amount. If split, deal one card to first of split, and one to second. push second pair to stack.
					can now hit, or stay. blackjack is an automatic stay and win. if hit, give another card, face up. repeat hit or stay. if player busts, automatic stay and loss.
					when stack empty: reveal dealer card. if below 16 or at 16, hit until above 16 or bust.
					exit loop.
		}
		Winning Phase:
		data -> behavior: here's players, hands, bets who won, and how much
		behavior -> data: i've upated your players' money based on who won, who lost, and how much they bet.

		... repeat, and so on.

	Now cards are a bad example, as they would require some serious callbacks. but hopefully it's a good reference, anyway.
*************************************************************
___________________________________________________
Step 1: Do I need it?
___________________________________________________
Design it first. Ask if it needs to be there, or if it can (sanely) be part of something else.
	First and foremost: does it have different types? For example, Butt and Hips realistically just wrap a byte with some flavor text and simple behaviors. Could they simply be part of another class? absolutely.
		I've kept them mostly for legacy reasons, but they also have descriptors, so it makes sense to keep them separate. But, they don't have different types, so they aren't Behavioral Parts.
		They're simply data classes. They obviously store data that helps make the player, so they need to be saved, and thus do that. if you have a part that satisfies this, just write a saver for them and you're good.

		An example of where something is NOT one of these is a body part with a piercing. Early versions of this code only allowed one group of piercings per body part, so i had a Nose class and an Eyebrow class, among others.
		These particular examples could be added to the Face. After a while, and a few revisions, i came up with a generic piercing class that will work with any piercing group, can be saved, and can be added to any	body part.
		If i had done a better job designing first, i would have realized this was redundant.

	If it does have multiple types with unique behaviors or text, can it be wholly defined as part of something else, but makes more sense to break it out into its own class? it may be what i call a sub-part or helper part.
		For example, Hands and Feet are sub-parts. could i define them in the Arm/LowerBody? Yes, but it'd be a lot of ugliness. and this way, multiple arms can use the same claws, or multiple lower bodies can use the same foot
		You'll notice these sub parts don't implement the full BehavioralSaveablePart/SaveableBehavior. Since we can determine EXACTLY which sub-part we have from their parent part, and all the unique data of these sub parts can
		be determined by using data in their parent part, we don't need to worry about saving them. They therefore implement the base for the behavior/data paradigm, but that's it. do the same.

		Also, the Epidermis is a helper part. (i was being pedantic, skin seemed to suggest skin, not scales or fur or whatever. i settled on epidermis because it roughly means outer-most layer <of skin>, and when we're talking about
		fur or to a lesser extent scales, they have skin underneath all that fur or scales or whatever, so outer-most layer <of the player> made more sense imo. Is it the best word? no. is it good enough for the average joe? yes.)
		does need to be its own class, even though it can be stored in body. it's far more useful to have a class for this instead of 6+ variables in every body part that has skin and it's important to it. Unlike the sub-parts, it does
		need to be saved, as it helps make up the body, so it does some save stuff as well. You'll note that skin is weird because it's never used directly in the old code, and we don't change skin types and display it to the user.
		As such, it doesn't need the transform/restore/player strings, and thus implements the base for behavior/data paradigm, the short and full desc, and save data, but this is a weird case. do the same, i guess.

		If neither a helper nor a sub-part, make sure it actually is needed. For example, i considered making ovipositors and/or abdomen their own class(es), but after swapping them, i found no real reason to do so. I didn't gain
		anything by making them have their own class(es); they were just fine as part of rear body/tail. I figured it made sense not to have a shark fin on your back when most of your back was this massive venom/web creating thing.
		and it also made sense not to have a tail where you have this egg inserter thing sticking out. so i left it alone. On the other hand, adding Womb instead of having it some weird perk/variable hybrid did add something, and
		it has unique types (harpy/salamander/basilisk/whatever kiha is/bunny/i ate too many ovi-max potions halp!), and some rather strange behavior (looking at you, basilisk womb. AKA once you have it you always have it.) granted,
		this may actually change and i may remove it later, at which point i'll leave this in as further evidence to think your shit out first!
___________________________________________________
Step 2: I need it, Let's get to it!
___________________________________________________
In the event you don't get a template with this project (likely, i tried creating one and it was a pain and didn't even work.), you'll have to manually create the body part's data and behavior classes
luckily, it's really simple - as long as you're using an IDE - Visual Studio is free, and available on Mac/PC. while VS Mac is limited a bit, we're actually using VS Core and xamarin, so we're good there! I guess MonoDevelop for linux, though
personally i'd recommend a virtual machine of Windows with Visual Studio installed, or biting the bullet and going with Windows + its Linux Subsystems magic (seriously, i can't go back to anything without WSL bash and Visual Studio
I can grep and sed and vim and git and ssh and it all just works! Plus, you know, use software designed with the fact that 90% userbase is on windows in mind and that not all devs can support multiple desktop environments. +Video Games!)

your data class should be a child or behavioral saveable part and your behavior class saveable behavior. to do this, you'll have to
it'll look like this
public sealed [partial] class MyDataClass : BehavioralSaveablePart<MyDataClass, MyBehaviorClass>
{

}

public [abstract/sealed] partial class MyBehaviorClass : SaveableBehavior<MyBehaviorClass, MyDataClass>
{

}


if you have an IDE, use it's intelligent code fixing tool, and say implement abstract class for data class, and add constructor(shortDesc...) for behavior class. Et viola. your basic code is in place. you'll need to implement it. more later.
if you don't have an IDE, i'm not helping you. I'm not being rude, but i've worked in VIM on systems without a gui and shitty debugging support long enough to know the dumbest shit happens when you don't have an IDE.
also, if you think you can neckbeard your code and not make errors, then you can fail on your own. spend 4 hours wondering why 15 > 43 in one location, before realizing there's a semi-colon at the end of your if statement.

i'm assuming you're using an IDE, if not, copy-pasta existing examples and morph them to your needs. you're still on your own. even if you are using an IDE, it may be an idea to have other examples open to reference.
One thing that could not be included in the abstract base class stuff was the update functions. updates require the unique data that makes your class unique, like length or whatever, so i couldn't make a generic
abstract class for that. this isn't flash, i can't make something dynamic and hope to god you know what you're doing.
_________________________________________

DATA CLASS
_________________________________________
override MyBehaviorClass type {get; protected set;} : behavior class type property.
generally, you can leave it alone like this, but you may find the need to not use the auto property. you may see some examples throughout existing code that does this, generally with a private _type as examples of not doing this.
_________________________________________
override isDefault: boolean property
you can't set this right now, but basically, you need to return type == <MyBehaviorClass>.<DEFAULT_INSTANCE>;
_________________________________________
Restore() : boolean function. takes nothing
this sets your type back to the default. if it already was default, return false.
otherwise return true.
_________________________________________
GenerateDefault(<optional values = null>) : MyDataClass function.
returns a new instance of your data class. fill it with the default type and set any values to their defaults, using the default type. Note that generally, it's expected that a default type won't have any variables,
	but it's possible for even a default type to be able to parse data. for example, the body class defaults to HUMANOID, which could use a skin tone and skin complexion, so we should allow the default generator to
	allow us to pass those in. However, if we don't know those, we obviously can't pass them in, so this function needs to either provide default values or be overloaded with a version that takes no parameters.
_________________________________________
GenerateDefaultOfType(MyBehaviorClass type) MyDataClass function. takes a behavior class type.
returns a new instance of your data class, but with the type set to whatever's passed in. values are set according to this passed in type.
__________________________________________
***UpdateType[WithOptionalHints](MyBehaviorClass type[, other parameters]) *** : boolean function. takes a variable number of arguments. REQUIRED, but not in the base class.
this is one or many functions, with extra variables as is necessary to allow the other devs to update this body part to a different type and its data accordingly.
for example, the body has like 30 of these, because the types of bodies are so varied, and they may want to change one or more options on transform
__________________________________________
Change[Data][WithOptionalHints]([data to change]) : OPTIONAL, though RECOMMENDED/REQUIRED if your data has any unique variables that can be changed during gameplay.
allows you to set private variables that make up this data class from external sources, according to whatever rules you deem fit. should return bool if it changed, or if applicable, the amount it changed.
for example, cock has a length, so it has a grow/shrink/setLength group of functions. grow/shrink change the length, then return how much it changed, so you can print out how much it was shortened or lengthened by.
	setLength is void as it just does it.
___________________________________________
[Reset()] : void. May have optional variables, but if it exists, it must be able to be called without passing in any parameters.
a more extreme version of Restore. not only does it reset the type to the default, any values that have changed from the initial values are also reset. i've never seen one of these used (so far), which is why it's optional.
if you feel it's necessary, cool, add it. if not, dont.
do the same for all the stuff labeled abstract in the BehavioralPartBase class.
_____________________________________________
Validate : bool. takes a bool to correct invalid data. if false, any invalid data should be left alone. if true, any invalid data should be corrected, if possible, and set to default if not possible.
	it's recommended to check if the type exists, then pass in your extra data to the type and tell it to check to see if that data is valid.
	returns true if all the data was valid, or false if anything was invalid.

BEHAVIOR CLASS
_____________________________________________
override index : int property
copy-pasta this:

public override int index => _index;
private readonly int _index;

set the index value in the constructor. i've been doing this with a static variable, but that's incredibly volatile, i'll probably hard-code constants later.
_____________________________________________
Constructor:

basically, copy-paste this.

private protected BEHAVIOR CLASS(/*add extra variables here as you need them do define this behavior*/
	SimpleDescriptor desc, DescriptorWithArg<DATA CLASS> longDesc, TypeAndPlayerDelegate<DATA CLASS> playerDesc,
	ChangeType<DATA CLASS> transformMessage, RestoreType<DATA CLASS> revertToDefault) : base(desc, longDesc, playerDesc, transformMessage, revertToDefault)
{

	_index = indexMaker++;
	<MY_LIST_OF_BEHAVIORS_FOR_DESERIALIZATION_OR_RNG>.AddAt(this, _index);

	//set any extra variables that determine the behavior here.
}
_______________________________________________
[Extra behavior functions for the data class to call during updates or changes or when it deals with other weird shit.]
self explanatory, really.

---------------------------------------------
Extending your class
---------------------------------------------
you'll notice some other classes implement interfaces,
there are 3 groups:
	The helper interfaces: tone/lotion/dye/growshrink/canattack with/
	the "awares"
	the "listeners"

	the helper interfaces extend the functionality of that body part. tone/lotion/dye/growshrink say that this body part can be affected by skin tone oil/ texture lotion/hair dye, and gro+/reducto, respectively.
		they're there to force you to have those functions so those items will work with them, no hard-coding needed in those classes.

	the awares need to know what another class is doing, but are "Lazy" about it. Laziness is not a bad thing in computer terminology - it just means that the data will only be obtained "on-demand"
		not all the time, or the moment something changes. Something that is body aware, for example, needs to know what is happening to the body, generally the current skin tone or fur color.
		but, they only care about this when they need to tell people what their current appearance is, which they rarely need to do.

	the listeners, on the other hand, need to know the moment something changes. for example, anything that cares about the time passing needs to know every hour on the hour - and react accordingly.
		for context, if you need to take a train/bus/whatever, you wouldn't periodically check to see if it's there and hope you happened to be there at the right time, you'd instead wait there for the bus
		to arrive, and hop on. other things react directly to something else changing. For example, some horns are very gender-sensitive, and will lengthen or shorten as you become more or less masculine.

...but why?
	simple answer: helpers are convenient and let us support new members, without needing to code in these new members. laziness is better than active listeners. events are smarter than doing the same thing all the time.

	full answer: We prefer to be notified when our order is ready, instead of checking every 2 minutes. we get to sit there and play on our phones/nintendo consoles
	and we let the wait staff take care of other people so we aren't holding everything up. well, computers work the same way. if we don't run a check at the beginning of every loop asking if the masculinity changed
	or if we have some strange akbal perk or whatever, we can let the computer do what it needs to. now, we don't care too much about the performance gains, true, but we do care about how much cleaner it is.
	saying hey - everything that needs to know i did something? i did something! is literally 1-4 lines of code, depending on how you do your braces (if at all for single if/for statements):

	foreach (var listener in listener group)
	{
		listener.IDidSomething(heresWhatIDid);
	}

	hell, i could make it a true one-liner:

	listenerGroup.ForEach((x) => x.IDidSomething(heresWhatIDid));

	that's way cleaner than seeing 30 hard coded checks the moment you enter your camp.

	yes, you need to store a list of all the listeners, and yes, you need to provide a subscribe/unsubscribe method. use a hashset, and subscribe and unsubscribe are O(1) (if you don't know what that means, basically as fast as possible).
	the allocation cost of a hashset is tiny, especially compared to the thousands upon thousands of characters this game uses. also, we do memory management instead of loading everything into memory. so frankly everything
	could have a hashset and it'd still cost less resources than the old version (seriously, everything was always in memory. always)
