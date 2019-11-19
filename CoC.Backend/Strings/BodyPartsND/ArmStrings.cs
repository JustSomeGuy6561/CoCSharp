//ArmStrings.cs
//Description: Implements the strings for the arm and armtype. separation of concerns.
//Author: JustSomeGuy
//1/18/2019, 9:30 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
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
		private static string HumanFullDesc(Arms arm)
		{
			return HumanDescStr();
		}
		private static string HumanPlayerStr(Arms arm, PlayerBase player)
		{
			return "";
		}
		private static string HumanTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return previousArmData.type.restoredString(previousArmData, player);
		}
		private static string HumanRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousArmData, player);
		}
		private static string HarpyDescStr()
		{
			return "feathered arms";
		}
		private static string HarpyFullDesc(Arms arm)
		{
			return arm.epidermis.justColor() + " feathered arms";
		}
		private static string HarpyPlayerStr(Arms arm, PlayerBase player)
		{
			return "Feathers hang off your arms from shoulder to wrist, giving them a slightly wing-like look.";
		}
		private static string HarpyTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder retVal = new StringBuilder("You look on in horror while");
			if (previousArmData.primaryEpidermis.usesFur)
			{
				retVal.Append("your fur falls off your arms, and avian plumage grows in its place.");
			}
			else
			{
				retVal.Append("avian plumage sprouts from your " + previousArmData.primaryEpidermis.shortDescription() + ", covering your forearms "
					+ "until <b>your arms look vaguely like wings.</b>");
			}
			if (previousArmData.handData.type != HandType.HUMAN)
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
		private static string HarpyRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your feathery arms are shedding their feathery coating."
				+ "\nThe wing-like shape your arms once had is gone in a matter of moments, leaving human skin behind.";
		}
		private static string SpiderDescStr()
		{
			return "spider arms";
		}
		private static string SpiderFullDesc(Arms arm)
		{
			return "chitinous " + arm.epidermis.justColor() + " spider arms";
		}
		private static string SpiderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.fullDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves from a distance.";
		}
		private static string SpiderTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder retVal = new StringBuilder();
			if (previousArmData.type == ArmType.HARPY)
			{
				retVal.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
			}
			retVal.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments");
			if (previousArmData.type != HUMAN && previousArmData.type != HARPY)
			{
				retVal.Append(", <b>turning the " + previousArmData.primaryEpidermis.shortDescription() + " into a shiny black carapace</b>. ");
			}
			else
			{
				retVal.Append("unti it becomes a shiny black carapace. ");
			}
			retVal.Append("You touch the onyx exoskeleton and discover to your delight that you can still feel through it as naturally as your own skin.");
			return retVal.ToString();
		}
		private static string SpiderRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your arms' chitinous covering is flaking away."
				+ "\nThe glossy black coating is soon gone, leaving human skin behind.";
		}
		private static string BeeDescStr()
		{
			return "fuzzy bee arms";
		}
		private static string BeeFullDesc(Arms arm)
		{
			return BeeDescStr();
		}
		private static string BeePlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.fullDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves ended with a yellow fuzz from a distance.";
		}
		private static string BeeTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("\n\n");
			if (previousArmData.type == ArmType.SPIDER)
			{
				sb.Append("The " + previousArmData.primaryEpidermis.shortDescription() + "on your upper arms slowly starting to grown yellow fuzz, making them looks more like those of bee.");
			}
			else
			{
				//(Bird pretext)
				if (previousArmData.type == ArmType.HARPY)
				{
					sb.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
				}
				sb.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments");
				if (previousArmData.type == HARPY || previousArmData.type == HUMAN)
				{
					sb.Append("<b>until it resembles a shiny black exoskeleton</b>");
				}
				else
				{
					sb.Append(", <b>turning the " + previousArmData.primaryEpidermis.shortDescription() + " into a shiny black carapace</b>. ");
				}
				sb.Append("A moment later the pain fades and you are able to turn your gaze down to your beautiful new arms, covered in " +
					"shining black chitin from the upper arm down, and downy yellow fuzz along your upper arm.");
			}
			return sb.ToString();
		}
		private static string BeeRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "\n\nYou arms start to itch like crazy, and no matter how much you scrath, you can't shake the itch. Looking down, you see the cause - the exoskeleton covering"
				+ "your arm is flaking away. Not to be outdone, the yellow fur towards the end starts to fall out as well."
				+ "<b> You now have human arms!</b>";
		}
		private static string DragonDescStr()
		{
			return "draconic arms";
		}
		private static string DragonFullDesc(Arms arm)
		{
			return DragonDescStr();
		}
		private static string DragonPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string DragonTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type.isPredatorArms())
			{
				return "\n\nYour " + previousArmData.handData.ShortDescription() + " change a little to become more dragon-like." +
					" <b>Your arms and claws are like those of a dragon.</b>";
			}
			return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. " +
				"After a longer moment of ignoring it you finally glance down in irritation, only to discover that your arms former " +
				"appearance has changed into those of some reptilian killer with shield-shaped " + player.body.mainEpidermis.tone +
				" scales and powerful, thick, curved steel-gray claws replacing your fingernails.\n<b>You now have dragon arms.</b>";
		}
		private static string DragonRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string ImpDescStr()
		{
			return "predator arms";
		}
		private static string ImpFullDesc(Arms arm)
		{
			return arm.epidermis.justColor() + " predator arms ending in imp claws";
		}
		private static string ImpPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string ImpTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (player.arms.type != HUMAN)
			{
				sb.Append("\n\nYour arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning."
					+ "The skin on your arms visibly thicken, then segment into a hybrid between scales and skin.");
			}
			if (!previousArmData.handData.type.isClaws)
			{
				sb.Append("\n\nYour " + previousArmData.handData.ShortDescription() + " suddenly ache in pain, and all you can do is curl " +
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
		private static string ImpRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string LizardDescStr()
		{
			return "predator arms";
		}
		private static string LizardFullDesc(Arms arm)
		{
			return "predator arms with " + arm.hands.fullDescription();

		}
		private static string LizardPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string LizardTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type.isPredatorArms())
			{
				return "\n\nYour " + previousArmData.handData.ShortDescription() + " change a little to become more lizard-like." +
					" <b>You now have lizard-like claws.</b>";
			}
			else return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer"
				+ " moment of ignoring it you finally glance down in irritation, only to discover that your arms' former appearance has changed into "
				+ "those of some reptilian killer, complete with scales and claws in place of fingernails. Strangely, your claws seem to match the "
				+ "tone of your arms.\n<b>You now have reptilian arms.</b>";
		}
		private static string LizardRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string SalamanderDescStr()
		{
			return "salamader arms";
		}
		private static string SalamanderFullDesc(Arms arm)
		{
			return "salamander arms with " + arm.hands.fullDescription();
		}
		private static string SalamanderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.fullDescription() + "cover your arms from the biceps down, and your fingernails are now " + arm.hands.fullDescription();
		}
		private static string SalamanderTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return "You scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer moment of"
				+ " ignoring it you finally glance down in irritation, only to discover that your arms former appearance has changed into those of a salamander,"
				+ " complete with leathery, red scales and short, fiery-red claws replacing your fingernails.  <b>You now have salamander arms.</b>";
		}
		private static string SalamanderRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your once scaly arms are shedding their scales and that"
				+ " your claws become normal human fingernails again. <b>You have normal human arms again.</b>";

		}
		private static string WolfDescStr()
		{
			return "wolfen arms";
		}
		private static string WolfFullDesc(Arms arm)
		{
			return "wolf-like arms and " + arm.hands.shortDescription();
		}
		private static string WolfPlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are shaped like a wolf's, overly muscular at your shoulders and biceps before quickly slimming down."
				+ " They're covered in " + arm.epidermis.fullDescription() + " and end in paws with just enough flexibility to be used as hands."
				+ " They're rather difficult to move in directions besides back and forth.";
		}
		//based off dog. it was never implemented in game, even though there was text for the PC having them. 
		private static string WolfTransformStr(ArmData previousArmData, PlayerBase player)
		{
			bool armChanged = false;
			StringBuilder sb = new StringBuilder("Your arms feel stiff, and despite any attempt to move them, they just sit there, limply." +
				" You soon realize the bones in your " + previousArmData.handData.ShortDescription() + " are changing, as well as the muscles on your arms.");
			if (previousArmData.type == ArmType.DOG)
			{
				sb.Append("Strangely, your arms don't seem to change much, though they feel much stronger than before. Stretching a little, you realize your paws are also much less." +
					"dextrous than before, though your claws are much sharper. <b> You now have wolf-like arms!</b>");
				return sb.ToString();
			}
			else if (!previousArmData.primaryEpidermis.usesFur && !previousArmData.secondaryEpidermis.usesFur)
			{
				armChanged = true;
				sb.Append(" A thick layer of fur quickly grows in, covering your arm from to the tips of your fingers.");
			}
			if (!previousArmData.handData.type.isPaws)
			{
				sb.Append(armChanged ? " Not to be outdone, your " : " Your ");
				if (previousArmData.handData.type.isHands)
				{
					sb.Append("hands gain pink, padded paws where your palms were once, and your nails become short claws.");
				}
				else if (previousArmData.handData.type.isClaws)
				{
					sb.Append("claws shorten significantly and small pads form under them, until they resemble paws.");
				}
				else //if other.
				{
					sb.Append("appendages shift towards human hands, but instead of nails, you get short claws. Padding appears in your palms and under your claws.");
				}
			}
			sb.Append(" Your claws look sharp and dangerous, easily able to tear flesh if you wanted. "
				+ " <b>Your arms have become like those of a wolf!</b>");
			return sb.ToString();
		}
		private static string WolfRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CockatriceDescStr()
		{
			return "feathery arms";
		}
		private static string CockatriceFullDesc(Arms arm)
		{
			return arm.epidermis.fur.AsString() + ", feathered cockatrice arms with " + arm.hands.fullDescription();
		}
		private static string CockatricePlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are covered in " + arm.epidermis.fullDescription()
		  + " from the shoulder down to the elbow where they stop in a fluffy cuff. A handful of long feathers grow from your"
		  + " elbow in the form of vestigial wings, and while they may not let you fly, they certainly help you jump. Your lower"
		  + " arm is coated in " + arm.secondaryEpidermis.fullDescription() + " and your fingertips terminate in " + arm.hands.shortDescription() + ".";
		}
		private static string CockatriceTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return "Prickling discomfort suddenly erupts all over your body, like every last inch of your skin has suddenly developed"
			+ " pins and needles. You try to rub feeling back into your body, but only succeed in shifting the feeling to your arms."
			+ " Your arms feel like they are " + (previousArmData.usesFur ? "shedding" : "molting") + ". Sure enough, the old"
			+ " outer layer falls off, replaced with leathery scales. Additionally, the upper part of your arms gain feathers, covering them"
			+ " from shoulder to elbow, where they end in a fluffy cuff. As suddenly as the itching came it fades, leaving you to marvel"
			+ " over your new arms.\n<b>You now have cockatrice arms!</b>";
		}
		private static string CockatriceRestoreStr(ArmData previousArmData, PlayerBase player)
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
		private static string RedPandaFullDesc(Arms arm)
		{
			return arm.epidermis.fullDescription() + "panda-arms";
		}
		private static string RedPandaPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.furTexture.ToString() + ", " + arm.epidermis.justColor() + " fluff cover your arms. Your paws have " + arm.hands.shortDescription() + ".";
		}
		private static string RedPandaTransformStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RedPandaRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FerretDescStr()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretFullDesc(Arms arm)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.justColor() + " fluff covers your arms, turning into "
				+ arm.secondaryEpidermis.fullDescription() + " from elbows to paws."
				+ " The latter have " + arm.hands.fullDescription();
		}
		private static string FerretTransformStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FerretRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CatDescStr()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatFullDesc(Arms arm)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string CatTransformStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CatRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string DogDescStr()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogFullDesc(Arms arm)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string DogPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.justColor() + " fluff covers your arms. Your paw-like hands have " + arm.hands.fullDescription()
				+ ". With the right legs (and the right motivation), you could run with them, much like the hellounds you see in the mountains.";
		}
		private static string DogTransformStr(ArmData previousArmData, PlayerBase player)
		{
			bool armChanged = false;
			StringBuilder sb = new StringBuilder("Your arms feel stiff, and despite any attempt to move them, they just sit there, limply." +
				" You soon realize the bones in your " + previousArmData.handData.ShortDescription() + " are changing, as well as the muscles on your arms.");
			if (previousArmData.type == ArmType.WOLF)
			{
				sb.Append("Strangely, your arms don't seem to change much, though they feel a bit weaker than before. Stretching a little, you realize your paws are also much more" +
					"dextrous than before, though your claws are much duller. <b> You now have dog-like arms!</b>");
				return sb.ToString();
			}
			else if (!previousArmData.primaryEpidermis.usesFur && !previousArmData.secondaryEpidermis.usesFur)
			{
				armChanged = true;
				sb.Append(" A thick layer of fur quickly grows in, covering your arm from to the tips of your fingers.");
			}
			if (!previousArmData.handData.type.isPaws)
			{
				sb.Append(armChanged ? " Not to be outdone, your " : " Your ");
				if (previousArmData.handData.type.isHands)
				{
					sb.Append("hands gain pink, padded paws where your palms were once, and your nails become short claws.");
				}
				else if (previousArmData.handData.type.isClaws)
				{
					sb.Append("claws shorten significantly and small pads form under them, until they resemble paws.");
				}
				else //if other.
				{
					sb.Append("appendages shift towards human hands, but instead of nails, you get short claws. Padding appears in your palms and under your claws.");
				}
			}
			sb.Append(" Your claws, while not sharp enough to tear flesh, are nimble enough to make climbing and exploring easier."
				+ " <b>Your arms have become like those of a dog!</b>");
			return sb.ToString();
		}
		private static string DogRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FoxDescStr()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxFullDesc(Arms arm)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string FoxTransformStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FoxRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}

		private static string GooDesc()
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooFullDesc(Arms arms)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooPlayerStr(Arms arm, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooTransformStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}


		private static string CatFoxPlayerStr(Arms arms, PlayerBase player)
		{
			return "Soft, " + arms.epidermis.justColor() + " fluff covers your arms. Your paw-like hands have " + arms.hands.fullDescription() + ".";
		}

		private static string PredatorPlayerStr(Arms arms, PlayerBase player)
		{
			return "Your arms are covered by " + arms.epidermis.shortDescription() + " and your hands are noew " + arms.hands.fullDescription() + ".";
		}

		private static string PredatorRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "\n\nYou feel a sudden tingle in your " + previousArmData.handData.ShortDescription() + " and then you realize,"
				+ " that they have become normal human fingernails again. Your arms quickly follow suit. "
				+ "<b>You have normal human arms again.</b>";
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