//ArmStrings.cs
//Description: Implements the strings for the arm and armtype. separation of concerns.
//Author: JustSomeGuy
//1/18/2019, 9:30 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public partial class ArmTattooLocation
	{
		private static string LeftHandButton()
		{
			return "Left Hand";
		}
		private static string LeftHandLocation()
		{
			return "left hand";
		}

		private static string LeftWristButton()
		{
			return "Left Wrist";
		}
		private static string LeftWristLocation()
		{
			return "left wrist";
		}
		private static string LeftInnerArmButton()
		{
			return "L.Inner Fore";
		}
		private static string LeftInnerArmLocation()
		{
			return "left inner forearm";
		}
		private static string LeftOuterArmButton()
		{
			return "L.Outer Fore";
		}
		private static string LeftOuterArmLocation()
		{
			return "left outer forearm";
		}

		private static string LeftForearmButton()
		{
			return "L. Forearm";
		}
		private static string LeftForearmLocation()
		{
			return "left forearm";
		}

		private static string LeftUpperArmButton()
		{
			return "L. Upper Arm";
		}
		private static string LeftUpperArmLocation()
		{
			return "left upper arm";
		}
		private static string LeftShoulderButton()
		{
			return "L. Shoulder";
		}
		private static string LeftShoulderLocation()
		{
			return "left shoulder";
		}
		private static string LeftSleeveButton()
		{
			return "Left Sleeve";
		}
		private static string LeftSleeveLocation()
		{
			return "entire left arm";
		}

		private static string RightHandButton()
		{
			return "Right Hand";
		}
		private static string RightHandLocation()
		{
			return "right hand";
		}

		private static string RightWristButton()
		{
			return "Right Wrist";
		}
		private static string RightWristLocation()
		{
			return "right wrist";
		}
		private static string RightInnerArmButton()
		{
			return "R.Inner Fore";
		}
		private static string RightInnerArmLocation()
		{
			return "right inner forearm";
		}
		private static string RightOuterArmButton()
		{
			return "R.Outer Fore";
		}
		private static string RightOuterArmLocation()
		{
			return "right outer forearm";
		}
		private static string RightForearmButton()
		{
			return "R. Forearm";
		}
		private static string RightForearmLocation()
		{
			return "right forearm";
		}
		private static string RightUpperArmButton()
		{
			return "R. Upper Arm";
		}
		private static string RightUpperArmLocation()
		{
			return "right upper arm";
		}
		private static string RightShoulderButton()
		{
			return "R. Shoulder";
		}
		private static string RightShoulderLocation()
		{
			return "right shoulder";
		}
		private static string RightSleeveButton()
		{
			return "Right Sleeve";
		}
		private static string RightSleeveLocation()
		{
			return "entire right arm";
		}
	}

