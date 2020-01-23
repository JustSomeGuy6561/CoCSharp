//Build.cs
//Description:
//Author: JustSomeGuy
//4/10/2019, 4:44 AM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{

	public sealed partial class Build : SimpleSaveablePart<Build, BuildData>
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
					var oldValue = AsReadOnlyData();
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
					var oldData = AsReadOnlyData();
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
					var oldData = AsReadOnlyData();
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

		#region Text

#warning There were no descriptors for tone or thickness i could recall. may want to add a few in anyway just for kicks.
		//public SimpleDescriptor thicknessAsText => ThicknessText;
		//public SimpleDescriptor toneAsText => ToneText;

		public string ButtSizeAsAdjective() => butt.SizeAsAdjective();
		public string HipSizeAsAdjective() => hips.SizeAsAdjective();

		public string ButtShortDescription() => butt.ShortDescription();
		public string ButtSingleItemDescription() => butt.SingleItemDescription();
		public string HipsShortDescription(bool plural = true) => hips.ShortDescription(plural);
		public string HipsSingleItemDescription() => hips.SingleItemDescription();

		public string ButtLongDescription(bool alternateFormat) => butt.LongDescription(alternateFormat, muscleTone);
		public string HipsLongDescription() => hips.LongDescription(thickness);
		#endregion
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

			butt.dataChange += Butt_dataChange;
			hips.dataChange += Hips_dataChange;
		}

		private void Hips_dataChange(object sender, SimpleDataChangeEvent<Hips, HipData> e)
		{
			NotifyDataChanged(new BuildData(creatureID, heightInInches, muscleTone, thickness, butt.AsReadOnlyData(), e.oldValues));
		}

		private void Butt_dataChange(object sender, SimpleDataChangeEvent<Butt, ButtData> e)
		{
			NotifyDataChanged(new BuildData(creatureID, heightInInches, muscleTone, thickness, e.oldValues, hips.AsReadOnlyData()));
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

		public short ChangeMuscleToneToward(byte desiredValue, byte increment)
		{
			var oldMuscleTone = muscleTone;
			if (muscleTone == desiredValue)
			{
				return 0;
			}
			else if (muscleTone > desiredValue)
			{
				muscleTone = muscleTone.subtract(increment);
				return muscleTone.delta(oldMuscleTone);
			}
			else
			{
				muscleTone = muscleTone.add(increment);
				return muscleTone.delta(oldMuscleTone);
			}
		}

		public byte DescreaseMuscleTone(byte amount = 1)
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

		public short ChangeThicknessToward(byte desiredValue, byte increment)
		{
			var oldThickness = thickness;
			if (thickness == desiredValue)
			{
				return 0;
			}
			else if (thickness > desiredValue)
			{
				thickness = thickness.subtract(increment);
				return thickness.delta(oldThickness);
			}
			else
			{
				thickness = thickness.add(increment);
				return thickness.delta(oldThickness);
			}
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

		public short ChangeHeightToward(byte desiredValue, byte increment)
		{
			var oldHeight = heightInInches;
			if (heightInInches == desiredValue)
			{
				return 0;
			}
			else if (heightInInches > desiredValue)
			{
				heightInInches = heightInInches.subtract(increment);
				return heightInInches.delta(oldHeight);
			}
			else
			{
				heightInInches = heightInInches.add(increment);
				return heightInInches.delta(oldHeight);
			}
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

		public override BuildData AsReadOnlyData()
		{
			return new BuildData(this);
		}
	}

	public sealed class BuildData : SimpleData
	{
		public readonly byte heightInInches;
		public readonly byte muscleTone;
		public readonly byte thickness;
		private readonly ButtData butt;
		public byte buttSize => butt.size;
		private readonly HipData hips;
		public byte hipSize => hips.size;

		#region Text

		//public SimpleDescriptor thicknessAsText => ThicknessText;
		//public SimpleDescriptor toneAsText => ToneText;

		public string ButtSizeAsAdjective() => butt.SizeAsAdjective();
		public string HipSizeAsAdjective() => hips.SizeAsAdjective();

		public string ButtShortDescription() => butt.ShortDescription();
		public string ButtSingleItemDescription() => butt.SingleItemDescription();
		public string HipsShortDescription(bool plural = true) => hips.ShortDescription(plural);
		public string HipsSingleItemDescription() => hips.SingleItemDescription();

		public string ButtLongDescription(bool alternateFormat) => butt.LongDescription(alternateFormat, muscleTone);
		public string HipsLongDescription() => hips.LongDescription(thickness);
		#endregion

		internal BuildData(Build build) : base(build?.creatureID ?? throw new ArgumentNullException(nameof(build)))
		{

		}

		public BuildData(Guid id, byte height, byte tone, byte thicc, ButtData butt, HipData hips) : base(id)
		{
			heightInInches = height;
			muscleTone = tone;
			thickness = thicc;
			this.butt = butt ?? throw new ArgumentNullException(nameof(butt));
			this.hips = hips ?? throw new ArgumentNullException(nameof(hips));

		}

		public BuildData(Guid id, byte height, byte tone, byte thicc, byte butt, byte hips, BodyType bodyType, LowerBodyType lowerBodyType) : base(id)
		{
			heightInInches = height;
			muscleTone = tone;
			thickness = thicc;
			this.butt = new ButtData(id, butt);
			this.hips = new HipData(id, hips, bodyType, lowerBodyType);
		}


		public BuildData(Guid id) : base(id)
		{
			heightInInches = Build.DEFAULT_HEIGHT;
			muscleTone = Build.THICKNESS_NORMAL;
			thickness = Build.TONE_SOFT;
			butt = new ButtData(id, Butt.AVERAGE);
			hips = new HipData(id, Hips.AVERAGE);
		}
	}
}
