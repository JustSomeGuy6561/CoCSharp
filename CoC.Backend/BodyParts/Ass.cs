﻿//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
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

	public enum AnalLooseness : byte { NORMAL, LOOSE, ROOMY, STRETCHED, GAPING } //if you want to add a clown car level here, may i suggest RENT_ASUNDER?
	public sealed partial class Ass : SimpleSaveablePart<Ass, AssData>, IBodyPartTimeLazy
	{
		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_ANAL_CAPACITY = ushort.MaxValue;

		private const byte LOOSENESS_LOOSE_TIMER = 72;
		private const byte LOOSENESS_ROOMY_TIMER = 48;
		private const byte LOOSENESS_STRETCHED_TIMER = 24;
		private const byte LOOSENESS_GAPING_TIMER = 12;

		private byte buttTightenTimer = 0;

		public AnalLooseness minLooseness
		{
			get => _minLooseness;
			internal set
			{
				if (_minLooseness != value)
				{
					minLooseness = value;
					if (looseness < minLooseness)
					{
						looseness = minLooseness; //let the looseness change handle the notification handle
					}
					if (maxLooseness < minLooseness)
					{
						maxLooseness = minLooseness;
					}
				}
			}
		}
		private AnalLooseness _minLooseness = AnalLooseness.NORMAL;

		public AnalLooseness maxLooseness
		{
			get => _maxLooseness;
			internal set
			{
				if (value < minLooseness)
				{
					value = minLooseness;
				}
				if (_maxLooseness != value)
				{
					_maxLooseness = value;
					if (looseness > maxLooseness)
					{
						looseness = maxLooseness; //again, let looseness handle firing event. 
					}
				}
			}
		}
		private AnalLooseness _maxLooseness = AnalLooseness.GAPING;

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

		public AnalWetness minWetness
		{
			get => _minWetness;
			internal set
			{
				if (_minWetness != value)
				{
					minWetness = value;
					if (wetness < minWetness)
					{
						wetness = minWetness; //let the wetness change handle the notification handle
					}
					if (maxWetness < minWetness)
					{
						maxWetness = minWetness;
					}
				}
			}
		}
		private AnalWetness _minWetness = AnalWetness.NORMAL;
		public AnalWetness maxWetness
		{
			get => _maxWetness;
			internal set
			{
				if (value < minWetness)
				{
					value = minWetness;
				}
				if (_maxWetness != value)
				{
					_maxWetness = value;
					if (wetness > maxWetness)
					{
						wetness = maxWetness; //again, let wetness handle firing event. 
					}
				}
			}
		}
		private AnalWetness _maxWetness = AnalWetness.SLIME_DROOLING;

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


		public ushort perkBonusAnalCapacity
		{
			get => _perkBonusAnalCapacity;
			internal set
			{
				if (_perkBonusAnalCapacity != value)
				{
					var oldData = AsReadOnlyData();
					_perkBonusAnalCapacity = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private ushort _perkBonusAnalCapacity;

		public ushort analCapacity()
		{

			byte loose = (byte)looseness;
			if (everPracticedAnal)
			{
				loose++;
			}
			byte wet = ((byte)wetness).add(1);
			uint cap = (uint)Math.Round(BASE_CAPACITY + bonusAnalCapacity + perkBonusAnalCapacity /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_ANAL_CAPACITY)
			{
				return MAX_ANAL_CAPACITY;
			}
			return (ushort)cap;
		}

		public uint sexCount { get; private set; } = 0;
		public uint penetrateCount { get; private set; } = 0;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public bool virgin { get; private set; } = true;
		public bool everPracticedAnal { get; private set; } = false;

		public SimpleDescriptor shortDescription => shortDesc;
		public SimpleDescriptor fullDescription => fullDesc;

		#region Constructor
		internal Ass(Guid creatureID) : base(creatureID)
		{
			looseness = AnalLooseness.NORMAL;
			wetness = AnalWetness.NORMAL;
			virgin = true;
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

			this.everPracticedAnal = (bool)everPracticedAnal;
			virgin = virginAnus;
		}

		#endregion

		public override string BodyPartName() => Name();


		public override AssData AsReadOnlyData()
		{
			return new AssData(this);
		}
		#region Update Variables - Ass-Specific
		internal byte StretchAnus(byte amount = 1)
		{

			AnalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumAdd(amount);
			return looseness - oldLooseness;
		}

		internal byte ShrinkAnus(byte amount = 1)
		{

			AnalLooseness oldLooseness = looseness;
			looseness = looseness.ByteEnumSubtract(amount);
			return oldLooseness - looseness;
		}

		internal bool SetAnalLooseness(AnalLooseness analLooseness)
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

		internal byte MakeWetter(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		internal byte MakeDrier(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		internal bool SetAnalWetness(AnalWetness analWetness)
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

		internal ushort AddBonusCapacity(ushort amountToAdd)
		{
			ushort currentCapacity = bonusAnalCapacity;
			bonusAnalCapacity = bonusAnalCapacity.add(amountToAdd);
			return bonusAnalCapacity.subtract(currentCapacity);
		}

		internal ushort SubtractBonusCapacity(ushort amountToRemove)
		{
			ushort currentCapacity = bonusAnalCapacity;
			bonusAnalCapacity = bonusAnalCapacity.subtract(amountToRemove);
			return bonusAnalCapacity.subtract(currentCapacity);
		}



		#endregion
		//Alias these in the creature class, adding the relevant features not in Ass itself (knockup, orgasm)
		#region Unique Functions
		
		internal bool PenetrateAsshole(ushort penetratorArea, float knotArea, float cumAmount, bool takeAnalVirginity, bool reachOrgasm/*, byte analExperiencedGained = 1*/)
		{
			penetrateCount++;
			if (!everPracticedAnal)
			{
				everPracticedAnal = true;
			}

			//experience = experience.add(analExperiencedGained);
			AnalLooseness oldLooseness = looseness;
			ushort capacity = analCapacity();

			HandleStretching(penetratorArea, knotArea);
			
			if (!everPracticedAnal)
			{
				everPracticedAnal = true;
			}
			if (takeAnalVirginity)
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
			virgin = false;
		}

		private void HandleStretching(ushort penetratorArea, float knotArea)
		{
			ushort capacity = analCapacity();
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
			else if (penetratorArea >= analCapacity() * 0.9f)
			{
				if (Utils.Rand(4) == 0) looseness++;
			}
			else if (penetratorArea >= analCapacity() * 0.75f)
			{
				if (Utils.Rand(10) == 0) looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				buttTightenTimer = 0;
			}
		}

		#endregion

		#region Validate
		internal override bool Validate(bool correctInvalidData)
		{
			looseness = looseness;
			wetness = wetness;
			if (penetrateCount > 0 && !everPracticedAnal) //i'm going to let this one go silently.
			{
				everPracticedAnal = true;
			}
			if (sexCount > 0 && virgin) //see above
			{
				virgin = false;
			}
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

	public sealed class AssData : SimpleData
	{
		public readonly AnalWetness wetness;
		public readonly AnalLooseness looseness;

		public readonly ushort analCapacity;

		internal AssData(Ass source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			wetness = source.wetness;
			looseness = source.looseness;
			analCapacity = source.analCapacity();
		}
	}
}