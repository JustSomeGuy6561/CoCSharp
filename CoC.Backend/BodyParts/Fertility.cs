//GenitalHelpers.cs
//Description:
//Author: JustSomeGuy
//4/15/2019, 9:13 PM

using CoC.Backend.Creatures;
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
	public sealed class Fertility : SimpleSaveablePart<Fertility, FertilityData>
	{
		public const byte MAX_TOTAL_FERTILITY = byte.MaxValue - 5;
		public const byte MAX_BASE_FERTILITY = 75;

		//byte value => something, idk.

		public bool isInfertile
		{
			get => _isInfertile;
			private set
			{
				if (_isInfertile != value)
				{
					var oldData = AsReadOnlyData();
					_isInfertile = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private bool _isInfertile;

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
		internal byte perkBonusFertility
		{
			get => _perkBonusFertility;
			set
			{
				if (_perkBonusFertility != value)
				{
					var oldData = AsReadOnlyData();
					_perkBonusFertility = value;
					NotifyDataChanged(oldData);
				}
			}

		}
		private byte _perkBonusFertility = 0;

		public override FertilityData AsReadOnlyData()
		{
			return new FertilityData(this);
		}

		public byte bonusFertility { get; internal set; }

		public byte totalFertility => Math.Min(MAX_TOTAL_FERTILITY, baseFertility.add(bonusFertility));
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
			isInfertile = artificiallyInfertile;
		}

		internal Fertility(Guid creatureID, byte value, bool artificiallyInfertile = false) : base(creatureID)
		{
			baseFertility = value;
			isInfertile = artificiallyInfertile;
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

		public bool MakeInfertile()
		{
			if (isInfertile)
			{
				return false;
			}
			isInfertile = true;
			return true;
		}

		public bool MakeFertile()
		{
			if (!isInfertile)
			{
				return false;
			}
			isInfertile = false;
			return true;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			baseFertility = baseFertility;
			return true;
		}
	}

	public sealed class FertilityData : SimpleData
	{
		public readonly byte currentFertilityValue;
		public readonly bool artificiallyInfertile;
		public byte fertility => artificiallyInfertile ? (byte)0 : currentFertilityValue;

		public FertilityData(Fertility source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			currentFertilityValue = source.currentFertility;
			artificiallyInfertile = source.isInfertile;
		}

	}
}