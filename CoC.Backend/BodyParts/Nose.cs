//Nose.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:54 PM

using System;
using System.Runtime.Serialization;

namespace  CoC.Backend.BodyParts
{
	public enum NosePiercings {LEFT_NOSTRIL, RIGHT_NOSTRIL, SEPTIMUS, BRIDGE}

	[DataContract]
	public class Nose : SimplePiercing<Nose, NosePiercings>
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
		internal override Type[] currentSaves => new Type[] { typeof(NoseSurrogateVersion1) };

		internal override Type currentSaveVersion => typeof(NoseSurrogateVersion1);

		internal override SimplePiercingSurrogate<Nose, NosePiercings> ToCurrentSave()
		{
			return new NoseSurrogateVersion1()
			{
				nosePiercings = serializePiercings()
			};
		}

		internal Nose(NoseSurrogateVersion1 surrogate)
		{
			deserializePiercings(surrogate.nosePiercings);
		}
	}

	[DataContract]
	public sealed class NoseSurrogateVersion1 : SimplePiercingSurrogate<Nose, NosePiercings>
	{
		[DataMember]
		public bool[] nosePiercings;
		internal override Nose ToSimplePiercingPart()
		{
			return new Nose(this);
		}
	}
}
