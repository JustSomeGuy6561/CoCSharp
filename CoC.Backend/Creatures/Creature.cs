//Creature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:13 PM
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Inventory;
using CoC.Backend.Items;
using CoC.Backend.Items.Wearables.Armor;
using CoC.Backend.Items.Wearables.LowerGarment;
using CoC.Backend.Items.Wearables.Shield;
using CoC.Backend.Items.Wearables.UpperGarment;
using CoC.Backend.Items.Wearables.Weapon;
using CoC.Backend.Perks;
using CoC.Backend.Pregnancies;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using CoC.Backend.UI;

namespace CoC.Backend.Creatures
{
	//The master class for any creature. It's responsible for exposing any internal functions related to all of its body parts, and providing common functionality that links them all
	//together.
	public abstract class Creature : ITimeActiveListenerSimple, ITimeDayMultiListenerSimple, ITimeLazyListener, IInteractiveStorage<CapacityItem>
	{
		public const byte DEFAULT_LIBIDO = 0;
		public const byte DEFAULT_SENSITIVITY = 0;

		public const byte DEFAULT_CORRUPTION = 0;
		//always start as these values;
		public const byte DEFAULT_LUST = 0;

		public const byte DEFAULT_STRENGTH = 15;
		public const byte DEFAULT_TOUGHNESS = 15;
		public const byte DEFAULT_SPEED = 15;
		public const byte DEFAULT_INTELLIGENCE = 15;

		internal const byte BASE_MAX_LIBIDO = 100;
		internal const byte BASE_MAX_SENSITIVITY = 100;
		internal const byte BASE_MAX_CORRUPTION = 100;
		internal const byte BASE_MAX_LUST = 100;

		internal const byte BASE_MAX_STRENGTH = 100;
		internal const byte BASE_MAX_TOUGHNESS = 100;
		internal const byte BASE_MAX_SPEED = 100;
		internal const byte BASE_MAX_INTELLIGENCE = 100;

		protected const byte ZERO = 0;
		public const byte LOWEST_POSSIBLE_MAX = 50;

		#region Stats
		public byte libido => (byte)Math.Floor(libidoTrue);
		public double libidoTrue
		{
			get => _libido;
			private protected set => _libido = Utils.Clamp2(value, minLibido, maxLibido);
		}
		private double _libido = 0;

		internal void ValidateLibido()
		{
			libidoTrue = libidoTrue;
		}

		public double relativeLibido => libidoTrue * (100f / maxLibido);

		public byte sensitivity => (byte)Math.Floor(sensitivityTrue);
		public double sensitivityTrue
		{
			get => _sensitivity;
			private protected set => _sensitivity = Utils.Clamp2(value, minSensitivity, maxSensitivity);
		}
		private double _sensitivity = 0;

		internal void ValidateSensitivity()
		{
			sensitivityTrue = sensitivityTrue;
		}

		public double relativeSensitivity => sensitivityTrue * (100f / maxSensitivity);

		public byte corruption => (byte)Math.Floor(corruptionTrue);
		public double corruptionTrue
		{
			get => _corruption;
			private protected set => _corruption = Utils.Clamp2(value, minCorruption, maxCorruption);
		}
		private double _corruption = 0;

		internal void ValidateCorruption()
		{
			corruptionTrue = corruptionTrue;
		}

		public double relativeCorruption => corruptionTrue * (100f / maxCorruption);

		public byte lust => (byte)Math.Floor(lustTrue);
		public double lustTrue
		{
			get => _lust;
			private protected set => _lust = Utils.Clamp2(value, minLust, maxLust);
		}
		private double _lust = 0;

		internal void ValidateLust()
		{
			lustTrue = lustTrue;
		}

		public double relativeLust => lustTrue * (100f / maxLust);

		public byte strength => (byte)Math.Floor(strengthTrue);
		public double strengthTrue
		{
			get => _strength;
			private protected set => _strength = Utils.Clamp2(value, minStrength, maxStrength);
		}
		private double _strength = 0;

		internal void ValidateStrength()
		{
			strengthTrue = strengthTrue;
		}

		public double relativeStrength => strengthTrue * (100f / maxStrength);

		public byte toughness => (byte)Math.Floor(toughnessTrue);
		public double toughnessTrue
		{
			get => _toughness;
			private protected set => _toughness = Utils.Clamp2(value, minToughness, maxToughness);
		}
		private double _toughness = 0;

		internal void ValidateToughness()
		{
			toughnessTrue = toughnessTrue;
		}

		public double relativeToughness => toughnessTrue * (100f / maxToughness);

		public byte speed => (byte)Math.Floor(speedTrue);
		public double speedTrue
		{
			get => _speed;
			private protected set => _speed = Utils.Clamp2(value, minSpeed, maxSpeed);
		}
		private double _speed = 0;

		internal void ValidateSpeed()
		{
			speedTrue = speedTrue;
		}

		public double relativeSpeed => speedTrue * (100f / maxSpeed);

		public byte intelligence => (byte)Math.Floor(intelligenceTrue);
		public double intelligenceTrue
		{
			get => _intelligence;
			private protected set => _intelligence = Utils.Clamp2(value, minIntelligence, maxIntelligence);
		}
		private double _intelligence = 0;

		internal void ValidateIntelligence()
		{
			intelligenceTrue = intelligenceTrue;
		}

		public double relativeIntelligence => intelligenceTrue * (100f / maxIntelligence);




		protected internal virtual sbyte bonusMinLibido => perks.baseModifiers.minLibidoDelta.GetValue();
		protected virtual byte baseMinLibido => 0;
		public byte minLibido => baseMinLibido.offset(bonusMinLibido);


		protected virtual sbyte bonusMinSensitivity => perks.baseModifiers.minSensitivityDelta.GetValue();
		protected virtual byte baseMinSensitivity => 0;
		public byte minSensitivity => baseMinSensitivity.offset(bonusMinSensitivity);


		protected virtual sbyte bonusMinCorruption => perks.baseModifiers.minCorruptionDelta.GetValue();
		protected virtual byte baseMinCorruption => 0;
		public byte minCorruption => baseMinCorruption.offset(bonusMinCorruption);


		protected virtual sbyte bonusMinLust => perks.baseModifiers.minLustDelta.GetValue();
		protected virtual byte baseMinLust => 0;
		public byte minLust => baseMinLust.offset(bonusMinLust);

		protected virtual sbyte bonusMinStrength => perks.baseModifiers.minStrengthDelta.GetValue();
		protected virtual byte baseMinStrength => 0;
		public byte minStrength => baseMinStrength.offset(bonusMinStrength);



		protected internal virtual sbyte bonusMinToughness => perks.baseModifiers.minToughnessDelta.GetValue();
		protected virtual byte baseMinToughness => 0;
		public byte minToughness => baseMinToughness.offset(bonusMinToughness);



		protected virtual sbyte bonusMinSpeed => perks.baseModifiers.minSpeedDelta.GetValue();
		protected virtual byte baseMinSpeed => 0;
		public byte minSpeed => baseMinSpeed.offset(bonusMinSpeed);


		protected virtual sbyte bonusMinIntelligence => perks.baseModifiers.minIntelligenceDelta.GetValue();
		protected virtual byte baseMinIntelligence => 0;
		public byte minIntelligence => baseMinIntelligence.offset(bonusMinIntelligence);


		protected virtual sbyte bonusMaxLibido => perks.baseModifiers.maxLibidoDelta.GetValue();
		protected internal virtual byte baseMaxLibido => BASE_MAX_LIBIDO;
		public byte maxLibido => HandleMaxStat(baseMaxLibido.offset(bonusMaxLibido), minLibido);

		protected virtual sbyte bonusMaxSensitivity => perks.baseModifiers.maxSensitivityDelta.GetValue();
		protected internal virtual byte baseMaxSensitivity => BASE_MAX_SENSITIVITY;
		public byte maxSensitivity => HandleMaxStat(baseMaxSensitivity.offset(bonusMaxSensitivity), minSensitivity);

		protected virtual sbyte bonusMaxCorruption => perks.baseModifiers.maxCorruptionDelta.GetValue();
		protected internal virtual byte baseMaxCorruption => BASE_MAX_CORRUPTION;
		public byte maxCorruption => HandleMaxStat(baseMaxCorruption.offset(bonusMaxCorruption), minCorruption);

		protected virtual sbyte bonusMaxLust => perks.baseModifiers.maxLustDelta.GetValue();
		protected internal virtual byte baseMaxLust => BASE_MAX_LUST;
		public byte maxLust => HandleMaxStat(baseMaxLust.offset(bonusMaxLust), minLust);

		protected virtual sbyte bonusMaxStrength => perks.baseModifiers.maxStrengthDelta.GetValue();
		protected internal virtual byte baseMaxStrength => BASE_MAX_STRENGTH;
		public byte maxStrength => HandleMaxStat(baseMaxStrength.offset(bonusMaxStrength), minStrength);

		protected virtual sbyte bonusMaxToughness => perks.baseModifiers.maxToughnessDelta.GetValue();
		protected internal virtual byte baseMaxToughness => BASE_MAX_TOUGHNESS;
		public byte maxToughness => HandleMaxStat(baseMaxToughness.offset(bonusMaxToughness), minToughness);

		protected virtual sbyte bonusMaxSpeed => perks.baseModifiers.maxSpeedDelta.GetValue();
		protected internal virtual byte baseMaxSpeed => BASE_MAX_SPEED;
		public byte maxSpeed => HandleMaxStat(baseMaxSpeed.offset(bonusMaxSpeed), minSpeed);

		protected virtual sbyte bonusMaxIntelligence => perks.baseModifiers.maxIntelligenceDelta.GetValue();
		protected internal virtual byte baseMaxIntelligence => BASE_MAX_INTELLIGENCE;
		public byte maxIntelligence => HandleMaxStat(baseMaxIntelligence.offset(bonusMaxIntelligence), minIntelligence);


		protected byte HandleMaxStat(byte computedValue, byte minValue)
		{
			if (computedValue >= minValue && computedValue >= LOWEST_POSSIBLE_MAX)
			{
				return computedValue;
			}
			else if (minValue > LOWEST_POSSIBLE_MAX)
			{
				return minValue;
			}
			else
			{
				return LOWEST_POSSIBLE_MAX;
			}
		}

		protected double libidoGainMultiplier => perks.baseModifiers.libidoGainMultiplier.GetValue();
		protected double libidoLossMultiplier => perks.baseModifiers.libidoLossMultiplier.GetValue();

		protected double sensitivityGainMultiplier => perks.baseModifiers.sensitivityGainMultiplier.GetValue();
		protected double sensitivityLossMultiplier => perks.baseModifiers.sensitivityLossMultiplier.GetValue();

		protected double corruptionGainMultiplier => perks.baseModifiers.corruptionGainMultiplier.GetValue();
		protected double corruptionLossMultiplier => perks.baseModifiers.corruptionLossMultiplier.GetValue();

		protected double lustGainMultiplier => perks.baseModifiers.lustGainMultiplier.GetValue();
		protected double lustLossMultiplier => perks.baseModifiers.lustLossMultiplier.GetValue();


		protected double strengthGainMultiplier => perks.baseModifiers.strengthGainMultiplier.GetValue();
		protected double strengthLossMultiplier => perks.baseModifiers.strengthLossMultiplier.GetValue();

		protected double toughnessGainMultiplier => perks.baseModifiers.toughnessGainMultiplier.GetValue();
		protected double toughnessLossMultiplier => perks.baseModifiers.toughnessLossMultiplier.GetValue();

		protected double speedGainMultiplier => perks.baseModifiers.speedGainMultiplier.GetValue();
		protected double speedLossMultiplier => perks.baseModifiers.speedLossMultiplier.GetValue();

		protected double intelligenceGainMultiplier => perks.baseModifiers.intelligenceGainMultiplier.GetValue();
		protected double intelligenceLossMultiplier => perks.baseModifiers.intelligenceLossMultiplier.GetValue();
		#endregion

		public int gems { get; protected set; }

		public double gemsGainRate => perks.baseModifiers.gemsGainRate.GetValue();

		public int AddGems(uint amount)
		{
			int oldGems = gems;
			gems += amount > int.MaxValue ? int.MaxValue : (int)amount;
			return gems - oldGems;
		}

