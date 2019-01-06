//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Strings.BodyParts;
using CoC.Tools;

namespace CoC.BodyParts
{
	class Hips
	{
		public const int BOYISH         =   0;
        public const int SLENDER        =   2;
        public const int AVERAGE        =   4;
        public const int AMPLE          =   6;
        public const int CURVY          =  10;
        public const int PUDGY          =  12;
        public const int FERTILE        =  15;
        public const int INHUMANLY_WIDE =  20;

		public int hipSize
		{
			get { return _hipsize; }
			private set
			{
				Utils.Clamp(ref value, BOYISH, INHUMANLY_WIDE);
				_hipsize = value;
			}
		}
		private int _hipsize;

		protected Hips(int size)
		{
			hipSize = size;
		}
		public int index => hipSize;
		public Hips GenerateHips(int size = AVERAGE)
		{
			return new Hips(size);
		}

		public int GrowHips(int amount = 1)
		{
			int oldSize = hipSize;
			hipSize += amount;
			return hipSize - oldSize;
		}

		public int ShrinkHips(int amount = 1)
		{
			int oldSize = hipSize;
			hipSize -= amount;
			return oldSize - hipSize;
		}

		public string shortDescription()
		{
			return AssButtHipStrings.hipDescription(hipSize);
		}
	}
}
