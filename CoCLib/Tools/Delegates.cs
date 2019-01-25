//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

using CoC.Creatures;
namespace CoC.Tools
{

	internal delegate string SimpleDescriptor();
	internal delegate string DescriptorWithArg<T>(T arg);

	internal delegate string PlayerStr(Player player);
	internal delegate string ChangeStr<T>(T newBehavior, Player player);
	internal delegate string RestoreStr(Player player);


	internal delegate string TypeAndPlayerDelegate<T>(T type, Player player);
	internal delegate string ChangeType<T>(T newType, Player player);
	internal delegate string RestoreType<T>(T originalType, Player player);

	//these may be changed or removed.
	internal delegate string AdjColorDescriptor(string adj, EpidermalColors.EpidermalColors color);
	internal delegate void CombatDelegate(Creature player, Creature enemy);

	//every function that uses a button will follow this format. note that with lambdas they can take virtually anything.
	internal delegate void PlayerFunction(Player player);
}
