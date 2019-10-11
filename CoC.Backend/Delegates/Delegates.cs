//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

using CoC.Backend.Areas;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;

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
	/// Used to resume idleing, called after all the complicated hourly stuff completes.
	/// </summary>
	/// <param name="hoursRemaining">number of hours left in the current idle period. </param>
	/// <param name="currentLocation">The current location the player finds themselves in after doing their hourly business</param>
	/// <returns>any text for the engine to print out.</returns>
	public delegate string ResumeTimeCallback(ushort hoursRemaining, AreaBase currentLocation);


	//passed into the item use function. It's called when the item is used, successfully or otherwise. This is due to the delayed, event-based nature of the item.
	public delegate void UseItemCallback(bool successfullyUsedItem, string itemUseContext, CapacityItem replacementItem);
	public delegate void UseItemCallbackSafe<T>(bool successfullyUsedItem, string itemUseContext, T replacementItem) where T : CapacityItem;
}
