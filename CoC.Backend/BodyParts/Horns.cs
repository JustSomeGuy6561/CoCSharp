//Horns.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:50 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	//Strictly the facial structure. it doesn't include ears or eyes or hair.
	//They're done seperately. if a tf affects all of them, just call each one.

	//This class is so much harder to implement than i thought it'd be.
	//Edit so much later: This class is probably the most complicated i've implemented to date. I still need to add attack data.

	public sealed class Horns : BehavioralSaveablePart<Horns, HornType>, IGrowShrinkable, IGenderListener
	{
		private readonly Femininity hornMasculinity = Femininity.GenerateDefault();
		public override HornType type { get; protected set; }

		public byte significantHornSize => _significantHornSize;
		private byte _significantHornSize;
		public byte numHorns => _numHorns;
		private byte _numHorns;

		#region Constructors
		private Horns()
		{
			type = HornType.NONE;
			_numHorns = 0;
			_significantHornSize = 0;
		}

		private Horns(HornType hornType)
		{
			type = hornType;
			_numHorns = type.defaultHorns;
			_significantHornSize = type.defaultLength;
		}

		private Horns(HornType hornType, byte hornLength, byte hornCount)
		{
			type = hornType;
			_significantHornSize = hornLength;
			_numHorns = hornCount;
		}
		#endregion
		public override bool isDefault => type == HornType.NONE;

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			HornType hornType = type;
			bool valid = HornType.Validate(ref hornType, ref _numHorns, ref _significantHornSize, in hornMasculinity, correctDataIfInvalid);
			type = hornType;
			return valid;
		}

		#region Generate
		internal static Horns GenerateDefault()
		{
			return new Horns();
		}

		internal static Horns GenerateDefaultOfType(HornType hornType)
		{
			return new Horns(hornType);
		}

		internal static Horns GenerateWithStrength(HornType hornType, byte hornStrength, bool uniform = false)
		{
			Horns retVal = new Horns(hornType);
			retVal.StrengthenTransform(hornStrength);
			return retVal;
		}

		internal static Horns GenerateOverride(HornType hornType, byte hornLength, byte numHorns)
		{
			return new Horns(hornType, hornLength, numHorns);
		}
		#endregion
		#region Update
		internal bool UpdateHorns(HornType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			_numHorns = type.defaultHorns;
			_significantHornSize = type.defaultLength;
			return true;
		}

		internal bool UpdateAndStrengthenHorns(HornType newType, byte byAmount)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			_numHorns = type.defaultHorns;
			_significantHornSize = type.defaultLength;
			StrengthenTransform(byAmount);
			return true;
		}
		#endregion
		#region Restore
		internal override bool Restore()
		{
			if (type == HornType.NONE)
			{
				return false;
			}
			type = HornType.NONE;
			_numHorns = 0;
			_significantHornSize = 0;
			return true;
		}
		#endregion
		#region Horn Specific Methods
		public bool CanStrengthen()
		{
			return type.CanGrow(numHorns, significantHornSize, in hornMasculinity);
		}

		public bool StrengthenTransform(byte numberOfTimes = 1)
		{
			if (numberOfTimes == 0)
			{
				return false;
			}
			return type.StrengthenTransform(numberOfTimes, ref _numHorns, ref _significantHornSize, in hornMasculinity);
		}

		public bool CanWeaken()
		{
			return type.CanShrink(numHorns, significantHornSize, in hornMasculinity);
		}

		public bool WeakenTransform(byte byAmount = 1)
		{
			if (byAmount == 0)
			{
				return false;
			}
			bool removeHorns = type.WeakenTransform(byAmount, ref _numHorns, ref _significantHornSize, in hornMasculinity);
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
		
		#region IGenderListener
		void IGenderAware.GetFemininityData(GenderDataGetter getter)
		{
			dataGetter = getter;
		}
		private GenderDataGetter dataGetter;

		void IGenderListener.reactToChangeInFemininity()
		{
			hornMasculinity.Update(dataGetter().femininity);
			type.reactToChangesInMasculinity(ref _numHorns, ref _significantHornSize, in hornMasculinity);
		}
		#endregion

		#region IGrowShrinkable
		bool IGrowShrinkable.CanReducto()
		{
			return type.AllowsReducto && CanWeaken();
		}

		float IGrowShrinkable.UseReducto()
		{
			if (!((IGrowShrinkable)this).CanReducto())
			{
				return 0;
			}
			byte len = significantHornSize;
			type.ReductoHorns(ref _numHorns, ref _significantHornSize, in hornMasculinity);
			return len - significantHornSize;
		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return type.AllowsGroPlus && CanStrengthen();
		}

		float IGrowShrinkable.UseGroPlus()
		{
			if (!((IGrowShrinkable)this).
CanGrowPlus())
			{
				return 0;
			}
			byte len = significantHornSize;
			type.GroPlusHorns(ref _numHorns, ref _significantHornSize, in hornMasculinity);
			return significantHornSize - len;
		}
		#endregion
	}

	//i could go with function pobyteers throughout this, but frankly it's complicated enough that it might as well just be abstract.

	public abstract partial class HornType : SaveableBehavior<HornType, Horns>
	{
		#region HornType
		#region variables
		//private vars
		private const byte MAX_HORN_LENGTH = 40;
		//index mgic
		private static byte indexMaker = 0;
		private static readonly List<HornType> horns = new List<HornType>();
		//members
		private readonly byte _index;
		public readonly byte maxHorns;
		public readonly byte minHorns;
		public readonly byte defaultHorns;
		public readonly byte defaultLength;
		public readonly byte maxHornLength;
		public readonly byte minHornLength;

		//properties
		public bool allowsHorns => maxHorns > 0;

		public override int index => _index;
		#endregion
		//call the other constructor with defaults set to min.
		private protected HornType(byte minHorns, byte maximumHorns, byte minLength, byte maxLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc, ChangeType<Horns> transform, RestoreType<Horns> restore)
			: this(minHorns, maximumHorns, minLength, maxLength, minHorns, minLength, shortDesc, fullDesc, playerDesc, transform, restore) { }

		private protected HornType(byte minimumHorns, byte maximumHorns, byte minLength, byte maxLength, byte defaultHornCount, byte defaultHornLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc,
			ChangeType<Horns> transform, RestoreType<Horns> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			//Woo data cleanup.
			Utils.Clamp(ref maximumHorns, (byte)0, byte.MaxValue);
			Utils.Clamp(ref minimumHorns, (byte)0, maximumHorns);
			Utils.Clamp(ref maxLength, (byte)0, MAX_HORN_LENGTH);
			Utils.Clamp(ref minLength, (byte)0, maxLength);
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

		internal static HornType Deserialize(byte index)
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
					throw new System.ArgumentException("index for horn type pobytes to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref HornType hornType, ref byte hornCount, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
		{
			bool valid = true;
			if (!horns.Contains(hornType))
			{
				if (!correctInvalidData)
				{
					return false;
				}
				valid = false;
				hornType = NONE;
			}
			valid &= hornType.ValidateData(ref hornCount, ref hornLength, in masculinity, correctInvalidData);
			return valid;
		}

		protected virtual bool ValidateData(ref byte hornCount, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
		{
			bool valid = true;
			byte correctedValue = hornCount;
			Utils.Clamp(ref correctedValue, minHorns, maxHorns);
			if (correctedValue != hornCount)
			{
				if (!correctInvalidData)
				{
					return false;
				}
				hornCount = correctedValue;
				valid = false;
			}
			correctedValue = hornLength;
			Utils.Clamp(ref correctedValue, minHornLength, maxHornLength);
			if (correctedValue != hornLength)
			{
				if (!correctInvalidData)
				{
					return false;
				}
				hornLength = correctedValue;
				valid = false;
			}
			return valid;
		}

		internal virtual void reactToChangesInMasculinity(ref byte hornCount, ref byte hornLength, in Femininity masculinity) { }
		internal virtual bool CanGrow(byte numHorns, byte largestHornLength, in Femininity masculinity)
		{
			return numHorns < maxHorns || largestHornLength < maxHornLength;
		}
		internal virtual bool CanShrink(byte numHorns, byte largestHornLength, in Femininity masculinity)
		{
			return numHorns > minHorns || largestHornLength > minHornLength;
		}

		internal virtual bool AllowsReducto => false;
		internal virtual float ReductoHorns(ref byte numHorns, ref byte maxHornLength, in Femininity masculinity)
		{
			return 0;
		}
		internal virtual bool AllowsGroPlus => false;
		internal virtual float GroPlusHorns(ref byte numHorns, ref byte maxHornLength, in Femininity masculinity)
		{
			return 0;
		}

		internal abstract bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte significantHornLength, in Femininity masculinity, bool uniform = false); //unknown

		internal abstract bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte significantHornLength, in Femininity masculinity);

		internal virtual void GrowToMax(ref byte numHorns, ref byte largestHorn, in Femininity masculinity)
		{
			numHorns = maxHorns;
			largestHorn = maxHornLength;
		}

		internal virtual void ShrinkToMin(ref byte numHorns, ref byte largestHorn, in Femininity masculinity)
		{
			numHorns = minHorns;
			largestHorn = minHornLength;
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
			public SimpleOrNoHorns(byte hornCount, byte hornLength,
				SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc, ChangeType<Horns> transform,
				RestoreType<Horns> restore) : base(hornCount, hornCount, hornLength, hornLength, shortDesc, fullDesc, playerDesc, transform, restore) { }

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte significantHornLength, in Femininity masculinity, bool uniform = false)
			{
				return false;
			}

			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte significantHornLength, in Femininity masculinity)
			{
				numHorns = 0;
				significantHornLength = 0;
				return true;
			}
		}

		private class DemonHorns : HornType
		{
			public DemonHorns() : base(2, 12, 2, 10, DemonShortDesc, DemonFullDesc, DemonPlayerStr, DemonTransformStr, DemonRestoreStr) { }

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform = false)
			{
				Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
				if (numHorns >= maxHorns || byAmount == 0)
				{
					return false;
				}
				numHorns = (byte)Math.Min(maxHorns, numHorns + (2 * byAmount));
				hornLength = demonLengthFromHornCount(numHorns);
				return true;
			}
			//Lose 4-6 horns. if that makes it 0 horns, return true
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				if (byAmount == 0)
				{
					return false;
				}
				while (byAmount-- > 0 && numHorns > 0)
				{
					numHorns.add((byte)(numHorns > 6 ? 6 : numHorns > 4 ? 4 : numHorns));
				}
				hornLength = demonLengthFromHornCount(numHorns);
				return numHorns <= 0;
			}


			protected override bool ValidateData(ref byte hornCount, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
			{
				bool valid = base.ValidateData(ref hornCount, ref hornLength, in masculinity, correctInvalidData);
				if (!valid && !correctInvalidData)
				{
					return false;
				}
				byte correctLength = demonLengthFromHornCount(hornCount);
				if (correctLength == hornLength)
				{
					return valid;
				}
				else if (correctInvalidData)
				{
					hornLength = correctLength;
				}
				return false;
			}
			private byte demonLengthFromHornCount(byte hornCount)
			{
				if (hornCount >= 8) return 10;
				else if (hornCount >= 6) return 8;
				else if (hornCount >= 4) return 4;
				else return 2;
			}
		}

		private class BullHorns : HornType
		{
			private static readonly byte maxFeminineLength = 5;
			public BullHorns() : base(2, 2, 2, MAX_HORN_LENGTH, BullShortDesc, BullFullDesc, BullPlayerStr, BullTransformStr, BullRestoreStr)
			{
				if (minHornLength > maxFeminineLength)
				{
					throw new ArgumentException("minimum horn length must be less than the max feminine length. should never procc.");
				}
			}

			internal override bool AllowsReducto => true;

			internal override void reactToChangesInMasculinity(ref byte hornCount, ref byte hornLength, in Femininity femininity)
			{
				if (femininity.isHyperFeminine)
				{
					byte x = 2;
					feminizeHorns(ref x, ref hornLength);
				}
				else if (femininity.isFemale)
				{
					byte x = 1;
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

			internal override float ReductoHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				if (!CanShrink(numHorns, hornLength, masculinity))
				{
					return 0;
				}
				byte reduceAmount = 0;
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
				else if (masculinity.isFemale && hornLength > maxFeminineLength)
				{
					reduceAmount = hornLength.subtract(maxFeminineLength);
				}
				//female and female horns above min size? make them min size
				else if (masculinity.isFemale && hornLength > minHornLength)
				{
					reduceAmount= hornLength.subtract(minHornLength);
				}
				//not feminine and large? remove some
				else if (hornLength >= 10)
				{
					reduceAmount = 6;
				}
				//not feminine and smallish? make feminine max.
				else if (hornLength > maxFeminineLength)
				{
					reduceAmount= hornLength.subtract(maxFeminineLength);
				}
				hornLength -= reduceAmount;
				return reduceAmount;
			}

			internal override bool CanGrow(byte numHorns, byte largestHornLength, in Femininity masculinity)
			{
				if (masculinity.isFemale) return largestHornLength < maxFeminineLength;
				else return largestHornLength < maxHornLength;
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform = false)
			{
				if (byAmount == 0 || (!masculinity.isFemale && hornLength >= maxHornLength))
				{
					return false;
				}
				else if (masculinity.isFemale)
				{
					Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
					feminizeHorns(ref byAmount, ref hornLength); //grow or lengthen until it reaches feminine max.
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
							hornLength += (byte)(Utils.Rand(4) + 3);
						}
					}
					hornLength = hornLength > maxHornLength ? maxHornLength : hornLength;
					return true;
				}
			}
			//Lose half of the length, down to 5inches. at that pobyte, revert to nubs if female
			//or lose the rest if male. after that, lose them regardless.
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				//early exit: no amount
				if (byAmount == 0)
				{
					return false;
				}
				Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
				//early exit, non-zero count and at minimum.
				if (hornLength == minHornLength || (hornLength <= maxFeminineLength && masculinity.isMale))
				{
					numHorns = 0;
					hornLength = 0;
					return true;
				}
				//feminine and horns are at or above max length for feminine characters.
				else if (masculinity.isFemale && hornLength >= maxFeminineLength)
				{
					byte amount = (byte)byAmount;
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
				else if (masculinity.isFemale && hornLength < maxFeminineLength)
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
						else if (hornLength > maxFeminineLength)
						{
							hornLength = maxFeminineLength;
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
			private void feminizeHorns(ref byte amount, ref byte hornLength)
			{
				if (hornLength == maxFeminineLength)
				{
					return;
				}
				else if (hornLength < maxFeminineLength)
				{
					amount--;
					hornLength = maxFeminineLength;
				}
				else
				{
					while (amount-- > 0 && hornLength > maxFeminineLength)
					{
						if (hornLength > 10)
						{
							hornLength /= 2;
						}
						else if (hornLength > maxFeminineLength)
						{
							hornLength = maxFeminineLength;
						}
					}
				}
			}

			internal override void GrowToMax(ref byte numHorns, ref byte largestHorn, in Femininity masculinity)
			{
				numHorns = maxHorns;
				largestHorn = masculinity.isFemale ? maxFeminineLength : maxHornLength;
			}

			protected override bool ValidateData(ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
			{
				bool primary = base.ValidateData(ref numHorns, ref hornLength, in masculinity, correctInvalidData);
				if (!primary && !correctInvalidData)
				{
					return false;
				}

				if (!masculinity.isFemale || hornLength <= maxFeminineLength) //if our data is good here, return primary. bool && true = bool;
				{
					return primary;
				}
				else if (correctInvalidData)
				{
					hornLength = maxFeminineLength;
				}
				return false;
			}
		}

		private class DragonHorns : HornType
		{
			public DragonHorns() : base(2, 4, 4, 12, DragonShortDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr) { }

			//video game logic, idk: horns can be shrunk via reducto, but since reducto cant remove horns (except antlers), you just keep 4 horns. 
			//which means that it is technically valid to have four horns, with the first two tiny af. 
			//which means it doesnt need custom validation. KK;

			//Executive decision: second pair of dragon horns can't be shrunk. 
			internal override bool AllowsReducto => true;
			internal override float ReductoHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				byte reduced = hornLength.subtract(2);
				byte oldHornLength = hornLength;
				hornLength = Math.Max(minHornLength, reduced);
				return oldHornLength - hornLength;
			}

			internal override bool AllowsGroPlus => true;

			internal override float GroPlusHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				byte grown = hornLength.add(2);
				byte oldHornLength = hornLength;
				hornLength = Math.Min(maxHornLength, grown);
				return oldHornLength - hornLength;
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform = false)
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
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
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
			public Antlers(bool reindeer, byte maxLength,
				SimpleDescriptor shortDesc, DescriptorWithArg<Horns> fullDesc, TypeAndPlayerDelegate<Horns> playerDesc,
				ChangeType<Horns> transform, RestoreType<Horns> restore) : base(2, 20, 6, maxLength, shortDesc, fullDesc, playerDesc, transform, restore)
			{
				isReindeer = reindeer;
			}

			internal override bool CanGrow(byte numHorns, byte largestHornLength, in Femininity masculinity)
			{
				return numHorns < maxHorns;
			}

			internal override bool CanShrink(byte numHorns, byte largestHornLength, in Femininity masculinity)
			{
				return numHorns > minHorns;
			}

			internal override bool AllowsReducto => true;

			internal override float ReductoHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				byte reduced = numHorns.subtract(4);
				byte oldHornCount = numHorns;
				numHorns = Math.Max(minHornLength, reduced);
				hornLength = setLengthFromHorns(numHorns);
				return oldHornCount - numHorns;
			}

			internal override bool AllowsGroPlus => isReindeer;

			internal override float GroPlusHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinitiy)
			{
				if (!isReindeer || numHorns >= maxHorns)
				{
					return 0;
				}
				byte growHorns = new Lottery<byte>(3, 3, 4, 4, 5, 6).Select();
				byte oldHornCount = numHorns;
				numHorns += growHorns;
				Utils.Clamp(ref numHorns, (byte)0, maxHorns);
				hornLength = setLengthFromHorns(numHorns);
				return numHorns - oldHornCount;
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform = false)
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
						byte growHorns = (new Lottery<byte>(3, 3, 4, 4, 5, 6)).Select();
						numHorns += growHorns;
					}
					Utils.Clamp(ref numHorns, (byte)0, maxHorns);
				}
				hornLength = setLengthFromHorns(numHorns);
				return true;

			}
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				//get value, then decrement. if you're not familiar with this, it's confusing, i know. but i always
				//forget to decrement the loop at the end, and infinite loops are worse.
				while (byAmount-- > 0 && numHorns > minHorns)
				{
					byte removeHorns = new Lottery<byte>(3, 3, 4, 4, 5, 6).Select();
					numHorns -= removeHorns;
					Utils.Clamp(ref numHorns, minHorns, maxHorns);
				}
				hornLength = setLengthFromHorns(numHorns);
				return byAmount > 0;
			}

			protected override bool ValidateData(ref byte hornCount, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
			{
				bool valid = base.ValidateData(ref hornCount, ref hornLength, in masculinity, correctInvalidData);
				if (!valid && !correctInvalidData)
				{
					return false;
				}
				byte correctLength = setLengthFromHorns(hornCount);
				if (correctLength == hornLength)
				{
					return valid;
				}
				else if (correctInvalidData)
				{
					hornLength = correctLength;
				}
				return false;
			}

			private byte setLengthFromHorns(byte hornCount)
			{
				if (!isReindeer || hornCount < 8)
				{
					return hornCount.add(4);
				}
				else if (hornCount >= 16)
				{
					return hornCount.add(16);
				}
				else if (hornCount >= 12)
				{
					return hornCount.add(12);
				}
				else //if (hornCount >= 8)
				{
					return hornCount.add(8);
				}
			}
		}

		private class GoatHorns : HornType
		{
			public GoatHorns() : base(2, 2, 1, 6, GoatShortDesc, GoatFullDesc, GoatPlayerStr, GoatTransformStr, GoatRestoreStr) { }

			internal override bool AllowsReducto => true;

			internal override float ReductoHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				byte changeAmount = 0;
				if (hornLength <= minHornLength)
					return 0;
				else if (hornLength + 1 == maxHornLength)
				{
					changeAmount = 1;
				}
				else
				{
					changeAmount = new Lottery<byte>(1, 1, 2, 2, 2, 2).Select();
				}
				hornLength -= changeAmount;
				return changeAmount;
			}

			internal override bool AllowsGroPlus => true;

			internal override float GroPlusHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				byte changeAmount = 0;
				if (hornLength >= maxHornLength)
				{
					return 0;
				}
				else if (hornLength + 1 == maxHornLength)
				{
					changeAmount = 1;
				}
				else
				{
					changeAmount = new Lottery<byte>(1, 1, 2, 2, 2, 2).Select();
				}
				hornLength += changeAmount;
				return changeAmount;
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform = false)
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
						hornLength += (new Lottery<byte>(1, 1, 2, 2, 2, 2)).Select();
					}
				}
				return true;

			}
			//nope.avi. they're so small there's just no pobyte. you just lose them.
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				hornLength = 0;
				return true;
			}
		}

		//Get it? That made me laugh way harder than it should have (which is not at all).
		private class UniHorn : HornType
		{
			public UniHorn() : base(1, 1, 6, 12, UniHornShortDesc, UniHornFullDesc, UniHornPlayerStr, UniHornTransformStr, UniHornRestoreStr) { }

			internal override bool AllowsReducto => true;

			internal override float ReductoHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				if (hornLength <= minHornLength)
					return 0;
				else
				{
					float retVal = hornLength - minHornLength;
					hornLength = minHornLength;
					return retVal;
				}
			}
			internal override bool AllowsGroPlus => true;

			internal override float GroPlusHorns(ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				if (hornLength >= maxHornLength)
					return 0;
				else
				{
					float retVal = maxHornLength - hornLength;
					hornLength = maxHornLength;
					return retVal;
				}
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculine, bool uniform)
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
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculine)
			{
				hornLength = minHornLength;
				return true;
			}
		}

		private class RhinoHorn : HornType
		{
			public RhinoHorn() : base(1, 2, 6, 12, RhinoShortDesc, RhinoFullDesc, RhinoPlayerStr, RhinoTransformStr, RhinoRestoreStr) { }

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte significantHornLength, in Femininity masculinity, bool uniform)
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
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
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

			protected override bool ValidateData(ref byte hornCount, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
			{
				bool valid = true;
				if (hornCount < minHorns)
				{
					if (correctInvalidData)
					{
						hornCount = minHorns;
						hornLength = minHornLength;
					}
					valid = false;
				}
				else if (hornCount == minHorns && hornLength != minHornLength)
				{
					if (correctInvalidData)
					{
						hornLength = minHornLength;
					}
					valid = false;
				}
				else if (hornCount > maxHorns)
				{
					if (correctInvalidData)
					{
						hornCount = maxHorns;
						hornLength = maxHornLength;
					}
					valid = false;
				}
				else if (hornCount == maxHorns && hornLength != maxHornLength)
				{
					if (correctInvalidData)
					{
						hornLength = maxHornLength;
					}
					valid = false;
				}
				return valid;
			}
		}

		private class SheepHorns : HornType
		{
			private static readonly byte maxFeminineLength = 7;

			public SheepHorns() : base(2, 2, 2, 30, SheepShortDesc, SheepFullDesc, SheepPlayerStr, SheepTransformStr, SheepRestoreStr)
			{
				if (minHornLength > maxFeminineLength)
				{
					throw new System.ArgumentException("minimum horn length must be less than the max feminine length. should never procc.");
				}
			}

			internal override bool AllowsReducto => true;
			internal override float ReductoHorns(ref byte numHorns, ref byte largestHornLength, in Femininity masculinity)
			{

				byte reduceAmount = 0;
				if (largestHornLength > maxFeminineLength)
					reduceAmount = 6;
				else if (largestHornLength > minHornLength)
				{
					reduceAmount = Math.Min((byte)2, largestHornLength.subtract(minHornLength));
				}
				largestHornLength -= reduceAmount;
				return reduceAmount;
			}

			internal override bool CanGrow(byte numHorns, byte largestHornLength, in Femininity masculinity)
			{
				if (masculinity.isFemale)
				{
					return largestHornLength < maxFeminineLength;
				}
				else
				{
					return largestHornLength < maxHornLength;
				}
			}

			internal override bool StrengthenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool uniform)
			{
				bool feminine = masculinity.isFemale;
				if (byAmount == 0 || feminine && hornLength >= maxHornLength)
				{
					return false;
				}
				//any change amount will immediately make horns appear feminine on feminine pcs.
				//quick exit.
				else if (feminine && hornLength >= maxFeminineLength)
				{
					hornLength = maxFeminineLength;
					return true;
				}
				else
				{
					Lottery<byte> lottery = new Lottery<byte>();
					byte maxLength = maxHornLength;
					if (feminine)
					{
						maxLength = maxFeminineLength;
					}

					if (feminine || uniform)
					{
						lottery.addItem(3);
					}
					else
					{
						lottery.addItems(2, 3, 4, 5, 6);
					}
					Utils.Clamp<byte>(ref byAmount, 0, byte.MaxValue);
					byte amount = (byte)byAmount;
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
			internal override bool WeakenTransform(byte byAmount, ref byte numHorns, ref byte hornLength, in Femininity masculinity)
			{
				while (byAmount-- > 0 && hornLength > minHornLength)
				{
					if (hornLength >= 12 && !masculinity.isFemale)
					{
						hornLength = (byte)Math.Floor(hornLength * 2.0 / 3);
					}
					else if (hornLength > maxFeminineLength)
					{
						hornLength = maxFeminineLength;
					}
					else // if (hornLength > minHornLength)
					{
						hornLength = minHornLength;
					}
				}
				return byAmount > 0;
			}

			internal override void GrowToMax(ref byte numHorns, ref byte largestHorn, in Femininity masculinity)
			{
				numHorns = maxHorns;
				largestHorn = masculinity.isFemale ? maxFeminineLength : maxHornLength;
			}

			protected override bool ValidateData(ref byte numHorns, ref byte hornLength, in Femininity masculinity, bool correctInvalidData = false)
			{
				bool primary = base.ValidateData(ref numHorns, ref hornLength, in masculinity, correctInvalidData);
				if (!primary && !correctInvalidData)
				{
					return false;
				}

				if (!masculinity.isFemale || hornLength <= maxFeminineLength) //if our data is good here, return primary. bool && true = bool;
				{
					return primary;
				}
				else if (correctInvalidData)
				{
					hornLength = maxFeminineLength;
				}
				return false;
			}
		}
		/*
		



		



		

		IMP: "a pair of short, imp-like horns"
		BULL: public override string GetDescriptor(byte numHorns, byte hornLength)
		{
				if (hornLength < 3)
					return "a pair of small nubs, like those on a young bovine";
				else if (hornLength < 6)
					return "a pair of moderately-sized, " + hornLength.ToString() + " inch bovine horns";
				else if (hornLength < 12)
					return "two large horns, roughly " + hornLength.ToString() + " inches in length, curve forwards like those of a bull.";
				else if (hornLength < 20)
					return "two very large and dangerous looking horns, curving forward, reaching at least " + (hornLength == 12 ? "a foot" : hornLength.ToString() + "inches") + ". They have dangerous looking pobytes.";
				else //if (player.horns.value >= 20)
					return "two huge horns, curving outward at first, then forwards. They reach at least " + hornLength.ToString() + "inches and end in sharp pobytes. They look incredibly dangerous";
		}
		RHINO public override string GetDescriptor(byte numHorns, byte hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		GOAT: public override string GetDescriptor(byte numHorns, byte hornLength)
		{
			return hornLength > minHornLength ? "a pair of " + hornLength + " inch goat horns. They are curved and patterned in ridges." : "a pair of short, nubby goat horns";
		}

		UNICORN: public override string GetDescriptor(byte numHorns, byte hornLength)
		{
			return hornLength < maxHornLength ? "a single sharp horn" : "a single foot-long unicorn horn, complete with a spiral";
		}
		DRAGON: public override string GetDescriptor(byte numHorns, byte hornLength)
		{
			if (numHorns == maxHorns)
				return "four draconic horns. The first pair of horns are " + hornLength + " inches. The second pair sits behind them and reaches one foot in length";
			else if (hornLength >= 8)
				return "a pair of long, " + hornLength.ToString() + " inch draconic horns";
			else
				return "a pair of " + hornLength.ToString() + " inch horns - relatively short for a dragon";
		}
		ANTLERS: public override string GetDescriptor(byte numHorns, byte hornLength)
		{
			return "a rack of " + (numHorns % 2 == 1 ? "asymmetric " : "") + "antlers, with a total of" + numHorns.ToString() + " pobytes.";
		}
		SHEEP: public override string GetDescriptor(byte numHorns, byte hornLength)
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