#warning Handle GOO TFs so they suck less
	public partial class Arms
	{
		public static string Name()
		{
			return "Arms";
		}

		private string AllTattoosShort(PlayerBase player)
		{
			//feel free to alter this flavor text or rewrite some of these to use other flavor text.
			string armFlavorText = ", visible for all to see.";

			//first, see if we have no tats.
			if (tattoos.currentTattooCount == 0)
			{
				return "";
			}
			//start with the tattoo-heavy options and work our way down
			//full tats.
			if (tattoos.currentTattooCount == tattoos.MaxTattoos)
			{
				return "A myriad of tattoos cover both of your arms from shoulder to " + hands.HandText(false) + armFlavorText;
			}
			//full tat (left arm)
			else if (tattoos.allOnLeftArm)
			{
				return "A myriad of tattoos cover your left arm" + armFlavorText + " They contrast sharply with your right arm, which has none.";
			}
			//full tat (right arm)
			else if (tattoos.allOnRightArm)
			{
				return "A myriad of tattoos cover your right arm" + armFlavorText + " They contrast sharply with your left arm, which has none.";
			}
			//more than a couple of tats.
			else if (tattoos.currentTattooCount > 2)
			{
				string armsText;

				if (tattoos.onlyOnLeftArm)
				{
					armsText = "covering your left arm, contrasting your right, which has none.";
				}
				else if (tattoos.onlyOnRightArm)
				{
					armsText = "covering your right arm, contrasting your right, which has none.";
				}
				else
				{
					var leftTats = tattoos.currentLeftArmTattoos;
					var rightTats = tattoos.currentRightArmTattoos;

					if (leftTats.Length > rightTats.Length)
					{
						armsText = "on both your arms, though you have more on your left.";
					}
					else if (leftTats.Length < rightTats.Length)
					{
						armsText = "on both your arms, though you have more on your right.";
					}
					else
					{
						return "on each of your arms.";
					}
				}

				return "You have several tattoos clearly visibile " + armsText;
			}
			//2 tats or less.

			//start with the sleeves.
			else if (tattoos.OnlySleeveTattoos)
			{
				if (tattoos.MatchingSleeveTattoosIgnoreColor())
				{
					return "Each one of your arms is covered in " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(true) + armFlavorText;
				}
				else
				{
					return "You have a pair of distinctly different tattoos running along your arms" + armFlavorText;
				}
			}
			else if (tattoos.OnlyShoulderTattoos)
			{
				if (tattoos.MatchingShoulderTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "tattoos on your shoulders" + armFlavorText;
				}
				else
				{
					return "You have a pair of tattoos on your arms, one on each shoulder" + armFlavorText;
				}
			}
			else if (tattoos.OnlyForearmTattoos)
			{
				if (tattoos.MatchingForearmTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "tattoos covering your forearms" + armFlavorText;
				}
				else
				{
					return "You have a pair of tattoos covering your forearms" + armFlavorText;
				}
			}
			else if (tattoos.OnlyOuterForearmTattoos)
			{
				if (tattoos.MatchingOuterForearmTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "on your upper arms" + armFlavorText;
				}
				else
				{
					return "You have a pair of tattoos on your upper arms" + armFlavorText;
				}
			}
			else if (tattoos.OnlyInnerForearmTattoos)
			{
				if (tattoos.MatchingInnerForearmTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "tattoos along the inner half of your forearms" + armFlavorText;
				}
				else
				{
					return "You have a pair of tattoos on the inner halves of your forarms" + armFlavorText;
				}
			}

			else if (tattoos.OnlyWristTattoos)
			{
				if (tattoos.MatchingWristTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "tattoos on your wrists" + armFlavorText;
				}
				else
				{
					return "You have a pair of tattoos on your wrists" + armFlavorText;
				}
			}
			else if (tattoos.OnlyHandTattoos)
			{
				if (tattoos.MatchingWristTattoosIgnoreColor())
				{
					return "You have a pair of " + tattoos[ArmTattooLocation.LEFT_SLEEVE].ShortDescription(false) + "tattoos, one on each " + hands.HandText(false) + armFlavorText;
				}
				else
				{
					return "You have a tattoo on each " + hands.HandText(false) + armFlavorText;
				}
			}

			else if (tattoos.currentTattooCount > 1)
			{
				return "A pair of tattoos adorn your arms" + armFlavorText + " The first is on your " + tattoos.currentTattoos[0].Description() + ", the other on your "
					+ tattoos.currentTattoos[1].Description() + ".";
			}
			else
			{
				return "You have " + tattoos[tattoos.currentTattoos[0]].LongDescription(true) + "tattoo on your " + tattoos.currentTattoos[0].Description() + armFlavorText;
			}
		}

		private string AllTattoosLong(PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("Your " + ShortDescription());
			if (tattoos.currentTattooCount == tattoos.MaxTattoos)
			{
				sb.Append("are covered in a myriad of tattoos: " + Environment.NewLine);
			}
			else if (tattoos.currentTattooCount > tattoos.MaxTattoos / 2.0)
			{
				sb.Append("are covered with tattoos: " + Environment.NewLine);
			}
			else if (tattoos.currentTattooCount > tattoos.MaxTattoos / 4.0)
			{
				sb.Append("have a few tattoos covering them in various places: " + Environment.NewLine);
			}
			else if (tattoos.currentTattooCount > 0)
			{
				sb.Append("are not completely devoid of tattoos: " + Environment.NewLine);
			}
			//generally, we won't ever see this - we just skip empty tattoos. But, if it's called, we have something that works and lets us exit quickly.
			else
			{
				sb.Append("are completely devoid of tattoos." + Environment.NewLine);
				return sb.ToString();
			}

			//start with the hands.
			if (tattoos.TattooedAt(ArmTattooLocation.LEFT_HAND) || tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
			{
				if (tattoos.MatchingHandTattoos())
				{
					sb.Append("You have matching " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(false) + "tattoos on your " + hands.HandText() + ".");
				}
				else if (tattoos.MatchingHandTattooIgnoreColor())
				{
					sb.Append("You have " + tattoos[ArmTattooLocation.LEFT_HAND].ShortDescription(true) + "tattoo on each " + hands.HandText(false) + " - the left is " +
						tattoos[ArmTattooLocation.LEFT_HAND].tattooColor.AsString() + "; the right " + tattoos[ArmTattooLocation.RIGHT_HAND].tattooColor.AsString() + ".");
				}
				else
				{
					sb.Append("Your ");
					if (tattoos.TattooedAt(ArmTattooLocation.LEFT_HAND))
					{
						sb.Append("left hand has " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(true) + "on it");
						if (tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
						{
							sb.Append(", and your ");
						}
					}

					if (tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
					{
						sb.Append("right hand has " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(true) + "on it");
					}
					sb.Append("." + Environment.NewLine);
				}
			}
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//then the wrists.
			//if (tattoos.TattooedAt(ArmTattooLocation.LEFT_HAND) || tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
			//{
			//	if (tattoos.MatchingHandTattoos())
			//	{
			//		sb.Append("You have matching " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(false) + "tattoos on your " + hands.HandText() + ".");
			//	}
			//	else if (tattoos.MatchingHandTattooIgnoreColor())
			//	{
			//		sb.Append("You have " + tattoos[ArmTattooLocation.LEFT_HAND].ShortDescription(true) + " on each " + hands.HandText(false) + " - the left is " +
			//			tattoos[ArmTattooLocation.LEFT_HAND].tattooColor.AsString() + "; the right " + tattoos[ArmTattooLocation.RIGHT_HAND].tattooColor.AsString() + ".");
			//	}
			//	else
			//	{
			//		sb.Append("Your ");
			//		if (tattoos.TattooedAt(ArmTattooLocation.LEFT_HAND))
			//		{
			//			sb.Append("left hand has " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(true) + "on it");
			//			if (tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
			//			{
			//				sb.Append(", and your ");
			//			}
			//		}

			//		if (tattoos.TattooedAt(ArmTattooLocation.RIGHT_HAND))
			//		{
			//			sb.Append("right hand has " + tattoos[ArmTattooLocation.LEFT_HAND].LongDescription(true) + "on it");
			//		}
			//		sb.Append("." + Environment.NewLine);
			//	}
			//}
		}


	}

	public partial class ArmType
	{
		private static string HandsOptionalFingers(HandData hands, bool includeFingers, bool plural = true)
		{
			return includeFingers ? hands.ShortDescription(plural) : hands.LongDescription(plural);
		}

		private static string IntroMaker(bool plural, bool articleFormat, bool useAn)
		{
			if (plural && articleFormat) return "a pair of ";
			else if (articleFormat) return useAn ? "an " : "a ";
			else return "";
		}

		public static string ArmEpidermisDescription(EpidermalData primary, EpidermalData secondary)
		{
			if (EpidermalData.CheckMixedTypes(primary, secondary))
			{
				return "mixed " + primary.ShortDescription() + " and " + secondary.ShortDescription();
			}
			else
			{
				return primary.ShortDescription();
			}
		}

		private static string HumanDesc(bool plural)
		{
			return plural ? "arms" : "arm";
		}

		private static string HumanSingleDesc()
		{
			return "an arm";
		}

		private static string HumanLongDesc(ArmData arm, bool articleForm, bool plural)
		{
			if (plural && articleForm) return "a pair of humainoid arms";
			if (plural) return "humanoid arms";
			else return "humanoid arm";
		}

		private static string HumanFullDesc(ArmData arm, bool articleForm, bool plural, bool includeFingers)
		{
			string hand = Utils.PluralizeIf("hand", plural);
			string handText;
			if (includeFingers)
			{
				handText = $", {hand}, and fingers";
			}
			else
			{
				handText = $" and {hand}";
			}

			if (plural && articleForm)
			{
				return "a pair of humainoid arms" + handText;
			}
			else if (plural) return "humanoid arms" + handText;
			else return "humanoid arm" + handText;
		}

		private static string HumanPlayerStr(Arms arm, PlayerBase player)
		{
			return "";
		}
		private static string HumanTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return previousArmData.type.RestoredString(previousArmData, player);
		}
		private static string HumanRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousArmData, player);
		}
		private static string HarpyDesc(bool plural)
		{
			return Utils.PluralizeIf("feathered arm", plural);
		}

		private static string HarpySingleDesc()
		{
			return "a feathered arm";
		}

		private static string HarpyLongDesc(ArmData arm, bool articleForm, bool plural)
		{
			if (plural)
			{
				string articleText = articleForm ? "a pair of " : "";
				return articleText + arm.epidermis.JustColor() + " feathered arms";
			}
			else if (articleForm)
			{
				return $"a {arm.epidermis.JustColor()} feathered arm";
			}
			else
			{
				return $"{arm.epidermis.JustColor()} feathered arm";
			}
		}

		private static string HarpyFullDesc(ArmData arm, bool articleForm, bool plural, bool includeFingers)
		{
			if (plural)
			{
				string articleText = articleForm ? "a pair of " : "";
				return articleText + arm.epidermis.JustColor() + " feathered arms ending in " + HandsOptionalFingers(arm.hands, includeFingers, plural);
			}
			else if (articleForm)
			{
				return $"a {arm.epidermis.JustColor()} feathered arm ending in " + HandsOptionalFingers(arm.hands, includeFingers, plural);
			}
			else
			{
				return "feathered arm with its " + HandsOptionalFingers(arm.hands, includeFingers, plural);
			}
		}

		private static string HarpyPlayerStr(Arms arm, PlayerBase player)
		{
			return "Feathers hang off your arms from shoulder to wrist, giving them a slightly wing-like look.";
		}
		private static string HarpyTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder retVal = new StringBuilder("You look on in horror while");
			if (previousArmData.usesAnyFur)
			{
				retVal.Append("your fur falls off your arms, and avian plumage grows in its place.");
			}
			else
			{
				retVal.Append("avian plumage sprouts from your " + previousArmData.epidermis.ShortDescription() + ", covering your forearms "
					+ "until " + SafelyFormattedString.FormattedText("your arms look vaguely like wings.", StringFormats.BOLD));
			}
			if (previousArmData.hands.type != HandType.HUMAN)
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
				 + Environment.NewLine + "The wing-like shape your arms once had is gone in a matter of moments, leaving human skin behind.";
		}
		private static string SpiderDesc(bool plural)
		{
			return Utils.PluralizeIf("spider arm", plural);
		}

		private static string SpiderSingleDesc()
		{
			return "a spider arm";
		}

		private static string SpiderLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural) intro = "a pair of ";
			else if (alternateFormat) intro = "a ";
			else intro = "";

			return intro + "chitinous " + arm.epidermis.JustColor() + Utils.PluralizeIf(" spider arm", plural);
		}

		private static string SpiderFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string SpiderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves from a distance.";
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
				retVal.Append(", " + SafelyFormattedString.FormattedText("turning the " + previousArmData.EpidermisDescription() + " into a shiny black carapace", StringFormats.BOLD) + ". ");
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
			return GlobalStrings.NewParagraph() + "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your arms' chitinous covering is flaking away."
				 + Environment.NewLine + "The glossy black coating is soon gone, leaving human skin behind.";
		}
		private static string BeeDesc(bool plural)
		{
			return Utils.PluralizeIf("fuzzy bee arm", plural);
		}

		private static string BeeSingleDesc()
		{
			return "a fuzzy bee arm";
		}
		private static string BeeLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro = IntroMaker(plural, alternateFormat, false);
			return $"{intro}fuzzy {arm.epidermis.JustColor()} bee-arms";
		}

		private static string BeeFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}


		private static string BeePlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + " covers your arms from the biceps down, resembling a pair of long black gloves ended with a yellow fuzz from a distance.";
		}
		private static string BeeTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());
			if (previousArmData.type == ArmType.SPIDER)
			{
				sb.Append("The " + previousArmData.epidermis.ShortDescription() + "on your upper arms slowly starting to grown yellow fuzz, making them looks more like those of bee.");
			}
			else
			{
				//(Bird pretext)
				if (previousArmData.type == ArmType.HARPY)
				{
					sb.Append("The feathers covering your arms fall away, leaving them to return to a far more human appearance. ");
				}
				else if (previousArmData.usesAnyFur)
				{
					sb.Append("The fur covering your arms fall away, leaving them to return to a far more human appearance. ");
				}
				sb.Append("You watch, spellbound, while your forearms gradually become shiny. The entire outer structure of your arms tingles while it divides into segments");
				if (previousArmData.type == HARPY || previousArmData.type == HUMAN)
				{
					sb.Append(SafelyFormattedString.FormattedText("until it resembles a shiny black exoskeleton", StringFormats.BOLD));
				}
				else
				{
					sb.Append(", " + SafelyFormattedString.FormattedText("turning the " + previousArmData.epidermis.ShortDescription() + " into a shiny black carapace", StringFormats.BOLD) + ". ");
				}
				sb.Append("A moment later the pain fades and you are able to turn your gaze down to your beautiful new arms, covered in " +
					"shining black chitin from the upper arm down, and downy yellow fuzz along your upper arm.");
			}
			return sb.ToString();
		}
		private static string BeeRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return GlobalStrings.NewParagraph() + "You arms start to itch like crazy, and no matter how much you scrath, you can't shake the itch. Looking down, you see the cause - "
				+ "the exoskeleton covering your arm is flaking away. Not to be outdone, the yellow fur towards the end starts to fall out as well. "
				+ SafelyFormattedString.FormattedText("You now have human arms!", StringFormats.BOLD);
		}
		private static string DragonDesc(bool plural)
		{
			return Utils.PluralizeIf("draconic arm", plural);
		}

		private static string DragonSingleDesc()
		{
			return "a draconic arm";
		}

		private static string DragonLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			return GenericLongDesc(arm, alternateFormat, plural);
		}
		private static string DragonFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string DragonPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string DragonTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type.isPredatorArms())
			{
				return GlobalStrings.NewParagraph() + "Your " + previousArmData.hands.ShortDescription() + " change a little to become more dragon-like." +
					" " + SafelyFormattedString.FormattedText("Your arms and claws are like those of a dragon.", StringFormats.BOLD);
			}
			return GlobalStrings.NewParagraph() + "You scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. " +
				"After a longer moment of ignoring it you finally glance down in irritation, only to discover that your arms former " +
				"appearance has changed into those of some reptilian killer with shield-shaped " + player.body.mainEpidermis.tone +
				" scales and powerful, thick, curved steel-gray claws replacing your fingernails." + Environment.NewLine + SafelyFormattedString.FormattedText("You now have dragon arms.", StringFormats.BOLD);
		}
		private static string DragonRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string ImpDesc(bool plural)
		{
			return Utils.PluralizeIf("predator arm", plural);
		}

		private static string ImpSingleDesc()
		{
			return "a predator arm";
		}

		private static string ImpLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			return GenericLongDesc(arm, alternateFormat, plural);
		}

		private static string ImpFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}



		private static string ImpPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string ImpTransformStr(ArmData previousArmData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (previousArmData.type != HUMAN)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Your arms twist and mangle, warping back into human-like arms. But that, you realize, is just the beginning."
					+ "The skin on your arms visibly thicken, then segment into a hybrid between scales and skin.");
			}
			if (!previousArmData.hands.isClaws)
			{
				sb.Append(GlobalStrings.NewParagraph() + "Your " + previousArmData.hands.ShortDescription() + " suddenly ache in pain, and all you can do is curl " +
					"them up to you. Against your body, you feel them form into three long claws, with a smaller one replacing your thumb but " +
					"just as versatile. " + SafelyFormattedString.FormattedText("You have imp claws!", StringFormats.BOLD));
			}
			else
			{ //has claws
				sb.Append(GlobalStrings.NewParagraph() + "Your claws suddenly begin to shift and change, starting to turn back into normal hands. But just before they do, they" +
					" stretch out into three long claws, with a smaller one coming to form a pointed thumb. " + SafelyFormattedString.FormattedText("You have imp claws!", StringFormats.BOLD));
			}
			return sb.ToString();
		}
		private static string ImpRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string LizardDesc(bool plural)
		{
			return Utils.PluralizeIf("predator arm", plural);
		}

		private static string LizardSingleDesc()
		{
			return "a predator arm";
		}
		private static string LizardLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			return GenericLongDesc(arm, alternateFormat, plural);
		}
		private static string LizardFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string LizardPlayerStr(Arms arm, PlayerBase player)
		{
			return PredatorPlayerStr(arm, player);
		}
		private static string LizardTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type.isPredatorArms())
			{
				return GlobalStrings.NewParagraph() + "Your " + previousArmData.hands.ShortDescription() + " change a little to become more lizard-like." +
					" " + SafelyFormattedString.FormattedText("You now have lizard-like claws.", StringFormats.BOLD);
			}
			else return GlobalStrings.NewParagraph() + "You scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer"
				+ " moment of ignoring it you finally glance down in irritation, only to discover that your arms' former appearance has changed into "
				+ "those of some reptilian killer, complete with scales and claws in place of fingernails. Strangely, your claws seem to match the "
				+ "tone of your arms." + Environment.NewLine + SafelyFormattedString.FormattedText("You now have reptilian arms.", StringFormats.BOLD);
		}
		private static string LizardRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PredatorRestoreStr(previousArmData, player);
		}
		private static string SalamanderDesc(bool plural)
		{
			return Utils.PluralizeIf("salamader arm", plural);
		}

		private static string SalamanderSingleDesc()
		{
			return "a salamader arm";
		}
		private static string SalamanderLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			return GenericLongDesc(arm, alternateFormat, plural);
		}
		private static string SalamanderFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string SalamanderPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.LongDescription() + "cover your arms from the biceps down, and your fingernails are now " + arm.hands.LongDescription();
		}
		private static string SalamanderTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return "You scratch your biceps absentmindedly, but no matter how much you scratch, you can't get rid of the itch. After a longer moment of"
				+ " ignoring it you finally glance down in irritation, only to discover that your arms former appearance has changed into those of a salamander,"
				+ " complete with leathery, red scales and short, fiery-red claws replacing your fingernails.  " + SafelyFormattedString.FormattedText("You now have salamander arms.", StringFormats.BOLD);
		}
		private static string SalamanderRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return GlobalStrings.NewParagraph() + "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch."
				+ "Glancing down in irritation, you discover that your once scaly arms are shedding their scales and that"
				+ " your claws become normal human fingernails again. " + SafelyFormattedString.FormattedText("You have normal human arms again.", StringFormats.BOLD);

		}
		private static string WolfDesc(bool plural)
		{
			return Utils.PluralizeIf("wolfen arm", plural);
		}

		private static string WolfSingleDesc()
		{
			return "a wolfen arm";
		}
		private static string WolfLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			return GenericLongDesc(arm, alternateFormat, plural);
		}
		private static string WolfFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string WolfPlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are shaped like a wolf's, overly muscular at your shoulders and biceps before quickly slimming down."
				+ " They're covered in " + arm.epidermis.LongDescription() + " and end in paws with just enough flexibility to be used as hands."
				+ " They're rather difficult to move in directions besides back and forth.";
		}
		//based off dog. it was never implemented in game, even though there was text for the PC having them.
		private static string WolfTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type == ArmType.DOG)
			{
				return "Your arms feel stiff, and despite any attempt to move them, they just sit there, limply. You soon realize the bones in your " + previousArmData.hands.ShortDescription() +
					" are changing, as well as the muscles on your arms. Strangely, your arms don't seem to change much, though they feel much stronger than before. " +
					"Stretching a little, you realize your paws are also much less dextrous than before, though your claws are much sharper. " + SafelyFormattedString.FormattedText(" You now have wolf-like arms!", StringFormats.BOLD);
			}
			else
			{
				return GenericPawTransformStr(previousArmData, "wolf", false, true);
			}
		}
		private static string WolfRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CockatriceDesc(bool plural)
		{
			return Utils.PluralizeIf("feathery arm", plural);
		}

		private static string CockatriceSingleDesc()
		{
			return "a feathery arm";
		}

		private static string CockatriceLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.JustColor() + " ";
			}
			else
			{
				intro = arm.epidermis.JustColor(alternateFormat);
			}

			return intro + Utils.PluralizeIf(", feathered cockatrice arm", plural);
		}
		private static string CockatriceFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string CockatricePlayerStr(Arms arm, PlayerBase player)
		{
			return "Your arms are covered in " + arm.epidermis.LongDescription()
		  + " from the shoulder down to the elbow where they stop in a fluffy cuff. A handful of long feathers grow from your"
		  + " elbow in the form of vestigial wings, and while they may not let you fly, they certainly help you jump. Your lower"
		  + " arm is coated in " + arm.secondaryEpidermis.LongDescription() + " and your fingertips terminate in " + arm.hands.ShortDescription() + ".";
		}
		private static string CockatriceTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return "Prickling discomfort suddenly erupts all over your body, like every last inch of your skin has suddenly developed"
			+ " pins and needles. You try to rub feeling back into your body, but only succeed in shifting the feeling to your arms."
			+ " Your arms feel like they are " + (previousArmData.usesAnyFur ? "shedding" : "molting") + ". Sure enough, the old"
			+ " outer layer falls off, replaced with leathery scales. Additionally, the upper part of your arms gain feathers, covering them"
			+ " from shoulder to elbow, where they end in a fluffy cuff. As suddenly as the itching came it fades, leaving you to marvel"
			+ " over your new arms." + Environment.NewLine + SafelyFormattedString.FormattedText("You now have cockatrice arms!", StringFormats.BOLD);
		}
		private static string CockatriceRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "You scratch at your biceps absentmindedly, but no matter how much you scratch, it isn't getting rid of the itch. "
				+ "You quickly notice your arms are shedding their scales, and while this is normal, you've never seen it to this extent. "
				+ "Weirder still, underneath it is not new scales, but rather skin. Your upper arms are also experiencing the change, as the feathers "
				+ "that once covered them are falling out. You soon realize " + SafelyFormattedString.FormattedText("You have normal arms again!", StringFormats.BOLD) + ".";
		}
		private static string RedPandaDesc(bool plural)
		{
			return Utils.PluralizeIf("panda-arm", plural);
		}

		private static string RedPandaSingleDesc()
		{
			return "a panda-arm";
		}
		private static string RedPandaLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, furry, (color)
			return intro + Utils.PluralizeIf("panda-arm", plural);
		}
		private static string RedPandaFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string RedPandaPlayerStr(Arms arm, PlayerBase player)
		{
			return arm.epidermis.furTexture.ToString() + ", " + arm.epidermis.JustColor() + " fluff cover your arms. Your paws have " + arm.hands.ShortDescription() + ".";
		}
		private static string RedPandaTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return GenericPawTransformStr(previousArmData, "red-panda", true, false, new FurColor(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.NO_PATTERN));
		}
		private static string RedPandaRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FerretDesc(bool plural)
		{
			return Utils.PluralizeIf("ferret-arm", plural);
		}

		private static string FerretSingleDesc()
		{
			return "a ferret-arm";
		}
		private static string FerretLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, furry, (color)
			return intro + " " + Utils.PluralizeIf("ferret arm", plural);
		}
		private static string FerretFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}

		private static string FerretPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.JustColor() + " fluff covers your arms, turning into "
				+ arm.secondaryEpidermis.LongDescription() + " from elbows to paws."
				+ " Each arm ends in " + arm.hands.LongDescription(true, false);
		}
		private static string FerretTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return GenericPawTransformStr(previousArmData, "ferret", true, false);

		}
		private static string FerretRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string CatDesc(bool plural)
		{
			return Utils.PluralizeIf("feline arm", plural);
		}

		private static string CatSingleDesc()
		{
			return "a feline arm";
		}
		private static string CatLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, furry, (color)
			return intro + Utils.PluralizeIf("feline arm", plural);
		}
		private static string CatFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string CatPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string CatTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return GenericPawTransformStr(previousArmData, "cat", true, true);
		}
		private static string CatRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string DogDesc(bool plural)
		{
			return Utils.PluralizeIf("canine arm", plural);
		}

		private static string DogSingleDesc()
		{
			return "a canine arm";
		}

		private static string DogLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, furry, (color)
			return intro + Utils.PluralizeIf("canine arm", plural);
		}
		private static string DogFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string DogPlayerStr(Arms arm, PlayerBase player)
		{
			return "Soft, " + arm.epidermis.JustColor() + " fluff covers your arms. Your paw-like hands have " + arm.hands.LongDescription()
				+ ". With the right legs (and the right motivation), you could run with them, much like the hellounds you see in the mountains.";
		}
		private static string DogTransformStr(ArmData previousArmData, PlayerBase player)
		{
			if (previousArmData.type == ArmType.WOLF)
			{
				return "Your arms feel stiff, and despite any attempt to move them, they just sit there, limply. You soon realize the bones in your " + previousArmData.hands.ShortDescription() +
					" are changing, as well as the muscles on your arms. Strangely, your arms don't seem to change much, though they feel a bit weaker than before. " +
					"Stretching a little, you realize your paws are also much more dextrous than before, though your claws are much duller. " + SafelyFormattedString.FormattedText(" You now have dog-like arms!", StringFormats.BOLD);
			}
			else
			{
				return GenericPawTransformStr(previousArmData, "dog", true, false);
			}
		}
		private static string DogRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}
		private static string FoxDesc(bool plural)
		{
			return Utils.PluralizeIf("vulpine arm", plural);
		}

		private static string FoxSingleDesc()
		{
			return "a vulpine arm";
		}
		private static string FoxLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, furry, (color)
			return intro + arm.type.ShortDescription();
		}
		private static string FoxFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string FoxPlayerStr(Arms arm, PlayerBase player)
		{
			return CatFoxPlayerStr(arm, player);
		}
		private static string FoxTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return GenericPawTransformStr(previousArmData, "fox", true, true);
		}
		private static string FoxRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return PawRestoreString();
		}

		private static string GooDesc(bool plural)
		{
			return Utils.PluralizeIf("gooey arm", plural);
		}

		private static string GooSingleDesc()
		{
			return "a gooey arm";
		}
		private static string GooLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.LongAdjectiveDescription();
			}
			else
			{
				intro = arm.epidermis.LongAdjectiveDescription(alternateFormat);
			}
			//smooth, gooey, (color)
			return intro + " goo arm" + (plural ? "s with " : " with its ") + arm.hands.LongDescription(plural);
		}
		private static string GooFullDesc(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			return GenericFullDescription(arm, alternateFormat, plural, includeFingers);
		}
		private static string GooPlayerStr(Arms arm, PlayerBase player)
		{
			bool gooBody = player.body.type == BodyType.GOO;

			string introText = gooBody ? "Much like the rest of your body, your " : "Your ";

			return introText + "arms are made almost entirely of goo. With some effort, you can roughly control the shape and size of your arms, allowing you to access small areas with ease. " +
				"What you've gained in flexibility, however, is lost in overall rigidity, making it harder to lift or carry things. You also don't really have hands, par se, but you can " +
				"force your gooey appendages into something that roughly resembles a mitten to hold onto things.";
		}
		private static string GooTransformStr(ArmData previousArmData, PlayerBase player)
		{
			return "Your arms feel strange, causing you to rub them absentmindedly. All at once, the feeling stops. For a moment, you're glad the feeling has passed... until you realize that " +
				"you're STILL rubbing your arms, and you can't feel it! Even more frightening, your " + previousArmData.hands.ShortDescription() + " seem to be melting into your arms. " +
				"This same effect is happening all along your arms, as everything softens into a gelatin-like consistency. Your arms become transparent, and you notice your bones are dissolving " +
				"as well. With ever-increasing levels of horror, you realize " + SafelyFormattedString.FormattedText("your arms are turning into goo!", StringFormats.BOLD) +
				" You scratch your head, wondering how you'll use your arms now that they're in this state. Seeing as you still have arms, they must solid enough. Wait - " +
				"you just scratched your head? Curiousity replaces any lingering horror as you experiement with your new gooey extremities. With conscious effort, you can control the rough shape " +
				"they take, though anything too small or sturdy like fingers tends to fall apart. The best you can manage is a crude mitten-like shape, and you suppose that'll have to do. On the " +
				"plus side, you can fit your arm into much smaller spaces now.";
		}
		private static string GooRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return "The goo forming your arms feels strange - more solid, perhaps. Slowly, each arm becomes less and less transparent, until you can't see through " +
				"it all. You go to tap it with your gooey, roughly hand-shaped apendages, and stop short when you notice these appendages are also undergoing transformations. " +
				"The mitten-like shape you use to approximate hands splits, forming digits far too solid and slender for just goo. Soon, they too became less transparent. Knuckles form, " +
				"and a strange sensation covers your arms. Curiously, you pinch one of your arms, cursing silently - it hurts! With a start, you realize " +
				SafelyFormattedString.FormattedText("You have human arms and hands again!", StringFormats.BOLD);

		}


		private static string CatFoxPlayerStr(Arms arms, PlayerBase player)
		{
			return "Soft, " + arms.epidermis.JustColor() + " fluff covers your arms. Your paw-like hands have " + arms.hands.LongDescription() + ".";
		}

		private static string GenericLongDesc(ArmData arm, bool alternateFormat, bool plural)
		{
			string intro;
			if (alternateFormat && plural)
			{
				intro = "a pair of " + arm.epidermis.JustColor();
			}
			else
			{
				intro = arm.epidermis.JustColor(alternateFormat);
			}

			return intro + arm.ShortDescription(plural);
		}

		private static string PredatorPlayerStr(Arms arms, PlayerBase player)
		{
			return "Your arms are covered by " + arms.epidermis.ShortDescription() + " and your hands are now " + arms.hands.LongDescription() + ".";
		}

		private static string PredatorRestoreStr(ArmData previousArmData, PlayerBase player)
		{
			return GlobalStrings.NewParagraph() + "You feel a sudden tingle in your " + previousArmData.hands.ShortDescription() + " and then you realize,"
				+ " that they have become normal human fingernails again. Your arms quickly follow suit. "
				 + SafelyFormattedString.FormattedText("You have normal human arms again.", StringFormats.BOLD);
		}

		private static string GenericPawTransformStr(ArmData previousArmData, string newArmString, bool canClimb, bool canTearFlesh, FurColor optionalFurColor = null)
		{
			bool armChanged = false;
			StringBuilder sb = new StringBuilder("Your arms feel stiff, and despite any attempt to move them, they just sit there, limply." +
				" You soon realize the bones in your " + previousArmData.hands.ShortDescription() + " are changing, as well as the muscles on your arms.");
			if (!previousArmData.usesAnyFur)
			{
				armChanged = true;
				sb.Append(" A thick layer of fur quickly grows in, covering your arm from to the tips of your fingers.");
			}
			if (!previousArmData.hands.isPaws)
			{
				sb.Append(armChanged ? " Not to be outdone, your " : " Your ");
				if (previousArmData.hands.isHands)
				{
					sb.Append("hands gain pink, padded paws where your palms were once, and your nails become decent-sized curved claws.");
				}
				else if (previousArmData.hands.isClaws)
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
				 + Environment.NewLine + "Your hands follow suit, losing their pads and claws until your have a normal hand and fingernails."
				 + SafelyFormattedString.FormattedText("You have normal human arms again.", StringFormats.BOLD);
		}


		private static string GenericRestoreString()
		{
			return GlobalStrings.NewParagraph() + "Your unusual arms change more and more until they are normal human arms, leaving only skin behind." +
				SafelyFormattedString.FormattedText("You have normal human arms again.", StringFormats.BOLD);
		}

		private static string GenericFullDescription(ArmData arm, bool alternateFormat, bool plural, bool includeFingers)
		{
			string transition = plural || alternateFormat ? " ending in " : " with its ";

			return arm.type.LongDescription(arm, alternateFormat, plural) + transition + HandsOptionalFingers(arm.hands, includeFingers, plural);
		}
	}
}