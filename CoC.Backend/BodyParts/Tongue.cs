//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Materials;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public sealed partial class TonguePiercingLocation : PiercingLocation, IEquatable<TonguePiercingLocation>
	{
		private static readonly List<TonguePiercingLocation> _allLocations = new List<TonguePiercingLocation>();

		public static readonly ReadOnlyCollection<TonguePiercingLocation> allLocations;

		private readonly byte index;

		static TonguePiercingLocation()
		{
			allLocations = new ReadOnlyCollection<TonguePiercingLocation>(_allLocations);
		}

		public TonguePiercingLocation(byte index, CompatibleWith allowsJewelryOfType, SimpleDescriptor btnText, SimpleDescriptor locationDesc)
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
			if (obj is TonguePiercingLocation tonguePiercing)
			{
				return Equals(tonguePiercing);
			}
			else return false;
		}

		public bool Equals(TonguePiercingLocation other)
		{
			return !(other is null) && other.index == index;
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public static readonly TonguePiercingLocation FRONT = new TonguePiercingLocation(0, SupportedJewelry, FrontButton, FrontLocation);
		public static readonly TonguePiercingLocation MIDDLE = new TonguePiercingLocation(1, SupportedJewelry, MiddleButton, MiddleLocation);
		public static readonly TonguePiercingLocation BACK = new TonguePiercingLocation(2, SupportedJewelry, BackButton, BackLocation);

		//these are the piercings for standard gameplay - if you want to create a scene where it's a giant ring and the PC gets dragged around by it
		//because you're into German dungeon porn, that's fine. just do it via text and give them a stud afterward. or just pierce it and leave it open
		//so the next time your kinky scene is called you can omit the bit where you pierce their tongue or whatever.
		private static bool SupportedJewelry(JewelryType jewelryType)
		{
			return jewelryType == JewelryType.BARBELL_STUD;
		}
	}

	public sealed class TonguePiercing : Piercing<TonguePiercingLocation>
	{
		public TonguePiercing(IBodyPart source, PiercingUnlocked LocationUnlocked, GenericCreatureText playerShortDesc, GenericCreatureText playerLongDesc) : base(source, LocationUnlocked, playerShortDesc, playerLongDesc)
		{
		}

		public override int MaxPiercings => TonguePiercingLocation.allLocations.Count;

		public override IEnumerable<TonguePiercingLocation> availableLocations => TonguePiercingLocation.allLocations;
	}

	//ugh. i guess this fires on tongue length/width change? idk why, but whatever - overruled. i guess if you want to unlock some achievement when your tongue can cosplay
	//for KISS, you can now easily do that.
	public sealed partial class Tongue : FullBehavioralPart<Tongue, TongueType, TongueData> //ICanAttackWith ? if we make tongues able to bind somebody or something.
	{
		public override string BodyPartName() => Name();


		public readonly TonguePiercing piercings;

		public override TongueType type { get; protected set; }
		public override TongueType defaultType => TongueType.defaultValue;

		public bool isLongTongue => type.longTongue;
		public ushort length => type.length;
		public float width => type.width;

		public uint penetrateCount { get; private set; } = 0;
		public uint cullingusCount { get; private set; } = 0;

		public uint selfPenetrateCount { get; private set; } = 0;
		public uint selfCullingusCount { get; private set; } = 0;

		internal Tongue(Guid creatureID) : this(creatureID, TongueType.defaultValue)
		{ }

		internal Tongue(Guid creatureID, TongueType tongueType) : base(creatureID)
		{
			type = tongueType ?? throw new ArgumentNullException(nameof(tongueType));

			piercings = new TonguePiercing(this, TonguePiercingUnlocked, AllTonguePiercingsShort, AllTonguePiercingsLong);
		}

		public override TongueData AsReadOnlyData()
		{
			return new TongueData(this);
		}

		internal override bool UpdateType(TongueType newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}
			var oldType = type;
			TongueData oldData = null;
			if (length != newType.length || width != newType.width)
			{
				oldData = AsReadOnlyData();
			}
			type = newType;
			NotifyDataChanged(oldData);
			NotifyTypeChanged(oldType);
			return true;
		}

		internal void DoPenetrate(bool isSelf)
		{
			penetrateCount++;

			if (isSelf)
			{
				selfPenetrateCount++;
			}

		}

		internal void DoLicking(bool isSelf)
		{
			cullingusCount++;

			if (isSelf)
			{
				selfCullingusCount++;
			}
		}

		internal override bool Validate(bool correctInvalidData)
		{
			var tongueType = type;
			bool valid = TongueType.Validate(ref tongueType, correctInvalidData);
			type = tongueType;
			if (valid || correctInvalidData)
			{
				valid &= piercings.Validate(correctInvalidData);
			}
			return valid;
		}

		//could be a one-liner. written this way because maybe people wanna change it, idk.
		private bool TonguePiercingUnlocked(TonguePiercingLocation piercingLocation, out string whyNot)
		{
			if (piercings.piercingFetish)
			{
				whyNot = null;
				return true;
			}
			//allow one tongue piercing. must have fetish for more than that.
			else if (piercings.piercingCount > 0 && !piercings.isPiercedAt(piercingLocation))
			{
				whyNot = OnlyOneTonguePiercingWithoutFetish();
				return false;
			}

			else
			{
				whyNot = null;
				return true;
			}
		}

		//oral sex is stored in
		public override bool IsIdenticalTo(TongueData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type && piercings.IsIdenticalTo(original.tonguePiercings) &&
				(ignoreSexualMetaData || (cullingusCount == original.cullingusCount && penetrateCount == original.penetrateCount
				&& selfCullingusCount == original.selfCullingusCount && selfPenetrateCount == original.selfPenetrateCount));
		}

		internal void Reset()
		{
			Restore();
			piercings.Reset();
		}
	}

	public partial class TongueType : FullBehavior<TongueType, Tongue, TongueData>
	{
		private static int indexMaker = 0;
		private static readonly List<TongueType> tongues = new List<TongueType>();
		public static readonly ReadOnlyCollection<TongueType> availableTypes = new ReadOnlyCollection<TongueType>(tongues);
		private readonly int _index;
		public readonly ushort length;

		public readonly float width;

		public static TongueType defaultValue => HUMAN;


		private protected TongueType(ushort tongueLength, float tongueWidth, ShortDescriptor shortDesc, PartDescriptor<TongueData> longDesc,
			PlayerBodyPartDelegate<Tongue> playerDesc, ChangeType<TongueData> transform, RestoreType<TongueData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			tongues.AddAt(this, _index);
			length = tongueLength;
			width = tongueWidth;
		}

		public bool longTongue => length >= 12;
		public override int id => _index;

		internal static TongueType Deserialize(int index)
		{
			if (index < 0 || index >= tongues.Count)
			{
				throw new System.ArgumentException("index for tongue type deserialize out of range");
			}
			else
			{
				TongueType tongue = tongues[index];
				if (tongue != null)
				{
					return tongue;
				}
				else
				{
					throw new System.ArgumentException("index for tongue type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}
		internal static bool Validate(ref TongueType tongue, bool correctInvalidData)
		{
			if (tongues.Contains(tongue))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				tongue = HUMAN;
			}
			return false;
		}
		//these widths are arbitrary af, so feel free to alter them. i figured the snake tongue should be really narrow, and available for cock sounding if you really want that.
		//though i suppose with a large enough cock any of these would be viable. also the length is the distance out of the mouth it can travel. add like 4 inches for full length if
		//you're deepthroating or some shit, idk.
		public static readonly TongueType HUMAN = new TongueType(4, 2.5f, HumanDesc, HumanLongDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(6, 0.25f, SnakeDesc, SnakeLongDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(24, 2.5f, DemonicDesc, DemonicLongDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(48, 2.5f, DraconicDesc, DraconicLongDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(12, 1f, EchidnaDesc, EchidnaLongDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(12, 1f, LizardDesc, LizardLongDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(4, 2f, CatDesc, CatLongDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}

	public static class TongueHelpers
	{
		public static PiercingJewelry GenerateTongueJewelry(this Tongue tongue, TonguePiercingLocation location, JewelryMaterial jewelryMaterial)
		{
			return new GenericPiercing(JewelryType.BARBELL_STUD, jewelryMaterial);
		}
	}

	public sealed class TongueData : FullBehavioralData<TongueData, Tongue, TongueType>
	{
		//standard data
		public float width => type.width;
		public ushort length => type.length;
		public bool isLongTongue => type.longTongue;

		public readonly ReadOnlyPiercing<TonguePiercingLocation> tonguePiercings;

		//sexual metadata.
		public readonly uint penetrateCount;
		public readonly uint cullingusCount;

		public readonly uint selfPenetrateCount;
		public readonly uint selfCullingusCount;

		public override TongueData AsCurrentData()
		{
			return this;
		}

		internal TongueData(Tongue tongue) : base(GetID(tongue), GetBehavior(tongue))
		{
			tonguePiercings = tongue.piercings.AsReadOnlyData();

			penetrateCount = tongue.penetrateCount;
			cullingusCount = tongue.cullingusCount;

			selfPenetrateCount = tongue.selfPenetrateCount;
			selfCullingusCount = tongue.selfCullingusCount;
		}
	}
}
