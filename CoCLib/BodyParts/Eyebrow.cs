//Lip - Copy.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:50 PM

//Lip.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:47 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.BodyParts
{
	public enum EyebrowPiercings { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2}

	public class Eyebrow : SimplePiercing<EyebrowPiercings>
	{
		protected Eyebrow(PiercingFlags flags) : base(flags) {}

		public static Eyebrow Generate(PiercingFlags flags)
		{
			return new Eyebrow(flags);
		}


		protected override bool PiercingLocationUnlocked(EyebrowPiercings piercingLocation)
		{
			return true;
		}
	}
}
