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
		public const byte BOYISH = 0;
		public const byte SLENDER = 2;
		public const byte AVERAGE = 4;
		public const byte AMPLE = 6;
		public const byte CURVY = 10;
		public const byte PUDGY = 12;
		public const byte FERTILE = 15;
		public const byte INHUMANLY_WIDE = 20;

		public byte hipSize
		{
			get { return _hipsize; }
			private set
			{
				Utils.Clamp(ref value, BOYISH, INHUMANLY_WIDE);
				_hipsize = value;
			}
		}
		private byte _hipsize;

		private Hips(byte size)
		{
			hipSize = size;
		}
		public byte index => hipSize;
		public static Hips GenerateHips(byte size = AVERAGE)
		{
			return new Hips(size);
		}

		public byte GrowHips(byte amount = 1)
		{
			byte oldSize = hipSize;
			hipSize += amount;
			return hipSize.subtract(oldSize);
		}

		public byte ShrinkHips(byte amount = 1)
		{
			byte oldSize = hipSize;
			hipSize -= amount;
			return oldSize.subtract(hipSize);
		}

		bool IGrowShrinkable.CanReducto()
		{
			return hipSize > SLENDER;
		}

		float IGrowShrinkable.UseReducto()
		{
			byte oldSize = hipSize;
			if (hipSize > CURVY)
			{
				hipSize -= 3;
			}
			else
			{
				hipSize -= (byte)(Utils.Rand(3) + 1);
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
