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
	//full desc adds "pierced " 30% of the time, if pierced.

	public partial class TongueType
	{
		private static string HumanDesc()
		{
			return "tongue";
			//return "human tongue";
		}
		private static string HumanFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, HumanDesc());
		}
		private static string HumanPlayerStr(Tongue tongue, Player player)
		{
			return "";
		}
		private static string HumanTransformStr(Tongue tongue, Player player)
		{
			return tongue.restoreString(player);
		}
		private static string HumanRestoreStr(Tongue tongue, Player player)
		{
			return GlobalStrings.RevertAsDefault(tongue, player);
		}
		private static string SnakeDesc()
		{
			return "serpentine tongue";
		}
		private static string SnakeFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, SnakeDesc());
		}
		private static string SnakePlayerStr(Tongue tongue, Player player)
		{
			return "A snake-like tongue occasionally flits between your lips, tasting the air.";
		}
		private static string SnakeTransformStr(Tongue tongue, Player player)
		{
			if (tongue.type == HUMAN)
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
		private static string SnakeRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}
		private static string DemonicDesc()
		{
			return "demonic tongue";
		}
		private static string DemonicFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, DemonicDesc());
		}
		private static string DemonicPlayerStr(Tongue tongue, Player player)
		{
			return "A slowly undulating tongue occasionally slips from between your lips."
				+ " It hangs nearly two feet long when you let the whole thing slide out, though you can retract it to appear normal.";
		}
		//currently game only allows snake tongue => demon tongue. i'll assume that's the default, but create a custom one for other use.
		private static string DemonicTransformStr(Tongue tongue, Player player)
		{
			StringBuilder sb = new StringBuilder();
			if (tongue.type == SNAKE)
			{
				sb.Append("Your snake-like tongue tingles, thickening in your mouth until it feels more like your old human tongue, "
					+ "at least for the first few inches.It bunches up inside you, and when you open up your mouth to release it, "
					+ "roughly two feet of tongue dangles out.");
			}
			else if (tongue.type.length >= 24)
			{
				sb.Append("Your incredibly-long tongue tingles, and starts to feel like it is shrinking down, though you can't be sure until you give it a good look. "
					+ "Your tongue flings out, but now only reaches two feet.");
			}
			else
			{
				sb.Append("Your tongue expands in your mouth, bulging to the point you need to open your mouth and release it. You realize it's now about " + GlobalStrings.FeetOrMeters(24)
					+ "in length. Experimenting with it a bit, you eventually get used to the new length. ");
			}
			sb.Append("You find it easy to move and control, as natural as walking. <b> You now have a long demon-tongue.</b>");
			return sb.ToString();
		}
		private static string DemonicRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}
		private static string DraconicDesc()
		{
			return "draconic tongue";
		}
		private static string DraconicFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, DraconicDesc());
		}
		private static string DraconicPlayerStr(Tongue tongue, Player player)
		{
			return "Your mouth contains a thick, fleshy tongue that, if you so desire, can telescope to a distance of about " + GlobalStrings.FeetOrMeters(48) +"."
				+ " It has sufficient manual dexterity that you can use it almost like a third arm.";
		}
		private static string DraconicTransformStr(Tongue tongue, Player player)
		{
			return "Your tongue suddenly falls out of your mouth and begins undulating as it grows longer. For a moment it swings wildly, "
				+ "completely out of control; but then settles down and you find you can control it at will, almost like a limb. "
				+ "You're able to stretch it to nearly 4 feet and retract it back into your mouth to the point it looks like a normal human tongue. <b>You now have a draconic tongue.</b>";
		}
		private static string DraconicRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}
		private static string EchidnaDesc()
		{
			return "echidna tongue";
		}
		private static string EchidnaFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, EchidnaDesc());
		}
		private static string EchidnaPlayerStr(Tongue tongue, Player player)
		{
			return "A thin echidna tongue, at least a foot long, occasionally flits out from between your lips. ";
		}
		private static string EchidnaTransformStr(Tongue tongue, Player player)
		{
			StringBuilder sb = new StringBuilder("You feel an uncomfortable pressure in your tongue as it begins to shift and change. ");
			if (tongue.type == SNAKE)
			{

			}
			else if (tongue.type.length > 12)
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
		private static string EchidnaRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}
		private static string LizardDesc()
		{
			return "lizard tongue";
		}
		private static string LizardFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, LizardDesc());
		}
		private static string LizardPlayerStr(Tongue tongue, Player player)
		{
			return "Your mouth contains a thick, fleshy lizard tongue, bringing to mind the tongue of large predatory reptiles."
				+ " It can reach up to one foot, its forked tips tasting the air as they flick at the end of each movement.";
		}
		private static string LizardTransformStr(Tongue tongue, Player player)
		{
			StringBuilder sb = new StringBuilder("Your tongue goes numb, making your surprised noise little more than a gurgle as your tongue flops comically. ");
			sb.AppendLine();
			if (tongue.type == SNAKE)
			{
				sb.Append("Slowly your tongue swells, thickening up until it's about as thick as your thumb, while staying quite "
					+ " flexible. You drool, your tongue lolling out of your mouth as you slowly begin to regain control of your forked"
					+ " organ. When you retract your tongue however, you are shocked to find it is much longer than it used to be,"
					+ " now a foot long. As you cram your newly shifted appendage back in your mouth, you feel a sudden SNAP,"
					+ " and on inspection, find you've snapped off your fangs! Well, you suppose you needed the room anyway."
					+ "\n\n<b>You now have a lizard tongue!</b>");
				return sb.ToString();
			}
			else if (tongue.type == DEMONIC)
			{
				sb.Append("Your tongue gently shrinks down, the thick appendage remaining flexible but getting much smaller. There's"
					+ " little you can do but endure the weird pinching feeling as your tongue eventually settles at being a foot long."
					+ " The pinching sensation continues as the tip of your tongue morphs, becoming a distinctly forked shape.");
			}
			else if (tongue.type == DRACONIC)
			{
				sb.Append("Your tongue rapidly shrinks down, the thick appendage remaining flexible but getting much smaller.There's"
					+ " little you can do but endure the weird pinching feeling as your tongue eventually settles at being a foot long."
					+ " The pinching sensation continues as the tip of your tongue morphs, becoming a distinctly forked shape.");
			}
			else if (tongue.type == ECHIDNA)
			{
				sb.Append("Slowly your tongue swells, thickening up until it’s about as thick as your thumb, while staying long."
					+ " The tip pinches making you wince, morphing into a distinctly forked shape.");
			}
			else
			{
				if (tongue.type != HUMAN)
				{
					sb.Append("It suddenly shifts towards a more human shape, but this doesn't last long.");
				}
				sb.Append("Slowly your tongue swells, thickening up until it’s about as thick as your thumb, filling your mouth as you"
					+ " splutter. It begins lengthening afterwards, continuing until it hangs out your mouth, settling at" + GlobalStrings.FeetOrMeters(12) + "long. "
					+ " The tip pinches making you wince, morphing into a distinctly forked shape.");
			}
			sb.Append(" As you inspect your tongue you slowly regain control, retracting it into your mouth, the forked tips picking up"
				+ " on things you couldn't taste before."
				+ "\n\n<b>You now have a lizard tongue!</b>");
			return sb.ToString();
		}
		private static string LizardRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}
		private static string CatDesc()
		{
			return "cat tongue";
		}
		private static string CatFullDesc(Tongue tongue)
		{
			return GenericFullDesc(tongue, CatDesc());
		}
		private static string CatPlayerStr(Tongue tongue, Player player)
		{
			return "Your tongue is rough like that of a cat. You sometimes groom yourself with it.";
		}
		private static string CatTransformStr(Tongue tongue, Player player)
		{
			StringBuilder sb = new StringBuilder();
			if (tongue.isLongTongue)
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
		private static string CatRestoreStr(Tongue tongue, Player player)
		{
			return GenericRestoreStr(tongue, player);
		}

		private static string GenericFullDesc(Tongue tongue, string partialDesc)
		{
			StringBuilder sb = new StringBuilder();
			if (Utils.Rand(10) <= 2)
			{
				if (tongue.tonguePiercings.jewelryCount > 0)
				{
					sb.Append("pierced ");
				}
			}
			sb.Append(partialDesc);
			return sb.ToString();
		}

		private static string GenericRestoreStr(Tongue tongue, Player player)
		{
			return "You feel something strange inside your face as your tongue shrinks and recedes until it feels smooth and rounded. <b>You realize your tongue has changed back into human tongue!</b>";
		}
	}
}
