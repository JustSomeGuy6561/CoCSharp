//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM

using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using CoC.Backend.Wearables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	public enum TonguePiercingLocation { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }

	[DataContract]
	[KnownType(typeof(JewelryDummyForDebugging))]
	public class Tongue : BehavioralSaveablePart<Tongue, TongueType>
	{
		private protected Tongue()
		{
			type = TongueType.HUMAN;
			tonguePiercings = new Piercing<TonguePiercingLocation>(TongueJewelry, PiercingLocationUnlocked);
			Console.WriteLine(tonguePiercings.Pierce(TonguePiercingLocation.BACK_CENTER, new JewelryDummyForDebugging()));
			Console.WriteLine(tonguePiercings.Pierce(TonguePiercingLocation.MIDDLE_CENTER, new JewelryDummyForDebugging()));
		}

		public override TongueType type { get; protected set; }

		private static readonly JewelryType TongueJewelry = JewelryType.STUD;

		public readonly Piercing<TonguePiercingLocation> tonguePiercings;

		public bool isLongTongue => type.longTongue;
		public int length => type.length;
		public override bool isDefault => type == TongueType.HUMAN;

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			var tongueType = type;
			bool valid = TongueType.Validate(ref tongueType, correctDataIfInvalid);
			type = tongueType;
			return valid;
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

		//could be a one-liner. written this way because maybe people wanna change it, idk.
		protected bool PiercingLocationUnlocked(TonguePiercingLocation piercingLocation)
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

		private protected override Type[] saveVersions => new Type[] { typeof(TongueSurrogateVersion1) };
		private protected override Type currentSaveVersion => typeof(TongueSurrogateVersion1);
		private protected override BehavioralSurrogateBase<Tongue, TongueType> ToCurrentSave()
		{
			return new TongueSurrogateVersion1()
			{
				tongueType = index,
				tonguePiercings = tonguePiercings
			};
		}

		internal Tongue(TongueSurrogateVersion1 surrogate)
		{
			type = TongueType.Deserialize(surrogate.tongueType);
			tonguePiercings = surrogate.tonguePiercings;
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
		internal static bool Validate(ref TongueType tongue, bool correctInvalidData = false)
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

	[DataContract]
	[KnownType(typeof(Piercing<TonguePiercingLocation>))]
	public sealed class TongueSurrogateVersion1 : BehavioralSurrogateBase<Tongue, TongueType>
	{
		[DataMember]
		public int tongueType;
		[DataMember]
		public Piercing<TonguePiercingLocation> tonguePiercings;

		internal override Tongue ToBodyPart()
		{
			return new Tongue(this);
		}
	}
}
