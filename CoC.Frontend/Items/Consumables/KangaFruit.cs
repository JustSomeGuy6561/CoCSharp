//KangaFruit.cs
//Description:
//Author: JustSomeGuy
//1/24/2020 9:23:31 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Tools;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class KangaFruit : ConsumableBase
	{
		public KangaFruit() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.



		public override string AbbreviatedName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemName()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//if your text uses "an" as an article instead of "a", be sure to change that here.
			string countText = displayCount ? (count == 1 ? "a" : Utils.NumberAsText(count)) : "";
			//when the text below is corrected, remove this throw.
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//update the text below to display what you need.
			return $"{count} <Your Text Here>";
		}

		public override string Appearance()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}



		//this can be removed safely if the item does a transformation. transformations handle bad end and results text for you.
		private string UseItemText()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//what other items should this be considered the same as? When dealing with inventory, This is used to put items in the same slot (if item slot capacity >1 of course),
		//but it may have other uses (such as a compendium or something of the sort), so make sure to actually implement this correctly. Two items should be equal if they have
		//the same type (i.e. both are Ferret Fruit or whatever), and any variables used to define the item (such as purity or enhanced booleans) are also equal, if applicable.
		//If you have no such variables, simply remove the throw and all is good. if it has other variables to check, be sure to check those too. see IncubiDraft for hints on
		//how to do so if you need to.

		//NOTE: this template is for Consumables only. Armor items and such have a more specific version of this equals that needs to be implemented.
		public override bool Equals(CapacityItem other)
		{
			//remove this throw when implementing. Note
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			return other is KangaFruit;
		}



		//does this consumable count as liquid for slimes and (kangaroo) diapause?
		public override bool countsAsLiquid => throw new NotImplementedException();
		//does this consumable count as cum (i.e. for succubi)?
		public override bool countsAsCum => throw new NotImplementedException();
		//how much hunger does consuming this sate?
		public override byte sateHungerAmount => throw new NotImplementedException();
		//note that this value is half of the value you get selling it to Oswald.
		//A constant, DEFAULT_VALUE, is defined in the base class as 6. if this is fine, simply replace the throw with DEFAULT_VALUE
		protected override int monetaryValue => throw new NotImplementedException();

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, out string whyNot)
		{
			//remove when implemented
			throw new NotImplementedException();
			//by default, it's assumed any item can be used, regardless of circumstance. if you need to change this, do so.
			//make sure to wrap the text in a function call though, so it can be moved to the strings section to make it easier for writers to handle.
			whyNot = null;
			return true;
		}

		//what happens when we try to use this item? note that it's unlikely, but possible, for this to be called if CanUse returns false.
		//you need to handle that, yourself, and you'll probably want to return some unique text saying you cant do it, you tried anyway,
		//and looked really dumb or something of the like
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			//remove when implemented
			throw new NotImplementedException();

			//if this item calls a TF, remove these
			resultsOfUse = UseItemText();
			isBadEnd = false;
			//and use these instead. you'll need to replace text accordingly. if not, remove these;
			//var tf = new <YourTF>(<AnyVariables>);
			//resultsOfUse =  tf.DoTransformation(consumer, out isBadEnd);

			return true;
		}
	}
}
