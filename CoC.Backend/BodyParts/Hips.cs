//Hips.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 1:35 AM
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Tools;

namespace CoC.Backend.BodyParts
{
	public sealed partial class Hips : SimpleSaveablePart<Hips>, IGrowShrinkable, IBodyAware
	{
		public const int BOYISH = 0;
		public const int SLENDER = 2;
		public const int AVERAGE = 4;
		public const int AMPLE = 6;
		public const int CURVY = 10;
		public const int PUDGY = 12;
		public const int FERTILE = 15;
		public const int INHUMANLY_WIDE = 20;

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

		private Hips(int size)
		{
			hipSize = size;
		}
		public int index => hipSize;
		public static Hips GenerateHips(int size = AVERAGE)
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

		bool IGrowShrinkable.CanReducto()
		{
			return hipSize > SLENDER;
		}

		float IGrowShrinkable.UseReducto()
		{
			int oldSize = hipSize;
			if (hipSize > CURVY)
			{
				hipSize -= 3;
			}
			else
			{
				hipSize -= Utils.Rand(3) + 1;
			}
			return oldSize - hipSize;
		}

		bool IGrowShrinkable.CanGrowPlus()
		{
			return false;
		}

		float IGrowShrinkable.UseGroPlus()
		{
			return 0;
		}

		void IBodyAware.GetBodyData(BodyDataGetter getter)
		{
			bodyData = getter;
		}
		private BodyDataGetter bodyData;

		public SimpleDescriptor shortDescription => AsText;

		public string fullDescription(LowerBodyType lowerBody, Frame frame)
		{
			return Description(lowerBody, frame);
		}

		internal override bool Validate(bool correctDataIfInvalid = false)
		{
			hipSize = hipSize;
			return true;
		}
	}
}
