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
	public enum TonguePiercingLocation { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }

	//ugh. i guess this fires on tongue length/width change? idk why, but whatever - overruled. i guess if you want to unlock some achievement when your tongue can cosplay
	//for KISS, you can now easily do that. 
	public sealed partial class Tongue : BehavioralSaveablePart<Tongue, TongueType, TongueData> //ICanAttackWith ? if we make tongues able to bind somebody or something. 
	{
		public override string BodyPartName() => Name();

		public const JewelryType TongueJewelry = JewelryType.BARBELL_STUD;
		private const JewelryType SUPPORTED_LIP_JEWELRY = JewelryType.HORSESHOE | JewelryType.BARBELL_STUD | JewelryType.RING | JewelryType.SPECIAL;

		public readonly Piercing<TonguePiercingLocation> tonguePiercings;
		public readonly Piercing<LipPiercingLocation> lipPiercings;

		public override TongueType type { get; protected set; }
		public override TongueType defaultType => TongueType.defaultValue;

		public bool isLongTongue => type.longTongue;
		public ushort length => type.length;
		public float width => type.width;

		public uint penetrateCount { get; private set; } = 0;
		public uint cullingusCount { get; private set; } = 0;


		internal Tongue(Guid creatureID) : this(creatureID, TongueType.defaultValue)
		{ }

		internal Tongue(Guid creatureID, TongueType tongueType) : base(creatureID)
		{
			type = tongueType ?? throw new ArgumentNullException(nameof(tongueType));

			tonguePiercings = new Piercing<TonguePiercingLocation>(TonguePiercingUnlocked, TongueSupportedJewelry);
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

		internal void DoPenetrate()
		{
			penetrateCount++;
		}

		internal void DoLicking()
		{
			cullingusCount++;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			var tongueType = type;
			bool valid = TongueType.Validate(ref tongueType, correctInvalidData);
			type = tongueType;
			if (valid || correctInvalidData)
			{
				valid &= tonguePiercings.Validate(correctInvalidData);
			}
			return valid;
		}

		//could be a one-liner. written this way because maybe people wanna change it, idk.
		private bool TonguePiercingUnlocked(TonguePiercingLocation piercingLocation)
		{
			if (tonguePiercings.piercingFetish)
			{
				return true;
			}
			//allow one tongue piercing. must have fetish for more than that.
			else if (tonguePiercings.piercingCount > 0 && !tonguePiercings.isPiercedAt(piercingLocation))
			{
				return false;
			}
			else return true;
		}

		//these are the piercings for standard gameplay - if you want to create a scene where it's a giant ring and the PC gets dragged around by it
		//because you're into German dungeon porn, that's fine. just do it via text and give them a stud afterward. or just pierce it and leave it open
		//so the next time your kinky scene is called you can omit the bit where you pierce their tongue or whatever. 
		private JewelryType TongueSupportedJewelry(TonguePiercingLocation piercingLocation)
		{
			return JewelryType.BARBELL_STUD;
		}

		internal void Reset()
		{
			Restore();
			tonguePiercings.Reset();
		}
	}

	public partial class TongueType : SaveableBehavior<TongueType, Tongue, TongueData>
	{
		private static int indexMaker = 0;
		private static readonly List<TongueType> tongues = new List<TongueType>();
		public static readonly ReadOnlyCollection<TongueType> availableTypes = new ReadOnlyCollection<TongueType>(tongues);
		private readonly int _index;
		public readonly ushort length;

		public readonly float width;

		public static TongueType defaultValue => HUMAN;


		private protected TongueType(ushort tongueLength, float tongueWidth, SimpleDescriptor shortDesc, DescriptorWithArg<Tongue> fullDesc, TypeAndPlayerDelegate<Tongue> playerDesc, ChangeType<Tongue> transform, RestoreType<Tongue> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			tongues.AddAt(this, _index);
			length = tongueLength;
			width = tongueWidth;
		}

		public bool longTongue => length >= 12;
		public override int index => _index;

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
		public static readonly TongueType HUMAN = new TongueType(4, 2.5f, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(6, 0.25f, SnakeDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(24, 2.5f, DemonicDesc, DemonicFullDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(48, 2.5f, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(12, 1f, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(12, 1f, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(4, 2f, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}

	public static class TongueHelpers
	{
		public static PiercingJewelry GenerateTongueJewelry(this Tongue tongue, TonguePiercingLocation location, JewelryMaterial jewelryMaterial)
		{
			return new GenericPiercing(JewelryType.BARBELL_STUD, jewelryMaterial);
		}
	}

	public sealed class TongueData : BehavioralSaveablePartData<TongueData, Tongue, TongueType>
	{
		public float width => currentType.width;
		public ushort length => currentType.length;
		public bool isLongTongue => currentType.longTongue;
		internal TongueData(Tongue tongue) : base(GetID(tongue), GetBehavior(tongue))
		{

		}
	}
}
