//Ass.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:21 PM
using CoC.Backend.Creatures;
using CoC.Backend.Tools;

namespace CoC.BodyParts
{
	internal partial class Ass
	{
		public const byte WETNESS_DRY = 0;
		public const byte WETNESS_NORMAL = 1;
		public const byte WETNESS_MOIST = 2;
		public const byte WETNESS_SLIMY = 3;
		public const byte WETNESS_DROOLING = 4;
		public const byte WETNESS_SLIME_DROOLING = 5;

		public const byte LOOSENESS_VIRGIN = 0;
		public const byte LOOSENESS_TIGHT = 1;
		public const byte LOOSENESS_NORMAL = 2;
		public const byte LOOSENESS_LOOSE = 3;
		public const byte LOOSENESS_STRETCHED = 4;
		public const byte LOOSENESS_GAPING = 5;

		public byte analWetness
		{
			get => _analWetness;
			set
			{
				Utils.Clamp(ref value, WETNESS_DRY, WETNESS_SLIME_DROOLING);
				_analWetness = value;
			}
		}
		private byte _analWetness = 0;

		public byte analLooseness
		{
			get => _analLooseness;
			set
			{
				Utils.Clamp(ref value, LOOSENESS_VIRGIN, LOOSENESS_GAPING);
				_analLooseness = value;
			}
		}
		private byte _analLooseness = 0;

		public ushort numTimesAnal { get; private set; } = 0;

		protected Ass()
		{
			analLooseness = 0;
			analWetness = 0;
			numTimesAnal = 0;
		}

		public static Ass GenerateDefault()
		{
			return new Ass();
		}

		public static Ass GenerateWithData(byte wetness, byte looseness)
		{
			return new Ass()
			{
				analWetness = wetness,
				analLooseness = looseness
			};
		}


		public string fullDescription()
		{
			return assFullDescription();
		}

		public string TypeAndPlayerDelegate(Player player)
		{
			return assPlayerStr(player);
		}
	}
}
