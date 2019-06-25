﻿//Lip.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:47 PM

namespace  CoC.BodyParts
{
	public enum LipPiercings {LABRET, MEDUSA, MONROE_LEFT, MONROE_RIGHT, LOWER_LEFT_1,	LOWER_LEFT_2, LOWER_RIGHT_1, LOWER_RIGHT_2 }
	internal class Lip : SimplePiercing<LipPiercings>
	{
		protected Lip() {}

		public static Lip Generate()
		{
			return new Lip();
		}

		protected override bool PiercingLocationUnlocked(LipPiercings piercingLocation)
		{
			return true;
		}
	}
}