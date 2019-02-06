//Eyebrow.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:50 PM

namespace  CoC.BodyParts
{
	public enum EyebrowPiercings { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2}

	internal class Eyebrow : SimplePiercing<EyebrowPiercings>
	{
		protected Eyebrow() {}

		public static Eyebrow Generate()
		{
			return new Eyebrow();
		}


		protected override bool PiercingLocationUnlocked(EyebrowPiercings piercingLocation)
		{
			return true;
		}
	}
}
