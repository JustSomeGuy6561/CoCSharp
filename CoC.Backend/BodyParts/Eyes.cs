//Eyes.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 1:32 AM
using CoC.Backend.Attacks;
using CoC.Backend.Attacks.BodyPartAttacks;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Races;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public enum EyeColor
	{
		AMBER, BLUE, BROWN, GRAY, GREEN, HAZEL, RED, VIOLET,
		//AND NOW THE CRAZY COLORS - WHY NOT?
		SILVER, YELLOW, PINK, ORANGE, INDIGO, TAN, BLACK
	}

	//Eye color is completely new territory, so bear with me. The player gets to pick an eye color (and we can potentially give them an item or interaction to change it)
	//or a pair of colors for heterochromia. Certain eye types in old code would unofficially set the eye color by saying it was a certain color in flavor text.
	//i've rewritten them to work with a sort of common ground - the transforms obviously alter the eye, and therefore may affect its appearance, but the original color 
	//is still used in some manner. For example, basilisk eyes are still blue, but they now use the player-chosen eye color for their lightning-like streaks. 
	//(unless the eyes are already blue, which then use white streaks). It's a bit more work, but i think it's nicer. 

	public sealed partial class Eyes : BehavioralSaveablePart<Eyes, EyeType, EyeData>, ICanAttackWith //Basilisk Eyes.
	{

		public EyeColor leftIrisColor
		{
			get => _leftIrisColor;
			private set => _leftIrisColor = _leftIrisColor.CheckValid(value);
		}
		private EyeColor _leftIrisColor;
		//People really like heterochromia in PCs.
		public EyeColor rightIrisColor
		{
			get => _rightIrisColor;
			private set => _rightIrisColor = _rightIrisColor.CheckValid(value);
		}
		private EyeColor _rightIrisColor;

		public bool isHeterochromia => leftIrisColor != rightIrisColor;
		public byte eyeCount => type.eyeCount;
		public bool isReptilian => type.isReptilianEyes;
		public override EyeType type { get; protected set; }

		public override EyeData AsReadOnlyData()
		{
			return new EyeData(this);
		}

		internal Eyes(Creature source, EyeType eyeType) : base(source)
		{
			type = eyeType ?? throw new ArgumentNullException(nameof(eyeType));
			leftIrisColor = type.defaultColor;
			rightIrisColor = type.defaultColor;
		}

		internal Eyes(Creature source, EyeType eyeType, EyeColor color) : this(source, eyeType, color, color)
		{ }
		internal Eyes(Creature source, EyeType eyeType, EyeColor leftEye, EyeColor rightEye) : base(source)
		{
			type = eyeType ?? throw new ArgumentNullException(nameof(eyeType));
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
		}

		public override EyeType defaultType => EyeType.defaultValue;

		internal Eyes(Creature source) : this(source, EyeType.defaultValue) { }
		
		//by design, there is no way to update eye color while changing types. Eye types are supposed to respect the current eye color.
		//if you REALLY want to change this, just call update, then call change. You'll probably want some unique flavor text, though, as
		//the calls to change color str and update str are not really designed with being called back to back in mind and may sound weird, idk.

		//update, restore are both fine as defaults.

		internal bool ChangeEyeColor(EyeColor color)
		{
			if (leftIrisColor == color && rightIrisColor == color)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			leftIrisColor = color;
			rightIrisColor = color;
			NotifyDataChanged(oldData);
			return true;
		}
		internal bool ChangeEyeColor(EyeColor leftEye, EyeColor rightEye)
		{
			if (leftIrisColor == leftEye && rightIrisColor == rightEye)
			{
				return false;
			}
			var oldData = AsReadOnlyData();
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
			NotifyDataChanged(oldData);
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
			EyeData OldData = null;
			if (type != EyeType.HUMAN || leftIrisColor != EyeType.HUMAN.defaultColor || leftIrisColor != rightIrisColor)
			{
				OldData = AsReadOnlyData();
			}
			type = EyeType.HUMAN;
			leftIrisColor = type.defaultColor;
			rightIrisColor = type.defaultColor;
			if (OldData != null)
			{
				NotifyDataChanged(OldData);
			}
		}

		internal override bool Validate(bool correctInvalidData)
		{
			var eyeType = type;
			bool valid = EyeType.Validate(ref eyeType, correctInvalidData);
			type = eyeType;
			//check left eye. skip this if the data is already invalid and we aren't correcting invalid data.
			//checks to see if the value is out of range for the Enum (C# doesn't check enums)
			if ((valid || correctInvalidData) && !Enum.IsDefined(typeof(EyeColor), (int)leftIrisColor))
			{
				if (correctInvalidData)
				{
					leftIrisColor = EyeColor.AMBER;
				}
				valid = false;
			}
			//check right eye. skip this if the data is already invalid and we aren't correcting invalid data.
			//checks to see if the value is out of range for the Enum (C# doesn't check enums)
			if ((valid || correctInvalidData) && !Enum.IsDefined(typeof(EyeColor), (int)rightIrisColor))
			{
				if (correctInvalidData)
				{
					rightIrisColor = EyeColor.AMBER;
				}
				valid = false;
			}
			return valid;
		}

		AttackBase ICanAttackWith.attack => type.attack;
		bool ICanAttackWith.canAttackWith() => type.attack != AttackBase.NO_ATTACK;
	}
	public enum ScleraColor
	{
		WHITE, //Human/Anthropomorphic
		BLACK, //Sand Trap
		CLEAR//, //Everything else
			 //RED   //Vampires? (silly mode, i guess)
	}
	public partial class EyeType : SaveableBehavior<EyeType, Eyes, EyeData>
	{
		private static int indexMaker = 0;
		private static readonly List<EyeType> eyes = new List<EyeType>();
		public static readonly ReadOnlyCollection<EyeType> availableTypes = new ReadOnlyCollection<EyeType>(eyes);

		//Normally the white of the human eye
		//Generally, animals' sclera are nearly invisible
		//Thanks, Sand Traps.
		public readonly ScleraColor scleraColor;

		public readonly byte eyeCount;
		public readonly EyeColor defaultColor;
		private readonly int _index;

		public static EyeType defaultValue => HUMAN;


		public bool isReptilianEyes => this == LIZARD || this == BASILISK || this == DRAGON;

		internal protected delegate string EyeChangeDelegate(EyeColor oldLeft, EyeColor newLeft, EyeColor oldRight, EyeColor newRight);

		internal readonly EyeChangeDelegate EyeChangeSpecial;

		private protected EyeType(EyeColor defaultEyeColor, EyeChangeDelegate eyeChange,
			SimpleDescriptor shortDesc, DescriptorWithArg<Eyes> fullDesc, TypeAndPlayerDelegate<Eyes> playerDesc, ChangeType<Eyes> transform,
			RestoreType<Eyes> restore, byte numEyes = 2, ScleraColor color = ScleraColor.CLEAR) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			EyeChangeSpecial = eyeChange;
			eyeCount = numEyes;
			defaultColor = defaultEyeColor;
			_index = indexMaker++;
			eyes.AddAt(this, _index);
			scleraColor = color;
		}
		public override int index => _index;

		internal virtual AttackBase attack => AttackBase.NO_ATTACK;

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

		internal static bool Validate(ref EyeType eyeType, bool correctInvalidData)
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

		public static EyeType HUMAN = new EyeType(Species.HUMAN.defaultEyeColor, HumanEyeChange, HumanShortStr, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr, color: ScleraColor.WHITE);
		public static EyeType SPIDER = new EyeType(Species.SPIDER.defaultEyeColor, SpiderEyeChange, SpiderShortStr, SpiderFullDesc, SpiderPlayerStr, SpiderTransformStr, SpiderRestoreStr, numEyes: 4);
		public static EyeType SAND_TRAP = new EyeType(Species.SAND_TRAP.defaultEyeColor, SandTrapEyeChange, SandTrapShortStr, SandTrapFullDesc, SandTrapPlayerStr, SandTrapTransformStr, SandTrapRestoreStr, color: ScleraColor.BLACK);
		public static EyeType LIZARD = new EyeType(Species.LIZARD.defaultEyeColor, LizardEyeChange, LizardShortStr, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static EyeType DRAGON = new EyeType(Species.DRAGON.defaultEyeColor, DragonEyeChange, DragonShortStr, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static EyeType BASILISK = new StoneStareEyeType(Species.BASILISK.defaultEyeColor, BasiliskEyeChange, BasiliskShortStr, BasiliskFullDesc, BasiliskPlayerStr, BasiliskTransformStr, BasiliskRestoreStr);
		public static EyeType WOLF = new EyeType(Species.WOLF.defaultEyeColor, WolfEyeChange, WolfShortStr, WolfFullDesc, WolfPlayerStr, WolfTransformStr, WolfRestoreStr);
		public static EyeType COCKATRICE = new StoneStareEyeType(Species.COCKATRICE.defaultEyeColor, CockatriceEyeChange, CockatriceShortStr, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
		public static EyeType CAT = new EyeType(Species.CAT.defaultEyeColor, CatEyeChange, CatShortStr, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);

		private class StoneStareEyeType : EyeType
		{
			internal override AttackBase attack => _attack;
			private static readonly AttackBase _attack = new BasiliskStare();
			public StoneStareEyeType(EyeColor defaultEyeColor, EyeChangeDelegate eyeChange, SimpleDescriptor shortDesc, DescriptorWithArg<Eyes> fullDesc, TypeAndPlayerDelegate<Eyes> playerDesc,
				ChangeType<Eyes> transform, RestoreType<Eyes> restore, byte numEyes = 2, ScleraColor color = ScleraColor.CLEAR)
				: base(defaultEyeColor, eyeChange, shortDesc, fullDesc, playerDesc, transform, restore, numEyes, color) { }
		}
	}

	public sealed class EyeData : BehavioralSaveablePartData<EyeData, Eyes, EyeType>
	{
		public readonly EyeColor leftIrisColor;
		public readonly EyeColor rightIrisColor;

		public readonly byte eyeCount;
		public readonly ScleraColor scleraColor;
		internal EyeData(Eyes source) : base(GetBehavior(source))
		{
			leftIrisColor = source.leftIrisColor;
			rightIrisColor = source.rightIrisColor;
			eyeCount = source.eyeCount;
			scleraColor = source.type.scleraColor;
		}
	}

}
