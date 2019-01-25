//Nose.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:54 PM

namespace  CoC.BodyParts
{
	public enum NosePiercings {LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE}

	internal class Nose : SimplePiercing<NosePiercings>
	{
		protected Nose(PiercingFlags flags) : base(flags) {}

		public static Nose Generate(PiercingFlags flags)
		{
			return new Nose(flags);
		}

		protected override bool PiercingLocationUnlocked(NosePiercings piercingLocation)
		{
			return true;
		}
	}
}
