﻿//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CoC.Backend.BodyParts
{
	public sealed partial class LabiaPiercingLocation : PiercingLocation, IEquatable<LabiaPiercingLocation>
	{
		private static readonly List<LabiaPiercingLocation> _allLocations = new List<LabiaPiercingLocation>();

		public static readonly ReadOnlyCollection<LabiaPiercingLocation> allLocations;

		private readonly byte index;

		static LabiaPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<LabiaPiercingLocation>(_allLocations);
		}

		public LabiaPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
			: base(allowsJewelryOfType, btnText, locationDesc)
		{
			this.index = index;

			if (!_allLocations.Contains(this))
			{
				_allLocations.Add(this);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is LabiaPiercingLocation labiaPiercing)
			{
				return Equals(labiaPiercing);
			}
			else return false;
		}

		public bool Equals(LabiaPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly LabiaPiercingLocation LEFT_1 = new LabiaPiercingLocation(0, SupportedJewelry, Left1Button, Left1Location);
		public static readonly LabiaPiercingLocation LEFT_2 = new LabiaPiercingLocation(1, SupportedJewelry, Left2Button, Left2Location);
		public static readonly LabiaPiercingLocation LEFT_3 = new LabiaPiercingLocation(2, SupportedJewelry, Left3Button, Left3Location);
		public static readonly LabiaPiercingLocation LEFT_4 = new LabiaPiercingLocation(3, SupportedJewelry, Left4Button, Left4Location);
		public static readonly LabiaPiercingLocation LEFT_5 = new LabiaPiercingLocation(4, SupportedJewelry, Left5Button, Left5Location);
		public static readonly LabiaPiercingLocation LEFT_6 = new LabiaPiercingLocation(5, SupportedJewelry, Left6Button, Left6Location);

		public static readonly LabiaPiercingLocation RIGHT_1 = new LabiaPiercingLocation(6, SupportedJewelry, Right1Button, Right1Location);
		public static readonly LabiaPiercingLocation RIGHT_2 = new LabiaPiercingLocation(7, SupportedJewelry, Right2Button, Right2Location);
		public static readonly LabiaPiercingLocation RIGHT_3 = new LabiaPiercingLocation(8, SupportedJewelry, Right3Button, Right3Location);
		public static readonly LabiaPiercingLocation RIGHT_4 = new LabiaPiercingLocation(9, SupportedJewelry, Right4Button, Right4Location);
		public static readonly LabiaPiercingLocation RIGHT_5 = new LabiaPiercingLocation(10, SupportedJewelry,Right5Button, Right5Location );
		public static readonly LabiaPiercingLocation RIGHT_6 = new LabiaPiercingLocation(11, SupportedJewelry,Right6Button, Right6Location );


		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}


	}

	public sealed class LabiaPiercing : Piercing<LabiaPiercingLocation>
	{
		public LabiaPiercing(PiercingUnlocked LocationUnlocked, PlayerStr playerDesc) : base(LocationUnlocked, playerDesc)
		{
		}

		public override int MaxPiercings => LabiaPiercingLocation.allLocations.Count;

		public override IEnumerable<LabiaPiercingLocation> availableLocations => LabiaPiercingLocation.allLocations;
	}


	//Note: this class is created after perks have been initialized, so its post perk init is never called.

	//unlike ass wetness changes, this seems fine by me. the normal case is some wetness, though dryer than that is not terribly strange.
	public enum VaginalWetness : byte { DRY, NORMAL, WET, SLICK, DROOLING, SLAVERING }

	//i have, however, renamed these. gaping-wide-> gaping. gaping-> roomy. could even rename clown car to gaping-wide if clown car is a little too bizarre, but i'm kinda fond of its bizarre-ness.
	public enum VaginalLooseness : byte { TIGHT, NORMAL, LOOSE, ROOMY, GAPING, CLOWN_CAR_WIDE }

	public sealed partial class Vagina : BehavioralSaveablePart<Vagina, VaginaType, VaginaData>, IBodyPartTimeLazy
	{
		public override string BodyPartName()
		{
			return Name();
		}


		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_VAGINAL_CAPACITY = ushort.MaxValue;

		public readonly Clit clit;

		public int vaginaIndex => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.genitals.vaginas.IndexOf(this) : 0;

		internal VaginalLooseness minLooseness
		{
			get => _minLooseness;
			set
			{
				_minLooseness = value;
				looseness = looseness; //auto-correct property;
				if (maxLooseness < _minLooseness)
				{
					maxLooseness = _minLooseness;
				}
			}
		}
		private VaginalLooseness _minLooseness = VaginalLooseness.TIGHT;
		internal VaginalLooseness maxLooseness
		{
			get => _maxLooseness;
			set
			{
				if (value < minLooseness)
				{
					value = minLooseness;
				}
				_maxLooseness = value;
				looseness = looseness; //auto-correct property;
			}
		}
		private VaginalLooseness _maxLooseness = VaginalLooseness.CLOWN_CAR_WIDE;


		internal VaginalWetness minWetness
		{
			get => _minWetness;
			set
			{
				_minWetness = value;
				wetness = wetness; //auto-correct property;
				if (maxWetness < _minWetness)
				{
					maxWetness = _minWetness;
				}
			}
		}
		private VaginalWetness _minWetness = VaginalWetness.DRY;
		internal VaginalWetness maxWetness
		{
			get => _maxWetness;
			set
			{
				if (value < minWetness)
				{
					value = minWetness;
				}
				_maxWetness = value;
				wetness = wetness; //auto-correct property;
			}
		}
		private VaginalWetness _maxWetness = VaginalWetness.SLAVERING;

		public VaginalWetness wetness
		{
			get => _wetness;
			private set
			{
				Utils.ClampEnum(ref value, minWetness, maxWetness);
				if (_wetness != value)
				{
					var oldData = AsReadOnlyData();
					_wetness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private VaginalWetness _wetness;

		public VaginalLooseness looseness
		{
			get => _looseness;
			private set
			{
				Utils.ClampEnum(ref value, minLooseness, maxLooseness);
				if (_looseness != value)
				{
					var oldData = AsReadOnlyData();
					_looseness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private VaginalLooseness _looseness;

		public ushort orgasmCount { get; private set; } = 0;
		public ushort sexCount { get; private set; } = 0;
		public ushort totalPenetrationCount { get; private set; } = 0;
		public ushort dryOrgasmCount { get; private set; } = 0;

		public bool isVirgin { get; private set; } = true;

		public bool everPracticedVaginal => totalPenetrationCount > 0;

		public ushort bonusVaginalCapacity
		{
			get => _bonusVaginalCapacity;
			private set
			{
				if (_bonusVaginalCapacity != value)
				{
					var oldData = AsReadOnlyData();
					_bonusVaginalCapacity = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private ushort _bonusVaginalCapacity = 0;

		internal ushort perkBonusVaginalCapacity
		{
			get => _perkBonusVaginalCapacity;
			set
			{
				if (_perkBonusVaginalCapacity != value)
				{
					var oldData = AsReadOnlyData();
					_perkBonusVaginalCapacity = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private ushort _perkBonusVaginalCapacity = 0;

		public ushort VaginalCapacity()
		{

			byte loose = (byte)looseness;
			if (!isVirgin)
			{
				loose++;
			}
			byte wet = ((byte)wetness).add(1);
			uint cap = (uint)Math.Floor(BASE_CAPACITY + bonusVaginalCapacity + perkBonusVaginalCapacity /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_VAGINAL_CAPACITY)
			{
				return MAX_VAGINAL_CAPACITY;
			}
			return (ushort)cap;
		}

		private const ushort LOOSENESS_LOOSE_TIMER = 200;
		private const ushort LOOSENESS_ROOMY_TIMER = 100;
		private const ushort LOOSENESS_GAPING_TIMER = 70;
		private const ushort LOOSENESS_CLOWN_CAR_TIMER = 50;
		private ushort vaginaTightenTimer = 0;
		public readonly LabiaPiercing labiaPiercings;
		public override VaginaType type { get; protected set; }
		public override VaginaType defaultType => VaginaType.defaultValue;

		#region Constructors
		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkData) : this(creatureID, initialPerkData, VaginaType.defaultValue, clitLength: initialPerkData.DefaultNewClitSize)
		{ }

		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkData, VaginaType vaginaType) : base(creatureID)
		{
			clit = new Clit(creatureID, this, initialPerkData);
			isVirgin = true;
			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));
			_wetness = initialPerkData.defaultWetnessNew;
			_looseness = initialPerkData.defaultLoosenessNew;

			labiaPiercings = new LabiaPiercing(PiercingLocationUnlocked, AllLabiaPiercingsStr);
		}

		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkData, VaginaType vaginaType, float clitLength,
			VaginalLooseness? vaginalLooseness = null, VaginalWetness? vaginalWetness = null, bool? isVirgin = null, bool omnibus = false) : base(creatureID)
		{
			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));

			clit = new Clit(creatureID, this, initialPerkData, clitLength, omnibus);
			if (isVirgin is null)
			{
				isVirgin = vaginalLooseness == VaginalLooseness.TIGHT || vaginalLooseness is null;
			}
			this.isVirgin = (bool)isVirgin;

			minLooseness = initialPerkData.minLooseness;
			maxLooseness = initialPerkData.maxLooseness;
			minWetness = initialPerkData.minWetness;
			maxWetness = initialPerkData.maxWetness;

			perkBonusVaginalCapacity = initialPerkData.perkBonusCapacity;

			//these are clamped by above set values.
			_wetness = vaginalWetness ?? initialPerkData.defaultWetnessNew;
			_looseness = vaginalLooseness ?? initialPerkData.defaultLoosenessNew;

			labiaPiercings = new LabiaPiercing(PiercingLocationUnlocked, AllLabiaPiercingsStr);
		}

		#endregion

		#region Generate
		internal static Vagina GenerateFromGender(Guid creatureID, VaginaPerkHelper initialPerkData, Gender gender)
		{
			if (gender.HasFlag(Gender.FEMALE)) return new Vagina(creatureID, initialPerkData);
			else return null;
		}

		public static VaginaData GenerateAggregate(Guid creatureID, VaginaType vaginaType, ClitData averageClit, VaginalLooseness looseness, VaginalWetness wetness, bool isVirgin,
			bool everPracticedVaginal, ushort averageCapacity)
		{
			return new VaginaData(creatureID, vaginaType, averageClit, looseness, wetness, isVirgin, everPracticedVaginal, -1, averageCapacity, new ReadOnlyPiercing<LabiaPiercingLocation>());
		}

		#endregion
		#region Text
		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string AdjectiveText(bool multipleAdjectives)
		{
			return VaginaType.VaginaAdjectiveText(AsReadOnlyData(), multipleAdjectives);
		}

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(AsReadOnlyData());

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(AsReadOnlyData());

		public string FullDescription(bool alternateFormat) => type.FullDescription(AsReadOnlyData(), alternateFormat);
		#endregion

		public override VaginaData AsReadOnlyData()
		{
			return new VaginaData(this, vaginaIndex);
		}

		#region Update

		internal override bool UpdateType(VaginaType newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			if (newType.orgasmOnTransform)
			{
				if (CreatureStore.TryGetCreature(creatureID, out Creature creature))
				{
					creature.HaveGenericVaginalOrgasm(vaginaIndex, false, true);
				}
				else
				{
					OrgasmGeneric(false);
				}
			}
			type = newType;

			NotifyTypeChanged(oldValue);


			return true;
		}

		#endregion
		#region Unique Functions

		internal bool PenetrateVagina(ushort penetratorArea, float knotArea, bool takeVirginity, bool reachOrgasm)
		{
			totalPenetrationCount++;

			//experience = experience.add(ExperiencedGained);
			VaginalLooseness oldLooseness = looseness;

			HandleStretching(penetratorArea, knotArea);

			if (takeVirginity)
			{
				sexCount++;
				isVirgin = false;
			}
			if (reachOrgasm)
			{
				orgasmCount++;
			}

			return oldLooseness != looseness;
		}

		internal void OrgasmGeneric(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
		}

		internal void HandleBirth(ushort size)
		{
			HandleStretching(size, 0);
		}

		private void HandleStretching(ushort penetratorArea, float knotArea)
		{
			ushort capacity = VaginalCapacity();

			//don't have to worry about overflow, as +1 will never overflow our artificial max.
			if (penetratorArea >= capacity * 1.5f)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity && Utils.RandBool())
			{
				looseness++;
			}
			else if (penetratorArea >= capacity * 0.9f && Utils.Rand(4) == 0)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity * 0.75f && Utils.Rand(10) == 0)
			{
				looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				vaginaTightenTimer = 0;
			}
		}

		#endregion
		#region Vagina-Specific
		internal bool Deflower()
		{
			if (!isVirgin)
			{
				return false;
			}
			isVirgin = false;
			return true;
		}

		internal byte StretchVagina(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumAdd(amount);
			return looseness - oldLooseness;
		}

		internal byte ShrinkVagina(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumSubtract(amount);
			return oldLooseness - looseness;
		}

		internal bool SetVaginalLooseness(VaginalLooseness Looseness)
		{
			if (Looseness >= minLooseness && Looseness <= maxLooseness)
			{
				looseness = Looseness;
				return true;
			}
			return false;
		}

		internal byte MakeWetter(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		internal byte MakeDrier(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		internal bool SetVaginalWetness(VaginalWetness Wetness)
		{
			if (Wetness >= minWetness && Wetness <= maxWetness)
			{
				wetness = Wetness;
				return true;
			}
			return false;
		}

		internal ushort AddBonusCapacity(ushort amountToAdd)
		{
			ushort currentCapacity = bonusVaginalCapacity;
			bonusVaginalCapacity = bonusVaginalCapacity.add(amountToAdd);
			return bonusVaginalCapacity.subtract(currentCapacity);
		}

		internal ushort SubtractBonusCapacity(ushort amountToRemove)
		{
			ushort currentCapacity = bonusVaginalCapacity;
			bonusVaginalCapacity = bonusVaginalCapacity.subtract(amountToRemove);
			return bonusVaginalCapacity.subtract(currentCapacity);
		}



		#endregion
		#region Clit Helpers
		public bool omnibusClit => clit.omnibusClit;

		public bool ActivateOmnibusClit()
		{
			if (!clit.omnibusClit)
			{
				var oldData = AsReadOnlyData();
				bool retVal = clit.ActivateOmnibusClit();
				NotifyDataChanged(oldData);
				return retVal;
			}
			return false;
		}

		public bool DeactivateOmnibusClit()
		{
			if (clit.omnibusClit)
			{
				var oldData = AsReadOnlyData();
				var retVal = clit.DeactivateOmnibusClit();
				NotifyDataChanged(oldData);
				return retVal;
			}
			return false;
		}

		public float growClit(float amount, bool ignorePerks = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = clit.growClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float shrinkClit(float amount, bool ignorePerks = false)
		{
			var oldData = AsReadOnlyData();
			var retVal = clit.shrinkClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float SetClitSize(float newSize)
		{
			var oldData = AsReadOnlyData();
			var oldSize = clit.length;
			var retVal = clit.SetClitSize(newSize);
			if (clit.length != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}
		#endregion
		#region Restore
		internal override bool Restore()
		{
			clit.Restore();
			if (type == VaginaType.HUMAN)
			{
				return false;
			}
			var oldType = type;
			type = VaginaType.HUMAN;

			NotifyTypeChanged(oldType);
			return true;
		}

		#endregion
		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			VaginaType vaginaType = type;
			bool valid = VaginaType.Validate(ref vaginaType, correctInvalidData);
			if (valid || correctInvalidData)
			{
				valid &= labiaPiercings.Validate(correctInvalidData); // = x & so we're fine.
			}
			if (valid || correctInvalidData)
			{
				valid &= clit.Validate(correctInvalidData);
			}
			return valid;
		}
		#endregion
		#region Piercing-Related
		private bool PiercingLocationUnlocked(LabiaPiercingLocation piercingLocation, out string whyNot)
		{
			whyNot = null;
			return true;
		}

		public bool isPierced => clit.isPierced || labiaPiercings.isPierced;
		public bool isClitPierced => clit.isPierced;
		public bool isLabiaPierced => labiaPiercings.isPierced;

		public bool wearingAnyJewelry => clit.wearingJewelry || labiaPiercings.wearingJewelry;
		public bool clitWearingJewelry => clit.wearingJewelry;
		public bool labiaWearingJewelry => labiaPiercings.wearingJewelry;

		internal void InitializePiercings(Dictionary<ClitPiercingLocation, PiercingJewelry> clitPiercings, Dictionary<LabiaPiercing, PiercingJewelry> labiaPiercings)
		{
#warning Implement Me!
			//throw new Tools.InDevelopmentExceptionThatBreaksOnRelease();
		}

		#endregion
		#region ITimeListener

		private ushort timerAmount
		{
			get
			{
				if (looseness < VaginalLooseness.LOOSE)
				{
					return 0;
				}
				else if (looseness == VaginalLooseness.LOOSE)
				{
					return LOOSENESS_LOOSE_TIMER;
				}
				else if (looseness == VaginalLooseness.ROOMY)
				{
					return LOOSENESS_ROOMY_TIMER;
				}
				else if (looseness == VaginalLooseness.GAPING)
				{
					return LOOSENESS_GAPING_TIMER;
				}
				else //if (looseness >= VaginalLooseness.CLOWN_CAR_LEVEL)
				{
					return LOOSENESS_CLOWN_CAR_TIMER;
				}
			}
		}

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			StringBuilder sb = new StringBuilder();

			if (looseness < minLooseness)
			{
				looseness = minLooseness;
				vaginaTightenTimer = 0;
			}
			else if (looseness > maxLooseness)
			{
				looseness = maxLooseness;
				vaginaTightenTimer = 0;
			}

			else if (looseness > VaginalLooseness.NORMAL && looseness > minLooseness) //whichever is greator.
			{
				vaginaTightenTimer += hoursPassed;
				var oldLooseness = looseness;
				while (vaginaTightenTimer >= timerAmount && looseness > minLooseness && looseness > VaginalLooseness.NORMAL)
				{
					vaginaTightenTimer -= timerAmount;
					looseness--;
				}
				if (isPlayer)
				{
					sb.Append(VaginaTightenedUpDueToInactivity(oldLooseness));
				}
			}

			else if (vaginaTightenTimer > 0)
			{
				vaginaTightenTimer = 0;
			}

			if (wetness < minWetness)
			{
				wetness = minWetness;
			}
			else if (wetness > maxWetness)
			{
				wetness = maxWetness;
			}

			//if we decide to change behavior so that clit size is forced, call clit react to time passing, appending its result to ours.

			return sb.ToString();
		}

		#endregion

		#region NYI or Potential Ideas
		//min and max looseness/wetness are locked to perks. because reasons.
		//internal byte IncreaseMinimumLooseness(byte amount = 1, bool forceIncreaseMax = false)
		//{
		//	VaginalLooseness looseness = minLooseness;
		//	minLooseness = minLooseness.ByteEnumAdd(amount);
		//	if (minLooseness > maxLooseness)
		//	{
		//		if (forceIncreaseMax)
		//		{
		//			maxLooseness = minLooseness;
		//		}
		//		else
		//		{
		//			minLooseness = maxLooseness;
		//		}
		//	}
		//	return minLooseness - looseness;
		//}
		//internal byte DecreaseMinimumLooseness(byte amount = 1)
		//{
		//	VaginalLooseness looseness = minLooseness;
		//	minLooseness = minLooseness.ByteEnumSubtract(amount);
		//	return looseness - minLooseness;
		//}
		//internal void SetMinLoosness(VaginalLooseness newValue)
		//{
		//	minLooseness = newValue;
		//}

		//internal byte IncreaseMaximumLooseness(byte amount = 1)
		//{
		//	VaginalLooseness looseness = maxLooseness;
		//	maxLooseness = maxLooseness.ByteEnumSubtract(amount);
		//	return maxLooseness - looseness;
		//}
		//internal byte DecreaseMaximumLooseness(byte amount = 1, bool forceDecreaseMin = false)
		//{
		//	VaginalLooseness looseness = minLooseness;
		//	maxLooseness = maxLooseness.ByteEnumSubtract(amount);
		//	if (minLooseness > maxLooseness)
		//	{
		//		if (forceDecreaseMin)
		//		{
		//			minLooseness = maxLooseness;
		//		}
		//		else
		//		{
		//			maxLooseness = minLooseness;
		//		}
		//	}
		//	return looseness - maxLooseness;
		//}
		//internal void SetMaxLoosness(VaginalLooseness newValue)
		//{
		//	maxLooseness = newValue;
		//}

		//internal byte IncreaseMinimumWetness(byte amount = 1, bool forceIncreaseMax = false)
		//{
		//	VaginalWetness wetness = minWetness;
		//	minWetness = minWetness.ByteEnumAdd(amount);
		//	if (minWetness > maxWetness)
		//	{
		//		if (forceIncreaseMax)
		//		{
		//			maxWetness = minWetness;
		//		}
		//		else
		//		{
		//			minWetness = maxWetness;
		//		}
		//	}
		//	return minWetness - wetness;
		//}
		//internal byte DecreaseMinimumWetness(byte amount = 1)
		//{
		//	VaginalWetness wetness = minWetness;
		//	minWetness = minWetness.ByteEnumSubtract(amount);
		//	return wetness - minWetness;
		//}
		//internal void SetMinWetness(VaginalWetness newValue)
		//{
		//	minWetness = newValue;
		//}
		//internal byte IncreaseMaximumWetness(byte amount = 1)
		//{
		//	VaginalWetness wetness = maxWetness;
		//	maxWetness = maxWetness.ByteEnumSubtract(amount);
		//	return maxWetness - wetness;
		//}
		//internal byte DecreaseMaximumWetness(byte amount = 1, bool forceDecreaseMin = false)
		//{
		//	VaginalWetness wetness = minWetness;
		//	maxWetness = maxWetness.ByteEnumSubtract(amount);
		//	if (minWetness > maxWetness)
		//	{
		//		if (forceDecreaseMin)
		//		{
		//			minWetness = maxWetness;
		//		}
		//		else
		//		{
		//			maxWetness = minWetness;
		//		}
		//	}
		//	return wetness - maxWetness;
		//}
		//internal void SetMaxWetness(VaginalWetness newValue)
		//{
		//	maxWetness = newValue;
		//}
		#endregion
	}

	public sealed partial class VaginaType : SaveableBehavior<VaginaType, Vagina, VaginaData>
	{
		//in this game, you can have two vaginas, so it technically needs to be plural. BUT, it makes no sense to have long/full description here do that
		//because they will generally have different stats (and even if they are the same that's a bit misleading). So we dont. regardless, genitals handles the multiple
		//vaginas text; so long as we provide a plural short description (for matching plural types - that actually makes sense), we don't really need to worry here.

		private static int indexMaker = 0;
		private static readonly List<VaginaType> types = new List<VaginaType>();
		public static readonly ReadOnlyCollection<VaginaType> availableTypes = new ReadOnlyCollection<VaginaType>(types);
		public readonly int typeCapacityBonus;

		public static VaginaType defaultValue => HUMAN;

		private readonly ShortPluralDescriptor shortPluralDesc;
		private readonly PartDescriptor<VaginaData> fullStr;

		public readonly bool orgasmOnTransform;

		//only should be used when actually dealing with 2. does not check if the creature has 2. Not aliased in the data or source classes because that doesn't make sense.
		public string ShortDescription(bool plural) => shortPluralDesc(plural);


		public string FullDescriptionPrimary(VaginaData data)
		{
			return FullDescription(data, false);
		}

		public string FullDescriptionAlternate(VaginaData data)
		{
			return FullDescription(data, true);
		}

		public string FullDescription(VaginaData vagina, bool alternateFormat) => fullStr(vagina, alternateFormat);

		private delegate string GrowVaginaDescriptor(PlayerBase player, byte grownVaginaIndex);
		private delegate string RemoveVaginaDescriptor(VaginaData removedVagina, PlayerBase player);

		private readonly GrowVaginaDescriptor grewVaginaStr;
		private readonly RemoveVaginaDescriptor removedVaginaStr;

		//aliased in the genitals class.
		internal string GrewVaginaText(PlayerBase player, byte grownVaginaIndex) => grewVaginaStr(player, grownVaginaIndex);


		private VaginaType(int capacityBonus, bool orgasmWhenTransforming,
			ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc, PartDescriptor<VaginaData> longDesc, PartDescriptor<VaginaData> fullDesc,
			PlayerBodyPartDelegate<Vagina> playerDesc, ChangeType<VaginaData> transform, GrowVaginaDescriptor growVaginaText, RestoreType<VaginaData> restore,
			RemoveVaginaDescriptor removeVaginaText) : base(PluralHelper(shortDesc, false), singleDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;

			shortPluralDesc = shortDesc;
			fullStr = fullDesc ?? throw new ArgumentNullException(nameof(fullDesc));

			types.AddAt(this, _index);

			grewVaginaStr = growVaginaText ?? throw new ArgumentNullException(nameof(growVaginaText));
			removedVaginaStr = removeVaginaText ?? throw new ArgumentNullException(nameof(removeVaginaText));
		}


		internal static bool Validate(ref VaginaType vaginaType, bool correctInvalidData)
		{
			if (types.Contains(vaginaType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				vaginaType = HUMAN;
			}
			return false;
		}

		public override int index => _index;
		private readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, false, HumanShortDesc, HumanSingleDesc, HumanLongDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanGrewVaginaStr, HumanRestoreStr, HumanRemovedVaginaStr);
		//defined, but never originally used in code (afaik, could be somewhere but i missed it because spaghetti). will be used in new code. feel free to update/fix any of the strings.
		public static readonly VaginaType EQUINE = new VaginaType(0, true, EquineDesc, EquineSingleDesc, EquineLongDesc, EquineFullDesc, EquinePlayerStr, EquineTransformStr, EquineGrewVaginaStr, EquineRestoreStr, EquineRemovedVaginaStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, false, SandTrapDesc, SandTrapSingleDesc, SandTrapLongDesc, SandTrapFullDesc, SandTrapPlayerStr, SandTrapTransformStr, SandTrapGrewVaginaStr, SandTrapRestoreStr, SandTrapRemovedVaginaStr);

	}

	public sealed partial class VaginaData : BehavioralSaveablePartData<VaginaData, Vagina, VaginaType>
	{

		public readonly ClitData clit;
		public readonly VaginalLooseness looseness;
		public readonly VaginalWetness wetness;
		public readonly bool isVirgin;
		public readonly bool everPracticedVaginal;
		public readonly int vaginaIndex;

		public readonly ushort capacity;

		public readonly ReadOnlyPiercing<LabiaPiercingLocation> labiaPiercings;

		public override VaginaData AsCurrentData()
		{
			return this;
		}

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string AdjectiveText(bool multipleAdjectives)
		{
			return VaginaType.VaginaAdjectiveText(this, multipleAdjectives);
		}

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(this);

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(this);

		public string FullDescription(bool alternateFormat) => type.FullDescription(this, alternateFormat);

		public VaginaData(Vagina source, int currIndex) : base(GetID(source), GetBehavior(source))
		{
			clit = source.clit.AsReadOnlyData();
			looseness = source.looseness;
			wetness = source.wetness;
			isVirgin = source.isVirgin;
			everPracticedVaginal = source.everPracticedVaginal;

			capacity = source.VaginalCapacity();
			vaginaIndex = currIndex;

			labiaPiercings = source.labiaPiercings.AsReadOnlyData();
		}

		public VaginaData(Guid creatureID, VaginaType vaginaType, ClitData clit, VaginalLooseness looseness, VaginalWetness wetness, bool isVirgin, bool everPracticedVaginal,
			int vaginaIndex, ushort capacity, ReadOnlyPiercing<LabiaPiercingLocation> labiaPiercings) : base(creatureID, vaginaType)
		{
			this.clit = clit ?? throw new ArgumentNullException(nameof(clit));
			this.looseness = looseness;
			this.wetness = wetness;
			this.isVirgin = isVirgin;
			this.everPracticedVaginal = everPracticedVaginal;
			this.vaginaIndex = vaginaIndex;
			this.capacity = capacity;
			this.labiaPiercings = labiaPiercings ?? throw new ArgumentNullException(nameof(labiaPiercings));
		}
	}
}
