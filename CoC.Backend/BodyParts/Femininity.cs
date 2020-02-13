//GenitalHelpers.cs
//Description:
//Author: JustSomeGuy
//4/15/2019, 9:13 PM

using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Text;

//most of these are simply bytes, though a few do have extra behavior. An common software engineering practice is to never use primitives directly - this can be
//confusing or arbitrary - 5 could mean 5 years, 5 decades, 5 score, 5 centuries, etc. While i don't agree with that assessment 100%, it sometimes has merit.

//i'm not 100% familiar with C#'s optimizations, though it may align objects to word (4 byte) boundaries, which would mean these could all use ints instead, but w/e.
//though i suppose with even remotely modern hardware (read: it runs windows XP+) this game will never require memory sufficiently large enough to be an issue.
//Honestly, if this thing costs more than a few mbs (if that) i'll be very surprised.
namespace CoC.Backend.BodyParts
{
	public sealed partial class Femininity : SimpleSaveablePart<Femininity, FemininityData>, IBodyPartTimeLazy
	{
		public override string BodyPartName() => Name();

		internal readonly HashSet<IFemininityListenerInternal> internalFemininityListeners = new HashSet<IFemininityListenerInternal>();

		private Creature creature
		{
			get
			{
				CreatureStore.TryGetCreature(creatureID, out Creature creatureSource);
				return creatureSource;
			}
		}
		public const byte MOST_FEMININE = 100;
		public const byte MOST_MASCULINE = 0;

		public const byte MIN_ANDROGYNOUS = 35;
		public const byte ANDROGYNOUS = 50;
		public const byte MAX_ANDROGYNOUS = 65;

		public const byte SLIGHTLY_FEMININE = 60;
		public const byte FEMININE = 70;
		public const byte HYPER_FEMININE = 90;

		public const byte SLIGHTLY_MASCULINE = 40;
		public const byte MASCULINE = 30;
		public const byte HYPER_MASCULINE = 10;

		private const byte HERM_GENDERLESS_MIN = 20;
		private const byte HERM_GENDERLESS_MAX = 85;

		public const byte FEMALE_DEFAULT = 75;
		public const byte MALE_DEFAULT = 25;
		public const byte GENDERLESS_DEFAULT = ANDROGYNOUS;
		public const byte HERM_DEFAULT = SLIGHTLY_FEMININE;

		public byte value
		{
			get => _value;
			private set
			{
				if (value != _value)
				{
					byte oldValue = _value;
					_value = Utils.Clamp2(value, MOST_MASCULINE, MOST_FEMININE);
				}
			}
		}
		private byte _value;

		private Gender currentGender => creature?.genitals.gender ?? Gender.MALE;

		public bool femininityLimitedByGender
		{
			get => _femininityLimitedByGender;
			private set
			{
				if (_femininityLimitedByGender != value)
				{
					_femininityLimitedByGender = value;
					UpdateFemininity(this.value);
				}
			}
		}
		private bool _femininityLimitedByGender = false;


		public static implicit operator byte(Femininity femininity)
		{
			return femininity.value;
		}

		//by default, we don't know if we have androgyny. so we'll just allow all the data. This is corrected in lateInit.
		internal Femininity(Guid creatureID, Gender initialGender) : this(creatureID, initialGender, null)
		{ }

		internal Femininity(Guid creatureID, Gender initialGender, byte initialFemininity) : this(creatureID, initialGender, (byte?)initialFemininity)
		{ }

		private Femininity(Guid creatureID, Gender initialGender, byte? initialFemininity = null) : base(creatureID)
		{
			if (initialFemininity is byte femininity)
			{
				_value = femininity;
			}
			else if (initialGender == Gender.GENDERLESS)
			{
				_value = 50;
			}
			else if (initialGender == Gender.HERM)
			{
				_value = 60;
			}
			else if (initialGender == Gender.MALE)
			{
				_value = 25;
			}
			else
			{
				_value = 75;
			}
		}

		protected internal override void PostPerkInit()
		{
			if (femininityLimitedByGender)
			{
				UpdateFemininity(value);
			}
		}

		protected internal override void LateInit()
		{
			if (creature != null) creature.genitals.onGenderChanged += OnGenderChanged;
		}

		private void OnGenderChanged(object sender, GenderChangedEventArgs e)
		{
			UpdateFemininity(value);
		}
		private readonly HashSet<IFemininityListenerInternal> femininityListeners = new HashSet<IFemininityListenerInternal>();

		internal bool RegisterListener(IFemininityListenerInternal listener)
		{
			return femininityListeners.Add(listener);
		}

		internal bool DeregisterListener(IFemininityListenerInternal listener)
		{
			return femininityListeners.Remove(listener);
		}

		public bool ActivateAndrogyny()
		{
			if (!femininityLimitedByGender)
			{
				return false;
			}
			femininityLimitedByGender = false;
			return true;
		}

