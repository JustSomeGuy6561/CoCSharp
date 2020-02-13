//BlackCatBeer.cs
//Description:
//Author: JustSomeGuy
//1/27/2020 11:24:43 PM

using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.StatusEffect;
using CoC.Frontend.Transformations; //use if this is an item that does a transformation. safe to remove if not.
using CoC.Frontend.UI; //used if the item has to deal with menus and such. safe to remove if not.
using System;

namespace CoC.Frontend.Items.Consumables
{
	public sealed partial class BlackCatBeer : StandardConsumable
	{
		public BlackCatBeer() : base()
		{
		}

		//move these to a dedicated file withing the strings folder group. they're here to make initial development easier.

		public override string AbbreviatedName()
		{
			return "BC Beer";
		}

		public override string ItemName()
		{
			//trademark for sillys.
			return "Black Cat Beer" + (SillyModeSettings.isEnabled ? "(TM)" : "");
		}

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";
			string mugText = count == 1 ? "mug" : "mugs";

			return $"{count}{mugText} of {ItemName()}";
		}

		public override string AboutItem()
		{
			return "A capped mug containing an alcoholic drink secreted from the breasts of Niamh. It smells tasty.";
		}

		private string UseItemText(bool alreadyDrunk)
		{
			string drunkText = alreadyDrunk ? GlobalStrings.NewParagraph() + "Damn, it's even better with every extra drink!" : "";

			return "Uncapping the mug, you swill the stuff down in a single swig, gasping as it burns a fiery trail into your belly. It's rich and sweet, but damn, " +
				"it's strong stuff!" + GlobalStrings.NewParagraph() +
				"A wonderful warmth fills your body, making your pain fade away. However, it also makes your crotch tingle - combined with the relaxation imparted already, " +
				"you feel " + SafelyFormattedString.FormattedText("very", StringFormats.UNDERLINE) + " turned-on. A beautiful, warm, fuzzy sensation follows and fills your head, " +
				"like your brain is being wrapped in cotton wool. You don't feel quite as smart as before, but that's all right, it feels so nice..." + GlobalStrings.NewParagraph() +
				"Your balance suddenly feels off-kilter and you stumble, narrowly avoiding falling. You just can't move as fast as you could, not with your head " +
				"feeling so full of fluff and fuzz; your body prickles and tingles with the warmth once your head feels full, the sensation concentrating around your erogenous zones." +
				" You just feel so fluffy... you want to hold somebody and share your warmth with them, too; it's just so wonderful." + drunkText;
		}

		public override bool Equals(CapacityItem other)
		{
			return other is BlackCatBeer;
		}



		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 0;

		protected override int monetaryValue => 1;

		//can we use this item on the given creature? if not, provide a valid string explaining why not. that text will be displayed as a hint to the user.
		public override bool CanUse(Creature target, bool isInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		//what happens when we try to use this item? note that it's unlikely, but possible, for this to be called if CanUse returns false.
		//you need to handle that, yourself, and you'll probably want to return some unique text saying you cant do it, you tried anyway,
		//and looked really dumb or something of the like
		protected override bool OnConsumeAttempt(Creature consumer, out string resultsOfUse, out bool isBadEnd)
		{
			(consumer as CombatCreature)?.AddHP(40 + (uint)Utils.Rand(21));

			bool alreadyDrunk = consumer.HasTimedEffect<NiamhDrunk>();

			if (alreadyDrunk)
			{
				consumer.GetTimedEffectData<NiamhDrunk>().StackEffect();
			}
			else
			{
				consumer.AddTimedEffect<NiamhDrunk>();
			}

			resultsOfUse = UseItemText(alreadyDrunk);
			isBadEnd = false;

			return true;
		}

		//combat consume is identical, so no need to override.
	}
}
