using System;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.Creatures;
using CoC.Backend.Items;
using CoC.Backend.Items.Consumables;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Frontend.Perks;
using CoC.Frontend.StatusEffect;

namespace CoC.Frontend.Items.Consumables
{


	/**
	 * @since April 3, 2018
	 * @author Stadler76
	 */
	public class BimboChampagne : StandardConsumable
	{
		public BimboChampagne() : base()
		{ }

		public override string AbbreviatedName() => "BimboCh";
		public override string ItemName() => "Bimbo Champagne";
		public override string ItemDescription(byte count, bool displayCount = false)
		{
			string vialText = count != 1 ? "bottles" : "bottle";

			string countText = displayCount ? (count == 1 ? "a " : Utils.NumberAsText(count)) + " " : "";

			return $"{count}{vialText} of bimbo champagne";
		}
		public override string AboutItem() => "A bottle of bimbo champagne. Drinking this might incur temporary bimbofication.";
		protected override int monetaryValue => 1;

		public override bool countsAsLiquid => true;
		public override bool countsAsCum => false;

		public override byte sateHungerAmount => 0;
		public override bool Equals(CapacityItem other)
		{
			return other is BimboChampagne;
		}


		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
		{
			isBadEnd = false;
			consumeItem = true;
			//has Bimbo/Futa/Bro perk (or else it'd return null) and bimbo Body flag is true.
			if (consumer.GetPerkData<BimBro>()?.hasBimboEffect == true)
			{
				return "You could've swore the stuff worked when you saw Niamh do it to others,"
					 + " but for some reason, it had, like, no effect on you. How weird!";
			}
			else if (!consumer.HasTimedEffect<TemporaryBimbification>())
			{
				consumer.AddTimedEffect<TemporaryBimbification>();
				string intro = "You uncork the bottle and breathe in the fizzy, spicy aroma of the sparkling liquor."
					+ " Breathing deeply, you open your mouth and begin pouring the ever-effervescent fluid inside."
					+ " It's sweet and slightly gooey, and the feel of it sliding down your throat is intensely... awesome? Like, totally!";

				return intro + consumer.GetTimedEffectData<TemporaryBimbification>().ObtainText();
			}
			else
			{
				consumer.GetTimedEffectData<TemporaryBimbification>().IncreaseEffect();

				return "You find yourself falling even further into the dense bimbo mindset. You do feel, like, super-good and all, though!"
					+ GlobalStrings.NewParagraph() + "Moaning lewdly, you begin to sway your hips from side to side, putting on a show for anyone "
					+ "who might manage to see you.  You just feel so... sexy. Too sexy to hide it. Your body aches to show itself and feel the gaze of someone, "
					+ "anyone upon it. Mmmm, it makes you so wet! You sink your fingers into your sloppy cunt with a groan of satisfaction."
					+ " Somehow, you feel like you could fuck anyone right now!";

			}
		}
	}
}
