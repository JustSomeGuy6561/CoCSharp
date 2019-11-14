//Build.cs
//Description:
//Author: JustSomeGuy
//4/10/2019, 4:44 AM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{

	public sealed partial class Build : SimpleSaveablePart<Build, BuildWrapper>
	{
		#region Height

		public const byte DEFAULT_HEIGHT = 60;
		public const byte MIN_HEIGHT = 24;
		public const byte MAX_HEIGHT = 132;

		public const byte MIN_PLAYER_HEIGHT = 42;

		public byte heightInInches
		{
			get => _heightInInches;
			private set
			{
				Utils.Clamp(ref value, MIN_HEIGHT, MAX_HEIGHT);
				if (_heightInInches != value)
				{
					var oldValue = AsData();
					_heightInInches = value;
					NotifyDataChanged(oldValue);
				}
			}
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
			private set
			{
				Utils.Clamp(ref value, TONE_FLABBY, TONE_PERFECTLY_DEFINED);
				if (_muscleTone != value)
				{
					var oldData = AsData();
					_muscleTone = value;
					NotifyDataChanged(oldData);
				}
			}
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
			private set
			{
				Utils.Clamp(ref value, THICKNESS_LITHE, THICKNESS_MASSIVE);
				if (_thickness != value)
				{
					var oldData = AsData();
					_thickness = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _thickness;
		#endregion
		#region Hips
		public readonly Hips hips;
		#endregion
		#region Butt
		public readonly Butt butt;
		#endregion

		public override string BodyPartName() => Name();

#warning There were no descriptors for tone or thickness i could recall. may want to add a few in anyway just for kicks. 
		//public SimpleDescriptor thicknessAsText => ThicknessText;
		//public SimpleDescriptor toneAsText => ToneText;

		public string ButtSizeAsText() => butt.AsText();
		public string HipSizeAsText() => hips.AsText();

		public string buttShortDescription() => butt.ShortDescription();
		public string hipsShortDescription() => hips.ShortDescription();

		public string buttLongDescription() => ButtLongDesc();
		public string hipsLongDescription() => HipsLongDesc();


		internal Build(Guid creatureID, byte heightInches, byte? characterThickness, byte? characterTone, byte? characterHipSize, byte? characterButtSize) : base(creatureID)
		{
			_heightInInches = Utils.Clamp2(heightInches, MIN_HEIGHT, MAX_HEIGHT);

			_thickness = Utils.Clamp2(characterThickness ?? THICKNESS_NORMAL, THICKNESS_LITHE, THICKNESS_MASSIVE);
			_muscleTone = Utils.Clamp2(characterTone ?? TONE_SOFT, TONE_FLABBY, TONE_PERFECTLY_DEFINED);

			butt = new Butt(creatureID, characterButtSize ?? Butt.AVERAGE);
			hips = new Hips(creatureID, characterHipSize ?? Hips.AVERAGE);
		}

		internal Build(Guid creatureID) : this(creatureID, DEFAULT_HEIGHT, THICKNESS_NORMAL, TONE_SOFT, Hips.AVERAGE, Butt.AVERAGE)
		{
		}

		protected internal override void PostPerkInit()
		{
			butt.PostPerkInit();
			hips.PostPerkInit();
		}

		protected internal override void LateInit()
		{
			butt.LateInit();
			hips.LateInit();

			butt.dataChanged += Butt_dataChange;
			hips.dataChanged += Hips_dataChange;
		}

		private void Hips_dataChange(object sender, SimpleDataChangedEvent<HipWrapper, HipsData> e)
		{
			var oldData = new BuildData(heightInInches, muscleTone, thickness, butt.AsData(), e.oldData);
			NotifyDataChanged(oldData);
		}

		private void Butt_dataChange(object sender, SimpleDataChangedEvent<ButtWrapper, ButtData> e)
		{
			var oldData = new BuildData(heightInInches, muscleTone, thickness, e.oldData, hips.AsData());
			NotifyDataChanged(oldData);
		}

		public byte GrowButt(byte amount = 1)
		{
			return butt.GrowButt(amount);
		}

		public byte ShrinkButt(byte amount = 1)
		{
			return butt.ShrinkButt(amount);
		}

		public byte SetButtSize(byte size)
		{
			butt.SetButtSize(size);
			return butt.size;
		}

		public byte GrowHips(byte amount = 1)
		{
			return hips.GrowHips(amount);
		}

		public byte ShrinkHips(byte amount = 1)
		{
			return hips.ShrinkHips(amount);
		}

		public byte SetHipSize(byte size)
		{
			hips.SetHipSize(size);
			return hips.size;
		}

		public byte GainMuscle(byte amount = 1)
		{
			byte oldTone = muscleTone;
			muscleTone = muscleTone.add(amount);
			return muscleTone.subtract(oldTone);
		}

		public byte LoseMuscle(byte amount = 1)
		{
			byte oldTone = muscleTone;
			muscleTone = muscleTone.subtract(amount);
			return oldTone.subtract(muscleTone);
		}

		public byte SetMuscleTone(byte tone)
		{
			muscleTone = tone;
			return muscleTone;
		}

		public byte GetThicker(byte amount = 1)
		{
			byte oldThickness = thickness;
			thickness = thickness.add(amount);
			return thickness.subtract(oldThickness);
		}

		public byte GetThinner(byte amount = 1)
		{
			byte oldThickness = thickness;
			thickness = thickness.subtract(amount); //rip. caught in debug testing.
			return oldThickness.subtract(thickness);
		}

		public byte SetThickness(byte newThickness)
		{
			thickness = newThickness;
			return thickness;
		}

		public byte GetTaller(byte increaseInInches = 1)
		{
			byte oldHeight = heightInInches;
			heightInInches = heightInInches.add(increaseInInches);
			return heightInInches.subtract(oldHeight);
		}

		public byte GetShorter(byte decreaseInInches = 1)
		{
			byte oldHeight = heightInInches;
			heightInInches = heightInInches.subtract(decreaseInInches);
			return oldHeight.subtract(heightInInches);
		}

		public byte SetHeight(byte newHeightInInches)
		{
			heightInInches = newHeightInInches;
			return heightInInches;
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

		public override BuildWrapper AsReadOnlyReference()
		{
			return new BuildWrapper(this);
		}

		public BuildData AsData()
		{
			return new BuildData(heightInInches, muscleTone, thickness, butt.AsData(), hips.AsData());
		}

		private readonly WeakEventSource<SimpleDataChangedEvent<BuildWrapper, BuildData>> dataChangeSource =
			new WeakEventSource<SimpleDataChangedEvent<BuildWrapper, BuildData>>();

		public event EventHandler<SimpleDataChangedEvent<BuildWrapper, BuildData>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private void NotifyDataChanged(BuildData oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangedEvent<BuildWrapper, BuildData>(AsReadOnlyReference(), oldData));
		}
	}

	public sealed class BuildWrapper : SimpleWrapper<BuildWrapper, Build>
	{
		public byte heightInInches => sourceData.heightInInches;

		public byte muscleTone => sourceData.muscleTone;

		public byte thickness => sourceData.thickness;

		public ButtWrapper butt => sourceData.butt.AsReadOnlyReference();

		public byte buttSize => butt.size;

		public HipWrapper hips => sourceData.hips.AsReadOnlyReference();

		public byte hipSize => hips.size;

		public string ButtSizeAsText() => sourceData.ButtSizeAsText();
		public string HipSizeAsText() => sourceData.HipSizeAsText();

		public string buttShortDescription() => sourceData.buttShortDescription();
		public string hipsShortDescription() => sourceData.hipsShortDescription();

		public string buttLongDescription() => sourceData.buttLongDescription();
		public string hipsLongDescription() => sourceData.hipsLongDescription();


		internal BuildWrapper(Build source) : base(source)
		{ }

		internal BuildWrapper(Guid id) : base(new Build(id))
		{ }
	}

	public sealed class BuildData
	{
		public readonly byte heightInInches;
		public readonly byte muscleTone;
		public readonly byte thickness;

		public readonly ButtData butt;

		public readonly HipsData hips;

		public BuildData(byte heightInInches, byte muscleTone, byte thickness, ButtData butt, HipsData hips)
		{
			this.heightInInches = heightInInches;
			this.muscleTone = muscleTone;
			this.thickness = thickness;
			this.butt = butt ?? throw new ArgumentNullException(nameof(butt));
			this.hips = hips ?? throw new ArgumentNullException(nameof(hips));
		}
	}
}
