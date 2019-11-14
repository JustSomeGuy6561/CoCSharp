//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Hips : SimpleSaveablePart<Hips, HipWrapper>, IShrinkable //Gro+ doesn't work on hips.
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
				Utils.Clamp(ref value, BOYISH, INHUMANLY_WIDE);
				if (_hipSize != value)
				{
					var oldData = AsData();
					_hipSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _hipSize;

		internal Hips(Guid creatureID) : this(creatureID, AVERAGE)
		{
		}
		internal Hips(Guid creatureID, byte hipSize) : base(creatureID)
		{
			_hipSize = Utils.Clamp2(hipSize, BOYISH, INHUMANLY_WIDE);
		}

		public byte index => size;

		public override HipWrapper AsReadOnlyReference()
		{
			return new HipWrapper(this);
		}

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

		internal override bool Validate(bool correctInvalidData)
		{
			size = size;
			return true;
		}

		bool IShrinkable.CanReducto()
		{
			return size > SLENDER;
		}

		float IShrinkable.UseReducto()
		{
			if (!((IShrinkable)this).CanReducto())
			{
				return 0;
			}
			byte oldSize = size;

			if (size > CURVY)
			{
				size -= 3;
			}
			else
			{
				size -= Math.Min((byte)(Utils.Rand(3) + 1), size);
			}
			return oldSize - size;
		}

		public HipsData AsData()
		{
			return new HipsData(size);
		}

		private readonly WeakEventSource<SimpleDataChangedEvent<HipWrapper, HipsData>> dataChangeSource =
			new WeakEventSource<SimpleDataChangedEvent<HipWrapper, HipsData>>();

		public event EventHandler<SimpleDataChangedEvent<HipWrapper, HipsData>> dataChanged
		{
			add => dataChangeSource.Subscribe(value);
			remove => dataChangeSource.Unsubscribe(value);
		}

		private void NotifyDataChanged(HipsData oldData)
		{
			dataChangeSource.Raise(this, new SimpleDataChangedEvent<HipWrapper, HipsData>(AsReadOnlyReference(), oldData));
		}
	}

	

	public sealed class HipWrapper : SimpleWrapper<HipWrapper, Hips>
	{
		public byte size => sourceData.size;
		public string AsText() => sourceData.AsText();

		public string ShortDescription() => sourceData.ShortDescription();

		internal HipWrapper(Hips source) : base(source)
		{ }
	}

	public sealed class HipsData
	{
		public readonly byte size;

		public HipsData(byte size)
		{
			this.size = size;
		}
	}
}
