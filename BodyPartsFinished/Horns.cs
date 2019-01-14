//Horns.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:50 AM
using System;
using CoC.Tools;
using CoC.BodyParts.SpecialInteraction;
using static CoC.UI.TextOutput;
using static CoC.Strings.BodyParts.HornStrings;
namespace CoC.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.

	//This class is so much harder to implement than i thought it'd be.
	public class Horns : BodyPartBase<Horns, HornType>, IGrowShrinkable, IMasculinityChangeAware
	{
		public override HornType type { get; protected set; }

		public int significantHornSize { get; protected set; }
		public int numHorns { get; protected set; }

		protected int hornMasculinity = 50;

		protected Horns()
		{
			type = HornType.NONE;
			numHorns = 0;
			significantHornSize = 0;
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

		public bool UpdateType(HornType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			numHorns = type.defaultHorns;
			significantHornSize = type.defaultLength;
			return true;
		}

		public bool UpdateTypeAndDisplayMessage(HornType newType, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateType(newType);
		}

		public bool UpdateTypeAndStrengthen(HornType newType, uint byAmount)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			numHorns = type.defaultHorns;
			significantHornSize = type.defaultLength;
			StrengthenTransform(byAmount);
			return true;
		}

		public bool UpdateTypeDisplayMessageAndStrengthen(HornType newType, uint byAmount, Player player)
		{
			if (type == newType)
			{
				return false;
			}
			OutputText(transformInto(newType, player));
			return UpdateType(newType);
		}

		public bool StrengthenTransform(uint numberOfTimes = 1)
		{
			if (numberOfTimes == 0)
			{
				return false;
			}
			return handleHornTransforms(numberOfTimes, type.StrengthenTransform);
		}
		public bool WeakenTransform(uint byAmount = 1)
		{
			if (byAmount == 0)
			{
				return false;
			}
			//because i can't pass a property by reference. delegates ftw!
			bool removeHorns = handleHornTransforms(byAmount, type.WeakenTransform);
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

		#region IGrowShrinkable
		public bool CanReducto()
		{
			return type.CanShrink(significantHornSize, hornMasculinity);
		}



		public float UseReducto()
		{
			if (!CanReducto())
			{
				return 0;
			}
			int len = significantHornSize;
			handleHornSet(type.ReductoHorns);
			return len - significantHornSize;
		}

		public bool CanGrowPlus()
		{
			return type.CanGrow(significantHornSize, hornMasculinity);
		}

		public float UseGroPlus()
		{
			if (!CanGrowPlus())
			{
				return 0;
			}
			int len = significantHornSize;
			handleHornSet(type.GroPlusHorns);
			return significantHornSize - len;
		}
		#endregion
		#region Masculinity
		public void reactToChangesInMasculinity(int masculinity)
		{
			hornMasculinity = masculinity;
			handleHornSet(makeBoolReturn);
		}
		#endregion
		private T handleHornTransforms<T>(uint byAmount, TransformDelegate<T> handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			T retVal = handle(byAmount, ref hornCount, ref hornSize, hornMasculinity);
			numHorns = hornCount;
			significantHornSize = hornSize;
			return retVal;
		}

		//generic function pointer taking uint, int, int, bool. return type can be specified manually.
		private delegate T TransformDelegate<T>(uint amt, ref int ct, ref int sz, int masc);

		private T handleHornSet<T>(SetHornDelegate<T> handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			T retVal = handle(ref hornCount, ref hornSize, hornMasculinity);
			numHorns = hornCount;
			significantHornSize = hornSize;
			return retVal;
		}

		private bool makeBoolReturn(ref int ct, ref int sz, int masc)
		{
			type.reactToChangesInMasculinity(ref ct, ref sz, masc);
			return true;
		}

		private delegate T SetHornDelegate<T>(ref int ct, ref int sz, int masc);
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

#endregion*/

	}

	//i could go with function pointers throughout this, but frankly it's complicated enough that it might as well just be abstract.
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

		//properties
		public bool allowsHorns => maxHorns > 0;

		public override int index => _index;
		#endregion
		//call the other constructor with defaults set to min.
		protected HornType(int minHorns, int maximumHorns, int minLength, int maxLength,
			GenericDescription shortDesc, FullDescription<Horns> fullDesc, PlayerDescription<Horns> playerDesc, ChangeType<Horns> transform, ChangeType<Horns> restore)
			: this(minHorns, maximumHorns, minLength, maxLength, minHorns, minLength, shortDesc, fullDesc, playerDesc, transform, restore) { }

		protected HornType(int minimumHorns, int maximumHorns, int minLength, int maxLength, int defaultHornCount, int defaultHornLength,
			GenericDescription shortDesc, FullDescription<Horns> fullDesc, PlayerDescription<Horns> playerDesc,
			ChangeType<Horns> transform, ChangeType<Horns> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
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

		public virtual void reactToChangesInMasculinity(ref int hornCount, ref int hornLength, int masculinity) { } // do nothing. 
		public virtual bool CanShrink(int largestHornLength, int masculinity)
		{
			return false;
		}
		public virtual bool ReductoHorns(ref int numHorns, ref int maxHornLength, int masculinity)
		{
			return false;
		}

		public virtual bool CanGrow(int largestHornLength, int masculinity)
		{
			return false;
		}

		public virtual bool GroPlusHorns(ref int numHorns, ref int maxHornLength, int masculinity)
		{
			return false;
		}
		/// <summary>
		/// makes the horns more closely resemble the fully morphed form of horms. most of the time it will grow and/or add horns.
		/// though, if you're particularly feminine and overly large horns, it may feminize them by shrinking.
		/// </summary>
		/// <param name="byAmount">number of times to procc the growth or lengthen</param>
		/// <param name="numHorns">number of current horns. may be updated with a new amount</param>
		/// <param name="significantHornLength">length of the current significant horn. it may be updated with a new amount</param>
		/// <param name="masculinity">0-100 integer value that represents the masculinity or femininity of the creature.</param>
		/// <returns>true if any changes were made to the horn length and/or count. false otherwise.</returns>
		public virtual bool StrengthenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, int masculinity)
		{
			return false;
		}

		/// <summary>
		/// shrinks and/or removes horns. defaults to removing all horns. non-default behavior is type dependant.
		/// </summary>
		/// <param name="byAmount">number of times to procc shrink or horn loss.</param>
		/// <param name="numHorns">number of horns currently. may be updated with new amount</param>
		/// <param name="significantHornLength">length of current significant horns. may be updated with new amount</param>
		/// <param name="masculinity">0-100 integer value that represents the masculinity or femininity of the creature.</param>
		/// <returns>true if all horns were removed. false otherwise.</returns>
		public virtual bool WeakenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, int masculinity)
		{
			numHorns = 0;
			significantHornLength = 0;
			return true;
		}


		public static readonly HornType NONE = new SimpleOrNoHorns(0, 0, NoHornsShortDesc, NoHornsFullDesc, NoHornsPlayerStr, NoHornsTransformStr, NoHornsRestoreStr);
		public static readonly HornType DEMON = new DemonHorns();
		public static readonly HornType BULL_LIKE = new BullHorns(); //female aware. fuck me.
		public static readonly HornType DRACONIC = new DragonHorns();
		//Fun fact: female reindeer (aka caribou in North America) grow horns. no other species of deer do that. which leads to the weird distinction here.
		//I've tried to remove clones, but i think this is the exception. On that note, water deer have long teeth, not horns. I'm, not adding them.
		public static readonly HornType DEER_ANTLERS = new Antlers(false, 24, DeerShortDesc, DeerFullDesc, DeerPlayerStr, DeerTransformStr, DeerRestoreStr);
		public static readonly HornType REINDEER_ANTLERS = new Antlers(true, 36, ReindeerShortDesc, ReindeerFullDesc, ReindeerPlayerStr, ReindeerTransformStr, ReindeerRestoreStr);

		public static readonly HornType SATYR = new GoatHorns();
		public static readonly HornType UNICORN = new UniHorn();
		public static readonly HornType RHINO = new RhinoHorn();
		public static readonly HornType SHEEP = new SheepHorns(); //female aware. see above.

		public static readonly HornType IMP = new SimpleOrNoHorns(2, 3, ImpShortDesc, ImpFullDesc, ImpPlayerStr, ImpTransformStr, ImpRestoreStr);//"a pair of short, imp-like horns");

		//these horns are immutable - if you have them, they do not grow or shrink, and you can't get any more of them.
		private class SimpleOrNoHorns : HornType
		{
			public SimpleOrNoHorns(int hornCount, int hornLength,
				GenericDescription shortDesc, FullDescription<Horns> fullDesc, PlayerDescription<Horns> playerDesc, ChangeType<Horns> transform,
				ChangeType<Horns> restore) : base(hornCount, hornCount, hornLength, hornLength, shortDesc, fullDesc, playerDesc, transform, restore) { }
		}

		private class DemonHorns : HornType
		{
			public DemonHorns() : base(2, 12, 2, 10, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				Tools.Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				numHorns = Math.Min(maxHorns, numHorns + (2 * (int)byAmount));
				hornLength = demonLengthFromHornCount(numHorns);
				return true;
			}
			//Lose 4-6 horns. if that makes it 0 horns, return true
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				if (byAmount == 0)
				{
					return false;
				}
				while (byAmount-- > 0 && numHorns > 0)
				{
					numHorns -= numHorns > 6 ? 6 : numHorns > 4 ? 4 : numHorns;
				}
				hornLength = demonLengthFromHornCount(numHorns);
				return numHorns <= 0;
			}

			private int demonLengthFromHornCount(int hornCount)
			{
				if (hornCount >= 8) return 10;
				else if (hornCount >= 6) return 8;
				else if (hornCount >= 4) return 4;
				else return 2;
			}

		}

		private class BullHorns : HornType
		{
			private const int MAX_FEMININE_HORN_LENGTH = 5;
			public BullHorns() : base(2, 2, 2, MAX_HORN_LENGTH, BullShortDesc, BullFullDesc, BullPlayerStr, BullTransformStr, BullRestoreStr)
			{
				if (minHornLength > MAX_FEMININE_HORN_LENGTH)
				{
					throw new System.ArgumentException("minimum horn length must be less than the max feminine length. should never procc.");
				}
			}

			public override void reactToChangesInMasculinity(ref int hornCount, ref int hornLength, int masculinity)
			{
				if (isHyperFeminine(masculinity))
				{
					int x = 2;
					feminizeHorns(ref x, ref hornLength);
				}
				else if (isSlightlyFeminine(masculinity))
				{
					int x = 1;
					feminizeHorns(ref x, ref hornLength);
				}
				else if (isAndrogenous(masculinity))
				{
					//do nothing
				}
				else if (isSlightlyMasculine(masculinity))
				{
					if (hornLength < 12)
					{
						hornLength++;
					}
				}
				else //hyper masculine.
				{
					if (hornLength < 24)
					{
						hornLength += 2;
					}
				}
			}

			public override bool CanShrink(int largestHornLength, int masculinity)
			{
				return largestHornLength > minHornLength;
			}

			public override bool ReductoHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				if (!CanShrink(hornLength, masculinity))
				{
					return false;
				}
				int reduceAmount = 0;
				//large horns and female? remove a lot.
				if (isFeminine(masculinity) && hornLength > 18)
				{
					reduceAmount = 12;
				}
				//less large, but still large, and female? remove some.
				else if (isFeminine(masculinity) && hornLength > 10)
				{
					reduceAmount = 6;
				}
				//smaller, but still above female max, and female? make female max.
				else if (isFeminine(masculinity) && hornLength > MAX_FEMININE_HORN_LENGTH)
				{
					reduceAmount = hornLength - MAX_FEMININE_HORN_LENGTH;
				}
				//female and female horns above min size? make them min size
				else if (isFeminine(masculinity) && hornLength > minHornLength)
				{
					reduceAmount = hornLength - minHornLength;
				}
				//not feminine and large? remove some
				else if (hornLength >= 10)
				{
					reduceAmount = 6;
				}
				//not feminine and smallish? make feminine max.
				else if (hornLength > MAX_FEMININE_HORN_LENGTH)
				{
					reduceAmount = hornLength - MAX_FEMININE_HORN_LENGTH;
				}
				//feminine and at min or not feminine and at feminine max or smaller? remove them.
				else //(hornLength <= max feminine length and masculine
				{
					reduceAmount = hornLength;
				}
				hornLength -= reduceAmount;
				return reduceAmount != 0;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				if (byAmount == 0 || (!isFeminine(masculinity) && hornLength >= maxHornLength)
				{
					return false;
				}
				else if (isFeminine(masculinity))
				{
					Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
					int amount = (int)byAmount;
					feminizeHorns(ref amount, ref hornLength); //grow or lengthen until it reaches feminine max.
					return true;
				}
				else
				{
					while (byAmount-- > 0 && hornLength < maxHornLength)
					{
						//grow horns 3-6 inches.
						hornLength += Tools.Utils.Rand(4) + 3;
					}
					hornLength = hornLength > maxHornLength ? maxHornLength : hornLength;
					return true;
				}
			}
			//Lose half of the length, down to 5inches. at that point, revert to nubs if female
			//or lose the rest if male. after that, lose them regardless.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				//early exit: no amount
				if (byAmount == 0)
				{
					return false;
				}
				Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				//early exit, non-zero count and at minimum.
				if (hornLength == minHornLength || (hornLength <= MAX_FEMININE_HORN_LENGTH && isMasculine(masculinity)))
				{
					numHorns = 0;
					hornLength = 0;
					return true;
				}
				//feminine and horns are at or above max length for feminine characters.
				else if (isFeminine(masculinity) && hornLength >= MAX_FEMININE_HORN_LENGTH)
				{
					int amount = (int)byAmount;
					feminizeHorns(ref amount, ref hornLength);
					if (amount > 1)
					{
						numHorns = 0;
						hornLength = 0;
						return true;
					}
					else if (amount == 1)
					{
						hornLength = minHornLength;
					}
					return false;
				}
				//feminine and horns are less than max. 
				else if (isFeminine(masculinity) && hornLength < MAX_FEMININE_HORN_LENGTH)
				{
					if (byAmount > 1)
					{
						numHorns = 0;
						hornLength = 0;
						return true;
					}
					else if (byAmount == 1)
					{
						hornLength = minHornLength;
					}
					return false;
				}
				//masculine and horms above feminine min.
				else
				{
					while (byAmount-- > 0 && hornLength > 0)
					{
						if (hornLength > 10)
						{
							hornLength /= 2;
						}
						else if (hornLength > MAX_FEMININE_HORN_LENGTH)
						{
							hornLength = MAX_FEMININE_HORN_LENGTH;
						}
						else
						{
							hornLength = 0;
						}
					}
					return hornLength == 0;
				}
			}
			//grows or shrinks 
			private void feminizeHorns(ref int amount, ref int hornLength)
			{
				if (hornLength == MAX_FEMININE_HORN_LENGTH)
				{
					return;
				}
				else if (hornLength < MAX_FEMININE_HORN_LENGTH)
				{
					amount--;
					hornLength = MAX_FEMININE_HORN_LENGTH;
				}
				else
				{
					while (amount-- > 0 && hornLength > MAX_FEMININE_HORN_LENGTH)
					{
						if (hornLength > 10)
						{
							hornLength /= 2;
						}
						else if (hornLength > MAX_FEMININE_HORN_LENGTH)
						{
							hornLength = MAX_FEMININE_HORN_LENGTH;
						}
					}
				}
			}

		}

		private class DragonHorns : HornType
		{
			public DragonHorns() : base(2, 4, 4, 12, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr) { }

			//Executive decision: second pair of dragon horns can't be shrunk. 
			public override bool CanShrink(int hornLength, int masculinity)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				int reductoVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Max(minHornLength, hornLength - reductoVal);
				return oldHornLength != hornLength;
			}

			public override bool CanGrow(int hornLength, int masculinity)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				int groVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Min(maxHornLength, hornLength + groVal);
				return oldHornLength != hornLength;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
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
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
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
			public Antlers(bool reindeer, int maxLength,
				GenericDescription shortDesc, FullDescription<Horns> fullDesc, PlayerDescription<Horns> playerDesc,
				ChangeType<Horns> transform, ChangeType<Horns> restore) : base(2, 20, 6, maxLength, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				isReindeer = reindeer;
			}

			public override bool CanShrink(int hornLength, int masculinity)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				int reductoVal = 4;
				int oldHornCount = numHorns;
				numHorns = Math.Max(minHornLength, hornLength - reductoVal);
				setLengthFromHorns(numHorns, ref hornLength);
				return oldHornCount != numHorns;
			}

			public override bool CanGrow(int hornLength, int masculinity)
			{
				return isReindeer && hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength, int masculinitiy)
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

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
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
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
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
			public GoatHorns() : base(2, 2, 1, 6, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr) { }

			public override bool CanShrink(int hornLength, int masculinity)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				if (hornLength <= minHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();

				return true;
			}

			public override bool CanGrow(int hornLength, int masculinity)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				if (hornLength >= maxHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();
				return true;
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
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
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				hornLength = 0;
				return true;
			}
		}

		//Get it? That made me laugh way harder than it should have (which is not at all).
		private class UniHorn : HornType
		{
			public UniHorn() : base(1, 1, 6, 12, UniHornShortDesc, UniHornFullDesc, UniHornPlayerStr, UniHornTransformStr, UniHornRestoreStr) { }

			public override bool CanShrink(int hornLength, int masculinity)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				if (hornLength <= minHornLength)
					return false;
				else
				{
					hornLength = minHornLength;
					return true;
				}
			}

			public override bool CanGrow(int hornLength, int masculinity)
			{
				return hornLength < maxHornLength;
			}

			public override bool GroPlusHorns(ref int numHorns, ref int hornLength, int masculinity)
			{
				if (hornLength >= maxHornLength)
					return false;
				else
				{
					hornLength = maxHornLength;
					return true;
				}
			}

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculine)
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
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculine)
			{
				hornLength = minHornLength;
				return true;
			}
		}

		private class RhinoHorn : HornType
		{
			public RhinoHorn() : base(1, 2, 6, 12, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr) { }

			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, int masculinity)
			{
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				else
				{
					numHorns = maxHorns;
					significantHornLength = maxHornLength;
					return true;
				}

			}
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				if (numHorns == maxHorns)
				{
					numHorns = minHorns;
					hornLength = minHornLength;
					return false;
				}
				else
				{
					numHorns = 0;
					hornLength = 0;
				}
				return true;
			}
		}

		private class SheepHorns : HornType
		{
			private readonly int MAX_FEMININE_HORN_LENGTH = 7;

			public SheepHorns() : base(2, 2, 2, 30, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr)
			{
				if (minHornLength > MAX_FEMININE_HORN_LENGTH)
				{
					throw new System.ArgumentException("minimum horn length must be less than the max feminine length. should never procc.");
				}
			}

			public override bool CanShrink(int hornLength, int masculinity)
			{
				return hornLength > minHornLength;
			}
			public override bool ReductoHorns(ref int numHorns, ref int largestHornLength, int masculinity)
			{

				int reduceAmount = 0;
				if (largestHornLength > MAX_FEMININE_HORN_LENGTH)
					reduceAmount = 6;
				else if (largestHornLength > minHornLength)
				{
					reduceAmount = Math.Min(2, largestHornLength - minHornLength);
				}
				largestHornLength -= reduceAmount;
				return reduceAmount != 0;
			}


			public override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				bool feminine = IsFeminine(masculinity);
				if (byAmount == 0 || feminine && hornLength >= maxHornLength)
				{
					return false;
				}
				//any change amount will immediately make horns appear feminine on feminine pcs.
				//quick exit.
				else if (feminine && hornLength >= MAX_FEMININE_HORN_LENGTH)
				{
					hornLength = MAX_FEMININE_HORN_LENGTH;
					return true;
				}
				Lottery<int> lottery = new Lottery<int>();
				int maxLength;
				if (feminine)
				{
					lottery.addItem(3);
					maxLength = MAX_FEMININE_HORN_LENGTH;
				}
				else
				{
					lottery.addItems(2, 3, 4, 5, 6);
					maxLength = maxHornLength;
				}
				Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				int amount = (int)byAmount;
				while (amount-- > 0 && hornLength < maxLength)
				{
					hornLength += lottery.Select();
				}
				hornLength = Math.Min(maxLength, hornLength);
				return true;
			}
			//if masculine, Lose third of length, down to max feminine length. 
			//if feminine, go to max feminine length immediately
			//after that go to min, then lose horns entirely.
			public override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, int masculinity)
			{
				while (byAmount-- > 0 && hornLength > minHornLength)
				{
					if (hornLength >= 12 && !isFeminine(masculinity))
					{
						hornLength = (int)Math.Floor(hornLength * 2.0 / 3);
					}
					else if (hornLength > MAX_FEMININE_HORN_LENGTH)
					{
						hornLength = MAX_FEMININE_HORN_LENGTH;
					}
					else // if (hornLength > minHornLength)
					{
						hornLength = minHornLength;
					}
				}
				return byAmount > 0;
			}
		}
		/*
		



		



		

		IMP: "a pair of short, imp-like horns"
		BULL: internal override string GetDescriptor(int numHorns, int hornLength)
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
		RHINO internal override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		GOAT: internal override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength > minHornLength ? "a pair of " + hornLength + " inch goat horns. They are curved and patterned in ridges." : "a pair of short, nubby goat horns";
		}

		UNICORN: internal override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		DRAGON: internal override string GetDescriptor(int numHorns, int hornLength)
		{
			if (numHorns == maxHorns)
				return "four draconic horns. The first pair of horns are " + hornLength + " inches. The second pair sits behind them and reaches one foot in length";
			else if (hornLength >= 8)
				return "a pair of long, " + hornLength.ToString() + " inch draconic horns";
			else
				return "a pair of " + hornLength.ToString() + " inch horns - relatively short for a dragon";
		}
		ANTLERS: internal override string GetDescriptor(int numHorns, int hornLength)
		{
			return "a rack of " + (numHorns % 2 == 1 ? "asymmetric " : "") + "antlers, with a total of" + numHorns.ToString() + " points.";
		}
		SHEEP: internal override string GetDescriptor(int numHorns, int hornLength)
		{
			if (hornLength < 3)
				return "a pair of small sheep horns";
			else if (hornLength < MAX_FEMININE_HORN_LENGTH)
				return "a pair of curved, " + hornLength.ToString() + " inch bovine horns";
			else if (hornLength <= maxHornLength / 2)
				return "two spiraled horns, roughly " + hornLength.ToString() + " inches in length.";
			else
				return "two very thick, very large, spiraled ram's horms. if unwound, they'd be at least " + hornLength.ToString() + " inches" + ". Getting rammed by these would hurt.";
		}
		*/
	}
}