		public int RemoveGems(uint amount, bool canGoNegative = false)
		{
			int lossAmt = amount > int.MaxValue ? int.MaxValue : (int)amount;
			int oldGems = gems;
			if (gems < 0 && !canGoNegative)
			{
				return 0;
			}
			gems -= lossAmt;
			if (gems < 0 && !canGoNegative)
			{
				gems = 0;
			}
			return gems - oldGems; //i can flip this if we want.

		}


		public virtual string possessiveNoun => Conjugate.FromCreature(this).PossessiveNoun();
		public virtual string objectNoun => Conjugate.FromCreature(this).ObjectNoun();
		public virtual string personalNoun => Conjugate.FromCreature(this).PersonalNoun();
		public virtual string possessiveAdjective => Conjugate.FromCreature(this).PossessiveAdjective();
		public virtual string reflexiveNoun => Conjugate.FromCreature(this).ReflexiveNoun();
		public virtual string personalNounWithHave => Conjugate.FromCreature(this).PersonalNounWithHave();
		public virtual string personalNounWithAre => Conjugate.FromCreature(this).PersonalNounWithAre();

		//definitive article means 'the' or the equivalent. if false, it's assumed to be the indefinite article, aka 'a' or the equivalent.
		//note the these may return the empty string if the name is a proper noun (aka a name).
		public abstract string Article(bool definitiveArticle);



		public readonly string name;

		public readonly Guid id;

		public readonly Antennae antennae;
		public readonly Arms arms;
		public readonly Back back;
		//public readonly Beard beard;
		public readonly Body body;
		public readonly Build build;
		public readonly Ears ears;
		public readonly Eyes eyes;
		public readonly Face face;

		public readonly Genitals genitals;
		public readonly Gills gills;
		public readonly Hair hair;
		public readonly Horns horns;
		public readonly LowerBody lowerBody;
		public readonly Neck neck;
		public Ovipositor ovipositor => tail.ovipositor;
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;

		//body aliases and helpers
		public bool hasAnyFur => body.hasActiveFurData;

		public bool hasPrimaryFur => body.mainEpidermis.usesFurColor;
		public bool hasSupplementaryFur => body.supplementaryEpidermis.usesFurColor;

		public bool hasPlainSkin => body.mainEpidermis.type == EpidermisType.SKIN;
		public FurColor ActiveHairOrFurColor() => body.ActiveHairOrFurColor();

		//aliases for build.
		public Butt butt => build.butt;
		public Hips hips => build.hips;

		//aliases for genitals.
		public Ass ass => genitals.ass;
		public ReadOnlyCollection<Breasts> breasts => genitals.breastRows;
		public ReadOnlyCollection<Cock> cocks => genitals.cocks;
		public ReadOnlyCollection<Vagina> vaginas => genitals.vaginas;
		public Balls balls => genitals.balls;

		public Clit[] clits => genitals.clits;
		public Femininity femininity => genitals.femininity;

		public Fertility fertility => genitals.fertility;

		//aliases for arms/legs
		public Hands hands => arms.hands;
		public Feet feet => lowerBody.feet;

		public readonly PerkCollection perks;
		//public readonly StatusEffectCollection statusEffects;

		public byte heightInInches => build.heightInInches;


		public Womb womb => genitals.womb;

		//other aliases that are nice.

		public bool hasTail => tail.type != TailType.NONE;

		public bool hasAntennae => antennae.type != AntennaeType.NONE;

		public bool hasHorns => horns.type != HornType.NONE;

		public bool hasOvipositor => ovipositor.type != OvipositorType.NONE;

		public bool hasGills => gills.type != GillType.NONE;

		public bool hasWings => wings.type != WingType.NONE;

		public bool hasBalls => genitals.hasBalls;

		public bool hasCock => cocks.Count > 0;
		//public bool hasCockOrClitCock => hasCock || hasClitCock;

		//public bool hasClitCock => genitals.hasClitCock;

		//public bool clitCockActive => !hasCock && hasClitCock;

		public bool hasVagina => vaginas.Count > 0;


		public Gender gender => genitals.gender;


		protected BasePerkModifiers modifiers => perks.baseModifiers;

		protected readonly Inventory.BasicInventory inventoryStore;

#warning NYI
		//raise equipment changed.
		public ArmorBase armor { get; protected set; }

		public UpperGarmentBase upperGarment { get; protected set; }
		public LowerGarmentBase lowerGarment { get; protected set; }

		public ShieldBase shield { get; protected set; }
		public WeaponBase weapon { get; protected set; }

		public bool wearingArmor => !ArmorBase.IsNullOrNothing(armor);
		public bool wearingUpperGarment => !UpperGarmentBase.IsNullOrNothing(upperGarment);
		public bool wearingLowerGarment => !LowerGarmentBase.IsNullOrNothing(lowerGarment);
		public bool wearingAnything => !ArmorBase.IsNullOrNothing(armor) || !UpperGarmentBase.IsNullOrNothing(upperGarment) || !LowerGarmentBase.IsNullOrNothing(lowerGarment);

		//public bool HasPerk<T>() where T : PerkBase
		//{
		//	return perks.HasPerk<T>();
		//}

		//public bool AddPerk(PerkBase perkBase) => perks.AddPerk(perkBase);

		//public bool AddPerk<T>() where T : PerkBase, new() => perks.AddPerk<T>();
		//public bool AddOrStackPerk<T>() where T : StackablePerk, new() => perks.AddOrStackPerk<T>();

		//public bool AddOrStackPerk(StackablePerk perk) => perks.AddOrStackPerk(perk);

		//public bool StackPerk<T>() where T : StackablePerk, new() => perks.StackPerk<T>();

		//public T GetPerk<T>() where T : PerkBase
		//{
		//	return perks.GetPerk<T>();
		//}

		//public bool RemovePerk<T>() where T : PerkBase
		//{
		//	return perks.RemovePerk<T>();
		//}

		//public bool AddStatusEffect(StatusEffectBase statusEffectBase) => statusEffects.AddStatusEffect(statusEffectBase);

		//public bool AddStatusEffect<T>() where T : StatusEffectBase, new() => statusEffects.AddStatusEffect<T>();

		////checks to see if an instance of this statusEffect type exists in the collection
		//public bool HasStatusEffect<T>() where T : StatusEffectBase => perks.HasTimedEffect<T>();

		//public bool RemoveStatusEffect<T>() where T : StatusEffectBase => statusEffects.RemoveStatusEffect<T>();

		//public bool RemoveStatusEffect(Type type) => statusEffects.RemoveStatusEffect(type);

		//public bool RemoveStatusEffect(StatusEffectBase effect) => statusEffects.RemoveStatusEffect(effect);

		//public T GetStatusEffect<T>() where T : StatusEffectBase
		//{
		//	return statusEffects.GetStatusEffect<T>();
		//}

		public bool AddPerk(StandardPerk perkBase) => perks.AddPerk(perkBase);

		public bool AddPerk<T>() where T : StandardPerk, new() => perks.AddPerk<T>();


		public bool AddOrStackPerk<T>() where T : StackablePerk, new() => perks.AddOrStackPerk<T>();


		public bool AddOrStackPerk(StackablePerk perk) => perks.AddOrStackPerk(perk);


		public bool StackPerk<T>() where T : StackablePerk => perks.StackPerk<T>();


		//checks to see if an instance of this perk type exists in the collection
		public bool HasPerk<T>() where T : StandardPerk => perks.HasPerk<T>();


		//checks to see if an instance of this perk type exists in the collection
		public bool HasAnyPerk() => perks.HasAnyPerk();

		public bool AddTimedEffect(TimedPerk timedPerk) => perks.AddTimedEffect(timedPerk);


		public bool AddTimedEffect<T>() where T : TimedPerk, new() => perks.AddTimedEffect<T>();


		//checks to see if an instance of this perk type exists in the collection
		public bool HasTimedEffect<T>() where T : TimedPerk => perks.HasTimedEffect<T>();


		public bool ConditionalPerkActive<T>() where T : ConditionalPerk => perks.ConditionalPerkActive<T>();


		//removes perk of this type if the character has it. if they didn't have it to begin with, returns false. otherwise removes it and returns true.
		public bool RemovePerk<T>() where T : StandardPerk => perks.RemovePerk<T>();

		public bool RemovePerk(Type type) => perks.RemovePerk(type);

		public bool RemovePerk(StandardPerk effect) => perks.RemovePerk(effect);

		public bool RemoveTimedEffect(TimedPerk effect) => perks.RemoveTimedEffect(effect);

		//retrieves a timed perk, so you can update it.
		public T GetPerkData<T>() where T : StandardPerk => perks.GetPerkData<T>();


		public bool RemoveTimedEffect<T>() where T : TimedPerk => perks.RemoveTimedEffect<T>();


		//retrieves a timed perk, so you can update it.
		public T GetTimedEffectData<T>() where T : TimedPerk => perks.GetTimedEffectData<T>();


		//retrives a conditional perk, so you can update it. note that by default, this does not return conditional perks that aren't currently active.
		public T GetConditionalPerkData<T>(bool ignoreIfInactive = true) where T : ConditionalPerk => perks.GetConditionalPerkData<T>(ignoreIfInactive);


		//public Weapon weapon { get; protected set; }
		//public Weapon weapon { get; protected set; }

		//protected bool antennaeParticipatesInTimeEvents = true;
		//protected bool armsParticipatesInTimeEvents = true;
		//protected bool backParticipatesInTimeEvents = true;
		////protected bool beardParticipatesInTimeEvents = true;
		//protected bool bodyParticipatesInTimeEvents = true;
		//protected bool buildParticipatesInTimeEvents = true;
		//protected bool earsParticipatesInTimeEvents = true;
		//protected bool eyesParticipatesInTimeEvents = true;
		//protected bool faceParticipatesInTimeEvents = true;

		//protected bool genitalsParticipatesInTimeEvents = true;
		//protected bool gillsParticipatesInTimeEvents = true;
		//protected bool hairParticipatesInTimeEvents = true;
		//protected bool hornsParticipatesInTimeEvents = true;
		//protected bool lowerBodyParticipatesInTimeEvents = true;
		//protected bool neckParticipatesInTimeEvents = true;
		//protected bool tailParticipatesInTimeEvents = true;
		//protected bool tongueParticipatesInTimeEvents = true;
		//protected bool wingsParticipatesInTimeEvents = true;

		public readonly ReadOnlyCollection<IBodyPart> bodyParts;

		#region Constructors
		protected Creature(CreatureCreator creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException();
			}

			libidoTrue = creator.libido ?? DEFAULT_LIBIDO;
			sensitivityTrue = creator.sensitivity ?? DEFAULT_SENSITIVITY;
			corruptionTrue = creator.corruption ?? DEFAULT_CORRUPTION;
			lustTrue = DEFAULT_LUST;

			name = creator.name;

			id = CreatureStore.GenerateCreature(this);

			//semantically, we Should do the things other parts can depend on first, but as long as we
			//dont actually require the data in the generate functions (which we generally shouldn't - that's why we're lazy)
			//it won't matter. Anything that needs this stuff for validation

			if (creator.bodyType is null)
			{
				body = new Body(id);
			}
			else
			{
				body = new Body(id, creator.bodyType, creator.furColor, creator.furTexture, creator.complexion, creator.skinTexture, creator.underFurColor,
					creator.underBodyFurTexture, creator.underTone, creator.underBodySkinTexture);
			}

			//build
			if (creator.heightInInches == 0)
			{
				creator.heightInInches = Build.DEFAULT_HEIGHT;
			}
			build = new Build(id, creator.heightInInches, creator.thickness, creator.tone, creator.hipSize, creator.buttSize);

			//genitals

			Gender gender;
			if (creator.defaultGender != null)
			{
				gender = (Gender)creator.defaultGender;
			}
			else if (creator.cocks?.Length > 0 || creator.vaginas?.Length > 0)
			{
				gender = Gender.GENDERLESS;
				if (creator.cocks?.Any(x => x != null) == true)
				{
					gender |= Gender.MALE;
				}
				if (creator.vaginas?.Any(x => x != null) == true)
				{
					gender |= Gender.FEMALE;
				}
			}
			else
			{
#if DEBUG
				Debug.WriteLine("No gender data available. Falling back to default gender of Male");
#endif
				gender = Gender.MALE;
			}


