//BodyStrings.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 8:22 PM
using CoC.Backend.Creatures;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Text;

namespace CoC.Backend.BodyParts
{

	public partial class Body
	{
		public static string Name()
		{
			return "Body";
		}
	}

	public partial class BodyType
	{
		public string ShortDescriptionWithoutBody() => ShortDescriptionWithoutBody(out bool _);
		public virtual string ShortDescriptionWithoutBody(out bool isPlural)
		{
			if (secondary.isEmpty)
			{
				return $"{MainDescription(out isPlural)}";
			}
			else
			{
				isPlural = true;
				return $"{MainDescription()} and {SupplementaryDescription()}";
			}
		}

		public virtual string LongDescriptionWithoutBody(BodyData body) => LongDescriptionWithoutBody(body, out bool _);
		public virtual string LongDescriptionWithoutBody(BodyData body, out bool isPlural)
		{
			if (secondary.isEmpty)
			{
				return $"{body.main.DescriptionWithoutType()} {MainDescription(out isPlural)}";
			}
			else
			{
				isPlural = true;
				return $"{body.main.DescriptionWithoutType()} {MainDescription()} and {body.supplementary.DescriptionWithoutType()} {SupplementaryDescription()}";
			}
		}


		#region Generics

		protected static string AllBodyDesc(bool isTone) => AllBodyDesc();

		protected static string AllBodyDesc()
		{
			return "Body (All)";
		}

		protected static string MainBodyDesc(bool isTone) => MainBodyDesc();
		protected static string MainBodyDesc()
		{
			return "Body (Main)";
		}

		protected static string UnderBodyDesc(bool isTone) => UnderBodyDesc();
		protected static string UnderBodyDesc()
		{
			return "Underbody";
		}

		protected static string YourBodyDesc(out bool isPlural)
		{
			isPlural = false;
			return " your body";
		}

		protected static string YourUnderBodyDesc(out bool isPlural)
		{
			isPlural = false;
			return " your underbody";
		}

		protected static string TheSkinUnderStr(string locationDesc)
		{
			return "the skin under " + locationDesc;
		}

		protected static string SkinUnderStr(Body body, string locationDesc)
		{
			return body.primarySkin.LongDescription() + " under your " + locationDesc;
		}

		protected static string YourLocationStr(string locationDesc)
		{
			return "your " + locationDesc;
		}

		protected static string GenericPostDesc(Body body)
		{
			return body.FullDescription(true);
		}
		#endregion



		#region Skin

		private static string SkinDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("body", alternateFormat);
		}

		private static string SkinDescNoType(out bool isPlural)
		{
			isPlural = false;
			return "skin";
		}

		private static string SkinLongDesc(BodyData body, bool alternateFormat)
		{
			return $"{body.main.JustColor(alternateFormat)}-skinned body";
		}

		private static string SkinFullDesc(BodyData body, bool alternateFormat)
		{
			return SkinLongDesc(body, alternateFormat);
		}

