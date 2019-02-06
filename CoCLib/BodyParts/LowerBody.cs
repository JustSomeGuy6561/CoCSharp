//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM

using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Strings;
using CoC.Tools;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	internal class LowerBody : BodyPartBase<LowerBody, LowerBodyType>, IToneAware, IFurAware, IHairAware
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

		/* NOTE: these do not store the right epidermis type - these are only used for storage. They are NOT to be used
		 * outside this class. in the event somebody needs to override the default descriptors, they are NOT to be used there, either.
		 * additionally, these are split into two parts - the body data and the lower body data. because i can't have a universal ruleset 
		 * as body parts are different and act differently, i must store both the body data and the current data. the types can then 
		 * do whatever they'd like with the type data, regardless of what happens to the body. so if you lose fur on your body, your ferret legs dont magically change color.
		 * maybe that's the desired behavior (as that's how it works now), but it should at least notify the player - something like "your legs shift colors to match your hair, now that your body fur is gone"
		 * it also could be leg-dependant - maybe some revert to their default colors, some keep the color, and some go to hair color, idk. it's possible to do any of those.
		 */
		private readonly Epidermis _bodyEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private readonly Epidermis _bodySecondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private HairFurColors _bodyHairColor = HairFurColors.BLACK; //stored in case someone wants to use it. i currently dont.
		private readonly Epidermis _epidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private readonly Epidermis _secondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);

		//these will be the right type. use these instead.
		public Epidermis epidermis => type.parseEpidermis(_epidermis, _secondaryEpidermis, _bodyHairColor);
		public Epidermis secondaryEpidermis => type.parseSecondaryEpidermis(_secondaryEpidermis, _epidermis, _bodyHairColor);

		public int legCount => type.legCount;

		public readonly Butt butt;
		public readonly Hips hips;

		protected LowerBody(LowerBodyType type, int ButtSize = Butt.AVERAGE, int HipSize = Hips.AVERAGE)
		{
			_type = type;
			feet = Feet.GenerateDefault(type.footType);
			butt = Butt.GenerateButt(size: ButtSize);
			hips = Hips.GenerateHips(HipSize);
		}

		protected LowerBody(LowerBodyType type, Butt newButt, Hips newHips)
		{

			_type = type;
			if (type == LowerBodyType.NO_LEG_MONSTERS)
			{
				butt = Butt.Generate_NoButt();
				hips = Hips.GenerateHips(Hips.BOYISH);
			}
			else
			{
				butt = newButt ?? Butt.GenerateButt();
				hips = newHips ?? Hips.GenerateHips();
			}
			feet = Feet.GenerateDefault(type.footType);
		}

		public override LowerBodyType type
		{
			get => _type;
			protected set
			{
				_type = value;
				feet.UpdateType(value.footType);
				UpdateEpidermisData(value);
			}
		}
		private LowerBodyType _type;

		public static LowerBody GenerateDefault(LowerBodyType type)
		{
			return new LowerBody(type);
		}

		public static LowerBody Generate(LowerBodyType type, int buttSize = Butt.AVERAGE, int hipSize = Hips.AVERAGE)
		{
			if (type == LowerBodyType.NO_LEG_MONSTERS)
			{
				return new LowerBody(type);
			}
			LowerBody retVal = new LowerBody(type);
			return retVal;
		}

		public static LowerBody Generate(LowerBodyType type, Butt butt, Hips hips)
		{
			if (type == LowerBodyType.NO_LEG_MONSTERS)
			{
				return new LowerBody(type);
			}
			else return new LowerBody(type, butt, hips);
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

		public bool UpdateLowerBody(FurLowerBody furLowerBody)
		{
			if (type == furLowerBody)
			{
				return false;
			}
			type = furLowerBody;
			return true;
		}

		public bool UpdateLowerBody(ToneLowerBody furLowerBody)
		{
			if (type == furLowerBody)
			{
				return false;
			}
			type = furLowerBody;
			return true;
		}

		public bool UpdateLowerBodyAndDisplayMessage(FurLowerBody newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateLowerBody(newType);
		}

		public bool UpdateLowerBodyAndDisplayMessage(ToneLowerBody newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateLowerBody(newType);
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == LowerBodyType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = LowerBodyType.HUMAN;
			return true;
		}

		public void reactToChangeInSkinTone(object sender, ToneAwareEventArg e)
		{
			_bodyEpidermis.UpdateTone(e.primaryTone);
			_bodyEpidermis.copyTo(_epidermis);

			_bodySecondaryEpidermis.UpdateTone(e.secondaryTone);
			if (!type.usesSecondaryTone || _bodySecondaryEpidermis.tone != Tones.NOT_APPLICABLE)
			{
				_secondaryEpidermis.UpdateTone(e.secondaryTone);
			}
		}

		public void reactToChangeInFurColor(object sender, FurAwareEventArg e)
		{
			_bodyEpidermis.UpdateFur(e.primaryColor);
			_bodySecondaryEpidermis.UpdateFur(e.secondaryColor);
			if (!_bodyEpidermis.fur.isNoFur() || !type.usesPrimaryFur)
			{
				_epidermis.UpdateFur(e.primaryColor);
			}
			else
			{
				_epidermis.UpdateFur(e.primaryColor);
			}
			if (!_bodySecondaryEpidermis.fur.isNoFur() || !type.usesSecondaryFur)
			{
				_secondaryEpidermis.UpdateFur(e.secondaryColor);
			}
		}

		public void reactToChangeInHairColor(object sender, HairColorEventArg e)
		{
			_bodyHairColor = e.hairColor;
		}

		//we have different data for body and arms - they are identical unless the body loses fur or "underbody", but the current type still uses it
		//of course, if a type changes, it may no longer need to remember the old fur color the body lost, nor may it need to remember the underbody. 
		//if this is the case, this function will resync the data. 
		private void UpdateEpidermisData(LowerBodyType bodyType)
		{
			if (!bodyType.usesPrimaryFur)
			{
				_epidermis.UpdateFur(_bodyEpidermis.fur);
			}
			if (!bodyType.usesSecondaryTone)
			{
				_secondaryEpidermis.UpdateTone(_bodySecondaryEpidermis.tone);
			}
			if (!bodyType.usesSecondaryFur)
			{
				_secondaryEpidermis.UpdateFur(_bodySecondaryEpidermis.fur);
			}
		}
	}

	internal abstract partial class LowerBodyType : BodyPartBehavior<LowerBodyType, LowerBody>
	{

		private const int NOLEGS = 0;
		private const int MONOPED = 1;
		private const int BIPED = 2;
		private const int QUADRUPED = 4;
		private const int SEXTOPED = 6;
		private const int OCTOPED = 8;

		public readonly int legCount;

		private static int indexMaker = 0;

		public readonly FootType footType;
		public readonly EpidermisType epidermisType;

		public abstract Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor);

		public virtual Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback, HairFurColors hairColor)
		{
			epidermisHelper.Reset(epidermisType);
			return epidermisHelper;
		}
		public virtual bool usesSecondaryTone => false;
		public virtual bool usesSecondaryFur => false;
		public virtual bool usesPrimaryFur => this is FurLowerBody;
		protected Epidermis epidermisHelper = Epidermis.GenerateDefault(EpidermisType.SKIN);

		public override int index => _index;
		private readonly int _index;

		public virtual TypeAndPlayerDelegate<LowerBody> buttHipsPlayerDescript => GenericButtHipsPlayerDesc;

		protected LowerBodyType(FootType foot, EpidermisType epidermis, int numLegs,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, RestoreType<LowerBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			footType = foot;
			epidermisType = epidermis;
			legCount = numLegs;
		}

		public static readonly ToneLowerBody HUMAN = new ToneLowerBody(FootType.HUMAN, EpidermisType.SKIN, BIPED, Tones.HUMAN_DEFAULT, SkinTexture.NONDESCRIPT, true, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurLowerBody HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, FurColor.HORSE_DEFAULT, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly FurLowerBody DOG = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, DogDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurLowerBody CENTAUR = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, QUADRUPED, Tones.HORSE_DEFAULT, FurTexture.NONDESCRIPT, true, HoovedDesc, HoovedFullDesc, HoovedPlayerStr, HoovedTransformStr, HoovedRestoreStr);
		public static readonly FurLowerBody FERRET = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.FERRET_DEFAULT, FurTexture.NONDESCRIPT, false, FerretDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly ToneLowerBody DEMONIC_HIGH_HEELS = new ToneLowerBody(FootType.DEMON_HEEL, EpidermisType.SKIN, BIPED, Tones.DEMON_DEFAULT, SkinTexture.NONDESCRIPT, true, DemonHiHeelsDesc, DemonHiHeelsFullDesc, DemonHiHeelsPlayerStr, DemonHiHeelsTransformStr, DemonHiHeelsRestoreStr);
		public static readonly ToneLowerBody DEMONIC_CLAWS = new ToneLowerBody(FootType.DEMON_CLAW, EpidermisType.SKIN, BIPED, Tones.DEMON_DEFAULT, SkinTexture.NONDESCRIPT, true, DemonClawDesc, DemonClawFullDesc, DemonClawPlayerStr, DemonClawTransformStr, DemonClawRestoreStr);
		public static readonly ToneLowerBody BEE = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, Tones.BEE_DEFAULT, SkinTexture.SHINY, false, BeeDesc, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		public static readonly ToneLowerBody GOO = new ToneLowerBody(FootType.NONE, EpidermisType.GOO, MONOPED, Tones.GOO_DEFAULT, SkinTexture.NONDESCRIPT, true, GooDesc, GooFullDesc, GooPlayerStr, GooTransformStr, GooRestoreStr);
		public static readonly FurLowerBody CAT = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.CAT_DEFAULT, FurTexture.NONDESCRIPT, true, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly ToneLowerBody LIZARD = new ToneLowerBody(FootType.LIZARD_CLAW, EpidermisType.SCALES, BIPED, Tones.LIZARD_DEFAULT, SkinTexture.NONDESCRIPT, true, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly FurLowerBody PONY = new FurLowerBody(FootType.BRONY, EpidermisType.FUR, QUADRUPED, FurColor.MLP_DEFAULT, FurTexture.NONDESCRIPT, true, PonyDesc, PonyFullDesc, PonyPlayerStr, PonyTransformStr, PonyRestoreStr);
		public static readonly FurLowerBody BUNNY = new FurLowerBody(FootType.RABBIT, EpidermisType.FUR, BIPED, FurColor.BUNNY_DEFAULT, FurTexture.NONDESCRIPT, true, BunnyDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FurLowerBody HARPY = new FurLowerBody(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, FurColor.HARPY_DEFAULT, FurTexture.NONDESCRIPT, true, HarpyDesc, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly FurLowerBody KANGAROO = new FurLowerBody(FootType.KANGAROO, EpidermisType.FUR, BIPED, FurColor.KANGAROO_DEFAULT, FurTexture.NONDESCRIPT, true, KangarooDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly ToneLowerBody CHITINOUS_SPIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, BIPED, Tones.SPIDER_DEFAULT, SkinTexture.SHINY, false, SpiderDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneLowerBody DRIDER = new ToneLowerBody(FootType.INSECTOID, EpidermisType.CARAPACE, OCTOPED, Tones.SPIDER_DEFAULT, SkinTexture.SHINY, false, DriderDesc, DriderFullDesc, DriderPlayerStr, DriderTransformStr, DriderRestoreStr);
		public static readonly FurLowerBody FOX = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.FOX_DEFAULT, FurTexture.NONDESCRIPT, true, FoxDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly ToneLowerBody DRAGON = new ToneLowerBody(FootType.DRAGON_CLAW, EpidermisType.SCALES, BIPED, Tones.DRAGON_DEFAULT, SkinTexture.NONDESCRIPT, true, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly FurLowerBody RACCOON = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.COON_DEFAULT, FurTexture.NONDESCRIPT, true, RaccoonDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FurLowerBody CLOVEN_HOOVED = new FurLowerBody(FootType.HOOVES, EpidermisType.FUR, BIPED, FurColor.HORSE_DEFAULT, FurTexture.NONDESCRIPT, true, ClovenHoofDesc, ClovenHoofFullDesc, ClovenHoofPlayerStr, ClovenHoofTransformStr, ClovenHoofRestoreStr);//?
		public static readonly ToneLowerBody NAGA = new NagaLowerBody();
		public static readonly FurLowerBody ECHIDNA = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.ECHIDNA_DEFAULT, FurTexture.NONDESCRIPT, true, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly ToneLowerBody SALAMANDER = new ToneLowerBody(FootType.MANDER_CLAW, EpidermisType.SCALES, BIPED, Tones.SALAMANDER_DEFAULT, SkinTexture.NONDESCRIPT, true, SalamanderDesc, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurLowerBody WOLF = new FurLowerBody(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.WOLF_DEFAULT, FurTexture.NONDESCRIPT, true, WolfDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly ToneLowerBody IMP = new ToneLowerBody(FootType.IMP_CLAW, EpidermisType.SCALES, BIPED, Tones.IMP_DEFAULT, SkinTexture.NONDESCRIPT, true, ImpDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);


#warning needs custom implementation
		public static readonly FurLowerBody COCKATRICE = new CockatriceLowerBody();
		public static readonly FurLowerBody RED_PANDA = new RedPandaLowerBody();

		//monsters that don't have feet, like an aquatic creature or something. vines also apply, unless you go all piranna plant or something.
		public static readonly NoLeg NO_LEG_MONSTERS = new NoLeg();

		private class NagaLowerBody : ToneLowerBody
		{
			public NagaLowerBody() : base(FootType.NONE, EpidermisType.SCALES, MONOPED, Tones.NAGA_DEFAULT, SkinTexture.NONDESCRIPT, true, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr)
			{ }
			public override bool usesSecondaryTone => true;

			public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
			{
				if (Tones.IsNagaColor(secondary.tone))
				{
					epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, secondary.tone);
				}
				else
				{
					epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, original.tone);
				}
				return epidermisHelper;
			}

			public override Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback, HairFurColors hairColor)
			{
				epidermisHelper.UpdateEpidermis(epidermisType);
				if (Tones.IsNagaColor(secondary.tone))
				{
					epidermisHelper.UpdateTone(Tones.NagaColor3());
				}
				else if (secondary.tone != Tones.NOT_APPLICABLE)
				{
					epidermisHelper.UpdateTone(secondary.tone);
				}
				else
				{
					epidermisHelper.UpdateTone(((ToneBasedEpidermisType)epidermisType).defaultTone);
				}
				return epidermisHelper;
			}
		}

		private class CockatriceLowerBody : FurLowerBody
		{
			public CockatriceLowerBody() : base(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, FurColor.COCKATRICE_DEFAULT, FurTexture.NONDESCRIPT, true, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr) { }

			public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
			{
				epidermisHelper.Reset(EpidermisType.FEATHERS);
				if (secondary.tone != Tones.NOT_APPLICABLE && !original.fur.isNoFur())
				{
					epidermisHelper.UpdateFur(original.fur);
				}
				else
				{
					epidermisHelper.UpdateFur(FurColor.Generate(hairColor));
				}
				return epidermisHelper;

			}

			public override bool usesSecondaryTone => true;
		}

		private class RedPandaLowerBody : FurLowerBody
		{
			public RedPandaLowerBody() : base(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.RED_PANDA_DEFAULT, FurTexture.NONDESCRIPT, true, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr) { }

			public override bool usesSecondaryFur => true;
			public override bool usesPrimaryFur => false;

			public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
			{
				epidermisHelper.Reset(EpidermisType.FUR);
				FurColor color = !secondary.fur.isNoFur() ? secondary.fur : defaultColor;
				epidermisHelper.UpdateFur(color, true);
				return epidermisHelper;

			}
		}
	}

	internal class FurLowerBody : LowerBodyType
	{
		public readonly FurColor defaultColor;
		public readonly FurTexture defaultTexture;
		protected readonly bool mutable;
		public FurLowerBody(FootType foot, FurBasedEpidermisType epidermis, int numLegs, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc, ChangeType<LowerBody> transform,
			RestoreType<LowerBody> restore) : base(foot, epidermis, numLegs, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultColor = FurColor.GenerateFromOther(defaultFurColor);
			defaultTexture = defaultFurTexture;
			mutable = canChange;
		}
		public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
		{
			if (mutable)
			{
				original.copyTo(epidermisHelper);
				epidermisHelper.UpdateEpidermis(epidermisType);
				if (epidermisHelper.fur.isNoFur())
				{
					if (hairColor != HairFurColors.NO_HAIR_FUR) epidermisHelper.UpdateFur(FurColor.Generate(hairColor));
					else epidermisHelper.UpdateFur(defaultColor);
				}
			}
			else
			{
				epidermisHelper.UpdateEpidermis((FurBasedEpidermisType)epidermisType, defaultColor);
			}
			return epidermisHelper;
		}
	}

	internal class ToneLowerBody : LowerBodyType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public ToneLowerBody(FootType foot, ToneBasedEpidermisType epidermis, int legCount, Tones defaultTone, SkinTexture defaultSkinTexture, bool canChange,
			 SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc, ChangeType<LowerBody> transform,
			 RestoreType<LowerBody> restore) : base(foot, epidermis, legCount, shortDesc, fullDesc, playerDesc, transform, restore)
		{
			defaultTexture = defaultSkinTexture;
			mutable = canChange;
		}

		public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
		{
			if (mutable)
			{
				original.copyTo(epidermisHelper);
				epidermisHelper.UpdateEpidermis(epidermisType);
			}
			else
			{
				epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, defaultTexture);
			}
			return epidermisHelper;
		}
	}

	internal class NoLeg : LowerBodyType
	{
		public NoLeg() : base(FootType.NONE, EpidermisType.SKIN, 0, GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None())
		{
		}

		public override TypeAndPlayerDelegate<LowerBody> buttHipsPlayerDescript => (x, y) => GlobalStrings.None();

		public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
		{
			epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, Tones.NOT_APPLICABLE);
			return epidermisHelper;
		}
	}
}