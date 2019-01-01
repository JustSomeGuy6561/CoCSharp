//Butt.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:10 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Tools;
using CoC.BodyPart.SpecialInteraction;
namespace CoC.BodyParts
{
	class Butt : BodyPartBehavior, IGrowShrinkable
	{
		public bool hasAsshole { get; protected set; }

		public const int BUTTLESS          =   0;
        public const int TIGHT             =   2;
        public const int AVERAGE           =   4;
        public const int NOTICEABLE        =   6;
        public const int LARGE             =   8;
        public const int JIGGLY            =  10;
        public const int EXPANSIVE         =  13;
        public const int HUGE              =  16;
        public const int INCONCEIVABLY_BIG =  20;

		public int buttSize
		{
			get { return index; }
			private set
			{
				Utils.Clamp(ref value, BUTTLESS, INCONCEIVABLY_BIG);
				index = value;
				updateDescriptor();
			}
		}

		protected Butt(int size)
		{
			buttSize = size;
		}

		public Butt GenerateButt(int size = AVERAGE)
		{
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

		public override string GetDescriptor()
		{
			return descriptor;
		}

		private void updateDescriptor()
		{
			if (buttSize == BUTTLESS)
			{
				descriptor = "buttless";
			}
			else if (buttSize < TIGHT)
			{
				descriptor = "very tight";
			}
			else if (buttSize < AVERAGE)
			{
				descriptor = "tight";
			}
			else if (buttSize < NOTICEABLE)
			{
				descriptor = "average";
			}
			else if (buttSize < LARGE)
			{
				descriptor = "noticable";
			}
			else if (buttSize < JIGGLY)
			{
				descriptor = "large";
			}
			else if (buttSize < EXPANSIVE)
			{
				descriptor = "jiggly";
			}
			else if (buttSize < HUGE)
			{
				descriptor = "expansive";
			}
			else if (buttSize < INCONCEIVABLY_BIG)
			{
				descriptor = "huge";
			}
			else
			{
				descriptor = "inconceivably big";
			}
		}

		public bool CanReducto()
		{
			return buttSize > TIGHT;
		}

		public int UseReducto()
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

		public int UseGroPlus()
		{
			return 0;
		}
	}
}
