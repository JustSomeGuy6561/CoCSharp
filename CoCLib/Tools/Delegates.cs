//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

using CoC.EpidermalColors;

namespace CoC.Tools
{
	public delegate string GenericDescription();
	public delegate string PlayerDescription<T>(T type, Player player);
	public delegate string FullDescription<T>(T type);
	//not designed for use with npcs, like when you tf rubi or katherine.
	//though you probably should do those by case anyway.
	public delegate string ChangeType<T>(T originalType, Player player);

	public delegate string TypeAndPlayerDelegate<T>(T type, Player player); 

	public delegate string GenericDescriptorWithArg<T>(T arg);

	public delegate string AdjColorDescriptor(string adj, EpidermalColors.EpidermalColors color);

	public delegate void CombatDelegate(ref Player player, ref Enemy enemy);
}
