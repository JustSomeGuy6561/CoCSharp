//Eyes.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 1:32 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
namespace CoC.BodyParts
{
	public enum EYE_COLOR
	{
		AMBER, BLUE, BROWN, GRAY, GREEN, HAZEL, RED, VIOLET,
		//AND NOW THE CRAZY COLORS - WHY NOT?
		YELLOW, PINK, ORANGE, INDIGO, TAN
	}
	public class Eyes : BodyPartBehavior
	{
		EyeType eyeType
		{
			get
			{
				return _eyeType;
			}
			set
			{
				_eyeType = value;
				index = _eyeType.index;
			}
		}
		protected EyeType _eyeType = EyeType.HUMAN; 

		EYE_COLOR leftIrisColor;
		//People really like heterochromia in PCs.
		EYE_COLOR rightIrisColor;

		protected Eyes(EYE_COLOR color, EyeType type)
		{
			eyeType = type;
			leftIrisColor = color;
			rightIrisColor = color;
		}
		protected Eyes(EYE_COLOR leftEye, EYE_COLOR rightEye, EyeType type)
		{
			eyeType = type;
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
		}

		public static Eyes GenerateEyes(EYE_COLOR color = EYE_COLOR.GRAY)
		{
			return new Eyes(color, EyeType.HUMAN);
		}

		public static Eyes GenerateEyes(EYE_COLOR leftEye, EYE_COLOR rightEye)
		{
			return new Eyes(leftEye, rightEye, EyeType.HUMAN);
		}
		public static Eyes GenerateNonHumanEyes(EYE_COLOR leftEye, EYE_COLOR rightEye, EyeType type)
		{
			return new Eyes(leftEye, rightEye, type);
		}

		public void UpdateEyeColor(EYE_COLOR color)
		{
			leftIrisColor = color;
			rightIrisColor = color;
		}
		public void UpdateEyeColors(EYE_COLOR leftEye, EYE_COLOR rightEye)
		{
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
		}

		public override string GetDescriptor()
		{
			return descriptor;
		}
		public void UpdateEyeType(EyeType type)
		{
			eyeType = type;
		}

		public void UpdateEyes(EYE_COLOR leftEye, EYE_COLOR rightEye, EyeType type)
		{
			leftIrisColor = leftEye;
			rightIrisColor = rightEye;
			eyeType = type;
		}

		public void Restore(bool eyeColor = false)
		{
			eyeType = EyeType.HUMAN;
			if (eyeColor)
			{
				leftIrisColor = EYE_COLOR.GRAY;
				rightIrisColor = EYE_COLOR.GRAY;
			}
		}

		//Because of the convenience shit. Standard compares that need to be explicitly defined because
		//the non-standard ones are too.
		public bool Equals(Eyes other)
		{
			return this == other;
		}

		public static bool operator ==(Eyes first, Eyes second)
		{
			return first.eyeType == second.eyeType && first.leftIrisColor == second.leftIrisColor && first.rightIrisColor == second.rightIrisColor;
		}

		public static bool operator !=(Eyes first, Eyes second)
		{
			return first.eyeType != second.eyeType || first.leftIrisColor != second.leftIrisColor || first.rightIrisColor != second.rightIrisColor;
		}

		//Convenience. Because everyone loves that shit
		public bool Equals(EyeType other)
		{
			return eyeType == other;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator== (Eyes first, EyeType second)
		{
			return first.eyeType == second;
		}

		public static bool operator!= (Eyes first, EyeType second)
		{
			return first.eyeType != second;
		}

		//Testing
		public static readonly Eyes batman = new Eyes(EYE_COLOR.BLUE, EyeType.HUMAN);
		public static readonly Eyes robin = new Eyes(EYE_COLOR.BLUE, EyeType.HUMAN);
		public static readonly Eyes nightwing = new Eyes(EYE_COLOR.GREEN, EyeType.HUMAN);

	}
	public enum SCLERA_COLOR
	{
		WHITE, //Human/Anthropomorphic
		BLACK, //Sand Trap
		CLEAR//, //Everything else
		//RED   //Vampires? (silly mode, i guess)
	}
	public class EyeType : BodyPartBehavior
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

		//public 
		protected EyeType(string data, int numEyes = 2, SCLERA_COLOR color = SCLERA_COLOR.CLEAR)
		{
			eyeCount = numEyes;
			index = indexMaker++;
		}
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
		public static EyeType HUMAN      = new EyeType(SpeciesName.HUMAN + " ", color: SCLERA_COLOR.WHITE);
		public static EyeType SPIDER     = new EyeType(SpeciesName.HUMAN + " ", numEyes: 4);
		public static EyeType SAND_TRAP  = new EyeType(SpeciesName.SAND_TRAP + " ", color: SCLERA_COLOR.BLACK);
		public static EyeType LIZARD     = new EyeType(SpeciesName.LIZARD+ "-");
		public static EyeType DRAGON     = new EyeType(SpeciesName.DRAGON + "-");
		public static EyeType BASILISK   = new EyeType(SpeciesName.BASILISK + " ");
		public static EyeType WOLF       = new EyeType(SpeciesName.WOLF + " ");
		public static EyeType COCKATRICE = new EyeType(SpeciesName.COCKATRICE + " ");
		public static EyeType CAT        = new EyeType(SpeciesName.CAT + "-");
	}
}
