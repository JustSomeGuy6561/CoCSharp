//BackStrings.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 1:29 AM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Back
	{
		public static string Name()
		{
			return "Back";
		}
	}

	public partial class BackType
	{
		private string GenericBtnkDesc()
		{
			return "Back";
		}

		private string GenericLocDesc()
		{
			return " your back";
		}

		private static string NormalDesc()
		{
			return "normal back";
		}
		private static string NormalLongDesc(Back back)
		{
			return NormalDesc();
		}
		private static string NormalPlayerStr(Back back, PlayerBase player)
		{
			return "";
		}
		private static string NormalTransformStr(Back back, PlayerBase player)
		{
			return back.type.restoreString(back, player);
		}
		private static string NormalRestoreStr(Back back, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(back, player);
		}

		protected static string ManeDesc()
		{
			return "Mane";
		}
		protected static string YourManeDesc()
		{
			return " your mane";
		}

		protected static string DraconicManeDesc()
		{
			return "draconic back-mane";
		}
		protected static string DraconicManeLongDesc(Back back)
		{
			return "a mane of " + back.epidermalData.JustColor() + " draconic " + back.epidermalData.ShortDescription() + " along the spine"; //a mane of red draconic fur along the spine.
		}

		protected static string DraconicManePlayerStr(Back back, PlayerBase player)
		{
			return " Tracing your spine, a mane of " + back.epidermalData.JustColor() + " hair grows, starting at the base of your neck and " +
				(player.tail.type == TailType.DRACONIC ? "continuing down your tail, ending on the tip of your tail in a small tuft. " : " ending just above your butt. ") +
				"It grows in a thick vertical strip, maybe " + Measurement.ToNearestSmallUnit(2.0, false, true) + " wide, and vaguely reminds you of a horse's mane.";
		}
		protected static string DraconicManeTransformStr(Back back, PlayerBase player)
		{
			string seeItHow;
			if (player.neck.type == NeckType.DRACONIC || player.neck.type == NeckType.COCKATRICE)
			{
				seeItHow = "you bend your neck around to look at it. ";
			}
			else if (player.tail.type == TailType.DRACONIC)
			{
				seeItHow = "you bend your tail around and look at it. ";
			}
			else
			{
				seeItHow = "strain your neck trying to see it, but eventually give up and find something reflective, "
					+ " and watch it that way. ";
			}

			string tailText = player.tail.type != TailType.NONE ? "your tail, all the way to the end, where it forms a small tuft. " : "until it reaches your butt. ";
			string oldText = back.type != BackType.NORMAL ? "Somehow, your back has become perfectly smooth, though the tingling remains. " : "";

			return Utils.NewParagraph() + "You feel a sudden tingle just above your spine. Eager to see what's causing this, " + seeItHow + oldText +
				"Tiny splotches of hair begin growing out of your " + player.body.mainEpidermis.ShortDescription() +
				". The hair grows longer and the splotches grow until they slowly merge to a vertical strip right above your spine." +
				Utils.NewParagraph() + "The hair forms a mane along your spine, starting at the base of your neck and continuing down " + tailText +
				"It is the same color as the hair on your head, but shorter and denser; it has grown in a thick vertical strip, maybe " + Measurement.ToNearestSmallUnit(2, false, true) +
				" wide. It reminds you vaguely of a horse's mane. " + SafelyFormattedString.FormattedText("You now have a hairy mane along your spine!", StringFormats.BOLD);
		}
		protected static string DraconicManeRestoreStr(Back back, PlayerBase player)
		{
			return Utils.NewParagraph() + "You feel a tingling just above your spine. Your glimpse at your back and see hair falling down from it, first in strands, " +
				"then in bigger and bigger chunks, until " + SafelyFormattedString.FormattedText("Your hairy draconic mane has completely disappeared!", StringFormats.BOLD);
		}
		private static string DraconicSpikesDesc()
		{
			return "draconic back-spikes";
		}
		private static string DraconicSpikesLongDesc(Back back)
		{
			return "a series of draconic spikes along the spine";
		}
		private static string DraconicSpikesPlayerStr(Back back, PlayerBase player)
		{
			return " A row of short steel-gray spikes protrude along your spine, each one curving backward slightly. They start at the base of your neck and" +
				(player.tail.type == TailType.DRACONIC ? "continue down your tail, ending on the tip of your tail. " : " ending just above your butt. ") +
				"Each spike is roughly " + Measurement.ToNearestSmallUnit(2.0, false, true) + "high, and are spaced evenly along the way.";
		}
		private static string DraconicSpikesTransformStr(Back back, PlayerBase player)
		{
			string seeItHow;
			if (player.neck.type == NeckType.DRACONIC || player.neck.type == NeckType.COCKATRICE)
			{
				seeItHow = "you bend your neck around to look at it. ";
			}
			else if (player.tail.type == TailType.DRACONIC)
			{
				seeItHow = "you bend your tail around and look at it. ";
			}
			else
			{
				seeItHow = "strain your neck trying to see it, but eventually give up and find something reflective, "
					+ " and watch it that way. ";
			}

			string oldText = back.type != BackType.NORMAL ? " your back shakes violently, forcibly reshaping until it's once again smooth, but the pain still remains. " : "";
			string tailText = player.tail.type != TailType.NONE ? "your tail, ending just before the tip. " : "until it reaches your butt. ";

			return Utils.NewParagraph() + "You feel a sudden pain along your spine. Eager to see what's causing this, " + seeItHow + oldText +
				"The pain seems to focus into smaller areas, which start to bulge outward against your skin. The bulges grow larger and more painful until, with one final " +
				"burst of pain, spikes break through your skin. You nearly black out, but manage to retain your senses as they continue their growth, curving down and away from your neck. " +
				Utils.NewParagraph() + "Once they stop growing, you notice the spikes form a line along your spine, starting at the base of your neck and continuing down " + tailText +
				"The spikes are spaced evenly, each one steel-gray in color and maybe " + Measurement.ToNearestSmallUnit(2, false, true) +
				" tall, their base roughly " + Measurement.ToNearestHalfSmallUnit(1, false, true) + " in diameter. "
				+ SafelyFormattedString.FormattedText("Your spine is now decorated with a row of curved spikes!", StringFormats.BOLD);
		}
		private static string DraconicSpikesRestoreStr(Back back, PlayerBase player)
		{
			return Utils.NewParagraph() + "Your spine starts to make painful cracking sounds and you feel your spikes retracting along it. " +
				"The pain ceases, replaced with a strange tingling sensation as the skin along your spine closes over where your spikes once were." +
				SafelyFormattedString.FormattedText("The spikes on your rear have disappeared!", StringFormats.BOLD);
		}
		private static string SharkFinDesc()
		{
			return "shark fin";
		}
		private static string SharkFinLongDesc(Back back)
		{
			return "a shark fin sitting along the spine";
		}
		private static string SharkFinPlayerStr(Back back, PlayerBase player)
		{
			return " A large shark-like fin has sprouted between your shoulder blades. With it you have far more control over swimming underwater.";
		}
		private static string SharkFinTransformStr(Back back, PlayerBase player)
		{

			string introText = "You groan as a sharp pain between your shoulders forces you to the ground. Your breathing becomes ragged and you start sweating profusely.";

			if (back.type == BackType.TENDRILS)
			{
				return introText + " A sharp sensation seems to be pushing your tendrils off from the inside, and they fall to the ground as something else bursts from your spine. " +
					"Replacing them is a strange fin-like structure, not nearly as wide but covering much more of your spine." +
					SafelyFormattedString.FormattedText("You now have a shark-like fin!", StringFormats.BOLD);
			}
			else if (back.type == BackType.DRACONIC_SPIKES)
			{
				return introText + " Your spikes seem to be moving up along your back, fusing together as they reach the area between your shoulders. They reshape, and skin forms around them." +
					"Once the pain subsides, you notice your spikes " + SafelyFormattedString.FormattedText("are now a shark's fin!", StringFormats.BOLD);
			}
			else if (back.type == BackType.BEHEMOTH)
			{
				return introText + "Your spikes start to move inward, toward the hair along your spine. As they do, the hair starts to fall away, briefly leaving clear skin before the spikes" +
					"cover where it once was. the two rows become one, fusing together as they travel along your towards your shoulders. ";
			}
			else if (back.type == BackType.DRACONIC_MANE)
			{
				return introText + " the hair forming the mane along your back starts falling off in great chunks, as if pushed off from the inside. Just as the final strand of hair falls away," +
					"a large, fin like structure pushes out of your back and replaces it. " + SafelyFormattedString.FormattedText("You now have a shark fin!", StringFormats.BOLD);
			}
			else if (back.type != BackType.NORMAL)
			{
				string removeArmorText = player.wearingAnything ? " You hastily remove your " + player.armor.shortName() + " and " : "You ";
					
				return introText + removeArmorText + "notice your back isn't the same anymore - in fact, it appears normal. But it doesn't remain that way for long when a strange fin-like " +
					"structure bursts from in-between your shoulders. You examine it carefully and make a few modifications to your " + player.armor.shortName() + " to accommodate your new fin.";
			}
			else
			{
				string removeArmorText = player.wearingAnything ? " You hastily remove your " + player.armor.shortName() + " barely getting it off before a " : "A ";

				return introText + removeArmorText + "strange fin-like structure bursts from in-between your shoulders. You examine it carefully and make a few modifications to your " + 
					player.armor.shortName() + " to accommodate your new fin.";
			}
		}
		private static string SharkFinRestoreStr(Back back, PlayerBase player)
		{
			return Utils.NewParagraph() + "A wave of tightness spreads through your back, and it feels as if someone is stabbing a dagger into your spine. " +
				SafelyFormattedString.FormattedText("After a moment the pain passes, though your fin is gone!", StringFormats.BOLD);
		}



		private static string TendrilShortDesc()
		{
			return "tendril-covered back";
		}
		private static string TenderilLongDesc(Back back)
		{
			return "several inky-black tendrils sprouting from the center of back along the spine";
		}
		private static string TendrilPlayerStr(Back back, PlayerBase player)
		{
			//initially starts with full tendrils, so if we aren't at max, then we are missing some from combat. 


			if (back.resources != 0)
			{
				double percent = back.resources * 1.0 / back.maxCharges;
				string howManyTendrils;
				if (percent >= 1)
				{
					howManyTendrils = "You have all of your tendrils, and they grow back quickly should you ever lose one. ";
				}
				else if (percent >= 0.66)
				{
					howManyTendrils = "You've lost a few tendrils from some previous encounters, though you still have most of them, and the others will grow back quickly. ";
				}
				else if (percent >= 0.33)
				{
					howManyTendrils = "You've lost some of your tendrils during previous encounters, but have enough should things get dicey. Plus, the lost ones will grow back relatively quickly. ";
				}
				else //if (percent > 0)
				{
					howManyTendrils = "You've lost most of your tendrils during previous encounters, though these will grow back over time. ";
				}

				return "A series of " + back.epidermalData.JustColor() + " tendrils form around the middle of your back. For the most part, you keep them pressed along your spine " +
					"in order to keep them relatively inconspicuous, but you can send them out to grab things when needed. " + howManyTendrils;
			}
			else
			{
				return "A series of dark patches along your back highlight the area you'd normally have tendrils. At the moment, you've lost all of them during your previous encounters, " +
					"but they grow back rather quickly.";
			}
		}
		private static string TendrilTransformStr(Back back, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("A stabbing pain erupts along your back. The pain is so intense, ");
			if (player.lowerBody.isBiped || player.lowerBody.type == LowerBodyType.GOO)
			{
				sb.Append("you're forced onto your " + player.hands.ShortDescription() + ", your back arched toward the sky. ");
			}
			else
			{
				sb.Append("you collapse to the ground, arching your back to try and alleviate it in some way. ");
			}

			//lazy man's way - just 'revert' it first, then go as if human.
			if (back.type != BackType.NORMAL)
			{
				sb.Append("Your back spasms violently, forcing you in and out of consciousness as your body handles a forced change. When it stops, you back is completely smooth, " +
					"but another wave of pain suggests it's far from over. ");
			}

			sb.Append("A scream escapes your lips as the pain intensifies, culminating in a tearing sound as something bursts out around the middle of your back. " +
				"It's quickly followed by several more, though you're in no condition to determine the exact amount. Finally, mercifully, it stops. Feeling along your back, " +
				"You grab ahold of something soft, almost squishy, and you pull it around to the front to see exactly what it is. It suddenly squirms to life, and you panic, " +
				"yanking it and throwing it away from you. Realizing what you just did, you brace for even more pain, but all you feel is minor discomfort. Cautiously, " +
				"you investigate what exactly it was you just ripped off, which has since stopped squirming. It's a tendril - long, thing, and inky-black. " +
				"You realize you have several of these along your back now, though one less than what your started with. Just as that thought crosses your mind, a tugging sensation " +
				"occurs where the now severed tendril attached to your back, and you place your hand back there again. It seems a new tendril is growing in, right where the old one was. " +
				"Seems like " + SafelyFormattedString.FormattedText("you now have tendrils growing along your back!", StringFormats.BOLD) + "You soon realize you can control them like a " +
				"third arm (well, third and fourth and fifth, and so on). No doubt, these could be useful in combat");

			if (player.corruption >= 50)
			{
				sb.Append(", or in other, \"kinkier\" situations, you note with a smirk");
			}
			sb.Append(".");

			return sb.ToString();
		}
		private static string TendrilRestoreStr(Back back, PlayerBase player)
		{
			return "You hear a faint pop, and see one of your tendrils falling to the ground out of the corner of your eye. " +
				SafelyFormattedString.FormattedText("That's strange", StringFormats.ITALIC) + " you think, though it's possible you whacked it on something earlier, and they are quite fragile." +
				" Several more pops follow, however, and now you're sure something's up. Each of your tentacles fall to the ground, and when you feel along your back, they show no signs of " +
				"regrowing. In fact, your back feels completely smooth. It seems " + SafelyFormattedString.FormattedText("you've lost your tendrils!", StringFormats.BOLD);
		}

		protected static string BehemothDesc()
		{
			return "spike-surrounded behemoth back-mane";
		}
		protected static string BehemothLongDesc(Back back)
		{
			return "behemoth-like, complete with a mane of " + back.epidermalData.LongDescription() + " along the spine, set between two rows of small spikes";
		}
		protected static string BehemothPlayerStr(Back back, PlayerBase player)
		{
			return " Tracing your spine, a mane of " + back.epidermalData.JustColor() + " hair grows, starting at the base of your neck and " +
				(player.tail.type == TailType.BEHEMOTH ? "continuing down the entire length, finally merging with the hair along your tail. " : " ending just above your butt. ") +
				"On each side of it are a set of small spikes, spaced evenly. each spike is maybe" + Measurement.ToNearestHalfSmallUnit(0.5, false, true, false) + "thick, " +
				"and the strip of hair is roughly " + Measurement.ToNearestSmallUnit(2.0, false, true) + " wide, tapering off near the base of your neck slightly";
		}
		protected static string BehemothTransformStr(Back back, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(Utils.NewParagraph() + "Your back and spine are suddenly overwhelmed with a sense of pins-and-needles. Unsure as to what exactly is causing this, you " +
				(player.wearingAnything ? "undress and " : "") + "find something reflective to check your back with.");

			if (back.type == BackType.DRACONIC_SPIKES)
			{
				sb.Append("Strangely, each one of your spikes has started to split, forming two smaller spikes to the left and right of their previous location. Hair starts to grow between each " +
					"new pair, which continue to move away from the center of your back. Eventually, they stop moving and straighten out instead. Once this finishes, you notice ");

			}
			else if (back.type == BackType.DRACONIC_MANE)
			{
				sb.Append("Along both sides of the mane running down your spine, a set of small spikes poke through your skin, causing intense discomfort. Once the spikes have finished, " +
					"you notice ");
			}

			else
			{
				if (back.type != BackType.NORMAL)
				{
					sb.Append("It seems that your back became completely normal while you were searching for something reflective. That was fast! You shrug, assuming that was it, when you" +
						"notice something else happening. ");
				}
				sb.Append("You begin to notice a unique tuft of hair forming along your spine, starting at the base of your neck and continuing down ");
				sb.Append(player.tail.type != TailType.NONE ? "your tail, all the way to the end, where it forms a small tuft. " : "until it reaches your butt. ");
				sb.Append("It is the same color as the hair on your head, but shorter and denser. Along either side of it, a set of small spikes poke through your skin, " +
					"causing intense discomfort. Once the spikes have finished, you notice ");
			}




			sb.Append(SafelyFormattedString.FormattedText("you have a back much like your behemoth friend!", StringFormats.BOLD) + "The hair forms a thick vertical strip, maybe " +
				Measurement.ToNearestSmallUnit(2, false, true) + " wide, and you'd estimate each spike is roughly half that width, but protrudes from your back at least that far.");
			return sb.ToString();
		}
		protected static string BehemothRestoreStr(Back back, PlayerBase player)
		{
			return "Your back and spine are suddenly overwhelmed with a sense of pins-and-needles. Unsure as to what exactly is causing this, you " +
				(player.wearingAnything ? "undress and " : "") + "find something reflective to check your back with. It seems that your back became completely normal " +
				"while you were searching for something reflective. That was fast! You wait for anything else to happen, and when nothing does, you are sure " +
				SafelyFormattedString.FormattedText("Your back is completely normal again!", StringFormats.BOLD);
		}
	}
}
