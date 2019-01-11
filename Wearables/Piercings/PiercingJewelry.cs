//PiercingJewelry.cs
//Description:
//Author: JustSomeGuy
//12/28/2018, 11:06 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Wearables.Piercings
{
	public enum PiercingJewelryType { RING, BARBELL_STUD, DANGLER}
	public class PiercingJewelry
	{
		public readonly bool isSeamless;
		public readonly PiercingJewelryType jewelryType;
		public readonly bool specialJewelry; //like a gauge, industrial, or nipple-chain. probably its own class but w/e.
	}
}
