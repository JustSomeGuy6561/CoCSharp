//Balls.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:57 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//It's possible to actually get 4 balls in source, despite my initial assumption otherwise. I've let you max up to 8 b/c i missed this. could revert to just 4, w/e.

	//though this probably needs more thought - is it two per sack, and multiple sacks? or is it all one sack?

	//note for writers: balls are technically plural, but it really doesn't make sense to say 'a ball.' the expected text would be 'one of [pronoun] balls'
	//the exception is uniball - if you have a uniball, it's kinda just one ball, i guess. all descriptions have an overload that allows you to query whether or not it's
	//plural, if you don't want to check the count beforehand. IMO, you should just check for a uniball and handle the text that way, but whatever, it's not really a ton of trouble
	//to support the out parameter.

	//I'VE GOT BIG BALLS! OH, I'VE GOT BIG BALLS! THERE SUCH BIG BALLS! DIRTY BIG BALLS! HE'S GOT BIG BALLS! AND SHE'S GOT BIG BALLS! BUT WE'VE GOT THE BIGGEST BALLS OF THEM ALL!
	//i'll see if i can hide this as an easter egg is some text somewhere.
	public sealed partial class Balls : SimpleSaveablePart<Balls, BallsData>, IGrowable, IShrinkable
	{
		//BasePerkModifiers modifiers => perkData();

		public const byte MAX_BALLS_SIZE = 30;
		public const byte MIN_BALLS_SIZE = 1;
		public const byte DEFAULT_BALLS_SIZE = 2;

		public const byte UNIBALL_SIZE_THRESHOLD = 1;

		public const byte MAX_BALLS_COUNT = 8;
		public const byte UNIBALL_COUNT = 1;
		public const byte DEFAULT_MIN_COUNT = 2;
		public const byte DEFAULT_BALLS_COUNT = 2;
		public int index => size;

		//huh. balls is never null, so we don't have to worry about it being correct.

		internal byte defaultNewSize = 1;
		internal sbyte newSizeOffset = 0;

		private byte getNewSize() => Utils.Clamp2(defaultNewSize, MIN_BALLS_SIZE, MAX_BALLS_SIZE);
		private byte getNewSize(byte baseSize) => Utils.Clamp2(baseSize.offset(newSizeOffset), MIN_BALLS_SIZE, MAX_BALLS_SIZE);

		internal float shrinkMultiplier = 1.0f;
		internal float growthMultiplier = 1.0f;

		//recommend saving the hasBalls bool even though it is a determined property - in the event of malformed data, it allows an additional way to catch
		//if the save should have balls.
		public bool hasBalls => count != 0;
		public bool uniBall => count == UNIBALL_COUNT;

		//Count and size get conviluted with self-validation, so i've removed what validation i can from these properties and placed it in helper functions.
		//basically, if the old count was 0 or the new count is 0, use the helpers.
		//if you dont use the helpers, make sure you set the count first - the size uses the count to parse the values passed into the setter.
		public byte count { get; private set; } = 0;

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

		internal int hoursSinceCum => CreatureStore.GetCreatureClean(creatureID)?.genitals.hoursSinceLastCum ?? 0;
		internal float relativeLust => CreatureStore.GetCreatureClean(creatureID)?.relativeLust ?? Creature.DEFAULT_LUST;
		internal BodyType bodyType => CreatureStore.GetCreatureClean(creatureID)?.body.type ?? BodyType.defaultValue;


		internal Balls(Guid creatureID, bool hasBalls) : base(creatureID)
		{
			setBalls(hasBalls, silent:true);
		}

		internal Balls(Guid creatureID, byte ballCount = DEFAULT_BALLS_COUNT, byte ballSize = DEFAULT_BALLS_SIZE) : base(creatureID)
		{
			if (ballCount == 0)
			{
				setBalls(false);
			}
			else
			{
				setBalls(true, ballCount, ballSize, true);
			}
		}

		//use this to initialize the balls object when the creature has balls.
		internal Balls(Guid creatureID, Gender gender) : this(creatureID, gender.HasFlag(Gender.MALE))
		{
		}

		internal static Balls GenerateUniBall(Guid creatureID)
		{
			Balls balls = new Balls(creatureID, false);
			balls.setUniBall(true, true);
			return balls;
		}

		public override string BodyPartName() => Name();

		public override BallsData AsReadOnlyData()
		{
			return new BallsData(this);
		}

		#region Unique Functions and Updating Properties
		//Grows a pair of balls. returns false if it already has balls.
		public bool GrowBalls()
		{
			if (hasBalls)
			{
				return false;
			}
			else
			{
				byte numBalls = 2;
				setBalls(true, numBalls, defaultNewSize);
			}
			return true;
		}

		public bool GrowBalls(byte numBalls, byte newSize = DEFAULT_BALLS_SIZE)
		{

			if (hasBalls)
			{
				return false;
			}
			else
			{
				setBalls(true, numBalls, getNewSize(newSize));
				return true;
			}
		}

		public bool GrowUniBall()
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

		public bool MakeUniBall()
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

		public bool MakeStandard()
		{
			if (hasBalls && !uniBall)
			{
				return false;
			}
			else
			{
				setBalls(true);
				return true;
			}
		}

		public byte AddBalls(byte addAmount)
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
		public byte RemoveBalls(byte removeAmount)
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

		public byte RemoveExtraBalls()
		{
			return RemoveBalls(count.subtract(2));
		}

		public bool RemoveAllBalls()
		{
			if (!hasBalls)
			{
				return false;
			}
			setBalls(false);
			return true;
		}

		public byte EnlargeBalls(byte amount, bool ignorePerks = false)
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
			if (!ignorePerks)
			{
				//multiply it by the perk. float should be sanitized within passiveBaseStatModifier class.
				ushort val = (ushort)Math.Round(growthMultiplier * amount);
				amount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}

			if (amount >= MAX_BALLS_SIZE)
			{
				size = MAX_BALLS_SIZE;
			}
			else
			{
				size += amount;
			}
			return size.subtract(originalSize);
		}

		public byte ShrinkBalls(byte amount = 1, bool ignorePerks = false)
		{
			if (!hasBalls || size == MIN_BALLS_SIZE)
			{
				return 0;
			}
			byte originalSize = size;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(amount * shrinkMultiplier);
				amount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}
			if (size - amount < MIN_BALLS_SIZE) //we actually want this as an int compare b/c negative numbers.
			{
				size = MIN_BALLS_SIZE;
			}
			else
			{
				size -= amount;
			}
			return originalSize.subtract(size);
		}
		#endregion
		#region Text
		//overloads that control the fallback text when the creature has no balls. by default, it goes to prostate, though some text like 'non-existant balls'
		//exists if you'd prefer that and set the flag accordingly.
		public string ShortDescription(bool prostateIfNoBalls, out bool isPlural) => BallsStrings.ShortDesc(count, prostateIfNoBalls, out isPlural);
		public string ShortDescription(bool prostateIfNoBalls) => BallsStrings.ShortDesc(count, prostateIfNoBalls, out bool _);

		public string ShortDescription(out bool isPlural) => BallsStrings.ShortDesc(count, true, out isPlural);
		public string ShortDescription() => BallsStrings.ShortDesc(count, true, out bool _);

		public string SingleItemDescription(bool prostateIfNoBalls) => BallsStrings.SingleBallDesc(count, prostateIfNoBalls);
		public string SingleItemDescription() => BallsStrings.SingleBallDesc(count, true);

		public string DescriptionWithCount(bool preciseCount, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.CountDescription(this, preciseCount, alternateFormat, prostateIfNoBalls, out bool _);
		public string DescriptionWithCount(bool preciseCount, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.CountDescription(this, preciseCount, alternateFormat, prostateIfNoBalls, out isPlural);

		public string DescriptionWithSize(bool preciseSize) => BallsStrings.SizeDescription(this, preciseSize, out bool _);
		public string DescriptionWithSize(bool preciseSize, out bool isPlural) => BallsStrings.SizeDescription(this, preciseSize, out isPlural);

		public string DescriptionWithSizeOrProstate(bool preciseSize, bool prostateFallbackUsesAlternateForm = false)
		{
			if (hasBalls) return BallsStrings.SizeDescription(this, preciseSize, out bool _);
			else return BallsStrings.FallbackProstateText(prostateFallbackUsesAlternateForm);
		}

		public string DescriptionWithSizeOrProstate(bool preciseSize, bool prostateFallbackUsesAlternateForm, out bool isPlural)
		{
			if (hasBalls) return BallsStrings.SizeDescription(this, preciseSize, out isPlural);
			else
			{
				isPlural = false;
				return BallsStrings.FallbackProstateText(prostateFallbackUsesAlternateForm);
			}
		}

		public string LongDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.LongDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out bool _);
		public string LongDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.LongDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out isPlural);

		public string SackDescription() => BallsStrings.SackDescription(this);

		public string FullDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.FullDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out bool _);
		public string FullDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.FullDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out isPlural);
		#endregion

		public override bool IsIdenticalTo(BallsData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && original.hasBalls == hasBalls && original.uniBall == uniBall && count == original.count && size == original.size;
		}

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

		#region IGrowShrinkable
		bool IShrinkable.CanReducto()
		{
			return size > MIN_BALLS_SIZE;
		}

		float IShrinkable.UseReducto()
		{
			byte startVal = size;
			//even chance of 2 - 5, or 3-6 if we have a somewhat large shrink multiplier.
			if (((IShrinkable)this).CanReducto())
			{
				size = size.subtract((byte)(Utils.Rand(4) + 2));
			}
			return startVal - size;
		}

		bool IGrowable.CanGroPlus()
		{
			return size < MAX_BALLS_SIZE;
		}

		//executive deicision: GRO+ always removes uniball. idgaf.
		float IGrowable.UseGroPlus()
		{
			int startVal = size;
			if (((IGrowable)this).CanGroPlus())
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
		#endregion

		#region Helpers
		private void setBalls(bool balls, byte numBalls = 0, byte ballSize = 0, bool silent = false)
		{
			if (balls)
			{
				if (!hasBalls)
				{
					ballSize = ballSize == 0 ? getNewSize() : getNewSize(ballSize);
				}

				Utils.Clamp(ref numBalls, DEFAULT_MIN_COUNT, MAX_BALLS_COUNT);

				if (numBalls % 2 == 1) numBalls--;

				Utils.Clamp(ref ballSize, MIN_BALLS_SIZE, MAX_BALLS_SIZE);
			}
			else
			{
				numBalls = 0;
				ballSize = 0;
			}
			BallsData oldData = null;
			if (!silent && (count != numBalls || size != ballSize))
			{
				oldData = AsReadOnlyData();
			}

			count = numBalls;
			size = ballSize;
			if (oldData != null && !silent)
			{
				NotifyDataChanged(oldData);
			}
		}

		private void setUniBall(bool isUniBall, bool silent = false) //uniball ignores perks.
		{
			BallsData oldData = null;
			if (isUniBall)
			{
				if ((count != 1 || size != 1) && !silent)
				{
					oldData = AsReadOnlyData();
				}
				count = 1;
				size = 1;
			}
			else
			{
				if (hasBalls && !silent)
				{
					oldData = AsReadOnlyData();
				}
				count = 0;
				size = 0;
			}
			if (oldData != null)
			{
				NotifyDataChanged(oldData);
			}
		}
		#endregion
	}

	public sealed partial class BallsData : SimpleData
	{
		public readonly byte count;
		public readonly byte size;

		public bool hasBalls => count != 0;

		public bool uniBall => count == Balls.UNIBALL_COUNT;

		internal readonly BodyType bodyType;
		internal readonly float relativeLust;
		internal readonly int hoursSinceLastCum;
		#region Text
		//overloads that control the fallback text when the creature has no balls. by default, it goes to prostate, though some text like 'non-existant balls'
		//exists if you'd prefer that and set the flag accordingly.
		public string ShortDescription(bool prostateIfNoBalls, out bool isPlural) => BallsStrings.ShortDesc(count,  prostateIfNoBalls, out isPlural);
		public string ShortDescription(bool prostateIfNoBalls) => BallsStrings.ShortDesc(count, prostateIfNoBalls, out bool _);

		public string ShortDescription(out bool isPlural) => BallsStrings.ShortDesc(count, true, out isPlural);
		public string ShortDescription() => BallsStrings.ShortDesc(count, true, out bool _);

		public string SingleItemDescription(bool prostateIfNoBalls) => BallsStrings.SingleBallDesc(count, prostateIfNoBalls);
		public string SingleItemDescription() => BallsStrings.SingleBallDesc(count, true);

		public string DescriptionWithCount(bool preciseCount, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.CountDescription(this, preciseCount, alternateFormat, prostateIfNoBalls, out bool _);
		public string DescriptionWithCount(bool preciseCount, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.CountDescription(this, preciseCount, alternateFormat, prostateIfNoBalls, out isPlural);

		public string DescriptionWithSize(bool preciseSize) => BallsStrings.SizeDescription(this, preciseSize, out bool _);
		public string DescriptionWithSize(bool preciseSize, out bool isPlural) => BallsStrings.SizeDescription(this, preciseSize, out isPlural);

		public string DescriptionWithSizeOrProstate(bool preciseSize, bool prostateFallbackUsesAlternateForm = false)
		{
			if (hasBalls) return BallsStrings.SizeDescription(this, preciseSize, out bool _);
			else return BallsStrings.FallbackProstateText(prostateFallbackUsesAlternateForm);
		}

		public string DescriptionWithSizeOrProstate(bool preciseSize, bool prostateFallbackUsesAlternateForm, out bool isPlural)
		{
			if (hasBalls) return BallsStrings.SizeDescription(this, preciseSize, out isPlural);
			else
			{
				isPlural = false;
				return BallsStrings.FallbackProstateText(prostateFallbackUsesAlternateForm);
			}
		}

		public string LongDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.LongDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out bool _);
		public string LongDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.LongDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out isPlural);

		public string SackDescription() => BallsStrings.SackDescription(this);

		public string FullDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls = true) =>
			BallsStrings.FullDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out bool _);
		public string FullDescription(bool preciseMeasurements, bool alternateFormat, bool prostateIfNoBalls, out bool isPlural) =>
			BallsStrings.FullDescription(this, preciseMeasurements, alternateFormat, prostateIfNoBalls, out isPlural);
		#endregion
		internal BallsData(Balls source) : base(GetID(source))
		{
			count = source.count;
			size = source.size;

			hoursSinceLastCum = source.hoursSinceCum;
			bodyType = source.bodyType;
			relativeLust = source.relativeLust;
		}
		internal BallsData(Guid id, byte numBalls, byte ballSize, int hoursSinceLastCum, float relativeLust, BodyType bodyType) : base(id)
		{
			this.count = numBalls;
			this.size = ballSize;

			this.bodyType = bodyType;
			this.relativeLust = relativeLust;
			this.hoursSinceLastCum = hoursSinceLastCum;
		}

		private static Guid GetID(Balls source)
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			return source.creatureID;
		}
	}
}
