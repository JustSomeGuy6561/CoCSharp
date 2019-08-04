//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Hips : SimpleSaveablePart<Hips>, IShrinkable //Gro+ doesn't work on hips.
	{
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
			get => _hipsize;
			private set => _hipsize = Utils.Clamp2(value, BOYISH, INHUMANLY_WIDE);
		}
		private byte _hipsize;
		public SimpleDescriptor AsText => AsStr;
		public SimpleDescriptor ShortDescription => ShortDesc;

		private Hips(byte hipSize)
		{
			size = hipSize;
		}
		public byte index => size;

		public static Hips Generate(byte size = AVERAGE)
		{
			return new Hips(size);
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
	}
}
