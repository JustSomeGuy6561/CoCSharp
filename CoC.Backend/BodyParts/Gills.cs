//Gills.cs
//Description:
//Author: JustSomeGuy
//12/27/2018, 7:29 PM
using CoC.Backend.Creatures;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using CoC.
namespace CoC.Backend.BodyParts
{
	//note: i suppose gills could be considered plural, so it could make use of the short description variant with plural flag, but i can't really think of a reason you would need
	//a verbose description and only one gill. feel free to add it if needed.

	//has no special data. its data change will never be called.
	public sealed partial class Gills : FullBehavioralPart<Gills, GillType, GillData>
	{
		public override string BodyPartName() => Name();

		internal Gills(Guid creatureID, GillType gillType) : base(creatureID)
		{
			type = gillType ?? throw new ArgumentNullException(nameof(gillType));
		}

		internal Gills(Guid creatureID) : this(creatureID, GillType.defaultValue)
		{ }

		public override GillType type { get; protected set; }

		public override GillType defaultType => GillType.defaultValue;

		public override GillData AsReadOnlyData()
		{
			return new GillData(this);
		}


		//Update, Restore both fine as defaults.

		internal override bool Validate(bool correctInvalidData)
		{
			GillType gillType = type;
			bool valid = GillType.Validate(ref gillType, correctInvalidData);
			type = gillType;
			return valid;
		}

		public override bool IsIdenticalTo(GillData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type;
		}
	}

	public partial class GillType : FullBehavior<GillType, Gills, GillData>
	{
		private static int indexMaker = 0;
		public static GillType defaultValue => NONE;

		private static readonly List<GillType> gills = new List<GillType>();
		public static readonly ReadOnlyCollection<GillType> availableTypes = new ReadOnlyCollection<GillType>(gills);

		ShortPluralDescriptor pluralDesc;

		public string ShortDescription(bool plural) => pluralDesc(plural);

		protected GillType(ShortPluralDescriptor shortDesc, SimpleDescriptor singleDesc, PartDescriptor<GillData> longDesc, PlayerBodyPartDelegate<Gills> playerDesc,
			ChangeType<GillData> transform, RestoreType<GillData> restore) : base(PluralHelper(shortDesc), singleDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			gills.AddAt(this, _index);

			pluralDesc = shortDesc;
		}

		internal static GillType Deserialize(int index)
		{
			if (index < 0 || index >= gills.Count)
			{
				throw new System.ArgumentException("index for body type deserialize out of range");
			}
			else
			{
				GillType gill = gills[index];
				if (gill != null)
				{
					return gill;
				}
				else
				{
					throw new System.ArgumentException("index for gill type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}
		internal static bool Validate(ref GillType gillType, bool correctInvalidData)
		{
			if (gills.Contains(gillType))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				gillType = NONE;
			}
			return false;
		}
		protected readonly int _index;
		public override int id => _index;

		public static readonly GillType NONE = new GillType(NoneDesc, NoneSingleDesc, NoneLongDesc, NonePlayerStr, NoneTransformStr, NoneRestoreStr);



		public static readonly GillType ANEMONE = new GillType(AnemoneDesc, AnemoneSingleDesc, AnemoneLongDesc, AnemonePlayerStr, AnemoneTransformStr, AnemoneRestoreStr);
		public static readonly GillType FISH = new GillType(FishDesc, FishSingleDesc, FishLongDesc, FishPlayerStr, FishTransformStr, FishRestoreStr);
	}

	public sealed class GillData : FullBehavioralData<GillData, Gills, GillType>
	{
		public override GillData AsCurrentData()
		{
			return this;
		}

		internal GillData(Gills source) : base(GetID(source), GetBehavior(source)) { }
	}
}
