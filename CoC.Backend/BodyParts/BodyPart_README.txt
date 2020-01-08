BODY_PARTS:

Disclaimer: if you know how to program, you may find this over-long and technically incorrect at times, perhaps dangerously so. You're not wrong, but i'm not teaching programming, i'm explaining the basics. So don't go insane when my definition of signature, for example, omits important details.

---------------------------------------------------
Introduction
---------------------------------------------------

These are all the classes for body parts. The overall design is such that a new type of a specific body part can be created easily by defining a new static readonly member of the type class, right below the other ones. The actual instantiation of these parts varies by specific body part, as some are more complex than others, but the end result is that it will just work(TM) - you don't need to write code anywhere else for it to work with player appearance, or to transform to or from this body part type. You will, of course, need to provide a means for the player to get this particular part, like a TF item or other interaction, but once it's in the game there's nothing else you need to worry about.

A quick aside, you are only required to create text for what happens when other types transform INTO this new type, but you may want to alter the text for other types so they say something special when they transfrom FROM this new type. ALWAYS assume someone else will add more types after you, and NOT update your transform text, so you'll need a default case. If you can't think of a specific way to do this, I would recommend first using some generic text about how this particular part seems to shift toward the default, then shifts again into your new type. A few body parts' types do that now, and it's perfectly fine.

---------------------------------------------------
General Programming Basics
---------------------------------------------------
One feature this new rewrite is aiming for is the ability to translate it into other languages. It's a ton of text, to be sure, but if we can separate the code from the text, it'll be much, much easier. The ideal case is it's as simple as placing a language file in a folder, and then the game natively allows you to use that language from a menu. The worst case is all the text is hard-coded, so we have to replace all the text and recompile for another language, and there's 30 versions of this game, but with different languages. We want to do the first one, not the last one. the common strategy for this is to wrap all the text into functions, so that's what we've done. But if we had to create new classes for every member to allow this, we'd go insane.

To prevent this, we use delegates, also known (informally, and perhaps incorrectly if we're being pedantic) as callbacks. If it helps, you may also think of these as type-safe function pointers (though if you know about function pointers and not delegates that's pretty strange imo).
____________________________________
Delegates 101:
delegates are custom versions of Func or Action, which are available from the System Namespace, but are significantly more flexible, and let us name the variable in a way that helps explain exactly what it does.
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
The interpreter needs to know that when it sees this function, it's giving it an int and getting back a string (among other things, but we'll ignore them for now). In terms of delegates, a signature is considered "matching" if the parameters and return type are the same (and in the same order).
____________________________________
Assigning Delegates pt1: 'Matching' signatures
delegates work just like any other variable - you can declare one in your class as a field or property, or in the local scope of a function as a local variable.
But delegates are (somewhat) smart - if you provide them a function WITHOUT the "()", and the signature "matches" (see above), they'll automatically treat it as that delegate. you can also be explicit and use "new" if that helps you. But sometimes, you find yourself in situations where that's not _quite_ the case. you could create a helper function that matches the signature and calls that function, or you could use a lambda.
_____________________________________
Assigning Delegates pt2: Lambdas (or Anonymous Functions)
lambdas are both incredibly useful, and incredibly confusing (at least at first). There are two major reasons to use a lambda. first, what you want to do does not match what it will do, and you don't want to create a helper function JUST for this one instance. It may be that the function you want doesn't need the extra helper variables, or it returns float instead of bool, and you want anything not zero to be considered true. Secondly, you may wish to "capture" a variable. sometimes you want to use a local variable in the function callback, but when that function is finally called, the local variable won't exist anymore. you can't reasonably expect the program to store that variable and pass it everywhere until it's needed again, so what do you do? you capture it. This is the true power of lambdas; the first reason is more for convenience.

Either way, a lambda is a function, but with no name, and it only exists where it's currently being used.
a lambda works as follows:
Action
Func<bool, int> doesItWork = (x) => ToString(x) != "";

//this exists somewhere in the current class.
string ToString(int x){ ...}

