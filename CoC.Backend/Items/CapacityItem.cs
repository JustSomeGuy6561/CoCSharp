using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Items
{


	public abstract class CapacityItem : IEquatable<CapacityItem>
	{

		//unlike the original, which put all of the data in the constructor, C# doesn't let us pass along function pointers to the base that use data from the current object
		//because the current object doesn't technically exist yet - the functions need to be static. personally, i'm not a fan of this approach, but there's nothing i can
		//do about it except some crazy workarounds that are just plain ugly. so, these are all abstract function calls. this lets us have variables unique to each object
		//factor in to their descriptions (so, if an armor has some unique perk or trait like durability or something, we can actually describe that), or lets us create 'enhanced'
		//items that use the same source but have different values and text. Its possible for people to misuse this, i guess, because it's easier to make the description mutable
		//(i.e. can change between function calls), but whatever.

		protected CapacityItem() { }

		//an abbreviated (~9 Characters or less) name for the item. this is primarily used for buttons, which need space to write 'x10' afterward. Note that if a capacity item limits
		//the number to only 1 per slot, you may use 12 characters, but that's really only recommended for armor or accessories. if the name is longer than the provided space, it will
		//be truncated (cut short).
		public abstract string AbbreviatedName();

		//name used when we don't have any character limit. there are situations where calling an item by it's name (like when gifting one to an NPC) is useful.
		public abstract string ItemName();

		//a short description of the item. primarily used when you initially obtain an item, and as part of the tooltip header. It includes a count and a display count flag.
		//the count tells you how many we are describing, so you know whether or not to make it plural. the display count flag tells you whether or not to display that count
		//as part of the description.
		//This lets us deal with multiple items nicely.
		//Example: enemy drops LaBova x1. Note that the reads section below add <> around the text that aren't normally there, to show what is being read from the function.
		//$"It seems the enemy dropped {ItemDescription(1,true)} as they fled. You place the {ItemDescription(1,false)} in your second pouch"
		//reads: "It seems the enemy dropped <A bottle of 'LaBova'> as they fled. You place the <bottle of LaBova> in your second pouch.
		//Example 2: same as above, but enemy drops LaBova x3
		//$"It seems the enemy dropped {ItemDescription(3,true)} as they fled. You place the {ItemDescription(3,false)} in your second pouch"
		//reads: "It seems the enemy dropped <three bottles of 'LaBova'> as they fled. You place the <bottles of LaBova> in your second pouch.
		//for the most part, make sure this sounds nice with count =1, for both display count and not. nearly all interactions only do one item at a time.

		public abstract string ItemDescription(byte count = 1, bool displayCount = false);

		//a more verbose description of the object, explaining any defining traits along with its physical appearance. this is used as part of the tooltip for the given item
		public abstract string Appearance();

		public abstract byte maxCapacityPerSlot { get; }

		/// <summary>
		/// Checks to see if the given creature can use this item. Note that just because an item can be used doesn't mean it will ultimately succeed, i.e. if the player cancels it.
		/// </summary>
		/// <param name="target">The creature attempting to use this item.</param>
		/// <param name="whyNot">A string explaining why the current item cannot be used. only checked if this returns false.</param>
		/// <returns>true if the creature can use this item, false otherwise.</returns>
		public abstract bool CanUse(Creature target, out string whyNot);

		/// <summary>
		/// Attempts to use the item, returning either its own page if it needs several pages, or the results of postItemUseCallback.
		/// </summary>
		/// <param name="target">the creature attempting to use this item.</param>
		/// <param name="postItemUseCallback">Callback to call when the item is actually used. </param>
		/// <returns>Either a page, or a string, explaining what it did, or why it failed.</returns>
		/// <remarks>Items that can result in a bad end will not call the post item use callback. If the base class they inherit does not have a built-in means of dealing with
		/// a bad end, it will need to override this.  </remarks>
		public abstract void AttemptToUse(Creature target, UseItemCallback postItemUseCallback);

		//how much it costs to buy this item. Generally, the sell price is 1/2 it's value because video game capitalism. Items that cannot be bought should be given a price of 0;
		protected abstract int monetaryValue { get; }

		public int buyPrice => Math.Max(monetaryValue, 0);

		//by default, items that can be bought are given a positive value. you may override this if you want an item that can be sold and thus has a price,
		public virtual bool canBuy => buyPrice > 0;
		public virtual bool canSell => true;

		//by default, no author. if there's an author, override this. the author credit will be passed along to the results, which should display it. note that the behavior for
		//using an item that does not get its own page while on another page is undefined - you don't want to override the original author's wall of text to say "you drank a potion
		//and got 50 health back". Generally, though, all items get their own page. (afaik, they always do, but don't quote me).
		public virtual string Author() => "";

		//equatable is used to determine items that are the same. it's not possible to simply go by type, because items may have variables that differentiate them (i.e. succubi milk
		//vs Purified Succubi Milk)
		public abstract bool Equals(CapacityItem other);
	}
}
