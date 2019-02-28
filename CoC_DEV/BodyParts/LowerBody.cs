//LowerBody.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:09 PM

using CoC.BodyParts.SpecialInteraction;
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Serialization;
using CoC.Strings;
using CoC.Tools;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	internal class LowerBody : BodyPartBase<LowerBody, LowerBodyType>, ISerializable
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

		private readonly Epidermis _epidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private readonly Epidermis _secondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private Tones restoreTone;
		public EpidermalData epidermis => _epidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermis => _secondaryEpidermis.GetEpidermalData();

		public int legCount => type.legCount;

		[Save]
		public readonly Butt butt;
		[Save]
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
			}
		}
		[Save]
		private LowerBodyType _type;

		public static LowerBody GenerateDefault()
		{
			return new LowerBody(LowerBodyType.HUMAN);
		}

		public static LowerBody GenerateDefaultOfType(LowerBodyType type)
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
				return new LowerBody(type, Butt.BUTTLESS, Hips.BOYISH);
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
			_epidermis.UpdateOrChange((ToneBasedEpidermisType)type.epidermisType, restoreTone, true);
			_secondaryEpidermis.Reset();
			return true;
		}


		public bool UpdateFromBody(FurLowerBody furBody, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			return UpdateFromBody(lowerBody: furBody, primary, secondary, hairColor, bodyType);
		}
		public bool UpdateFromBody(ToneLowerBody toneBody, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			return UpdateFromBody(lowerBody: toneBody, primary, secondary, hairColor, bodyType);
		}
		private bool UpdateFromBody(LowerBodyType lowerBody, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			if (type == lowerBody)
			{
				return false;
			}
			type = lowerBody;
			UpdateEpidermisFromBody(primary, secondary, hairColor, bodyType);
			return true;
		}

		public bool UpdateFromBodyAndDisplayMessage(ToneLowerBody newType, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType, Player player)
		{
			return UpdateFromBodyAndDisplayMessage(lowerBody: newType, primary, secondary, hairColor, bodyType, player);
		}
		public bool UpdateFromBodyAndDisplayMessage(FurLowerBody newType, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType, Player player)
		{
			return UpdateFromBodyAndDisplayMessage(lowerBody: newType, primary, secondary, hairColor, bodyType, player);
		}
		private bool UpdateFromBodyAndDisplayMessage(LowerBodyType lowerBody, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType, Player player)
		{
			if (type == lowerBody)
			{
				return false;
			}
			OutputText(transformInto(lowerBody, player));
			return UpdateFromBody(lowerBody, primary, secondary, hairColor, bodyType);
		}

		public bool UpdateEpidermisFromBody(Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			restoreTone = primary.tone;
			return type.UpdateEpidermis(_epidermis, _secondaryEpidermis, primary, secondary, hairColor, bodyType);
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

		protected LowerBody(SerializationInfo info, StreamingContext context)
		{
			_type = LowerBodyType.Deserialize(info.GetInt32(nameof(type)));
			butt = (Butt)info.GetValue(nameof(butt), typeof(Butt));
			hips = (Hips)info.GetValue(nameof(hips), typeof(Hips));
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(type), _type.index);
			info.AddValue(nameof(butt), butt, typeof(Butt));
			info.AddValue(nameof(hips), hips, typeof(Hips));
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
		private static List<LowerBodyType> lowerBodyTypes = new List<LowerBodyType>();

		public readonly FootType footType;
		public readonly EpidermisType epidermisType;

		public static LowerBodyType Deserialize(int index)
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


		public abstract bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType);

		public override int index => _index;
		private readonly int _index;

		public virtual TypeAndPlayerDelegate<LowerBody> buttHipsPlayerDescript => GenericButtHipsPlayerDesc;

		protected LowerBodyType(FootType foot, EpidermisType epidermis, int numLegs,
			SimpleDescriptor shortDesc, DescriptorWithArg<LowerBody> fullDesc, TypeAndPlayerDelegate<LowerBody> playerDesc,
			ChangeType<LowerBody> transform, RestoreType<LowerBody> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			lowerBodyTypes[_index] = this;
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


		public static readonly FurLowerBody COCKATRICE = new CockatriceLowerBody();
		public static readonly FurLowerBody RED_PANDA = new RedPandaLowerBody();

		//monsters that don't have feet, like an aquatic creature or something. vines also apply, unless you go all piranna plant or something.
		public static readonly NoLeg NO_LEG_MONSTERS = new NoLeg();

		private class NagaLowerBody : ToneLowerBody
		{
			public NagaLowerBody() : base(FootType.NONE, EpidermisType.SCALES, MONOPED, Tones.NAGA_DEFAULT, SkinTexture.NONDESCRIPT, true, NagaDesc, NagaFullDesc, NagaPlayerStr, NagaTransformStr, NagaRestoreStr)
			{ }

			public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
			{
				Tones color = currPrimary.tone;
				Tones underColor = defaultTone;

				if (bodyType == BodyType.NAGA && currSecondary.tone != Tones.NOT_APPLICABLE)
				{
					color = currSecondary.tone;
					underColor = Tones.ParseNaga(color);
				}
				else if (bodyType == BodyType.REPTILIAN && currSecondary.tone != Tones.NOT_APPLICABLE)
				{
					underColor = currSecondary.tone;
				}
				bool retVal = original.UpdateOrChange((ToneBasedEpidermisType)epidermisType, color, true);
				retVal |= secondaryOriginal.UpdateOrChange((ToneBasedEpidermisType)epidermisType, underColor, true);
				return retVal;
			}

			//public override Epidermis parseEpidermis(Epidermis original, Epidermis secondary, HairFurColors hairColor)
			//{
			//	if (Tones.IsNagaColor(secondary.tone))
			//	{
			//		epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, secondary.tone);
			//	}
			//	else
			//	{
			//		epidermisHelper.UpdateEpidermis((ToneBasedEpidermisType)epidermisType, original.tone);
			//	}
			//	return epidermisHelper;
			//}

			//public override Epidermis parseSecondaryEpidermis(Epidermis secondary, Epidermis originalFallback, HairFurColors hairColor)
			//{
			//	epidermisHelper.UpdateEpidermis(epidermisType);
			//	if (Tones.IsNagaColor(secondary.tone))
			//	{
			//		epidermisHelper.UpdateTone(Tones.NagaColor3());
			//	}
			//	else if (secondary.tone != Tones.NOT_APPLICABLE)
			//	{
			//		epidermisHelper.ChangeTone(secondary.tone);
			//	}
			//	else
			//	{
			//		epidermisHelper.ChangeTone(((ToneBasedEpidermisType)epidermisType).defaultTone);
			//	}
			//	return epidermisHelper;
			//}
		}

		private class CockatriceLowerBody : FurLowerBody
		{
			public CockatriceLowerBody() : base(FootType.HARPY_TALON, EpidermisType.FEATHERS, BIPED, FurColor.COCKATRICE_DEFAULT, FurTexture.NONDESCRIPT, true, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr) { }

			public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
			{
				FurColor color = this.defaultColor;

				if (bodyType == BodyType.COCKATRICE && currPrimary.usesFur && !currPrimary.fur.isNoFur())
				{
					color = currPrimary.fur;
				}
				else if (currHair != HairFurColors.NO_HAIR_FUR)
				{
					color = FurColor.Generate(currHair);
				}
				secondaryOriginal.Reset();
				return original.UpdateOrChange((FurBasedEpidermisType)epidermisType, color);
			}
		}

		private class RedPandaLowerBody : FurLowerBody
		{
			public RedPandaLowerBody() : base(FootType.PAW, EpidermisType.FUR, BIPED, FurColor.RED_PANDA_DEFAULT, FurTexture.NONDESCRIPT, true, RedPandaDesc, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr) { }

			public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
			{
				FurColor color = currSecondary.usesFur && !currSecondary.fur.isNoFur() ? currSecondary.fur : defaultColor;
				secondaryOriginal.Reset();
				return original.UpdateOrChange((FurBasedEpidermisType)epidermisType, color, true);
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
		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			FurColor color = this.defaultColor;

			if (mutable)
			{
				if (currSecondary.usesFur && !currSecondary.fur.isNoFur())
				{
					color = currSecondary.fur;
				}
				else if (currPrimary.usesFur && !currPrimary.fur.isNoFur())
				{
					color = currPrimary.fur;
				}
				else if (currHair != HairFurColors.NO_HAIR_FUR)
				{
					color = FurColor.Generate(currHair);
				}

			}
			bool retVal = original.UpdateOrChange((FurBasedEpidermisType)epidermisType, color, true);
			secondaryOriginal.Reset();
			return retVal;
		}
	}

	internal class ToneLowerBody : LowerBodyType
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

		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			Tones color = mutable ? currPrimary.tone : defaultTone;
			bool retVal = original.UpdateOrChange((ToneBasedEpidermisType)epidermisType, color, true);
			secondaryOriginal.Reset();
			return retVal;
		}
	}

	internal class NoLeg : LowerBodyType
	{
		public NoLeg() : base(FootType.NONE, EpidermisType.SKIN, 0, GlobalStrings.None, (x) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None(), (x, y) => GlobalStrings.None())
		{
		}

		public override TypeAndPlayerDelegate<LowerBody> buttHipsPlayerDescript => (x, y) => GlobalStrings.None();

		public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
		{
			original.Reset();
			secondaryOriginal.Reset();
			return true;
		}
	}
}