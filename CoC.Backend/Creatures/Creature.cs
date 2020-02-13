﻿//Creature.cs
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
using CoC.Backend.Items.Wearables.UpperGarment;
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

		internal const byte BASE_MAX_LIBIDO = 100;
		internal const byte BASE_MAX_SENSITIVITY = 100;
		internal const byte BASE_MAX_CORRUPTION = 100;
		internal const byte BASE_MAX_LUST = 100;

		protected const byte ZERO = 0;
		public const byte LOWEST_POSSIBLE_MAX = 50;

		#region Stats
		public byte libido => (byte)Math.Floor(libidoTrue);
		public float libidoTrue
		{
			get => _libido;
			private protected set => _libido = Utils.Clamp2(value, minLibido, maxLibido);
		}
		private float _libido = 0;

		public float relativeLibido => libidoTrue * (100f / maxLibido);

		public byte sensitivity => (byte)Math.Floor(sensitivityTrue);
		public float sensitivityTrue
		{
			get => _sensitivity;
			private protected set => _sensitivity = Utils.Clamp2(value, minSensitivity, maxSensitivity);
		}
		private float _sensitivity = 0;

		public float relativeSensitivity => sensitivityTrue * (100f / maxSensitivity);

		public byte corruption => (byte)Math.Floor(corruptionTrue);
		public float corruptionTrue
		{
			get => _corruption;
			private protected set => _corruption = Utils.Clamp2(value, minCorruption, maxCorruption);
		}
		private float _corruption = 0;

		public float relativeCorruption => corruptionTrue * (100f / maxCorruption);

		public byte lust => (byte)Math.Floor(lustTrue);
		public float lustTrue
		{
			get => _lust;
			private protected set => _lust = Utils.Clamp2(value, minLust, maxLust);
		}
		private float _lust = 0;

		public float relativeLust => lustTrue * (100f / maxLust);

		protected internal virtual sbyte bonusMinLibido { get; set; }
		protected virtual byte baseMinLibido => 0;
		public byte minLibido => baseMinLibido.offset(bonusMinLibido);


		protected internal virtual sbyte bonusMinSensitivity { get; set; }
		protected virtual byte baseMinSensitivity => 0;
		public byte minSensitivity => baseMinSensitivity.offset(bonusMinSensitivity);


		protected internal virtual sbyte bonusMinCorruption { get; set; }
		protected virtual byte baseMinCorruption => 0;
		public byte minCorruption => baseMinCorruption.offset(bonusMinCorruption);


		protected internal virtual sbyte bonusMinLust { get; set; }
		protected virtual byte baseMinLust => 0;
		public byte minLust => baseMinLust.offset(bonusMinLust);

		protected internal virtual byte baseMaxLibido => BASE_MAX_LIBIDO;
		protected internal virtual sbyte bonusMaxLibido { get; set; } = 0;
		public byte maxLibido => HandleMaxStat(baseMaxLibido.offset(bonusMaxLibido), minLibido);

		protected internal virtual byte baseMaxSensitivity => BASE_MAX_SENSITIVITY;
		protected internal virtual sbyte bonusMaxSensitivity { get; set; } = 0;
		public byte maxSensitivity => HandleMaxStat(baseMaxSensitivity.offset(bonusMaxSensitivity), minSensitivity);

		protected internal virtual byte baseMaxCorruption => BASE_MAX_CORRUPTION;
		protected internal virtual sbyte bonusMaxCorruption { get; set; } = 0;
		public byte maxCorruption => HandleMaxStat(baseMaxCorruption.offset(bonusMaxCorruption), minCorruption);

		protected internal virtual byte baseMaxLust => BASE_MAX_LUST;
		protected internal virtual sbyte bonusMaxLust { get; set; } = 0;
		public byte maxLust => HandleMaxStat(baseMaxLust.offset(bonusMaxLust), minLust);

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

		protected internal float LibidoGainMultiplier = 1.0f;
		protected internal float LibidoLossMultiplier = 1.0f;

		protected internal float SensitivityGainMultiplier = 1.0f;
		protected internal float SensitivityLossMultiplier = 1.0f;

		protected internal float CorruptionGainMultiplier = 1.0f;
		protected internal float CorruptionLossMultiplier = 1.0f;

		protected internal float LustGainMultiplier = 1.0f;
		protected internal float LustLossMultiplier = 1.0f;
		#endregion

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

		public bool wearingArmor => armor != null;
		public bool wearingUpperGarment => upperGarment != null;
		public bool wearingLowerGarment => lowerGarment != null;
		public bool wearingAnything => armor != null || upperGarment != null || lowerGarment != null;

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
		public float IncreaseLibidoBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = libidoTrue;
			float offset = percent * maxLibido;
			if (!ignorePerks)
			{
				offset *= LibidoGainMultiplier;
			}
			libidoTrue += offset;
			return libidoTrue - oldValue;
		}



		public float IncreaseLibido(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= LibidoGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = libidoTrue;
			libidoTrue += amount;
			return libidoTrue - oldValue;
		}
		public float DecreaseLibidoBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = libidoTrue;
			float offset = percent * maxLibido;
			if (!ignorePerks)
			{
				offset *= LibidoLossMultiplier;
			}
			libidoTrue -= offset;
			return oldValue - libidoTrue;
		}

		public float DecreaseLibido(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= LibidoGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = libidoTrue;
			libidoTrue -= amount;
			return oldValue - libidoTrue;
		}
		public float SetLibidoPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxLibido * percent;
			libidoTrue = value;
			return libidoTrue;
		}

		public float SetLibido(byte value)
		{
			libidoTrue = value;
			return libidoTrue;
		}

		public float IncreaseSensitivityBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = sensitivityTrue;
			float offset = percent * maxSensitivity;
			if (!ignorePerks)
			{
				offset *= SensitivityGainMultiplier;
			}
			sensitivityTrue += offset;
			return sensitivityTrue - oldValue;
		}

		public float IncreaseSensitivity(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= SensitivityGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = sensitivityTrue;
			sensitivityTrue += amount;
			return sensitivityTrue - oldValue;
		}
		public float DecreaseSensitivityBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = sensitivityTrue;
			float offset = percent * maxSensitivity;
			if (!ignorePerks)
			{
				offset *= SensitivityLossMultiplier;
			}
			sensitivityTrue -= offset;
			return oldValue - sensitivityTrue;
		}

		public float DecreaseSensitivity(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= SensitivityGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = sensitivityTrue;
			sensitivityTrue -= amount;
			return oldValue - sensitivityTrue;
		}
		public float SetSensitivityPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxSensitivity * percent;
			sensitivityTrue = value;
			return sensitivityTrue;
		}

		public float SetSensitivity(byte value)
		{
			sensitivityTrue = value;
			return sensitivityTrue;
		}

		public float IncreaseCorruptionBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = corruptionTrue;
			float offset = percent * maxCorruption;
			if (!ignorePerks)
			{
				offset *= CorruptionGainMultiplier;
			}
			corruptionTrue += offset;
			return corruptionTrue - oldValue;
		}

		public float IncreaseCorruption(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= CorruptionGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = corruptionTrue;
			corruptionTrue += amount;
			return corruptionTrue - oldValue;
		}
		public float DecreaseCorruptionBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = corruptionTrue;
			float offset = percent * maxCorruption;
			if (!ignorePerks)
			{
				offset *= CorruptionLossMultiplier;
			}
			corruptionTrue -= offset;
			return oldValue - corruptionTrue;
		}

		public float DecreaseCorruption(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= CorruptionGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = corruptionTrue;
			corruptionTrue -= amount;
			return oldValue - corruptionTrue;
		}
		public float SetCorruptionPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxCorruption * percent;
			corruptionTrue = value;
			return corruptionTrue;
		}

		public float SetCorruption(byte value)
		{
			corruptionTrue = value;
			return corruptionTrue;
		}

		public float IncreaseLustBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = lustTrue;
			float offset = percent * maxLust;
			if (!ignorePerks)
			{
				offset *= LustGainMultiplier;
			}
			lustTrue += offset;
			return lustTrue - oldValue;
		}

		public float IncreaseLust(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= LustGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = lustTrue;
			lustTrue += amount;
			return lustTrue - oldValue;
		}
		public float DecreaseLustBy(float percent, bool ignorePerks = true)
		{
			Utils.Clamp(ref percent, 0, 1);
			float oldValue = lustTrue;
			float offset = percent * maxLust;
			if (!ignorePerks)
			{
				offset *= LustLossMultiplier;
			}
			lustTrue -= offset;
			return oldValue - lustTrue;
		}

		public float DecreaseLust(float amount = 1, bool ignorePerks = false)
		{
			if (!ignorePerks)
			{
				amount *= LustGainMultiplier;
			}
			if (amount <= 0)
			{
				return 0;
			}
			float oldValue = lustTrue;
			lustTrue -= amount;
			return oldValue - lustTrue;
		}
		public float SetLustPercent(float percent)
		{
			Utils.Clamp(ref percent, 0, 1);
			float value = maxLust * percent;
			lustTrue = value;
			return lustTrue;
		}

		public float SetLust(byte value)
		{
			lustTrue = value;
			return lustTrue;
		}

		public void IncreaseCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, bool ignorePerks = false)
		{
			float amount;
			if (lus != 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= LustGainMultiplier;
				}

				lustTrue += amount;
			}
			if (lib != 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= LibidoGainMultiplier;
				}

				libidoTrue += amount;
			}
			if (sens != 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= SensitivityGainMultiplier;
				}

				sensitivityTrue += amount;
			}
			if (corr != 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= CorruptionGainMultiplier;
				}

				corruptionTrue += amount;
			}
		}

		public void DecreaseCreatureStats(byte lus = 0, byte lib = 0, byte sens = 0, byte corr = 0, bool ignorePerks = false)
		{
			float amount;
			if (lus != 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= LustLossMultiplier;
				}

				lustTrue -= amount;
			}
			if (lib != 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= LibidoLossMultiplier;
				}

				libidoTrue -= amount;
			}
			if (sens != 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= SensitivityLossMultiplier;
				}

				sensitivityTrue -= amount;
			}
			if (corr != 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= CorruptionLossMultiplier;
				}

				corruptionTrue -= amount;
			}
		}

		public void SetCreatureStats(byte? lus = null, byte? lib = null, byte? sens = null, byte? corr = null)
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
		}

		public void DeltaCreatureStats(float lus = 0, float lib = 0, float sens = 0, float corr = 0, bool ignorePerks = false)
		{
			float amount;
			if (lus < 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= LustLossMultiplier;
				}

				lustTrue += amount;
			}
			else if (lus > 0)
			{
				amount = lus;
				if (!ignorePerks)
				{
					amount *= LustGainMultiplier;
				}

				lustTrue += amount;
			}
			if (lib < 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= LibidoLossMultiplier;
				}

				libidoTrue += amount;
			}
			else if (lib > 0)
			{
				amount = lib;
				if (!ignorePerks)
				{
					amount *= LibidoGainMultiplier;
				}

				libidoTrue += amount;
			}
			if (sens < 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= SensitivityLossMultiplier;
				}

				sensitivityTrue += amount;
			}
			else if (sens > 0)
			{
				amount = sens;
				if (!ignorePerks)
				{
					amount *= SensitivityGainMultiplier;
				}

				sensitivityTrue += amount;
			}
			if (corr < 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= CorruptionLossMultiplier;
				}

				corruptionTrue += amount;
			}
			else if (corr > 0)
			{
				amount = corr;
				if (!ignorePerks)
				{
					amount *= CorruptionGainMultiplier;
				}

				corruptionTrue += amount;
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
		public bool AddCock(CockType newCockType)
		{
			return genitals.AddCock(newCockType);
		}

		public bool AddCock(CockType newCockType, float length, float girth, float? knotMultiplier = null)
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

		public bool AddVagina(float clitLength) => genitals.AddVagina(clitLength);
		public bool AddVagina(VaginaType newVaginaType, float clitLength)
		{
			return genitals.AddVagina(newVaginaType, clitLength);
		}

		public bool AddVagina(float clitLength, VaginalLooseness looseness, VaginalWetness wetness) => genitals.AddVagina(clitLength, looseness, wetness);
		public bool AddVagina(VaginaType newVaginaType, float clitLength, VaginalLooseness looseness, VaginalWetness wetness)
		{
			return genitals.AddVagina(newVaginaType, clitLength, looseness, wetness);
		}

		public int RemoveVagina(int count = 1)
		{
			return genitals.RemoveVagina(count);
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


		internal ArmorBase ChangeArmor(ArmorBase armorBase, out string removeText)
		{
			ArmorBase retVal = armor;
			armor = armorBase;
			retVal?.OnRemove(this);
			removeText = retVal?.OnRemoveText();
			if (retVal?.destroyOnRemoval == true)
			{
				retVal = null;
			}
			return retVal;
		}

		internal UpperGarmentBase ChangeUpperGarment(UpperGarmentBase upperGarmentBase, out string removeText)
		{
			UpperGarmentBase retVal = upperGarment;
			upperGarment = upperGarmentBase;
			retVal?.OnRemove(this);
			removeText = retVal?.OnRemoveText();
			if (retVal?.destroyOnRemoval == true)
			{
				retVal = null;
			}
			return retVal;
		}

		internal LowerGarmentBase ChangeLowerGarment(LowerGarmentBase lowerGarmentBase, out string removeText)
		{
			LowerGarmentBase retVal = lowerGarment;
			lowerGarment = lowerGarmentBase;
			retVal?.OnRemove(this);
			removeText = retVal?.OnRemoveText();
			if (retVal?.destroyOnRemoval == true)
			{
				retVal = null;
			}
			return retVal;
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
				return item.AttemptToUse(this, onUseItemReturn);
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
				return item.AttemptToUse(this, onUseItemReturn);
			}
		}

		public DisplayBase EquipArmorManual(ArmorBase armor, UseItemCallbackSafe<ArmorBase> postEquipCallback)
		{
			if (armor is null)
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
				return armor.AttemptToUseSafe(this, postEquipCallback);
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
				return item.AttemptToUseSafe(this, postEquipCallback);
			}
		}

		//remove the armor, retrieving it for you to manually parse. note that if the armor destroys itself when being removed, this will return null.
		public ArmorBase RemoveArmorManual(out string removeText)
		{
			return ChangeArmor(null, out removeText);
		}

		//remove the current armor, and replace it with the current one. since it may be possible for an armor equip to use a menu, the removed armor is sent along to the callback.
		//you will need to manually parse the display returned by this. if the removed armor destroys itself when being removed, the corresponding value passed along to the
		//callback will be null.
		public DisplayBase ReplaceArmorManual(ArmorBase armor, UseItemCallbackSafe<ArmorBase> postReplaceArmorCallback)
		{
			if (armor is null)
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
				return armor.AttemptToUseSafe(this, postReplaceArmorCallback);
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
				return item.AttemptToUseSafe(this, postReplaceArmorCallback);
			}
		}

		public DisplayBase EquipUpperGarmentManual(UpperGarmentBase upperGarment, UseItemCallbackSafe<UpperGarmentBase> postEquipCallback)
		{
			if (upperGarment is null)
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
				return upperGarment.AttemptToUseSafe(this, postEquipCallback);
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
				return item.AttemptToUseSafe(this, postEquipCallback);
			}
		}

		public UpperGarmentBase RemoveUpperGarmentManual(out string removeText)
		{
			return ChangeUpperGarment(null, out removeText);
		}

		public DisplayBase ReplaceUpperGarmentManual(UpperGarmentBase upperGarment, UseItemCallbackSafe<UpperGarmentBase> postReplaceUpperGarmentCallback)
		{
			if (upperGarment is null)
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
				return upperGarment.AttemptToUseSafe(this, postReplaceUpperGarmentCallback);
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
				return item.AttemptToUseSafe(this, postReplaceUpperGarmentCallback);
			}
		}

		public DisplayBase EquipLowerGarmentManual(LowerGarmentBase lowerGarment, UseItemCallbackSafe<LowerGarmentBase> postEquipCallback)
		{
			if (lowerGarment is null)
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
				return lowerGarment.AttemptToUseSafe(this, postEquipCallback);
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
				return item.AttemptToUseSafe(this, postEquipCallback);
			}
		}

		public LowerGarmentBase RemoveLowerGarmentManual(out string removeText)
		{
			return ChangeLowerGarment(null, out removeText);
		}

		public DisplayBase ReplaceLowerGarmentManual(LowerGarmentBase lowerGarment, UseItemCallbackSafe<LowerGarmentBase> postReplaceLowerGarmentCallback)
		{
			if (lowerGarment is null)
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
				return lowerGarment.AttemptToUseSafe(this, postReplaceLowerGarmentCallback);
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
				return item.AttemptToUseSafe(this, postReplaceLowerGarmentCallback);
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
		/*
		#region Take Anal




		public bool TakeAnalPenetration(float length, float girth, float knotWidth, StandardSpawnType knockupType, float cumAmount, byte virilityBonus, bool countAsSex,
			bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleAnalPenetration(length, girth, knotWidth, knockupType, cumAmount, virilityBonus, countAsSex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public void TakeAnalPenetrationNoKnockup(float length, float girth, float knotWidth, float cumAmount, bool countAsSex, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleAnalPenetration(length, girth, knotWidth, cumAmount, countAsSex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public bool TakeAnalSex(Cock source, StandardSpawnType knockupType, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleAnalPenetration(source, knockupType, cumAmountOverride, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeAnalSex(Cock source, StandardSpawnType knockupType, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleAnalPenetration(source, knockupType, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool AttemptManualAnalKnockup(StandardSpawnType knockupType, float knockupRate)
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

		public void PenetrateAButtGeneric()
		{
			TimesFuckedAnotherAss++;
		}
		#endregion
		#region Take Vaginal

		public bool TakeVaginalPenetrationNoKnockup(int vaginaIndex, float length, float girth, float knotWidth, float cumAmount, bool countAsSex, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, cumAmount, countAsSex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}
		public bool TakeVaginalPenetration(int vaginaIndex, float length, float girth, float knotWidth, StandardSpawnType knockupType, float cumAmount, byte virilityBonus, bool countAsSex,
			bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, knockupType, cumAmount, virilityBonus, countAsSex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeVaginalSex(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool TakeVaginalSex(int vaginaIndex, Cock sourceCock, StandardSpawnType knockupType, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, cumAmountOverride, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
			return retVal;
		}

		public bool AttemptManualVaginalKnockup(int vaginaIndex, StandardSpawnType knockupType, float knockupRate)
		{
			return genitals.HandleVaginalPregnancyOverride(vaginaIndex, knockupType, knockupRate);
		}
		*/
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
		/*
		#endregion
		#region Give Vaginal
		public uint TimesFuckedAnotherVagina { get; private set; } = 0;
		public uint TimesSoundedAnotherClitCock { get; private set; } = 0;

		public void PenetrateAPussyGeneric()
		{
			TimesFuckedAnotherVagina++;
		}

		public void SoundAnotherClitCockGeneric()
		{
			TimesSoundedAnotherClitCock++;
		}


		#endregion
		#region Vaginal Penetrates

		internal void PenetrateSomethingWithAClit(int vaginaIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleClitPenetrate(vaginaIndex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Take Tit-Sex
		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		public void GetTittyFucked(int breastIndex, Cock sourceCock, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleTittyFuck(breastIndex, sourceCock, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void GetTittyFucked(int breastIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleTittyFuck(breastIndex, sourceCock, cumAmountOverride, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void GetTittyFucked(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleTittyFuck(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm);
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

		public void TakeNipplePenetration(int breastIndex, Cock sourceCock, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleNipplePenetration(breastIndex, sourceCock, reachOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeNipplePenetration(int breastIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleNipplePenetration(breastIndex, sourceCock, cumAmountOverride, reachOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeNipplePenetration(int breastIndex, float length, float girth, float knotWidth, float cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleNipplePenetration(breastIndex, length, girth, knotWidth, cumAmount, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		#endregion
		#region Penetrate With Tits
		public void PenetrateSomethingWithNippleDicks(int breastIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleNippleDickPenetrate(breastIndex, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Give Tit-Sex
		public uint TimesFuckedSomeTitties { get; private set; } = 0;
		public uint TimesFuckedANipple { get; private set; } = 0;

		public void PenetrateANippleGeneric()
		{
			TimesFuckedANipple++;
		}

		public void DoATitFuckGeneric()
		{
			TimesFuckedSomeTitties++;
		}
		#endregion
		#region Take with Cock
		public void TakeCockSounding(int cockIndex, float penetratorLength, float penetratorWidth, float knotSize, float cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
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

		public void TakeCockSounding(int cockIndex, Cock sourceCock, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleCockSounding(cockIndex, sourceCock, cumAmountOverride, reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Give Cock Sex
		public void PenetrateWithCockGeneric(int cockIndex, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			genitals.HandleCockPenetrate(cockIndex, reachOrgasm);
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

		public void TakeOralPenetration(float penetratorArea, float knotWidth, int cumAmount, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			face.HandleOralPenetration(penetratorArea, knotWidth, cumAmount, reachOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeOralSex(Cock penetrator, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			face.HandleOralPenetration(penetrator, reachOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void TakeOralSex(Cock penetrator, float cumAmountOverride, bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			face.HandleOralPenetration(penetrator, cumAmountOverride, reachOrgasm);
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
		public void TakeFootLicking(bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			feet.GetLicked(reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		#region Feet-Sex - Give
		public void PenetrateSomethingWithFeet(bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			feet.DoPenetrate(reachOrgasm);

			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void RubSomethingWithFeet(bool reachOrgasm, bool countTowardOrgasmTotal)
		{
			feet.DoRubbing(reachOrgasm);
			if (reachOrgasm && countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}

		public void HaveGenericFootOrgasm(bool dryOrgasm, bool countTowardOrgasmTotal)
		{
			feet.DoGenericOrgasm(dryOrgasm);
			if (countTowardOrgasmTotal)
			{
				Orgasmed();
			}
		}
		#endregion
		*/
		#endregion
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
			float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
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

		public bool SetHairLength(float newLength)
		{
			return hair.SetHairLength(newLength);
		}

		public float GrowHair(float byAmount, bool ignoreCanLengthen = false)
		{
			return hair.GrowHair(byAmount, ignoreCanLengthen);
		}

		public float ShortenHair(float byAmount, bool ignoreCanCut = false)
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
