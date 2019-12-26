//TailStrings.cs
//Description:
//Author: JustSomeGuy
//1/7/2019, 9:33 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Tail
	{
		public string OneTailShortDescription(string pronoun = "your")
		{
			if (tailCount == 0)
			{
				return "";
			}
			else if (tailCount == 1)
			{
				return pronoun + " " + ShortDescription();
			}
			else return "one of " + pronoun + " " + ShortDescription();
		}

		public string EachTailShortDescription(string pronoun = "your")
		{
			if (tailCount == 0)
			{
				return "";
			}
			else if (tailCount == 1)
			{
				return pronoun + " " + ShortDescription();
			}
			else return "each of " + pronoun + " " + ShortDescription();
		}

		public static string Name()
		{
			return "Tail";
		}
	}

	public partial class TailType
	{
		protected static string NoTailShortDesc(bool alternateFormat)
		{
			return "";
		}
		protected static string NoTailLongDesc(TailData tail, bool alternateFormat)
		{
			return alternateFormat ? "no tails" : "non-existent tail";
		}
		protected static string NoTailPlayerStr(Tail tail, PlayerBase player)
		{
			return "";
		}
		protected static string NoTailTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			return previousTailData.type.RestoredString(previousTailData, player);
		}
		protected static string NoTailRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			return GlobalStrings.RevertAsDefault(previousTailData, player);
		}
		private static string HorseShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("horse tail", alternateFormat);
		}
		private static string HorseLongDesc(TailData tail, bool alternateFormat)
		{
			return tail.primaryEpidermis.AdjectiveDescription(alternateFormat) + "horse tail";
		}
		private static string HorsePlayerStr(Tail tail, PlayerBase player)
		{
			return "A long " + tail.epidermis.JustColor() + " horsetail hangs from your butt, smooth and shiny.";
		}
		private static string HorseTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//StringBuilder sb = new StringBuilder();
			//if (tail.type == NONE)
			//{
			//	sb.Append("There is a sudden tickling on your ass, and you notice you have sprouted a long shiny horsetail of the same " + player.hair.color + " color as your hair.");
			//}
			//else if (tail.type == BEE_ABDOMEN || tail.type == SPIDER_ABDOMEN)
			//{
			//	sb.Append("Your insect-like abdomen bunches up as it begins shrinking, exoskeleton flaking off like a snake sheds its skin. "
			//		+ "It bunches up until it is as small as a tennis ball, then explodes outwards, growing into an animalistic tail shape. "
			//		+ "Moments later, it explodes into filaments of pain, dividing into hundreds of strands and turning into a shiny horsetail.");
			//}
			//else
			//{
			//	sb.Append("Pain lances up your " + player.assholeDescript() + " as your tail shifts and morphs disgustingly. " +
			//		"With one last wave of pain, it splits into hundreds of tiny filaments, transforming into a horsetail.");
			//}
			//sb.Append(" <b>You now have a horse-tail.</b>");
			//return sb.ToString();
		}
		private static string HorseRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("dog tail", alternateFormat);
		}
		private static string DogLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "fluffy dog tail";
		}
		private static string DogPlayerStr(Tail tail, PlayerBase player)
		{
			return "A fuzzy " + tail.epidermis.JustColor() + " dogtail sprouts just above your " + player.butt.ShortDescription() + ", wagging to and fro whenever you are happy.";
		}
		private static string DogTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			StringBuilder sb = new StringBuilder();
			if (previousTailData.type == NONE)
			{
				sb.Append("A pressure builds on your backside. You feel under your clothes and discover an odd bump that seems to be growing larger by the moment. " +
					"In seconds it passes between your fingers, bursts out the back of your clothes, and grows most of the way to the ground. A thick coat of fur springs up to cover your new tail. ");
			}
			else if (previousTailData.type == HORSE)
			{
				sb.Append("You feel a tightness in your rump, matched by the tightness with which the strands of your tail clump together. In seconds they fuse into a single tail, rapidly sprouting thick fur. ");
			}
			else if (previousTailData.type == DEMONIC)
			{
				sb.Append("The tip of your tail feels strange. As you pull it around to check on it, the spaded tip disappears, quickly replaced by a thick coat of fur over the entire surface of your tail. ");
			}
			//Generic message for now
			else
			{
				sb.Append("You feel your backside shift and change, flesh molding and displacing into a long puffy tail! ");
			}
			sb.Append("<b>You now have a dog-tail.</b>");
			return sb.ToString();
		}
		private static string DogRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DemonShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("demonic tail", alternateFormat);
		}
		private static string DemonLongDesc(TailData tail, bool alternateFormat)
		{

			if (tail.tailPiercings.wearingJewelry && Utils.Rand(10) <= 2)
			{
				return (alternateFormat ? "a " : "") + "pierced, demonic tail";
			}
			return (alternateFormat ? "a " : "") + "demonic tail";
		}


		private static string DemonPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//string retVal = "  A narrow tail ending in a ";
			//if (tail.isPierced && Utils.Rand(10) <= 2)
			//{
			//	retVal += "pierced, ";
			//}
			//retVal += "spaded tip curls down from your " + player.butt.shortDescription();
			//if (player.lowerBody.legCount > 1)
			//{
			//	retVal += ", wrapping around your leg sensually at every opportunity";
			//}
			//retVal += ".";
			//return retVal;
		}
		private static string DemonTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			if (previousTailData.type == NONE)
			{
				return "A pain builds in your backside... growing more and more pronounced. The pressure suddenly disappears with a loud ripping and tearing noise. <b>You realize you now have a demon tail</b>... complete with a cute little spade.";
			}
			else if (previousTailData.type == SPIDER_SPINNERET || previousTailData.type == BEE_STINGER)
			{
				return "You feel a tingling in your insectile abdomen as it stretches, narrowing, the exoskeleton flaking off as it transforms into a flexible demon-tail, complete with a round spaded tip. ";
			}
			else
			{
				return "You feel a tingling in your tail. You are amazed to discover it has shifted into a flexible demon-tail, complete with a round spaded tip. <b>Your tail is now demonic in appearance.</b> ";
			}
		}
		private static string DemonRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CowShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("cow tail", alternateFormat);
		}
		private static string CowLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "cow tail";
		}
		private static string CowPlayerStr(Tail tail, PlayerBase player)
		{
			return "A long cowtail with a puffy tip swishes back and forth as if swatting at flies.";
		}
		private static string CowTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.type == NONE)
			//{
			//	return "You feel the flesh above your " + player.butt.shortDescription() + " knotting and growing. It twists and writhes around itself before flopping straight down, now shaped into a distinctly bovine form. You have a <b>cow tail</b>.";
			//}
			//else if (tail.type == SPIDER_ABDOMEN || tail.type == BEE_ABDOMEN)
			//{
			//	return "Your insect-like abdomen tingles pleasantly as it begins shrinking and softening, chitin morphing and reshaping until it looks exactly like a <b>cow tail</b>.";
			//}
			//else
			//{
			//	return "Your tail bunches uncomfortably, twisting and writhing around itself before flopping straight down, now shaped into a distinctly bovine form. You have a <b>cow tail</b>.";
			//}
		}
		private static string CowRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string SpiderShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("spider abdomen", alternateFormat);
		}
		private static string SpiderLongDesc(TailData tail, bool alternateFormat)
		{
			//maybe include venom count indication, idk.
			return (alternateFormat ? "a " : "") + "spider abdomen";
		}
		private static string SpiderPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//StringBuilder sb = new StringBuilder("A large, spherical spider-abdomen has grown out from your backside, covered in shiny black chitin. "
			//+ "Though it's heavy and bobs with every motion, it doesn't seem to slow you down.");
			//				if (player.tail.venom > 50 && player.tail.venom < 80)
			//					Console.WriteLine("  Your bulging arachnid posterior feels fairly full of webbing.");
			//				if (player.tail.venom >= 80 && player.tail.venom < 100)
			//					Console.WriteLine("  Your arachnid rear bulges and feels very full of webbing.");
			//				if (player.tail.venom == 100)
			//					Console.WriteLine("  Your swollen spider-butt is distended with the sheer amount of webbing it's holding.");
			//return sb.ToString();
		}
		private static string SpiderTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.type == NONE)
			//{
			//	return "A burst of pain hits you just above your " + player.butt.shortDescription() + ", coupled with a sensation of burning heat and pressure. "
			//		+ "You can feel your " + tail.epidermis.FullDescription() + " tearing as something forces its way out of your body. Reaching back, you grab at it with your hands. "
			//		+ "It's huge... and you can feel it hardening under your touches, firming up until the whole tail has become rock-hard and spherical in shape. "
			//		+ "The heat fades, leaving behind a gentle warmth, and you realize your tail has become a spider's abdomen! With one experimental clench, you even discover that it can shoot"
			//		+ " webs from some of its spinnerets, both sticky and non-adhesive ones. That may prove useful. <b>You now have a spider's abdomen hanging from above your " + player.butt.shortDescription() + "!</b>";
			//}
			//else
			//{
			//	return "Your tail shudders as heat races through it, twitching violently until it feels almost as if it's on fire. "
			//		+ "You jump from the pain at your " + player.butt.shortDescription() + " and grab at it with your hands.  "
			//		+ "It's huge... and you can feel it hardening under your touches, firming up until the whole tail has become rock-hard and spherical in shape. "
			//		+ "The heat fades, leaving behind a gentle warmth, and you realize your tail has become a spider's abdomen! "
			//		+ "With one experimental clench, you even discover that it can shoot webs from some of its spinnerets, both sticky and non-adhesive ones. "
			//		+ " That may prove useful.  <b>You now have a spider's abdomen hanging from above your " + player.butt.shortDescription() + "!</b>";
			//}

		}
		private static string SpiderRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BeeShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("bee abdomen", alternateFormat);
		}
		private static string BeeLongDesc(TailData tail, bool alternateFormat)
		{
			//maybe include venom count indication, idk.
			return (alternateFormat ? "a " : "") + "bee abdomen";
		}
		private static string BeePlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//StringBuilder sb = new StringBuilder("A large insectile bee-abdomen dangles from just above your backside, bobbing with its own weight as you shift. "
			//+ "It is covered in hard chitin with black and yellow stripes, and tipped with a dagger-like stinger.");
			//if (player.tail.venom > 50 && player.tail.venom < 80)
			//	Console.WriteLine("  A single drop of poison hangs from your exposed stinger.");
			//if (player.tail.venom >= 80 && player.tail.venom < 100)
			//	Console.WriteLine("  Poisonous bee venom coats your stinger completely.");
			//if (player.tail.venom == 100)
			//	Console.WriteLine("  Venom drips from your poisoned stinger regularly.");
			//return sb.ToString();
		}
		private static string BeeTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.type == NONE)
			//{
			//	return "Painful swelling just above your " + player.butt.shortDescription() + " doubles you over. It gets worse and worse as the swollen lump"
			//		+ "begins to protrude from your backside, swelling and rounding with a series of pops until you have a bulbous abdomen hanging just above your butt. "
			//		+ "The whole thing is covered in a hard chitinous material, and large enough to be impossible to hide. "
			//		+ "You sigh as your stinger slides into place with a 'snick', finishing the transformation.  <b>You have a bee's abdomen.</b>";
			//}
			//else
			//{
			//	return "Painful swelling just above your " + player.butt.shortDescription() + " doubles you over, and you hear the sound of your tail dropping off onto the ground! "
			//		+ "Before you can consider the implications, the pain gets worse, and you feel your backside bulge outward sickeningly, cracking and popping as a rounded bee-like abdomen"
			//		+ " grows in place of your old tail. It grows large enough to be impossible to hide, and with a note of finality, your stinger slides free with an audible 'snick'.";
			//}
		}
		private static string BeeRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SharkShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("shark tail", alternateFormat);
		}
		private static string SharkLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "rough shark tail";
		}

		private static string SharkPlayerStr(Tail tail, PlayerBase player)
		{
			return "A long shark-tail trails down from your backside, swaying to and fro while giving you a dangerous air.";

		}
		private static string SharkTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.type == NONE)
			//{
			//	string shiftText = "self ";
			//	if (player.hasArmor())
			//	{
			//		shiftText = " " + player.armor.name();
			//	}
			//	else if (player.hasUndergarments())
			//	{
			//		shiftText = " " + player.undergarment.name;
			//	}
			//	return "Jets of pain shoot down your spine, causing you to gasp in surprise and fall to your hands and knees. "
			//		+ "Feeling a bulging at the end of your back, you quickly shift your" + shiftText + "to comfortably accomodate whatever is forming there, and "
			//		+ "you finish just in time for a fully formed shark tail to burst through. You swish it around a few times, surprised by how flexible it is. "
			//		+ "You now have a <b>brand new shark tail.</b>";
			//}
			//else
			//{
			//	return "Jets of pain shoot down your spine into your tail. You feel the tail bulging out until it explodes into a large and flexible shark-tail. "
			//		+ "You swish it about experimentally, and find it quite easy to control.";
			//}
		}
		private static string SharkRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("cat tail", alternateFormat);
		}
		private static string CatLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "feline tail";
		}

		private static string CatPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A soft " + tail.epidermis.justColor() + " cat-tail sprouts just above your " +
			//player.butt.shortDescription() + ", curling and twisting with every step to maintain perfect balance.";

		}
		private static string CatTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.type == NONE)
			//{
			//	int temp = Utils.Rand(3);
			//	if (temp == 0)
			//	{
			//		StringBuilder sb = new StringBuilder("A pressure builds in your backside. ");
			//		sb.Append(player.hasArmor() ? "You feel under your " + player.armorName : "the small of your back");
			//		sb.Append("and discover an odd bump that seems to be growing larger by the moment. In seconds it passes between your fingers and ");
			//		if (player.hasArmor())
			//		{
			//			sb.Append("bursts out the back of your clothes and ");
			//		}
			//		sb.Append("grows most of the way to the ground. A thick coat of fur springs up to cover your new tail. "
			//			+ "You instinctively keep adjusting it to improve your balance. <b>You now have a cat-tail.</b>");
			//		return sb.ToString();
			//	}
			//	else if (temp == 1)
			//	{
			//		return "You feel your backside shift and change, flesh molding and displacing into a long, flexible tail! <b>You now have a cat tail.</b>";
			//	}
			//	else
			//	{
			//		return "You feel an odd tingling in your spine and your tail bone starts to throb and then swell. Within a few moments it begins to grow, "
			//			+ "adding new bones to your spine. Before you know it, you have a tail. Just before you think it's over, the tail begins to sprout soft, glossy fur. "
			//			+ "<b>You now have a cat tail.</b>";
			//	}
			//}
			//else
			//{
			//	return "You pause and tilt your head... something feels different. Ah, that's what it is; you turn around and look down at your tail as it starts to change shape, " +
			//		"narrowing and sprouting glossy fur. <b>You now have a cat tail.</b>";
			//}
		}
		private static string CatRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("lizard tail", alternateFormat);
		}
		private static string LizardLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "reptilian tail";
		}

		private static string LizardPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (tail.epidermis.epidermisType != tail.secondaryEpidermis.epidermisType
			//	|| (tail.epidermis.usesFur && tail.epidermis.fur != tail.secondaryEpidermis.fur)
			//	|| (tail.epidermis.usesTone && tail.epidermis.tone != tail.secondaryEpidermis.tone))
			//{
			//	return "A tapered tail, covered in " + tail.epidermis.FullDescription() + " with " + tail.secondaryEpidermis.FullDescription() + " along its underside hangs down from just"
			//			 + " above your " + player.butt.shortDescription() + ". It sways back and forth, assisting you with keeping your balance.";
			//}
			//else
			//{
			//	return "A tapered tail hangs down from just above your " + player.butt.shortDescription() + ". It sways back and forth, assisting you with keeping your balance.";
			//}
		}
		private static string LizardTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LizardRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RabbitShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("rabbit tail", alternateFormat);
		}
		private static string RabbitLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "puffy rabbit tail";
		}

		private static string RabbitPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A short, soft bunny tail sprouts just above your " + player.butt.shortDescription()
			//+ ", twitching constantly whenever you don't think about it.";
		}
		private static string RabbitTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RabbitRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("harpy tail", alternateFormat);
		}
		private static string HarpyLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "feathery harpy tail";
		}

		private static string HarpyPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A tail of feathers fans out from just above your " + player.butt.shortDescription()
			//+ ", twitching instinctively to help guide you if you were to take flight.";
		}
		private static string HarpyTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string HarpyRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("kangaroo tail", alternateFormat);
		}
		private static string KangarooLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "marsupial tail";
		}

		private static string KangarooPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A conical, furry, " + tail.epidermis.justColor() + " tail extends from your " + player.butt.shortDescription()
			//+ ", bouncing up and down as you move and helping to counterbalance you.";
		}
		private static string KangarooTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string KangarooRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="treatAsPlural">Flag altering the behavior of the description. if null, use default. if true, treat as plural if applicable. if false, treat as singular.</param>
		/// <returns></returns>
		protected static string FoxShortDesc(bool treatAsPlural, out bool isPlural)
		{
			isPlural = treatAsPlural;
			if (treatAsPlural) return "kitsune tails";
			else return "vulpine tail";
		}

		protected static string FoxSingleDesc()
		{
			return "a fox-like tail";
		}

		protected static string FoxLongDesc(TailData tail, bool alternateFormat, bool pluralIfApplicable, out bool isPlural)
		{
			isPlural = tail.tailCount != 1;
			string intro = alternateFormat ? "a " : "";
			byte count = tail.tailCount;
			if (!pluralIfApplicable && count > 1)
			{
				return intro + "kitsune tail";
			}
			if (count > 5)
			{
				return intro + "bundle of kitsune tails";
			}
			else if (count > 4)
			{
				return intro + "quintet of kitsune tails";
			}
			else if (count > 3)
			{
				return intro + "quartet of kitsune tails";
			}
			else if (count > 2)
			{
				return intro + "trio of kitsune tails";
			}
			else if (count > 1)
			{
				return intro + "pair of kitsune tails";
			}
			else //if (count == 1)
			{
				return intro + "fox tail";
			}
		}

		protected static string FoxPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//if (player.tail.tailCount < 2) return "A swishing " + tail.epidermis.justColor() + " fox's brush extends from your "
			//		+ player.butt.shortDescription() + ", curling around your body - the soft fur feels lovely.";
			//else return GlobalStrings.NumberAsText(tail.tailCount) + " swishing " + tail.epidermis.justColor() + " fox's tails extend from your "
			//		+ player.butt.shortDescription() + ", curling around your body - the soft fur feels lovely.";
		}
		protected static string FoxTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string FoxRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string DragonShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("draconic tail", alternateFormat);
		}
		private static string DragonLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "fierce draconic tail";
		}

		private static string DragonPlayerStr(Tail tail, PlayerBase player)
		{
			if (tail.epidermis.type != tail.secondaryEpidermis.type
				|| (tail.epidermis.usesFur && !tail.epidermis.fur.Equals(tail.secondaryEpidermis.fur))
				|| (tail.epidermis.usesTone && tail.epidermis.tone != tail.secondaryEpidermis.tone))
			{
				return "A thick, muscular, reptilian tail covered in " + tail.epidermis.LongDescription() + " with "
					+ tail.secondaryEpidermis.LongDescription() + " along its underside, almost as long as you are tall,"
					+ " swishes slowly from side to side behind you. Its tip menaces with sharp spikes of bone, "
					+ "and could easily cause serious harm with a good sweep.";
			}
			else
			{
				return "A thick, muscular, reptilian tail, almost as long as you are tall, unconsciously swings behind you slowly"
						 + " from side to side. Its tip menaces with sharp spikes of bone, and could easily cause grievous harm"
						 + " with a single, powerful sweep.";
			}
		}
		private static string DragonTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DragonRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("raccoon tail", alternateFormat);
		}
		private static string RaccoonLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "raccoon tail";
		}

		private static string RaccoonPlayerStr(Tail tail, PlayerBase player)
		{
			//may want to check for black.
			return "A black-and-" + tail.epidermis.JustColor() + "-ringed raccoon tail waves behind you.";
		}
		private static string RaccoonTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RaccoonRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("mouse tail", alternateFormat);
		}
		private static string MouseLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "bushy mouse tail";
		}

		private static string MousePlayerStr(Tail tail, PlayerBase player)
		{
			return "A naked, " + tail.epidermis.JustColor() + " mouse tail pokes from your butt, dragging on the ground and twitching occasionally.";
		}
		private static string MouseTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string MouseRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("ferret tail", alternateFormat);
		}
		private static string FerretLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "ferret tail";
		}

		private static string FerretPlayerStr(Tail tail, PlayerBase player)
		{
			return "Sprouting from your backside, you have a long, bushy tail. It’s covered in a fluffy layer of " + tail.epidermis.DescriptionWithColor()
				+ " It twitches and moves happily with your body when you are excited.";
		}
		private static string FerretTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BehemothShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("behemoth tail", alternateFormat);
		}
		private static string BehemothLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "behemoth tail";
		}

		private static string BehemothPlayerStr(Tail tail, PlayerBase player)
		{
			return "A long seemingly-tapering tail pokes from your butt, ending in spikes just like behemoth's.";
		}
		private static string BehemothTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BehemothRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("pig tail", alternateFormat);
		}
		private static string PigLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "curly pig tail";
		}

		private static string PigPlayerStr(Tail tail, PlayerBase player)
		{
			return "A short, curly pig tail sprouts from just above your butt.";
		}
		private static string PigTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string PigRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScorpionShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("scorpion tail", alternateFormat);
		}
		private static string ScorpionLongDesc(TailData tail, bool alternateFormat)
		{
			//add venom count flavor text.
			return (alternateFormat ? "a " : "") + "scorpion tail";
		}

		private static string ScorpionPlayerStr(Tail tail, PlayerBase player)
		{
			return "A chitinous scorpion tail sprouts from just above your butt, ready to dispense venom.";
		}
		private static string ScorpionTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScorpionRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GoatShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("goat tail", alternateFormat);
		}
		private static string GoatLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "goat tail";
		}

		private static string GoatPlayerStr(Tail tail, PlayerBase player)
		{
			return "A very short, stubby goat tail sprouts from just above your butt.";
		}
		private static string GoatTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GoatRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("rhino tail", alternateFormat);
		}
		private static string RhinoLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "rhino tail";
		}

		private static string RhinoPlayerStr(Tail tail, PlayerBase player)
		{
			return "A ropey rhino tail sprouts from just above your butt, swishing from time to time.";
		}
		private static string RhinoTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RhinoRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("echidna tail", alternateFormat);
		}
		private static string EchidnaLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "echidna tail";
		}

		private static string EchidnaPlayerStr(Tail tail, PlayerBase player)
		{
			//return "A stumpy echidna tail forms just about your " + player.butt.shortDescription() + ".";
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string EchidnaRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("deer tail", alternateFormat);
		}
		private static string DeerLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "short deer tail";
		}

		private static string DeerPlayerStr(Tail tail, PlayerBase player)
		{
			return "A very short, stubby deer tail sprouts from just above your butt.";
		}
		private static string DeerTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DeerRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("salamander tail", alternateFormat);
		}
		private static string SalamanderLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "flame-tipped salamander tail";
		}

		private static string SalamanderPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A tapered tail, covered in " + tail.epidermis.descriptionWithColor()
			//	+ ", hangs down from just above your " + player.butt.shortDescription() + ". It sways back and forth, assisting you with keeping your balance. " +
			//	"When you are in battle or when you want could set ablaze whole tail in red-hot fire.";
		}
		private static string SalamanderTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SalamanderRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("wolf tail", alternateFormat);
		}
		private static string WolfLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "wolf tail";
		}

		private static string WolfPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A thick-furred wolf tail hangs above your " + player.butt.shortDescription() + ".";
		}
		private static string WolfTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WolfRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("sheep tail", alternateFormat);
		}
		private static string SheepLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "short, fluffy sheep tail";
		}

		private static string SheepPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A fluffy sheep tail hangs down from your " + player.butt.shortDescription() +
			//". It occasionally twitches and shakes, its puffy fluff begging to be touched.";
		}
		private static string SheepTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SheepRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("imp tail", alternateFormat);
		}
		private static string ImpLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "an " : "") + "imp tail";
		}

		private static string ImpPlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();

			//return "A thin imp tail almost as long as you are tall hangs from above your "
			//+ player.butt.shortDescription() + ", dotted at the end with a small puff of hair.";
		}
		private static string ImpTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ImpRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("cockatrice tail", alternateFormat);
		}
		private static string CockatriceLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "cockatrice tail";
		}

		private static string CockatricePlayerStr(Tail tail, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A thick, scaly, prehensile reptilian tail hangs from your "
			//+ player.butt.shortDescription() + ", about half as long as you are tall.";
		}
		private static string CockatriceTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CockatriceRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaShortDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("red-panda tail", alternateFormat);
		}
		private static string RedPandaLongDesc(TailData tail, bool alternateFormat)
		{
			return (alternateFormat ? "a " : "") + "red-panda tail";
		}

		private static string RedPandaPlayerStr(Tail tail, PlayerBase player)
		{
			return "Sprouting from your backside, you have a long, bushy tail. It has a beautiful pattern of rings in "
				+ tail.epidermis.JustColor() + " and " + tail.secondaryEpidermis.JustColor() + "fluffy fur. "
				+ "It waves playfully as you walk giving to your step a mesmerizing touch.";
		}
		private static string RedPandaTransformStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(TailData previousTailData, PlayerBase player, bool describeOvipositorChangeIfApplicable = true)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
	}
}
