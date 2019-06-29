using CoC.Backend.Tools;
using CoC.Backend.BodyParts.SpecialInteraction;
namespace CoC.Backend.BodyParts
{

	public sealed partial class Build : SimpleSaveablePart<Build>, IBodyAware, ILowerBodyAware
	{
		#region Height

		public const byte DEFAULT_HEIGHT = 60;
		public const byte MIN_HEIGHT = 24;
		public const byte MAX_HEIGHT = 132;

		public const byte MIN_PLAYER_HEIGHT = 42;

		public byte heightInInches
		{
			get => _heightInInches;
			set => _heightInInches = Utils.Clamp2(value, MIN_HEIGHT, MAX_HEIGHT);
		}
		private byte _heightInInches;

		#endregion
		#region Tone
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
			set => _muscleTone = Utils.Clamp2(value, TONE_FLABBY, TONE_PERFECTLY_DEFINED);
		}
		private byte _muscleTone;
		#endregion
		#region Thickness
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
			private set => _thickness = Utils.Clamp2(value, THICKNESS_LITHE, THICKNESS_MASSIVE);
		}
		private byte _thickness;
		#endregion
		#region Hips
		public readonly Hips hips;
		#endregion
		#region Butt
		public readonly Butt butt;
		#endregion

#warning There were no descriptors for tone or thickness i could recall. may want to add a few in anyway just for kicks. 
		//public SimpleDescriptor thicknessAsText => ThicknessText;
		//public SimpleDescriptor toneAsText => ToneText;

		public SimpleDescriptor buttSizeAsText => butt.AsText;
		public SimpleDescriptor hipSizeAsText => hips.AsText;

		public SimpleDescriptor buttShortDescription => butt.ShortDescription;
		public SimpleDescriptor hipsShortDescription => hips.ShortDescription;

		public SimpleDescriptor buttFullDescription=> ButtFullDesc;
		public SimpleDescriptor hipsFullDescription=> HipsFullDesc;


		private Build(byte heightInches, byte characterThickness, byte characterTone, byte characterHipSize, byte characterButtSize)
		{
			thickness = characterThickness;
			muscleTone = characterTone;
			butt = Butt.Generate(characterButtSize);
			hips = Hips.Generate(characterHipSize);
		}

		internal static Build GenerateDefault()
		{
			return new Build(DEFAULT_HEIGHT, THICKNESS_NORMAL, TONE_SOFT, Hips.AVERAGE, Butt.AVERAGE);
		}

		internal static Build Generate(byte heightInInches, byte? thickness = null, byte? muscleTone = null, byte? hipSize = null, byte? buttSize = null)
		{
			byte thick, tone, hip, butt;
			thick = thickness ?? THICKNESS_NORMAL;
			tone = muscleTone ?? TONE_SOFT;
			hip = hipSize ?? Hips.AVERAGE;
			butt = buttSize ?? Butt.AVERAGE;
			return new Build(heightInInches, thick, tone, hip, butt);
		}

		internal static Build GenerateButtless(byte heightInInches, byte thickness = THICKNESS_NORMAL, byte muscleTone = TONE_SOFT, byte hipSize = Hips.AVERAGE)
		{
			return new Build(heightInInches, thickness, muscleTone, hipSize, Butt.BUTTLESS);
		}

		public byte GrowButt(byte amount = 1)
		{
			return butt.GrowButt(amount);
		}

		public byte ShrinkButt(byte amount = 1)
		{
			return butt.ShrinkButt(amount);
		}

		public void SetButtSize(byte size)
		{
			butt.SetButtSize(size);
		}

		public byte GrowHips(byte amount = 1)
		{
			return hips.GrowHips(amount);
		}

		public byte ShrinkHips(byte amount = 1)
		{
			return hips.ShrinkHips(amount);
		}

		public void SetHipSize(byte size)
		{
			hips.SetHipSize(size);
		}

		public byte GainMuscle(byte amount = 1)
		{
			byte oldTone = muscleTone;
			muscleTone += amount;
			return muscleTone.subtract(oldTone);
		}

		public byte LoseMuscle(byte amount = 1)
		{
			byte oldTone = muscleTone;
			muscleTone -= amount;
			return oldTone.subtract(muscleTone);
		}

		public void SetMuscleTone(byte tone)
		{
			muscleTone = tone;
		}

		public byte GetThicker(byte amount = 1)
		{
			byte oldThickness = thickness;
			thickness += amount;
			return thickness.subtract(oldThickness);
		}

		public byte GetThinner(byte amount = 1)
		{
			byte oldThickness = thickness;
			thickness -= amount;
			return oldThickness.subtract(thickness);
		}

		public void SetThickness(byte newThickness)
		{
			thickness = newThickness;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			//auto-validate data.
			heightInInches = heightInInches;
			thickness = thickness;
			muscleTone = muscleTone;
			//validate sub-parts.
			bool valid = butt.Validate(correctInvalidData);
			valid &= hips.Validate(correctInvalidData);
			return valid;
		}

		private BodyDataGetter bodyData;
		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}

		private LowerBodyDataGetter lowerBodyData;
		void ILowerBodyAware.GetLowerBodyData(LowerBodyDataGetter getter)
		{
			lowerBodyData = getter;
		}

		internal void SetupBuildAware(IBuildAware buildAware)
		{
			buildAware.GetBuildData(ToBuildData);
		}
		private BuildData ToBuildData()
		{
			return new BuildData(heightInInches, muscleTone, thickness, butt.size, hips.size);
		}
	}

	public sealed class BuildData
	{
		public readonly byte heightInInches;
		public readonly byte muscleTone;
		public readonly byte thickness;
		public readonly byte buttSize;
		public readonly byte hipSize;

		internal BuildData(byte height, byte tone, byte thicc, byte butt, byte hips)
		{
			heightInInches = height;
			muscleTone = tone;
			thickness = thicc;
			buttSize = butt;
			hipSize = hips;

		}
	}
}
