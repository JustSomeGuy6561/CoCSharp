//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Perks;
using CoC.Backend.Tools;
using System;
using System.Text;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//as with looseness, anal wetness defaults to dry, so that should be normal.
	//Revised ruling: Normal - no wetness. a rather uncomfortable experience unless you're used to it. DAMP - slightly wet, should be considered barely self-lubricating.
	//Not exactly strange (especially by mareth standards), it just suggests you're rather into recieving anal penetration. obtained naturally if you have an anal fetish
	//or if you have a lot of experience in anal sex.
	//moist and up: same as before. not naturally achievable, though of course TF items or interactions can cause it.
	public enum AnalWetness : byte { NORMAL, DAMP, MOIST, SLIMY, DROOLING, SLIME_DROOLING }

	//Pretty sure normal for ass size is pretty tight. so

#warning consider adding a means to prevent creatures from changing asshole location, and handling that better.

	public enum AnalLooseness : byte { NORMAL, LOOSE, ROOMY, STRETCHED, GAPING } //if you want to add a clown car level here, may i suggest RENT_ASUNDER?

	public enum AssholeLocation : byte { BUTT, MOUTH }

	public sealed partial class Ass : SimpleSaveablePart<Ass, AssData>, IBodyPartTimeLazy
	{
		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_ANAL_CAPACITY = ushort.MaxValue;

		private const byte LOOSENESS_LOOSE_TIMER = 72;
		private const byte LOOSENESS_ROOMY_TIMER = 48;
		private const byte LOOSENESS_STRETCHED_TIMER = 24;
		private const byte LOOSENESS_GAPING_TIMER = 12;

		private byte buttTightenTimer = 0;

		BasePerkModifiers baseModifiers => CreatureStore.GetCreatureClean(creatureID)?.perks.baseModifiers;

		public AnalLooseness minLooseness => baseModifiers?.minAnalLooseness.GetValue() ?? AnalLooseness.NORMAL;

		public AnalLooseness maxLooseness => baseModifiers?.maxAnalLooseness.GetValue() ?? AnalLooseness.GAPING;

		public AnalLooseness looseness
		{
			get => _analLooseness;
			private set
			{
				Utils.ClampEnum(ref value, minLooseness, maxLooseness);
				if (value != _analLooseness)
				{
					//if we shrink or grow the looseness, reset the timer.
					buttTightenTimer = 0;
					//then do the standard event stuff.
					var oldData = AsReadOnlyData();
					_analLooseness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private AnalLooseness _analLooseness = AnalLooseness.NORMAL;

		internal void CheckLooseness()
		{
			looseness = looseness;
		}

		public AnalWetness minWetness => baseModifiers?.minAnalWetness.GetValue() ?? AnalWetness.NORMAL;

		public AnalWetness maxWetness => baseModifiers?.maxAnalWetness.GetValue() ?? AnalWetness.SLIME_DROOLING;

		public AnalWetness wetness
		{
			get => _analWetness;
			private set
			{
				Utils.ClampEnum(ref value, minWetness, maxWetness);
				if (_analWetness != value)
				{
					var oldData = AsReadOnlyData();
					_analWetness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private AnalWetness _analWetness = AnalWetness.NORMAL;

		internal void CheckWetness()
		{
			wetness = wetness;
		}

		public ushort bonusAnalCapacity
		{
			get => _bonusAnalCapacity;
			private set
			{
				if (_bonusAnalCapacity != value)
				{
					var oldData = AsReadOnlyData();
					_bonusAnalCapacity = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private ushort _bonusAnalCapacity = 0;

		//these are wired directly from perk collection, because ass is immutable - it will always exist.
		public ushort perkBonusAnalCapacity => baseModifiers?.perkBasedBonusAnalCapacity.GetValue() ?? 0;

		internal void OnPerkAnalCapacityChanged(ushort oldPerkValue)
		{
			var capacity = AnalCapacity((byte)looseness, ((byte)wetness).add(1), bonusAnalCapacity, oldPerkValue, everPracticedAnal);
			var oldData = new AssData(creatureID, wetness, looseness, capacity, location, totalSexCount, selfSexCount, totalPenetrateCount, selfPenetrateCount,
				orgasmCount, dryOrgasmCount, totalAnalBirths);
			NotifyDataChanged(oldData);
		}



		public ushort AnalCapacity()
		{
			return AnalCapacity((byte)looseness, ((byte)wetness).add(1), bonusAnalCapacity, perkBonusAnalCapacity, everPracticedAnal);
		}

		private static ushort AnalCapacity(byte loose, byte wet, ushort bonusCap, ushort perkBonusCap, bool everPracticedAnal)
		{
			if (everPracticedAnal)
			{
				loose++;
			}
			uint cap = (uint)Math.Round(BASE_CAPACITY + bonusCap + perkBonusCap /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_ANAL_CAPACITY)
			{
				return MAX_ANAL_CAPACITY;
			}
			return (ushort)cap;
		}


		public AssholeLocation location { get; private set; } = AssholeLocation.BUTT;

		#region Sexual MetaData
		//anything that would qualify as taking anal virginity
		public uint totalSexCount { get; private set; } = 0;
		//times you've done it to yourself because reality need not apply.
		public uint selfSexCount { get; private set; } = 0;
		//includes total sex count and things you wouldn't qualify as taking anal viriginity, such as dildoes or whatever.
		public uint totalPenetrateCount { get; private set; } = 0;
		//times you've done it to yourself because reality need not apply.
		public uint selfPenetrateCount { get; private set; } = 0;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint totalAnalBirths { get; private set; } = 0;

		public bool virgin => totalSexCount == 0 && totalAnalBirths == 0;
		public bool everPracticedAnal => totalPenetrateCount > 0;
		#endregion
		#region Constructor
		internal Ass(Guid creatureID) : base(creatureID)
		{
			looseness = AnalLooseness.NORMAL;
			wetness = AnalWetness.NORMAL;
		}

		//default behavior is to let the ass determine if it's still virgin.
		//allows PC to masturbate/"practice" w/o losing anal virginity.
		//if set to false, it will be ignored if looseness is still normal.
		//nothing here can be null so we're fine.
		internal Ass(Guid creatureID, AnalWetness analWetness, AnalLooseness analLooseness, bool virginAnus, bool? everPracticedAnal = null) : base(creatureID)
		{
			wetness = analWetness;
			looseness = analLooseness;
			//if not set or explicitly null
			if (everPracticedAnal == null)
			{
				everPracticedAnal = analLooseness != AnalLooseness.NORMAL;
			}

			totalPenetrateCount = (uint)((bool)everPracticedAnal ? 1 : 0);
			totalSexCount = (uint)(virgin ? 0 : 1);
		}

		#endregion

		public override string BodyPartName() => Name();


		public override AssData AsReadOnlyData()
		{
			return new AssData(this);
		}
		public string ShortDescription() => AssStrings.ShortDescription();
		public string SingleItemDescription() => AssStrings.ShortDescription(true);

		public string LongDescription(bool alternateFormat = false) => AssStrings.LongDescription(this, alternateFormat);
		public string FullDescription(bool alternateFormat = false) => AssStrings.FullDescription(this, alternateFormat);

		#region Update Variables - Ass-Specific
		public byte IncreaseLooseness(byte amount = 1)
		{

			AnalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumAdd(amount);
			return looseness - oldLooseness;
		}

		public byte DecreaseLooseness(byte amount = 1)
		{

			AnalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumSubtract(amount);
			return oldLooseness - looseness;
		}

		public bool SetAnalLooseness(AnalLooseness analLooseness)
		{
			if (analLooseness >= minLooseness && analLooseness <= maxLooseness)
			{
				looseness = analLooseness;
				return true;
			}
			else
			{
				looseness = Utils.ClampEnum2(analLooseness, minLooseness, maxLooseness);
				return false;
			}
		}

		public byte IncreaseWetness(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		public byte DecreaseWetness(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		public bool SetAnalWetness(AnalWetness analWetness)
		{
			if (analWetness >= minWetness && analWetness <= maxWetness)
			{
				wetness = analWetness;
				return true;
			}
			else
			{
				wetness = Utils.ClampEnum2(analWetness, minWetness, maxWetness);
				return false;
			}
		}

		public ushort IncreaseBonusCapacity(ushort amountToAdd)
		{
			ushort currentCapacity = bonusAnalCapacity;
			bonusAnalCapacity = bonusAnalCapacity.add(amountToAdd);
			return bonusAnalCapacity.subtract(currentCapacity);
		}

		public ushort DecreaseBonusCapacity(ushort amountToRemove)
		{
			ushort currentCapacity = bonusAnalCapacity;
			bonusAnalCapacity = bonusAnalCapacity.subtract(amountToRemove);
			return bonusAnalCapacity.subtract(currentCapacity);
		}

		public bool SetAssholeLocation(AssholeLocation newLocation)
		{
			if (location == newLocation)
			{
				return false;
			}

			location = newLocation;
			return true;
		}


		#endregion
		//Alias these in the creature class, adding the relevant features not in Ass itself (knockup, orgasm)
		#region Unique Functions

		internal bool PenetrateAsshole(ushort penetratorArea, double knotArea, double cumAmount, bool takeAnalVirginity, bool reachOrgasm, bool sourceIsSelf)
		{
			totalPenetrateCount++;
			if (sourceIsSelf)
			{
				selfPenetrateCount++;
			}

			//experience = experience.add(analExperiencedGained);
			AnalLooseness oldLooseness = looseness;
			ushort capacity = AnalCapacity();

			HandleStretching(penetratorArea, knotArea);

			if (takeAnalVirginity)
			{
				totalSexCount++;

				if (sourceIsSelf)
				{
					selfSexCount++;
				}

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
			totalAnalBirths++;
		}

		private void HandleStretching(ushort penetratorArea, double knotArea)
		{
			ushort capacity = AnalCapacity();
			//don't have to worry about overflow, as +1 or +2 will never overflow our artificial max.
			if (penetratorArea >= capacity * 3f)
			{
				looseness += 2;
			}
			else if (penetratorArea >= capacity * 1.5f)
			{
				looseness++;
			}
			else if (penetratorArea >= capacity)
			{
				if (Utils.RandBool()) looseness++;
			}
			else if (penetratorArea >= AnalCapacity() * 0.9f)
			{
				if (Utils.Rand(4) == 0) looseness++;
			}
			else if (penetratorArea >= AnalCapacity() * 0.75f)
			{
				if (Utils.Rand(10) == 0) looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				buttTightenTimer = 0;
			}
		}

		#endregion

		public override bool IsIdenticalTo(AssData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && original.analCapacity == AnalCapacity() && original.location == location && looseness == original.looseness
				&& wetness == original.wetness && (ignoreSexualMetaData || (totalSexCount == original.totalSexCount && selfSexCount == original.selfSexCount
				&& totalPenetrateCount == original.totalPenetrateCount && selfPenetrateCount == original.selfPenetrateCount && orgasmCount == original.orgasmCount
				&& dryOrgasmCount == original.dryOrgasmCount && totalAnalBirths == original.totalAnalBirths));
		}

		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			looseness = looseness;
			wetness = wetness;

			return true;
		}
		#endregion
		#region BodyPartTime
		private byte timerAmount
		{
			get
			{
				if (looseness < AnalLooseness.LOOSE)
				{
					return 0;
				}
				else if (looseness == AnalLooseness.LOOSE)
				{
					return LOOSENESS_LOOSE_TIMER;
				}
				else if (looseness == AnalLooseness.ROOMY)
				{
					return LOOSENESS_ROOMY_TIMER;
				}
				else if (looseness == AnalLooseness.STRETCHED)
				{
					return LOOSENESS_STRETCHED_TIMER;
				}
				else //if (looseness >= AnalLooseness.GAPING)
				{
					return LOOSENESS_GAPING_TIMER;
				}
			}
		}

		string IBodyPartTimeLazy.reactToTimePassing(bool isPlayer, byte hoursPassed)
		{
			StringBuilder outputBuilder = new StringBuilder();

			//these should be done automatically, but if it's missed, we'll silently correct it.
			if (looseness < minLooseness)
			{
				looseness = minLooseness;
				buttTightenTimer = 0;
			}
			else if (looseness > maxLooseness)
			{
				looseness = maxLooseness;
				buttTightenTimer = 0;
			}
			//normal stuff.
			else if (looseness > minLooseness)
			{
				buttTightenTimer.addIn(hoursPassed);
				if (buttTightenTimer >= timerAmount)
				{
					if (isPlayer)
					{
						outputBuilder.Append(AssTightenedUpDueToInactivity(looseness));
					}
					looseness--;
					buttTightenTimer = 0;
				}
			}
			else if (buttTightenTimer > 0)
			{
				buttTightenTimer = 0;
			}

			return outputBuilder.ToString();
		}
		#endregion

		#region Not Implemented - Ideas
		//how "experienced" the character is with anal sex. not used atm. as of now, it just increases by 1 with each experience.
		//idk, maybe change this.
		//public byte experience
		//{
		//	get => _experience;
		//	set
		//	{
		//		_experience = Utils.Clamp2<byte>(value, 0, 100);
		//	}
		//}
		//private byte _experience;

		//minimum and maximum looseness/wetness are locked to perks. it's theoretically possible to not do that, but umm... fuck it.

		//internal byte IncreaseMinimumLooseness(byte amount = 1, bool forceIncreaseMax = false)
		//{
		//	AnalLooseness looseness = minLooseness;
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
		//	AnalLooseness looseness = minLooseness;
		//	minLooseness = minLooseness.ByteEnumSubtract(amount);
		//	return looseness - minLooseness;
		//}
		//internal void SetMinLoosness(AnalLooseness newValue)
		//{
		//	minLooseness = newValue;
		//}

		//internal byte IncreaseMaximumLooseness(byte amount = 1)
		//{
		//	AnalLooseness looseness = maxLooseness;
		//	maxLooseness = maxLooseness.ByteEnumSubtract(amount);
		//	return maxLooseness - looseness;
		//}
		//internal byte DecreaseMaximumLooseness(byte amount = 1, bool forceDecreaseMin = false)
		//{
		//	AnalLooseness looseness = minLooseness;
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
		//internal void SetMaxLoosness(AnalLooseness newValue)
		//{
		//	maxLooseness = newValue;
		//}


		//internal byte IncreaseMinimumWetness(byte amount = 1, bool forceIncreaseMax = false)
		//{
		//	AnalWetness wetness = minWetness;
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
		//	AnalWetness wetness = minWetness;
		//	minWetness = minWetness.ByteEnumSubtract(amount);
		//	return wetness - minWetness;
		//}
		//internal void SetMinWetness(AnalWetness newValue)
		//{
		//	minWetness = newValue;
		//}
		//internal byte IncreaseMaximumWetness(byte amount = 1)
		//{
		//	AnalWetness wetness = maxWetness;
		//	maxWetness = maxWetness.ByteEnumSubtract(amount);
		//	return maxWetness - wetness;
		//}
		//internal byte DecreaseMaximumWetness(byte amount = 1, bool forceDecreaseMin = false)
		//{
		//	AnalWetness wetness = minWetness;
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
		//internal void SetMaxWetness(AnalWetness newValue)
		//{
		//	maxWetness = newValue;
		//}
		#endregion
	}

	public sealed class AssData : SimpleData, IAss
	{
		public readonly AnalWetness wetness;
		public readonly AnalLooseness looseness;

		public readonly ushort analCapacity;

		public bool virgin => totalSexCount == 0 && totalAnalBirths == 0;
		public bool everPracticedAnal => totalPenetrateCount > 0;

		public readonly AssholeLocation location;

		#region Sexual MetaData
		public readonly uint totalSexCount;
		public readonly uint selfSexCount;
		public readonly uint totalPenetrateCount;
		public readonly uint selfPenetrateCount;
		public readonly uint orgasmCount;
		public readonly uint dryOrgasmCount;

		public readonly uint totalAnalBirths;

		#endregion

		public string ShortDescription() => AssStrings.ShortDescription();
		public string LongDescription(bool alternateFormat = false) => AssStrings.LongDescription(this, alternateFormat);
		public string FullDescription(bool alternateFormat = false) => AssStrings.FullDescription(this, alternateFormat);

		internal AssData(Ass source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			wetness = source.wetness;
			looseness = source.looseness;
			analCapacity = source.AnalCapacity();
			location = source.location;

			totalSexCount = source.totalSexCount;
			selfSexCount = source.selfSexCount;
			totalPenetrateCount = source.totalPenetrateCount;
			selfPenetrateCount = source.selfPenetrateCount;
			orgasmCount = source.orgasmCount;
			dryOrgasmCount = source.dryOrgasmCount;

			totalAnalBirths = source.totalAnalBirths;
		}

		public AssData(Guid creatureID, AnalWetness wetness, AnalLooseness looseness, ushort analCapacity, AssholeLocation location, uint totalSexCount,
			uint selfSexCount, uint totalPenetrateCount, uint selfPenetrateCount, uint orgasmCount, uint dryOrgasmCount, uint totalAnalBirths) : base(creatureID)
		{
			this.wetness = wetness;
			this.looseness = looseness;
			this.analCapacity = analCapacity;
			this.location = location;
			this.totalSexCount = totalSexCount;
			this.selfSexCount = selfSexCount;
			this.totalPenetrateCount = totalPenetrateCount;
			this.selfPenetrateCount = selfPenetrateCount;
			this.orgasmCount = orgasmCount;
			this.dryOrgasmCount = dryOrgasmCount;
			this.totalAnalBirths = totalAnalBirths;
		}

		bool IAss.virgin => virgin;

		AnalLooseness IAss.looseness => looseness;

		AnalWetness IAss.wetness => wetness;

		bool IAss.everPracticedAnal => everPracticedAnal;

		AssholeLocation IAss.location => location;
	}
}
