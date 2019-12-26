﻿//Delegates.cs
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
	//a description that may or may not be plural. it may be important to know from a grammar perspective whether or not this is the case.
	public delegate string MaybePluralDescriptor(out bool isPlural);


	public delegate string ShortDescriptor(bool alternateFormat = false);

	public delegate string ShortPluralDescriptor(bool plural = true);

	//short description delegate where the object being described may or may not be plural, depending on the context. for example, a unicorn only has one horn, whereas an imp always has
	//two horns. it may be necessary to know whether or not it actually is plural so you can use it in a sentence. If plural if applicable is set to false, the value returned will
	//always be singular.
	public delegate string ShortMaybePluralDescriptor(bool pluralIfApplicable, out bool isPlural);

	public delegate string DescriptorWithArg<T>(T arg);

	//standard long description delegate.
	public delegate string PartDescriptor<T>(T arg, bool alternateFormat = false);

	//long descripton delegate when the long description describes more than one member. by default, it will describe all members (none is also plural as far as English is concerned).
	//However, there may be some instance(s) where you only want one member and thus don't want it to be plural. this delegate allows that.
	//when plural is set to false, it will always return only one member in singular form, and when true it will always be plural.

	//if a type can be singular or plural (for example, some types of horns only allow 1 horn, while others allow none or multiple horns), the maybe variant is used instead.
	public delegate string PluralPartDescriptor<T>(T arg, bool alternateFormat = false, bool plural = true);

	//long description delegate where the object being described may or may not be plural, depending on the context. for example, a unicorn only has one horn, whereas an imp always has
	//two horns. it may be necessary to know whether or not it actually is plural so you can use it in a sentence. If plural if applicable is set to false, the value returned will
	//always be singular. If true, it will be plural if possible, and fallback to singular. the isPlural bool represents this result.
	public delegate string MaybePluralPartDescriptor<T>(T arg, bool alternateFormat, bool pluralIfApplicable, out bool isPlural);


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
