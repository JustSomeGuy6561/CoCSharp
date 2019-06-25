//Balls.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:57 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	//As of this writing, you can't get more than one set of balls unless you start with more.
	//this is not my decision - it's simply how the game is rn. i've provided options to remove or add balls,
	//maxing at 8. 
	//though this probably needs more thought - is it two per sack, and multiple sacks? or is it all one sack?

	//I'VE GOT BIG BALLS! OH, I'VE GOT BIG BALLS! THERE SUCH BIG BALLS! DIRTY BIG BALLS! HE'S GOT BIG BALLS! AND SHE'S GOT BIG BALLS! BUT WE'VE GOT THE BIGGEST BALLS OF THEM ALL!
	//i'll see if i can hide this as an easter egg is some text somewhere. 
	public sealed partial class Balls : SimpleSaveablePart<Balls>, IGrowShrinkable //IPerkAware ? is there a big balls perk? maybe an extra cum perk? i still think that should be part of genitals.
	{

		public const byte MAX_BALLS_SIZE = 30;
		public const byte MIN_BALLS_SIZE = 1;
		public const byte DEFAULT_BALLS_SIZE = 2;

		public const byte UNIBALL_SIZE_THRESHOLD = 1;

		public const byte MAX_BALLS_COUNT = 8;
		public const byte UNIBALL_COUNT = 1;
		public const byte DEFAULT_MIN_COUNT = 2;
		public const byte DEFAULT_BALLS_COUNT = 2;
		public int index => size;

		//recommend saving the hasBalls bool even though it is a determined property - in the event of malformed data, it allows an additional way to catch
		//if the save should have balls. 
		public bool hasBalls => count != 0;
		public bool uniBall => count == UNIBALL_COUNT;

		//Count and size get conviluted with self-validation, so i've removed what validation i can from these properties and placed it in helper functions.
		//basically, if the old count was 0 or the new count is 0, use the helpers.
		//if you dont use the helpers, make sure you set the count first - the size uses the count to parse the values passed into the setter.
		public byte count { get; private set; }

		public byte size
		{
			get => _size;
			private set
			{
				if (!hasBalls) value = 0;
				else Utils.Clamp(ref value, MIN_BALLS_SIZE, MAX_BALLS_SIZE);
				_size = value;
			}
		}
		private byte _size;

		private Balls(bool hasBalls)
		{
			setBalls(hasBalls);
		}

		private Balls(byte ballCount, byte ballSize)
		{
			setBalls(true, ballCount, ballSize);
		}

		//use this to initialize the balls object when the creature has balls.
		internal static Balls GenerateFromGender(Gender gender)
		{
			return new Balls(gender.HasFlag(Gender.MALE));
		}

		//balls with a size of <1 are allowed, but only if the ballcount is 1
		internal static Balls GenerateBalls(byte ballCount = DEFAULT_BALLS_COUNT, byte ballSize = DEFAULT_BALLS_SIZE)
		{
			return new Balls(ballCount, ballSize);
		}

		internal static Balls GenerateUniBall()
		{
			Balls balls = new Balls(false);
			balls.setUniBall(true);
			return balls;
		}

		//public string shortDescription()
		//{
		//	if (hasBalls) return BallsDescript(count, size, uniBall);
		//	else return GlobalStrings.None();
		//}

		//public DescriptorWithArg<Balls> fullDescription => BallsFullDesc;
		//public TypeAndPlayerDelegate<Balls> TypeAndPlayerDelegate => BallsPlayerStr;

		internal override bool Validate(bool correctInvalidData)
		{
			bool valid = true;
			//auto-Validate;
			size = size;
			
			//validate uniball. default corrective behavior is to remove uniball.
			if (uniBall && size > UNIBALL_SIZE_THRESHOLD)
			{
				if (correctInvalidData)
				{
					count++;
				}
				valid = false;
			}
			if (valid || correctInvalidData)
			{
				byte oldCount = count;
				Utils.Clamp(ref oldCount, UNIBALL_COUNT, MAX_BALLS_COUNT);

				valid &= count == oldCount;
				if (correctInvalidData)
				{
					count = oldCount;
				}
			}
			return valid;
		}

		//Grows a pair of balls. returns false if it already has balls. 
		internal bool growBalls()
		{
			if (hasBalls)
			{
				return false;
			}
			else
			{
				setBalls(true);
			}
			return true;
		}

		internal bool growBalls(byte numBalls, byte newSize = DEFAULT_BALLS_SIZE)
		{

			if (hasBalls)
			{
				return false;
			}
			else
			{
				setBalls(true, numBalls, newSize);
				return true;
			}
		}

		internal bool growUniBall()
		{
			if (hasBalls)
			{
				return false;
			}
			else
			{
				setUniBall(true);
				return true;
			}
		}

		internal bool makeUniBall()
		{
			if (hasBalls && uniBall)
			{
				return false;
			}
			else
			{
				setUniBall(true);
				return true;
			}
		}

		internal byte addBalls(byte addAmount)
		{
			Utils.Clamp(ref addAmount, (byte)0, MAX_BALLS_COUNT);

			if ((hasBalls && count == MAX_BALLS_COUNT) || addAmount == 0)
			{
				return 0;
			}
			if (hasBalls)
			{
				addAmount += count;
			}
			byte oldCount = count;
			setBalls(true, addAmount, size);
			return count.subtract(oldCount);
		}

		//
		internal int removeBalls(byte removeAmount)
		{
			Utils.Clamp(ref removeAmount, (byte)0, MAX_BALLS_COUNT);
			if (!hasBalls || removeAmount == 0)
			{
				return 0;
			}
			else if (removeAmount >= count)
			{
				byte retVal = count;
				setBalls(false);
				return retVal;
			}
			else
			{
				byte oldCount = count;
				count -= removeAmount;
				setBalls(true, count, size);
				return oldCount.subtract(count);
			}
		}
		internal bool removeAllBalls()
		{
			if (!hasBalls)
			{
				return false;
			}
			setBalls(false);
			return true;
		}

		internal byte EnlargeBalls(byte amount)
		{
			if (!hasBalls || size == MAX_BALLS_SIZE || amount == 0)
			{
				return 0;
			}
			if (uniBall && amount + size > UNIBALL_SIZE_THRESHOLD)
			{
				count++;
			}
			byte originalSize = size;
			size += amount;
			return size.subtract(originalSize);
		}

		internal byte ShrinkBalls(byte amount)
		{
			if (!hasBalls || size == MIN_BALLS_SIZE)
			{
				return 0;
			}
			byte originalSize = size;
			size -= amount;
			return originalSize.subtract(size);
		}

		bool IGrowShrinkable.CanReducto()
		{
			return size > MIN_BALLS_SIZE;
		}

		float IGrowShrinkable.UseReducto()
		{
			byte startVal = size;
			//even chance of 2 - 5.
			if (((IGrowShrinkable)this).CanReducto())
			{
				size -= (byte)(Utils.Rand(4) + 2);
			}
			return startVal - size;
		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return size < MAX_BALLS_SIZE;
		}

		//executive deicision: GRO+ always removes uniball. idgaf.
		float IGrowShrinkable.UseGroPlus()
		{
			int startVal = size;
			if (((IGrowShrinkable)this).CanGrowPlus())
			{
				if (uniBall)
				{
					count++;
				}
				int randVal = Utils.Rand(16);
				//75% of 1 or 2, then 25% of 2-5 which means 1,2: 3/8 and 2,3,4,5:1/16. In total, 2 has a 7/16 chance.
				if (randVal < 6) size += 1;
				else if (randVal < 13) size += 2;
				else if (randVal < 14) size += 3;
				else if (randVal < 15) size += 4;
				else size += 5;
			}
			return size - startVal;
		}

		private void setBalls(bool balls, byte numBalls = 0, byte ballSize = 0)
		{
			if (balls)
			{
				if (numBalls == 0) numBalls = DEFAULT_BALLS_COUNT;
				else Utils.Clamp(ref numBalls, DEFAULT_MIN_COUNT, MAX_BALLS_COUNT);

				if (numBalls % 2 == 1) numBalls--;

				if (ballSize == 0) ballSize = DEFAULT_BALLS_SIZE;
			}
			else
			{
				numBalls = 0;
				ballSize = 0;
			}
			count = numBalls;

			size = ballSize;
		}

		private void setUniBall(bool isUniBall)
		{
			if (isUniBall)
			{
				count = 1;
				size = 1;
			}
			else
			{
				count = 0;
				size = 0;
			}
		}
	}
}
