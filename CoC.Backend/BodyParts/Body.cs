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

namespace CoC.Backend.BodyParts
{
	//Note: rubber is now a perk. we'll give it some flavor text as a perk, and include an option for perks to display text during player appearance.

	//Dev note: rubber perk grants you an optional rubber coating, which will either cause special text in some scenes or unlock special scenes.
	//this is up to the content creators to implement, however. it's stackable, so some scenes may be different depending on what stack you're at or only
	//unlock once you've reached a certain level. this perk can be destacked or removed outright, if an item/tf wants to. currently river root is the only thing to do this

	//rubber perk adds 1 to mutant score.

	//Trying to come up with decent logic for this thing is a pain in the ass. This is maybe the 5th revision. I think it works well enough now.

	//First off, it should be noted that these body relates to the core of the creature, the anthropomorphied 'chest,' so to speak. it's responsible for setting the
	//skin tone and fur color, and anything that uses fur or skin tone should parse this data accordingly before setting their color or whatever.
	//This is not to be consfused with build - this simply sets the type and tells the body what it's covered in, and thus doesn't deal with physique.


	//Now for how it all works:

	//we store two "Main" elements: Fur and Skin. Fur stores an epidermis that uses fur colors (fur or feathers or wool in current implementation).
	//Skin stores an epidermis dealing with Tone Colors (skin, scales, carapace, goo, etc). For the most part, body parts that make use of one main
	//will not use the other. These will be given priority when dyeing or toneing.

	//We also have two Epidermis instances: primary and secondary. These can be any epidermis type, though the primary cannot be empty.
	//The values for primary and secondary are determined from the body type itself -
	//	the primary type is either the main fur or the main skin, depending on which one the body type uses.
	//	The secondary is based on what the body type declares it to be.

	//For example: A fur-based body type will make the primary point to mainFur, and secondary to a new fur instance if it has an underbody.
	//Similarly, a tone-based body type will point to mainSkin for primary, and set the secondary to a new tone-based instance if it has an underbody.

	//stranger things occur with mixed types; cockatrice and kitsune both use mainFur and mainSkin;
	//for cockatrice, feathers are dominant, so primary is mainFur, secondary is mainSkin. for Kitsune, the opposite (primary=mainSKin, secondary=mainFur)

	//this allows the best of both worlds - we can tell body parts to respond to the fur or skin first, based on which is primary, but when attempting to change the skin tone or fur color,
	//we dont have to find out where the main skin or fur is stored. similarly, if a body part needs a fur color, and does not care if it's primary or not, we can simply call the main fur.
	//It also allows cases where body parts need the skinTone, but the body is covered in fur. Technically, there is skin under the fur, so the mainSkin equates to that.
	//This does lead to a weird caveat where we could potentially double proc an effect if we apply it to both the mainFur/skin and the secondary, and the secondary references the mainFur/Skin.
	//fortunately, we do have a few ways to detect that, the simplest being ReferenceEquals. ReferenceEquals(a, b) is true if a and b point to the same object.

	//finally, there are helper booleans for the main skin and fur telling whether or not they are active in the current body type. this allows for even more complicated behavior, if that's what you want.

	//Some body types may have the exact same underlying data, but be presented in different ways. currently, there are several different types that all use
	//scales for the main epidermis and scales for the secondary epidermis, but are described differently. While we try to avoid this, as it causes things to become more complex,
	//it's perfectly ok, and in some cases it's necessary so things make sense - you would't want to describe fish scales the same way you would a naga's scaled.

	//currently, the only type to do this is Naga - it could be taken care of in Scales, but it's weird because underside and such, and powerful stomach muscles
	//defining the form (along with the giant tail for a leg)


	//Quick Aside: Fur color supports two colors at once, so it's possible to do something like a cat with orange and grey stripes, and a white underbody.
	//Tones, however, only have one, as the original logic only ever had one tone. If you need two, see if you can get away with just using main and suppliment.
	//if not, i could add it in, though that would require some rewrite - basically, it would require a new ToneColor class, which works the same as the FurColor class.
	//all references to Tones would need to be changed to ToneColor, made readonly, and passed by value instead of by reference where applicable - not hard, just tedious.

	//Also worth noting: We do weird shit with dyes - because we allow dual-colored fur/feathers, we need the ability to create this multicolored magic with dyes, but also need to provide the option
	//to dye to a single color. Conceptually, it's really easy, which means implementing it in the frontend is straightforward (though you do need to deal with type checking),
	//but actually getting it all to work regardless of the current body type means it gets conviluted really quickly. i'm sorry, i know it's not elegant, but i've done what i can.

	//Final Note: Validation assumes that the data is properly set during serialization - that is, the body type's Init was called, or, barring that, mainFur and mainSkin were at least initialized correctly.

	public enum NavelPiercingLocation { TOP, BOTTOM }

	//i mean, i don't actually know how you'd do dermal piercings in the land of swords and such, but, uhhh... video game logic. It seems to be important to people in some
	//circles, and i'm not going to prevent anyone from recreating their OC because i'm worried about plausibility in a video game.
	public enum HipPiercingLocation { LEFT_TOP, LEFT_CENTER, LEFT_BOTTOM, RIGHT_TOP, RIGHT_CENTER, RIGHT_BOTTOM }

	internal enum ToneDyeLotionLocations : byte { EVERYTHING, PRIMARY, ALTERNATE }
	internal static class ToneDyeLotionExtensions
	{
		public static bool IsDefined(this ToneDyeLotionLocations location)
		{
			return Enum.IsDefined(typeof(ToneDyeLotionLocations), location);
		}
	}

	public sealed partial class Body : BehavioralSaveablePart<Body, BodyType, BodyData>, IMultiPatternable, IMultiLotionable, IMultiToneable
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
		public HairFurColors hairColor => hairData.hairColor;

		public bool hairActive => !hairData.hairDeactivated;

		//optimization so i don't keep recreating it every time i need it. that's dumb. idk if c# does this automatically though, but if it does, oh well.

		public EpidermalData mainEpidermis
		{
			get
			{
				if (_mainEpidermis?.IsIdenticalTo(primary) != true) //false or null.
				{
					_mainEpidermis = primary.AsReadOnlyData();
				}
				return _mainEpidermis;
			}
		}
		private EpidermalData _mainEpidermis = null;
		public EpidermalData supplementaryEpidermis
		{
			get
			{
				if (_supplementaryEpidermis?.IsIdenticalTo(secondary) != true)
				{
					_supplementaryEpidermis = secondary.AsReadOnlyData();
				}
				return _supplementaryEpidermis;
			}
		}
		private EpidermalData _supplementaryEpidermis = null;

		public bool hasSecondaryEpidermis => !secondary.isEmpty;

		//private readonly Epidermis primaryEpidermis;
		//private readonly Epidermis secondaryEpidermis;

		public readonly Piercing<NavelPiercingLocation> navelPiercings;
		public readonly Piercing<HipPiercingLocation> hipPiercings;
		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;
		//use these if the part does not care on the state of the body, but just needs the main color.

		//the current skin the creature has.
		private readonly Epidermis mainSkin; //stores the current skin that is primarily used. if it's multi-tone, the secondary tone is stored in the supplementary epidermis.

