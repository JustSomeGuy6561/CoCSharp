//Tail.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM
using CoC.Creatures;
using CoC.Engine.Combat.Attacks;
using CoC.EpidermalColors;
using CoC.Tools;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	public enum TailPiercings { SUCCUBUS_SPADE }
	internal class Tail : PiercableBodyPart<Tail, TailType, TailPiercings>//, ICanAttackWith
	{

		private readonly Epidermis _epidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private readonly Epidermis _secondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		public EpidermalData epidermis => _epidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermis => _secondaryEpidermis.GetEpidermalData();

		public const int MAX_ATTACK_CHARGES = 100;
		public byte tailCount
		{
			get => _tailCount;
			protected set
			{
				Utils.Clamp(ref value, type.initialTailCount, type.maxTailCount);
				_tailCount = value;
			}
		}
		private byte _tailCount = 0;

		protected Tail()
		{
			type = TailType.NONE;
		}



		public override TailType type
		{
			get => _type;
			protected set
			{
				if (_type != value)
				{
					_tailCount = value.initialTailCount;

				}
				_type = value;
			}
		}
		private TailType _type;



		public static Tail GenerateDefault()
		{
			return new Tail();
		}

		public static Tail GenerateDefaultOfType(TailType tailType)
		{
			return new Tail()
			{
				type = tailType
			};
		}

		public bool UpdateTailFromBody(NoTail noTail)
		{
			return Restore();
		}

		public bool UpdateTailFromBody(FurryTail tail, Epidermis primary, Epidermis secondary, HairFurColors hair, BodyType bodyType)
		{
			if (type == tail)
			{
				return false;
			}
			type = tail;
			UpdateEpidermisFromBody(primary, secondary, hair, bodyType);
			return true;
		}

		public bool UpdateTailFromBody(ToneTail tail, Epidermis primary, Epidermis secondary, HairFurColors hair, BodyType bodyType)
		{
			if (type == tail)
			{
				return false;
			}
			type = tail;
			UpdateEpidermisFromBody(primary, secondary, hair, bodyType);
			return true;
		}

		public bool UpdateTailFromBody(FoxTail tail, Epidermis primary, Epidermis secondary, HairFurColors hair, BodyType bodyType)
		{
			if (type == tail)
			{
				return false;
			}
			type = tail;
			UpdateEpidermisFromBody(primary, secondary, hair, bodyType);
			return true;
		}

		public bool UpdateEpidermisFromBody(Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			return type.UpdateEpidermis(_epidermis, _secondaryEpidermis, primary, secondary, hairColor, bodyType);
		}

		public bool GrowAdditionalTail()
		{
			if (!type.hasMultipleTails || tailCount >= type.maxTailCount)
			{
				return false;
			}
			tailCount++;
			return true;
		}

		public override bool Restore()
		{
			if (type == TailType.NONE)
			{
				return false;
			}
			type = TailType.NONE; //type resets everything.
			_epidermis.Reset();
			_secondaryEpidermis.Reset();
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == TailType.NONE)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		protected override bool PiercingLocationUnlocked(TailPiercings piercingLocation)
		{
			return type.supportsTailPiercing && piercingFlags.piercingFetishEnabled;
		}

		//public bool canAttackWith()
		//{
		//	return type.canAttackWith;
		//}

		//public AttackBase attack => type.attack;

	}

	internal abstract partial class TailType : PiercableBodyPartBehavior<TailType, Tail, TailPiercings>
	{
		public static int indexMaker = 0;

		public readonly AttackBase attack;
		public readonly bool mutable;
		public abstract bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType);

		private readonly int _index;
		//holy shit, fuck tails!
		//public bool canAttackWith => attack != AttackBase.NO_ATTACK;
		public bool hasMultipleTails => maxTailCount > 1;

		public virtual bool supportsTailPiercing => false;

		public virtual byte initialTailCount => 1;
		public virtual byte maxTailCount => initialTailCount;

		protected TailType(bool canChange, //AttackBase attackData,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			mutable = canChange;
			_index = indexMaker++;
			//attack = attackData ?? AttackBase.NO_ATTACK;
		}

		public override int index => _index;

		public static readonly NoTail NONE = new NoTail();
		public static readonly FurryTail HORSE = new FurryTail(EpidermisType.FUR, Species.HORSE.defaultTailFur, true, HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly FurryTail DOG = new FurryTail(EpidermisType.FUR, Species.DOG.defaultTailFur, true, DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly ToneTail DEMONIC = new SuccubusTail();
		public static readonly FurryTail COW = new FurryTail(EpidermisType.FUR, Species.COW.defaultTailFur, true, CowShortDesc, CowFullDesc, CowPlayerStr, CowTransformStr, CowRestoreStr);
		public static readonly ToneTail SPIDER_ABDOMEN = new ToneTail(EpidermisType.CARAPACE, Species.SPIDER.defaultAbdomenTone, false, SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr); //web
		public static readonly ToneTail BEE_ABDOMEN = new ToneTail(EpidermisType.CARAPACE, Species.BEE.defaultAbdomenTone, false, BeeShortDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr); //sting
		public static readonly ToneTail SHARK = new ToneTail(EpidermisType.SKIN, Species.SHARK.defaultTailTone, true, SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr); //slam
		public static readonly FurryTail CAT = new FurryTail(EpidermisType.FUR, Species.CAT.defaultTailFur, true, CatShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly ToneTail LIZARD = new ToneTail(EpidermisType.SCALES, Species.LIZARD.defaultTailTone, true, LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr); //whip
		public static readonly FurryTail RABBIT = new FurryTail(EpidermisType.FUR, Species.RABBIT.defaultTailFur, true, RabbitShortDesc, RabbitFullDesc, RabbitPlayerStr, RabbitTransformStr, RabbitRestoreStr);
		public static readonly FurryTail HARPY = new FurryTail(EpidermisType.FEATHERS, Species.HARPY.defaultTailFur, true, HarpyShortDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly FurryTail KANGAROO = new FurryTail(EpidermisType.FUR, Species.KANGAROO.defaultTailFur, true, KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr); //whip
		public static readonly FoxTail FOX = new FoxTail();
		public static readonly ToneTail DRACONIC = new ToneTail(EpidermisType.SCALES, Species.DRAGON.defaultTailTone, true, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr); //slam (dragon)
		public static readonly FurryTail RACCOON = new FurryTail(EpidermisType.FUR, Species.RACCOON.defaultTailFur, true, RaccoonShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr); //whip
		public static readonly FurryTail MOUSE = new FurryTail(EpidermisType.FUR, Species.MOUSE.defaultTailFur, true, MouseShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FurryTail FERRET = new FurryTail(EpidermisType.FUR, Species.FERRET.defaultTailFur, true, FerretShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr); //whip
		public static readonly FurryTail BEHEMOTH = new FurryTail(EpidermisType.FUR, Species.BEHEMOTH.defaultTailFur, true, BehemothShortDesc, BehemothFullDesc, BehemothPlayerStr, BehemothTransformStr, BehemothRestoreStr); //slam
		public static readonly FurryTail PIG = new FurryTail(EpidermisType.FUR, Species.PIG.defaultTailFur, true, PigShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr);
		public static readonly ToneTail SCORPION = new ToneTail(EpidermisType.CARAPACE, Species.SCORPION.defaultTailTone, false, ScorpionShortDesc, ScorpionFullDesc, ScorpionPlayerStr, ScorpionTransformStr, ScorpionRestoreStr); //sting
		public static readonly FurryTail GOAT = new FurryTail(EpidermisType.FUR, Species.GOAT.defaultTailFur, true, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr);
		public static readonly FurryTail RHINO = new FurryTail(EpidermisType.FUR, Species.RHINO.defaultTailFur, true, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly FurryTail ECHIDNA = new FurryTail(EpidermisType.FUR, Species.ECHIDNA.defaultTailFur, true, EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly FurryTail DEER = new FurryTail(EpidermisType.FUR, Species.DEER.defaultTailFur, true, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly ToneTail SALAMANDER = new ToneTail(EpidermisType.SCALES, Species.SALAMANDER.defaultTailTone, false, SalamanderShortDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr); //whip, also slap
		public static readonly FurryTail WOLF = new FurryTail(EpidermisType.FUR, Species.WOLF.defaultTailFur, true, WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurryTail SHEEP = new FurryTail(EpidermisType.WOOL, Species.SHEEP.defaultTailFur, true, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr);
		public static readonly FurryTail IMP = new FurryTail(EpidermisType.FUR, Species.IMP.defaultTailFur, true, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly FurryTail COCKATRICE = new FurryTail(EpidermisType.FUR, Species.COCKATRICE.defaultTailFur, true, CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly FurryTail RED_PANDA = new FurryTail(EpidermisType.FUR, Species.RED_PANDA.defaultTailFur, true, RedPandaShortDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);

		private class SuccubusTail : ToneTail
		{
			public SuccubusTail() : base(EpidermisType.SKIN, Species.DEMON.defaultTailTone, true, AttackBase.NO_ATTACK, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

			public override bool supportsTailPiercing => true;
		}
	}
	internal class NoTail : TailType
	{
		public NoTail() : base(false, NoTailShortDesc, NoTailFullDesc, NoTailPlayerStr, NoTailTransformStr, NoTailRestoreStr) { }

		public override byte initialTailCount => 0;

		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			original.Reset();
			secondaryOriginal.Reset();
			return true;
		}
	}
	internal class FurryTail : TailType
	{
		public readonly FurColor defaultFur;
		public readonly FurBasedEpidermisType type;
		public FurryTail(FurBasedEpidermisType furType, FurColor defaultColor, bool mutable,
			SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
			RestoreType<Tail> restore) : base(mutable, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultFur = defaultColor;
		}

		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			FurColor color = this.defaultFur;
			if (mutable)
			{
				if (currPrimary.usesFur && !currPrimary.fur.isNoFur())
				{
					color = currPrimary.fur;
				}
				else if (currHair != HairFurColors.NO_HAIR_FUR)
				{
					color = FurColor.Generate(currHair);
				}
			}
			bool retVal = original.UpdateOrChange(type, color, true);
			secondaryOriginal.Reset();
			return retVal;
		}
	}

	internal class ToneTail : TailType
	{
		public readonly Tones defaultTone;
		public readonly ToneBasedEpidermisType type;
		public ToneTail(ToneBasedEpidermisType toneType, Tones defaultColor, bool mutable,
	SimpleDescriptor shortDesc, DescriptorWithArg<Tail> fullDesc, TypeAndPlayerDelegate<Tail> playerDesc, ChangeType<Tail> transform,
	RestoreType<Tail> restore) : base(mutable, shortDesc, fullDesc, playerDesc, transform, restore) { }

		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			Tones color = mutable ? currPrimary.tone : defaultTone;

			bool retVal = original.UpdateOrChange(type, color, true);
			secondaryOriginal.Reset();
			return retVal;
		}
	}

	internal class FoxTail : FurryTail
	{
		public FoxTail() : base(EpidermisType.FUR, Species.FOX.defaultTailFur, true, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr)
		{

		}

		public override byte maxTailCount => 9;
	}



}
