//Lip.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:47 PM

using System;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	public enum LipPiercings { LABRET, MEDUSA, MONROE_LEFT, MONROE_RIGHT, LOWER_LEFT_1, LOWER_LEFT_2, LOWER_RIGHT_1, LOWER_RIGHT_2 }

	[DataContract]
	public class Lip : SimplePiercing<Lip, LipPiercings>
	{
		protected Lip() { }
		internal static Lip Generate()
		{
			return new Lip();
		}

		protected override bool PiercingLocationUnlocked(LipPiercings piercingLocation)
		{
			return true;
		}

		internal override Type[] currentSaves => new Type[] { typeof(LipSurrogateVersion1) };

		internal override Type currentSaveVersion => typeof(LipSurrogateVersion1);


		internal override SimplePiercingSurrogate<Lip, LipPiercings> ToCurrentSave()
		{
			return new LipSurrogateVersion1()
			{
				lipPiercings = serializePiercings()
			};
		}

		internal Lip(LipSurrogateVersion1 surrogate)
		{
			deserializePiercings(surrogate.lipPiercings);
		}
	}

	[DataContract]
	public sealed class LipSurrogateVersion1 : SimplePiercingSurrogate<Lip, LipPiercings>
	{
		[DataMember]
		public bool[] lipPiercings;
		internal override Lip ToSimplePiercingPart()
		{
			return new Lip(this);
		}
	}
}
