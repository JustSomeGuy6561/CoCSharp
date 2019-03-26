//Body.cs
//Description:
//Author: JustSomeGuy
//1/18/2019, 9:56 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{

	public enum NavelPiercings { TOP, BOTTOM }
	[DataContract]
	public class Body : PiercableBodyPart<Body, BodyType, NavelPiercings> //IDyeable, IToneable
	{
		private static readonly HairFurColors DEFAULT_HAIR = HairFurColors.BLACK;
		//Hair, Fur, Tone
		//[Save]
		public HairFurColors hairColor { get; private set; }

		public EpidermalData primaryEpidermis => _primaryEpidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermis => type.hasUnderBody ? _secondaryEpidermis.GetEpidermalData() : null;

		//types are always correct, as the body updates them when it changes.
		//[Save]
		internal Epidermis _primaryEpidermis;
		//[Save]
		internal Epidermis _secondaryEpidermis;

		private FurColor primaryFur => _primaryEpidermis.fur;
		private Tones primaryTone => _primaryEpidermis.tone;
		private FurColor secondaryFur => _secondaryEpidermis.fur;
		private Tones secondaryTone => _secondaryEpidermis.tone;
		//End Hair/Fur/Tone

		public override BodyType type { get; protected set; }

		public override bool isDefault => type == BodyType.HUMANOID;

		public bool isFurry => type.isFurry;
		public bool isTone => type.isTone;
		public bool isCockatrice => type.isCockatrice;

		private protected Body(BodyType bodyType)
		{
			if (HairFurColors.IsNullOrEmpty(hairColor)) 
			{
				hairColor = HairFurColors.BLACK;
			}
			this.hairColor = hairColor;
			type = bodyType;

			_primaryEpidermis = Epidermis.GenerateEmpty();
			_secondaryEpidermis = Epidermis.GenerateEmpty();
			hairColor = DEFAULT_HAIR;
			if (bodyType.isFurry)
			{
				FurBodyType furBody = (FurBodyType)type;
				hairColor = furBody.defaultFurColor.primaryColor;
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
		}

		#region Generate
		public static Body GenerateDefault()
		{
			return new Body(BodyType.HUMANOID);
		}
		public static Body GenerateDefaultOfType(BodyType bodyType)
		{
			return new Body(bodyType);
		}

		public static Body GenerateHumanoid(Tones skinTone)
		{
			Body retVal = new Body(BodyType.HUMANOID);
			retVal._primaryEpidermis.ChangeTone(skinTone);
			return retVal;
		}

		//i hate you so much.
		public static Body GenerateCockatrice(FurColor featherColor, Tones scaleColor)
		{
			Body retVal = new Body(BodyType.COCKATRICE);
			if (!featherColor.isNoFur())
			{
				retVal._primaryEpidermis.ChangeFur(featherColor);
			}
			if (scaleColor != Tones.NOT_APPLICABLE)
			{
				retVal._secondaryEpidermis.ChangeTone(scaleColor);
			}
			return retVal;
		}

		public static Body GenerateKitsune(Tones skinTone, FurColor furColor)
		{
			Body retVal = new Body(BodyType.KITSUNE);
			if (!furColor.isNoFur())
			{
				retVal._primaryEpidermis.ChangeFur(furColor);
				retVal._secondaryEpidermis.ChangeFur(furColor);
			}
			if (skinTone != Tones.NOT_APPLICABLE)
			{
				retVal._primaryEpidermis.ChangeTone(skinTone);
			}
			return retVal;
		}

		public static Body GenerateTonedNoUnderbody(ToneBodyType toneBody, Tones primaryTone, SkinTexture texture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal._primaryEpidermis.ChangeTone(primaryTone);
			}
			retVal._primaryEpidermis.ChangeTexture(texture);
			return retVal;
		}
		public static Body GenerateToneWithUnderbody(ToneBodyType toneBody, Tones primaryTone, Tones secondaryTone, 
			SkinTexture primaryTexture = SkinTexture.NONDESCRIPT, SkinTexture secondaryTexture = SkinTexture.NONDESCRIPT)
		{
			Body retVal = new Body(toneBody);
			if (primaryTone != Tones.NOT_APPLICABLE)
			{
				retVal._primaryEpidermis.ChangeTone(primaryTone);
			}
			retVal._primaryEpidermis.ChangeTexture(primaryTexture);

			if (secondaryTone != Tones.NOT_APPLICABLE && toneBody.hasUnderBody)
			{
				retVal._secondaryEpidermis.ChangeTone(secondaryTone);
			}
			retVal._secondaryEpidermis.ChangeTexture(secondaryTexture);

			return retVal;
		}

		public static Body GenerateFurredNoUnderbody(FurBodyType furryBody, FurColor primaryFur, FurTexture texture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!primaryFur.isNoFur())
			{
				retVal._primaryEpidermis.ChangeFur(primaryFur);
			}
			retVal._primaryEpidermis.ChangeTexture(texture);
			return retVal;
		}
		public static Body GenerateFurredWithUnderbody(FurBodyType furryBody, FurColor primaryFur, FurColor secondaryFur, 
			FurTexture primaryTexture = FurTexture.NONDESCRIPT, FurTexture secondaryTexture = FurTexture.NONDESCRIPT)
		{
			Body retVal = new Body(furryBody);
			if (!primaryFur.isNoFur())
			{
				retVal._primaryEpidermis.ChangeFur(primaryFur);
			}
			retVal._primaryEpidermis.ChangeTexture(primaryTexture);
			if (!secondaryFur.isNoFur())
			{
				retVal._secondaryEpidermis.ChangeFur(secondaryFur);
			}
			retVal._secondaryEpidermis.ChangeTexture(secondaryTexture);
			return retVal;
		}
		#endregion
		#region Updates

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			//if both null, make new one default.
			if (scaleTone == Tones.NOT_APPLICABLE && _secondaryEpidermis.tone == Tones.NOT_APPLICABLE)
			{
				scaleTone = cockatriceBodyType.defaultScales;
			}
			if (featherColor.isNoFur() && _primaryEpidermis.fur.isNoFur())
			{
				featherColor = cockatriceBodyType.defaultFur;
			}
			//if only one fur is good, and it's the new color, replace the null color.
			if (!featherColor.isNoFur())
			{
				_primaryEpidermis.ChangeFur(featherColor);
			}
			//if it's the old color, do nothing.

			//do the same for tones.
			if (scaleTone != Tones.NOT_APPLICABLE)
			{
				_secondaryEpidermis.ChangeTone(scaleTone);
			}
			type = cockatriceBodyType;
			return true;
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType)
		{
			if (type == cockatriceBodyType)
			{
				return false;
			}
			if (_primaryEpidermis.fur.isNoFur())
			{

				if (hairColor != HairFurColors.NO_HAIR_FUR)
				{
					_primaryEpidermis.ChangeFur(new FurColor(hairColor));
				}
				else
				{
					_primaryEpidermis.ChangeFur(cockatriceBodyType.defaultFur);
				}
			}
			if (_secondaryEpidermis.tone == Tones.NOT_APPLICABLE)
			{
				_secondaryEpidermis.ChangeTone(cockatriceBodyType.defaultScales);
			}
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
			else if (primaryColor.isNoFur() && secondaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			else if (!furryType.hasUnderBody || secondaryColor.isNoFur())
			{
				return UpdateBody(furryType, primaryColor);
			}

			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;

			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			//set the hair color.
			if (hairColor == HairFurColors.NO_HAIR_FUR && primaryColor.isNoFur())
			{
				hairColor = furryType.defaultFurColor.primaryColor;
			}
			else if (hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = primaryColor.primaryColor;
			}
			//set the primary fur. if we can use the passed value, do so.
			if (!primaryColor.isNoFur())
			{
				_primaryEpidermis.ChangeFur(primaryColor);
			}
			//if not, and the fur is not currently set, use the hair value.
			else if (primaryFur.isNoFur())
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
			else if (primaryColor.isNoFur())
			{
				return UpdateBody(furryType);
			}
			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			if (hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = primaryColor.primaryColor;
			}
			_primaryEpidermis.ChangeFur(primaryColor);
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
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
			//Check vals for if stuff changed. it's simpler this way.
			bool previouslyUsedTone = type.epidermisType.usesTone;
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			//set the primary fur (and hair, if needed)
			if (primaryFur.isNoFur() && hairColor == HairFurColors.NO_HAIR_FUR)
			{
				hairColor = furryType.defaultFurColor.primaryColor;
				_primaryEpidermis.ChangeFur(furryType.defaultFurColor);
			}
			else if (primaryFur.isNoFur())
			{
				_primaryEpidermis.ChangeFur(new FurColor(hairColor));
			}
			//set the secondary fur.
			if (furryType.hasUnderBody && secondaryFur.isNoFur())
			{
				_secondaryEpidermis.ChangeFur(primaryFur);
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
			else if (primaryColor == Tones.NOT_APPLICABLE && secondaryColor == Tones.NOT_APPLICABLE)
			{
				return UpdateBody(toneType);
			}
			else if (secondaryColor == Tones.NOT_APPLICABLE || !toneType.hasUnderBody)
			{
				return UpdateBody(toneType, primaryColor);
			}
			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;


			if (primaryColor != Tones.NOT_APPLICABLE)
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
			else if (primaryColor == Tones.NOT_APPLICABLE)
			{
				return UpdateBody(toneType);
			}

			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			_primaryEpidermis.ChangeTone(primaryColor);
			if (toneType.hasUnderBody && secondaryTone == Tones.NOT_APPLICABLE)
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

			bool previouslyUsedFur = type.epidermisType.usesFur;
			FurColor firstFur = new FurColor(primaryFur);
			FurColor secondFur = new FurColor(secondaryFur);
			Tones firstTone = primaryTone;
			Tones secondTone = secondaryTone;

			if (!toneType.hasUnderBody)
			{
				_secondaryEpidermis.Reset();
			}
			else if (secondaryTone == Tones.NOT_APPLICABLE)
			{
				_secondaryEpidermis.ChangeTone(primaryTone);
			}
			type = toneType;
			return true;
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
		#region Helpers
		protected override bool PiercingLocationUnlocked(NavelPiercings piercingLocation)
		{
			return true;
		}
		#endregion
		#region Serialization

		internal override Type[] saveVersions => new Type[] { typeof(BodySurrogateVersion1) };
		internal override Type currentSaveVersion => typeof(BodySurrogateVersion1);
		internal override BodyPartSurrogate<Body, BodyType> ToCurrentSave()
		{
			return new BodySurrogateVersion1()
			{
				bodyType = index,
				primaryEpidermis = _primaryEpidermis,
				secondaryEpidermis = _secondaryEpidermis,
				hairColor = this.hairColor,
				navelPiercings = serializePiercings()
			};
		}

		internal Body(BodySurrogateVersion1 surrogate)
		{
			type = BodyType.Deserialize(surrogate.bodyType);
			_primaryEpidermis = surrogate.primaryEpidermis;
			_secondaryEpidermis = surrogate.secondaryEpidermis;
			hairColor = surrogate.hairColor;
#warning Add piercing jewelry when that's implemented
			deserializePiercings(surrogate.navelPiercings);
		}
		#endregion
	}

	public abstract partial class BodyType : PiercableBodyPartBehavior<BodyType, Body, NavelPiercings>
	{
		private static int indexMaker = 0;

		private static readonly List<BodyType> bodyTypes = new List<BodyType>();

		public readonly bool hasUnderBody;
		public readonly SimpleDescriptor underBodyDescription;
		public override int index => _index;
		private readonly int _index;

		internal abstract void ValidateDataPostInit(Epidermis primary, Epidermis secondary);

		public readonly EpidermisType epidermisType;
		public virtual EpidermisType secondaryEpidermisType => epidermisType;

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

		internal override void ValidateDataPostInit(Epidermis primary, Epidermis secondary)
		{
			if (primary.fur.isNoFur())
			{
				primary.ChangeFur(defaultFurColor);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (hasUnderBody && secondary.fur.isNoFur())
			{
				secondary.ChangeFur(defaultFurColor);
			}
			if (hasUnderBody && secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
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

		internal override void ValidateDataPostInit(Epidermis primary, Epidermis secondary)
		{
			if (primary.tone == Tones.NOT_APPLICABLE)
			{
				primary.ChangeTone(defaultTone);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (hasUnderBody && secondary.tone == Tones.NOT_APPLICABLE)
			{
				secondary.ChangeTone(defaultTone);
			}
			if (hasUnderBody && secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
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

		internal override void ValidateDataPostInit(Epidermis primary, Epidermis secondary)
		{
			if (primary.fur.isNoFur())
			{
				primary.ChangeFur(defaultFur);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (secondary.tone == Tones.NOT_APPLICABLE)
			{
				secondary.ChangeTone(defaultScales);
			}
			if (secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
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

		public override EpidermisType secondaryEpidermisType => EpidermisType.FUR;

		internal override void ValidateDataPostInit(Epidermis primary, Epidermis secondary)
		{
			if (primary.tone == Tones.NOT_APPLICABLE)
			{
				primary.ChangeTone(defaultSkin);
			}
			if (primary.type != epidermisType)
			{
				primary.UpdateEpidermis(epidermisType);
			}
			if (secondary.fur.isNoFur())
			{
				secondary.ChangeFur(defaultFur);
			}
			if (secondary.type != secondaryEpidermisType)
			{
				secondary.UpdateEpidermis(secondaryEpidermisType);
			}
		}
	}

	[DataContract]
	[KnownType(typeof(Epidermis))]
	[KnownType(typeof(bool[]))]
	public sealed class BodySurrogateVersion1 : BodyPartSurrogate<Body, BodyType>
	{
		[DataMember]
		public int bodyType;
		[DataMember]
		public HairFurColors hairColor;
		[DataMember]
		public Epidermis primaryEpidermis;
		[DataMember]
		public Epidermis secondaryEpidermis;
		[DataMember]
		public bool[] navelPiercings;

		internal override Body ToBodyPart()
		{
			return new Body(this);
		}
	}
}
