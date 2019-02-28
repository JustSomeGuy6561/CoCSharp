//Face.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 3:04 AM
using CoC.Creatures;
using CoC.Strings;
using CoC.Tools;
using static CoC.UI.TextOutput;

namespace CoC.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.
	internal class Face : BodyPartBase<Face, FaceType>
	{
		public readonly Eyes eyes;
		public readonly Ears ears;

		public readonly Eyebrow eyebrow;
		public readonly Lip lip;
		public readonly Nose nose;

		public readonly Epidermis epidermis;

#warning add update and update with display messages.
#warning add messages for strengthening?
		public bool secondLevel
		{
			get => _secondLevel;
			protected set
			{
				_secondLevel = type.hasSecondLevel ? value : false;
			}
		}
		private bool _secondLevel;

		public bool humanoidFace => type.isHumanoid(secondLevel);

		public override FaceType type { get; protected set; }

		protected Face()
		{
			type = FaceType.HUMAN;
			secondLevel = false;
			lip = Lip.Generate();
			nose = Nose.Generate();
			eyebrow = Eyebrow.Generate();
		}

		public static Face GenerateDefault()
		{
			return new Face();
		}

		public static Face GenerateNonStandardFace(FaceType faceType, bool fullMorph = false)
		{
			return new Face()
			{
				type = faceType,
				secondLevel = fullMorph
			};
		}

		public override bool Restore()
		{
			if (type == FaceType.HUMAN)
			{
				return false;
			}
			type = FaceType.HUMAN;
			secondLevel = false;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == FaceType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			return Restore();
		}

		public override SimpleDescriptor shortDescription => type.hasSecondLevel && secondLevel ? type.secondLevelShortDescription : type.shortDescription;

		public bool UpdateFace(FaceType newFeatures, bool fullMorph = false)
		{
			if (type == newFeatures)
			{
				return false;
			}
			else
			{
				type = newFeatures;
				secondLevel = fullMorph;
				return true;
			}
		}

		public bool UpdateFaceAndDisplayMessage(FaceType newFeatures, Player player, bool fullMorph = false)
		{
			if (type == newFeatures)
			{
				return false;
			}
			OutputText(transformInto(newFeatures, player));
			return UpdateFace(newFeatures, fullMorph);
		}

		public bool LessenTransform()
		{
			if (!secondLevel)
			{
				return false;
			}
			secondLevel = false;
			return true;
		}
		public bool StrengthenTransform()
		{
			if (secondLevel)
			{
				return false;
			}
			secondLevel = true;
			return true;
		}
	}

	//by default, any face that doesn't have two levels is treated as non-humanoid.
	//if it does, the first level is assumed to be humanoid.
	//this behavior can be overridden, and is for pig, etc.
	internal partial class FaceType : BodyPartBehavior<FaceType, Face>
	{
		public readonly bool hasSecondLevel;

		//first
		public virtual bool isHumanoid(bool isSecondLevel)
		{
			if (hasSecondLevel)
			{
				return isSecondLevel;
			}
			return false;
		}
		private static int indexMaker = 0;

		public readonly SimpleDescriptor secondLevelShortDescription;

		public override int index => _index;
		private readonly int _index;


		protected FaceType(SimpleDescriptor firstLevelShortDesc, SimpleDescriptor secondLevelShortDesc, //short Desc for two levels.
			DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr, ChangeType<Face> transform,
			RestoreType<Face> restore) : base(firstLevelShortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = secondLevelShortDesc;
		}

		protected FaceType(//only one level short desc
			SimpleDescriptor shortDesc, DescriptorWithArg<Face> fullDesc, TypeAndPlayerDelegate<Face> playerStr,
			ChangeType<Face> transform, RestoreType<Face> restore) : base(shortDesc, fullDesc, playerStr, transform, restore)
		{
			_index = indexMaker++;
			secondLevelShortDescription = GlobalStrings.None;
		}

		public static readonly FaceType HUMAN = new FaceType(HumanShortDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly FaceType HORSE = new FaceType(HorseShortDesc, HorseFullDesc, HorsePlayerStr, HorseTransformStr, HorseRestoreStr);
		public static readonly FaceType DOG = new FaceType(DogShortDesc, DogFullDesc, DogPlayerStr, DogTransformStr, DogRestoreStr);
		public static readonly FaceType COW_MINOTAUR = new FaceType(CowShortDesc, MinotaurShortDesc, MinotaurFullDesc, MinotaurPlayerStr, MinotaurTransformStr, MinotaurRestoreStr);
		public static readonly FaceType SHARK = new SharkFace();
		public static readonly FaceType SNAKE = new SnakeFace();
		public static readonly FaceType CAT = new FaceType(CatGirlShortDesc, CatMorphShortDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
		public static readonly FaceType LIZARD = new FaceType(LizardShortDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly FaceType BUNNY = new FaceType(BunnyFirstLevelShortDesc, BunnySecondLevelShortDesc, BunnyFullDesc, BunnyPlayerStr, BunnyTransformStr, BunnyRestoreStr);
		public static readonly FaceType KANGAROO = new FaceType(KangarooShortDesc, KangarooFullDesc, KangarooPlayerStr, KangarooTransformStr, KangarooRestoreStr);
		public static readonly FaceType SPIDER = new SpiderFace();
		public static readonly FaceType FOX = new FaceType(KitsuneShortDesc, FoxShortDesc, FoxFullDesc, FoxPlayerStr, FoxTransformStr, FoxRestoreStr);
		public static readonly FaceType DRAGON = new FaceType(DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly FaceType RACCOON = new FaceType(RaccoonMaskShortDesc, RaccoonFaceShortDesc, RaccoonFullDesc, RaccoonPlayerStr, RaccoonTransformStr, RaccoonRestoreStr);
		public static readonly FaceType MOUSE = new FaceType(MouseTeethShortDesc, MouseFaceShortDesc, MouseFullDesc, MousePlayerStr, MouseTransformStr, MouseRestoreStr);
		public static readonly FaceType FERRET = new FaceType(FerretMaskShortDesc, FerretFaceShortDesc, FerretFullDesc, FerretPlayerStr, FerretTransformStr, FerretRestoreStr);
		public static readonly FaceType PIG = new PigFace(); //both pig and boar are not humanoid.
		public static readonly FaceType RHINO = new FaceType(RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr);
		public static readonly FaceType ECHIDNA = new FaceType(EchidnaShortDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly FaceType DEER = new FaceType(DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly FaceType WOLF = new FaceType(WolfShortDesc, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static readonly FaceType COCKATRICE = new FaceType(CockatriceShortDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static readonly FaceType BEAK = new FaceType(BeakShortDesc, BeakFullDesc, BeakPlayerStr, BeakTransformStr, BeakRestoreStr);
		public static readonly FaceType RED_PANDA = new FaceType(PandaShortDesc, PandaFullDesc, PandaPlayerStr, PandaTransformStr, PandaRestoreStr);

		private class PigFace : FaceType
		{
			public PigFace() : base(PigShortDesc, BoarShortDesc, PigFullDesc, PigPlayerStr, PigTransformStr, PigRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return false;
			}
		}
		//Yes, i'm aware spider, snake, and shark all use the same format. there may come a time when one or all of these get a second level.
		//so i'm leaving them separate.
		private class SpiderFace : FaceType
		{
			public SpiderFace() : base(SpiderShortDesc, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}
		private class SharkFace : FaceType
		{
			public SharkFace() : base(SharkShortDesc, SharkFullDesc, SharkPlayerStr, SharkTransformStr, SharkRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}
		private class SnakeFace : FaceType
		{
			public SnakeFace() : base(SnakeShortDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr) { }

			public override bool isHumanoid(bool isSecondLevel)
			{
				return true;
			}
		}
	}
}