//Tongue.cs
//Description:
//Author: JustSomeGuy
//1/6/2019, 1:26 AM

using CoC.Backend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	public enum TonguePiercings { FRONT_CENTER, MIDDLE_CENTER, BACK_CENTER }
	public class Tongue : PiercableBodyPart<Tongue, TongueType, TonguePiercings>
	{
		private protected Tongue()
		{
			type = TongueType.HUMAN;
		}

		public override TongueType type { get; protected set; }

		public bool isLongTongue => type.longTongue;
		public int length => type.length;
		public override bool isDefault => type == TongueType.HUMAN;

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
		protected override bool PiercingLocationUnlocked(TonguePiercings piercingLocation)
		{
			if (piercingFetish)
			{
				return true;
			}
			//allow one tongue piercing. must have fetish for more than that.
			else if (currentPiercingCount > 0 && !IsPierced(piercingLocation))
			{
				return false;
			}
			else return true;
		}

		internal override Type[] saveVersions => new Type[] { typeof(TongueSurrogateVersion1) };
		internal override Type currentSaveVersion => typeof(TongueSurrogateVersion1);
		internal override BodyPartSurrogate<Tongue, TongueType> ToCurrentSave()
		{
			return new TongueSurrogateVersion1()
			{
				tongueType = index,
				piercingsUnlocked = piercingLookup.Values.ToArray()
			};
		}

		internal Tongue(TongueSurrogateVersion1 surrogate)
		{
			type = TongueType.Deserialize(surrogate.tongueType);
			piercingLookup = new Dictionary<TonguePiercings, bool>();
#warning Add piercing jewelry when that's implemented
			foreach (var val in Utils.AsIterable<TonguePiercings>())
			{
				piercingLookup[val] = surrogate.piercingsUnlocked.Length > (int)val ? surrogate.piercingsUnlocked[(int)val] : false;
			}
		}
	}
	public partial class TongueType : PiercableBodyPartBehavior<TongueType, Tongue, TonguePiercings>
	{
		private static int indexMaker = 0;
		private static readonly List<TongueType> tongues = new List<TongueType>();
		private readonly int _index;
		public readonly short length;

		private protected TongueType(short tongueLength, SimpleDescriptor shortDesc, DescriptorWithArg<Tongue> fullDesc, TypeAndPlayerDelegate<Tongue> playerDesc, ChangeType<Tongue> transform, RestoreType<Tongue> restore) : base(shortDesc, fullDesc, playerDesc, transform, restore)
		{
			_index = indexMaker++;
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

		public static readonly TongueType HUMAN = new TongueType(4, HumanDesc, HumanFullDesc, HumanPlayerStr, HumanTransformStr, HumanRestoreStr);
		public static readonly TongueType SNAKE = new TongueType(6, SnakeDesc, SnakeFullDesc, SnakePlayerStr, SnakeTransformStr, SnakeRestoreStr);
		public static readonly TongueType DEMONIC = new TongueType(24, DemonicDesc, DemonicFullDesc, DemonicPlayerStr, DemonicTransformStr, DemonicRestoreStr);
		public static readonly TongueType DRACONIC = new TongueType(48, DraconicDesc, DraconicFullDesc, DraconicPlayerStr, DraconicTransformStr, DraconicRestoreStr);
		public static readonly TongueType ECHIDNA = new TongueType(12, EchidnaDesc, EchidnaFullDesc, EchidnaPlayerStr, EchidnaTransformStr, EchidnaRestoreStr);
		public static readonly TongueType LIZARD = new TongueType(12, LizardDesc, LizardFullDesc, LizardPlayerStr, LizardTransformStr, LizardRestoreStr);
		public static readonly TongueType CAT = new TongueType(4, CatDesc, CatFullDesc, CatPlayerStr, CatTransformStr, CatRestoreStr);
	}

	[DataContract]
	public sealed class TongueSurrogateVersion1 : BodyPartSurrogate<Tongue, TongueType>
	{
		[DataMember]
		public int tongueType;
		[DataMember]
		public bool[] piercingsUnlocked;

		internal override Tongue ToBodyPart()
		{
			return new Tongue(this);
		}
	}
}
