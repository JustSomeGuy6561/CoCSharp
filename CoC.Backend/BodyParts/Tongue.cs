//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Items.Wearables.Piercings;
using CoC.Backend.Tools;
using System;
using System.Collections.Generic;

namespace CoC.Backend.BodyParts
{
	public enum TonguePiercingLocation { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }

	public sealed class Tongue : BehavioralSaveablePart<Tongue, TongueType> //ICanAttackWith ? if we make tongues able to bind somebody or something. 
	{
		public const JewelryType TongueJewelry = JewelryType.BARBELL_STUD;

		public readonly Piercing<TonguePiercingLocation> tonguePiercings;

		public override TongueType type { get; protected set; }
		public override bool isDefault => type == TongueType.HUMAN;


		public bool isLongTongue => type.longTongue;
		public int length => type.length;

		private Tongue()
		{
			type = TongueType.HUMAN;

			tonguePiercings = new Piercing<TonguePiercingLocation>(TongueJewelry, PiercingLocationUnlocked);
		}


		internal static Tongue GenerateDefault()
		{
			return new Tongue();
		}

		internal static Tongue GenerateDefaultOfType(TongueType tongueType)
		{
			return new Tongue()
			{
				type = tongueType
			};
		}

		internal bool UpdateTongue(TongueType newType)
		{
			if (type == newType)
			{
				return false;
			}
			type = newType;
			return true;
		}

		internal override bool Restore()
		{
			if (type == TongueType.HUMAN)
			{
				return false;
			}
			type = TongueType.HUMAN;
			return true;
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
		private bool PiercingLocationUnlocked(TonguePiercingLocation piercingLocation)
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
	}
	public partial class TongueType : SaveableBehavior<TongueType, Tongue>
	{
		private static int indexMaker = 0;
		private static readonly List<TongueType> tongues = new List<TongueType>();
		private readonly int _index;
		public readonly short length;

		private protected TongueType(short tongueLength, SimpleDescriptor shortDesc, DescriptorWithArg<Tongue> fullDesc, TypeAndPlayerDelegate<Tongue> playerDesc, ChangeType<Tongue> transform, RestoreType<Tongue> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
			tongues.AddAt(this, _index);
			length = tongueLength;
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

		public static readonly TongueType HUMAN = new TongueType(4, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(6, SnakeDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(24, DemonicDesc, DemonicFullDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(48, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(12, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(12, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(4, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}
}
