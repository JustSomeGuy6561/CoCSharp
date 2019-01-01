//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

namespace CoC.Tools
{
	public delegate string GenericDescription();
	public delegate string PlayerDescription<T>(T type, Player player);
	public delegate string CreatureDescription<T>(T type, Gender gender);
	//not designed for use with npcs, like when you tf rubi or katherine.
	//though you probably should do those by case anyway.
	public delegate string ChangeType<T>(T previousType, Player player); 

	public delegate string GenericDescriptorWithArg<T>(T arg);
}
