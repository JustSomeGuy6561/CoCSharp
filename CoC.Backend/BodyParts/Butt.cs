//Butt.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:10 PM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;
using System;

namespace CoC.Backend.BodyParts
{
	//it's literally just a wrapper for an int.
	//but now it has validation! woo!
	//oh, and a descriptor.
	public sealed partial class Butt : SimpleSaveablePart<Butt>, IGrowShrinkable
	{
		public const byte BUTTLESS = 0;

		public const byte TIGHT = 2;
		public const byte AVERAGE = 4;
		public const byte NOTICEABLE = 6;
		public const byte LARGE = 8;
		public const byte JIGGLY = 10;
		public const byte EXPANSIVE = 13;
		public const byte HUGE = 16;
		public const byte INCONCEIVABLY_BIG = 20;

		public bool hasButt => size >= TIGHT;
		public int index => size;

		public byte size
		{
			get => _buttSize;
			private set => _buttSize = Utils.Clamp2(value, TIGHT, INCONCEIVABLY_BIG);
		}
		private byte _buttSize;

		private Butt(byte size)
		{
			if (size < TIGHT)
			{
				_buttSize = BUTTLESS;
			}
			else
			{
				this.size = size;
			}
		}

		internal static Butt Generate(byte size = AVERAGE)
		{
			return new Butt(size);
		}

		public SimpleDescriptor AsText => AsStr;

		public SimpleDescriptor ShortDescription => ShortDesc;

		public byte GrowButt(byte amount = 1)
		{
			byte oldSize = size;
			size = size.add(amount);
			return size.subtract(oldSize);
		}

		public byte ShrinkButt(byte amount = 1)
		{
			byte oldSize = size;
			size = size.subtract(amount);
			return oldSize.subtract(size);
		}

		public void SetButtSize(byte newSize)
		{
			size = newSize;
		}

		bool IGrowShrinkable.CanReducto()
		{
			return size > TIGHT;
		}

		float IGrowShrinkable.UseReducto()
		{
			int oldSize = size;
			if (size >= HUGE)
			{
				size = (byte)Math.Round(size * 2.0 / 3);
			}
			else if (size >= JIGGLY)
			{
				size -= 3;
			}
			else if (size > TIGHT)
			{
				//shrink by up to 3, to a minimum rating of TIGHT
				byte val = (byte)Math.Min(3, size - TIGHT);
				//increase by 1, as rand returns 0 to (val-1), we want 1 to val
				size -= (byte)(Utils.Rand(val) + 1);
			}
			return oldSize - size;
		}

		//apparently gro+ doesnt work on butt or hips.
		bool IGrowShrinkable.CanGrowPlus()
		{
			return false;
		}

		float IGrowShrinkable.UseGroPlus()
		{
			return 0;
		}

		internal override bool Validate(bool correctInvalidData)
		{
			//auto-validates data.
			size = size;
			return true;
		}
	}
}