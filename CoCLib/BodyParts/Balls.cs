//Balls.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:57 PM
using CoC.BodyParts.SpecialInteraction;
using CoC.Strings;
using CoC.Tools;
using static CoC.Strings.BodyParts.CockNBallzStrings;
namespace CoC.BodyParts
{
	//As of this writing, you can't get more than one set of balls unless you start with more.
	//this is not my decision - it's simply how the game is rn. i've provided options to remove or add balls,
	//maxing at 8. 
	//though this probably needs more thought - is it two per sack, and multiple sacks? or is it all one sack?

	public class Balls: IGrowShrinkable
	{
#warning Uniball implementation needed, probably as a bool.
#warning add display descriptors.
		public const int MAX_BALLS_SIZE = 30;
		public const int MIN_BALLS_SIZE = 1;
		public const int DEFAULT_BALLS_SIZE = 2;

		public const int MAX_BALLS_COUNT = 8;
		public const int MIN_BALLS_COUNT = 1;
		public const int DEFAULT_BALLS_COUNT = 2;
		public int index => size;

		public bool hasBalls { get; protected set; }

		public int size
		{
			get => hasBalls ? _size : 0;
			set
			{
				Utils.Clamp(ref value, MIN_BALLS_SIZE, MAX_BALLS_SIZE);
				_size = value;
			}
		}
		private int _size;

		public int count
		{
			get => hasBalls ? _count : 0;
			set
			{
				Utils.Clamp(ref value, MIN_BALLS_COUNT, MAX_BALLS_COUNT);
				_count = value;
			}
		}
		private int _count;

		protected Balls()
		{
			hasBalls = false;
			count = DEFAULT_BALLS_COUNT;
			size = DEFAULT_BALLS_SIZE;
		}
		//use this to initialize the balls object when the creature has balls.
		public static Balls GenerateBalls()
		{
			return new Balls()
			{
				hasBalls = true
			};
		}
		public static Balls GenerateBalls(int ballCount, int ballSize)
		{
			return new Balls()
			{
				count = ballCount,
				size = ballSize,
				hasBalls = true
			};
		}
		//use this to initialize the balls object when the creature doesn't have balls
		public static Balls GenerateNoBalls()
		{
			return new Balls();
		}
		public string shortDescription()
		{
			if (hasBalls) return BallsDescript(count, size);
			else return GlobalStrings.None();
		}

		public DescriptorWithArg<Balls> fullDescription => BallsFullDesc;
		public TypeAndPlayerDelegate<Balls> TypeAndPlayerDelegate => BallsPlayerStr;

		//Grows a pair of balls. returns false if it already has balls. 
		public bool growBalls()
		{
			if (hasBalls)
			{
				return false;
			}
			else
			{
				count = DEFAULT_BALLS_COUNT;
				size = DEFAULT_BALLS_SIZE;
				hasBalls = true;
			}
			return true;
		}

		public bool growBalls(int numBalls, int  newSize = DEFAULT_BALLS_SIZE)
		{

			if (hasBalls)
			{
				return false;
			}
			else
			{
				Utils.Clamp(ref numBalls, MIN_BALLS_COUNT, MAX_BALLS_COUNT);
				Utils.Clamp(ref newSize, MIN_BALLS_SIZE, MAX_BALLS_SIZE);
				count = numBalls;
				size = newSize;
				hasBalls = true;
			}
			return true;
		}

		//Forces balls into uniball format. returns false if already a uniball
		//will grow them if needed. probably should add a size constraint too.
		//as uniballs require that in this game. will do when i can find it.
		public bool makeUniBall()
		{
#warning add check here for uniballs instead of 9999. will need to also make sure size is tiny.
			if (hasBalls && count == MIN_BALLS_COUNT && 9999==9999)
			{
				return false;
			}
			else
			{
				hasBalls = true;
				count = MIN_BALLS_COUNT;
				size = MIN_BALLS_SIZE;
#warning add uniball set here. idk if it's a perk or something. probably done internally with a bool.
				return true;
			}
		}

		public int addBalls(int addAmount)
		{
#warning add check for uniballs. either revert to duo balls then add, or just revert, idk.
			Utils.Clamp(ref addAmount, 0, MAX_BALLS_COUNT);
			if ((hasBalls && count == MAX_BALLS_COUNT) || addAmount == 0)
			{
				return 0;
			}
			else if (!hasBalls)
			{
				count = addAmount;
				return addAmount;
			}
			else
			{
				int originalCount = count;
				count += addAmount;
				return count - originalCount;
			}
		}

		public int removeBalls(int removeAmount)
		{
#warning if uniball, make sure to remove perk/reset flag or whatever.
			Utils.Clamp(ref removeAmount, 0, MAX_BALLS_COUNT);
			if (!hasBalls || removeAmount == 0)
			{
				return 0;
			}
			else if (removeAmount >= count)
			{
				hasBalls = false;
				size = DEFAULT_BALLS_SIZE;
				count = DEFAULT_BALLS_COUNT;
				return count;
			}
			else
			{
				count -= removeAmount;
				return removeAmount;
			}
		}
		public bool removeAllBalls()
		{
#warning if uniball, make sure to remove perk/reset flag or whatever.
			if (!hasBalls)
			{
				return false;
			}
			hasBalls = false;
			size = DEFAULT_BALLS_SIZE;
			count = DEFAULT_BALLS_COUNT;
			return true;
		}

		public int EnlargeBalls(int amount)
		{
#warning add check for uniballs. remove it if they grow, pretty much at all.
			if (!hasBalls || size == MAX_BALLS_SIZE)
			{
				return 0;
			}
			int originalSize = size;
			size += amount;
			return size - originalSize;
		}

		public int ShrinkBalls(int amount)
		{
			if (!hasBalls || size == MIN_BALLS_SIZE)
			{
				return 0;
			}
			int originalSize = size;
			size -= amount;
			return originalSize - size;
		}

		public bool CanReducto()
		{
			return size > MIN_BALLS_SIZE;
		}

		public float UseReducto()
		{
			int startVal = size;
			//even chance of 2 - 5.
			if (CanReducto())
			{
				size -= Utils.Rand(4) + 2;
			}
			return startVal - size;
		}

		public bool CanGrowPlus()
		{
			return size < MAX_BALLS_SIZE;
		}

		public float UseGroPlus()
		{
			int startVal = size;
			if (CanGrowPlus())
			{
				int randVal = Utils.Rand(16);
				//75% of 1 or 2, then 25% of 2-5
				if (randVal < 6) size += 1;
				else if (randVal < 13) size += 2;
				else if (randVal < 14) size += 3;
				else if (randVal < 15) size += 4;
				else size += 5;
			}
			return size - startVal;
		}
	}
}
