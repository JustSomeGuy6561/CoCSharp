//Delegates.cs
//Description: Stores all the delegates publicly available in one easy to find location
//Author: JustSomeGuy
//12/29/2018, 11:03 AM

using CoC.Backend.Areas;
using CoC.Backend.Attacks;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using System;

namespace CoC.Backend
{

	public delegate string SimpleDescriptor();

	public delegate string DescriptorWithMetaData(out bool isPlural);
	//short description delegate where the short description can be plural. by default, it will be plural if the type or behavior has more than one member (or none, see below)
	//or singular if it does not. if the flag is set to false, it will return a singular format, regardless of whether that type or behavior has more than one member. Note that
	//the edge case where this flag is set to false when the behavior or type has no members is not defined.
	public delegate string SimplePluralDescriptor(bool pluralIfApplicable = true);

	public delegate string DescriptorWithArg<T>(T arg);



	//standard long description delegate.
	public delegate string LongDescriptor<T>(T arg, bool alternateForm = false);

	//long descripton delegate when the long description can be plural. by default, it will be plural if the type or behavior has more than one member (or none, as english treats
	//no members as plural as opposed to one member); otherwise it will be singular). However, there may be some instance(s) where you only want one member and thus don't want it to be
	//plural. this delegate allows that. Note that what happens when plural if applicable is false and the type or behavior has no members is not defined.

	//if a type can be singular or plural (for example, some types of horns only allow 1 horn, while others allow none or multiple horns), and you use this to manually parse complex edge
	//cases, make sure you check to see if there are more than one members or if it's just one (or none) and react accordingly.
	public delegate string LongPluralDescriptor<T>(T arg, bool alternateForm = false, bool pluralIfApplicable = true);


	public delegate string AdjectiveDescriptor(bool withArticle);

	public delegate string PlayerStr(PlayerBase player);
	public delegate string ChangeStr<T>(T newBehavior, PlayerBase player);
	public delegate string RestoreStr(PlayerBase player);


	public delegate string PlayerBodyPartDelegate<T>(T bodyPart, PlayerBase player);
	public delegate string ChangeType<T>(T oldData, PlayerBase player);
	public delegate string RestoreType<T>(T originalData, PlayerBase player);

	//these may be changed or removed.
	public delegate string AdjColorDescriptor(string adj, CoCColors color);
	public delegate void CombatDelegate(Creature player, Creature enemy);

	//every function that uses a button will follow this format. note that with lambdas they can take virtually anything.
	public delegate void PlayerFunction(PlayerBase player);

	/// <summary>
	/// Used to resume idleing, called after all the complicated hourly stuff completes.
	/// </summary>
	/// <param name="hoursRemaining">number of hours left in the current idle period. </param>
	/// <param name="currentLocation">The current location the player finds themselves in after doing their hourly business</param>
	/// <returns>any text for the engine to print out.</returns>
	public delegate string ResumeTimeCallback(ushort hoursRemaining, AreaBase currentLocation);

	//passed into the item use function. It's called when the item is used, successfully or otherwise. This is due to the delayed, event-based nature of the item.
	public delegate void UseItemCallback(bool successfullyUsedItem, string whatHappened, CapacityItem replacementItem);
	//public delegate PageDataWrapper UseItemCallback(bool successfullyUsedItem, CapacityItem replacementItem);
	public delegate void UseItemCallbackSafe<T>(bool successfullyUsedItem, string whatHappened, T replacementItem) where T : CapacityItem;
	//public delegate PageDataWrapper UseItemCallbackSafe<T>(bool successfullyUsedItem, T replacementItem) where T : CapacityItem;


	public delegate string SimpleReactionDelegate(bool currentlyIdling, bool hasIdleHours);

	//public delegate void RunPageCallback(DisplayBase currentDisplay);

	public delegate void HomeBaseReactionCallback(bool isReload);

	public delegate ResourceAttackBase GenerateResourceAttack(Func<ushort> get, Action<ushort> set);
}
