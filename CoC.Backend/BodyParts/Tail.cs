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

		public byte tailCount
		{
			get => _tailCount;
			private set => _tailCount = Utils.Clamp2(value, type.initialTailCount, type.maxTailCount);
		}
		private byte _tailCount = 0;

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
		{
		}

		internal Tail(Guid creatureID, TailType tailType) : base(creatureID)
		{
			_type = tailType ?? throw new ArgumentNullException(nameof(tailType));
			_tailCount = _type.initialTailCount;
			tailPiercings = new Piercing<TailPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);
		}

		internal Tail(Guid creatureID, TailType tailType, byte count) : this(creatureID, tailType)
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

		public virtual bool supportsTailPiercing => false;

		public virtual byte initialTailCount => 1;
		public virtual byte maxTailCount => initialTailCount;

		private protected TailType(EpidermisType epidermis, bool toneFurMutable, //AttackBase attackData,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform,
			RestoreType<TailData> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			epidermisType = epidermis;
			mutable = toneFurMutable;
			_index = indexMaker++;
			tails.AddAt(this, _index);
			//attack = attackData ?? AttackBase.NO_ATTACK;
		}

		public override int index => _index;

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
		public static readonly TailType HORSE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultHorseTailFur, true, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly TailType DOG = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultDogFur, true, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly TailType DEMONIC = new SuccubusTail();

		public static readonly TailType COW = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCowFur, true, CowShortDesc, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly TailType SPIDER_SPINNERET = new ToneTailWithResourceAttack(SPIDER_ATTACK, EpidermisType.CARAPACE, DefaultValueHelpers.defaultSpiderTone, false, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //webbing, resource-based
		public static readonly TailType BEE_STINGER = new ToneTailWithResourceAttack(BEE_STING, EpidermisType.CARAPACE, DefaultValueHelpers.defaultBeeTone, false, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting, resource-based

		public static readonly TailType SHARK = new ToneTailWithSlam(4, EpidermisType.SKIN, DefaultValueHelpers.defaultSharkTone, true, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly TailType CAT = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCatFur, true, CatShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly TailType LIZARD = new ToneTailWithWhip(EpidermisType.SCALES, DefaultValueHelpers.defaultLizardTone, true, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly TailType RABBIT = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultBunnyFur, true, RabbitShortDesc, RabbitFullDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly TailType HARPY = new FurryTail(EpidermisType.FEATHERS, DefaultValueHelpers.defaultHarpyFeathers, true, HarpyShortDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly TailType KANGAROO = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultKangarooTailFur, true, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly TailType FOX = new FoxTail();
		public static readonly TailType DRACONIC = new ToneTailWithSlam(5, EpidermisType.SCALES, DefaultValueHelpers.defaultDragonTone, true, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly TailType RACCOON = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultRaccoonFur, true, RaccoonShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly TailType MOUSE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultMouseFur, true, MouseShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly TailType FERRET = new FurryTailWithWhip(EpidermisType.FUR, DefaultValueHelpers.defaultFerretFur, true, FerretShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly TailType BEHEMOTH = new FurryTailWithSlam(5, EpidermisType.FUR, DefaultValueHelpers.defaultBehemothFur, true, BehemothShortDesc, BehemothFullDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly TailType PIG = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultPigFur, true, PigShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly TailType SCORPION = new ToneTailWithResourceAttack(SCORPION_STING, EpidermisType.CARAPACE, DefaultValueHelpers.defaultScorpionTailTone, false, ScorpionShortDesc, ScorpionFullDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly TailType SATYR = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultSatyrTailColor, true, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly TailType RHINO = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultRhinoTailFur, true, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly TailType ECHIDNA = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultEchidnaTailFur, true, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TailType DEER = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultDeerTail, true, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly TailType SALAMANDER = new SalamanderTail();
		public static readonly TailType WOLF = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultWolfTailFur, true, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly TailType SHEEP = new FurryTail(EpidermisType.WOOL, DefaultValueHelpers.defaultSheepWoolFur, true, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly TailType IMP = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultImpTailFur, true, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly TailType COCKATRICE = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultCockatriceTailFeaithers, true, CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly TailType RED_PANDA = new FurryTail(EpidermisType.FUR, DefaultValueHelpers.defaultRedPandaFaceEarTailFur, true, RedPandaShortDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);

		private sealed class NoTail : TailType
		{
			public NoTail() : base(EpidermisType.EMPTY, false, NoTailShortDesc, NoTailFullDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr) { }

			public override byte initialTailCount => 0;

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				return new EpidermalData();
			}
		}

		public class FurryTail : TailType
		{
			public readonly FurColor defaultFur;
			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;
			public FurryTail(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable,
				SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform,
				RestoreType<TailData> restore) : base(furType, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
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

			public ToneTail(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable,
		SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform,
		RestoreType<TailData> restore) : base(toneType, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
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

			public ToneTailWithResourceAttack(GenerateResourceAttack attackGetter, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable,
				SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform,
				RestoreType<TailData> restore) : base(toneType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
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
			public FurryTailWithWhip(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc,
				PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform, RestoreType<TailData> restore)
				: base(furType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore) { }

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return TAIL_WHIP;
			}
		}

		private class ToneTailWithWhip : ToneTail
		{
			public ToneTailWithWhip(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc,
				PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform, RestoreType<TailData> restore)
				: base(toneType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore) { }

			internal override AttackBase GetAttackOnTransform(Func<ushort> get, Action<ushort> set)
			{
				return TAIL_WHIP;
			}
		}

		private class SalamanderTail : ToneTail
		{
			public SalamanderTail() : base(EpidermisType.SCALES, DefaultValueHelpers.defaultSalamanderTone, false, SalamanderShortDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr)
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
			public ToneTailWithSlam(byte attackStrength, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, SimpleDescriptor shortDesc,
				DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform, RestoreType<TailData> restore)
				: base(toneType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
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
			public FurryTailWithSlam(byte attackStrength, FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, SimpleDescriptor shortDesc,
				DescriptorWithArg<Tail> fullDesc, PlayerBodyPartDelegate<Tail> playerDesc, ChangeType<TailData> transform, RestoreType<TailData> restore)
				: base(furType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				_attack = new TailSlam(shortDesc, attackStrength);
			}
		}

		private static readonly TailWhip TAIL_WHIP = new TailWhip();



		private class FoxTail : FurryTail
		{
			public FoxTail() : base(EpidermisType.FUR, DefaultValueHelpers.defaultFoxTailFur, true, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr) { }
			public override byte maxTailCount => 9;
		}


		private class SuccubusTail : ToneTail
		{
			public SuccubusTail() : base(EpidermisType.SKIN, DefaultValueHelpers.defaultDemonTone, true, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

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

		internal TailData(Tail source) : base(GetID(source), GetBehavior(source))
		{
			tailCount = source.tailCount;

			primaryEpidermis = source.epidermis;
			secondaryEpidermis = source.secondaryEpidermis;
		}
	}
}
