//Butt.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:10 PM
using System;
using CoC.Tools;
using  CoC.BodyParts.SpecialInteraction;
using   CoC.BodyParts;

namespace  CoC.BodyParts
{
	//it's literally just a wrapper for an int.
	//but now it has validation! woo!
	//oh, and a descriptor.
	internal class Butt : IGrowShrinkable
	{
#warning add descriptors
		public const int BUTTLESS = 0;

		public const int TIGHT = 2;
		public const int AVERAGE = 4;
		public const int NOTICEABLE = 6;
		public const int LARGE = 8;
		public const int JIGGLY = 10;
		public const int EXPANSIVE = 13;
		public const int HUGE = 16;
		public const int INCONCEIVABLY_BIG = 20;

		public bool hasButt => buttSize >= TIGHT;
		public int index => buttSize;

		public int buttSize
		{
			get => _buttSize;
			protected set
			{

				Utils.Clamp(ref value, TIGHT, INCONCEIVABLY_BIG);
				_buttSize = value;
			}

		}
		protected int _buttSize;
		protected Butt(int size = TIGHT)
		{
			Utils.Clamp(ref size, BUTTLESS, INCONCEIVABLY_BIG);
			if (size < TIGHT)
			{
				size = BUTTLESS;
			}
			_buttSize = size;
		}
		
		public string shortDescription()
		{
			return AssButtHipStrings.hipDescription(buttSize);
		}

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
		public static Butt GenerateButt(AssLocation assLocation = AssLocation.BUTT, int size = AVERAGE)
		{
			if (assLocation == AssLocation.NOT_APPLICABLE)
			{
				return Generate_NoButt();
			}
			Utils.Clamp(ref size, TIGHT, INCONCEIVABLY_BIG);
			return new Butt(size);
		}

		public int GrowButt(int amount = 1)
		{
			int oldSize = buttSize;
			buttSize += amount;
			return buttSize - oldSize;
		}

		public int ShrinkButt(int amount = 1)
		{
			int oldSize = buttSize;
			buttSize -= amount;
			return oldSize - buttSize;
		}

		public bool CanReducto()
		{
			return buttSize > TIGHT;
		}

		public float UseReducto()
		{
			int oldSize = buttSize;
			if (buttSize >= HUGE)
			{
				buttSize = (int)Math.Round(buttSize * 2.0 / 3);
			}
			else if (buttSize >= JIGGLY)
			{
				buttSize -= 3;
			}
			else if (buttSize > TIGHT)
			{
				//shrink by up to 3, to a minimum rating of TIGHT
				int val = Math.Min(3, buttSize - TIGHT);
				//increase by 1, as rand returns 0 to (val-1), we want 1 to val
				buttSize -= (Utils.Rand(val) + 1);
			}
			return oldSize - buttSize;
		}

		//apparently gro+ doesnt work on butt or hips.
		public bool CanGrowPlus()
		{
			return false;
		}

		public float UseGroPlus()
		{
			return 0;
		}
	}
}