		public bool DeactivateAndrogyny()
		{
			if (femininityLimitedByGender)
			{
				return false;
			}
			femininityLimitedByGender = true;
			return true;
		}

		//private Femininity(Gender initialGender, byte femininity)
		//{
		//	genderAccessor = () => initialGender;
		//	_value = Utils.Clamp2(femininity, MOST_MASCULINE, MOST_FEMININE);
		//}

		//private Femininity(Femininity other)
		//{
		//	_value = other.value;
		//	genderAccessor = other.genderAccessor;
		//	femininityListeners = other.femininityListeners;
		//}

		public override FemininityData AsReadOnlyData()
		{
			return new FemininityData(this);
		}

		public bool isFemale => atLeastSlightlyFeminine;
		public bool isMale => atLeastSlightlyMasculine;

		public bool isAndrogynous => value >= MIN_ANDROGYNOUS && value <= MAX_ANDROGYNOUS;

		public bool isSlightlyFeminine => value >= SLIGHTLY_FEMININE && value < FEMININE;
		public bool atLeastSlightlyFeminine => value >= SLIGHTLY_FEMININE;
		public bool isFeminine => value >= FEMININE && value < HYPER_FEMININE;
		public bool atLeastFeminine => value >= FEMININE;
		public bool isHyperFeminine => value >= HYPER_FEMININE;
		public bool isSlightlyMasculine => value <= SLIGHTLY_MASCULINE && value > MASCULINE;
		public bool atLeastSlightlyMasculine => value <= SLIGHTLY_MASCULINE;
		public bool isMasculine => value <= MASCULINE && value > HYPER_MASCULINE;
		public bool atLeastMasculine => value <= MASCULINE;
		public bool isHyperMasculine => value <= HYPER_MASCULINE;

		public static bool valueIsFemale(byte fem) => valueAtLeastSlightlyFeminine(fem);
		public static bool valueIsMale(byte fem) => valueAtLeastSlightlyMasculine(fem);

		public static bool valueIsAndrogynous(byte fem) => fem >= MIN_ANDROGYNOUS && fem <= MAX_ANDROGYNOUS;

		public static bool valueIsSlightlyFeminine(byte fem) => fem >= SLIGHTLY_FEMININE && fem < FEMININE;
		public static bool valueAtLeastSlightlyFeminine(byte fem) => fem >= SLIGHTLY_FEMININE && fem < FEMININE;
		public static bool valueIsFeminine(byte fem) => fem >= FEMININE && fem < HYPER_FEMININE;
		public static bool valueAtLeastFeminine(byte fem) => fem >= FEMININE;
		public static bool valueIsHyperFeminine(byte fem) => fem >= HYPER_FEMININE;
		public static bool valueIsSlightlyMasculine(byte fem) => fem <= SLIGHTLY_MASCULINE && fem > MASCULINE;
		public static bool valueAtLeastSlightlyMasculine(byte fem) => fem <= SLIGHTLY_MASCULINE;
		public static bool valueIsMasculine(byte fem) => fem <= MASCULINE && fem > HYPER_MASCULINE;
		public static bool valueAtLeastMasculine(byte fem) => fem <= MASCULINE;
		public static bool valueIsHyperMasculine(byte fem) => fem <= HYPER_MASCULINE;

		public byte IncreaseFemininity(byte amount)
		{
			if (value >= MOST_FEMININE)
			{
				return 0;
			}
			byte oldFemininity = value;
			UpdateFemininity(value.add(amount));
			return value.subtract(oldFemininity);
		}

		public byte IncreaseMasculinity(byte amount)
		{
			if (value == 0)
			{
				return 0;
			}
			byte oldFemininity = value;
			UpdateFemininity(value.subtract(amount));
			return oldFemininity.subtract(value);
		}

		public byte SetFemininity(byte newValue)
		{
			UpdateFemininity(newValue);
			return value;
		}

		public short ChangeFemininityToward(byte target, byte increment)
		{
			if (target == value) return 0;

			byte oldFemininity = value;
			if (target < value)
			{
				if (value - increment <= target)
				{
					UpdateFemininity(target);
				}
				else
				{
					UpdateFemininity(value.subtract(increment));
				}
			}
			else
			{
				if (value + increment >= target)
				{
					UpdateFemininity(target);
				}
				else
				{
					UpdateFemininity(value.add(increment));
				}
			}

			return value.delta(oldFemininity);
		}

		internal byte FeminizeWithText(byte amount, out string output)
		{
			if (value >= MOST_FEMININE)
			{
				output = "";
				return 0;
			}
			byte oldFemininity = value;
			output = UpdateFemininity(value.add(amount));
			return value.subtract(oldFemininity);
		}

		internal byte MasculinizeWithText(byte amount, out string output)
		{
			if (value == 0)
			{
				output = "";
				return 0;
			}
			byte oldFemininity = value;
			output = UpdateFemininity(value.subtract(amount));
			return oldFemininity.subtract(value);
		}

