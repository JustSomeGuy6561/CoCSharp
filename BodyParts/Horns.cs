//Horns.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:50 AM
using System;
using CoC.Tools;
using CoC.BodyParts.SpecialInteraction;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.

	//This class is so much harder to implement than i thought it'd be.
	public class Horns : BodyPartBase<Horns, HornType>, IGrowShrinkable, IMasculinityChangeAware
	{
		public override HornType type { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		public int significantHornSize { get; protected set; }
		public int numHorns { get; protected set; }

		protected int hornMasculinity = 50;

		protected Horns()
		{
			Restore();
		}
		public static Horns GenerateFace()
		{
			return new Horns();
		}

		public override bool Restore()
		{
			if (type == HornType.NONE)
			{
				return false;
			}
			type = HornType.NONE;
			numHorns = 0;
			significantHornSize = 0;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == HornType.NONE)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			Restore();
			return true;
		}

		#region IGrowShrinkable
		public bool CanReducto()
		{
			return type.CanShrink(significantHornSize);
		}

		public float UseReducto()
		{
			int len = significantHornSize;
			handleHornSet(type.ReductoHorns);
			return len - significantHornSize;
		}

		public bool CanGrowPlus()
		{
			return type.CanGrow(significantHornSize);
		}

		public float UseGroPlus()
		{
			int len = significantHornSize;
			handleHornSet(type.GroPlusHorns);
			return significantHornSize - len;
		}
		#endregion
		private T handleHornTransforms<T>(uint byAmount, bool male, TransformDelegate<T> handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			T retVal = handle(byAmount, ref hornCount, ref hornSize, male);
			numHorns = hornCount;
			significantHornSize = hornSize;
			return retVal;
		}

		//generic function pointer taking uint, int, int, bool. return type can be specified manually.
		private delegate T TransformDelegate<T>(uint amt, ref int ct, ref int sz, bool isTrue);

		private T handleHornSet<T>(SetHornDelegate<T> handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			T retVal = handle(ref hornCount, ref hornSize);
			numHorns = hornCount;
			significantHornSize = hornSize;
			return retVal;
		}

		private delegate T SetHornDelegate<T>(ref int ct, ref int sz);
		/*

#region Class Functions

public bool UpdateHorns(HornType newHorns)
{
	if (newHorns == type)
	{
		return false;
	}
	type = newHorns;
	return true;
}

/// <summary>
/// Attempt to change horns, failing if it is already this type. if successful,
/// resets the number of horns to this type's default, then grows or adds horns 
/// according to this type's strengthening rules. it will do so (amount) times.
/// </summary>
/// <param name="newHorns">the type of horns we should change to.</param>
/// <param name="masculine">is the creature these horns belong to masculine?</param>
/// <param name="amount">number of times it will be strengthened, according to type behavior</param>
/// <returns>whether or not it changed types.</returns>
public bool ChangeAndStrengthenHorns(HornType newHorns, bool masculine, uint amount)
{
	if (newHorns == type)
	{
		return false;
	}
	type = newHorns;
	if (amount > 0)
	{
		StrengthenTransform(masculine, amount);
	}
	type = newHorns;
	return true;
}

/// <summary>
/// Lengthens or Adds horns, according to how these horns behave. This behavior is
/// dependant on the horn type. for your convenience, you can provide this with an
/// amount, instead of calling this multiple times.
/// </summary>
/// <param name="masculine"></param>
/// <param name="numberOfTimes">number of times to strengthen these horns</param>
/// <returns></returns>
public bool StrengthenTransform(bool masculine, uint numberOfTimes = 1)
{
	return handleHornTransforms(numberOfTimes, masculine, type.StrengthenTransform);
}
public bool WeakenTransform(bool male, uint byAmount = 1)
{
	//because i can't pass a property by reference. delegates ftw!
	bool removeHorns = handleHornTransforms(byAmount, male, type.WeakenTransform);
	if (removeHorns && type != HornType.NONE)
	{
		Restore();
	}
	else //remove horns is false or it's true but we already have no horns.
	{
		removeHorns = false;
	}
	return removeHorns;
}
#endregion

#region Helpers
//I was writing the same thing all the time, so i thought "turn it into a function"
//turns out function pointers are a pain in c#, when dealing with "ref". CLR nonsense.
//Takes a uint, bool, and function. copies the properties (which can't be passed by 
//reference), then executes the function. the return value is stored, the properties
//set to the new values, and then the function's return is returned.
private T handleHornTransforms<T>(uint byAmount, bool male, TransformDelegate<T> handle)
{
	int hornCount, hornSize;
	hornCount = numHorns;
	hornSize = largestHornLength;
	T retVal = handle(byAmount, ref hornCount, ref hornSize, male);
	numHorns = hornCount;
	largestHornLength = hornSize;
	return retVal;
}

//generic function pointer taking uint, int, int, bool. return type can be specified manually.
private delegate T TransformDelegate<T>(uint amt, ref int ct, ref int sz, bool isTrue);

private T handleHornSet<T>(SetHornDelegate<T> handle)
{
	int hornCount, hornSize;
	hornCount = numHorns;
	hornSize = largestHornLength;
	T retVal = handle(ref hornCount, ref hornSize);
	numHorns = hornCount;
	largestHornLength = hornSize;
	return retVal;
}

//generic function pointer taking uint, int, int, bool. return type can be specified manually.
private delegate T SetHornDelegate<T>(ref int ct, ref int sz);

#endregion*/

	}

	public abstract class HornType : BodyPartBehavior<HornType, Horns>
	{
		#region variables
		//private vars
		private const int MAX_HORN_LENGTH = 40;
		//index mgic
		private static int indexMaker = 0;

		//members
		private readonly int _index;
		public readonly int maxHorns;
		public readonly int minHorns;
		public readonly int defaultHorns;
		public readonly int defaultLength;
		public readonly int maxHornLength;
		public readonly int minHornLength;

		//call the other constructor with defaults set to min.
		protected HornType(int minHorns, int maximumHorns, int minLength, int maxLength,
			GenericDescription shortDesc, CreatureDescription<Horns> creatureDesc, PlayerDescription<Horns> playerDesc, ChangeType<Horns> transform, ChangeType<Horns> restore) 
			: this(minHorns, maximumHorns, minLength, maxLength, minHorns, minLength, shortDesc, creatureDesc, playerDesc, transform, restore) {}

		protected HornType(int minimumHorns, int maximumHorns, int minLength, int maxLength, int defaultHornCount, int defaultHornLength,
			GenericDescription shortDesc, CreatureDescription<Horns> creatureDesc, PlayerDescription<Horns> playerDesc,
			ChangeType<Horns> transform, ChangeType<Horns> restore) : base(shortDesc, creatureDesc, playerDesc, transform, restore)
		{
			//Woo data cleanup.
			Utils.Clamp(ref maximumHorns, 0, int.MaxValue);
			Utils.Clamp(ref minimumHorns, 0, maximumHorns);
			Utils.Clamp(ref maxLength, 0, MAX_HORN_LENGTH);
			Utils.Clamp(ref minLength, 0, maxLength);
			Utils.Clamp(ref defaultHornCount, minHorns, maxHorns);
			Utils.Clamp(ref defaultHornLength, minLength, maxLength);
			//and now set them. finally
			maxHorns = maximumHorns;
			minHorns = minimumHorns;
			maxHornLength = maxLength;
			minHornLength = minLength;
			defaultHorns = defaultHornCount;
			defaultLength = defaultHornLength;
			//and the static magic.
			_index = indexMaker++;
		}

		//properties
		public bool allowsHorns
		{
			get
			{
				return maxHorns > 0;
			}
		}
		#endregion

		public override int index => _index;

		/*
		#region Constructors

		//Overload for laziness. Default amount is the min amount.
		
		protected HornType(int minHorns, int maximumHorns, int minLength, int maxLength) : this(minHorns, maximumHorns, minLength, maxLength, minHorns, minLength) { }
		protected HornType(int minimumHorns, int maximumHorns, int minLength, int maxLength, int defaultHornCount, int defaultHornLength)
		{
			//Woo data cleanup.
			Tools.Utils.Clamp(ref maximumHorns, 0, int.MaxValue);
			Tools.Utils.Clamp(ref minimumHorns, 0, maximumHorns);
			Tools.Utils.Clamp(ref maxLength, 0, MAX_HORN_LENGTH);
			Tools.Utils.Clamp(ref minLength, 0, maxLength);
			Tools.Utils.Clamp(ref defaultHornCount, minHorns, maxHorns);
			Tools.Utils.Clamp(ref defaultHornLength, minLength, maxLength);
			//and now set them. finally
			maxHorns = maximumHorns;
			minHorns = minimumHorns;
			maxHornLength = maxLength;
			minHornLength = minLength;
			defaultHorns = defaultHornCount;
			defaultLength = defaultHornLength;
			//and the static magic.
			index = indexMaker++;
		}
		#endregion

		#region Abstracts

		//Reducto/Gro+ related	
		internal abstract bool CanShrink(int largestHornLength);
		public abstract bool ReductoHorns(ref int numHorns, ref int hornLength);
		internal abstract bool CanGrow(int largestHornLength);
		public abstract bool GroPlusHorns(ref int numHorns, ref int hornLength);

		public abstract bool StrengthenTransform(uint byAmount, ref int numHorns, ref int largestHornLength, bool male);

		public abstract bool WeakenTransform(uint byAmount, ref int numHorns, ref int largestHornLength, bool masculine);

		internal bool SetHornsToDefault(ref int numHorns, ref int maxHornLength)
		{
			if (numHorns == defaultHorns && maxHornLength == defaultLength)
			{
				return false;
			}
			numHorns = defaultHorns;
			maxHornLength = defaultLength;
			return true;
		}



		#endregion
		#region TYPES
		public static readonly HornType NONE = new GenericHorns(0, 0, "");
		public static readonly HornType DEMON = new DemonHorns();
		public static readonly HornType BULL_LIKE = new BullHorns();
		public static readonly HornType DRACONIC = new DragonHorns();

		//Fun fact: female reindeer (aka caribou in North America) grow horns.
		//no other species of deer do that. which leads to the weird distinction here.
		//I've tried to remove clones, but i think this is the exception.
		//On that note, water deer have husks, not horns. I'm, not adding them.
		public static readonly HornType DEER_ANTLERS = new Antlers(false);
		public static readonly HornType REINDEER_ANTLERS = new Antlers(true);

		public static readonly HornType SATYR = new GoatHorns();
		public static readonly HornType UNICORN = new UniHorn();
		public static readonly HornType RHINO = new RhinoHorn();
		public static readonly HornType SHEEP = new SheepHorns();

		public static readonly HornType IMP = new GenericHorns(2, 3, "a pair of short, imp-like horns");
		#endregion

		#region Implementations
		private class GenericHorns : HornType
		{
			private readonly string desc;
			public GenericHorns(int hornCount, int hornLength, string description) : base(hornCount, hornCount, hornLength, hornLength)
			{
				desc = description;
			}

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				return desc;
			}

			internal override bool CanShrink(int largestHornLength)
			{
				return false;
			}
			public override bool ReductoHorns(ref int numHorns, ref int maxHornLength)
			{
				return false;
			}

			internal override bool CanGrow(int largestHornLength)
			{
				return false;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int maxHornLength)
			{
				return false;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int maxHornLength, bool male)
			{
				return false;
			}

			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int maxHornLength, bool male)
			{
				return true;
			}
		}
		//----------------------------------------
		private class DemonHorns : HornType
		{
			public DemonHorns() : base(2, 12, 2, 10) { }

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				if (numHorns == 2)
				{
					return "a pair of " + hornLength.ToString() + " inch demon horns";
				}
				else
				{
					return numHorns + "demonic horns. The front pair are " + hornLength.ToString() + "inches and point forward menacingly. The rest curve back along your head.";
				}
			}

			internal override bool CanShrink(int largestHornLength)
			{
				return false;
			}
			public override bool ReductoHorns(ref int numHorns, ref int largestHornLength)
			{
				return false;
			}

			internal override bool CanGrow(int largestHornLength)
			{
				return false;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int largestHornLength)
			{
				return false;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool male)
			{
				Tools.Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				numHorns = Math.Min(maxHorns, numHorns + (2 * (int)byAmount));

				if (numHorns >= 8)
				{
					hornLength = 10;
				}
				else if (numHorns >= 6)
				{
					hornLength = 8;
				}
				else if (numHorns >= 4)
				{
					hornLength = 4;
				}
				else
				{
					hornLength = 2;
				}
				return true;
			}
			//Lose 4-6 horns. if that makes it 0 horns, return true
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool male)
			{
				if (numHorns > 6)
				{
					numHorns -= 6;
				}
				else
				{
					numHorns = Math.Max(0, numHorns - 4);
				}
				if (numHorns <= 2)
				{
					hornLength = 2;
				}
				else if (hornLength <= 4)
				{
					hornLength = 4;
				}
				else
				{
					hornLength = 8;
				}
				return numHorns <= 0;
			}
		}

		private class BullHorns : HornType
		{
			private readonly int maxFeminineHornLength;
			public BullHorns() : base(2, 2, 2, 40)
			{
				maxFeminineHornLength = 5;
			}

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				if (hornLength < 3)
					return "a pair of small nubs, like those on a young bovine";
				else if (hornLength < 6)
					return "a pair of moderately-sized, " + hornLength.ToString() + " inch bovine horns";
				else if (hornLength < 12)
					return "two large horns, roughly " + hornLength.ToString() + " inches in length, curve forwards like those of a bull.";
				else if (hornLength < 20)
					return "two very large and dangerous looking horns, curving forward, reaching at least " + (hornLength == 12 ? "a foot" : hornLength.ToString() + "inches") + ". They have dangerous looking points.";
				else //if (player.horns.value >= 20)
					return "two huge horns, curving outward at first, then forwards. They reach at least " + hornLength.ToString() + "inches and end in sharp points. They look incredibly dangerous";
			}

			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int largestHornLength)
			{
				//For now, 3. Because i'm lazy.

				int reduceAmount = 0;
				if (largestHornLength > 12)
					reduceAmount = 6;
				else if (largestHornLength > minHornLength)
				{
					reduceAmount = Math.Min(3, largestHornLength - minHornLength);
				}
				largestHornLength -= reduceAmount;
				return reduceAmount != 0;
			}

			internal override bool CanGrow(int largestHornLength)
			{
				return false;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int largestHornLength)
			{
				return false;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				int maxLength = masculine ? maxHornLength : maxFeminineHornLength;
				if (hornLength >= maxLength || byAmount == 0)
				{
					return false;
				}
				while (byAmount > 0 && hornLength < maxLength)
				{
					if (!masculine)
					{
						hornLength = maxLength;
					}
					else
					{
						//grow horns 3-6 inches.
						hornLength += Tools.Utils.Rand(4) + 3;
					}
					byAmount--;
				}
				hornLength = hornLength > maxLength ? maxLength : hornLength;
				return true;

			}
			//Lose half of the length, down to 5inches. at that point, revert to nubs if female
			//or lose the rest if male. after that, lose them regardless.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				while (byAmount-- > 0 && hornLength > 0)
				{
					if (hornLength > 10)
					{
						hornLength /= 2;
					}
					else if (hornLength > maxFeminineHornLength)
					{
						hornLength = maxFeminineHornLength;
					}
					else if (!masculine && hornLength > minHornLength)
					{
						hornLength = minHornLength;
					}
					else
					{
						hornLength = 0;
					}
				}
				return hornLength == 0;
			}
		}

		private class DragonHorns : HornType
		{
			public DragonHorns() : base(2, 4, 4, 12) { }

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				if (numHorns == maxHorns)
					return "four draconic horns. The first pair of horns are " + hornLength + " inches. The second pair sits behind them and reaches one foot in length";
				else if (hornLength >= 8)
					return "a pair of long, " + hornLength.ToString() + " inch draconic horns";
				else
					return "a pair of " + hornLength.ToString() + " inch horns - relatively short for a dragon";
			}

			//Executive decision: second pair of dragon horns can't be shrunk. 
			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength)
			{
				int reductoVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Max(minHornLength, hornLength - reductoVal);
				return oldHornLength != hornLength;
			}

			internal override bool CanGrow(int hornLength)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength)
			{
				int groVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Min(maxHornLength, hornLength + groVal);
				return oldHornLength != hornLength;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if ((hornLength >= maxHornLength && numHorns >= maxHorns) || byAmount == 0)
				{
					return false;
				}
				while (byAmount > 0 && (hornLength < 12 || numHorns < maxHorns))
				{
					if (hornLength < 12)
					{
						hornLength += 2;
					}
					else if (numHorns != maxHorns)
					{
						numHorns = maxHorns;
					}
					byAmount--;
				}
				return true;

			}
			//if 4 horns, become 2 horns. then shrink horns to 6in. then remove them completely.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				while (byAmount > 0 && hornLength > 0)
				{
					if (numHorns > minHorns)
					{
						numHorns = minHorns;
					}
					else if (hornLength > 6)
					{
						hornLength = 6;
					}
					else
					{
						hornLength = 0;
					}
					byAmount--;
				}
				return hornLength == 0;
			}

		}

		//i technically don't support button bucks. Huh. i also can't spell.
		private class Antlers : HornType
		{
			private readonly bool isReindeer;
			public Antlers(bool reindeer) : base(2, 20, 6, reindeer ? 36 : 24)
			{
				isReindeer = reindeer;
			}

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				return "a rack of " + (numHorns % 2 == 1 ? "asymmetric " : "") + "antlers, with a total of" + numHorns.ToString() + " points.";
			}

			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength)
			{
				int reductoVal = 4;
				int oldHornCount = numHorns;
				numHorns = Math.Max(minHornLength, hornLength - reductoVal);
				setLengthFromHorns(numHorns, ref hornLength);
				return oldHornCount != numHorns;
			}

			internal override bool CanGrow(int hornLength)
			{
				return isReindeer && hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength)
			{
				if (!isReindeer || numHorns >= maxHorns)
				{
					return false;
				}
				int growHorns = (new Lottery<int>(3, 3, 4, 4, 5, 6)).Select();
				int oldHornCount = numHorns;
				numHorns += growHorns;
				Utils.Clamp(ref numHorns, 0, maxHorns);
				setLengthFromHorns(numHorns, ref hornLength);
				return oldHornCount != numHorns;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				//get value, then decrement. if you're not familiar with this, it's confusing, i know. but i always
				//forget to decrement the loop at the end, and infinite loops are worse.
				while (byAmount-- > 0 && numHorns < maxHorns)
				{
					int growHorns = (new Lottery<int>(3, 3, 4, 4, 5, 6)).Select();
					numHorns += growHorns;
					Utils.Clamp(ref numHorns, 0, maxHorns);
				}
				setLengthFromHorns(numHorns, ref hornLength);
				return true;

			}
			//if 4 horns, become 2 horns. then shrink horns to 6in. then remove them completely.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				//get value, then decrement. if you're not familiar with this, it's confusing, i know. but i always
				//forget to decrement the loop at the end, and infinite loops are worse.
				while (byAmount-- > 0 && numHorns > minHorns)
				{
					int removeHorns = (new Lottery<int>(3, 3, 4, 4, 5, 6)).Select();
					numHorns -= removeHorns;
					Utils.Clamp(ref numHorns, minHorns, maxHorns);
				}
				setLengthFromHorns(numHorns, ref hornLength);
				return byAmount > 0;
			}

			private void setLengthFromHorns(int hornCount, ref int hornLength)
			{
				if (!isReindeer || hornCount < 8)
				{
					hornLength = hornCount + 4;
				}
				else if (hornCount >= 16)
				{
					hornLength = hornCount + 16;
				}
				else if (hornCount >= 12)
				{
					hornLength = hornCount + 12;
				}
				else //if (hornCount >= 8)
				{
					hornLength = hornCount + 8;
				}

			}
		}

		private class GoatHorns : HornType
		{
			public GoatHorns() : base(2, 2, 1, 6) { }

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				return hornLength > minHornLength ? "a pair of " + hornLength + " inch goat horns. They are curved and patterned in ridges." : "a pair of short, nubby goat horns";
			}

			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength)
			{
				if (hornLength <= minHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();

				return true;
			}

			internal override bool CanGrow(int hornLength)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength)
			{
				if (hornLength >= maxHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();

				return true;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if (hornLength >= maxHornLength || byAmount == 0)
				{
					return false;
				}
				//get value, then decrement. if you're not familiar with this, it's confusing, i know. but i always
				//forget to decrement the loop at the end, and infinite loops are worse.
				while (byAmount-- > 0 && hornLength < maxHornLength)
				{
					if (hornLength + 1 == maxHornLength)
						hornLength++;
					else
						hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();
				}
				return true;

			}
			//nope.avi. they're so small there's just no point. you just lose them.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				hornLength = minHornLength;
				return true;
			}
		}

		//Get it? That made me laugh way harder than it should have (which is not at all).
		private class UniHorn : HornType
		{
			public UniHorn() : base(1, 1, 6, 12) { }

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
			}

			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength)
			{
				if (hornLength <= minHornLength)
					return false;
				else
				{
					hornLength = minHornLength;
					return true;
				}
			}

			internal override bool CanGrow(int hornLength)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength)
			{
				if (hornLength >= maxHornLength)
					return false;
				else
				{
					hornLength = maxHornLength;
					return true;
				}
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if (hornLength >= maxHornLength || byAmount == 0)
				{
					return false;
				}
				else
				{
					hornLength = maxHornLength;
					return true;
				}

			}
			//Just lose it (ah aah aah)
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				hornLength = minHornLength;
				return true;
			}
		}

		private class RhinoHorn : HornType
		{
			public RhinoHorn() : base(1, 2, 6, 12) { }

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
			}

			internal override bool CanShrink(int hornLength)
			{
				return false;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength)
			{
				return false;
			}

			internal override bool CanGrow(int hornLength)
			{
				return false;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength)
			{
				return false;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				else
				{
					numHorns = maxHorns;
					hornLength = maxHornLength;
					return true;
				}

			}
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				if (numHorns == maxHorns)
				{
					numHorns = minHorns;
					hornLength = minHornLength;
					return false;
				}
				return true;
			}
		}

		private class SheepHorns : HornType
		{
			private readonly int maxFeminineHornLength;
			public SheepHorns() : base(2, 2, 2, 30)
			{
				maxFeminineHornLength = 7;
			}

			internal override string GetDescriptor(int numHorns, int hornLength)
			{
				if (hornLength < 3)
					return "a pair of small sheep horns";
				else if (hornLength < maxFeminineHornLength)
					return "a pair of curved, " + hornLength.ToString() + " inch bovine horns";
				else if (hornLength <= maxHornLength / 2)
					return "two spiraled horns, roughly " + hornLength.ToString() + " inches in length.";
				else
					return "two very thick, very large, spiraled ram's horms. if unwound, they'd be at least " + hornLength.ToString() + " inches" + ". Getting rammed by these would hurt.";
			}

			internal override bool CanShrink(int hornLength)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int largestHornLength)
			{

				int reduceAmount = 0;
				if (largestHornLength > maxFeminineHornLength)
					reduceAmount = 6;
				else if (largestHornLength > minHornLength)
				{
					reduceAmount = Math.Min(2, largestHornLength - minHornLength);
				}
				largestHornLength -= reduceAmount;
				return reduceAmount != 0;
			}

			internal override bool CanGrow(int largestHornLength)
			{
				return false;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int largestHornLength)
			{
				return false;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				int maxLength = masculine ? maxHornLength : maxFeminineHornLength;
				if (hornLength >= maxLength || byAmount == 0)
				{
					return false;
				}
				while (byAmount-- > 0 && hornLength < maxLength)
				{
					if (!masculine)
					{
						if (hornLength + 3 >= maxLength)
						{
							hornLength = maxLength;
						}
						else
						{
							hornLength += 3;
						}
					}
					else
					{
						//grow horns 2-6 inches.
						hornLength += Utils.Rand(5) + 2;
					}
				}
				hornLength = hornLength > maxLength ? maxLength : hornLength;
				return true;

			}
			//if masculine, Lose third of length, down to max feminine length. 
			//if feminine, go to max feminine length immediately
			//after that go to min, then lose horns entirely.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, bool masculine)
			{
				while (byAmount-- > 0 && hornLength > minHornLength)
				{
					if (hornLength >= 12 && masculine)
					{
						hornLength = (int)Math.Floor(hornLength * 2.0 / 3);
					}
					else if (hornLength > maxFeminineHornLength)
					{
						hornLength = maxFeminineHornLength;
					}
					else // if (hornLength > minHornLength)
					{
						hornLength = minHornLength;
					}
				}
				return byAmount > 0;
			}
		}

		#endregion
		*/
	}
}