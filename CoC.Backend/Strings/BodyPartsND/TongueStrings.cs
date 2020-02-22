//TongueStrings.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 12:52 AM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class TonguePiercingLocation
	{
		private static string FrontButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string FrontLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MiddleButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string MiddleLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BackButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BackLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class Tongue
	{
		public static string Name()
		{
			return "Tongue";
		}

		private string OnlyOneTonguePiercingWithoutFetish()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTonguePiercingsShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllTonguePiercingsLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	//full desc adds "pierced " 30% of the time, if pierced.
	public partial class TongueType
	{
		private static string HumanDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("tongue", singleMemberFormat);
			//return "human tongue";
		}
		private static string HumanLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string HumanPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "";
		}
		private static string HumanTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			return previousTongueData.type.RestoredString(previousTongueData, player);
		}
		private static string HumanRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousTongueData, player);
		}
		private static string SnakeDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("forked, serpentine tongue", singleMemberFormat);
		}
		private static string SnakeLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string SnakePlayerStr(Tongue tongue, PlayerBase player)
		{
			return "A snake-like tongue occasionally flits between your lips, tasting the air.";
		}
		private static string SnakeTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			if (previousTongueData.type == HUMAN)
			{
				return "Your taste-buds start aching as they swell to an uncomfortably large size. "
					+ "Trying to understand what in the world could have provoked such a reaction, you bring your hands up to your mouth, "
					+ "your tongue feeling like it's trying to push its way past your lips."
					+ "\nThe soreness stops and you stick out your tongue to try and see what would have made it feel the way it did. "
					+ "As soon as you stick your tongue out you realize that it sticks out much further than it did before, "
					+ "and now appears to have split at the end, creating a forked tip."
					+ "\n<b>The scents in the air are much more noticeable to you with your snake-like tongue.</b>";
			}
			else
			{
				return "Your inhuman tongue shortens, pulling tight in the very back of your throat."
					+ " After a moment the bunched-up tongue-flesh begins to flatten out, then extend forwards."
					+ " By the time the transformation has finished, <b>your tongue has changed into a long, forked snake-tongue.</b>";
			}
		}
		private static string SnakeRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}
		private static string DemonicDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("demonic tongue", singleMemberFormat);
		}
		private static string DemonicLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string DemonicPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "A slowly undulating tongue occasionally slips from between your lips."
				+ " It hangs nearly two feet long when you let the whole thing slide out, though you can retract it to appear normal.";
		}
		//currently game only allows snake tongue => demon tongue. i'll assume that's the default, but create a custom one for other use.
		private static string DemonicTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (previousTongueData.type == SNAKE)
			{
				sb.Append("Your snake-like tongue tingles, thickening in your mouth until it feels more like your old human tongue, "
					+ "at least for the first few inches.It bunches up inside you, and when you open up your mouth to release it, "
					+ "roughly two feet of tongue dangles out.");
			}
			else if (previousTongueData.type.length >= 24)
			{
				sb.Append("Your incredibly-long tongue tingles, and starts to feel like it is shrinking down, though you can't be sure until you give it a good look. "
					+ "Your tongue flings out, but now only reaches two feet.");
			}
			else
			{
				sb.Append("Your tongue expands in your mouth, bulging to the point you need to open your mouth and release it. You realize it's now about " + Measurement.ToNearestHalfLargeUnit(24, false, true)
					+ "in length. Experimenting with it a bit, you eventually get used to the new length. ");
			}
			sb.Append("You find it easy to move and control, as natural as walking. <b> You now have a long demon-tongue.</b>");
			return sb.ToString();
		}
		private static string DemonicRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}
		private static string DraconicDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("draconic tongue", singleMemberFormat);
		}
		private static string DraconicLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string DraconicPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "Your mouth contains a thick, fleshy tongue that, if you so desire, can telescope to a distance of about " + Measurement.ToNearestHalfLargeUnit(48, false, true) + "."
				+ " It has sufficient manual dexterity that you can use it almost like a third arm.";
		}
		private static string DraconicTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			return "Your tongue suddenly falls out of your mouth and begins undulating as it grows longer. For a moment it swings wildly, "
				+ "completely out of control; but then settles down and you find you can control it at will, almost like a limb. "
				+ "You're able to stretch it to nearly 4 feet and retract it back into your mouth to the point it looks like a normal human tongue. <b>You now have a draconic tongue.</b>";
		}
		private static string DraconicRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}
		private static string EchidnaDesc(bool singleMemberFormat)
		{
			if (singleMemberFormat) return "an echidna tongue";
			else return "echidna tongue";
		}
		private static string EchidnaLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "an ");
		}
		private static string EchidnaPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "A thin echidna tongue, at least a foot long, occasionally flits out from between your lips. ";
		}
		private static string EchidnaTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("You feel an uncomfortable pressure in your tongue as it begins to shift and change. ");
			if (previousTongueData.type == SNAKE)
			{

			}
			else if (previousTongueData.type.length > 12)
			{
				sb.Append("Your tongue, thankfully, seems to be getting smaller. Strangely, though, it also is getting thinner. You behold your new tongue - ");
			}
			else
			{
				sb.Append("Within moments, you are able to behold your long, thin tongue. ");
			}
			sb.Append("It has to be at least a foot long. <b>You now have an echidna tongue!</b>");
			return sb.ToString();
		}
		private static string EchidnaRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}
		private static string LizardDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("reptilian tongue", singleMemberFormat);
		}
		private static string LizardLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string LizardPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "Your mouth contains a thick, fleshy lizard tongue, bringing to mind the tongue of large predatory reptiles."
				+ " It can reach up to one foot, its forked tips tasting the air as they flick at the end of each movement.";
		}
		private static string LizardTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("Your tongue goes numb, making your surprised noise little more than a gurgle as your tongue flops comically. ");
			sb.AppendLine();
			if (previousTongueData.type == SNAKE)
			{
				sb.Append("Slowly your tongue swells, thickening up until it's about as thick as your thumb, while staying quite "
					+ " flexible. You drool, your tongue lolling out of your mouth as you slowly begin to regain control of your forked"
					+ " organ. When you retract your tongue however, you are shocked to find it is much longer than it used to be,"
					+ " now a foot long. As you cram your newly shifted appendage back in your mouth, you feel a sudden SNAP,"
					+ " and on inspection, find you've snapped off your fangs! Well, you suppose you needed the room anyway."
					+ "\n\n<b>You now have a lizard tongue!</b>");
				return sb.ToString();
			}
			else if (previousTongueData.type == DEMONIC)
			{
				sb.Append("Your tongue gently shrinks down, the thick appendage remaining flexible but getting much smaller. There's"
					+ " little you can do but endure the weird pinching feeling as your tongue eventually settles at being a foot long."
					+ " The pinching sensation continues as the tip of your tongue morphs, becoming a distinctly forked shape.");
			}
			else if (previousTongueData.type == DRACONIC)
			{
				sb.Append("Your tongue rapidly shrinks down, the thick appendage remaining flexible but getting much smaller.There's"
					+ " little you can do but endure the weird pinching feeling as your tongue eventually settles at being a foot long."
					+ " The pinching sensation continues as the tip of your tongue morphs, becoming a distinctly forked shape.");
			}
			else if (previousTongueData.type == ECHIDNA)
			{
				sb.Append("Slowly your tongue swells, thickening up until it’s about as thick as your thumb, while staying long."
					+ " The tip pinches making you wince, morphing into a distinctly forked shape.");
			}
			else
			{
				if (previousTongueData.type != HUMAN)
				{
					sb.Append("It suddenly shifts towards a more human shape, but this doesn't last long.");
				}
				sb.Append("Slowly your tongue swells, thickening up until it’s about as thick as your thumb, filling your mouth as you"
					+ " splutter. It begins lengthening afterwards, continuing until it hangs out your mouth, settling at" + Measurement.ToNearestHalfLargeUnit(12, false, true) + "long. "
					+ " The tip pinches making you wince, morphing into a distinctly forked shape.");
			}
			sb.Append(" As you inspect your tongue you slowly regain control, retracting it into your mouth, the forked tips picking up"
				+ " on things you couldn't taste before."
				+ "\n\n<b>You now have a lizard tongue!</b>");
			return sb.ToString();
		}
		private static string LizardRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}
		private static string CatDesc(bool singleMemberFormat)
		{
			return Utils.AddArticleIf("feline tongue", singleMemberFormat);
		}
		private static string CatLongDesc(TongueData tongue, bool alternateFormat)
		{
			return GenericLongDesc(tongue, alternateFormat, "a ");
		}
		private static string CatPlayerStr(Tongue tongue, PlayerBase player)
		{
			return "Your tongue is rough like that of a cat. You sometimes groom yourself with it.";
		}
		private static string CatTransformStr(TongueData previousTongueData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (previousTongueData.isLongTongue)
			{
				sb.Append("Your tongue shortens dramatically, until it reaches a relatively normal length, though it still feels weird. ");
			}
			else
			{
				sb.Append("Your tongue suddenly feel weird. ");
			}
			sb.Append("You try to stick it out to see whats going on and discover it changed to look"
				+ " similar to the tongue of a cat. At least you will be able to groom yourself properly with <b>your new cat tongue</b>.");
			return sb.ToString();
		}
		private static string CatRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return GenericRestoreStr(previousTongueData, player);
		}

		private static string GenericLongDesc(TongueData tongue, bool alternateFormat, string alternateArticle)
		{
			StringBuilder sb = new StringBuilder();
			if (Utils.Rand(10) <= 2 && tongue.tonguePiercings.wearingJewelry)
			{
				if (alternateFormat)
				{
					sb.Append("a ");
				}
				sb.Append("pierced ");
			}
			else if (alternateFormat)
			{
				sb.Append(alternateArticle);
			}
			sb.Append(tongue.type.ShortDescription());
			return sb.ToString();
		}

		private static string GenericRestoreStr(TongueData previousTongueData, PlayerBase player)
		{
			return "You feel something strange inside your face as your tongue shrinks and recedes until it feels smooth and rounded. <b>You realize your tongue has changed back into human tongue!</b>";
		}
	}
}
