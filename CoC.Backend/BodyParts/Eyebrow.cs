//Eyebrow.cs
//Description:
//Author: JustSomeGuy
//1/5/2019, 5:50 PM

using System;
using System.Runtime.Serialization;

namespace CoC.Backend.BodyParts
{
	public enum EyebrowPiercings { LEFT_1, LEFT_2, RIGHT_1, RIGHT_2 }

	[DataContract]
	public class Eyebrow : SimplePiercing<Eyebrow, EyebrowPiercings>
	{
		protected Eyebrow() { }

		internal static Eyebrow Generate()
		{
			return new Eyebrow();
		}

		protected override bool PiercingLocationUnlocked(EyebrowPiercings piercingLocation)
		{
			return true;
		}

		internal override Type[] currentSaves => new Type[] { typeof(EyebrowSurrogateVersion1) };

		internal override Type currentSaveVersion => typeof(EyebrowSurrogateVersion1);

		internal override SimplePiercingSurrogate<Eyebrow, EyebrowPiercings> ToCurrentSave()
		{
			return new EyebrowSurrogateVersion1()
			{
				eyebrowPiercings = serializePiercings()
			};
		}

		internal Eyebrow(EyebrowSurrogateVersion1 surrogate)
		{
			deserializePiercings(surrogate.eyebrowPiercings);
		}
	}

	[DataContract]
	public sealed class EyebrowSurrogateVersion1 : SimplePiercingSurrogate<Eyebrow, EyebrowPiercings>
	{
		[DataMember]
		public bool[] eyebrowPiercings;
		internal override Eyebrow ToSimplePiercingPart()
		{
			return new Eyebrow(this);
		}
	}
}