		//stores the current fur that is primarily used. if it's a multi-fur, the secondary fur is stored in supplementary epidermis. if it's no-fur, it's expected to be empty
		//though there is nothing that expressly requires this. regardless, any value in main fur will not be used when inactive.
		private readonly Epidermis mainFur;
		public bool furActive => ReferenceEquals(mainFur, primary) || ReferenceEquals(mainFur, secondary);
		public EpidermalData primarySkin => mainSkin.AsReadOnlyData();
		public EpidermalData activeFur => furActive ? mainFur.AsReadOnlyData() : new EpidermalData();

		private bool skinActive => ReferenceEquals(mainSkin, primary) || ReferenceEquals(mainSkin, secondary);

		private Epidermis primary => type.primaryIsFur ? mainFur : mainSkin;
		//secondary is completely determined by the body type itself. Note that there's nothing stopping primary and secondary pointing to the same object, though this should never happen.
		private Epidermis secondary;

		public FurColor ActiveHairOrFurColor()
		{
			if (furActive)
			{
				return mainFur.fur;
			}
			else if (!hairData.hairDeactivated)
			{
				return new FurColor(hairData.activeHairColor);
			}
			else
			{
				return new FurColor(Hair.DEFAULT_COLOR);
			}
		}

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
				type.ParseBodyDataOnTransform(mainFur, mainSkin, oldSecondary, hairData, out secondary);
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
				var oldData = new BodyData(this, oldSkin, oldFur, oldPrimary, oldSecondary);
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
			return new BodyData(this);
		}

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(AsReadOnlyData());

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(AsReadOnlyData());

		public string FullDescription(bool alternateFormat = false) => type.FullDescription(AsReadOnlyData(), alternateFormat);

		//describe the main epidermis (no body)
		public string MainDescription() => type.MainDescription();
		public string MainDescription(out bool isPlural) => type.MainDescription(out isPlural);

		//describe the supplementary epidermis (no body). Empty string if no supplementary epidermis
		public string SupplementaryDescription() => type.SupplementaryDescription();
		public string SupplementaryDescription(out bool isPlural) => type.SupplementaryDescription(out isPlural);

		//describe the main and supplementary epidermis (if applicable) without mentioning body.
		public string ShortEpidermisDescription() => type.ShortDescriptionWithoutBody();
		public string ShortEpidermisDescription(out bool isPlural) => type.ShortDescriptionWithoutBody(out isPlural);

		//same as above, but the more verbose version.
		public string LongEpidermisDescription() => type.LongDescriptionWithoutBody(AsReadOnlyData());
		public string LongEpidermisDescription(out bool isPlural) => type.LongDescriptionWithoutBody(AsReadOnlyData(), out isPlural);

		#region MultiPatternable
		//3 possibilities: all fur, primary fur, or secondary fur. if there is no under fur available, primary and secondary are disabled.
		byte IMultiDyeable.numDyeableMembers => 3;

		string IMultiDyeable.buttonText()
		{
			return Name();
		}


		string IMultiDyeable.memberButtonText(byte index)
		{
			return ButtonText(index, false);
		}

		string IMultiDyeable.memberLocationDesc(byte index, out bool isPlural)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				isPlural = false;
				return "";
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return type.AllDyeDescription(out isPlural);
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return type.PrimaryDyeDescription(out isPlural);
			}
			else
			{
				return type.SecondaryDyeDescription(out isPlural);
			}
		}

		string IMultiDyeable.memberPostDyeDescription(byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return "";
			}
			if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return type.PostDyeTextAll(this);
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return type.PostDyeTextPrimary(this);
			}
			else
			{
				return type.PostDyeTextAlternate(this);
			}
		}

		//only allow us to dye the primary and secondary if the body allows a secondary color. if not, primary and everything are identical and we don't want to confuse anyone.
		bool IMultiDyeable.allowsDye(byte index)
		{
			//Note: a weird edge case is technically possible where one fur type could not be mutable and the other is. in that case, it would make sense to disable everything
			//and enable primary or secondary, because that actually makes sense in this case. currently, all fur epidermis types are mutable, so this will never happen.
			//if this changes, feel free to clean up this logic.

			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined() || !furActive)
			{
				return false;
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				//check if either one uses fur, and it's mutable.
				return primary.furMutable || secondary.furMutable;
			}
			else
			{
				//check if both use fur, and both are mutable.
				return primary.furMutable && secondary.furMutable;
			}
		}

		bool IMultiDyeable.isDifferentColor(HairFurColors dyeColor, byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;

			if (!location.IsDefined() || !dyeable.allowsDye(index) || !furActive)
			{
				return false;
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return !mainFur.fur.IsIdenticalTo(dyeColor);
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return !secondary.fur.IsIdenticalTo(dyeColor);
			}
			else
			{
				return !mainFur.fur.IsIdenticalTo(dyeColor) || !secondary.fur.IsIdenticalTo(dyeColor);
			}
		}

		bool IMultiDyeable.attemptToDye(HairFurColors dye, byte index)
		{
			if (!dyeable.allowsDye(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			bool success = false;
			//alt or everything.
			if (location != ToneDyeLotionLocations.PRIMARY)
			{
				success |= secondary.ChangeFur(dye);
			}
			//primary or everything.
			if (location != ToneDyeLotionLocations.ALTERNATE)
			{
				success |= primary.ChangeFur(dye);
			}

			return success;
		}

		bool IMultiPatternable.allowsPatterning(byte index)
		{
			//handles invalid indices.
			return dyeable.allowsDye(index);
		}

		bool IMultiPatternable.isDifferentPrimaryColor(HairFurColors dyeColor, byte index)
		{
			//handles invalid indices.
			if (!patternable.allowsPatterning(index))
			{
				return false;
			}
			//all indicies from this point forward are valid.
			var location = (ToneDyeLotionLocations)index;
			if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return differentColor(primary.fur, dyeColor, true);
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return differentColor(secondary.fur, dyeColor, true);
			}
			else
			{
				return differentColor(primary.fur, dyeColor, true) || differentColor(secondary.fur, dyeColor, true);
			}
		}

		bool IMultiPatternable.isDifferentSecondaryColor(HairFurColors dyeColor, byte index)
		{
			//handles invalid indices.
			if (!patternable.allowsPatterning(index))
			{
				return false;
			}
			//all indicies from this point forward are valid.
			var location = (ToneDyeLotionLocations)index;
			if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return differentColor(primary.fur, dyeColor, false);
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return differentColor(secondary.fur, dyeColor, false);
			}
			else
			{
				return differentColor(primary.fur, dyeColor, false) || differentColor(secondary.fur, dyeColor, false);
			}
		}

		bool IMultiPatternable.attemptToPattern(HairFurColors dyeColor, FurMulticolorPattern pattern, bool primaryColor, byte index)
		{
			if (!patternable.allowsPatterning(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			bool success = false;
			//alt or everything.
			if (location != ToneDyeLotionLocations.PRIMARY)
			{
				FurColor temp = primaryColor ? new FurColor(dyeColor, secondary.fur.GetSecondaryColor(), pattern) : new FurColor(secondary.fur.primaryColor, dyeColor, pattern);
				success |= secondary.ChangeFur(temp);
			}
			//primary or everything.
			if (location != ToneDyeLotionLocations.ALTERNATE)
			{
				FurColor temp = primaryColor ? new FurColor(dyeColor, primary.fur.GetSecondaryColor(), pattern) : new FurColor(primary.fur.primaryColor, dyeColor, pattern);
				success |= primary.ChangeFur(temp);
			}

			return success;
		}

		private bool differentColor(FurColor color, HairFurColors dye, bool primary)
		{
			//only true if the color is not multi-colored. from this point forward, we can assume that color.secondaryColor != color.primaryColor or the solid color != dye
			if (color.IsIdenticalTo(dye))
			{
				//primary color == dye, secondary color empty or equal to primary color. thus, regardless of primary flag, we know the color is the same.
				return false;
			}
			else if (primary)
			{
				return color.primaryColor != dye;
			}
			else
			{
				//if we're here, either
				//	A: the secondary color is empty, and the primary color is not the dye color, OR
				//	B: the secondary color is the primary color and the primary color is not the dye color.

				//either way, that means we don't match. thus return true.
				return true;
			}
		}

		private IMultiDyeable dyeable => this;
		private IMultiPatternable patternable => this;

		#endregion
		#region MultiLotionable
		//primary, secondary, or all. note that if the current body does not use skin for main or supplementary epidermis, it is still available and still can be updated.
		//note that some epidermis types may prevent you from updating the texture. this is respected here.
		byte IMultiLotionable.numLotionableMembers => 3;

		string IMultiLotionable.buttonText()
		{
			return Name();
		}

		string IMultiLotionable.memberButtonText(byte index)
		{
			return ButtonText(index, true);
		}

		string IMultiLotionable.memberLocationDesc(byte index, out bool isPlural)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				isPlural = false;
				return "";
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return AllToneText(out isPlural);
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return PrimaryToneText(out isPlural);
			}
			else
			{
				return SecondaryToneText(out isPlural);
			}
		}

		SkinTexture IMultiLotionable.postUseSkinTexture(byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return SkinTexture.NONDESCRIPT;
			}

			if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return primary.skinTexture;
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return secondary.skinTexture;
			}
			else //(location == ToneDyeLotionLocations.EVERYTHING)
			{
				return mainSkin.skinTexture;
			}
		}

		bool IMultiLotionable.canLotion(byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return false;
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return mainSkin.toneMutable || primary.toneMutable || secondary.toneMutable;
			}
			else
			{
				return primary.toneMutable && secondary.toneMutable;
			}
		}

		bool IMultiLotionable.isDifferentTexture(SkinTexture lotionTexture, byte index)
		{
			if (!lotionable.canLotion(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return primary.skinTexture != lotionTexture;
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return secondary.skinTexture != lotionTexture;
			}
			else
			{
				return mainSkin.skinTexture != lotionTexture || (secondary.usesTone && secondary.skinTexture != lotionTexture);
			}
		}

		bool IMultiLotionable.attemptToLotion(SkinTexture lotionTexture, byte index)
		{
			if (!lotionable.canLotion(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			bool success = false;

			if (skinActive)
			{
				//alt or everything.
				if (location != ToneDyeLotionLocations.PRIMARY)
				{
					success |= secondary.ChangeTexture(lotionTexture);
				}
				//primary or everything.
				if (location != ToneDyeLotionLocations.ALTERNATE)
				{
					success |= primary.ChangeTexture(lotionTexture);
				}
			}
			else
			{
				success |= mainSkin.ChangeTexture(lotionTexture);
			}

			return success;
		}

		private IMultiLotionable lotionable => this;

		#endregion
		#region IMultiToneable
		//primary, secondary, or all. note that if the current body does not use skin for main or supplementary epidermis, it is still available and still can be updated.
		//note that some epidermis types may prevent you from updating the tone. this is respected here.
		byte IMultiToneable.numToneableMembers => 3;

		string IMultiToneable.buttonText()
		{
			return Name();
		}

		string IMultiToneable.memberButtonText(byte index)
		{
			return ButtonText(index, true);
		}

		string IMultiToneable.memberLocationDesc(byte index, out bool isPlural)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				isPlural = false;
				return "";
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return AllToneText(out isPlural);
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return PrimaryToneText(out isPlural);
			}
			else
			{
				return SecondaryToneText(out isPlural);
			}
		}

		string IMultiToneable.memberPostToneDescription(byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return "";
			}
			if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				return type.PostToneTextAll(this);
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return type.PostToneTextPrimary(this);
			}
			else
			{
				return type.PostToneTextAlternate(this);
			}
		}

		bool IMultiToneable.canToneOil(byte index)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return false;
			}
			else if (location == ToneDyeLotionLocations.EVERYTHING)
			{
				//can tone, even if no skin currently active (you always have skin under fur/feathers)
				return mainSkin.toneMutable || primary.toneMutable || secondary.toneMutable;
			}
			else
			{
				return primary.toneMutable && secondary.toneMutable;
			}
		}

		bool IMultiToneable.isDifferentTone(Tones oilTone, byte index)
		{
			if (!toneable.canToneOil(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return primary.tone != oilTone;
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return secondary.tone != oilTone;
			}
			else
			{
				return mainSkin.tone != oilTone || (secondary.usesTone && secondary.tone != oilTone);
			}
		}

		bool IMultiToneable.attemptToTone(Tones oilTone, byte index)
		{
			if (!lotionable.canLotion(index))
			{
				return false;
			}
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			bool success = false;

			if (skinActive)
			{
				//alt or everything.
				if (location != ToneDyeLotionLocations.PRIMARY)
				{
					success |= secondary.ChangeTone(oilTone);
				}
				//primary or everything.
				if (location != ToneDyeLotionLocations.ALTERNATE)
				{
					success |= primary.ChangeTone(oilTone);
				}
			}
			else
			{
				success |= mainSkin.ChangeTone(oilTone);
			}

			return success;
		}

		private IMultiToneable toneable => this;
		#endregion
		#region Tone and Lotion Helpers
		private string AllToneText(out bool isPlural) => type.AllToneDescription(out isPlural);
		private string PrimaryToneText(out bool isPlural) => type.PrimaryToneDescription(out isPlural);
		private string SecondaryToneText(out bool isPlural) => type.SecondaryToneDescription(out isPlural);
		#endregion
		#region Pattern Tone and Lotion Helpers
		private string ButtonText(byte index, bool isTone)
		{
			ToneDyeLotionLocations location = (ToneDyeLotionLocations)index;
			if (!location.IsDefined())
			{
				return "";
			}
			else if (location == ToneDyeLotionLocations.PRIMARY)
			{
				return type.PrimaryButtonDescription(isTone);
			}
			else if (location == ToneDyeLotionLocations.ALTERNATE)
			{
				return type.SecondaryButtonDescription(isTone);
			}
			else
			{
				return type.AllButtonDescription(isTone);
			}
		}
		#endregion
	}

	//this is a clusterfuck, but it's now able to handle everything i need it to.
	//honestly, it might be more readible (readable? not sure, need sleep) to just make body type completely abstract and have each type implement it as their own instead of this hybrid.
	//idk.


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

		public bool hasFurOrFeathersOrWool => primaryIsFur || secondaryIsFur;

		public bool primaryIsFur => primary.usesFur;
		public bool secondaryIsFur => secondary.usesFur;

		public bool hasScales => primary.epidermisType == EpidermisType.SCALES || secondary.epidermisType == EpidermisType.SCALES;

		public EpidermisType epidermisType => primary.epidermisType;
		public EpidermisType secondaryEpidermisType => secondary.epidermisType;

		public string MainDescription() => primary.MemberDescription(out bool _);
		public virtual string MainDescription(out bool isPlural) => primary.MemberDescription(out isPlural);


		public string SupplementaryDescription() => secondary.MemberDescription(out bool _);
		public string SupplementaryDescription(out bool isPlural) => secondary.MemberDescription(out isPlural);


		//the same as the standard long descriptor, but without the 'body' text. this is useful if you just want to say "your fur and scales" or whatever.

		private readonly PartDescriptor<BodyData> fullDescriptor;

		public string FullDescription(BodyData bodyData, bool alternateFormat = false)
		{
			return fullDescriptor(bodyData, alternateFormat);
		}

		public string FullDescriptionPrimary(BodyData body) => fullDescriptor(body, false);

		public string FullDescriptionAlternate(BodyData body) => fullDescriptor(body, true);


		//allows you to rename the buttons for dyeing and toning. by default, they are "body" and "underbody". For example, Cockatrice uses this to say "feathers"
		//when dyeing primary, and "scales" when toning secondary. Also, furry types override "body" with "skin" when lotioning/oiling, as it affects the skin under the fur/feathers.
		private protected BodyType(BodyMember primaryMember, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: this(primaryMember, new EmptyBodyMember(), shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{ }

		private protected BodyType(BodyMember primaryMember, BodyMember secondaryMember, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(shortDesc, longDesc, playerDesc, transform, restore)
		{
			primary = primaryMember ?? throw new ArgumentNullException();

			if (primary.isEmpty) throw new ArgumentException("Cannot have an empty primary builder");

			secondary = secondaryMember ?? throw new ArgumentNullException();

			_index = indexMaker++;
			bodyTypes.AddAt(this, _index);

			fullDescriptor = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));
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

			HandleTypeChange(mainFur, mainSkin, new EpidermalData(), new HairData(Guid.Empty), out secondaryEpidermis);
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
					bodyType.ParseBodyDataOnTransform(mainFur, mainSkin, secondaryEpidermis.AsReadOnlyData(), hairData, out secondaryEpidermis);
				}
				return false;
			}
			return true;
		}


		#region Dye/Tone/Lotion

