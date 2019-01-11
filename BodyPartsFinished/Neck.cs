//Neck.cs
//Description:
//Author: JustSomeGuy
//12/29/2018, 10:12 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.BodyParts.SpecialInteraction;
using CoC.EpidermalColors;
using CoC.Strings;
using CoC.Tools;
using static CoC.Strings.BodyParts.NeckStrings;
using static CoC.UI.TextOutput;
namespace CoC.BodyParts
{
	public class Neck : BodyPartBase<Neck, NeckType>, IDyeable
	{
		public int length { get; protected set; }
		public override NeckType type
		{
			get => _type;
			protected set
			{
				if (value != _type)
				{
					neckColor = value.defaultColor;
				}
				_type = value;
			}
		}
		private NeckType _type;
		public HairFurColors neckColor { get; protected set; }

		protected Neck()
		{
			type = NeckType.HUMANOID;
			length = NeckType.MIN_NECK_LENGTH;
			neckColor = HairFurColors.NO_HAIR_FUR;
		}

		public static Neck Generate()
		{
			return new Neck();
		}

		public static Neck GenerateNonDefault(NeckType neckType, int neckLength = NeckType.MIN_NECK_LENGTH)
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

		public override bool Restore()
		{
			if (type == NeckType.HUMANOID)
			{
				return false;
			}
			int oldMaxLength = type.maxNeckLength;
			type = NeckType.HUMANOID;
			int lengthData = length;
			type.convertNeckSize(ref lengthData, oldMaxLength);
			length = lengthData;
			return true;
		}

		public override bool RestoreAndDisplayMessage(Player player)
		{
			if (type == NeckType.HUMANOID)
			{
				return false;
			}
			OutputText(restoreString(this, player));
			return Restore();
		}

		public bool UpdateNeck(NeckType neckType)
		{
			if (type == neckType)
			{
				return false;
			}
			type = neckType;
			return true;
		}

		public bool UpdateNeckAndDisplayMessage(NeckType neckType, Player player)
		{
			if (type == neckType)
			{
				return false;
			}
			OutputText(transformFrom(this, player));
			type = neckType;
			return true;
		}

		public bool canGrowNeck()
		{
			return type.canGrowNeck(length);
		}

		public bool canDye()
		{
			return type.canDye;
		}

		public bool attemptToDye(HairFurColors dye)
		{
			if (canDye())
			{
				HairFurColors color = neckColor;
				bool retVal = type.ParseDye(ref color, dye);
				neckColor = color;
				return retVal;
			}
			return false;
		}



		/*
		public override GenericDescription shortDescription {get; protected set;}
		public override fullDescription fullDescription {get; protected set;}
		public override PlayerDescription playerDescription {get; protected set;}
		public override ChangeType<Neck> transformFrom {get; protected set;}

		protected string neckDescription()
		{
			return neckType.shortDescription(length);
		}

		public static void Restore(ref Neck neck)
		{
			neck.Restore();
		}

		public void Restore()
		{
			length = MIN_NECK_LENGTH;
			neckType = NeckType.HUMANOID;
		}

		public static Neck GenerateNeck()
		{
			return new Neck();
		}
		*/
	}

	public class NeckType : BodyPartBehavior<NeckType, Neck>
	{
		private static int indexMaker = 0;
		public const int MIN_NECK_LENGTH = 2;
		public readonly int _index;

		public readonly int maxNeckLength;
		public readonly HairFurColors defaultColor;

		protected NeckType(int maxLength,
			GenericDescription shortDesc, FullDescription<Neck> fullDesc, PlayerDescription<Neck> playerDesc, ChangeType<Neck> transform, 
			ChangeType<Neck> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			maxNeckLength = maxLength;
			canDye = false;
			defaultColor = HairFurColors.NO_HAIR_FUR;
		}

		protected NeckType(int maxLength, HairFurColors defaultHairFur,
			GenericDescription shortDesc, FullDescription<Neck> fullDesc, PlayerDescription<Neck> playerDesc, ChangeType<Neck> transform,
			ChangeType<Neck> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			maxNeckLength = maxLength;
			defaultColor = defaultHairFur;
			canDye = defaultColor != HairFurColors.NO_HAIR_FUR;
		}

		public override int index => _index;

		public virtual bool canGrowNeck(int currNeckLength)
		{
			return currNeckLength < maxNeckLength;
		}

		public virtual int StrengthenTransform(ref int neckLength, int level)
		{
			if (neckLength >= maxNeckLength || level <= 0)
			{
				return 0;
			}
			int oldLength = neckLength;
			neckLength += level;
			Utils.Clamp(ref neckLength, MIN_NECK_LENGTH, maxNeckLength);
			return neckLength - oldLength;
		}

		public virtual bool ParseDye(ref HairFurColors current, HairFurColors dye)
		{
			if (!canDye)
			{
				return false;
			}
			current = dye;
			return true;
		}

		public readonly bool canDye;

		//this uses a percent approach. if you want just a flat value, override this.

		/// <summary>
		/// Updates the neck length, by determining how close it was to the old max,
		/// and updating the length to match according to this type's max.
		/// </summary>
		/// <param name="length">the current neck length</param>
		/// <param name="oldMaxLength"></param>
		public virtual void convertNeckSize(ref int length, int oldMaxLength)
		{
			if (maxNeckLength == MIN_NECK_LENGTH)
			{
				length = MIN_NECK_LENGTH;
			}
			else
			{
				double percent = percentTowardsMaxLength(length, oldMaxLength);
				length = (int)(Math.Round(MIN_NECK_LENGTH + (maxNeckLength - MIN_NECK_LENGTH) * percent));
			}
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
