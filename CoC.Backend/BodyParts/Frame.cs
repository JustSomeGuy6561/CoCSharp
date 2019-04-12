using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
#warning There were no descriptors for tone or thickness i could recall. may want to add a few in anyway just for kicks. 

	public sealed class Frame : SimpleSaveablePart<Frame>
	{
		public const byte TONE_PERFECTLY_DEFINED = 100;
		public const byte TONE_VISIBLE = 80;
		public const byte TONE_SLIGHTLY_VISIBLE = 65;
		public const byte TONE_PLAIN = 50;
		public const byte TONE_SOFT = 30;
		public const byte TONE_SLIGHTLY_FLABBY = 20;
		public const byte TONE_FLABBY = 0;

		public byte muscleTone
		{
			get => _muscleTone;
			set
			{
				Utils.Clamp(ref value, TONE_FLABBY, TONE_PERFECTLY_DEFINED);
				_muscleTone = value;
			}
		}
		private byte _muscleTone;

		public const byte THICKNESS_LITHE = 0;
		public const byte THICKNESS_THIN = 20;
		public const byte THICKNESS_NORMAL = 35;
		public const byte THICKNESS_THICK = 50;
		public const byte THICKNESS_LARGE = 65; 
		public const byte THICKNESS_HUGE = 80;
		public const byte THICKNESS_MASSIVE = 100;

		public byte thickness
		{
			get => _thickness;
			private set
			{
				Utils.Clamp(ref value, THICKNESS_LITHE, THICKNESS_MASSIVE);
				_thickness = value;
			}
		}
		private byte _thickness;

		private Frame(byte thick, byte tone)
		{
			thickness = thick;
			muscleTone = tone;
		}

		internal static Frame GenerateDefault()
		{
			return new Frame(THICKNESS_NORMAL, TONE_SOFT);
		}

		internal static Frame Generate(byte thickness = THICKNESS_NORMAL, byte muscleTone = TONE_SOFT)
		{
			return new Frame(thickness, muscleTone);
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			//auto-validate data.
			thickness = thickness;
			muscleTone = muscleTone;
			return true;
		}
	}
}