			Ass ass = new Ass(id, creator.analWetness, creator.analLooseness, creator.assVirgin, creator.hasAnalPractice);

			Balls balls;
			if (creator.numBalls != null && creator.ballSize != null)
			{
				balls = new Balls(id, (byte)creator.numBalls, (byte)creator.ballSize);
			}
			else
			{
				balls = new Balls(id, gender);
			}

			Fertility fertility;
			if (creator.fertility == null)
			{
				fertility = new Fertility(id, gender, creator.artificiallyInfertile);
			}
			else
			{
				fertility = new Fertility(id, (byte)creator.fertility, creator.artificiallyInfertile);
			}

			Womb womb = creator.GetWomb(id);

			CupSize cup = gender.HasFlag(Gender.FEMALE) ? Breasts.DEFAULT_FEMALE_SIZE : Breasts.DEFAULT_MALE_SIZE;

			BreastCreator[] breasts = creator.breasts ?? new BreastCreator[] { new BreastCreator(cup) };

			switch (gender)
			{
				case Gender.GENDERLESS:
					genitals = new Genitals(id, ass, creator.breasts, null, new Balls(id, false), null, womb, creator.femininity, fertility); break;
				case Gender.MALE:

					CockCreator[] cocks = creator.cocks;
					if (cocks == null || cocks.Length == 0)
					{
						cocks = new CockCreator[] { new CockCreator() };
					}
					genitals = new Genitals(id, ass, breasts, cocks, balls, null, womb, creator.femininity, fertility); break;
				case Gender.FEMALE:
					VaginaCreator[] vaginas = creator.vaginas;
					if (vaginas == null || vaginas.Length == 0)
					{
						vaginas = new VaginaCreator[] { new VaginaCreator() };
					}
					genitals = new Genitals(id, ass, breasts, null, balls, vaginas, womb, creator.femininity, fertility); break;
				case Gender.HERM:
				default:
					cocks = creator.cocks;
					if (cocks == null)
					{
						cocks = new CockCreator[] { new CockCreator() };
					}
					vaginas = creator.vaginas;
					if (vaginas == null)
					{
						vaginas = new VaginaCreator[] { new VaginaCreator() };
					}
					genitals = new Genitals(id, ass, breasts, cocks, balls, vaginas, womb, creator.femininity, fertility); break;
			}

			//antennae
			antennae = creator.antennaeType != null ? new Antennae(id, creator.antennaeType) : new Antennae(id);
			//arms
			arms = creator.armType != null ? new Arms(id, creator.armType) : new Arms(id);
			//back
			if (creator.backType == null)
			{
				back = new Back(id);
			}
			else if (creator.backType == BackType.DRACONIC_MANE && !HairFurColors.IsNullOrEmpty(creator.backHairFur))
			{
				back = new Back(id, BackType.DRACONIC_MANE, creator.backHairFur);
			}
			else
			{
				back = new Back(id, creator.backType);
			}

			//ears
			ears = creator.earType != null ? new Ears(id, creator.earType) : new Ears(id);
			//eyes
			if (creator.eyeType == null && creator.leftEyeColor == null && creator.rightEyeColor == null)
			{
				eyes = new Eyes(id);
			}
			else
			{
				if (creator.eyeType == null)
				{
					creator.eyeType = EyeType.defaultValue;
				}

				if (creator.leftEyeColor == null && creator.rightEyeColor == null)
				{
					eyes = new Eyes(id, creator.eyeType);
				}
				else if (creator.leftEyeColor == null || creator.rightEyeColor == null)
				{
					EyeColor eyeColor = creator.leftEyeColor ?? (EyeColor)creator.rightEyeColor;
					eyes = new Eyes(id, creator.eyeType, eyeColor);
				}
				else
				{
					eyes = new Eyes(id, creator.eyeType, (EyeColor)creator.leftEyeColor, (EyeColor)creator.rightEyeColor);
				}
			}
			//face
			if (creator.faceType is null)
			{
				creator.faceType = FaceType.defaultValue;
			}

			if (creator.facialSkinTexture != null)
			{
				face = new Face(id, creator.faceType, creator.isFaceFullMorph, (SkinTexture)creator.facialSkinTexture);
			}
			else if (creator.isFaceFullMorph != null)
			{
				face = new Face(id, creator.faceType, creator.isFaceFullMorph);
			}
			else
			{
				face = new Face(id, creator.faceType);
			}

			//gills
			gills = creator.gillType != null ? new Gills(id, creator.gillType) : new Gills(id);

			if (creator.hairType is null && HairFurColors.IsNullOrEmpty(creator.hairColor) && creator.hairLength == null
				&& creator.hairHighlightColor is null && creator.hairStyle == HairStyle.NO_STYLE)
			{
				hair = new Hair(id);
			}
			else
			{
				if (creator.hairType == null)
				{
					creator.hairType = HairType.defaultValue;
				}
				hair = new Hair(id, creator.hairType, creator.hairColor, creator.hairHighlightColor, creator.hairLength, creator.hairStyle, creator.hairTransparent);
			}

			//FemininityData femininityData = new FemininityData(genitals.femininity);

			//horns
			if (creator?.hornType == null)
			{
				horns = new Horns(id);
			}
			else if (creator.hornCount != null && creator.hornSize != null)
			{
				horns = new Horns(id, creator.hornType, (byte)creator.hornSize, (byte)creator.hornCount);
			}
			else if (creator.additionalHornTransformStrength != 0)
			{
				horns = new Horns(id, creator.hornType, creator.additionalHornTransformStrength, creator.forceUniformHornGrowthOnCreate);
			}
			else
			{
				horns = new Horns(id, creator.hornType);
			}
			//Lower Body
			lowerBody = creator.lowerBodyType != null ? new LowerBody(id, creator.lowerBodyType) : new LowerBody(id);
			//Neck
			if (creator.neckType == null)
			{
				neck = new Neck(id);
			}
			else if (creator.neckLength != 0)
			{
				neck = new Neck(id, creator.neckType, creator.hairColor, creator.neckLength);
			}
			else
			{
				neck = new Neck(id, creator.neckType);
			}

			//Tail (with ovipositor)
			if (creator.tailType is null)
			{
				tail = new Tail(id);
			}
			else if (creator.tailCount != null)
			{
				if (creator.tailType.allowsOvipositor)
				{
					tail = new Tail(id, creator.tailType, (byte)creator.tailCount, creator.hasOvipositorIfApplicable);
				}
				else
				{
					tail = new Tail(id, creator.tailType, (byte)creator.tailCount);
				}
			}
			else if (creator.tailType.allowsOvipositor)
			{
				tail = new Tail(id, creator.tailType, creator.hasOvipositorIfApplicable);
			}
			else
			{
				tail = new Tail(id, creator.tailType);
			}
			//tongue
			tongue = creator.tongueType != null ? new Tongue(id, creator.tongueType) : new Tongue(id);

			if (creator.wingType is null)
			{
				wings = new Wings(id);
			}
			else if (creator.wingType is TonableWings tonableWings)
			{
				wings = new Wings(id, tonableWings, creator.primaryWingTone, creator.secondaryWingTone);
			}
			else if (creator.wingType is FeatheredWings featheredWings)
			{
				wings = new Wings(id, featheredWings, creator.wingFeatherColor);
			}
			else
			{
				wings = new Wings(id, creator.wingType);
			}


			//body.InializePiercings(creator?.navelPiercings);
			//ears.InitializePiercings(creator?.earPiercings);
			//face.InitializePiercings(creator?.eyebrowPiercings, creator?.lipPiercings, creator?.nosePiercings);
			//tongue.InitializePiercings(creator?.tonguePiercings);

			//tail.InitializePiercings(creator?.tailPiercings);

			inventoryStore = new BasicInventory();

			perks = new PerkCollection(this, GameEngine.GetConditionalPerks());
			perks.InitPerks(creator.perks?.ToArray());

			//statusEffects = new StatusEffectCollection(this);

			DoPostPerkInit();
			DoLateInit();

			List<IBodyPart> bps = new List<IBodyPart>()
			{
				antennae,
				arms,
				back,
				//beard,
				body,
				build,
				ears,
				eyes,
				face,
				genitals,
				gills,
				hair,
				horns,
				lowerBody,
				neck,
				tail,
				tongue,
				wings,
			};

			bodyParts = new ReadOnlyCollection<IBodyPart>(bps);

			AttachBodyListeners();
			FreezeCreature(); //by default. simply call unfreeze where needed.
		}

		//internal Creature(CreatureSaveFormat format)
		//{
		//	//pull data from format
		//	SetupBindings();
		//	//ValidateData();
		//}
		private void DoPostPerkInit()
		{
			antennae.PostPerkInit();
			arms.PostPerkInit();
			back.PostPerkInit();
			//beard.PostPerkInit();
			body.PostPerkInit();
			build.PostPerkInit();
			ears.PostPerkInit();
			eyes.PostPerkInit();
			face.PostPerkInit();
			genitals.PostPerkInit();
			gills.PostPerkInit();
			hair.PostPerkInit();
			horns.PostPerkInit();
			lowerBody.PostPerkInit();
			neck.PostPerkInit();
			tail.PostPerkInit();
			tongue.PostPerkInit();
			wings.PostPerkInit();
		}

		private void DoLateInit()
		{
			antennae.LateInit();
			arms.LateInit();
			back.LateInit();
			//beard.LateInit();
			body.LateInit();
			build.LateInit();
			ears.LateInit();
			eyes.LateInit();
			face.LateInit();
			genitals.LateInit();
			gills.LateInit();
			hair.LateInit();
			horns.LateInit();
			lowerBody.LateInit();
			neck.LateInit();
			tail.LateInit();
			tongue.LateInit();
			wings.LateInit();
		}

		private protected virtual void AttachBodyListeners()
		{
			lazyBodyListeners.Add(back);
			lazyBodyListeners.Add(genitals);
			lazyBodyListeners.Add(hair);
			//back, femininity, genitals, hair - lazy.
			//genitals - daily.
		}
		#endregion

		public void IngestCum()
		{
			IngestLiquid();
			throw new NotImplementedException();
		}

		public void IngestLiquid()
		{
			throw new NotImplementedException();
		}

