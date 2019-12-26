//EyesStrings.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 3:16 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public static class EyeHelper
	{
		public static string AsString(this EyeColor eyeColor, bool withArticle = false)
		{
			switch (eyeColor)
			{
				case EyeColor.AMBER: return (withArticle ? "an " : "") + "amber";
				case EyeColor.BLUE: return (withArticle ? "a " : "") + "blue";
				case EyeColor.GRAY: return (withArticle ? "a " : "") + "gray";
				case EyeColor.GREEN: return (withArticle ? "a " : "") + "green";
				case EyeColor.HAZEL: return (withArticle ? "a " : "") + "hazel";
				case EyeColor.INDIGO: return (withArticle ? "a " : "") + "indigo";
				case EyeColor.ORANGE: return (withArticle ? "a " : "") + "orange";
				case EyeColor.PINK: return (withArticle ? "a " : "") + "pink";
				case EyeColor.RED: return (withArticle ? "a " : "") + "red";
				case EyeColor.TAN: return (withArticle ? "a " : "") + "tan";
				case EyeColor.VIOLET: return (withArticle ? "a " : "") + "violet";
				case EyeColor.YELLOW: return (withArticle ? "a " : "") + "yellow";
				case EyeColor.BROWN:
				default: return (withArticle ? "a " : "") + "brown";
			}
		}
	}

	public partial class Eyes
	{
		public static string Name()
		{
			return "Eyes";
		}
	}

	public partial class EyeType
	{
		private static string GenericCountText(byte eyeCount, bool withArticle)
		{
			if (eyeCount == 0) return "";
			else if (eyeCount == 1)
			{
				if (withArticle)
				{
					return Utils.RandomChoice("a single ", "one ", "a ", "a single ", "a ");
				}
				else
				{
					return Utils.RandomChoice("single ", "one ", "", "single ", "");
				}
			}
			else if (eyeCount == 2)
			{
				if (withArticle)
				{
					return Utils.RandomChoice("a pair of ", "two ", "a pair of ");
				}
				else
				{
					return Utils.RandomChoice("pair of ", "two ", "pair of ");
				}
			}
			else if (eyeCount % 2 == 1)
			{
				return Utils.NumberAsText(eyeCount) + " ";
			}
			else if (eyeCount < 10)
			{
				return Utils.NumberAsText(eyeCount / 2) + " pairs of ";
			}
			else
			{
				return eyeCount.ToString() + " ";
			}
		}

		public string ShortDescriptionWithColor(EyeColor leftIris, EyeColor rightIris)
		{
			if (eyeCount <= 1) return leftIris.AsString() + ShortDescription(false);
			else if (leftIris == rightIris) return leftIris.AsString() + " " + ShortDescription();
			else return "mismatched " + ShortDescription();
		}

		private static string HumanEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			return GenericEyeChange(oldLeft, newLeft, oldRight, newRight);
		}
		private static string HumanShortStr(bool plural)
		{
			return "human eye" + (plural ? "s" : "");
		}
		private static string HumanSingleDesc()
		{
			return "a human eye";
		}
		private static string HumanLongDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericLongDescription(eyes, alternateFormat);
		}
		private static string HumanFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string HumanSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			if (useLeftIrisColor) return eyes.leftIrisColor.AsString(alternateFormat) + " eye";
			else return eyes.rightIrisColor.AsString(alternateFormat) + " eye";
		}
		private static string HumanPlayerStr(Eyes eyes, PlayerBase player)
		{
			if (eyes.isHeterochromia)
			{
				return "Your eyes, while human, are heterochromatic, with the left a shade of " + eyes.leftIrisColor.AsString() + " and your right a shade of " + eyes.rightIrisColor.AsString() + ".";
			}
			else return "You have normal " + eyes.leftIrisColor.AsString() + " eyes.";
		}
		private static string HumanTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			return previousEyeData.type.RestoredString(previousEyeData, player);
		}
		//these eyes / these eyes have seen a lot of loves...
		//(holy shit i'm old.)
		private static string HumanRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousEyeData, player);
		}
		private static string SpiderEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder("Though difficult to see, after a close inspection, ");
			if (oldLeft != newLeft && oldRight != newRight)
			{
				sb.Append("the rings around your outer irises change to their new");

				if (newLeft != newRight)
				{
					if (oldLeft != oldRight)
					{
						sb.Append(", albeit still mismatched, ");
					}
					else
					{
						sb.Append(" mismatched ");
					}
				}
				sb.Append("colors");
			}
			else if (oldLeft != newLeft)
			{
				sb.Append("the ring of color around your left irises changes to ");
				sb.Append(newLeft == newRight ? "match your right ones. " : "their new color. ");
			}
			else //if (oldRight != newRight)
			{
				sb.Append("your right eyes change to ");
				sb.Append(newLeft == newRight ? "match your left ones. " : "their new color. ");
			}
			return sb.ToString();
		}
		private static string SpiderShortStr(bool plural)
		{
			return "spider eye" + (plural ? "s" : "");
		}
		private static string SpiderSingleDesc()
		{
			return "a spider eye";
		}
		//i suppose technically i could have supported tetrachromia, but that seems excessive.
		private static string SpiderLongDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericLongDescription(eyes, alternateFormat);
		}

		private static string SpiderSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return GenericOneEyeDescription(eyes, alternateFormat, useLeftIrisColor);
		}

		private static string SpiderFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string SpiderPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your eyes appear human - if your pupils were fully dialated all the time, and partially reflected the light. A faint ring of ");
			if (eyes.isHeterochromia)
			{
				sb.Append("color surround your eyes, ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append("around the left and ");
				sb.Append(eyes.rightIrisColor.AsString());
				sb.Append("around the right, which hint at their natural colors.");
			}
			else
			{
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append("surround both eyes, hinting at their natural color.");
			}
			return sb.ToString();
		}
		private static string SpiderTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			if (previousEyeData.eyeCount < 4)
			{
				return "Suddenly, the world darkens around you, and your eyes water. After blinking a few times, everything seems back to normal, but"
					+ " you have the strangest case of double vision. You stumble around, trying to blink it away again, but that only gives you a headache."
					+ " You rub your temples then move on to your face, but withdraw them when you accidently poke yourself in the eye. Wait, those fingers were on your forehead!"
					+ " You tentatively run your fingertips across your forehead, not quite believing what you felt. Sure enough, <b>There's now a pair of eyes on your forehead, "
					+ "positioned just above your normal ones!</b> This will take some getting used to!";
			}
			else
			{
				return "Your eyes shift, becoming more spiderlike. You now have <b>two pairs of spider-eyes, the second on your forehead just above the first.</b> This will take some getting used to!";
			}
		}
		private static string SpiderRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return "A dull ache hits your forehead near your second set of eyes. suddenly your eyesight goes out of focus and a stabbing pain "
				+ "hits as your facial structure changes. You quickly realize your spider eyes are gone! <b>You have normal eyes again!</b>";
		}
		private static string SandTrapEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			return "While you're certain the change took effect, it's impossible to tell with Sand Trap eyes. Perhaps such a change would be better suited for a different type of eyes?";
		}
		private static string SandTrapShortStr(bool plural)
		{
			return "sandtrap eye" + (plural ? "s" : "");
		}
		private static string SandTrapSingleDesc()
		{
			return "a sandtrap eye";
		}
		private static string SandTrapLongDesc(EyeData eyes, bool alternateFormat)
		{
			string countText = Utils.RandBool() ? (alternateFormat ? "a " : "") + "half dozen " : GenericCountText(eyes.eyeCount, alternateFormat);
			return countText + "solid, inky black eyes";
		}

		private static string SandTrapSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return (alternateFormat ? "an " : "") + "inky black sand-trap eye";
		}

		private static string SandTrapFullDesc(EyeData eyes, bool alternateFormat)
		{
			return SandTrapLongDesc(eyes, alternateFormat);
		}
		private static string SandTrapPlayerStr(Eyes eyes, PlayerBase player)
		{
			return " Your eyes are solid spheres of inky, alien darkness.";
		}
		private static string SandTrapTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("You blink, and then blink again. It feels like something is irritating your eyes."
				+ " Panic sets in as black suddenly blooms in the corner of your left eye and then your right, as if drops of ink were falling into them."
				+ " You go to rub your eyes, but quickly come to the conclusion that rubbing at them will certainly make whatever is happening to them worse. "
				+ " Still, it requires all of your willpower to hold your hands behind your back and wait for the strange affliction to run its course."
				+ " The strange inky substance pools over your entire vision before slowly fading, thankfully taking the irritation with it.");
			//the following should never proc, as it only lets these tfs happen if the pc has human eyes first, but w/e.
			if (previousEyeData.eyeCount > 2)
			{
				sb.Append("Despite the lack of irritation, you wonder if there may be some lingering effects, as it seems like you aren't getting a full image."
					+ " Then it hits you: <b> You've lost your extra eyes!</b>. When did that happen? Sensing even more changes, you ");

			}
			else if (previousEyeData.eyeCount < 2)
			{
				sb.Append("Despite the lack of irritation, you wonder if there may be some lingering effects, as it seems like you seeing double."
					+ " Then it hits you: <b> You have two eyes!</b>. When did that happen? Sensing even more changes, you ");
			}
			else
			{
				sb.Append("You ");
			}
			sb.Append("stride quickly over to the stream and stare at your reflection. <b>Your pupils, your irises, your entire eye has"
				+ " turned a liquid black</b>, leaving you looking vaguely like the many half insect creatures which inhabit these lands. You find you are merely grateful the "
				+ "change apparently hasn't affected your vision.");
			return sb.ToString();
		}
		private static string SandTrapRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return "Your eyes... itch? It's truly a strange feeling, though you suppose it's similar to shedding or molting, just... on your eyes."
				+ "Suddenly, the blackness covering your sclera falls off, leaving you with <b> normal eyes again!</b>";
		}
		private static string LizardEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			bool leftWasReptilian = lizardEyeColor(oldLeft);
			bool rightWasReptilian = lizardEyeColor(oldRight);
			bool leftIsReptilian = lizardEyeColor(newLeft);
			bool rightIsReptilian = lizardEyeColor(newRight);

			//undetectable change, either because only one eye changed, and the other is undetectable, or both eyes changed, and neither is detectable.
			if ((leftIsReptilian && rightIsReptilian && leftWasReptilian && rightWasReptilian) //both undetectable
				|| (oldLeft == newLeft && rightWasReptilian && rightIsReptilian) //left unchanged, right undetectable
				|| (oldRight == newRight && leftWasReptilian && leftIsReptilian)) //right unchanged, left undetectable
			{
				return "Curiously, your eyes seem unchanged. Perhaps such a change would be better suited for a different type of eyes?";
			}
			StringBuilder sb = new StringBuilder("At first you think your eye's haven't changed, but after further inspection you do notice they're slightly different. ");
			//One eye detectably changed, one not. This works by checking if either eye was undetectably changed. If both eyes were undetectable, it would have returned already.
			//we also check to see if one eye color was unchanged, as again, both being unchanged was already checked for.
			if (leftWasReptilian == leftIsReptilian || rightWasReptilian == rightIsReptilian || oldLeft == newLeft || oldRight == newRight)
			{
				EyeColor newColor, oldColor;
				string changeSide, otherSide;
				bool oldWasReptile, newIsReptile, otherReptilian, otherUnchanged;

				if (oldLeft == newLeft || leftWasReptilian == rightWasReptilian)
				{
					newColor = newRight;
					oldColor = oldRight;
					changeSide = "right";
					otherSide = "left";
					otherReptilian = lizardEyeColor(oldLeft);
					otherUnchanged = oldLeft == newLeft;
				}
				else
				{
					newColor = newLeft;
					oldColor = oldLeft;
					changeSide = "left";
					otherSide = "right";
					otherReptilian = lizardEyeColor(oldRight);
					otherUnchanged = oldRight == newRight;
				}
				oldWasReptile = lizardEyeColor(oldColor);
				newIsReptile = lizardEyeColor(newColor);

				if (oldWasReptile)
				{
					sb.Append("Small dots now appear in your ");
					sb.Append(changeSide);
					sb.Append("iris, just around your pupil, signalling the change in color. ");
					if (!otherUnchanged)
					{
						sb.Append("Strangely, though, you can't tell if your ");
						sb.Append(otherSide);
						sb.Append("iris changed ");
						sb.Append(newLeft == newRight ? "to match. " : "as well. ");
					}
					else if (newLeft == newRight)
					{
						sb.Append("Sure enough, you've lost your heterochromia. ");
					}
					else
					{
						sb.Append("Sure enough, you're now heterochromatic. ");
					}

				}
				else if (newIsReptile)
				{
					sb.Append("The dots of color in your ");
					sb.Append(changeSide);
					sb.Append("iris fade out, leaving the standard green-yellow lizard eyes. ");
					//edge case if heterochromia but both are reptilian colors. this is the only weird eye i've encountered requiring this as of current development.
					//obviously, if the other eye has dots and this one doesn't, it's heterochromatic. so check for that.
					if (newLeft != newRight && otherReptilian)
					{
						sb.Append("Curiously, you can't determine if you ");
						sb.Append(oldLeft == oldRight ? "now have " : "still have ");
						sb.Append("a heterochromia. Perhaps a different type of eyes would make this more clear? ");
					}
				}
				else
				{
					sb.Append("The dots in your ");
					sb.Append(changeSide);
					sb.Append("iris change to match their new color.");
					if (!otherUnchanged)
					{
						sb.Append("Strangely, though, you can't tell if your ");
						sb.Append(otherSide);
						sb.Append("iris changed as well. ");
					}
					else
					{
						sb.Append(newLeft == newRight ? "no longer " : "now");
						sb.Append("heterochromatic ");
					}
				}
			}
			//all that remains is that both eyes changed in a detectable manner.
			//If both lost their reptilian coloring
			else if (leftWasReptilian && rightWasReptilian)
			{
				sb.Append("Small dots now fill your irises in a ring around both your pupils, reflecting your new ");
				if (newLeft != newRight)
				{
					sb.Append("mismatched ");
				}
				sb.Append("colors. ");
			}
			//If they both gained their reptilian coloring
			else if (leftIsReptilian && rightIsReptilian)
			{
				sb.Append("The small dots of color surrounding both pupils fade out, leaving both irises a solid green-yellow hue.");
				if (newLeft != newRight)
				{
					sb.Append("Curiously, you can't determine if you " + (oldLeft == oldRight ? "now have " : "still have ") + "a heterochromia. Perhaps a different type of eyes would make this more clear?");
				}
				else if (oldLeft != oldRight)
				{
					sb.Append("Curiously, you can't determine if you " + (newLeft == newRight ? "no longer have " : "still have ") + "a heterochromia. Perhaps a different type of eyes would make this more clear?");
				}
			}
			//If one eye lost the reptilian colors, and the other gained them.
			else if (leftIsReptilian || rightIsReptilian)
			{
				string lostEye, gainedEye;
				if (leftIsReptilian)
				{
					lostEye = "left";
					gainedEye = "right";
				}
				else
				{
					lostEye = "right";
					gainedEye = "left";
				}
				sb.Append("Small dots now surround your ");
				sb.Append(gainedEye);
				sb.Append("pupil, reflecting its new color. Meanwhile, the dots surrounding your ");
				sb.Append(lostEye);
				sb.Append("pupil fade out, leaving its iris a solid green-yellow hue");
			}
			//If the both remained non-reptilian.
			else
			{
				sb.Append("The dots in your irises surrounding both your pupils shift, reflecting your new ");
				if (newLeft != newRight)
				{
					sb.Append("mismatched ");
				}
				sb.Append("colors. ");
			}
			return sb.ToString();
		}
		private static bool lizardEyeColor(EyeColor color)
		{
			return color == EyeColor.YELLOW || color == EyeColor.GREEN;
		}

		private static string LizardShortStr(bool plural)
		{
			return "lizard eye" + (plural ? "s" : "");
		}
		private static string LizardSingleDesc()
		{
			return "a lizard eye";
		}
		private static string LizardLongDesc(EyeData eyes, bool alternateFormat)
		{
			if (eyes.isHeterochromia)
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + "mismatched reptilian eyes";
			}
			else
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + eyes.leftIrisColor.AsString() + " reptilian eyes";
			}
		}


		private static string LizardSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return GenericOneEyeDescription(eyes, alternateFormat, useLeftIrisColor);
		}

		private static string LizardFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string LizardPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your eyes are those of a lizard with vertically slitted pupils and ");
			if (eyes.isHeterochromia)
			{
				sb.Append("mismatched irises. ");
				if (eyes.leftIrisColor == EyeColor.GREEN)
				{
					sb.Append("Your left appears green-yellow, while the right is ");
				}
				else if (eyes.leftIrisColor == EyeColor.YELLOW)
				{
					sb.Append("Your left appears yellow-green, while the right is ");
				}
				else
				{
					sb.Append("Your left appears green-yellow, though hints of ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(" surround your pupil. Meanwhile, the right is ");
				}
				//right eye.
				if (eyes.rightIrisColor == EyeColor.GREEN)
				{
					sb.Append("green-yellow. ");
				}
				else if (eyes.rightIrisColor == EyeColor.YELLOW)
				{
					sb.Append("yellow-green. ");
				}
				else
				{
					sb.Append("green-yellow, with hints of ");
					sb.Append(eyes.rightIrisColor.AsString());
					sb.Append(" near the pupils.");
				}
			}
			else
			{
				if (eyes.leftIrisColor == EyeColor.GREEN)
				{
					sb.Append("green-yellow irises.");
				}
				else if (eyes.leftIrisColor == EyeColor.YELLOW)
				{
					sb.Append("yellow-green irises.");
				}
				else
				{
					sb.Append("green-yellow irises, with hints of ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(" near the pupils");
				}
			}
			sb.Append(" They come with the typical second set of eyelids, allowing you to blink twice as much as others.");
			return sb.ToString();
		}
		private static string LizardTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			if (previousEyeData.type.isReptilianEyes)
				return "Your eyes change slightly in their appearance.\n<b>You now have lizard eyes.</b>";
			else
			{
				StringBuilder sb = new StringBuilder("You feel a sudden surge of pain in your eyes as they begin to reshape. "
					+ "Your pupils begin to elongate becoming vertically slitted and your irises change their color, too. ");
				if (previousEyeData.eyeCount > 2)
				{
					sb.Append("Your extra eyes disappear, leaving you partially blind until your brain adapts. ");
				}
				else if (previousEyeData.eyeCount < 2)
				{
					sb.Append("Your eye splits back into two, leaving you seeing double until your brain adjusts. ");
				}
				sb.AppendLine();
				sb.Append("As the pain passes, you examine your eyes in a nearby puddle. You look into your new eyes with vertically slitted pupils surrounded by green-yellowish irises");
				if ((previousEyeData.leftIrisColor != EyeColor.GREEN && previousEyeData.leftIrisColor != EyeColor.YELLOW) || (previousEyeData.rightIrisColor != EyeColor.GREEN && previousEyeData.rightIrisColor != EyeColor.YELLOW))
				{
					sb.Append(", though hints of your original color");
					sb.Append(previousEyeData.isHeterochromia ? "s remain" : " remains");
				}
				sb.AppendLine(". With a few tears remaining, the look is a bit blurry. Wanting to get a clearer look at them, you blink your remaining tears away and suddenly you realize"
					+ " that you just did that with your second set of eyelids.");
				sb.Append("<b>You now have lizard eyes.</b>");
				return sb.ToString();
			}
		}
		private static string LizardRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return ReptilianCatRestoreStr(previousEyeData, player);
		}
		private static string DragonEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			//no change.
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder("At first you think your eye's haven't changed, but after further inspection you do notice they're slightly different. ");
			//one change.
			if (oldLeft == newLeft || oldRight == newRight)
			{
				EyeColor oldColor, newColor;
				string thisEye, otherEye;
				if (oldLeft == newLeft)
				{
					oldColor = oldRight;
					newColor = newRight;
					thisEye = "right";
					otherEye = "left";
				}
				else
				{
					oldColor = oldLeft;
					newColor = newLeft;
					thisEye = "left";
					otherEye = "right";
				}
				if (oldColor == EyeColor.ORANGE)
				{
					sb.Append("Flecks of the new color appear in your ");
					sb.Append(thisEye);
					sb.Append(" iris, ");
					sb.Append(newLeft == newRight ? "matching your" : "contrasting with your");
					sb.Append(otherEye);
					sb.Append(" one. ");
				}
				else if (newColor == EyeColor.ORANGE)
				{
					sb.Append("The flecks of color in your ");
					sb.Append(thisEye);
					sb.Append(" iris disappear, leaving a solid shade of vibrant orange in their wake, ");
					if (newLeft != newRight)
					{
						sb.Append("a sharp contrast to the multicolored one on your ");
					}
					else
					{
						sb.Append("now matching your ");
					}
					sb.Append(otherEye);
					sb.Append(". ");
				}
				else
				{
					sb.Append("The flecks of color in your ");
					sb.Append(thisEye);
					sb.Append(" iris change to their new color, ");
					sb.Append(newLeft == newRight ? "matching " : "contrasting with ");
					sb.Append("that on your ");
					sb.Append(otherEye);
					sb.Append(". ");
				}
			}

			//both change (from orange)
			else if (oldLeft == oldRight && oldLeft == EyeColor.ORANGE)
			{
				sb.Append("Flecks of color appear in both eyes, showing their new");
				if (newLeft != newRight)
				{
					sb.Append(", mismatched");
				}
				sb.Append(" colors. ");
			}
			//both change (to orange).
			else if (newLeft == EyeColor.ORANGE && newRight == EyeColor.ORANGE)
			{
				sb.Append("The flecks of color in both irises disappear, leaving only a ");
				sb.Append(oldLeft == oldRight ? "solid " : "matching ");
				sb.Append(" shade of vibrant orange.");
			}
			//one to orange, one from orange.
			else if (newLeft == EyeColor.ORANGE || newRight == EyeColor.ORANGE)
			{
				string lostEye, gainedEye;
				if (newLeft == EyeColor.ORANGE)
				{
					lostEye = "left";
					gainedEye = "right";
				}
				else
				{
					lostEye = "right";
					gainedEye = "left";
				}
				sb.Append("Small dots now surround your ");
				sb.Append(gainedEye);
				sb.Append("pupil, reflecting its new color. Meanwhile, the dots surrounding your ");
				sb.Append(lostEye);
				sb.Append("pupil fade out, leaving only a solid shade of vibrant orange");
			}
			//both change (and remain not orange)
			else
			{
				sb.Append("The flecks of color in both your irises change, reflecting their new");
				sb.Append(newLeft == newRight ? " color. " : ", mismatched colors. ");
			}
			return sb.ToString();
		}
		private static string DragonShortStr(bool plural)
		{
			return "dragon eye" + (plural ? "s" : "");
		}
		private static string DragonSingleDesc()
		{
			return "a dragon eye";
		}
		private static string DragonLongDesc(EyeData eyes, bool alternateFormat)
		{
			if (eyes.isHeterochromia)
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + "mismatched draconic eyes";
			}
			else
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + eyes.leftIrisColor.AsString() + " draconic eyes";
			}
		}
		private static string DragonSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return GenericOneEyeDescription(eyes, alternateFormat, useLeftIrisColor);
		}

		private static string DragonFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string DragonPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your eyes are prideful, fierce dragon eyes with vertically slitted pupils and burning orange irises.");
			if (eyes.isHeterochromia)
			{
				if (eyes.leftIrisColor == EyeColor.ORANGE)
				{
					sb.Append("Flecks of ");
					sb.Append(eyes.rightIrisColor.AsString());
					sb.Append(" dot your right iris, adding a unique flair. ");
				}
				else if (eyes.rightIrisColor == EyeColor.ORANGE)
				{
					sb.Append("Flecks of ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(" dot your left iris, adding a unique flair. ");
				}
				else
				{
					sb.Append("Flecks of ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(" and ");
					sb.Append(eyes.rightIrisColor.AsString());
					sb.Append(" dot your left and right eyes, respectively, adding a unique flair.");
				}
			}
			else if (eyes.leftIrisColor != EyeColor.ORANGE)
			{
				sb.Append("Flecks of ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append(" dot your irises, adding a unique flair. ");
			}
			sb.Append("They glitter even in the darkness and they come with the typical second set of eyelids, allowing you to blink twice as much as others. ");
			return sb.ToString();
		}
		private static string DragonTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (previousEyeData.type.isReptilianEyes)
				sb.Append("Your eyes change slightly in their appearance.");
			else
			{
				sb.AppendLine("You feel a sudden surge of pain in your eyes as they begin to reshape. Your pupils begin to elongate becoming vertically slitted and your irises change their color, too.");
				if (previousEyeData.eyeCount > 2)
				{
					sb.Append("Your extra eyes disappear, leaving you partially blind until your brain adapts. ");
				}
				else if (previousEyeData.eyeCount < 2)
				{
					sb.Append("Your eye splits back into two, leaving you seeing double until your brain adjusts. ");
				}
				sb.Append(" As the pain passes, you examine your eyes in a nearby puddle. You look into your new prideful, fierce dragon eyes with vertically slitted pupils and burning orange irises");
				if (previousEyeData.isHeterochromia || previousEyeData.leftIrisColor != EyeColor.ORANGE)
				{
					sb.Append("though flecks of your original ");
					sb.Append(previousEyeData.isHeterochromia ? "colors remain" : "color remains");
				}
				sb.Append(". They glitter even in the darkness. With a few tears remaining, the look is a bit blurry. Wanting to get a clearer look at them, you blink your remaining tears away and suddenly you realize, that you just did that with your second set of eyelids.");
			}
			sb.Append("<b>You now have fierce dragon eyes.</b>");
			return sb.ToString();
		}
		private static string DragonRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return ReptilianCatRestoreStr(previousEyeData, player);
		}
		private static string BasiliskEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			//no change.
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder("At first you think your eye's haven't changed, but after further inspection you do notice they're slightly different. ");
			//one change.
			if (oldLeft == newLeft || oldRight == newRight)
			{
				EyeColor oldColor, newColor;
				string thisEye, otherEye;
				if (oldLeft == newLeft)
				{
					oldColor = oldRight;
					newColor = newRight;
					thisEye = "right";
					otherEye = "left";
				}
				else
				{
					oldColor = oldLeft;
					newColor = newLeft;
					thisEye = "left";
					otherEye = "right";
				}
				if (oldColor == EyeColor.GRAY)
				{
					sb.Append("Hints of the new color swirl around in your ");
					sb.Append(thisEye);
					sb.Append(" iris, ");
					sb.Append(newLeft == newRight ? "matching your" : "contrasting with your");
					sb.Append(otherEye);
					sb.Append(" one. ");
				}
				else if (newColor == EyeColor.GRAY)
				{
					sb.Append("Try as you might, you can no longer find any hints of color in your ");
					sb.Append(thisEye);
					sb.Append(" iris, leaving it a swirling mix of dull grays, ");
					sb.Append(newLeft == newRight ? "matching your" : "contrasting with your");
					sb.Append(otherEye);
					sb.Append(". ");
				}
				else
				{
					sb.Append("The hints of color swirling about your ");
					sb.Append(thisEye);
					sb.Append(" iris change to their new color, ");
					sb.Append(newLeft == newRight ? "matching " : "contrasting with ");
					sb.Append("that on your ");
					sb.Append(otherEye);
					sb.Append(". ");
				}
			}

			//both change (from GRAY)
			else if (oldLeft == oldRight && oldLeft == EyeColor.GRAY)
			{
				sb.Append("Hints of color appear in both irises, showing their new");
				if (newLeft != newRight)
				{
					sb.Append(", mismatched");
				}
				sb.Append(" colors. ");
			}
			//both change (to GRAY).
			else if (newLeft == EyeColor.GRAY && newRight == EyeColor.GRAY)
			{
				sb.Append("Try as you might, you can't find any hints of additional colors swirling in either eye, leaving only ");
				sb.Append(oldLeft == oldRight ? " a shade of dull gray. " : "matching shades of dull gray. ");
			}
			//one to GRAY, one from GRAY.
			else if (newLeft == EyeColor.GRAY || newRight == EyeColor.GRAY)
			{
				string lostEye, gainedEye;
				bool colorAfterShifting;
				if (newLeft == EyeColor.GRAY)
				{
					lostEye = "left";
					gainedEye = "right";
					colorAfterShifting = oldLeft != newRight;
				}
				else
				{
					lostEye = "right";
					gainedEye = "left";
					colorAfterShifting = oldRight != newLeft;
				}
				sb.Append("In a strange turn of events, you watch as the swirling color in your ");
				sb.Append(lostEye);
				sb.Append(" iris shifts to your ");
				sb.Append(gainedEye);
				sb.Append(" one");
				if (!colorAfterShifting)
				{
					sb.Append(", then shifts to its new color");
				}
				sb.Append(". ");
			}
			//both change (and remain not GRAY)
			else
			{
				sb.Append("The swirling hints of color in both your irises change, reflecting their new");
				sb.Append(newLeft == newRight ? " color. " : ", mismatched colors. ");
			}
			sb.Append("Despite this, you have no doubt your stare remain just as mesmerizing. ");
			return sb.ToString();
		}
		private static string BasiliskShortStr(bool plural)
		{
			return "basilisk eye" + (plural ? "s" : "");
		}
		private static string BasiliskSingleDesc()
		{
			return "a basilisk eye";
		}
		private static string BasiliskLongDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericCountText(eyes.eyeCount, alternateFormat) + "dazzling" + (eyes.isHeterochromia ? " but mismatched " : eyes.leftIrisColor.AsString()) + eyes.type.ShortDescription();
		}
		private static string BasiliskSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return (alternateFormat ? "a " : "") + "dazzling " + GenericOneEyeDescription(eyes, false, useLeftIrisColor);
		}

		private static string BasiliskFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string BasiliskPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your eyes are those of a basilisk, dull grey reptilian pools with vertically slitted pupils");
			if (eyes.isHeterochromia)
			{
				if (eyes.leftIrisColor == EyeColor.GRAY)
				{
					sb.Append(". Strangely, your right eye has the slightest hint of ");
					sb.Append(eyes.rightIrisColor.AsString());
					sb.Append(" to it. ");
				}
				else if (eyes.rightIrisColor == EyeColor.GRAY)
				{
					sb.Append(". Strangely, your left eye has the slightest hint of ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(" to it. ");
				}
				else
				{
					sb.Append(", though each eye has a slight hint of color to it. The left appears slightly ");
					sb.Append(eyes.leftIrisColor.AsString());
					sb.Append(", while the right appears slightly ");
					sb.Append(eyes.rightIrisColor.AsString());
					sb.Append(". ");
				}
			}
			else if (eyes.leftIrisColor != EyeColor.GRAY)
			{
				sb.Append(", and only the slightest hint of ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append("around your irises. ");
			}
			else
			{
				sb.Append(". ");
			}
			sb.Append("They come with the typical second set of eyelids, allowing you to blink twice as much as others. Others seem compelled to look into them.");
			return sb.ToString();
		}

		//unless some weird shit happens (some sort of randomizer item), this should never be used.
		//the only way to get these is via Benoit, and that is an edge case that should implement its own transform text.
		private static string BasiliskTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder();
			if (previousEyeData.type.isReptilianEyes)
				sb.Append("Your eyes change slightly in their appearance. ");
			else
			{
				sb.Append("You feel a sudden surge of pain in your eyes as they begin to reshape. ");
				if (previousEyeData.eyeCount > 2)
				{
					sb.Append("Your extra eyes disappear, leaving you partially blind until your brain adapts. ");
				}
				else if (previousEyeData.eyeCount < 2)
				{
					sb.Append("Your eye splits back into two, leaving you seeing double until your brain adjusts. ");
				}
				sb.AppendLine("Your pupils begin to elongate becoming vertically slitted and your irises change their color, too.");
				sb.Append(" As the pain passes, you examine your eyes in a nearby puddle. You look into your new eyes, and have to pull yourself away from their gaze. Your irises are a swirling pool of gray");
				if (previousEyeData.isHeterochromia || previousEyeData.leftIrisColor != EyeColor.GRAY)
				{
					sb.Append("with the occasional swirl of your original");
					if (previousEyeData.isHeterochromia)
					{
						sb.Append(" mismatched ");
						sb.Append(previousEyeData.leftIrisColor.AsString());
						sb.Append(" and ");
						sb.Append(previousEyeData.rightIrisColor.AsString());
						sb.Append(" mixed in");
					}
					else
					{
						sb.Append(previousEyeData.leftIrisColor.AsString());
						sb.Append(" mixed in");
					}
				}
				sb.Append(". You catch yourself staring at them in your reflection again, and blink rapidly to break the charm. At this point, you realize you have a second layer or eyelids. ");
			}
			sb.Append("<b>You now have mesmerizing basilisk eyes.</b>");
			return sb.ToString();
		}
		private static string BasiliskRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return ReptilianCatRestoreStr(previousEyeData, player) + " You're gonna miss the ability to turn things to stone with a glare, though.";
		}
		private static string WolfEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			return GenericEyeChange(oldLeft, newLeft, oldRight, newRight);
		}
		private static string WolfShortStr(bool plural)
		{
			return "wolven eye" + (plural ? "s" : "");
		}
		private static string WolfSingleDesc()
		{
			return "a wolven eye";
		}
		private static string WolfLongDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericLongDescription(eyes, alternateFormat);
		}
		//most of these i've kept the color in the vanilla code, and adapted it to use the iris color.
		//for wolf, i've just replaced "amber" with the right color. it seemed right to me, idk. if you wish to
		//make it like the others, go for it.
		private static string WolfSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return GenericOneEyeDescription(eyes, alternateFormat, useLeftIrisColor);
		}

		private static string WolfFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string WolfPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your ");
			if (eyes.isHeterochromia)
			{
				sb.Append("heterochromatic ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append(" and ");
				sb.Append(eyes.rightIrisColor.AsString());

			}
			else
			{
				sb.Append(eyes.leftIrisColor.AsString());
			}
			sb.Append(" eyes are circled by darkness to help keep the sun from obscuring your view and have a second eyelid to keep them wet. You're rather near-sighted, but your peripherals are great!");
			return sb.ToString();
		}
		private static string WolfTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("You feel a sudden surge of pain in your face as your eye");
			if (previousEyeData.eyeCount < 2)
			{
				sb.Append(" begins to change, at first splitting into two. You close your new set of eyes ");
			}
			else if (previousEyeData.eyeCount > 2)
			{
				sb.Append("s begin to change, as you lose your extra eyes. You close your remaining eyes ");
			}
			else
			{
				sb.Append("s begin to change. You close them ");
			}
			sb.Append("and feel something wet slide under your eyelids. You jump in surprise. "
				+ "The feeling's gone, but now the distance is a blurred view, and greens seem to be mixed with yellows."
				+ System.Environment.NewLine + System.Environment.NewLine
				+ "You turn to a nearby reflective surface to investigate. Your eyes have massive irises and are dipped into your face, hiding any sign of your sclera. "
				+ "Blackness surrounds them and emphasise the wolfish shape of your face. You blink a few times as you stare at your reflection. <b>You now have wolf eyes!</b> "
				+ "Your peripherals and night vision has probably improved, too.");
			return sb.ToString();
		}
		private static string WolfRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return "Your eyes feel strangely in focus, like you just put on a pair of glasses. Then it hits you: <b>You have normal eyes again!</b> You are gonna miss the night vision, but you'll " +
				"certainly take being able to distinguish yellows and greens again.";
		}
		private static string CockatriceEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			//no change.
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder("Despite the fact both eyes remain a dazzling blue, it'd be hard to miss the changes that have occurred. ");
			//one change.
			if (oldLeft == newLeft || oldRight == newRight)
			{
				sb.Append("The spiderweb of color that run through your");
				sb.Append(oldLeft == newLeft ? "right" : "left");
				sb.Append(" iris change to their new color in a dramatic fashion, flickering from top to bottom like little lightning strickes. ");
			}
			else
			{
				sb.Append("The spiderweb of color that run through both irises change to their new");
				sb.Append(newLeft == newRight ? " color " : "respective colors ");
				sb.Append("in dramatic fashion, flickering from top to bottom like little lightning strickes.");
			}
			sb.Append("Despite the change, your eyes feel just as powerful. ");
			return sb.ToString();
		}
		private static string CockatriceShortStr(bool plural)
		{
			return "cockatrice eye" + (plural ? "s" : "");
		}
		private static string CockatriceSingleDesc()
		{
			return "a cockatrice eye";
		}
		private static string CockatriceLongDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericCountText(eyes.eyeCount, alternateFormat) + "dazzling" + (eyes.isHeterochromia ? " but mismatched " : eyes.leftIrisColor.AsString()) + eyes.type.ShortDescription();
		}

		private static string CockatriceSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return (alternateFormat ? "a " : "") + "dazzling " + GenericOneEyeDescription(eyes, false, useLeftIrisColor);
		}

		private static string CockatriceFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string CockatricePlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("Your eyes have the slit-shaped pupils of a reptile, but your irises are an electric blue. Lightning-like streaks");
			if (eyes.isHeterochromia)
			{
				sb.Append(", ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append(" in the left, ");
				sb.Append(eyes.rightIrisColor.AsString());
				sb.Append(" in the right, ");
			}
			else if (eyes.leftIrisColor != EyeColor.BLUE)
			{
				sb.Append(" of ");
				sb.Append(eyes.leftIrisColor.AsString());
			}
			sb.Append(" spiderweb through your irises, betraying their power. When excited your pupils dilate into wide circles.");
			return sb.ToString();
		}
		private static string CockatriceTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder("Your eyes suddenly burn, tears streaming down your cheeks. Curious as to what's happening, "
				+ "you quickly find a source of water and watch the changes unfold.");
			if (previousEyeData.eyeCount != 2)
			{
				sb.Append("A blazing headache forces your eyes shut, and when you open them again, you notice you only have two of them. "
					+ "Though your brain isn't completely adjusted yet, you manage to catch the rest of the changes. ");
			}
			sb.Append("Your irises shift color to a dazzling vibrant blue, and then grow to take up their respective eyes. "
				+ " Each eye retains a bit of its original color, though, as streaks of it cover your irises like streaks of lightning. Your pupils rapidly grow to"
				+ " match, elongating into slit like shapes, similar to that of a feline. When your eyes stop watering you finally get a"
				+ " look at yourself. Your eyes are now the same as that of the cockatrice you met in the mountains! Your excitement over"
				+ " this causes your pupils to widen into large circles, giving you a cute and excited look. Seems you won’t be able to have"
				+ " much of a poker face anymore." + System.Environment.NewLine + "<b>You now have cockatrice eyes!</b>");
			return sb.ToString();
		}
		private static string CockatriceRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return "Your eyes feel less powerful, and you have a sinking feeling you know the cause. Finding a source of water nearby, you watch, mesmerized, as your cockatrice eyes shift back to normal."
				+ "The little lines in your irises expand outward, overtaking the rapidly dwindling blue that defines a cockatrice's eyes. At the same time, your irises shrink down to a more human size, "
				+ "and soon nothing is left of the dazzling blue. Your pupils also shrink and round out, leaving no doubt <b>You once again have human eyes!</b> It's a shame you had to lose their power,"
				+ "but it appears you can't have one without the other.";
		}
		private static string CatEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			return GenericEyeChange(oldLeft, newLeft, oldRight, newRight);
		}
		private static string CatShortStr(bool plural)
		{
			return "cat eye" + (plural ? "s" : "");
		}
		private static string CatSingleDesc()
		{
			return "a cat eye";
		}
		private static string CatLongDesc(EyeData eyes, bool alternateFormat)
		{
			if (eyes.isHeterochromia)
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + "mismatched feline eyes";
			}
			else
			{
				return GenericCountText(eyes.eyeCount, alternateFormat) + eyes.leftIrisColor.AsString() + " feline eyes";
			}
		}


		private static string CatSingleDesc(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			return GenericOneEyeDescription(eyes, alternateFormat, useLeftIrisColor);
		}

		private static string CatFullDesc(EyeData eyes, bool alternateFormat)
		{
			return GenericFullDescription(eyes, alternateFormat);
		}
		private static string CatPlayerStr(Eyes eyes, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(" Your eyes are similar to those of a cat, with slit pupils and ");
			if (eyes.isHeterochromia)
			{
				sb.Append("mismatched irises - the left ");
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append(" and the right ");
				sb.Append(eyes.rightIrisColor.AsString());
				sb.Append(".");
			}
			else
			{
				sb.Append(eyes.leftIrisColor.AsString());
				sb.Append(" irises.");
			}
			return sb.ToString();
		}
		private static string CatTransformStr(EyeData previousEyeData, PlayerBase player)
		{
			return "For a moment your sight shifts as the ambient light suddenly turns extremely bright, almost blinding you."
				+ " You walk around disoriented until the luminosity fades back to normal."
				+ " You run to a puddle of water to check your reflection."
				+ (previousEyeData.eyeCount != 2 ? " You seem to have regained the normal single pair of eyes, and what's more, your " : " Your ")
				+ "pupils have become cat-like." + System.Environment.NewLine + "<b>You now have cat-eyes!</b>";
		}
		private static string CatRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return ReptilianCatRestoreStr(previousEyeData, player);
		}

		private static string ReptilianCatRestoreStr(EyeData previousEyeData, PlayerBase player)
		{
			return "Your pupils shorten and widen, becoming round again. Your irises return to their original color, and shrink down to a human size. Where they used to be is a familiar white sclera."
				+ " <b>You have human eyes again!</b>";
		}

		private static string GenericEyeChange(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight)
		{
			if (oldLeft == newLeft && oldRight == newRight)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder("Sure enough, ");
			if (oldLeft != newLeft && oldRight != newRight)
			{
				sb.Append("both eyes change to their new");

				if (newLeft != newRight)
				{
					if (oldLeft != oldRight)
					{
						sb.Append(", albeit still mismatched, ");
					}
					else
					{
						sb.Append(" mismatched ");
					}
				}
				sb.Append("colors");
			}
			else if (oldLeft != newLeft)
			{
				sb.Append("your left eye changes to ");
				sb.Append(newLeft == newRight ? "match your right." : "its new color. ");
			}
			else //if (oldRight != newRight)
			{
				sb.Append("your right eye changes to ");
				sb.Append(newLeft == newRight ? "match your left." : "its new color. ");
			}
			return sb.ToString();
		}

		private static string GenericLongDescription(EyeData eyes, bool alternateFormat)
		{
			return GenericCountText(eyes.eyeCount, alternateFormat) + eyes.type.ShortDescriptionWithColor(eyes.leftIrisColor, eyes.rightIrisColor);
		}

		private static string GenericOneEyeDescription(EyeData eyes, bool alternateFormat, bool useLeftIrisColor)
		{
			if (useLeftIrisColor) return eyes.leftIrisColor.AsString(alternateFormat) + eyes.ShortDescription(false);
			else return eyes.rightIrisColor.AsString(alternateFormat) + eyes.ShortDescription(false);
		}

		private static string GenericFullDescription(EyeData eyes, bool alternateFormat)
		{
			if (eyes.eyeCount == 2 && eyes.isHeterochromia)
			{
				return eyes.LongDescription(alternateFormat) + " - the left " + eyes.leftIrisColor.AsString() + ", the right " + eyes.rightIrisColor.AsString();
			}
			else if (eyes.eyeCount > 2 && eyes.isHeterochromia)
			{
				return eyes.LongDescription(alternateFormat) + " - the left ones" + eyes.leftIrisColor.AsString() + ", the right ones" + eyes.rightIrisColor.AsString();
			}
			else return eyes.LongDescription(alternateFormat);
		}
	}

}
