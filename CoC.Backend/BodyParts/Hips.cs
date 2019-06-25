//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Hips : SimpleSaveablePart<Hips>, IGrowShrinkable
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
			size += amount;
			return size.subtract(oldSize);
		}

		public byte ShrinkHips(byte amount = 1)
		{
			byte oldSize = size;
			size -= amount;
			return oldSize.subtract(size);
		}

		public void SetHipSize(byte newSize)
		{
			size = newSize;
		}

		bool IGrowShrinkable.CanReducto()
		{
			return size > SLENDER;
		}

		float IGrowShrinkable.UseReducto()
		{
			byte oldSize = size;
			if (size > CURVY)
			{
				size -= 3;
			}
			else
			{
				size -= (byte)(Utils.Rand(3) + 1);
			}
			return oldSize - size;
		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return false;
		}

		float IGrowShrinkable.UseGroPlus()
		{
			return 0;
		}

		public SimpleDescriptor AsText => AsStr;
		public SimpleDescriptor ShortDescription => ShortDesc;

		internal override bool Validate(bool correctInvalidData)
		{
			size = size;
			return true;
		}
	}
}
