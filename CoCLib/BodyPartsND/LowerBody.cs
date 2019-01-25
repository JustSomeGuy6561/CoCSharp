//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Strings;
using CoC.Tools;
using System;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	internal class LowerBody : BodyPartBase<LowerBody, LowerBodyType>, IFurAware, IToneAware
	{
		//No magic constants. Woo!
		//not even remotely necessary, but it makes it a hell of a lot easier to debug
		//when numbers aren't magic constants. (running grep with a string is much easier
		//than a regular expression looking for legs.count = [A-Za-z0-9]+ or something worse
		public const byte MONOPED_LEG_COUNT = 1;
		public const byte BIPED_LEG_COUNT = 2;
		public const byte QUADRUPED_LEG_COUNT = 4;
		public const byte OCTOPED_LEG_COUNT = 8;

		public readonly Epidermis epidermis;

		public readonly Butt butt;
		public readonly Hips hips;

		//you'll need to change this if taurs are suddenly part of everything that supports it. 
		public int legCount => type.legCount;

		public override LowerBodyType type
		{
			get => _type;
			protected set
			{
				epidermis.UpdateEpidermis(value.epidermisType);
				if (value.isFurry)
				{
					//epidermis.UpdateFur
				}
				_type = value;
			}
		}
		private LowerBodyType _type;
		protected LowerBody()
		{
			butt = Butt.Generate_NoButt();
			hips = Hips.GenerateHips(0);
			epidermis = Epidermis.Generate(EpidermisType.SKIN, Tones.LIGHT, FurColor.Generate(HairFurColors.BLACK));
			type = LowerBodyType.NO_LEG_MONSTERS;
		}

		protected LowerBody(LowerBodyType lowerBody, AssLocation assLocation, int buttSize, int hipSize)
		{
			butt = Butt.GenerateButt(assLocation, buttSize);
			hips = Hips.GenerateHips(hipSize);
			type = lowerBody;
		}

		public static LowerBody GenerateNoLowerBody()
		{
			return new LowerBody();
		}

		public static LowerBody Generate(LowerBodyType lowerBody, AssLocation assLocation = AssLocation.BUTT, int ButtSize = Butt.AVERAGE, int HipSize = Hips.AVERAGE)
		{
			return new LowerBody(lowerBody, assLocation, ButtSize, HipSize);
		}

		public override bool Restore()
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			type = LowerBodyType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		public bool UpdateType(LowerBodyType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		public bool UpdateTypeAndDisplayMessage(LowerBodyType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateType(newType);
		}



		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			if (!e.primaryColor.isNoFur())
			{
				epidermis.UpdateFur(e.primaryColor);
			}
		}

		public void reactToChangeInSkinTone(object sender, ToneAwareEventArg e)
		{
			if (e.primaryToneActive)
			{
				epidermis.UpdateTone(e.primaryTone);
			}
		}
	}

	internal partial class LowerBodyType : BodyPartBehavior<LowerBodyType, LowerBody>
	{
		private const int NOLEGS = 1;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int OCTOPED = 8;

		private static int indexMaker = 0;

		private readonly int _index;

		public virtual FurColor defaultFur => FurColor.GenerateEmpty();
		public virtual Tones defaultTone => Tones.NOT_APPLICABLE;

		public readonly int legCount;
		public readonly EpidermisType epidermisType;
		protected LowerBodyType(EpidermisType type, int numLegs,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, RestoreType<LowerBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			legCount = numLegs;
			epidermisType = type;
		}

		public override int index => _index;

		public static readonly LowerBodyType HUMAN = new LowerBodyType(EpidermisType.SKIN, BIPED, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly LowerBodyType HOOVED = new LowerBodyType(EpidermisType.FUR, BIPED, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DOG = new LowerBodyType(EpidermisType.FUR, BIPED, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly LowerBodyType NAGA = new LowerBodyType(EpidermisType.SCALES, MONOPED, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr);
		public static readonly LowerBodyType CENTAUR = new LowerBodyType(EpidermisType.FUR, QUADRUPED, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly LowerBodyType DEMONIC_HIGH_HEELS = new LowerBodyType(EpidermisType.SKIN, BIPED, DemonHiHeelsDesc, DemonHiHeelsFullDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly LowerBodyType DEMONIC_CLAWS = new LowerBodyType(EpidermisType.SKIN, BIPED, DemonClawDesc, DemonClawFullDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly LowerBodyType BEE = new LowerBodyType(EpidermisType.CARAPACE, BIPED, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly LowerBodyType GOO = new LowerBodyType(EpidermisType.GOO, MONOPED, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly LowerBodyType CAT = new LowerBodyType(EpidermisType.FUR, BIPED, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly LowerBodyType LIZARD = new LowerBodyType(EpidermisType.SCALES, BIPED, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly LowerBodyType PONY = new LowerBodyType(EpidermisType.FUR, QUADRUPED, PonyDesc, PonyFullDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly LowerBodyType BUNNY = new LowerBodyType(EpidermisType.FUR, BIPED, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FurryLowerBody HARPY = new FurryLowerBody(FurColor.Generate(HairFurColors.WHITE), true, BIPED, HarpyDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly LowerBodyType KANGAROO = new LowerBodyType(EpidermisType.FUR, BIPED, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly LowerBodyType CHITINOUS_SPIDER = new LowerBodyType(EpidermisType.CARAPACE, BIPED, SpiderDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly LowerBodyType DRIDER = new LowerBodyType(EpidermisType.CARAPACE, OCTOPED, DriderDesc, DriderFullDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly LowerBodyType FOX = new LowerBodyType(EpidermisType.FUR, BIPED, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly LowerBodyType DRAGON = new LowerBodyType(EpidermisType.SCALES, BIPED, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly LowerBodyType RACCOON = new LowerBodyType(EpidermisType.FUR, BIPED, RaccoonDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FurryLowerBody FERRET = new FurryLowerBody(FurColor.Generate(HairFurColors.BLACK), false, BIPED, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly LowerBodyType CLOVEN_HOOVED = new LowerBodyType(EpidermisType.FUR, BIPED, ClovenHoofDesc, ClovenHoofFullDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly LowerBodyType ECHIDNA = new LowerBodyType(EpidermisType.FUR, BIPED, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly LowerBodyType SALAMANDER = new LowerBodyType(EpidermisType.SCALES, BIPED, SalamanderDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly LowerBodyType WOLF = new LowerBodyType(EpidermisType.FUR, BIPED, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly LowerBodyType IMP = new LowerBodyType(EpidermisType.SCALES, BIPED, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly LowerBodyType COCKATRICE = new LowerBodyType(EpidermisType.FEATHERS, BIPED, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly LowerBodyType RED_PANDA = new LowerBodyType(EpidermisType.FUR, BIPED, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		//monsters that don't have feet, like an aquatic creature or something. vines also apply, unless you go all piranna plant or something.
		public static readonly LowerBodyType NO_LEG_MONSTERS = new LowerBodyType(EpidermisType.SKIN, NOLEGS, GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None());

		public bool isFurry => this is FurryLowerBody;
		public bool isToned => this is ToneLowerBody;
	}

	internal class FurryLowerBody : LowerBodyType
	{
		private readonly FurColor _defaultFur;
		public FurryLowerBody(FurColor defaultColor, bool feathered,
			int numLegs, SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, RestoreType<LowerBody> restore) : base(feathered ? EpidermisType.FEATHERS : EpidermisType.FUR, numLegs, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_defaultFur = FurColor.GenerateFromOther(defaultColor);
		}

		public override FurColor defaultFur => _defaultFur;
	}
}
