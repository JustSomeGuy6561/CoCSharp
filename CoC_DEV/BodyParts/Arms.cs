//Arms.cs
//Description: Arm Body Part class.
//Author: JustSomeGuy
//12/26/2018, 7:58 PM
using CoC.Creatures;
using CoC.EpidermalColors;
using CoC.Serialization;
using CoC.Tools;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static CoC.UI.TextOutput;




namespace CoC.BodyParts
{
	/*
	 * Quick Note: Arms now have a consistent logic - if the arm is furry, it will first try to use the secondary (underbody) color, if the body has one. if not, it will fallback to the
	 * body's regular fur color, if one exists. It will fallback to hair if not available, and if THAT is not available, fallback to the default for whatever type of arm it is. 
	 * Tones will simply use the primary skin tone - i cannot think of a reason arms would have a special tone different from the body. 
	 * Since this logic is implemented in the arm type, a derived class can override this behavior for custom arm types. currently, Ferrets do this.
	 * 
	 * Serialization note: This class uses the Save Attribute for readibility ONLY. it isn't actually used. I could use it with reflection to automatically serialize the class
	 * using a great deal of magic/bullshit, but it's frankly slow, and manually implementing it isn't terribly hard.
	 */
	[DataContract]
	internal class Arms : BodyPartBase<Arms, ArmType>, ISerializable
	{
		public readonly Hands hands;

		private readonly Epidermis _epidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);
		private readonly Epidermis _secondaryEpidermis = Epidermis.GenerateDefault(EpidermisType.SKIN);

		private Tones restoreTone;

		public EpidermalData epidermis => _epidermis.GetEpidermalData();
		public EpidermalData secondaryEpidermis => _secondaryEpidermis.GetEpidermalData();

		protected Arms(ArmType type)
		{
			_type = type;
			hands = Hands.Generate(type.handType);
		}

		public override ArmType type
		{
			get => _type;
			protected set
			{
				_type = value;
				hands.UpdateHands(value.handType);
			}
		}
		[Save]
		private ArmType _type;

		public static Arms GenerateDefault()
		{
			return new Arms(ArmType.HUMAN);
		}

		public static Arms GenerateDefaultOfType(ArmType type)
		{
			return new Arms(type);
		}

		public override bool Restore()
		{
			if (type == ArmType.HUMAN)
			{
				return false;
			}
			_epidermis.UpdateOrChange((ToneBasedEpidermisType)type.epidermisType, restoreTone, true);
			_secondaryEpidermis.Reset();
			type = ArmType.HUMAN;
			
			return true;
		}

		public bool UpdateArmsFromBody(ArmType armType, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			if (type == armType)
			{
				return false;
			}
			type = armType;
			UpdateEpidermisFromBody(primary, secondary, hairColor, bodyType);
			return true;
		}

		public bool UpdateArmsFromBodyAndDisplayMessage(ArmType newType, Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateArmsFromBody(newType, primary, secondary, hairColor, bodyType);
		}

		public bool UpdateEpidermisFromBody(Epidermis primary, Epidermis secondary, HairFurColors hairColor, BodyType bodyType)
		{
			hands.reactToChangeInSkinTone(primary.tone, secondary.tone);
			restoreTone = primary.tone;
			return type.UpdateEpidermis(_epidermis, _secondaryEpidermis, primary, secondary, hairColor, bodyType);
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == ArmType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = ArmType.HUMAN;
			return true;
		}