		#region Stat Updates
		public double IncreaseLibido(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= libidoGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = libidoTrue;
			libidoTrue += amount;
			return libidoTrue - oldValue;
		}
		public double IncreaseLibidoBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = libidoTrue;
			double offset = percent * maxLibido;
			if (!ignorePerks)
			{
				offset *= libidoGainMultiplier;
			}
			libidoTrue += offset;
			return libidoTrue - oldValue;
		}

		public double DecreaseLibido(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= libidoGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = libidoTrue;
			libidoTrue -= amount;
			return oldValue - libidoTrue;
		}
		public double DecreaseLibidoBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = libidoTrue;
			double offset = percent * maxLibido;
			if (!ignorePerks)
			{
				offset *= libidoLossMultiplier;
			}
			libidoTrue -= offset;
			return oldValue - libidoTrue;
		}

		public double ChangeLibido(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseLibido(delta, ignorePerks);
			}
			else
			{
				return -DecreaseLibido(-delta, ignorePerks);
			}
		}

		public double SetLibido(byte value)
		{
			libidoTrue = value;
			return libidoTrue;
		}
		public double SetLibidoPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxLibido * percent;
			libidoTrue = value;
			return libidoTrue;
		}


		public double IncreaseSensitivity(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= sensitivityGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = sensitivityTrue;
			sensitivityTrue += amount;
			return sensitivityTrue - oldValue;
		}
		public double IncreaseSensitivityBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = sensitivityTrue;
			double offset = percent * maxSensitivity;
			if (!ignorePerks)
			{
				offset *= sensitivityGainMultiplier;
			}
			sensitivityTrue += offset;
			return sensitivityTrue - oldValue;
		}

		public double DecreaseSensitivity(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= sensitivityGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = sensitivityTrue;
			sensitivityTrue -= amount;
			return oldValue - sensitivityTrue;
		}
		public double DecreaseSensitivityBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = sensitivityTrue;
			double offset = percent * maxSensitivity;
			if (!ignorePerks)
			{
				offset *= sensitivityLossMultiplier;
			}
			sensitivityTrue -= offset;
			return oldValue - sensitivityTrue;
		}

		public double ChangeSensitivity(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseSensitivity(delta, ignorePerks);
			}
			else
			{
				return -DecreaseSensitivity(-delta, ignorePerks);
			}
		}

		public double SetSensitivity(byte value)
		{
			sensitivityTrue = value;
			return sensitivityTrue;
		}
		public double SetSensitivityPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxSensitivity * percent;
			sensitivityTrue = value;
			return sensitivityTrue;
		}

		public double IncreaseCorruption(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= corruptionGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = corruptionTrue;
			corruptionTrue += amount;
			return corruptionTrue - oldValue;
		}
		public double IncreaseCorruptionBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = corruptionTrue;
			double offset = percent * maxCorruption;
			if (!ignorePerks)
			{
				offset *= corruptionGainMultiplier;
			}
			corruptionTrue += offset;
			return corruptionTrue - oldValue;
		}

		public double DecreaseCorruption(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= corruptionGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = corruptionTrue;
			corruptionTrue -= amount;
			return oldValue - corruptionTrue;
		}
		public double DecreaseCorruptionBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = corruptionTrue;
			double offset = percent * maxCorruption;
			if (!ignorePerks)
			{
				offset *= corruptionLossMultiplier;
			}
			corruptionTrue -= offset;
			return oldValue - corruptionTrue;
		}

		public double ChangeCorruption(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseCorruption(delta, ignorePerks);
			}
			else
			{
				return -DecreaseCorruption(-delta, ignorePerks);
			}
		}

		public double SetCorruption(byte value)
		{
			corruptionTrue = value;
			return corruptionTrue;
		}
		public double SetCorruptionPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxCorruption * percent;
			corruptionTrue = value;
			return corruptionTrue;
		}

		public double IncreaseLust(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= lustGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = lustTrue;
			lustTrue += amount;
			return lustTrue - oldValue;
		}
		public double IncreaseLustBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = lustTrue;
			double offset = percent * maxLust;
			if (!ignorePerks)
			{
				offset *= lustGainMultiplier;
			}
			lustTrue += offset;
			return lustTrue - oldValue;
		}

		public double DecreaseLust(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= lustGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			double oldValue = lustTrue;
			lustTrue -= amount;
			return oldValue - lustTrue;
		}
		public double DecreaseLustBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = lustTrue;
			double offset = percent * maxLust;
			if (!ignorePerks)
			{
				offset *= lustLossMultiplier;
			}
			lustTrue -= offset;
			return oldValue - lustTrue;
		}

		public double ChangeLust(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseLust(delta, ignorePerks);
			}
			else
			{
				return -DecreaseLust(-delta, ignorePerks);
			}
		}

		public double SetLust(byte value)
		{
			lustTrue = value;
			return lustTrue;
		}
		public double SetLustPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxLust * percent;
			lustTrue = value;
			return lustTrue;
		}

		public double IncreaseStrength(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= strengthGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = strengthTrue;
			strengthTrue += amount;
			return strengthTrue - oldValue;
		}
		public double IncreaseStrengthBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = strengthTrue;
			double delta = percent * maxStrength;
			if (!ignorePerks)
			{
				delta *= strengthGainMultiplier;
			}
			strengthTrue += delta;
			return strengthTrue - oldValue;
		}

		public double DecreaseStrength(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= strengthGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = strengthTrue;
			strengthTrue -= amount;
			return oldValue - strengthTrue;
		}
		public double DecreaseStrengthByPercent(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = strengthTrue;
			double delta = percent * maxStrength;
			if (!ignorePerks)
			{
				delta *= strengthLossMultiplier;
			}
			strengthTrue -= delta;
			return oldValue - strengthTrue;
		}

		public double ChangeStrength(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseStrength(delta, ignorePerks);
			}
			else
			{
				return -DecreaseStrength(-delta, ignorePerks);
			}
		}

		public double SetStrength(byte value)
		{
			strengthTrue = value;
			return strengthTrue;
		}
		public double SetStrengthPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxStrength * percent;
			strengthTrue = value;
			return strengthTrue;
		}

		public double IncreaseToughness(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= toughnessGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = toughnessTrue;
			toughnessTrue += amount;
			return toughnessTrue - oldValue;
		}
		public double IncreaseToughnessBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = toughnessTrue;
			double delta = percent * maxToughness;
			if (!ignorePerks)
			{
				delta *= toughnessGainMultiplier;
			}
			toughnessTrue += delta;
			return toughnessTrue - oldValue;
		}

		public double DecreaseToughness(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= toughnessGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = toughnessTrue;
			toughnessTrue -= amount;
			return oldValue - toughnessTrue;
		}
		public double DecreaseToughnessByPercent(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = toughnessTrue;
			double delta = percent * maxToughness;
			if (!ignorePerks)
			{
				delta *= toughnessLossMultiplier;
			}
			toughnessTrue -= delta;
			return oldValue - toughnessTrue;
		}

		public double ChangeToughness(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseToughness(delta, ignorePerks);
			}
			else
			{
				return -DecreaseToughness(-delta, ignorePerks);
			}
		}

		public double SetToughness(byte value)
		{
			toughnessTrue = value;
			return toughnessTrue;
		}
		public double SetToughnessPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxToughness * percent;
			toughnessTrue = value;
			return toughnessTrue;
		}

		public double IncreaseSpeed(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= speedGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = speedTrue;
			speedTrue += amount;
			return speedTrue - oldValue;
		}
		public double IncreaseSpeedBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = speedTrue;
			double delta = percent * maxSpeed;
			if (!ignorePerks)
			{
				delta *= speedGainMultiplier;
			}
			speedTrue += delta;
			return speedTrue - oldValue;
		}

		public double DecreaseSpeed(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= speedGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = speedTrue;
			speedTrue -= amount;
			return oldValue - speedTrue;
		}
		public double DecreaseSpeedBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = speedTrue;
			double delta = percent * maxSpeed;
			if (!ignorePerks)
			{
				delta *= speedLossMultiplier;
			}
			speedTrue -= delta;
			return oldValue - speedTrue;
		}

		public double ChangeSpeed(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseSpeed(delta, ignorePerks);
			}
			else
			{
				return -DecreaseSpeed(-delta, ignorePerks);
			}
		}

		public double SetSpeed(byte value)
		{
			speedTrue = value;
			return speedTrue;
		}
		public double SetSpeedPercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxSpeed * percent;
			speedTrue = value;
			return speedTrue;
		}

		public double IncreaseIntelligence(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= intelligenceGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = intelligenceTrue;
			intelligenceTrue += amount;
			return intelligenceTrue - oldValue;
		}
		public double IncreaseIntelligenceBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = intelligenceTrue;
			double delta = percent * maxIntelligence;
			if (!ignorePerks)
			{
				delta *= intelligenceGainMultiplier;
			}
			intelligenceTrue += delta;
			return intelligenceTrue - oldValue;
		}

		public double DecreaseIntelligence(double amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= intelligenceGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			var oldValue = intelligenceTrue;
			intelligenceTrue -= amount;
			return oldValue - intelligenceTrue;
		}
		public double DecreaseIntelligenceBy(double percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			double oldValue = intelligenceTrue;
			double delta = percent * maxIntelligence;
			if (!ignorePerks)
			{
				delta *= intelligenceLossMultiplier;
			}
			intelligenceTrue -= delta;
			return oldValue - intelligenceTrue;
		}

		public double ChangeIntelligence(double delta, bool ignorePerks = false)
		{
			if (delta == 0)
			{
				return 0;
			}
			else if (delta > 0)
			{
				return IncreaseIntelligence(delta, ignorePerks);
			}
			else
			{
				return -DecreaseIntelligence(-delta, ignorePerks);
			}
		}

		public double SetIntelligence(byte value)
		{
			intelligenceTrue = value;
			return intelligenceTrue;
		}
		public double SetIntelligencePercent(double percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			double value = maxIntelligence * percent;
			intelligenceTrue = value;
			return intelligenceTrue;
		}

		public void IncreaseCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, byte str = 0, byte tou = 0, byte spe = 0, byte inte = 0, bool ignorePerks = false)
		{
			double amount;
			if (lus != 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= lustGainMultiplier;
				}

				lustTrue += amount;
			}
			if (lib != 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= libidoGainMultiplier;
				}

				libidoTrue += amount;
			}
			if (sens != 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= sensitivityGainMultiplier;
				}

				sensitivityTrue += amount;
			}
			if (corr != 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= corruptionGainMultiplier;
				}

				corruptionTrue += amount;
			}

			if (str != 0)
			{
				amount = str;
				if (!ignorePerks) amount *= strengthGainMultiplier;
				strengthTrue += amount;
			}
			if (tou != 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= toughnessGainMultiplier;
				toughnessTrue += amount;
			}
			if (spe != 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= speedGainMultiplier;
				speedTrue += amount;
			}
			if (inte != 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= intelligenceGainMultiplier;
				intelligenceTrue += amount;
			}
		}

		public void DecreaseCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, byte str = 0, byte tou = 0, byte spe = 0, byte inte = 0, bool ignorePerks = false)
		{
			double amount;
			if (lus != 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= lustLossMultiplier;
				}

				lustTrue -= amount;
			}
			if (lib != 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= libidoLossMultiplier;
				}

				libidoTrue -= amount;
			}
			if (sens != 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= sensitivityLossMultiplier;
				}

				sensitivityTrue -= amount;
			}
			if (corr != 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= corruptionLossMultiplier;
				}

				corruptionTrue -= amount;
			}

			if (str != 0)
			{
				amount = str;
				if (!ignorePerks) amount *= strengthLossMultiplier;
				strengthTrue -= amount;
			}
			if (tou != 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= toughnessLossMultiplier;
				toughnessTrue -= amount;
			}
			if (spe != 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= speedLossMultiplier;
				speedTrue -= amount;
			}
			if (inte != 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= intelligenceLossMultiplier;
				intelligenceTrue -= amount;
			}
		}

		public void SetCreatureStats(byte? lus = null, byte? lib = null, byte? sens = null, byte? corr = null, byte? str = null, byte? tou = null, byte? spe = null, byte? inte = null)
		{
			if (lus is byte ltb)
			{
				lustTrue = ltb;
			}
			if (lib is byte lbb)
			{
				libidoTrue = lbb;
			}
			if (sens is byte sb)
			{
				sensitivityTrue = sb;
			}
			if (corr is byte cb)
			{
				corruptionTrue = cb;
			}

			if (str is byte stb)
			{
				strengthTrue = stb;
			}
			if (tou is byte tb)
			{
				toughnessTrue = tb;
			}
			if (spe is byte spb)
			{
				speedTrue = spb;
			}
			if (inte is byte ib)
			{
				intelligenceTrue = ib;
			}
		}

		public void DeltaCreatureStats(double lus = 0, double lib = 0, double sens = 0, double corr = 0, double str = 0, double tou = 0, double spe = 0, double inte = 0, bool ignorePerks = false)
		{
			double amount;
			if (lus < 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= lustLossMultiplier;
				}

				lustTrue += amount;
			}
			else if (lus > 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= lustGainMultiplier;
				}

				lustTrue += amount;
			}
			if (lib < 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= libidoLossMultiplier;
				}

				libidoTrue += amount;
			}
			else if (lib > 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= libidoGainMultiplier;
				}

				libidoTrue += amount;
			}
			if (sens < 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= sensitivityLossMultiplier;
				}

				sensitivityTrue += amount;
			}
			else if (sens > 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= sensitivityGainMultiplier;
				}

				sensitivityTrue += amount;
			}
			if (corr < 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= corruptionLossMultiplier;
				}

				corruptionTrue += amount;
			}
			else if (corr > 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= corruptionGainMultiplier;
				}

				corruptionTrue += amount;
			}

			if (str < 0)
			{
				amount = str;
				if (!ignorePerks) amount *= strengthLossMultiplier;
				strengthTrue += amount;
			}
			else if (str > 0)
			{
				amount = str;
				if (!ignorePerks) amount *= strengthGainMultiplier;
				strengthTrue += amount;
			}
			if (tou < 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= toughnessLossMultiplier;
				toughnessTrue += amount;
			}
			else if (tou > 0)
			{
				amount = tou;
				if (!ignorePerks) amount *= toughnessGainMultiplier;
				toughnessTrue += amount;
			}
			if (spe < 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= speedLossMultiplier;
				speedTrue += amount;
			}
			else if (spe > 0)
			{
				amount = spe;
				if (!ignorePerks) amount *= speedGainMultiplier;
				speedTrue += amount;
			}
			if (inte < 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= intelligenceLossMultiplier;
				intelligenceTrue += amount;
			}
			else if (inte > 0)
			{
				amount = inte;
				if (!ignorePerks) amount *= intelligenceGainMultiplier;
				intelligenceTrue += amount;
			}
		}


		#endregion
		#region Add/Remove Genitals Related

		#region Breast Add/Remove
		public bool AddBreastRow()
		{
			return genitals.AddBreastRow();
		}
		public bool AddBreastRowAverage()
		{
			return genitals.AddBreastRowAverage();
		}

		public bool AddBreastRow(CupSize cup)
		{
			return genitals.AddBreastRow(cup);
		}

		public int RemoveBreastRow(int count = 1)
		{
			return genitals.RemoveBreastRows(count);
		}

		public int RemoveExtraBreastRows()
		{
			return genitals.RemoveExtraBreastRows();
		}
		#endregion
		#region Cock Add/Remove
		public bool AddCock() => AddCock(CockType.defaultValue);

		public bool AddCock(CockType newCockType)
		{
			return genitals.AddCock(newCockType);
		}

		public bool AddCock(CockType newCockType, double length, double girth, double? knotMultiplier = null)
		{
			return genitals.AddCock(newCockType, length, girth, knotMultiplier);
		}

		public int RemoveCock(int count = 1)
		{
			return genitals.RemoveCock(count);
		}

		public int RemoveCockAt(int index, int count = 1)
		{
			return genitals.RemoveCockAt(index, count);
		}

		public bool RemoveCock(Cock cock)
		{
			return genitals.RemoveCock(cock);
		}

		public int RemoveExtraCocks()
		{
			return genitals.RemoveExtraCocks();
		}

		public int RemoveAllCocks()
		{
			return genitals.RemoveAllCocks();
		}
		#endregion
		#region Vagina Add/Remove

		public bool AddVagina() => genitals.AddVagina();
		public bool AddVagina(VaginaType newVaginaType)
		{
			return genitals.AddVagina(newVaginaType);
		}

		public bool AddVagina(double clitLength) => genitals.AddVagina(clitLength);
		public bool AddVagina(VaginaType newVaginaType, double clitLength)
		{
			return genitals.AddVagina(newVaginaType, clitLength);
		}

		public bool AddVagina(double clitLength, VaginalLooseness looseness, VaginalWetness wetness) => genitals.AddVagina(clitLength, looseness, wetness);
		public bool AddVagina(VaginaType newVaginaType, double clitLength, VaginalLooseness looseness, VaginalWetness wetness)
		{
			return genitals.AddVagina(newVaginaType, clitLength, looseness, wetness);
		}

		public int RemoveVagina(int count = 1)
		{
			return genitals.RemoveVagina(count);
		}

		public bool RemoveVagina(Vagina vagina)
		{
			return genitals.RemoveVagina(vagina);
		}

		public int RemoveVaginaAt(int index, int count = 1)
		{
			return genitals.RemoveVaginaAt(index, count);
		}

		public int RemoveExtraVaginas()
		{
			return genitals.RemoveExtraVaginas();
		}

		public int RemoveAllVaginas()
		{
			return genitals.RemoveAllVaginas();
		}
		#endregion

		#endregion
		#region Equipment Related
		//equip is handled by use item.

		//Normally, items are passive and just tell their consumer what to do, but equipment requires them to actively handle any other equipment they replace.
		//This means the standard way of using items, with just a public UseItemOrWhatever(...) is not enough. We also need a way to retrieve the information about
		//whatever item we are replacing (if any), and we need to do so without exposing it to the rest of the code base, because that would circumvent all the checks
		//we do and any consequences of doing so. hence, the following internal change functions for the various pieces of equipment.


		internal ArmorBase ChangeArmor(ArmorBase armorBase)
		{
			ArmorBase currentArmor = armor;
			armor = armorBase ?? ArmorBase.NOTHING;
			return currentArmor;
		}

		internal UpperGarmentBase ChangeUpperGarment(UpperGarmentBase upperGarmentBase)
		{
			UpperGarmentBase garment = upperGarment;
			upperGarment = upperGarmentBase ?? UpperGarmentBase.NOTHING;
			return garment;
		}

		internal LowerGarmentBase ChangeLowerGarment(LowerGarmentBase lowerGarmentBase)
		{
			LowerGarmentBase garment = lowerGarment;
			this.lowerGarment = lowerGarmentBase ?? LowerGarmentBase.NOTHING;
			return garment;
		}

		#endregion
		#region Inventory Related
		public ReadOnlyCollection<ReadOnlyItemSlot> inventory => inventoryStore.itemSlots;

		public byte UnlockAdditionalInventorySlots(byte amount = 1)
		{
			return inventoryStore.UnlockAdditionalSlots(amount);
		}

		public bool AddKeyItemToInventory(KeyItem key)
		{
			return inventoryStore.AddItem(key);
		}

		public bool InventoryHasKeyItem(KeyItem key)
		{
			return inventoryStore.HasItem(key);
		}

		public bool InventoryHasKeyItem(Predicate<KeyItem> condition)
		{
			return inventoryStore.HasItem(condition);
		}

		public KeyItem GetKeyItemFromInventory(Predicate<KeyItem> condition)
		{
			return inventoryStore.GetItem(condition);
		}

		public IEnumerable<KeyItem> GetAllKeyItemsMatching(Predicate<KeyItem> condition)
		{
			return inventoryStore.GetAllItems(condition);
		}

		public bool RemoveKeyItemFromInventory(KeyItem key)
		{
			return inventoryStore.RemoveKeyItem(key);
		}

		public int RemoveKeyItemsFromInventoryWhere(Predicate<KeyItem> condition)
		{
			return inventoryStore.RemoveWhere(condition);
		}

		public bool RemoveFirstKeyItemWhere(Predicate<KeyItem> condition)
		{
			return inventoryStore.RemoveFirst(condition);
		}

		ReadOnlyCollection<ReadOnlyItemSlot> IInteractiveStorage<CapacityItem>.ItemSlots()
		{
			return inventory;
		}

		string IInteractiveStorage<CapacityItem>.PlaceItemInSlot(CapacityItem item, byte slot)
		{
			return PlaceItemInCreatureStorageText(item, slot);
		}
		protected abstract string PlaceItemInCreatureStorageText(CapacityItem item, byte slot);

		string IInteractiveStorage<CapacityItem>.ReturnItemToSlot(CapacityItem item, byte slot)
		{
			return ReturnItemToCreatureStorageText(item, slot);
		}
		protected abstract string ReturnItemToCreatureStorageText(CapacityItem item, byte slot);

		string IInteractiveStorage<CapacityItem>.ReplaceItemInSlotWith(CapacityItem item, byte slot)
		{
			return ReplaceItemInCreatureStorageWithNewItemText(item, slot);
		}
		protected abstract string ReplaceItemInCreatureStorageWithNewItemText(CapacityItem newItem, byte slot);

		public CapacityItem RetrieveItemFromSlot(byte slot)
		{
			return inventoryStore.RemoveItem(slot);
		}

		public bool PlaceItem(CapacityItem item, byte slot)
		{
			return inventoryStore.AddItemBack(slot, item);
		}

		bool IInteractiveStorage<CapacityItem>.ReplaceItem(CapacityItem replacement, byte slot)
		{
			CapacityItem item = inventory[slot].item;
			ReplaceItemInSlot(slot, item, true);
			return item != inventory[slot].item;
		}

		public bool CanAddItem(CapacityItem item)
		{
			return inventoryStore.CanAddItem(item);
		}

		public int TryAddItem(CapacityItem item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			return inventoryStore.AddItemReturnSlot(item);
		}

		public bool TryAddItem(CapacityItem item, out string whatHappened)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			int slot = inventoryStore.AddItemReturnSlot(item);
			if (slot != -1)
			{
				whatHappened = AddedItemToSlot(item, slot);
				return true;
			}
			else
			{
				whatHappened = ItemsAreFull();
				return false;
			}
		}

		private string ItemsAreFull()
		{
			throw new NotImplementedException();
		}

		public void ClearItemSlot(byte index)
		{
			inventoryStore.ClearSlot(index);
		}

		public void ReplaceItemInSlot(byte index, CapacityItem replacement, bool addIfSameItem = true)
		{
			inventoryStore.ReplaceItemInSlot(index, replacement, addIfSameItem);
		}

		//Using items manually will return a display. if it's not null, it's expected you will load that display so the player sees it, though how you choose to do so is up to you.
		//(for example, you may choose to merge the text from the current display into the new one, OR call DoNext and have it load the display when the player clicks next).
		//This is for all items, including armors and other equipables. Though no equipables currently do this, it's now possible for equipables to display a menu adjusting their
		//settings when they are equipped, which is consistent with how consumable items work.
		//Note: Just like with consumable items, if an item does not require a menu, these will return null (because they have no display to give you)

		//This is admittedly more complex than the original, but there's an automatic version of these that takes care of all of this for you.

		public DisplayBase UseItemManual(CapacityItem item, UseItemCallback onUseItemReturn)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (onUseItemReturn is null)
			{
				throw new ArgumentNullException(nameof(onUseItemReturn));
			}

			if (item.CanUse(this, false, out string whyNot))
			{
				return item.UseItem(this, onUseItemReturn);
			}
			else
			{
				onUseItemReturn(false, whyNot, item.Author(), item);
				return null;
			}
		}

		public DisplayBase UseItemInInventoryManual(byte index, UseItemCallback onUseItemReturn)
		{
			if (onUseItemReturn is null)
			{
				throw new ArgumentNullException(nameof(onUseItemReturn));
			}

			if (index >= inventory.Count)
			{
				throw new IndexOutOfRangeException("inventory does not have that many slots currently.");
			}

			if (inventory[index].isEmpty)
			{
				onUseItemReturn(false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!inventory[index].item.CanUse(this, false, out string whyNot))
			{
				onUseItemReturn(false, whyNot, inventory[index].item.Author(), null);
				return null;
			}
			else
			{
				CapacityItem item = inventoryStore.RemoveItem(index);
				return item.UseItem(this, onUseItemReturn);
			}
		}

		public DisplayBase EquipArmorManual(ArmorBase armor, UseItemCallbackSafe<ArmorBase> postEquipCallback)
		{
			if (ArmorBase.IsNullOrNothing(armor))
			{
				postEquipCallback(false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!armor.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, armor.Author(), armor);
				return null;
			}
			else
			{
				return armor.UseItemSafe(this, postEquipCallback);
			}
		}

		public DisplayBase EquipArmorFromInventoryManual(byte index, UseItemCallbackSafe<ArmorBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is ArmorBase armorItem))
			{
				postEquipCallback(false, InCorrectTypeErrorText(typeof(ArmorBase)), string.Empty, null);
				return null;
			}
			else if (!armorItem.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, armorItem.Author(), null);
				return null;
			}
			else
			{
				ArmorBase item = (ArmorBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postEquipCallback);
			}
		}

		//remove the armor, retrieving it for you to manually parse. note that if the armor destroys itself when being removed, this will return null.
		public ArmorBase RemoveArmorManual(out string removeText)
		{
			var old = ChangeArmor(null);

			if (old is null)
			{
				removeText = "";
				return null;
			}
			else
			{
				return old.DoRemove(this, out removeText);
			}
		}

		//remove the current armor, and replace it with the current one. since it may be possible for an armor equip to use a menu, the removed armor is sent along to the callback.
		//you will need to manually parse the display returned by this. if the removed armor destroys itself when being removed, the corresponding value passed along to the
		//callback will be null.
		public DisplayBase ReplaceArmorManual(ArmorBase armor, UseItemCallbackSafe<ArmorBase> postReplaceArmorCallback)
		{
			if (ArmorBase.IsNullOrNothing(armor))
			{
				ArmorBase item = RemoveArmorManual(out string removeText);
				postReplaceArmorCallback(true, removeText, item.Author(), item);
				return null;
			}
			else if (!armor.CanUse(this, false, out string whyNot))
			{
				postReplaceArmorCallback(false, whyNot, armor.Author(), armor);
				return null;
			}
			else
			{
				return armor.UseItemSafe(this, postReplaceArmorCallback);
			}
		}

		public DisplayBase ReplaceArmorFromInventoryManual(byte index, UseItemCallbackSafe<ArmorBase> postReplaceArmorCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceArmorCallback(false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is ArmorBase armorItem))
			{
				postReplaceArmorCallback(false, InCorrectTypeErrorText(typeof(ArmorBase)), string.Empty, null);
				return null;
			}
			else if (!armorItem.CanUse(this, false, out string whyNot))
			{
				postReplaceArmorCallback(false, whyNot, armorItem.Author(), null);
				return null;
			}
			else
			{
				ArmorBase item = (ArmorBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postReplaceArmorCallback);
			}
		}

		public DisplayBase EquipUpperGarmentManual(UpperGarmentBase upperGarment, UseItemCallbackSafe<UpperGarmentBase> postEquipCallback)
		{
			if (UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				postEquipCallback(false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!upperGarment.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, upperGarment.Author(), upperGarment);
				return null;
			}
			else
			{
				return upperGarment.UseItemSafe(this, postEquipCallback);
			}
		}

		public DisplayBase EquipUpperGarmentFromInventoryManual(byte index, UseItemCallbackSafe<UpperGarmentBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, NoItemInSlotErrorText(), string.Empty, null);
				return null;
			}
			else if (!(inventory[index].item is UpperGarmentBase upperGarmentItem))
			{
				postEquipCallback(false, InCorrectTypeErrorText(typeof(UpperGarmentBase)), string.Empty, null);
				return null;
			}
			else if (!upperGarmentItem.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, upperGarmentItem.Author(), null);
				return null;
			}
			else
			{
				UpperGarmentBase item = (UpperGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postEquipCallback);
			}
		}

		public UpperGarmentBase RemoveUpperGarmentManual(out string removeText)
		{
			var result = ChangeUpperGarment(null);

			if (result is null)
			{
				removeText = "";
				return null;
			}
			else
			{
				return result.DoRemove(this, out removeText);
			}

		}

		public DisplayBase ReplaceUpperGarmentManual(UpperGarmentBase upperGarment, UseItemCallbackSafe<UpperGarmentBase> postReplaceUpperGarmentCallback)
		{
			if (UpperGarmentBase.IsNullOrNothing(upperGarment))
			{
				UpperGarmentBase item = RemoveUpperGarmentManual(out string removeText);
				postReplaceUpperGarmentCallback(true, removeText, item.Author(), item);
				return null;
			}
			else if (!upperGarment.CanUse(this, false, out string whyNot))
			{
				postReplaceUpperGarmentCallback(false, whyNot, upperGarment.Author(), upperGarment);
				return null;
			}
			else
			{
				return upperGarment.UseItemSafe(this, postReplaceUpperGarmentCallback);
			}
		}

		public DisplayBase ReplaceUpperGarmentFromInventoryManual(byte index, UseItemCallbackSafe<UpperGarmentBase> postReplaceUpperGarmentCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceUpperGarmentCallback(false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is UpperGarmentBase upperGarmentItem))
			{
				postReplaceUpperGarmentCallback(false, InCorrectTypeErrorText(typeof(UpperGarmentBase)), "", null);
				return null;
			}
			else if (!upperGarmentItem.CanUse(this, false, out string whyNot))
			{
				postReplaceUpperGarmentCallback(false, whyNot, upperGarmentItem.Author(), null);
				return null;
			}
			else
			{
				UpperGarmentBase item = (UpperGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postReplaceUpperGarmentCallback);
			}
		}

		public DisplayBase EquipLowerGarmentManual(LowerGarmentBase lowerGarment, UseItemCallbackSafe<LowerGarmentBase> postEquipCallback)
		{
			if (LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				postEquipCallback(false, YouGaveMeANull(), null, null);
				return null;
			}
			else if (!lowerGarment.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, lowerGarment.Author(), lowerGarment);
				return null;
			}
			else
			{
				return lowerGarment.UseItemSafe(this, postEquipCallback);
			}
		}

		public DisplayBase EquipLowerGarmentFromInventoryManual(byte index, UseItemCallbackSafe<LowerGarmentBase> postEquipCallback)
		{
			if (inventory[index].isEmpty)
			{
				postEquipCallback(false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is LowerGarmentBase lowerGarmentItem))
			{
				postEquipCallback(false, InCorrectTypeErrorText(typeof(LowerGarmentBase)), "", null);
				return null;
			}
			else if (!lowerGarmentItem.CanUse(this, false, out string whyNot))
			{
				postEquipCallback(false, whyNot, lowerGarmentItem.Author(), null);
				return null;
			}
			else
			{
				LowerGarmentBase item = (LowerGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postEquipCallback);
			}
		}

		public LowerGarmentBase RemoveLowerGarmentManual(out string removeText)
		{
			var result = ChangeLowerGarment(null);

			if (result is null)
			{
				removeText = "";
				return null;
			}
			else
			{
				return result.DoRemove(this, out removeText);
			}
		}

		public DisplayBase ReplaceLowerGarmentManual(LowerGarmentBase lowerGarment, UseItemCallbackSafe<LowerGarmentBase> postReplaceLowerGarmentCallback)
		{
			if (LowerGarmentBase.IsNullOrNothing(lowerGarment))
			{
				LowerGarmentBase item = RemoveLowerGarmentManual(out string removeText);
				postReplaceLowerGarmentCallback(true, removeText, "", item);
				return null;
			}
			else if (!lowerGarment.CanUse(this, false, out string whyNot))
			{
				postReplaceLowerGarmentCallback(false, whyNot, lowerGarment.Author(), lowerGarment);
				return null;
			}
			else
			{
				return lowerGarment.UseItemSafe(this, postReplaceLowerGarmentCallback);
			}
		}

		public DisplayBase ReplaceLowerGarmentFromInventoryManual(byte index, UseItemCallbackSafe<LowerGarmentBase> postReplaceLowerGarmentCallback)
		{
			if (inventory[index].isEmpty)
			{
				postReplaceLowerGarmentCallback(false, NoItemInSlotErrorText(), "", null);
				return null;
			}
			else if (!(inventory[index].item is LowerGarmentBase lowerGarmentItem))
			{
				postReplaceLowerGarmentCallback(false, InCorrectTypeErrorText(typeof(LowerGarmentBase)), "", null);
				return null;
			}
			else if (!lowerGarmentItem.CanUse(this, false, out string whyNot))
			{
				postReplaceLowerGarmentCallback(false, whyNot, lowerGarmentItem.Author(), null);
				return null;
			}
			else
			{
				LowerGarmentBase item = (LowerGarmentBase)inventoryStore.RemoveItem(index);
				return item.UseItemSafe(this, postReplaceLowerGarmentCallback);
			}
		}









		protected string YouGaveMeANull()
		{
			throw new NotImplementedException();
		}

		protected string NoItemInSlotErrorText()
		{
			throw new NotImplementedException();
		}
		protected string ReturnItemToPreviousSlotFailedForSomeReasonText(byte originalIndex, CapacityItem originalItem)
		{
			throw new NotImplementedException();
		}

		protected string ReturnItemToPreviousSlotText(byte originalIndex, CapacityItem originalItem)
		{
			throw new NotImplementedException();
		}



		protected string InCorrectTypeErrorText(Type type)
		{
			throw new NotImplementedException();
		}

		protected string AddedItemToSlot(CapacityItem item, int slot)
		{
			throw new NotImplementedException();
		}

		protected string FailedToAddItemAutomaticallyDiscard(CapacityItem item)
		{
			throw new NotImplementedException();
		}

		#endregion
		#region Sex-Related
		//Note: it's possible to have multiple body parts achieve orgasm, but only want to count it as one orgasm for the global total.
		//this is denoted with the countTowardOrgasmTotal bool. set it to false to prevent these multiple orgasm instances from falsely incrementing the total.

		private GameDateTime timeLastOrgasm { get; set; }
		public int hoursSinceLastOrgasm => timeLastOrgasm.hoursToNow();
		public uint orgasmCount { get; private set; } = 0;

		private void Orgasmed()
		{
			timeLastOrgasm = GameDateTime.Now;
			orgasmCount++;
			SetLust(0);
			//raise orgasm event.
		}

		//#region Take Anal




		public bool TakeAnalPenetration(double length, double girth, double knotWidth, StandardSpawnType knockupType, double cumAmount, byte virilityBonus, bool countAsSex,
			bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleAnalPenetration(length, girth, knotWidth, knockupType, cumAmount, virilityBonus, countAsSex, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public void TakeAnalPenetrationNoKnockup(double length, double girth, double knotWidth, double cumAmount, bool countAsSex, bool reachOrgasm,
			bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleAnalPenetration(length, girth, knotWidth, cumAmount, countAsSex, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public bool TakeAnalSex(Cock source, StandardSpawnType knockupType, double cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleAnalPenetration(source, knockupType, cumAmountOverride, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeAnalSex(Cock source, StandardSpawnType knockupType, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest)
		{
			bool retVal = genitals.HandleAnalPenetration(source, knockupType, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool AttemptManualAnalKnockup(StandardSpawnType knockupType, double knockupRate)
		{
			return genitals.HandleAnalPregnancyOverride(knockupType, knockupRate);
		}

		/// <summary>
		/// Provides a means for achieving anal orgasm when not caused by standard penetration and sex functions.
		/// </summary>
		/// <param name="dryOrgasm">is this particular orgasm caused without any anal stimultion?</param>
		/// <param name="countTowardOrgasmTotal">does this orgasm count toward the total orgasm count?</param>
		public void HaveGenericAnalOrgasm(bool dryOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleAnalOrgasmGeneric(dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Give Anal
		//used to determine if you have an anal-sex fetish, mostly.
		public uint TimesFuckedAnotherAss { get; private set; } = 0;

		//public void FuckAButtWithADick(int cockIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{

		//}
		//public void FuckAButtWithAClit(int vaginaIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{

		//}
		//public void FuckAButtWithAClitCock(int vaginaIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{

		//}
		//public void TongueAButt(bool reachOrgasm, bool countTowardOrgasmTotal)
		//{

		//}

		//public void FuckAButtWithDickNipples(int breastIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{

		//}

		//public void PenetrateAButtGeneric()
		//{
		//	TimesFuckedAnotherAss++;
		//}
		//#endregion
		//#region Take Vaginal

		public bool TakeVaginalPenetrationNoKnockup(int vaginaIndex, double length, double girth, double knotWidth, double cumAmount, bool countAsSex, bool reachOrgasm,
			bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, cumAmount, countAsSex, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}
		public bool TakeVaginalPenetration(int vaginaIndex, double length, double girth, double knotWidth, StandardSpawnType knockupType, double cumAmount, byte virilityBonus,
			bool countAsSex, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, knockupType, cumAmount, virilityBonus, countAsSex, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeVaginalSex(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeVaginalSex(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, double cumAmountOverride, bool reachOrgasm,
			bool countTowardOrgasmTotal, bool selfCest = false)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, cumAmountOverride, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool AttemptManualVaginalKnockup(int vaginaIndex, StandardSpawnType knockupType, double knockupRate)
		{
			return genitals.HandleVaginalPregnancyOverride(vaginaIndex, knockupType, knockupRate);
		}

		//'Dry' orgasm is orgasm without stimulation.
		public void HaveGenericVaginalOrgasm(int vaginaIndex, bool dryOrgasm, bool countTowardOrgasmTotal)
		{

			genitals.HandleVaginaOrgasmGeneric(vaginaIndex, dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		//
		//#endregion
		//#region Give Vaginal
		//public uint TimesFuckedAnotherVagina { get; private set; } = 0;
		//public uint TimesSoundedAnotherClitCock { get; private set; } = 0;

		//public void PenetrateAPussyGeneric()
		//{
		//	TimesFuckedAnotherVagina++;
		//}

		//public void SoundAnotherClitCockGeneric()
		//{
		//	TimesSoundedAnotherClitCock++;
		//}


		//#endregion
		//#region Vaginal Penetrates

		internal void PenetrateSomethingWithAClit(int vaginaIndex, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleClitPenetrate(vaginaIndex, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Take Tit-Sex
		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		public void GetTittyFucked(int breastIndex, Cock sourceCock, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest= false)
		{
			genitals.HandleTittyFuck(breastIndex, sourceCock, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void GetTittyFucked(int breastIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleTittyFuck(breastIndex, sourceCock, cumAmountOverride, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void GetTittyFucked(int breastIndex, double length, double girth, double knotWidth, double cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal,
			bool selfCest = false)
		{
			genitals.HandleTittyFuck(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void HaveGenericTitOrgasm(int breastIndex, bool dryOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleTitOrgasmGeneric(breastIndex, dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeNipplePenetration(int breastIndex, Cock sourceCock, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleNipplePenetration(breastIndex, sourceCock, reachOrgasm, selfCest);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeNipplePenetration(int breastIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleNipplePenetration(breastIndex, sourceCock, cumAmountOverride, reachOrgasm, selfCest);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeNipplePenetration(int breastIndex, double length, double girth, double knotWidth, double cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal,
			bool selfCest = false)
		{
			genitals.HandleNipplePenetration(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm, selfCest);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		//#endregion
		//#region Penetrate With Tits
		//public void PenetrateSomethingWithNippleDicks(int breastIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		//{
		//	genitals.HandleNippleDickPenetrate(breastIndex, reachOrgasm);
		//	if (reachOrgasm && countTowardOrgasmTotal)
		//	{
		//		Orgasmed();
		//	}
		//}
		//#endregion
		//#region Give Tit-Sex
		//public uint TimesFuckedSomeTitties { get; private set; } = 0;
		//public uint TimesFuckedANipple { get; private set; } = 0;

		//public void PenetrateANippleGeneric()
		//{
		//	TimesFuckedANipple++;
		//}

		//public void DoATitFuckGeneric()
		//{
		//	TimesFuckedSomeTitties++;
		//}
		//#endregion
		//#region Take with Cock
		public void TakeCockSounding(int cockIndex, double penetratorLength, double penetratorWidth, double knotSize, double cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleCockSounding(cockIndex, penetratorLength, penetratorWidth, knotSize, cumAmount, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeCockSounding(int cockIndex, Cock sourceCock, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleCockSounding(cockIndex, sourceCock, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeCockSounding(int cockIndex, Cock sourceCock, double cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleCockSounding(cockIndex, sourceCock, cumAmountOverride, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		//#endregion
		#region Give Cock Sex
		public void PenetrateWithCockGeneric(int cockIndex, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			genitals.HandleCockPenetrate(cockIndex, reachOrgasm, selfCest);
		}

		public void HaveGenericCockOrgasm(int cockIndex, bool dryOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.DoCockOrgasmGeneric(cockIndex, dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Take in Mouth (Oral)

		public void TakeOralPenetration(double penetratorArea, double knotWidth, int cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			face.HandleOralPenetration(penetratorArea, knotWidth, cumAmount, reachOrgasm, selfCest);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeOralSex(Cock penetrator, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			face.HandleOralPenetration(penetrator, reachOrgasm, selfCest);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeOralSex(Cock penetrator, double cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal, bool selfCest = false)
		{
			face.HandleOralPenetration(penetrator, cumAmountOverride, reachOrgasm, selfCest);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void HaveGenericOralOrgasm(bool dryOrgasm, bool countTowardOrgasmTotal)
		{
			face.HandleOralOrgasmGeneric(dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Give With Mouth
		public void PenetrateSomethingWithTongue(bool reachOrgasm, bool countTowardOrgasmTotal, bool isSelf = false)
		{
			tongue.DoPenetrate(isSelf);
			if (reachOrgasm)
			{
				face.HandleOralOrgasmGeneric(false);
				if (countTowardOrgasmTotal)
				{
					Orgasmed();
				}
			}
		}

		public void LickSomethingWithTongue(bool reachOrgasm, bool countTowardOrgasmTotal, bool isSelf = false)
		{
			tongue.DoLicking(isSelf);
			if (reachOrgasm)
			{
				face.HandleOralOrgasmGeneric(false);
				if (countTowardOrgasmTotal)
				{
					Orgasmed();
				}
			}
		}

		#endregion
		#region Feet Sex - Take
		//public void TakeFootLicking()
		//{
		//	feet.GetLicked();
		//}
		#endregion
		#region Feet-Sex - Give
		public void PenetrateSomethingWithFeet()
		{
			feet.DoPenetrate();
		}

		public void RubSomethingWithFeet()
		{
			feet.DoRubbing();
		}
		#endregion

		#endregion

		public string UpperBodyArmorShort(bool both = true)
		{
			if (wearingArmor && both && wearingUpperGarment)
			{
				return armor.ItemName() + " and " + upperGarment.ItemName();
			}
			else if (wearingArmor)
			{
				return armor.ItemName();
			}
			else if (wearingUpperGarment)
			{
				return upperGarment.ItemName();
			}
			return null;
		}


		public string UpperBodyArmorTextHelper(string armorAndUpperGarmentText, string armorText, string upperGarmentText, string nakedText)
		{
			if (wearingArmor && wearingUpperGarment)
			{
				return armorAndUpperGarmentText;
			}
			else if (wearingArmor)
			{
				return armorText;
			}
			else if (wearingUpperGarment)
			{
				return upperGarmentText;
			}
			else
			{
				return nakedText;
			}
		}

		public string UpperBodyArmorTextHelper(string armorText, string upperGarmentText, string nakedText)
		{
			if (wearingArmor)
			{
				return armorText;
			}
			else if (wearingUpperGarment)
			{
				return upperGarmentText;
			}
			else
			{
				return nakedText;
			}
		}

		public string LowerBodyArmorShort(bool both = true)
		{
			if (wearingArmor && both && wearingLowerGarment)
			{
				return armor.ItemName() + " and " + lowerGarment.ItemName();
			}
			else if (wearingArmor)
			{
				return armor.ItemName();
			}
			else if (wearingLowerGarment)
			{
				return lowerGarment.ItemName();
			}
			return null;
		}

		public string LowerBodyArmorTextHelper(string armorAndLowerGarmentText, string armorText, string lowerGarmentText, string nakedText)
		{
			if (wearingArmor && wearingLowerGarment)
			{
				return armorAndLowerGarmentText;
			}
			else if (wearingArmor)
			{
				return armorText;
			}
			else if (wearingLowerGarment)
			{
				return lowerGarmentText;
			}
			else
			{
				return nakedText;
			}
		}

		public string LowerBodyArmorTextHelper(string armorText, string lowerGarmentText, string nakedText)
		{
			if (wearingArmor)
			{
				return armorText;
			}
			else if (wearingLowerGarment)
			{
				return lowerGarmentText;
			}
			else
			{
				return nakedText;
			}
		}

		public string ClothingOrNakedTextHelper(string clothingText, string nakedText)
		{
			if (wearingAnything)
			{
				return clothingText;
			}
			else
			{
				return nakedText;
			}
		}

		//everything that modifies data within a body part is supposed to be internal, and then called from here. this way, we can debug eaiser, while still making it somewhat intuitive for non-programmers.
		//we also do this so that we can correctly handle the data, without the frontend devs having to lookup functions and such. for example, we can create a single function for sexual intercourse
		//that calls the genitals and womb classes here, instead of forcing the dev to call multiple functions, and worry about sharing the data - we'll automate that process. It also lets us hook up events
		//and proc them here, so we don't have to worry about doing that in the body parts themselves. The downside is it's more work for us here in the backend and turns this class into a several thousand line
		//long monstrosity. Side note: I'm aware this is how code works on large projects, but dear God do I hate large code files.
		#region Body Part Update/Restore and related event handlers

		#region Antennae
		public bool UpdateAntennae(AntennaeType antennaeType)
		{
			if (antennaeType == null)
			{
				throw new ArgumentNullException(nameof(antennaeType));
			}

			return antennae.UpdateType(antennaeType);
		}

		public bool RestoreAntennae()
		{
			return antennae.Restore();
		}
		#endregion
		#region Arms
		//arms are weird b/c hands. technically it's own class, so we'll let you subscribe to it, but it may not change whenever armType does.
		public bool UpdateArms(ArmType armType)
		{
			if (armType == null)
			{
				throw new ArgumentNullException(nameof(armType));
			}

			return arms.UpdateType(armType);
		}

		public bool RestoreArms()
		{
			return arms.Restore();
		}
		#endregion
		#region Back


		public bool UpdateBack(BackType backType)
		{
			if (backType == null)
			{
				throw new ArgumentNullException(nameof(backType));
			}

			if (backType is DragonBackMane dragonBack)
			{
				return back.UpdateType(dragonBack, hair.hairColor);
			}
			else
			{
				return back.UpdateType(backType);
			}
		}

		public bool UpdateBack(DragonBackMane dragonMane, HairFurColors maneColor)
		{
			return back.UpdateType(dragonMane, maneColor);
		}

		public bool RestoreBack()
		{
			return back.Restore();
		}
		#endregion
		#region Body

		public bool UpdateBody(BodyType bodyType)
		{
			return body.UpdateType(bodyType);
		}

		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return body.UpdateBody(cockatriceBodyType, featherTexture, scaleTexture);
		}
		public bool UpdateBody(CockatriceBodyType cockatriceBodyType, FurColor featherColor, Tones scaleTone, FurTexture? featherTexture = null, SkinTexture? scaleTexture = null)
		{
			return body.UpdateBody(cockatriceBodyType, featherColor, scaleTone, featherTexture, scaleTexture);
		}

		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return body.UpdateBody(kitsuneBodyType, skinTexture);
		}
		public bool UpdateBody(KitsuneBodyType kitsuneBodyType, Tones skinTone, FurColor furColor, SkinTexture? skinTexture = null, FurTexture? furTexture = null)
		{
			return body.UpdateBody(kitsuneBodyType, skinTone, furColor, skinTexture);
		}

		public bool UpdateBody(SimpleFurBodyType furryType, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furTexture);
		}
		public bool UpdateBody(SimpleFurBodyType furryType, FurColor furColor, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furColor, furTexture);
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, furTexture);
		}
		public bool UpdateBody(CompoundFurBodyType furryType, FurColor mainFurColor, FurTexture? furTexture = null)
		{
			return body.UpdateBody(furryType, mainFurColor, furTexture);
		}

		public bool UpdateBody(CompoundFurBodyType furryType, FurColor primaryColor, FurColor secondaryColor, FurTexture? primaryTexture = null, FurTexture? secondaryTexture = null)
		{
			return body.UpdateBody(furryType, primaryColor, secondaryColor, primaryTexture);
		}

		public bool UpdateBody(CompoundToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, toneTexture);
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, primaryColor, toneTexture);
		}
		public bool UpdateBody(CompoundToneBodyType toneType, Tones primaryColor, Tones secondaryColor, SkinTexture? primaryTexture = null, SkinTexture? secondaryTexture = null)
		{
			return body.UpdateBody(toneType, primaryColor, secondaryColor, primaryTexture);
		}

		public bool UpdateBody(SimpleToneBodyType toneType, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, toneTexture);
		}
		public bool UpdateBody(SimpleToneBodyType toneType, Tones color, SkinTexture? toneTexture = null)
		{
			return body.UpdateBody(toneType, color, toneTexture);
		}

		public bool RestoreBody()
		{
			return body.Restore();
		}
		#endregion
		#region Ears


		public bool UpdateEars(EarType earType)
		{
			return ears.UpdateType(earType);
		}

		public bool RestoreEar()
		{
			return ears.Restore();
		}
		#endregion
		#region Eyes


		public bool UpdateEyes(EyeType newType)
		{
			return eyes.UpdateType(newType);
		}

		public bool RestoreEyes()
		{
			return eyes.Restore();
		}

		public void ResetEyes()
		{
			eyes.Reset();
		}
		#endregion
		#region Face

		public bool UpdateFace(FaceType faceType)
		{
			return face.UpdateType(faceType);
		}

		public bool UpdateFaceWithMorph(FaceType faceType, bool fullMorph)
		{
			return face.UpdateFaceWithMorph(faceType, fullMorph);
		}

		public bool UpdateFaceWithComplexion(FaceType faceType, SkinTexture complexion)
		{
			return face.UpdateFaceWithComplexion(faceType, complexion);
		}

		public bool UpdateFaceWithMorphAndComplexion(FaceType faceType, bool fullMorph, SkinTexture complexion)
		{
			return face.UpdateFaceWithMorphAndComplexion(faceType, fullMorph, complexion);
		}

		public bool RestoreFace()
		{
			return face.Restore();
		}

		public void ResetFace()
		{
			face.Reset();
		}

		#endregion
		#region Gills


		public bool UpdateGills(GillType gillType)
		{
			return gills.UpdateType(gillType);
		}

		public bool RestoreGills()
		{
			return gills.Restore();
		}
		#endregion
		#region Hair


		public bool UpdateHair(HairType hairType)
		{
			return hair.UpdateType(hairType);
		}

		//may want to create nicer versions of this, idk. i was getting tired of dealing with that shit lol.
		public bool UpdateHair(HairType newType, bool? setHairGrowthFlag = null, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null,
			double? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			return hair.UpdateType(newType, setHairGrowthFlag, newHairColor, newHighlightColor, newHairLength, newStyle, ignoreCanLengthenOrCut);
		}

		public bool RestoreHair()
		{
			return hair.Restore();
		}

		public void ResetHair()
		{
			hair.Reset();
		}

		#endregion
		#region Horns


		public bool UpdateHorns(HornType hornType)
		{
			return horns.UpdateType(hornType);
		}

		public bool UpdateHornsAndStrengthenTransform(HornType newType, byte byAmount, bool uniform = false)
		{
			return horns.UpdateAndStrengthenHorns(newType, byAmount, uniform);
		}

		public bool UpdateOrStrengthenHorns(HornType newType, bool uniform = false) => horns.UpdateOrStrengthenHornType(newType, uniform);

		public bool RestoreHorns()
		{
			return horns.Restore();
		}
		#endregion
		#region LowerBody



		public bool UpdateLowerBody(LowerBodyType lowerBodyType)
		{
			return lowerBody.UpdateType(lowerBodyType);
		}

		public bool RestoreLowerBody()
		{
			return lowerBody.Restore();
		}
		#endregion
		#region Neck


		public bool UpdateNeck(NeckType neckType)
		{
			return neck.UpdateType(neckType);
		}

		public bool UpdateTypeWithAddedLength(NeckType newType, byte additionalLength) => neck.UpdateTypeWithAddedLength(newType, additionalLength);

		public bool UpdateNeck(CockatriceNeck newType, HairFurColors featherColor) => neck.UpdateType(newType, featherColor);

		public bool RestoreNeck()
		{
			return neck.Restore();
		}
		#endregion
		#region Tail


		public bool UpdateTail(TailType tailType)
		{
			return tail.UpdateType(tailType);
		}

		public bool RestoreTail()
		{
			return tail.Restore();
		}
		#endregion
		#region Tongue


		public bool UpdateTongue(TongueType tongueType)
		{
			return tongue.UpdateType(tongueType);
		}

		public bool RestoreTongue()
		{
			return tongue.Restore();
		}
		#endregion
		#region Wings


		public bool UpdateWings(WingType wingType)
		{
			return wings.UpdateType(wingType);
		}

		public bool UpdateWingsAndChangeColor(FeatheredWings featheredWings, HairFurColors featherCol)
		{
			return wings.UpdateWingsAndChangeColor(featheredWings, featherCol);
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone)
		{
			return wings.UpdateWingsAndChangeColor(toneWings, tone);
		}

		public bool UpdateWingsAndChangeColor(TonableWings toneWings, Tones tone, Tones boneTone)
		{
			return wings.UpdateWingsAndChangeColor(toneWings, tone, boneTone);
		}

		public bool UpdateWingsForceSize(WingType wingType, bool large)
		{
			return wings.UpdateWingsForceSize(wingType, large);
		}

		public bool UpdateWingsForceSizeChangeColor(FeatheredWings featheredWings, HairFurColors featherColor, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(featheredWings, featherColor, large);
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, large);
		}

		public bool UpdateWingsForceSizeChangeColor(TonableWings toneWings, Tones wingTone, Tones wingBoneTone, bool large)
		{
			return wings.UpdateWingsForceSizeChangeColor(toneWings, wingTone, wingBoneTone, large);
		}

		public bool RestoreWings()
		{
			return wings.Restore();
		}
		#endregion

		#endregion

		#region Body Part Aliases and other useful Functions

		public bool isBiped => lowerBody.isBiped;
		public bool isMonoped => lowerBody.isMonoped;
		public bool isQuadruped => lowerBody.isQuadruped;

		#endregion

		/*#region Body Part Change Aliases

		//semantics: Set vs Change - Set will generally be void, as it'll just set it to the value given, though it can return a boolean if it may not be possible to set a value for this data based on other factors.
		//Change will always return a boolean, and it will only be true if the data actually changed. if the value given and the current data are the same, it will return false.

		#region Body

		#endregion
		#region Eyes
		public bool ChangeEyeColor(EyeColor color)
		{
			return eyes.ChangeEyeColor(color);
		}
		public bool ChangeEyeColor(EyeColor leftEye, EyeColor rightEye)
		{
			return eyes.ChangeEyeColor(leftEye, rightEye);
		}
		#endregion
		#region Face
		public bool StrengthenFacialMorph()
		{
			return face.StrengthenFacialMorph();
		}

		//can call restore. so we need to fire an event if it does.
		public bool WeakenFacialMorph(bool restoreIfAlreadyLevelOne = true)
		{
			return face.WeakenFacialMorph(restoreIfAlreadyLevelOne);
		}

		public bool ChangeFacialComplexion(SkinTexture newTexture)
		{
			return face.ChangeComplexion(newTexture);
		}
		#endregion
		#region Hair
		public bool SetHairColor(HairFurColors newHairColor, bool clearHighlights = false)
		{
			return hair.SetHairColor(newHairColor, clearHighlights);
		}

		public bool SetHairHighlightColor(HairFurColors newHighlightColor)
		{
			return hair.SetHighlightColor(newHighlightColor);
		}

		public bool SetBothHairColors(HairFurColors hairColor, HairFurColors highlightColor)
		{
			return hair.SetBothHairColors(hairColor, highlightColor);
		}

		public bool SetHairStyle(HairStyle newStyle)
		{
			return hair.SetHairStyle(newStyle);
		}

		public bool SetHairLength(double newLength)
		{
			return hair.SetHairLength(newLength);
		}

		public double GrowHair(double byAmount, bool ignoreCanLengthen = false)
		{
			return hair.GrowHair(byAmount, ignoreCanLengthen);
		}

		public double ShortenHair(double byAmount, bool ignoreCanCut = false)
		{
			return hair.ShortenHair(byAmount, ignoreCanCut);
		}
		#endregion
		#region Horns
		public bool StrengthenHornTransformation(byte numberOfTimes = 1)
		{
			return horns.StrengthenTransform(numberOfTimes);
		}

		//NOTE: this may cause you to lose your horns!. hence we do the event shit.
		public bool WeakenHornTransformation(byte numberOfTimes = 1)
		{
			return horns.WeakenTransform(numberOfTimes);
		}
		#endregion
		#region Neck
		internal byte GrowNeck(byte amount)
		{
			return neck.GrowNeck(amount
);
		}
		#endregion
		#region Tail
		public bool GrowAdditionalTail()
		{
			return tail.GrowAdditionalTail();
		}

		public byte GrowMultipleAdditionalTails(byte amount = 1)
		{
			return tail.GrowMultipleAdditionalTails(amount);
		}
		#endregion
		#region Wings
		internal bool GrowWingsToLarge()
		{
			return wings.GrowLarge();
		}

		internal bool ShrinkWingsToSmall()
		{
			return wings.ShrinkToSmall();
		}

		#endregion
		#endregion*/

		#region TimeListeners

		private protected readonly List<IBodyPartTimeLazy> lazyBodyListeners = new List<IBodyPartTimeLazy>();
		private protected readonly List<IBodyPartTimeActive> activeBodyListeners = new List<IBodyPartTimeActive>();
		private protected readonly List<IBodyPartTimeDaily> dailyBodyListeners = new List<IBodyPartTimeDaily>();
		private protected readonly List<IBodyPartTimeDayMulti> multiDailyBodyListeners = new List<IBodyPartTimeDayMulti>();

		public bool timeAware { get; private set; } = false;


		public void FreezeCreature()
		{
			timeAware = false;
			CreatureStore.markInactive(id);
		}

		public void UnFreezeCreature()
		{
			timeAware = true;
			CreatureStore.markActive(id, this);
		}

		private string QueryActiveBodyListenerData()
		{
			StringBuilder sb = new StringBuilder();
			activeBodyListeners.ForEach(x => sb.Append(x.reactToHourPassing(this is PlayerBase)));
			return sb.ToString();
		}

		private string QueryLazyBodyListenerData(byte hoursPassed)
		{
			StringBuilder sb = new StringBuilder();
			lazyBodyListeners.ForEach(x => sb.Append(x.reactToTimePassing(this is PlayerBase, hoursPassed)));
			return sb.ToString();
		}

		private string QueryDailyBodyListenerData(byte currentHour)
		{
			StringBuilder sb = new StringBuilder();
			dailyBodyListeners.ForEach(x => { if (x.hourToTrigger == currentHour) { sb.Append(x.reactToDailyTrigger(this is PlayerBase)); } });
			multiDailyBodyListeners.ForEach(x => { if (x.triggerHours.Contains(currentHour)) { sb.Append(x.reactToTrigger(this is PlayerBase, currentHour)); } });

			return sb.ToString();
		}

		internal IEnumerable<ITimeDailyListenerFull> QueryFullDailyListeners(byte currentHour)
		{
			return womb.GetDailyListeners().Where(x => x.hourToTrigger == currentHour);
		}

		internal IEnumerable<ITimeDailyListenerSimple> QuerySimpleDailyListeners(byte currentHour)
		{
			List<ITimeDailyListenerSimple> res = new List<ITimeDailyListenerSimple>();
			if (womb is ITimeDailyListenerSimple ds && ds.hourToTrigger == currentHour)
			{
				res.Add(ds);
			}
			return res;
		}

		internal IEnumerable<ITimeDayMultiListenerFull> QueryFullDayMultiListeners(byte currentHour)
		{
			return womb.GetDayMultiListeners().Where(x => x.triggerHours.Contains(currentHour));
		}

		internal IEnumerable<ITimeDayMultiListenerSimple> QuerySimpleDayMultiListeners(byte currentHour)
		{
			List<ITimeDayMultiListenerSimple> res = new List<ITimeDayMultiListenerSimple>()
			{
				this
			};
			return res;
		}

		internal IEnumerable<ITimeLazyListener> QueryLazyListeners()
		{
			return womb.GetLazyListeners().Union(new ITimeLazyListener[1] { this });
		}

		internal IEnumerable<ITimeActiveListenerFull> QueryFullActiveListeners()
		{
			return womb.GetActiveListeners();
		}

		internal IEnumerable<ITimeActiveListenerSimple> QuerySimpleActiveListeners()
		{
			List<ITimeActiveListenerSimple> res = new List<ITimeActiveListenerSimple>
			{
				this
			};
			return res;
		}


		string ITimeLazyListener.reactToTimePassing(byte hoursPassed)
		{
			return QueryLazyBodyListenerData(hoursPassed);
		}

		string ITimeActiveListenerSimple.reactToHourPassing()
		{
			return QueryActiveBodyListenerData();
		}

		byte[] ITimeDayMultiListenerSimple.triggerHours
		{
			get
			{
				return dailyBodyListeners.Select(x => x.hourToTrigger).Union(multiDailyBodyListeners.SelectMany(x => x.triggerHours)).Distinct().ToArray();
			}
		}


		string ITimeDayMultiListenerSimple.reactToTrigger(byte currHour)
		{
			return QueryDailyBodyListenerData(currHour);
		}



		#endregion

		#region Body Part Piercing Aliases and Event Helpers

		#endregion
		//public



	}
}