the lambda here is (x) => ToString(x) != "";
this is an "anonymous" function that takes an int and returns a bool. it does this by calling ToString(x), which returns a string, and returns true if that string is not "", otherwise it returns false.
you'll note the syntax is weird here, because the (x) part doesn't have any type before the x. This is because the type of x is already known, or more accurately can be inferred. It's also possible to write a lambda with the type expressly provided, like so:
Func<bool, int> doesItWork2 => (int x) => ToString(x) != "";
Either way is fine. common practice is to omit the type; i'm not exactly sure why. In some cases, however, notably with the "in", "out", or "ref" keywords, it may actually be required to write the types in order for the interpreter to understand what you're doing. Personally, i'd recommend including the types if it helps you, and omitting them if not.

It's also worth noting that
(x) => ToString(x) != "";

is identical to
(x) =>
{
	return ToString(x) != "";
};
which is also equal to the much more beginner-friendly
(int x) =>
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

This is good to know if your lambda requires multiple calculations, and cannot be written in one line. the => operator is what's called "syntactic sugar" which is just a fancy way of saying it looks nicer and saves space, but otherwise functions exactly the same as the more explicit option.
the => operator in this case just says this is a function with only one line of code, and since this delegate requires a return, it's also the return statement, and this is what it's returning.
Note that it also works with void functions, but there it simply executes the command to the right of => continues along.

you'll also see the => operator used for properties, where it often means a property on has a "get" attribue, and this is the value of it (but not always. if you see set =>, it's doing something else; not important here).


---------------------------------------------------
Implementating a new type to an Existing body part:
---------------------------------------------------
All body parts are different, but at the very least, will require some variation of 5 or 6 string functions. Generally speaking, unless you're creating a new type, you can simply copy the format from another type, replace the variables as needed, then implement new string functions, using the existing type string functions as a guide.

Some body parts are more advanced than others, and have actions that are unique to that particular part. as such, they require additional variables to be defined, like a default skin or fur color, or default length, etc. We've attempted to keep these relatively simple, but in more extreme cases, these classes become too complicated or unwieldy to use just variables. In this case, we use an abstract base class, and derived members.

If possible, we'd recommend using one of these derived classes to define your new part, but if that's not possible because your type is really unique (like BASILISK_SPINES in Hair, for example), you'll have to derive it yourself. Fortunately, these are relatively straightforward, especially with Visual Studio. simply override what is abstract, and if you need to, override things labeled virtual (or override, if you are a few levels in). Remember that this is an Object-Oriented language, so inheritance is a big thing, and we're not worried about the costs of several levels of derived classes. Do whatever is easiest for you.

