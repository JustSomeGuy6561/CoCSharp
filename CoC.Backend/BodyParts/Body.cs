//Body.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Races;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	//Trying to come up with decent logic for this thing is a pain in the ass. This is maybe the 5th revision. I think it works well enough now. 

	//First off, we store two "Main" elements: Fur and Skin. Fur stores an epidermis that uses fur colors (fur or feathers in current implementation). 
	//Skin stores an epidermis dealing with Tone Colors (skin, scales, carapace, goo, rubber, etc). For the most part, body parts that make use of one main will not use the other.
	//These will be given priority when dyeing or toneing. 

	//We also have two Epidermis instances: primary and secondary. These can be any epidermis type (though the primary cannot be empty). The body type determines what these values store
	//For example: A fur-based body type will set the primary to the mainFur, and secondary to a new fur instance if it has an underbody. Similarly, a tone-based body type will set primary
	//to mainSkin, and set the secondary to a new tone-based instance if it has an underbody. stranger things occur with mixed types; cockatrice and kitsune both use mainFur and mainSkin;
	//for cockatrice, feathers are dominant, so primary is mainFur, secondary is mainSkin. for Kitsune, the opposite is true.

	//this allows the best of both worlds - we can tell body parts to respond to the fur or skin first, based on which is primary, but when attempting to change the skin tone or fur color,
	//we dont have to find out where the main skin or fur is stored. similarly, if a body part needs a fur color, and does not care if it's primary or not, we can simply call the main fur.
	//It also allows cases where body parts need the skinTone, but the body is covered in fur. Technically, there is skin under the fur, so the mainSkin equates to that.
	//This does lead to a weird caveat where we could potentially double proc an effect if we apply it to both the mainFur/skin and the secondary, and the secondary references the mainFur/Skin.
	//fortunately, we do have a few ways to detect that, the simplest being ReferenceEquals. ReferenceEquals(a, b) is true if a and b point to the same object.

	//finally, there are helper booleans for the main skin and fur telling whether or not they are active in the current body type. this allows for even more complicated behavior, if that's what you want. 

	//Quick Aside: Fur color supports two colors at once, so it's possible to do something like a cat with orange and grey stripes, and a white underbody.
	//Tones, however, only have one, as the original logic only ever had one tone. If you need two, see if you can get away with just using main and suppliment.
	//if not, i could add it in, though that would require some rewrite - basically, it would require a new ToneColor class, which works the same as the FurColor class. 
	//all references to Tones would need to be changed to ToneColor, made readonly, and passed by value instead of by reference where applicable - not hard, just tedious.

	//Also worth noting: We do weird shit with dyes - because we allow dual-colored fur/feathers, we need the ability to create this multicolored magic with dyes, but also need to provide the option
	//to dye to a single color. Doing this in code is easy - displaying it, not so much. As such, a custom version of text is applied, as well as the patternable interface. It's conviluted, ik. i'm sorry.

	//Final Note: Validation assumes that the data is properly set during serialization - that is, the body type's Init was called, or, barring that, mainFur and mainSkin were at least initialized correctly.

	public enum NavelPiercingLocation { TOP, BOTTOM }

	public sealed partial class Body : BehavioralSaveablePart<Body, BodyType>, IHairAware, IMultiDyeableCustomText, IPatternable, ISimultaneousMultiToneable, IMultiLotionableCustomText
	{


		private const JewelryType AVAILABLE_NAVEL_PIERCINGS = JewelryType.HORSESHOE | JewelryType.DANGLER | JewelryType.RING | JewelryType.BARBELL_STUD | JewelryType.SPECIAL;
		//Hair, Fur, Tone
		//private HairFurColors hairColor => hairData().hairColor;

		private HairFurColors activeHairColor
		{
			get
			{
				HairData data = hairData();
				return data.hairDeactivated ? HairFurColors.NO_HAIR_FUR : data.hairColor;
			}
		}

		public EpidermalData mainEpidermis => primary.GetEpidermalData();
		public EpidermalData supplementaryEpidermis => secondary.GetEpidermalData();


		//private readonly Epidermis primaryEpidermis;
		//private readonly Epidermis secondaryEpidermis;

		public readonly Piercing<NavelPiercingLocation> navelPiercings;

		//use these if the part does not care on the state of the body, but just needs the main color. 

		private readonly Epidermis mainFur; //stores the current fur that is primarily used. if it's a multi-fur, the secondary fur is stored in supplementary epidermis. if it's no-fur, this is empty.
		public bool furActive => ReferenceEquals(mainFur, primary) || ReferenceEquals(mainFur, secondary);
		private readonly Epidermis mainSkin; //stores the current skin that is primarily used. if it's multi-tone, the secondary tone is stored in the supplementary epidermis.
		private bool skinActive => ReferenceEquals(mainSkin, primary) || ReferenceEquals(mainSkin, secondary);

		private Epidermis primary;
		private Epidermis secondary;


		public override BodyType type { get; protected set; }

		public override bool isDefault => type == BodyType.HUMANOID;

		private Body(BodyType bodyType)
		{
			type = bodyType ?? throw new ArgumentNullException();

			bodyType.Init(out mainSkin, out mainFur, out primary, out secondary);

			navelPiercings = new Piercing<NavelPiercingLocation>(AVAILABLE_NAVEL_PIERCINGS, PiercingLocationUnlocked);
		}

		//called after deserialization. We're making the following assumptions: If the bodyType is not null, its Init was called. this should be true, because we control
		//deserialization, though i suppose if we use a method of serialization that doesn't call constructors, that wouldn't apply. Honestly though, using a serialization technique that
		//doesnt call constructors would invite so many problems due to readonlys that it wouldn't be all that smart. If the bodyType is null, assumes mainSkin and mainFur are not null.
		//also assumes the hairData getter has been connected already. 

		internal override bool Validate(bool correctInvalidData)
		{
			BodyType bodyType = type;
			bool valid = BodyType.Validate(ref bodyType, mainSkin, mainFur, ref primary, ref secondary, hairData(), correctInvalidData);
			type = bodyType;
			if (valid || correctInvalidData)
			{
				valid &= navelPiercings.Validate(correctInvalidData);
			}
			return valid;
		}

		private bool PiercingLocationUnlocked(NavelPiercingLocation piercingLocation)
		{
			return true;
		}

		internal BodyData ToBodyData()
		{
			return new BodyData(primary, secondary, mainFur, mainSkin, hairData(), type);
		}

		#region Generate
		internal static Body GenerateDefault()
		{
			return new Body(BodyType.HUMANOID);
		}
		internal static Body GenerateDefaultOfType(BodyType bodyType)
		{
			return new Body(bodyType);
		}

		internal static Body GenerateCockatrice(FurColor featherColor, Tones scaleColor, FurTexture featherTexture = FurTexture.NONDESCRIPT, SkinTexture scaleTexture = SkinTexture.NONDESCRIPT)
		{
			//initializes all the main fur/skin/primary/secondary.
			Body retVal = new Body(BodyType.COCKATRICE);

			//update the main fur and skin.
			if (!FurColor.IsNullOrEmpty(featherColor))
			{
				retVal.mainFur.ChangeFur(featherColor);
			}
			retVal.mainFur.ChangeTexture(featherTexture);
			if (!Tones.IsNullOrEmpty(scaleColor))
			{
				retVal.mainSkin.ChangeTone(scaleColor);
			}
			retVal.mainSkin.ChangeTexture(scaleTexture);

			return retVal;
		}

		internal static Body GenerateKitsune(Tones skinTone, FurColor furColor, SkinTexture skinTexture = SkinTexture.NONDESCRIPT, FurTexture furTexture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(BodyType.KITSUNE);
			//the constructor automatically initializes these to default values. this overrides them if valid
			if (!FurColor.IsNullOrEmpty(furColor))
			{
				retVal.mainFur.ChangeFur(furColor);
			}
			retVal.mainFur.ChangeTexture(furTexture);

			if (!Tones.IsNullOrEmpty(skinTone))
			{
				retVal.mainSkin.ChangeTone(skinTone);
			}
			retVal.mainSkin.ChangeTexture(skinTexture);
			return retVal;
		}

		internal static Body GenerateTonedNoUnderbody(SimpleToneBodyType toneBody, Tones tone, SkinTexture texture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (!Tones.IsNullOrEmpty(tone))
			{
				retVal.mainSkin.ChangeTone(tone);
			}
			retVal.mainSkin.ChangeTexture(texture);
			return retVal;
		}
		internal static Body GenerateToneWithUnderbody(CompoundToneBodyType toneBody, Tones primaryTone, Tones secondaryTone,
			SkinTexture primaryTexture = SkinTexture.NONDESCRIPT, SkinTexture secondaryTexture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (!Tones.IsNullOrEmpty(primaryTone))
			{
				retVal.mainSkin.ChangeTone(primaryTone);
			}
			retVal.mainSkin.ChangeTexture(primaryTexture);

			if (!Tones.IsNullOrEmpty(secondaryTone))
			{
				retVal.secondary.ChangeTone(secondaryTone);
			}
			retVal.secondary.ChangeTexture(secondaryTexture);

			return retVal;
		}

		internal static Body GenerateFurredNoUnderbody(SimpleFurBodyType furryBody, FurColor primaryFur, FurTexture texture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!FurColor.IsNullOrEmpty(primaryFur))
			{
				retVal.mainFur.ChangeFur(primaryFur);
			}
			retVal.mainFur.ChangeTexture(texture);
			return retVal;
		}
		internal static Body GenerateFurredWithUnderbody(CompoundFurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur,
			FurTexture primaryTexture = FurTexture.NONDESCRIPT, FurTexture secondaryTexture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!FurColor.IsNullOrEmpty(primaryFur))
			{
				retVal.mainFur.ChangeFur(primaryFur);
			}
			retVal.mainFur.ChangeTexture(primaryTexture);
			if (!FurColor.IsNullOrEmpty(secondaryFur))
			{
				retVal.secondary.ChangeFur(secondaryFur);
			}
			retVal.secondary.ChangeTexture(secondaryTexture);
			return retVal;
		}
		#endregion
		#region Updates

		internal bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return UpdateBody(cockatriceBodyType, null, null, featherTexture, scaleTexture);
		}
		internal bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			//i'll write that code every time. so irksome. just do it for me, ok?
			if (!UpdateHelper(cockatriceBodyType))
			{
				return false;
			}

			if (!Tones.IsNullOrEmpty(scaleTone))
			{
				mainSkin.ChangeTone(scaleTone);
			}
			if (scaleTexture != null)
			{
				mainSkin.ChangeTexture((SkinTexture)scaleTexture);
			}
			if (!FurColor.IsNullOrEmpty(featherColor))
			{
				mainFur.ChangeFur(featherColor);
			}
			if (featherTexture != null)
			{
				mainFur.ChangeTexture((FurTexture)featherTexture);
			}

			type = cockatriceBodyType;
			return true;
		}

		internal bool UpdateBody(KitsuneBodyType kitsuneBodyType, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return UpdateBody(kitsuneBodyType, null, null, skinTexture, furTexture);
		}
		internal bool UpdateBody(KitsuneBodyType kitsuneBodyType, Tones skinTone, FurColor furColor, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			//i'll write that code every time. so irksome. just do it for me, ok?
			if (!UpdateHelper(kitsuneBodyType))
			{
				return false;
			}

			if (!Tones.IsNullOrEmpty(skinTone))
			{
				mainSkin.ChangeTone(skinTone);
			}
			if (skinTexture != null)
			{
				mainSkin.ChangeTexture((SkinTexture)skinTexture);
			}
			if (!FurColor.IsNullOrEmpty(furColor))
			{
				mainFur.ChangeFur(furColor);
			}
			if (furTexture != null)
			{
				mainFur.ChangeTexture((FurTexture)furTexture);
			}


			type = kitsuneBodyType;
			return true;
		}

		internal bool UpdateBody(SimpleFurBodyType furryType, FurTexture? furTexture = null)
		{
			return UpdateBody(furryType, null, furTexture);
		}
		internal bool UpdateBody(SimpleFurBodyType furryType, FurColor furColor, FurTexture? furTexture = null)
		{
			//i'll write that code every time. so irksome. just do it for me, ok?
			if (!UpdateHelper(furryType))
			{
				return false;
			}

			if (!FurColor.IsNullOrEmpty(furColor))
			{
				mainFur.ChangeFur(furColor);
			}
			if (furTexture != null)
			{
				mainFur.ChangeTexture((FurTexture)furTexture);
			}

			type = furryType;
			return true;
		}

		internal bool UpdateBody(CompoundFurBodyType furryType, FurTexture? furTexture = null)
		{
			return UpdateBody(furryType, null, furTexture);
		}
		internal bool UpdateBody(CompoundFurBodyType furryType, FurColor mainFurColor, FurTexture? furTexture = null)
		{
			return UpdateBody(furryType, mainFurColor, mainFurColor, furTexture);
		}

		internal bool UpdateBody(CompoundFurBodyType furryType, FurColor primaryColor, FurColor secondaryColor, FurTexture? primaryTexture = null, FurTexture? secondaryTexture = null)
		{
			if (!UpdateHelper(furryType))
			{
				return false;
			}

			if (!FurColor.IsNullOrEmpty(primaryColor))
			{
				mainFur.ChangeFur(primaryColor);
			}
			if (primaryTexture != null)
			{
				mainFur.ChangeTexture((FurTexture)primaryTexture);
			}

			if (!FurColor.IsNullOrEmpty(secondaryColor))
			{
				secondary.ChangeFur(secondaryColor);
			}
			if (secondaryTexture != null)
			{
				secondary.ChangeTexture((FurTexture)secondaryTexture);
			}

			type = furryType;
			return true;
		}

		internal bool UpdateBody(CompoundToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return UpdateBody(toneType, null, toneTexture);
		}
		internal bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, SkinTexture? toneTexture = null)
		{
			return UpdateBody(toneType, primaryColor, primaryColor, toneTexture, toneTexture);
		}
		internal bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, Tones secondaryColor, SkinTexture? primaryTexture = null, SkinTexture? secondaryTexture = null)
		{
			UpdateHelper(toneType);

			if (!Tones.IsNullOrEmpty(primaryColor))
			{
				mainSkin.ChangeTone(primaryColor);
			}
			if (primaryTexture != null)
			{
				mainSkin.ChangeTexture((SkinTexture)primaryTexture);
			}

			if (!Tones.IsNullOrEmpty(secondaryColor))
			{
				secondary.ChangeTone(secondaryColor);
			}
			if (secondaryTexture != null)
			{
				secondary.ChangeTexture((SkinTexture)secondaryTexture);
			}

			type = toneType;
			return true;
		}

		internal bool UpdateBody(SimpleToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return UpdateBody(toneType, null, toneTexture);
		}
		internal bool UpdateBody(SimpleToneBodyType toneType, Tones color, SkinTexture? toneTexture = null)
		{
			UpdateHelper(toneType);
			if (!Tones.IsNullOrEmpty(color))
			{
				mainSkin.ChangeTone(color);
			}
			if (toneTexture != null)
			{
				mainSkin.ChangeTexture((SkinTexture)toneTexture);
			}


			type = toneType;
			return true;
		}

		private bool UpdateHelper(BodyType bodyType)
		{
			if (bodyType == null || type == bodyType)
			{
				return false;
			}

			//cockatrice uses the default scales if scaleTone is null. because the same color is weird.
			bodyType.ParseEpidermisDataOnTransform(mainFur, mainSkin, secondary, hairData(), out primary, out secondary);
			return true;
		}

		#endregion
		#region Changes
		internal bool ChangePrimarySkinTexture(SkinTexture skinTexture)
		{
			return mainSkin.ChangeTexture(skinTexture);
		}

		internal bool ChangeSecondarySkinTexture(SkinTexture skinTexture)
		{
			return secondary.ChangeTexture(skinTexture);
		}

		internal bool ChangePrimaryFurTexture(FurTexture furTexture)
		{
			return mainFur.ChangeTexture(furTexture);
		}

		internal bool ChangeSecondaryFurTexture(FurTexture furTexture)
		{
			return secondary.ChangeTexture(furTexture);
		}

		internal bool ChangePrimaryFurColor(FurColor furColor)
		{
			return mainFur.ChangeFur(furColor);
		}

		internal bool ChangePrimaryTone(Tones tone)
		{
			return mainSkin.ChangeTone(tone);
		}

		internal bool ChangeSecondaryFurColor(FurColor furColor)
		{
			return secondary.ChangeFur(furColor);
		}

		internal bool ChangeSecondaryTone(Tones tone)
		{
			return secondary.ChangeTone(tone);
		}

		#endregion
		#region Restore
		internal override bool Restore()
		{
			if (type == BodyType.HUMANOID)
			{
				return false;
			}
			return UpdateBody(BodyType.HUMANOID);
		}
		#endregion


		#region IMultiDyeable
		//DYES
		//MULTI:

		//You start by dyeing your <string> (if not dyeing another part : being careful to skip over your (list each part not dyeing)).
		//(if dyeing other parts as well : (foreach part to dye(if not last one or count is 2 : You then move on to your <next part>, not stopping until it too, is fully covered 
		//| else : "Finally, you finish by coating your <last part>.)) Satisfied with your work, you give it some time to set, then grab a bucket of cool lakewater to clean up, 
		//though it takes a few refills before you really feel clean. Your <list each part that was dyed> are all now <dye color>. if theres some that fail for strange reason, you could 
		//deal with them here. (for example, slimes apparently dont change tone. i think that's dumb.)

		//SINGLE: 
		//You rub the <dye> into your <part>, making sure to avoid <list each part not dyeing>. Once you're satisfied, it's fully set, you grab a bucket of cool lakewater to clean up. 
		//Your <part> is now <dye color> 

		//INCLUDE YOUR IN THE PART, then you can do things like "parts of your " instead of "your " and the game will be none the wiser, but sound better.

		//TONES
		//MULTI:

		//You start rubbing the lotion into your <first part>, determined to work in sections so you don't miss anything. (If 3+ parts You move on to your <second part>(if 4+ parts: then your <third>.)
		//You keep up this process until you reach your <last part>, glad to be done (if skipped any parts: and thankful you successfully skipped over your <skipped parts, listed>. 
		//After a while, you notice a pleasant tingling sensation start to spread across your <nameof part> where you applied the lotion. Sure enough, the lotion shifted (if all lotioned: all | else part) of
		//your <nameof part> to match it's <lotion color> hue.

		//SINGLE

		//You start rubbing the lotion into your <part>, making sure to avoid <list each part not lotioning>. After a while 

		//NOTE: IToneable and IDyeable can use the same format as the single version of IMultiDyeable and IMultiToneable, just omitting the "making sure to avoid ... bit"

		string IMultiDyeable.buttonText(byte index)
		{
			if (index >= numDyeables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 2) //as of now, with 3 elems, this never procs.
			{
				throw new NotImplementedException("Body has more than 3 dyeable members, but only 3 are implemented.");
			}
			else if (index == 2)
			{
				return type.secondaryButtonText(false);
			}
			else if (index == 1)
			{
				return type.primary2ButtonText();
			}
			else // (index == 0)
			{
				return type.primaryButtonText(false);
			}
		}

		string IMultiDyeable.locationDesc(byte index)
		{
			if (index >= numDyeables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 2)
			{
				throw new NotImplementedException("Body has more than 3 dyeable members, but only 3 are implemented.");
			}
			else if (index == 2)
			{
				return type.secondaryLocationDesc(false);
			}
			else if (index == 1)
			{
				return type.primary2LocationDesc();
			}
			else
			{
				return type.primaryLocationDesc(false);
			}
		}

		bool IMultiDyeable.allowsDye(byte index)
		{
			if (index >= numDyeables)
			{
				return false;
			}
			else if (index > 2)
			{
				throw new NotImplementedException("Body has more than 3 dyeable members, but only 3 are implemented.");
			}
			else if (index == 2)
			{
				return secondary.furMutable && !ReferenceEquals(mainFur, secondary);
			}
			else if (index == 1)
			{
				return mainFur.furMutable;
			}
			else
			{
				return mainFur.furMutable;
			}
		}

		bool IMultiDyeable.isDifferentColor(HairFurColors dyeColor, byte index)
		{
			if (index >= numDyeables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 2)
			{
				throw new NotImplementedException("Body has more than 3 dyeable members, but only 3 are implemented.");
			}
			else if (HairFurColors.IsNullOrEmpty(dyeColor))
			{
				throw dyeColor == null ? new ArgumentNullException() : new ArgumentException();
			}
			else if (index == 2)
			{
				return secondary.fur.IsIdenticalTo(dyeColor);
			}
			else if (index == 1)
			{
				return mainFur.fur.isMultiColored ? mainFur.fur.secondaryColor != dyeColor : mainFur.fur.primaryColor != dyeColor;
			}
			else
			{
				return mainFur.fur.primaryColor != dyeColor;
			}
		}

		bool IMultiDyeable.attemptToDye(HairFurColors dye, byte index)
		{
			//automatically takes care of index out of bound. we don't need to do it here. But we will for clarity.
			if (!asDyeable.allowsDye(index) || !asDyeable.isDifferentColor(dye, index))
			{
				return false;
			}
			if (index == 2)
			{
				return secondary.ChangeFur(new FurColor(dye));

			}
			else if (index == 1)
			{
				return mainFur.ChangeFur(new FurColor(mainFur.fur.primaryColor, dye, mainFur.fur.multiColorPattern));
			}
			else //if (index == 0)
			{
				return mainFur.ChangeFur(new FurColor(dye, mainFur.fur.secondaryColor, mainFur.fur.multiColorPattern));
			}
		}

		byte IMultiDyeable.numDyeableMembers => 3;

		bool IPatternable.canPattern(HairFurColors dye, params byte[] indices)
		{
			//for now we only let the main fur be patterned. if this is no longer the case,
			//determine which one we're trying to pattern. then check if that is valid.
			if (!mainFur.furMutable) //check if we can change the fur. this is false if it's currently empty, or if its immutable.
			{
				return false;
			}
			//if the secondary can pattern in the future, combine the first check with this.
			//if secondary can pattern in the future, check if the secondary uses fur, and we arent dyeing both parts. 
			return indices.Contains<byte>(0) != indices.Contains<byte>(1); 
		}

		//remember, applying a dye in a pattern is perfectly legal, even if it's the same pattern currently used. 
		bool IPatternable.isDifferentPattern(HairFurColors dye, FurMulticolorPattern pattern, params byte[] indices)
		{
			//for now, we only use the primary for patterning. if this changes, 
			if (!mainFur.fur.isMultiColored)
			{
				return true;
			}
			return pattern != mainFur.fur.multiColorPattern;
		}

		//only call this after the dye is applied. 
		bool IPatternable.attemptToPatternPostDye(HairFurColors dye, FurMulticolorPattern pattern, params byte[] indices)
		{
			if (!patternable.canPattern(dye, indices))
			{
				return false;
			}
			mainFur.fur.UpdateFurPattern(pattern);
			return true;
		}

		private IPatternable patternable => this;


		string IMultiDyeableCustomText.ApplyMultiDye(HairFurColors dyeColor, params byte[] index)
		{
			return MultiDye(dyeColor, new HashSet<byte>(index));
		}

		string IMultiDyeableCustomText.ApplySingleDye(HairFurColors dyeColor, byte index)
		{
			return SingleDye(dyeColor, index);
		}

		private byte numDyeables => asDyeable.numDyeableMembers;
		private IMultiDyeable asDyeable => this;

		#endregion
		#region IMultiToneable
		byte IMultiToneable.numToneableMembers => 2;

		string IMultiToneable.buttonText(byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1) //as long as numToneableMembers = 2, this will never proc.
			{
				throw new NotImplementedException("Body has more than 2 toneable members, but only two are implemented. ");
			}
			else if (index == 1)
			{
				return type.secondaryButtonText(true);
			}
			else //if (index == 0)
			{
				return type.primaryButtonText(true);
			}
		}

		string IMultiToneable.locationDesc(byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("Body has more than 2 toneable members, but only two are implemented. ");
			}
			else if (index == 1)
			{
				return type.secondaryLocationDesc(true);
			}
			else //(index == 0)
			{
				return type.primaryLocationDesc(true);
			}
		}

		bool IMultiToneable.canToneOil(byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("Body has more than 2 toneable members, but only two are implemented. ");
			}
			else if (index == 1)
			{
				return secondary.toneMutable && !ReferenceEquals(mainSkin, secondary); //check if we can change the tone, and the secondary isn't the main skin - main skin takes care of that.
			}
			else //if (index == 0)
			{
				return mainSkin.toneMutable;
			}
		}

		//note: this will proc true if the tones are different, even if you can't tone the respective part.
		bool IMultiToneable.isDifferentTone(Tones oilTone, byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("Body has more than 2 toneable members, but only two are implemented. ");
			}
			else if (Tones.IsNullOrEmpty(oilTone))
			{
				throw oilTone == null ? new ArgumentNullException() : new ArgumentException();
			}
			else if (index == 1)
			{
				return secondary.tone != oilTone;
			}
			else //if (index == 0)
			{
				return mainSkin.tone != oilTone; //this is the primary epidermis value if the primary uses tone. otherwise, it's the correct tone to apply to.
			}
		}

		bool IMultiToneable.attemptToTone(Tones oilTone, byte index)
		{
			if (!asToneable.canToneOil(index) || !asToneable.isDifferentTone(oilTone, index))
			{
				return false;
			}
			else if (index == 1) //this is guarenteed to not be the main skin if the previous query didn't proc.

			{
				return secondary.ChangeTone(oilTone);
			}
			else
			{
				return mainSkin.ChangeTone(oilTone);
			}
		}
		private byte numToneables => asToneable.numToneableMembers;
		private IMultiToneable asToneable => this;

		#endregion

		#region IMultiLotionable
		bool IMultiLotionable.canLotion(byte index)
		{
			if (index >= numLotionables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("There's more than 2 possible lotion members for body, but only two are implemented");
			}
			else if (index == 1)
			{
				return secondary.toneMutable && !ReferenceEquals(secondary, mainSkin);
			}
			else
			{
				return mainSkin.toneMutable;
			}
		}

		bool IMultiLotionable.isDifferentTexture(SkinTexture lotionTexture, byte index)
		{
			if (index >= numLotionables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("There's more than 2 possible lotion members for body, but only two are implemented");
			}
			else if (index == 1)
			{
				return secondary.skinTexture != lotionTexture;
			}
			else
			{
				return mainSkin.skinTexture != lotionTexture;
			}
		}

		bool IMultiLotionable.attemptToLotion(SkinTexture lotionTexture, byte index)
		{
			if (!multiLotionable.canLotion(index) || !multiLotionable.isDifferentTexture(lotionTexture, index))
			{
				return false;
			}
			else if (index == 1)
			{
				return secondary.ChangeTexture(lotionTexture);
			}
			else
			{
				return mainSkin.ChangeTexture(lotionTexture);
			}
		}

		//private IMultiLotionable multiLotionable => this;
		string IMultiLotionable.buttonText(byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1) //as long as numToneableMembers = 2, this will never proc.
			{
				throw new NotImplementedException("Body has more than 2 lotionable members, but only two are implemented. ");
			}
			else if (index == 1)
			{
				return type.secondaryButtonText(true);
			}
			else //if (index == 0)
			{
				return type.primaryButtonText(true);
			}
		}

		string IMultiLotionable.locationDesc(byte index)
		{
			if (index >= numToneables)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (index > 1)
			{
				throw new NotImplementedException("Body has more than 2 lotionable members, but only two are implemented. ");
			}
			else if (index == 1)
			{
				return type.secondaryLocationDesc(true);
			}
			else //(index == 0)
			{
				return type.primaryLocationDesc(true);
			}
		}

		byte IMultiLotionable.numLotionableMembers => 2;

		string IMultiLotionableCustomText.ApplyMultiLotion(SkinTexture lotionTexture, params byte[] index)
		{
			return MultiLotions(lotionTexture, new HashSet<byte>(index));
		}

		string IMultiLotionableCustomText.ApplySingleLotion(SkinTexture lotionTexture, byte index)
		{
			return SingleLotion(lotionTexture, index);
		}

		private IMultiLotionable multiLotionable => this;
		private byte numLotionables => multiLotionable.numLotionableMembers;

		#endregion

		#region IHairAware
		void IHairAware.GetHairData(HairDataGetter hairDataGetter)
		{
			hairData = hairDataGetter;
		}
		private HairDataGetter hairData;
		#endregion
	}

	public abstract partial class BodyType : SaveableBehavior<BodyType, Body>
	{
		private static int indexMaker = 0;
		private static readonly List<BodyType> bodyTypes = new List<BodyType>();

		public override int index => _index;
		private readonly int _index;

		private protected readonly BodyMember primary;
		private protected readonly BodyMember secondary;

		public SimpleDescriptor secondaryShortDesc => secondary.shortDescription;

		public EpidermisType epidermisType => primary.epidermisType;
		public EpidermisType secondaryEpidermisType => secondary.epidermisType;

		//allows you to rename the buttons for dyeing and toning. by default, they are "body" and "underbody". For example, Cockatrice uses this to say "feathers"
		//when dyeing primary, and "scales" when toning secondary. Also, furry types override "body" with "skin" when lotioning/oiling, as it affects the skin under the fur/feathers.
		private protected BodyType(BodyMember primaryMember, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : this(primaryMember, new EmptyBodyMember(), fullDesc, playerDesc, transform, restore) { }

		private protected BodyType(BodyMember primaryMember, BodyMember secondaryMember, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(primaryMember.shortDescription, fullDesc, playerDesc, transform, restore)
		{
			primary = primaryMember ?? throw new ArgumentNullException();

			if (primary.isEmpty) throw new ArgumentException("Cannot have an empty primary builder");

			secondary = secondaryMember ?? throw new ArgumentNullException();

			_index = indexMaker++;
			bodyTypes.AddAt(this, _index);
		}

		//initializes the main skin, main fur, primary, and secondary values for this type.
		internal virtual void Init(out Epidermis mainSkin, out Epidermis mainFur, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			if (primary.usesFur)
			{
				FurBodyMember furMember = (FurBodyMember)primary;
				FurBasedEpidermisType furType = furMember.furType;
				mainFur = new Epidermis(furType, furMember.defaultFur);
				mainSkin = new Epidermis(EpidermisType.SKIN);
			}
			else
			{
				ToneBodyMember toneMember = (ToneBodyMember)primary;
				ToneBasedEpidermisType toneType = (ToneBasedEpidermisType)toneMember.epidermisType;
				mainFur = new Epidermis();
				mainSkin = new Epidermis(toneType, toneMember.defaultTone);
			}

			ParseEpidermisDataOnTransform(mainFur, mainSkin, new Epidermis(), new HairData(), out primaryEpidermis, out secondaryEpidermis);
		}

		//validate is called after deserialization. Validate makes the following assumptions that the following are true after deserialization:
		//mainFur, mainSkin are valid - at worst
		//epidermis types for main fur, main skin are valid. this is guarenteed by the body's validate.
		internal static bool Validate(ref BodyType bodyType, Epidermis mainSkin, Epidermis mainFur, ref Epidermis primaryEpidermis, ref Epidermis secondaryEpidermis, in HairData hairData, bool correctInvalidData)
		{
			if (!bodyTypes.Contains(bodyType))
			{
				if (correctInvalidData)
				{
					bodyType = HUMANOID;
					bodyType.ParseEpidermisDataOnTransform(mainFur, mainSkin, secondaryEpidermis, hairData, out primaryEpidermis, out secondaryEpidermis);
				}
				return false;
			}
			return true;
		}

		//these are only virtual if you want to do some weird edge case - like the fur is too thick, preventing you from rubbing the lotion on the skin... underneath.
		//(Silence of the Lambs reference? Check!)
		#region Dye/Tone/Lotion
		//internal virtual bool allowsPrimaryDye => primary.usesFur && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryDye => secondary.usesFur && secondary.epidermisType.usesFur; //either usesTone and tone is mutabl

		//internal virtual bool allowsPrimaryOil => primary.usesTone && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryOil => secondary.usesTone && secondary.epidermisType.usesTone;

		//internal virtual bool allowsPrimaryLotion => primary.usesTone && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryLotion => secondary.usesTone && secondary.epidermisType.updateable;

		internal virtual string primaryButtonText(bool isTone)
		{
			return BodyDesc();
		}

		internal virtual string primary2ButtonText()
		{
			return Body2Desc();
		}

		internal virtual string secondaryButtonText(bool isTone)
		{
			return UnderBodyDesc();
		}

		//similarly, you can change these from " your body" and " your underbody" to whatever you want.
		internal virtual string primaryLocationDesc(bool isTone)
		{
			return primary.usesTone == isTone ? primary.dyeOrToneDescriptor() : YourBodyDesc();
		}

		internal virtual string primary2LocationDesc()
		{
			return PartsOfFurPatternDesc();
		}

		internal virtual string secondaryLocationDesc(bool isTone)
		{
			return (secondary.usesTone && isTone) || (secondary.usesFur && !isTone) ? secondary.dyeOrToneDescriptor() : YourUnderBodyDesc();
		}
		#endregion

		//we'll call this on transform. It's job is to take any main data, and the current supplementary data (if any). it'll update the main data so that it is valid
		//with this body type's primary and secondary epidermis types (if applicable), and spit out references to this body type's primary and secondary data. 

		//To ensure data works correctly, primary MUST reference either mainFur or mainSkin. 
		internal abstract void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis);

		internal static BodyType Deserialize(int index)
		{
			if (index < 0 || index >= bodyTypes.Count)
			{
				throw new System.ArgumentException("index for body type deserialize out of range");
			}
			else
			{
				BodyType body = bodyTypes[index];
				if (body != null)
				{
					return body;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		//apparently cat, fox, wolf, horse, and dog use fur underbody, kindof. 

		#region Instances
		private static readonly FurBodyMember GENERIC_FUR_MEMBER = new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurDesc);

		public static readonly SimpleToneBodyType HUMANOID = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.SKIN, Species.HUMAN.defaultTone, false, SkinDesc, YourBodyDesc),
			SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);

		public static readonly CompoundToneBodyType REPTILIAN = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, Species.LIZARD.defaultTone, false, ScalesDesc),
			new ToneBodyMember(EpidermisType.SCALES, Species.LIZARD.defaultTailTone, false, ScalesUnderbodyDesc, YourUnderScalesDesc),
			ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);

		public static readonly CompoundToneBodyType NAGA = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, Species.NAGA.defaultTone, false, NagaDesc),
			new ToneBodyMember(EpidermisType.SCALES, Species.NAGA.defaultUnderTone, false, NagaUnderbodyDesc, YourUnderNagaDesc),
			NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);

		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType();
		public static readonly KitsuneBodyType KITSUNE = new KitsuneBodyType();
		public static readonly SimpleToneBodyType WOODEN = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.BARK, Species.DRYAD.defaultBarkColor, true, BarkDesc),
			BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		////one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly SimpleFurBodyType SIMPLE_FUR = new SimpleFurBodyType(GENERIC_FUR_MEMBER, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly CompoundFurBodyType UNDERBODY_FUR = new CompoundFurBodyType(GENERIC_FUR_MEMBER,
			new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurUnderbodyDesc, YourUnderFurDesc),
			FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		public static readonly CompoundFurBodyType FEATHERED = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.FEATHERS, Species.HARPY.defaultFeathers, false, FeatherDesc),
			new FurBodyMember(EpidermisType.FEATHERS, Species.HARPY.defaultFeathers, false, UnderFeatherDesc, YourUnderFeatherDesc),
			FeatherFullDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr);

		public static readonly CompoundFurBodyType WOOL = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.WOOL, Species.SHEEP.defaultColor, false, WoolDesc),
			new FurBodyMember(EpidermisType.WOOL, Species.SHEEP.defaultColor, false, WoolUnderbodyDesc, YourUnderWoolDesc),
			WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		////now, if you have gooey body, give the goo innards perk. simple.
		////Also: Goo body is getting a rework/revamp. it was originally a spaghetti code of a mess of partially implemented checks on a perk. now it's its own type. 
		////any body part not "Goo" will act like it should, regardless of the gooey body. It never really made sense before; it still doesn't.
		////You can lampshade this if you feel the need to by saying something like:
		////"Your normally flexible goo-like body solidifies around your arms|legs|cock|wings|whatever, allowing you to use it properly."
		////additionally, if the goo body gains perks or abilities when partially or fully goo, you could add flavor text like: 
		////"if more of your parts shared this gooey structure, you might be able to make better use of your gooey form"
		////a chimera-like monster could get text like: "it's arms|legs|whatever clash with the rest of its goo-like form, though it succeeds in making it more disturbing";

		public static readonly SimpleToneBodyType GOO = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.GOO, Species.GOO.defaultTone, false, GooDesc),
			GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		////cleaner - we don't need umpteen checks to see if it's "rubbery"
		public static readonly SimpleToneBodyType RUBBER = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.RUBBER, Tones.GRAY, true, RubberDesc),
			RubberFullDesc, RubberPlayerStr, RubberTransformStr, RubberRestoreStr);
		////like a turtle shell or bee exoskeleton.
		public static readonly SimpleToneBodyType CARAPACE = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.CARAPACE, Tones.BLACK, true, CarapaceStr),
			CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
		#endregion

		internal abstract class BodyMember
		{
			internal bool usesTone => epidermisType.usesTone;
			internal bool usesFur => epidermisType.usesFur;

			internal readonly SimpleDescriptor shortDescription;
			internal readonly EpidermisType epidermisType;

			internal readonly SimpleDescriptor dyeOrToneDescriptor;

			internal BodyMember(EpidermisType epidermis, SimpleDescriptor shortDesc, SimpleDescriptor descriptionText)
			{
				if (epidermis == null || epidermis == EpidermisType.EMPTY)
				{
					throw new ArgumentException();
				}

				shortDescription = shortDesc;
				epidermisType = epidermis;
				dyeOrToneDescriptor = descriptionText;
			}
			internal BodyMember(EpidermisType epidermis, SimpleDescriptor shortDesc)
			{
				if (epidermis == null || epidermis == EpidermisType.EMPTY)
				{
					throw new ArgumentException();
				}

				epidermisType = epidermis;
				shortDescription = shortDesc;

				dyeOrToneDescriptor = YourDescriptor(epidermis);
			}

			internal bool isEmpty => this is EmptyBodyMember;
		}

		internal sealed class FurBodyMember : BodyMember
		{
			public readonly FurColor defaultFur;
			public readonly bool overrideFur;
			public FurBasedEpidermisType furType => (FurBasedEpidermisType)epidermisType;
			public FurBodyMember(FurBasedEpidermisType furType, FurColor fallbackColor, bool useFallbackColorOnTF,
				SimpleDescriptor shortDesc, SimpleDescriptor dyeDescription)
				: base(furType, shortDesc, dyeDescription)
			{
				defaultFur = fallbackColor;
				overrideFur = useFallbackColorOnTF;
			}

			public FurBodyMember(FurBasedEpidermisType furType, FurColor fallbackColor, bool useFallbackColorOnTF, SimpleDescriptor shortDesc)
				: base(furType, shortDesc)
			{
				defaultFur = fallbackColor;
				overrideFur = useFallbackColorOnTF;
			}
		}

		internal sealed class ToneBodyMember : BodyMember
		{
			public readonly Tones defaultTone;
			public readonly bool overrideTone;
			public ToneBodyMember(ToneBasedEpidermisType toneType, Tones fallbackTone, bool useFallbackToneOnTF,
				SimpleDescriptor shortDesc, SimpleDescriptor lotionDescription)
				: base(toneType, shortDesc, lotionDescription)
			{
				defaultTone = fallbackTone;
				overrideTone = useFallbackToneOnTF;
			}

			public ToneBodyMember(ToneBasedEpidermisType toneType, Tones fallbackTone, bool useFallbackToneOnTF, SimpleDescriptor shortDesc)
				: base(toneType, shortDesc)
			{
				defaultTone = fallbackTone;
				overrideTone = useFallbackToneOnTF;
			}
		}

		internal sealed class EmptyBodyMember : BodyMember
		{
			public EmptyBodyMember() : base(EpidermisType.EMPTY, GlobalStrings.None, YourUnderBodyDesc) { }
		}
	}

	public abstract class SimpleBodyType : BodyType
	{
		private protected SimpleBodyType(BodyMember builder, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(builder, fullDesc, playerDesc, transform, restore) { }
	}

	public abstract class CompoundBodyType : BodyType
	{
		private protected CompoundBodyType(BodyMember primaryBuilder, BodyMember secondaryBuilder, DescriptorWithArg<Body> fullDesc,
			TypeAndPlayerDelegate<Body> playerDesc, ChangeType<Body> transform, RestoreType<Body> restore)
			: base(primaryBuilder, secondaryBuilder, fullDesc, playerDesc, transform, restore) { }
	}

	public class SimpleFurBodyType : SimpleBodyType
	{
		internal FurBodyMember furMember => (FurBodyMember)primary;
		public FurBasedEpidermisType furType => (FurBasedEpidermisType)furMember.epidermisType;
		public FurColor defaultFur => furMember.defaultFur;
		public bool overrideFur => furMember.overrideFur;

		internal SimpleFurBodyType(FurBodyMember builder, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(builder, fullDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			mainSkin.UpdateEpidermis(EpidermisType.SKIN); //This type doesn't use tone directly, but skin exists under the fur. so make sure the rest of the body uses skin (not scales or something).

			//get the color.
			FurColor overrideColor = overrideFur ? defaultFur : null;
			FurColor color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, hairData.activeHairColor, defaultFur);

			//set the mainFur accordingly. Remember, it can be empty. 
			mainFur.UpdateOrChange(furType, color);
			primaryEpidermis = mainFur; //set the primary to reference mainFur.

			//we can't reuse the secondary epidermis, as it may point to mainFur or mainSkin :(
			//i suppose i could optimize this with some ReferenceEquals, but that just makes it more visually complicated for very little memory gain.
			secondaryEpidermis = new Epidermis();
		}
	}

	public class CompoundFurBodyType : CompoundBodyType
	{
		internal FurBodyMember primaryFurMember => (FurBodyMember)primary;
		public FurColor defaultMainFur => primaryFurMember.defaultFur;
		public bool overrideMainFur => primaryFurMember.overrideFur;
		public FurBasedEpidermisType primaryFurType => (FurBasedEpidermisType)primaryFurMember.epidermisType;

		internal FurBodyMember secondaryFurMember => (FurBodyMember)secondary;
		public FurColor defaultSupplementaryFur => secondaryFurMember.defaultFur;
		public bool overrideSupplementaryFur => secondaryFurMember.overrideFur;
		public FurBasedEpidermisType secondaryFurType => (FurBasedEpidermisType)secondaryFurMember.epidermisType;

		internal CompoundFurBodyType(FurBodyMember primaryBuilder, FurBodyMember secondaryBuilder, DescriptorWithArg<Body> fullDesc,
			TypeAndPlayerDelegate<Body> playerDesc, ChangeType<Body> transform, RestoreType<Body> restore)
			: base(primaryBuilder, secondaryBuilder, fullDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			mainSkin.UpdateEpidermis(EpidermisType.SKIN); //This type doesn't use tone directly, but skin exists under the fur. so make sure the rest of the body uses skin (not scales or something).

			//old secondary may point to current mainFur. since this is the case, let's take care of the secondary first.
			FurColor overrideColor = overrideSupplementaryFur ? defaultSupplementaryFur : null;
			FurColor color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, currSecondary.fur, hairData.activeHairColor, defaultSupplementaryFur);
			FurTexture texture = currSecondary.usesFur ? currSecondary.furTexture : mainFur.usesFur ? mainFur.furTexture : FurTexture.NONDESCRIPT;
			secondaryEpidermis = new Epidermis(secondaryFurType, color, texture);

			overrideColor = overrideMainFur ? defaultMainFur : null;
			color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, hairData.activeHairColor, defaultMainFur);

			mainFur.UpdateOrChange(primaryFurType, color);
			primaryEpidermis = mainFur;
		}
	}
	public class SimpleToneBodyType : SimpleBodyType
	{
		internal ToneBodyMember toneMember => (ToneBodyMember)primary;
		public ToneBasedEpidermisType toneType => (ToneBasedEpidermisType)epidermisType;
		public Tones defaultTone => toneMember.defaultTone;
		public bool overrideTone => toneMember.overrideTone;

		internal SimpleToneBodyType(ToneBodyMember builder, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(builder, fullDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			mainFur.Reset(); //we aren't using mainFur, so reset it to empty.

			Tones overrideValue = overrideTone ? defaultTone : null;
			Tones color = BodyHelpers.GetValidTone(overrideValue, mainSkin.tone, defaultTone);

			mainSkin.UpdateOrChange(toneType, color);
			primaryEpidermis = mainSkin;

			secondaryEpidermis = new Epidermis();
		}
	}

	public class CompoundToneBodyType : CompoundBodyType
	{
		internal ToneBodyMember primaryMember => (ToneBodyMember)primary;

		public ToneBasedEpidermisType primaryType => (ToneBasedEpidermisType)primaryMember.epidermisType;
		public Tones defaultMainTone => primaryMember.defaultTone;
		public bool overrideMainTone => primaryMember.overrideTone;

		internal ToneBodyMember secondaryMember => (ToneBodyMember)secondary;
		public ToneBasedEpidermisType secondaryType => (ToneBasedEpidermisType)secondaryMember.epidermisType;

		public Tones defaultSupplementaryTone => secondaryMember.defaultTone;
		public bool overrideSupplementaryTone => secondaryMember.overrideTone;

		internal CompoundToneBodyType(ToneBodyMember primaryBuilder, ToneBodyMember secondaryBuilder, DescriptorWithArg<Body> fullDesc,
			TypeAndPlayerDelegate<Body> playerDesc, ChangeType<Body> transform, RestoreType<Body> restore)
			: base(primaryBuilder, secondaryBuilder, fullDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			mainFur.Reset(); //we aren't using mainFur, so reset it to empty.

			Tones tones = currSecondary.tone;
			if (overrideSupplementaryTone)
			{
				tones = defaultSupplementaryTone;
			}
			else if (tones.isEmpty)
			{
				tones = mainSkin.tone;
			}

			SkinTexture texture = currSecondary.usesTone ? currSecondary.skinTexture : mainSkin.skinTexture;
			secondaryEpidermis = new Epidermis(secondaryType, tones, texture);


			if (epidermisType != mainSkin.type)
			{
				mainSkin.UpdateEpidermis(primaryType);
			}

			if (overrideMainTone)
			{
				mainSkin.ChangeTone(defaultMainTone);
			}
			primaryEpidermis = mainSkin;

		}
	}

	public sealed class KitsuneBodyType : CompoundBodyType
	{
		public Tones defaultTone => ((ToneBodyMember)primary).defaultTone;
		public FurColor defaultFur => ((FurBodyMember)secondary).defaultFur;
		internal KitsuneBodyType() : base(
			new ToneBodyMember(EpidermisType.SKIN, Species.KITSUNE.defaultSkin, false, KitsuneDesc, YourDescriptor(EpidermisType.SKIN)),
			new FurBodyMember(EpidermisType.FUR, Species.KITSUNE.defaultFur, false, KitsuneUnderbodyDesc, YourDescriptor(EpidermisType.FUR)),
			KitsuneFullDesc, KitsunePlayerStr, KitsuneTransformStr, KitsuneRestoreStr)
		{ }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			mainSkin.UpdateEpidermis(epidermisType);
			primaryEpidermis = mainSkin;

			FurColor color = BodyHelpers.GetValidFurColor(null, mainFur.fur, hairData.activeHairColor, defaultFur);
			mainFur.UpdateEpidermis((FurBasedEpidermisType)secondaryEpidermisType, color);
			secondaryEpidermis = mainFur;
		}
	}

	public sealed class CockatriceBodyType : CompoundBodyType
	{
		public FurColor defaultFeathers => ((FurBodyMember)primary).defaultFur;
		public Tones defaultScales => ((ToneBodyMember)secondary).defaultTone;

		internal CockatriceBodyType() : base(
				new FurBodyMember(EpidermisType.FEATHERS, Species.COCKATRICE.defaultPrimaryFeathers, false, CockatriceDesc, YourDescriptor(EpidermisType.FEATHERS)),
				new ToneBodyMember(EpidermisType.SCALES, Species.COCKATRICE.defaultScaleTone, false, CockatriceUnderbodyDesc, YourDescriptor(EpidermisType.SCALES)),
				CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
		{ }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis primaryEpidermis, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			FurColor color = BodyHelpers.GetValidFurColor(null, mainFur.fur, hairData.activeHairColor, defaultFeathers);
			mainFur.UpdateEpidermis((FurBasedEpidermisType)epidermisType, color);
			primaryEpidermis = mainFur;

			mainSkin.UpdateEpidermis(secondaryEpidermisType);
			secondaryEpidermis = mainSkin;
		}
	}

	internal sealed class BodyData
	{

		internal readonly EpidermalData activeFur;
		internal readonly EpidermalData mainSkin;

		internal readonly EpidermalData main; //the current epidermis data, in an immutable form. never empty or null.
		internal readonly EpidermalData supplementary; //the current supplementary epidermis data, if any, in immutable form. note that this can be empty, but will never be null.

		internal readonly BodyType bodyType; //current body type. never empty or null.

		internal readonly HairFurColors hairColor; //current hair color. if the character cannot and therefore does not have hair, this will be empty. otherwise, this will be valid, even if the character is currently bald.
		internal HairFurColors activeHairColor => hasHair ? hairColor : HairFurColors.NO_HAIR_FUR; //this is the same as hairColor, but will be empty if the character is bald. 
		internal readonly bool hasHair; //boolean determining if the character has any hair. this will be false if the character is bald or cannot grow hair.

		internal BodyData(Epidermis primary, Epidermis secondary, Epidermis fur, Epidermis skin, in HairData hairData, BodyType bodyType)
		{
			main = primary.GetEpidermalData();
			supplementary = secondary.GetEpidermalData();

			hairColor = hairData.hairColor;
			hasHair = !hairData.hairDeactivated;

			activeFur = fur.GetEpidermalData();
			mainSkin = skin.GetEpidermalData();

			this.bodyType = bodyType;
		}
	}
}
