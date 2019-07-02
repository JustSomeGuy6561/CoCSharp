//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Perks;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Text;

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
	public sealed partial class Ass : IBodyPartTimeLazy, IBaseStatPerkAware
	{
		public const ushort BASE_CAPACITY = 10; //you now have a base capacity so you can handle insertions, even if you don't have any wetness or whatever.
		public const ushort MAX_ANAL_CAPACITY = ushort.MaxValue;

		private const byte LOOSENESS_LOOSE_TIMER = 72;
		private const byte LOOSENESS_ROOMY_TIMER = 48;
		private const byte LOOSENESS_STRETCHED_TIMER = 24;
		private const byte LOOSENESS_GAPING_TIMER = 12;
		private BackendSessionData saveData => BackendSessionData.data;

		private byte buttTightenTimer = 0;

		private AnalLooseness maxLooseness => baseStats().maxAnalLooseness;
		private AnalLooseness minLooseness => baseStats().minAnalLooseness;

		private AnalWetness minWetness => baseStats().minAnalWetness;
		private AnalWetness maxWetness => baseStats().maxAnalWetness;

		public AnalWetness wetness
		{
			get => _analWetness;
			private set => _analWetness = Utils.ClampEnum2(value, minWetness, maxWetness);
		}

		private AnalWetness _analWetness = AnalWetness.NORMAL;

		public AnalLooseness looseness
		{
			get => _analLooseness;
			private set
			{
				//if we shrink or grow the looseness, reset the timer. 
				if (value != _analLooseness)
				{
					buttTightenTimer = 0;
				}
				_analLooseness = Utils.ClampEnum2(value, minLooseness, maxLooseness);
			}
		}
		private AnalLooseness _analLooseness = AnalLooseness.NORMAL;

		public ushort bonusAnalCapacity { get; private set; } = 0;

		private ushort perkBonusAnalCapacity => baseStats?.Invoke().PerkBasedBonusAnalCapacity ?? 0;
		public ushort analCapacity()
		{

			byte loose = (byte)looseness;
			if (!virgin)
			{
				loose++;
			}
			byte wet = ((byte)wetness).add(1);
			uint cap = (uint)Math.Floor(BASE_CAPACITY + bonusAnalCapacity + perkBonusAnalCapacity /*+ experience / 10*/ + 6 * loose * loose * wet / 10.0);
			if (cap > MAX_ANAL_CAPACITY)
			{
				return MAX_ANAL_CAPACITY;
			}
			return (ushort)cap;
		}

		public ushort numTimesAnal { get; private set; } = 0;

		public bool virgin { get; private set; } = true;

		public SimpleDescriptor shortDescription => shortDesc;
		public SimpleDescriptor fullDescription => fullDesc;

		#region Constructor
		private Ass()
		{
			looseness = AnalLooseness.NORMAL;
			wetness = AnalWetness.NORMAL;
			virgin = true;
			numTimesAnal = 0;
		}
		#endregion
		#region Generate
		internal static Ass GenerateDefault()
		{
			return new Ass();
		}

		//default behavior is to let the ass determine if it's still virgin.
		//allows PC to masturbate/"practice" w/o losing anal virginity.
		//if set to false, it will be ignored if looseness is still normal.
		//nothing here can be null so we're fine.
		internal static Ass Generate(AnalWetness analWetness, AnalLooseness analLooseness, bool? virginAnus = null)
		{
			Ass ass = new Ass()
			{
				wetness = analWetness,
				looseness = analLooseness
			};
			//if not set or explicitly null
			if (virginAnus == null)
			{
				ass.virgin = analLooseness == AnalLooseness.NORMAL;
			}
			else
			{
				ass.virgin = (bool)virginAnus;
			}
			return ass;
		}
		#endregion
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

		internal bool SetAnalLooseness(AnalLooseness analLooseness, bool forceIfLessThanCurrentMin = false)
		{
			if (forceIfLessThanCurrentMin && analLooseness < minLooseness)
			{
				return true;
			}
			else if (analLooseness >= minLooseness)
			{
				looseness = analLooseness;
				return true;
			}
			return false;
		}

		internal byte AddWetness(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumAdd(amount);
			return wetness - oldWetness;
		}

		internal byte SubtractWetness(byte amount = 1)
		{
			AnalWetness oldWetness = wetness;
			wetness = wetness.ByteEnumSubtract(amount);
			return oldWetness - wetness;
		}
		internal void ForceAnalWetness(AnalWetness analWetness)
		{
			wetness = analWetness;
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
		internal bool analSex(ushort penetratorArea)
		{
			numTimesAnal++;
			return analPenetrate(penetratorArea, true);
		}
		internal bool analPenetrate(ushort penetratorArea, bool takeAnalVirginity = false/*, byte analExperiencedGained = 1*/)
		{

			//experience = experience.add(analExperiencedGained);
			AnalLooseness oldLooseness = looseness;
			ushort capacity = analCapacity();

			//don't have to worry about overflow, as +1 will never overflow our artificial max.
			if (penetratorArea >= capacity * 1.5f)
			{
				looseness++; 
			}
			else if (penetratorArea >= capacity && Utils.RandBool())
			{
				looseness++;
			}
			else if (penetratorArea >= analCapacity() * 0.9f && Utils.Rand(4) == 0)
			{
				looseness++;
			}
			else if (penetratorArea >= analCapacity() * 0.75f && Utils.Rand(10) == 0)
			{
				looseness++;
			}
			if (penetratorArea >= capacity / 2)
			{
				buttTightenTimer = 0;
			}
			if (virgin && takeAnalVirginity)
			{
				virgin = false;
			}
			return oldLooseness != looseness;
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

			PassiveStatModifiers perkStats = baseStats();
			//these should be done automatically by the perk that activated them, but if it's missed, we'll silently correct it. 
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
		#region BasePerkStats
		private BasePerkDataGetter baseStats;

		void IBaseStatPerkAware.GetBasePerkStats(BasePerkDataGetter getter)
		{
			baseStats = getter;
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
		#endregion
	}
}