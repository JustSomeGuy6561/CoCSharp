﻿//Body.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{



	//NOTE: primary epidermis here stores the current skin/scales/etc tone even if fur is in use. that's because under that fur is some form of skin.
	//generally, you'll use the epidermal data to get this data, but weird cases arise when you want the skin under the fur/feathers, so a skin tone
	//has been provided. similarly, there is an edge case where the creature has fur, but their primary epidermis does not use it. as such, if you want
	//the fur color ignoring the current primary, that too has been provided. thanks, Kitsunes!

	//secondary epidermis should only store one kind and set the other to empty, though i've probably missed a few cases. regardless, epidermal data will
	//fix that for me, so use that if you're looking for that data. no alternatives have been provided. 
	public enum NavelPiercingLocation { TOP, BOTTOM }

	public sealed class Body : BehavioralSaveablePart<Body, BodyType>, IHairAware //IDyeable, IToneable
	{
		private static readonly HairFurColors DEFAULT_HAIR = HairFurColors.BLACK;
		private const JewelryType AVAILABLE_NAVEL_PIERCINGS = JewelryType.CURVED_BARBELL | JewelryType.DANGLER | JewelryType.RING | JewelryType.STUD | JewelryType.SPECIAL;
		//Hair, Fur, Tone
		private HairFurColors hairColor => hairData().hairColor;

		public EpidermalData primaryEpidermis => _primaryEpidermis.GetEpidermalData();
		//kitsune is weird af man.
		public EpidermalData secondaryEpidermis => type.hasUnderBody ? _secondaryEpidermis.GetEpidermalData() : new EpidermalData();

		private readonly Epidermis _primaryEpidermis;
		private readonly Epidermis _secondaryEpidermis;

		public readonly Piercing<NavelPiercingLocation> navelPiercings;

		public Tones SkinTone => _primaryEpidermis.tone;
		public FurColor FurColor => type.usesUnderFurInstead ? _secondaryEpidermis.fur : _primaryEpidermis.fur;
		public override BodyType type { get; protected set; }

		public override bool isDefault => type == BodyType.HUMANOID;

		private Body(BodyType bodyType)
		{
			type = bodyType ?? throw new ArgumentNullException();

			_primaryEpidermis = Epidermis.GenerateEmpty();
			_secondaryEpidermis = Epidermis.GenerateEmpty();
			if (bodyType.isFurry)
			{
				FurBodyType furBody = (FurBodyType)type;
				_primaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)type.epidermisType, furBody.defaultFurColor);
				if (furBody.hasUnderBody)
				{
					_secondaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)type.epidermisType, furBody.defaultFurColor);
				}
			}
			else if (bodyType.isTone)
			{
				ToneBodyType toneBody = (ToneBodyType)type;
				_primaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)type.epidermisType, toneBody.defaultTone, true);
				if (toneBody.hasUnderBody)
				{
					_secondaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)type.epidermisType, toneBody.defaultTone);
				}
			}
			else if (bodyType.isCockatrice)
			{
				CockatriceBodyType cockatriceBody = (CockatriceBodyType)type;
				_primaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)cockatriceBody.epidermisType, cockatriceBody.defaultFur);
				_secondaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)cockatriceBody.secondaryEpidermisType, cockatriceBody.defaultScales, true);
			}
			else if (bodyType.isKitsune)
			{
				KitsuneBodyType kitsuneBody = (KitsuneBodyType)type;
				_primaryEpidermis.UpdateEpidermis((ToneBasedEpidermisType)kitsuneBody.epidermisType, kitsuneBody.defaultSkin);
				_primaryEpidermis.ChangeFur(kitsuneBody.defaultFur); // needs to override.
				_secondaryEpidermis.UpdateEpidermis((FurBasedEpidermisType)kitsuneBody.secondaryEpidermisType, kitsuneBody.defaultFur);
			}

			navelPiercings = new Piercing<NavelPiercingLocation>(AVAILABLE_NAVEL_PIERCINGS, PiercingLocationUnlocked);
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			BodyType bodyType = type;
			HairFurColors hair = hairColor;
			bool valid = BodyType.Validate(ref bodyType, _primaryEpidermis, _secondaryEpidermis, ref hair, correctDataIfInvalid);
			if (valid || correctDataIfInvalid)
			{
				valid &= navelPiercings.Validate();
			}
			return valid;
		}

		public bool isFurry => type.isFurry;
		public bool isTone => type.isTone;
		public bool isCockatrice => type.isCockatrice;

		private bool PiercingLocationUnlocked(NavelPiercingLocation piercingLocation)
		{
			return true;
		}

		internal BodyData ToBodyData()
		{
			return new BodyData(_primaryEpidermis, _secondaryEpidermis, hairColor, type);
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

		//i hate you so much.
		internal static Body GenerateCockatrice(FurColor featherColor, Tones scaleColor, FurTexture featherTexture = FurTexture.NONDESCRIPT, SkinTexture scaleTexture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(BodyType.COCKATRICE);
			//the constructor automatically initializes these to default values. this overrides them if valid
			if (!FurColor.IsNullOrEmpty(featherColor))
			{
				retVal._primaryEpidermis.ChangeFur(featherColor);
			}
			retVal._primaryEpidermis.ChangeTexture(featherTexture);
			if (!Tones.isNullOrEmpty(scaleColor))
			{
				retVal._secondaryEpidermis.ChangeTone(scaleColor);
			}
			retVal._secondaryEpidermis.ChangeTexture(scaleTexture);
			return retVal;
		}

		internal static Body GenerateKitsune(Tones skinTone, FurColor furColor, SkinTexture skinTexture = SkinTexture.NONDESCRIPT, FurTexture furTexture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(BodyType.KITSUNE);
			//the constructor automatically initializes these to default values. this overrides them if valid
			if (!FurColor.IsNullOrEmpty(furColor))
			{
				retVal._primaryEpidermis.ChangeFur(furColor);
				retVal._secondaryEpidermis.ChangeFur(furColor);
			}
			retVal._primaryEpidermis.ChangeTexture(furTexture);
			retVal._secondaryEpidermis.ChangeTexture(furTexture);

			if (!Tones.isNullOrEmpty(skinTone))
			{
				retVal._primaryEpidermis.ChangeTone(skinTone);
			}
			retVal._primaryEpidermis.ChangeTexture(skinTexture);
			return retVal;
		}

		internal static Body GenerateTonedNoUnderbody(ToneBodyType toneBody, Tones primaryTone, SkinTexture texture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (!Tones.isNullOrEmpty(primaryTone))
			{
				retVal._primaryEpidermis.ChangeTone(primaryTone);
			}
			retVal._primaryEpidermis.ChangeTexture(texture);
			return retVal;
		}
		internal static Body GenerateToneWithUnderbody(ToneBodyType toneBody, Tones primaryTone, Tones secondaryTone,
			SkinTexture primaryTexture = SkinTexture.NONDESCRIPT, SkinTexture secondaryTexture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (!Tones.isNullOrEmpty(primaryTone))
			{
				retVal._primaryEpidermis.ChangeTone(primaryTone);
			}
			retVal._primaryEpidermis.ChangeTexture(primaryTexture);

			if (!Tones.isNullOrEmpty(secondaryTone) && toneBody.hasUnderBody)
			{
				retVal._secondaryEpidermis.ChangeTone(secondaryTone);
			}
			retVal._secondaryEpidermis.ChangeTexture(secondaryTexture);

			return retVal;
		}

		internal static Body GenerateFurredNoUnderbody(FurBodyType furryBody, FurColor primaryFur, FurTexture texture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!FurColor.IsNullOrEmpty(primaryFur))
			{
				retVal._primaryEpidermis.ChangeFur(primaryFur);
			}
			retVal._primaryEpidermis.ChangeTexture(texture);
			return retVal;
		}
		internal static Body GenerateFurredWithUnderbody(FurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur,
			FurTexture primaryTexture = FurTexture.NONDESCRIPT, FurTexture secondaryTexture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!FurColor.IsNullOrEmpty(primaryFur))
			{
				retVal._primaryEpidermis.ChangeFur(primaryFur);
			}
			retVal._primaryEpidermis.ChangeTexture(primaryTexture);
			if (!FurColor.IsNullOrEmpty(secondaryFur))
			{
				retVal._secondaryEpidermis.ChangeFur(secondaryFur);
			}
			retVal._secondaryEpidermis.ChangeTexture(secondaryTexture);
			return retVal;
		}
		#endregion
		#region Updates

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			if (_primaryEpidermis.fur.isEmpty) //can't be null
			{

				if (!hairColor.isEmpty)
				{
					_primaryEpidermis.ChangeFur(new FurColor(hairColor));
				}
				else
				{
					_primaryEpidermis.ChangeFur(cockatriceBodyType.defaultFur);
				}
			}
			if (_secondaryEpidermis.tone.isEmpty) //can't be null
			{
				_secondaryEpidermis.ChangeTone(cockatriceBodyType.defaultScales);
			}
			type = cockatriceBodyType;
			return true;
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture featherTexture = FurTexture.NONDESCRIPT, SkinTexture scaleTexture = SkinTexture.NONDESCRIPT)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			//if both null, make new one default.
			if (Tones.isNullOrEmpty(scaleTone) && _secondaryEpidermis.tone.isEmpty) //epidermis values are never null. argument passed may be.
			{
				scaleTone = cockatriceBodyType.defaultScales;
			}
			else if (!Tones.isNullOrEmpty(scaleTone))
			{
				_secondaryEpidermis.ChangeTone(scaleTone);
			}
			//else secondaryEpidermis is set already and we can't replace it. do nothing.
			_secondaryEpidermis.ChangeTexture(scaleTexture);
			if (FurColor.IsNullOrEmpty(featherColor) && _primaryEpidermis.fur.isEmpty)
			{
				featherColor = cockatriceBodyType.defaultFur;
			}
			//if only one fur is good, and it's the new color, replace the null color.
			else if (!FurColor.IsNullOrEmpty(featherColor))
			{
				_primaryEpidermis.ChangeFur(featherColor);
			}
			//else primary epidermis fur is set alread and we can't replace it. do nothing
			_primaryEpidermis.ChangeTexture(featherTexture);

			type = cockatriceBodyType;
			return true;
		}

		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor, FurColor secondaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			else if (FurColor.IsNullOrEmpty(primaryColor) && FurColor.IsNullOrEmpty(secondaryColor))
			{
				return UpdateBody(furryType);
			}
			else if (!furryType.hasUnderBody || FurColor.IsNullOrEmpty(secondaryColor))
			{
				return UpdateBody(furryType, primaryColor);
			}
			//past this point, secondaryColor is guarenteed to not be null.

			//set the primary fur. if we can use the passed value, do so.
			if (!FurColor.IsNullOrEmpty(primaryColor))
			{
				_primaryEpidermis.ChangeFur(primaryColor);
			}
			//if not, and the fur is not currently set, use the hair value.
			else if (_primaryEpidermis.fur.isEmpty)
			{
				_primaryEpidermis.ChangeFur(new FurColor(hairColor));
			}

			//set the secondary fur.
			if (furryType.hasUnderBody)
			{
				_secondaryEpidermis.ChangeFur(secondaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
			}

			type = furryType;
			return true;
		}
		public bool UpdateBody(FurBodyType furryType, FurColor primaryColor)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}
			else if (FurColor.IsNullOrEmpty(primaryColor))
			{
				return UpdateBody(furryType);
			}
			_primaryEpidermis.ChangeFur(primaryColor);
			//set the secondary fur.
			if (furryType.hasUnderBody && _secondaryEpidermis.fur.isEmpty)
			{
				_secondaryEpidermis.ChangeFur(primaryColor);
			}
			//or clear it
			else if (!furryType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}

			type = furryType;
			return true;
		}
		public bool UpdateBody(FurBodyType furryType)
		{
			//same type? quick exit.
			if (type == furryType)
			{
				return false;
			}


			//set the primary fur (and hair, if needed)
			if (_primaryEpidermis.fur.isEmpty && hairColor.isEmpty)
			{
				_primaryEpidermis.ChangeFur(furryType.defaultFurColor);
			}
			else if (_primaryEpidermis.fur.isEmpty)
			{
				_primaryEpidermis.ChangeFur(new FurColor(hairColor));
			}
			//set the secondary fur.
			if (furryType.hasUnderBody && _secondaryEpidermis.fur.isEmpty)
			{
				_secondaryEpidermis.ChangeFur(_primaryEpidermis.fur);
			}
			else if (!furryType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}

			type = furryType;
			return true;
		}

		public bool UpdateBody(ToneBodyType toneType, Tones primaryColor, Tones secondaryColor)
		{
			if (type == toneType)
			{
				return false;
			}
			else if (Tones.isNullOrEmpty(primaryColor) && Tones.isNullOrEmpty(secondaryColor))
			{
				return UpdateBody(toneType);
			}
			else if (Tones.isNullOrEmpty(secondaryColor) || !toneType.hasUnderBody)
			{
				return UpdateBody(toneType, primaryColor);
			}

			if (!Tones.isNullOrEmpty(primaryColor))
			{
				_primaryEpidermis.ChangeTone(primaryColor);
			}
			if (toneType.hasUnderBody)
			{
				_secondaryEpidermis.ChangeTone(secondaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
			}

			type = toneType;
			return true;
		}
		public bool UpdateBody(ToneBodyType toneType, Tones primaryColor)
		{
			if (type == toneType)
			{
				return false;
			}
			else if (Tones.isNullOrEmpty(primaryColor))
			{
				return UpdateBody(toneType);
			}

			_primaryEpidermis.ChangeTone(primaryColor);
			if (toneType.hasUnderBody && _secondaryEpidermis.tone.isEmpty)
			{
				_secondaryEpidermis.ChangeTone(primaryColor);
			}
			else
			{
				_secondaryEpidermis.Reset();
			}
			type = toneType;
			return true;
		}
		public bool UpdateBody(ToneBodyType toneType)
		{
			if (type == toneType)
			{
				return false;
			}

			if (!toneType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}
			else if (_secondaryEpidermis.tone.isEmpty)
			{
				_secondaryEpidermis.ChangeTone(_primaryEpidermis.tone);
			}
			type = toneType;
			return true;
		}
		#endregion
		#region Changes
		public bool ChangePrimarySkinTexture(SkinTexture skinTexture)
		{
			return _primaryEpidermis.ChangeTexture(skinTexture);
		}

		public bool ChangeSecondarySkinTexture(SkinTexture skinTexture)
		{
			return _secondaryEpidermis.ChangeTexture(skinTexture);
		}

		public bool ChangePrimaryFurTexture(FurTexture furTexture)
		{
			return _primaryEpidermis.ChangeTexture(furTexture);
		}

		public bool ChangeSecondaryFurTexture(FurTexture furTexture)
		{
			return _secondaryEpidermis.ChangeTexture(furTexture);
		}

		public bool ChangePrimaryFurColor(FurColor furColor)
		{
			return _primaryEpidermis.ChangeFur(furColor);
		}

		public bool ChangePrimaryToneColor(Tones tone)
		{
			return _primaryEpidermis.ChangeTone(tone);
		}

		public bool ChangeSecondaryFurColor(FurColor furColor)
		{
			return _secondaryEpidermis.ChangeFur(furColor, true);
		}

		public bool ChangeSecondaryToneColor(Tones tone)
		{
			return _secondaryEpidermis.ChangeTone(tone, true);
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

		public readonly bool hasUnderBody;
		public readonly SimpleDescriptor underBodyDescription;
		public override int index => _index;
		private readonly int _index;

		public virtual bool usesUnderFurInstead => false;


		public readonly EpidermisType epidermisType;
		public virtual EpidermisType secondaryEpidermisType => epidermisType;

		internal static bool Validate(ref BodyType bodyType, Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false)
		{
			bool valid = true;
			if (bodyType == null == !bodyTypes.Contains(bodyType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				bodyType = HUMANOID;
				valid = false;
			}
			valid &= bodyType.ValidateData(primary, secondary, ref hairColor, correctInvalidData);
			return valid;
		}
		internal abstract bool ValidateData(Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false);

		private protected BodyType(EpidermisType type, SimpleDescriptor underbodyDescript,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
			underBodyDescription = underbodyDescript;
			hasUnderBody = underbodyDescript != GlobalStrings.None;

			_index = indexMaker++;
			bodyTypes.AddAt(this, _index);
		}

		private protected BodyType(EpidermisType type,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = type;
			underBodyDescription = GlobalStrings.None;
			hasUnderBody = false;

			_index = indexMaker++;
			bodyTypes.AddAt(this, _index);
		}

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
#warning Consider replacing hard coded values with some sort of global reference to species where available. most dont have this options, so idk.

		public static readonly ToneBodyType HUMANOID = new ToneBodyType(EpidermisType.SKIN, Tones.LIGHT, SkinDesc, SkinFullDesc, SkinPlayerStr, SkinTransformStr, SkinRestoreStr);
		public static readonly ToneBodyType REPTILIAN = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, ScalesUnderbodyDesc, ScalesDesc, ScalesFullDesc, ScalesPlayerStr, ScalesTransformStr, ScalesRestoreStr);
		public static readonly ToneBodyType NAGA = new ToneBodyType(EpidermisType.SCALES, Tones.DARK_RED, NagaUnderbodyDesc, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly CockatriceBodyType COCKATRICE = new CockatriceBodyType(new FurColor(HairFurColors.WHITE), Tones.TAN);
		public static readonly KitsuneBodyType KITSUNE = new KitsuneBodyType(Tones.TAN, new FurColor(HairFurColors.WHITE));
		public static readonly ToneBodyType WOODEN = new ToneBodyType(EpidermisType.BARK, Tones.WOODLY_BROWN, BarkDesc, BarkFullDesc, BarkPlayerStr, BarkTransformStr, BarkRestoreStr);
		//one color (or two in a pattern, like zebra stripes) over the entire body.
		public static readonly FurBodyType SIMPLE_FUR = new FurBodyType(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);

		//the anthropomorphic equivalent of underbody, at least. this means that most of the body is the first color (or pattern), while the chest is the other. note that this may also
		//effect the arms, legs, and face (and possibly others if implemented), as they may utilize both or just one of these colors, depending on the type. 
		public static readonly FurBodyType UNDERBODY_FUR = new FurBodyType(EpidermisType.FUR, new FurColor(HairFurColors.BLACK), FurUnderbodyDesc, FurDesc, FurFullDesc, FurPlayerStr, FurTransformStr, FurRestoreStr);
		public static readonly FurBodyType WOOL = new FurBodyType(EpidermisType.WOOL, new FurColor(HairFurColors.WHITE), WoolUnderbodyDesc, WoolDesc, WoolFullDesc, WoolPlayerStr, WoolTransformStr, WoolRestoreStr);
		//now, if you have gooey body, give the goo innards perk. simple.
		public static readonly ToneBodyType GOO = new ToneBodyType(EpidermisType.GOO, Tones.CERULEAN, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		//cleaner - we don't need umpteen checks to see if it's "rubbery"
		public static readonly ToneBodyType RUBBER = new ToneBodyType(EpidermisType.RUBBER, Tones.GRAY, RubberDesc, RubberFullDesc, RubberPlayerStr, RubberTransformStr, RubberRestoreStr);
		//like a turtle shell or bee exoskeleton.
		public static readonly ToneBodyType CARAPACE = new ToneBodyType(EpidermisType.CARAPACE, Tones.BLACK, CarapaceStr, CarapaceFullDesc, CarapacePlayerStr, CarapaceTransformStr, CarapaceRestoreStr);

		public bool isFurry => this is FurBodyType;
		public bool isTone => this is ToneBodyType;
		public bool isCockatrice => this is CockatriceBodyType;
		public bool isKitsune => this is KitsuneBodyType;
	}

	public class FurBodyType : BodyType
	{
		public readonly FurColor defaultFurColor;
		internal FurBodyType(EpidermisType type, FurColor defFur,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = new FurColor(defFur);
		}

		internal FurBodyType(EpidermisType type, FurColor defFur, SimpleDescriptor underbodyDesc,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFurColor = new FurColor(defFur);
		}
		internal override bool ValidateData(Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false)
		{
			bool valid = true;
			if (primary.fur.isEmpty || primary.type != epidermisType || (hasUnderBody && secondary.fur.isEmpty) || (hasUnderBody && secondary.type != secondaryEpidermisType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
			}
			if (primary.fur.isEmpty)
			{
				primary.ChangeFur(defaultFurColor);
			}

			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (hasUnderBody && secondary.fur.isEmpty)
			{
				secondary.ChangeFur(defaultFurColor);
			}
			if (hasUnderBody && secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
			return valid;
		}

	}

	public class ToneBodyType : BodyType
	{
		public readonly Tones defaultTone;
		internal ToneBodyType(EpidermisType type, Tones defTone,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}

		internal ToneBodyType(EpidermisType type, Tones defTone, SimpleDescriptor underbodyDesc,
			SimpleDescriptor shortDesc, DescriptorWithArg<Body> fullDesc, TypeAndPlayerDelegate<Body> playerDesc,
			ChangeType<Body> transform, RestoreType<Body> restore) : base(type, underbodyDesc, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTone = defTone;
		}

		internal override bool ValidateData(Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false)
		{
			bool valid = true;
			if (primary.tone.isEmpty || primary.type != epidermisType || (hasUnderBody && secondary.type != secondaryEpidermisType) || (hasUnderBody && secondary.tone.isEmpty))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
			}
			if (primary.tone.isEmpty)
			{
				primary.ChangeTone(defaultTone);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (hasUnderBody && secondary.tone.isEmpty)
			{
				secondary.ChangeTone(defaultTone);
			}
			if (hasUnderBody && secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
			return valid;
		}
	}

	public class CockatriceBodyType : BodyType
	{
		public readonly FurColor defaultFur;
		public readonly Tones defaultScales;
		internal CockatriceBodyType(FurColor feathers, Tones underbodyScales) : base(EpidermisType.FEATHERS, CockatriceUnderbodyDesc,
			CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
		{
			defaultFur = feathers;
			defaultScales = underbodyScales;
		}

		public override EpidermisType secondaryEpidermisType => EpidermisType.SCALES;

		internal override bool ValidateData(Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false)
		{
			bool valid = true;
			if (primary.fur.isEmpty || primary.type != epidermisType || secondary.tone.isEmpty || secondary.type != secondaryEpidermisType)
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
			}
			if (FurColor.IsNullOrEmpty(primary.fur)) //guarenteed not to be null, but idk, futureproof check.
			{
				primary.ChangeFur(defaultFur); //need to overwrite
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (secondary.tone.isEmpty)
			{
				secondary.ChangeTone(defaultScales);
			}
			if (secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
			return valid;
		}
	}

	public class KitsuneBodyType : BodyType
	{
		public readonly FurColor defaultFur;
		public readonly Tones defaultSkin;
		internal KitsuneBodyType(Tones skinTone, FurColor fur) : base(EpidermisType.SKIN, KitsuneUnderbodyDesc, KitsuneDesc, KitsuneFullDesc, KitsunePlayerStr, KitsuneTransformStr, KitsuneRestoreStr)
		{
			defaultFur = fur;
			defaultSkin = skinTone;
		}

		public override bool usesUnderFurInstead => true;
		public override EpidermisType secondaryEpidermisType => EpidermisType.FUR;

		internal override bool ValidateData(Epidermis primary, Epidermis secondary, ref HairFurColors hairColor, bool correctInvalidData = false)
		{
			bool valid = true;
			if (primary.tone.isEmpty || primary.type != epidermisType || primary.fur.isEmpty || secondary.fur.isEmpty || secondary.type != secondaryEpidermisType)
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
			}
			if (primary.tone.isEmpty)
			{
				primary.ChangeTone(defaultSkin);
			}
			if (primary.fur.isEmpty)
			{
				primary.ChangeFur(defaultFur);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (secondary.fur.isEmpty)
			{
				secondary.ChangeFur(defaultFur, true);
			}
			if (secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
			return valid;
		}
	}

	internal sealed class BodyData
	{
		internal readonly Epidermis primary;
		internal readonly Epidermis secondary;
		internal readonly BodyType bodyType;
		internal readonly HairFurColors hairColor;
		internal BodyData(Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			this.primary = Epidermis.Generate(primary);
			this.secondary = Epidermis.Generate(secondary);
			this.hairColor = hairColor;
			this.bodyType = bodyType;
		}
	}
}
