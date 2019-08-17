//Tail.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Races;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public enum TailPiercings { SUCCUBUS_SPADE }
	public sealed class Tail : BehavioralSaveablePart<Tail, TailType>, IBodyAware, ICanAttackWith
	{
		public const JewelryType SUPPORTED_TAIL_PIERCINGS = JewelryType.RING;
		public const int MAX_ATTACK_CHARGES = 100;

		public EpidermalData epidermis => type.ParseEpidermis(bodyData());
		public EpidermalData secondaryEpidermis => type.ParseSecondaryEpidermis(bodyData());

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
		public static TailType defaultType => TailType.NONE;
		public override bool isDefault => type == defaultType;

		private Tail(TailType tailType)
		{
			_type = tailType ?? throw new ArgumentNullException(nameof(tailType));
			_tailCount = _type.initialTailCount;
			tailPiercings = new Piercing<TailPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);
		}

		internal static Tail GenerateDefault()
		{
			return new Tail(TailType.NONE);
		}

		internal static Tail GenerateDefaultOfType(TailType tailType)
		{
			return new Tail(tailType);
		}

		internal static Tail GenerateWithCount(TailType tailType, byte count)
		{
			return new Tail(tailType)
			{
				tailCount = count
			};
		}

		internal override bool UpdateType(TailType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		internal bool GrowAdditionalTail()
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return false;
			}
			tailCount++;
			return true;
		}

		internal byte GrowMultipleAdditionalTails(byte amount = 1)
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return 0;
			}
			byte oldCount = tailCount;
			tailCount = tailCount.add(amount);
			return tailCount.subtract(oldCount);
		}

		internal override bool Restore()
		{
			if (type == TailType.NONE)
			{
				return false;
			}
			type = TailType.NONE; //type resets everything.
			return true;
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
			bool piercingFetish = BackendSessionSave.data.piercingFetish;
			return type.supportsTailPiercing && piercingFetish;
		}

		private JewelryType SupportedJewelryByLocation(TailPiercings piercingLocation)
		{
			return JewelryType.RING;
		}

		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}
		private BodyDataGetter bodyData;
		//public bool canAttackWith()
		//{
		//	return type.canAttackWith;
		//}

		//public AttackBase attack => type.attack;

		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.canAttackWith;

	}

	public abstract partial class TailType : SaveableBehavior<TailType, Tail>
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
		//holy shit, fuck tails!
		//public bool canAttackWith => attack != AttackBase.NO_ATTACK;

		internal virtual AttackBase attack => AttackBase.NO_ATTACK;
		internal virtual bool canAttackWith => attack != AttackBase.NO_ATTACK;

		public bool hasMultipleTails => maxTailCount > 1;

		public virtual bool supportsTailPiercing => false;

		public virtual byte initialTailCount => 1;
		public virtual byte maxTailCount => initialTailCount;

		private protected TailType(EpidermisType epidermis, bool toneFurMutable, //AttackBase attackData,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
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

		public static readonly TailType NONE = new NoTail();
		public static readonly TailType HORSE = new FurryTail(EpidermisType.FUR, Species.HORSE.defaultTailFur, true, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly TailType DOG = new FurryTail(EpidermisType.FUR, Species.DOG.defaultTailFur, true, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly TailType DEMONIC = new SuccubusTail();
		public static readonly TailType COW = new FurryTail(EpidermisType.FUR, Species.COW.defaultTailFur, true, CowShortDesc, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly TailType SPIDER_OVIPOSITOR = new ToneTail(EpidermisType.CARAPACE, Species.SPIDER.defaultAbdomenTone, false, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //none
		public static readonly TailType BEE_OVIPOSITOR = new ToneTail(EpidermisType.CARAPACE, Species.BEE.defaultAbdomenTone, false, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //none)

		public static readonly TailType SHARK = new ToneTailWithSlam(4, EpidermisType.SKIN, Species.SHARK.defaultTailTone, true, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly TailType CAT = new FurryTail(EpidermisType.FUR, Species.CAT.defaultTailFur, true, CatShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly TailType LIZARD = new ToneTailWithWhip(EpidermisType.SCALES, Species.LIZARD.defaultTailTone, true, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly TailType RABBIT = new FurryTail(EpidermisType.FUR, Species.BUNNY.defaultTailFur, true, RabbitShortDesc, RabbitFullDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly TailType HARPY = new FurryTail(EpidermisType.FEATHERS, Species.HARPY.defaultTailFeathers, true, HarpyShortDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly TailType KANGAROO = new FurryTailWithWhip(EpidermisType.FUR, Species.KANGAROO.defaultTailFur, true, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly TailType FOX = new FoxTail();
		public static readonly TailType DRACONIC = new ToneTailWithSlam(5, EpidermisType.SCALES, Species.DRAGON.defaultTailTone, true, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly TailType RACCOON = new FurryTailWithWhip(EpidermisType.FUR, Species.RACCOON.defaultTailFur, true, RaccoonShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly TailType MOUSE = new FurryTail(EpidermisType.FUR, Species.MOUSE.defaultTailFur, true, MouseShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly TailType FERRET = new FurryTailWithWhip(EpidermisType.FUR, Species.FERRET.defaultTailFur, true, FerretShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly TailType BEHEMOTH = new FurryTailWithSlam(5, EpidermisType.FUR, Species.BEHEMOTH.defaultTailFur, true, BehemothShortDesc, BehemothFullDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly TailType PIG = new FurryTail(EpidermisType.FUR, Species.PIG.defaultTailFur, true, PigShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		//public static readonly TailType SCORPION = new ToneTail(EpidermisType.CARAPACE, Species.SCORPION.defaultTailTone, false, ScorpionShortDesc, ScorpionFullDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly TailType GOAT = new FurryTail(EpidermisType.FUR, Species.GOAT.defaultTailFur, true, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly TailType RHINO = new FurryTail(EpidermisType.FUR, Species.RHINO.defaultTailFur, true, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly TailType ECHIDNA = new FurryTail(EpidermisType.FUR, Species.ECHIDNA.defaultTailFur, true, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TailType DEER = new FurryTail(EpidermisType.FUR, Species.DEER.defaultTailFur, true, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly TailType SALAMANDER = new SalamanderTail();
		public static readonly TailType WOLF = new FurryTail(EpidermisType.FUR, Species.WOLF.defaultTailFur, true, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly TailType SHEEP = new FurryTail(EpidermisType.WOOL, Species.SHEEP.defaultTailFur, true, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly TailType IMP = new FurryTail(EpidermisType.FUR, Species.IMP.defaultTailFur, true, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly TailType COCKATRICE = new FurryTail(EpidermisType.FUR, Species.COCKATRICE.defaultTailFeaithers, true, CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly TailType RED_PANDA = new FurryTail(EpidermisType.FUR, Species.RED_PANDA.defaultFaceEarTailFur, true, RedPandaShortDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);

		private sealed class NoTail : TailType
		{
			public NoTail() : base(EpidermisType.EMPTY, false, NoTailShortDesc, NoTailFullDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr) { }

			public override byte initialTailCount => 0;

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				return new EpidermalData();
			}
		}

		private class FurryTail : TailType
		{
			public readonly FurColor defaultFur;
			protected FurBasedEpidermisType primaryEpidermis => (FurBasedEpidermisType)epidermisType;
			public FurryTail(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable,
				SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
				RestoreType<Tail> restore) : base(furType, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
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
		SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
		RestoreType<Tail> restore) : base(toneType, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				defaultTone = defaultColor;
			}

			internal override EpidermalData ParseEpidermis(in BodyData bodyData)
			{
				Tones color = mutable ? bodyData.main.tone : defaultTone;
				return new EpidermalData(primaryEpidermis, color, SkinTexture.NONDESCRIPT);
			}
		}

		private class FurryTailWithWhip : FurryTail
		{
			public FurryTailWithWhip(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc,
				TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform, RestoreType<Tail> restore)
				: base(furType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore) { }

			internal override AttackBase attack => TAIL_WHIP;
		}

		private class ToneTailWithWhip : ToneTail
		{
			public ToneTailWithWhip(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc,
				TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform, RestoreType<Tail> restore)
				: base(toneType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore) { }

			internal override AttackBase attack => TAIL_WHIP;
		}

		private class SalamanderTail : ToneTail
		{
			public SalamanderTail() : base(EpidermisType.SCALES, Species.SALAMANDER.defaultTailTone, false, SalamanderShortDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr)
			{ }

			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new TailSlap();
		}

		private class ToneTailWithSlam : ToneTail
		{
			internal override AttackBase attack => _attack;
			private readonly TailSlam _attack;
			public ToneTailWithSlam(byte attackStrength, ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable, SimpleDescriptor shortDesc,
				DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform, RestoreType<Tail> restore)
				: base(toneType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				_attack = new TailSlam(shortDesc, attackStrength);
			}
		}

		private class FurryTailWithSlam : FurryTail
		{
			internal override AttackBase attack => _attack;
			private readonly TailSlam _attack;
			public FurryTailWithSlam(byte attackStrength, FurBasedEpidermisType furType, FurColor defaultColor, bool mutable, SimpleDescriptor shortDesc,
				DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform, RestoreType<Tail> restore)
				: base(furType, defaultColor, mutable, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				_attack = new TailSlam(shortDesc, attackStrength);
			}
		}

		private static readonly TailWhip TAIL_WHIP = new TailWhip();



		private class FoxTail : FurryTail
		{
			public FoxTail() : base(EpidermisType.FUR, Species.FOX.defaultTailFur, true, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr) { }
			public override byte maxTailCount => 9;
		}


		private class SuccubusTail : ToneTail
		{
			public SuccubusTail() : base(EpidermisType.SKIN, Species.DEMON.defaultTailTone, true, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

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


}
