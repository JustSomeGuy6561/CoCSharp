using CoC.BodyParts;
using CoC.Creatures;

using System.Text;

namespace CoC.Strings.BodyParts
{
	public static class ArmsStrings
	{
		public static string HumanDescStr()
		{
			return "human arms";
		}
		public static string HumanFullDesc(Arms arm)
		{
			return HumanDescStr();
		}
		public static string HumanPlayerStr(Arms arm, Player player)
		{
			return "";
		}
		public static string HumanTransformStr(Arms oldArms, Player player)
		{
			return oldArms.restoreString(player);
		}
		public static string HumanRestoreStr(Arms currentArms, Player player)
		{
			return GlobalStrings.RevertAsDefault(currentArms, player);
		}
		public static string HarpyDescStr()
		{
			return "feathered arms";
		}
		public static string HarpyFullDesc(Arms arm)
		{
			return arm.epidermis.justColor() + " feathered arms";
		}
		public static string HarpyPlayerStr(Arms arm, Player player)
		{
			return "Feathers hang off your arms from shoulder to wrist, giving them a slightly wing-like look.";
		}
		public static string HarpyTransformStr(Arms oldArms, Player player)
		{
			StringBuilder retVal = new StringBuilder("You look on in horror while");
			if (oldArms.epidermis.type == EpidermisType.FUR)
			{
				retVal.Append("your fur falls off your arms, and avian plumage grows in its place.");
			}
			else
			{
				retVal.Append("avian plumage sprouts from your " + oldArms.epidermis.shortDescription() + ", covering your forearms "
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
		public static string HarpyRestoreStr(Arms currentArms, Player player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your feathery arms are shedding their feathery coating."
				+ "\nThe wing-like shape your arms once had is gone in a matter of moments, leaving human skin behind.";
		}
		public static string SpiderDescStr()
		{
			return "spider arms";
		}
		public static string SpiderFullDesc(Arms arm)
		{
			return "chitinous " + arm.epidermis.justColor() + " spider arms";
		}
		public static string SpiderPlayerStr(Arms arm, Player player)
		{
			return arm.epidermis.FullDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves from a distance.";
		}
		public static string SpiderTransformStr(Arms oldArms, Player player)
		{
			StringBuilder retVal = new StringBuilder();
			if (oldArms.type == ArmType.HARPY)
			{
				retVal.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
			}
			retVal.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments, " +
				"<b>turning the " + player.arms.epidermis.shortDescription() + " into a shiny black carapace</b>. You touch the onyx exoskeleton and discover to your delight that " +
				"you can still feel through it as naturally as your own skin.");
			return retVal.ToString();
		}
		public static string SpiderRestoreStr(Arms currentArms, Player player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch." 
				+ "Glancing down in irritation, you discover that your arms' chitinous covering is flaking away."
				+ "\nThe glossy black coating is soon gone, leaving human skin behind.";
		}
		public static string BeeDescStr()
		{
			return "fuzzy bee arms";
		}
		public static string BeeFullDesc(Arms arm)
		{
			return BeeDescStr();
		}
		public static string BeePlayerStr(Arms arm, Player player)
		{
			return arm.epidermis.FullDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves ended with a yellow fuzz from a distance.";
		}
		public static string BeeTransformStr(Arms oldArms, Player player)
		{
			StringBuilder sb = new StringBuilder("\n\n");
			if (oldArms.type == ArmType.SPIDER)
			{
				sb.Append("On your upper arms slowly starting to grown yellow fuzz making them looks more like those of bee.");
			}
			else
			{
				//(Bird pretext)
				if (oldArms.type == ArmType.HARPY)
				{
					sb.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
				}
				sb.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles"
						  + " while it divides into segments, <b>turning the [skinFurScales] into a shiny black carapace</b>. A moment later the"
						  + " pain fades and you are able to turn your gaze down to your beautiful new arms, covered in shining black chitin from"
						  + " the upper arm down, and downy yellow fuzz along your upper arm.");
			}
			return sb.ToString();
		}
		public static string BeeRestoreStr(Arms currentArms, Player player)
		{
			return "\n\nYou arms start to itch like crazy, and no matter how much you scrath, you can't shake the itch. Looking down, you see the cause - the exoskeleton covering"
				+ "your arm is flaking away. Not to be outdone, the yellow fur towards the end starts to fall out as well."
				+ "<b> You now have human arms!</b>";
		}
		public static string DragonDescStr()
		{
			return "draconic arms";
		}
		public static string DragonFullDesc(Arms arm)
		{
			return DragonDescStr();
		}
		public static string DragonPlayerStr(Arms arm, Player player)
		{
			return PredatorPlayerStr(arm, player);
		}
		public static string DragonTransformStr(Arms oldArms, Player player)
		{
			if (oldArms.type.isPredatorArms())
			{
				return "\n\nYour " + oldArms.hands.shortDescription() + " change a little to become more dragon-like." +
					" <b>You now have [claws].</b>";
			}
			return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. " +
				"After a longer moment of ignoring it you finally glance down in irritation, only to discover that your arms former " +
				"appearance has changed into those of some reptilian killer with shield-shaped " + player.body.primaryEpidermis.tone + 
				" scales and powerful, thick, curved steel-gray claws replacing your fingernails.\n<b>You now have dragon arms.</b>";
		}
		public static string DragonRestoreStr(Arms currentArms, Player player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		public static string ImpDescStr()
		{
			return "predator arms";
		}
		public static string ImpFullDesc(Arms arm)
		{
			return arm.epidermis.justColor() + " predator arms ending in imp claws";
		}
		public static string ImpPlayerStr(Arms arm, Player player)
		{
			return PredatorPlayerStr(arm, player);
		}
		public static string ImpTransformStr(Arms oldArms, Player player)
		{
			StringBuilder sb = new StringBuilder();
			if (player.arms.type != ArmType.HUMAN)
			{
				sb.Append("\n\nYour arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning."
					+ "The skin on your arms visibly thicken, then segment into a hybrid between scales and skin.");
			}
			if (!oldArms.hands.type.isClaws)
			{
				sb.Append("\n\nYour " + (oldArms.hands.type.isPaws ? "paws" : "hands") + " suddenly ache in pain, and all you can do is curl " +
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
		public static string ImpRestoreStr(Arms currentArms, Player player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		public static string LizardDescStr()
		{
			return "predator arms";
		}
		public static string LizardFullDesc(Arms arm)
		{
			return "predator arms with " + arm.hands.fullDescription();

		}
		public static string LizardPlayerStr(Arms arm, Player player)
		{
			return PredatorPlayerStr(arm, player);
		}
		public static string LizardTransformStr(Arms oldArms, Player player)
		{
			return "\n\nYou scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer" 
				+ " moment of ignoring it you finally glance down in irritation, only to discover that your arms' former appearance has changed into " 
				+ "those of some reptilian killer, complete with scales and claws in place of fingernails. Strangely, your claws seem to match the "
				+ "tone of your arms.\n<b>You now have reptilian arms.</b>";
		}
		public static string LizardRestoreStr(Arms currentArms, Player player)
		{
			return PredatorRestoreStr(currentArms, player);
		}
		public static string SalamanderDescStr()
		{

		}
		public static string SalamanderFullDesc(Arms arm)
		{

		}
		public static string SalamanderPlayerStr(Arms arm, Player player)
		{
			return arm.epidermis.FullDescription() + "cover your arms from the biceps down, and your fingernails are now " + arm.hands.shortDescription();
		}
		public static string SalamanderTransformStr(Arms oldArms, Player player)
		{

		}
		public static string SalamanderRestoreStr(Arms currentArms, Player player)
		{
			return "\n\nYou scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch." 
				+ "Glancing down in irritation, you discover that your once scaly arms are shedding their scales and that"
				+ " your claws become normal human fingernails again.";
		}
		public static string WolfDescStr()
		{

		}
		public static string WolfFullDesc(Arms arm)
		{

		}
		public static string WolfPlayerStr(Arms arm, Player player)
		{
			return "Your arms are shaped like a wolf's, overly muscular at your shoulders and biceps before quickly slimming down."
				+ " They're covered in " + arm.epidermis.FullDescription() + " and end in paws with just enough flexibility to be used as hands."
				+ " They're rather difficult to move in directions besides back and forth.";
		}
		public static string WolfTransformStr(Arms oldArms, Player player)
		{

		}
		public static string WolfRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string CockatriceDescStr()
		{

		}
		public static string CockatriceFullDesc(Arms arm)
		{

		}
		public static string CockatricePlayerStr(Arms arm, Player player)
		{
			return "Your arms are covered in " + arm.epidermis.FullDescription()
		  + " from the shoulder down to the elbow where they stop in a fluffy cuff. A handful of long feathers grow from your"
		  + " elbow in the form of vestigial wings, and while they may not let you fly, they certainly help you jump. Your lower"
		  + " arm is coated in " + arm.secondaryEpidermis.FullDescription() + " and your fingertips terminate in deadly looking avian talons.";
		}
		public static string CockatriceTransformStr(Arms oldArms, Player player)
		{

		}
		public static string CockatriceRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string RedPandaDescStr()
		{

		}
		public static string RedPandaFullDesc(Arms arm)
		{

		}
		public static string RedPandaPlayerStr(Arms arm, Player player)
		{
			return "Soft, " + arm.epidermis.justColor() + " fluff cover your arms. Your paws have cute, pink paw pads and short claws.";
		}
		public static string RedPandaTransformStr(Arms oldArms, Player player)
		{

		}
		public static string RedPandaRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string FerretDescStr()
		{

		}
		public static string FerretFullDesc(Arms arm)
		{

		}
		public static string FerretPlayerStr(Arms arm, Player player)
		{
			return "Soft, " + arm.epidermis.justColor() + " fluff covers your arms, turning into "
				+ arm.secondaryEpidermis.fullDescript() + " from elbows to paws."
				+ " The latter have cute, pink paw pads and short claws.";
		}
		public static string FerretTransformStr(Arms oldArms, Player player)
		{

		}
		public static string FerretRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string CatDescStr()
		{

		}
		public static string CatFullDesc(Arms arm)
		{

		}
		public static string CatPlayerStr(Arms arm, Player player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		public static string CatTransformStr(Arms oldArms, Player player)
		{

		}
		public static string CatRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string DogDescStr()
		{

		}
		public static string DogFullDesc(Arms arm)
		{

		}
		public static string DogPlayerStr(Arms arm, Player player)
		{
			return "Soft, " + arm.epidermis.justColor() + " fluff covers your arms. Your paw-like hands have cute, pink paw "
				+ "pads and short claws. With the right legs (and the right motivation), you could run with them, much like "
				+ "the hellounds you see in the mountains.";
		}
		public static string DogTransformStr(Arms oldArms, Player player)
		{

		}
		public static string DogRestoreStr(Arms currentArms, Player player)
		{

		}
		public static string FoxDescStr()
		{

		}
		public static string FoxFullDesc(Arms arm)
		{

		}
		public static string FoxPlayerStr(Arms arm, Player player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		public static string FoxTransformStr(Arms oldArms, Player player)
		{

		}
		public static string FoxRestoreStr(Arms currentArms, Player player)
		{

		}


		private static string CatFoxPlayerStr(Arms arms, Player player)
		{
			return "Soft, " + arms.epidermis.justColor() + " fluff covers your arms. Your paw-like hands have cute, pink paw pads and " + arms.hands.shortDescription() + ".";
		}

		private static string PredatorPlayerStr(Arms arms, Player player)
		{
			return "Your arms are covered by " + arms.epidermis.shortDescription() + " and your fingernails are now " + arms.hands.shortDescription() + ".";
		}

		private static string PredatorRestoreStr(Arms arms, Player player)
		{
			return "\n\nYou feel a sudden tingle in your " + arms.hands.shortDescription() +" and then you realize,"
				+ " that they have become normal human fingernails again. Your arms quickly follow suit. " 
				+ "<b>You have normal human arms again.</b>"
		}

		private static string GenericRestoreString()
		{
			return "\n\nYour unusual arms change more and more until they are normal human arms, leaving only skin behind." +
				"<b>You have normal human arms again.</b>";

		}

		"<b>You have normal human arms again.</b>"
	}
}