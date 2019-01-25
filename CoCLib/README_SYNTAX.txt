For quick reference: unless a class is made available to the UI (which is almost nothing) a class is to be marked internal. Now, there's a weird syntax in C# where public members of an internal class
are actually internal. 90% of the time, marking something as public in an internal class is EXACTLY the same as marking it internal, and can be used interchangeably. 
there are two exceptions: interfaces and properties. 
	to make an interface work, its implementation MUST be public, even if the interface is internal. C# rules, not mine. again, these functions are actually internal, but marked as public.
	properties are super weird - if you want to limit mutators (setters) to protected, the property needs to be marked "protected internal" (or "internal protected"). This means "internal" OR "protected"
	for the property, NOT internal AND protected. Thankfully, a property marked "protected internal" is syntacticly the same as marking it public within an internal class.

Because of these two reasons, everything marked in an internal class is given the access modifier "public"
I understand that this is confusing, and I agree with you 100%. if it were up to me, C# compiler would prevent marking these public, and an internal interface would require internal implementations, though
this would cause cases where public classes implementing an internal interface were all screwy, so i understand why the compiler does it this way. 

TL;DR: all classes not available to the UI are marked "internal". all their members that are marked public are actually internal. protected and private members are still protected/private. why? Compiler reasons.