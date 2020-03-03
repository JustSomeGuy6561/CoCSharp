//Vagina.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:57 PM
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;

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
			else
			{
				return false;
			}
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
		public static readonly LabiaPiercingLocation RIGHT_5 = new LabiaPiercingLocation(10, SupportedJewelry, Right5Button, Right5Location);
		public static readonly LabiaPiercingLocation RIGHT_6 = new LabiaPiercingLocation(11, SupportedJewelry, Right6Button, Right6Location);


		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}


	}

	public sealed class LabiaPiercing : Piercing<LabiaPiercingLocation>
	{
		public LabiaPiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, CreatureStr playerShortDesc, CreatureStr playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => LabiaPiercingLocation.allLocations.Count;

		public override IEnumerable<LabiaPiercingLocation> availableLocations => LabiaPiercingLocation.allLocations;
	}

	//tattoos here are part of genitals class.

	//Note: this class is created after perks have been initialized, so its post perk init is never called.

	//unlike ass wetness changes, this seems fine by me. the normal case is some wetness, though dryer than that is not terribly strange.
	public enum VaginalWetness : byte { DRY, NORMAL, WET, SLICK, DROOLING, SLAVERING }

	//i have, however, renamed these. gaping-wide-> gaping. gaping-> roomy. could even rename clown car to gaping-wide if clown car is a little too bizarre, but i'm kinda fond of its bizarre-ness.
	public enum VaginalLooseness : byte { TIGHT, NORMAL, LOOSE, ROOMY, GAPING, CLOWN_CAR_WIDE }

	public sealed partial class Vagina : FullBehavioralPart<Vagina, VaginaType, VaginaData>, IBodyPartTimeLazy
	{
		public override string BodyPartName()
		{
			return Name();
		}


		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_VAGINAL_CAPACITY = ushort.MaxValue;

		//used for natural tightening of vagina over time. value is in in-game hours.
		private const ushort LOOSENESS_LOOSE_TIMER = 200;
		private const ushort LOOSENESS_ROOMY_TIMER = 100;
		private const ushort LOOSENESS_GAPING_TIMER = 70;
		private const ushort LOOSENESS_CLOWN_CAR_TIMER = 50;

		public readonly Clit clit;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		private readonly VaginaCollection source;
		public readonly uint collectionID;

		public int vaginaIndex => source.vaginas.IndexOf(this);

		//private GenitalPerkData perkData => creature?.genitals.perkData;

		public VaginalLooseness minLooseness => source.minVaginalLooseness;
		public VaginalLooseness maxLooseness => source.maxVaginalLooseness;

		public VaginalWetness minWetness => source.minVaginalWetness;
		public VaginalWetness maxWetness => source.maxVaginalWetness;

		private VaginalWetness defaultNewVaginaWetness => source.defaultNewVaginaWetness ?? VaginalWetness.NORMAL;
		private VaginalLooseness? defaultNewVaginaLooseness => source.defaultNewVaginaLooseness;

		private VaginalLooseness DefaultNewVaginaLooseness(bool virgin)
		{
			return defaultNewVaginaLooseness ?? (virgin ? VaginalLooseness.TIGHT : VaginalLooseness.NORMAL);
		}


		//called when min or max wetness changes in the perk data.
		internal void OnWetnessPerkValueChange()
		{
			//try to set the wetness to itself. note that this forces the setter to auto-correct if the data is now invalid. do not remove this.
			wetness = wetness;
		}

		internal void OnLoosenessPerkValueChange()
		{
			//try to set the looseness to itself. note that this forces the setter to auto-correct if the data is now invalid. do not remove this.
			looseness = looseness;
		}

		public VaginalWetness wetness
		{
			get => _wetness;
			private set
			{
				Utils.ClampEnum(ref value, minWetness, maxWetness);
				if (_wetness != value)
				{
					VaginaData oldData = AsReadOnlyData();
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
					VaginaData oldData = AsReadOnlyData();
					_looseness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private VaginalLooseness _looseness;

		//Sexual metadata.
		//Note that all sexual metadata requires it be significant - for example, fingering oneself as a combat tease does not count here. Generally speaking,
		//a good measure of thumb is whether or not it results in an orgasm, though some instances that do not (i.e. orgasm denial) may still qualify.
		//We use this stuff to adjust scenes to the sexual preferences or quirks a given creature may have, and to draw conclusions based on sexual habits.
		//Insert joke about Data Collection, Privacy Policy, and or Terms of Use here.

		//The Vagina Sexual MetaData is as follows:

		//sex count: times sexually penetrated in such a way that it would take this vagina's virginity (if applicable)
		//penetration count: times sexually penetrated by anything. this includes the sex count, as well as any penetrations that don't rob virginity (like dildoes, i guess)
		//non penetration count: times sexually stimulated without actual penetration.

		//all of the above data also has a subset that only counts any self-stimulation, so we can handle that, i guess.

		//orgasm count: times orgasmed. this includes any sex, penetration, or non-penetration that results in an orgasm, any dry orgasms, and any other situation
		//	that causes this vagina to reach an orgasm that is not covered by this metadata (like vagina type change, vagina piercings, giving birth)
		//dry orgasm count: times orgasmed without any stimulation to this vagina. this can be caused by sex with other organs (i.e. cocks, ass, tits), among others.

		public uint totalSexCount { get; private set; } = 0;
		public uint selfSexCount { get; private set; } = 0;

		public uint totalPenetrationCount { get; private set; } = 0;
		public uint selfPenetrationCount { get; private set; } = 0;

		public uint totalNonPenetrationCount { get; private set; } = 0;
		public uint selfNonPenetrationCount { get; private set; } = 0;

		public uint totalStimulationCount => totalPenetrationCount.add(totalNonPenetrationCount);
		public uint totalSelfStimulationCount => selfPenetrationCount.add(selfNonPenetrationCount);

		public uint totalOrgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint totalBirths { get; private set; } = 0;

		public bool isVirgin => totalSexCount == 0 && totalBirths == 0;
		public bool isChaste => totalNonPenetrationCount == 0 && totalPenetrationCount == 0 && totalBirths == 0;

		public bool everPracticedVaginal => totalPenetrationCount > 0;

		private ushort bonusCapacity => creature?.genitals.allVaginas.totalBonusCapacity ?? 0;



		public ushort VaginalCapacity()
		{

			byte loose = (byte)looseness;
			if (!isVirgin)
			{
				loose++;
			}
			byte wet = ((byte)wetness).add(1);
			uint cap = (uint)Math.Floor(BASE_CAPACITY + bonusCapacity + type.typeCapacityBonus /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_VAGINAL_CAPACITY)
			{
				return MAX_VAGINAL_CAPACITY;
			}
			return (ushort)cap;
		}


		private ushort vaginaTightenTimer = 0;
		public readonly LabiaPiercing labiaPiercings;
		public override VaginaType type { get; protected set; }
		public override VaginaType defaultType => VaginaType.defaultValue;

		#region Constructors
		internal Vagina(VaginaCollection parent, uint id) : this(parent, id, VaginaType.defaultValue)
		{ }

		internal Vagina(VaginaCollection parent, uint id, VaginaType vaginaType) : base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;
			collectionID = id;

			clit = new Clit(creatureID, this);

			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));
			_wetness = defaultNewVaginaWetness;
			_looseness = DefaultNewVaginaLooseness(true);

			//default to virgin.
			totalSexCount = 0;

			labiaPiercings = new LabiaPiercing(this, PiercingLocationUnlocked, AllLabiaPiercingsShort, AllLabiaPiercingsLong);
		}

		internal Vagina(VaginaCollection parent, uint id, VaginaType vaginaType, double clitLength, VaginalLooseness? vaginalLooseness = null,
			VaginalWetness? vaginalWetness = null, bool? isVirgin = null, IEnumerable<KeyValuePair<LabiaPiercingLocation, PiercingJewelry>> initialLabiaPiercings = null,
			IEnumerable<KeyValuePair<ClitPiercingLocation, PiercingJewelry>> initialClitPiercings = null)
			: base(parent?.creatureID ?? throw new ArgumentNullException(nameof(parent)))
		{
			source = parent;
			collectionID = id;


			type = vaginaType ?? throw new ArgumentNullException(nameof(vaginaType));

			clit = new Clit(creatureID, this, clitLength);
			if (isVirgin is null)
			{
				isVirgin = vaginalLooseness == VaginalLooseness.TIGHT || vaginalLooseness is null;

			}

			if ((bool)isVirgin)
			{
				totalSexCount = 0;
			}
			else
			{
				totalSexCount = 1;
			}

			totalPenetrationCount = totalSexCount;


			//these are clamped by above set values.
			_wetness = vaginalWetness ?? defaultNewVaginaWetness;
			_looseness = vaginalLooseness ?? DefaultNewVaginaLooseness(totalSexCount == 0);

			labiaPiercings = new LabiaPiercing(this, PiercingLocationUnlocked, AllLabiaPiercingsShort, AllLabiaPiercingsLong);

			labiaPiercings.InitializePiercings(initialLabiaPiercings);
			clit.piercings.InitializePiercings(initialClitPiercings);
		}

		#endregion

		#region Generate
		internal static Vagina GenerateFromGender(VaginaCollection parent, uint id, Gender gender)
		{
			if (gender.HasFlag(Gender.FEMALE))
			{
				return new Vagina(parent, id);
			}
			else
			{
				return null;
			}
		}

		public static VaginaData GenerateAggregate(Guid creatureID, VaginaType vaginaType, ClitData averageClit, VaginalLooseness looseness, VaginalWetness wetness,
			ushort averageCapacity, bool isVirgin, bool everPracticedVaginal, bool chaste)
		{
			return new VaginaData(creatureID, vaginaType, -1, averageClit, looseness, wetness, averageCapacity, isVirgin, everPracticedVaginal, chaste, new ReadOnlyPiercing<LabiaPiercingLocation>());
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

			VaginaType oldValue = type;
			if (newType.orgasmOnTransform)
			{
				if (!(creature is null))
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
		#region Sexual Functions

		internal bool PenetrateVagina(ushort penetratorArea, double knotArea, bool takeVirginity, bool reachOrgasm, bool sourceIsSelf)
		{
			totalPenetrationCount++;

			if (sourceIsSelf)
			{
				selfPenetrationCount++;
			}


			//experience = experience.add(ExperiencedGained);
			VaginalLooseness oldLooseness = looseness;

			HandleStretching(penetratorArea, knotArea);

			if (takeVirginity)
			{
				totalSexCount++;

				if (sourceIsSelf)
				{
					selfSexCount++;
				}
			}
			if (reachOrgasm)
			{
				totalOrgasmCount++;
			}

			return oldLooseness != looseness;
		}

		internal void StimulateVagina(bool reachOrgasm, bool sourceIsSelf)
		{
			totalNonPenetrationCount++;

			if (sourceIsSelf)
			{
				selfNonPenetrationCount++;
			}

			//experience = experience.add(ExperiencedGained);

			if (reachOrgasm)
			{
				totalOrgasmCount++;
			}
		}

		internal void OrgasmGeneric(bool dryOrgasm)
		{
			totalOrgasmCount++;
			if (dryOrgasm)
			{
				dryOrgasmCount++;
			}
		}

		internal void HandleBirth(ushort size)
		{
			HandleStretching(size, 0);
		}

		private void HandleStretching(ushort penetratorArea, double knotArea)
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

		public byte IncreaseLooseness(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumAdd(amount);
			return looseness - oldLooseness;
		}

		public byte DecreaseLooseness(byte amount = 1)
		{

			VaginalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumSubtract(amount);
			return oldLooseness - looseness;
		}

		public bool SetVaginalLooseness(VaginalLooseness Looseness)
		{
			if (Looseness >= minLooseness && Looseness <= maxLooseness)
			{
				looseness = Looseness;
				return true;
			}
			return false;
		}

		public byte IncreaseWetness(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		public byte DecreaseWetness(byte amount = 1)
		{
			VaginalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		public bool SetVaginalWetness(VaginalWetness Wetness)
		{
			if (Wetness >= minWetness && Wetness <= maxWetness)
			{
				wetness = Wetness;
				return true;
			}
			return false;
		}

		#endregion
		#region Clit Helpers

		public double GrowClit(double amount, bool ignorePerks = false)
		{
			VaginaData oldData = AsReadOnlyData();
			double retVal = clit.GrowClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public double ShrinkClit(double amount, bool ignorePerks = false)
		{
			VaginaData oldData = AsReadOnlyData();
			double retVal = clit.ShrinkClit(amount, ignorePerks);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public double SetClitSize(double newSize)
		{
			VaginaData oldData = AsReadOnlyData();
			double oldSize = clit.length;
			double retVal = clit.SetClitSize(newSize);
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
			VaginaType oldType = type;
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
		public override bool IsIdenticalTo(VaginaData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && vaginaIndex == original.vaginaIndex && looseness == original.looseness && wetness == original.wetness
				&& clit.IsIdenticalTo(original.clit) && original.isVirgin == isVirgin && original.everPracticedVaginal == everPracticedVaginal &&
				labiaPiercings.IsIdenticalTo(original.labiaPiercings) &&
				(ignoreSexualMetaData || (totalSexCount == original.totalSexCount && totalPenetrationCount == original.totalPenetrationCount &&
				totalNonPenetrationCount == original.totalNonPenetrationCount && selfSexCount == original.selfSexCount &&
				selfPenetrationCount == original.selfPenetrationCount && selfNonPenetrationCount == original.selfNonPenetrationCount &&
				totalOrgasmCount == original.totalOrgasmCount && dryOrgasmCount == original.dryOrgasmCount && totalBirths == original.totalBirths));
		}

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

		internal void InitializePiercings(Dictionary<ClitPiercingLocation, PiercingJewelry> clitPiercings, Dictionary<LabiaPiercingLocation, PiercingJewelry> labiaPiercings)
		{
			clit.piercings.InitializePiercings(clitPiercings);
			this.labiaPiercings.InitializePiercings(labiaPiercings);
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
				VaginalLooseness oldLooseness = looseness;
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

	public sealed partial class VaginaType : FullBehavior<VaginaType, Vagina, VaginaData>
	{
		//in this game, you can have two vaginas, so it technically needs to be plural. BUT, it makes no sense to have long/full description here do that
		//because they will generally have different stats (and even if they are the same that's a bit misleading). So we dont. regardless, genitals handles the multiple
		//vaginas text; so long as we provide a plural short description (for matching plural types - that actually makes sense), we don't really need to worry here.

		private static int indexMaker = 0;
		private static readonly List<VaginaType> types = new List<VaginaType>();
		public static readonly ReadOnlyCollection<VaginaType> availableTypes = new ReadOnlyCollection<VaginaType>(types);
		public readonly short typeCapacityBonus;

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


		private VaginaType(short capacityBonus, bool orgasmWhenTransforming,
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

		public override int id => _index;
		private readonly int _index;

		public static readonly VaginaType HUMAN = new VaginaType(0, false, HumanShortDesc, HumanSingleDesc, HumanLongDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanGrewVaginaStr, HumanRestoreStr, HumanRemovedVaginaStr);
		//defined, but never originally used in code (afaik, could be somewhere but i missed it because spaghetti). will be used in new code. feel free to update/fix any of the strings.
		public static readonly VaginaType EQUINE = new VaginaType(0, true, EquineDesc, EquineSingleDesc, EquineLongDesc, EquineFullDesc, EquinePlayerStr, EquineTransformStr, EquineGrewVaginaStr, EquineRestoreStr, EquineRemovedVaginaStr);
		public static readonly VaginaType SAND_TRAP = new VaginaType(0, false, SandTrapDesc, SandTrapSingleDesc, SandTrapLongDesc, SandTrapFullDesc, SandTrapPlayerStr, SandTrapTransformStr, SandTrapGrewVaginaStr, SandTrapRestoreStr, SandTrapRemovedVaginaStr);

	}

	public sealed partial class VaginaData : FullBehavioralData<VaginaData, Vagina, VaginaType>, IVagina
	{
		#region Standard Data
		public readonly ClitData clit;
		public readonly VaginalLooseness looseness;
		public readonly VaginalWetness wetness;
		//public readonly bool isVirgin;
		//public readonly bool everPracticedVaginal;
		public readonly int vaginaIndex;


		public readonly ushort capacity;

		public readonly ReadOnlyPiercing<LabiaPiercingLocation> labiaPiercings;
		internal readonly uint? collectionID;

		#endregion

		#region Sex MetaData
		public readonly uint totalSexCount;
		public readonly uint totalPenetrationCount;
		public readonly uint totalNonPenetrationCount;

		public uint totalStimulationCount => totalPenetrationCount.add(totalNonPenetrationCount);

		public readonly uint selfSexCount;
		public readonly uint selfPenetrationCount;
		public readonly uint selfNonPenetrationCount;

		public uint totalSelfStimulationCount => selfPenetrationCount.add(selfNonPenetrationCount);


		public readonly uint totalOrgasmCount;
		public readonly uint dryOrgasmCount;

		public readonly uint totalBirths;

		public bool isVirgin => totalSexCount == 0 && totalBirths == 0;
		public bool isChaste => totalNonPenetrationCount == 0 && totalPenetrationCount == 0 && totalBirths == 0;

		public bool everPracticedVaginal => totalPenetrationCount > 0;
		#endregion

		public override VaginaType defaultType => VaginaType.defaultValue;

		public override VaginaData AsCurrentData()
		{
			return this;
		}

		#region Text

		IClit IVagina.clit => clit;

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string AdjectiveText(bool multipleAdjectives)
		{
			return VaginaType.VaginaAdjectiveText(this, multipleAdjectives);
		}

		public string FullDescriptionPrimary() => type.FullDescriptionPrimary(this);

		public string FullDescriptionAlternate() => type.FullDescriptionAlternate(this);

		public string FullDescription(bool alternateFormat) => type.FullDescription(this, alternateFormat);
		#endregion

		public VaginaData(Vagina source, int currIndex) : base(GetID(source), GetBehavior(source))
		{
			clit = source.clit.AsReadOnlyData();
			looseness = source.looseness;
			wetness = source.wetness;

			capacity = source.VaginalCapacity();
			vaginaIndex = currIndex;

			labiaPiercings = source.labiaPiercings.AsReadOnlyData();

			collectionID = source.collectionID;

			totalSexCount = source.totalSexCount;
			totalPenetrationCount = source.totalPenetrationCount;
			totalNonPenetrationCount = source.totalNonPenetrationCount;

			selfSexCount = source.selfSexCount;
			selfPenetrationCount = source.selfPenetrationCount;
			selfNonPenetrationCount = source.selfNonPenetrationCount;

			totalBirths = source.totalBirths;
		}

		public VaginaData(Guid creatureID, VaginaType vaginaType, int vaginaIndex, ClitData clit, VaginalLooseness looseness, VaginalWetness wetness,
			ushort capacity, bool isVirgin, bool everPracticedVaginal, bool chaste, ReadOnlyPiercing<LabiaPiercingLocation> labiaPiercings) : base(creatureID, vaginaType)
		{
			this.clit = clit ?? throw new ArgumentNullException(nameof(clit));
			this.looseness = looseness;
			this.wetness = wetness;
			this.vaginaIndex = vaginaIndex;
			this.capacity = capacity;
			this.labiaPiercings = labiaPiercings ?? throw new ArgumentNullException(nameof(labiaPiercings));

			collectionID = null;

			totalSexCount = (uint)(!isVirgin ? 1 : 0);
			totalPenetrationCount = (uint)(!isVirgin || everPracticedVaginal ? 1 : 0);
			totalNonPenetrationCount = (uint)(chaste ? 0 : 1);

			selfSexCount = 0;
			selfPenetrationCount = 0;
			selfNonPenetrationCount = 0;

			dryOrgasmCount = 0;
			totalBirths = 0;
		}


		public VaginaData(Guid creatureID, VaginaType vaginaType, int vaginaIndex, ClitData clit, VaginalLooseness looseness, VaginalWetness wetness, ushort capacity,
			ReadOnlyPiercing<LabiaPiercingLocation> labiaPiercings, uint totalSexCount, uint totalPenetrationCount, uint totalNonPenetrationCount, uint selfSexCount,
			uint selfPenetrationCount, uint selfNonPenetrationCount, uint totalOrgasmCount, uint dryOrgasmCount, uint totalBirths) : base(creatureID, vaginaType)
		{
			this.clit = clit ?? throw new ArgumentNullException(nameof(clit));
			this.looseness = looseness;
			this.wetness = wetness;
			this.vaginaIndex = vaginaIndex;
			this.capacity = capacity;
			this.labiaPiercings = labiaPiercings ?? new ReadOnlyPiercing<LabiaPiercingLocation>();
			this.totalSexCount = totalSexCount;

			this.totalPenetrationCount = totalPenetrationCount;
			this.totalNonPenetrationCount = totalNonPenetrationCount;
			this.selfSexCount = selfSexCount;
			this.selfPenetrationCount = selfPenetrationCount;
			this.selfNonPenetrationCount = selfNonPenetrationCount;
			this.totalOrgasmCount = totalOrgasmCount;
			this.dryOrgasmCount = dryOrgasmCount;

			this.totalBirths = totalBirths;

			collectionID = null;
		}

		#region IVagina
		VaginaType IVagina.type => type;
		#endregion

	}
}
