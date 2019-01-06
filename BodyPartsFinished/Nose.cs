//Nose.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:54 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public enum NosePiercings {LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE}

	public class Nose : SimplePiercing<NosePiercings>
	{
		protected Nose(PiercingFlags flags) : base(flags) {}

		protected override bool PiercingLocationUnlocked(NosePiercings piercingLocation)
		{
			return true;
		}
	}
}
