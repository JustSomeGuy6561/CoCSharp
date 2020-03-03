//Clit.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 6:03 PM
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{

	//public enum ClitPiercings {

	//}

	public sealed partial class ClitPiercingLocation : PiercingLocation, IEquatable<ClitPiercingLocation>
	{
		private static readonly List<ClitPiercingLocation> _allLocations = new List<ClitPiercingLocation>();

		public static readonly ReadOnlyCollection<ClitPiercingLocation> allLocations;

		private readonly byte index;


		static ClitPiercingLocation()
		{
			allLocations = new ReadOnlyCollection<ClitPiercingLocation>(_allLocations);
		}

		public ClitPiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is ClitPiercingLocation clitPiercing)
			{
				return Equals(clitPiercing);
			}
			else
			{
				return false;
			}
		}

		public bool Equals(ClitPiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly ClitPiercingLocation CHRISTINA = new ClitPiercingLocation(0, SupportedJewelry, ChristinaButton, ChristinaLocation);
		public static readonly ClitPiercingLocation HOOD_VERTICAL = new ClitPiercingLocation(1, SupportedJewelry, VerticalHoodButton, VerticalHoodLocation);
		public static readonly ClitPiercingLocation HOOD_HORIZONTAL = new ClitPiercingLocation(2, SupportedJewelry, HorizontalHoodButton, HorizontalHoodLocation);
		public static readonly ClitPiercingLocation HOOD_TRIANGLE = new ClitPiercingLocation(3, SupportedJewelry, TriangleButton, TriangleLocation);
		public static readonly ClitPiercingLocation CLIT_ITSELF = new ClitPiercingLocation(4, SupportedJewelry, ThroughClitButton, ThroughClitLocation);
		public static readonly ClitPiercingLocation LARGE_CLIT_1 = new ClitPiercingLocation(5, SupportedJewelry, ThroughLargeClit1Button, ThroughLargeClit1Location);
		public static readonly ClitPiercingLocation LARGE_CLIT_2 = new ClitPiercingLocation(6, SupportedJewelry, ThroughLargeClit2Button, ThroughLargeClit2Location);
		public static readonly ClitPiercingLocation LARGE_CLIT_3 = new ClitPiercingLocation(7, SupportedJewelry, ThroughLargeClit3Button, ThroughLargeClit3Location);

		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD || jewelryType == JewelryType.RING || jewelryType == JewelryType.SPECIAL;
		}
	}

	public sealed class ClitPiercing : Piercing<ClitPiercingLocation>
	{
		public ClitPiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, CreatureStr playerShortDesc, CreatureStr playerLongDesc)
			: base(source, LocationUnlocked, playerShortDesc, playerLongDesc) { }

		public override int MaxPiercings => ClitPiercingLocation.allLocations.Count;

		public override IEnumerable<ClitPiercingLocation> availableLocations => ClitPiercingLocation.allLocations;
	}

	//note: perks are guarenteed to be valid by the time this is created, so it's post perk init won't be called.
	public sealed partial class Clit : SimpleSaveablePart<Clit, ClitData>, IGrowable, IShrinkable
	{
		public const double MIN_CLIT_SIZE = 0.25f;
		public const double DEFAULT_CLIT_SIZE = 0.25f;
		public const double MAX_CLIT_SIZE = 100f;

		public const double LARGEST_NATURAL_SIZE = 3f;


		public override string BodyPartName() => Name();

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);


		private double clitGrowthMultiplier => creature?.genitals.perkData.ClitGrowthMultiplier ?? 1;
		private double clitShrinkMultiplier => creature?.genitals.perkData.ClitShrinkMultiplier ?? 1;

		public double minClitSize => creature?.genitals.perkData.MinClitSize ?? MIN_CLIT_SIZE;

		//the largest this clit can be through 'natural' means. this factors in any perks that might alter the normal size. considering what game this is, this is hardly used.
		public double largestNormalSize => LARGEST_NATURAL_SIZE + (creature?.genitals.perkData.NewClitSizeDelta ?? 0);

		private double defaultNewClitSize => creature?.genitals.perkData.DefaultNewClitSize ?? MIN_CLIT_SIZE;
		private double newClitSizeDelta => creature?.genitals.perkData.NewClitSizeDelta ?? 0;

		//called by perks when min clit size changes. this ensures the clit size is still valid.
		internal void CheckClitSize()
		{
			//make sure it's still valid, even if min clit size has changed.
			length = length;
		}

		private double resetSize => Math.Max(defaultNewClitSize, minClitSize);

		public uint totalPenetrateCount { get; private set; } = 0;
		public uint selfPenetrateCount { get; private set; } = 0;

		public uint orgasmCount => parent.totalOrgasmCount;


		private readonly Vagina parent;
		private int vaginaIndex => creature?.genitals.vaginas.IndexOf(parent) ?? 0;

		private static readonly ClitPiercingLocation[] requiresFetish = { ClitPiercingLocation.LARGE_CLIT_1, ClitPiercingLocation.LARGE_CLIT_2, ClitPiercingLocation.LARGE_CLIT_3 };

		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;



		private double minSize
		{
			get
			{
				double currMin = minClitSize;
				return Math.Max(MIN_CLIT_SIZE, currMin);
			}
		}
		public double length
		{
			get => _length;
			private set
			{
				Utils.Clamp(ref value, minSize, MAX_CLIT_SIZE);
				if (_length != value)
				{
					ClitData oldData = AsReadOnlyData();
					_length = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private double _length;

		public readonly ClitPiercing piercings;

		internal Clit(Guid creatureID, Vagina source)
			: this(creatureID, source, null)
		{ }

		internal Clit(Guid creatureID, Vagina source, double? clitSize) : base(creatureID)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			_length = NewClitSize(clitSize);

			piercings = new ClitPiercing(this, PiercingLocationUnlocked, AllClitPiercingsShort, AllClitPiercingsLong);
		}

		private double NewClitSize(double? givenSize = null)
		{
			double minValue = Utils.Clamp2(Math.Max(defaultNewClitSize, minClitSize), MIN_CLIT_SIZE, MAX_CLIT_SIZE);
			if (givenSize != null)
			{
				givenSize += newClitSizeDelta;
			}
			if (givenSize is null || givenSize < minValue)
			{
				return minValue;
			}
			else
			{
				return (double)givenSize;
			}
		}

		public override ClitData AsReadOnlyData()
		{
			return new ClitData(this, vaginaIndex);
		}

		public static ClitData GenerateAggregate(Guid creatureID, double averageSize)
		{
			return new ClitData(creatureID, -1, averageSize, new ReadOnlyPiercing<ClitPiercingLocation>());
		}




		public void Restore()
		{
			//omnibusClit = false;
			length = MIN_CLIT_SIZE;
		}

		public void Reset()
		{
			Restore();
			piercings.Reset();
		}



		internal double GrowClit(double amount, bool ignorePerks = false)
		{
			if (length >= MAX_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}

			//hope this never matters but doubles don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			double oldLength = length;
			if (!ignorePerks)
			{
				length += amount * clitGrowthMultiplier;
			}
			else
			{
				length += amount;
			}
			return length - oldLength;
		}

		internal double ShrinkClit(double amount, bool ignorePerks = false)
		{
			if (length <= MIN_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}
			//hope this never matters but doubles don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			double oldLength = length;
			if (!ignorePerks)
			{
				length -= amount * clitShrinkMultiplier;
			}
			else
			{
				length -= amount;
			}
			return oldLength - length;
		}

		internal double SetClitSize(double newSize)
		{
			length = newSize;
			return length;
		}

		internal void DoPenetration(bool toSelf)
		{
			totalPenetrateCount++;

			if (toSelf)
			{
				selfPenetrateCount++;
			}
		}

		public override bool IsIdenticalTo(ClitData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && original.length == length && piercings.IsIdenticalTo(original.clitPiercings)
				&& (ignoreSexualMetaData || (selfPenetrateCount == original.selfPenetrateCount && totalPenetrateCount == original.totalPenetrateCount));
		}

		internal override bool Validate(bool correctInvalidData)
		{
			length = length;
			return piercings.Validate(correctInvalidData);
		}
		#region Text
		public static string PluralClitNoun() => ClitStrings.PluralClitNoun();
		public static string SingularClitNoun() => ClitStrings.ClitNoun(true);
		public static string ClitNoun() => ClitStrings.ClitNoun(false);
		public string ShortDescription() => ClitStrings.ShortDesc(length);
		public string LongDescription(bool alternateFormat = false) => ClitStrings.Desc(this, alternateFormat, false);
		public string FullDescription(bool alternateFormat = false) => ClitStrings.Desc(this, alternateFormat, true);
		#endregion
		#region Piercing Related
		private bool PiercingLocationUnlocked(ClitPiercingLocation piercingLocation, out string whyNot)
		{

			if (!requiresFetish.Contains(piercingLocation))
			{
				whyNot = null;
				return true;
			}
			else if (!piercingFetish)
			{
				whyNot = RequiresPiercingFetish();
				return false;
			}
			else if (piercingLocation == ClitPiercingLocation.LARGE_CLIT_1)
			{
				whyNot = RequiresClitBeAtLeastThisLong(3);
				return length >= 3;
			}
			else if (piercingLocation == ClitPiercingLocation.LARGE_CLIT_2)
			{
				whyNot = RequiresClitBeAtLeastThisLong(5);
				return length >= 5;
			}
			else //if (piercingLocation == ClitPiercingLocation.LARGE_CLIT_3)
			{
				whyNot = RequiresClitBeAtLeastThisLong(7);
				return length >= 7;
			}
		}

		public bool isPierced => piercings.isPierced;

		public bool wearingJewelry => piercings.wearingJewelry;


		#endregion
		#region Grow/Shrinkable
		bool IGrowable.CanGroPlus()
		{
			return length < MAX_CLIT_SIZE;
		}

		bool IShrinkable.CanReducto()
		{
			return length > minSize;
		}

		string IGrowable.UseGroPlus()
		{
			if (((IGrowable)this).CanGroPlus())
			{
				length += 1;
			}
			return null;
		}

		string IShrinkable.UseReducto()
		{
			if (((IShrinkable)this).CanReducto())
			{
				length /= 1.7f;
			}
			return null;
		}


		#endregion
	}

	public sealed partial class ClitData : SimpleData
	{
		public readonly double length;
		public readonly int vaginaIndex;

		public readonly ReadOnlyPiercing<ClitPiercingLocation> clitPiercings;

		public readonly uint totalPenetrateCount;
		public readonly uint selfPenetrateCount;

		#region Text
		public static string PluralClitNoun() => ClitStrings.PluralClitNoun();
		public static string SingularClitNoun() => ClitStrings.ClitNoun(true);
		public static string ClitNoun() => ClitStrings.ClitNoun(false);
		public string ShortDescription() => ClitStrings.ShortDesc(length);
		public string LongDescription(bool alternateFormat) => ClitStrings.Desc(this, alternateFormat, false);
		public string FullDescription(bool alternateFormat) => ClitStrings.Desc(this, alternateFormat, true);
		#endregion
		internal ClitData(Clit source, int currIndex) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			length = source.length;
			vaginaIndex = currIndex;

			clitPiercings = source.piercings.AsReadOnlyData();

			selfPenetrateCount = source.selfPenetrateCount;
			totalPenetrateCount = source.totalPenetrateCount;
		}

		public ClitData(Guid creatureID, int currentIndex, double length, ReadOnlyPiercing<ClitPiercingLocation> piercings) : base(creatureID)
		{
			this.length = length;

			vaginaIndex = currentIndex;

			clitPiercings = piercings;

			selfPenetrateCount = 0;
			totalPenetrateCount = 0;
		}

		public ClitData(Guid creatureID, int currentIndex, double length, ReadOnlyPiercing<ClitPiercingLocation> piercings, uint totalPenetrateCount,
			uint selfPenetrateCount) : base(creatureID)
		{
			this.length = length;

			vaginaIndex = currentIndex;

			clitPiercings = piercings;

			this.totalPenetrateCount = totalPenetrateCount;
			this.selfPenetrateCount = selfPenetrateCount;
		}
	}
}
