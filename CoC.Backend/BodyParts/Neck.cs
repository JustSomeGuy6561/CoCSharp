//Neck.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:12 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	public sealed partial class NeckTattooLocation : TattooLocation
	{

		private static readonly List<NeckTattooLocation> _allLocations = new List<NeckTattooLocation>();

		public static readonly ReadOnlyCollection<NeckTattooLocation> allLocations;

		private readonly byte index;

		static NeckTattooLocation()
		{
			allLocations = new ReadOnlyCollection<NeckTattooLocation>(_allLocations);
		}

		private NeckTattooLocation(byte index, TattooSizeLimit limitSize, SimpleDescriptor btnText, SimpleDescriptor locationDesc) : base(limitSize, btnText, locationDesc)
		{
			this.index = index;
		}

		public static NeckTattooLocation FRONT = new NeckTattooLocation(0, MediumTattoosOrSmaller, FrontButton, FrontLocation);
		public static NeckTattooLocation BACK = new NeckTattooLocation(1, MediumTattoosOrSmaller, BackButton, BackLocation);
		public static NeckTattooLocation LEFT = new NeckTattooLocation(2, MediumTattoosOrSmaller, LeftButton, LeftLocation);
		public static NeckTattooLocation RIGHT = new NeckTattooLocation(3, MediumTattoosOrSmaller, RightButton, RightLocation);

		public static bool LocationsCompatible(NeckTattooLocation first, NeckTattooLocation second)
		{
			return true;
		}
	}

	public sealed class NeckTattoo : TattooablePart<NeckTattooLocation>
	{
		public NeckTattoo(IBodyPart source, GenericCreatureText allTattoosShort, GenericCreatureText allTattoosLong) : base(source, allTattoosShort, allTattoosLong)
		{
		}

		public override int MaxTattoos => NeckTattooLocation.allLocations.Count;

		public override IEnumerable<NeckTattooLocation> availableLocations => NeckTattooLocation.allLocations;

		public override bool LocationsCompatible(NeckTattooLocation first, NeckTattooLocation second) => NeckTattooLocation.LocationsCompatible(first, second);
	}

	//NOTE: Removed a boolean (very adequately named pos - i can tell exactly what it does from that name.)
	//that was used to determine if the neck was positioned out of the bottom of the head or out the back. however,
	//despite the fact that most lizards have their neck out the back of the their head (and most fish, for that matter)
	//they were never implemented. it seemed like a half-baked idea, so it's no longer here. if it ever becomes full-baked,
	//by all means, add it back.
	public sealed partial class Neck : FullBehavioralPart<Neck, NeckType, NeckData>, IDyeable
	{
		public override string BodyPartName() => Name();

		public byte length { get; private set; } = NeckType.MIN_NECK_LENGTH;

		public bool neckAtBaseOfSkull => type.neckAtBaseOfSkull; //(relative to humans, of course)

		private HairFurColors hairColor => CreatureStore.TryGetCreature(creatureID, out Creature creature) ? creature.hair.hairColor : Hair.DEFAULT_COLOR;

		public override NeckType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					byte len = length;
					HairFurColors col = color;
					value.convertNeck(ref len, ref col, hairColor, _type);
					length = len;
					color = col;
				}
				_type = value;
			}
		}
		private NeckType _type = NeckType.defaultValue;
		public HairFurColors color { get; private set; } = HairFurColors.NO_HAIR_FUR;

		public override NeckType defaultType => NeckType.defaultValue;

		public readonly NeckTattoo tattoos;

		public override NeckData AsReadOnlyData()
		{
			return new NeckData(this);
		}

		internal Neck(Guid creatureID) : this(creatureID, NeckType.defaultValue)
		{ }
		internal Neck(Guid creatureID, NeckType neckType) : base(creatureID)
		{
			type = neckType ?? throw new ArgumentNullException(nameof(neckType));

			tattoos = new NeckTattoo(this, AllTattoosShort, AllTattoosLong);
		}

		internal Neck(Guid creatureID, NeckType neckType, HairFurColors initialNeckColor = null, byte neckLength = NeckType.MIN_NECK_LENGTH) : this(creatureID, neckType)
		{
			GrowNeckInternal(neckLength.subtract(NeckType.MIN_NECK_LENGTH), true); //add in the remaining amount.
			var neckCol = color;
			type.ParseDye(ref neckCol, initialNeckColor);
			color = neckCol;
		}

		internal override bool UpdateType(NeckType newType)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			var oldData = AsReadOnlyData();

			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldValue);
			return true;
		}

		internal bool UpdateTypeWithAddedLength(NeckType newType, byte additionalLength)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			var oldData = AsReadOnlyData();

			type = newType;

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldValue);
			return true;
		}

		internal bool UpdateType(CockatriceNeck newType, HairFurColors featherColor)
		{
			if (newType is null || newType == type)
			{
				return false;
			}

			var oldValue = type;
			var oldData = AsReadOnlyData();

			type = newType;
			if (!HairFurColors.IsNullOrEmpty(featherColor))
			{
				color = featherColor;
			}

			CheckDataChanged(oldData);
			NotifyTypeChanged(oldValue);
			return true;
		}

		private void CheckDataChanged(NeckData oldData)
		{
			if (color != oldData.neckHairColor || length != oldData.length)
			{
				NotifyDataChanged(oldData);
			}
		}

		public byte GrowNeck(byte amount)
		{
			return GrowNeckInternal(amount, false);
		}

		private byte GrowNeckInternal(byte amount, bool silent)
		{
			if (canGrowNeck)
			{
				byte neckLength = length;
				byte retVal = type.StrengthenTransform(ref neckLength, amount);

				if (length != neckLength)
				{
					NeckData oldData = null;
					if (!silent)
					{
						oldData = AsReadOnlyData();
					}
					length = neckLength;
					if (oldData != null)
					{
						NotifyDataChanged(oldData);
					}

				}
				return retVal;
			}
			return 0;
		}

		public bool canGrowNeck => type.canGrowNeck(length);


		internal override bool Validate(bool correctInvalidData)
		{
			byte len = length;
			HairFurColors col = color;
			bool valid = NeckType.Validate(ref _type, ref len, ref col, correctInvalidData);
			length = len;
			color = col;
			return valid;
		}

		public override bool IsIdenticalTo(NeckData original, bool ignoreSexualMetaData)
		{
			return !(original is null) && type == original.type && (!type.usesHair || this.color == original.neckHairColor) && length == original.length &&
				tattoos.IsIdenticalTo(original.tattoos);
		}

		bool IDyeable.allowsDye()
		{
			return type.canDye;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor)
		{
			return color != dyeColor;
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (((IDyeable)this).allowsDye())
			{
				HairFurColors col = color;
				bool retVal = type.ParseDye(ref col, dye);
				color = col;
				return retVal;
			}
			return false;
		}

		string IDyeable.buttonText()
		{
			return type.buttonText();
		}

		string IDyeable.locationDesc(out bool isPlural)
		{
			return type.locationDesc(out isPlural);
		}

		string IDyeable.postDyeDescription()
		{
			return type.postDyeDescription(color);
		}
	}

	public partial class NeckType : FullBehavior<NeckType, Neck, NeckData>
	{
		public const byte MIN_NECK_LENGTH = 2;

		protected const int MAX_HUMAN_LENGTH = 2;
		protected const int MAX_DRAGON_LENGTH = 30;
		protected const int MAX_COCKATRICE_LENGTH = 5;

		private static int indexMaker = 0;
		private static readonly List<NeckType> necks = new List<NeckType>();
		public static readonly ReadOnlyCollection<NeckType> availableTypes = new ReadOnlyCollection<NeckType>(necks);
		private readonly int _index;

		public readonly byte maxNeckLength;
		public virtual HairFurColors defaultColor => HairFurColors.NO_HAIR_FUR;

		public bool canDye => !defaultColor.isEmpty && allowsDye;

		private protected virtual bool allowsDye => true;

		public virtual byte minNeckLength => MIN_NECK_LENGTH;

		public static NeckType defaultValue => HUMANOID;


		public bool usesHair => canDye;

		internal virtual string buttonText() => GenericButtonDesc();
		internal virtual string locationDesc(out bool isPlural) => GenericLocationText(out isPlural);

		internal virtual string postDyeDescription(HairFurColors color) => GenericPostDyeText(color);

		private protected NeckType(byte maxLength,
			ShortDescriptor shortDesc, PartDescriptor<NeckData> longDesc, PlayerBodyPartDelegate<Neck> playerDesc, ChangeType<NeckData> transform,
			RestoreType<NeckData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			maxNeckLength = maxLength;
			necks.AddAt(this, _index);
		}

		//private protected NeckType(byte minLength, byte maxLength, HairFurColors defaultHairFur,
		//	SimpleDescriptor shortDesc, DescriptorWithArg<NeckData> longDesc, PlayerBodyPartDelegate<Neck> playerDesc, ChangeType<NeckData> transform,
		//	RestoreType<NeckData> restore) : base(shortDesc, longDesc, playerDesc, transform, restore)
		//{
		//	_index = indexMaker++;
		//	maxNeckLength = maxLength;
		//	defaultColor = defaultHairFur;
		//	canDye = defaultColor != HairFurColors.NO_HAIR_FUR;
		//	necks.AddAt(this, _index);
		//}

		public override int id => _index;

		internal virtual bool canGrowNeck(int currNeckLength)
		{
			return currNeckLength < maxNeckLength;
		}

		public virtual bool neckAtBaseOfSkull => true;

		internal virtual byte StrengthenTransform(ref byte neckLength, byte amount)
		{
			if (neckLength >= maxNeckLength || amount == 0)
			{
				return 0;
			}
			byte oldLength = neckLength;
			neckLength += amount;
			Utils.Clamp(ref neckLength, MIN_NECK_LENGTH, maxNeckLength);
			return neckLength.subtract(oldLength);
		}

		internal virtual bool ParseDye(ref HairFurColors current, HairFurColors dye)
		{
			if (!canDye)
			{
				return false;
			}
			current = dye;
			return true;
		}


		//this uses a percent approach. if you want just a flat value, override this.

		/// <summary>
		/// Updates the neck length, by determining how close it was to the old max,
		/// and updating the length to match according to this type's max.
		/// </summary>
		/// <param name="length">the current neck length</param>
		/// <param name="oldMaxLength"></param>
		internal virtual void convertNeck(ref byte length, ref HairFurColors color, in HairFurColors currentHairColor, NeckType oldType)
		{
			if (maxNeckLength == minNeckLength)
			{
				length = minNeckLength;
			}
			else
			{
				length = Utils.LerpRound(oldType.minNeckLength, oldType.maxNeckLength, length, minNeckLength, maxNeckLength);
			}

			//current behavior: we always use new default hair - we never save the old color. in the event this behavior gets changed
			//as more necktypes are added, provide a means to determine that behavior and implement it here. or override this i guess.
			color = defaultColor;
		}

		internal static bool Validate(ref NeckType neckType, ref byte length, ref HairFurColors color, bool correctInvalidData)
		{
			if (!necks.Contains(neckType))
			{
				if (correctInvalidData)
				{
					neckType = HUMANOID;
					length = MIN_NECK_LENGTH;
					color = HairFurColors.NO_HAIR_FUR;
				}
				return false;
			}
			return neckType.ValidateData(ref length, ref color, correctInvalidData);
		}

		internal virtual bool ValidateData(ref byte length, ref HairFurColors color, bool correctInvalidData)
		{
			bool valid = true;
			if (!usesHair && !color.isEmpty)
			{
				if (correctInvalidData)
				{
					color = HairFurColors.NO_HAIR_FUR;
				}
				valid = false;
			}
			if (valid || correctInvalidData)
			{
				byte len = length;
				Utils.Clamp(ref len, minNeckLength, maxNeckLength);
				if (length != len)
				{
					if (correctInvalidData)
					{
						length = len;
					}
					valid = false;
				}
			}
			return valid;
		}

		public static readonly NeckType HUMANOID = new NeckType(MAX_HUMAN_LENGTH, HumanDesc, HumanLongDesc, HumanPlayerStr, (x, y) => x.type.RestoredString(x, y), GlobalStrings.RevertAsDefault);
		public static readonly NeckType DRACONIC = new DraconicNeck();
		public static readonly CockatriceNeck COCKATRICE = new CockatriceNeck();

		//cockatrice does not grow, but is slightly larger than human neck (5inches). before, it was 2-5, with no way to grow.


		private sealed class DraconicNeck : NeckType
		{
			public DraconicNeck() : base(MAX_DRAGON_LENGTH, DragonDesc, DragonLongDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr)
			{
			}

			public override bool neckAtBaseOfSkull => false;
		}
	}

	public sealed class CockatriceNeck : NeckType
	{
		public CockatriceNeck() : base(MAX_COCKATRICE_LENGTH, CockatriceDesc, CockatriceLongDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr)
		{
		}

		public override HairFurColors defaultColor => HairFurColors.GREEN;

		public override byte minNeckLength => MAX_COCKATRICE_LENGTH;

		internal override string locationDesc(out bool isPlural)
		{
			return CockatriceDyeText(out isPlural);
		}

		internal override string postDyeDescription(HairFurColors color)
		{
			return CockatricePostDyeText(color);
		}

	}

	public sealed class NeckData : FullBehavioralData<NeckData, Neck, NeckType>
	{
		public readonly byte length;
		public readonly HairFurColors neckHairColor; //if applicable

		public bool neckAtBaseOfSkull => type.neckAtBaseOfSkull; //(relative to humans, of course)

		public readonly ReadOnlyTattooablePart<NeckTattooLocation> tattoos;

		public override NeckData AsCurrentData()
		{
			return this;
		}

		internal NeckData(Neck neck) : base(GetID(neck), GetBehavior(neck))
		{
			length = neck.length;
			neckHairColor = neck.color;

			tattoos = neck.tattoos.AsReadOnlyData();
		}
	}
}
