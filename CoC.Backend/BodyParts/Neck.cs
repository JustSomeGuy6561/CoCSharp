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

		public override bool isDefault => type == NeckType.HUMANOID;

		private Neck()
		{
			type = NeckType.HUMANOID;
			length = NeckType.MIN_NECK_LENGTH;
			neckColor = HairFurColors.NO_HAIR_FUR;
		}

		internal static Neck GenerateDefault()
		{
			return new Neck();
		}

		internal static Neck GenerateDefaultOfType(NeckType neckType)
		{
			return new Neck()
			{
				type = neckType,
			};
		}

		internal static Neck GenerateNonDefault(NeckType neckType, byte neckLength = NeckType.MIN_NECK_LENGTH)
		{
			if (neckType == NeckType.HUMANOID)
			{
				return new Neck();
			}
			Utils.Clamp(ref neckLength, NeckType.MIN_NECK_LENGTH, neckType.maxNeckLength);
			return new Neck()
			{
				type = neckType,
				neckColor = neckType.defaultColor,
				length = neckLength
			};
		}

		internal override bool Restore()
		{
			if (type == NeckType.HUMANOID)
			{
				return false;
			}
			type = NeckType.HUMANOID;
			return true;
		}

		internal bool UpdateNeck(NeckType neckType)
		{
			if (type == neckType)
			{
				return false;
			}
			type = neckType;
			return true;
		}


		internal byte GrowNeck(byte amount)
		{
			if (canGrowNeck())
			{
				byte neckLength = length;
				byte retVal = type.StrengthenTransform(ref neckLength, amount);
				length = neckLength;
				return retVal;
			}
			return 0;
		}

		public bool canGrowNeck()
		{
			return type.canGrowNeck(length);
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			byte len = length;
			HairFurColors col = neckColor;
			bool valid = NeckType.Validate(ref _type, ref len, ref col, correctDataIfInvalid);
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
	}

	public partial class NeckType : SaveableBehavior<NeckType, Neck>
	{
		private static int indexMaker = 0;
		private static readonly List<NeckType> necks = new List<NeckType>();
		public const byte MIN_NECK_LENGTH = 2;
		public readonly int _index;

		public readonly byte maxNeckLength;
		public readonly HairFurColors defaultColor;

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

		public readonly bool canDye;
		public bool usesHair => canDye;

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
				double percent = percentTowardsMaxLength(length, oldType.maxNeckLength);
				length = (byte)Math.Round(MIN_NECK_LENGTH + (maxNeckLength - MIN_NECK_LENGTH) * percent);
			}

			//current behavior: we always use new default hair - we never save the old color. in the event this behavior gets changed 
			//as more necktypes are added, provide a means to determine that behavior and implement it here. or override this i guess.
			color = defaultColor;
		}

		internal static bool Validate(ref NeckType neckType, ref byte length, ref HairFurColors color, bool correctInvalidData = false)
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

		internal virtual bool ValidateData(ref byte length, ref HairFurColors color, bool correctInvalidData = false)
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

		protected const int MAX_HUMAN_LENGTH = 2;
		protected const int MAX_DRAGON_LENGTH = 30;
		protected const int MAX_COCKATRICE_LENGTH = 2;

		public static readonly NeckType HUMANOID = new NeckType(MAX_HUMAN_LENGTH, HumanDesc, HumanFullDesc, HumanPlayerStr, GlobalStrings.TransformToDefault<Neck, NeckType>, GlobalStrings.RevertAsDefault);
		public static readonly NeckType DRACONIC = new NeckType(MAX_DRAGON_LENGTH, DragonDesc, DragonFullDesc, DragonPlayerStr, DragonTransformStr, DragonRestoreStr);
		public static readonly NeckType COCKATRICE = new NeckType(MAX_COCKATRICE_LENGTH, HairFurColors.GREEN, CockatriceDesc, CockatriceFullDesc, CockatricePlayerStr, CockatriceTransformStr, CockatriceRestoreStr);

		private double percentTowardsMaxLength(int length, int max)
		{
			//prevents divide by zero errors.
			if (length <= MIN_NECK_LENGTH || MIN_NECK_LENGTH == max)
			{
				return 0;
			}
			else if (length >= max)
			{
				return 1;
			}
			else
			{
				return (max - length * 1.0) / (max - MIN_NECK_LENGTH);
			}
		}
	}
}
