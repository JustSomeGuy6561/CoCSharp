//Breasts.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:27 AM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//Note: Breasts aren't generated until after perks have been created. Thus, their post perk init is never called, but initial constructor can use perk data without fail.
	public sealed partial class Breasts : SimpleSaveablePart<Breasts, BreastWrapper>, IGrowable, IShrinkable
	{
		public override string BodyPartName() => Name();

		private Creature creature
		{
			get
			{
				CreatureStore.TryGetCreature(creatureID, out Creature creatureSource);
				return creatureSource;
			}
		}

		private Gender currGender => creature?.genitals.gender ?? Gender.MALE;
		private int currentBreastRow => creature?.genitals.breastRows.IndexOf(this) ?? 0;

		public int index => currentBreastRow;

		public const CupSize DEFAULT_MALE_SIZE = CupSize.FLAT;
		public const CupSize DEFAULT_FEMALE_SIZE = CupSize.C;

		public CupSize maleMinCup
		{
			get => _maleMinCup;
			internal set
			{
				_maleMinCup = value;
				if (cupSize < minimumCupSize)
				{
					var oldValue = AsData();
					cupSize = minimumCupSize;
					NotifyDataChanged(oldValue);
				}
			}
		}
		private CupSize _maleMinCup = CupSize.FLAT;
		public CupSize femaleMinCup
		{
			get => _femaleMinCup;
			internal set
			{
				_femaleMinCup = value;
				if (cupSize < minimumCupSize)
				{
					var oldValue = AsData();
					cupSize = minimumCupSize;
					NotifyDataChanged(oldValue);
				}
			}
		}
		private CupSize _femaleMinCup = CupSize.C;

		internal CupSize maleDefaultCup;
		internal CupSize femaleDefaultCup;

		private CupSize resetSize => currGender.HasFlag(Gender.FEMALE) ? EnumHelper.Min(femaleMinCup, femaleDefaultCup) : EnumHelper.Min(maleMinCup, maleDefaultCup);
		private CupSize minimumCupSize => currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

		internal float bigTiddyMultiplier = 1;
		internal float tinyTiddyMultiplier = 1;

		private bool makeBigTits => bigTiddyMultiplier > 1.1f;
		private bool makeSmallTits => tinyTiddyMultiplier > 1.1f;

		public float lactationAmount(bool perBreast)
		{
			if (creature is null)
			{
				return 0;
			}
			else if (!perBreast)
			{
				return creature.genitals.currentLactationAmount;
			}
			else
			{
				return creature.genitals.currentLactationAmount / (creature.genitals.breastRows.Count * NUM_BREASTS);
			}
		}


		public const byte NUM_BREASTS = 2;
		public byte numBreasts => NUM_BREASTS;

		public override BreastWrapper AsReadOnlyReference()
		{
			return new BreastWrapper(this, currentBreastRow);
		}

		public CupSize cupSize
		{
			get => _cupSize;
			private set
			{
				Utils.ClampEnum(ref value, minimumCupSize, CupSize.JACQUES00); //enums: icomparable, but not really. woooo!
				if (_cupSize != value)
				{
					var oldData = AsData();
					_cupSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private CupSize _cupSize;
		public uint orgasmCount { get; private set; } = 0;
		public uint dryOrgasmCount { get; private set; } = 0;

		public uint titFuckCount { get; private set; } = 0;
		public uint dickNippleFuckCount => nipples.dickNippleFuckCount;
		public uint nippleFuckCount => nipples.nippleFuckCount;
		public uint nippleOrgasmCount => nipples.orgasmCount;
		public uint nippleDryOrgasmCount => nipples.dryOrgasmCount;

		public readonly Nipples nipples;
		public bool isMale => cupSize == CupSize.FLAT && nipples.length <= .5f;

		internal Breasts(Guid creatureID, BreastPerkHelper initialPerkWrapper, Gender initialGender) : base(creatureID)
		{

			if (initialGender.HasFlag(Gender.FEMALE))
			{
				_cupSize = initialPerkWrapper.FemaleNewLength();
			}
			else
			{
				_cupSize = initialPerkWrapper.MaleNewLength();
			}
			nipples = new Nipples(creatureID, this, initialPerkWrapper, initialGender);

			_maleMinCup = initialPerkWrapper.MaleMinCup;
			_femaleMinCup = initialPerkWrapper.FemaleMinCup;
			maleDefaultCup = initialPerkWrapper.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkWrapper.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkWrapper.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkWrapper.TitsShrinkMultiplier;


			CupSize min = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}
		internal Breasts(Guid creatureID, BreastPerkHelper initialPerkWrapper, CupSize cup, float nippleLength) : base(creatureID)
		{
			nipples = new Nipples(creatureID, this, initialPerkWrapper, nippleLength);
			_cupSize = Utils.ClampEnum2(cup, CupSize.FLAT, CupSize.JACQUES00);

			_maleMinCup = initialPerkWrapper.MaleMinCup;
			_femaleMinCup = initialPerkWrapper.FemaleMinCup;
			maleDefaultCup = initialPerkWrapper.MaleNewDefaultCup;
			femaleDefaultCup = initialPerkWrapper.FemaleNewDefaultCup;

			bigTiddyMultiplier = initialPerkWrapper.TitsGrowthMultiplier;
			tinyTiddyMultiplier = initialPerkWrapper.TitsShrinkMultiplier;

		}

		private void OnGenderChanged(object sender, EventHelpers.GenderChangedEventArgs e)
		{
			CupSize min = e.newGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;
			if (cupSize < min)
			{
				cupSize = min;
			}
		}

		protected internal override void PostPerkInit()
		{
			nipples.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			nipples.LateInit();
			if (creature != null) creature.genitals.onGenderChanged += OnGenderChanged;
		}

		public byte GrowBreasts(byte byAmount, bool ignorePerks = false)
		{
			if (cupSize >= CupSize.JACQUES00)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * bigTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}

			var oldData = AsData();
			cupSize = cupSize.ByteEnumAdd(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return cupSize - oldSize;
		}

		public byte ShrinkBreasts(byte byAmount, bool ignorePerks = false)
		{
			if (cupSize <= CupSize.FLAT)
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (!ignorePerks)
			{
				ushort val = (ushort)Math.Round(byAmount * tinyTiddyMultiplier);
				byAmount = val > byte.MaxValue ? byte.MaxValue : (byte)val;
			}
			var oldData = AsData();

			cupSize = cupSize.ByteEnumSubtract(byAmount);
			if (cupSize != oldSize)
			{
				NotifyDataChanged(oldData);
			}
			return cupSize - oldSize;
		}

		public void SetCupSize(CupSize size)
		{

			Utils.ClampEnum(ref size, CupSize.FLAT, CupSize.JACQUES00);
			if (cupSize != size)
			{
				var oldData = AsData();
				cupSize = size;
				NotifyDataChanged(oldData);
			}
		}
		public bool MakeMale(bool removeStatus = true)
		{
			if (isMale)
			{
				return false;
			}
			var oldData = AsData();

			cupSize = maleMinCup;
			nipples.ShrinkNipple(nipples.length - Nipples.MIN_NIPPLE_LENGTH);
			NotifyDataChanged(oldData);
			return true;
		}

		public bool MakeFemale(bool removeStatus = true)
		{
			if (!isMale)
			{
				return false;
			}
			var oldData = AsData();
			if (cupSize < femaleMinCup)
			{
				cupSize = femaleMinCup;
			}
			else if (cupSize == CupSize.FLAT)
			{
				cupSize = CupSize.B;
			}

			//nipple data change 
			if (nipples.length < 0.5)
			{
				nipples.GrowNipple(0.5f - nipples.length);
			}
			NotifyDataChanged(oldData);
			return true;
		}

		public void Reset(bool resetPiercings = false)
		{
			cupSize = currGender.HasFlag(Gender.FEMALE) ? femaleMinCup : maleMinCup;

			nipples.Reset(resetPiercings);
			//nippleFuckCount = 0;
			titFuckCount = 0;
			//dickNippleFuckCount = 0;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			cupSize = cupSize;
			return nipples.Validate(correctInvalidData);
		}

		public BreastData AsData()
		{
			return new BreastData(cupSize);
		}

		private readonly WeakEventSource<SimpleDataChangedEvent<BreastWrapper, BreastData>> dataChangeSource =
			new WeakEventSource<SimpleDataChangedEvent<BreastWrapper, BreastData>>();

		public event EventHandler<SimpleDataChangedEvent<BreastWrapper, BreastData>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private void NotifyDataChanged(BreastData oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangedEvent<BreastWrapper, BreastData>(AsReadOnlyReference(), oldData));
		}


		#region Nipple Alias

		public float GrowNipple(float growAmount, bool ignorePerk = false)
		{
			var oldData = AsData();
			var retVal = nipples.GrowNipple(growAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}

		public float ShrinkNipple(float shrinkAmount, bool ignorePerk = false)
		{
			var oldData = AsData();
			var retVal = nipples.ShrinkNipple(shrinkAmount, ignorePerk);
			if (retVal != 0)
			{
				NotifyDataChanged(oldData);
			}
			return retVal;
		}
		#endregion
		//to be frank, idk what would actually orgasm when being titty fucked, but, uhhhh... i guess it can be stored in stats or some shit?
		internal void DoTittyFuck(float length, float girth, float knotWidth, bool reachOrgasm)
		{
			titFuckCount++;
			if (reachOrgasm)
			{
				orgasmCount++;
			}
		}

		internal void OrgasmTits(bool dryOrgasm)
		{
			orgasmCount++;
			if (dryOrgasm) dryOrgasmCount++;
		}



		#region IGrowShrinkable
		bool IGrowable.CanGroPlus()
		{
			return cupSize < CupSize.JACQUES00;
		}

		bool IShrinkable.CanReducto()
		{
			return cupSize > minimumCupSize;
		}

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			this.cupSize += (byte)(Utils.Rand(2) + 1);
			//c# is a bitch in that all numbers are treated as ints or doubles unless explicitly cast - byte me
			this.cupSize += (byte)(makeBigTits && Utils.RandBool() ? 1 : 0); //add one for big tits perk 50% of the time
			return cupSize - oldSize;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			CupSize oldSize = cupSize;
			if (cupSize.ByteEnumSubtract(1) == minimumCupSize || !makeSmallTits || Utils.RandBool())
			{
				cupSize--;
			}
			else
			{
				cupSize -= 2;
			}
			return oldSize - cupSize;
		}
		#endregion
	}

	public sealed class BreastWrapper : SimpleWrapper<BreastWrapper, Breasts>
	{
		public readonly NippleWrapper nipples;
		public CupSize cupSize => sourceData.cupSize;
		public readonly int currBreastRowIndex;

		public CupSize maleMinCup => sourceData.maleMinCup;
		public CupSize femaleMinCup => sourceData.femaleMinCup;
		
		public byte numBreasts => sourceData.numBreasts;

		public uint orgasmCount => sourceData.orgasmCount;
		public uint dryOrgasmCount => sourceData.dryOrgasmCount;

		public uint titFuckCount => sourceData.titFuckCount;
		public uint dickNippleFuckCount => sourceData.dickNippleFuckCount;
		public uint nippleFuckCount => sourceData.nippleFuckCount;
		public uint nippleOrgasmCount => sourceData.nippleOrgasmCount;
		public uint nippleDryOrgasmCount => sourceData.nippleDryOrgasmCount;

		public bool isMale => sourceData.isMale;

		public string ShortDescription() => sourceData.ShortDescription();

		public string LongDescription() => sourceData.LongDescription();

		public string LongDescriptionWithSize() => sourceData.LongDescriptionWithSize();

		internal BreastWrapper(Breasts breasts, int currentBreastRow) : base(breasts)
		{
			nipples = breasts.nipples.AsReadOnlyReference();

			currBreastRowIndex = currentBreastRow;
		}

		internal BreastWrapper(Breasts breasts, NippleWrapper overrideNippleWrapper, int currentBreastRow) : base(breasts)
		{
			nipples = overrideNippleWrapper ?? throw new ArgumentNullException(nameof(overrideNippleWrapper));

			currBreastRowIndex = currentBreastRow;
		}
	}

	public sealed class BreastData
	{
		public readonly CupSize cupSize;

		public BreastData(CupSize cupSize)
		{
			this.cupSize = cupSize;
		}
	}
}
