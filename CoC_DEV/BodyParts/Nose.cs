//Nose.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:54 PM

namespace  CoC.BodyParts
{
	public enum NosePiercings {LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE}

	public class Nose : SimplePiercing<NosePiercings>
	{
		protected Nose() {}

		internal static Nose Generate()
		{
			return new Nose();
		}

		protected override bool PiercingLocationUnlocked(NosePiercings piercingLocation)
		{
			return true;
		}
	}
}
