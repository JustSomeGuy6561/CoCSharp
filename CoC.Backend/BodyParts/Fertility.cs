//GenitalHelpers.cs
//Description:
//Author: JustSomeGuy
//4/15/2019, 9:13 PM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Engine.Time;
using CoC.Backend.Tools;
using System;

//most of these are simply bytes, though a few do have extra behavior. An common software engineering practice is to never use primitives directly - this can be
//confusing or arbitrary - 5 could mean 5 years, 5 decades, 5 score, 5 centuries, etc. While i don't agree with that assessment 100%, it sometimes has merit.

//i'm not 100% familiar with C#'s optimizations, though it may align objects to word (4 byte) boundaries, which would mean these could all use ints instead, but w/e.
//though i suppose with even remotely modern hardware (read: it runs windows XP+) this game will never require memory sufficiently large enough to be an issue.
//Honestly, if this thing costs more than a few mbs (if that) i'll be very surprised.
namespace CoC.Backend.BodyParts
{
	//it wraps a byte. I dunno.
	//now capping max base fertility to 75. Perks could boost this past the base 75 value.
	public sealed partial class Fertility : SimpleSaveablePart<Fertility, FertilityData>, IBodyPartTimeLazy
	{
		public override string BodyPartName() => Name();

		public const byte MAX_TOTAL_FERTILITY = byte.MaxValue - 5;
		public const byte MAX_BASE_FERTILITY = 75;

		//byte value => something, idk.

		private GameDateTime timeContraceptivesWearOff
		{
			get => _timeContraceptivesWearOff;

			set
			{
				if (!_timeContraceptivesWearOff.Equals(value))
				{
					var oldData = AsReadOnlyData();
					_timeContraceptivesWearOff = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private GameDateTime _timeContraceptivesWearOff;


		public int hoursUntilContraceptivesWearOff => timeContraceptivesWearOff?.hoursToNow() ?? 0;

		public bool permanentlyInfertile
		{
			get => _permanentlyInfertile;
			private set
			{
				if (_permanentlyInfertile != value)
				{
					var oldData = AsReadOnlyData();
					_permanentlyInfertile = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private bool _permanentlyInfertile = false;

		public bool temporarilyInfertile => hoursUntilContraceptivesWearOff > 0;

		public bool isInfertile => permanentlyInfertile || temporarilyInfertile;

		public byte baseFertility
		{
			get => _baseValue;
			private set
			{
				Utils.Clamp<byte>(ref value, 0, MAX_BASE_FERTILITY);
				if (_baseValue != value)
				{
					var oldData = AsReadOnlyData();
					_baseValue = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _baseValue;
		private byte perkBonusFertility => CreatureStore.GetCreatureClean(creatureID)?.perks.baseModifiers.bonusFertility.GetValue() ?? 0;

		internal void OnPerkFertilityChange(byte oldValue)
		{
			var oldFertilityValue = Math.Min(MAX_TOTAL_FERTILITY, baseFertility.add(oldValue));
			var oldData = new FertilityData(this, oldFertilityValue);
			NotifyDataChanged(oldData);
		}

		public override FertilityData AsReadOnlyData()
		{
			return new FertilityData(this);
		}

		public byte totalFertility => Math.Min(MAX_TOTAL_FERTILITY, baseFertility.add(perkBonusFertility));
		public byte currentFertility => isInfertile ? (byte)0 : totalFertility;


		internal Fertility(Guid creatureID, Gender gender, bool artificiallyInfertile = false) : base(creatureID)
		{
			switch (gender)
			{
				case Gender.FEMALE:
				case Gender.HERM:
					baseFertility = 10; break;
				case Gender.MALE:
				case Gender.GENDERLESS:
				default:
					baseFertility = 5; break;
			}
			permanentlyInfertile = artificiallyInfertile;
		}

		internal Fertility(Guid creatureID, byte value, bool artificiallyInfertile = false) : base(creatureID)
		{
			baseFertility = value;
			permanentlyInfertile = artificiallyInfertile;
		}

		public byte IncreaseFertility(byte amount = 1, bool increaseIfInfertile = true)
		{
			if (!isInfertile || increaseIfInfertile)
			{
				byte oldAmount = baseFertility;
				baseFertility = baseFertility.add(amount);
				return baseFertility.subtract(oldAmount);
			}
			return 0;
		}

		public byte DecreaseFertility(byte amount = 1, bool decreaseIfInfertile = true)
		{
			if (!isInfertile || decreaseIfInfertile)
			{
				byte oldAmount = baseFertility;
				baseFertility = baseFertility.subtract(amount);
				return oldAmount.subtract(baseFertility);
			}
			return 0;
		}

		public byte SetFertility(byte newValue, bool changeIfInfertile = true)
		{
			if (!isInfertile || changeIfInfertile)
			{
				baseFertility = newValue;
			}
			return baseFertility;
		}

		public bool MakeTemporarilyInfertile(byte timeout)
		{
			if (permanentlyInfertile)
			{
				return false;
			}
			if (hoursUntilContraceptivesWearOff > timeout)
			{
				return false;
			}
			else if (hoursUntilContraceptivesWearOff == timeout)
			{
				return true;
			}
			else
			{
				timeContraceptivesWearOff = GameDateTime.HoursFromNow(timeout);
				return true;
			}
		}

		public bool RemoveTemporaryInfertility()
		{
			if (timeContraceptivesWearOff is null)
			{
				return false;
			}

			timeContraceptivesWearOff = null;
			return true;
		}

		public bool MakePermanentlyInfertile()
		{
			if (permanentlyInfertile)
			{
				return false;
			}
			permanentlyInfertile = true;
			return true;
		}

		public bool ClearPermanentInfertility()
		{
			if (!permanentlyInfertile)
			{
				return false;
			}
			permanentlyInfertile = false;
			return true;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			baseFertility = baseFertility;
			return true;
		}

		public override bool IsIdenticalTo(FertilityData originalData, bool ignoreSexualMetaData)
		{
			return !(originalData is null) && originalData.currentFertilityValue == totalFertility && isInfertile == originalData.artificiallyInfertile;
		}

		public string reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			if (timeContraceptivesWearOff.CompareTo(GameDateTime.Now) <= 0)
			{
				timeContraceptivesWearOff = null;
			}
			return null;
		}
	}

	public sealed class FertilityData : SimpleData
	{
		public readonly byte currentFertilityValue;
		public readonly bool temporarilyInfertile;
		public readonly bool permanentlyInfertile;

		public bool artificiallyInfertile => temporarilyInfertile || permanentlyInfertile;

		public readonly int hoursUntilContraceptivesWearOff;

		public byte fertility => artificiallyInfertile ? (byte)0 : currentFertilityValue;

		public FertilityData(Fertility source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			currentFertilityValue = source.currentFertility;
			temporarilyInfertile = source.temporarilyInfertile;
			permanentlyInfertile = source.permanentlyInfertile;
			hoursUntilContraceptivesWearOff = source.hoursUntilContraceptivesWearOff;
		}

		public FertilityData(Fertility source, byte overrideFertility) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			currentFertilityValue = overrideFertility;
			temporarilyInfertile = source.temporarilyInfertile;
			permanentlyInfertile = source.permanentlyInfertile;
			hoursUntilContraceptivesWearOff = source.hoursUntilContraceptivesWearOff;
		}

	}
}
