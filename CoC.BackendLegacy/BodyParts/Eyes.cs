//Eyes.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 1:32 AM
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	public enum EyeColor
	{
		AMBER, BLUE, BROWN, GRAY, GREEN, HAZEL, RED, VIOLET,
		//AND NOW THE CRAZY COLORS - WHY NOT?
		YELLOW, PINK, ORANGE, INDIGO, TAN, BLACK
	}

	[DataContract]
	public partial class Eyes : BehavioralSaveablePart<Eyes, EyeType>
	{

		public EyeColor leftIrisColor { get; protected set; }
		//People really like heterochromia in PCs.
		public EyeColor rightIrisColor { get; protected set; }

		public bool isHeterochromia => leftIrisColor != rightIrisColor;
		public int eyeCount => type.eyeCount;
		public bool isReptilian => type.isReptilianEyes;
		public override EyeType type { get; protected set; }


		private protected Eyes(EyeType eyetype)
		{
			type = eyetype;
			leftIrisColor = type.defaultColor;
			rightIrisColor = type.defaultColor;
		}
		private protected Eyes(EyeType eyetype, EyeColor color)
		{
			type = eyetype;
			leftIrisColor = color;
			rightIrisColor = color;
		}
		private protected Eyes(EyeType eyetype, EyeColor leftEye, EyeColor rightEye)
		{
			type = eyetype;
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
		}

		public override bool isDefault => type == EyeType.HUMAN;

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			var eyeType = type;
			bool valid = EyeType.Validate(ref eyeType, correctDataIfInvalid);
			type = eyeType;
			//check left eye. skip this if the data is already invalid and we aren't correcting invalid data.
			//checks to see if the value is out of range for the Enum (C# doesn't check enums)
			if ((valid || correctDataIfInvalid) && !Enum.IsDefined(typeof(EyeColor), (int)leftIrisColor))
			{
				if (correctDataIfInvalid)
				{
					leftIrisColor = EyeColor.AMBER;
				}
				valid = false;
			}
			//check right eye. skip this if the data is already invalid and we aren't correcting invalid data.
			//checks to see if the value is out of range for the Enum (C# doesn't check enums)
			if ((valid || correctDataIfInvalid) && !Enum.IsDefined(typeof(EyeColor), (int)rightIrisColor))
			{
				if (correctDataIfInvalid)
				{
					rightIrisColor = EyeColor.AMBER;
				}
				valid = false;
			}
			return valid;
		}

		internal static Eyes GenerateDefault()
		{
			return new Eyes(EyeType.HUMAN);
		}


		internal static Eyes GenerateDefaultOfType(EyeType eyeType)
		{
			return new Eyes(eyeType);
		}

		internal static Eyes GenerateWithColor(EyeType eyes, EyeColor primaryColor)
		{
			return new Eyes(eyes, primaryColor);
		}
		internal static Eyes GenerateWithHeterochromia(EyeType eyes, EyeColor leftEye, EyeColor rightEye)
		{
			return new Eyes(eyes, leftEye, rightEye);
		}

		internal bool UpdateEyeColor(EyeColor color)
		{
			if (leftIrisColor == color && rightIrisColor == color)
			{
				return false;
			}
			leftIrisColor = color;
			rightIrisColor = color;
			return true;
		}
		internal bool UpdateEyeColors(EyeColor leftEye, EyeColor rightEye)
		{
			if (leftIrisColor == leftEye && rightIrisColor == rightEye)
			{
				return false;
			}
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
			return true;
		}

		internal bool UpdateEyeType(EyeType newtype)
		{
			if (type == newtype)
			{
				return false;
			}
			type = newtype;
			return true;
		}

		public string EyeColorChangeFlavorText(EyeColor newColor)
		{
			return type.EyeChangeSpecial(leftIrisColor, newColor, rightIrisColor, newColor);
		}

		public string EyeColorChangeFlavorText(EyeColor leftEye, EyeColor rightEye)
		{
			return type.EyeChangeSpecial(leftIrisColor, leftEye, rightIrisColor, rightEye);
		}

		internal void Reset()
		{
			type = EyeType.HUMAN;
			leftIrisColor = type.defaultColor;
			rightIrisColor = type.defaultColor;
		}

		internal override bool Restore()
		{
			if (type == EyeType.HUMAN)
			{
				return false;
			}
			type = EyeType.HUMAN;
			return true;
		}

		private protected override Type[] saveVersions => new Type[] { typeof(EyeSurrogateVersion1) };
		private protected override Type currentSaveVersion => typeof(EyeSurrogateVersion1);
		private protected override BehavioralSurrogateBase<Eyes, EyeType> ToCurrentSave()
		{
			return new EyeSurrogateVersion1()
			{
				eyeType = index,
				leftEyeVal = (int)leftIrisColor,
				rightEyeVal = (int)rightIrisColor
			};
		}


		internal Eyes(EyeSurrogateVersion1 surrogate)
		{
			type = EyeType.Deserialize(surrogate.eyeType);
			leftIrisColor = (EyeColor)surrogate.leftEyeVal;
			rightIrisColor = (EyeColor)surrogate.rightEyeVal;
		}

	}
	public enum SCLERA_COLOR
	{
		WHITE, //Human/Anthropomorphic
		BLACK, //Sand Trap
		CLEAR//, //Everything else
			 //RED   //Vampires? (silly mode, i guess)
	}
	public partial class EyeType : SaveableBehavior<EyeType, Eyes>
	{
		private static int indexMaker = 0;
		private static readonly List<EyeType> eyes = new List<EyeType>();

		//Normally the white of the human eye
		//Generally, animals' sclera are nearly invisible
		//Thanks, Sand Traps.
		public readonly SCLERA_COLOR scleraColor;

		public readonly int eyeCount;
		public readonly EyeColor defaultColor;
		private readonly int _index;

		public bool isReptilianEyes => this == LIZARD || this == BASILISK || this == DRAGON;

		internal protected delegate string EyeChangeDelegate(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight);

		internal readonly EyeChangeDelegate EyeChangeSpecial;

		private protected EyeType(EyeColor defaultEyeColor, EyeChangeDelegate eyeChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Eyes> fullDesc, TypeAndPlayerDelegate<Eyes> playerDesc, ChangeType<Eyes> transform,
			RestoreType<Eyes> restore, int numEyes = 2, SCLERA_COLOR color = SCLERA_COLOR.CLEAR) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			EyeChangeSpecial = eyeChange;
			eyeCount = numEyes;
			defaultColor = defaultEyeColor;
			_index = indexMaker++;
			eyes.AddAt(this, _index);
			scleraColor = color;
		}

		internal static EyeType Deserialize(int index)
		{
			if (index < 0 || index >= eyes.Count)
			{
				throw new System.ArgumentException("index for body type deserialize out of range");
			}
			else
			{
				EyeType eye = eyes[index];
				if (eye != null)
				{
					return eye;
				}
				else
				{
					throw new System.ArgumentException("index for arm type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref EyeType eyeType, bool correctInvalidData = false)
		{
			if (eyes.Contains(eyeType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				eyeType = HUMAN;
			}
			return false;
		}

		public static EyeType HUMAN = new EyeType(Species.HUMAN.defaultEyeColor, HumanEyeChange, HumanShortStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr, color: SCLERA_COLOR.WHITE);
		public static EyeType SPIDER = new EyeType(Species.SPIDER.defaultEyeColor, SpiderEyeChange, SpiderShortStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr, numEyes: 4);
		public static EyeType SAND_TRAP = new EyeType(Species.SAND_TRAP.defaultEyeColor, SandTrapEyeChange, SandTrapShortStr, SandTrapFullDesc, SandTrapPlayerStr, SandTrapTransformStr, SandTrapRestoreStr, color: SCLERA_COLOR.BLACK);
		public static EyeType LIZARD = new EyeType(Species.LIZARD.defaultEyeColor, LizardEyeChange, LizardShortStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static EyeType DRAGON = new EyeType(Species.DRAGON.defaultEyeColor, DragonEyeChange, DragonShortStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static EyeType BASILISK = new EyeType(Species.BASILISK.defaultEyeColor, BasiliskEyeChange, BasiliskShortStr, BasiliskFullDesc, BasiliskPlayerStr, BasiliskTransformStr, BasiliskRestoreStr);
		public static EyeType WOLF = new EyeType(Species.WOLF.defaultEyeColor, WolfEyeChange, WolfShortStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static EyeType COCKATRICE = new EyeType(Species.COCKATRICE.defaultEyeColor, CockatriceEyeChange, CockatriceShortStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static EyeType CAT = new EyeType(Species.CAT.defaultEyeColor, CatEyeChange, CatShortStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);

		public override int index => _index;
	}

	[DataContract]
	public sealed class EyeSurrogateVersion1 : BehavioralSurrogateBase<Eyes, EyeType>
	{
		[DataMember]
		public int eyeType;
		[DataMember]
		public int leftEyeVal;
		[DataMember]
		public int rightEyeVal;
		internal override Eyes ToBodyPart()
		{
			return new Eyes(this);
		}
	}
}