#warning consider adding these back in.
		//these are only virtual if you want to do some weird edge case - like the fur is too thick, preventing you from rubbing the lotion on the skin... underneath.
		//(Silence of the Lambs reference? Check!)
		//internal virtual bool allowsPrimaryDye => primary.usesFur && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryDye => secondary.usesFur && secondary.epidermisType.usesFur; //either usesTone and tone is mutabl

		//internal virtual bool allowsPrimaryOil => primary.usesTone && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryOil => secondary.usesTone && secondary.epidermisType.usesTone;

		//internal virtual bool allowsPrimaryLotion => primary.usesTone && primary.epidermisType.updateable;
		//internal virtual bool allowsSecondaryLotion => secondary.usesTone && secondary.epidermisType.updateable;

		//these provide text for when the body is lotioned, toned, or dyed. it looks complicated and unweildy (it is), but it means we have full control over what displays
		//when doing this, making it as flexible as we want. Unfortunately, body is supremely complicated because it can be dyed, toned, or lotioned, and toned/lotioned can occur
		//even if the skin isn't in use, so we need a lot of flexibility to make it sound correct.

		internal abstract string AllButtonDescription(bool isTone);

		internal abstract string PrimaryButtonDescription(bool isTone);

		internal abstract string SecondaryButtonDescription(bool isTone);

		internal abstract string AllDyeDescription(out bool isPlural);

		internal abstract string PrimaryDyeDescription(out bool isPlural);

		internal abstract string SecondaryDyeDescription(out bool isPlural);

		internal abstract string AllToneDescription(out bool isPlural);

		internal abstract string PrimaryToneDescription(out bool isPlural);

		internal abstract string SecondaryToneDescription(out bool isPlural);

		internal abstract string PostDyeTextAll(Body body);

		internal abstract string PostDyeTextPrimary(Body body);

		internal abstract string PostDyeTextAlternate(Body body);

		internal abstract string PostToneTextAll(Body body);

		internal abstract string PostToneTextPrimary(Body body);

		internal abstract string PostToneTextAlternate(Body body);

		#endregion


		internal void ParseBodyDataOnTransform(Epidermis mainFur, Epidermis mainSkin, in EpidermalData currSecondary, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			HandleTypeChange(mainFur, mainSkin, in currSecondary, in hairData, out secondaryEpidermis);
			if (mainFur.usesTone) throw new Exception("Internal error: a body part parse function incorrectly updated the main fur to a tone based epidermis type.");
			if (mainSkin.usesFur) throw new Exception("Internal error: a body part parse function incorrectly updated the main skin to a fur based epidermis type.");
		}

		//called on transform. if you break the main fur or main skin (updating the main fur to use a tone, or main skin to use a fur), the caller will immediately throw.
		private protected abstract void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData currSecondary, in HairData hairData, out Epidermis secondaryEpidermis);

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
		private static FurBodyMember GeneratePrimaryFurMember(bool hasUnderbody)
		{
			if (hasUnderbody)
			{
				return new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurPrimaryDesc, MainFurButton, GenericPostDesc);
			}
			else
			{
				return new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurDescNoType, MainFurButtonNoUnderbody, GenericPostDesc);
			}

		}

		public static readonly SimpleToneBodyType HUMANOID = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.SKIN, DefaultValueHelpers.defaultHumanTone, false, SkinDescNoType, GenericPostDesc, true),
			SkinDesc, SkinLongDesc, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);

		public static readonly CompoundToneBodyType DRAGON = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, false, DragonBodyDesc, MainScalesButton, GenericPostDesc),
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, false, DragonUnderbodyDesc, AlternateScalesButton, GenericPostDesc),
			AllScalesButton, AllScalesDesc, GenericPostDesc, DragonDesc, DragonLongDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);

		public static readonly CompoundToneBodyType REPTILE = new CompoundToneBodyType(
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, false, ReptileBodyDesc, MainScalesButton, GenericPostDesc),
			new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, false, ReptileUnderbodyDesc, AlternateScalesButton, GenericPostDesc),
			AllScalesButton, AllScalesDesc, GenericPostDesc, ReptileDesc, ReptileLongDesc, ReptileFullDesc, ReptilePlayerStr, ReptileTransformStr, ReptileRestoreStr);

		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType();
		public static readonly KitsuneBodyType KITSUNE = new KitsuneBodyType();
		public static readonly SimpleToneBodyType WOODEN = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.BARK, DefaultValueHelpers.defaultBarkColor, true, BarkDescNoType, GenericPostDesc, true),
			BarkDesc, BarkLongDesc, BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);

		//NOTE: Fur represents all body types that use fur. there are two variations: simple and underbody. Simple will have one color (or color pattern) for the entire body
		//while underbody will have one color for most of the body and a second color for underbody (or anthropomorphic equivalent).
		//Changing the fur color will have different behaviors based on these types - the simple will only react when the primary color is changed, whereas the underbody will
		//react when either fur color is changed, and will only change that color - so if you update the secondary color, the primary color will remain unchanged.
		//There is a variant that changes ALL fur colors, which sets the primary color and secondary color (if applicable) to the given value.

		//Use whichever behavior type you'd prefer for your given species or transformation. Going from simple to underbody will simply set the secondary color to the primary color
		//(which you can change to another color if needed), and going from underbody to simple will just discard the secondary color.

		//The better way would be to either have a unique body type for each distinct species that has a furry body transformation, or simply have one class and handle the underbody
		//manually everywhere. This exists as sort of a bridge - legacy code doesn't handle underbody, but may in the future if implemented, so for now we need a means to say
		//this doesn't support underbody (yet), so ignore any underbody. Note that it's always possible to treat an underbody as a simple type just by setting the underbody (secondary)
		//fur color to match the primary color.

		//future idea: remove simple fur. give the body class a boolean property for allows underbody color, and change the update type functions to allow you to set this value
		//also create a change underbody fur status function that can update this boolean, but only if the current type is fur.


		//one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly SimpleFurBodyType SIMPLE_FUR = new SimpleFurBodyType(GeneratePrimaryFurMember(false), FurDesc, FurLongDesc, FurFullDesc,
			FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type.
		public static readonly CompoundFurBodyType UNDERBODY_FUR = new CompoundFurBodyType(GeneratePrimaryFurMember(true),
			new FurBodyMember(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), false, FurUnderBodyDesc, AlternateFurButton, GenericPostDesc),
			AllFurButton, AllFurDye, AllFurTone, GenericPostDesc, PostAllFurTone, FurDesc, FurLongDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		public static readonly CompoundFurBodyType FEATHERED = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, false, PrimaryFeatherDesc, MainFeathersButton, GenericPostDesc),
			new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, false, UnderFeatherDesc, AlternateFeathersButton, GenericPostDesc),
			AllFeathersButton, AllFeathersDye, AllFeathersTone, GenericPostDesc, PostAllFeathersTone, FeatherDesc, FeatherLongDesc, FeatherFullDesc, FeatherPlayerStr,
			FeatherTransformStr, FeatherRestoreStr);

		public static readonly CompoundFurBodyType WOOL = new CompoundFurBodyType(
			new FurBodyMember(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, false, WoolBodyDesc, MainWoolButton, GenericPostDesc),
			new FurBodyMember(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, false, WoolUnderbodyDesc, AlternateWoolButton, GenericPostDesc),
			AllWoolButton, AllWoolDye, AllWoolTone, GenericPostDesc, PostAllWoolTone, WoolDesc, WoolLongDesc, WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		//now, if you have gooey body, give the goo innards perk. simple.
		//Also: Goo body is getting a rework/revamp. it was originally a spaghetti code of a mess of partially implemented checks on a perk. now it's its own type.
		//any body part not "Goo" will act like it should, regardless of the gooey body. It never really made sense before; it still doesn't.
		//You can lampshade this if you feel the need to by saying something like:
		//"Your normally flexible goo-like body solidifies around your arms|legs|cock|wings|whatever, allowing you to use it properly."
		//additionally, if the goo body gains perks or abilities when partially or fully goo, you could add flavor text like:
		//"if more of your parts shared this gooey structure, you might be able to make better use of your gooey form"
		//a chimera-like monster could get text like: "it's arms|legs|whatever clash with the rest of its goo-like form, though it succeeds in making it more disturbing";

		public static readonly SimpleToneBodyType GOO = new SimpleToneBodyType(
			new ToneBodyMember(EpidermisType.GOO, DefaultValueHelpers.defaultGooTone, false, GooDescNoType, GenericPostDesc, true),
			GooDesc, GooLongDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//like a turtle shell or bee exoskeleton.
		public static readonly SimpleToneBodyType CARAPACE = new SimpleToneBodyType(new ToneBodyMember(EpidermisType.CARAPACE, Tones.BLACK, true, CarapaceDescNoType, GenericPostDesc, true),
			CarapaceDesc, CarapaceLongDesc, CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);
		#endregion

		internal abstract class BodyMember
		{
			internal bool usesTone => epidermisType.usesTone;
			internal bool usesFur => epidermisType.usesFur;

			internal readonly EpidermisType epidermisType;

			internal string MemberDescription(out bool isPlural) => locationDesc(out isPlural);

			internal abstract string FurLocationDescription(out bool isPlural);

			internal abstract string ToneLocationDescription(out bool isPlural);

			internal abstract string PostToneDescription(Body body);
			internal abstract string PostDyeDescription(Body body);

			//what do we call this when the player wants to describe this? i.e. the draconic underbody is "ventral scales". the regular one is just scales or 'standard scales'
			internal readonly DescriptorWithArg<bool> buttonText;
			protected readonly MaybePluralDescriptor locationDesc;
			//how do we describe this after a dye or toner has been applied to it? options that cannot dye or tone should simply return "";
			protected readonly DescriptorWithArg<Body> dyeOrToneDescriptor;

			//body member that cannot be altered by dye or tone.
			internal BodyMember(EpidermisType epidermis, MaybePluralDescriptor locationText, bool primary)
			{
				epidermisType = epidermis ?? throw new ArgumentNullException(nameof(epidermis));

				locationDesc = locationText ?? throw new ArgumentNullException(nameof(locationText));

				buttonText = primary ? (DescriptorWithArg<bool>)MainBodyDesc : (DescriptorWithArg<bool>)UnderBodyDesc;
				dyeOrToneDescriptor = (x) => GlobalStrings.None();
			}

			//default case.
			internal BodyMember(EpidermisType epidermis, MaybePluralDescriptor locationText, DescriptorWithArg<Body> overrideToneDyeText, bool primary)
			{
				epidermisType = epidermis ?? throw new ArgumentNullException(nameof(epidermis));

				locationDesc = locationText ?? throw new ArgumentNullException(nameof(locationText));

				this.buttonText = primary ? (DescriptorWithArg<bool>)MainBodyDesc : (DescriptorWithArg<bool>)UnderBodyDesc;
				dyeOrToneDescriptor = overrideToneDyeText ?? throw new ArgumentNullException(nameof(overrideToneDyeText));

			}

			internal BodyMember(EpidermisType epidermis, MaybePluralDescriptor locationText, DescriptorWithArg<bool> buttonText, DescriptorWithArg<Body> overrideToneDyeText)
			{
				epidermisType = epidermis ?? throw new ArgumentNullException(nameof(epidermis));

				locationDesc = locationText ?? throw new ArgumentNullException(nameof(locationText));

				this.buttonText = buttonText ?? throw new ArgumentNullException(nameof(buttonText));
				dyeOrToneDescriptor = overrideToneDyeText ?? throw new ArgumentNullException(nameof(overrideToneDyeText));

			}

			internal bool isEmpty => this is EmptyBodyMember;
		}

		internal sealed class FurBodyMember : BodyMember
		{
			public readonly FurColor defaultFur;
			public readonly bool overrideFur;
			public FurBasedEpidermisType furType => (FurBasedEpidermisType)epidermisType;

			public FurBodyMember(FurBasedEpidermisType furType, FurColor fallbackColor, bool useFallbackColorOnTF,
				 MaybePluralDescriptor locationText, DescriptorWithArg<Body> overrideToneDyeText, bool primary)
				: base(furType, locationText, overrideToneDyeText, primary)
			{
				defaultFur = fallbackColor;
				overrideFur = useFallbackColorOnTF;
			}

			public FurBodyMember(FurBasedEpidermisType furType, FurColor fallbackColor, bool useFallbackColorOnTF,
				MaybePluralDescriptor locationText, DescriptorWithArg<bool> buttonText, DescriptorWithArg<Body> overrideToneDyeText)
				: base(furType, locationText, buttonText, overrideToneDyeText)
			{
				defaultFur = fallbackColor;
				overrideFur = useFallbackColorOnTF;
			}

			internal override string FurLocationDescription(out bool isPlural)
			{
				return YourLocationStr(locationDesc(out isPlural));
			}

			internal override string ToneLocationDescription(out bool isPlural)
			{
				isPlural = false;
				return TheSkinUnderStr(locationDesc(out bool _));
			}

			internal override string PostToneDescription(Body body)
			{
				return SkinUnderStr(body, locationDesc(out bool _));
			}

			internal override string PostDyeDescription(Body body)
			{
				return dyeOrToneDescriptor(body);
			}
		}

		internal sealed class ToneBodyMember : BodyMember
		{
			public readonly Tones defaultTone;
			public readonly bool overrideTone;

			public ToneBodyMember(ToneBasedEpidermisType furType, Tones fallbackColor, bool useFallbackColorOnTF,
				MaybePluralDescriptor locationText, DescriptorWithArg<Body> overrideToneDyeText, bool primary)
				: base(furType, locationText, overrideToneDyeText, primary)
			{
				defaultTone = fallbackColor;
				overrideTone = useFallbackColorOnTF;
			}

			public ToneBodyMember(ToneBasedEpidermisType furType, Tones fallbackColor, bool useFallbackColorOnTF,
				MaybePluralDescriptor locationText, DescriptorWithArg<bool> buttonText, DescriptorWithArg<Body> overrideToneDyeText)
				: base(furType, locationText, buttonText, overrideToneDyeText)
			{
				defaultTone = fallbackColor;
				overrideTone = useFallbackColorOnTF;
			}

			internal override string ToneLocationDescription(out bool isPlural)
			{
				return YourLocationStr(locationDesc(out isPlural));
			}

			internal override string FurLocationDescription(out bool isPlural)
			{
				isPlural = false;
				return "";
			}

			internal override string PostDyeDescription(Body body)
			{
				return "";
			}

			internal override string PostToneDescription(Body body)
			{
				return dyeOrToneDescriptor(body);
			}
		}

		internal sealed class EmptyBodyMember : BodyMember
		{
			public EmptyBodyMember() : base(EpidermisType.EMPTY, GlobalStrings.PluralNone, false) { }

			internal override string FurLocationDescription(out bool isPlural)
			{
				isPlural = false;
				return "";
			}

			internal override string ToneLocationDescription(out bool isPlural)
			{
				isPlural = false;
				return "";
			}

			internal override string PostDyeDescription(Body body)
			{
				return "";
			}

			internal override string PostToneDescription(Body body)
			{
				return "";
			}


		}
	}

	public abstract class SimpleBodyType : BodyType
	{
		protected readonly DescriptorWithArg<bool> allButtonText;

		private protected SimpleBodyType(BodyMember primaryMember, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allButtonText = AllBodyDesc;
		}

		private protected SimpleBodyType(BodyMember primaryMember, DescriptorWithArg<bool> buttonText, ShortDescriptor shortDesc,
			PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allButtonText = buttonText ?? throw new ArgumentNullException(nameof(buttonText));
		}
		//handle all the tone and dye texts. it's really simple here because nothing is here to fuck it up.

		internal override string AllDyeDescription(out bool isPlural)
		{
			return primary.FurLocationDescription(out isPlural);
		}

		internal override string AllToneDescription(out bool isPlural)
		{
			return primary.ToneLocationDescription(out isPlural);
		}

		internal override string PrimaryDyeDescription(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		internal override string SecondaryDyeDescription(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		internal override string PrimaryToneDescription(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		internal override string SecondaryToneDescription(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		internal override string PostDyeTextAlternate(Body body)
		{
			return "";
		}

		internal override string PostDyeTextPrimary(Body body)
		{
			return "";
		}

		internal override string PostToneTextAlternate(Body body)
		{
			return "";
		}

		internal override string PostToneTextPrimary(Body body)
		{
			return "";
		}

		internal override string PostDyeTextAll(Body body)
		{
			return primary.PostDyeDescription(body);
		}

		internal override string PostToneTextAll(Body body)
		{
			return primary.PostToneDescription(body);
		}

		internal override string AllButtonDescription(bool isTone)
		{
			return allButtonText(isTone);
		}

		//disabled, so we can use generic text.
		internal override string PrimaryButtonDescription(bool isTone)
		{
			return MainBodyDesc(isTone);
		}

		internal override string SecondaryButtonDescription(bool isTone)
		{
			return UnderBodyDesc(isTone);
		}
	}

	public abstract class CompoundBodyType : BodyType
	{
		protected readonly DescriptorWithArg<bool> allButtonText;

		private protected CompoundBodyType(BodyMember primaryBuilder, BodyMember secondaryBuilder, ShortDescriptor shortDesc,
			PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allButtonText = AllBodyDesc;
		}

		private protected CompoundBodyType(BodyMember primaryBuilder, BodyMember secondaryBuilder, DescriptorWithArg<bool> buttonText, ShortDescriptor shortDesc,
			PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allButtonText = buttonText ?? throw new ArgumentNullException(nameof(buttonText));
		}

		//private protected CompoundBodyType(BodyMember primaryBuilder, BodyMember secondaryBuilder,
		//	LongDescriptor<BodyData> longDesc, LongDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
		//	: base(primaryBuilder, secondaryBuilder, longDesc, fullDesc, playerDesc, transform, restore) { }

		//handle all the tone and dye texts. this becomes complicated because a body type can potentially mix fur and tone types.
		//we can't handle 'all' functions because we don't know if it's mixed types.
		internal override string PrimaryDyeDescription(out bool isPlural)
		{
			if (primary.usesFur && secondary.usesFur)
			{
				return primary.FurLocationDescription(out isPlural);
			}
			else
			{
				isPlural = false;
				return "";
			}
		}

		internal override string SecondaryDyeDescription(out bool isPlural)
		{
			if (primary.usesFur && secondary.usesFur)
			{
				return secondary.FurLocationDescription(out isPlural);
			}
			else
			{
				isPlural = false;
				return "";
			}
		}

		internal override string PrimaryToneDescription(out bool isPlural)
		{
			if (primary.usesTone && secondary.usesTone)
			{
				return primary.ToneLocationDescription(out isPlural);
			}
			else
			{
				isPlural = false;
				return "";
			}
		}

		internal override string SecondaryToneDescription(out bool isPlural)
		{
			if (primary.usesTone && secondary.usesTone)
			{
				return secondary.ToneLocationDescription(out isPlural);
			}
			else
			{
				isPlural = false;
				return "";
			}
		}

		internal override string PostDyeTextPrimary(Body body)
		{
			if (primary.usesFur && secondary.usesFur)
			{
				return primary.PostDyeDescription(body);
			}
			else
			{
				return "";
			}
		}

		internal override string PostDyeTextAlternate(Body body)
		{
			if (primary.usesFur && secondary.usesFur)
			{
				return secondary.PostDyeDescription(body);
			}
			else
			{
				return "";
			}
		}

		internal override string PostToneTextPrimary(Body body)
		{
			if (primary.usesTone && secondary.usesTone)
			{
				return primary.PostToneDescription(body);
			}
			else
			{
				return "";
			}
		}

		internal override string PostToneTextAlternate(Body body)
		{
			if (primary.usesTone && secondary.usesTone)
			{
				return secondary.PostToneDescription(body);
			}
			else
			{
				return "";
			}
		}

		internal override string AllButtonDescription(bool isTone)
		{
			return allButtonText(isTone);
		}
	}

	public class SimpleFurBodyType : SimpleBodyType
	{
		internal SimpleFurBodyType(FurBodyMember primaryMember, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{ }

		internal SimpleFurBodyType(FurBodyMember primaryMember, DescriptorWithArg<bool> buttonText, ShortDescriptor shortDesc,
			PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, buttonText, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{ }

		internal FurBodyMember furMember => (FurBodyMember)primary;
		public FurBasedEpidermisType furType => (FurBasedEpidermisType)furMember.epidermisType;
		public FurColor defaultFur => furMember.defaultFur;
		public bool overrideFur => furMember.overrideFur;


		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			mainSkin.UpdateEpidermis(EpidermisType.SKIN); //This type doesn't use tone directly, but skin exists under the fur. so make sure the rest of the body uses skin (not scales or something).

			//get the color.
			FurColor overrideColor = overrideFur ? defaultFur : null;
			FurColor color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, hairData.activeHairColor, defaultFur);

			//set the mainFur accordingly. Remember, it can be empty.
			mainFur.UpdateOrChange(furType, color);

			//clear the secondary, by default. we don't care about the original data.
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

		protected readonly MaybePluralDescriptor allDyeLocation;
		protected readonly MaybePluralDescriptor allToneLocation;
		protected readonly DescriptorWithArg<Body> postDyeAllText;
		protected readonly DescriptorWithArg<Body> postToneAllText;

		internal CompoundFurBodyType(FurBodyMember primaryBuilder, FurBodyMember secondaryBuilder, DescriptorWithArg<bool> buttonText, MaybePluralDescriptor allDyeDesc,
			MaybePluralDescriptor allToneDesc, DescriptorWithArg<Body> postAllDyeDesc, DescriptorWithArg<Body> postAllToneDesc, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, buttonText, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allDyeLocation = allDyeDesc ?? throw new ArgumentNullException(nameof(allDyeDesc));
			allToneLocation = allToneDesc ?? throw new ArgumentNullException(nameof(allToneDesc));
			postDyeAllText = postAllDyeDesc ?? throw new ArgumentNullException(nameof(postAllDyeDesc));
			postToneAllText = postAllToneDesc ?? throw new ArgumentNullException(nameof(postAllToneDesc));
		}

		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			mainSkin.UpdateEpidermis(EpidermisType.SKIN); //This type doesn't use tone directly, but skin exists under the fur. so make sure the rest of the body uses skin (not scales or something).

			//old secondary may point to current mainFur. since this is the case, let's take care of the secondary first.
			FurColor overrideColor = overrideSupplementaryFur ? defaultSupplementaryFur : null;
			FurColor color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, secondaryData.fur, hairData.activeHairColor, defaultSupplementaryFur);
			FurTexture texture = secondaryData.usesFur ? secondaryData.furTexture : mainFur.usesFur ? mainFur.furTexture : FurTexture.NONDESCRIPT;
			secondaryEpidermis = new Epidermis(secondaryFurType, color, texture);

			overrideColor = overrideMainFur ? defaultMainFur : null;
			color = BodyHelpers.GetValidFurColor(overrideColor, mainFur.fur, hairData.activeHairColor, defaultMainFur);

			mainFur.UpdateOrChange(primaryFurType, color);
		}

		internal override string AllDyeDescription(out bool isPlural)
		{
			return allDyeLocation(out isPlural);
		}

		internal override string AllToneDescription(out bool isPlural)
		{
			return allToneLocation(out isPlural);
		}

		internal override string PostDyeTextAll(Body body)
		{
			return postDyeAllText(body);
		}

		internal override string PostToneTextAll(Body body)
		{
			return postToneAllText(body);
		}

		internal override string PrimaryButtonDescription(bool isTone)
		{
			return primary.buttonText(isTone);
		}

		internal override string SecondaryButtonDescription(bool isTone)
		{
			return secondary.buttonText(isTone);
		}
	}
	public class SimpleToneBodyType : SimpleBodyType
	{


		internal ToneBodyMember toneMember => (ToneBodyMember)primary;
		public ToneBasedEpidermisType toneType => (ToneBasedEpidermisType)epidermisType;
		public Tones defaultTone => toneMember.defaultTone;
		public bool overrideTone => toneMember.overrideTone;

		internal SimpleToneBodyType(BodyMember primaryMember, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc,
			PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, shortDesc, longDesc, fullDesc, playerDesc, transform, restore) { }

		internal SimpleToneBodyType(BodyMember primaryMember, DescriptorWithArg<bool> buttonText, ShortDescriptor shortDesc,
			PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc, PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryMember, buttonText, shortDesc, longDesc, fullDesc, playerDesc, transform, restore) { }

		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
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

		protected readonly MaybePluralDescriptor allToneLocation;
		protected readonly DescriptorWithArg<Body> postToneAllText;

		internal CompoundToneBodyType(ToneBodyMember primaryBuilder, ToneBodyMember secondaryBuilder, DescriptorWithArg<bool> buttonText, MaybePluralDescriptor allToneDesc,
			DescriptorWithArg<Body> postAllToneDesc, ShortDescriptor shortDesc, PartDescriptor<BodyData> longDesc, PartDescriptor<BodyData> fullDesc,
			PlayerBodyPartDelegate<Body> playerDesc, ChangeType<BodyData> transform, RestoreType<BodyData> restore)
			: base(primaryBuilder, secondaryBuilder, buttonText, shortDesc, longDesc, fullDesc, playerDesc, transform, restore)
		{
			allToneLocation = allToneDesc ?? throw new ArgumentNullException(nameof(allToneDesc));
			postToneAllText = postAllToneDesc ?? throw new ArgumentNullException(nameof(postAllToneDesc));
		}

		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			mainFur.Reset(); //we aren't using mainFur, so reset it to empty.

			Tones tones = secondaryData.tone;
			if (overrideSupplementaryTone)
			{
				tones = defaultSupplementaryTone;
			}
			else if (tones.isEmpty)
			{
				tones = mainSkin.tone;
			}

			SkinTexture texture = secondaryData.usesTone ? secondaryData.skinTexture : mainSkin.skinTexture;
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

		internal override string PrimaryButtonDescription(bool isTone)
		{
			return primary.buttonText(isTone);
		}

		internal override string SecondaryButtonDescription(bool isTone)
		{
			return secondary.buttonText(isTone);
		}

		internal override string AllDyeDescription(out bool isPlural)
		{
			isPlural = false;
			return "";
		}

		internal override string AllToneDescription(out bool isPlural)
		{
			return allToneLocation(out isPlural);
		}

		internal override string PostDyeTextAll(Body body)
		{
			return "";
		}

		internal override string PostToneTextAll(Body body)
		{
			return postToneAllText(body);
		}
	}
	public sealed class KitsuneBodyType : CompoundBodyType
	{
		public Tones defaultTone => ((ToneBodyMember)primary).defaultTone;
		public FurColor defaultFur => ((FurBodyMember)secondary).defaultFur;
		internal KitsuneBodyType() : base(
			new ToneBodyMember(EpidermisType.SKIN, DefaultValueHelpers.defaultKitsuneSkin, false, KitsuneBodyDesc, KitsuneSkinButton, GenericPostDesc),
			new FurBodyMember(EpidermisType.FUR, DefaultValueHelpers.defaultKitsuneFur, false, KitsuneUnderbodyDesc, KitsuneFurButton, GenericPostDesc),
			KitsuneDesc, KitsuneLongDesc, KitsuneFullDesc, KitsunePlayerStr, KitsuneTransformStr, KitsuneRestoreStr)
		{ }

		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			mainSkin.UpdateEpidermis(epidermisType);

			FurColor color = BodyHelpers.GetValidFurColor(null, mainFur.fur, hairData.activeHairColor, defaultFur);
			mainFur.UpdateEpidermis((FurBasedEpidermisType)secondaryEpidermisType, color);
			secondaryEpidermis = mainFur;
		}

		internal override string PrimaryButtonDescription(bool isTone)
		{
			return MainBodyDesc(isTone);
		}

		internal override string SecondaryButtonDescription(bool isTone)
		{
			return UnderBodyDesc(isTone);
		}

		internal override string AllDyeDescription(out bool isPlural)
		{
			return secondary.FurLocationDescription(out isPlural);
		}

		internal override string AllToneDescription(out bool isPlural)
		{
			return primary.ToneLocationDescription(out isPlural);
		}

		internal override string PostDyeTextAll(Body body)
		{
			return secondary.PostDyeDescription(body);
		}

		internal override string PostToneTextAll(Body body)
		{
			return primary.PostToneDescription(body);
		}
	}
	public sealed class CockatriceBodyType : CompoundBodyType
	{

		public FurColor defaultFeathers => ((FurBodyMember)primary).defaultFur;
		public Tones defaultScales => ((ToneBodyMember)secondary).defaultTone;

		internal CockatriceBodyType() : base(
				new FurBodyMember(EpidermisType.FEATHERS, DefaultValueHelpers.defaultCockatricePrimaryFeathers, false, CockatriceBodyDesc, CockatriceFeathersButton, GenericPostDesc),
				new ToneBodyMember(EpidermisType.SCALES, DefaultValueHelpers.defaultCockatriceScaleTone, false, CockatriceUnderbodyDesc, CockatriceScalesButton, GenericPostDesc),
				CockatriceDesc, CockatriceLongDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
		{ }

		private protected override void HandleTypeChange(Epidermis mainFur, Epidermis mainSkin, in EpidermalData secondaryData, in HairData hairData, out Epidermis secondaryEpidermis)
		{
			//neither override on TF, so we're good.
			FurColor color = BodyHelpers.GetValidFurColor(null, mainFur.fur, hairData.activeHairColor, defaultFeathers);
			mainFur.UpdateEpidermis((FurBasedEpidermisType)epidermisType, color);

			mainSkin.UpdateEpidermis(secondaryEpidermisType);
			secondaryEpidermis = mainSkin;
		}

		internal override string PrimaryButtonDescription(bool isTone)
		{
			return MainBodyDesc(isTone);
		}

		internal override string SecondaryButtonDescription(bool isTone)
		{
			return UnderBodyDesc(isTone);
		}

		internal override string AllDyeDescription(out bool isPlural)
		{
			return primary.FurLocationDescription(out isPlural);
		}

		internal override string AllToneDescription(out bool isPlural)
		{
			return secondary.ToneLocationDescription(out isPlural);
		}

		internal override string PostDyeTextAll(Body body)
		{
			return primary.PostDyeDescription(body);
		}

		internal override string PostToneTextAll(Body body)
		{
			return secondary.PostToneDescription(body);
		}
	}

	public sealed class BodyData : BehavioralSaveablePartData<BodyData, Body, BodyType>
	{

		public readonly EpidermalData activeFur;
		public readonly EpidermalData mainSkin;

		public readonly EpidermalData main; //the current epidermis data, in an immutable form. never empty or null.
		public readonly EpidermalData supplementary; //the current supplementary epidermis data, if any, in immutable form. note that this can be empty, but will never be null.


		public readonly HairFurColors hairColor; //current hair color. if the character cannot and therefore does not have hair, this will be empty. otherwise, this will be valid, even if the character is currently bald.
		public HairFurColors activeHairColor => hasHair ? hairColor : HairFurColors.NO_HAIR_FUR; //this is the same as hairColor, but will be empty if the character is bald.
		public readonly bool hasHair; //boolean determining if the character has any hair. this will be false if the character is bald or cannot grow hair.

		public readonly ReadOnlyPiercing<NavelPiercingLocation> navelPiercings;
		public readonly ReadOnlyPiercing<HipPiercingLocation> hipPiercings;

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(this);

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(this);

		public string FullDescription(bool alternateFormat = false) => type.FullDescription(this, alternateFormat);

		//describe the main epidermis (no body)
		public string MainDescription() => type.MainDescription();
		public string MainDescription(out bool isPlural) => type.MainDescription(out isPlural);

		//describe the supplementary epidermis (no body). Empty string if no supplementary epidermis
		public string SupplementaryDescription() => type.SupplementaryDescription();
		public string SupplementaryDescription(out bool isPlural) => type.SupplementaryDescription(out isPlural);

		//describe the main and supplementary epidermis (if applicable) without mentioning body.
		public string ShortEpidermisDescription() => type.ShortDescriptionWithoutBody();
		public string ShortEpidermisDescription(out bool isPlural) => type.ShortDescriptionWithoutBody(out isPlural);

		//same as above, but the more verbose version.
		public string LongEpidermisDescription() => type.LongDescriptionWithoutBody(this);
		public string LongEpidermisDescription(out bool isPlural) => type.LongDescriptionWithoutBody(this, out isPlural);

		public bool hasAnyFur => !activeFur.isEmpty;

		public override BodyData AsCurrentData()
		{
			return this;
		}

		internal BodyData(Body source) : base(GetID(source), GetBehavior(source))
		{
			main = source.mainEpidermis;
			supplementary = source.supplementaryEpidermis;

			hairColor = source.hairColor;
			hasHair = source.hairActive;

			activeFur = source.activeFur;
			mainSkin = source.primarySkin;

			navelPiercings = source.navelPiercings.AsReadOnlyData();
			hipPiercings = source.hipPiercings.AsReadOnlyData();
		}

		internal BodyData(Body source, EpidermalData mainSkinOverride, EpidermalData mainFurOverride, EpidermalData primaryOverride, EpidermalData secondaryOverride)
			: base(GetID(source), GetBehavior(source))
		{
			main = primaryOverride ?? source.mainEpidermis;
			supplementary = secondaryOverride ?? source.supplementaryEpidermis;

			hairColor = source.hairColor;
			hasHair = source.hairActive;

			activeFur = mainFurOverride ?? source.activeFur;
			mainSkin = mainSkinOverride ?? source.primarySkin;

			navelPiercings = source.navelPiercings.AsReadOnlyData();
			hipPiercings = source.hipPiercings.AsReadOnlyData();
		}

		internal BodyData(Guid id) : base(id, BodyType.defaultValue)
		{
			main = new Epidermis(BodyType.defaultValue.epidermisType).AsReadOnlyData();
			supplementary = new EpidermalData();

			hairColor = Hair.DEFAULT_COLOR;
			hasHair = true;

			activeFur = new EpidermalData();
			mainSkin = main;

			navelPiercings = new ReadOnlyPiercing<NavelPiercingLocation>();
			hipPiercings = new ReadOnlyPiercing<HipPiercingLocation>();
		}
	}
}
