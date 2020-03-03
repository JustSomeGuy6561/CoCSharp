//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Engine;
using CoC.Backend.Strings;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//hip tattoos are part of lower body. it makes sense there, i guess.
	public sealed partial class Hips : SimpleSaveablePart<Hips, HipData>, IShrinkable //Gro+ doesn't work on hips.
	{
		public override string BodyPartName() => Name();

		public const byte BOYISH = 0;
		public const byte SLENDER = 2;
		public const byte AVERAGE = 4;
		public const byte AMPLE = 6;
		public const byte CURVY = 10;
		public const byte PUDGY = 12;
		public const byte FERTILE = 15;
		public const byte INHUMANLY_WIDE = 20;

		public byte size
		{
			get => _hipSize;
			private set
			{
				Utils.Clamp(ref value, minSize, INHUMANLY_WIDE);
				if (_hipSize != value)
				{
					var oldData = AsReadOnlyData();
					_hipSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _hipSize;

		private Creature creature => CreatureStore.GetCreatureClean(creatureID);

		private byte minSize => creature?.perks.baseModifiers.minHipsSize.GetValue() ?? BOYISH;

		internal void CheckHips()
		{
			size = size;
		}

		internal LowerBodyType lowerBodyType => creature?.lowerBody.type ?? LowerBodyType.defaultValue;
		internal BodyType bodyType => creature?.body.type ?? BodyType.defaultValue;

		internal Hips(Guid creatureID) : this(creatureID, AVERAGE)
		{
		}
		internal Hips(Guid creatureID, byte hipSize) : base(creatureID)
		{
			_hipSize = Utils.Clamp2(hipSize, BOYISH, INHUMANLY_WIDE);
		}
		public byte index => size;

		public override HipData AsReadOnlyData()
		{
			return new HipData(this);
		}

#warning NYI. hook up to perk value.
		public byte smallestPossibleHipSize { get; private set; }


		public byte GrowHips(byte amount = 1)
		{
			byte oldSize = size;
			size = size.add(amount);
			return size.subtract(oldSize);
		}

		public byte ShrinkHips(byte amount = 1)
		{
			byte oldSize = size;
			size = size.subtract(amount);
			return oldSize.subtract(size);
		}

		public byte SetHipSize(byte newSize)
		{
			size = newSize;
			return size;
		}
		#region Text
		public string SizeAsAdjective() => BuildStrings.HipAdjective(size);

		public string ShortDescription(bool plural = true) => BuildStrings.HipShortDescription(size, plural);
		public string SingleItemDescription() => BuildStrings.HipSingleDescription(size);

		public string LongDescription(byte thickness) => BuildStrings.HipLongDescription(size, lowerBodyType, bodyType, thickness);
		#endregion
		internal override bool Validate(bool correctInvalidData)
		{
			size = size;
			return true;
		}

		bool IShrinkable.CanReducto()
		{
			var min = Math.Max(SLENDER, minSize);
			return size > min;
		}

		string IShrinkable.UseReducto()
		{
			var oldData = AsReadOnlyData();
			bool nothingHappened = false;

			if (!((IShrinkable)this).CanReducto())
			{
				nothingHappened = true;
			}
			if (size >= FERTILE)
			{
				size -= (byte)(3 + size / 3);
			}
			else if (size >= CURVY)
			{
				size -= 3;
			}
			else
			{
				size -= Math.Min((byte)(Utils.Rand(3) + 1), size);
			}

			return ReductoHips(oldData, nothingHappened);
		}

		public override bool IsIdenticalTo(HipData originalData, bool ignoreSexualMetaData)
		{
			return !(originalData is null) && size == originalData.size;
		}
	}

	public sealed class HipData : SimpleData
	{
		public readonly byte size;

		internal readonly LowerBodyType lowerBodyType;
		internal readonly BodyType bodyType;

		#region Text
		public string SizeAsAdjective() => BuildStrings.HipAdjective(size);

		public string ShortDescription(bool plural = true) => BuildStrings.HipShortDescription(size, plural);
		public string SingleItemDescription() => BuildStrings.HipSingleDescription(size);

		public string LongDescription(byte thickness) => BuildStrings.HipLongDescription(size, lowerBodyType, bodyType, thickness);
		#endregion
		internal HipData(Hips source) : base(source?.creatureID ?? throw new ArgumentNullException(nameof(source)))
		{
			size = source.size;
			lowerBodyType = source.lowerBodyType;
			bodyType = source.bodyType;
		}

		public HipData(Guid creatureID, byte size) : base(creatureID)
		{
			this.size = size;

			this.bodyType = BodyType.defaultValue;
			lowerBodyType = LowerBodyType.defaultValue;
		}

		public HipData(Guid creatureID, byte size, BodyType bodyType, LowerBodyType lowerBodyType) : base(creatureID)
		{
			this.size = size;

			this.bodyType = bodyType ?? throw new ArgumentNullException(nameof(bodyType));
			this.lowerBodyType = lowerBodyType ?? throw new ArgumentNullException(nameof(lowerBodyType));
		}
	}
}
