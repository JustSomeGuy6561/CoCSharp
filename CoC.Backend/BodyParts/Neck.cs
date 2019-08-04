//Neck.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:12 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.CoC_Colors;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoC.Backend.BodyParts
{
	//NOTE: Removed a boolean (very adequately named pos - i can tell exactly what it does from that name.)
	//that was used to determine if the neck was positioned out of the bottom of the head or out the back. however, 
	//despite the fact that most lizards have their neck out the back of the their head (and most fish, for that matter)
	//they were never implemented. it seemed like a half-baked idea, so it's no longer here. if it ever becomes full-baked,
	//by all means, add it back.

	public sealed class Neck : BehavioralSaveablePart<Neck, NeckType>, IDyeable
	{
		public byte length { get; private set; } = NeckType.MIN_NECK_LENGTH;
		public override NeckType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					byte len = length;
					HairFurColors col = neckColor;
					value.convertNeck(ref len, ref col, _type);
					length = len;
					neckColor = col;
				}
				_type = value;
			}
		}
		private NeckType _type = NeckType.HUMANOID;
		public HairFurColors neckColor { get; private set; } = HairFurColors.NO_HAIR_FUR;

		public static NeckType defaultType => NeckType.HUMANOID;
		public override bool isDefault => type == defaultType;

		private Neck(NeckType neckType)
		{
			type = neckType ?? throw new ArgumentNullException(nameof(neckType));
			length = NeckType.MIN_NECK_LENGTH;
			neckColor = HairFurColors.NO_HAIR_FUR;
		}

		internal static Neck GenerateDefault()
		{
			return new Neck(NeckType.HUMANOID);
		}

		internal static Neck GenerateDefaultOfType(NeckType neckType)
		{
			return new Neck(neckType);
		}

		internal static Neck GenerateNonDefault(NeckType neckType, byte neckLength = NeckType.MIN_NECK_LENGTH)
		{
			if (neckType == NeckType.HUMANOID)
			{
				return new Neck(neckType);
			}
			return new Neck(neckType)
			{
				neckColor = neckType.defaultColor,
				length = Utils.Clamp2(neckLength, NeckType.MIN_NECK_LENGTH, neckType.maxNeckLength),
			};
		}


		internal override bool UpdateType(NeckType newType)
		{
			if (newType == null || type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}


		internal byte GrowNeck(byte amount)
		{
			if (canGrowNeck)
			{
				byte neckLength = length;
				byte retVal = type.StrengthenTransform(ref neckLength, amount);
				length = neckLength;
				return retVal;
			}
			return 0;
		}

		public bool canGrowNeck => type.canGrowNeck(length);

		internal override bool Restore()
		{
			if (type == NeckType.HUMANOID)
			{
				return false;
			}
			type = NeckType.HUMANOID;
			return true;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			byte len = length;
			HairFurColors col = neckColor;
			bool valid = NeckType.Validate(ref _type, ref len, ref col, correctInvalidData);
			length = len;
			neckColor = col;
			return valid;
		}


		bool IDyeable.allowsDye()
		{
			return type.canDye;
		}

		bool IDyeable.isDifferentColor(HairFurColors dyeColor)
		{
			return neckColor != dyeColor;
		}

		bool IDyeable.attemptToDye(HairFurColors dye)
		{
			if (((IDyeable)this).allowsDye())
			{
				HairFurColors color = neckColor;
				bool retVal = type.ParseDye(ref color, dye);
				neckColor = color;
				return retVal;
			}
			return false;
		}

		string IDyeable.buttonText()
		{
			return type.buttonText();
		}

		string IDyeable.locationDesc()
		{
			return type.locationDesc();
		}
	}

	public partial class NeckType : SaveableBehavior<NeckType, Neck>
	{
		public const byte MIN_NECK_LENGTH = 2;

		protected const int MAX_HUMAN_LENGTH = 2;
		protected const int MAX_DRAGON_LENGTH = 30;
		protected const int MAX_COCKATRICE_LENGTH = 2;

		private static int indexMaker = 0;
		private static readonly List<NeckType> necks = new List<NeckType>();
		public static readonly ReadOnlyCollection<NeckType> availableTypes = new ReadOnlyCollection<NeckType>(necks);
		private readonly int _index;

		public readonly byte maxNeckLength;
		public readonly HairFurColors defaultColor;

		public readonly bool canDye;
		public bool usesHair => canDye;

		internal virtual SimpleDescriptor buttonText => GenericButtonDesc;
		internal virtual SimpleDescriptor locationDesc => GenericLocationText;

		private protected NeckType(byte maxLength,
			SimpleDescriptor shortDesc, DescriptorWithArg<Neck> fullDesc, TypeAndPlayerDelegate<Neck> playerDesc, ChangeType<Neck> transform,
			RestoreType<Neck> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			maxNeckLength = maxLength;
			canDye = false;
			defaultColor = HairFurColors.NO_HAIR_FUR;
			necks.AddAt(this, _index);
		}

		private protected NeckType(byte maxLength, HairFurColors defaultHairFur,
			SimpleDescriptor shortDesc, DescriptorWithArg<Neck> fullDesc, TypeAndPlayerDelegate<Neck> playerDesc, ChangeType<Neck> transform,
			RestoreType<Neck> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			maxNeckLength = maxLength;
			defaultColor = defaultHairFur;
			canDye = defaultColor != HairFurColors.NO_HAIR_FUR;
			necks.AddAt(this, _index);
		}

		public override int index => _index;

		internal virtual bool canGrowNeck(int currNeckLength)
		{
			return currNeckLength < maxNeckLength;
		}

		internal virtual byte StrengthenTransform(ref byte neckLength, byte level)
		{
			if (neckLength >= maxNeckLength || level <= 0)
			{
				return 0;
			}
			byte oldLength = neckLength;
			neckLength += level;
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
		internal virtual void convertNeck(ref byte length, ref HairFurColors color, NeckType oldType)
		{
			if (maxNeckLength == MIN_NECK_LENGTH)
			{
				length = MIN_NECK_LENGTH;
			}
			else
			{
				length = Utils.LerpRound(MIN_NECK_LENGTH, oldType.maxNeckLength, length, MIN_NECK_LENGTH, maxNeckLength);
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
				Utils.Clamp(ref len, MIN_NECK_LENGTH, maxNeckLength);
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

		public static readonly NeckType HUMANOID = new NeckType(MAX_HUMAN_LENGTH, HumanDesc, HumanFullDesc, HumanPlayerStr, GlobalStrings.TransformToDefault<Neck, NeckType>, GlobalStrings.RevertAsDefault);
		public static readonly NeckType DRACONIC = new NeckType(MAX_DRAGON_LENGTH, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly NeckType COCKATRICE = new NeckType(MAX_COCKATRICE_LENGTH, HairFurColors.GREEN, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);
	}
}
