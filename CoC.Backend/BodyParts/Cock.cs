﻿//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public enum CockGroup { HUMAN, MAMMALIAN, CORRUPTED, AQUATIC, REPTILIAN, FLYING, OTHER }

	public enum CockPiercings
	{
		ALBERT,
		FRENUM_UPPER_1, FRENUM_UPPER_2, FRENUM_MIDDLE_1, FRENUM_MIDDLE_2,
		FRENUM_MIDDLE_3, FRENUM_MIDDLE_4, FRENUM_LOWER_1, FRENUM_LOWER_2
	}
	//whoever wrote the AS version of cock, thank you. your cock was awesome. (Obligatory: no homo)
	//i didn't have to search everywhere for the things that were part of it.
	//well, mostly. knots were still a pain.
	//whoever decided to use the AS3 equivalent hack for an enum, though, not so much. that was some ugly ass shit.

	//Note: this class exists after perks have been created, so it's postperk init is not called. 

	public sealed class Cock : BehavioralSaveablePart<Cock, CockType, CockData>, IGrowable, IShrinkable
	{

		#region Consts

		public const float MAX_COCK_LENGTH = 240f;
		public const float MAX_COCK_THICKNESS = 50f;
		public const float MIN_COCK_THICKNESS = 0.2f;
		public const float MAX_KNOT_MULTIPLIER = 10f;
		public const float MIN_KNOT_MULTIPLIER = 1.1f;
		public const float MIN_COCK_LENGTH = 3.0f;

		public const float DEFAULT_COCK_LENGTH = 5.5f;
		//public const float DEFAULT_BIG_COCK_LENGTH = 8f;
		public const float DEFAULT_COCK_GIRTH = 1.25f;
		//public const float DEFAULT_BIG_COCK_GIRTH = 1.5f;

		public const JewelryType SUPPORTED_JEWELRY_FRENUM = JewelryType.BARBELL_STUD | JewelryType.RING;
		public const JewelryType SUPPORTED_JEWELRY_ALBERT = JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;
		#endregion

		#region Properties
		internal float cockGrowthMultiplier;
		internal float cockShrinkMultiplier;
		internal float minCockLength
		{
			get => _minCockLength;
			set => _minCockLength = value;
		}
		private float _minCockLength = MIN_COCK_LENGTH;

		internal float newCockDefaultSize; //needed for reset.

		private float resetLength => Math.Max(newCockDefaultSize, minCockLength);

		public readonly Piercing<CockPiercings> cockPiercings;

		public float knotMultiplier
		{
			get => _knotMultiplier;
			private set
			{
				if (type.hasKnot)
				{
					_knotMultiplier = Utils.Clamp2(value, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
				}
				else _knotMultiplier = 0;
			}
		}
		private float _knotMultiplier;

		public bool hasKnot => knotMultiplier >= 1.1f;

		public float knotSize => type.knotSize(cockGirth, knotMultiplier);

		public float cockLength => _cockLength;
		private float _cockLength;

		public float cockGirth => _cockGirth;
		private float _cockGirth;
		public float cockArea => cockGirth * cockLength;

		public override CockType type
		{
			get => _type;
			protected set
			{
				if (_type != value && value.hasKnot != _type.hasKnot)
				{
					_knotMultiplier = value.baseKnotMultiplier;
				}
				_type = value;
			}
		}
		private CockType _type = CockType.HUMAN;

		public override CockType defaultType => CockType.defaultValue;
		#endregion
		#region Constructors
		internal Cock(Creature source, CockPerkHelper initialPerkValues) : this(source, initialPerkValues, CockType.defaultValue)
		{ }

		internal Cock(Creature source, CockPerkHelper initialPerkValues, CockType cockType) : base(source)
		{
			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			updateLength(initialPerkValues.NewLength());

			knotMultiplier = type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			cockGrowthMultiplier = initialPerkValues.CockGrowthMultiplier;
			cockShrinkMultiplier = initialPerkValues.CockShrinkMultiplier;
			newCockDefaultSize = initialPerkValues.NewCockDefaultSize;
			minCockLength = initialPerkValues.MinCockLength;
		}

		internal Cock(Creature source, CockPerkHelper initialPerkValues, CockType cockType, float length, float girth,
			float? initialKnotMultiplier = null) : base(source)
		{
			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			length = initialPerkValues.NewLength(length);

			updateLengthAndGirth(length, girth);

			knotMultiplier = initialKnotMultiplier ?? type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			cockGrowthMultiplier = initialPerkValues.CockGrowthMultiplier;
			cockShrinkMultiplier = initialPerkValues.CockShrinkMultiplier;
			newCockDefaultSize = initialPerkValues.NewCockDefaultSize;
			minCockLength = initialPerkValues.MinCockLength;
		}
		#endregion

		#region Generate
		internal static Cock GenerateFromGender(Creature source, CockPerkHelper initialPerkValues, Gender gender)
		{
			if (gender.HasFlag(Gender.MALE))
			{
				return new Cock(source, initialPerkValues);
			}
			else return null;
		}

		internal void InitializePiercings(Dictionary<CockPiercings, PiercingJewelry> piercings)
		{
#warning Implement Me!
			//throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}


		#endregion

		public override CockData AsReadOnlyData()
		{
			return new CockData(this);
		}

		#region Update

		internal override bool UpdateType(CockType newType)
		{
			return UpdateCock(newType, () => SetLength(cockLength));
		}

		internal bool UpdateCockTypeWithLength(CockType cockType, float newLength)
		{
			return UpdateCock(cockType, () => SetLength(newLength));
		}

		internal bool UpdateCockTypeWithLengthAndGirth(CockType cockType, float newLength, float newGirth)
		{
			return UpdateCock(cockType, () => SetLengthAndGirth(newLength, newGirth));
		}

		internal bool UpdateCockTypeWithKnotMultiplier(CockType cockType, float newKnotMultiplier)
		{
			return UpdateCock(cockType, () => knotMultiplier = newKnotMultiplier);
		}

		internal bool UpdateCockTypeWithAll(CockType cockType, float newLength, float newGirth, float newKnotMultiplier)
		{
			return UpdateCock(cockType, () =>
			{
				SetLengthAndGirth(newLength, newGirth);
				knotMultiplier = newKnotMultiplier;
			});
		}

		private bool UpdateCock(CockType newType, Action callback)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			var oldType = type;
			type = newType;
			callback?.Invoke();
			NotifyTypeChanged(oldType);
			return true;
		}
		#endregion

		#region Restore
		//standard restore is fine. 

		internal void Reset(bool resetPiercings = true)
		{
			type = CockType.HUMAN;
			float length = resetLength;
			float girth = DEFAULT_COCK_GIRTH;
			updateLengthAndGirth(length, girth);

			if (resetPiercings)
			{
				cockPiercings.Reset();
			}
		}

		#endregion

		#region Attribute Updates

		internal float LengthenCock(float lengthenAmount, bool ignorePerk = false)
		{
			if (lengthenAmount <= 0) return 0;

			float oldLength = cockLength;
			if (!ignorePerk)
			{
				lengthenAmount *= cockGrowthMultiplier;
			}
			updateLength(cockLength + lengthenAmount);
			return cockLength - oldLength;
		}

		internal float ShortenCock(float shortenAmount, bool ignorePerk = false)
		{
			if (shortenAmount <= 0) return 0;
			float oldLength = cockLength;
			if (!ignorePerk)
			{
				shortenAmount *= cockShrinkMultiplier;
			}
			updateLength(cockLength - shortenAmount);
			return oldLength - cockLength;
		}

		internal float SetLength(float newLength)
		{
			updateLength(newLength);
			return cockLength;
		}

		internal float ThickenCock(float thickenAmount)
		{
			float oldGirth = cockGirth;
			updateGirth(cockGirth + thickenAmount);
			return cockGirth - oldGirth;
		}

		internal float ThinCock(float thinAmount)
		{
			float oldGirth = cockGirth;
			updateGirth(cockGirth - thinAmount);
			return oldGirth - cockGirth;
		}

		internal float SetGirth(float newGirth)
		{
			updateGirth(newGirth);
			return cockGirth;
		}

		internal void SetLengthAndGirth(float newLength, float newGirth)
		{
			updateLengthAndGirth(newLength, newGirth);
		}

		internal float IncreaseKnotMultiplier(float amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			float oldMultiplier = knotMultiplier;
			knotMultiplier += amount;
			return knotMultiplier - amount;
		}

		internal float DecreaseKnotMultiplier(float amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			float oldMultiplier = knotMultiplier;
			knotMultiplier -= amount;
			return amount - knotMultiplier;
		}

		internal bool SetKnotMultiplier(float multiplier)
		{
			if (multiplier < MIN_KNOT_MULTIPLIER)
			{
				return multiplier == 0 && !type.hasKnot;
			}
			else if (type.hasKnot)
			{
				if (multiplier > MAX_KNOT_MULTIPLIER)
				{
					multiplier = MAX_KNOT_MULTIPLIER;
				}
				knotMultiplier = multiplier;
				return true;
			}
			return false;

		}

		#endregion
		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			CockType cockType = type;
			valid = CockType.Validate(ref cockType, correctInvalidData);
			//should auto-Validate length and girth.
			type = cockType;
			if (valid || correctInvalidData)
			{
				valid &= cockPiercings.Validate(correctInvalidData);
			}
			//give length priority.
			if (valid || correctInvalidData)
			{
				updateLengthAndGirth(cockLength, cockGirth);
			}
			return valid;
		}
		#endregion
		#region Piercings
		private bool PiercingLocationUnlocked(CockPiercings piercingLocation)
		{
			return true;
		}

		private JewelryType SupportedJewelryByLocation(CockPiercings piercingLocation)
		{
			if (piercingLocation == CockPiercings.ALBERT)
			{
				return SUPPORTED_JEWELRY_ALBERT;
			}
			else
			{
				return SUPPORTED_JEWELRY_FRENUM;
			}
		}

		#endregion
		#region IGrowShrinkable

		bool IShrinkable.CanReducto()
		{
			return cockArea > 6;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = cockLength;
			float multiplier = 2.0f / 3 * cockShrinkMultiplier;
			updateLength(cockLength * multiplier);
			return oldLength - cockLength;

		}

		bool IGrowable.CanGroPlus()
		{
			return cockLength < MAX_COCK_LENGTH;
		}

		//grows cock 1-2 inches, in increments of 0.25. 
		//automatically increases cockGirth to min value.
		//if possible, will also increase cockGirth, up to 0.5 inches
		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			float oldCockLength = cockLength;

			if (cockGrowthMultiplier != 1)
			{
				float multiplier = cockGrowthMultiplier;
				int rand;
				if (multiplier < 1)
				{
					rand = (int)Math.Floor(multiplier * 4);
				}
				else
				{
					rand = (int)Math.Ceiling(multiplier * 4) + 1;
				}
				updateLength(cockLength + 1 + Utils.Rand(rand) / 4.0f);
			}
			else
			{
				updateLength(cockLength + 1 + Utils.Rand(4) / 4.0f);
			}
			if ((cockGirth + 0.5f) < maxGirth)
			{
				updateGirth(cockGirth + 0.5f);
			}
			else if (cockGirth < maxGirth)
			{
				updateGirth(maxGirth);
			}
			return cockLength - oldCockLength;
		}
		#endregion
		#region Helpers
		private void updateLength(float newLength)
		{
			_cockLength = newLength;
			Utils.Clamp(ref _cockLength, minCockLength, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}

		private void updateGirth(float newGirth)
		{
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockGirth, MIN_COCK_THICKNESS, MAX_COCK_THICKNESS);
			Utils.Clamp(ref _cockLength, minCockLength, maxLength);
		}

		private void updateLengthAndGirth(float newLength, float newGirth)
		{
			_cockLength = newLength;
			_cockGirth = newGirth;
			Utils.Clamp(ref _cockLength, minCockLength, MAX_COCK_LENGTH);
			Utils.Clamp(ref _cockGirth, minGirth, maxGirth);
		}


		private float maxGirth => _cockLength * type.maxGirthToLengthRatio;
		private float minGirth => _cockLength * type.minGirthToLengthRatio;
		private float minLength => _cockGirth / type.maxGirthToLengthRatio;
		private float maxLength => _cockGirth / type.minGirthToLengthRatio;
		#endregion
	}



	public partial class CockType : SaveableBehavior<CockType, Cock, CockData>
	{

		private static int indexMaker = 0;
		private static readonly List<CockType> types = new List<CockType>();
		public static readonly ReadOnlyCollection<CockType> availableTypes = new ReadOnlyCollection<CockType>(types);
		public override int index => _index;
		private readonly int _index;

		public const float MAX_INITIAL_MULTIPLIER = 2.5f;

		public static CockType defaultValue => HUMAN;

		public bool hasKnot => baseKnotMultiplier != 0f;
		public readonly float baseKnotMultiplier;

		//vanilla game limited cock width to 2/5 the length. i realize that some cocks may require that ratio be lower, so if it irks you, you
		//now have the option to change that.

		public virtual float maxGirthToLengthRatio => 0.4f;

		//similarly, you now have the option to mess with min ratio. by default, any cock will at the very least, have one inch in width per 10 inches in length.
		public virtual float minGirthToLengthRatio => 0.1f;

		//allows a straightforward way to check for flexible, extendable cocks, like tentacles. also allows other cocks to act like tentacles (see the newly added goo-cock)
		//i'm aware this breaks all the rules with length and girth, but tentacles need to tentacle things, so. 
		//I'm hanging the lampshade in the tentacle and goo descriptions, saying that they can extend to crazy lengths, but they only feel pleasure at the tip (which happens to be length long and girth wide)
		public virtual bool flexibleOrStretchyCock => false;

		//you can now alter the formula for knots. Iirc, the text for dragon cock knots said it didn't swell with size like the others, but still used the formula. if it's important enough to you to change this,
		//you can. 
		public virtual float knotSize(float girth, float multiplier) => girth * multiplier;

		private protected CockType(CockGroup cockGroup,
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			baseKnotMultiplier = 0;
			types.AddAt(this, _index);

		}

		private protected CockType(CockGroup cockGroup, float initialKnotMultiplier, //any cocktype specific values.
			SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
			ChangeType<Cock> transform, RestoreType<Cock> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			Utils.Clamp(ref initialKnotMultiplier, Cock.MIN_KNOT_MULTIPLIER, MAX_INITIAL_MULTIPLIER);
			baseKnotMultiplier = initialKnotMultiplier;
			types.AddAt(this, _index);
		}

		internal static bool Validate(ref CockType cockType, bool correctInvalidData)
		{
			if (types.Contains(cockType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				cockType = HUMAN;
			}
			return false;
		}

		public static readonly CockType HUMAN = new CockType(CockGroup.HUMAN, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly CockType HORSE = new CockType(CockGroup.MAMMALIAN, HorseDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly CockType DOG = new CockType(CockGroup.MAMMALIAN, 1.1f, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);// can range up to 2.1 depending on item.
		public static readonly CockType DEMON = new CockType(CockGroup.CORRUPTED, DemonDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr);
		public static readonly CockType TENTACLE = new FlexiCock(CockGroup.CORRUPTED, 1.1f, TentacleDesc, TentacleFullDesc, TentaclePlayerStr, TentacleTransformStr, TentacleRestoreStr);
		public static readonly CockType CAT = new CockType(CockGroup.MAMMALIAN, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly CockType LIZARD = new CockType(CockGroup.REPTILIAN, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly CockType ANEMONE = new CockType(CockGroup.AQUATIC, AnemoneDesc, AnemoneFullDesc, AnemonePlayerStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly CockType KANGAROO = new CockType(CockGroup.MAMMALIAN, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly CockType DRAGON = new CockType(CockGroup.REPTILIAN, 1.3f, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly CockType DISPLACER = new CockType(CockGroup.OTHER, 1.5f, DisplacerDesc, DisplacerFullDesc, DisplacerPlayerStr, DisplacerTransformStr, DisplacerRestoreStr);
		public static readonly CockType FOX = new CockType(CockGroup.MAMMALIAN, 1.25f, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly CockType BEE = new CockType(CockGroup.FLYING, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly CockType PIG = new CockType(CockGroup.MAMMALIAN, PigDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly CockType AVIAN = new CockType(CockGroup.FLYING, AvianDesc, AvianFullDesc, AvianPlayerStr, AvianTransformStr, AvianRestoreStr);
		public static readonly CockType RHINO = new CockType(CockGroup.MAMMALIAN, RhinoDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly CockType ECHIDNA = new CockType(CockGroup.MAMMALIAN, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly CockType WOLF = new CockType(CockGroup.MAMMALIAN, 1.5f, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly CockType RED_PANDA = new CockType(CockGroup.MAMMALIAN, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly CockType FERRET = new CockType(CockGroup.MAMMALIAN, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly CockType GOO = new FlexiCock(CockGroup.AQUATIC, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);

		private class FlexiCock : CockType
		{
			public override bool flexibleOrStretchyCock => true;
			public FlexiCock(CockGroup cockGroup, SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
				ChangeType<Cock> transform, RestoreType<Cock> restore) : base(cockGroup, shortDesc, fullDesc, playerDesc, transform, restore) { }

			public FlexiCock(CockGroup cockGroup, float initialKnotMultiplier, SimpleDescriptor shortDesc, DescriptorWithArg<Cock> fullDesc, TypeAndPlayerDelegate<Cock> playerDesc,
				ChangeType<Cock> transform, RestoreType<Cock> restore) : base(cockGroup, initialKnotMultiplier, shortDesc, fullDesc, playerDesc, transform, restore) { }
		}
	}

	public sealed class CockData : BehavioralSaveablePartData<CockData, Cock, CockType>
	{
		public readonly float knotMultiplier;
		public readonly float knotSize;
		public readonly float length;
		public readonly float girth;
		public float cockArea => length * girth;

		public CockData(Cock source) : base(GetBehavior(source))
		{
			knotMultiplier = source.knotMultiplier;
			length = source.cockLength;
			girth = source.cockGirth;
			knotSize = source.knotSize;
		}
	}
}
