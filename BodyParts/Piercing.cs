//Piercing.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 10:17 PM
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoC.Wearables.Piercings;
namespace CoC.BodyParts
{
	//Piercing revamp.
	//Ceraph, eat your heart out.
	public abstract class DeleteMe
	{

		public const byte TONGUE_1 = 0;
		public const byte TONGUE_2 = 0;

		public const byte LEFT_NIPPLE_HORIZONTAL  = 0;
		public const byte LEFT_NIPPLE_VERTICAL    = 1;
		public const byte RIGHT_NIPPLE_HORIZONTAL = 2;
		public const byte RIGHT_NIPPLE_VERTICAL   = 3;

		//technically this could be an array, or even an int/long.
		//but i think 16 is enough - be glad it's not one anymore lol.
		private short piercingBitFlag;

		internal PiercingJewelry[] Jewelry { get; private set; }

		protected bool piercingInit(byte maxPiercingCount, short bitwisePiercings = 0)
		{
			piercingBitFlag = bitwisePiercings;
			if (maxPiercingCount > 16)
			{
				maxPiercingCount = 16;
				Jewelry = new PiercingJewelry[maxPiercingCount];
				throw new WarningException("Only 16 piercings are supported per body part. if you've implemented more, they will never proc. If you really need more, feel free to change this, but you're on your own.");
				return false;
			}
			else
			{
				Jewelry = new PiercingJewelry[maxPiercingCount];
			}
			return true;
		}

		public bool Pierce(byte index = 0)
		{

			if (index > maxPiercing)
			{
				return false;
			}
			short x = (short)(1 << index);
			if ((piercingBitFlag & x) != 0)
			{
				piercingBitFlag &= x;
				return true;
			}
			else return false;
		}

		public bool EquipPiercing(byte index, PiercingJewelry jewelry, bool force = false)
		{
			if (index)
		}
	}
}
