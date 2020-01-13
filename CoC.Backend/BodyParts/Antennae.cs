//Antennae.cs
//Description: Contains the Antennae and AntennaeType classes, a body part that helps make up creature.
//Author: JustSomeGuy
//12/30/2018, 10:08 PM

using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//Note: Never fires a data change event, as it has no data that can be changed.

	public sealed partial class Antennae : BehavioralSaveablePart<Antennae, AntennaeType, AntennaeData>
	{
		public override string BodyPartName() => Name();

		public override AntennaeType type { get; protected set; }

		public override AntennaeType defaultType => AntennaeType.defaultValue;

		public bool hasAntennae => type != AntennaeType.NONE;

		internal Antennae(Guid creatureID) : this(creatureID, AntennaeType.defaultValue)
		{ }

		internal Antennae(Guid creatureID, AntennaeType antennaeType) : base(creatureID)
		{
			type = antennaeType ?? throw new ArgumentNullException(nameof(antennaeType));
		}

		//default implementations of update and restore are valid.

		//description overloads.
		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(AsReadOnlyData(), alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(AsReadOnlyData(), plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(AsReadOnlyData(), plural);

		internal override bool Validate(bool correctInvalidData)
		{
			AntennaeType antennae = type;
			bool retVal = AntennaeType.Validate(ref antennae, correctInvalidData);
			type = antennae;
			return retVal;
		}

		public override AntennaeData AsReadOnlyData()
		{
			return new AntennaeData(this);
		}
	}

	public sealed partial class AntennaeType : SaveableBehavior<AntennaeType, Antennae, AntennaeData>
	{
		private static int indexMaker = 0;
		private static readonly List<AntennaeType> antennaes = new List<AntennaeType>();
		public static readonly ReadOnlyCollection<AntennaeType> availableTypes = new ReadOnlyCollection<AntennaeType>(antennaes);

		public static AntennaeType defaultValue => AntennaeType.NONE;



		//C# 7.2 magic. basically, prevents it from being messed with except internally.
		private AntennaeType(ShortPluralDescriptor desc, SimpleDescriptor singleItemDesc, PluralPartDescriptor<AntennaeData> longDesc,
			PlayerBodyPartDelegate<Antennae> playerDesc, ChangeType<AntennaeData> transformMessage, RestoreType<AntennaeData> revertToDefault)
			: base(PluralHelper(desc), singleItemDesc, LongPluralHelper(longDesc), playerDesc, transformMessage, revertToDefault)
		{
			shortPluralDesc = desc ?? throw new ArgumentNullException(nameof(desc));
			longPluralDesc = longDesc ?? throw new ArgumentNullException(nameof(longDesc));

			_index = indexMaker++;
			antennaes.AddAt(this, _index);
		}

		private readonly ShortPluralDescriptor shortPluralDesc;
		private readonly PluralPartDescriptor<AntennaeData> longPluralDesc;

		public string ShortDescription(bool plural) => shortPluralDesc(plural);

		public string LongDescription(AntennaeData data, bool alternateFormat, bool plural) => longPluralDesc(data, alternateFormat, plural);

		public string LongDescriptionPrimary(AntennaeData data, bool plural) => longPluralDesc(data, false, plural);

		public string LongDescriptionAlternate(AntennaeData data, bool plural) => longPluralDesc(data, true, plural);

		public override int id => _index;
		private readonly int _index;

		internal static AntennaeType Deserialize(int index)
		{
			if (index < 0 || index >= antennaes.Count)
			{
				throw new ArgumentException("index for antennae type desrialize out of range");
			}
			else
			{
				AntennaeType antennae = antennaes[index];
				if (antennae != null)
				{
					return antennae;
				}
				else
				{
					throw new ArgumentException("index for antennae type points to an object that does not exist. this may be due to obsolete code");
				}
			}
		}

		internal static bool Validate(ref AntennaeType antennae, bool correctInvalidData)
		{
			if (antennaes.Contains(antennae))
			{
				return true;
			}
			else if (correctInvalidData)
			{
				antennae = NONE;
			}
			return false;
		}

		public static readonly AntennaeType NONE = new AntennaeType(NoneDesc, NoneSingleDesc, NoneLongdesc, NonePlayerStr, RemoveAntennaeStr, NoneRestoreStr);

		public static readonly AntennaeType BEE = new AntennaeType(BeeDesc, BeeSingleDesc, BeeLongDesc,
			(x, y) => BeePlayerStr(y), BeeTransformStr, BeeRestoreStr);

		public static readonly AntennaeType COCKATRICE = new AntennaeType(CockatriceDesc, CockatriceSingleDesc, CockatriceLongDesc,
			(x, y) => CockatricePlayerStr(y), CockatriceTransformStr, CockatriceRestoreStr);
	}

	public sealed class AntennaeData : BehavioralSaveablePartData<AntennaeData, Antennae, AntennaeType>
	{

		public override AntennaeData AsCurrentData()
		{
			return this;
		}

		public string ShortDescription(bool plural) => type.ShortDescription(plural);

		public string LongDescription(bool alternateFormat, bool plural) => type.LongDescription(this, alternateFormat, plural);

		public string LongDescriptionPrimary(bool plural) => type.LongDescriptionPrimary(this, plural);

		public string LongDescriptionAlternate(bool plural) => type.LongDescriptionAlternate(this, plural);

		internal AntennaeData(Antennae source) : base(GetID(source), GetBehavior(source))
		{ }
	}

}

