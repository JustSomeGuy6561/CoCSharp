//Horns.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:50 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.

	//This class is so much harder to implement than i thought it'd be.
	[DataContract]
	public class Horns : BodyPartBase<Horns, HornType>, IGrowShrinkable
	{
		private readonly FemininityData hornMasculinity = new FemininityData();
		public override HornType type { get; protected set; }

		public int significantHornSize { get; protected set; }
		public int numHorns { get; protected set; }

		private protected Horns()
		{
			type = HornType.NONE;
			numHorns = 0;
			significantHornSize = 0;
		}

		private protected Horns(HornType hornType)
		{
			type = hornType;
			numHorns = type.defaultHorns;
			significantHornSize = type.defaultLength;
		}

		private protected Horns(HornType hornType, int hornLength, int hornCount)
		{
			type = hornType;
			significantHornSize = hornLength;
			numHorns = hornCount;
		}

		public override bool isDefault => type == HornType.NONE;

		internal static Horns GenerateDefault()
		{
			return new Horns();
		}

		internal static Horns GenerateDefaultOfType(HornType hornType)
		{
			return new Horns(hornType);
		}

		internal static Horns GenerateWithStrength(HornType hornType, int hornStrength, bool uniform = false)
		{
			Horns retVal = new Horns(hornType);
			uint strength = hornStrength < -1 ? 0 : hornStrength == -1 ? int.MaxValue : (uint)hornStrength;
			retVal.StrengthenTransform(strength);
			return retVal;
		}

		internal static Horns GenerateOverride(HornType hornType, int hornLength, int numHorns)
		{
			return new Horns(hornType, hornLength, numHorns);
		}

		internal override bool Restore()
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

		public bool StrengthenTransform(uint numberOfTimes = 1)
		{
			if (numberOfTimes == 0)
			{
				return false;
			}
			bool transform(uint x, ref int y, ref int z, in FemininityData a) { return type.StrengthenTransform(x, ref y, ref z, in a); }
			return handleHornTransforms(numberOfTimes, transform);
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

		internal void ReactToChangeInFemininity(FemininityData femininity)
		{
			hornMasculinity.Update(femininity);
			handleHornSet(type.reactToChangesInMasculinity);
		}

		#region IGrowShrinkable
		public bool CanReducto()
		{
			return type.CanShrink(significantHornSize, in hornMasculinity);
		}



		float IGrowShrinkable.UseReducto()
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
			return type.CanGrow(significantHornSize, in hornMasculinity);
		}

		float IGrowShrinkable.UseGroPlus()
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
		private T handleHornTransforms<T>(uint byAmount, TransformDelegate<T> handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			T retVal = handle(byAmount, ref hornCount, ref hornSize, in hornMasculinity);
			numHorns = hornCount;
			significantHornSize = hornSize;
			return retVal;
		}

		//generic function pointer taking uint, int, int, bool. return type can be specified manually.
		private delegate T TransformDelegate<T>(uint amt, ref int ct, ref int sz, in FemininityData masc);

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

		private void handleHornSet(SetHornDelegate handle)
		{
			int hornCount, hornSize;
			hornCount = numHorns;
			hornSize = significantHornSize;
			handle(ref hornCount, ref hornSize, hornMasculinity);
			numHorns = hornCount;
			significantHornSize = hornSize;
		}

		private delegate T SetHornDelegate<T>(ref int ct, ref int sz, in FemininityData masc);
		private delegate void SetHornDelegate(ref int ct, ref int sz, in FemininityData masc);

		internal override Type currentSaveVersion => typeof(HornSurrogateVersion1);
		internal override Type[] saveVersions => new Type[] { typeof(HornSurrogateVersion1) };
		internal override BodyPartSurrogate<Horns, HornType> ToCurrentSave()
		{
			return new HornSurrogateVersion1()
			{
				hornType = index,
				hornLength = significantHornSize,
				numHorns = this.numHorns,
			};
		}

		internal Horns(HornSurrogateVersion1 surrogate)
		{
			type = HornType.Deserialize(surrogate.hornType);
			numHorns = surrogate.numHorns;
			significantHornSize = surrogate.hornLength;
		}
	}

	//i could go with function pointers throughout this, but frankly it's complicated enough that it might as well just be abstract.

	public abstract partial class HornType : BodyPartBehavior<HornType, Horns>
	{
		#region HornType
		#region variables
		//private vars
		private const int MAX_HORN_LENGTH = 40;
		//index mgic
		private static int indexMaker = 0;
		private static List<HornType> horns = new List<HornType>();
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
		private protected HornType(int minHorns, int maximumHorns, int minLength, int maxLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc, ChangeType<Horns> transform, RestoreType<Horns> restore)
			: this(minHorns, maximumHorns, minLength, maxLength, minHorns, minLength, shortDesc, fullDesc, playerDesc, transform, restore) { }

		private protected HornType(int minimumHorns, int maximumHorns, int minLength, int maxLength, int defaultHornCount, int defaultHornLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc,
			ChangeType<Horns> transform, RestoreType<Horns> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
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
			horns.AddAt(this, _index);
		}

		internal static HornType Deserialize(int index)
		{
			if (index < 0 || index >= horns.Count)
			{
				throw new System.ArgumentException("index for horn type deserialize out of range");
			}
			else
			{
				HornType horn = horns[index];
				if (horn != null)
				{
					return horn;
				}
				else
				{
					throw new System.ArgumentException("index for horn type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal virtual void reactToChangesInMasculinity(ref int hornCount, ref int hornLength, in FemininityData masculinity) { } // do nothing. 
		internal virtual bool CanShrink(int largestHornLength, in FemininityData masculinity)
		{
			return false;
		}
		internal virtual bool ReductoHorns(ref int numHorns, ref int maxHornLength, in FemininityData masculinity)
		{
			return false;
		}

		internal virtual bool CanGrow(int largestHornLength, in FemininityData masculinity)
		{
			return false;
		}

		internal virtual bool GroPlusHorns(ref int numHorns, ref int maxHornLength, in FemininityData masculinity)
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
		internal virtual bool StrengthenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, in FemininityData masculinity, bool uniform = false)
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
		internal virtual bool WeakenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, in FemininityData masculinity)
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
		#endregion
		//these horns are immutable - if you have them, they do not grow or shrink, and you can't get any more of them.
		private class SimpleOrNoHorns : HornType
		{
			public SimpleOrNoHorns(int hornCount, int hornLength,
				SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc, ChangeType<Horns> transform,
				RestoreType<Horns> restore) : base(hornCount, hornCount, hornLength, hornLength, shortDesc, fullDesc, playerDesc, transform, restore) { }
		}

		private class DemonHorns : HornType
		{
			public DemonHorns() : base(2, 12, 2, 10, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform = false)
			{
				Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				numHorns = Math.Min(maxHorns, numHorns + (2 * (int)byAmount));
				hornLength = demonLengthFromHornCount(numHorns);
				return true;
			}
			//Lose 4-6 horns. if that makes it 0 horns, return true
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
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
					throw new ArgumentException("minimum horn length must be less than the max feminine length. should never procc.");
				}
			}

			internal override void reactToChangesInMasculinity(ref int hornCount, ref int hornLength, in FemininityData femininity)
			{
				if (femininity.isHyperFeminine)
				{
					int x = 2;
					feminizeHorns(ref x, ref hornLength);
				}
				else if (femininity.isFemale)
				{
					int x = 1;
					feminizeHorns(ref x, ref hornLength);
				}
				else if (femininity.isHyperMasculine) //hyper masculine.
				{
					if (hornLength < 24)
					{
						hornLength += 2;
					}
				}
				else if (femininity.isMale)
				{
					if (hornLength < 12)
					{
						hornLength++;
					}
				}

			}

			internal override bool CanShrink(int largestHornLength, in FemininityData masculinity)
			{
				return largestHornLength > minHornLength;
			}

			internal override bool ReductoHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				if (!CanShrink(hornLength, masculinity))
				{
					return false;
				}
				int reduceAmount = 0;
				//large horns and female? remove a lot.
				if (masculinity.isFemale && hornLength > 18)
				{
					reduceAmount = 12;
				}
				//less large, but still large, and female? remove some.
				else if (masculinity.isFemale && hornLength > 10)
				{
					reduceAmount = 6;
				}
				//smaller, but still above female max, and female? make female max.
				else if (masculinity.isFemale && hornLength > MAX_FEMININE_HORN_LENGTH)
				{
					reduceAmount = hornLength - MAX_FEMININE_HORN_LENGTH;
				}
				//female and female horns above min size? make them min size
				else if (masculinity.isFemale && hornLength > minHornLength)
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

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform = false)
			{
				if (byAmount == 0 || (!masculinity.isFemale && hornLength >= maxHornLength))
				{
					return false;
				}
				else if (masculinity.isFemale)
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
						//uniform is 4.
						if (uniform)
						{
							hornLength += 4;
						}
						//grow horns 3-6 inches.
						else
						{
							hornLength += Tools.Utils.Rand(4) + 3;
						}
					}
					hornLength = hornLength > maxHornLength ? maxHornLength : hornLength;
					return true;
				}
			}
			//Lose half of the length, down to 5inches. at that point, revert to nubs if female
			//or lose the rest if male. after that, lose them regardless.
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				//early exit: no amount
				if (byAmount == 0)
				{
					return false;
				}
				Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
				//early exit, non-zero count and at minimum.
				if (hornLength == minHornLength || (hornLength <= MAX_FEMININE_HORN_LENGTH && masculinity.isMale))
				{
					numHorns = 0;
					hornLength = 0;
					return true;
				}
				//feminine and horns are at or above max length for feminine characters.
				else if (masculinity.isFemale && hornLength >= MAX_FEMININE_HORN_LENGTH)
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
				else if (masculinity.isFemale && hornLength < MAX_FEMININE_HORN_LENGTH)
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
			internal override bool CanShrink(int hornLength, in FemininityData masculinity)
			{
				return hornLength > minHornLength;
			}
			internal override bool ReductoHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				int reductoVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Max(minHornLength, hornLength - reductoVal);
				return oldHornLength != hornLength;
			}

			internal override bool CanGrow(int hornLength, in FemininityData masculinity)
			{
				return hornLength < maxHornLength;
			}

			internal override bool GroPlusHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				int groVal = 2;
				int oldHornLength = hornLength;
				hornLength = Math.Min(maxHornLength, hornLength + groVal);
				return oldHornLength != hornLength;
			}

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform = false)
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
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
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
				SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc,
				ChangeType<Horns> transform, RestoreType<Horns> restore) : base(2, 20, 6, maxLength, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				isReindeer = reindeer;
			}

			internal override bool CanShrink(int hornLength, in FemininityData masculinity)
			{
				return hornLength > minHornLength;
			}
			internal override bool ReductoHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				int reductoVal = 4;
				int oldHornCount = numHorns;
				numHorns = Math.Max(minHornLength, hornLength - reductoVal);
				setLengthFromHorns(numHorns, ref hornLength);
				return oldHornCount != numHorns;
			}

			internal override bool CanGrow(int hornLength, in FemininityData masculinity)
			{
				return isReindeer && hornLength < maxHornLength;
			}

			internal override bool GroPlusHorns(ref int numHorns, ref int hornLength, in FemininityData masculinitiy)
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

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform = false)
			{
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				//get value, then decrement. if you're not familiar with this, it's confusing, i know. but i always
				//forget to decrement the loop at the end, and infinite loops are worse.
				while (byAmount-- > 0 && numHorns < maxHorns)
				{
					if (uniform)
					{
						numHorns += 4;
					}
					else
					{
						int growHorns = (new Lottery<int>(3, 3, 4, 4, 5, 6)).Select();
						numHorns += growHorns;
					}
					Utils.Clamp(ref numHorns, 0, maxHorns);
				}
				setLengthFromHorns(numHorns, ref hornLength);
				return true;

			}
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
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

			internal override bool CanShrink(int hornLength, in FemininityData masculinity)
			{
				return hornLength > minHornLength;
			}
			internal override bool ReductoHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				if (hornLength <= minHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();

				return true;
			}

			internal override bool CanGrow(int hornLength, in FemininityData masculinity)
			{
				return hornLength < maxHornLength;
			}

			internal override bool GroPlusHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				if (hornLength >= maxHornLength)
					return false;
				else if (hornLength + 1 == maxHornLength)
					hornLength++;
				else
					hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();
				return true;
			}

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform = false)
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
					{
						hornLength++;
					}
					else if (uniform)
					{
						hornLength += 2;
					}
					else
					{ 
						hornLength += (new Lottery<int>(1, 1, 2, 2, 2, 2)).Select();
					}
				}
				return true;

			}
			//nope.avi. they're so small there's just no point. you just lose them.
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				hornLength = 0;
				return true;
			}
		}

		//Get it? That made me laugh way harder than it should have (which is not at all).
		private class UniHorn : HornType
		{
			public UniHorn() : base(1, 1, 6, 12, UniHornShortDesc, UniHornFullDesc, UniHornPlayerStr, UniHornTransformStr, UniHornRestoreStr) { }

			internal override bool CanShrink(int hornLength, in FemininityData masculinity)
			{
				return hornLength > minHornLength;
			}
			internal override bool ReductoHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				if (hornLength <= minHornLength)
					return false;
				else
				{
					hornLength = minHornLength;
					return true;
				}
			}

			internal override bool CanGrow(int hornLength, in FemininityData masculinity)
			{
				return hornLength < maxHornLength;
			}

			internal override bool GroPlusHorns(ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				if (hornLength >= maxHornLength)
					return false;
				else
				{
					hornLength = maxHornLength;
					return true;
				}
			}

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculine, bool uniform)
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
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculine)
			{
				hornLength = minHornLength;
				return true;
			}
		}

		private class RhinoHorn : HornType
		{
			public RhinoHorn() : base(1, 2, 6, 12, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr) { }

			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int significantHornLength, in FemininityData masculinity, bool uniform)
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
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
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

			internal override bool CanShrink(int hornLength, in FemininityData masculinity)
			{
				return hornLength > minHornLength;
			}
			internal override bool ReductoHorns(ref int numHorns, ref int largestHornLength, in FemininityData masculinity)
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


			internal override bool StrengthenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity, bool uniform)
			{
				bool feminine = masculinity.isFemale;
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
				else
				{
					Lottery<int> lottery = new Lottery<int>();
					int maxLength = maxHornLength;
					if (feminine)
					{
						maxLength = MAX_FEMININE_HORN_LENGTH;
					}
					
					if (feminine || uniform)
					{
						lottery.addItem(3);
					}
					else
					{
						lottery.addItems(2, 3, 4, 5, 6);
					}
					Utils.Clamp<uint>(ref byAmount, 0, int.MaxValue);
					int amount = (int)byAmount;
					while (amount-- > 0 && hornLength < maxLength)
					{
						hornLength += lottery.Select();
					}
					hornLength = Math.Min(maxLength, hornLength);
				}
				return true;
			}
			//if masculine, Lose third of length, down to max feminine length. 
			//if feminine, go to max feminine length immediately
			//after that go to min, then lose horns entirely.
			internal override bool WeakenTransform(uint byAmount, ref int numHorns, ref int hornLength, in FemininityData masculinity)
			{
				while (byAmount-- > 0 && hornLength > minHornLength)
				{
					if (hornLength >= 12 && !masculinity.isFemale)
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
		BULL: public override string GetDescriptor(int numHorns, int hornLength)
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
		RHINO public override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		GOAT: public override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength > minHornLength ? "a pair of " + hornLength + " inch goat horns. They are curved and patterned in ridges." : "a pair of short, nubby goat horns";
		}

		UNICORN: public override string GetDescriptor(int numHorns, int hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		DRAGON: public override string GetDescriptor(int numHorns, int hornLength)
		{
			if (numHorns == maxHorns)
				return "four draconic horns. The first pair of horns are " + hornLength + " inches. The second pair sits behind them and reaches one foot in length";
			else if (hornLength >= 8)
				return "a pair of long, " + hornLength.ToString() + " inch draconic horns";
			else
				return "a pair of " + hornLength.ToString() + " inch horns - relatively short for a dragon";
		}
		ANTLERS: public override string GetDescriptor(int numHorns, int hornLength)
		{
			return "a rack of " + (numHorns % 2 == 1 ? "asymmetric " : "") + "antlers, with a total of" + numHorns.ToString() + " points.";
		}
		SHEEP: public override string GetDescriptor(int numHorns, int hornLength)
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

	[DataContract]
	public sealed class HornSurrogateVersion1 : BodyPartSurrogate<Horns, HornType>
	{
		[DataMember]
		public int hornType;
		[DataMember]
		public int numHorns;
		[DataMember]
		public int hornLength;
		internal override Horns ToBodyPart()
		{
			return new Horns(this);
		}
	}
}