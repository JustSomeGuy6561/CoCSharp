///**
// * Created by aimozg on 11.01.14.
// */
//using System;
//using System.Text;
//using CoC.Backend.BodyParts;
//using CoC.Backend.Creatures;
//using CoC.Backend.Items.Consumables;
//using CoC.Backend.Strings;
//using CoC.Backend.Tools;

//namespace CoC.Frontend.Items.Consumables
//{

//	//
//	public sealed class HairExtensionSerum : StandardConsumable
//	{

//		public HairExtensionSerum() : base()
//		{ }

//		public override string AbbreviatedName() => "ExtSerm";
//		public override string ItemName() => "ExtSerm";
//		public override string ItemDescription(byte count, bool displayCount = false)
//		{
//			throw;
//			//"a bottle of hair extension serum";
//		}
//		public override string AboutItem() => "This is a bottle of foamy pink liquid, purported by the label to increase the speed at which the user's hair grows.";
//		protected override int monetaryValue => 6;

//		public override bool countsAsLiquid => true;
//		public override bool countsAsCum => false;

//		public override bool CanUse(Creature target, bool currentlyInCombat, out string whyNot)
//		{
//			if (flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED] <= 2)
//			{
//				return true;
//			}

//			sb.Append("<b>No way!</b> Your head itches like mad from using the rest of these, and you will NOT use another." + Environment.NewLine);
//			return false;
//		}

//		protected override string OnConsumeAttempt(Creature consumer, out bool consumeItem, out bool isBadEnd)
//		{
//			sb.Append("You open the bottle of hair extension serum and follow the directions carefully, massaging it into your scalp and being careful to keep it from getting on any other skin. You wash off your hands with lakewater just to be sure.");
//			if (player.hair.type == HairType.BASILISK_SPINES)
//			{
//				sb.Append(GlobalStrings.NewParagraph() + "You wait a while, expecting a tingle on your head, but nothing happens. You sigh as you realize, that your "
//					 + player.hair.color + " basilisk spines are immune to the serum ...");
//				return false;
//			}
//			if (flags[kFLAGS.INCREASED_HAIR_GROWTH_TIME_REMAINING] <= 0)
//			{
//				sb.Append(GlobalStrings.NewParagraph() + "The tingling on your head lets you know that it's working!");
//				flags[kFLAGS.INCREASED_HAIR_GROWTH_TIME_REMAINING] = 7;
//				flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED] = 1;
//			}
//			else if (flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED] == 1)
//			{
//				sb.Append(GlobalStrings.NewParagraph() + "The tingling intensifies, nearly making you feel like tiny invisible faeries are massaging your scalp.");
//				flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED]++;
//			}
//			else if (flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED] == 2)
//			{
//				sb.Append(GlobalStrings.NewParagraph() + "The tingling on your scalp is intolerable! It's like your head is a swarm of angry ants, though you could swear your hair is growing so fast that you can feel it weighing you down more and more!");
//				flags[kFLAGS.INCREASED_HAIR_GROWTH_SERUM_TIMES_APPLIED]++;
//			}
//			if (flags[kFLAGS.HAIR_GROWTH_STOPPED_BECAUSE_LIZARD] > 0 && player.hair.type != HairType.ANEMONE)
//			{
//				flags[kFLAGS.HAIR_GROWTH_STOPPED_BECAUSE_LIZARD] = 0;
//				sb.Append(GlobalStrings.NewParagraph() + "<b>Somehow you know that your " + player.hairDescript() + " is growing again.</b>");
//			}
//			if (flags[kFLAGS.INCREASED_HAIR_GROWTH_TIME_REMAINING] < 7)
//			{
//				flags[kFLAGS.INCREASED_HAIR_GROWTH_TIME_REMAINING] = 7;
//			}

//			return false;
//		}
//	}
//}
