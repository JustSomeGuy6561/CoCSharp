//ArmStrings.cs
//Description: Implements the strings for the arm and armtype. separation of concerns.
//Author: JustSomeGuy
//1/18/2019, 9:30 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class Arms
	{
		public static string Name()
		{
			return "Arms";
		}
	}

	public partial class ArmType
	{
		private static string HumanDescStr()
		{
			return "human arms";
		}
		private static string HumanLongDesc(Arms arm)
		{
			return HumanDescStr();
		}
		private static string HumanPlayerStr(Arms arm, PlayerBase player)
		{
			return "";
		}
		private static string HumanTransformStr(Arms oldArms, PlayerBase player)
		{
			return oldArms.type.restoreString(oldArms, player);
		}
		private static string HumanRestoreStr(Arms currentArms, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(currentArms, player);
		}
		private static string HarpyDescStr()
		{
			return "feathered arms";
		}
		private static string HarpyLongDesc(Arms arm)
		{
			return arm.epidermis.JustColor() + " feathered arms";
		}
		private static string HarpyPlayerStr(Arms arm, PlayerBase player)
		{
			return "Feathers hang off your arms from shoulder to wrist, giving them a slightly wing-like look.";
		}
		private static string HarpyTransformStr(Arms oldArms, PlayerBase player)
		{
			StringBuilder retVal = new StringBuilder("You look on in horror while");
			if (oldArms.epidermis.usesFur)
			{
				retVal.Append("your fur falls off your arms, and avian plumage grows in its place.");
			}
			else
			{
				retVal.Append("avian plumage sprouts from your " + oldArms.epidermis.ShortDescription() + ", covering your forearms "
					+ "until <b>your arms look vaguely like wings.</b>");
			}
			if (oldArms.hands.type != HandType.HUMAN)
			{
				retVal.Append("What's more, your hands revert to a more human appearance! ");
			}
			else
			{
				retVal.Append("Your hands remain unchanged, thankfully. It'd be impossible to be a champion without hands! ");
			}
			retVal.Append("The feathery limbs might help you maneuver if you were to fly, but there's no way they'd support you alone.");
			return retVal.ToString();
		}
		private static string HarpyRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your feathery arms are shedding their feathery coating."
				+ "\nThe wing-like shape your arms once had is gone in a matter of moments, leaving human skin behind.";
		}
		private static string SpiderDescStr()
		{
			return "spider arms";
		}
		private static string SpiderLongDesc(Arms arm)
		{
			return "chitinous " + arm.epidermis.JustColor() + " spider arms";
		}
		private static string SpiderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves from a distance.";
		}
		private static string SpiderTransformStr(Arms oldArms, PlayerBase player)
		{
			StringBuilder retVal = new StringBuilder();
			if (oldArms.type == ArmType.HARPY)
			{
				retVal.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
			}
			retVal.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments");
			if (oldArms.type != HUMAN && oldArms.type != HARPY)
			{
				retVal.Append(", <b>turning the " + oldArms.epidermis.ShortDescription() + " into a shiny black carapace</b>. ");
			}
			else
			{
				retVal.Append("unti it becomes a shiny black carapace. ");
			}
			retVal.Append("You touch the onyx exoskeleton and discover to your delight that you can still feel through it as naturally as your own skin.");
			return retVal.ToString();
		}
		private static string SpiderRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your arms' chitinous covering is flaking away."
				+ "\nThe glossy black coating is soon gone, leaving human skin behind.";
		}
		private static string BeeDescStr()
		{
			return "fuzzy bee arms";
		}
		private static string BeeLongDesc(Arms arm)
		{
			return BeeDescStr();
		}
		private static string BeePlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves ended with a yellow fuzz from a distance.";
		}
		private static string BeeTransformStr(Arms oldArms, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("\n\n");
			if (oldArms.type == ArmType.SPIDER)
			{
				sb.Append("The " + oldArms.epidermis.ShortDescription() + "on your upper arms slowly starting to grown yellow fuzz, making them looks more like those of bee.");
			}
			else
			{
				//(Bird pretext)
				if (oldArms.type == ArmType.HARPY)
				{
					sb.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
				}
				sb.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments");
				if (oldArms.type == HARPY || oldArms.type == HUMAN)
				{
					sb.Append("<b>until it resembles a shiny black exoskeleton</b>");
				}
				else
				{
					sb.Append(", <b>turning the " + oldArms.epidermis.ShortDescription() + " into a shiny black carapace</b>. ");
				}
				sb.Append("A moment later the pain fades and you are able to turn your gaze down to your beautiful new arms, covered in " +
					"shining black chitin from the upper arm down, and downy yellow fuzz along your upper arm.");
			}
			return sb.ToString();
		}
		private static string BeeRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "\n\nYou arms start to itch like crazy, and no matter how much you scrath, you can't shake the itch. Looking down, you see the cause - the exoskeleton covering"
				+ "your arm is flaking away. Not to be outdone, the yellow fur towards the end starts to fall out as well."
				+ "<b> You now have human arms!</b>";
		}
		private static string DragonDescStr()
		{
			return "draconic arms";
		}
		private static string DragonLongDesc(Arms arm)
		{
			return DragonDescStr();
		}
		private static string DragonPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string DragonTransformStr(Arms oldArms, PlayerBase player)
		{
			if (oldArms.type.isPredatorArms())
			{
				return "\n\nYour " + oldArms.hands.ShortDescription() + " change a little to become more dragon-like." +
					" <b>Your arms and claws are like those of a dragon.</b>";
			}
			return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. " +
				"After a longer moment of ignoring it you finally glance down in irritation, only to discover that your arms former " +
				"appearance has changed into those of some reptilian killer with shield-shaped " + player.body.mainEpidermis.tone +
				" scales and powerful, thick, curved steel-gray claws replacing your fingernails.\n<b>You now have dragon arms.</b>";
		}
		private static string DragonRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		private static string ImpDescStr()
		{
			return "predator arms";
		}
		private static string ImpLongDesc(Arms arm)
		{
			return arm.epidermis.JustColor() + " predator arms ending in imp claws";
		}
		private static string ImpPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string ImpTransformStr(Arms oldArms, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (oldArms.type != HUMAN)
			{
				sb.Append("\n\nYour arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning."
					+ "The skin on your arms visibly thicken, then segment into a hybrid between scales and skin.");
			}
			if (!oldArms.hands.type.isClaws)
			{
				sb.Append("\n\nYour " + oldArms.hands.ShortDescription() + " suddenly ache in pain, and all you can do is curl " +
					"them up to you. Against your body, you feel them form into three long claws, with a smaller one replacing your thumb but " +
					"just as versatile. <b>You have imp claws!</b>");
			}
			else
			{ //has claws
				sb.Append("\n\nYour claws suddenly begin to shift and change, starting to turn back into normal hands. But just before they do, they" +
					" stretch out into three long claws, with a smaller one coming to form a pointed thumb. <b>You have imp claws!</b>");
			}
			return sb.ToString();
		}
		private static string ImpRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		private static string LizardDescStr()
		{
			return "predator arms";
		}
		private static string LizardLongDesc(Arms arm)
		{
			return "predator arms with " + arm.hands.FullDescription();

		}
		private static string LizardPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string LizardTransformStr(Arms oldArms, PlayerBase player)
		{
			if (oldArms.type.isPredatorArms())
			{
				return "\n\nYour " + oldArms.hands.ShortDescription() + " change a little to become more lizard-like." +
					" <b>You now have lizard-like claws.</b>";
			}
			else return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer"
				+ " moment of ignoring it you finally glance down in irritation, only to discover that your arms' former appearance has changed into "
				+ "those of some reptilian killer, complete with scales and claws in place of fingernails. Strangely, your claws seem to match the "
				+ "tone of your arms.\n<b>You now have reptilian arms.</b>";
		}
		private static string LizardRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		private static string SalamanderDescStr()
		{
			return "salamader arms";
		}
		private static string SalamanderLongDesc(Arms arm)
		{
			return "salamander arms with " + arm.hands.FullDescription();
		}
		private static string SalamanderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + "cover your arms from the biceps down, and your fingernails are now " + arm.hands.FullDescription();
		}
		private static string SalamanderTransformStr(Arms oldArms, PlayerBase player)
		{
			return "You scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer moment of"
				+ " ignoring it you finally glance down in irritation, only to discover that your arms former appearance has changed into those of a salamander,"
				+ " complete with leathery, red scales and short, fiery-red claws replacing your fingernails.  <b>You now have salamander arms.</b>";
		}
		private static string SalamanderRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your once scaly arms are shedding their scales and that"
				+ " your claws become normal human fingernails again. <b>You have normal human arms again.</b>";

		}
		private static string WolfDescStr()
		{
			return "wolfen arms";
		}
		private static string WolfLongDesc(Arms arm)
		{
			return "wolf-like arms and " + arm.hands.ShortDescription();
		}
		private static string WolfPlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are shaped like a wolf's, overly muscular at your shoulders and biceps before quickly slimming down."
				+ " They're covered in " + arm.epidermis.LongDescription() + " and end in paws with just enough flexibility to be used as hands."
				+ " They're rather difficult to move in directions besides back and forth.";
		}
		//based off dog. it was never implemented in game, even though there was text for the PC having them. 
		private static string WolfTransformStr(Arms oldArms, PlayerBase player)
		{
			if (oldArms.type == ArmType.DOG)
			{
				return "Your arms feel stiff, and despite any attempt to move them, they just sit there, limply. You soon realize the bones in your " + oldArms.hands.ShortDescription() +
					" are changing, as well as the muscles on your arms. Strangely, your arms don't seem to change much, though they feel much stronger than before. " +
					"Stretching a little, you realize your paws are also much less dextrous than before, though your claws are much sharper. <b> You now have wolf-like arms!</b>";
			}
			else
			{
				return GenericPawTransformStr(oldArms, "wolf", false, true);
			}
		}
		private static string WolfRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CockatriceDescStr()
		{
			return "feathery arms";
		}
		private static string CockatriceLongDesc(Arms arm)
		{
			return arm.epidermis.fur.AsString() + ", feathered cockatrice arms with " + arm.hands.FullDescription();
		}
		private static string CockatricePlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are covered in " + arm.epidermis.LongDescription()
		  + " from the shoulder down to the elbow where they stop in a fluffy cuff. A handful of long feathers grow from your"
		  + " elbow in the form of vestigial wings, and while they may not let you fly, they certainly help you jump. Your lower"
		  + " arm is coated in " + arm.secondaryEpidermis.LongDescription() + " and your fingertips terminate in " + arm.hands.ShortDescription() + ".";
		}
		private static string CockatriceTransformStr(Arms oldArms, PlayerBase player)
		{
			return "Prickling discomfort suddenly erupts all over your body, like every last inch of your skin has suddenly developed"
			+ " pins and needles. You try to rub feeling back into your body, but only succeed in shifting the feeling to your arms."
			+ " Your arms feel like they are " + (oldArms.usesFur ? "shedding" : "molting") + ". Sure enough, the old"
			+ " outer layer falls off, replaced with leathery scales. Additionally, the upper part of your arms gain feathers, covering them"
			+ " from shoulder to elbow, where they end in a fluffy cuff. As suddenly as the itching came it fades, leaving you to marvel"
			+ " over your new arms.\n<b>You now have cockatrice arms!</b>";
		}
		private static string CockatriceRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch. "
				+ "You quickly notice your arms are shedding their scales, and while this is normal, you've never seen it to this extent. "
				+ "Weirder still, underneath it is not new scales, but rather skin. Your upper arms are also experiencing the change, as the feathers "
				+ "that once covered them are falling out. You soon realize <b>You have normal arms again!</b>.";
		}
		private static string RedPandaDescStr()
		{
			return "panda-arms";
		}
		private static string RedPandaLongDesc(Arms arm)
		{
			return arm.epidermis.LongAdjectiveDescription() + "panda-arms";
		}
		private static string RedPandaPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.furTexture.ToString() + ", " + arm.epidermis.JustColor() + " fluff cover your arms. Your paws have " + arm.hands.ShortDescription() + ".";
		}
		private static string RedPandaTransformStr(Arms oldArms, PlayerBase player)
		{
			return GenericPawTransformStr(oldArms, "red-panda", true, false, new FurColor(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.NO_PATTERN));
		}
		private static string RedPandaRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FerretDescStr()
		{
			return "ferret-arms";
		}
		private static string FerretLongDesc(Arms arm)
		{
			return arm.epidermis.LongAdjectiveDescription() + " ferret arms with " + arm.hands.FullDescription();
		}
		private static string FerretPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.JustColor() + " fluff covers your arms, turning into "
				+ arm.secondaryEpidermis.LongDescription() + " from elbows to paws."
				+ " The latter have " + arm.hands.FullDescription();
		}
		private static string FerretTransformStr(Arms oldArms, PlayerBase player)
		{
			return GenericPawTransformStr(oldArms, "ferret", true, false);

		}
		private static string FerretRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CatDescStr()
		{
			return "feline arms";
		}
		private static string CatLongDesc(Arms arm)
		{
			return arm.epidermis.LongAdjectiveDescription() + " feline arms with " + arm.hands.FullDescription();
		}
		private static string CatPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string CatTransformStr(Arms oldArms, PlayerBase player)
		{
			return GenericPawTransformStr(oldArms, "cat", true, true);
		}
		private static string CatRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string DogDescStr()
		{
			return "canine arms";
		}
		private static string DogLongDesc(Arms arm)
		{
			return arm.epidermis.LongAdjectiveDescription() + " canine arms with " + arm.hands.FullDescription();
		}
		private static string DogPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.JustColor() + " fluff covers your arms. Your paw-like hands have " + arm.hands.FullDescription()
				+ ". With the right legs (and the right motivation), you could run with them, much like the hellounds you see in the mountains.";
		}
		private static string DogTransformStr(Arms oldArms, PlayerBase player)
		{
			if (oldArms.type == ArmType.WOLF)
			{
				return "Your arms feel stiff, and despite any attempt to move them, they just sit there, limply. You soon realize the bones in your " + oldArms.hands.ShortDescription() +
					" are changing, as well as the muscles on your arms. Strangely, your arms don't seem to change much, though they feel a bit weaker than before. " +
					"Stretching a little, you realize your paws are also much more dextrous than before, though your claws are much duller. <b> You now have dog-like arms!</b>";
			}
			else
			{
				return GenericPawTransformStr(oldArms, "dog", true, false);
			}
		}
		private static string DogRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FoxDescStr()
		{
			return "fox-like arms";
		}
		private static string FoxLongDesc(Arms arm)
		{
			return arm.epidermis.LongAdjectiveDescription() + " , fox-like arms with " + arm.hands.FullDescription();
		}
		private static string FoxPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string FoxTransformStr(Arms oldArms, PlayerBase player)
		{
			return GenericPawTransformStr(oldArms, "fox", true, true);
		}
		private static string FoxRestoreStr(Arms currentArms, PlayerBase player)
		{
			return PawRestoreString();
		}

		private static string GooDesc()
		{
			return "gooey arms";
		}
		private static string GooLongDesc(Arms arms)
		{
			return arms.epidermis.LongAdjectiveDescription() + " goo arms with " + arms.hands.FullDescription();
		}
		private static string GooPlayerStr(Arms arm, PlayerBase player)
		{
			bool gooBody = player.body.type == BodyType.GOO;

			string introText = gooBody ? "Much like the rest of your body, your " : "Your ";

			return introText + "arms are made almost entirely of goo. With some effort, you can roughly control the shape and size of your arms, allowing you to access small areas with ease. " +
				"What you've gained in flexibility, however, is lost in overall rigidity, making it harder to lift or carry things. You also don't really have hands, par se, but you can " +
				"force your gooey appendages into something that roughly resembles a mitten to hold onto things.";
		}
		private static string GooTransformStr(Arms oldArms, PlayerBase player)
		{
			return "Your arms feel strange, causing you to rub them absentmindedly. All at once, the feeling stops. For a moment, you're glad the feeling has passed... until you realize that " +
				"you're STILL rubbing your arms, and you can't feel it! Even more frightening, your " + oldArms.hands.ShortDescription() + " seem to be melting into your arms. " +
				"This same effect is happening all along your arms, as everything softens into a gelatin-like consistency. Your arms become transparent, and you notice your bones are dissolving " +
				"as well. With ever-increasing levels of horror, you realize " + SafelyFormattedString.FormattedText("your arms are turning into goo!", StringFormats.BOLD) +
				" You scratch your head, wondering how you'll use your arms now that they're in this state. Seeing as you still have arms, they must solid enough. Wait - " +
				"you just scratched your head? Curiousity replaces any lingering horror as you experiement with your new gooey extremities. With conscious effort, you can control the rough shape " +
				"they take, though anything too small or sturdy like fingers tends to fall apart. The best you can manage is a crude mitten-like shape, and you suppose that'll have to do. On the " +
				"plus side, you can fit your arm into much smaller spaces now.";
		}
		private static string GooRestoreStr(Arms currentArms, PlayerBase player)
		{
			return "The goo forming your arms feels strange - more solid, perhaps. Slowly, each arm becomes less and less transparent, until you can't see through " +
				"it all. You go to tap it with your gooey, roughly hand-shaped apendages, and stop short when you notice these appendages are also undergoing transformations. " +
				"The mitten-like shape you use to approximate hands splits, forming digits far too solid and slender for just goo. Soon, they too became less transparent. Knuckles form, " +
				"and a strange sensation covers your arms. Curiously, you pinch one of your arms, cursing silently - it hurts! With a start, you realize " +
				SafelyFormattedString.FormattedText("You have human arms and hands again!", StringFormats.BOLD);

		}


		private static string CatFoxPlayerStr(Arms arms, PlayerBase player)
		{
			return "Soft, " + arms.epidermis.JustColor() + " fluff covers your arms. Your paw-like hands have " + arms.hands.FullDescription() + ".";
		}

		private static string PredatorPlayerStr(Arms arms, PlayerBase player)
		{
			return "Your arms are covered by " + arms.epidermis.ShortDescription() + " and your hands are noew " + arms.hands.FullDescription() + ".";
		}

		private static string PredatorRestoreStr(Arms arms, PlayerBase player)
		{
			return "\n\nYou feel a sudden tingle in your " + arms.hands.ShortDescription() + " and then you realize,"
				+ " that they have become normal human fingernails again. Your arms quickly follow suit. "
				+ "<b>You have normal human arms again.</b>";
		}

		private static string GenericPawTransformStr(Arms oldArms, string newArmString, bool canClimb, bool canTearFlesh, FurColor optionalFurColor = null)
		{
			bool armChanged = false;
			StringBuilder sb = new StringBuilder("Your arms feel stiff, and despite any attempt to move them, they just sit there, limply." +
				" You soon realize the bones in your " + oldArms.hands.ShortDescription() + " are changing, as well as the muscles on your arms.");
			if (!oldArms.epidermis.usesFur && !oldArms.secondaryEpidermis.usesFur)
			{
				armChanged = true;
				sb.Append(" A thick layer of fur quickly grows in, covering your arm from to the tips of your fingers.");
			}
			if (!oldArms.hands.type.isPaws)
			{
				sb.Append(armChanged ? " Not to be outdone, your " : " Your ");
				if (oldArms.hands.type.isHands)
				{
					sb.Append("hands gain pink, padded paws where your palms were once, and your nails become decent-sized curved claws.");
				}
				else if (oldArms.hands.type.isClaws)
				{
					sb.Append("claws shorten and small pads form under them, until they resemble paws.");
				}
				else //if other.
				{
					sb.Append("appendages shift towards human hands, but instead of nails, you get short claws. Padding appears in your palms and under your claws.");
				}
			}

			if (canClimb)
			{
				if (canTearFlesh)
				{
					sb.Append(" Your claws, while not the sharpest or longest, they are certainly sharp enough to tear flesh and nimble enough to make climbing and exploring easier.");
				}
				else
				{
					sb.Append(" Your claws, while not sharp enough to tear flesh, are nimble enough to make climbing and exploring easier.");
				}
			}
			else if (canTearFlesh)
			{
				sb.Append(" Your claws aren't overly large, but they are still sharp and dangerous, easily able to tear flesh if you wanted. ");
			}
			else
			{
				sb.Append("Your claws don't really provide much benefit, unfortunately. ");
			}
			sb.Append(" " + SafelyFormattedString.FormattedText("Your arms have become like those of a " + newArmString + "!", StringFormats.BOLD));
			return sb.ToString();
		}


		//something like this is fine, genrally. i'm lazy, so this is what i'm using. if you want you can provide specific examples or copy and modify this, w/e.
		private static string PawRestoreString()
		{
			return "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your arms are shedding their furry coating."
				+ "\nYour hands follow suit, losing their pads and claws until your have a normal hand and fingernails."
				+ "<b>You have normal human arms again.</b>";
		}


		private static string GenericRestoreString()
		{
			return "\n\nYour unusual arms change more and more until they are normal human arms, leaving only skin behind." +
				"<b>You have normal human arms again.</b>";
		}



		//"<b>You have normal human arms again.</b>"
	}
}