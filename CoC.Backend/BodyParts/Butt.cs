//Butt.cs
//Description:
//Author: JustSomeGuy
//12/26/2018, 11:10 PM
using CoC.Backend.BodyParts.EventHelpers;
using CoC.Backend.BodyParts.SpecialInteraction;
using CoC.Backend.Creatures;
using CoC.Backend.Tools;
using System;
using WeakEvent;

namespace CoC.Backend.BodyParts
{
	//it's literally just a wrapper for an int.
	//but now it has validation! woo!
	//oh, and a descriptor.
	public sealed partial class Butt : SimpleSaveablePart<Butt, ButtData>, IShrinkable //Gro+ doesnt work on butt.
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

		public readonly bool hasButt;// => size >= TIGHT;
		public int index => size;

		private byte minVal => hasButt ? TIGHT : BUTTLESS;
		private byte maxVal => hasButt ? INCONCEIVABLY_BIG : BUTTLESS;

		public byte size
		{
			get => _buttSize;
			private set
			{

				Utils.Clamp(ref value, minVal, maxVal);
				if (_buttSize != value)
				{
					var oldData = AsReadOnlyData();
					_buttSize = value;
					NotifyDataChanged(oldData);
				}
			}
		}
		private byte _buttSize;

		internal Butt(Guid creatureID, byte size) : base(creatureID)
		{
			if (size < TIGHT)
			{
				hasButt = false;
				_buttSize = BUTTLESS;
			}
			else
			{
				hasButt = true;
				_buttSize = Utils.Clamp2(size, TIGHT, INCONCEIVABLY_BIG);
			}
		}

		internal static Butt GenerateButtless(Guid creatureID)
		{
			return new Butt(creatureID, 0);
		}

		public override string BodyPartName() => Name();

		public override ButtData AsReadOnlyData()
		{
			return new ButtData(creatureID, size);
		}

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

		public byte SetButtSize(byte newSize)
		{
			size = newSize;
			return size;
		}


		internal override bool Validate(bool correctInvalidData)
		{
			//auto-validates data.
			size = size;
			return true;
		}

		bool IShrinkable.CanReducto()
		{
			return size > TIGHT;
		}

		float IShrinkable.UseReducto()
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
	}

	public sealed class ButtData : SimpleData
	{
		public readonly byte size;
		internal ButtData(Guid creatureID, byte buttSize) : base(creatureID)
		{
			size = buttSize;
		}
	}
}
