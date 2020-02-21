using System;
using System.Text;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Frontend.Items.Wearables.Armor
{

	public class LethiciteArmor : ArmorBase
	{
		public override string AbbreviatedName() => "LthcArm";

		public override string ItemName() => "lethicite armor";

		public override string ItemDescription(byte count = 1, bool displayCount = false)
		{
			//iirc theres only one of these ever, so it gets the 'you broke it' text if you have more than one at once.
			if (count == 0)
			{
				return "non-existent lethicite armor";
			}
			else if (count == 1)
			{
				return (displayCount ? "a " : "") + "suit of glowing purple lethicite armor";
			}
			else
			{
				return (displayCount ? Utils.NumberAsText(count) : "") + " suits of glowing purple lethicite armor. (This shouldn't be possible. Congrats on breaking things!)";
			}
		}

		//public override double DefensiveRating(Creature wearer) => 28;

		protected override int monetaryValue => 3000;

		public override string AboutItem()
		{
			return "This is a suit of lethicite armor. It's all purple and it seems to glow. The pauldrons are spiky to give this armor an imposing appearance. " +
				"It doesn't seem to cover your crotch and nipples though. It appears to be enchanted to never break and you highly doubt the demons will be able to eat it!";
		}

		public LethiciteArmor() : base(ArmorType.HEAVY_ARMOR)
		{
		}

		public override double PhysicalDefensiveRating(Creature wearer) => 20 + (int)Math.Floor(wearer.corruptionTrue / 10);

		protected override string EquipText(Creature wearer)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("You " + (wearer.wearingAnything ? "strip yourself naked before you " : "") + "proceed to put on the strange, purple crystalline armor. ");
			if (wearer.corruption < 33)
			{
				sb.Append("You hesitate at how the armor will expose your groin but you proceed to put it on anyway. ");
			}
			else if (wearer.corruption < 66)
			{
				sb.Append("You are not sure about the crotch-exposing armor. ");
			}
			else //if (wearer.corruption >= 66)
			{
				sb.Append("You are eager to show off once you get yourself suited up. ");
			}

			//Put on breastplate
			sb.Append(GlobalStrings.NewParagraph() + "First, you clamber into the breastplate. It has imposing, spiked pauldrons to protect your shoulders. The breastplate shifts to accommodate your [chest] and when you look down, your [nipples] are exposed. ");
			if (wearer.genitals.lactationRate >= 4)
			{
				sb.Append("A bit of milk gradually flows over your breastplate. ");
			}
			//Put on leggings
			if (wearer.isBiped)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Next, you slip into the leggings. By the time you get the leggings fully adjusted, you realize that the intricately-designed opening gives access to your groin! ");
				if (wearer.hasCock && !wearer.wearingLowerGarment)
				{
					sb.Append("Your " + wearer.genitals.AllCocksShortDescription(out bool isPlural) + (isPlural ? " hang" : "hangs") + " freely. ");
				}

				if (wearer.corruption < 33)
				{ //Low corruption
					if (wearer.wearingLowerGarment)
					{
						sb.Append("Good thing you have your " + wearer.lowerGarment + " on!");
					}
					else
					{
						sb.Append("You blush with embarrassment. ");
					}
				}
				else if (wearer.corruption < 66)
				{ //Medium corruption
					if (wearer.wearingLowerGarment)
					{
						sb.Append("You are unsure about whether you should keep your " + wearer.lowerGarment + " on or not.");
					}
					else
					{
						sb.Append("You are unsure how you feel about your crotch being exposed to the world.");
					}
				}
				else //if (wearer.corruption >= 66)
				{ //High corruption
					if (wearer.wearingLowerGarment)
					{
						sb.Append("You ponder over taking off your undergarments.");
					}
					else
					{
						sb.Append("You delight in having your nether regions open to the world.");
					}
				}
				sb.Append(" Then, you slip your feet into the 'boots'; they aren't even covering your feet. You presume they were originally designed for demons, considering how the demons either have high-heels or clawed feet.");
			}
			else
			{
				sb.Append(GlobalStrings.NewParagraph() + "The leggings are designed for someone with two legs so you leave them into your pack.");
			}
			//Finishing touches
			sb.Append(GlobalStrings.NewParagraph() + "Finally, you put the bracers on to protect your arms. Your fingers are still exposed so you can still get a good grip.");
			sb.Append(GlobalStrings.NewParagraph() + "You are ready to set off on your adventures!" + GlobalStrings.NewParagraph());

			return sb.ToString();
		}

		public override bool Equals(ArmorBase other)
		{
			return other is LethiciteArmor;
		}
	}

}