		protected Arms(SerializationInfo info, StreamingContext context)
		{
			_type = ArmType.Deserialize(info.GetInt32(nameof(type)));
			hands = Hands.Generate(_type.handType);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(type), _type.index);
		}
	}

	internal abstract partial class ArmType : BodyPartBehavior<ArmType, Arms>
	{
		private static int indexMaker = 0;
		private static List<ArmType> arms = new List<ArmType>();

		public readonly HandType handType;
		public readonly EpidermisType epidermisType;

		//update the original and secondary original based on the current data. 
		public abstract bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType);

		public override int index => _index;
		private readonly int _index;

		protected ArmType(HandType hand, EpidermisType epidermis,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc,
			ChangeType<Arms> transform, RestoreType<Arms> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			handType = hand;
			epidermisType = epidermis;
			arms[_index] = this;
		}
		public static ArmType Deserialize(int index)
		{
			if (index < 0 || index >= arms.Count)
			{
				throw new System.ArgumentException("index for arm type desrialize out of range");
			}
			else
			{
				ArmType arm = arms[index];
				if (arm != null)
				{
					return arm;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		//DO NOT REORDER THESE (Under penalty of death lol)
		public static readonly ToneArms HUMAN = new ToneArms(HandType.HUMAN, EpidermisType.SKIN, Tones.HUMAN_DEFAULT, SkinTexture.NONDESCRIPT, true, HumanDescStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FurArms HARPY = new FurArms(HandType.HUMAN, EpidermisType.FEATHERS, FurColor.HARPY_DEFAULT, FurTexture.NONDESCRIPT, true, HarpyDescStr, HarpyFullDesc, HarpyPlayerStr, HarpyTransformStr, HarpyRestoreStr);
		public static readonly ToneArms SPIDER = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.SPIDER_DEFAULT, SkinTexture.SHINY, false, SpiderDescStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr);
		public static readonly ToneArms BEE = new ToneArms(HandType.HUMAN, EpidermisType.CARAPACE, Tones.BEE_DEFAULT, SkinTexture.SHINY, false, BeeDescStr, BeeFullDesc, BeePlayerStr, BeeTransformStr, BeeRestoreStr);
		//I broke up predator arms to make the logic here easier. now all arms have one hand/claw type.
		//you still have the ability to check for predator arms via a function below. no functionality has been lost.
		public static readonly ToneArms DRAGON = new ToneArms(HandType.DRAGON, EpidermisType.SCALES, Tones.DRAGON_DEFAULT, SkinTexture.NONDESCRIPT, true, DragonDescStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly ToneArms IMP = new ToneArms(HandType.IMP, EpidermisType.SCALES, Tones.IMP_DEFAULT, SkinTexture.NONDESCRIPT, true, ImpDescStr, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);
		public static readonly ToneArms LIZARD = new ToneArms(HandType.LIZARD, EpidermisType.SCALES, Tones.LIZARD_DEFAULT, SkinTexture.NONDESCRIPT, true, LizardDescStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly ToneArms SALAMANDER = new ToneArms(HandType.SALAMANDER, EpidermisType.SCALES, Tones.SALAMANDER_DEFAULT, SkinTexture.NONDESCRIPT, false, SalamanderDescStr, SalamanderFullDesc, SalamanderPlayerStr, SalamanderTransformStr, SalamanderRestoreStr);
		public static readonly FurArms WOLF = new FurArms(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, WolfDescStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FurArms COCKATRICE = new CockatriceArms();
		public static readonly FurArms RED_PANDA = new FurArms(HandType.RED_PANDA, EpidermisType.FUR, FurColor.RED_PANDA_DEFAULT, FurTexture.NONDESCRIPT, false, RedPandaDescStr, RedPandaFullDesc, RedPandaPlayerStr, RedPandaTransformStr, RedPandaRestoreStr);
		public static readonly FurArms FERRET = new FerretArms();
		public static readonly FurArms CAT = new FurArms(HandType.CAT, EpidermisType.FUR, FurColor.CAT_DEFAULT, FurTexture.NONDESCRIPT, true, CatDescStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FurArms DOG = new FurArms(HandType.DOG, EpidermisType.FUR, FurColor.DOG_DEFAULT, FurTexture.NONDESCRIPT, true, DogDescStr, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FurArms FOX = new FurArms(HandType.FOX, EpidermisType.FUR, FurColor.CAT_DEFAULT, FurTexture.NONDESCRIPT, true, FoxDescStr, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		//Add new Arm Types Here.

		private class FerretArms : FurArms
		{
			private readonly FurColor defaultSecondaryColor = FurColor.Generate(HairFurColors.BROWN, HairFurColors.BLACK, FurMulticolorPattern.MIXED);
			public FerretArms() : base(HandType.FERRET, EpidermisType.FUR, FurColor.FERRET_DEFAULT, FurTexture.NONDESCRIPT, true, FerretDescStr,
				FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr)
			{ }

			//ferret arms are weird - they change part-way down, so the upper half is actually the primary color if applicable. the second half is the underbody color or the default.
			public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
			{
				bool retVal = false;
				FurColor color = this.defaultColor;
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
				retVal |= original.UpdateOrChange((FurBasedEpidermisType)epidermisType, color, true);

				color = defaultSecondaryColor;
				if (currSecondary.usesFur && !currSecondary.fur.isNoFur())
				{
					color = currSecondary.fur;
				}
				retVal |= secondaryOriginal.UpdateOrChange((FurBasedEpidermisType)epidermisType, color, true);
				return retVal;
			}
		}

		//upper half of arm - primary color, as fur. lower half: scales, uses secondary tone. 
		private class CockatriceArms : FurArms
		{
			public CockatriceArms() : base(HandType.COCKATRICE, EpidermisType.FUR, FurColor.COCKATRICE_DEFAULT, FurTexture.NONDESCRIPT, true,
				CockatriceDescStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
			{ }

			public override bool UpdateEpidermis(Epidermis original, Epidermis secondaryOriginal, Epidermis currPrimary, Epidermis currSecondary, HairFurColors currHair, BodyType bodyType)
			{
				bool retVal = false;
				FurColor color = this.defaultColor;
				if (mutable)
				{
					if (bodyType == BodyType.COCKATRICE && currPrimary.usesFur && !currPrimary.fur.isNoFur())
					{
						color = currPrimary.fur;
					}
					else if (currHair != HairFurColors.NO_HAIR_FUR)
					{
						color = FurColor.Generate(currHair);
					}

				}
				retVal |= original.UpdateOrChange((FurBasedEpidermisType)epidermisType, color, true);

				Tones tone = currPrimary.tone;
				if (currSecondary.usesTone && !currSecondary.fur.isNoFur())
				{
					color = currSecondary.fur;
				}
				retVal |= secondaryOriginal.UpdateOrChange(EpidermisType.SCALES, tone, true);
				return retVal;
			}
		}
		public bool isPredatorArms()
		{
			return this == DRAGON || this == IMP || this == LIZARD;
		}
	}

	internal class FurArms : ArmType
	{
		public readonly FurColor defaultColor;
		public readonly FurTexture defaultTexture;
		protected readonly bool mutable;
		public FurArms(HandType hand, FurBasedEpidermisType epidermis, FurColor defaultFurColor, FurTexture defaultFurTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
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

	internal class ToneArms : ArmType
	{
		public readonly SkinTexture defaultTexture;
		public readonly bool mutable;
		public readonly Tones defaultTone;
		public ToneArms(HandType hand, ToneBasedEpidermisType epidermis, Tones defTone, SkinTexture defaultSkinTexture, bool canChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Arms> fullDesc, TypeAndPlayerDelegate<Arms> playerDesc, ChangeType<Arms> transform, RestoreType<Arms> restore) :
			base(hand, epidermis, shortDesc, fullDesc, playerDesc, transform, restore)
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
}
