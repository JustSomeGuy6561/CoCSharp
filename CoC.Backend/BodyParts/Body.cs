//Body.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	//Trying to come up with decent logic for this thing is a pain in the ass. This is maybe the 5th revision. I think it works well enough now. 

	//First off, we store two "Main" elements: Fur and Skin. Fur stores an epidermis that uses fur colors (fur or feathers in current implementation). 
	//Skin stores an epidermis dealing with Tone Colors (skin, scales, carapace, goo, rubber, etc). For the most part, body parts that make use of one main 
	//will not use the other. These will be given priority when dyeing or toneing. 

	//We also have two Epidermis instances: primary and secondary. These can be any epidermis type, though the primary cannot be empty. 
	//The values for primary and secondary are determined from the body type itself -
	//	the primary type is either the main fur or the main skin, depending on which one the body type uses. 
	//	The secondary is based on what the body type declares it to be. 

	//For example: A fur-based body type will make the primary point to mainFur, and secondary to a new fur instance if it has an underbody. 
	//Similarly, a tone-based body type will point to mainSkin for primary, and set the secondary to a new tone-based instance if it has an underbody. 

	//stranger things occur with mixed types; cockatrice and kitsune both use mainFur and mainSkin;
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

	public enum HipPiercingLocation { LEFT_TOP, LEFT_CENTER, LEFT_BOTTOM, RIGHT_TOP, RIGHT_CENTER, RIGHT_BOTTOM }

	public sealed partial class Body : BehavioralSaveablePart<Body, BodyType, BodyData>, IMultiDyeableCustomText, IPatternable, ISimultaneousMultiToneable, IMultiLotionableCustomText
	{

		private const JewelryType AVAILABLE_NAVEL_PIERCINGS = JewelryType.HORSESHOE | JewelryType.DANGLER | JewelryType.RING | JewelryType.BARBELL_STUD | JewelryType.SPECIAL;
		private const JewelryType AVAILABLE_HIP_PIERCINGS = JewelryType.BARBELL_STUD;
		//Hair, Fur, Tone
		//private HairFurColors hairColor => hairData().hairColor;

		private HairData hairData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.hair.AsReadOnlyData() : new HairData(Guid.Empty);

		private HairFurColors activeHairColor
		{
			get
			{
				HairData data = hairData;
				return data.hairDeactivated ? HairFurColors.NO_HAIR_FUR : data.hairColor;
			}
		}

		public EpidermalData mainEpidermis => primary.AsReadOnlyData();
		public EpidermalData supplementaryEpidermis => secondary.AsReadOnlyData();

		public bool hasSecondaryEpidermis => !secondary.isEmpty;

		//private readonly Epidermis primaryEpidermis;
		//private readonly Epidermis secondaryEpidermis;

		public readonly Piercing<NavelPiercingLocation> navelPiercings;
		public readonly Piercing<HipPiercingLocation> hipPiercings;
		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;
		//use these if the part does not care on the state of the body, but just needs the main color. 

		private readonly Epidermis mainFur; //stores the current fur that is primarily used. if it's a multi-fur, the secondary fur is stored in supplementary epidermis. if it's no-fur, this is empty.
		public bool furActive => ReferenceEquals(mainFur, primary) || ReferenceEquals(mainFur, secondary);
		private readonly Epidermis mainSkin; //stores the current skin that is primarily used. if it's multi-tone, the secondary tone is stored in the supplementary epidermis.
		public EpidermalData primarySkin => mainSkin.AsReadOnlyData();
		private bool skinActive => ReferenceEquals(mainSkin, primary) || ReferenceEquals(mainSkin, secondary);

		private Epidermis primary => type.primaryIsFur ? mainFur : mainSkin;
		//secondary is completely determined by the body type itself. Note that there's nothing stopping primary and secondary pointing to the same object, though this should never happen.
		private Epidermis secondary;


		public override BodyType type { get; protected set; }

		public override BodyType defaultType => BodyType.defaultValue;

		internal Body(Guid creatureID) : this(creatureID, BodyType.defaultValue) { }

		internal Body(Guid creatureID, BodyType bodyType) : base(creatureID)
		{
			type = bodyType ?? throw new ArgumentNullException();

			bodyType.Init(out mainSkin, out mainFur, out secondary);

			navelPiercings = new Piercing<NavelPiercingLocation>(NavelLocationUnlocked, NavelSupportedJewelry);
			hipPiercings = new Piercing<HipPiercingLocation>(HipLocationUnlocked, HipSupportedJewelry);
		}

		internal Body(Guid creatureID, BodyType bodyType, FurColor primaryFurColor = null, FurTexture? primaryFurTexture = null, Tones primarySkinTone = null,
			SkinTexture? primarySkinTexture = null, FurColor secondaryFurColor = null, FurTexture? secondaryFurTexture = null, Tones secondarySkinTone = null,
			SkinTexture? secondarySkinTexture = null, bool secondaryUsesPrimaryIfNull = true) : this(creatureID, bodyType)
		{
			if (secondaryUsesPrimaryIfNull)
			{
				if (FurColor.IsNullOrEmpty(secondaryFurColor) && !FurColor.IsNullOrEmpty(primaryFurColor))
				{
					secondaryFurColor = primaryFurColor;
				}

				if (secondaryFurTexture is null && primaryFurTexture != null)
				{
					secondaryFurTexture = primaryFurTexture;
				}

				if (Tones.IsNullOrEmpty(secondarySkinTone) && !Tones.IsNullOrEmpty(primarySkinTone))
				{
					secondarySkinTone = primarySkinTone;
				}

				if (secondarySkinTexture is null && primarySkinTexture != null)
				{
					secondarySkinTexture = primarySkinTexture;
				}
			}

			ChangeFur(true, primaryFurColor, primaryFurTexture, true);
			ChangeFur(false, secondaryFurColor, secondaryFurTexture, true);
			ChangeSkin(true, primarySkinTone, primarySkinTexture, true);
			ChangeSkin(false, secondarySkinTone, secondarySkinTexture, true);
		}

		public override string BodyPartName() => Name();

		#region Updates

		//need to override the default update type because we do validation on type change. it's done via UpdateHelper. Note that some versions will require extra updates
		//before returning. this is handled by an anonymous lambda. It's not strictly speaking necessary; these all could be written manually, but i didn't feel like
		//rewriting the same thing 30x.

		internal override bool UpdateType(BodyType newType)
		{
			return UpdateHelper(newType, null);
		}

		internal bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return UpdateBody(cockatriceBodyType, null, null, featherTexture, scaleTexture);
		}
		internal bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return UpdateHelper(cockatriceBodyType, () =>
			{
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
			});
		}

		internal bool UpdateBody(KitsuneBodyType kitsuneBodyType, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return UpdateBody(kitsuneBodyType, null, null, skinTexture, furTexture);
		}
		internal bool UpdateBody(KitsuneBodyType kitsuneBodyType, Tones skinTone, FurColor furColor, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return UpdateHelper(kitsuneBodyType, () =>
			{
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
			});
		}

		internal bool UpdateBody(SimpleFurBodyType furryType, FurTexture? furTexture = null)
		{
			return UpdateBody(furryType, null, furTexture);
		}
		internal bool UpdateBody(SimpleFurBodyType furryType, FurColor furColor, FurTexture? furTexture = null)
		{
			return UpdateHelper(furryType, () =>
			{
				if (!FurColor.IsNullOrEmpty(furColor))
				{
					mainFur.ChangeFur(furColor);
				}
				if (furTexture != null)
				{
					mainFur.ChangeTexture((FurTexture)furTexture);
				}
			});
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
			return UpdateHelper(furryType, () =>
			{
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
			});
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
			return UpdateHelper(toneType, () =>
			{
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
			});
		}

		internal bool UpdateBody(SimpleToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return UpdateBody(toneType, null, toneTexture);
		}
		internal bool UpdateBody(SimpleToneBodyType toneType, Tones color, SkinTexture? toneTexture = null)
		{
			return UpdateHelper(toneType, () =>
			{
				if (!Tones.IsNullOrEmpty(color))
				{
					mainSkin.ChangeTone(color);
				}
				if (toneTexture != null)
				{
					mainSkin.ChangeTexture((SkinTexture)toneTexture);
				}
			});
		}

		private bool UpdateHelper(BodyType bodyType, Action extraUpdates)
		{
			if (bodyType == type || bodyType is null)
			{
				return false;
			}
			else
			{
				EpidermalData oldSkin = mainSkin.AsReadOnlyData();
				EpidermalData oldPrimary = primary.AsReadOnlyData();
				EpidermalData oldSecondary = secondary.AsReadOnlyData();
				EpidermalData oldFur = mainFur.AsReadOnlyData();

				var oldType = type;
				type = bodyType;
				type.ParseEpidermisDataOnTransform(mainFur, mainSkin, secondary, hairData, out secondary);
				extraUpdates?.Invoke();

				CheckOuterLayerChanged(oldSkin, oldPrimary, oldSecondary, oldFur);
				NotifyTypeChanged(oldType);
				return true;
			}
		}

		private void CheckOuterLayerChanged(EpidermalData oldSkin, EpidermalData oldPrimary, EpidermalData oldSecondary, EpidermalData oldFur)
		{
			if (!oldSkin.Equals(mainSkin.AsReadOnlyData()) || !oldPrimary.Equals(mainEpidermis) || !oldSecondary.Equals(supplementaryEpidermis))
			{
				var oldData = new BodyData(creatureID, oldPrimary, oldSecondary, oldFur, oldSkin, hairData, type);
				NotifyDataChanged(oldData);
			}
		}


		#endregion
		#region Changes
		//change all skin 
		public bool ChangeMainSkin(Tones newTone, bool ignoreIfFurPrimary = false)
		{
			if (ignoreIfFurPrimary && primary.usesFur)
			{
				return false;
			}
			return ChangeSkin(true, newTone, null);
		}

		public bool ChangeMainSkin(SkinTexture newTexture, bool ignoreIfFurPrimary = false)
		{
			if (ignoreIfFurPrimary && primary.usesFur)
			{
				return false;
			}
			return ChangeSkin(true, null, newTexture);
		}

		public bool ChangeMainSkin(Tones newTone, SkinTexture newTexture, bool ignoreIfFurPrimary = false)
		{
			if (ignoreIfFurPrimary && primary.usesFur)
			{
				return false;
			}
			return ChangeSkin(true, newTone, newTexture);
		}

		public bool ChangeSecondarySkin(Tones newTone)
		{
			return ChangeSkin(false, newTone, null);
		}

		public bool ChangeSecondarySkin(SkinTexture newTexture)
		{
			return ChangeSkin(false, null, newTexture);
		}

		public bool ChangeSecondarySkin(Tones newTone, SkinTexture newTexture)
		{
			return ChangeSkin(false, newTone, newTexture);
		}

		public bool ChangeAllSkin(Tones newTone)
		{
			return ChangeMainSkin(newTone) | ChangeSecondarySkin(newTone);
		}

		public bool ChangeAllSkin(SkinTexture newTexture)
		{
			return ChangeMainSkin(newTexture) | ChangeSecondarySkin(newTexture);
		}

		public bool ChangeAllSkin(Tones newTone, SkinTexture newTexture)
		{
			return ChangeMainSkin(newTone, newTexture) | ChangeSecondarySkin(newTone, newTexture);
		}

		private bool ChangeSkin(bool primary, Tones tone, SkinTexture? texture, bool silent = false)
		{

			if (tone is null && texture is null || (!primary && !secondary.usesTone))
			{
				return false;
			}

			Action doIt;
			if (tone is null)
			{
				if (primary) doIt = () => mainSkin.ChangeTexture((SkinTexture)texture);
				else doIt = () => secondary.ChangeTexture((SkinTexture)texture);

			}
			else if (texture is null)
			{
				if (primary) doIt = () => mainSkin.ChangeTone(tone);
				else doIt = () => secondary.ChangeTone(tone);
			}
			else
			{
				SkinTexture skinTexture = (SkinTexture)texture;
				if (primary) doIt = () => mainSkin.ChangeToneAndTexture(tone, skinTexture);
				else doIt = () => secondary.ChangeToneAndTexture(tone, skinTexture);
			}
			return ChangeHelper(true, doIt, silent);
		}

		public bool ChangeMainFur(FurColor newFurColor, bool ignoreIfTonePrimary = false)
		{
			if (ignoreIfTonePrimary && primary.usesTone)
			{
				return false;
			}
			return ChangeFur(true, newFurColor, null);
		}

		public bool ChangeMainFur(FurTexture newTexture, bool ignoreIfTonePrimary = false)
		{
			if (ignoreIfTonePrimary && primary.usesTone)
			{
				return false;
			}
			return ChangeFur(true, null, newTexture);
		}

		public bool ChangeMainFur(FurColor newFurColor, FurTexture newTexture, bool ignoreIfTonePrimary = false)
		{
			if (ignoreIfTonePrimary && primary.usesTone)
			{
				return false;
			}
			return ChangeFur(true, newFurColor, newTexture);
		}

		public bool ChangeSecondaryFur(FurColor newFurColor)
		{
			return ChangeFur(false, newFurColor, null);
		}

		public bool ChangeSecondaryFur(FurTexture newTexture)
		{
			return ChangeFur(false, null, newTexture);
		}

		public bool ChangeSecondaryFur(FurColor newFurColor, FurTexture newTexture)
		{
			return ChangeFur(false, newFurColor, newTexture);
		}

		public bool ChangeAllFur(FurColor newFurColor)
		{
			return ChangeFur(true, newFurColor, null) | ChangeFur(false, newFurColor, null);
		}

		public bool ChangeAllFur(FurTexture newTexture)
		{
			return ChangeFur(true, null, newTexture) | ChangeFur(false, null, newTexture);
		}

		public bool ChangeAllFur(FurColor newFurColor, FurTexture newTexture)
		{
			return ChangeFur(true, newFurColor, newTexture) | ChangeFur(false, newFurColor, newTexture);
		}
		//change all fur.

		private bool ChangeFur(bool primary, FurColor furColor, FurTexture? texture, bool silent = false)
		{
			if (furColor is null && texture is null || (!primary && !secondary.usesFur))
			{
				return false;
			}

			Action doIt;
			if (furColor is null)
			{
				if (primary) doIt = () => mainSkin.ChangeTexture((FurTexture)texture);
				else doIt = () => secondary.ChangeTexture((FurTexture)texture);

			}
			else if (texture is null)
			{
				if (primary) doIt = () => mainSkin.ChangeFur(furColor);
				else doIt = () => secondary.ChangeFur(furColor);
			}
			else
			{
				FurTexture furTexture = (FurTexture)texture;
				if (primary) doIt = () => mainSkin.ChangeFurAndTexture(furColor, furTexture);
				else doIt = () => secondary.ChangeFurAndTexture(furColor, furTexture);
			}
			return ChangeHelper(true, doIt, silent);
		}

		private bool ChangeHelper(bool canChange, Action doChange, bool silent = false)
		{
			if (canChange)
			{
				EpidermalData oldPrimary, oldSecondary, oldSkin, oldFur;
				oldPrimary = mainEpidermis;
				oldSecondary = supplementaryEpidermis;
				oldSkin = mainSkin.AsReadOnlyData();
				oldFur = mainFur.AsReadOnlyData();
				doChange();
				if (!silent)
				{
					CheckOuterLayerChanged(oldSkin, oldPrimary, oldSecondary, oldFur);
				}
				return true;
			}
			return false;
		}


		#endregion
		#region Restore
		//default restore is fine.

		internal void Reset()
		{
			Restore();
			hipPiercings.Reset();
			navelPiercings.Reset();
		}
		#endregion

		#region Validate
		//called after deserialization. We're making the following assumptions: If the bodyType is not null, its Init was called. this should be true, because we control
		//deserialization, though i suppose if we use a method of serialization that doesn't call constructors, that wouldn't apply. Honestly though, using a serialization technique that
		//doesnt call constructors would invite so many problems due to readonlys that it wouldn't be all that smart. If the bodyType is null, assumes mainSkin and mainFur are not null.
		//also assumes the hairData getter has been connected already. 
		internal override bool Validate(bool correctInvalidData)
		{
			BodyType bodyType = type;
			bool valid = BodyType.Validate(ref bodyType, mainSkin, mainFur, ref secondary, hairData, correctInvalidData);
			type = bodyType;
			if (valid || correctInvalidData)
			{
				valid &= navelPiercings.Validate(correctInvalidData);
			}
			return valid;
		}
		#endregion
		#region Piercing Helper

		private JewelryType NavelSupportedJewelry(NavelPiercingLocation piercingLocation)
		{
			return AVAILABLE_NAVEL_PIERCINGS;
		}

		private bool NavelLocationUnlocked(NavelPiercingLocation piercingLocation)
		{
			return true;
		}

		private JewelryType HipSupportedJewelry(HipPiercingLocation piercingLocation)
		{
			return AVAILABLE_HIP_PIERCINGS;
		}

		private bool HipLocationUnlocked(HipPiercingLocation piercingLocation)
		{
			return piercingFetish;
		}
		#endregion
		public override BodyData AsReadOnlyData()
		{
			return new BodyData(creatureID, primary, secondary, mainFur, mainSkin, hairData, type);
		}


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
	}

	public abstract partial class BodyType : SaveableBehavior<BodyType, Body, BodyData>
	{

		static BodyType()
		{

		}

		private static int indexMaker = 0;
		private static readonly List<BodyType> bodyTypes = new List<BodyType>();
		public static readonly ReadOnlyCollection<BodyType> availableTypes = new ReadOnlyCollection<BodyType>(bodyTypes);

		public static BodyType defaultValue => HUMANOID;

		public override int index => _index;
		private readonly int _index;

		private protected readonly BodyMember primary;
		private protected readonly BodyMember secondary;

		public bool primaryIsFur => primary.usesFur;

		public SimpleDescriptor secondaryShortDesc => secondary.shortDescription;

		public EpidermisType epidermisType => primary.epidermisType;
		public EpidermisType secondaryEpidermisType => secondary.epidermisType;

		//allows you to rename the buttons for dyeing and toning. by default, they are "body" and "underbody". For example, Cockatrice uses this to say "feathers"
		//when dyeing primary, and "scales" when toning secondary. Also, furry types override "body" with "skin" when lotioning/oiling, as it affects the skin under the fur/feathers.
		private protected BodyType(BodyMember primaryMember, DescriptorWithArg<Body> longDesc, PlayerBodyPartDelegate<Body> playerDesc,
			ChangeType<BodyData> transform, RestoreType<BodyData> restore) : this(primaryMember, new EmptyBodyMember(), longDesc, playerDesc, transform, restore) { }

		private protected BodyType(BodyMember primaryMember, BodyMember secondaryMember, DescriptorWithArg<Body> longDesc, PlayerBodyPartDelegate<Body> playerDesc,
			ChangeType<BodyData> transform, RestoreType<BodyData> restore) : base(primaryMember.shortDescription, longDesc, playerDesc, transform, restore)
		{
			primary = primaryMember ?? throw new ArgumentNullException();

			if (primary.isEmpty) throw new ArgumentException("Cannot have an empty primary builder");

			secondary = secondaryMember ?? throw new ArgumentNullException();

			_index = indexMaker++;
			bodyTypes.AddAt(this, _index);
		}

		//initializes the main skin, main fur, primary, and secondary values for this type.
		internal virtual void Init(out Epidermis mainSkin, out Epidermis mainFur, out Epidermis secondaryEpidermis)
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

			ParseEpidermisDataOnTransform(mainFur, mainSkin, new Epidermis(), new HairData(Guid.Empty), out secondaryEpidermis);
		}

		//validate is called after deserialization. Validate makes the following assumptions that the following are true after deserialization:
		//mainFur, mainSkin are valid - at worst
		//epidermis types for main fur, main skin are valid. this is guarenteed by the body's validate.
		internal static bool Validate(ref BodyType bodyType, Epidermis mainSkin, Epidermis mainFur, ref Epidermis secondaryEpidermis, in HairData hairData, bool correctInvalidData)
		{
			if (!bodyTypes.Contains(bodyType))
			{
				if (correctInvalidData)
				{
					bodyType = HUMANOID;
					bodyType.ParseEpidermisDataOnTransform(mainFur, mainSkin, secondaryEpidermis, hairData, out secondaryEpidermis);
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
		internal abstract void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis);

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
			new ToneBodyMember(EpidermisType.SKIN, DefaultValueHelpers.defaultHumanTone, false, SkinDesc, YourBodyDesc),
			SkinLongDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);

		public static readonly CompoundToneBodyType REPTILIAN = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, false, ScalesDesc),
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, false, ScalesUnderbodyDesc, YourUnderScalesDesc),
			ScalesLongDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);

		public static readonly CompoundToneBodyType NAGA = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultNagaTone, false, NagaDesc),
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultNagaUnderTone, false, NagaUnderbodyDesc, YourUnderNagaDesc),
			NagaLongDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);

		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType();
		public static readonly KitsuneBodyType KITSUNE = new KitsuneBodyType();
		public static readonly SimpleToneBodyType WOODEN = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.BARK, DefaultValueHelpers.defaultBarkColor, true, BarkDesc),
			BarkLongDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		////one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly SimpleFurBodyType SIMPLE_FUR = new SimpleFurBodyType(GENERIC_FUR_MEMBER, FurLongDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly CompoundFurBodyType UNDERBODY_FUR = new CompoundFurBodyType(GENERIC_FUR_MEMBER,
			new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurUnderbodyDesc, YourUnderFurDesc),
			FurLongDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		public static readonly CompoundFurBodyType FEATHERED = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, false, FeatherDesc),
			new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, false, UnderFeatherDesc, YourUnderFeatherDesc),
			FeatherLongDesc, FeatherPlayerStr, FeatherTransformStr, FeatherRestoreStr);

		public static readonly CompoundFurBodyType WOOL = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, false, WoolDesc),
			new FurBodyMember(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, false, WoolUnderbodyDesc, YourUnderWoolDesc),
			WoolLongDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		////now, if you have gooey body, give the goo innards perk. simple.
		////Also: Goo body is getting a rework/revamp. it was originally a spaghetti code of a mess of partially implemented checks on a perk. now it's its own type. 
		////any body part not "Goo" will act like it should, regardless of the gooey body. It never really made sense before; it still doesn't.
		////You can lampshade this if you feel the need to by saying something like:
		////"Your normally flexible goo-like body solidifies around your arms|legs|cock|wings|whatever, allowing you to use it properly."
		////additionally, if the goo body gains perks or abilities when partially or fully goo, you could add flavor text like: 
		////"if more of your parts shared this gooey structure, you might be able to make better use of your gooey form"
		////a chimera-like monster could get text like: "it's arms|legs|whatever clash with the rest of its goo-like form, though it succeeds in making it more disturbing";

		public static readonly SimpleToneBodyType GOO = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.GOO, DefaultValueHelpers.defaultGooTone, false, GooDesc),
			GooLongDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		////cleaner - we don't need umpteen checks to see if it's "rubbery"
		public static readonly SimpleToneBodyType RUBBER = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.RUBBER, Tones.GRAY, true, RubberDesc),
			RubberLongDesc, RubberPlayerStr, RubberTransformStr, RubberRestoreStr);
		////like a turtle shell or bee exoskeleton.
		public static readonly SimpleToneBodyType CARAPACE = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.CARAPACE, Tones.BLACK, true, CarapaceStr),
			CarapaceLongDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
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
				epidermisType = epidermis ?? throw new ArgumentNullException(nameof(epidermis));

				shortDescription = shortDesc;
				dyeOrToneDescriptor = descriptionText;
			}

			//internal BodyMember()
			//{
			//	shortDescription = GlobalStrings.None;
			//	epidermisType = EpidermisType.EMPTY;
			//	dyeOrToneDescriptor = GlobalStrings.None;
			//}

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
		private protected SimpleBodyType(BodyMember builder, DescriptorWithArg<Body> longDesc, PlayerBodyPartDelegate<Body> playerDesc,
			ChangeType<BodyData> transform, RestoreType<BodyData> restore) : base(builder, longDesc, playerDesc, transform, restore) { }
	}

	public abstract class CompoundBodyType : BodyType
	{
		private protected CompoundBodyType(BodyMember primaryBuilder, BodyMember secondaryBuilder, DescriptorWithArg<Body> longDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, longDesc, playerDesc, transform, restore) { }
	}

	public class SimpleFurBodyType : SimpleBodyType
	{
		internal FurBodyMember furMember => (FurBodyMember)primary;
		public FurBasedEpidermisType furType => (FurBasedEpidermisType)furMember.epidermisType;
		public FurColor defaultFur => furMember.defaultFur;
		public bool overrideFur => furMember.overrideFur;

		internal SimpleFurBodyType(FurBodyMember builder, DescriptorWithArg<Body> longDesc, PlayerBodyPartDelegate<Body> playerDesc,
			ChangeType<BodyData> transform, RestoreType<BodyData> restore) : base(builder, longDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			mainSkin.UpdateEpidermis(EpidermisType.SKIN); //This type doesn't use tone directly, but skin exists under the fur. so make sure the rest of the body uses skin (not scales or something).

			//get the color.
			FurColor overrideColor = overrideFur ? defaultFur : null;
			FurColor color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, hairData.activeHairColor, defaultFur);

			//set the mainFur accordingly. Remember, it can be empty. 
			mainFur.UpdateOrChange(furType, color);
			//primaryEpidermis = mainFur; //set the primary to reference mainFur.

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

		internal CompoundFurBodyType(FurBodyMember primaryBuilder, FurBodyMember secondaryBuilder, DescriptorWithArg<Body> longDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, longDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
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
		}
	}
	public class SimpleToneBodyType : SimpleBodyType
	{
		internal ToneBodyMember toneMember => (ToneBodyMember)primary;
		public ToneBasedEpidermisType toneType => (ToneBasedEpidermisType)epidermisType;
		public Tones defaultTone => toneMember.defaultTone;
		public bool overrideTone => toneMember.overrideTone;

		internal SimpleToneBodyType(ToneBodyMember builder, DescriptorWithArg<Body> longDesc, PlayerBodyPartDelegate<Body> playerDesc,
			ChangeType<BodyData> transform, RestoreType<BodyData> restore) : base(builder, longDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			mainFur.Reset(); //we aren't using mainFur, so reset it to empty.

			Tones overrideValue = overrideTone ? defaultTone : null;
			Tones color = BodyHelpers.GetValidTone(overrideValue, mainSkin.tone, defaultTone);

			mainSkin.UpdateOrChange(toneType, color);

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

		internal CompoundToneBodyType(ToneBodyMember primaryBuilder, ToneBodyMember secondaryBuilder, DescriptorWithArg<Body> longDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, longDesc, playerDesc, transform, restore) { }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
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
		}
	}
	public sealed class KitsuneBodyType : CompoundBodyType
	{
		public Tones defaultTone => ((ToneBodyMember)primary).defaultTone;
		public FurColor defaultFur => ((FurBodyMember)secondary).defaultFur;
		internal KitsuneBodyType() : base(
			new ToneBodyMember(EpidermisType.SKIN, DefaultValueHelpers.defaultKitsuneSkin, false, KitsuneDesc, YourDescriptor(EpidermisType.SKIN)),
			new FurBodyMember(EpidermisType.FUR, DefaultValueHelpers.defaultKitsuneFur, false, KitsuneUnderbodyDesc, YourDescriptor(EpidermisType.FUR)),
			KitsuneLongDesc, KitsunePlayerStr, KitsuneTransformStr, KitsuneRestoreStr)
		{ }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			mainSkin.UpdateEpidermis(epidermisType);

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
				new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, false, CockatriceDesc, YourDescriptor(EpidermisType.FEATHERS)),
				new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultCockatriceScaleTone, false, CockatriceUnderbodyDesc, YourDescriptor(EpidermisType.SCALES)),
				CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
		{ }

		internal override void ParseEpidermisDataOnTransform(Epidermis mainFur, Epidermis mainSkin, Epidermis currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			FurColor color = BodyHelpers.GetValidFurColor(null, mainFur.fur, hairData.activeHairColor, defaultFeathers);
			mainFur.UpdateEpidermis((FurBasedEpidermisType)epidermisType, color);

			mainSkin.UpdateEpidermis(secondaryEpidermisType);
			secondaryEpidermis = mainSkin;
		}
	}

	//consider making this public when dealing with the body data bullshit.
	public sealed class BodyData : BehavioralSaveablePartData<BodyData, Body, BodyType>
	{

		public readonly EpidermalData activeFur;
		public readonly EpidermalData mainSkin;

		public readonly EpidermalData main; //the current epidermis data, in an immutable form. never empty or null.
		public readonly EpidermalData supplementary; //the current supplementary epidermis data, if any, in immutable form. note that this can be empty, but will never be null.


		public readonly HairFurColors hairColor; //current hair color. if the character cannot and therefore does not have hair, this will be empty. otherwise, this will be valid, even if the character is currently bald.
		public HairFurColors activeHairColor => hasHair ? hairColor : HairFurColors.NO_HAIR_FUR; //this is the same as hairColor, but will be empty if the character is bald. 
		public readonly bool hasHair; //boolean determining if the character has any hair. this will be false if the character is bald or cannot grow hair.

		internal BodyData(Guid id, Epidermis primary, Epidermis secondary, Epidermis fur, Epidermis skin, in HairData hairData, BodyType bodyType) : base(id, bodyType)
		{
			main = primary.AsReadOnlyData();
			supplementary = secondary.AsReadOnlyData();

			hairColor = hairData.hairColor;
			hasHair = !hairData.hairDeactivated;

			activeFur = fur.AsReadOnlyData();
			mainSkin = skin.AsReadOnlyData();
		}

		internal BodyData(Guid id, EpidermalData primary, EpidermalData secondary, EpidermalData fur, EpidermalData skin, in HairData hairData, BodyType bodyType) : base(id, bodyType)
		{
			main = primary ?? throw new ArgumentNullException(nameof(primary));
			supplementary = secondary ?? throw new ArgumentNullException(nameof(secondary));

			hairColor = hairData.hairColor;
			hasHair = !hairData.hairDeactivated;

			activeFur = fur ?? throw new ArgumentNullException(nameof(fur));
			mainSkin = skin ?? throw new ArgumentNullException(nameof(skin));
		}

		internal BodyData(Guid id) : base(id, BodyType.defaultValue)
		{
			main = new Epidermis(BodyType.defaultValue.epidermisType).AsReadOnlyData();
			supplementary = new EpidermalData();

			hairColor = Hair.DEFAULT_COLOR;
			hasHair = true;

			activeFur = new EpidermalData();
			mainSkin = main;
		}
	}
}
