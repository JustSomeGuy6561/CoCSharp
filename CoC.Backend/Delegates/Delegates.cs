//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

using CoC.Backend.Areas;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;

namespace CoC.Backend
{

	public delegate string SimpleDescriptor();
	public delegate string DescriptorWithArg<T>(T arg);

	public delegate string PlayerStr(Player player);
	public delegate string ChangeStr<T>(T newBehavior, Player player);
	public delegate string RestoreStr(Player player);


	public delegate string TypeAndPlayerDelegate<T>(T type, Player player);
	public delegate string ChangeType<T>(T newType, Player player);
	public delegate string RestoreType<T>(T originalType, Player player);

	//these may be changed or removed.
	public delegate string AdjColorDescriptor(string adj, CoCColors color);
	public delegate void CombatDelegate(Creature player, Creature enemy);

	//every function that uses a button will follow this format. note that with lambdas they can take virtually anything.
	public delegate void PlayerFunction(Player player);

	/// <summary>
	/// Used to 
	/// </summary>
	/// <param name="hoursRemaining"></param>
	/// <param name="currentLocation"></param>
	/// <param name="isIdling"></param>
	public delegate void ResumeTimeCallback(ushort hoursRemaining, AreaBase currentLocation);
}
