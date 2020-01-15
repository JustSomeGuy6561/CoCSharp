//Clit.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 6:03 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.SaveData;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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
			else return false;
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
		public ClitPiercing(PiercingUnlocked LocationUnlocked, PlayerStr playerShortDesc, PlayerStr playerLongDesc) : base(LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => ClitPiercingLocation.allLocations.Count;

		public override IEnumerable<ClitPiercingLocation> availableLocations => ClitPiercingLocation.allLocations;
	}

	//note: perks are guarenteed to be valid by the time this is created, so it's post perk init won't be called.
	public sealed partial class Clit : SimpleSaveablePart<Clit, ClitData>, IGrowable, IShrinkable
	{
		public override string BodyPartName() => Name();

		internal float clitGrowthMultiplier = 1;
		internal float clitShrinkMultiplier = 1;
		internal float minClitSize
		{
			get => _minClitSize;
			set
			{
				_minClitSize = value;
				if (length < minSize)
				{
					length = minSize;
				}
			}
		}
		private float _minClitSize = MIN_CLIT_SIZE;

		internal float minNewClitSize;

		private float resetSize => Math.Max(minNewClitSize, minClitSize);

		public uint penetrateCount { get; private set; } = 0;

		public uint orgasmCount => parent.orgasmCount;

		private readonly Vagina parent;
		private int vaginaIndex => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.genitals.vaginas.IndexOf(parent) : 0;

		private static readonly ClitPiercingLocation[] requiresFetish = { ClitPiercingLocation.LARGE_CLIT_1, ClitPiercingLocation.LARGE_CLIT_2, ClitPiercingLocation.LARGE_CLIT_3 };

		private bool piercingFetish => BackendSessionSave.data.piercingFetishEnabled;

		public const float MIN_CLIT_SIZE = 0.25f;
		public const float DEFAULT_CLIT_SIZE = 0.25f;
		public const float MAX_CLIT_SIZE = 100f;

		private float minSize
		{
			get
			{
				float currMin = minClitSize;
				return Math.Max(MIN_CLIT_SIZE, currMin);
			}
		}
		public float length
		{
			get => _length;
			private set
			{
				Utils.Clamp(ref value, minSize, MAX_CLIT_SIZE);
				if (_length != value)
				{
					var oldData = AsReadOnlyData();
					_length = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private float _length;

		public readonly ClitPiercing clitPiercings;

		internal Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, bool isOmnibusClit = false)
			: this(creatureID, source, initialPerkData, null, isOmnibusClit)
		{ }

		internal Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, float clitSize, bool isOmnibusClit = false)
			: this(creatureID, source, initialPerkData, (float?)clitSize, isOmnibusClit)
		{ }

		private Clit(Guid creatureID, Vagina source, VaginaPerkHelper initialPerkData, float? clitSize, bool isOmnibusClit) : base(creatureID)
		{
			parent = source ?? throw new ArgumentNullException(nameof(source));
			_length = initialPerkData.NewClitSize(clitSize);

			clitPiercings = new ClitPiercing(PiercingLocationUnlocked, AllClitPiercingsShort, AllClitPiercingsLong);

			_minClitSize = initialPerkData.MinClitSize;
			minNewClitSize = initialPerkData.DefaultNewClitSize;
			clitGrowthMultiplier = initialPerkData.ClitGrowthMultiplier;
			clitShrinkMultiplier = initialPerkData.ClitShrinkMultiplier;
		}

		public override ClitData AsReadOnlyData()
		{
			return new ClitData(this, vaginaIndex);
		}

		public static ClitData GenerateAggregate(Guid creatureID, float averageSize)
		{
			return new ClitData(creatureID, -1, averageSize, new ReadOnlyPiercing<ClitPiercingLocation>());
		}


		//public bool omnibusActive => CreatureStore.GetCreatureClean(creatureID)?.cocks.Count == 0 && omnibusClit;

		//public Cock AsClitCock()
		//{
		//	if (!omnibusClit)
		//	{
		//		return null;
		//	}

		//	if (clitCock == null)
		//	{
		//		clitCock = Cock.GenerateClitCock(creatureID, this);
		//	}
		//	else
		//	{
		//		clitCock.SetLength(length + 5);
		//	}
		//	return clitCock;
		//}
		//private Cock clitCock = null;

		//internal uint asCockSexCount => clitCock?.sexCount ?? 0;
		//internal uint asCockSoundCount => clitCock?.soundCount ?? 0;
		//internal uint asCockOrgasmCount => clitCock?.orgasmCount ?? 0;
		//internal uint asCockDryOrgasmCount => clitCock?.dryOrgasmCount ?? 0;


		public void Restore()
		{
			//omnibusClit = false;
			length = MIN_CLIT_SIZE;
		}

		public void Reset()
		{
			Restore();
			clitPiercings.Reset();
		}

		//internal bool ActivateOmnibusClit()
		//{
		//	if (omnibusClit)
		//	{
		//		return false;
		//	}
		//	omnibusClit = true;
		//	return true;
		//}

		//internal bool DeactivateOmnibusClit()
		//{
		//	if (!omnibusClit)
		//	{
		//		return false;
		//	}
		//	omnibusClit = false;
		//	return true;
		//}

		internal float growClit(float amount, bool ignorePerks = false)
		{
			if (length >= MAX_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}

			//hope this never matters but floats don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			float oldLength = length;
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

		internal float shrinkClit(float amount, bool ignorePerks = false)
		{
			if (length <= MIN_CLIT_SIZE || amount <= 0)
			{
				return 0;
			}
			//hope this never matters but floats don't wrap. which means we're fine, though if it ever happens in debug land, we'll know.
			float oldLength = length;
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

		internal float SetClitSize(float newSize)
		{
			length = newSize;
			return length;
		}

		internal void DoPenetration()
		{
			penetrateCount++;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			length = length;
			return clitPiercings.Validate(correctInvalidData);
		}
		#region Text
		public static string PluralClitNoun() => ClitStrings.PluralClitNoun();
		public static string SingularClitNoun() => ClitStrings.ClitNoun(true);
		public static string ClitNoun() => ClitStrings.ClitNoun(false);
		public string ShortDescription() => ClitStrings.ShortDesc(length);
		public string LongDescription(bool alternateFormat) => ClitStrings.Desc(this, alternateFormat, false);
		public string FullDescription(bool alternateFormat) => ClitStrings.Desc(this, alternateFormat, true);
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

		public bool isPierced => clitPiercings.isPierced;

		public bool wearingJewelry => clitPiercings.wearingJewelry;


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

		float IGrowable.UseGroPlus()
		{
			if (!((IGrowable)this).CanGroPlus())
			{
				return 0;
			}
			float oldLength = length;
			length += 1;
			return length - oldLength;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			float oldLength = length;
			length /= 1.7f;
			return oldLength - length;
		}


		#endregion
	}

	public sealed partial class ClitData : SimpleData
	{
		public readonly float length;
		public readonly int vaginaIndex;

		public readonly ReadOnlyPiercing<ClitPiercingLocation> clitPiercings;

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

			clitPiercings = source.clitPiercings.AsReadOnlyData();
		}

		public ClitData(Guid creatureID, int currentIndex, float length, ReadOnlyPiercing<ClitPiercingLocation> piercings) : base(creatureID)
		{
			this.length = length;

			vaginaIndex = currentIndex;

			clitPiercings = piercings;
		}
	}
}