More specifically, only worry about how optimized it is if you really, REALLY care and it's actually within your skillset. I'm the guy who wrote most of this rework, and i hardly bother (unless it's cool/weird like a bit shift instead of powers of 2)
For the trolls out there, i'm sure it's within your skillset to do the exact opposite, and misuse this to a point it's actually noticable on even modern computers. Don't be that guy(or gal, though from experience computer A-Holes tend to be male)

The exact format of these string functions will vary, depending on if the body part has multiple members (aka is plural), can have multiple members (aka may be plural), or only has one member (aka is not plural). The overall goal is to provide the people writing the text as much information as they need, so the grammar makes sense. Regardless of format, these functions will always be required, and alway in the same format:
___________________________
PlayerBodyPartDelegate<ContainerClass> playerDesc: a delegate. Takes the body part source class and the player that has that body part. returns a string. This string function describes the body part in full sentences. When the player asks for their appearance, this will be called, for all available body parts. You can assume the player and body part class will never be null.
___________________________
ChangeType<DataClass> transformDesc: a delegate. Takes the old body part data (before a transformation) and the player that did the transformation. returns a string. This string function describes what happened when a transformation occured from an old body part to this behavior/type. if needed, the current body part data can be retrieved from the player to help describe exactly what has changed.
___________________________
RestoreType<DataClass> restoreDesc: a delegate. Takes the original body part data (before restoring to the default type, which is generally human or none) and the player whose body part was restored. This string function describes what happens when the body part changes from the current behavior to the default one. just like restore, the current body part data can be retrieved from the player if needed.
___________________________

Additionally, all classes have some variation of the short description, long description, and singular description.

*****************************
SIMPLE FORMAT (NOT PLURAL)
*****************************
This is the most common case. It requires the least information to implement, and is thus the most straightforward.

BodyPartType([Any additional members as needed...,] ShortDescriptor shortDesc, PartDescriptor<DataClass> longDesc, PlayerBodyPartDelegate<SourceClass> playerDesc, ChangeType<DataClass> transform, RestoreType<DataClass> restore)

This format combines the singular description and the short description into one function. longDesc is simple and straightforward.
______________________________
ShortDescriptor : a delegate. takes a boolean. returns a string. it combines the short description and the singular description into one function, with a boolean flag differentiating them. For many body parts, the short description and singular description are virtually identical; the singular simply adds an article to the short description. If this is the case, there's no reason to rewrite the same thing over again, so we can just combine them and it's really straightforward. behind the scenes, the engine simply converts these to SimpleDescriptors and calls them accordingly.
______________________________
PartDescriptor<DataClass>: a delegate. takes the current body part data and a boolean. returns a string. It provides a more verbose description, and therefore requires additional information about the current body part. therefore, it takes a body part data so it can describe things like skin tone or fur color, or length, etc. The alternate format allows it to be used in other sentence structures, based on the current language. In english, for example, this means it will work for the text "You have ", as opposed to the default "your ". In some cases, this may return identical text; this is okay, so long as it makes sense in both formats.

*****************************
COMPLETE FORMAT (IS PLURAL)
*****************************
The next most common case is when we know something is plural. Wings, Ears, Antennae, Arms, etc. are always guarenteed to have 2 (or more, potentially) members. In this case, we need more freedom than the combined option provides us - what if they only want one, but it doesn't require any unique formatting? In this case, we use a new format:

BodyPartType([Any additional members as needed...,] ShortPluralDescriptor desc, SimpleDescriptor singleItemDesc, PluralPartDescriptor<DataClass> longDesc,
			PlayerBodyPartDelegate<SourceClass> playerDesc, ChangeType<DataClass> transformMessage, RestoreType<DataClass> revertToDefault)

This format keeps the singular and short description separate, but also makes the short description allow us to specify additional options.
___________________________
ShortPluralDescriptor desc: a delegate. Takes a boolean, returns a string. This variant of a short description provides the user with the option of specifying if they want it to be plural, or if they only want one member, but does not otherwise provide any special formatting. This is useful for things like "Hands," where you might want to say "it fits in your hand" (or talon, or claw, or whatever), where the unique formatting of single item description would not make sense (like "it fits in your a hand"). Behaviors that do this should then provide an overload of ShortDescription() that takes a boolean and calls this variant, though unless you are creating a new type, this is unimportant.
___________________________
PluralPartDescriptor<DataClass> longDesc: a delegate. Takes a data class, a boolean for alternate format, and a boolean for plural. returns a string. this variant of long description adds a boolean allowing the user to ask for a single member instead of plural. The alternate format flag and the plural flag are not mutually exclusive; both flags should be accounted form when writing the text. note that some cases may make sense grammatically for either setting of the alternat format flag; this is ok.

*****************************
COMPLETE FORMAT (MAY BE PLURAL)
*****************************
This is an uncommon, but very important case: the type may be plural, but may also be singular. Horns are the most obvious example: some behaviors/types have multiple horns, some only have one horn. Even worse, some types have one horn to begin with, but as you increase the transformation strength, you may get a second (or more) horn(s). So, if may be important to describe the horn(s), but writing the '(s)' at the end is just ugly, and arguably game breaking. Additionally, what if you want (and ask for) multiple horns, but the type only has one? The solution is to provide a 'Maybe' type, that returns the text, but also sets a flag telling the caller whether or not the result is plural. Other examples include tails, lowerbody (legs), Eyes. Technically, Cock and Vaginal also have to deal with this, but since these are handled by the genitals class, they are taken care of differently. These follow this format:

BodyPartType([Any additional members as needed...,] ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<DataClass> longDesc, PlayerBodyPartDelegate<SourceClass> playerDesc, ChangeType<DataClass> transform, RestoreType<DataClass> restore)

Like the previous complete format, the single item descriptor is simply a function that takes nothing and returns a string. However, since this is a maybe type, it uses the 'out' keyword. 'out' is a special type of keyword - any variable designated with 'out' must be set before the function returns. For context, it's generally considered bad practice to use these unless the situation requires it, because it can be confusing, or it can be a sign of bad design. That said, it's MUCH MUCH Easier to just have a function tell the caller "i'm plural" or "i'm not plural" than it is to try to determine if something is plural. Alternatively, you could wrap the boolean and text in a unique class, but that's even worse in this case, imo.

For these, i've included the full signature because they are so complicated (relatively, anyway);
____________________________
ShortMaybePluralDescriptor(bool pluralIfApplicable, out bool isPlural): takes a boolean flag for plural (if applicable) and an 'out' isPlural flag. sets the isPlural flag before returning, which is available to the caller. returns a string to the caller. A variant of the plural short description that tries to return a plural variant of the text, but may fail to do so if the type is not plural. isPlural will be true if the resulting string is plural, or false if it is not. if the plural if applicable flag is set to false, is plural will return false.
____________________________
MaybePluralPartDescriptor<DataClass>(DataClass bodyPartData, bool alternateFormat, bool pluralIfApplicable, out bool isPlural): takes the current body part data, an alternate format flag, a pluralIfApplicable flag, and an 'out' isPlural flag. returns a string. A long description variant that tells the caller if the result is plural or not. generally, the value of isPlural will be the same as pluralIfApplicable, so you should just set isPlural to pluralIfApplicable. however, if the user sets the pluralIfApplicable flag and the type cannot be plural, isPlural should be set to false. This allows the caller to use the correct 'copula' or 'linking verb' (am, is, are, was, were, etc.) for the given text.

an example:
"your " + player.horns.ShortDescription(true, out bool isPlural) + (isPlural ? " are" : " is") + " ugly";
returns
(dragon horns) : "your draconic horns are ugly"
or
(unicorn horn) : "your single unicorn horn is ugly"
or
(rhino horn, first level) : "your rhino horn is ugly"
or
(rhinor horn, last level) : "your rhino horns are ugly"

This lets us translate the writer-friendly pseudo-code:
"your [hornsShort] [if hornsPlural then 'is' else 'are'] ugly".
in just one line of code. in fact, with string interpolation, we can actually make the writers coders and they don't need to know anything.
$"your {player.horns.ShortDescription(true, out bool isPlural)} {(isPlural ? "are" : "is")} ugly";

Granted, the formatting of {Function} is slightly more complex than [Function], but not much.

*****************************
EDGE CASES:
*****************************
Both Cock and Vagina are edge cases: It may be possible for you to have multiple of them, but they may be mixed types. Furthermore, even if they are the same type, they may have different values like length, wetness, etc. So we can't really do a standard long plural description. However, the short description does not care about any of that, so it MAY be possible for us to use a short plural description... but it won't work if it's mixed types. as such, the class storing all this data, genitals, has unique functions for this. Additionally, the cock/vagina short description provides a plural alternative, but both default to NOT plural, which is different from the other classes, which default to plural.

_________________
Additional Information
___________________________________________
Note that these are the defaults, they are not linked to any items. This is because we want to allow players to transform without forcing them to consume a certain TF item. Sometimes other interactions are introduced, like an armor that gradually shifts the player toward a certain race (currently, Naga Dress and Forest Gown do this with Naga and Dryad, respectively.), and it sounds really weird when it says you quaffed snake oil every morning you're wearing the naga dress. If you want text unique to your current transformation item, feel free to not call the default transform text, and instead use your own text.

FURTHERMORE: always assume ANY type can transform into the new type, even if your current logic, says otherwise (For example, the only way to get X arm type is to have Y arm type. Never assume this to be the case)
	First off, if your logic is flawed, there may be some instances in game where this occurs naturally. If it says "this shouldn't happen" that's a problem, or even worse, the game crashes because you threw an error there for argument exception or something.
	Secondly, some things are designed to throw logic out the window. The game has tricksters in it (hello, kitsunes), and they like to mess with things. Hell, your posessy ghost follower turns your fingers into dicks if you meet certain conditions, and other conditions let you turn ceraph into your personal cocksleeve for a scene (found that one naturally by playing the game, and honestly it was pretty cool)
	If that's not enough, we have a Trickster God in game, and new content may give him a bigger role. An example of such a thing already exists in regular mythology; one famous story involving Pan resulted in a poor human gaining a mule's ears (literally an Ass's Ears iirc).

	To handle this, a generic transform string is fine, like "your "+ oldtype.shortDesc() +" shift uncomfortably, and when it finally stops you notice you have <new type> now", but try to make it work for everything. Granted, for some things (like body, holy shit) it's a huge pain in the ass. Just do your best, i guess.

---------------------------------------------------
Implementing a new Body Part
---------------------------------------------------
If you're reading this because you need to, not because you're learning how this all works, congrats! you're awesome/weird, and you've decided we need a whole new body part. maybe you want to break out spinnerets or
whatever organ produces venom in spiders/scorpions, or whatever - doesn't matter. Here's how you do it.
___________________________________________________
Background Info:
___________________________________________________
The way body parts are build is in three sections: the source class, the behavior class, and the data class. the behavior class explains how each unique type of your new part will work, the source class stores the current behavior type and any pertinent data, and th data class provides a read-only snapshot of the source class. The data and source class are fundamentally similar, but the source class provides means for the data to be updated (generally, the type changes, but some have additional options), whereas the data class is readonly and none of its data can be updated. This distinction is particularly useful because it allows hardcoded NPCs who do not implement the creature class to still be able to describe a body part. in the old code, there was no data class; each variable had to be passed in to a function, and updating or altering that function would break the calls everywhere. By wrapping that data in a read-only data class, we can avoid that, and still keep the creature-friendly and NPC-friendly text together.

Generally speaking, you new type inherits saveable part, saveable behavior and saveable part data, for the source, behavior, and data class, respectively. Feel free to use the other body parts for reference.

Unfortunately, this isn't _quite_ as plug-and-play as an existing type; you will have to add the data class as a member of creature for it to have that body part, and you will have to add a means of creating it from the character creator. the data class will also need to implement a means of saving. #9999# TODO explain this when implemented.
*************************************************************
IMPORTANT: THE BEHAVIOR CLASS IS IMMUTABLE. DO NOT STORE ANY VARIABLES IN HERE THAT CAN CHANGE OVER THE COURSE OF GAMEPLAY!
	Basically, the behavior class exists to define how a type behaves, but that's it. it doesn't actually store anything; that's the data class's job. if the behavior class affects the data in some way, it must work with the data class
	for example, arms have their own epidermis data, but it's entirely based on what the player currently has for a body type and their fur/skin colors and textures. further, each type of arm behaves differently - reptilian arms use the skin tone for their color, and even may change the tone of their claws, where a red-panda arm uses the main fur color the player has, and any secondary fur color the player has, if possible. But, the behavior class doesn't store that data. Instead, the data class passes in that data, (usually by reference) and then the behavior class determines how to change it, and returns that value. Think of it like an enum with a ruleset attached.

	If it helps, think of the behavior class like the rules to cards. One instance is the ruleset for Texas Hold'Em, and that there's 100 games going on at once. the rules don't care how much money a player in game 43 or 72 or 91 won or lost. The rules don't store that data. The rules just explain that A pair beats a high card, which loses to two-pair, which loses to trips, and so on. the data about the cards on the table and who has what amount of money - that's for each unique instance of the source class. you don't need 100 instances of the rules of hold'em if they're all the same, do you? so create one ruleset, and share it will all 100 games. pass the data from the current game into the ruleset (let's say, each player's hand), and let the ruleset return the results (in this case, the winner of the round). the data set updates the player's chips accordingly, then shuffles and deals again.
	Now let's say 50 games get bored of poker and switch to blackjack. that's fine, another instance is the ruleset for blackjack. Now cards might be overly complicated for this paradigm, but they'd do this

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
		For example, Hands and Feet are sub-parts. could i define them in the Arm/LowerBody? Yes, but it'd be a lot of ugliness. and this way, multiple arms can use the same claws, or multiple lower bodies can use the same foot. You'll notice these sub parts don't implement the full BehavioralSaveablePart/SaveableBehavior. Since we can determine EXACTLY which sub-part we have from their parent part, and all the unique data of these sub parts can be determined by using data in their parent part, we don't need to worry about saving them. They therefore implement the base for the behavior/data paradigm, but that's it. do the same.

		Also, the Epidermis is a helper part. (i was being pedantic, skin seemed to suggest skin, not scales or fur or whatever. i settled on epidermis because it roughly means outer-most layer <of skin>, and when we're talking about fur or to a lesser extent scales, they have skin underneath all that fur or scales or whatever, so outer-most layer <of the player> made more sense imo. Is it the best word? no. is it good enough for the average joe? yes.) does need to be its own class, even though it can be stored in body. it's far more useful to have a class for this instead of 6+ variables in every body part that has skin and it's important to it. Unlike the sub-parts, it does need to be saved, as it helps make up the body, so it does some save stuff as well. You'll note that skin is weird because it's never used directly in the old code, and we don't change skin types and display it to the user. As such, it doesn't need the transform/restore/player strings, and thus implements the base for behavior/data paradigm, the short and full desc, and save data, but this is a weird case. do the same, i guess. It's also super weird because a short description is either plural or singular; we have no control over that. scales, for example, are plural ("your scales itch") whereas fur is singular ("your fur itches"). As such, it instead uses a variant of SimpleDescriptor with an out bool isPlural flag.

		Finally, Ovipositor is arguably the one i flip-flopped over so many times - it was not staying as a perk - that was just dumb. first it was a full body part. that seemed excessive. then it was part of back. that didnt make sense. Eventually, i made it a full part again. then i read how it was actually used in-game, and found it was directly linked to the tail class - when the bee or spider tail was lost, so too was the respective ovipositor. it's now finalized as a sub-part of tail. If i had read the (admittedly conviluted) source, i'd have found how they were actually used and avoided all of that.


		If neither a helper nor a sub-part, make sure it actually is needed. For example, i considered making the 'abdomen' class its own thing, but eventually settled into keeping at part of back or tail, which is essentially what it already was. On the other hand, a common "Body" class would allow us to change how the creature appeared in ways the previous skin and underbody would not - we can now treat a secondary epidermis as an underbody, as a ventral (basically the same), as some weird mix of types (kitsune, cockatrice), and do so in a way that is far more flexible and makes a great deal more sense than before. this also lets us manually parse the body data, so we don't have to handle whether or not our claws are 'goopy' or not, which was really ugly and hard to handle consistently (though it now means we have to handle goo on certain transforms, which is annoying).

___________________________________________________
Step 2: I need it, Let's get to it!
___________________________________________________
In the event you don't get a template with this project (no promises, i can create them but it's not always straightforward.), you'll have to manually create the body part's data and behavior classes. luckily, it's really simple - as long as you're using an IDE - Visual Studio is free, and available on Mac/PC. while VS Mac is limited a bit, we're actually using VS Core and xamarin, so we're good there! I guess MonoDevelop for linux, though personally i'd recommend a virtual machine of Windows with Visual Studio installed, or biting the bullet and going with Windows + its Linux Subsystems magic (seriously, i can't go back to anything without WSL bash and Visual Studio. I can grep and sed and vim and git and ssh and it all just works! Plus, you know, use software designed with the fact that 90% userbase is on windows in mind and that not all devs can support multiple desktop environments. +Video Games!)

your data class should be a child or behavioral saveable part and your behavior class saveable behavior. to do this, you'll have to
it'll look like this
public sealed [partial] class MyClass : BehavioralSaveablePart<MyClass, MyBehaviorClass, MyDataClass>
{

}

public [abstract/sealed] partial class MyBehaviorClass : SaveableBehavior<MyBehaviorClass, MyClass, MyDataClass>
{

}

public sealed [partial] class MyDataClass : BehavioralSaveablePartData<MyDataClass, MyClass, MyBehaviorClass>
{

}


if you have an IDE, use it's intelligent code fixing tool, and say implement abstract class for source class, add constructor(shortDesc...) for behavior class, source class, and data class. Et viola. your basic code is in place. you'll need to implement it. more later.
if you don't have an IDE, i'm not helping you. I'm not being rude, but i've worked in VIM on systems without a gui and shitty debugging support long enough to know the dumbest shit happens when you don't have an IDE.
also, if you think you can neckbeard your code and not make errors, then you can fail on your own. spend 4 hours wondering why 15 > 43 in one location, before realizing there's a semi-colon at the end of your if statement. (it happens, even in the workforce. Holy fucking shit that pissed me off)

i'm assuming you're using an IDE, if not, copy-pasta existing examples and morph them to your needs. you're still on your own. even if you are using an IDE, it may be a good idea to have other examples open to reference.
Even with a template, you may still need to override the update, or overload the update with additional options. these are on you; i can't tell you how to write the code if i don't know what you need. I can provide an example, however: wings allow you to set the size to small or large. it may be important for your uses to do that when updating the type, so you'll need to overload your UpdateType function with a variant that allows the user to set a isLarge bool.

Unfortunately, this isn't flash: you can't give something an anonymous class and hope it can parse it or whatever - c# is statically typed (before you start, yes, i'm aware of dynamic. It breaks compatibility and is generally a bad idea), and a lot safer for it. so you'll need to provide functions for these, and generally make it as flexible as possible for everyone.
_________________________________________

#9999# TODO add info about saving when done.