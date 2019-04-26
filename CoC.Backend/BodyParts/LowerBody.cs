//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Races;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	public sealed class LowerBody : BehavioralSaveablePart<LowerBody, LowerBodyType>, IBodyAware
	{
		public readonly Feet feet;
		//		//No magic constants. Woo!
		//		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//		//when numbers aren't magic constants. (running grep with a string is much easier
		//		//than a regular expression looking for legs.count = [A-Za-z0-9]+ or something worse

		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADRUPED_LEG_COUNT = 4;
		public const byte SEXTOPED_LEG_COUNT = 6; //for squids/octopi, if i implement them. technically, an octopus is 2 legs and 6 arms, but i like the 6 legs 2 arms because legs have a count, arms dont.
		public const byte OCTOPED_LEG_COUNT = 8;

		public EpidermalData primaryEpidermis => type.ParseEpidermis(bodyData());
		public EpidermalData secondaryEpidermis => type.ParseEpidermis(bodyData());
		public int legCount => type.legCount;

		private LowerBody(LowerBodyType type)
		{
			_type = type;
			feet = Feet.GenerateDefault(type.footType);
		}

		public override LowerBodyType type
		{
			get => _type;
			protected set
			{
				_type = value;
				feet.UpdateType(value.footType);
			}
		}

		public override bool isDefault => type == LowerBodyType.HUMAN;

		private LowerBodyType _type;

		internal static LowerBody GenerateDefault()
		{
			return new LowerBody(LowerBodyType.HUMAN);
		}

		internal static LowerBody GenerateDefaultOfType(LowerBodyType type)
		{
			return new LowerBody(type);
		}

		internal bool UpdateLowerBody(LowerBodyType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		internal override bool Restore()
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			type = LowerBodyType.HUMAN;
			return true;
		}


		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			throw new System.NotImplementedException();
		}
		private BodyDataGetter bodyData;

		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}

		public bool isMonoped => legCount == MONOPED_LEG_COUNT;
		public bool isBiped => legCount == BIPED_LEG_COUNT;
		public bool isQuadruped => legCount == QUADRUPED_LEG_COUNT;
		public bool isSextoped => legCount == SEXTOPED_LEG_COUNT;
		public bool isOctoped => legCount == OCTOPED_LEG_COUNT;
	}

	public abstract partial class LowerBodyType : SaveableBehavior<LowerBodyType, LowerBody>
	{

		private const int NOLEGS = 0;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int SEXTOPED = 6;
		private const int OCTOPED = 8;

		public readonly int legCount;

		private static int indexMaker = 0;
		private static List<LowerBodyType> lowerBodyTypes = new List<LowerBodyType>();

		public readonly FootType footType;
		public readonly EpidermisType epidermisType;

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

		public override int index => _index;
		private readonly int _index;

		protected LowerBodyType(FootType foot, EpidermisType epidermis, int numLegs,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, RestoreType<LowerBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			lowerBodyTypes.AddAt(this, _index);
			footType = foot;
			epidermisType = epidermis;
			legCount = numLegs;
		}

		public static readonly LowerBodyType HUMAN = new ToneLowerBody(FootType.HUMAN, EpidermisType.SKIN, BIPED, Species.HUMAN.defaultTone, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly LowerBodyType HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, Species.HORSE.defaultFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DOG = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.DOG.defaultFur, FurTexture.NONDESCRIPT, true, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly LowerBodyType CENTAUR = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, QUADRUPED, Species.HORSE.defaultFur, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType FERRET = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.FERRET.defaultFur, FurTexture.NONDESCRIPT, false, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly LowerBodyType DEMONIC_HIGH_HEELS = new ToneLowerBody(FootType.DEMON_HEEL, EpidermisType.SKIN, BIPED, Species.DEMON.defaultTone, SkinTexture.NONDESCRIPT, true, DemonHiHeelsDesc, DemonHiHeelsFullDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly LowerBodyType DEMONIC_CLAWS = new ToneLowerBody(FootType.DEMON_CLAW, EpidermisType.SKIN, BIPED, Species.DEMON.defaultTone, SkinTexture.NONDESCRIPT, true, DemonClawDesc, DemonClawFullDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly LowerBodyType BEE = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, Species.BEE.defaultTone, SkinTexture.SHINY, false, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly LowerBodyType GOO = new ToneLowerBody(FootType.NONE, EpidermisType.GOO, MONOPED, Species.GOO.defaultTone, SkinTexture.NONDESCRIPT, true, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly LowerBodyType CAT = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.CAT.defaultFur, FurTexture.NONDESCRIPT, true, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly LowerBodyType LIZARD = new ToneLowerBody(FootType.LIZARD_CLAW, EpidermisType.SCALES, BIPED, Species.LIZARD.defaultTone, SkinTexture.NONDESCRIPT, true, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly LowerBodyType PONY = new FurLowerBody(FootType.BRONY, EpidermisType.FUR, QUADRUPED, Species.PONY.MLP_Fur, FurTexture.NONDESCRIPT, true, PonyDesc, PonyFullDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly LowerBodyType BUNNY = new FurLowerBody(FootType.RABBIT, EpidermisType.FUR, BIPED, Species.BUNNY.defaultFur, FurTexture.NONDESCRIPT, true, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly LowerBodyType HARPY = new FurLowerBody(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, Species.HARPY.defaultFeathers, FurTexture.NONDESCRIPT, true, HarpyDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly LowerBodyType KANGAROO = new FurLowerBody(FootType.KANGAROO, EpidermisType.FUR, BIPED, Species.KANGAROO.defaultFur, FurTexture.NONDESCRIPT, true, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly LowerBodyType CHITINOUS_SPIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, Species.SPIDER.defaultTone, SkinTexture.SHINY, false, SpiderDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly LowerBodyType DRIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, OCTOPED, Species.SPIDER.defaultTone, SkinTexture.SHINY, false, DriderDesc, DriderFullDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly LowerBodyType FOX = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.FOX.defaultFur, FurTexture.NONDESCRIPT, true, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly LowerBodyType DRAGON = new ToneLowerBody(FootType.DRAGON_CLAW, EpidermisType.SCALES, BIPED, Species.DRAGON.defaultTone, SkinTexture.NONDESCRIPT, true, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly LowerBodyType RACCOON = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.RACCOON.defaultFur, FurTexture.NONDESCRIPT, true, RaccoonDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly LowerBodyType CLOVEN_HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, Species.HORSE.defaultFur, FurTexture.NONDESCRIPT, true, ClovenHoofDesc, ClovenHoofFullDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly LowerBodyType NAGA = new NagaLowerBody();
		public static readonly LowerBodyType ECHIDNA = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.ECHIDNA.defaultFur, FurTexture.NONDESCRIPT, true, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly LowerBodyType SALAMANDER = new ToneLowerBody(FootType.MANDER_CLAW, EpidermisType.SCALES, BIPED, Species.SALAMANDER.defaultTone, SkinTexture.NONDESCRIPT, true, SalamanderDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly LowerBodyType WOLF = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, Species.WOLF.defaultFurColor, FurTexture.NONDESCRIPT, true, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly LowerBodyType IMP = new ToneLowerBody(FootType.IMP_CLAW, EpidermisType.SCALES, BIPED, Species.IMP.defaultTone, SkinTexture.NONDESCRIPT, true, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);


		public static readonly LowerBodyType COCKATRICE = new CockatriceLowerBody();
		public static readonly LowerBodyType RED_PANDA = new RedPandaLowerBody();

		//monsters that don't have feet, like an aquatic creature or something. vines also apply, unless you go all piranna plant or something.
		public static readonly NoLeg NO_LEG_MONSTERS = new NoLeg();


		private class FurLowerBody : LowerBodyType
		{
			public readonly FurColor defaultColor;
			public readonly FurTexture defaultTexture;
			protected readonly bool mutable;
			public FurLowerBody(FootType foot, FurBasedEpidermisType epidermis, int numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
				SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc, ChangeType<LowerBody> transform,
				RestoreType<LowerBody> restore) : base(foot, epidermis, numLegs, shortDesc, fullDesc, playerDesc, transform, restore)
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
				return new EpidermalData(epidermisType, color, defaultTexture);
			}
		}

		private class ToneLowerBody : LowerBodyType
		{
			public readonly SkinTexture defaultTexture;
			public readonly bool mutable;
			public readonly Tones defaultTone;
			public ToneLowerBody(FootType foot, ToneBasedEpidermisType epidermis, int legCount, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
				 SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc, ChangeType<LowerBody> transform,
				 RestoreType<LowerBody> restore) : base(foot, epidermis, legCount, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				defaultTexture = defaultSkinTexture;
				defaultTone = defTone;
				mutable = canChange;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = mutable ? bodyData.mainSkin.tone : defaultTone;
				SkinTexture texture = mutable && bodyData.main.usesTone ? bodyData.main.skinTexture : defaultTexture;

				return new EpidermalData(epidermisType, color, texture);
			}
		}


		private class NagaLowerBody : ToneLowerBody
		{
			private Tones defaultUnderTone => Species.NAGA.defaultUnderTone;
			public NagaLowerBody() : base(FootType.NONE, EpidermisType.SCALES, MONOPED, Species.NAGA.defaultTone, 
				SkinTexture.NONDESCRIPT, true, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr)	{ }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.bodyType == BodyType.NAGA && !Tones.IsNullOrEmpty(bodyData.supplementary.tone) ? bodyData.supplementary.tone : bodyData.mainSkin.tone;
				return new EpidermalData (epidermisType, color, defaultTexture);
			}

			internal override EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
			{
				Tones color = bodyData.supplementary.tone;
				if (bodyData.bodyType == BodyType.NAGA && !Tones.IsNullOrEmpty(bodyData.supplementary.tone))
				{
					color = Species.NAGA.UnderToneFrom(color);
				}
				else if (Tones.IsNullOrEmpty(color))
				{
					color = defaultUnderTone;
				}
				return new EpidermalData(epidermisType, color, defaultTexture);
			}
		}

		private class CockatriceLowerBody : FurLowerBody
		{
			public CockatriceLowerBody() : base(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, Species.COCKATRICE.defaultPrimaryFeathers, FurTexture.NONDESCRIPT, true, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr) { }

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
				return new EpidermalData((FurBasedEpidermisType)epidermisType, color, bodyData.main.furTexture);
			}
		}

		private class RedPandaLowerBody : FurLowerBody
		{
			public RedPandaLowerBody() : base(FootType.PAW, EpidermisType.FUR, BIPED, Species.RED_PANDA.defaultFur, FurTexture.NONDESCRIPT, true, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr) { }

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				FurColor color = bodyData.supplementary.usesFur && !FurColor.IsNullOrEmpty(bodyData.supplementary.fur) ? bodyData.supplementary.fur : defaultColor;
				FurTexture texture = bodyData.supplementary.usesFur && bodyData.supplementary.furTexture != FurTexture.NONDESCRIPT ? bodyData.supplementary.furTexture : defaultTexture;
				return new EpidermalData(epidermisType, color, texture);
			}
		}
	}

	public sealed class NoLeg : LowerBodyType
	{
		public NoLeg() : base(FootType.NONE, EpidermisType.SKIN, 0, GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None())
		{
		}

		internal override EpidermalData ParseEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}

		internal override EpidermalData ParseSecondaryEpidermis(in BodyData bodyData)
		{
			return new EpidermalData();
		}
	}
}