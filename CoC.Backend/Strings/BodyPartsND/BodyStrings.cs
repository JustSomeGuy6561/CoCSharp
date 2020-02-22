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
	public partial class NavelPiercingLocation
	{
		private static string TopButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string TopLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BottomButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BottomLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class HipPiercingLocation
	{
		private static string LeftTopButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftTopLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftCenterButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftCenterLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBottomButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftBottomLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightTopButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightTopLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCenterButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightCenterLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBottomButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightBottomLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	//internal static string TopDesc()
	//{
	//	return "top of your belly button";
	//}

	//internal static string BottomDesc()
	//{
	//	return "bottom of your belly button";
	//}

	public partial class CoreTattooLocation
	{
		private static string LeftShoulderbladeButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftShoulderbladeLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftRibsButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftRibsLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftLowerStomachButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string LeftLowerStomachLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightShoulderbladeButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightShoulderbladeLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightRibsButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightRibsLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightLowerStomachButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string RightLowerStomachLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NavelButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NavelLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CoreButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CoreLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullButton()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FullLocation()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
	}

	public partial class FullBodyTattooLocation
	{
		private static string MainLocation()
		{
			throw new NotImplementedException();
		}

		private static string MainButton()
		{
			throw new NotImplementedException();
		}


		private static string AlternateLocation()
		{
			throw new NotImplementedException();
		}

		private static string AlternateButton()
		{
			throw new NotImplementedException();
		}
	}

	public partial class Body
	{
		public static string Name()
		{
			return "Body";
		}

		private string AllNavelPiercingsShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllNavelPiercingsLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllHipPiercingsShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string AllHipPiercingsLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private string HipPiercingsRequirePiercingFetish()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllCoreTattoosShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string AllCoreTattoosLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string FullBodyTattoosShort(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string FullBodyTattoosLong(Creature creature)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
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

		public string LongDescriptionWithoutBody(BodyData body) => LongDescriptionWithoutBody(body, out bool _);
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
			return body.LongDescription(true);
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
					sb.Append(" out of your " + player.armor.ItemName());
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
				sb.Append("You peel back your " + player.armor.ItemName() + " and the transformation has already finished on the rest of your body.");
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

			return GlobalStrings.NewParagraph() + bodyText + legsText + " panting as the pressure increases. Sweat drips from your brow. "
				+ "You don't know how long you can take this and soon you drift into unconsciousness." + Environment.NewLine
				+ "When you awaken you check yourself to see what has changed now that the overwhelming pressure has left your body."
				+ " The first thing you notice is feathers, lots and lots of feathers that now cover your body in a downy layer."
				+ " Just below your neck is a ruff of soft fluffy feathers, like you'd see on an exotic bird. As you look down to your " + player.genitals.ChestOrAllBreastsShort()
				+ ", you see that from your chest to your groin you are covered in a layer of " + player.body.supplementaryEpidermis.LongDescription() + "."
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
			sb.Append("You now have " + SafelyFormattedString.FormattedText(player.body.LongDescription(true), StringFormats.BOLD));

			return sb.ToString();
		}

		//idk if vag of holding is a body perk or a genital perk, but could mention removing it here? idk
		protected static string KitsuneRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			string nailsText = player.hands.NailsText();
			return "Your skin suddenly feels itchy all over, particularly around your patches of fur. As you scratch, you begin to notice some of that fur " +
			"stuck beneath your " + nailsText + ". Soon, the rest of it is falling out in large clumps, " +
				SafelyFormattedString.FormattedText("leaving you with just " + player.body.primarySkin.LongDescription() + "!", StringFormats.BOLD);
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
			return body.main.AdjectiveDescriptionWithoutType(alternateFormat) + ", bark-covered body";
		}

		private static string BarkPlayerStr(Body body, PlayerBase player)
		{
			return $"The {body.mainEpidermis.LongAdjectiveDescription(false)} outer layer of your body contrasts starkly with your form, which remains distinctly humanoid. " +
				$"The thickness varies by location: your vitals are covered in thick, overlapping layers, while your muscles gain a much thinner, flexible, vine-like membrane." +
				$" Your {player.genitals.ChestOrAllBreastsShort()} and {(player.gender == Gender.GENDERLESS ? "nether region" : "genitals")}, on the other hand, only have a paper-thin" +
				$" outer-layer, doing little to ";

		}
		private static string BarkTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			if (previousBodyData.type == BodyType.WOODEN)
			{
				return "";
			}
			else
			{
				return $"A tingling runs up along your {previousBodyData.LongDescription()}. Suddenly, vines start to sprout from your {previousBodyData.ShortEpidermisDescription()} " +
					"and begin to wrap themselves around your form. You begin to struggle against your sudden bonds, but you soon realize it is impossible to fight your own body, " +
					"and decide to paitently wait out the inevitable changes. More foliage flows out of you, eventually covering your entire body. All things considered, " +
					"it's not the worse situation you've found yourself in - you can still breath fine, and the considerable thickness and density of the vines and foliage that prevents " +
					"you from moving also prevents anything from getting to you. After what seems like hours (but was probably just a few moments), the foliage begins to unwind and fall " +
					$"away. Peering down, you notice your recent captivity also caused more persistent changes - your previously {previousBodyData.ShortDescription()} " +
					SafelyFormattedString.FormattedText("is now covered in something resembling bark!", StringFormats.BOLD) + "It varies in thickness and overall appearance, and provides " +
					"some natural protection for your vitals, but otherwise functions like normal skin.";
			}
		}
		private static string BarkRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return "Your usually fresh and solid bark suddenly feels a bit dry. Before you get a chance to process it, it begins peeling away, giving you the rough impression of " +
				"a reptile shedding its skin. You're fairly certain you've never seen fauna do that, though. As if to confirm your observation, your old bark is not replaced with" +
				"a new layer underneath, but instead a layer of normal " + player.body.mainEpidermis.DescriptionWithColor() + "." +
				SafelyFormattedString.FormattedText("You have normal, human skin again!", StringFormats.BOLD);
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
			if (EpidermalData.MixedFurColors(body.main, body.supplementary))
			{
				return Utils.AddArticleIf("multi-colored, furry body. ", alternateFormat);
			}
			else
			{
				return body.main.LongAdjectiveDescription(alternateFormat) + " body";
			}
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
		//there is a bunch of these in vanilla; i've taken bits of all of them and made it something generic. feel free to use your own custom text for each tf item as you see fit.
		private static string FurTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			if (previousBodyData.type == player.body.type)
			{
				return "";
			}
			//handle the generic fur bullshit.

			//only has fur. both had underbody or both didnt have underbody. it's just generic text saying nothing really happened.
			else if (previousBodyData.IsBoth(EpidermisType.FUR) && player.body.IsBoth(EpidermisType.FUR)
				|| (previousBodyData.OnlyHas(EpidermisType.FUR) && player.body.OnlyHas(EpidermisType.FUR)))
			{
				return "Your fur tingles, then shifts slightly. You now have " + player.body.LongEpidermisDescription() + ".";
			}
			//only fur. underbody=> no underbody.
			else if (previousBodyData.IsBoth(EpidermisType.FUR) && player.body.OnlyHas(EpidermisType.FUR))
			{
				return "Your fur tingles, then shifts slightly. Your fur is now just " + player.body.mainEpidermis.JustColor() + ".";
			}
			//only fur. no underbody => underbody
			else if (previousBodyData.OnlyHas(EpidermisType.FUR) && player.body.IsBoth(EpidermisType.FUR))
			{
				return "Your fur tingles, then shifts slightly. You now have " + player.body.LongEpidermisDescription() + ", instead of just" + previousBodyData.LongEpidermisDescription();
			}
			//previously has some fur, but mixed types.
			else if (previousBodyData.HasAny(EpidermisType.FUR))
			{

				string nonFurText;
				EpidermalData nonFurType;
				EpidermalData furType;
				bool nonFurIsPlural;
				if (previousBodyData.main.type == EpidermisType.FUR)
				{
					nonFurText = previousBodyData.SupplementaryDescription(out nonFurIsPlural);
					furType = previousBodyData.main;
					nonFurType = previousBodyData.supplementary;
				}
				else
				{
					nonFurText = previousBodyData.MainDescription(out nonFurIsPlural);
					nonFurType = previousBodyData.main;
					furType = previousBodyData.supplementary;
				}

				string furGrowText;

				if (nonFurType.usesFurColor)
				{
					furGrowText = "Your " + nonFurText + (nonFurIsPlural ? "fall " : "falls ") + "out, no doubt forced by the fur that is now growing in " +
						(nonFurIsPlural ? "their " : "its ") + "place. This new fur blends in with the " + furType.DescriptionWithColor() + " your already have, and " +
						"fur now covers your entire body.";
				}
				else if (nonFurType.type != EpidermisType.SKIN && nonFurType.type != EpidermisType.GOO)
				{
					furGrowText = "As you scatch, you begin to notice " + nonFurType.type.ShortSingleItemDescription() + " coming away, leaving just skin behind. " +
						"Fur grows in shortly afterward, matching the fur your already have. Soon, you have fur covering all of your body.";
				}
				else
				{
					furGrowText = "The feeling intensifies, though your fur is strangely unaffected. As you scratch yourself madly, you notice your fur spreading, growing in until " +
						"your entire body is covered.";
				}

				string colorChange = "";
				//both colors are different (if applicable)
				if (!furType.fur.Equals(player.body.mainEpidermis.fur) && (!player.body.hasSecondaryEpidermis || !furType.fur.Equals(player.body.supplementaryEpidermis.fur)))
				{
					colorChange = ", then changes color. ";
				}
				//has two colors and they are both different.
				else if (!furType.fur.Equals(player.body.mainEpidermis.fur) || (player.body.hasSecondaryEpidermis && !furType.fur.Equals(player.body.supplementaryEpidermis.fur)))
				{
					colorChange = ", then partially changes color. ";
				}
				else
				{
					colorChange = ", but otherwise remains the same. ";
				}
				return "Your body itches all over. " + furGrowText + " It shifts slightly, growing in some places while shortening in others" + colorChange + "Once you're certain the " +
					"changes are complete, you look yourself over and realize " + SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() +
					"covering your body!", StringFormats.BOLD);
			}
			//no fur.
			//Handle Goo and Skin cases because they're unique like that.
			else if (previousBodyData.type == HUMANOID)
			{
				return "Your skin itches all over, the sudden intensity and uniformity making you too paranoid to scratch. As you hold still through an agony of tiny tingles and pinches, " +
					" fur sprouts from every bare inch of your skin! " + SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() +
					" covering your body!", StringFormats.BOLD) + " Looks like you'll have to get used to being furry...";
			}
			else if (previousBodyData.type == GOO)
			{
				return "Your gooey core tingles. Looking down, you notice it becoming less and less opaque, shifting back until " +
					SafelyFormattedString.FormattedText("you have a normal body once again.", StringFormats.BOLD) + " Despite this, the sensation intensifies until it feels as though" +
					" you itch all over. Paranoid your newly formed skin is still too tender and new to scratch, you simply wait in agony for the feeling to pass. " +
					"Just as you're about to give in and risk damaging your sensative skin, it's replaced with a faint tugging sensation as fur grows in until it covers your body. " +
					SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() + "!", StringFormats.BOLD)
					+ " Looks like you'll have to get used to being furry...";
			}
			//everything else.
			else
			{
				return $"Your {previousBodyData.ShortEpidermisDescription(out bool isPlural)} {(isPlural ? "begin" : "begins")} to itch insufferably. You reflexively scratch yourself," +
					$"and notice your {previousBodyData.main.ShortSingleItemDescription()} " + (previousBodyData.hasSupplementaryEpidermis ? "and " +
					previousBodyData.supplementary.ShortDescription() : "") + "falling away, leaving only the skin beneath. The itching intensifies until you notice a coat of " +
					"fur behind. " + SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() + " covering your body!", StringFormats.BOLD);
			}
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
			return "downy short-feathers";
		}
		private static string FeatherLongDesc(BodyData body, bool alternateFormat)
		{
			return body.main.AdjectiveDescription(alternateFormat) + "body" + (EpidermalData.MixedFurColors(body.main, body.supplementary) ? "with " + body.supplementary.JustColor() +
				UnderFeatherDesc() : "");
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
		//never used in game. i'm just taking fur and reusing it. feel free to change it.
		private static string FeatherTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			if (previousBodyData.type == FEATHERED)
			{
				return "";
			}
			else if (previousBodyData.type == HUMANOID)
			{
				return "Your skin itches all over, the sudden intensity and uniformity making you too paranoid to scratch. As you hold still through an agony of tiny tingles and pinches, " +
					" row after row of feathers sprout from your skin, until it's completely covered. " + SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() +
					" covering your body!", StringFormats.BOLD);
			}
			else if (previousBodyData.type == GOO)
			{
				return "Your gooey core tingles. Looking down, you notice it becoming less and less opaque, shifting back until " +
					SafelyFormattedString.FormattedText("you have a normal body once again.", StringFormats.BOLD) + " Despite this, the sensation intensifies until it feels as though" +
					" you itch all over. Paranoid your newly formed skin is still too tender and new to scratch, you simply wait in agony for the feeling to pass. " +
					"Just as you're about to give in and risk damaging your sensative skin, it's replaced with a faint tugging sensation as feathers grow in until they cover your body. " +
					SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() + "!", StringFormats.BOLD);
			}
			else if (previousBodyData.OnlyHas(EpidermisType.FEATHERS))
			{
				return "Your feathers tingle, shifting slightly. You now have " + player.body.LongEpidermisDescription() + ".";
			}
			else if (previousBodyData.HasAny(EpidermisType.FEATHERS))
			{
				string nonFeatherText;
				EpidermalData nonFeatherType;
				EpidermalData featherType;
				bool nonFurIsPlural;
				if (previousBodyData.main.type == EpidermisType.FEATHERS)
				{
					nonFeatherText = previousBodyData.SupplementaryDescription(out nonFurIsPlural);
					featherType = previousBodyData.main;
					nonFeatherType = previousBodyData.supplementary;
				}
				else
				{
					nonFeatherText = previousBodyData.MainDescription(out nonFurIsPlural);
					nonFeatherType = previousBodyData.main;
					featherType = previousBodyData.supplementary;
				}

				string featherGrowText;

				if (nonFeatherType.usesFurColor)
				{
					featherGrowText = "Your " + nonFeatherText + (nonFurIsPlural ? "fall " : "falls ") + "out, no doubt forced by the feathers that are now growing in " +
						(nonFurIsPlural ? "their " : "its ") + "place. The new feathers blend in with the " + featherType.DescriptionWithColor() + " your already have, and " +
						"feathers now cover your entire body.";
				}
				else if (nonFeatherType.type != EpidermisType.SKIN && nonFeatherType.type != EpidermisType.GOO)
				{
					featherGrowText = "As you scatch, you begin to notice " + nonFeatherType.type.ShortSingleItemDescription() + " coming away, leaving just skin behind. " +
						"Feathers grow in shortly afterward, matching the feathers your already have. Soon, you have feathers covering all of your body.";
				}
				else
				{
					featherGrowText = "The feeling intensifies, though your feathers are strangely unaffected. As you scratch yourself madly, you notice your feathers spreading, " +
						"growing in until your entire body is covered.";
				}

				string colorChange = "";
				//both colors are different (if applicable)
				if (!featherType.fur.Equals(player.body.mainEpidermis.fur) && (!player.body.hasSecondaryEpidermis || !featherType.fur.Equals(player.body.supplementaryEpidermis.fur)))
				{
					colorChange = ", then changes color. ";
				}
				//has two colors and they are both different.
				else if (!featherType.fur.Equals(player.body.mainEpidermis.fur) || (player.body.hasSecondaryEpidermis && !featherType.fur.Equals(player.body.supplementaryEpidermis.fur)))
				{
					colorChange = ", then partially changes color. ";
				}
				else
				{
					colorChange = ", but otherwise remains the same. ";
				}
				return "Your body itches all over. " + featherGrowText + " They shifts slightly, growing in some places while shortening in others" + colorChange +
					"Once you're certain the changes are complete, you look yourself over and realize " + SafelyFormattedString.FormattedText("You now have " +
					player.body.LongEpidermisDescription() + " covering your body!", StringFormats.BOLD);
			}
			//everything else.
			else
			{
				return $"Your {previousBodyData.ShortEpidermisDescription(out bool isPlural)} {(isPlural ? "begin" : "begins")} to itch insufferably. You reflexively scratch yourself," +
					$"and notice your {previousBodyData.main.ShortSingleItemDescription()} " + (previousBodyData.hasSupplementaryEpidermis ? "and " +
					previousBodyData.supplementary.ShortDescription() : "") + "falling away, leaving only the skin beneath. The itching intensifies, finally stopping as row after row " +
					"of feathers grow in, until your entire body is covered. " + SafelyFormattedString.FormattedText("You now have " + player.body.LongEpidermisDescription() + "!",
					StringFormats.BOLD);
			}
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
			return body.main.AdjectiveDescription(alternateFormat) + "body" + (EpidermalData.MixedFurColors(body.main, body.supplementary) ? "with " + body.supplementary.JustColor(true) +
				" underbody" : "");
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

		//old text didnt really handle scales. i'm just magically ignoring it, and lampshading by making it disappear.
		private static string WoolTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			//silently return with wool-> wool.
			if (previousBodyData.type == WOOL || previousBodyData.OnlyHas(EpidermisType.WOOL))
			{
				return "";
			}

			//handle any type specific intros we need to explain how we're getting the fur.
			string intro;
			//handle the actual growing of said fur.
			string growWoolText;

			if (previousBodyData.HasAny(EpidermisType.WOOL))
			{
				intro = "";
			}
			//fur or feathers. (or anything that uses fur type)
			else if (previousBodyData.hasActiveFurType)
			{
				if (previousBodyData.OnlyHas(EpidermisType.FUR))
				{
					intro = "You feel your fur suddenly stand on end, every follicle suddenly detaching and leaving your skin bare. As you stand with a pile of shed fur around your feet, " +
						"you feel your entire body tingle, and you're sure it isn't from the sudden exposure. ";
				}
				else if (previousBodyData.OnlyHas(EpidermisType.FEATHERS))
				{
					intro = "Something seems to rustle your feathers, but you can't find an external source. They suddenly fall out, leaving your skin bare. As you stand, " +
						"surrounded by shed feathers, you feel your entire body tingle, and you're sure it isn't from the cold. ";
				}
				//mixed type or something not implemented when this was written.
				else
				{
					intro = "You feel a tingling sensation in your " + previousBodyData.ShortEpidermisDescription(out bool isPlural) + "and watch " + (isPlural ? "them" : "it") +
						" fall out around you. Your now bare skin continues to tingle, and you're sure it isn't due to the sudden exposure. ";
				}
			}
			else if (previousBodyData.type == GOO)
			{
				intro = "Your gelatenous form begins to solidify, reforming and losing its translucency and flexibility. Once you've regained a more human composition," +
					" you begin to notice a tingling sensation from most of your body. at first, you think it might just be your body getting used to feeling the elements on your skin, " +
					"but soon realize that's not the case. ";
			}
			//skin or scales or whatever. intro is just going to be left alone. we're gonna do it afterward instead.
			else
			{
				intro = "";
			}

			//handle weird case with partial wool. should never happen, but could in the future i guess.
			if (previousBodyData.HasAny(EpidermisType.WOOL))
			{
				string otherText = previousBodyData.main.type == EpidermisType.WOOL ? previousBodyData.SupplementaryDescription() : previousBodyData.MainDescription();

				growWoolText = " With an almost audible " + SafelyFormattedString.FormattedText("POMF", StringFormats.ITALIC) + ", a soft fleece erupts from your " + otherText +
					", matching the wool you already have. Soon, your entire body is covered in a woolen fleece. ";
			}
			else
			{
				growWoolText = " With an almost audible " + SafelyFormattedString.FormattedText("POMF", StringFormats.ITALIC) + ", a soft fleece erupts from your body. ";
			}

			string coreText = EpidermalData.MixedFurColors(player.body.mainEpidermis, player.body.supplementaryEpidermis)
				? ", and shifts to a distinct " + player.body.supplementaryEpidermis.JustColor() + ". It doesn't fully hide your sexual features, instead obscuring "
				: ", but it doesn't fully hide your sexual features - instead, it obscures ";

			string lampshadeScalesOrWhatever = "";
			//previously had something that used a tone but wasn't skin or goo. (goo becomes skin before we handle it. )
			if ((previousBodyData.main.usesTone && previousBodyData.main.isNotSkinOrGoo) ||
				(previousBodyData.hasSupplementaryEpidermis && previousBodyData.supplementary.usesTone && previousBodyData.supplementary.isNotSkinOrGoo))
			{
				//both weren't skin or goo
				if (previousBodyData.main.usesTone && previousBodyData.main.isNotSkinOrGoo && previousBodyData.hasSupplementaryEpidermis &&
					previousBodyData.supplementary.usesTone && previousBodyData.supplementary.isNotSkinOrGoo)
				{
					lampshadeScalesOrWhatever = "Strangely, when you check under your wool, you notice your " + previousBodyData.LongEpidermisDescription(out bool isPlural) +
						(isPlural ? "are" : "is") + " gone; you only have " + player.body.primarySkin.DescriptionWithColor() + " underneath your new wool. Weird.";
				}
				//supplementary only
				else if (previousBodyData.hasSupplementaryEpidermis && previousBodyData.supplementary.usesTone && previousBodyData.supplementary.isNotSkinOrGoo)
				{
					lampshadeScalesOrWhatever = "Strangely, when you check to see what happened to your " + previousBodyData.SupplementaryDescription(out bool isPlural) +
						" after " + (isPlural ? "they were" : "it was") + " covered in wool, you notice " + (isPlural ? "they are" : "it is") + "gone; you only " +
						"have " + player.body.primarySkin.DescriptionWithColor() + " in its place. Weird.";
				}
				//primary only. (has supplementary)
				else if (previousBodyData.hasSupplementaryEpidermis)
				{
					lampshadeScalesOrWhatever = "Strangely, when you check to see what happened to your " + previousBodyData.MainDescription(out bool isPlural) +
						" after " + (isPlural ? "they were" : "it was") + " covered in wool, you notice " + (isPlural ? "they are" : "it is") + "gone; you only " +
						"have " + player.body.primarySkin.DescriptionWithColor() + " in its place. Weird.";
				}
				//whole body (no supplementary)
				else
				{
					lampshadeScalesOrWhatever = "Strangely, when you check under your wool, you notice your " + previousBodyData.LongEpidermisDescription(out bool isPlural) +
						(isPlural ? "are" : "is") + " gone; you only have " + player.body.primarySkin.DescriptionWithColor() + " underneath your new wool. Weird.";
				}
			}

			return intro + growWoolText + "The fleece covers most of your body, and is incredibly thick and fluffy. It's less thick around your " + player.genitals.ChestOrAllBreastsLong()
				+ "and nethers" + coreText + "them in an enticing manner. You can't help but run your hands over your " + player.body.LongEpidermisDescription() +
				", reveling in plushness. " + SafelyFormattedString.FormattedText("You now have sheep wool!", StringFormats.BOLD) + lampshadeScalesOrWhatever;
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
			return body.main.LongAdjectiveDescription(alternateFormat) + " body";
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
			if (previousBodyData.type == GOO)
			{
				return "";
			}
			else
			{
				return "A strange feeling in your stomach forces you to pause. Your " + previousBodyData.LongDescription() + " seems to be getting, well, " +
					SafelyFormattedString.FormattedText("soft", StringFormats.ITALIC) + ", for lack of a better word. Stranger still, the feeling isn't limited to your outsides; your " +
					"innards seem to be affected as well. Experimentally, you place a " + player.hands.HandText(false) + " to your stomach, and are shocked when it faces " +
					"almost no resistance. Strangely, your stomach seems to bend inward, as if made of some elastic substance. As you pull back your " + player.hands.HandText(false) +
					", your core returns to its normal shape, but bits of a gelatenous substance remain on your " + player.hands.NailsText() + ". Huh. Looking back down at your body, " +
					"you notice it's now made entirely of this strange, semi-transparent substance. With a start, you realize " +
					SafelyFormattedString.FormattedText("you have a flexible, gooey body!", StringFormats.BOLD) + " You're a bit worried when you can't seem to see any internal organs, " +
					"but considering you can still feel, and breathe, you guess you're fine. Besides, this elasticity could really come in handy!";
			}
		}
		private static string GooRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return "Your gooey core tingles. Looking down, you notice it becoming less and less opaque, shifting back until " +
				SafelyFormattedString.FormattedText("you have a normal body once again!", StringFormats.BOLD) + " It takes a moment, but you begin to relearn how to use your " +
				"now human organs and muscles, which are notably not as flexible as their gooey predecessors, though you are otherwise no worse for wear.";
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
			return body.main.LongAdjectiveDescription(alternateFormat) + " body";
		}

		private static string CarapacePlayerStr(Body body, PlayerBase player)
		{
			return "Your body is humanoid in shape and stature, but your hard, almost shell-like outer layer is not something a human would have. Instead of skin, your body is covered in "
				+ "a " + body.mainEpidermis.LongDescription() + ", providing you with a natural defense. It parts around your joints and privates, granting you a full range of motion "
				+ "and allowing you to handle your business without any difficulty,";
		}

		//magic! because it's not exactly in game, but could be i guess. your skin just hardens into a shell. if you have fur or whatever, it falls out because there's no pores anymore.
		private static string CarapaceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			if (previousBodyData.type == CARAPACE)
			{
				return "";
			}
			else
			{
				string skinDesc = null;
				string handleOldTypeFurOrGoo = "";
				if (previousBodyData.hasActiveFurType)
				{



					string furTypeText;
					bool furTextPlural;
					if (previousBodyData.main.usesFurColor && (previousBodyData.supplementary.usesFurColor || previousBodyData.supplementary.isEmpty))
					{
						furTypeText = previousBodyData.LongEpidermisDescription(out furTextPlural);
						skinDesc = previousBodyData.mainSkin.LongDescription();

						skinDesc = "the " + previousBodyData.mainSkin.LongDescription() + "under your " + previousBodyData.ShortEpidermisDescription();
					}
					else if (previousBodyData.main.usesFurColor)
					{
						furTypeText = previousBodyData.MainDescription(out furTextPlural);
					}
					else
					{
						furTypeText = previousBodyData.SupplementaryDescription(out furTextPlural);
					}

					handleOldTypeFurOrGoo = "The now solid surfaces pushes away your " + furTypeText + ", which " + (furTextPlural ? "fall" : "falls") + " to the ground around you. ";
				}
				else if (previousBodyData.type == GOO)
				{
					handleOldTypeFurOrGoo = "The change also radiates inwards, completely restoring you to a non-gelatenous form. ";
				}

				if (skinDesc is null)
				{
					skinDesc = "your " + previousBodyData.mainSkin.LongDescription();
				}

				return "Your body tingles with an unknown sensation. Suddenly, " + skinDesc + " hardens, forming together into a hard, unblemished surface. " + handleOldTypeFurOrGoo +
					"It thickens, and becomes slightly reflective. After it finishes, you realize "
					+ SafelyFormattedString.FormattedText("You now have a sturdy, external carapace!", StringFormats.BOLD) + " It looks a little strange on your otherwise humanoid body, " +
					"but the natural protection it will provide helps negate that fact.";
			}
		}
		private static string CarapaceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			return "Your body begins to feel strange. After a few seconds, you notice the hard outer layer that is your carapace is begining to flake off. Bits and pieces " +
				"continue to fall away, revealing a more natural skin beneath. The process slows, and some pieces remain stubbornly attached, like a scab, of sorts. You're sure" +
				"the rest would fall away over some period of time, but it begins to itch and you'd rather just be done with it. You scratch away the remaining carapace, " +
				SafelyFormattedString.FormattedText("leaving you with completely " + player.body.mainEpidermis.LongDescription() + "!", StringFormats.BOLD);
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