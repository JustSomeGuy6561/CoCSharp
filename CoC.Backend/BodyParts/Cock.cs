//Cock.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:55 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.UI;
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
public sealed partial class Cock : BehavioralSaveablePart<Cock, CockType, CockData>, IGrowable, IShrinkable
	{

		public override string BodyPartName() => Name();
		
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

		public const float MIN_URETHRA_WIDTH = (float)(0.5 * Measurement.TO_INCHES);
		public const float MIN_CUM = 2;

		public const JewelryType SUPPORTED_JEWELRY_FRENUM = JewelryType.BARBELL_STUD | JewelryType.RING;
		public const JewelryType SUPPORTED_JEWELRY_ALBERT = JewelryType.BARBELL_STUD | JewelryType.HORSESHOE | JewelryType.RING | JewelryType.SPECIAL;

		#endregion

		#region Properties
		public readonly bool isClitCock;

		private int cockIndex => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? isClitCock ? creature.genitals.cocks.IndexOf(this) : creature.genitals.allCocks.IndexOf(this) : 0;

		private float cockGrowthMultiplier => creature?.genitals.CockGrowthMultiplier ?? 1;
		private float cockShrinkMultiplier => creature?.genitals.CockShrinkMultiplier ?? 1;
		internal float minCockLength
		{
			get => _minCockLength;
			set
			{
				_minCockLength = value;
				if (length < _minCockLength)
				{
					var oldData = AsReadOnlyData();
					_cockLength = _minCockLength;
					NotifyDataChanged(oldData);
				}
			}
		}
		private float _minCockLength = MIN_COCK_LENGTH;

		internal float newCockDefaultSize; //needed for reset.

		private Creature creature
		{
			get
			{
				CreatureStore.TryGetCreature(creatureID, out Creature creatureSource);
				return creatureSource;
			}
		}

		internal float perkBonusVirilityMultiplier => creature?.genitals.perkBonusVirilityMultiplier ?? 1;
		internal sbyte perkBonusVirility => creature?.genitals.perkBonusVirility ?? 0;

		private float resetLength => Math.Max(newCockDefaultSize, minCockLength);

		public readonly Piercing<CockPiercings> cockPiercings;

		public float urethraWidth => Math.Max(type.UrethraWidth(girth), MIN_URETHRA_WIDTH);

		public float minCumAmount => Math.Max(MIN_CUM, (float)(Math.Pow((urethraWidth * Measurement.TO_CENTIMETERS), 2) * Math.PI * length));

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

		public float knotSize => type.knotSize(girth, knotMultiplier);

		public float length => _cockLength;
		private float _cockLength;

		public float girth => _cockGirth;
		private float _cockGirth;
		public float area => girth * length;

		public uint soundCount { get; private set; } = 0;
		public uint sexCount { get; private set; } = 0;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public float cumAmount => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.genitals.totalCum : minCumAmount;

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
		internal Cock(Guid creatureID, CockPerkHelper initialPerkValues) : this(creatureID, initialPerkValues, CockType.defaultValue)
		{ }

		internal Cock(Guid creatureID, CockPerkHelper initialPerkValues, CockType cockType) : base(creatureID)
		{
			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			updateLength(initialPerkValues.NewLength());

			knotMultiplier = type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			newCockDefaultSize = initialPerkValues.NewCockDefaultSize;
			minCockLength = initialPerkValues.MinCockLength;

			isClitCock = false;
		}

		internal Cock(Guid creatureID, CockPerkHelper initialPerkValues, CockType cockType, float length, float girth,
			float? initialKnotMultiplier = null) : this(creatureID, initialPerkValues, cockType, length, girth, initialKnotMultiplier, false)
		{ }

		private Cock(Guid creatureID, CockPerkHelper initialPerkValues, CockType cockType, float length, float girth,
			float? initialKnotMultiplier, bool clitCock) : base(creatureID)
		{
			type = cockType ?? throw new ArgumentNullException(nameof(cockType));
			length = initialPerkValues.NewLength(length);

			updateLengthAndGirth(length, girth);

			knotMultiplier = initialKnotMultiplier ?? type.baseKnotMultiplier;

			cockPiercings = new Piercing<CockPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			newCockDefaultSize = initialPerkValues.NewCockDefaultSize;
			minCockLength = initialPerkValues.MinCockLength;

			isClitCock = clitCock;

		}
		#endregion

		#region Generate
		internal static Cock GenerateFromGender(Guid creatureID, CockPerkHelper initialPerkValues, Gender gender)
		{
			if (gender.HasFlag(Gender.MALE))
			{
				return new Cock(creatureID, initialPerkValues);
			}
			else return null;
		}

		internal static Cock GenerateClitCock(Guid creatureID, Clit clit)
		{
			//clit cock doesn't care about perks, also this way i can write it easily lol.
			return new Cock(creatureID, new CockPerkHelper(), CockType.defaultValue, clit.length + 5, DEFAULT_COCK_GIRTH, null, true);
		}

		internal void InitializePiercings(Dictionary<CockPiercings, PiercingJewelry> piercings)
		{
#warning Implement Me!
			//throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}


		#endregion

		public override CockData AsReadOnlyData()
		{
			return new CockData(this, cockIndex);
		}

		private void CheckDataChanged(CockData oldData)
		{
			if (length != oldData.length || this.girth != oldData.girth || knotMultiplier != oldData.knotMultiplier || knotSize != oldData.knotSize)
			{
				NotifyDataChanged(oldData);
			}
		}

		#region Update

		internal override bool UpdateType(CockType newType)
		{
			return UpdateCock(newType, () => SetLength(length));
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
			var oldData = AsReadOnlyData();
			var oldType = type;
			type = newType;
			callback?.Invoke();

			CheckDataChanged(oldData);
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

		public float LengthenCock(float lengthenAmount, bool ignorePerk = false)
		{
			if (lengthenAmount <= 0) return 0;

			float oldLength = length;
			if (!ignorePerk)
			{
				lengthenAmount *= cockGrowthMultiplier;
			}
			var oldData = AsReadOnlyData();
			updateLength(length + lengthenAmount);
			CheckDataChanged(oldData);
			return length - oldLength;
		}

		public float ShortenCock(float shortenAmount, bool ignorePerk = false)
		{
			if (shortenAmount <= 0) return 0;
			float oldLength = length;
			if (!ignorePerk)
			{
				shortenAmount *= cockShrinkMultiplier;
			}
			var oldData = AsReadOnlyData();
			updateLength(length - shortenAmount);
			CheckDataChanged(oldData);
			return oldLength - length;
		}

		public float SetLength(float newLength)
		{
			var oldData = AsReadOnlyData();
			updateLength(newLength);
			CheckDataChanged(oldData);
			return length;
		}

		public float ThickenCock(float thickenAmount)
		{
			float oldGirth = girth;
			var oldData = AsReadOnlyData();
			updateGirth(girth + thickenAmount);
			CheckDataChanged(oldData);
			return girth - oldGirth;
		}

		public float ThinCock(float thinAmount)
		{
			float oldGirth = girth;
			var oldData = AsReadOnlyData();
			updateGirth(girth - thinAmount);
			CheckDataChanged(oldData);
			return oldGirth - girth;
		}

		public float SetGirth(float newGirth)
		{
			var oldData = AsReadOnlyData();
			updateGirth(newGirth);
			CheckDataChanged(oldData);
			return girth;
		}

		public void SetLengthAndGirth(float newLength, float newGirth)
		{
			var oldData = AsReadOnlyData();
			updateLengthAndGirth(newLength, newGirth);
			CheckDataChanged(oldData);
		}

		public float IncreaseKnotMultiplier(float amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			float oldMultiplier = knotMultiplier;
			var oldData = AsReadOnlyData();
			knotMultiplier += amount;
			CheckDataChanged(oldData);
			return knotMultiplier - oldMultiplier;
		}

		public float DecreaseKnotMultiplier(float amount)
		{
			if (!type.hasKnot)
			{
				return 0;
			}
			float oldMultiplier = knotMultiplier;
			var oldData = AsReadOnlyData();
			knotMultiplier -= amount;
			CheckDataChanged(oldData);
			return oldMultiplier - knotMultiplier;
		}

		public float SetKnotMultiplier(float multiplier)
		{
			Utils.Clamp(ref multiplier, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
			if (type.hasKnot)
			{
				var oldData = AsReadOnlyData();
				knotMultiplier = multiplier;
				CheckDataChanged(oldData);
			}
			return knotMultiplier;
		}

		public void SetAll(float newLength, float newGirth, float newKnotMultiplier)
		{
			var oldData = AsReadOnlyData();
			updateLengthAndGirth(newLength, newGirth);
			Utils.Clamp(ref newKnotMultiplier, MIN_KNOT_MULTIPLIER, MAX_KNOT_MULTIPLIER);
			if (type.hasKnot)
			{
				knotMultiplier = newKnotMultiplier;
			}
			CheckDataChanged(oldData);
		}
		#endregion
		#region Sex-Related
		internal void SoundCock(float penetratorLength, float penetratorWidth, float penetratorKnotSize, bool reachOrgasm)
		{
			soundCount++;
			//i guess we could do stuff with the cock being sore af or whatever, but whatever. 
		}

		internal void DoSex(bool reachOrgasm)
		{
			sexCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void OrgasmGeneric(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
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
				updateLengthAndGirth(length, girth);
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
			return area > 6;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			float multiplier = 2.0f / 3 * cockShrinkMultiplier;
			updateLength(length * multiplier);
			return oldLength - length;

		}

		bool IGrowable.CanGroPlus()
		{
			return length < MAX_COCK_LENGTH;
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
			float oldCockLength = length;

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
				updateLength(length + 1 + Utils.Rand(rand) / 4.0f);
			}
			else
			{
				updateLength(length + 1 + Utils.Rand(4) / 4.0f);
			}
			if ((girth + 0.5f) < maxGirth)
			{
				updateGirth(girth + 0.5f);
			}
			else if (girth < maxGirth)
			{
				updateGirth(maxGirth);
			}
			return length - oldCockLength;
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

		public byte virility
		{
			get
			{
				double value;
				if (cumAmount < 250)
				{
					value = 1;
				}
				else if (cumAmount < 800)
				{
					value = 2;
				}
				else if (cumAmount < 1600)
				{
					value = 3;
				}
				else
				{
					value = 5;
				}

				value *= perkBonusVirilityMultiplier + perkBonusVirility;
				return (byte)Utils.Clamp2(Math.Round(value), 0, 100);
			}
		}
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

		internal virtual float UrethraWidth(float girth)
		{
			return girth / 8f;
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
		public readonly int cockIndex;

		public float cockArea => length * girth;

		public CockData(Cock source, int currIndex) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)),  GetBehavior(source))
		{
			knotMultiplier = source.knotMultiplier;
			length = source.length;
			girth = source.girth;
			knotSize = source.knotSize;
			cockIndex = currIndex;
		}
	}
}
