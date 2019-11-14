//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	public enum LabiaPiercings
	{
		LEFT_1, LEFT_2, LEFT_3, LEFT_4, LEFT_5, LEFT_6,
		RIGHT_1, RIGHT_2, RIGHT_3, RIGHT_4, RIGHT_5, RIGHT_6
	}

	//Note: this class is created after perks have been initialized, so its post perk init is never called.

	//unlike ass wetness changes, this seems fine by me. the normal case is some wetness, though dryer than that is not terribly strange.
	public enum VaginalWetness : byte { DRY, NORMAL, WET, SLICK, DROOLING, SLAVERING }

	//i have, however, renamed these. gaping-wide-> gaping. gaping-> roomy. could even rename clown car to gaping-wide if clown car is a little too bizarre, but i'm kinda fond of its bizarre-ness.
	public enum VaginalLooseness : byte { TIGHT, NORMAL, LOOSE, ROOMY, GAPING, CLOWN_CAR_WIDE }

	public sealed partial class Vagina : BehavioralSaveablePart<Vagina, VaginaType, VaginaWrapper>, IBodyPartTimeLazy
	{
		public override string BodyPartName()
		{
			return Name();
		}


		private const JewelryType SUPPORTED_LABIA_JEWELRY = JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;

		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_VAGINAL_CAPACITY = ushort.MaxValue;

		public readonly Clit clit;

		private int vaginaIndex => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.genitals.vaginas.IndexOf(this) : 0;

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
					var oldData = AsData();
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
					var oldData = AsData();
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

		public bool virgin { get; private set; } = true;

		public bool everPracticedVaginal => totalPenetrationCount > 0;

		public ushort bonusVaginalCapacity
		{
			get => _bonusVaginalCapacity;
			private set
			{
				if (_bonusVaginalCapacity != value)
				{
					var oldData = AsData();
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
					var oldData = AsData();
					_perkBonusVaginalCapacity = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private ushort _perkBonusVaginalCapacity = 0;

		public ushort VaginalCapacity()
		{

			byte loose = (byte)looseness;
			if (!virgin)
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
		public readonly Piercing<LabiaPiercings> labiaPiercings;
		public override VaginaType type { get; protected set; }
		public override VaginaType defaultType => VaginaType.defaultValue;

		#region Constructors
		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkWrapper) : this(creatureID, initialPerkWrapper, VaginaType.defaultValue, clitLength: initialPerkWrapper.DefaultNewClitSize)
		{ }

		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkWrapper, VaginaType vaginaType) : base(creatureID)
		{
			clit = new Clit(creatureID, this, initialPerkWrapper);
			virgin = true;
			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));
			_wetness = initialPerkWrapper.defaultWetnessNew;
			_looseness = initialPerkWrapper.defaultLoosenessNew;

			labiaPiercings = new Piercing<LabiaPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);
		}

		internal Vagina(Guid creatureID, VaginaPerkHelper initialPerkWrapper, VaginaType vaginaType, float clitLength,
			VaginalLooseness? vaginalLooseness = null, VaginalWetness? vaginalWetness = null, bool? isVirgin = null, bool omnibus = false) : base(creatureID)
		{
			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));

			clit = new Clit(creatureID, this, initialPerkWrapper, clitLength, omnibus);
			if (isVirgin is null)
			{
				isVirgin = vaginalLooseness == VaginalLooseness.TIGHT || vaginalLooseness is null;
			}
			virgin = (bool)isVirgin;

			minLooseness = initialPerkWrapper.minLooseness;
			maxLooseness = initialPerkWrapper.maxLooseness;
			minWetness = initialPerkWrapper.minWetness;
			maxWetness = initialPerkWrapper.maxWetness;

			perkBonusVaginalCapacity = initialPerkWrapper.perkBonusCapacity;

			//these are clamped by above set values.
			_wetness = vaginalWetness ?? initialPerkWrapper.defaultWetnessNew;
			_looseness = vaginalLooseness ?? initialPerkWrapper.defaultLoosenessNew;

			labiaPiercings = new Piercing<LabiaPiercings>(PiercingLocationUnlocked, SupportedJewelryByLocation);
		}
		#endregion

		#region Generate
		internal static Vagina GenerateFromGender(Guid creatureID, VaginaPerkHelper initialPerkWrapper, Gender gender)
		{
			if (gender.HasFlag(Gender.FEMALE)) return new Vagina(creatureID, initialPerkWrapper);
			else return null;
		}
		#endregion
		public override VaginaWrapper AsReadOnlyReference()
		{
			return new VaginaWrapper(this, vaginaIndex);
		}

		#region Update

		//default update is fine.
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
				virgin = false;
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
			if (!virgin)
			{
				return false;
			}
			virgin = false;
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
				var oldData = AsData();
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
				var oldData = AsData();
				var retVal = clit.DeactivateOmnibusClit();
				NotifyDataChanged(oldData);
				return retVal;
			}
			return false;
		}

		public float growClit(float amount, bool ignorePerks = false)
		{
			var oldData = AsData();
			var retVal = clit.growClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float shrinkClit(float amount, bool ignorePerks = false)
		{
			var oldData = AsData();
			var retVal = clit.shrinkClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float SetClitSize(float newSize)
		{
			var oldData = AsData();
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

		public VaginaData AsData()
		{
			return new VaginaData(this, vaginaIndex);
		}

		private readonly WeakEventSource<SimpleDataChangedEvent<VaginaWrapper, VaginaData>> dataChangeSource =
			new WeakEventSource<SimpleDataChangedEvent<VaginaWrapper, VaginaData>>();

		public event EventHandler<SimpleDataChangedEvent<VaginaWrapper, VaginaData>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private void NotifyDataChanged(VaginaData oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangedEvent<VaginaWrapper, VaginaData>(AsReadOnlyReference(), oldData));
		}

		#region Piercing-Related
		private bool PiercingLocationUnlocked(LabiaPiercings piercingLocation)
		{
			return true;
		}

		private JewelryType SupportedJewelryByLocation(LabiaPiercings piercingLocation)
		{
			return SUPPORTED_LABIA_JEWELRY;
		}

		public bool isPierced => clit.isPierced || labiaPiercings.isPierced;
		public bool isClitPierced => clit.isPierced;
		public bool isLabiaPierced => labiaPiercings.isPierced;

		public bool wearingAnyJewelry => clit.wearingJewelry || labiaPiercings.wearingJewelry;
		public bool clitWearingJewelry => clit.wearingJewelry;
		public bool labiaWearingJewelry => labiaPiercings.wearingJewelry;

		internal void InitializePiercings(Dictionary<ClitPiercings, PiercingJewelry> clitPiercings, Dictionary<LabiaPiercings, PiercingJewelry> labiaPiercings)
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
				if (vaginaTightenTimer >= timerAmount)
				{
					if (isPlayer)
					{
						sb.Append(VaginaTightenedUpDueToInactivity(looseness));
					}
					looseness--;
					vaginaTightenTimer = 0;
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

	public sealed partial class VaginaType : SaveableBehavior<VaginaType, Vagina, VaginaWrapper>
	{
		private static int indexMaker = 0;
		private static readonly List<VaginaType> types = new List<VaginaType>();
		public static readonly ReadOnlyCollection<VaginaType> availableTypes = new ReadOnlyCollection<VaginaType>(types);
		public readonly int typeCapacityBonus;

		public static VaginaType defaultValue => HUMAN;


		private VaginaType(int capacityBonus,
			SimpleDescriptor shortDesc, DescriptorWithArg<Vagina> fullDesc, TypeAndPlayerDelegate<Vagina> playerDesc,
			ChangeType<Vagina> transform, RestoreType<Vagina> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			typeCapacityBonus = capacityBonus;
			types.AddAt(this, _index);
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

		public static readonly VaginaType HUMAN = new VaginaType(0, VagHumanDesc, VagHumanLongDesc, VagHumanPlayerStr, (x, y) => x.type.restoreString(x, y), GlobalStrings.RevertAsDefault);
		public static readonly VaginaType EQUINE = new VaginaType(0, VagEquineDesc, VagEquineLongDesc, VagEquinePlayerStr, VagEquineTransformStr, VagEquineRestoreStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, VagSandTrapDesc, VagSandTrapLongDesc, VagSandTrapPlayerStr, VagSandTrapTransformStr, VagSandTrapRestoreStr);

	}

	public sealed partial class VaginaWrapper : BehavioralSaveablePartWrapper<VaginaWrapper, Vagina, VaginaType>
	{

		public ClitWrapper clit => sourceData.clit.AsReadOnlyReference();

		 public bool virgin => sourceData.virgin;

		 public VaginalLooseness minLooseness => sourceData.minLooseness;
		 public VaginalLooseness maxLooseness => sourceData.maxLooseness;
		 public VaginalWetness minWetness => sourceData.minWetness;
		 public VaginalWetness maxWetness => sourceData.maxWetness;

		 public ushort bonusVaginalCapacity => sourceData.bonusVaginalCapacity;
		 public ushort perkBonusVaginalCapacity => sourceData.perkBonusVaginalCapacity;

		 public VaginalWetness wetness => sourceData.wetness;
		 public VaginalLooseness looseness => sourceData.looseness;

		 public ushort orgasmCount => sourceData.orgasmCount;
		 public ushort sexCount => sourceData.sexCount;
		 public ushort totalPenetrationCount => sourceData.totalPenetrationCount;
		 public ushort dryOrgasmCount => sourceData.dryOrgasmCount;

		 //private ushort vaginaTightenTimer => sourceData.vaginaTightenTimer;

		public bool everPracticedVaginal => totalPenetrationCount > 0;

		public ushort Capacity() => sourceData.VaginalCapacity();

		public string FullDescription() => sourceData.FullDescription();

		public ReadOnlyPiercing<LabiaPiercings> labiaPiercings => sourceData.labiaPiercings.AsReadOnlyCopy();

		public ReadOnlyPiercing<ClitPiercings> clitPiercings => sourceData.clit.clitPiercings.AsReadOnlyCopy();

		public bool isPierced => clit.isPierced || labiaPiercings.isPierced;
		public bool isClitPierced => clit.isPierced;
		public bool isLabiaPierced => labiaPiercings.isPierced;

		public bool wearingAnyJewelry => clit.wearingJewelry || labiaPiercings.wearingJewelry;
		public bool clitWearingJewelry => clit.wearingJewelry;
		public bool labiaWearingJewelry => labiaPiercings.wearingJewelry;

		public VaginaWrapper(Vagina source, int currIndex) : base(source)
		{

		}
	}

	public sealed partial class VaginaData
	{

		public readonly ClitData clit;
		public readonly VaginalLooseness looseness;
		public readonly VaginalWetness wetness;
		public readonly bool isVirgin;
		public readonly int vaginaIndex;

		public readonly ushort capacity;

		public VaginaData(Vagina source, int currIndex)
		{
			clit = source.clit.AsData();
			looseness = source.looseness;
			wetness = source.wetness;
			isVirgin = source.virgin;

			capacity = source.VaginalCapacity();
			vaginaIndex = currIndex;
		}
	}
}