		internal byte SetFemininityWithText(byte newValue, out string output)
		{
			output = UpdateFemininity(newValue);
			return value;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			value = value;
			return true;
		}

		public override bool IsIdenticalTo(FemininityData originalData, bool ignoreSexualMetaData)
		{
			return !(originalData is null) && originalData.value == value && femininityLimitedByGender == originalData.femininityLimitedByGender;
		}

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			if (femininityLimitedByGender)
			{
				byte oldValue = value;
				UpdateFemininity(value);
				StringBuilder sb = new StringBuilder();
				if (oldValue != value)
				{
					List<IFemininityListenerInternal> items = new List<IFemininityListenerInternal>(femininityListeners);
					//copy list so they can be removed as they fire. Not really necessary, but good practice, i guess.
					items.ForEach(x => sb.AppendLine(x.reactToFemininityChangeFromTimePassing(isPlayer, hoursPassed, oldValue)));
				}
				return sb.ToString();
			}
			return "";
		}


		private string UpdateFemininity(byte newValue, bool doOutput = false)
		{
			//set current min, max based on gender;
			byte minVal, maxVal;
			if (femininityLimitedByGender)
			{
				switch (currentGender)
				{
					case Gender.MALE:
						minVal = MOST_MASCULINE; maxVal = FEMININE; break;
					case Gender.FEMALE:
						minVal = MASCULINE; maxVal = MOST_FEMININE; break;
					case Gender.GENDERLESS:
					case Gender.HERM:
						minVal = HERM_GENDERLESS_MIN; maxVal = HERM_GENDERLESS_MAX; break;
					default:
						throw new ArgumentException("Current gender not recognized");
				}
			}
			else
			{
				minVal = MOST_MASCULINE;
				maxVal = MOST_FEMININE;
			}
			Utils.Clamp(ref newValue, minVal, maxVal);
			if (newValue != value)
			{
				var oldData = AsReadOnlyData();
				value = newValue;
				NotifyDataChanged(oldData);
				StringBuilder sb = new StringBuilder();
				sb.Append(FemininityChangedDueToGenderHormonesStr(oldData.value.delta(value)));
				foreach (var item in internalFemininityListeners)
				{
					sb.Append(item.reactToFemininityChange(oldData.value) ?? "");
				}
				return sb.ToString();
			}

			return "";
		}
	}

	public sealed class FemininityData : SimpleData
	{
		public const byte MOST_FEMININE = Femininity.MOST_FEMININE;
		public const byte MOST_MASCULINE = Femininity.MOST_MASCULINE;

		public const byte MIN_ANDROGYNOUS = Femininity.MIN_ANDROGYNOUS;
		public const byte ANDROGYNOUS = Femininity.ANDROGYNOUS;
		public const byte MAX_ANDROGYNOUS = Femininity.MAX_ANDROGYNOUS;

		public const byte SLIGHTLY_FEMININE = Femininity.SLIGHTLY_FEMININE;
		public const byte FEMININE = Femininity.FEMININE;
		public const byte HYPER_FEMININE = Femininity.HYPER_FEMININE;

		public const byte SLIGHTLY_MASCULINE = Femininity.SLIGHTLY_MASCULINE;
		public const byte MASCULINE = Femininity.MASCULINE;
		public const byte HYPER_MASCULINE = Femininity.HYPER_MASCULINE;

		public readonly byte value;
		public readonly bool femininityLimitedByGender;

		//enums are passed by value, so this should be fine.
		internal FemininityData(Femininity fem) : base(fem?.creatureID ?? throw new ArgumentNullException(nameof(fem)))
		{
			value = fem;
			femininityLimitedByGender = fem.femininityLimitedByGender;
		}

		internal FemininityData(Guid id, byte fem, bool limitedByGender = true) : base(id)
		{
			value = fem;
			femininityLimitedByGender = limitedByGender;
		}

		public static implicit operator byte(FemininityData femininity)
		{
			return femininity.value;
		}

		public bool isFemale => Femininity.valueIsFemale(value);
		public bool isMale => Femininity.valueIsMale(value);

		public bool isAndrogynous => Femininity.valueIsAndrogynous(value);

		public bool isSlightlyFeminine => Femininity.valueIsSlightlyFeminine(value);
		public bool atLeastSlightlyFeminine => Femininity.valueAtLeastSlightlyFeminine(value);
		public bool isFeminine => Femininity.valueIsFeminine(value);
		public bool atLeastFeminine => Femininity.valueAtLeastFeminine(value);
		public bool isHyperFeminine => Femininity.valueIsHyperFeminine(value);
		public bool isSlightlyMasculine => Femininity.valueIsSlightlyMasculine(value);
		public bool atLeastSlightlyMasculine => Femininity.valueAtLeastSlightlyMasculine(value);
		public bool isMasculine => Femininity.valueIsMasculine(value);
		public bool atLeastMasculine => Femininity.valueAtLeastMasculine(value);
		public bool isHyperMasculine => Femininity.valueIsHyperMasculine(value);
	}
}
