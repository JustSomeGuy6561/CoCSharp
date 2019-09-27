//Creature.cs
//Description:
//Author: JustSomeGuy
//2/20/2019, 4:13 PM
using CoC.Backend.BodyParts;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Perks;
using CoC.Backend.Pregnancies;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CoC.Backend.Creatures
{
	//creature class breaks with non-standard creatures - it's generall 
	public enum CreatureType { STANDARD, PRIMITIVE, ARTIFICIAL }

	public abstract class Creature
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

		public byte sensitivity => (byte)Math.Floor(sensitivityTrue);
		public float sensitivityTrue
		{
			get => _sensitivity;
			private protected set => _sensitivity = Utils.Clamp2(value, minSensitivity, maxSensitivity);
		}
		private float _sensitivity = 0;

		public byte corruption => (byte)Math.Floor(corruptionTrue);
		public float corruptionTrue
		{
			get => _corruption;
			private protected set => _corruption = Utils.Clamp2(value, minCorruption, maxCorruption);
		}
		private float _corruption = 0;

		public byte lust => (byte)Math.Floor(lustTrue);
		public float lustTrue
		{
			get => _lust;
			private protected set => _lust = Utils.Clamp2(value, minLust, maxLust);
		}
		private float _lust = 0;

		protected internal virtual sbyte bonusMinLibido { get; set; }
		protected virtual byte baseMinLibido => 0;
		public byte minLibido => baseMinLibido.delta(bonusMinLibido);


		protected internal virtual sbyte bonusMinSensitivity { get; set; }
		protected virtual byte baseMinSensitivity => 0;
		public byte minSensitivity => baseMinSensitivity.delta(bonusMinSensitivity);


		protected internal virtual sbyte bonusMinCorruption { get; set; }
		protected virtual byte baseMinCorruption => 0;
		public byte minCorruption => baseMinCorruption.delta(bonusMinCorruption);


		protected internal virtual sbyte bonusMinLust { get; set; }
		protected virtual byte baseMinLust => 0;
		public byte minLust => baseMinLust.delta(bonusMinLust);

		protected internal virtual byte baseMaxLibido => BASE_MAX_LIBIDO;
		protected internal virtual sbyte bonusMaxLibido { get; set; } = 0;
		public byte maxLibido => HandleMaxStat(baseMaxLibido.delta(bonusMaxLibido), minLibido);

		protected internal virtual byte baseMaxSensitivity => BASE_MAX_SENSITIVITY;
		protected internal virtual sbyte bonusMaxSensitivity { get; set; } = 0;
		public byte maxSensitivity => HandleMaxStat(baseMaxSensitivity.delta(bonusMaxSensitivity), minSensitivity);

		protected internal virtual byte baseMaxCorruption => BASE_MAX_CORRUPTION;
		protected internal virtual sbyte bonusMaxCorruption { get; set; } = 0;
		public byte maxCorruption => HandleMaxStat(baseMaxCorruption.delta(bonusMaxCorruption), minCorruption);

		protected internal virtual byte baseMaxLust => BASE_MAX_LUST;
		protected internal virtual sbyte bonusMaxLust { get; set; } = 0;
		public byte maxLust => HandleMaxStat(baseMaxLust.delta(bonusMaxLust), minLust);

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
			else return LOWEST_POSSIBLE_MAX;
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
		public readonly Tail tail;
		public readonly Tongue tongue;
		public readonly Wings wings;

		//aliases for build.
		public Butt butt => build.butt;
		public Hips hips => build.hips;

		//aliases for genitals.
		public Ass ass => genitals.ass;
		public ReadOnlyCollection<Breasts> breasts => genitals.breastRows;
		public ReadOnlyCollection<Cock> cocks => genitals.cocks;
		public ReadOnlyCollection<Vagina> vaginas => genitals.vaginas;
		public Balls balls => genitals.balls;

		public ReadOnlyCollection<Clit> clits => genitals.clits;
		public ReadOnlyCollection<Nipples> nipples => genitals.nipples;

		//aliases for arms/legs
		public Hands hands => arms.hands;
		public Feet feet => lowerBody.feet;

		public readonly PerkCollection perks;

		public Womb womb => genitals.womb;

		protected BasePerkModifiers modifiers => perks.baseModifiers;

		#region Constructors
		protected Creature(CreatureCreator creator)
		{
			if (creator == null) throw new ArgumentNullException();

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

			var womb = creator.wombMaker?.Invoke(this) ?? new GenericWomb(id);

			var cup = gender.HasFlag(Gender.FEMALE) ? Breasts.DEFAULT_FEMALE_SIZE : Breasts.DEFAULT_MALE_SIZE;

			var breasts = creator.breasts ?? new BreastCreator[] { new BreastCreator(cup) };

			switch (gender)
			{
				case Gender.GENDERLESS:
					genitals = new Genitals(id, ass, creator.breasts, null, new Balls(id, false), null, womb, creator.femininity, fertility); break;
				case Gender.MALE:

					var cocks = creator.cocks;
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
			//Tail

			if (creator.tailType is null)
			{
				tail = new Tail(id);
			}
			else if (creator.tailCount != null)
			{
				tail = new Tail(id, creator.tailType, (byte)creator.tailCount);
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

			perks = new PerkCollection(this);
			perks.InitPerks(creator.perks?.ToArray());

			DoPostPerkInit();
			DoLateInit();
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
		#endregion

		#region stat updates
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
			var oldValue = libidoTrue;
			libidoTrue += amount;
			return libidoTrue - oldValue;
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
			var oldValue = libidoTrue;
			libidoTrue -= amount;
			return oldValue - libidoTrue;
		}
		public float SetLibido(byte value)
		{
			libidoTrue = value;
			return libidoTrue;
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
			var oldValue = sensitivityTrue;
			sensitivityTrue += amount;
			return sensitivityTrue - oldValue;
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
			var oldValue = sensitivityTrue;
			sensitivityTrue -= amount;
			return oldValue - sensitivityTrue;
		}
		public float SetSensitivity(byte value)
		{
			sensitivityTrue = value;
			return sensitivityTrue;
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
			var oldValue = corruptionTrue;
			corruptionTrue += amount;
			return corruptionTrue - oldValue;
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
			var oldValue = corruptionTrue;
			corruptionTrue -= amount;
			return oldValue - corruptionTrue;
		}
		public float SetCorruption(byte value)
		{
			corruptionTrue = value;
			return corruptionTrue;
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
			var oldValue = lustTrue;
			lustTrue += amount;
			return lustTrue - oldValue;
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
			var oldValue = lustTrue;
			lustTrue -= amount;
			return oldValue - lustTrue;
		}
		public float SetLust(byte value)
		{
			lustTrue = value;
			return lustTrue;
		}


		#endregion

		/// <summary>
		/// Generic sexing for vagina, allows creature to be sexed and possibly knocked up even if the source isn't another creature.
		/// </summary>
		/// <param name="vaginaIndex">index of the vagina that is being penetrated.</param>
		/// <param name="length">length of the penetrator</param>
		/// <param name="girth">girth of the penetrator</param>
		/// <param name="knotWidth">any width of a knot if the penetrator has one. negative and 0 are treated as no knot.</param>
		/// <param name="knockupType">type of spawn this penetrator will create if the creature is successfully knocked up</param>
		/// <param name="cumAmount">the amount of cum used to knockup the player. affects knockup chance.</param>
		/// <param name="reachOrgasm">whether or not this creature reaches orgasm as a result of this penetration</param>
		/// <returns>true if the creature ends up pregnant, false otherwise.</returns>
		public bool GetVaginallyPenetrated(int vaginaIndex, float length, float girth, float knotWidth, SpawnType knockupType, byte bonusVirility, bool reachOrgasm = true)
		{
			return genitals.HandleVaginalPenetration(vaginaIndex, length, girth, knotWidth, knockupType, bonusVirility, reachOrgasm);
		}

		public bool GetAnallyPenetrated(float length, float girth, float knotWidth, SpawnType knockupType, byte bonusVirility, bool reachOrgasm = true)
		{
			return genitals.HandleAnalPenetration(length, girth, knotWidth, knockupType, bonusVirility, reachOrgasm);
		}

		public bool GetVaginallyPenetrated(int vaginaIndex, Creature penetrator, int penetratorCockIndex, SpawnType knockupType, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.cocks[penetratorCockIndex];
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, reachOrgasm);
			penetrator.genitals.HandleCockPenetrate(penetratorCockIndex, penetratorReachesOrgasm);
			return retVal;
		}

		public bool GetAnallyPenetrated(Creature penetrator, int penetratorCockIndex, SpawnType knockupType, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.cocks[penetratorCockIndex];
			bool retVal = genitals.HandleAnalPenetration(sourceCock, knockupType, reachOrgasm);
			penetrator.genitals.HandleCockPenetrate(penetratorCockIndex, penetratorReachesOrgasm);
			return retVal;
		}

		public bool GetVaginallyPenetratedWithClitCock(int vaginaIndex, Creature penetrator, int penetratorVaginaIndex, SpawnType knockupType, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.vaginas[penetratorVaginaIndex].clit.AsClitCock();
			bool retVal = genitals.HandleVaginalPenetration(vaginaIndex, sourceCock, knockupType, reachOrgasm);
			penetrator.genitals.HandleClitCockPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
			return retVal;
		}

		public bool GetAnallyPenetratedWithClitCock(Creature penetrator, int penetratorVaginaIndex, SpawnType knockupType, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.vaginas[penetratorVaginaIndex].clit.AsClitCock();
			bool retVal = genitals.HandleAnalPenetration(sourceCock, knockupType, reachOrgasm);
			penetrator.genitals.HandleClitCockPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
			return retVal;
		}

		public void GetVaginallyPenetratedWithClit(int vaginaIndex, Creature penetrator, int penetratorVaginaIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			float length = penetrator.vaginas[penetratorVaginaIndex].clit.length;
			genitals.HandleVaginalPenetration(vaginaIndex, length, 1, 0, null, 0, reachOrgasm);
			penetrator.genitals.HandleClitPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
		}

		public void GetAnallyPenetratedWithClit(Creature penetrator, int penetratorVaginaIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			float length = penetrator.vaginas[penetratorVaginaIndex].clit.length;
			genitals.HandleAnalPenetration(length, 1, 0, null, 0, reachOrgasm);
			penetrator.genitals.HandleClitPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
		}

		public void GetVaginallyPenetratedWithNippleDicks(int vaginaIndex, Creature penetrator, int penetratorBreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Nipples source = penetrator.breasts[penetratorBreastIndex].nipples;
			genitals.HandleVaginalPenetration(vaginaIndex, source.length, source.width, 0, null, 0, reachOrgasm);
			penetrator.genitals.HandleNippleDickPenetrate(penetratorBreastIndex, penetratorReachesOrgasm);
		}

		public void GetAnallyPenetratedWithNippleDicks(Creature penetrator, int penetratorBreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Nipples source = penetrator.breasts[penetratorBreastIndex].nipples;
			genitals.HandleAnalPenetration(source.length, source.width, 0, null, 0, reachOrgasm);
			penetrator.genitals.HandleNippleDickPenetrate(penetratorBreastIndex, penetratorReachesOrgasm);
		}

		public void GetCockSounded(int cockIndex, float penetratorLength, float penetratorWidth, bool reachOrgasm = true)
		{
			genitals.HandleCockSounding(cockIndex, penetratorLength, penetratorWidth, reachOrgasm);
		}

		public void GetCockSoundedWithAClit(int cockIndex, Creature penetrator,  int penetratorVaginaIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			float length = penetrator.vaginas[penetratorVaginaIndex].clit.length;
			genitals.HandleCockSounding(cockIndex, length, 1, reachOrgasm);
			penetrator.genitals.HandleClitPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
		}

		public void GetCockSoundedWithAClitCock(int cockIndex, Creature penetrator, int penetratorVaginaIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock source = penetrator.vaginas[penetratorVaginaIndex].clit.AsClitCock();
			genitals.HandleCockSounding(cockIndex, source.length, source.girth, reachOrgasm);
			penetrator.genitals.HandleClitCockPenetrate(penetratorVaginaIndex, penetratorReachesOrgasm);
		}

		public void GetCockSoundedWithACock(int cockIndex, Creature penetrator, int penetratorCockIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock source = penetrator.cocks[penetratorCockIndex];
			genitals.HandleCockSounding(cockIndex, source.length, source.girth, reachOrgasm);
			penetrator.genitals.HandleCockPenetrate(penetratorCockIndex, penetratorReachesOrgasm);
		}

		public void GetCockSoundedWithNippleDicks(int cockIndex, Creature penetrator, int penetratorBreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Nipples source = penetrator.breasts[penetratorBreastIndex].nipples;
			genitals.HandleCockSounding(cockIndex, source.length, source.width, reachOrgasm);
			penetrator.genitals.HandleNippleDickPenetrate(penetratorBreastIndex, penetratorReachesOrgasm);
		}

		public void GetNippleFucked(int breastIndex, float length, float girth, float knotWidth, bool reachOrgasm = true)
		{
			genitals.HandleNipplePenetration(breastIndex, length, girth, knotWidth, reachOrgasm);
		}

		public void GetNippleFucked(int breastIndex, Creature penetrator, int penetratorCockIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.cocks[penetratorCockIndex];
			genitals.HandleNipplePenetration(breastIndex, sourceCock, reachOrgasm);
			penetrator.genitals.HandleCockPenetrate(penetratorCockIndex, penetratorReachesOrgasm);
		}

		public void GetNippleFuckedWithClitCock(int breastIndex, Creature penetrator, int penetratorbreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Cock sourceCock = penetrator.vaginas[penetratorbreastIndex].clit.AsClitCock();
			genitals.HandleNipplePenetration(breastIndex, sourceCock, reachOrgasm);
			penetrator.genitals.HandleClitCockPenetrate(penetratorbreastIndex, penetratorReachesOrgasm);
		}

		public void GetNippleFuckedWithClit(int breastIndex, Creature penetrator, int penetratorbreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			float length = penetrator.vaginas[penetratorbreastIndex].clit.length;
			genitals.HandleNipplePenetration(breastIndex, length, 1, 0, reachOrgasm);
			penetrator.genitals.HandleClitPenetrate(penetratorbreastIndex, penetratorReachesOrgasm);
		}

		public void GetNippleFuckedWithNippleDicks(int breastIndex, Creature penetrator, int penetratorBreastIndex, bool reachOrgasm = true, bool penetratorReachesOrgasm = true)
		{
			Nipples source = penetrator.breasts[penetratorBreastIndex].nipples;
			genitals.HandleNipplePenetration(breastIndex, source.length, source.width, 0, reachOrgasm);
			penetrator.genitals.HandleNippleDickPenetrate(penetratorBreastIndex, penetratorReachesOrgasm);
		}

		public void MasturbateCock(int index, bool reachOrgasm = true)
		{
			genitals.HandleCockPenetrate(index, reachOrgasm);
		}

		public void MasturbateClitCock(int index, bool reachOrgasm = true)
		{
			genitals.HandleClitCockPenetrate(index, reachOrgasm);
		}

		public void MasturbateFuckableNipples(int index, bool reachOrgasm = true)
		{

		}

		public void MasturbateDickNipples(int index, bool reachOrgasm = true)
		{

		}

		public void MasturbateVagina(int index, bool reachOrgasm = true)
		{

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
			if (antennaeType == null) throw new ArgumentNullException(nameof(antennaeType));
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
			if (armType == null) throw new ArgumentNullException(nameof(armType));
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
			if (backType == null) throw new ArgumentNullException(nameof(backType));
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


		public bool UpdateEar(EarType earType)
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
		public bool UpdateHair(HairType newType, HairFurColors newHairColor = null, HairFurColors newHighlightColor = null,
			float? newHairLength = null, HairStyle? newStyle = null, bool ignoreCanLengthenOrCut = false)
		{
			return hair.UpdateType(newType, newHairColor, newHighlightColor, newHairLength, newStyle, ignoreCanLengthenOrCut);
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

		public bool UpdateHornsAndStrengthenTransform(HornType newType, byte byAmount)
		{
			return horns.UpdateAndStrengthenHorns(newType, byAmount);
		}

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

		#region Body Part Piercing Aliases and Event Helpers

		#endregion
		#region Time Listeners

		private bool isPlayer => this is Player;

		#region "Anonymous" classes
		//C# isn't java, so we don't have anonymous classes. This is the closest we can achieve. YMMV on which is better. 
		//basically, we need to wrap all the body part events into something the game engine can handle. i'd do it all in creature, but 
		//there's the whole multipage mess to deal with. 
		private sealed class LazyWrapper : ITimeLazyListener
		{
			public readonly IBodyPartTimeLazy listener;
			private readonly bool isPlayer;

			public string reactToTimePassing(byte hoursPassed)
			{
				return listener.reactToTimePassing(isPlayer, hoursPassed);
			}

			public LazyWrapper(bool player, IBodyPartTimeLazy lazyMember)
			{
				isPlayer = player;
				listener = lazyMember;
			}
		}

		private sealed class ActiveWrapper : ITimeActiveListener
		{
			public readonly IBodyPartTimeActive listener;
			private readonly bool isPlayer;

			public EventWrapper reactToHourPassing()
			{
				return new EventWrapper(listener.reactToHourPassing(isPlayer));
			}

			public ActiveWrapper(bool player, IBodyPartTimeActive activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DailyWrapper : ITimeDailyListener
		{
			public readonly IBodyPartTimeDaily listener;
			private readonly bool isPlayer;

			public byte hourToTrigger => listener.hourToTrigger;

			public EventWrapper reactToDailyTrigger()
			{
				return new EventWrapper(listener.reactToDailyTrigger(isPlayer));
			}

			public DailyWrapper(bool player, IBodyPartTimeDaily activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}

		private sealed class DayMultiWrapper : ITimeDayMultiListener
		{
			public readonly IBodyPartTimeDayMulti listener;
			private readonly bool isPlayer;

			public byte[] triggerHours => listener.triggerHours;

			public EventWrapper reactToTrigger(byte currHour)
			{
				return new EventWrapper(listener.reactToTrigger(isPlayer, currHour));
			}

			public DayMultiWrapper(bool player, IBodyPartTimeDayMulti activeMember)
			{
				isPlayer = player;
				listener = activeMember;
			}
		}
		#endregion
		//we store a reference to all the listeners, bot the ones we use and the ones the game engine uses, so when we create or destroy this class, we don't "leak" events.

		private protected bool listenersActive { get; private set; } = false;
		private readonly Dictionary<IBodyPartTimeLazy, LazyWrapper> lazyListeners = new Dictionary<IBodyPartTimeLazy, LazyWrapper>();
		private readonly Dictionary<IBodyPartTimeActive, ActiveWrapper> activeListeners = new Dictionary<IBodyPartTimeActive, ActiveWrapper>();
		private readonly Dictionary<IBodyPartTimeDaily, DailyWrapper> dailyListeners = new Dictionary<IBodyPartTimeDaily, DailyWrapper>();
		private readonly Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper> dayMultiListeners = new Dictionary<IBodyPartTimeDayMulti, DayMultiWrapper>();

		#region Register/Deregister
		private protected bool AddTimeListener(IBodyPartTimeLazy listener)
		{
			if (lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = new LazyWrapper(isPlayer, listener);
				lazyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeLazy listener)
		{
			if (!lazyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				LazyWrapper wrapper = lazyListeners[listener];
				lazyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterLazyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeActive listener)
		{
			if (activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = new ActiveWrapper(isPlayer, listener);
				activeListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeActive listener)
		{
			if (!activeListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				ActiveWrapper wrapper = activeListeners[listener];
				activeListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterActiveListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDaily listener)
		{
			if (dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = new DailyWrapper(isPlayer, listener);
				dailyListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDaily listener)
		{
			if (!dailyListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DailyWrapper wrapper = dailyListeners[listener];
				dailyListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDailyListener(wrapper);
				}
				return true;
			}
		}

		private protected bool AddTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = new DayMultiWrapper(isPlayer, listener);
				dayMultiListeners.Add(listener, wrapper);
				if (listenersActive)
				{
					GameEngine.RegisterDayMultiListener(wrapper);
				}
				return true;
			}
		}

		private protected bool RemoveTimeListener(IBodyPartTimeDayMulti listener)
		{
			if (!dayMultiListeners.ContainsKey(listener))
			{
				return false;
			}
			else
			{
				DayMultiWrapper wrapper = dayMultiListeners[listener];
				dayMultiListeners.Remove(listener);
				if (listenersActive)
				{
					GameEngine.DeregisterDayMultiListener(wrapper);
				}
				return true;
			}
		}
		#endregion
		protected void ActivateTimeListeners()
		{
			if (!listenersActive)
			{
				listenersActive = true;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.RegisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.RegisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.RegisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.RegisterDayMultiListener(listener);
				}
			}
		}

		protected void DeactivateTimeListeners()
		{
			if (listenersActive)
			{
				listenersActive = false;
				foreach (var listener in lazyListeners.Values)
				{
					GameEngine.DeregisterLazyListener(listener);
				}
				foreach (var listener in activeListeners.Values)
				{
					GameEngine.DeregisterActiveListener(listener);
				}
				foreach (var listener in dailyListeners.Values)
				{
					GameEngine.DeregisterDailyListener(listener);
				}
				foreach (var listener in dayMultiListeners.Values)
				{
					GameEngine.DeregisterDayMultiListener(listener);
				}
			}
		}

		#endregion

		//public


		#region DESTRUCTOR/FINALIZER
		~Creature()
		{
			CleanupCreatureForDeletion();
		}

		internal void CleanupCreatureForDeletion()
		{
			if (listenersActive)
			{
				//remove all events if any exist. 
				DeactivateTimeListeners();

				lazyListeners.Clear();
				activeListeners.Clear();
				dailyListeners.Clear();
				dayMultiListeners.Clear();
			}
		}
		#endregion

	}
}
