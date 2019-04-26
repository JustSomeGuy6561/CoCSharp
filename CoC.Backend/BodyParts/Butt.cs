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
	public sealed partial class Butt : SimpleSaveablePart<Butt>, IGrowShrinkable //IPerkAware? no idea if this thing uses perks.
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

		public bool hasButt => buttSize >= TIGHT;
		public int index => buttSize;

		public byte buttSize
		{
			get => _buttSize;
			private set
			{

				Utils.Clamp(ref value, TIGHT, INCONCEIVABLY_BIG);
				_buttSize = value;
			}

		}
		private byte _buttSize;
		private Butt(byte size = TIGHT)
		{
			Utils.Clamp(ref size, BUTTLESS, INCONCEIVABLY_BIG);
			if (size < TIGHT)
			{
				size = BUTTLESS;
			}
			_buttSize = size;
		}

		public SimpleDescriptor AsText => toText;

		public SimpleDescriptor shortDescription => shortDesc;
		public DescriptorWithArg<Frame> fullDescription => fullDesc;

		public static Butt Generate_NoButt()
		{
			return new Butt(BUTTLESS);
		}

		/// <summary>
		/// Generates a butt that can resize. minimum value is "TIGHT",
		/// and max is "INCONCEIVABLY_BIG". if a value is outside this 
		/// range it will be clamped into this range.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Butt GenerateButt(byte size = AVERAGE)
		{
			Utils.Clamp(ref size, TIGHT, INCONCEIVABLY_BIG);
			return new Butt(size);
		}

		public byte GrowButt(byte amount = 1)
		{
			byte oldSize = buttSize;
			buttSize += amount;
			return buttSize.subtract(oldSize);
		}

		public byte ShrinkButt(byte amount = 1)
		{
			byte oldSize = buttSize;
			buttSize -= amount;
			return oldSize.subtract(buttSize);
		}

		bool IGrowShrinkable.CanReducto()
		{
			return buttSize > TIGHT;
		}

		float IGrowShrinkable.UseReducto()
		{
			int oldSize = buttSize;
			if (buttSize >= HUGE)
			{
				buttSize = (byte)Math.Round(buttSize * 2.0 / 3);
			}
			else if (buttSize >= JIGGLY)
			{
				buttSize -= 3;
			}
			else if (buttSize > TIGHT)
			{
				//shrink by up to 3, to a minimum rating of TIGHT
				byte val = (byte)Math.Min(3, buttSize - TIGHT);
				//increase by 1, as rand returns 0 to (val-1), we want 1 to val
				buttSize -= (byte)(Utils.Rand(val) + 1);
			}
			return oldSize - buttSize;
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

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			//auto-validates data.
			buttSize = buttSize;
			return true;
		}
	}
}