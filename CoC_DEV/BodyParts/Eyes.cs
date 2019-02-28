//Eyes.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 1:32 AM
using CoC.Creatures;
using CoC.Tools;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public enum EyeColor
	{
		AMBER, BLUE, BROWN, GRAY, GREEN, HAZEL, RED, VIOLET,
		//AND NOW THE CRAZY COLORS - WHY NOT?
		YELLOW, PINK, ORANGE, INDIGO, TAN
	}
	internal partial class Eyes : BodyPartBase<Eyes, EyeType>
	{
		protected EyeType _eyeType = EyeType.HUMAN;

		EyeColor leftIrisColor;
		//People really like heterochromia in PCs.
		EyeColor rightIrisColor;

		public override EyeType type { get; protected set; }

		protected Eyes(EyeColor color, EyeType eyetype)
		{
			type = eyetype;
			leftIrisColor = color;
			rightIrisColor = color;
		}
		protected Eyes(EyeColor leftEye, EyeColor rightEye, EyeType eyetype)
		{
			type = eyetype;
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
		}

		public static Eyes GenerateEyes(EyeColor color = EyeColor.GRAY)
		{
			return new Eyes(color, EyeType.HUMAN);
		}

		public static Eyes GenerateEyes(EyeColor leftEye, EyeColor rightEye)
		{
			return new Eyes(leftEye, rightEye, EyeType.HUMAN);
		}
		public static Eyes GenerateNonHumanEyes(EyeType type, EyeColor leftEye, EyeColor rightEye)
		{
			return new Eyes(leftEye, rightEye, type);
		}

		public bool UpdateEyeColor(EyeColor color)
		{
			if (leftIrisColor == color && rightIrisColor == color)
			{
				return false;
			}
			leftIrisColor = color;
			rightIrisColor = color;
			return true;
		}
		public bool UpdateEyeColors(EyeColor leftEye, EyeColor rightEye)
		{
			if (leftIrisColor == leftEye && rightIrisColor == rightEye)
			{
				return false;
			}
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
			return true;
		}

		public bool UpdateEyeType(EyeType newtype)
		{
			if (type == newtype)
			{
				return false;
			}
			type = newtype;
			return true;
		}

		public bool UpdateEyeTypeAndDisplayMessage(EyeType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			type = newType;
			return true;
		}

		public bool UpdateEyeColorsAndDisplayMessage(EyeColor leftEye, EyeColor rightEye)
		{
			if (leftIrisColor == leftEye && rightIrisColor == rightEye)
			{
				return false;
			}
			OutputText(ChangeEyeColorStr(this, leftEye, rightEye));
			UpdateEyeColors(leftEye, rightEye);
			return true;
		}

		public string ChangeEyeColorStr(Eyes currentEyes, EyeColor leftEye, EyeColor rightEye)
		{
			return EyeChangeStr(currentEyes, leftEye, rightEye);
		}

		public void Reset()
		{
			type = EyeType.HUMAN;
			leftIrisColor = EyeColor.GRAY;
			rightIrisColor = EyeColor.GRAY;
		}

		public override bool Restore()
		{
			if (type == EyeType.HUMAN)
			{
				return false;
			}
			type = EyeType.HUMAN;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == EyeType.HUMAN)
			{
				return false;
			}
			OutputText(restoreString(player));
			type = EyeType.HUMAN;
			return true;
		}
	}
	public enum SCLERA_COLOR
	{
		WHITE, //Human/Anthropomorphic
		BLACK, //Sand Trap
		CLEAR//, //Everything else
			 //RED   //Vampires? (silly mode, i guess)
	}
	internal partial class EyeType : BodyPartBehavior<EyeType, Eyes>
	{
		private const string SCLERA_BLACK = "black";
		private const string SCLERA_WHITE = "white";
		//private string SCLERA_RED = "red";
		private const string SCLERA_CLEAR = "clear";

		//Normally the white of the human eye
		//Generally, animals' sclera are nearly invisible
		//Thanks, Sand Traps.
		public readonly SCLERA_COLOR scleraColor;

		public readonly int eyeCount;
		private static int indexMaker = 0;
		private readonly int _index;


		protected EyeType(SimpleDescriptor shortDesc, DescriptorWithArg<Eyes> fullDesc, TypeAndPlayerDelegate<Eyes> playerDesc, ChangeType<Eyes> transform,
			RestoreType<Eyes> restore, int numEyes = 2, SCLERA_COLOR color = SCLERA_COLOR.CLEAR) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			eyeCount = numEyes;
			_index = indexMaker++;
			scleraColor = color;
		}

		/*
		
		public override string GetDescriptor()
		{
			string retVal = eyeCount.ToString() + " " + descriptor + "eyes";
			//written this way to allow for more colors proccing this
			switch (scleraColor)
			{
				case SCLERA_COLOR.BLACK:
					retVal += " surrounded by a dark " + SCLERA_BLACK + " film";
					break;
				default:
					break;
			}
			return retVal;
		}
		*/
		public static EyeType HUMAN = new EyeType(HumanShortStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr, color: SCLERA_COLOR.WHITE);
		public static EyeType SPIDER = new EyeType(SpiderShortStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr, numEyes: 4);
		public static EyeType SAND_TRAP = new EyeType(SandTrapShortStr, SandTrapFullDesc, SandTrapPlayerStr, SandTrapTransformStr, SandTrapRestoreStr, color: SCLERA_COLOR.BLACK);
		public static EyeType LIZARD = new EyeType(LizardShortStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static EyeType DRAGON = new EyeType(DragonShortStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static EyeType BASILISK = new EyeType(BasiliskShortStr, BasiliskFullDesc, BasiliskPlayerStr, BasiliskTransformStr, BasiliskRestoreStr);
		public static EyeType WOLF = new EyeType(WolfShortStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static EyeType COCKATRICE = new EyeType(CockatriceShortStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static EyeType CAT = new EyeType(CatShortStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);

		public override int index => _index;
	}
}
