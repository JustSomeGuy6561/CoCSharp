DELEGATES 101
----------------------
GENERAL:
all body parts now use delegates. For those of you not familiar with C#, they are like Actionscript's callbacks or C/C++'s function pointers.
if you don't know either of those, think of it like this: 
we can give a class a function that returns a string, and we can make it different for each instance of the class by making it a parameter.
a Person class, for example, might have a name paramter, which stores the name of the person. we could then say Person.@name, and get their name.
this is fine if it's a simple name, but what happens when we want to do logic? we can't pass an if statement. To do that, we'd need to pass an entire
function. That's what delegates do.

EXAMPLE: ArmType (Arms.cs)
ArmType has several possible choices, but whichever one the player has, we need to print out a description when the player asks for their appearance.
now, we could implement each specific arm type as an subclass of ArmType, but that gets tedious and overly redundant (Note that you still can if you need to, 
but preferably on a case-by-case basis instead of ever time). 
Or we could create the arms constructor to take a string, but then it can't use any variables (such as skin color or tone in this case) that the arm has.

the first option, inheritance, is the most flexible, but the most complicated and inelegant. the second sacrifices flexibility for simplicity. neither
are ideal. Fortunately, we can take the middle road using delegates. We can define a function tailored specificly towards each arm, but not have to 
create a new subclass to do so. When we declare that arm type, we just pass in the respective funcion we just defined for it, and we're done.
It also has the added benefit of allowing multi-language support. more on that later.

OK, HOW DOES IT WORK?:

C# delegates are type safe, which means they are slightly less elegant to implement, but a lot easier to debug, and won't cause runtime errors (i.e. segfaults in C).
delegates are broken into two parts: the definition and the declaration. The definition looks like this

public delegate <return type> <delegateName>([parameters]);
delegates are like structs and enums - they do not need to be in a class, and can be accessed just like an enum/struct.

they are then used in your code like any other variable:
<delegateName> <variableName>;
<delegateName> <variableName2> = <functionThatMatchesThisFormat>;

they work as properties too:
<delegateName> <variableName3> {get; set;}

Example: 
(Delegates.cs)
...
public delegate string GenericDescription();
...
(Arms.cs)
class Arms
{
  //we can even pass it as a parameter.
  protected Arms(...GenericDescription desc,...) : BodyPartBase
  {
  ...
    shortDescription = desc; //note that there are no () at the end of corresponding function.
  ...
  }

  //using it as a property
  public GenericDescription shortDescription {get; protected set;}

  ...
}

USAGE:
we know how to use them as variables, but how do we _set_ a function?
well, like anything, there are multiple ways. generally, we just take a function with a matching signature, and assign it to the delegate variable.
//outside of a function
public delegate string GenericDescription();
private string HelloWorld() {return "Hello World!";}
//then in a function somewhere.
GenericDescription foo = HelloWorld; //note that we dont put parentheses here. we want the function, not the result of the function.
foo(); //return "Hello World"

if a function has multiple signatures (like Console.WriteLine, for example), the above method won't work. luckily, we can do it using new.

public delegate void StringWriter(string data);
stringWriter = new StringWriter(Console.WriteLine);
//in code
stringWriter("Hello World"); //prints "Hello World"

finally, we can use anonymous functions (lambdas). more on them later.

For a full practical example: 

class Arms
{
  protected Arms(...GenericDescription desc,...) : BodyPartBase
  {
  ...
    shortDescription = desc; //note that there are no () at the end of corresponding function.
  ...
  }

  //using it as a property
  public GenericDescription shortDescription {get; protected set;}

  ...

  private static string nondescriptArms() { return = "arms" }
  private static string beeArms() { return = "fuzzy bee arms" }

  public static Arms NONDESCRIPT = new Arms(nondescriptArms);
  public static Arms BEE = new Arms(beeArms);
  ...
}

class Example
{
	public static void main()
	{
	  Arms arm = Arms.NONDESCRIPT;
	  Console.Writeline(arm.shortDescription()); // prints "arms";
	  arm = Arms.BEE;
	  Console.Writeline(arm.shortDescription()); // prints "fuzzy bee arms";

	  //Console.Writeline(Arms.beeArms()); //throws an error
	}
}


there's a lot there, so a quick rundown. This is an overly simplified version of arms, created for this explanation, and NONDESCRIPT and BEE are instances of it.
beeArms and nonDescriptArms are functions that take no variables and return a string. they fit the format GenericDescription wants, so they're fine. note that in
this example NONDESCRIPT and BEE are static, so the functions they use must also be static. we pass in the delegate in the Arms constructor and set it there, so 
we wont get errors for not initializing it. in the main method, we get the nondescript arm instance, and get and call the delegate. in the backend, it resolves the
delegate, and calls that function. for the first writeline, that's NONDESCRIPT.shortDescription, which is nondescriptArms. then we set it to bee, and do it all again.
this time, the backend resolves to beeArms and returns that result. 

SIDE EFFECTS:
one cool thing is that this allows you to call private functions in a manner you have full control over. much like how you can use properties to set and get variables
publicly, but use a private variable for storange and changing internally, you can do the same with these delegates, and otherwise manipulate them internally. 
a simple example would be incrementing a counter every time the player asks for their appearance and they have a dragon face- maybe there's an easter egg once they reach 100 
or something. you obviously don't want people accidently calling the increment counter out of place, so you make it private. you can then use it as a delegate that is called 
when they ask for their appearance, which is a public delegate. 

WHAT IF I DON'T NEED ALL THE VARIABLES (OR I NEED MORE)?
type safe, static type languages are great - until you need something with a different signature. perhaps a function passes in the player so you can describe their hair as your
ears grow through it, but whatever ear you're implementing doesn't use it. or, perhaps your bodypart has another body part that is part of it, like hands are part of arms or feet
are a part of legs. You may need this variable in your logic, but don't have it in the delegate. 
The simplest method is to create a function that calls another function, including or removing extra variables, and this is the recommended method if you aren't comfortable with
lambdas (or any of this, for that matter). But it's limited, as you can't use variables unless you have them stored in the same scope as the function. 

luckily there's a solution: anonymous functions (or lambdas. I will use the terms interchangably, i apologize if it's confusing)
lambdas let you use any variables in scope where you write them. so if you have a local variable in a function, you can use it in the lambda

adding a variable:

public delegate string GenericDescription();
GenericDescription appleDescription;

public void runMe()
{
	appleDescription = Apple;
	Console.WriteLine(appleDescription()); //prints "Apple";
	int x = 5;
	appleDescription = () => AppleCount(x);
	appleDescription(); //prints "5 apples"
	x = 1;
	appleDescription(); //prints "1 apple"
}
//x is no longer in scope, but calling appleDescription will return "1 apple";

public string Apple()
{
	return "Apple";
}

public string AppleCount(uint count)
{
	string retVal = count.ToString() + " apple";
	if (count != 1)
	{
		retVal+= "s";
	}
	return retVal;
}

What's really cool is that if you use a variable in local scope, then change it after setting the lambda up, it will use the latest value.
even cooler, if you exit that scope, the variable doesn't get lost - the lambda still uses it, so it gets stored, but nothing else can reference it.
obviously the next time you change the delegate, it can get garbage collected. 