		private static string SkinPlayerStr(Body body, PlayerBase player)
		{
			//feel free to rephrase this, but you'd need to store the "acceptable/normal" skin tones for a human, then say something like,
			return GenericBodyPlayerDesc(false) + " You have " + body.mainEpidermis.LongDescription();
		}
		private static string SkinTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			return previousBodyData.type.RestoredString(previousBodyData, player);
		}
		private static string SkinRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GlobalStrings.RevertAsDefault(previousBodyData, player);
		}
		#endregion
		#region Scales Generic

		protected static string AllScalesButton(bool isTone)
		{
			if (isTone)
			{
				return "All Scales";
			}
			else return AllBodyDesc();
		}

		protected static string MainScalesButton(bool isTone)
		{
			if (isTone)
			{
				return "Main Scales";
			}
			else
			{
				return MainBodyDesc();
			}
		}

		protected static string AlternateScalesButton(bool isTone)
		{
			if (isTone)
			{
				return "Vent. Scales";
			}
			else
			{
				return UnderBodyDesc();
			}
		}

		protected static string AllScalesDesc(out bool isPlural)
		{
			isPlural = true;
			return "all of your scales";
		}

		protected static string PrimaryScalesDesc(out bool isPlural)
		{
			isPlural = true;
			return "primary scales";
		}

		protected static string VentralScalesDesc(out bool isPlural)
		{
			isPlural = true;
			return "ventral scales";
		}

		private static string GenericScalesRestoreText(PlayerBase player)
		{
			return "You feel an odd sensation, followed by the telltale signs that your body is beginning to molt. It feels different than what you'd expect, however, and you can't "
				+ "quite determine why. Once the last of your former scales peel away, the reason becomes obvious: " + SafelyFormattedString.FormattedText("you no longer have any scales!",
				StringFormats.BOLD) + " In their place is " + player.body.mainEpidermis.LongDescription();
		}

		#endregion
		#region Reptile (Lizard/Salamander/Naga)

		private static string ReptileDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("scaley body", alternateFormat);
		}
		private static string ReptileBodyDesc(out bool isPlural)
		{
			return PrimaryScalesDesc(out isPlural);
		}

		private static string ReptileUnderbodyDesc(out bool isPlural)
		{
			return VentralScalesDesc(out isPlural);
		}
		private static string ReptileLongDesc(BodyData body, bool alternateFormat)
		{
			if (EpidermalData.MixedTones(body.main, body.supplementary))
			{
				return "multi-colored, scaley body";
			}
			else
			{
				return body.main.LongAdjectiveDescription(alternateFormat) + " body";
			}
		}

		private static string ReptileFullDesc(BodyData body, bool alternateFormat)
		{
			if (EpidermalData.MixedTones(body.main, body.supplementary))
			{
				return $"{(alternateFormat ? "a " : "")}predominantly {body.main.JustColor()}, scaley body, with {body.supplementary.DescriptionWithoutType()} ventrals";
			}
			else
			{
				return body.main.LongAdjectiveDescription(alternateFormat) + " body";
			}
		}

		private static string ReptilePlayerStr(Body body, PlayerBase player)
		{
			if (EpidermalData.MixedTones(body.mainEpidermis, body.supplementaryEpidermis))
			{
				string ventral = body.supplementaryEpidermis.skinTexture != SkinTexture.NONDESCRIPT
					? "a " + body.supplementaryEpidermis.JustTexture() + " " + body.supplementaryEpidermis.JustColor()
					: body.supplementaryEpidermis.JustColor();
				return GenericBodyPlayerDesc() + "Most of your body is covered in " + body.mainEpidermis.LongDescription() + ", though your ventral scales are " + ventral + ".";
			}
			else if (body.mainEpidermis.skinTexture != body.supplementaryEpidermis.skinTexture)
			{
				return GenericBodyPlayerDesc() + "Your body is covered in " + body.mainEpidermis.DescriptionWithColor() + ". Most of them are " + body.mainEpidermis.JustTexture() + ", though your ventral ones are "
					+ body.supplementaryEpidermis.JustTexture() + ".";
			}
			else
			{
				return GenericBodyPlayerDesc() + "Your body is covered in " + body.mainEpidermis.LongDescription() + ".";
			}
		}
		private static string ReptileTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph() + "You idly reach back to scratch yourself and nearly jump");

			//fur or feathers or wool or bark or carapace.
			if (previousBodyData.type.hasFurOrFeathersOrWool || previousBodyData.type == BodyType.WOODEN || previousBodyData.type == BodyType.CARAPACE)
			{
				string oldEpidermis = previousBodyData.type.epidermisType.ShortDescription();
				sb.Append(" in surprise when you see some " + oldEpidermis + "stuck in your " + player.hands.NailsText(true) + ". A quick glance down reveals your "
					+ oldEpidermis + "is falling away, forced off your body");

				//somehow have fur and scales (i.e. cockatrice)
				if (previousBodyData.type.primary.epidermisType == EpidermisType.SCALES || previousBodyData.type.secondary.epidermisType == EpidermisType.SCALES)
				{
					sb.Append(" as your scales rapidly overtake them.");
				}
				else
				{
					sb.Append(" by the scales that are rapidly forming beneath them.");
				}
				sb.Append(" As you watch the remaining " + oldEpidermis + "fall away, you notice the scales ");
				if (previousBodyData.main.type == EpidermisType.SCALES || previousBodyData.supplementary.type == EpidermisType.SCALES)
				{
					sb.Append("are shifting, growing in in some places and shedding in others until they finalize in a formation that is essentially seamless.");
				}
				else
				{
					sb.Append("are interlinking together so tightly that they are essentially seamless.");
				}


			}
			else if (previousBodyData.type == BodyType.HUMANOID || previousBodyData.type == GOO)
			{
				if (player.wearingArmor)
				{
					sb.Append(" out of your " + player.armor.shortName());
				}
				else if (previousBodyData.type == GOO)
				{
					sb.Append(" out of your goo");
				}
				else
				{
					sb.Append(" out of your skin");
				}
				sb.Append(" when you hit something hard. A quick glance down reveals that scales are growing out of your " + previousBodyData.LongDescription() + " with"
					+ " alarming speed. You continue to watch as the rest of your " + previousBodyData.ShortDescription() + " changes until is covered in smooth scales. "
					+ "They interlink together so well that they may as well be seamless.");

				if (previousBodyData.type == GOO)
				{
					sb.Append(" The changes radiate inward until, your body solidifying as it loses its gooey elasticity.");
				}

			}
			else if (previousBodyData.main.type == EpidermisType.SCALES || previousBodyData.supplementary.type == EpidermisType.SCALES)
			{
				sb.Append(" in surprise when you notice the cause of your itch - you're molting. As your old scales fall away");
				bool allScales = previousBodyData.main.type == EpidermisType.SCALES && (previousBodyData.supplementary.type == EpidermisType.SCALES || previousBodyData.supplementary.isEmpty);

				if (allScales)
				{
					sb.Append("You notice the new scales are shifting, growing in in some places and shedding in others until they finalize in a formation that is essentially seamless. ");
				}
				else
				{
					sb.Append("you notice they new scales aren't just replacing the old ones, they're also overtaking the rest of your body! As they do, they shift, interlocking together" +
						"in such a way that they may as well be seamless.");
				}
			}
			//generic
			else
			{
				sb.Append(" in surprise when you hit something unexpected. A quick glance down reveals that scales are growing out of your " + previousBodyData.LongDescription() + " with"
					+ " alarming speed. You continue to watch as the rest of your " + previousBodyData.ShortDescription() + " changes until is covered in smooth scales. "
					+ "They interlink together so well that they may as well be seamless.");
			}
			if (player.wearingArmor)
			{
				sb.Append("You peel back your " + player.armor.shortName() + " and the transformation has already finished on the rest of your body.");
			}
			else
			{
				sb.Append("You can't help but watch in awe as transformation continues on the rest of your body.");
			}
			sb.Append(GlobalStrings.NewParagraph() + SafelyFormattedString.FormattedText("You now have scales covering most of your body!", StringFormats.BOLD));
			sb.Append("Most of them are " + player.body.mainEpidermis.AdjectiveDescriptionWithoutType(true) + ", but your ventral ones are "
				+ player.body.supplementaryEpidermis.AdjectiveDescriptionWithoutType(true));

			return sb.ToString();
		}
		private static string ReptileRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GenericScalesRestoreText(player);
		}
		#endregion
		#region Dragon
		private static string DragonDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("drakescale-covered body", alternateFormat);
		}

		private static string DragonBodyDesc(out bool isPlural)
		{
			isPlural = true;
			return "draconic primary scales";
		}

		private static string DragonUnderbodyDesc(out bool isPlural)
		{
			isPlural = true;
			return "draconic ventral scales";
		}
		private static string DragonLongDesc(BodyData body, bool alternateFormat)
		{
			if (EpidermalData.MixedTones(body.main, body.supplementary))
			{
				return "multi-colored, drakescale-covered body";
			}
			else
			{
				return body.main.LongAdjectiveDescription(alternateFormat) + " body";
			}
		}

		private static string DragonFullDesc(BodyData body, bool alternateFormat)
		{
			if (EpidermalData.MixedTones(body.main, body.supplementary))
			{
				if (alternateFormat)
				{
					return $"{body.main.DescriptionWithoutType()} draconic scales covering most of your body, shifting to {body.supplementary.DescriptionWithoutType(true)} on your ventrals";
				}
				else
				{
					return $"body, covered in {body.main.DescriptionWithoutType()} draconic scales that shift to {body.supplementary.DescriptionWithoutType(true)} on your ventrals";
				}
			}
			else
			{
				return body.main.AdjectiveDescriptionWithoutType(alternateFormat) + "drakescale-covered body";
			}
		}

		private static string DragonPlayerStr(Body body, PlayerBase player)
		{
			if (EpidermalData.MixedTones(body.mainEpidermis, body.supplementaryEpidermis))
			{
				string ventral = body.supplementaryEpidermis.skinTexture != SkinTexture.NONDESCRIPT
					? "a " + body.supplementaryEpidermis.JustTexture() + " " + body.supplementaryEpidermis.JustColor()
					: body.supplementaryEpidermis.JustColor();
				return GenericBodyPlayerDesc() + "Most of your body is covered in " + body.mainEpidermis.DescriptionWithoutType() + "draconic scales, though they shift to " +
					ventral + " around your ventrals. Their considerable thickness grants you a natural armor.";
			}
			else if (body.mainEpidermis.skinTexture != body.supplementaryEpidermis.skinTexture)
			{
				return GenericBodyPlayerDesc() + "Your body is covered in " + body.mainEpidermis.JustColor() + " draconic scales. Most of them are " + body.mainEpidermis.JustTexture()
					+ ", though your ventral ones are " + body.supplementaryEpidermis.JustTexture() + ". Their considerable thickness grants you a natural armor.";
			}
			else
			{
				return GenericBodyPlayerDesc() + "Your body is covered in " + body.mainEpidermis.DescriptionWithoutType() + "draconic scales. " +
					"Their considerable thickness grants you a natural armor.";
			}
		}
		private static string DragonTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			if (previousBodyData.type != BodyType.GOO && previousBodyData.type != REPTILE)
			{
				return GlobalStrings.NewParagraph() + "Prickling discomfort suddenly erupts all over your body, like every last inch of your skin has suddenly developed"
					+ " pins and needles. You scratch yourself, hoping for relief, though all this does is get little bits of your " + previousBodyData.main.ShortDescription()
					+ " stuck in your " + player.hands.NailsText() + ". Despite this, you continue to scratch yourself until finally, mercifully, the feeling is gone. Looking yourself "
					+ "over, you notice that new, shield-like scales have grown to replace your peeled off " + previousBodyData.type.ShortDescription() + ". They are smooth "
					+ "and look nearly as tough as iron. " + SafelyFormattedString.FormattedText($"Your body is now covered in {player.body.mainEpidermis.JustColor()}, shield-shaped " +
						$"dragon scales with {player.body.supplementaryEpidermis.JustColor()} ventral scales covering your underside", StringFormats.BOLD) + ".";
			}
			else if (previousBodyData.type == GOO)
			{
				return GlobalStrings.NewParagraph() + "Prickling discomfort suddenly erupts throughout your gelatenous form. You shudder involuntarily as it begins to solidify, starting" +
					"at the very core of your being and working its way outward. Eventually, your body regains a more normal, solid form, though the outermost layer is still gooey. " +
					"This excess goo then shifts, clumping together in a way that almost resembles thick scales, then starts to harden. Once the process finishes, you are left with a " +
					$"normal frame that's covered in {player.body.mainEpidermis.JustColor()}, shield-shaped dragon scales with {player.body.supplementaryEpidermis.JustColor()} ventral " +
					$"scales covering your underside. " + SafelyFormattedString.FormattedText("You now have a distinctly draconic body!", StringFormats.BOLD);
			}
			else
			{
				return GlobalStrings.NewParagraph() + "Prickling discomfort suddenly erupts all over your body, quickly followed by a very rapid, very unexpected molting. " +
					"Once the process is finished, you notice the new scales underneath have shifted, and " + SafelyFormattedString.FormattedText("are now a set of thick, " +
						"disjointed draconic scales!", StringFormats.BOLD);
			}
		}
		private static string DragonRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GenericScalesRestoreText(player);
		}
		#endregion
		#region Cockatrice
		protected static string CockatriceFeathersButton(bool isTone)
		{
			if (!isTone)
			{
				return "Feathers";
			}
			else return MainBodyDesc();
		}

		protected static string CockatriceScalesButton(bool isTone)
		{
			if (isTone)
			{
				return "Scales";
			}
			else return UnderBodyDesc();
		}

		protected static string CockatriceDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("partially feathered, partially scaled body", alternateFormat);
		}

		protected static string CockatriceBodyDesc(out bool isPlural)
		{
			isPlural = true;
			return "body feathers";
		}

		protected static string CockatriceUnderbodyDesc(out bool isPlural)
		{
			isPlural = true;
			return "scaley chest and nethers";
		}
		protected static string CockatriceLongDesc(BodyData body, bool alternateFormat)
		{
			return Utils.AddArticleIf("feathered body and scaly core", alternateFormat);
		}

		protected static string CockatriceFullDesc(BodyData body, bool alternateFormat)
		{
			return Utils.AddArticleIf("predominantly ", alternateFormat) + body.main.LongAdjectiveDescription() + " body with " + body.supplementary.LongDescription() + "around the core";
		}

		protected static string CockatricePlayerStr(Body body, PlayerBase player)
		{
			return GenericBodyPlayerDesc(false) + " Your body is covered in a curious amalgum of feathers and scales. " + body.mainEpidermis.LongDescription().CapitalizeFirstLetter() +
				" covering most of your body, while " + body.supplementaryEpidermis.LongDescription() + " coat you from chest to groin.";
		}
		//old text was generic, which is nice. These transfomrations may bet derivative quickly if they all cause you to be KOed then see what happens afterward, but they are
		//really, really easy to implement.
		protected static string CockatriceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			string bodyText;
			//handle gooeyness. NGL it's a bit of a pain handling you being goo in every transform, but i suppose that's the price you pay.
			if (previousBodyData.type == BodyType.GOO)
			{
				bodyText = "An unnatural heat rises through your gooey form, making you wobble uncontrollably. ";
			}
			else if (previousBodyData.type.epidermisType != EpidermisType.SKIN)
			{
				string skinText = previousBodyData.main.ShortDescription(out bool isPlural) + (isPlural ? " feel as if they are" : " feels like it is");
				bodyText = "Your body feels hot and your " + skinText + " being forced inward, almost like " +
					"you're wearing a shirt several sizes too small. ";
			}
			else
			{
				bodyText = "Your body feels hot and your skin feels tight, as if you're wearing a shirt several sizes too small. ";
			}
			string legsText;
			if (player.lowerBody.type == LowerBodyType.GOO)
			{
				legsText = "A sudden rush or vertigo causes you to collapse onto yourself, and you remain there, ";
			}
			else if (player.lowerBody.isMonoped)
			{
				legsText = "A sudden rush or vertigo causes you collapse to the ground. You lay there ";
			}
			else
			{
				legsText = " You fall to your knees in a bout of lightheadedness, ";
			}

			return GlobalStrings.NewParagraph() + bodyText +legsText + " panting as the pressure increases. Sweat drips from your brow. "
				+ "You don't know how long you can take this and soon you drift into unconsciousness." + Environment.NewLine
				+ "When you awaken you check yourself to see what has changed now that the overwhelming pressure has left your body."
				+ " The first thing you notice is feathers, lots and lots of feathers that now cover your body in a downy layer."
				+ " Just below your neck is a ruff of soft fluffy feathers, like you'd see on an exotic bird. As you look down to your " + player.genitals.ChestOrAllBreastsShort()
				+", you see that from your chest to your groin you are covered in a layer of " + player.body.supplementaryEpidermis.LongDescription() + "."
				+ SafelyFormattedString.FormattedText("Your body is now covered in scales and feathers!", StringFormats.BOLD);
		}
		//there is literally nothing to use for reference for this, but that also means i've got complete freedom to do whatever. so i've decided to lean into the cockatrice/basilisk thing
		//for restore text. if you don't like it, change it.

		//also, poetry. Walt Whitman for the win.
		protected static string CockatriceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			string intro = "";
			if (player.wearingAnything)
			{
				intro = "Your body suddenly feels as if it is on fire, so much so you strip down, desperate to find the source. As you do, the feeling is suddenly replaced " +
					"with a numbness, making it difficult to move.";
			}
			else
			{
				intro = "Your body suddenly feels stiff, and you're finding it more and more difficult to move.";
			}
			return intro + " Panic rushes through you as everything below your neck becomes paralyzed. You manage to move your " + player.neck.ShortDescription()
				+ " down enough to look at your body amd notice the source of your current predicament: your body is slowly turning to stone! You're still able to think and breathe, "
				+ "so you're not in immediate mortal danger, but you'd also greatly prefer not to be stuck as a living, immobile statue. Try as you might, though, "
				+ "you can't think up a way out of this, and yell out in frustration. The sound of cracking snaps you back to attention - no doubt the result of your barbaric yawp. "
				+ "After a few more screams, (which were rather cathartic, you might add), you notice it's less about the sound and more about the movement from making it. "
				+ "You begin to rock your head back and forth as best you can, hoping the cracks will spread. It works, though the motion also has an unexpected side-effect: "
				+ "without the control of your " + player.lowerBody.ShortDescription() + ", you're going to fall over! You land with a crash, the impact shattering the stone around you. "
				+ "You groan, and move your " + player.arms.ShortDescription(false) + " to wipe away some of the stone fragments still on you, then realize what you just did. "
				+ "You can move again! That's not all, however. A quick assessment reveals that "
				+ SafelyFormattedString.FormattedText("You no longer have scales and feathers!", StringFormats.BOLD) + " For better or worse, the stony transformation has left you with "
				+ player.body.LongDescription();
		}
		#endregion
		#region Kitsune
		protected static string KitsuneSkinButton(bool isTone)
		{
			if (isTone)
			{
				return "Skin";
			}
			else return MainBodyDesc();
		}

		protected static string KitsuneFurButton(bool isTone)
		{
			if (!isTone)
			{
				return "Fur";
			}
			else return UnderBodyDesc();
		}

		//i've gone with a kitsune look that leans more toward a human with fox-like traits (see Ahri of League of Legends, for example); otherwise the distinction between kitsune
		//is fox are more or less non-existent. Again, if you want to alter this, go for it.

		protected static string KitsuneDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("partially furry body", alternateFormat);
		}

		protected static string KitsuneBodyDesc(out bool isPlural)
		{
			isPlural = false;
			return "skin";
		}

		protected static string KitsuneUnderbodyDesc(out bool isPlural)
		{
			isPlural = true;
			return "patches of fur";
		}
		protected static string KitsuneLongDesc(BodyData body, bool alternateFormat)
		{
			return body.main.JustTexture(alternateFormat) + "-skinned body with occasional patches of fur";
		}

		protected static string KitsuneFullDesc(BodyData body, bool alternateFormat)
		{
			return body.main.AdjectiveDescriptionWithoutType(alternateFormat) + "-skinned body with occasional patches of " + body.supplementary.LongDescription();
		}

		protected static string KitsunePlayerStr(Body body, PlayerBase player)
		{
			return GenericBodyPlayerDesc(false) + $"{body.supplementaryEpidermis.LongDescription()} covers parts of your body, though much of your {body.mainEpidermis.LongDescription()}" +
				"remains exposed for a distinctly kitsune look.";
		}
		//tf: revert anything not skin to skin. if it has fur, retain part of the fur. otherwise, grow in patches of fur.
		protected static string KitsuneTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			bool mixedTypes = EpidermalData.CheckMixedTypes(previousBodyData.main, previousBodyData.supplementary);
			bool primaryWasFur = previousBodyData.main.type == EpidermisType.FUR;
			bool supplementaryWasFur = previousBodyData.supplementary.type == EpidermisType.FUR;
			bool hadFur = primaryWasFur || supplementaryWasFur;


			StringBuilder sb = new StringBuilder(GlobalStrings.NewParagraph());

			if (previousBodyData.type == GOO)
			{
				sb.Append("Your gelatenous form begins to tingle, then begins to solidify, shifting toward something much more human. Pinching yourself, you confirm " +
					"you have human skin again!");
			}
			//Generic text:
			else
			{
				string skinText = previousBodyData.type.ShortDescriptionWithoutBody(out bool plural);
				sb.Append("You begin to tingle all over your " + skinText + ", starting as a cool, pleasant sensation but gradually worsening until you are furiously itching all over." +
					"You stare in horror as bits of your " + skinText);
				if (plural)
				{
					sb.Append(" get");
				}
				else
				{
					sb.Append(" gets");
				}

				sb.Append(" stuck in your " + player.hands.NailsText() + ". ");
				if (plural)
				{
					sb.Append("It falls ");
				}
				else
				{
					sb.Append("They fall ");
				}
				sb.Append("away under your fervent scratching, leaving only tender " + player.body.primarySkin.DescriptionWithColor() + " behind.");
			}

			//now grow the fur in.
			if (hadFur)
			{
				sb.Append(" Strangely, some of your old fur is regrowing, but only in a few distinct areas, giving you a very exotic look");
			}
			else
			{
				sb.Append(" Patches of " + player.body.supplementaryEpidermis.JustColor() + "fur begin to grow in some areas along your body, giving you an alluring, exotic look.");
			}
			sb.Append("You now have " + SafelyFormattedString.FormattedText(player.body.FullDescription(true), StringFormats.BOLD));

			return sb.ToString();
		}

		//idk if vag of holding is a body perk or a genital perk, but could mention removing it here? idk
		protected static string KitsuneRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//the feathers|fur covering parts of your body.
		protected static string PartialDyeDesc(EpidermisType epidermis)
		{
			return "the" + epidermis.ShortDescription() + "covering most of your body";
		}


		#endregion
		#region Bark
		protected static string BarkDescNoType(out bool isPlural)
		{
			isPlural = false;
			return "bark";
		}

		private static string BarkDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("bark-covered body", alternateFormat);
		}
		private static string BarkLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BarkFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string BarkPlayerStr(Body body, PlayerBase player)
		{
			return $"The {body.mainEpidermis.LongAdjectiveDescription(false)} outer layer of your body contrasts starkly with your form, which remains diestinctly humanoid.";

		}
		private static string BarkTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Fur
		protected static string AllFurButton(bool isTone)
		{
			if (!isTone)
			{
				return "All Fur";
			}
			else
			{
				return AllBodyDesc();
			}
		}

		protected static string MainFurButtonNoUnderbody(bool isTone)
		{
			if (!isTone)
			{
				return "Fur";
			}
			else
			{
				return MainBodyDesc();
			}
		}

		protected static string MainFurButton(bool isTone)
		{
			if (!isTone)
			{
				return "Standard Fur";
			}
			else
			{
				return MainBodyDesc();
			}
		}
		protected static string AlternateFurButton(bool isTone)
		{
			if (!isTone)
			{
				return "UnderbodyFur";
			}
			else
			{
				return UnderBodyDesc();
			}
		}

		protected static string FurDescNoType(out bool isPlural)
		{
			isPlural = false;
			return "fur";
		}

		private static string AllFurDye(out bool isPlural)
		{
			isPlural = false;
			return "all of your fur";
		}

		private static string AllFurTone(out bool isPlural)
		{
			isPlural = false;
			return TheSkinUnderStr(AllFurDye(out bool _));
		}

		private static string PostAllFurTone(Body body)
		{
			return SkinUnderStr(body, AllFurDye(out bool _));
		}

		private static string FurDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("furry body", alternateFormat);
		}

		private static string FurPrimaryDesc(out bool isPlural)
		{
			isPlural = false;
			return "primary fur";
		}

		private static string FurUnderBodyDesc(out bool isPlural)
		{
			isPlural = false;
			return "underbody fur";
		}

		private static string FurLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string FurFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		//applies to both simple and underbody.
		private static string FurPlayerStr(Body body, PlayerBase player)
		{
			//first, see if we have different color underbody (if we have one at all). if so
			if (body.hasSecondaryEpidermis && EpidermalData.MixedFurColors(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $" Your body is covered in fur, with most of it {body.mainEpidermis.AdjectiveDescriptionWithoutType(true)}. As it approaches your " +
					$"core and {player.genitals.AllBreastsLongDescription()}, the color shifts until it's {body.supplementaryEpidermis.AdjectiveDescriptionWithoutType(true)}. Despite this, " +
					$"it provides little in terms of modesty as the fur around your stomach and {player.genitals.AllBreastsLongDescription()} is significantly shorter than the rest.";
			}
			//same color, different textures.
			else if (body.hasSecondaryEpidermis && EpidermalData.MixedTextures(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $" Your body is covered in {body.mainEpidermis.DescriptionWithColor()}. The fur around your {player.genitals.AllBreastsLongDescription()}"
					+ $" appears {body.supplementaryEpidermis.JustTexture()}, contrasting with the rest, which appears {body.mainEpidermis.JustTexture()}. Despite this, it provides little "
					+ $"in terms of modesty as the fur around your stomach and {player.genitals.AllBreastsLongDescription()} is significantly shorter than the rest.";
			}
			//solid color/texture throughout, or the underbody is identical to the main.
			else
			{
				return GenericBodyPlayerDesc() + $" Your body is covered in {body.mainEpidermis.LongDescription()}, hiding the {body.primarySkin.LongDescription()} underneath. " +
					$"Despite this, it provides little by way of modesty as the fur around your stomach and {player.genitals.AllBreastsLongDescription()} is significantly shorter " +
					$"than the rest.";
			}
		}
		private static string FurTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GenericFurRestoreText(EpidermisType.FUR, player);
		}
		#endregion
		#region Feathers
		protected static string AllFeathersButton(bool isTone)
		{
			if (!isTone)
			{
				return "All Feathers";
			}
			else
			{
				return AllBodyDesc();
			}
		}

		protected static string MainFeathersButton(bool isTone)
		{
			if (!isTone)
			{
				return "LongFeathers";
			}
			else
			{
				return MainBodyDesc();
			}
		}

		protected static string AlternateFeathersButton(bool isTone)
		{
			if (!isTone)
			{
				return "Downy Fthrs.";
			}
			else
			{
				return UnderBodyDesc();
			}
		}



		protected static string PrimaryFeatherDesc(out bool isPlural)
		{
			isPlural = true;
			return "main feathers";
		}
		private static string UnderFeatherDesc(out bool isPlural)
		{
			isPlural = true;
			return "downy feathers";
		}

		private static string AllFeathersDye(out bool isPlural)
		{
			isPlural = true;
			return "all of your feathers";
		}

		private static string AllFeathersTone(out bool isPlural)
		{
			isPlural = false;
			return TheSkinUnderStr(AllFeathersDye(out bool _));
		}

		private static string PostAllFeathersTone(Body body)
		{
			return SkinUnderStr(body, AllFeathersDye(out bool _));
		}

		private static string FeatherDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("feathered body", alternateFormat);
		}
		private static string UnderFeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string FeatherFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string FeatherPlayerStr(Body body, PlayerBase player)
		{
			//first, see if we have different color underbody (if we have one at all). if so
			if (body.hasSecondaryEpidermis && EpidermalData.MixedFurColors(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $"{body.mainEpidermis.LongDescription()} covers most of your body, shifting to	{body.supplementaryEpidermis.AdjectiveDescriptionWithoutType(true)}" +
					$" as it approaches your {player.genitals.AllBreastsLongDescription()} and nether regions. The downy feathers here do little to hide your features, and have no effect" +
					" on your sensitivity - if anything, they actually increase it.";
			}
			//same color, different textures.
			else if (body.hasSecondaryEpidermis && EpidermalData.MixedTextures(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $"{body.mainEpidermis.LongDescription()} covers your body, though the texture changes as it approaches your " +
					$"{player.genitals.AllBreastsLongDescription()} and nethers, where it becomes {body.supplementaryEpidermis.JustTexture()}. The downy feathers here do little " +
					$"to hide your features, and have no effect on your sensitivity - if anything, they actually increase it.";
			}
			//the underbody is identical to the main.
			else
			{
				return GenericBodyPlayerDesc() + $" Your body is covered in {body.mainEpidermis.LongDescription()}, hiding the {body.primarySkin.LongDescription()} underneath. " +
					$"You only have short downy feathers around your {player.genitals.AllBreastsLongDescription()} and nethers, which does little to protect the sensitive skin " +
					$"underneath - if anything, you'd say it makes it even more sensitive.";
			}
		}
		private static string FeatherTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GenericFurRestoreText(EpidermisType.FEATHERS, player);
		}


		#endregion
		#region Wool
		protected static string AllWoolButton(bool isTone)
		{
			if (!isTone)
			{
				return "All Wool";
			}
			else
			{
				return AllBodyDesc();
			}
		}

		protected static string MainWoolButton(bool isTone)
		{
			if (!isTone)
			{
				return "Normal Wool";
			}
			else
			{
				return MainBodyDesc();
			}
		}

		protected static string AlternateWoolButton(bool isTone)
		{
			if (!isTone)
			{
				return "Under-Wool";
			}
			else
			{
				return UnderBodyDesc();
			}
		}

		private static string AllWoolDye(out bool isPlural)
		{
			isPlural = false;
			return "all of your wool";
		}

		protected static string WoolBodyDesc(out bool isPlural)
		{
			isPlural = false;
			return "your main wool";
		}


		private static string WoolUnderbodyDesc(out bool isPlural)
		{
			isPlural = false;
			return "your underbody-wool";
		}

		private static string AllWoolTone(out bool isPlural)
		{
			isPlural = false;
			return TheSkinUnderStr(AllWoolDye(out bool _));
		}

		private static string PostAllWoolTone(Body body)
		{
			return SkinUnderStr(body, AllWoolDye(out bool _));
		}

		private static string WoolDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("wooly body", alternateFormat);
		}

		private static string WoolLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string WoolFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string WoolPlayerStr(Body body, PlayerBase player)
		{
			//first, see if we have different color underbody (if we have one at all). if so
			if (body.hasSecondaryEpidermis && EpidermalData.MixedFurColors(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $"{body.mainEpidermis.LongDescription()} covers your body, though the regions around your {player.genitals.AllBreastsLongDescription()} "
					+ $"and nethers are {body.supplementaryEpidermis.AdjectiveDescriptionWithoutType(true)}. Fortunately, it doesn't seem to grow too long or too quickly, so you're able "
					+ $"to keep it just the way it is with only a little effort.";
			}
			//same color, different textures.
			else if (body.hasSecondaryEpidermis && EpidermalData.MixedTextures(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + $"{body.mainEpidermis.LongDescription()} covers your body, though its texture changes to {body.supplementaryEpidermis.JustTexture()} " +
					$"in the regious surrounding your {player.genitals.AllBreastsLongDescription()} and nethers. Fortunately, it doesn't seem to grow too long or too quickly, " +
					$"so you're able to keep it just the way it is with only a little effort.";
			}
			//the underbody is identical to the main.
			else
			{
				return GenericBodyPlayerDesc() + $"{body.mainEpidermis.LongDescription()} covers your body, hiding the {body.primarySkin.LongDescription()} underneath. " +
					$"Fortunately, it doesn't seem to grow too long or too quickly, so you're able to keep it just the way it is with only a little effort.";
			}
		}
		private static string WoolTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return GenericFurRestoreText(EpidermisType.WOOL, player);
		}


		#endregion
		#region Goo

		protected static string GooDescNoType(out bool isPlural)
		{
			isPlural = false;
			return "goo";
		}

		private static string GooDesc(bool alternateFormat)
		{
			if (alternateFormat)
			{
				return "a goo-body";
			}
			else
			{
				return "gooey body";
			}
		}
		private static string GooLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string GooPlayerStr(Body body, PlayerBase player)
		{
#warning ToDo: Add text about the heart crystal key item. IDK if that's a backend thing or a body accessory.
			//realistically, if the body is goo, all the appendages should be too (except cocks, but that's an exception because reasons). but if it's not, we have some unique text
			//lampshading how absurd it is.
			string bodyText;

			if (player.face.type == FaceType.GOO && player.arms.type == ArmType.GOO && player.lowerBody.type == LowerBodyType.GOO)
			{
				bodyText = " You're able to shift your form, allowing you to fit through tight spaces if you really wanted or needed to. ";
			}
			else
			{
				string sillyText = SillyModeSettings.isEnabled ? ", because video game logic" : "";
				bodyText = $" Strangely, the goo appears to solidify near your non-gooey appendages, which retain their shape and form{sillyText}. Unfortunately, this means you " +
					$"can't formshift to fit through tight spaces, but at least you're not completely gooey!";
			}

			return "Your body appears humanoid shape and structure, but the fact it is made entirely of " + body.mainEpidermis.LongDescription() + " makes it very clear it is not. " +
				"While you appear to have no skeleton or nervous system, you remain in control of your extremeties and still have access to the same senses as everyone else, though " +
				"they sometimes feel a little muted." + bodyText;

		}
		private static string GooTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
			//return "A strange feeling in your stomach forces you to pause. Your " + previousBodyData.LongDescription() + " seems to be getting, well, " +
			//	SafelyFormattedString.FormattedText("soft", StringFormats.ITALIC) + ", for lack of a better word. Stranger still, the feeling isn't limited to your outsides; your " +
			//	"innards seem to be affected as well. Experimentally, you place a " + player.hands.HandText(false) + " to your stomach, and are shocked when it faces " +
			//	"almost no resistance. Strangely, your stomach seems to bend ";
		}
		private static string GooRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Carapace
		protected static string CarapaceDescNoType(out bool isPlural)
		{
			isPlural = false;
			return "carapace";
		}

		private static string CarapaceDesc(bool alternateFormat)
		{
			return Utils.AddArticleIf("carapace-covered body", alternateFormat);
		}
		private static string CarapaceLongDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string CarapaceFullDesc(BodyData body, bool alternateFormat)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string CarapacePlayerStr(Body body, PlayerBase player)
		{
			return "Your body is humanoid in shape and stature, but your hard, almost shell-like outer layer is not something a human would have. Instead of skin, your body is covered in "
				+ "a " + body.mainEpidermis.LongDescription() + ", providing you with a natural defense. It parts around your joints and privates, granting you a full range of motion "
				+ "and allowing you to handle your business without any difficulty,";
		}
		private static string CarapaceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapaceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion

		private static string GenericBodyPlayerDesc(bool notHuman = true)
		{
			if (notHuman)
			{
				return "Your body is humanoid in shape and stature, but with a few key differences from a normal human. ";
			}
			else
			{
				return "Your body is humanoid shape and structure. ";
			}
		}

		private static string GenericFurRestoreText(FurBasedEpidermisType epidermis, PlayerBase player)
		{
			string nailsText = player.hands.NailsText();
			return "Your skin suddenly feels itchy all over. As you scratch it, " + epidermis.ShortDescription(out bool isPlural) + (isPlural ? "get" : "gets") + " stuck beneath your "
				+ nailsText + ". Soon, " + (isPlural ? "they are " : "it is ") + "falling out in large clumps, " +
				SafelyFormattedString.FormattedText("revealing the " + player.body.primarySkin.LongDescription() + "underneath!", StringFormats.BOLD);
		}
		/*
		 if (player.hasFur()) outputText("Your skin suddenly feels itchy as your fur begins falling out in clumps, <b>revealing inhumanly smooth skin</b> underneath.");
 924                     if (player.hasScales()) outputText("Your scales begin to itch as they begin falling out in droves, <b>revealing your inhumanly smooth " + player.skin.tone + " skin</b> underneath.");
		 */
	}

}