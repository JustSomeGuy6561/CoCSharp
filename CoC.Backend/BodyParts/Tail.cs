//Tail.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public enum TailPiercings { SUCCUBUS_SPADE }
	public sealed partial class Tail : BehavioralSaveablePart<Tail, TailType, TailData>, ICanAttackWith, IBodyPartTimeLazy
	{
		public override string BodyPartName() => Name();

		public const JewelryType SUPPORTED_TAIL_PIERCINGS = JewelryType.RING;
		public const int MAX_ATTACK_CHARGES = 100;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public EpidermalData epidermis => type.ParseEpidermis(bodyData);
		public EpidermalData secondaryEpidermis => type.ParseSecondaryEpidermis(bodyData);

		public readonly Ovipositor ovipositor;

		public bool canHaveOvipositor => type.allowsOvipositor;

		public bool hasOvipositor => canHaveOvipositor && ovipositorEnabled;

		private bool ovipositorEnabled;

		public byte tailCount
		{
			get => _tailCount;
			private set => _tailCount = Utils.Clamp2(value, type.initialTailCount, type.maxTailCount);
		}
		private byte _tailCount = 0;

		public bool isLongTail => type.isLongTail;

		public readonly Piercing<TailPiercings> tailPiercings;
		public bool isPierced => tailPiercings.isPierced;
		public override TailType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					_attack = value.GetAttackOnTransform(() => resources, (x) => resources = x);
					if (_attack is ResourceAttackBase resourceAttack)
					{
						resources = resourceAttack.initialResource;
						regenRate = resourceAttack.initialRechargeRate;
					}
					else
					{
						resources = 0;
						regenRate = 0;
					}

					if (!value.allowsOvipositor)
					{
						ovipositorEnabled = false;
					}
					else if (!_type.allowsOvipositor)
					{
						ovipositorEnabled = false;
					}
					//otherwise, keep the same value.

					_tailCount = value.initialTailCount;
					if (!value.supportsTailPiercing && tailPiercings.isPierced)
					{
						tailPiercings.Reset();
					}
				}
				_type = value;
			}
		}
		private TailType _type;
		public override TailType defaultType => TailType.defaultValue;

		public override TailData AsReadOnlyData()
		{
			return new TailData(this);
		}

		internal Tail(Guid creatureID) : this(creatureID, TailType.defaultValue)
		{ }

		internal Tail(Guid creatureID, TailType tailType) : this(creatureID, tailType, false)
		{ }

		internal Tail(Guid creatureID, TailType tailType, byte count) : this(creatureID, tailType)
		{
			GrowMultipleAdditionalTails(count.subtract(type.initialTailCount));
		}

		internal Tail(Guid creatureID, TailType tailType, bool hasOvipositorIfApplicable) : base(creatureID)
		{
			_type = tailType ?? throw new ArgumentNullException(nameof(tailType));
			_tailCount = _type.initialTailCount;
			tailPiercings = new Piercing<TailPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);

			ovipositorEnabled = tailType.allowsOvipositor && hasOvipositorIfApplicable;
		}

		internal Tail(Guid creatureID, TailType tailType, byte count, bool hasOvipositorIfApplicable) : this(creatureID, tailType, hasOvipositorIfApplicable)
		{
			GrowMultipleAdditionalTails(count.subtract(type.initialTailCount));
		}

		//standard update, restore are fine. may want an additional update with tail count, idk.

		internal override bool UpdateType(TailType newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			var oldData = AsReadOnlyData();
			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldValue);
			return true;
		}

		public void UpdateResources(short resourceDelta = 0, short regenRateDelta = 0)
		{
			if (!(_attack is ResourceAttackBase) || (resourceDelta == 0 && regenRateDelta == 0))
			{
				return;
			}

			if (regenRateDelta != 0)
			{
				regenRate = (ushort)Utils.Clamp2(regenRateDelta + regenRate, minRegen, maxRegen);
			}
			if (resourceDelta != 0)
			{
				resources = (ushort)Utils.Clamp2(resourceDelta + resources, 0, maxCharges);
			}
		}

		internal bool GrowAdditionalTail()
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			tailCount++;
			NotifyDataChanged(oldData);
			return true;
		}

		internal byte GrowMultipleAdditionalTails(byte amount = 1)
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return 0;
			}
			byte oldCount = tailCount;
			var oldData = AsReadOnlyData();
			tailCount = tailCount.add(amount);
			var result = tailCount.subtract(oldCount);
			if (result != 0)
			{
				NotifyDataChanged(oldData);
			}
			return result;
		}

		private void CheckDataChanged(TailData oldData)
		{
			if (oldData.primaryEpidermis != epidermis || oldData.secondaryEpidermis != secondaryEpidermis || oldData.tailCount != tailCount)
			{
				NotifyDataChanged(oldData);
			}
		}

		internal override bool Validate(bool correctInvalidData)
		{
			TailType tailType = type;
			bool valid = TailType.Validate(ref tailType, ref _tailCount, correctInvalidData);
			if (valid || correctInvalidData)
			{
				valid &= tailPiercings.Validate(correctInvalidData);
			}
			return valid;
		}

		//default short description will take into account current number of tails.
		//if you want the short description without any tail count, use the singletailshortdescription.
		public override string ShortDescription()
		{
			return type.ShortDescription(tailCount != 1);
		}

		#region Text
		public string ShortDescription(bool pluralIfApplicable) => type.ShortDescription(pluralIfApplicable);


		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => type.ShortDescription(pluralIfApplicable, out isPlural);

		public string LongDescription(bool alternateFormat, bool pluralIfApplicable) => type.LongDescription(AsReadOnlyData(), alternateFormat, pluralIfApplicable);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => type.LongDescription(AsReadOnlyData(), alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(bool pluralIfApplicable) => type.LongDescriptionPrimary(AsReadOnlyData(), pluralIfApplicable);
		public string LongDescriptionPrimary(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionPrimary(AsReadOnlyData(), pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(bool pluralIfApplicable) => type.LongDescriptionAlternate(AsReadOnlyData(), pluralIfApplicable);
		public string LongDescriptionAlternate(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionAlternate(AsReadOnlyData(), pluralIfApplicable, out isPlural);

		public string SingleTailDescription() => type.ShortDescription(false);
		public string SingleTailLongDescription(bool alternateFormat) => type.LongDescription(AsReadOnlyData(), alternateFormat, false);
		public string SingleTailLongPrimaryDescription() => type.LongDescriptionPrimary(AsReadOnlyData(), false);
		public string SingleTailLongAlternateDescription() => type.LongDescriptionAlternate(AsReadOnlyData(), false);

		//overload that lets you control whether or not the ovipositor change text appears. by default, any text from a change in ovipositor will be displayed.
		public string TransformFromText(TailData previousTypeData, bool describeOvipositorChangeIfApplicable)
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				return type.TransformFrom(previousTypeData, player, describeOvipositorChangeIfApplicable);
			}
			else return "";
		}

		//overload that lets you control whether or not the ovipositor change text appears. by default, any text from a change in ovipositor will be displayed.
		public string RestoredText(TailData previousTypeData, bool describeOvipositorChangeIfApplicable)
		{
			if (CreatureStore.TryGetCreature(creatureID, out Creature creature) && creature is PlayerBase player)
			{
				return previousTypeData.type.RestoredString(previousTypeData, player, describeOvipositorChangeIfApplicable);
			}
			else return "";
		}
		#endregion


		private bool PiercingLocationUnlocked(TailPiercings piercingLocation)
		{
			bool piercingFetish = BackendSessionSave.data.piercingFetishEnabled;
			return type.supportsTailPiercing && piercingFetish;
		}

		private JewelryType SupportedJewelryByLocation(TailPiercings piercingLocation)
		{
			return JewelryType.RING;
		}

		//public bool canAttackWith()
		//{
		//	return type.canAttackWith;
		//}

		//public AttackBase attack => type.attack;

		AttackBase ICanAttackWith.attack => _attack;
		bool ICanAttackWith.canAttackWith() => _attack != AttackBase.NO_ATTACK && _attack != null;

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			if (_attack is ResourceAttackBase resourceAttack && resources < maxCharges) //slight optimization. make sure we aren't at max.
			{
				uint regen = regenRate.mult(hoursPassed);
				ushort newCount = (ushort)Math.Min(regen + resources, maxCharges);
				resources = newCount;
			}
			//no output.
			return "";
		}

		public ushort resources { get; private set; } = 0;
		public ushort regenRate { get; private set; } = 0;


		private AttackBase _attack = AttackBase.NO_ATTACK;
		public ushort maxCharges => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxResource : (ushort)0;
		public ushort maxRegen => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).maxRechargeRate : (ushort)0;
		public ushort minRegen => _attack is ResourceAttackBase ? ((ResourceAttackBase)_attack).minRechargeRate : (ushort)0;
	}

	public abstract partial class TailType : SaveableBehavior<TailType, Tail, TailData>
	{
		private static int indexMaker = 0;
		private static readonly List<TailType> tails = new List<TailType>();
		public static readonly ReadOnlyCollection<TailType> availableTypes = new ReadOnlyCollection<TailType>(tails);
		//public readonly AttackBase attack;
		public readonly bool mutable;

		public readonly EpidermisType epidermisType;

		internal abstract EpidermalData ParseEpidermis(in BodyData bodyData);
		internal virtual EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}


		private readonly int _index;

		public static TailType defaultValue => TailType.NONE;

		internal virtual AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
		{
			return AttackBase.NO_ATTACK;
		}

		public bool hasMultipleTails => maxTailCount > 1;

		public readonly bool isLongTail;
		public readonly OvipositorType ovipositorType;

		public virtual bool supportsTailPiercing => false;

		public readonly byte initialTailCount;
		public readonly byte maxTailCount;

		private readonly ShortMaybePluralDescriptor shortPluralDesc;
		private readonly MaybePluralPartDescriptor<TailData> longTailDesc;

		public string ShortDescription(bool pluralIfApplicable) => shortPluralDesc(pluralIfApplicable, out bool _);
		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => shortPluralDesc(pluralIfApplicable, out isPlural);
		public string ShortDescription(out bool isPlural) => shortPluralDesc(true, out isPlural);
		public string LongDescription(TailData data, bool alternateFormat, out bool isPlural) => longTailDesc(data, alternateFormat, true, out isPlural);
		public string LongDescription(TailData data, bool alternateFormat, bool pluralIfApplicable) => longTailDesc(data, alternateFormat, pluralIfApplicable, out bool _);
		public string LongDescription(TailData data, bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => longTailDesc(data, alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(TailData data, bool pluralIfApplicable) => longTailDesc(data, false, pluralIfApplicable, out bool _);
		public string LongDescriptionPrimary(TailData data, bool pluralIfApplicable, out bool isPlural) => longTailDesc(data, false, pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(TailData data, bool pluralIfApplicable) => longTailDesc(data, true, pluralIfApplicable, out bool _);
		public string LongDescriptionAlternate(TailData data, bool pluralIfApplicable, out bool isPlural) => longTailDesc(data, true, pluralIfApplicable, out isPlural);

		internal delegate string TailTransform(TailData previousTail, PlayerBase player, bool describeOvipositorChangeIfApplicable = true);
		internal delegate string TailRestore(TailData previousTail, PlayerBase player, bool describeOvipositorChangeIfApplicable = true);

		private TailTransform tailTransform;
		private TailRestore tailRestore;

		public string TransformFrom(TailData previousTail, PlayerBase player, bool describeOvipositorChangeIfApplicable)
		{
			return tailTransform(previousTail, player, describeOvipositorChangeIfApplicable);
		}



		//this is called on the behavior we transformed from.
		//requires any old data. it should know how it restores the old data based on what it does internally, but if needed it can just get
		//any new data from hte player passed in.
		public string RestoredString(TailData previousTail, PlayerBase player, bool describeOvipositorChangeIfApplicable)
		{
			return tailRestore(previousTail, player, describeOvipositorChangeIfApplicable);
		}

		//no tail
		private protected TailType(ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc,
			PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
			: base(shortDesc, longDesc, playerDesc, ParseTransform(transform), ParseRestore(restore))
		{
			epidermisType = EpidermisType.EMPTY;
			mutable = false;
			_index = indexMaker++;
			tails.AddAt(this, _index);
			//attack = attackData ?? AttackBase.NO_ATTACK;

			shortPluralDesc = (bool x, out bool y) =>
			{
				y = false;
				return ShortDescription();
			};
			longTailDesc = (TailData x, bool y, bool _, out bool z) =>
			{
				z = false;
				return LongDescription(x, y);
			};

			this.initialTailCount = 1;
			this.maxTailCount = 1;

			isLongTail = false;

			tailTransform = transform;
			tailRestore = restore;
		}

		//simple tail (with optional ovipositor)
		private protected TailType(OvipositorType ovipositor, EpidermisType epidermis, bool toneFurMutable, //AttackBase attackData,
			bool longTail,/*float tailLength,*/ ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc,
			PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
			: base(shortDesc, longDesc, playerDesc, ParseTransform(transform), ParseRestore(restore))
		{
			epidermisType = epidermis;
			mutable = toneFurMutable;
			_index = indexMaker++;
			tails.AddAt(this, _index);
			//attack = attackData ?? AttackBase.NO_ATTACK;

			shortPluralDesc = (bool x, out bool y) =>
			{
				y = false;
				return ShortDescription();
			};

			longTailDesc = (TailData x, bool y, bool _, out bool z) =>
			{
				z = false;
				return LongDescription(x, y);
			};

			this.initialTailCount = 1;
			this.maxTailCount = 1;

			isLongTail = longTail;

			tailTransform = transform;
			tailRestore = restore;
		}

		//simple tail
		private protected TailType(EpidermisType epidermis, bool toneFurMutable, //AttackBase attackData,
			bool longTail,/*float tailLength,*/ ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc,
			PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
			: base(shortDesc, longDesc, playerDesc, ParseTransform(transform), ParseRestore(restore))
		{
			epidermisType = epidermis;
			mutable = toneFurMutable;
			_index = indexMaker++;
			tails.AddAt(this, _index);
			//attack = attackData ?? AttackBase.NO_ATTACK;

			shortPluralDesc = (bool x, out bool y) =>
			{
				y = false;
				return ShortDescription();
			};

			longTailDesc = (TailData x, bool y, bool _, out bool z) =>
			{
				z = false;
				return LongDescription(x, y);
			};

			this.initialTailCount = 1;
			this.maxTailCount = 1;

			isLongTail = longTail;

			ovipositorType = OvipositorType.NONE;

			tailTransform = transform;
			tailRestore = restore;
		}

		//multi-tail
		private protected TailType(EpidermisType epidermis, bool toneFurMutable, //AttackBase attackData,
			bool longTail,/*float tailLength,*/ byte initialTailCount, byte maxTailCount, ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc,
			MaybePluralPartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
			: base(PluralHelper(shortDesc), singleDesc, LongPluralHelper(longDesc), playerDesc, ParseTransform(transform), ParseRestore(restore))
		{
			epidermisType = epidermis;
			mutable = toneFurMutable;
			_index = indexMaker++;
			tails.AddAt(this, _index);
			//attack = attackData ?? AttackBase.NO_ATTACK;

			shortPluralDesc = shortDesc;
			longTailDesc = longDesc;

			this.initialTailCount = initialTailCount;
			this.maxTailCount = maxTailCount;

			isLongTail = longTail;

			ovipositorType = OvipositorType.NONE;

			tailTransform = transform;
			tailRestore = restore;
		}

		private static ChangeType<TailData> ParseTransform(TailTransform transform)
		{
			if (transform is null) throw new ArgumentNullException(nameof(transform));

			return (x, y) => transform(x, y);
		}

		private static RestoreType<TailData> ParseRestore(TailRestore restore)
		{
			if (restore is null) throw new ArgumentNullException(nameof(restore));

			return (x, y) => restore(x, y);
		}

		public override int index => _index;

		public bool allowsOvipositor => ovipositorType != OvipositorType.NONE;

		internal static bool Validate(ref TailType tailType, ref byte tailCount, bool correctInvalidData)
		{
			if (!tails.Contains(tailType))
			{
				if (correctInvalidData)
				{
					tailType = NONE;
					tailCount = 0;
				}
				return false;
			}
			byte numTails = tailCount;
			Utils.Clamp(ref numTails, tailType.initialTailCount, tailType.maxTailCount);
			if (tailCount == numTails)
			{
				return true;
			}
			else if (correctInvalidData)
			{
				tailCount = numTails;
			}
			return false;

		}

		private static ResourceAttackBase SPIDER_ATTACK(Func<ushort> x, Action<ushort> y) => new SpiderWeb(x, y);
		private static ResourceAttackBase BEE_STING(Func<ushort> x, Action<ushort> y) => new BeeSting(x, y);

		private static ResourceAttackBase SCORPION_STING(Func<ushort> x, Action<ushort> y) => new ScorpionSting(x, y);

		public static readonly TailType NONE = new NoTail();
		public static readonly TailType HORSE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultHorseTailFur, true, false, HorseShortDesc, HorseLongDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly TailType DOG = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, true, true, DogShortDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly TailType DEMONIC = new SuccubusTail();
		public static readonly TailType COW = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCowFur, true, true, CowShortDesc, CowLongDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);

		public static readonly TailType SPIDER_SPINNERET = new ToneTailWithResourceAttack(OvipositorType.SPIDER, SPIDER_ATTACK, EpidermisType.CARAPACE, DefaultValueHelpers.defaultSpiderTone, false, false, SpiderShortDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //webbing, resource-based
		public static readonly TailType BEE_STINGER = new ToneTailWithResourceAttack(OvipositorType.BEE, BEE_STING, EpidermisType.CARAPACE, DefaultValueHelpers.defaultBeeTone, false, false, BeeShortDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting, resource-based

		public static readonly TailType SHARK = new ToneTailWithSlam(4, EpidermisType.SKIN, DefaultValueHelpers.defaultSharkTone, true, SharkShortDesc, SharkLongDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly TailType CAT = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, true, true, CatShortDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly TailType LIZARD = new ToneTailWithWhip(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, true, true, LizardShortDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly TailType RABBIT = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultBunnyFur, true, false, RabbitShortDesc, RabbitLongDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly TailType HARPY = new FurryTail(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, true, false, HarpyShortDesc, HarpyLongDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly TailType KANGAROO = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultKangarooTailFur, true, true, KangarooShortDesc, KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly TailType FOX = new FoxTail();
		public static readonly TailType DRACONIC = new ToneTailWithSlam(5, EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, true, DragonShortDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly TailType RACCOON = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultRaccoonFur, true, true, RaccoonShortDesc, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly TailType MOUSE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultMouseFur, true, false, MouseShortDesc, MouseLongDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly TailType FERRET = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, true, true, FerretShortDesc, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly TailType BEHEMOTH = new FurryTailWithSlam(5, EpidermisType.FUR, DefaultValueHelpers.defaultBehemothFur, true, BehemothShortDesc, BehemothLongDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly TailType PIG = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultPigFur, true, false, PigShortDesc, PigLongDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly TailType SCORPION = new ToneTailWithResourceAttack(OvipositorType.NONE, SCORPION_STING, EpidermisType.CARAPACE, DefaultValueHelpers.defaultScorpionTailTone, false, true, ScorpionShortDesc, ScorpionLongDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly TailType SATYR = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultSatyrTailColor, true, false, GoatShortDesc, GoatLongDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly TailType RHINO = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultRhinoTailFur, true, false, RhinoShortDesc, RhinoLongDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly TailType ECHIDNA = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultEchidnaTailFur, true, false, EchidnaShortDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TailType DEER = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultDeerTail, true, false, DeerShortDesc, DeerLongDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly TailType SALAMANDER = new SalamanderTail();
		public static readonly TailType WOLF = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultWolfTailFur, true, true, WolfShortDesc, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly TailType SHEEP = new FurryTail(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, true, false, SheepShortDesc, SheepLongDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly TailType IMP = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultImpTailFur, true, true, ImpShortDesc, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly TailType COCKATRICE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCockatriceTailFeaithers, true, true, CockatriceShortDesc, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly TailType RED_PANDA = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaFaceEarTailFur, true, true, RedPandaShortDesc, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);

		private sealed class NoTail : TailType
		{
			public NoTail() : base(NoTailShortDesc, NoTailLongDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr) { }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				return new EpidermalData();
			}
		}

		public class FurryTail : TailType
		{
			public readonly FurColor defaultFur;
			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;

			internal FurryTail(OvipositorType ovipositor, FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, bool longTail,
				ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform,
				TailRestore restore) : base(ovipositor, furType, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultFur = defaultColor;
			}

			internal FurryTail(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, bool longTail,
				ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform,
				TailRestore restore) : base(furType, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultFur = defaultColor;
			}

			internal FurryTail(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, bool longTail,
				byte initialTailCount, byte maxTailCount, ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<TailData> longDesc,
				PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
				: base(furType, mutable, longTail, initialTailCount, maxTailCount, shortDesc, singleDesc, longDesc, playerDesc, transform, restore)
			{
				defaultFur = defaultColor;
			}
			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = this.defaultFur;
				if (mutable)
				{
					if (bodyData.main.usesFur && !FurColor.IsNullOrEmpty(bodyData.main.fur))
					{
						color = bodyData.main.fur;
					}
					else if (!bodyData.activeHairColor.isEmpty)
					{
						color = new FurColor(bodyData.hairColor);
					}
				}
				return new EpidermalData(primaryEpidermis, color, FurTexture.NONDESCRIPT);
			}
		}

		public class ToneTail : TailType
		{
			public readonly Tones defaultTone;
			protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;

			internal ToneTail(OvipositorType ovipositor, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, bool longTail,
				ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform,
				TailRestore restore) : base(ovipositor, toneType, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultTone = defaultColor;
			}

			internal ToneTail(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, bool longTail,
				ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform,
				TailRestore restore) : base(toneType, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultTone = defaultColor;
			}

			internal ToneTail(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, bool longTail,
				byte initialTailCount, byte maxTailCount, ShortMaybePluralDescriptor shortDesc, SimpleDescriptor singleDesc, MaybePluralPartDescriptor<TailData> longDesc,
				PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
				: base(toneType, mutable, longTail, initialTailCount, maxTailCount, shortDesc, singleDesc, longDesc, playerDesc, transform, restore)
			{
				defaultTone = defaultColor;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = mutable ? bodyData.main.tone : defaultTone;
				return new EpidermalData(primaryEpidermis, color, SkinTexture.NONDESCRIPT);
			}
		}

		private class ToneTailWithResourceAttack : ToneTail
		{
			private readonly GenerateResourceAttack resourceAttackGetter;

			public ToneTailWithResourceAttack(OvipositorType ovipositor, GenerateResourceAttack attackGetter, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable,
				bool longTail, ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform,
				TailRestore restore) : base(ovipositor, toneType, defaultColor, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore)
			{
				resourceAttackGetter = attackGetter ?? throw new ArgumentNullException(nameof(attackGetter));
			}

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return resourceAttackGetter(get, set);
			}
		}




		private class FurryTailWithWhip : FurryTail
		{
			public FurryTailWithWhip(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, bool longTail, ShortDescriptor shortDesc,
				PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
				: base(furType, defaultColor, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore) { }

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return TAIL_WHIP;
			}
		}

		private class ToneTailWithWhip : ToneTail
		{
			public ToneTailWithWhip(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, bool longTail,
				ShortDescriptor shortDesc, PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc,
				TailTransform transform, TailRestore restore)
				: base(toneType, defaultColor, mutable, longTail, shortDesc, longDesc, playerDesc, transform, restore) { }

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return TAIL_WHIP;
			}
		}

		private class SalamanderTail : ToneTail
		{
			public SalamanderTail() : base(EpidermisType.SCALES, DefaultValueHelpers.defaultSalamanderTone, false, false, SalamanderShortDesc, SalamanderLongDesc,
				SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr)
			{ }

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return _attack;
			}
			private static readonly AttackBase _attack = new TailSlap();
		}

		private class ToneTailWithSlam : ToneTail
		{
			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return _attack;
			}

			private readonly TailSlam _attack;
			public ToneTailWithSlam(byte attackStrength, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, ShortDescriptor shortDesc,
				PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
				: base(toneType, defaultColor, mutable, true, shortDesc, longDesc, playerDesc, transform, restore)
			{
				_attack = new TailSlam(shortDesc, attackStrength);
			}
		}

		private class FurryTailWithSlam : FurryTail
		{
			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return _attack;
			}

			private readonly TailSlam _attack;
			public FurryTailWithSlam(byte attackStrength, FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, ShortDescriptor shortDesc,
				PartDescriptor<TailData> longDesc, PlayerBodyPartDelegate<Tail> playerDesc, TailTransform transform, TailRestore restore)
				: base(furType, defaultColor, mutable, true, shortDesc, longDesc, playerDesc, transform, restore)
			{
				_attack = new TailSlam(shortDesc, attackStrength);
			}
		}

		private static readonly TailWhip TAIL_WHIP = new TailWhip();



		private class FoxTail : FurryTail
		{
			public FoxTail() : base(EpidermisType.FUR, DefaultValueHelpers.defaultFoxTailFur, true, true, 1, 9, FoxShortDesc,
				FoxSingleDesc, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr) { }
		}


		private class SuccubusTail : ToneTail
		{
			public SuccubusTail() : base(EpidermisType.SKIN, DefaultValueHelpers.defaultDemonTone, true, true, DemonShortDesc, DemonLongDesc, DemonPlayerStr,
				DemonTransformStr, DemonRestoreStr) { }

			public override bool supportsTailPiercing => true;
		}
	}

	public static class TailHelpers
	{
		public static PiercingJewelry GenerateTailJewelry(this Body body, JewelryMaterial jewelryMaterial)
		{
			return new GenericPiercing(JewelryType.RING, jewelryMaterial);
		}
	}

	public sealed class TailData : BehavioralSaveablePartData<TailData, Tail, TailType>
	{
		public readonly byte tailCount;

		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;

		public readonly OvipositorData ovipositor;

		public bool isLongTail => type.isLongTail;

		public readonly ushort resources;
		public readonly ushort regenRate;
		public readonly ushort maxResources;

		public readonly ReadOnlyPiercing<TailPiercings> tailPiercings;

		//default short description will take into account current number of tails.
		//if you want the short description without any tail count, use the singletailshortdescription.
		public override string ShortDescription()
		{
			return type.ShortDescription(tailCount != 1);
		}

		#region Text
		public string ShortDescription(bool pluralIfApplicable) => type.ShortDescription(pluralIfApplicable);


		public string ShortDescription(bool pluralIfApplicable, out bool isPlural) => type.ShortDescription(pluralIfApplicable, out isPlural);

		public string LongDescription(bool alternateFormat, bool pluralIfApplicable) => type.LongDescription(this, alternateFormat, pluralIfApplicable);
		public string LongDescription(bool alternateFormat, bool pluralIfApplicable, out bool isPlural) => type.LongDescription(this, alternateFormat, pluralIfApplicable, out isPlural);

		public string LongDescriptionPrimary(bool pluralIfApplicable) => type.LongDescriptionPrimary(this, pluralIfApplicable);
		public string LongDescriptionPrimary(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionPrimary(this, pluralIfApplicable, out isPlural);

		public string LongDescriptionAlternate(bool pluralIfApplicable) => type.LongDescriptionAlternate(this, pluralIfApplicable);
		public string LongDescriptionAlternate(bool pluralIfApplicable, out bool isPlural) => type.LongDescriptionAlternate(this, pluralIfApplicable, out isPlural);

		public string SingleTailDescription() => type.ShortDescription(false);
		public string SingleTailLongDescription(bool alternateFormat) => type.LongDescription(this, alternateFormat, false);
		public string SingleTailLongPrimaryDescription() => type.LongDescriptionPrimary(this, false);
		public string SingleTailLongAlternateDescription() => type.LongDescriptionAlternate(this, false);
		#endregion

		public override TailData AsCurrentData()
		{
			return this;
		}

		internal TailData(Tail source) : base(GetID(source), GetBehavior(source))
		{
			tailCount = source.tailCount;

			primaryEpidermis = source.epidermis;
			secondaryEpidermis = source.secondaryEpidermis;

			resources = source.resources;
			regenRate = source.regenRate;
			maxResources = source.maxCharges;

			ovipositor = source.ovipositor.AsReadOnlyData();

			tailPiercings = source.tailPiercings.AsReadOnlyData();
		}
	}
}
