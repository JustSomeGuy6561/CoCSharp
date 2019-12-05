//BodyStrings.cs
//Description:
//Author: JustSomeGuy
//1/4/2019, 8:22 PM
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Settings.Gameplay;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{

	public partial class Body
	{
		public static string Name()
		{
			return "Body";
		}

		private string MultiDye(HairFurColors dyeColor, HashSet<byte> indices)
		{
			throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string SingleDye(HairFurColors dyeColor, byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string MultiLotions(SkinTexture lotionTexture, HashSet<byte> indices)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private string SingleLotion(SkinTexture lotionTexture, byte index)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		public bool IsFurry()
		{
			return type.epidermisType.usesFur;
		}
	}

	public partial class BodyType
	{
		#region Generics

		protected static string BodyDesc()
		{
			return "Body";
		}

		protected static string Body2Desc()
		{
			return "Body 2";
		}

		protected static string YourBodyDesc()
		{
			return " your body";
		}

		protected static string PartsOfFurPatternDesc()
		{
			return " parts of your fur to form a pattern";
		}

		protected static string UnderBodyDesc()
		{
			return "Underbody";
		}

		protected static string YourUnderBodyDesc()
		{
			return " your underbody";
		}

		protected static SimpleDescriptor YourDescriptor(EpidermisType epidermisType)
		{
			return () => YourDesc(epidermisType);
		}

		private static string YourDesc(EpidermisType epidermisType)
		{
			return " your " + epidermisType.ShortDescription;
		}
		#endregion
		//primary fur: fur
		//secondary fur: fur on your underbody

		//but during apply land, it's fur|feathers on your underside. TY GAME!

		//primary tone: body
		//secondary tone: underbody

		//Apply <dye color> dye to the <this function>?
		//The <this function> is aleady <dye color>.
		//You applied the dye to <this function>. It is now <dye color>
		private string BodyDyeDesc()
		{
			//if (this.)
			return "the " + epidermisType.ShortDescription() + " covering your body";
		}

		private string BodyToneDesc()
		{
			return "your " + secondaryEpidermisType.ShortDescription();
		}


		private string UnderBodyDyeDesc()
		{
			return secondaryEpidermisType.ShortDescription() + " covering your underbody";
		}

		//apply <tone color> lotion to <this function>?
		//
		private string UnderBodyToneDesc()
		{
			return "the " + secondaryEpidermisType.ShortDescription() + " on your underside";
		}

		#region Skin
		private static string SkinDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinLongDesc(BodyData body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinPlayerStr(Body body, PlayerBase player)
		{
			//feel free to rephrase this, but you'd need to store the "acceptable/normal" skin tones for a human, then say something like, 
			return GenericBodyPlayerDesc(false) + " You have " + body.mainEpidermis.LongDescription();
		}
		private static string SkinTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string SkinRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Scales
		private static string ScalesDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesLongDesc(BodyData body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesPlayerStr(Body body, PlayerBase player)
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
		private static string ScalesTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string ScalesRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderScalesDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Naga
		private static string NagaDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaLongDesc(BodyData body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaPlayerStr(Body body, PlayerBase player)
		{
			if (EpidermalData.MixedTones(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + "Your body is covered in a blend of scales - most of it is " + body.mainEpidermis.DescriptionWithoutType(true) + ", but it shifts to " +
					body.supplementaryEpidermis.DescriptionWithoutType(true) + " around your lower half and along parts of your stomach.";
			}
			else if (EpidermalData.MixedTextures(body.mainEpidermis, body.supplementaryEpidermis))
			{
				return GenericBodyPlayerDesc() + "Your body is covered in snake-like scales. Most of the them are " + body.mainEpidermis.JustTexture(true) + "but the lower ones, along with some near your" +
					"stomach, are " + body.supplementaryEpidermis.JustTexture(true);
			}
			else
			{
				return GenericBodyPlayerDesc() + "Your body is covered in " + body.mainEpidermis.AdjectiveDescriptionWithoutType() + "snake-like scales. ";
			}
		}
		private static string NagaTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string NagaRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderNagaDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Cockatrice
		protected static string CockatriceDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceLongDesc(BodyData body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatricePlayerStr(Body body, PlayerBase player)
		{
			return GenericBodyPlayerDesc(false) + " You've got a thick layer of " + body.mainEpidermis.LongDescription() + " covering most of your body, while "
				+ body.supplementaryEpidermis.LongDescription() + "coat you from chest to groin.";
		}
		protected static string CockatriceTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string CockatriceRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Kitsune
		protected static string KitsuneDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsuneLongDesc(BodyData body)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		protected static string KitsunePlayerStr(Body body, PlayerBase player)
		{
			return GenericBodyPlayerDesc(false) + $"{body.supplementaryEpidermis.LongDescription()} covers parts of your body, though much of your {body.mainEpidermis.LongDescription()}" +
				"remains exposed for a distinctly kitsune look.";
		}
		protected static string KitsuneTransformStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
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
		private static string BarkDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string BarkLongDesc(BodyData body)
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
		private static string FurDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FurLongDesc(BodyData body)
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
					+$" appears {body.supplementaryEpidermis.JustTexture()}, contrasting with the rest, which appears {body.mainEpidermis.JustTexture()}. Despite this, it provides little "
					+$"in terms of modesty as the fur around your stomach and {player.genitals.AllBreastsLongDescription()} is significantly shorter than the rest.";
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderFurDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Feathers
		private static string FeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string UnderFeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string FeatherLongDesc(BodyData body)
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderFeatherDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Wool
		private static string WoolDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolUnderbodyDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string WoolLongDesc(BodyData body)
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
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}

		private static string YourUnderWoolDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Goo
		private static string GooDesc()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string GooLongDesc(BodyData body)
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
		}
		private static string GooRestoreStr(BodyData previousBodyData, PlayerBase player)
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		#endregion
		#region Carapace
		private static string CarapaceStr()
		{
			throw new InDevelopmentExceptionThatBreaksOnRelease();
		}
		private static string CarapaceLongDesc(BodyData body)
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
	}

}
