//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM

using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoC.Backend.BodyParts
{
	//data changed event will never fire.
	public sealed partial class LowerBody : BehavioralSaveablePart<LowerBody, LowerBodyType, LowerBodyData>, ICanAttackWith
	{
		public override string BodyPartName() => Name();

		public readonly Feet feet;
		//No magic constants. Woo!
		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//when numbers aren't magic constants. (running grep with a string is much easier
		//than a regular expression looking for legs.count = [A-Za-z0-9]+ or something worse

		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADRUPED_LEG_COUNT = 4;
		public const byte SEXTOPED_LEG_COUNT = 6; //for squids/octopi, if i implement them. technically, an octopus is 2 legs and 6 arms, but i like the 6 legs 2 arms because legs have a count, arms dont.
		public const byte OCTOPED_LEG_COUNT = 8;

		private BodyData bodyData => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.body.AsReadOnlyData() : new BodyData(creatureID);

		public override LowerBodyType type
		{
			get => _type;
			protected set
			{
				_type = value;
				feet.UpdateType(value.footType);
			}
		}
		private LowerBodyType _type;

		public override LowerBodyType defaultType => LowerBodyType.defaultValue;

		public EpidermalData primaryEpidermis => type.ParseEpidermis(bodyData);
		public EpidermalData secondaryEpidermis => type.ParseEpidermis(bodyData);
		public int legCount => type.legCount;

		public bool isMonoped => legCount == MONOPED_LEG_COUNT;
		public bool isBiped => legCount == BIPED_LEG_COUNT;
		public bool isQuadruped => legCount == QUADRUPED_LEG_COUNT;
		public bool isSextoped => legCount == SEXTOPED_LEG_COUNT;
		public bool isOctoped => legCount == OCTOPED_LEG_COUNT;

		internal LowerBody(Guid creatureID, LowerBodyType type) : base(creatureID)
		{
			_type = type ?? throw new ArgumentNullException(nameof(type));
			feet = new Feet(creatureID, type.footType);
		}

		internal LowerBody(Guid creatureID) : this(creatureID, LowerBodyType.defaultValue)
		{ }

		protected internal override void PostPerkInit()
		{
			feet.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			feet.LateInit();
		}

		//standard update, restore are fine.

		internal override bool Validate(bool correctInvalidData)
		{
			LowerBodyType lowerBodyType = type;
			bool valid = LowerBodyType.Validate(ref lowerBodyType, correctInvalidData);
			type = lowerBodyType;
			return valid;
		}

		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.canAttackWith;

		public override LowerBodyData AsReadOnlyData()
		{
			return new LowerBodyData(creatureID, type, primaryEpidermis, secondaryEpidermis);
		}
	}

	public abstract partial class LowerBodyType : SaveableBehavior<LowerBodyType, LowerBody, LowerBodyData>
	{

		private const int NOLEGS = 0;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int SEXTOPED = 6;
		private const int OCTOPED = 8;

		public readonly byte legCount;

		private static int indexMaker = 0;
		private static readonly List<LowerBodyType> lowerBodyTypes = new List<LowerBodyType>();
		public static ReadOnlyCollection<LowerBodyType> availableTypes => new ReadOnlyCollection<LowerBodyType>(lowerBodyTypes.Where(x => x != null).ToList());

		public static LowerBodyType defaultValue => HUMAN;


		public readonly FootType footType;
		public readonly EpidermisType epidermisType;
		protected LowerBodyType(FootType foot, EpidermisType epidermis, byte numLegs,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBodyData> longDesc, PlayerBodyPartDelegate<LowerBody> playerDesc,
			ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			lowerBodyTypes.AddAt(this, _index);
			footType = foot;
			epidermisType = epidermis;
			legCount = numLegs;
		}

		internal static bool Validate(ref LowerBodyType type, bool correctInvalidData)
		{
			if (lowerBodyTypes.Contains(type))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				type = HUMAN;
			}
			return false;
		}

		internal static LowerBodyType Deserialize(int index)
		{
			if (index < 0 || index >= lowerBodyTypes.Count)
			{
				throw new System.ArgumentException("index for lower body type desrialize out of range");
			}
			else
			{
				LowerBodyType lowerBody = lowerBodyTypes[index];
				if (lowerBody != null)
				{
					return lowerBody;
				}
				else
				{
					throw new System.ArgumentException("index for lower body type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		public bool isMonoped => legCount == MONOPED;
		public bool isBiped => legCount == BIPED;
		public bool isQuadruped => legCount == QUADRUPED;
		public bool isSextoped => legCount == SEXTOPED;
		public bool isOctoped => legCount == OCTOPED;

		internal abstract EpidermalData ParseEpidermis(in BodyData bodyData);

		internal virtual EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}

		//internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith => attack != AttackBase.NO_ATTACK;
		//internal abstract bool canAttackWith { get; }

		public override int index => _index;
		private readonly int _index;


		public static readonly LowerBodyType HUMAN = new ToneLowerBody(FootType.HUMAN, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultHumanTone, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly LowerBodyType HOOVED = new FurLowerBodyWithKick(FootType.HOOVES, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedLongDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DOG = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultDogFur, FurTexture.NONDESCRIPT, true, DogDesc, DogLongDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly LowerBodyType CENTAUR = new FurLowerBodyWithKick(FootType.HOOVES, EpidermisType.FUR, QUADRUPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedLongDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType FERRET = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultFerretFur, FurTexture.NONDESCRIPT, false, FerretDesc, FerretLongDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly LowerBodyType DEMONIC_HIGH_HEELS = new ToneLowerBody(FootType.DEMON_HEEL, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultDemonTone, SkinTexture.NONDESCRIPT, true, DemonHiHeelsDesc, DemonHiHeelsLongDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly LowerBodyType DEMONIC_CLAWS = new ToneLowerBody(FootType.DEMON_CLAW, EpidermisType.SKIN, BIPED, DefaultValueHelpers.defaultDemonTone, SkinTexture.NONDESCRIPT, true, DemonClawDesc, DemonClawLongDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly LowerBodyType BEE = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, DefaultValueHelpers.defaultBeeTone, SkinTexture.SHINY, false, BeeDesc, BeeLongDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly LowerBodyType GOO = new ToneLowerBody(FootType.NONE, EpidermisType.GOO, MONOPED, DefaultValueHelpers.defaultGooTone, SkinTexture.NONDESCRIPT, true, GooDesc, GooLongDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly LowerBodyType CAT = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultCatFur, FurTexture.NONDESCRIPT, true, CatDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly LowerBodyType LIZARD = new ToneLowerBody(FootType.LIZARD_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultLizardTone, SkinTexture.NONDESCRIPT, true, LizardDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly LowerBodyType PONY = new FurLowerBodyWithKick(FootType.BRONY, EpidermisType.FUR, QUADRUPED, DefaultValueHelpers.defaultBronyFur, FurTexture.NONDESCRIPT, true, PonyDesc, PonyLongDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly LowerBodyType BUNNY = new FurLowerBodyWithKick(FootType.RABBIT, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultBunnyFur, FurTexture.NONDESCRIPT, true, BunnyDesc, BunnyLongDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly LowerBodyType HARPY = new FurLowerBody(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, DefaultValueHelpers.defaultHarpyFeathers, FurTexture.NONDESCRIPT, true, HarpyDesc, HarpyLongDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly LowerBodyType KANGAROO = new FurLowerBodyWithKick(FootType.KANGAROO, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultKangarooFur, FurTexture.NONDESCRIPT, true, KangarooDesc, KangarooLongDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly LowerBodyType CHITINOUS_SPIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, SpiderDesc, SpiderLongDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly LowerBodyType DRIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, OCTOPED, DefaultValueHelpers.defaultSpiderTone, SkinTexture.SHINY, false, DriderDesc, DriderLongDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly LowerBodyType FOX = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultFoxFur, FurTexture.NONDESCRIPT, true, FoxDesc, FoxLongDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly LowerBodyType DRAGON = new ToneLowerBody(FootType.DRAGON_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultDragonTone, SkinTexture.NONDESCRIPT, true, DragonDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly LowerBodyType RACCOON = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultRaccoonFur, FurTexture.NONDESCRIPT, true, RaccoonDesc, RaccoonLongDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly LowerBodyType CLOVEN_HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultHorseFur, FurTexture.NONDESCRIPT, true, ClovenHoofDesc, ClovenHoofLongDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly LowerBodyType NAGA = new NagaLowerBody();
		public static readonly LowerBodyType ECHIDNA = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultEchidnaFur, FurTexture.NONDESCRIPT, true, EchidnaDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly LowerBodyType SALAMANDER = new ToneLowerBody(FootType.MANDER_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultSalamanderTone, SkinTexture.NONDESCRIPT, true, SalamanderDesc, SalamanderLongDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly LowerBodyType WOLF = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultWolfFurColor, FurTexture.NONDESCRIPT, true, WolfDesc, WolfLongDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly LowerBodyType IMP = new ToneLowerBody(FootType.IMP_CLAW, EpidermisType.SCALES, BIPED, DefaultValueHelpers.defaultImpTone, SkinTexture.NONDESCRIPT, true, ImpDesc, ImpLongDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		//remind me to add this later.
		//public static readonly LowerBodyType KID_OR_SQUID = new ToneLowerBody(FootType.TENDRIL, EpidermisType.SKIN, SEXTOPED, DefaultValueHelpers.defaultTENTACLE_BEASTTone, SkinTexture.SLIMY, true, OctoDesc, OctoLongDesc, OctoPlayerStr, OctiTransformStr, OctoRestoreStr);
		public static readonly LowerBodyType COCKATRICE = new CockatriceLowerBody();
		public static readonly LowerBodyType RED_PANDA = new RedPandaLowerBody();

		private class FurLowerBody : LowerBodyType
		{
			public readonly FurColor defaultColor;
			public readonly FurTexture defaultTexture;
			protected readonly bool mutable;

			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;

			public FurLowerBody(FootType foot, FurBasedEpidermisType epidermis, byte numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
				SimpleDescriptor shortDesc, DescriptorWithArg<LowerBodyData> longDesc, PlayerBodyPartDelegate<LowerBody> playerDesc, ChangeType<LowerBodyData> transform,
				RestoreType<LowerBodyData> restore) : base(foot, epidermis, numLegs, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultColor = new FurColor(defaultFurColor);
				defaultTexture = defaultFurTexture;
				mutable = canChange;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = this.defaultColor;
				FurTexture texture = this.defaultTexture;
				if (mutable)
				{
					if (bodyData.supplementary.usesFur && !FurColor.IsNullOrEmpty(bodyData.supplementary.fur))
					{
						color = bodyData.supplementary.fur;
					}
					else if (!FurColor.IsNullOrEmpty(bodyData.main.fur))
					{
						color = bodyData.main.fur;
					}
					else if (!HairFurColors.IsNullOrEmpty(bodyData.hairColor))
					{
						color = new FurColor(bodyData.hairColor);
					}

					if (bodyData.supplementary.usesFur && bodyData.supplementary.furTexture != FurTexture.NONDESCRIPT)
					{
						texture = bodyData.supplementary.furTexture;
					}
					else if (bodyData.main.usesFur && bodyData.main.furTexture != FurTexture.NONDESCRIPT)
					{
						texture = bodyData.main.furTexture;
					}
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}
		}

		private class FurLowerBodyWithKick : FurLowerBody
		{
			public FurLowerBodyWithKick(FootType foot, FurBasedEpidermisType epidermis, byte numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
				SimpleDescriptor shortDesc, DescriptorWithArg<LowerBodyData> longDesc, PlayerBodyPartDelegate<LowerBody> playerDesc, ChangeType<LowerBodyData> transform, RestoreType<LowerBodyData> restore)
				: base(foot, epidermis, numLegs, defaultFurColor, defaultFurTexture, canChange, shortDesc, longDesc, playerDesc, transform, restore) { }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new GenericKick();
		}

		private class ToneLowerBody : LowerBodyType
		{
			public readonly SkinTexture defaultTexture;
			public readonly bool mutable;
			public readonly Tones defaultTone;

			protected ToneBasedEpidermisType primaryEpidermis => (ToneBasedEpidermisType)epidermisType;
			public ToneLowerBody(FootType foot, ToneBasedEpidermisType epidermis, byte legCount, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
				 SimpleDescriptor shortDesc, DescriptorWithArg<LowerBodyData> longDesc, PlayerBodyPartDelegate<LowerBody> playerDesc, ChangeType<LowerBodyData> transform,
				 RestoreType<LowerBodyData> restore) : base(foot, epidermis, legCount, shortDesc, longDesc, playerDesc, transform, restore)
			{
				defaultTexture = defaultSkinTexture;
				defaultTone = defTone;
				mutable = canChange;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
				SkinTexture texture = mutable && bodyData.main.usesTone ? bodyData.main.skinTexture : defaultTexture;

				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}


		private class NagaLowerBody : ToneLowerBody
		{
			private Tones defaultUnderTone => DefaultValueHelpers.defaultNagaUnderTone;
			public NagaLowerBody() : base(FootType.NONE, EpidermisType.SCALES, MONOPED, DefaultValueHelpers.defaultNagaTone,
				SkinTexture.NONDESCRIPT, true, NagaDesc, NagaLongDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr)
			{ }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.type == BodyType.NAGA && !Tones.IsNullOrEmpty(bodyData.supplementary.tone) ? bodyData.supplementary.tone : bodyData.mainSkin.tone;
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			internal override EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.tone;
				if (bodyData.type == BodyType.NAGA && !Tones.IsNullOrEmpty(bodyData.supplementary.tone))
				{
					color = DefaultValueHelpers.GetNageUnderToneFrom(color);
				}
				else if (Tones.IsNullOrEmpty(color))
				{
					color = defaultUnderTone;
				}
				return new EpidermalData(primaryEpidermis, color, defaultTexture);
			}

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new NagaConstrict();
		}

		private class CockatriceLowerBody : FurLowerBody
		{
			public CockatriceLowerBody() : base(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, DefaultValueHelpers.defaultCockatricePrimaryFeathers, FurTexture.NONDESCRIPT, true, CockatriceDesc, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr) { }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = this.defaultColor;
				FurTexture texture = this.defaultTexture;
				if (!FurColor.IsNullOrEmpty(bodyData.activeFur.fur))
				{
					color = bodyData.activeFur.fur;
				}
				else if (!HairFurColors.IsNullOrEmpty(bodyData.hairColor))
				{
					color = new FurColor(bodyData.hairColor);
				}
				return new EpidermalData(primaryEpidermis, color, bodyData.main.furTexture);
			}
		}

		private class RedPandaLowerBody : FurLowerBody
		{
			public RedPandaLowerBody() : base(FootType.PAW, EpidermisType.FUR, BIPED, DefaultValueHelpers.defaultRedPandaFur, FurTexture.NONDESCRIPT, true, RedPandaDesc, RedPandaLongDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr) { }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = bodyData.supplementary.usesFur && !FurColor.IsNullOrEmpty(bodyData.supplementary.fur) ? bodyData.supplementary.fur : defaultColor;
				FurTexture texture = bodyData.supplementary.usesFur && bodyData.supplementary.furTexture != FurTexture.NONDESCRIPT ? bodyData.supplementary.furTexture : defaultTexture;
				return new EpidermalData(primaryEpidermis, color, texture);
			}
		}
	}

	public sealed class LowerBodyData : BehavioralSaveablePartData<LowerBodyData, LowerBody, LowerBodyType>
	{
		public readonly EpidermalData primaryEpidermis;
		public readonly EpidermalData secondaryEpidermis;

		public byte legCount => type.legCount;
		public FootType footType => type.footType;

		public bool isMonoped => legCount == LowerBody.MONOPED_LEG_COUNT;
		public bool isBiped => legCount == LowerBody.BIPED_LEG_COUNT;
		public bool isQuadruped => legCount == LowerBody.QUADRUPED_LEG_COUNT;
		public bool isSextoped => legCount == LowerBody.SEXTOPED_LEG_COUNT;
		public bool isOctoped => legCount == LowerBody.OCTOPED_LEG_COUNT;

		public override LowerBodyData AsCurrentData()
		{
			return this;
		}

		internal LowerBodyData(Guid id, LowerBodyType type, EpidermalData epidermis, EpidermalData secondary) : base(id, type)
		{
			primaryEpidermis = epidermis;
			secondaryEpidermis = secondary;
		}

		internal LowerBodyData(Guid id) : base(id, LowerBodyType.defaultValue)
		{
			primaryEpidermis = new Epidermis(type.epidermisType).AsReadOnlyData();
			secondaryEpidermis = new EpidermalData();
		}
	}
}
