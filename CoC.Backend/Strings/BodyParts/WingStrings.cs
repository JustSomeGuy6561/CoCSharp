//WingStrings.cs
//Description:
//Author: JustSomeGuy
//1/6/2019,A 10:27 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public partial class Wings
	{
		public static string Name()
		{
			return "Wings";
		}
	}
	public partial class WingType
	{
		private static string Wing2Text()
		{
			return "Wings 2";
		}
		private static string YourBoneDesc(out bool isPlural)
		{
			isPlural = true;
			return " your wings' bones";
		}
		private static string WingText()
		{
			return "Wings";
		}
		private static string WingDesc(bool isLotion, out bool isPlural)
		{
			if (isLotion)
			{
				isPlural = true;
				return " your wings' membranes";
			}
			else
			{
				isPlural = true;
				return " your wings";
			}
		}

		private string WingsDyeText(HairFurColors color)
		{
			return "";
		}

		private string WingsToneText(Wings wings, byte index)
		{
			return "";
		}

		protected string TonablePostToneText(Wings wings, byte index)
		{
			return $"{wings.wingTone.AsString()} membranes and {wings.wingBoneTone.AsString()} bones";
		}

		private static string NoneDesc(bool plural)
		{
			return "";
		}

		private static string NoneSingleDesc()
		{
			return "";
		}

		private static string NoneLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			return "";
		}
		private static string NonePlayerStr(Wings wings, PlayerBase player)
		{
			return "";
		}
		private static string NoneTransformStr(WingData previousWingData, PlayerBase player)
		{
			return previousWingData.type.RestoredString(previousWingData, player);
		}
		private static string NoneRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousWingData, player);
		}
		private static string BeeLikeDesc(bool? isLarge, bool plural = true)
		{
			string size = "";
			if (isLarge is bool large)
			{
				size = large ? "large, " : "tiny, ";
			}
			return size + "bee-like wing" + (plural ? "s" : "");
		}

		private static string BeeLikeSingleDesc(bool? isLarge)
		{
			string size = "";
			if (isLarge is bool large)
			{
				size = large ? "large, " : "tiny, ";
			}
			return "a " + size + "bee-like wing";
		}

		private static string BeeLikeLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + BeeLikeDesc(wings.isLarge);
		}
		private static string BeeLikePlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large bee-wings sprout from your back, reflecting the light through their clear membranes beautifully. They flap quickly, " +
					"allowing you to easily hover in place or fly.";
			}
			else
			{
				return " A pair of tiny-yet-beautiful bee-wings sprout from your back, too small to allow you to fly.";
			}
		}

		private static string BeeChangeSizeText(bool grewLarge, PlayerBase player)
		{
			if (grewLarge)
			{
				return "Your wings tingle as they grow, filling out until they are large enough to lift you from the ground and allow you to fly! " +
					SafelyFormattedString.FormattedText("You now have large bee wings!", StringFormats.BOLD) + " You give a few experimental flaps and begin hovering in place, " +
					"a giddy smile plastered on your face by the thrill of flight.";
			}
			else
			{
				return "Your wings tingle as they shrink, growing smaller until they are no longer large enough to lift you from the ground. " +
					SafelyFormattedString.FormattedText("You now have small bee wings!", StringFormats.BOLD) + " You sigh, already missing the power of flight.";
			}
		}

		private static string BeeLikeTransformStr(WingData previousWingData, PlayerBase player)
		{
			if (previousWingData.type == WingType.NONE)
			{
				string sizeText = player.wings.isLarge
					? " As you do, you briefly lift off the ground. It seems like they are large enough to fly with! "
					: " Unfortunately you can't seem to flap your little wings fast enough to fly, but they would certainly slow a fall. ";

				string armorText = player.wearingArmor
					? "You hastily remove the top portion of your " + player.armor.shortName() + " and marvel as "
					: "Checking your exposed back, you can't help but marvel as ";

				string modifyText = player.wearingArmor
					? "A few quick modifications to your " + player.armor.shortName() + " later and"
					: "Once you figure out how to handle your wings when you aren't using them,";

				return "You feel an itching between your shoulder-blades as something begins growing there."
					+ " You twist and contort yourself, trying to scratch and bring yourself relief, and failing miserably."
					+ " A sense of relief erupts from you as you feel something new grow out from yourbody. " + armorText + player.wings.LongDescription()
					+ " sprout from your back. Tenderly flexing your new muscles, you find you can flap them quite fast. " + sizeText + modifyText
					+ " you are ready to continue your journey with " + SafelyFormattedString.FormattedText("your new bee wings!", StringFormats.BOLD);
			}
			else
			{
				string sizeText = player.wings.isLarge
					? " A quick flap of your now bee-like wings confirms they are large enough to fly with!"
					: " A quick flap of your now bee-like wings confirms they are too small to fly with, unfortunately.";

				return "you feel a strange substance suddenly coating your wings. Bringing a " + player.hands.ShortDescription(false) + " to them, you notice it's sticky, " +
					"and you're pretty sure it's honey. Before you have time to confirm your suspicion, your " + previousWingData.ShortDescription() + "fully convert into the " +
					"honey-like substance and fall off your back in a giant, goopy mess. Some of the substance remains on you, where your old, " + previousWingData.LongDescription()
					+ " met your back. You go to wipe it off when you notice " + player.wings.LongDescription(true, true) + " sprouting there. " +
					SafelyFormattedString.FormattedText("You now have a pair of bee wings!", StringFormats.BOLD) + sizeText;
			}
		}
		private static string BeeLikeRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}

		protected string FeatheredWingsPostDyeText(HairFurColors color)
		{
			return $"{color.AsString()}-tinted feathers on your wings";
		}

		private static string FeatheredDesc(bool? isLarge, bool plural)
		{
			if (isLarge is bool large)
			{
				return large ? "large feathered wing" + (plural ? "s" : "") : "feathered harpy wing" + (plural ? "s" : "");
			}
			else
			{
				return "feathered wing" + (plural ? "s" : "");
			}
		}

		private static string FeatheredSingleDesc(bool? isLarge)
		{
			if (isLarge == true)
			{
				return "a large feathered wing";
			}
			else if (isLarge == false)
			{
				return "a feathered harpy wing";
			}
			else
			{
				return "a feathered wing";
			}
		}

		private static string FeatheredLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro;
			if (wings.isLarge)
			{
				if (alternateFormat && plural)
				{
					intro = "a pair of ";
				}
				else if (alternateFormat)
				{
					intro = "a ";
				}
				else if (plural)
				{
					intro = "pair of ";
				}
				else
				{
					intro = "";
				}
				return intro + Utils.PluralizeIf("large wing", plural) + " covered in " + wings.featherColor.AsString() + " feathers";

			}
			else
			{
				if (alternateFormat && plural)
				{
					intro = "a pair of " + wings.featherColor.AsString();
				}
				else if (alternateFormat)
				{
					intro = Utils.AddArticle(wings.featherColor.AsString());
				}
				else if (plural)
				{
					intro = "pair of " + wings.featherColor.AsString();
				}
				else
				{
					intro = wings.featherColor.AsString();
				}
				return intro + Utils.PluralizeIf("harpy wing", plural);
			}
		}
		private static string FeatheredPlayerStr(Wings wings, PlayerBase player)
		{
			return " A pair of large, feathery wings sprout from your back. Though you usually keep the " + wings.wingTone.AsString() + "-colored wings folded close, " +
				"they can unfurl to allow you to soar as gracefully as a harpy.";
		}

		private static string FeatheredChangeSizeText(bool grewLarge, PlayerBase player)
		{
			return "Through means not yet implemented, you've managed to change the size of your feathered wings. Current implementation limits feathered wings to large, " +
				"as small wings are reserved for enemy harpies. Congratulations! Contact a dev.";
		}

		private static string FeatheredTransformStr(WingData previousWingData, PlayerBase player)
		{
			string intro = "Pain ";
			//someone took care of other wing types so i didn't have to do it manually! WOOOO!!!
			if (previousWingData.type != NONE)
			{
				intro = $"Sensation fades from your {previousWingData.LongDescription()} slowly but surely, leaving them dried out husks that break off to fall on the"
					+ " ground. Your back closes up to conceal the loss, as smooth and unbroken as any normal human's. Before you can process the loss, pain ";
			}

			return intro + "lances through your back, the muscles knotting oddly and pressing up to bulge your skin. It hurts, oh gods does"
				 + " it hurt, but you can't get a good angle to feel at the source of your agony. A loud crack splits the air, and then your"
				 + " body is forcing a pair of narrow limbs through a gap in your comfortable clothes. Blood pumps through the new"
				 + " appendages, easing the pain as they fill out and grow. Tentatively, you find yourself flexing muscles you didn't know"
				 + " you had, and " + SafelyFormattedString.FormattedText("you're able to curve the new growths far enough around to behold your brand new, "
					+ player.wings.featherColor.AsString() + " wings", StringFormats.BOLD);
		}
		private static string FeatheredRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}
		private static string BatLikeDesc(bool? isLarge, bool plural)
		{
			string sizeStr = "";
			if (isLarge is bool large)
			{
				sizeStr = large ? "large, " : "tiny, ";
			}

			return sizeStr + "demonic, bat-like wing" + (plural ? "s" : "");
		}

		private static string BatLikeSingleDesc(bool? isLarge)
		{
			string sizeStr = "";
			if (isLarge is bool large)
			{
				sizeStr = large ? "large, " : "tiny, ";
			}

			return "a " + sizeStr + "demonic, bat-like wing";
		}

		private static string BatLikeLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			//large and cute both use "a ", so i can cheat.
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}

			if (wings.isLarge)
			{
				return intro + "large demonic wings, which appear similar to those of a bat.";
			}
			return intro + "cute, tiny, demonic bat-like wings.";
		}
		private static string BatLikePlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large bat-like demon-wings fold behind your shoulders. With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.";
			}
			else
			{
				return " A pair of tiny bat-like demon-wings sprout from your back, flapping cutely, but otherwise being of little use.";
			}
		}

		private static string BatChangeSizeText(bool grewLarge, PlayerBase player)
		{
			if (grewLarge)
			{
				return "Your small demonic wings stretch and grow, tingling with the pleasure of being attached to such a tainted body. You stretch over your shoulder " +
					"to stroke them as they unfurl, turning into full-sized demon-wings. " + SafelyFormattedString.FormattedText("Your demonic wings have grown!", StringFormats.BOLD);
			}
			else
			{
				return "Your large demonic wings start to contract inward, providing a rather unpleasant sensation. You stretch over your shoulder " +
					"to check what's happening and see they are now tiny, bat-like demon-wings. " +
					SafelyFormattedString.FormattedText("Your demonic wings have shrunk!", StringFormats.BOLD);
			}
		}

		private static string BatLikeTransformStr(WingData previousWingData, PlayerBase player)
		{
			if (previousWingData.type == NONE)
			{
				string armorText = player.wearingArmor
					? "ripping a pair of holes in the back of your " + player.armor.shortName() + "."
					: "and you're glad you aren't wearing any armor because they might've ripped through it.";

				return "A knot of pain forms in your shoulders as they tense up. With a surprising force, " + player.wings.LongDescription(true, true) + " sprout from your back, "
					+ armorText + SafelyFormattedString.FormattedText("You now have " + player.wings.LongDescription() + "!", StringFormats.BOLD);
			}
			else
			{
				return "The muscles around your shoulders bunch up uncomfortably, changing to support your wings as you feel their weight increasing. You twist your head " +
					"as far as you can for a look and realize they've changed into " +
					SafelyFormattedString.FormattedText((player.wings.isLarge ? "large, " : "small, ") + "bat-like demon-wings!", StringFormats.BOLD);
			}
		}
		private static string BatLikeRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}
		private static string DraconicDesc(bool? isLarge, bool plural = true)
		{
			string wingStr = plural ? "wings" : "wing";
			if (isLarge is null)
			{
				return "draconic " + wingStr;
			}
			else if (isLarge == true)
			{
				return "large draconic " + wingStr;
			}
			else
			{
				return "small, vestigial " + wingStr;
			}
		}

		private static string DraconicSingleDesc(bool? isLarge)
		{
			if (isLarge is null)
			{
				return "a draconic wing";
			}
			else if (isLarge == true)
			{
				return "large draconic wing";
			}
			else
			{
				return "a small, vestigial wing";
			}
		}

		private static string DraconicLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of " + wings.wingTone.AsString();
			}
			else if (alternateFormat)
			{
				intro = Utils.AddArticle(wings.wingTone.AsString());
			}
			else if (plural)
			{
				intro = "pair of " + wings.wingTone.AsString();
			}
			else
			{
				intro = wings.wingTone.AsString();
			}
			return intro + " " + DraconicDesc(wings.isLarge, plural);
		}
		private static string DraconicPlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " Magnificent wings sprout from your shoulders. When unfurled they stretch further than your arm span, and a single beat of them is all you need " +
					"to set out toward the sky. They look a bit like bat wings, but the membranes are covered in fine, delicate " + wings.wingTone.AsString() + " scales supported by " +
					wings.wingBoneTone.AsString() + " bones. A wicked talon juts from the end of each bone.";
			}
			else
			{
				return " Small, vestigial wings sprout from your shoulders. They might look like bat wings, but the membranes are covered in fine, delicate " + wings.wingTone.AsString() +
					" scales supported by " + wings.wingBoneTone.AsString() + " bones.";
			}
		}

		private static string DraconicChangeSizeText(bool grewLarge, PlayerBase player)
		{
			if (grewLarge)
			{
				return "A not-unpleasant tingling sensation fills your wings, almost but not quite drowning out the odd, tickly feeling as they swell larger and stronger. " +
					"You spread them wide - they stretch further than your arms do - and beat them experimentally, the powerful thrusts sending gusts of wind, " +
					"and almost lifting you off your feet. " + SafelyFormattedString.FormattedText("You now have fully-grown dragon wings, capable of winging you " +
						"through the air elegantly!", StringFormats.BOLD);
			}
			else
			{
				return "A not-unpleasant tingling sensation fills your wings, almost but not quite drowning out the odd, tickly feeling as they shrink. " +
					"You spread them wide and are disappointed at the much smaller wingspan. Beating them experimentally, you feel a small rush of wind along your back, but that's it. " +
					SafelyFormattedString.FormattedText("Sadly, you now have small dragon wings! At least they look somewath cute.", StringFormats.BOLD);
			}
		}

		private static string DraconicTransformStr(WingData previousWingData, PlayerBase player)
		{

			if (previousWingData.type == NONE)
			{
				string sizeText = player.wings.isLarge
					? SafelyFormattedString.FormattedText("You now have large dragon wings! They're powerful enough to lift you into the air with just one flap!", StringFormats.BOLD)
					: SafelyFormattedString.FormattedText("You now have small dragon wings! They're not big enough to fly with, but they look adorable.", StringFormats.BOLD);

				return "You double over as waves of pain suddenly fill your shoulderblades; your back feels like it's swelling, flesh and muscles ballooning. " +
					"A sudden sound of tearing brings with it relief and you straighten up. Upon your back now sit small, leathery wings, not unlike a bat's. " +
					sizeText;
			}
			else
			{
				return "A sensation of numbness suddenly fills your wings. When it dies away, they feel... different. Looking back, you realize that they have been replaced by new, " +
					(player.wings.isLarge ? "large" : "small") + " wings, ones that you can only describe as draconic. " +
					SafelyFormattedString.FormattedText("Your wings have changed into dragon wings!", StringFormats.BOLD);
			}
		}
		private static string DraconicRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}
		private static string FaerieDesc(bool? isLarge, bool plural = true)
		{
			string size = isLarge is bool large ? (large ? "large " : "small ") : "";
			return size + "faerie-like wing" + (plural ? "s" : "");
		}

		private static string FaerieSingleDesc(bool? isLarge)
		{
			string size = isLarge is bool large ? (large ? "large " : "small ") : "";
			return "a " + size + "faerie-like wing";
		}

		private static string FaerieLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + FaerieDesc(wings.isLarge, plural);
		}
		private static string FaeriePlayerStr(Wings wings, PlayerBase player)
		{
			return "You have somehow obtained " + wings.LongDescription(true) + ". Congratulations! You should never see this, because this is just for NPCs and monsters! "
					+ "Have a cookie. Also, report this to a dev, so they can either fix this if it was caused by a bug, or update this text if added intentionally.";
		}

		private static string FaerieChangeSizeText(bool grewLarge, PlayerBase player)
		{
			return "Through means not yet implemented, you've managed to change the size of your faerie wings. Current implementation limits faerie wings to monsters, " +
				"so you should never see this. Congratulations! Contact a dev.";
		}

		private static string FaerieTransformStr(WingData previousWingData, PlayerBase player)
		{
			return "through powers unknown to me, you have managed to transform your wings into faerie wings. You shouldn't be able to do this unless content has changed. Either way, " +
				"please report this to a dev.";
		}
		private static string FaerieRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return "As if to remedy this strange case of you (a player) having faerie wings, they have decided to cease to exist, leaving you without wings. Still, " +
				"this should never happen unless someone changed the content and didn't update this. Regardless, please report this to a dev.";
		}
		//always large.
		private static string DragonflyDesc(bool plural = true)
		{
			return "giant dragonfly wing" + (plural ? "s" : "");
		}

		private static string DragonflySingleDesc()
		{
			return "a giant dragonfly wing";
		}

		private static string DragonflyLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + DragonflyDesc();
		}
		private static string DragonflyPlayerStr(Wings wings, PlayerBase player)
		{
			return " Giant dragonfly wings hang from your shoulders. At a whim, you could twist them into a whirring rhythm fast enough to lift you off the ground and allow you to fly.";
		}

		private static string DragonflyTransformStr(WingData previousWingData, PlayerBase player)
		{
			string intro;
			if (previousWingData.type == NONE)
			{
				intro = "You scream and fall to your knees as incredible pain snags at your shoulders, as if needle like hooks were being sunk into your flesh, " +
					"just below your shoulder blades. After about five seconds of white hot, keening agony it is with almost sexual relief that something splits out of your upper back. " +
					"You clench the dirt as you slide what feel like giant leaves of paper into the open air. Eventually the sensation passes and you groggily get to your feet. ";

			}
			else
			{
				intro = "You scream and fall to your knees as incredible pain flows through your " + previousWingData.LongDescription() + ". You gasp as they being to shift and reform," +
					"becoming almost paper thin. It takes about five seconds too complete, and the sudden relief from the indescribable agony is almost orgasmic. ";
			}

			return intro + "You can barely believe what you can see by craning your neck behind you - " +
				SafelyFormattedString.FormattedText("you've grown a set of four giant dragonfly wings", StringFormats.BOLD) + ", thinner, longer and more pointed than the ones " +
				"you've seen upon the forest bee girls, but no less diaphanous and beautiful. You cautiously flex the new muscle groups in your shoulder blades and gasp " +
				"as your new wings whirr and lift you several inches off the ground. What fun this is going to be!";
		}
		private static string DragonflyRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}
		private static string ImpDesc(bool? isLarge, bool plural = true)
		{
			string size = isLarge is bool large ? (large ? "large " : "small ") : "";
			return size + "imp-like wing" + (plural ? "s" : "");
		}

		private static string ImpSingleDesc(bool? isLarge)
		{
			string intro;
			if (isLarge is bool large)
			{
				intro = "a " + (large ? "large " : "small ");
			}
			else
			{
				intro = "an ";
			}
			return intro + "imp-like wing";

		}

		private static string ImpLongDesc(WingData wings, bool alternateFormat, bool plural)
		{
			string intro = "";
			if (alternateFormat && plural)
			{
				intro = "a pair of ";
			}
			else if (alternateFormat)
			{
				intro = "a ";
			}
			else if (plural)
			{
				intro = "pair of ";
			}
			else
			{
				intro = "";
			}
			return intro + ImpDesc(wings.isLarge);
		}
		private static string ImpPlayerStr(Wings wings, PlayerBase player)
		{
			if (wings.isLarge)
			{
				return " A pair of large imp wings fold behind your shoulders. With a muscle-twitch, you can extend them, and use them to soar gracefully through the air.";
			}
			else
			{
				return " A pair of imp wings sprout from your back, flapping cutely but otherwise being of little use.";
			}
		}

		private static string ImpChangeSizeText(bool grewLarge, PlayerBase player)
		{
			if (grewLarge)
			{
				return "Your small imp wings stretch and grow, tingling with the pleasure of being attached to such a tainted body. You stretch over your shoulder to stroke them " +
					"as they unfurl, turning into large imp-wings. " + SafelyFormattedString.FormattedText("Your imp wings have grown!", StringFormats.BOLD);
			}
			else
			{
				return "Your large imp-like wings start to contract inward, providing a rather unpleasant sensation. You stretch over your shoulder " +
					"to check what's happening and see they are now tiny, imp-like wings. " +
					SafelyFormattedString.FormattedText("Your impish wings have shrunk!", StringFormats.BOLD);
			}
		}

		private static string ImpTransformStr(WingData previousWingData, PlayerBase player)
		{
			if (previousWingData.type == NONE)
			{
				string armorText = player.wearingArmor
					? "ripping a pair of holes in the back of your " + player.armor.shortName() + "."
					: "and you're glad you aren't wearing any armor because they might've ripped through it.";

				return "A knot of pain forms in your shoulders as they tense up. With a surprising force, " + player.wings.LongDescription(true, true) + " sprout from your back, "
					+ armorText + SafelyFormattedString.FormattedText("You now have " + player.wings.LongDescription() + "!", StringFormats.BOLD);
			}
			else
			{
				return "The muscles around your shoulders bunch up uncomfortably, changing to support your wings as you feel their weight increasing. You twist your head " +
					"as far as you can for a look and realize they've changed into " +
					SafelyFormattedString.FormattedText((player.wings.isLarge ? "large, " : "small, ") + "imp-like wings!", StringFormats.BOLD);
			}
		}
		private static string ImpRestoreStr(WingData previousWingData, PlayerBase player)
		{
			return GenericRestoreText(previousWingData, player);
		}

		//taken from shriveled tentacle and snake oil. imo the shriveled tentacle one is better, but the whole shriveling thing might be thematic. so i kept both, and you get them randomly.
		private static string GenericRestoreText(WingData originalWings, PlayerBase player)
		{
			if (Utils.RandBool())
			{
				return "Your wings twitch and flap involuntarily. You crane your neck to look at them as best you are able. From what you can see, they " +
					"seem to be shriveling and curling up. They're starting to look a lot like they did when they first popped out, wet and new. " +
					SafelyFormattedString.FormattedText("As you watch, they shrivel all the way, then recede back into your body.", StringFormats.BOLD);
			}
			else
			{
				return "A wave of tightness spreads through your back, and it feels as if someone is stabbing a dagger into each of your shoulder-blades. " +
					"After a moment the pain passes, and you realize " + SafelyFormattedString.FormattedText("your wings are gone!", StringFormats.BOLD);
			}
		}

	}
}
